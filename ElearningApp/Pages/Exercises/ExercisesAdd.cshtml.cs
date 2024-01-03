using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
    public class ExercisesAddModel : PageModel
    {
		public String fullname, email;
		public Question questioninfo = new Question();
		public List<Category> categorylist = new List<Category>();
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

							// Add categoryinfo to the list
							categorylist.Add(categoryinfo);
						}
					}
				}
			}

		}
		public void OnPost()
		{
			questioninfo.Question1 = Request.Form["question"];
			questioninfo.Answer = Request.Form["answer"];
			questioninfo.Marks = Request.Form["marks"];
			questioninfo.Category = Request.Form["category"];
			if (questioninfo.Question1.Length == 0)

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
					String sqlquery = "insert into question(question,answer,marks,category) values(@question,@answer,@marks,@category)";
					using (SqlCommand cmd = new SqlCommand(sqlquery, con))
					{
						cmd.Parameters.AddWithValue("@question", questioninfo.Question1);
						cmd.Parameters.AddWithValue("@answer", questioninfo.Answer);
						cmd.Parameters.AddWithValue("@marks", questioninfo.Marks);
						cmd.Parameters.AddWithValue("@category", questioninfo.Category);

						cmd.ExecuteNonQuery();
					}
				}

			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			questioninfo.Question1 = "";
			questioninfo.Answer = "";
			questioninfo.Marks = "";
			questioninfo.Category = "";


			successMessage = "Category Added";
			Response.Redirect("/Exercises/ExerciseView");
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
