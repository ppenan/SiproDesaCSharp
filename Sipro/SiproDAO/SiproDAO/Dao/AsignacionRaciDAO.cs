using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class AsignacionRaciDAO
    {
        public static List<AsignacionRaci> getAsignacionesRaci(int objetoId, int objetoTipo, String lineaBase)
        {
            List<AsignacionRaci> ret = new List<AsignacionRaci>();

            try
            {
                using (DbConnection db = lineaBase != null ? new OracleContext().getConnectionHistory() : new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT a.* FROM .asignacion_raci a",
                        "WHERE a.estado=1 AND a.objeto_id=:objId AND a.objeto_tipo=:objTipo",
                        lineaBase != null ? "AND a.linea_base '%" + lineaBase + "%'" : "");

                    ret = db.Query<AsignacionRaci>(query, new { objId = objetoId, objTipo = objetoTipo }).AsList<AsignacionRaci>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "AsignacionRaciDAO.class", e);
            }
            return ret;
        }

        public static List<Colaborador> getColaboradoresPorProyecto(int proyectoId, String lineaBase)
        {
            List<Colaborador> ret = new List<Colaborador>();
            try
            {
                using (DbConnection db = lineaBase != null ? new OracleContext().getConnectionHistory() : new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT distinct c.* FROM asignacion_raci ar",
                    "INNER JOIN colaborador c ON c.id=ar.colaboradorid",
                    "WHERE ar.objeto_tipo = 5",
                    lineaBase != null ? "AND ar.linea_base LIKE '%" + lineaBase + "%'" : "",
                    "AND ar.estado=1",
                    "AND ar.objeto_id IN (",
                        "SELECT a.id actividad a",
                        "WHERE a.estado = 1",
                        "AND a.treePath LIKE '" + (10000000 + proyectoId) + "%'",
                        lineaBase != null ? "AND a.linea_base LIKE '%" + lineaBase + "%'" : "",
                    ")");

                    ret = db.Query<Colaborador>(query).AsList<Colaborador>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "AsignacionRaciDAO.class", e);
            }
            return ret;
        }

        public static AsignacionRaci getAsignacionPorRolTarea(int objetoId, int objetoTipo, String rol, String lineaBase)
        {
            AsignacionRaci ret = null;

            try
            {
                String query = "";
                if (lineaBase != null)
                {
                    using (DbConnection db = new OracleContext().getConnectionHistory())
                    {
                        query = String.Join(" ", "SELECT a.* FROM asignacion_raci a",
                            "WHERE a.objeto_id=:objetoId",
                            "AND a.objeto_tipo=:objetoTipo",
                            "AND LOWER(a.rol_raci)=:rol",
                            "AND a.estado=1",
                            "AND a.linea_base LIKE '%" + lineaBase + "%'");

                        ret = db.QueryFirstOrDefault<AsignacionRaci>(query, new { objetoId = objetoId, objetoTipo = objetoTipo, rol = rol });
                    }
                }
                else
                {
                    using (DbConnection db = new OracleContext().getConnection())
                    {
                        query = String.Join(" ", "SELECT a.* FROM asignacion_raci a",
                            "WHERE a.objeto_id=:id",
                            "AND a.objeto_tipo=:objetoTipo",
                            "AND LOWER(a.rol_raci)=:rol",
                            "AND a.estado=1");

                        ret = db.QueryFirstOrDefault<AsignacionRaci>(query, new { objetoId = objetoId, objetoTipo = objetoTipo, rol = rol.ToLower() });
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "AsignacionRaciDAO.class", e);
            }
            return ret;
        }

        public static Colaborador getResponsablePorRol(int objetoId, int objetoTipo, String rol, String lineaBase)
        {
            Colaborador ret = null;
            try
            {
                using (DbConnection db = lineaBase != null ? new OracleContext().getConnectionHistory() : new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT c.* FROM colaborador c, asignacion_raci a",
                        "WHERE c.id=a.colaboradorid",
                        "AND a.objeto_id =:objId",
                        "AND a.objeto_tipo =:objTipo",
                        "AND a.rol_raci=:rol ",
                        "AND a.estado=1 ",
                        lineaBase != null ? "and a.linea_base like '%" + lineaBase + "%'" : "");

                    ret = db.QueryFirstOrDefault<Colaborador>(query, new { objId = objetoId, objTipo = objetoTipo, rol = rol });
                }
            }
            catch (Exception e)
            {
                CLogger.write("4", "AsignacionRaciDAO.class", e);
            }
            return ret;
        }

        public static bool GuardarAsignacion(AsignacionRaci asignacion)
        {
            bool resultado = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {

                    int existe = db.ExecuteScalar<int>("SELECT COUNT (*) FROM asignacion_raci WHERE id= :id", new
                    {
                        asignacion.id
                    });

                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE asignacion_raci SET matriz_raciid ) = :MatrizRaciid, colaboradorid = :colaboradorid, rol_raci = :rolRaci, objeto_id = :rolId, objeto_tipo = :objetoTipo, usuario_creo = :usuarioCreo, usuario_actualizo = :usuarioActualizo, fecha_creacion = :fechaCreacion, fecha_actualizacion = :fechaActualizacion, estado = :estado", asignacion);
                        resultado = (guardado > 0) ? true : false;

                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_asignacion_raci.nextval FROM DUAL");
                        asignacion.id = sequenceId;

                        int guardado = db.Execute("INSERT INTO asignacion_raci VALUES (:id, :MatrizRaciid, :colaboradorid, :rolRaci, :objetoId, :objetoTipo, :ususuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado, :matrizRacis, :colaborars, :asignacionracis)", asignacion);

                        resultado = (guardado > 0) ? true : false;
                    }

                }
            }
            catch (Exception ex)
            {
                CLogger.write("5", "AsignacionRaciDAO.class", ex);
            }

            return resultado;
        }

        public static bool EliminarTotalAsignacion(AsignacionRaci asignacion)
        {
            bool resultado = false;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute(
                        "DELETE FROM asignacion_raci WHERE id = :id",
                        new { asignacion.id }
                        );

                    resultado = (eliminado > 0) ? true : false;
                }
            }
            catch (Exception ex)
            {
                CLogger.write("7", "AsignacionRaciDAO.class", ex);
            }

            return resultado;
        }

        public static List<AsignacionRaci> GetAsignacionPorTarea(int objetoId, int objetoTipo, String lineaBase)
        {
            List<AsignacionRaci> resultado = null;
            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    String query = String.Join(" ", "SELECT * FROM asignacion_raci",
                                "WHERE objeto_id = :objId",
                                "AND objeto_tipo = :objTipo",
                                "AND estado = 1", new { objId = objetoId, objTipo = objetoTipo });
                    resultado = db.Query<AsignacionRaci>(query).AsList<AsignacionRaci>();
                }
            }
            catch (Exception ex)
            {
                CLogger.write("8", "AsignacionRaciDAO.class", ex);
            }

            return resultado;
        }
    }
}
