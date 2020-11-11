using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Television
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool PowerStatus { get; set; }
        public string CurrentChannel { get; set; }
        public bool RecordingStatus { get; set; }
        [Range(0, 100)]
        public int Volume { get; set; }
        public string SourceCode { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
