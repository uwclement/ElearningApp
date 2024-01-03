using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
    public class ExerciseEditModel : PageModel
    {
		public String fullname, email;
		public Question questioninfo = new Question();
		public List<Category> categorylist = new List<Category>();
		public List<Question> questionlist = new List<Question>();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
			category_retrieve();
			if (HttpContext.Session.GetString("fullname") == null || HttpContext.Session.GetString("email") == null)
			{
				Response.Redirect("/Login/Login");

			}
			else
			{
				fullname = HttpContext.Session.GetString("fullname");
				email = HttpContext.Session.GetString("email");
			}
			String id = Request.Query["id"];
			String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
			using (SqlConnection con = new SqlConnection(conString))
			{
				con.Open();
				String sqlquery = "select * from question where id=@id";
				using (SqlCommand cmd = new SqlCommand(sqlquery, con))
				{
					cmd.Parameters.AddWithValue("@id", id);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							
							questioninfo.Id = "" + reader.GetInt32(0);
							questioninfo.Question1 = reader.GetString(1);
							questioninfo.Answer = reader.GetString(2);
							questioninfo.Marks = ""+reader.GetInt32(3);
							questioninfo.Category = ""+reader.GetInt32(4);

							
						}
					}
				}


			}
		}
		public void OnPost()
		{
			questioninfo.Id = Request.Form["id"];
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
					String sqlquery = "UPDATE question SET question = @question, answer = @answer, marks = @marks, category = @category WHERE id = @id";

					using (SqlCommand cmd = new SqlCommand(sqlquery, con))
					{
						cmd.Parameters.AddWithValue("@question", questioninfo.Question1);
						cmd.Parameters.AddWithValue("@answer", questioninfo.Answer);
						cmd.Parameters.AddWithValue("@marks", questioninfo.Marks);
						cmd.Parameters.AddWithValue("@category", questioninfo.Category);
						cmd.Parameters.AddWithValue("@id", questioninfo.Id);

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

		public void category_retrieve()
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
	}
}
