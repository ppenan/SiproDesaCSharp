using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ActividadPropiedadValorDAO
    {
        /// <summary>
        /// Obtiene el listado de actividades propiedad valor que pertenecen a la actividad
        /// </summary>
        /// <param name="actividadId">Identificador de la actividad</param>
        /// <returns>Lista de Actividad propiedad valor</returns>
        public static List<ActividadPropiedadValor> GetActividadTipoValorUsandoActividadId(int actividadId)
        {
            List<ActividadPropiedadValor> resultado = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT * FROM actividad_propiedad_valor",
                                "WHERE actividadid = :actividadId",
                                "AND estado = 1", new { actividadId });
                    resultado = db.Query<ActividadPropiedadValor>(query).AsList<ActividadPropiedadValor>();
                }

                return resultado;
            }
            catch (Exception ex)
            {
                CLogger.write("1", "ActividadPropiedadValorDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Registra el objeto Actividad Propiedad Valor en base de datos
        /// </summary>
        /// <param name="actividadPropiedadValor">Objeto a registrar</param>
        /// <returns>TRUE si pudo guardar, FALSE en caso de error</returns>
        public static bool GuardarActividadPropiedadValor(ActividadPropiedadValor actividadPropiedadValor)
        {
            bool resultado = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int existe = db.ExecuteScalar<int>(
                        "SELECT COUNT (*) FROM actividad_propiedad_valor WHERE actividadid = :actividadid AND actividad_propiedadid = :actividadPropiedadId",
                        new { actividadId = actividadPropiedadValor.actividadid, actividadPropiedadId = actividadPropiedadValor.actividadPropiedadid }
                        );

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE actividad_propiedad_valor SET valor_entero = :valorentero, valor_string = :valorString, valor_decimal = :valorDecimal, valor_tiempo = :valorTiempo, usuario_creo = :usuarioCreo, usuario_actualizo = :usuarioActualizo, fecha_creacion = :fechaCreacion, fecha_actualizacion = :fechaActualizacion, estado = :estado WHERE id = :id", actividadPropiedadValor);

                        resultado = (guardado > 0) ? true : false;
                    }
                    else
                    {
                        int guardado = db.Execute(
                            "INSERT INTO actividad_propiedad_valor VALUES (:actividadid, :actividadPropiedadid, :valorEntero, :valorString, :valorDecimal, :valorTiempo, :usuarioCreo, :usuarioActualizo :fechaCreacion, :fechaActualizacion, :estado)", actividadPropiedadValor);

                        resultado = (guardado > 0) ? true : false;
                    }
                }

            }
            catch (Exception ex)
            {
                CLogger.write("2", "ActividadPropiedadValorDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Elimina de la base de datos todas las actividades que pertenecen a la actividad y propiedad indicada
        /// </summary>
        /// <param name="actividadPropiedadValor">Objeto que contiene la información de la actividad y propiedad a borrar</param>
        /// <returns>TRUE si ejecutó correctamente el comando, FALSE en caso de error</returns>
        public static bool EliminarTotalActividadPropiedadValor(ActividadPropiedadValor actividadPropiedadValor)
        {
            bool resultado = false;
            
            try
            {
                using (DbConnection db = new OracleContext().getConnection()) {
                    int eliminado = db.Execute("DELETE FROM actividad_propiedad_valor WHERE actividadid = :actividadId AND actividad_propiedadid = :actividadPropiedadId",
                        new { actividadId = actividadPropiedadValor.actividadid, actividadPropiedadId = actividadPropiedadValor.actividadPropiedadid });

                    resultado = eliminado > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                CLogger.write("3", "ActividadPropiedadValorDAO.class", ex);
    		}
		
	    	return resultado;            
        }

        /// <summary>
        /// Obtiene el Valor de la Propiedad
        /// </summary>
        /// <param name="idPropiedad"></param>
        /// <param name="idActividad"></param>
        /// <returns></returns>
        public static ActividadPropiedadValor getValorPorActividadYPropiedad(int idPropiedad, int idActividad)
        {
            ActividadPropiedadValor ret = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    ret = db.QueryFirstOrDefault<ActividadPropiedadValor>("SELECT * FROM actividad_propiedad_valor WHERE actividadid=:actividadId AND " +
                        "actividad_propiedadid=:propiedadid", new { actividadId = idActividad, propiedadid = idPropiedad });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadPropiedadValorDAO.class", e);
            }
            return ret;
        }

    }
}
