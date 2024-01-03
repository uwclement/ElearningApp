using System;
using System.Collections.Generic;

namespace ElearningApp.Models
{
    public partial class Account
    {
        public string Id { get; set; }
        public string? Fullnames { get; set; }
        public string? Email { get; set; }
        public string? Upassword { get; set; }
        public string? Role { get; set; }
    }
}
