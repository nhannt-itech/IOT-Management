using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTManagementGroup7.Models
{
    public class Device
    {
        [Key]
        public string Id { get; set; } //Dv
        [Required]
        public string Name { get; set; }
        public bool PowerButton { get; set; }
        [Range(0, 1)]
        public int PowerStatus { get; set; } //return API
        public bool SliderButton { get; set; } 
        public int SliderMaxRange { get; set; }
        public int SliderRange { get; set; } //return API

        [Required]
        public string SensorBoardId { get; set; }
        [ForeignKey("SensorBoardId")]
        public Sensor Sensor { get; set; }

        [Required]
        public string DeviceTypeId { get; set; }
        [ForeignKey("DeviceTypeId")]
        public DeviceType DeviceType { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> DeviceTypeList { get; set; }
    }
}
