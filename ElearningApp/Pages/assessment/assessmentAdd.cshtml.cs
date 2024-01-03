using ElearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace ElearningApp.Pages.assessment
{
    public class assessmentAddModel : PageModel
    {
        public String fullname, email;
        public Question questioninfo = new Question();
        public UserAnswer userAnswer = new UserAnswer();
        public List<Question> questionlist = new List<Question>();
        public List<Category> categorylist = new List<Category>();
        public Category categoryinfo = new Category();
        public String Message = "";
        public String AnswerDB, AnswerForm, questionDB, marksDB;
        public String idString = "0";
        public void OnGet()
        {

            get_data();
        }
        public void OnPost()
        {
            get_data();
            userAnswer.Status = Request.Form["status"];
            userAnswer.Marks = Request.Form["marks"];
            questioninfo.Answer = Request.Form["answer"];
            AnswerForm = questioninfo.Answer;
            if (questioninfo.Answer.Length == 0)

            {
                Message = "All Fields are Required";
                return;
            }
            //save data
            String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    if (string.Equals(AnswerForm, AnswerDB, StringComparison.OrdinalIgnoreCase))
                    {
                        con.Open();
                        String sqlquery = "user_status";
                        using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user", fullname);
                            cmd.Parameters.AddWithValue("@question", questionDB);
                            cmd.Parameters.AddWithValue("@status", "correct");
                            cmd.Parameters.AddWithValue("@marks", marksDB);

                            cmd.ExecuteNonQuery();
                            Message = "Answer Saved";
                        }
                    }
                    else if (!string.Equals(AnswerForm, AnswerDB, StringComparison.OrdinalIgnoreCase))
                    {
                        con.Open();
                        String sqlquery = "user_status";
                        using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@user", fullname);
                            cmd.Parameters.AddWithValue("@question", questionDB);
                            cmd.Parameters.AddWithValue("@status", "Incorrect");
                            cmd.Parameters.AddWithValue("@marks", 0);

                            cmd.ExecuteNonQuery();
                            Message = "Answer Saved";
                        }

                    }


                    

                }
            }
            
            catch (Exception ex)
            {
                Message = ex.Message;
                return;
            }
            questioninfo.Question1 = "";
            questioninfo.Answer = "";
            questioninfo.Marks = "";
            questioninfo.Category = "";


            Message = "Answer Saved";
            //Response.Redirect("/Exercises/ExerciseView");
        }
        public IActionResult OnPostLogout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToPage("/Login/Login");
        }

        public void get_data()
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
            idString = Request.Query["id"];
            String conString = @"Data Source=CLEMENT\SQLEXPRESS;Initial Catalog=Elearning;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                String sqlquery = "select * from question where id=@id";
                using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                {
                    cmd.Parameters.AddWithValue("@id", idString);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            Question questioninfo = new Question();
                            questioninfo.Id = "" + reader.GetInt32(0);
                            questioninfo.Question1 = reader.GetString(1);
                            questioninfo.Answer = reader.GetString(2);
                            questioninfo.Marks = ""+reader.GetInt32(3);
                            questioninfo.Category = ""+reader.GetInt32(4);
                            questionDB = questioninfo.Id;
                            AnswerDB = questioninfo.Answer;
                            marksDB = questioninfo.Marks;
                            // Add categoryinfo to the list
                            questionlist.Add(questioninfo);
                        }
                    }
                }
            }
        }
    }
}
