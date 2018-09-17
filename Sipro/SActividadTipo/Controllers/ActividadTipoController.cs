using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Utilities;

namespace SActividadTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ActividadTipoController : Controller
    {
        private class Stactividadtipo
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpGet]
        [Authorize("Actividad Tipos - Visualizar")]
        public IActionResult GetActividadtipos()
        {
            try
            {
                List<ActividadTipo> actividadtipos = ActividadTipoDAO.ActividadTipos();
                List<Stactividadtipo> stactividadtipos = new List<Stactividadtipo>();

                foreach (ActividadTipo actividadtipo in actividadtipos)
                {
                    Stactividadtipo temp = new Stactividadtipo();

                    temp.descripcion = actividadtipo.descripcion;
                    temp.estado = actividadtipo.estado;
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadtipo.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadtipo.fechaCreacion);
                    temp.id = actividadtipo.id;
                    temp.nombre = actividadtipo.nombre;
                    temp.usuarioActualizo = actividadtipo.usuarioActualizo;
                    temp.usuarioCreo = actividadtipo.usuarioCreo;

                    stactividadtipos.Add(temp);
                }

                return Ok(new { success = true, actividadTipo = stactividadtipos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Tipos - Visualizar")]
        public IActionResult ActividadTiposPagina([FromBody] dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 0;
                int numeroCooperantesTipo = value.numero_actividades_tipo != null ? (int)value.numero_actividades_tipo : 0;
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;

                List<ActividadTipo> actividadtipos = ActividadTipoDAO.ActividadTiposPagina(pagina, numeroCooperantesTipo, filtro_busqueda, columna_ordenada, orden_direccion);

                List<Stactividadtipo> stactividadtipos = new List<Stactividadtipo>();

                foreach (ActividadTipo actividadtipo in actividadtipos)
                {
                    Stactividadtipo temp = new Stactividadtipo();

                    temp.descripcion = actividadtipo.descripcion;
                    temp.estado = actividadtipo.estado;
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadtipo.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadtipo.fechaCreacion);
                    temp.id = actividadtipo.id;
                    temp.nombre = actividadtipo.nombre;
                    temp.usuarioActualizo = actividadtipo.usuarioActualizo;
                    temp.usuarioCreo = actividadtipo.usuarioCreo;

                    stactividadtipos.Add(temp);
                }

                return Ok(new { success = true, actividadtipos = stactividadtipos });
            }
            catch (Exception ex)
            {
                CLogger.write("2", "ActividadTipoController.class", ex);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Tipos - Crear")]
        public IActionResult ActividadTipo([FromBody]dynamic value)
        {
            try
            {
                ActividadTipoValidator validator = new ActividadTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ActividadTipo actividadTipo = new ActividadTipo();
                    actividadTipo.nombre = value.nombre;
                    actividadTipo.descripcion = value.descripcion;
                    actividadTipo.usuarioCreo = User.Identity.Name;
                    actividadTipo.fechaCreacion = DateTime.Now;
                    actividadTipo.estado = 1;

                    bool guardado = false;
                    guardado = ActividadTipoDAO.GuardarActividadTipo(actividadTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                AtipoPropiedad satipoPropiedad = new AtipoPropiedad();
                                satipoPropiedad.actividadTipoid = actividadTipo.id;
                                satipoPropiedad.actividadPropiedadid = Convert.ToInt32(idPropiedad);
                                satipoPropiedad.fechaCreacion = DateTime.Now;
                                satipoPropiedad.usuarioCreo = User.Identity.Name;
                                guardado = guardado & ATipoPropiedadDAO.GuardarATipoPropiedad(satipoPropiedad);
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            actividadTipo.id,
                            actividadTipo.usuarioCreo,
                            fechaCreacion = Utils.ConvierteAFormatoFecha(actividadTipo.fechaCreacion),
                            actividadTipo.usuarioActualizo,
                            fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadTipo.fechaActualizacion)
                        });
                    }
                    else
                    {
                        return Ok(new { success = false });
                    }
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Actividad Tipos - Editar")]
        public IActionResult Actividadtipo(int id, [FromBody] dynamic value)
        {
            try
            {
                ActividadTipoValidator validator = new ActividadTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ActividadTipo actividadTipo = ActividadTipoDAO.ActividadTipoPorId(id);

                    actividadTipo.nombre = value.nombre;
                    actividadTipo.descripcion = value.descripcion;
                    actividadTipo.fechaActualizacion = DateTime.Now;
                    actividadTipo.usuarioActualizo = User.Identity.Name;

                    bool guardado = false;
                    guardado = ActividadTipoDAO.GuardarActividadTipo(actividadTipo);

                    if (guardado)
                    {
                        List<AtipoPropiedad> propiedadesTemp = ATipoPropiedadDAO.GetATipoPropiedades(actividadTipo.id);

                        if (propiedadesTemp != null)
                        {
                            foreach (AtipoPropiedad atipoPropiedad in propiedadesTemp)
                            {
                                guardado = guardado & ATipoPropiedadDAO.EliminarTotalATipoPropiedad(atipoPropiedad);
                            }

                            if (guardado)
                            {
                                string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                                String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                                if (idsPropiedades != null && idsPropiedades.Length > 0)
                                {
                                    foreach (String idPropiedad in idsPropiedades)
                                    {
                                        AtipoPropiedad atipoPropiedad = new AtipoPropiedad();
                                        atipoPropiedad.actividadTipoid = actividadTipo.id;
                                        atipoPropiedad.actividadPropiedadid = Convert.ToInt32(idPropiedad);
                                        atipoPropiedad.fechaCreacion = DateTime.Now;
                                        atipoPropiedad.usuarioCreo = User.Identity.Name;

                                        guardado = guardado & ATipoPropiedadDAO.GuardarATipoPropiedad(atipoPropiedad);
                                    }
                                }
                            }
                            else
                            {
                                return Ok(new { success = false });
                            }
                        }

                        return Ok(new
                        {
                            success = guardado,
                            actividadTipo.id,
                            actividadTipo.usuarioCreo,
                            fechaCreacion = Utils.ConvierteAFormatoFecha(actividadTipo.fechaCreacion),
                            actividadTipo.usuarioActualizo,
                            fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadTipo.fechaActualizacion)
                        });
                    }
                    else
                    {
                        return Ok(new { success = false });
                    }
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception ex)
            {
                CLogger.write("4", "ActividadTipoController.class", ex);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Actividad Tipos - Eliminar")]
        public IActionResult ActividadTipo(int id)
        {
            try
            {
                bool eliminado = false;

                ActividadTipo actividadTipo = ActividadTipoDAO.ActividadTipoPorId(id);
                actividadTipo.usuarioActualizo = User.Identity.Name;

                eliminado = ActividadTipoDAO.EliminarActividadTipo(actividadTipo);

                return Ok(new { success = eliminado });
            }
            catch (Exception ex)
            {
                CLogger.write("5", "ActividadTipoController.class", ex);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Tipos - Visualizar")]
        public IActionResult NumeroActividadTipos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                long total = ActividadTipoDAO.GetTotalActividadTipo(filtro_busqueda);

                return Ok(new { success = true, totalactividadtipos = total });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadTipoController.class", e);
                return BadRequest(500);
            }
        }

    }

}
