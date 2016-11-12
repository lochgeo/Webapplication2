using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountRegistry.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Required]
        [Display(Name ="Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "CVR Number")]
        public string CVRNumber { get; set; }

        [Required]
        [Display(Name = "Ethereum Address")]
        public string Address { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }
    }
}