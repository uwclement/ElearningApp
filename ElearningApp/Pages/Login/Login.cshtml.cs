using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace ElearningApp.Pages.Login
{
    public class LoginModel : PageModel
    {
        Hashing hc = new Hashing();
        public String fullname, email;
        String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public string message = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginModel([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Account account = new Account();
        public void onGet()
        {


        }

        public void OnPost()
        {
            String emails = Request.Form["email"];
            String passwords = hc.PassHash( Request.Form["password"]);
            String haspass;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string query = "SELECT fullnames,email,Upassword,role FROM account WHERE email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", emails);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the email exists in the database
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    fullname = reader.GetString(0);
                                    email = reader.GetString(1);
                                    string Password = reader.GetString(2);
                                    string role = reader.GetString(3);
                                    
                                    if (passwords.Equals(Password))
                                    {
                                        if (role.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string Email = emails;
                                            _httpContextAccessor.HttpContext.Session.SetString("email", Email);
                                            _httpContextAccessor.HttpContext.Session.SetString("fullname", fullname);
                                            Response.Redirect("/Exercises/CategoryView");
                                        }
                                        else if (role.Trim().Equals("user", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string Email = emails;
                                            _httpContextAccessor.HttpContext.Session.SetString("email", Email);
                                            _httpContextAccessor.HttpContext.Session.SetString("fullname", fullname);

                                            Response.Redirect("/Report/Report");
                                        }

                                    }
                                    else
                                    {
                                        message = "Invalid Password";
                                    }
                                }
                            }
                            else
                            {
                                // Email does not exist in the database
                                message = "Email not found. Please check your credentials.";
                            }
                        }




                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

        }
    }
}
