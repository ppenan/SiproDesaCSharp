using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using System;
using System.Collections.Generic;
using Utilities;

namespace SSubproducto.Controllers
{
    [Route("api/[controller]")]
    public class SubproductoController : Controller
    {
        private class Stsubproducto
        {
            public int id;
            public int productoid;
            public int subproductoTipoid;
            public String subProductoTipo;
            public int ueunidadEjecutora;
            public int entidadentidad;
            public int ejercicio;
            public String nombreUnidadEjecutora;
            public String entidadnombre;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
            public long? snip;
            public int? programa;
            public int? subprograma;
            public int? proyecto;
            public int? actividad;
            public int? obra;
            public int? renglon;
            public int? ubicacionGeografica;
            public int duracion;
            public String duracionDimension;
            public String fechaInicio;
            public String fechaFin;
            public String latitud;
            public String longitud;
            public decimal costo;
            public int acumulacionCostoid;
            public String acumulacionCostoNombre;
            public bool tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int inversionNueva;
            public int orden;
            public String treepath;
            public int nivel;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        [HttpPost]
        public IActionResult SubproductoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                int productoid = value.productoid != null ? (int)value.productoid : default(int);
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<Subproducto> subproductos = SubproductoDAO.getSubproductosPagina(pagina, registros, productoid, filtro_busqueda, columna_ordenada,
                    orden_direccion, User.Identity.Name);

                List<Stsubproducto> lstsubproductos = new List<Stsubproducto>();
                foreach (Subproducto subproducto in subproductos)
                {
                    Stsubproducto temp = new Stsubproducto();
                    temp.id = subproducto.id;
                    temp.nombre = subproducto.nombre;
                    temp.descripcion = subproducto.descripcion;
                    temp.programa = subproducto.programa;
                    temp.subprograma = subproducto.subprograma;
                    temp.proyecto = subproducto.proyecto;
                    temp.obra = subproducto.obra;
                    temp.actividad = subproducto.actividad;
                    temp.renglon = subproducto.renglon;
                    temp.ubicacionGeografica = subproducto.ubicacionGeografica;
                    temp.duracion = subproducto.duracion;
                    temp.duracionDimension = subproducto.duracionDimension;
                    temp.fechaInicio = subproducto.fechaInicio.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaFin = subproducto.fechaFin != null ? subproducto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.snip = subproducto.snip;
                    temp.estado = subproducto.estado;
                    temp.usuarioCreo = subproducto.usuarioCreo;
                    temp.usuarioActualizo = subproducto.usuarioActualizo;
                    temp.fechaCreacion = subproducto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = subproducto.fechaActualizacion != null ? subproducto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.latitud = subproducto.latitud;
                    temp.longitud = subproducto.longitud;
                    temp.costo = subproducto.costo ?? default(decimal);

                    subproducto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subproducto.acumulacionCostoid));

                    temp.acumulacionCostoid = Convert.ToInt32(subproducto.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subproducto.acumulacionCostos.nombre;

                    subproducto.subproductoTipos = SubproductoTipoDAO.getSubproductoTipo(subproducto.subproductoTipoid);

                    if (subproducto.subproductoTipos != null)
                    {
                        temp.subproductoTipoid = subproducto.subproductoTipos.id;
                        temp.subProductoTipo = subproducto.subproductoTipos.nombre;
                    }

                    subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora);
                    subproducto.productos = ProductoDAO.getProductoPorId(subproducto.productoid);
                    subproducto.productos.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.productos.ejercicio, subproducto.productos.entidad ?? default(int), subproducto.productos.ueunidadEjecutora);

                    if (subproducto.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subproducto.ueunidadEjecutora;
                        temp.nombreUnidadEjecutora = subproducto.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.entidad ?? default(int);
                        temp.ejercicio = subproducto.ejercicio;

                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio);
                        temp.entidadnombre = subproducto.unidadEjecutoras.entidads.nombre;
                    }
                    else if (subproducto.productos.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subproducto.productos.ueunidadEjecutora;
                        temp.nombreUnidadEjecutora = subproducto.productos.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.productos.entidad ?? default(int);
                        temp.ejercicio = subproducto.productos.ejercicio;
                        subproducto.productos.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.productos.entidad ?? default(int), subproducto.productos.ejercicio);
                        temp.entidadnombre = subproducto.productos.unidadEjecutoras.entidads.nombre;
                    }

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 4);

                    temp.fechaInicioReal = subproducto.fechaInicioReal != null ? subproducto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subproducto.fechaFinReal != null ? subproducto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subproducto.inversionNueva;
                }
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproducto - Crear")]
        public IActionResult Subproducto([FromBody]dynamic value)
        {
            try
            {
                bool resultado = false;
                SubproductoValidator validator = new SubproductoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Subproducto subproducto = new Subproducto();

                    subproducto.nombre = value.nombre;
                    subproducto.descripcion = value.descripcion;

                    subproducto.productoid = (int)value.producto;
                    // todo no venia subproductopadre
                    subproducto.su.subproductoPadreId = (int)value.subproductoPadre;

                    subproducto.subproductoTipoid = (int)value.tiposubproductoid;
                    subproducto.ueunidadEjecutora = value.unidadEjecutora != null ? (int)value.unidadEjecutora : default(int);

                    subproducto.entidad = value.entidad != null ? (int)value.entidad : default(int);
                    subproducto.ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);

                    subproducto.snip = value.snip;
                    subproducto.programa = value.programa;
                    subproducto.subprograma = value.subprograma;
                    subproducto.proyecto = value.proyecto;
                    subproducto.obra = value.obra;
                    subproducto.renglon = value.renglon;
                    subproducto.ubicacionGeografica = value.ubicacionGeografica;
                    subproducto.actividad = value.actividad;
                    subproducto.latitud = value.latitud;
                    subproducto.longitud = value.longitud;
                    subproducto.costo = value.costo;
                    subproducto.acumulacionCostoid = value.acumulacionCostoId;
                    subproducto.fechaInicio = value.fechaInicio;
                    subproducto.fechaFin = Convert.ToDateTime(value.fechaFin);
                    subproducto.duracion = value.duaracion;
                    subproducto.duracionDimension = value.duracionDimension;
                    subproducto.inversionNueva = value.inversionNueva;

                    Producto producto = ProductoDAO.getProductoPorId(subproducto.productoid);

                    //TODO no hay subproductopadre
                    Subproducto subproductoPadre = new Subproducto();
                    subproductoPadre.setId(subproductoPadreId);  // todo esta data no venia

                    SubproductoTipo subproductoTipo = new SubproductoTipo();
                    subproductoTipo.id = subproducto.subproductoTipoid;

                    if ((value.ejercicio != null) && (value.entidad != null) && (value.unidadEjecutora != null))
                    {
                        UnidadEjecutoraDAO.getUnidadEjecutora(value.ejercicio, value.entidad, value.ueunidadEjecutora);
                    }

                    /*UnidadEjecutora unidadEjecutora = (subproducto.ejercicio != null && subproducto.entidad != null && subproducto.ueunidadEjecutora != null) ? UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad, subproducto.ueunidadEjecutora) : null;
                     */
                    resultado = SubproductoDAO.guardarSubproducto(subproducto, false);

                    if (resultado)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!subproducto.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(subproducto.id, 4);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (subproducto.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);

                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);

                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"]) : default(decimal);

                                PagoPlanificado pagoPlanificado = new PagoPlanificado();

                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = subproducto.id;
                                pagoPlanificado.objetoTipo = 4;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.estado = 1;

                                resultado = resultado && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }


                        if (resultado)
                        {
                            JArray datos = JArray.Parse((string)value.datadinamica);

                            for (int i = 0; i < datos.Count; i++)
                            {
                                JObject data = (JObject)datos[i];

                                if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                                {
                                    SubproductoPropiedad producotPropiedad = SubproductoPropiedadDAO.getSubproductoPropiedadPorId(Convert.ToInt32(data["id"]));

                                    SubproductoPropiedadValor valor = new SubproductoPropiedadValor();
                                    valor.subproductoid = subproducto.id;
                                    valor.subproductos = subproducto;
                                    valor.subproductoPropiedads = producotPropiedad;
                                    valor.usuarioCreo = User.Identity.Name;
                                    valor.fechaCreacion = DateTime.Now;
                                    valor.estado = 1;

                                    switch (producotPropiedad.datoTipoid)
                                    {
                                        case 1:
                                            valor.valorString = data["valor"].ToString();
                                            break;
                                        case 2:
                                            valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                            break;
                                        case 3:
                                            valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                            break;
                                        case 4:
                                            break;
                                        case 5:
                                            valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                            break;
                                    }
                                    resultado = (resultado && SubproductoPropiedadValorDAO.guardarSubproductoPropiedadValor(valor));
                                }
                            }
                        }

                        return Ok(new
                        {
                            success = resultado,
                            subproducto.id,
                            subproducto.usuarioCreo,
                            subproducto.fechaCreacion,
                            subproducto.usuarioActualizo,
                            subproducto.fechaActualizacion
                        });
                    }
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subproducto - Editar")]
        public IActionResult Subproducto(int id, [FromBody]dynamic value)
        {
            try
            {
                bool resultado = false;
                SubproductoValidator validator = new SubproductoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Subproducto subproducto = SubproductoDAO.getSubproductoPorId(id);

                    subproducto.nombre = value.nombre;
                    subproducto.descripcion = value.descripcion;
                    subproducto.productoid = (int)value.producto;
                    // todo no venia subproductopadre
                    subproducto.subproductoPadreId = (int)value.subproductoPadre;
                    subproducto.subproductoTipoid = (int)value.tiposubproductoid;
                    subproducto.ueunidadEjecutora = value.unidadEjecutora != null ? (int)value.unidadEjecutora : default(int);

                    subproducto.entidad = value.entidad != null ? (int)value.entidad : default(int);
                    subproducto.ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                    subproducto.snip = value.snip;
                    subproducto.programa = value.programa;
                    subproducto.subprograma = value.subprograma;
                    subproducto.proyecto = value.proyecto;
                    subproducto.obra = value.obra;
                    subproducto.renglon = value.renglon;
                    subproducto.ubicacionGeografica = value.ubicacionGeografica;
                    subproducto.actividad = value.actividad;
                    subproducto.latitud = value.latitud;
                    subproducto.longitud = value.longitud;
                    subproducto.costo = value.costo;
                    subproducto.acumulacionCostoid = value.acumulacionCostoId;
                    subproducto.fechaInicio = value.fechaInicio;
                    subproducto.fechaFin = Convert.ToDateTime(value.fechaFin);
                    subproducto.duracion = value.duaracion;
                    subproducto.duracionDimension = value.duracionDimension;
                    subproducto.inversionNueva = value.inversionNueva;

                    Producto producto = ProductoDAO.getProductoPorId(subproducto.productoid);

                    //TODO no hay subproductopadre
                    Subproducto subproductoPadre = new Subproducto();
                    subproductoPadre.setId(subproductoPadreId);  // todo esta data no venia

                    SubproductoTipo subproductoTipo = new SubproductoTipo();
                    subproductoTipo.id = subproducto.subproductoTipoid;

                    if ((value.ejercicio != null) && (value.entidad != null) && (value.unidadEjecutora != null))
                    {
                        UnidadEjecutoraDAO.getUnidadEjecutora(value.ejercicio, value.entidad, value.ueunidadEjecutora);
                    }

                    /*UnidadEjecutora unidadEjecutora = (subproducto.ejercicio != null && subproducto.entidad != null && subproducto.ueunidadEjecutora != null) ? UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad, subproducto.ueunidadEjecutora) : null;
                     */
                    resultado = SubproductoDAO.guardarSubproducto(subproducto, false);

                    if (resultado)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!subproducto.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(subproducto.id, 4);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (subproducto.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);

                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);

                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"]) : default(decimal);

                                PagoPlanificado pagoPlanificado = new PagoPlanificado();

                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = subproducto.id;
                                pagoPlanificado.objetoTipo = 4;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.estado = 1;

                                resultado = resultado && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }

                        if (resultado)
                        {
                            JArray datos = JArray.Parse((string)value.datadinamica);

                            for (int i = 0; i < datos.Count; i++)
                            {
                                JObject data = (JObject)datos[i];

                                if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                                {
                                    SubproductoPropiedad producotPropiedad = SubproductoPropiedadDAO.getSubproductoPropiedadPorId(Convert.ToInt32(data["id"]));

                                    SubproductoPropiedadValor valor = new SubproductoPropiedadValor();
                                    valor.subproductoid = subproducto.id;
                                    valor.subproductos = subproducto;
                                    valor.subproductoPropiedads = producotPropiedad;
                                    valor.usuarioCreo = User.Identity.Name;
                                    valor.fechaCreacion = DateTime.Now;
                                    valor.estado = 1;

                                    switch (producotPropiedad.datoTipoid)
                                    {
                                        case 1:
                                            valor.valorString = data["valor"].ToString();
                                            break;
                                        case 2:
                                            valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                            break;
                                        case 3:
                                            valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                            break;
                                        case 4:
                                            break;
                                        case 5:
                                            valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                            break;
                                    }
                                    resultado = (resultado && SubproductoPropiedadValorDAO.guardarSubproductoPropiedadValor(valor));
                                }
                            }
                        }

                        return Ok(new
                        {
                            success = resultado,
                            subproducto.id,
                            subproducto.usuarioCreo,
                            subproducto.fechaCreacion,
                            subproducto.usuarioActualizo,
                            subproducto.fechaActualizacion
                        });
                    }
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subproducto - Visualizar")]
        public IActionResult Subproducto(int id)
        {
            try
            {
                Subproducto subproducto = SubproductoDAO.getSubproductoPorId(id);
                bool resultado = ObjetoDAO.borrarHijos(subproducto.treepath, 4, User.Identity.Name);

                return Ok(new { success = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                int? subProductoId = value.subproductoid != null ? (int)value.subproductoid : default(int);
                String filtroBusqueda = value.filtro_busqueda;
                long total = SubproductoDAO.GetTotalSubProductos(subProductoId, filtroBusqueda, User.Identity.Name);

                return Ok(new { success = true, total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerSubproductoPorId(int id)
        {
            try
            {
                // todo aca voy
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SubproductoPorId(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("10", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }
    }
}

