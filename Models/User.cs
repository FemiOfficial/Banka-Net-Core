using System;

namespace banka_net_core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Type { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Accounts Account { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }




    }
}