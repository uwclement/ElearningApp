using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace ElearningApp.Pages.assessment
{


    public class assessmentMarksModel : PageModel
    {
        public User user = new User();
        public Account account = new Account();
        public UserAnswer userAnswer = new UserAnswer();
        public Question question = new Question();
        public List<User> list = new List<User>();
        public List<(UserAnswer, Question)> Userlist = new List<(UserAnswer, Question)>();

        public String fullname, email;
        public string Message = "";
        public int total;
        public String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public void OnGet()
        {
            get_marks();
            total_marks();


        }
        public void get_marks()
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
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "GetUserData ";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@users", fullname);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserAnswer userAnswer = new UserAnswer();
                                Question question = new Question();
                                question.Question1 = reader.GetString(6);
                                userAnswer.Status = reader.GetString(3);
                                userAnswer.Marks = "" + reader.GetInt32(4);
                                Userlist.Add((userAnswer, question));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception:" + ex.Message);
                Message = ex.Message;

            }

        }
        public IActionResult OnPostLogout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToPage("/Login/Login");
        }
        public void total_marks()
        {
            get_marks();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    String sqlquery = "SELECT fullnames,total_score FROM users where fullnames=@fullnames";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        cmd.Parameters.AddWithValue("@fullnames", fullname);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User User = new User();
                                //User.Fullnames = reader.GetString(0);
                                User.TotalScore = "" + reader.GetInt32(1);
                                list.Add(User);
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
    }
}
