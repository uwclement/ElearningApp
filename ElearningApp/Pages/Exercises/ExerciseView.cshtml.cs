using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ElearningApp.Pages.Exercises
{
	public class ExerciseViewModel : PageModel
	{
		public String fullname, email;
		public Question questioninfo = new Question();
		public List<Question> questionlist = new List<Question>();
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
			questionlist.Clear();
			try
			{
				String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection con = new SqlConnection(conString))
				{
					con.Open();
					String sqlquery = "select * from Question";
					using (SqlCommand cmd = new SqlCommand(sqlquery, con))
					{
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								Question questioninfo = new Question();
								questioninfo.Id = "" + reader.GetInt32(0);
								questioninfo.Question1 = reader.GetString(1);
								questioninfo.Answer = reader.GetString(2);
								questioninfo.Marks = "" + reader.GetInt32(3);
								questioninfo.Category = "" + reader.GetInt32(4);

								questionlist.Add(questioninfo);
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
	}
}