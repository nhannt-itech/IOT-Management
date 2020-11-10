using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Using package Identity.Stored
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
