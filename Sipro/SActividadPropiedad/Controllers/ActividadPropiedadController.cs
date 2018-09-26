using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Utilities;

namespace SActividadPropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ActividadPropiedadController : Controller
    {
        private class Stactividadpropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datotipoid;
            public String datotiponombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpGet("{idActividadTipo}")]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult ActividadPropiedadPaginaPorTipo(int idActividadTipo)
        {
            try
            {
                List<ActividadPropiedad> actividadpropiedades = ActividadPropiedadDAO.getActividadPropiedadesPorTipoActividadPagina(idActividadTipo);
                List<Stactividadpropiedad> stactividadpropiedad = new List<Stactividadpropiedad>();
                foreach (ActividadPropiedad actividadpropiedad in actividadpropiedades)
                {
                    Stactividadpropiedad temp = new Stactividadpropiedad();
                    temp.id = actividadpropiedad.id;
                    temp.nombre = actividadpropiedad.nombre;
                    temp.descripcion = actividadpropiedad.descripcion;

                    actividadpropiedad.datoTipos = DatoTipoDAO.getDatoTipo(actividadpropiedad.datoTipoid);

                    temp.datotipoid = actividadpropiedad.datoTipoid;
                    temp.datotiponombre = actividadpropiedad.datoTipos.nombre;

                    //temp.fechaActualizacion = actividadpropiedad.fechaActualizacion != null ? actividadpropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    //temp.fechaCreacion = actividadpropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaCreacion);

                    temp.usuarioActualizo = actividadpropiedad.usuarioActualizo;
                    temp.usuarioCreo = actividadpropiedad.usuarioCreo;
                    stactividadpropiedad.Add(temp);
                }
                return Ok(new { success = true, actividadpropiedades = stactividadpropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult ActividadPropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int numeroActividadPropiedad = value.numero_actividad_propiedad != null ? (int)value.numero_actividad_propiedad : 20;
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;
                List<ActividadPropiedad> actividadpropiedades = ActividadPropiedadDAO.getActividadPropiedadesPagina(pagina, numeroActividadPropiedad,
                        filtro_busqueda, columna_ordenada, orden_direccion);
                List<Stactividadpropiedad> stactividadpropiedad = new List<Stactividadpropiedad>();
                foreach (ActividadPropiedad actividadpropiedad in actividadpropiedades)
                {
                    Stactividadpropiedad temp = new Stactividadpropiedad();
                    temp.id = actividadpropiedad.id;
                    temp.nombre = actividadpropiedad.nombre;
                    temp.descripcion = actividadpropiedad.descripcion;
                    actividadpropiedad.datoTipos = DatoTipoDAO.getDatoTipo(actividadpropiedad.datoTipoid);
                    temp.datotipoid = actividadpropiedad.datoTipoid;
                    temp.datotiponombre = actividadpropiedad.datoTipos.nombre;
                    //temp.fechaActualizacion = actividadpropiedad.fechaActualizacion != null ? actividadpropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    //temp.fechaCreacion = actividadpropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaCreacion);


                    temp.usuarioActualizo = actividadpropiedad.usuarioActualizo;
                    temp.usuarioCreo = actividadpropiedad.usuarioCreo;
                    stactividadpropiedad.Add(temp);
                }
                return Ok(new { success = true, actividadpropiedades = stactividadpropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult ActividadPropiedadesTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                String idsPropiedades = value.idspropiedades != null ? (string)value.idspropiedades : "0";
                int numeroActividadPropiedad = value.numeroactividadpropiedad != null ? (int)value.numeroactividadpropiedad : 20;
                List<ActividadPropiedad> actividadpropiedades = ActividadPropiedadDAO.getActividadPropiedadPaginaTotalDisponibles(pagina, numeroActividadPropiedad, idsPropiedades);
                List<Stactividadpropiedad> stactividadpropiedad = new List<Stactividadpropiedad>();
                foreach (ActividadPropiedad actividadpropiedad in actividadpropiedades)
                {
                    Stactividadpropiedad temp = new Stactividadpropiedad();
                    temp.id = actividadpropiedad.id;
                    temp.nombre = actividadpropiedad.nombre;
                    temp.descripcion = actividadpropiedad.descripcion;
                    actividadpropiedad.datoTipos = DatoTipoDAO.getDatoTipo(actividadpropiedad.datoTipoid);
                    temp.datotipoid = actividadpropiedad.datoTipoid;
                    temp.datotiponombre = actividadpropiedad.datoTipos.nombre;
                    //temp.fechaActualizacion = actividadpropiedad.fechaActualizacion != null ? actividadpropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaActualizacion);
                    //temp.fechaCreacion = actividadpropiedad.fechaCreacion != null ? actividadpropiedad.fechaCreacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadpropiedad.fechaCreacion);
                    temp.usuarioActualizo = actividadpropiedad.usuarioActualizo;
                    temp.usuarioCreo = actividadpropiedad.usuarioCreo;
                    stactividadpropiedad.Add(temp);
                }
                return Ok(new { success = true, actividadpropiedades = stactividadpropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult NumeroActividadPropiedadesDisponibles()
        {
            try
            {
                long total = ActividadPropiedadDAO.getTotalActividadPropiedades();
                return Ok(new { success = true, totalactividadpropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }
        
        [HttpPost]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult NumeroActividadPropiedades([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                long total = ActividadPropiedadDAO.getTotalActividadPropiedad(filtro_busqueda);
                return Ok(new { success = true, totalactividadpropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividad Propiedades - Crear")]
        public IActionResult ActividadPropiedad([FromBody]dynamic value)
        {
            try
            {
                ActividadPropiedadValidator validator = new ActividadPropiedadValidator();
                ValidationResult results = validator.Validate(value);
                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = (int)value.datotipoid;

                    ActividadPropiedad actividadPropiedad = new ActividadPropiedad();
                    actividadPropiedad.nombre = nombre;
                    actividadPropiedad.usuarioCreo = User.Identity.Name;
                    actividadPropiedad.fechaCreacion = DateTime.Now;
                    actividadPropiedad.estado = 1;
                    actividadPropiedad.descripcion = descripcion;
                    actividadPropiedad.datoTipoid = datoTipoId;

                    bool result = ActividadPropiedadDAO.guardarActividadPropiedad(actividadPropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = actividadPropiedad.id,
                        usuarioCreo = actividadPropiedad.usuarioCreo,
                        fechaCreacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaCreacion),
                        usuarioActualizo = actividadPropiedad.usuarioActualizo,
                        fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaActualizacion)
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Actividad Propiedades - Editar")]
        public IActionResult ActividadPropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                ActividadPropiedadValidator validator = new ActividadPropiedadValidator();
                ValidationResult results = validator.Validate(value);
                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = (int)value.datotipoid;

                    ActividadPropiedad actividadPropiedad = ActividadPropiedadDAO.GetActividadPropiedadPorId(id);
                    actividadPropiedad.nombre = nombre;
                    actividadPropiedad.usuarioActualizo = User.Identity.Name;
                    actividadPropiedad.fechaActualizacion = DateTime.Now;
                    actividadPropiedad.descripcion = descripcion;
                    actividadPropiedad.datoTipoid = datoTipoId;

                    bool result = ActividadPropiedadDAO.guardarActividadPropiedad(actividadPropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = actividadPropiedad.id,
                        usuarioCreo = actividadPropiedad.usuarioCreo,
                        fechaCreacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaCreacion),
                        usuarioActualizo = actividadPropiedad.usuarioActualizo,
                        fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaActualizacion)
                    });
                }
                else
                    return Ok(new { success = false });

            }
            catch (Exception e)
            {
                CLogger.write("7", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Actividad Propiedades - Eliminar")]
        public IActionResult ActividadPropiedad(int id)
        {
            try
            {
                ActividadPropiedad actividadPropiedad = ActividadPropiedadDAO.GetActividadPropiedadPorId(id);
                actividadPropiedad.usuarioActualizo = User.Identity.Name;
                actividadPropiedad.fechaActualizacion = DateTime.Now;
                bool eliminado = ActividadPropiedadDAO.eliminarActividadPropiedad(actividadPropiedad);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idActividad}/{idActividadTipo}")]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult ActividadPropiedadPorTipo(int idActividad, int idActividadTipo)
        {
            try
            {
                List<ActividadPropiedad> actividadpropiedades = ActividadPropiedadDAO.getActividadPropiedadesPorTipoActividad(idActividadTipo);

                List<Dictionary<String, Object>> campos = new List<Dictionary<String, Object>>();
                foreach (ActividadPropiedad actividadpropiedad in actividadpropiedades)
                {
                    Dictionary<String, Object> campo = new Dictionary<String, Object>();
                    campo.Add("id", actividadpropiedad.id);
                    campo.Add("nombre", actividadpropiedad.nombre);
                    campo.Add("tipo", actividadpropiedad.datoTipoid);
                    ActividadPropiedadValor actividadPropiedadValor = ActividadPropiedadValorDAO.getValorPorActividadYPropiedad(actividadpropiedad.id, idActividad);
                    if (actividadPropiedadValor != null)
                    {
                        switch (actividadpropiedad.datoTipoid)
                        {
                            case 1:
                                campo.Add("valor", actividadPropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", actividadPropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", actividadPropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", actividadPropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", actividadPropiedadValor.valorTiempo != null ? actividadPropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
                                break;
                        }
                    }
                    else
                    {
                        campo.Add("valor", "");
                    }
                    campos.Add(campo);
                }

                List<object> estructuraCamposDinamicos = CFormaDinamica.convertirEstructura(campos);

                return Ok(new { success = true, actividadpropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("9", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idActividadTipo}")]
        [Authorize("Actividad Propiedades - Visualizar")]
        public IActionResult ActividadPropiedadPorTipo(int idActividadTipo)
        {
            try
            {
                List<ActividadPropiedad> actividadPropiedades = ActividadPropiedadDAO.getActividadPropiedadesPorTipoActividadPagina(idActividadTipo);
                List<Stactividadpropiedad> stActividadPropiedades = new List<Stactividadpropiedad>();

                foreach (ActividadPropiedad actividadPropiedad in actividadPropiedades)
                {
                    Stactividadpropiedad temp = new Stactividadpropiedad();
                    temp.id = actividadPropiedad.id;
                    temp.nombre = actividadPropiedad.nombre;
                    temp.descripcion = actividadPropiedad.descripcion;

                    actividadPropiedad.datoTipos = DatoTipoDAO.getDatoTipo(actividadPropiedad.datoTipoid);
                    temp.datotipoid = actividadPropiedad.datoTipoid;
                    temp.datotiponombre = actividadPropiedad.datoTipos.nombre;

                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividadPropiedad.fechaCreacion);
                    temp.usuarioActualizo = actividadPropiedad.usuarioActualizo;
                    temp.usuarioCreo = actividadPropiedad.usuarioCreo;
                    temp.estado = actividadPropiedad.estado;

                    stActividadPropiedades.Add(temp);
                }

                return Ok(new { success = true, actividadpropiedades = stActividadPropiedades });
            }
            catch (Exception e)
            {
                CLogger.write("10", "ActividadPropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
