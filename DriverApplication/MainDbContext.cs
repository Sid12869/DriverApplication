using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DriverApplication.Models;

namespace DriverApplication
{
    public class MainDbContext : DbContext
    {
        public MainDbContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}