using System;

namespace WPF_LoginForm.Model
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public bool IsActive { get; set; } 
        public string Role { get; set; } 
    }
}
