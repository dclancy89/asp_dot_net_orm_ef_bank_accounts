using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class AccountActivityViewModel : BaseEntity 
    {

        [Required]
        [Display(Name="Deposit / Withdraw")]
        public int Amount { get; set; }


    }

}
