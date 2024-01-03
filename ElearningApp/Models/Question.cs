using System;
using System.Collections.Generic;

namespace ElearningApp.Models
{
    public partial class Question
    {
        public Question()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }

        public string Id { get; set; }
        public string? Question1 { get; set; }
        public string? Answer { get; set; }
        public string? Marks { get; set; }
        public string? Category { get; set; }

        public virtual Category? CategoryNavigation { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
