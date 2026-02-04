using NewsletterAppMVC.Models;
using NewsletterAppMVC.Models.ViewModels;
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
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, SocialSecurityNumber FROM dbo.SignUps";

            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();
            List<SignUpVm> signupVms = new List<SignUpVm>();   // <-- moved outside

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var signup = new NewsletterSignUp
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            SocialSecurityNumber = reader["SocialSecurityNumber"].ToString()
                        };

                        signups.Add(signup);

                        signupVms.Add(new SignUpVm
                        {
                            FirstName = signup.FirstName,
                            LastName = signup.LastName,
                            EmailAddress = signup.EmailAddress
                            // Add SocialSecurityNumber here if needed
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Content("SQL ERROR: " + ex.Message);
                }
            }

            return View(signupVms);
        }
    }
}