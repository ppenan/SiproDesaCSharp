using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;
using System;
using System.Collections.Generic;

namespace SActividad.Controllers
{
    //[Route("api/[controller]/[action]")]
    public class ActividadController : Controller
    {
        private class StActividad
        {
            public int id;
            public string nombre;
            public string descripcion;
            public string usuarioCreo;
            public string usuarioActualizo;
            public string fechaCreacion;
            public string fechaActualizacion;
            public string fechaInicio;
            public string fechaFin;
            public int actividadtipoid;
            public string actividadtiponombre;
            public int porcentajeavance;
            public int programa;
            public int subprograma;
            public int proyecto;
            public int actividad;
            public int obra;
            public int renglon;
            public int ubicacionGeografica;
            public string longitud;
            public string latitud;
            public int prececesorId;
            public int predecesorTipo;
            public int duracion;
            public string duracionDimension;
            public decimal? costo;
            public int acumulacionCostoId;
            public string acumulacionCostoNombre;
            public decimal presupuestoModificado;
            public decimal presupuestoPagado;
            public decimal presupuestoVigente;
            public decimal presupuestoDevengado;
            public int avanceFinanciero;
            public int estado;
            public int proyectoBase;
            public bool tieneHijos;
            public string fechaInicioReal;
            public string fechaFinReal;
            public int congelado;
            public string fechaElegibilidad;
            public string fechaCierre;
            public int inversionNueva;
        }

        private class Stdatadinamico
        {
            public string id;
            public string tipo;
            public string label;
            public string valor;
            public string valor_f;
        }

        private class Stasignacionroles
        {
            public int id;
            public string nombre;
            public string rol;
            public string nombrerol;
        }


        [HttpPost]
        [Authorize("Actividades - Visualizar")]
        public IActionResult ActividadesPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroActividades = value.numeroactividades != null ? (int)value.numeroactividades : 20;

                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null; ;

                List<Actividad> actividades = ActividadDAO.GetActividadesPagina(pagina, numeroActividades, filtro_busqueda, columna_ordenada, orden_direccion, User.Identity.Name);
                List<StActividad> stactividades = new List<StActividad>();

                int congelado = 0;
                String fechaElegibilidad = null;
                String fechaCierre = null;

                if (actividades != null && actividades.Count > 0)
                {
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(actividades[0].treepath);

                    if (proyecto != null)
                    {
                        congelado = proyecto.congelado ?? 0;

                        fechaElegibilidad = proyecto.fechaElegibilidad != null ? proyecto.fechaElegibilidad.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                        fechaCierre = proyecto.fechaCierre != null ? proyecto.fechaCierre.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    }
                }


                foreach (Actividad actividad in actividades)
                {
                    StActividad temp = new StActividad();
                    temp.descripcion = actividad.descripcion;
                    temp.estado = actividad.estado;

                    temp.fechaActualizacion = actividad.fechaActualizacion != null ? actividad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;                    
                    temp.fechaCreacion = actividad.fechaActualizacion != null ? actividad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    temp.id = actividad.id;
                    temp.nombre = actividad.nombre;
                    temp.usuarioActualizo = actividad.usuarioActualizo;
                    temp.usuarioCreo = actividad.usuarioCreo;

                    temp.actividadtipoid = actividad.ActividadTipo.Id;
                    temp.actividadtiponombre = actividad.ActividadTipo.Nombre;

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

                    temp.acumulacionCostoId = actividad.AcumulacionCosto.Id;
                    temp.acumulacionCostoNombre = actividad.AcumulacionCosto.Nombre;

                    // todo verificar si hizo el casteo correcto de long a int ?
                    temp.proyectoBase = actividad.proyectoBase != null ?  Convert.ToInt32(actividad.proyectoBase) : 0;

                    temp.fechaInicioReal = actividad.fechaInicioReal != null ? actividad.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    temp.fechaFinReal = actividad.fechaFinReal != null ? actividad.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 5);
                    temp.inversionNueva = actividad.inversionNueva;

                    temp.congelado = congelado;
                    temp.fechaElegibilidad = fechaElegibilidad;
                    temp.fechaCierre = fechaCierre;

                    stactividades.Add(temp);
                }

                /*
                response_text = new GsonBuilder().serializeNulls().create().toJson(stactividades);
                response_text = String.join("", "\"actividades\":", response_text);
                response_text = String.join("", "{\"success\":true,", response_text, "}");
                */

                return Ok(new { success = true, actividad = temp });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        public IActionResult Actividades()
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
