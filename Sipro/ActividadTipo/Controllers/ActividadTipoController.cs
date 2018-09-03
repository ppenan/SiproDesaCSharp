using Microsoft.AspNetCore.Mvc;
using System;

namespace SActividadTipo.Controllers
{
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


    }

}
