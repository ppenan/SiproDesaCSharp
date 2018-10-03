using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubproducto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
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
            public int? congelado;
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
            public Int64 acumulacionCostoid;
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
        [Authorize("Subproductos - Visualizar")]
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
                    //temp.fechaInicio = subproducto.fechaInicio.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaInicio = subproducto.fechaInicio != null ? subproducto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
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

                    //subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora);
                    subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio ?? default(int), subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora ?? default(int));
                    subproducto.productos = ProductoDAO.getProductoPorId(subproducto.productoid);
                    subproducto.productos.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.productos.ejercicio, subproducto.productos.entidad ?? default(int), subproducto.productos.ueunidadEjecutora);

                    if (subproducto.unidadEjecutoras != null)
                    {
                        //temp.ueunidadEjecutora = subproducto.ueunidadEjecutora;
                        temp.ueunidadEjecutora = subproducto.ueunidadEjecutora ?? default(int);
                        temp.nombreUnidadEjecutora = subproducto.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.entidad ?? default(int);
                        //temp.ejercicio = subproducto.ejercicio;
                        temp.ejercicio = subproducto.ejercicio ?? default(int);

                        //subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio);
                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio ?? default(int));
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
        [Authorize("Subproductos - Crear")]
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
                    ///subproducto.su.subproductoPadreId = (int)value.subproductoPadre; no tiene subproductopadre
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
                    //Se agregan estas lineas
                    subproducto.fechaCreacion = DateTime.Now;
                    subproducto.usuarioCreo = User.Identity.Name;
                    subproducto.estado = 1;
                    // Se agregan estas lineas
                    DateTime fechaInicioReal;
                    DateTime.TryParse((string)value.fechaInicioReal, out fechaInicioReal);
                    subproducto.fechaInicioReal = fechaInicioReal;

                    DateTime fechaFinReal;
                    DateTime.TryParse((string)value.fechaFinReal, out fechaFinReal);
                    subproducto.fechaFinReal = fechaFinReal;

                    // Se comenta Producto producto = ProductoDAO.getProductoPorId(subproducto.productoid);

                    // Se comenta Subproducto subproductoPadre = new Subproducto();
                    ///subproductoPadre.setId(subproductoPadreId);  // esta data no viene
                    // Se comenta SubproductoTipo subproductoTipo = new SubproductoTipo();
                    // Se comenta subproductoTipo.id = subproducto.subproductoTipoid;

                    // Se comenta if ((value.ejercicio != null) && (value.entidad != null) && (value.unidadEjecutora != null))
                    // Se comenta {
                    // Se comenta    UnidadEjecutoraDAO.getUnidadEjecutora(value.ejercicio, value.entidad, value.ueunidadEjecutora);
                    // Se comenta }

                    /*UnidadEjecutora unidadEjecutora = (subproducto.ejercicio != null && subproducto.entidad != null && subproducto.ueunidadEjecutora != null) ? UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad, subproducto.ueunidadEjecutora) : null;
                     */
                    //resultado = SubproductoDAO.guardarSubproducto(subproducto, false);
                    resultado = SubproductoDAO.guardarSubproducto(subproducto, true);

                    if (resultado)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!subproducto.acumulacionCostoid.Equals(4) || pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(subproducto.id, 4);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (subproducto.acumulacionCostoid.Equals(4) && pagosPlanificados != null && pagosPlanificados.Replace("[", "").Replace("]", "").Length > 0)
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
                                // se agrega linea
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                resultado = resultado && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (resultado)
                    {
                        // Se agrega este bloque probarlo
                        List<SubproductoPropiedad> subproductoPropiedades = SubproductoPropiedadDAO.getSubproductoPropiedadesPorTipo(subproducto.subproductoTipoid);

                        foreach (SubproductoPropiedad subProductoPropiedad in subproductoPropiedades)
                        {
                            SubcomponentePropiedadValor subProdPropVal = SubcomponentePropiedadValorDAO.getValorPorSubComponenteYPropiedad(subProductoPropiedad.id, subproducto.id);
                            if (subProdPropVal != null)
                                resultado = resultado && SubcomponentePropiedadValorDAO.eliminarTotalSubComponentePropiedadValor(subProdPropVal);
                        } 
                         // Hasta aqui

                        JArray datos = JArray.Parse((string)value.datadinamica);
                        for (int i = 0; i < datos.Count; i++)
                        {
                            JObject data = (JObject)datos[i];
                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                SubproductoPropiedad subProductoPropiedad = SubproductoPropiedadDAO.getSubproductoPropiedadPorId(Convert.ToInt32(data["id"]));
                                SubproductoPropiedadValor valor = new SubproductoPropiedadValor();
                                valor.subproductoid = subproducto.id;
                                valor.subproductos = subproducto;
                                valor.subproductoPropiedads = subProductoPropiedad;
                                //
                                valor.subproductoPropiedadid = subProductoPropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;
                                valor.estado = 1;

                                switch (subProductoPropiedad.datoTipoid)
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
                                        valor.valorEntero = data["valor"].ToString() == "true" ? 1 : 0;
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
                        /*success = resultado,
                        subproducto.id,
                        subproducto.usuarioCreo,
                        subproducto.fechaCreacion,
                        subproducto.usuarioActualizo,
                        subproducto.fechaActualizacion*/
                        success = resultado,
                        id = subproducto.id,
                        usuarioCreo = subproducto.usuarioCreo,
                        usuarioActualizo = subproducto.usuarioActualizo,
                        fechaCreacion = subproducto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subproducto.fechaActualizacion != null ? subproducto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    {
                        return Ok(new { success = false });
                    }
                //borrar }
                // borrar else
                // borrar {
                // borrar    return Ok(new { success = false });
                // borrar}
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subproductos - Editar")]
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
                    /// subproducto.subproductoPadreId = (int)value.subproductoPadre; // no hay nada de subproductopadreid
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

                    Subproducto subproductoPadre = new Subproducto();
                    /// subproductoPadre.setId(subproductoPadreId);  // todo esta data no venia

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
                CLogger.write("3", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subproductos - Visualizar")]
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
                String filtroBusqueda = value.filtro_busqueda;
                int productoId = value.productoid != null ? (int)value.productoid : default(int);
                //int? productoId = value.productoid != null ? (int)value.productoid : default(int);
                long total = SubproductoDAO.GetTotalSubProductos(productoId, filtroBusqueda, User.Identity.Name);

                return Ok(new { success = true, totalsubproductos = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult ObtenerSubproductoPorId(int id)
        {
            try
            {
                Subproducto subproducto = SubproductoDAO.getSubproductoPorId(id);

                return Ok(new
                {
                    success = true,
                    subproducto.id,
                    fechaInicio = Utils.ConvierteAFormatoFecha(subproducto.fechaInicio),
                    fechaFin = Utils.ConvierteAFormatoFecha(subproducto.fechaFin),
                    subproducto.duracion,
                    subproducto.duracionDimension,
                    subproducto.nombre
                });
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult SubproductoPorId(int id)
        {
            try
            {
                Subproducto subproducto = SubproductoDAO.getSubproductoPorId(id);

                if (subproducto != null)
                {
                    Stsubproducto temp = new Stsubproducto();
                    temp.id = subproducto.id;
                    temp.nombre = subproducto.nombre;
                    temp.descripcion = subproducto.descripcion;
                    temp.usuarioCreo = subproducto.usuarioCreo;
                    temp.usuarioActualizo = subproducto.usuarioActualizo;

                    temp.fechaCreacion = Utils.ConvierteAFormatoFecha(subproducto.fechaCreacion);
                    temp.fechaActualizacion = Utils.ConvierteAFormatoFecha(subproducto.fechaActualizacion);

                    temp.estado = subproducto.estado;
                    temp.snip = subproducto.snip;
                    temp.programa = subproducto.programa;
                    temp.subprograma = subproducto.subprograma;
                    temp.proyecto = subproducto.proyecto;
                    temp.actividad = subproducto.actividad;
                    temp.obra = subproducto.obra;
                    temp.renglon = subproducto.renglon;
                    temp.ubicacionGeografica = subproducto.ubicacionGeografica;
                    temp.latitud = subproducto.latitud;
                    temp.longitud = subproducto.longitud;
                    temp.fechaInicio = Utils.ConvierteAFormatoFecha(subproducto.fechaInicio);
                    temp.fechaFin = Utils.ConvierteAFormatoFecha(subproducto.fechaFin);
                    temp.duracion = subproducto.duracion;
                    temp.duracionDimension = subproducto.duracionDimension;
                    temp.costo = subproducto.costo ?? default(decimal);

                    subproducto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(subproducto.acumulacionCostoid);
                    temp.acumulacionCostoid = subproducto.acumulacionCostoid;
                    temp.acumulacionCostoNombre = subproducto.acumulacionCostos.nombre;
                    temp.productoid = subproducto.productoid;

                    //subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora);
                    subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio ?? default(int), subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora ?? default(int));
                    if (subproducto.unidadEjecutoras != null)
                    {
                        //subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio);
                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio ?? default(int));
                        temp.entidadnombre = subproducto.unidadEjecutoras.entidads.nombre;
                        temp.nombreUnidadEjecutora = subproducto.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.unidadEjecutoras.entidadentidad;
                    }

                    //temp.ejercicio = subproducto.ejercicio;
                    temp.ejercicio = subproducto.ejercicio ?? default(int);

                    subproducto.subproductoTipos = SubproductoTipoDAO.getSubproductoTipo(subproducto.subproductoTipoid);

                    if (subproducto.subproductoTipos != null)
                    {
                        temp.subproductoTipoid = subproducto.subproductoTipoid;
                        temp.subProductoTipo = subproducto.subproductoTipos.nombre;
                    }

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 4);
                    temp.fechaInicioReal = Utils.ConvierteAFormatoFecha(subproducto.fechaInicioReal);
                    temp.fechaFinReal = Utils.ConvierteAFormatoFecha(subproducto.fechaFinReal);

                    subproducto.productos = ProductoDAO.getProductoPorId(subproducto.productoid);
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(subproducto.productos.treepath);

                    temp.congelado = proyecto.congelado != null ? proyecto.congelado : 0;
                    temp.fechaElegibilidad = Utils.ConvierteAFormatoFecha(proyecto.fechaElegibilidad);
                    temp.fechaCierre = Utils.ConvierteAFormatoFecha(proyecto.fechaCierre);

                    temp.inversionNueva = subproducto.inversionNueva;

                    return Ok(new { success = true, subproducto = temp });
                }
                else
                {
                    return Ok(new { success = false });
                }

            }
            catch (Exception e)
            {
                CLogger.write("7", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                String versiones = SubproductoDAO.GetVersiones(id);
                return Ok(new { success = true, versiones });
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                String historia = SubproductoDAO.GetHistoria(id, version);

                return Ok(new { success = true, historia });
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                DateTime cal = new DateTime();
                int ejercicio = cal.Year;
                int id = value.id;

                Subproducto objSubproducto = SubproductoDAO.getSubproductoPorId(id);
                Proyecto objProyecto = ProyectoDAO.getProyectobyTreePath(objSubproducto.treepath);
                int entidad = objProyecto.entidad ?? default(int);
                int programa = value.programa;
                int subprograma = value.subprograma;
                int proyecto = value.proyecto;
                int actividad = value.actividad;
                int obra = value.obra;
                int renglon = value.renglon;
                int geografico = value.geografico;
                decimal asignado = ObjetoDAO.getAsignadoPorLineaPresupuestaria(ejercicio, entidad, programa, subprograma, proyecto, actividad, obra, renglon, geografico);

                decimal planificado = decimal.Zero;
                switch (objSubproducto.acumulacionCostoid)
                {
                    case 1:
                        //cal = objSubproducto.fechaInicio;
                        cal = objSubproducto.fechaInicio ?? default(DateTime);
                        int ejercicioInicial = cal.Year;
                        if (ejercicio.Equals(ejercicioInicial))
                        {
                            planificado = objSubproducto.costo ?? default(decimal);
                        }
                        break;
                    case 2:
                        List<PagoPlanificado> lstPagos = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objSubproducto.id, 4);
                        foreach (PagoPlanificado pago in lstPagos)
                        {
                            cal = pago.fechaPago;
                            int ejercicioPago = cal.Year;
                            if (ejercicio.Equals(ejercicioPago))
                            {
                                planificado += pago.pago;
                            }
                        }
                        break;
                    case 3:
                        cal = objSubproducto.fechaFin ?? default(DateTime);
                        int ejercicioFinal = cal.Year;
                        if (ejercicio.Equals(ejercicioFinal))
                        {
                            planificado += objSubproducto.costo ?? default(decimal);
                        }
                        break;
                }

                bool sobrepaso = false;
                asignado = asignado - planificado;

                if (asignado.CompareTo(decimal.Zero) == -1)
                {
                    sobrepaso = true;
                }

                return Ok(new { success = true, asignado, sobrepaso });
            }
            catch (Exception e)
            {
                CLogger.write("10", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        //Visualiza los subproductos del Producto
        [HttpPost]
        [Authorize("Subproductos - Visualizar")]
        public IActionResult SubProductosPaginaPorProducto([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int productoId = value.productoid != null ? (int)value.productoid : default(int);
                int numeroSubProductos = value.numerosubproductos != null ? (int)value.numerosubproductos : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<Subproducto> subproductos = SubproductoDAO.getSubProductosPaginaPorProducto(pagina, numeroSubProductos,
                        productoId, filtro_busqueda, columna_ordenada, orden_direccion, User.Identity.Name);
                List<Stsubproducto> stsubproductos = new List<Stsubproducto>();
                foreach (Subproducto subproducto in subproductos)
                {
                    Stsubproducto temp = new Stsubproducto();
                    temp.descripcion = subproducto.descripcion;
                    temp.estado = subproducto.estado;
                    temp.fechaActualizacion = subproducto.fechaActualizacion != null ? subproducto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subproducto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.id = subproducto.id;
                    temp.nombre = subproducto.nombre;
                    temp.usuarioActualizo = subproducto.usuarioActualizo;
                    temp.usuarioCreo = subproducto.usuarioCreo;

                    subproducto.subproductoTipos = SubproductoTipoDAO.getSubProductoTipoPorId(subproducto.subproductoTipoid);
                    temp.subproductoTipoid = subproducto.subproductoTipoid;
                    temp.subProductoTipo = subproducto.subproductoTipos.nombre;

                    temp.snip = subproducto.snip;
                    temp.programa = subproducto.programa;
                    temp.subprograma = subproducto.subprograma;
                    temp.proyecto = subproducto.proyecto;
                    temp.actividad = subproducto.actividad;
                    temp.renglon = subproducto.renglon;
                    temp.ubicacionGeografica = subproducto.ubicacionGeografica;
                    temp.duracion = subproducto.duracion;
                    temp.duracionDimension = subproducto.duracionDimension;
                    temp.fechaInicio = subproducto.fechaInicio != null ? subproducto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = subproducto.fechaFin != null ? subproducto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.obra = subproducto.obra;

                    subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio ?? default(int), subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora ?? default(int));
                    if (subproducto.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subproducto.ueunidadEjecutora ?? default(int);
                        temp.ejercicio = subproducto.ejercicio ?? default(int);

                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio ?? default(int));
                        temp.entidadentidad = subproducto.entidad ?? default(int);
                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio ?? default(int));
                        temp.nombreUnidadEjecutora = subproducto.unidadEjecutoras.nombre;
                        temp.entidadnombre = subproducto.unidadEjecutoras.entidads != null ? subproducto.unidadEjecutoras.entidads.nombre : "SIN ENTIDAD";
                    }
                    else
                    {
                        Producto producto = ProductoDAO.getProducto(subproducto.productoid);
                        producto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.ejercicio, producto.entidad ?? default(int), producto.ueunidadEjecutora);
                        if (producto.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = producto.ueunidadEjecutora;
                            temp.ejercicio = producto.ejercicio;

                            producto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.entidad ?? default(int), producto.ejercicio);
                            temp.entidadentidad = producto.entidad ?? default(int);
                            subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.entidad ?? default(int), producto.ejercicio);
                            temp.nombreUnidadEjecutora = producto.unidadEjecutoras.nombre;
                            temp.entidadnombre = subproducto.unidadEjecutoras.entidads != null ? subproducto.unidadEjecutoras.entidads.nombre : "SIN ENTIDAD";
                        }
                    }

                    temp.latitud = subproducto.latitud;
                    temp.longitud = subproducto.longitud;
                    temp.costo = subproducto.costo ?? default(decimal);

                    subproducto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subproducto.acumulacionCostoid));
                    temp.acumulacionCostoid = Convert.ToInt32(subproducto.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subproducto.acumulacionCostos.nombre;

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 2);
                    temp.fechaInicioReal = subproducto.fechaInicioReal != null ? subproducto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subproducto.fechaFinReal != null ? subproducto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subproducto.inversionNueva;

                    stsubproductos.Add(temp);
                }

                return Ok(new { success = true, subproductos = stsubproductos });
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubproductoController.class", e);
                return BadRequest(500);
            }
        } /* */


    }
}

