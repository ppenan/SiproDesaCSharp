using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace STipoAdquisicion.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class TipoAdquisicionController : Controller
    {
        class StTipoAdquisicion
        {
            public long id;
            public String nombre;
            public String cooperanteNombre;
            public int cooperantecodigo;
            public int cooperanteejercicio;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
            public Boolean convenioCdirecta;
        }

        [HttpPost]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult NumeroTipoAdquisicionesDisponibles([FromBody]dynamic value)
        {
            try
            {
                String cooperanteIds = value.tipoAdquisicionesIds;
                long total = TipoAdquisicionDAO.getTotalTipoAdquisicionDisponibles(cooperanteIds);
                return Ok(new { success = true, totaltiposAdquisiciones = total });
            }
            catch (Exception e)
            {
                CLogger.write("1", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult TiposAdquisicionTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                String idsTipoAdquisiciones = value.tipoAdquisicionesIds != null ? (string)value.tipoAdquisicionesIds : "0";
                int numeroTipoAdquisicion = value.numeroTipoAdquisicion != null ? (int)value.numeroTipoAdquisicion : 20;

                List<TipoAdquisicion> tipoAdquisiciones = TipoAdquisicionDAO.getTipoAdquisicionPaginaTotalDisponibles(pagina, numeroTipoAdquisicion, idsTipoAdquisiciones);
                List<StTipoAdquisicion> sttipoadquisicion = new List<StTipoAdquisicion>();

                foreach (TipoAdquisicion tipoAdquisicion in tipoAdquisiciones)
                {
                    StTipoAdquisicion temp = new StTipoAdquisicion();
                    temp.id = tipoAdquisicion.id;
                    temp.cooperantecodigo = tipoAdquisicion.cooperantecodigo;
                    temp.nombre = tipoAdquisicion.nombre;
                    temp.estado = tipoAdquisicion.estado;
                    temp.fechaActualizacion = Utils.getFechaHoraNull(tipoAdquisicion.fechaActualizacion);
                    temp.fechaCreacion = Utils.getFechaHora(tipoAdquisicion.fechaCreacion);
                    temp.usuarioActualizo = tipoAdquisicion.usuarioActualizo;
                    temp.usuarioCreo = tipoAdquisicion.usuarioCreo;
                    temp.convenioCdirecta = tipoAdquisicion.convenioCdirecta == 1 ? true : false;
                    sttipoadquisicion.Add(temp);
                }

                return Ok(new { success = true, cooperanteTipoAdquisiciones  = sttipoadquisicion });
            }
            catch (Exception e)
            {
                CLogger.write("2", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult NumeroTipoAdquisicion([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                long total = TipoAdquisicionDAO.getTotalTipoAdquisicion(filtro_busqueda);
                return Ok(new { success = true, totalTipoAdquisicion = total });
            }
            catch (Exception e)
            {
                CLogger.write("3", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{objetoId}/{objetoTipo}")]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult TipoAdquisicionPorObjeto(int objetoId, int objetoTipo)
        {
            try
            {
                List<TipoAdquisicion> tipoAdquisiciones = TipoAdquisicionDAO.getTipoAdquisicionPorObjeto(objetoId, objetoTipo);

                List<StTipoAdquisicion> sttipoadquisicion = new List<StTipoAdquisicion>();
                foreach (TipoAdquisicion tipoAdquisicion in tipoAdquisiciones)
                {
                    StTipoAdquisicion temp = new StTipoAdquisicion();
                    temp.id = tipoAdquisicion.id;
                    temp.cooperantecodigo = tipoAdquisicion.cooperantecodigo;
                    temp.nombre = tipoAdquisicion.nombre;
                    temp.estado = tipoAdquisicion.estado;
                    temp.fechaActualizacion = Utils.getFechaHoraNull(tipoAdquisicion.fechaActualizacion);
                    temp.fechaCreacion = Utils.getFechaHora(tipoAdquisicion.fechaCreacion);
                    temp.usuarioActualizo = tipoAdquisicion.usuarioActualizo;
                    temp.usuarioCreo = tipoAdquisicion.usuarioCreo;
                    temp.convenioCdirecta = tipoAdquisicion.convenioCdirecta == 1 ? true : false;
                    sttipoadquisicion.Add(temp);
                }
                return Ok(new { success = true, tipoAdquisiciones = sttipoadquisicion });
            }
            catch (Exception e)
            {
                CLogger.write("4", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Tipos de Adquisiciones - Crear")]
        public IActionResult TipoAdquisicion([FromBody]dynamic value)
        {
            try
            {
                TipoAdquisicionValidator validator = new TipoAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    TipoAdquisicion tipoAdquisicion = new TipoAdquisicion();
                    tipoAdquisicion.convenioCdirecta = value.convenioCdirecta != null ? (int)value.convenioCdirecta : 0;
                    tipoAdquisicion.cooperantecodigo = value.cooperantecodigo;
                    tipoAdquisicion.cooperanteejercicio = DateTime.Now.Year;
                    tipoAdquisicion.estado = 1;
                    tipoAdquisicion.fechaCreacion = DateTime.Now;
                    tipoAdquisicion.nombre = value.nombreTipoAdquisicion;
                    tipoAdquisicion.usuarioCreo = User.Identity.Name;

                    bool guardado = TipoAdquisicionDAO.guardarTipoAdquisicion(tipoAdquisicion);

                    return Ok(new
                    {
                        id = tipoAdquisicion.id,
                        usuarioCreo = tipoAdquisicion.usuarioCreo,
                        usuarioActualizo = tipoAdquisicion.usuarioActualizo,
                        fechaCreacion = Utils.getFechaHora(tipoAdquisicion.fechaCreacion),
                        fechaActualizacion = Utils.getFechaHoraNull(tipoAdquisicion.fechaActualizacion),
                        success = guardado
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Tipos de Adquisiciones - Editar")]
        public IActionResult TipoAdquisicion(int id, [FromBody]dynamic value)
        {
            try
            {
                TipoAdquisicionValidator validator = new TipoAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    TipoAdquisicion tipoAdquisicion = new TipoAdquisicion();
                    tipoAdquisicion.convenioCdirecta = value.convenioCdirecta != null ? (int)value.convenioCdirecta : 0;
                    tipoAdquisicion.cooperantecodigo = value.cooperantecodigo;
                    tipoAdquisicion.cooperanteejercicio = DateTime.Now.Year;
                    tipoAdquisicion.fechaActualizacion = DateTime.Now;
                    tipoAdquisicion.nombre = value.nombreTipoAdquisicion;
                    tipoAdquisicion.usuarioActualizo = User.Identity.Name;

                    bool guardado = TipoAdquisicionDAO.guardarTipoAdquisicion(tipoAdquisicion);

                    return Ok(new
                    {
                        id = tipoAdquisicion.id,
                        usuarioCreo = tipoAdquisicion.usuarioCreo,
                        usuarioActualizo = tipoAdquisicion.usuarioActualizo,
                        fechaCreacion = Utils.getFechaHora(tipoAdquisicion.fechaCreacion),
                        fechaActualizacion = Utils.getFechaHoraNull(tipoAdquisicion.fechaActualizacion),
                        success = guardado
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Tipos de Adquisiciones - Eliminar")]
        public IActionResult TipoAdquisicion(int id)
        {
            try
            {
                TipoAdquisicion tipoAdquisicion = TipoAdquisicionDAO.getTipoAdquisicionPorId(id);
                tipoAdquisicion.usuarioActualizo = User.Identity.Name;

                bool eliminado = TipoAdquisicionDAO.borrarTipoAdquisicion(tipoAdquisicion);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("7", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult TipoAdquisicionPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroTipoAdquisicion = value.numeroTipoAdquisicion != null ? (int)value.numeroTipoAdquisicion : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<TipoAdquisicion> tipoAdquisiciones = TipoAdquisicionDAO.getTipoAdquisicionPagina(pagina, numeroTipoAdquisicion, filtro_busqueda, columna_ordenada, orden_direccion);
                List<StTipoAdquisicion> stTipoAdquisicion = new List<StTipoAdquisicion>();

                foreach (TipoAdquisicion tipoAdquisicion in tipoAdquisiciones)
                {
                    StTipoAdquisicion temp = new StTipoAdquisicion();
                    temp.id = tipoAdquisicion.id;
                    temp.cooperantecodigo = tipoAdquisicion.cooperantecodigo;
                    temp.nombre = tipoAdquisicion.nombre;
                    temp.estado = tipoAdquisicion.estado;
                    temp.fechaActualizacion = Utils.getFechaHoraNull(tipoAdquisicion.fechaActualizacion);
                    temp.fechaCreacion = Utils.getFechaHora(tipoAdquisicion.fechaCreacion);
                    temp.usuarioActualizo = tipoAdquisicion.usuarioActualizo;
                    temp.usuarioCreo = tipoAdquisicion.usuarioCreo;
                    temp.convenioCdirecta = tipoAdquisicion.convenioCdirecta == 1 ? true : false;
                    stTipoAdquisicion.Add(temp);
                }

                return Ok(new { success = true, tipoAdquisiciones = stTipoAdquisicion });
            }
            catch (Exception e)
            {
                CLogger.write("8", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{adquisicionTipoId}")]
        [Authorize("Tipos de Adquisiciones - Visualizar")]
        public IActionResult ConvenioCDirecta(int adquisicionTipoId)
        {
            try
            {
                TipoAdquisicion tipoAdquisicion = TipoAdquisicionDAO.getTipoAdquisicionPorId(adquisicionTipoId);
                return Ok(new { success = true, esConvenioCDirecta = tipoAdquisicion.convenioCdirecta == 1 ? true : false });
            }
            catch (Exception e)
            {
                CLogger.write("9", "TipoAdquisicionController.class", e);
                return BadRequest(500);
            }
        }
    }
}
