using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Login
{
    public class SignupModel : PageModel
    {
        Hashing hc = new Hashing();
        String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public string message = "";
        public void OnGet()
        {
        }
        public Account account = new Account();
        public void OnPost()
        {

            account.Fullnames = Request.Form["fullName"];
            account.Email = Request.Form["email"];
            string password = Request.Form["password"];
            //string roles = Request.Form["role"];
            if (account.Email.Length == 0)
            {
                message = "Provide All Info";
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("create_accounts", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FullName", account.Fullnames);
                        cmd.Parameters.AddWithValue("Email", account.Email);          
                        cmd.Parameters.AddWithValue("@password", hc.PassHash(password));
                        //cmd.Parameters.AddWithValue("@Role", roles);
                        int rowAffected = cmd.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            message = "Account Created";

                        }
                        else
                        {
                            message = "account not Created";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY "))
                {
                    message = "There's a problem: account already exists";

                }
                else
                {
                    message = "There's a problem: " + ex.Message;

                }

            }
            Account pat = new Account();
            password = "";

        }
    }
}
