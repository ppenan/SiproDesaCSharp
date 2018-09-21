using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SCategoriaAdquisicion.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class CategoriaAdquisicionController : Controller
    {
        private class Stcategoriaadquisicion
        {
            public long id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpGet]
        [Authorize("Categorías de Adquisiciones - Visualizar")]
        public IActionResult CategoriasAdquisicion()
        {
            try
            {
                List<CategoriaAdquisicion> categoriaAdquisiciones = CategoriaAdquisicionDAO.getCategoriaAdquisicion();

                List<Stcategoriaadquisicion> lstCategoriaAdquisicion = new List<Stcategoriaadquisicion>();
                foreach (CategoriaAdquisicion categoriaAdquisicion in categoriaAdquisiciones)
                {
                    Stcategoriaadquisicion temp = new Stcategoriaadquisicion();
                    temp.id = categoriaAdquisicion.id;
                    temp.nombre = categoriaAdquisicion.nombre;
                    temp.descripcion = categoriaAdquisicion.descripcion;
                    temp.usuarioCreo = categoriaAdquisicion.usuarioCreo;
                    temp.usuarioActualizo = categoriaAdquisicion.usuarioActualizo;
                    temp.fechaCreacion = Utils.getFechaHora(categoriaAdquisicion.fechaCreacion);
                    temp.fechaActualizacion = Utils.getFechaHoraNull(categoriaAdquisicion.fechaActualizacion);
                    temp.estado = categoriaAdquisicion.estado;
                    lstCategoriaAdquisicion.Add(temp);
                }

                return Ok(new { success = true, categoriasAdquisicion = lstCategoriaAdquisicion });
            }
            catch (Exception e)
            {
                CLogger.write("1", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Categorías de Adquisiciones - Visualizar")]
        public IActionResult NumeroCategoriaPorObjeto([FromBody]dynamic value)
        {
            try
            {

                String filtro_busqueda = (string)value.filtro_busqueda;
                long total = CategoriaAdquisicionDAO.getTotalCategoriaAdquisicion(filtro_busqueda);

                return Ok(new { success = true, totalCategoriaAdquisiciones = total });
            }
            catch (Exception e)
            {
                CLogger.write("2", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Categorías de Adquisiciones - Visualizar")]
        public IActionResult CategoriaAdquisicionPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroCategoriaAdquisicion = value.numeroCategoriaAdquisicion != null ? value.numeroCategoriaAdquisicion : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion; 
                List<CategoriaAdquisicion> categoriaAdquisiciones = CategoriaAdquisicionDAO.getCategoriaAdquisicionPagina(pagina, numeroCategoriaAdquisicion, filtro_busqueda, columna_ordenada, orden_direccion);
                List<Stcategoriaadquisicion> lstCategoriaAdquisicion = new List<Stcategoriaadquisicion>();
                foreach (CategoriaAdquisicion categoriaAdquisicion in categoriaAdquisiciones)
                {
                    Stcategoriaadquisicion temp = new Stcategoriaadquisicion();
                    temp.id = categoriaAdquisicion.id;
                    temp.nombre = categoriaAdquisicion.nombre;
                    temp.descripcion = categoriaAdquisicion.descripcion;
                    temp.usuarioCreo = categoriaAdquisicion.usuarioCreo;
                    temp.usuarioActualizo = categoriaAdquisicion.usuarioActualizo;
                    temp.fechaCreacion = Utils.getFechaHora(categoriaAdquisicion.fechaCreacion);
                    temp.fechaActualizacion = Utils.getFechaHoraNull(categoriaAdquisicion.fechaActualizacion);
                    temp.estado = categoriaAdquisicion.estado;
                    lstCategoriaAdquisicion.Add(temp);
                }

                return Ok(new { success = true, categoriaAdquisiciones = lstCategoriaAdquisicion });
            }
            catch (Exception e)
            {
                CLogger.write("3", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Categorías de Adquisiciones - Crear")]
        public IActionResult CategoriaAdquisicion([FromBody]dynamic value)
        {
            try
            {
                CategoriaAdquisicionValidator validator = new CategoriaAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool ret = false;
                    CategoriaAdquisicion categoria = new CategoriaAdquisicion();
                    categoria.nombre = value.nombre;
                    categoria.descripcion = value.descripcion;
                    categoria.estado = 1;
                    categoria.fechaCreacion = DateTime.Now;
                    categoria.usuarioCreo = User.Identity.Name;

                    ret = CategoriaAdquisicionDAO.guardarCategoria(categoria);

                    return Ok(new
                    {
                        success = ret,
                        id = categoria.id,
                        usuarioCreo = categoria.usuarioCreo,
                        usuarioActualizo = categoria.usuarioActualizo,
                        fechaCreacion = Utils.getFechaHora(categoria.fechaCreacion),
                        fechaActualizacion = Utils.getFechaHoraNull(categoria.fechaActualizacion)
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Categorías de Adquisiciones - Editar")]
        public IActionResult CategoriaAdquisicion(int id, [FromBody]dynamic value)
        {
            try
            {
                CategoriaAdquisicionValidator validator = new CategoriaAdquisicionValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool ret = false;
                    CategoriaAdquisicion categoria = CategoriaAdquisicionDAO.getCategoriaPorId(id);
                    categoria.nombre = value.nombre;
                    categoria.descripcion = value.descripcion;
                    categoria.fechaActualizacion = DateTime.Now;
                    categoria.usuarioActualizo = User.Identity.Name;

                    ret = CategoriaAdquisicionDAO.guardarCategoria(categoria);

                    return Ok(new
                    {
                        success = ret,
                        id = categoria.id,
                        usuarioCreo = categoria.usuarioCreo,
                        usuarioActualizo = categoria.usuarioActualizo,
                        fechaCreacion = Utils.getFechaHora(categoria.fechaCreacion),
                        fechaActualizacion = Utils.getFechaHoraNull(categoria.fechaActualizacion)
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Categorías de Adquisiciones - Eliminar")]
        public IActionResult CategoriaAdquisicion(int id)
        {
            try
            {
                CategoriaAdquisicion categoria = CategoriaAdquisicionDAO.getCategoriaPorId(id);
                categoria.usuarioActualizo = User.Identity.Name;
                bool eliminado = CategoriaAdquisicionDAO.eliminarCategoria(categoria);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("6", "CategoriaAdquisicionController.class", e);
                return BadRequest(500);
            }
        }
    }
}
