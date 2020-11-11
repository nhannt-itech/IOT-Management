using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class WashingMachine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool PowerStatus { get; set; }
        public string ProgramStatus { get; set; }
        [Range(0,15)]
        public double Weight { get; set; }
        public int RemainingTime { get; set; }
        public string SourceCode { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
