using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ActividadDAO
    {
        /// <summary>
        /// Devuelve todas las actividades
        /// </summary>
        /// <param name="usuario">Usuario que busca sus actividades</param>
        /// <returns>Listado de actividades por usuario</returns>
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

        /// <summary>
        /// Obtiene la actividad usando su Identificador
        /// </summary>
        /// <param name="id">Identificador de la actividad a buscar</param>
        /// <returns>Información de la actividad</returns>
        public static Actividad GetActividadPorId(int id)
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

        /// <summary>
        /// Registra la actividad en la Base de datos
        /// </summary>
        /// <param name="Actividad">Objeto de actividad</param>
        /// <param name="calcular_valores_agregados">Determina si debe calcular los valores agregados</param>
        /// <returns>TRUE si guardó con éxito, FALSE en caso de error</returns>
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
                                    Actividad actividad = ActividadDAO.GetActividadPorId(Convert.ToInt32(Actividad.objetoId));
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
                            guardado = db.Execute("INSERT INTO actividad_usuario VALUES (:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au);
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
                                guardado = db.Execute("INSERT INTO actividad_usuario VALUES (:actividadid, :usuario, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", au_admin);
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

        /// <summary>
        /// Devuelve las actividades por página
        /// </summary>
        /// <param name="pagina">Número de página de la que desea información</param>
        /// <param name="numeroActividad">Número de actividades a buscar</param>
        /// <param name="filtro_busqueda">Cadena de búsqueda</param>
        /// <param name="columna_ordenada">Nombre de columna a ordenar</param>
        /// <param name="orden_direccion">ASC o DESC para ordenar la columna</param>
        /// <param name="usuario">Usuario que desea las actividades por página</param>
        /// <returns>Listado de actividades por usuario y paginadas</returns>
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
                CLogger.write("4", "ActividadDAO.class", e);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene las actividades por objeto, por tipo y con estado = 1
        /// </summary>
        /// <param name="objetoId">Identificador del objeto</param>
        /// <param name="objetoTipo">Tipo del objeto</param>
        /// <returns>Lista de actividades</returns>
        public static List<Actividad> GetActividadesPorObjeto(int objetoId, int objetoTipo)
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
                CLogger.write("5", "ActividadDAO.class", e);
            }
            return ret;
        }


        /// <summary>
        /// Obtiene el total de las actividades con estado 1, usando el filtro
        /// indicado y que pertenecen al usuario logueado
        /// </summary>
        /// <param name="filtro_busqueda">Filtro a usar en la búsqueda</param>
        /// <param name="usuario">Nombre del usuario</param>
        /// <returns>Número total de actividades</returns>
        public static long GetTotalActividades(String filtro_busqueda, string usuario)
        {
            long resultado = 0L;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query_a = "";
                    String query = "SELECT COUNT(a.id) FROM actividad a WHERE a.estado = 1 ";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "a.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " a.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        if (DateTime.TryParse(filtro_busqueda, out DateTime fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(a.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, "AND a.id in (SELECT u.actividadid FROM actividad_usuario u WHERE u.usuario= :usuario)");
                    resultado = db.ExecuteScalar<long>(query, new { usuario });
                }
            }
            catch (Exception ex)
            {
                CLogger.write("6", "ActividadDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el total de actividades que pertenecen a un objeto dado
        /// </summary>
        /// <param name="objetoId">Identificador del objeto</param>
        /// <param name="objetoTipo">Nivel en el que se encuentra la actividad</param>
        /// <param name="filtro_busqueda">Condición de búsqueda</param>
        /// <param name="usuario">Nombre del usuario que tiene asignada esas actividades</param>
        /// <returns>Total de actividades</returns>
        public static long GetTotalActividadesPorObjeto(int objetoId, int objetoTipo, string filtro_busqueda, string usuario)
        {
            long resultado = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query_a = "";
                    String query = "SELECT COUNT(a.id) FROM actividad a WHERE a.estado = 1 AND a.objeto_id = :objetoId AND a.objeto_tipo = :objetoTipo ";


                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "a.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " a.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(a.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = String.Join(" ", query, "AND a.id in (SELECT u.actividadid FROM actividad_usuario u WHERE u.usuario= :usuario)");
                    resultado = db.ExecuteScalar<long>(query, new { objetoId, objetoTipo, usuario });
                }
            }
            catch (Exception ex)
            {
                CLogger.write("7", "ActividadDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene actividades por objeto y realiza la paginación
        /// </summary>
        /// <param name="pagina">Número de página</param>
        /// <param name="numeroActividades">Número de actividad actual</param>
        /// <param name="objetoId">Id del objeto</param>
        /// <param name="objetoTipo">Tipo del objeto</param>
        /// <param name="filtroBusqueda">Criterio de búsqueda</param>
        /// <param name="columnaOrdenada">Nombre de la columna a ordenar</param>
        /// <param name="ordenDireccion">Tipo de orden a usar (ASC, DESC)</param>
        /// <param name="usuario">Usuario dueño de las actividades</param>
        /// <returns>Listado de actividades ya paginadas</returns>
        public static List<Actividad> GetActividadesPaginaPorObjeto(int pagina, int numeroActividades, int objetoId,
            int objetoTipo, string filtro_busqueda, string columnaOrdenada, string ordenDireccion, string usuario)
        {
            List<Actividad> resultado = new List<Actividad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT a.* FROM Actividad a WHERE a.estado = 1 AND a.objeto_id = :objetoId AND a.objeto_tipo = :objetoTipo ";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join(" ", query_a, "a.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " a.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(a.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    if (usuario != null)
                    {
                        query = String.Join("", query, "AND a.estado = 1 AND a.id in (SELECT u.actividadid FROM actividad_usuario u where u.usuario=:usuario)");
                    }
                    query = columnaOrdenada != null && columnaOrdenada.Trim().Length > 0 ? String.Join(" ", query, " ORDER BY", columnaOrdenada, ordenDireccion) : query;
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroActividades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroActividades + ") + 1)");

                    resultado = db.Query<Actividad>(query, new { objetoId, objetoTipo, usuario }).AsList<Actividad>();
                }
            }
            catch (Exception ex)
            {
                CLogger.write("8", "ActividadDAO.class", ex);
            }
            return resultado;
        }

        /// <summary>
        /// Obtiene un historico de las actividades
        /// </summary>
        /// <param name="id">Id de la actividad a buscar</param>
        /// <returns>Cadena con el historico de la actividad</returns>
        public static string GetVersiones(int id)
        {
            String resultado = "";

            try
            {
                using (DbConnection db = new OracleContext().getConnectionHistory())
                {
                    String query = "SELECT DISTINCT(version) FROM sipro_history.actividad WHERE id = " + id;

                    List<dynamic> versiones = db.Query<dynamic>(query).AsList<dynamic>();

                    if (versiones != null)
                    {
                        for (int i = 0; i < versiones.Count; i++)
                        {
                            if (resultado.Length > 0)
                            {
                                resultado += ",";
                            }
                            resultado += (int)versiones[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.write("9", "ActividadDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Crea el query de la consulta para obtener la historia de la actividad
        /// </summary>
        /// <param name="id">Identificador de la actividad</param>
        /// <param name="version">Version de la actividad</param>
        /// <returns>Historia de la actividad</returns>
        public static string GetHistoria(int id, int version)
        {
            String resultado = "";
            String query = "SELECT a.version, a.nombre, a.descripcion, ati.nombre tipo, a.costo, ac.nombre tipo_costo, "
                    + " a.programa, a.subprograma, a.proyecto, a.actividad, a.obra, a.renglon, a.ubicacion_geografica, a.latitud, a.longitud, "
                    + " a.fecha_inicio, a.fecha_fin, a.duracion, a.fecha_inicio_real, a.fecha_fin_real, "
                    + " a.porcentaje_avance, "
                    + " a.fecha_creacion, a.usuario_creo, a.fecha_actualizacion, a.usuario_actualizo, "
                    + " CASE WHEN a.estado = 1 "
                    + " THEN 'Activo' "
                    + " ELSE 'Inactivo' "
                    + " END AS estado "
                    + " FROM sipro_history.actividad a "
                    + " JOIN sipro_history.actividad_tipo ati ON a.actividad_tipoid = ati.id "
                    + " JOIN sipro_history.acumulacion_costo ac ON a.acumulacion_costo = ac.id "
                    + " WHERE a.id = " + id
                    + " AND a.version = " + version;

            String[] campos = {"Version", "Nombre", "Descripción", "Tipo", "Monto Planificado", "Tipo Acumulación de Monto Planificado",
                "Programa", "Subprograma", "Proyecto", "Actividad", "Obra", "Renglon", "Ubicación Geográfica", "Latitud", "Longitud",
                "Fecha Inicio", "Fecha Fin", "Duración", "Fecha Inicio Real", "Fecha Fin Real",
                "Porcentaje de Avance",
                "Fecha Creación", "Usuario que creo", "Fecha Actualización", "Usuario que actualizó",
                "Estado"};
            resultado = CHistoria.getHistoria(query, campos);

            return resultado;
        }
    }
}
