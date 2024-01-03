using System;
using System.Collections.Generic;

namespace ElearningApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Questions = new HashSet<Question>();
        }

        public string Id { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
