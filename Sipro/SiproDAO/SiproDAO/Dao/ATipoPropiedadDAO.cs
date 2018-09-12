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


        public static List<AtipoPropiedad> GetATipoPropiedades(int actividadTipoId)
        {
            /*                   
                 ACTIVIDAD_TIPOID                          NOT NULL NUMBER(10)                  
                 ACTIVIDAD_PROPIEDADID                     NOT NULL NUMBER(10)                  
                 USUARIO_CREO                              NOT NULL VARCHAR2(30)                
                 USUARIO_ACTUALIZO                                  VARCHAR2(30)                
                 FECHA_CREACION                            NOT NULL TIMESTAMP(0)                
                 FECHA_ACTUALIZACION                                TIMESTAMP(0)                
            */
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
