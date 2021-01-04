using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class DeviceType
    {
        [Key]
        public string Id { get; set; } //DvT
        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        public string Name { get; set; }
        public string OffImage { get; set; }
        public string OnImage { get; set; }
    }
}
