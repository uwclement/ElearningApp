using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
    public class CategoryEditModel : PageModel
    {
        public String fullname, email;
        public Category categoryinfo = new Category();
        public String errorMessage = "";
        public String successMessage = "";
        public string userEmail;
        public string userName;
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
            //userEmail = HttpContext.Session.GetString("email");
            //userName = HttpContext.Session.GetString("fullname");
            String id = Request.Query["id"];
            try
            {
                String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "select * from category where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                categoryinfo.Id = "" + reader.GetInt32(0);
                                categoryinfo.CategoryName = reader.GetString(1);
                                



                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            categoryinfo.Id = Request.Form["id"];
            categoryinfo.CategoryName = Request.Form["CategoryName"];

            if (categoryinfo.CategoryName.Length == 0 )
            {
                errorMessage = "All Fields are Required";
                return;
            }
            try
            {
                String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "UPDATE category SET category_name = @category_name WHERE id=@id;";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryinfo.Id);
                        cmd.Parameters.AddWithValue("@category_name", categoryinfo.CategoryName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            successMessage = "Category Upadted";
            Response.Redirect("/Exercises/CategoryView");
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
