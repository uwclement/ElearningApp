using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
    public class CategoryAddModel : PageModel
    {
        public String fullname, email;
        public Category categoryinfo = new Category();
        public String errorMessage = "";
        public String successMessage = "";

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

        }
        public void OnPost()
        {
            categoryinfo.CategoryName = Request.Form["CategoryName"];
            if (categoryinfo.CategoryName.Length == 0 )

            {
                errorMessage = "All Fields are Required";
                return;
            }
            //save data
            String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "insert into category(category_name) values(@category_name)";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
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
            categoryinfo.CategoryName = "";


            successMessage = "Category Added";
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
