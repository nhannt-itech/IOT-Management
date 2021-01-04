using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Sensor
    {
        [Key]
        public string Id { get; set; } // Ss
        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        public string Name { get; set; }
        public int MaxDevice { get; set; }
        public string SourceCode { get; set; }
        [Required]
        public string ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [NotMapped]
        public IEnumerable<Device> Devices;
    }
}
