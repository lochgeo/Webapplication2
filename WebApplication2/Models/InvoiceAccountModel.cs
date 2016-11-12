using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountRegistry.Models
{
    public class InvoiceAccount
    {
        public int InvoiceAccountId { get; set; }

        [Required]
        [Display(Name = "Account #")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}