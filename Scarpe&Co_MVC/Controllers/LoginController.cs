using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Scarpe_Co_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private readonly string connectionString;
        public LoginController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ScarpeAndCoDB"].ConnectionString;

        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryVisualizzaUtenti = new SqlCommand("SELECT COUNT(*) FROM Utenti WHERE UserName = @Username AND Password=@Password", conn);
                queryVisualizzaUtenti.Parameters.AddWithValue("@UserName", username);
                queryVisualizzaUtenti.Parameters.AddWithValue("@Password", password);
                int count = (int)queryVisualizzaUtenti.ExecuteScalar();

                if (count > 0)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                        
                        return RedirectToAction("AdminIndex", "Home");
                    
                    
                }
                else
                {
                    ViewBag.ErrorMessage = "Username o password non validi";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore durante l'autenticazione";
                return View();
            }
            finally
            {
                conn.Close();
            }

        }
    }
}