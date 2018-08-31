using Microsoft.AspNetCore.Mvc;
using System;
using Utilities;

namespace SActividad.Controllers
{
    [Route("api/[controller]")]
    public class ActividadController : Controller
    {
        private class SActividad
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
            public float costo;
            public int acumulacionCostoId;
            public String acumulacionCostoNombre;
            public decimal presupuestoModificado;
            public decimal presupuestoPagado;
            public decimal presupuestoVigente;
            public decimal presupuestoDevengado;
            public int avanceFinanciero;
            public int estado;
            public int proyectoBase;
            public Boolean tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public int congelado;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int inversionNueva;
        }

        [HttpPost]
        public IActionResult ActividadsPagina([FromBody]dynamic value)
        {
            try
            {
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
                CLogger.write("2", "ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
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
                CLogger.write("2", "Borrar ActividadController.class", e);
                return BadRequest(500);
            }
        }




    }
}
