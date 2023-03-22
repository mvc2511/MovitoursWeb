using MovitoursWeb.Models;
using MovitoursWeb.View_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MovitoursWeb.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=MARIANO;Initial Catalog=BDMovitours;Integrated Security=true";

        // GET: Acceso
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            login.Contrasena = ConvertirSha256(login.Contrasena);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", login.Correo);
                cmd.Parameters.AddWithValue("Contrasena", login.Contrasena);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                login.IdCliente = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (login.IdCliente != 0)
            {
                Session["usuario"] = login;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }            
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(Registro registro)
        {
            bool registrado;
            string mensaje;

            if (registro.Contrasena == registro.ConfirmarContrasena)
            {
                registro.Contrasena = ConvertirSha256(registro.Contrasena);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseña no coinciden";
                return View();
            } 
            
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Nombre", registro.Nombre);
                cmd.Parameters.AddWithValue("Apellido", registro.Apellido);
                cmd.Parameters.AddWithValue("Correo", registro.Correo);
                cmd.Parameters.AddWithValue("Contrasena", registro.Contrasena);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            ViewData["Mensaje"] = mensaje;
            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }
        }

        public string ConvertirSha256(string texto)
        {
            //using System.Text
            //usar la referencia de "System.Security.Cryptography"
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                foreach (byte b in result) 
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}