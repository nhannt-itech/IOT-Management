using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Light> Lights { get; set; }
        public DbSet<Television> Television { get; set; }
        public DbSet<WashingMachine> WashingMachine { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Camera> Cameras { get; set; }


    }
}
