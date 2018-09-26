using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SMatrizRACI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class MatrizRaciController : Controller
    {
        private class Stmatriz
        {
            public int objetoId;
            public String objetoNombre;
            public String nombreR;
            public int idR;
            public String nombreA;
            public int idA;
            public String nombreC;
            public int idC;
            public String nombreI;
            public int idI;
            public int objetoTipo;
            public int nivel;
        }

        private class Stcolaborador
        {
            public int id;
            public String nombre;
        }

        private class Stasignacion
        {
            public int colaboradorId;
            public String colaboradorNombre;
            public String rolId;
            public String rolNombre;
        }

        private class Stinformacion
        {
            public String nombreTarea;
            public String estadoTarea;
            public String rol;
            public String nombreColaborador;
            public String estadoColaborador;
            public String fechaInicio;
            public String fechaFin;
            public String email;
        }

        [HttpPost]
        [Authorize("Asignación Raci - Visualizar")]
        public IActionResult Matriz([FromBody]dynamic value)
        {
            try
            {
                List<Stmatriz> lstMatriz = new List<Stmatriz>();
                int idPrestamo = value.idPrestamo != null ? (int)value.idPrestamo : 0;
                String lineaBase = value.lineaBase;

                List<EstructuraProyecto> estructuraProyecto = EstructuraProyectoDAO.getEstructuraProyecto(idPrestamo, lineaBase);
                foreach (EstructuraProyecto objeto in estructuraProyecto)
                {
                    Stmatriz tempmatriz = new Stmatriz();
                    tempmatriz.objetoId = objeto.id;
                    tempmatriz.objetoNombre = objeto.nombre;
                    tempmatriz.nivel = objeto.treePath.Length / 8;
                    tempmatriz.objetoTipo = objeto.objeto_tipo;

                    List<AsignacionRaci> asignaciones = AsignacionRaciDAO.getAsignacionesRaci(tempmatriz.objetoId, tempmatriz.objetoTipo, lineaBase);
                    if (asignaciones.Count > 0)
                    {
                        foreach (AsignacionRaci asignacion in asignaciones)
                        {
                            asignacion.colaboradors = ColaboradorDAO.getColaborador(asignacion.colaboradorid);
                            if (asignacion.rolRaci.Equals("r"))
                            {
                                tempmatriz.nombreR = asignacion.colaboradors.pnombre + " " + asignacion.colaboradors.papellido;
                                tempmatriz.idR = asignacion.colaboradorid;
                            }
                            else if (asignacion.rolRaci.Equals("a"))
                            {
                                tempmatriz.nombreA = asignacion.colaboradors.pnombre + " " + asignacion.colaboradors.papellido;
                                tempmatriz.idA = asignacion.colaboradorid;
                            }
                            else if (asignacion.rolRaci.Equals("c"))
                            {
                                tempmatriz.nombreC = asignacion.colaboradors.pnombre + " " + asignacion.colaboradors.papellido;
                                tempmatriz.idC = asignacion.colaboradorid;
                            }
                            else if (asignacion.rolRaci.Equals("i"))
                            {
                                tempmatriz.nombreI = asignacion.colaboradors.pnombre + " " + asignacion.colaboradors.papellido;
                                tempmatriz.idI = asignacion.colaboradorid;
                            }
                        }
                    }

                    lstMatriz.Add(tempmatriz);
                }

                bool sinColaborador = false;
                List<Stcolaborador> lstcolaboradores = new List<Stcolaborador>();
                Proyecto proyecto = ProyectoDAO.getProyectoPorId(idPrestamo, User.Identity.Name);

                if (proyecto != null)
                {
                    List<Colaborador> colaboradores = AsignacionRaciDAO.getColaboradoresPorProyecto(idPrestamo, null);
                    foreach (Colaborador colaborador in colaboradores)
                    {
                        Stcolaborador temp = new Stcolaborador();
                        temp.id = colaborador.id;
                        temp.nombre = colaborador.pnombre + " " + colaborador.papellido;
                        lstcolaboradores.Add(temp);
                    }
                }

                if (lstcolaboradores.Count == 0)
                {
                    sinColaborador = true;
                    Stcolaborador temp = new Stcolaborador();
                    temp.id = 1;
                    temp.nombre = "R";
                    lstcolaboradores.Add(temp);
                    temp = new Stcolaborador();
                    temp.id = 1;
                    temp.nombre = "A";
                    lstcolaboradores.Add(temp);
                    temp = new Stcolaborador();
                    temp.id = 1;
                    temp.nombre = "C";
                    lstcolaboradores.Add(temp);
                    temp = new Stcolaborador();
                    temp.id = 1;
                    temp.nombre = "I";
                    lstcolaboradores.Add(temp);
                }

                return Ok(new
                {
                    success = lstMatriz != null && lstcolaboradores != null ? true : false,
                    matriz = lstMatriz, sinColaboradores = sinColaborador ? true : false,
                    colaboradores = lstcolaboradores
                });
            }
            catch (Exception e)
            {
                CLogger.write("1", "MatrizRaciController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Asignación Raci - Visualizar")]
        public IActionResult InformacionTarea([FromBody]dynamic value)
        {
            try
            {
                int objetoId = value.objetoId != null ? (int)value.objetoId : 0;
                int objetoTipo = value.objetoTipo != null ? (int)value.objetoTipo : 0;
                String lineaBase = value.lineaBase;
                String rol = value.rol;

                Stinformacion informacion = new Stinformacion();
                switch (objetoTipo)
                {
                    case 0:
                        Proyecto proyecto = ProyectoDAO.getProyectoPorId(objetoId, User.Identity.Name);
                        informacion.nombreTarea = proyecto.nombre;
                        break;
                    case 1:
                        Componente componente = ComponenteDAO.getComponentePorId(objetoId, User.Identity.Name);
                        informacion.nombreTarea = componente.nombre;
                        break;
                    case 2:
                        Subcomponente subcomponente = SubComponenteDAO.getSubComponentePorId(objetoId, User.Identity.Name);
                        informacion.nombreTarea = subcomponente.nombre;
                        break;
                    case 3:
                        Producto producto = ProductoDAO.getProductoPorId(objetoId, User.Identity.Name);
                        informacion.nombreTarea = producto.nombre;
                        break;
                    case 4:
                        Subproducto subproducto = SubproductoDAO.getSubproductoPorId(objetoId, User.Identity.Name);
                        informacion.nombreTarea = subproducto.nombre;
                        break;
                    case 5:
                        Actividad actividad = ActividadDAO.GetActividadPorId(objetoId);
                        informacion.nombreTarea = actividad.nombre;
                        break;
                }

                AsignacionRaci asignacion = AsignacionRaciDAO.getAsignacionPorRolTarea(objetoId, objetoTipo, rol, lineaBase);
                asignacion.colaboradors = ColaboradorDAO.getColaborador(Convert.ToInt32(asignacion.id));
                asignacion.colaboradors.usuarios = UsuarioDAO.getUsuario(User.Identity.Name);

                if (rol.ToLower().Equals("R"))
                {
                    informacion.rol = "Responsable";
                }
                else if (rol.ToLower().Equals("a"))
                {
                    informacion.rol = "Cuentadante";
                }
                else if (rol.ToLower().Equals("c"))
                {
                    informacion.rol = "Consultor";
                }
                else if (rol.ToLower().Equals("i"))
                {
                    informacion.rol = "Quien informa";
                }

                informacion.nombreColaborador = String.Join(" ", asignacion.colaboradors.pnombre,
                        asignacion.colaboradors.snombre != null ? asignacion.colaboradors.snombre : "",
                        asignacion.colaboradors.papellido,
                        asignacion.colaboradors.sapellido != null ? asignacion.colaboradors.sapellido : "");

                informacion.estadoColaborador = asignacion.colaboradors.estado == 1 ? "Alta" : "Baja";
                informacion.email = asignacion.colaboradors.usuarios != null ? asignacion.colaboradors.usuarios.email : "";

                return Ok(new { success = true, informacion = informacion });
            }
            catch (Exception e)
            {
                CLogger.write("2", "MatrizRaciController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{objetoId}/{objetoTipo}")]
        [Authorize("Asignación Raci - Visualizar")]
        public IActionResult AsignacionPorObjeto(int objetoId, int objetoTipo)
        {
            try
            {
                List<AsignacionRaci> asignaciones = AsignacionRaciDAO.getAsignacionesRaci(objetoId, objetoTipo, null);
                List<Stasignacion> asignacionesRet = new List<Stasignacion>();
                foreach (AsignacionRaci asignacion in asignaciones)
                {
                    Stasignacion temp = new Stasignacion();
                    asignacion.colaboradors = ColaboradorDAO.getColaborador(asignacion.colaboradorid);
                    temp.colaboradorId = asignacion.colaboradors.id;
                    temp.colaboradorNombre = asignacion.colaboradors.pnombre + " "
                            + asignacion.colaboradors.papellido;
                    temp.rolId = asignacion.rolRaci;
                    asignacionesRet.Add(temp);
                }
                return Ok(new { success = true, asignaciones = asignacionesRet });
            }
            catch (Exception e)
            {
                CLogger.write("3", "MatrizRaciController.class", e);
                return BadRequest(500);
            }
        }
    }
}
