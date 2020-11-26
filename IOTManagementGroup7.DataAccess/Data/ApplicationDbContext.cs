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

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<DeviceType> DeviceType { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
    }
}
