using System;
using System.Collections.Generic;

namespace ElearningApp.Models
{
    public partial class UserAnswer
    {
        public string Id { get; set; }
        public string? Users { get; set; }
        public string? Question { get; set; }
        public string? Status { get; set; }
        public string? Marks { get; set; }

        public virtual Question? QuestionNavigation { get; set; }
    }
}
