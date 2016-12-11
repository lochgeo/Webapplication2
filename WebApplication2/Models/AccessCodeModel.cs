using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AccountRegistry.Models
{
    public class AccessCode
    {
        public int AccessCodeId { get; set; }

        [Required]
        [Display(Name = "Buyer Email")]
        public string BuyerEmail { get; set; }

        [Required]
        [Display(Name = "Access Code")]
        public string UniqueCode { get; set; }

        [Required]
        [Display(Name = "Confirmed")]
        public string Confirmed { get; set; }

        public virtual InvoiceAccount Account { get; set; }

        [Required]
        public int InvoiceAccountId { get; set; }

    }
}