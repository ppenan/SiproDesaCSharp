using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ATipoPropiedadDAO
    {
        /// <summary>
        /// Guarda la actividad tipo de propiedad
        /// </summary>
        /// <param name="atipoPropiedad">Objeto a guardar de la Actividad Tipo Propiedad</param>
        /// <returns>TRUE registro con éxito, FALSE generó error</returns>
        public static bool GuardarATipoPropiedad(AtipoPropiedad atipoPropiedad)
        {
            bool resultado = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int guardado = 0;

                    int existe = db.ExecuteScalar<int>("SELECT COUNT(*) FROM atipo_propiedad WHERE actividad_tipoid = :actividadTipoId AND actividad_propiedadid = :actividadPropiedadId", new { actividadTipoId = atipoPropiedad.actividadTipoid, actividadPropiedadId = atipoPropiedad.actividadPropiedadid });

                    if (existe > 0)
                    {
                        guardado = db.Execute("UPDATE atipo_propiedad SET usuario_creo = :usuarioCreo, usuario_actualizo = :usuarioActualizo, fecha_creacion = :fechaCreacion, fecha_actualizacion = :fechaActualizacion WHERE actividad_tipoid = :actividadTipoid AND actividad_propiedadid = :actividadPropiedadid", atipoPropiedad);
                    }
                    else
                    {
                        guardado = db.Execute("INSERT INTO atipo_propiedad VALUES (:actividadTipoid, :actividadPropiedadid, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion)", atipoPropiedad);
                    }

                    resultado = guardado > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                CLogger.write("1", "ATipoPropiedadDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Eliminación total de la actividad tipo propiedad
        /// </summary>
        /// <param name="aTipoPropiedad">Objeto de la actividad tipo propiedad</param>
        /// <returns>TRUE eliminación exitosa, FALSE en caso de error</returns>
        public static bool EliminarTotalATipoPropiedad(AtipoPropiedad aTipoPropiedad)
        {
            bool resultado = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute("DELETE FROM atipo_propiedad WHERE actividad_tipoid = :actividadTipoId AND actividad_propiedadid = :actividadPropiedadId", new { actividadTipoId = aTipoPropiedad.actividadTipoid, actividadPropiedadId = aTipoPropiedad.actividadPropiedadid });

                    resultado = eliminado > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                CLogger.write("3", "ATipoPropiedadDAO.class", ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el listado de los tipos de propiedades
        /// </summary>
        /// <param name="actividadTipoId">Identificador del tipo de actividad</param>
        /// <returns>Lista de tipos de propiedades que pertenecen a la actividad indicada</returns>
        public static List<AtipoPropiedad> GetATipoPropiedades(int actividadTipoId)
        {
            List<AtipoPropiedad> resultado = new List<AtipoPropiedad>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    resultado = db.Query<AtipoPropiedad>("SELECT * FROM atipo_propiedad WHERE actividad_tipoid = :actividadTipoId", new { actividadTipoId }).AsList<AtipoPropiedad>();
                }
            }
            catch (Exception ex)
            {
                CLogger.write("4", "ATipoPropiedadDAO.class", ex);
            }

            return resultado;
        }
    }
}
