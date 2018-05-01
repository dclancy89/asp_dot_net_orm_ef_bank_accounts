using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Record : BaseEntity 
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

}
