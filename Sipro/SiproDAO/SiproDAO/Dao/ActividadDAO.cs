using System;
using Dapper;
using System.Data.Common;
using Utilities;
using SiproModelCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace SiproDAO.Dao
{
    public class ActividadDAO
    {

        public static List<Actividad> GetActividades(string usuario)
        {
            List<Actividad> resultado = new List<Actividad>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM actividad p WHERE p.estado = 1 AND p.id in (SELECT u.actividadid FROM actividad_usuario u where u.usuario= :usuario )");
                    resultado = db.Query<Actividad>(query, new { usuario }).AsList<Actividad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadDAO.class", e);
            }

            return resultado;
        }


        public static Actividad getActividadPorId(int id)
        {
            Actividad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<Actividad>("SELECT * FROM ACTIVIDAD WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadDAO.class", e);
            }
            return ret;
        }

        public static bool guardarActividad(Actividad Actividad, bool calcular_valores_agregados)
        {
            bool ret = false;
            int guardado = 0;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    if (Actividad.id < 1)
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_actividad.nextval FROM DUAL");
                        Actividad.id = sequenceId;
                        guardado = db.Execute("INSERT INTO ACTIVIDAD VALUES (:id, :nombre, :descripcion, :fechaInicio, :fechaFin, :porcentajeAvance, :usuarioCreo, " +
                            ":usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :actividadTipoid, :snip, :programa, :subprograma, :proyecto, :actividad, " +
                            ":obra, :objetoId, :objetoTipo, :duracion, :duracionDimension, :predObjetoId, :predObjetoTipo, :latitud, :longitud, :costo, :acumulacionCosto, " +
                            ":renglon, :ubicacionGeografica, :orden, :treePath, :nivel, :proyectoBase, :componenteBase, :productoBase, :fechaInicioReal, :fechaFinReal, " +
                            ":inversionNueva)", Actividad);

                        if (guardado > 0)
                        {
                            switch (Actividad.objetoTipo)
                            {
                                case 0:
                                    Proyecto proyecto = ProyectoDAO.getProyecto(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = proyecto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 1:
                                    Componente componente = ComponenteDAO.getComponente(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = componente.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 2:
                                    Subcomponente subcomponente = SubComponenteDAO.getSubComponente(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = subcomponente.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 3:
                                    Producto producto = ProductoDAO.getProductoPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = producto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 4:
                                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = subproducto.treepath + "" + (10000000 + Actividad.id);
                                    break;
                                case 5:
                                    Actividad actividad = ActividadDAO.getActividadPorId(Convert.ToInt32(Actividad.objetoId));
                                    Actividad.treepath = actividad.treepath + "" + (10000000 + Actividad.id);
                                    break;
                            }
                        }
                    }

                    guardado = db.Execute("UPDATE actividad SET nombre=:nombre, descripcion=:descripcion, fecha_inicio=:fechaInicio, fecha_fin=:fechaFin, porcentaje_avance=:porcentajeAvance, " +
                        "usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, " +
                        "estado=:estado, actividad_tipoid=:actividadTipoid, snip=:snip, programa=:programa, subprograma=:subprograma, proyecto=:proyecto, actividad=:actividad, " +
                        "obra=:obra, objeto_id=:objetoId, objeto_tipo=:objetoTipo, duracion=:duracion, duracion_dimension=:duracionDimension, pred_objeto_id=:predObjetoId, " +
                        "pred_objeto_tipo=:predObjetoTipo, latitud=:latitud, longitud=:longitud, costo=:costo, acumulacion_costo=:acumulacionCosto, renglon=:renglon, " +
                        "ubicacion_geografica=:ubicacionGeografica, orden=:orden, treePath=:treePath, nivel=:nivel, proyecto_base=:proyectoBase, componente_base=:componenteBase, " +
                        "producto_base=:productoBase, fecha_inicio_real=:fechaInicioReal, fecha_fin_real=:fechaFinReal, inversion_nueva=:inversionNueva WHERE id=:id", Actividad);

                    if (guardado > 0)
                    {
                        ActividadUsuario au = new ActividadUsuario();
                        au.actividads = Actividad;
                        au.actividadid = Actividad.id;
                        au.usuario = Actividad.usuarioCreo;
                        au.fechaCreacion = DateTime.Now;
                        au.usuarioCreo = Actividad.usuarioCreo;

                        int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ACTIVIDAD_USUARIO WHERE actividadid=:id AND usuario=:usuario", new { id = au.actividadid, usuario = au.usuario });

                        if (existe > 0)
                        {
                            guardado = db.Execute("UPDATE ACTIVIDAD_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                "fecha_actualizacion=:fechaActualizacion WHERE actividadid=:actividadid AND usuario=:usuario", au);
                        }
                        else
                        {
                            guardado = db.Execute("INSERT INTO actividad_usuario(:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au);
                        }

                        if (guardado > 0 && !Actividad.usuarioCreo.Equals("admin"))
                        {
                            ActividadUsuario au_admin = new ActividadUsuario();
                            au_admin.actividads = Actividad;
                            au_admin.actividadid = Actividad.id;
                            au_admin.usuario = "admin";
                            au_admin.fechaCreacion = DateTime.Now;
                            au.usuarioCreo = Actividad.usuarioCreo;

                            existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM ACTIVIDAD_USUARIO WHERE actividadid=:id AND usuario=:usuario", new { id = au_admin.actividadid, usuario = au_admin.usuario });

                            if (existe > 0)
                            {
                                guardado = db.Execute("UPDATE ACTIVIDAD_USUARIO SET usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, fecha_creacion=:fechaCreacion, " +
                                    "fecha_actualizacion=:fechaActualizacion WHERE actividadid=:actividadid AND usuario=:usuario", au_admin);
                            }
                            else
                            {
                                guardado = db.Execute("INSERT INTO actividad_usuario(:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au_admin);
                            }
                        }

                        if (calcular_valores_agregados)
                        {
                            ProyectoDAO.calcularCostoyFechas(Convert.ToInt32(Actividad.treepath.Substring(0, 8)) - 10000000);
                        }

                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadDAO.class", e);
            }
            return ret;
        }

        public static List<Actividad> GetActividadesPagina(int pagina, int numeroActividad, String filtro_busqueda, String columna_ordenada, String orden_direccion, String usuario)
        {

            List<Actividad> resultado = new List<Actividad>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM actividad c WHERE c.estado = 1",
                        "AND c.id in (SELECT u.actividadid FROM actividad_usuario u WHERE u.usuario=:usuario)");


                    string query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;

                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroActividad + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroActividad + ") + 1)");

                    resultado = db.Query<Actividad>(query, new { usuario }).AsList<Actividad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadDAO.class", e);
            }

            return resultado;
        }


        public static List<Actividad> getActividadesPorObjeto(int objetoId, int objetoTipo)
        {
            List<Actividad> ret = new List<Actividad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.Query<Actividad>("SELECT * FROM ACTIVIDAD WHERE estado=1 AND objeto_id=:objetoId AND objeto_tipo=:objetoTipo",
                        new { objetoId = objetoId, objetoTipo = objetoTipo }).AsList<Actividad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("19", "ActividadDAO.class", e);
            }
            return ret;
        }

    }
}
