using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Camera
    {
        [Key]
        public string Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public bool PowerStatus { get; set; }
        public bool ConnectionStatus { get; set; }
        public string NightVersionStatus { get; set; }
        public bool TimelapsRecordingStatus { get; set; }
        public string SourceCode { get; set; }
    }
}
