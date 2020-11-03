using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Light
    {
        [Key]
        public int Id { get; set; }
        public string PositionName { get; set; }
        public bool Status { get; set; }
        public string PowerConsumption { get; set; }
        public string VoltageRange { get; set; }

        [Range(0,100)]
        public int Dim { get; set; }
        public string Type { get; set; } //Vị trí trong hay ngoài
    }
}
