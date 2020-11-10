using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class TV
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Channel { get; set; }
        [Range(0, 100)]
        public int Volume { get; set; }
    }
}
