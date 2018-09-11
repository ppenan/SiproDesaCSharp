using Dapper;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace SiproDAO.Dao
{
    public class ActividadTipoDAO
    {
        /// <summary>
        /// Obtiene los tipos de actividad que tengan estado = 1
        /// </summary>
        /// <returns>Lista de Actividad Tipo</returns>
        public static List<ActividadTipo> ActividadTipos()
        {
            List<ActividadTipo> result = new List<ActividadTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    result = db.Query<ActividadTipo>("SELECT * FROM actividad_tipo WHERE estado = 1").AsList<ActividadTipo>();
                }
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadTipoDAO.class", e);
            }

            return result;
        }

        /// <summary>
        /// Obtiene tipos de actividad por ID
        /// </summary>
        /// <param name="id">Identificador del Tipo de Actividad</param>
        /// <returns>Tipo de Actividad</returns>
        public static ActividadTipo ActividadTipoPorId(int id)
        {

            ActividadTipo resultado = null;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    resultado = db.QueryFirstOrDefault<ActividadTipo>("SELECT * FROM actividad_tipo WHERE id = :id AND estado = 1", new { id });
                }

            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadTipoDAO.class", e);
            }

            return resultado;
        }

        /// <summary>
        /// Guarda o actualiza un Tipo de actividad
        /// </summary>
        /// <param name="actividadTipo">Objeto de tipo Actividad Tipo que va a guardar o actualizar</param>
        /// <returns>TRUE si guardó o actualizó la información, FALSE si hubo un error</returns>
        public static bool GuardarActividadTipo(ActividadTipo actividadTipo)
        {
            bool resultado = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {

                    int existe = db.ExecuteScalar<int>("SELECT COUNT (*) FROM actividad_tipo WHERE id= :id", new
                    {
                        actividadTipo.id
                    });


                    if (existe > 0)
                    {
                        int guardado = db.Execute("UPDATE actividad_tipo SET nombre = :nombre, descripcion = :descripcion, usuario_creo = :usuarioCreo, usuario_actualizo = :usuarioActualizo, fecha_creacion = :fechaCreacion, fecha_actualizacion = :fechaActualizacion, estado = :estado WHERE id = :id", actividadTipo);

                        resultado = (guardado > 0) ? true : false;
                    }
                    else
                    {
                        int sequenceId = db.ExecuteScalar<int>("SELECT seq_actividad_tipo.nextval FROM DUAL");

                        actividadTipo.id = sequenceId;

                        int guardado = db.Execute("INSERT INTO actividad_tipo VALUES (:id, :nombre, :descripcion, :usuarioCreo, :usuarioActualizo, :fechaCreacion, :fechaActualizacion, :estado)", actividadTipo);

                        resultado = (guardado > 0) ? true : false;
                    }
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadTipoDAO.class", e);
            }
            return resultado;
        }


        /// <summary>
        /// Elimina tipo de actividad, solo cambia su estado a valor 0
        /// </summary>
        /// <param name="actividadTipo">Objeto de actividad tipo</param>
        /// <returns>TRUE si se pudo realizar el cambio de estado, FALSE si hubo error</returns>
        public static bool EliminarActividadTipo(ActividadTipo actividadTipo)
        {
            bool resultado = false;

            try
            {
                actividadTipo.estado = 0;
                actividadTipo.fechaActualizacion = DateTime.Now;

                resultado = GuardarActividadTipo(actividadTipo);
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadTipoDAO.class", e);
            }

            return resultado;
        }

        /// <summary>
        /// Elimina a nivel lógico la Actividad Tipo
        /// </summary>
        /// <param name="actividadTipo">Objeto de Actividad Tipo</param>
        /// <returns>TRUE si pudo eliminar la actividad tipo, FALSE en caso de error</returns>
        public static bool EliminarTotalActividadTipo(ActividadTipo actividadTipo)
        {
            bool resultado = false;

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {
                    int eliminado = db.Execute(
                        "DELETE FROM actividad_tipo WHERE id = :id",
                        new { actividadTipo }
                        );

                    resultado = (eliminado > 0) ? true : false;
                }
            }
            catch (Exception e)
            {
                CLogger.write("5", "ActividadTipoDAO.class", e);
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve Actividad Tipo con paginación y de acuerdo a la página solicitada
        /// </summary>
        /// <param name="pagina">Número de página que se busca</param>
        /// <param name="numeroactividadstipo">Número de la actividad tipo</param>
        /// <param name="filtro_busqueda">Cadena a buscar en nombre, descripción y fecha de creación</param>
        /// <param name="columna_ordenada">Nombre de la columna a ordenar</param>
        /// <param name="orden_direccion">Tipo de ordenamiento ASC o DESC</param>
        /// <returns>Listado de Actividad Tipo, ya paginada y en el orden solicitado</returns>
        public static List<ActividadTipo> ActividadTiposPagina(
            int pagina,
            int numeroactividadstipo,
            String filtro_busqueda,
            String columna_ordenada,
            String orden_direccion)
        {
            List<ActividadTipo> resultado = new List<ActividadTipo>();

            try
            {
                using (DbConnection db = new OracleContext().getConnection())
                {

                    String query = "SELECT * FROM (SELECT a.*, rownum r__ FROM (SELECT * FROM actividad_tipo c WHERE c.estado = 1 ";
                    String query_a = "";

                    if (filtro_busqueda != null && filtro_busqueda.Length > 0)
                    {
                        query_a = String.Join("", query_a, " c.nombre LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.descripcion LIKE '%" + filtro_busqueda + "%' ");
                        query_a = String.Join("", query_a, (query_a.Length > 0 ? " OR " : ""), " c.usuarioCreo LIKE '%" + filtro_busqueda + "%' ");

                        if (DateTime.TryParse(filtro_busqueda, out DateTime fecha_creacion))
                        {
                            query_a = String.Join(" ", query_a, (query_a.Length > 0 ? " OR " : ""), " TO_DATE(TO_CHAR(c.fecha_creacion,'DD/MM/YY'),'DD/MM/YY') LIKE TO_DATE('" + fecha_creacion.ToString("dd/MM/yyyy") + "','DD/MM/YY') ");
                        }
                    }

                    query = String.Join(" ", query, (query_a.Length > 0 ? String.Join("", "AND (", query_a, ")") : ""));

                    query = (columna_ordenada != null && columna_ordenada.Trim().Length > 0)
                        ? String.Join(" ", query, "ORDER BY", columna_ordenada, orden_direccion)
                        : query;

                    query = String.Join(" ", query, ") a WHERE rownum < ((" + pagina + " * " + numeroactividadstipo + ") + 1) ) WHERE r__ >= (((" + pagina + " - 1) * " + numeroactividadstipo + ") + 1)");

                    resultado = db.Query<ActividadTipo>(query).AsList<ActividadTipo>();
                }

            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadTipoDAO.class", e);
            }

            return resultado;
        }
    }
}
