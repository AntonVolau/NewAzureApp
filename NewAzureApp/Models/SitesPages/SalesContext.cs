using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewAzureApp.Models.SitesPages
{
    public class SalesContext : DbContext
    {
        public SalesContext() : base("DefaultConnection")
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}