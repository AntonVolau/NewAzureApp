using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewAzureApp.Models.SitesPages
{
    public class Customer
    {
        public virtual ICollection<Sale> Sales { get; set; }
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }
        [Key]
        [ScaffoldColumn(false)]
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(30, ErrorMessage = "Name length must be less than 30 characters long")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Surname")]
        [StringLength(30, ErrorMessage = "Surname length must be less than 30 characters long")]
        public string Surname { get; set; }
    }
}