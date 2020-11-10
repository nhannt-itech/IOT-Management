using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Fridge
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        [Range(0, 10)]
        public double Temperature { get; set; }
    }
}
