using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.SqlClient;

namespace ElearningApp.Pages
{
    public class IndexModel : PageModel
    {
        public String fullname, email;
        public string Message = "";
        public Category categoryinfo = new Category();
        public List<Category> categorylist = new List<Category>();
        private readonly ILogger<IndexModel> _logger;

        public Question questioninfo = new Question();
        public List<Question> questionlist = new List<Question>();
        public String errorMessage = "";
        public String successMessage = "";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            exercises();
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
        public void exercises()
        {
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