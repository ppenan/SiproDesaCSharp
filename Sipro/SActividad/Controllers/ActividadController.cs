using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Utilities;

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
            public Int64 acumulacionCostoId;
            public string acumulacionCostoNombre;
            public decimal presupuestoModificado;
            public decimal presupuestoPagado;
            public decimal presupuestoVigente;
            public decimal presupuestoDevengado;
            public int avanceFinanciero;
            public int estado;
            public Int64 proyectoBase;
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

                        fechaElegibilidad = Utils.ConvierteAFormatoFecha(proyecto.fechaElegibilidad);
                        fechaCierre = Utils.ConvierteAFormatoFecha(proyecto.fechaCierre);
                    }
                }

                foreach (Actividad actividad in actividades)
                {
                    StActividad temp = new StActividad();
                    temp.descripcion = actividad.descripcion;
                    temp.estado = actividad.estado;

                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividad.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividad.fechaCreacion);

                    temp.id = actividad.id;
                    temp.nombre = actividad.nombre;

                    temp.usuarioActualizo = actividad.usuarioActualizo;
                    temp.usuarioCreo = actividad.usuarioCreo;

                    actividad.actividadTipos = ActividadTipoDAO.ActividadTipoPorId(actividad.actividadTipoid);
                    temp.actividadtipoid = actividad.actividadTipos.id;
                    temp.actividadtiponombre = actividad.actividadTipos.nombre;

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

                    actividad.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(actividad.acumulacionCosto);
                    temp.acumulacionCostoId = actividad.acumulacionCostos.id;
                    temp.acumulacionCostoNombre = actividad.acumulacionCostos.nombre;

                    temp.proyectoBase = actividad.proyectoBase ?? 0;
                    temp.fechaInicioReal = Utils.ConvierteAFormatoFecha(actividad.fechaInicioReal);
                    temp.fechaFinReal = Utils.ConvierteAFormatoFecha(actividad.fechaFinReal);

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 5);
                    temp.inversionNueva = actividad.inversionNueva;

                    temp.congelado = congelado;
                    temp.fechaElegibilidad = fechaElegibilidad;
                    temp.fechaCierre = fechaCierre;

                    stactividades.Add(temp);
                }

                return Ok(new { success = true, actividades = stactividades });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet]
        [Authorize("Actividades - Visualizar")]
        public IActionResult Actividades()
        {
            try
            {
                List<Actividad> actividades = ActividadDAO.GetActividades(User.Identity.Name);
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
                        fechaElegibilidad = Utils.ConvierteAFormatoFecha(proyecto.fechaElegibilidad);
                        fechaCierre = Utils.ConvierteAFormatoFecha(proyecto.fechaCierre);
                    }
                }

                foreach (Actividad actividad in actividades)
                {
                    StActividad temp = new StActividad();

                    temp.descripcion = actividad.descripcion;
                    temp.estado = actividad.estado;
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(actividad.fechaActualizacion);
                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(actividad.fechaCreacion);
                    temp.id = actividad.id;
                    temp.nombre = actividad.nombre;
                    temp.usuarioActualizo = actividad.usuarioActualizo;
                    temp.usuarioCreo = actividad.usuarioCreo;

                    actividad.actividadTipos = ActividadTipoDAO.ActividadTipoPorId(actividad.actividadTipoid);
                    temp.actividadtipoid = actividad.actividadTipoid;
                    temp.actividadtiponombre = actividad.actividadTipos.nombre;

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

                    actividad.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(actividad.acumulacionCosto);
                    temp.acumulacionCostoId = actividad.acumulacionCosto;
                    temp.acumulacionCostoNombre = actividad.acumulacionCostos.nombre;

                    temp.fechaInicio = Utils.ConvierteAFormatoFecha(actividad.fechaInicio);
                    temp.fechaFin = Utils.ConvierteAFormatoFecha(actividad.fechaFin);

                    temp.proyectoBase = actividad.proyectoBase ?? 0;
                    temp.fechaInicioReal = Utils.ConvierteAFormatoFecha(actividad.fechaInicioReal);
                    temp.fechaFinReal = Utils.ConvierteAFormatoFecha(actividad.fechaFinReal);
                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 5);
                    temp.inversionNueva = actividad.inversionNueva;

                    temp.congelado = congelado;
                    temp.fechaElegibilidad = fechaElegibilidad;
                    temp.fechaCierre = fechaCierre; ;
                    stActividades.Add(temp);
                }

                return Ok(new { success = true, actividades = stActividades });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ActividadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Actividades - Crear")]
        public IActionResult GuardarActividad([FromBody]dynamic values)
        {
            try
            {
                ActividadValidator validator = new ActividadValidator();
                ValidationResult results = validator.Validate(values);

                bool resultado = false;

                if (results.IsValid)
                {
                    Int64? proyectoBase = 0;
                    Int64? componenteBase = 0;
                    Int64? productoBase = 0;

                    Actividad actividad = new Actividad();

                    actividad.nombre = values.nombre;
                    actividad.descripcion = values.descripcion;
                    actividad.actividadTipoid = values.actividadTipoId;
                    DateTime fechaInicio = values.fechainicio;

                    DateTime.TryParse((string)values.fechaFin, out DateTime fechaFin);
                    DateTime fechaFinal = fechaFin;

                    actividad.snip = values.snip;
                    actividad.programa = values.programa;
                    actividad.subprograma = values.subprograma;
                    actividad.proyecto = values.proyecto;
                    actividad.actividad = values.actividad;
                    actividad.obra = values.obra;
                    actividad.objetoId = values.objetoId;
                    actividad.objetoTipo = values.objetoTipo;
                    actividad.porcentajeAvance = values.porcentajeAvance;
                    //int duracion = values.duracion;
                    actividad.duracionDimension = values.duracionDimension;
                    actividad.longitud = values.longitud;
                    actividad.costo = values.costo;
                    actividad.latitud = values.latitud;
                    actividad.acumulacionCosto = values.acumulacionCosto;
                    actividad.renglon = values.renglon;
                    actividad.ubicacionGeografica = values.ubicacionGeografica;
                    actividad.inversionNueva = values.inversionNueva;

                    switch (values.objetoTipo)
                    {
                        case 1:
                            proyectoBase = values.objetoTipo;
                            break;
                        case 2:
                            componenteBase = values.objetoTipo;
                            break;
                        case 3:
                            productoBase = values.objetoTipo;
                            break;
                        case 4:
                            Subproducto subproducto = SubproductoDAO.getSubproductoPorId(values.objetoTipo);
                            productoBase = subproducto.productoid;
                            break;
                        case 5:
                            Actividad actividadBase = ActividadDAO.getActividadPorId(values.objetoTipo);
                            proyectoBase = actividadBase.proyectoBase;
                            componenteBase = actividadBase.componenteBase;
                            productoBase = actividadBase.productoBase;
                            break;
                    }

                    ActividadTipo actividadTipo = new ActividadTipo();
                    actividadTipo.id = actividad.actividadTipoid;

                    AcumulacionCosto acumulacionCosto = null;
                    if (actividad.acumulacionCosto != 0)
                    {
                        acumulacionCosto = new AcumulacionCosto();
                        acumulacionCosto.id = actividad.acumulacionCosto;
                    }

                    //TODO como convertir typetoken, se usa ?
                    //type = new TypeToken<List<stdatadinamico>>()
                    //{
                    //}.getType();
                    //List<Stdatadinamico> datos = (values.datadinamica != null && values.datadinamica.compareTo("{}") != 0) ? gson.fromJson(values.datadinamica, type) : new List<Stdatadinamico>();

                    Actividad actividadTemp = new Actividad();

                    Double fechaFinTimestamp = (fechaFinal.Ticks * 1.0 - fechaFinal.Ticks) / 86400000;
                    Double fechaInicioTimestamp = (fechaInicio.Ticks * 1.0 - fechaInicio.Ticks) / 86400000;

                    actividad.duracion = (int)(fechaFinTimestamp - fechaInicioTimestamp);
                    actividad.duracionDimension = "d";

                    // calcula la fecha de inicio real y el porcentaje de avance
                    if (actividad.porcentajeAvance > 0 && actividad.porcentajeAvance < 100)
                    {
                        actividad.fechaInicioReal = new DateTime();
                    }
                    else if (actividad.porcentajeAvance == 100)
                    {
                        actividad.fechaFinReal = new DateTime();
                        actividad.fechaInicioReal = new DateTime();
                    }

                    //actividad = new Actividad(actividadTipo, acumulacionCosto, nombre, descripcion, fechaInicio, fechaFin,
                    //        porcentajeAvance, usuario, null, new Date(), null, 1, snip, programa, subprograma, proyecto, iactividad, obra,
                    //        objetoId, objetoTipo, duracion, duracionDimension, null, null, latitud, longitud, costo, renglon, ubicacionGeografica, null, null, null
                    //        , proyectoBase, componenteBase, productoBase, fechaInicioReal, fechaFinReal, inversionNueva, null, null);

                    resultado = ActividadDAO.guardarActividad(actividad, true);

                    if (resultado)
                    {
                        List<AsignacionRaci> asignaciones_temp = AsignacionRaciDAO.GetAsignacionPorTarea(actividad.id, 5, null);

                        if (asignaciones_temp != null)
                        {
                            foreach (AsignacionRaci asign in asignaciones_temp)
                            {
                                AsignacionRaciDAO.EliminarTotalAsignacion(asign);
                            }
                        }

                        String asignaciones_param = values.asignacionroles;

                        if (!asignaciones_param.Equals(""))
                        {
                            String[] asignaciones = asignaciones_param.Split("\\|");

                            if (asignaciones.Length > 0)
                            {
                                foreach (String temp in asignaciones)
                                {
                                    AsignacionRaci asigna_temp = new AsignacionRaci();
                                    String[] datosaasignacion = temp.Split("~");
                                    Colaborador colaborador = new Colaborador();
                                    colaborador.id = Convert.ToInt16(datosaasignacion[0]);

                                    asigna_temp.colaboradorid = colaborador.id;
                                    asigna_temp.estado = 1;
                                    asigna_temp.fechaCreacion = new DateTime();
                                    asigna_temp.objetoId = actividad.id;
                                    asigna_temp.objetoTipo = 5;
                                    asigna_temp.rolRaci = datosaasignacion[1];
                                    asigna_temp.usuarioCreo = User.Identity.Name;

                                    resultado = resultado && AsignacionRaciDAO.GuardarAsignacion(asigna_temp);
                                }
                            }
                        }
                    }


                    if (resultado)
                    {
                        String pagosPlanificados = values.pagosPlanificados;
                        if (!actividad.acumulacionCosto.Equals(2) || pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(actividad.getId(), 5);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (actividad.acumulacionCosto.Equals(2) && pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {                            
                            JArray pagosArreglo = JArray.Parse((string)values.pagosPlanificados);

                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];

                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);

                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"].ToString()) : default(decimal);

                                PagoPlanificado pagoPlanificado = new PagoPlanificado();
                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = actividad.id;
                                pagoPlanificado.objetoTipo = 5;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                
                                resultado = resultado && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    // como se soluciona, debo crear el llamado a ActividadPropiedadValor y revisar el método
                    /*
                    List<ActividadPropiedadValor> valores_temp = actividad.getActividadPropiedadValors();
                    actividad.setActividadPropiedadValors(null);
                    if (valores_temp != null)
                    {
                        for (ActividadPropiedadValor valor : valores_temp)
                        {
                            ActividadPropiedadValorDAO.eliminarTotalActividadPropiedadValor(valor);
                        }
                    }
                    */

                    //foreach (Stdatadinamico data in datos)
                    //{
                    //    if (data.valor != null && data.valor.Length > 0 && data.valor.CompareTo("null") != 0)
                    //    {
                    //        ActividadPropiedad actividadPropiedad = ActividadPropiedadDAO.getActividadPropiedadPorId((int)(data.id));
                    //        ActividadPropiedadValorId idValor = new ActividadPropiedadValorId(actividad.getId(), (int)(data.id));
                    //        ActividadPropiedadValor valor = new ActividadPropiedadValor(idValor, actividad, actividadPropiedad, null, null, null, null,
                    //                usuario, null, new DateTime().toDate(), null, 1);

                    //        switch (actividadPropiedad.getDatoTipo().getId())
                    //        {
                    //            case 1:
                    //                valor.setValorString(data.valor);
                    //                break;
                    //            case 2:
                    //                valor.setValorEntero(Utils.String2Int(data.valor, null));
                    //                break;
                    //            case 3:
                    //                valor.setValorDecimal(Utils.String2BigDecimal(data.valor, null));
                    //                break;
                    //            case 4:
                    //                valor.setValorEntero(Utils.String2Boolean(data.valor, null));
                    //                break;
                    //            case 5:
                    //                SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
                    //                valor.setValorTiempo(data.valor_f.compareTo("") != 0 ? sdf.parse(data.valor_f) : null);
                    //                break;
                    //        }
                    //        result = (result && ActividadPropiedadValorDAO.guardarActividadPropiedadValor(valor));
                    //    }
                    //}

                    response_text = String.join("", "{ \"success\": ", (result ? "true" : "false"), ", "
                            + "\"id\": " + actividad.getId(), ","
                            , "\"usuarioCreo\": \"", actividad.getUsuarioCreo(), "\","
                            , "\"fechaCreacion\":\" ", Utils.formatDateHour(actividad.getFechaCreacion()), "\","
                            , "\"usuarioactualizo\": \"", actividad.getUsuarioActualizo() != null ? actividad.getUsuarioActualizo() : "", "\","
                            , "\"fechaactualizacion\": \"", Utils.formatDateHour(actividad.getFechaActualizacion()), "\","
                            , "\"fechaInicioReal\": ", actividad.getFechaInicioReal() != null ? "\"" + Utils.formatDate(actividad.getFechaInicioReal()) + "\"" : null, ","
                            , "\"fechaFinReal\": ", actividad.getFechaFinReal() != null ? "\"" + Utils.formatDate(actividad.getFechaFinReal()) + "\"" : null
                            , " }");
                }
                else
                    response_text = "{ \"success\": false }";












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
