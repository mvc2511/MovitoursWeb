using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovitoursWeb.View_Models
{
    public class Registro
    {
        public int IdCliente { get; set; }  
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }

        public string ConfirmarContrasena { get; set; }
    }
}