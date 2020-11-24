using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Light
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Name { get; set; }
        public bool PowerStatus { get; set; }
        public bool ConnectionStatus { get; set; }
        [Required]
        public string VoltageRange { get; set; }
        [Required]
        [Range(0,100)]
        public int Dim { get; set; }
        [Required]
        public string SourceCode { get; set; }
    }
}
