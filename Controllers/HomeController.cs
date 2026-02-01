using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string FirstName, string LastName, string EmailAddress)
        {
            
            if(string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(EmailAddress))
            {
                
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Newsletter;Integrated Security=True;";
                string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress) VALUES
                                    (@FirstName, @LastName, @EmailAddress)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = FirstName;
                    command.Parameters["@LastName"].Value = LastName;
                    command.Parameters["@EmailAddress"].Value = EmailAddress;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                return View("Success");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}