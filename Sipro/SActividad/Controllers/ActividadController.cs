using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Utilities;


namespace SActividad.Controllers
{
    [Route("api/[controller]")]
    public class ActividadController : Controller
    {
        private class StActividad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public String fechaInicio;
            public String fechaFin;
            public int actividadtipoid;
            public String actividadtiponombre;
            public int porcentajeavance;
            public int programa;
            public int subprograma;
            public int proyecto;
            public int actividad;
            public int obra;
            public int renglon;
            public int ubicacionGeografica;
            public String longitud;
            public String latitud;
            public int prececesorId;
            public int predecesorTipo;
            public int duracion;
            public String duracionDimension;
            public decimal? costo;
            public int acumulacionCostoId;
            public String acumulacionCostoNombre;
            public decimal presupuestoModificado;
            public decimal presupuestoPagado;
            public decimal presupuestoVigente;
            public decimal presupuestoDevengado;
            public int avanceFinanciero;
            public int estado;
            public Int64 proyectoBase;
            public Boolean tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public int congelado;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int inversionNueva;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        private class Stasignacionroles
        {
            public int id;
            public String nombre;
            public String rol;
            public String nombrerol;
        }


        [HttpPost]
        public IActionResult ActividadsPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 0;
                int numeroActividades = value.numeroactividades != null ? (int)value.numeroactividades : 0;

                string filtro_nombre = value.filtro_nombre;
                string filtro_usuario_creo = value.filtro_usuario_creo;
                string filtro_fecha_creacion = value.filtro_fecha_creacion;
                string columna_ordenada = value.columna_ordenada;
                string orden_direccion = value.orden_direccion;


                List<Actividad> actividades = ActividadDAO.GetActividadesPagina(
                    pagina,
                    numeroActividades,
                    User.Identity.Name
                    );

                List<StActividad> stActividades = new List<StActividad>();                

                int congelado = 0;
                String fechaElegibilidad = null;
                String fechaCierre = null;


                if (actividades != null && actividades.Count > 0)
                {
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(actividades[0].treepath);

                    if (proyecto != null)
                    {
                        congelado = proyecto.congelado ?? 0;

                        fechaElegibilidad = proyecto.fechaElegibilidad?.ToString("dd/MM/yyyy H:mm:ss");
                        fechaCierre = proyecto.fechaCierre?.ToString("dd/MM/yyyy H:mm:ss");

                    }
                }

                foreach (Actividad actividad in actividades)
                {
                    StActividad temp = new StActividad();

                    temp.descripcion = actividad.descripcion;
                    temp.estado = actividad.estado;

                    temp.fechaActualizacion = actividad.fechaActualizacion?.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaCreacion = actividad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");

                    temp.id = actividad.id;
                    temp.nombre = actividad.nombre;
                    temp.usuarioActualizo = actividad.usuarioActualizo;
                    temp.usuarioCreo = actividad.usuarioCreo;


                    // TODO actividad tipo
                    //temp.actividadtipoid = actividad.ActividadTipo().Id();
                    //temp.actividadtiponombre = actividad.ActividadTipo().Nombre();

                    temp.porcentajeavance = actividad.porcentajeAvance;
                    temp.programa = actividad.programa ?? 0;
                    temp.subprograma = actividad.subprograma ?? 0;
                    temp.proyecto = actividad.proyecto ?? 0;
                    temp.actividad = actividad.actividad ?? 0;
                    temp.obra = actividad.obra ?? 0;
                    temp.renglon = actividad.renglon ?? 0;
                    temp.ubicacionGeografica = actividad.ubicacionGeografica ?? 0;
                    temp.longitud = actividad.longitud;
                    temp.latitud = actividad.latitud;
                    temp.costo = actividad.costo;

                    //temp.acumulacionCostoId = actividad.AcumulacionCosto().Id();
                    //temp.acumulacionCostoNombre = actividad.AcumulacionCosto().Nombre();

                    temp.proyectoBase = actividad.proyectoBase ?? 0;

                    temp.fechaInicioReal = actividad.fechaInicioReal?.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaFinReal = actividad.fechaFinReal?.ToString("dd/MM/yyyy H:mm:ss");

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 5);
                    temp.inversionNueva = actividad.inversionNueva;

                    temp.congelado = congelado;
                    temp.fechaElegibilidad = fechaElegibilidad;
                    temp.fechaCierre = fechaCierre;

                    stActividades.Add(temp);
                }

                /*                response_text=new GsonBuilder().serializeNulls().create().toJson(stactividads);
                                response_text = String.join("", "\"actividads\":",response_text);
                                response_text = String.join("", "{\"success\":true,", response_text,"}");
                                     */


                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult Actividads()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadController.class", e);
                return BadRequest(500);
            }

        }


        [HttpPost]
        public IActionResult GuardarActividad([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadController.class", e);
                return BadRequest(500);
            }
        }


        [HttpDelete]
        public IActionResult BorrarActividad([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("4", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult numeroActividads([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("5", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult numeroActividadesPorObjeto([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult getActividadesPaginaPorObjeto([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult obtenerActividadPorId([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("8", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult getActividadPorId([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("9", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult GuardarModal([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("10", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult GetCantidadHistoria([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("11", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult GetHistoria([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("12", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult GetValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("13", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }




    }
}
