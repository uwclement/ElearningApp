using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Report
{
    public class AdminReportModel : PageModel
    {
        public User User = new User();
        public Account account = new Account();
        public List<User> Userlist = new List<User>();
        public String fullname, email;
        public int tusers;
        public String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public void OnGet()
        {
            get_marks();
            if (HttpContext.Session.GetString("fullname") == null || HttpContext.Session.GetString("email") == null)
            {
                Response.Redirect("/Login/Login");

            }
            else
            {
                fullname = HttpContext.Session.GetString("fullname");
                email = HttpContext.Session.GetString("email");
            }


        }
        public void get_marks()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "SELECT fullnames,total_score FROM users ORDER BY total_score DESC";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User User = new User();
                                User.Fullnames = reader.GetString(0);
                                User.TotalScore = "" + reader.GetInt32(1);
                                Userlist.Add(User);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception:" + ex.Message);

            }
        }
        public void get_users()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "SELECT count(role) FROM account where role='user'";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();
                                account.Role = "" + reader.GetInt32(0);
                                tusers = Int32.Parse(account.Role);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception:" + ex.Message);

            }
        }
        public IActionResult OnPostLogout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToPage("/Login/Login");
        }
    }
}
