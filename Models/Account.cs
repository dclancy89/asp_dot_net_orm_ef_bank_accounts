using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Account : BaseEntity 
    {
        public int Id { get; set; }
        public int AccountBalance { get; set; }

        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Record> Records { get; set; }
 
        public Account()
        {
            Records = new List<Record>();
        }
    }

}
