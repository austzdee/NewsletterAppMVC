using NewsletterAppMVC.Models;
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
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Newsletter;Integrated Security=True;";
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
               
                string queryString = @"INSERT INTO dbo.SignUps (FirstName, LastName, EmailAddress) VALUES
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

        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress FROM dbo.SignUps";
            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var signup = new NewsletterSignUp();
                        signup.Id = (int)reader["Id"];
                        signup.FirstName = reader["FirstName"].ToString();
                        signup.LastName = reader["LastName"].ToString();
                        signup.EmailAddress = reader["EmailAddress"].ToString();
                        signups.Add(signup);
                    }
                }
                catch (Exception ex)
                {
                    return Content("SQL ERROR: " + ex.Message);
                }
            }

            return View(signups);
        }
    }
}