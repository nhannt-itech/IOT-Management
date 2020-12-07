using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public string CustomerUserId { get; set; }
        [ForeignKey("CustomerUserId")]
        public ApplicationUser CustomerUser { get; set; }

        [NotMapped]


    }
}
