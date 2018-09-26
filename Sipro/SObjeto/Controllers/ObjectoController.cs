using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SObjeto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ObjectoController : Controller
    {
        [HttpGet("{objetoId}/{objetoTipo}")]
        [Authorize("Actividades - Visualizar")]
        public IActionResult ObjetoPorId(int objetoId, int objetoTipo)
        {
            try
            {
                String nombre = "";
                String tiponombre = "";
                String fechaInicio = "";
                switch (objetoTipo)
                {
                    case 0: //Proyecto;
                        tiponombre = "Proyecto";
                        Proyecto proyecto = ProyectoDAO.getProyectoPorId(objetoId, User.Identity.Name);
                        nombre = (proyecto != null) ? proyecto.nombre : "";
                        fechaInicio = Utils.getFechaHoraNull(proyecto.fechaInicio);
                        break;
                    case 1: //Componente;
                        tiponombre = "Componente";
                        Componente componente = ComponenteDAO.getComponentePorId(objetoId, User.Identity.Name);
                        nombre = (componente != null) ? componente.nombre : "";
                        fechaInicio = Utils.getFechaHoraNull(componente.fechaInicio);
                        break;
                    case 2: //Subcomponente;
                        tiponombre = "Subcomponente";
                        Subcomponente subcomponente = SubComponenteDAO.getSubComponentePorId(objetoId, User.Identity.Name);
                        nombre = (subcomponente != null) ? subcomponente.nombre: "";
                        fechaInicio = Utils.getFechaHoraNull(subcomponente.fechaInicio);
                        break;
                    case 3: //Producto
                        tiponombre = "Producto";
                        Producto producto = ProductoDAO.getProductoPorId(objetoId, User.Identity.Name);
                        nombre = (producto != null) ? producto.nombre : "";
                        fechaInicio = Utils.getFechaHoraNull(producto.fechaInicio);
                        break;
                    case 4: //Subproducto
                        tiponombre = "Subproducto";
                        Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId, User.Identity.Name);
                        nombre = (subproducto != null) ? subproducto.nombre : "";
                        fechaInicio = Utils.getFechaHoraNull(subproducto.fechaInicio);
                        break;
                    case 5: //Actividad
                        tiponombre = "Actividad";
                        Actividad actividad = ActividadDAO.GetActividadPorId(objetoId);
                        nombre = (actividad != null) ? actividad.nombre : "";
                        fechaInicio = Utils.getFechaHoraNull(actividad.fechaInicio);
                        break;
                }
                
                return Ok(new { success = true, nombre = nombre, tiponombre = tiponombre, fechaInicio = fechaInicio});
            }
            catch (Exception e)
            {
                CLogger.write("1", "ObjectoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
