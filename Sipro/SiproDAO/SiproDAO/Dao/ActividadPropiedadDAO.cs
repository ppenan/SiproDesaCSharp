using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ActividadPropiedadDAO
    {
        public static List<ActividadPropiedad> getActividadPropiedadesPorTipoActividadPagina(int idTipoActividad)
        {
            List<ActividadPropiedad> ret = new List<ActividadPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT p.* FROM actividad_propiedad p",
                        "INNER JOIN atipo_propiedad ptp ON ptp.actividad_propiedadid=p.id",
                        "INNER JOIN actividad_tipo pt ON pt.id=ptp.actividad_tipoid",
                        "WHERE pt.id=:id");

                    ret = db.Query<ActividadPropiedad>(query, new { id = idTipoActividad }).AsList<ActividadPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }

        public static long getTotalActividadPropiedades()
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = "SELECT COUNT(*) FROM actividad_propiedad p WHERE p.estado=1";
                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }

        //SIGUE 07-09-18 17:48
        // Dudas por que utiliza string en minuscula y String en mayúscula
        // por que en el where está r___, 
        // La comparativa WHERE p.estado=1 es correcta?
        public static List<ActividadPropiedad> getActividadPropiedadPaginaTotalDisponibles(int pagina, int numeroactividadpropiedades, String idPropiedades)
        {
            List<ActividadPropiedad> ret = new List<ActividadPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "select p from actividad_propiedad p  where p.estado = 1 ",
                        (idPropiedades != null && idPropiedades.Length > 0 ? " and p.id NOT IN (" + idPropiedades + ")" : "WHERE p.estado=1"));
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroactividadpropiedades + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroactividadpropiedades + ") + 1)");
                    ret = db.Query<ActividadPropiedad>(query).AsList<ActividadPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }
                    
    


        public static ActividadPropiedad GetActividadPropiedadPorId(int id)
        {
            ActividadPropiedad ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ActividadPropiedad>("SELECT * FROM actividad_propiedad WHERE id=:id", new { id = id });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }

        /// <summary>
        /// Método para Guardar
        /// </summary>
        /// <param name="actividadPropiedad"></param>
        /// <returns></returns>
        public static bool guardarActividadPropiedad(ActividadPropiedad actividadPropiedad)
        {
            bool ret = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>("SELECT COUNT(c.id) FROM actividad_propiedad c WHERE id=:id", new { id = actividadPropiedad.id });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE actividad_propiedad SET nombre=:nombre, descripcion=:descripcion, usuario_creo=:usuarioCreo, usuario_actualizo=:usuarioActualizo, " +
                            "fecha_creacion=:fechaCreacion, fecha_actualizacion=:fechaActualizacion, estado=:estado, dato_tipoid=:datoTipoid WHERE id=:id", actividadPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_actividad_propiedad.nextval FROM DUAL");
                        actividadPropiedad.id = sequenceId;
                        int guardado = db.Execute("INSERT INTO actividad_propiedad VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, " +
                            ":estado, :datoTipoId)", actividadPropiedad);

                        ret = guardado > 0 ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }


        public static bool eliminarActividadPropiedad(ActividadPropiedad actividadPropiedad)
        {
            bool ret = false;
            try
            {
                actividadPropiedad.estado = 0;
                actividadPropiedad.fechaActualizacion = DateTime.Now;
                ret = guardarActividadPropiedad(actividadPropiedad);
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }



/* public static boolean eliminarTotalActividadPropiedad(ActividadPropiedad actividadPropiedad){
    boolean ret = false;
    Session session = CHibernateSession.getSessionFactory().openSession();
    try{
        session.beginTransaction();
        session.delete(actividadPropiedad);
        session.getTransaction().commit();
        ret = true;
    }
    catch(Throwable e){
        CLogger.write("7", ActividadPropiedadDAO.class, e);
    }
    finally{
        session.close();
    }
    return ret;
} */

                    public static List<ActividadPropiedad> getActividadPropiedadesPagina(int pagina, int numeroActividadPropiedad,
                String filtro_busqueda, String columna_ordenada, String orden_direccion)
        {
            List<ActividadPropiedad> ret = new List<ActividadPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM actividad_propiedad c WHERE c.estado=1";
                    String query_a = "";                    

                    if(filtro_busqueda != null && filtro_busqueda.Length>0)
                    { 
                        query_a = String.Join("", query_a, "c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");

                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    query = columna_ordenada != null && columna_ordenada.Trim().Length > 0 ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion) : query;
                    
                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroActividadPropiedad + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroActividadPropiedad + ") + 1)");

                    ret = db.Query<ActividadPropiedad>(query).AsList<ActividadPropiedad>();
                }
            }
        
            catch (Exception e)
            {
                CLogger.write("8", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }

        // Sigue 10/09/2018
        public static long getTotalActividadPropiedad(String filtro_busqueda)
        {
            long ret = 0L;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = "SELECT COUNT(c.id) FROM actividad_propiedad c WHERE c.estado=1";
                    String query_a = "";
                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuario_creo LIKE '%" + filtro_busqueda + "%' ");
                        DateTime fecha_creacion;
                        if (DateTime.TryParse(filtro_busqueda, out fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }
                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));
                    ret = db.ExecuteScalar<long>(query);
                }
            }
            catch (Exception e)
            {
                CLogger.write("9", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }
                          

        public static List<ActividadPropiedad> getActividadPropiedadesPorTipoActividad(int idTipoActividad)
        {
            List<ActividadPropiedad> ret = new List<ActividadPropiedad>();
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    string query = String.Join(" ", "SELECT cp.* FROM actividad_propiedad cp",
                        "INNER JOIN atipo_propiedad atp ON atp.actividad_propiedadid=cp.id",
                        "INNER JOIN actividad_tipo at ON at.id=atp.actividad_tipoid",
                        "WHERE at.id=:idTipoComp AND cp.estado=1");
                    ret = db.Query<ActividadPropiedad>(query, new { idTipoComp = idTipoActividad }).AsList<ActividadPropiedad>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("10", "ActividadPropiedadDAO.class", e);
            }
            return ret;
        }

        /* Equivalente Java
         	public static List<ActividadPropiedad> getActividadPropiedadesPorTipoActividad(int idTipoActividad){
		List<ActividadPropiedad> ret = new ArrayList<ActividadPropiedad>();
		Session session = CHibernateSession.getSessionFactory().openSession();
		try{
			Query<ActividadPropiedad> criteria = session.createNativeQuery(" select cp.* "
				+ "from actividad_tipo ct "
				+ "join atipo_propiedad ctp ON ctp.actividad_tipoid = ct.id "
				+ "join actividad_propiedad cp ON cp.id = ctp.actividad_propiedadid "
				+ " where ct.id = :idTipoComp",ActividadPropiedad.class);
			
			criteria.setParameter("idTipoComp", idTipoActividad);
			ret = criteria.getResultList();
		}
		catch(Throwable e){
			CLogger.write("10", ActividadPropiedadDAO.class, e);
		}
		finally{
			session.close();
		}
		return ret;
	} 
         
        */


    }
}
