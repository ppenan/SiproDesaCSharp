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
                CLogger.write("1", "ProductoPropiedadValorDAO.class", ex);
            }

            return resultado;
        }
        
        public static bool GuardarActividadPropiedadValor(ActividadPropiedadValor valor)
        {
            return true;
        }
    }
}
