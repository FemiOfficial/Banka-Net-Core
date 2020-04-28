using System;

namespace banka_net_core.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public float AccountBalance { get; set; }
        public float AccountOpeningBalance { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }



    }
}