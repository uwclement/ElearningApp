using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
    public class CategoryViewModel : PageModel
    {
        public String fullname, email;
        public Category categoryinfo = new Category();
        public List<Category> categorylist = new List<Category>();
        public void OnGet()
        {
            if (HttpContext.Session.GetString("fullname") == null || HttpContext.Session.GetString("email") == null)
            {
                Response.Redirect("/Login/Login");

            }
            else
            {
                fullname = HttpContext.Session.GetString("fullname");
                email = HttpContext.Session.GetString("email");
            }
            categorylist.Clear();
            try
            {
                String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "select * from category";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Category categoryinfo = new Category();
                                categoryinfo.Id = "" + reader.GetInt32(0);
                                categoryinfo.CategoryName = reader.GetString(1);
 
                              categorylist.Add(categoryinfo);
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
