using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovitoursWeb.Models
{
    public class Login
    {
        public int IdCliente { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}