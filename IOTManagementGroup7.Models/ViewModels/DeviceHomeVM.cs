using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.Models.ViewModels
{
    public class DeviceHomeVM
    {
        public IEnumerable<Device> Devices { get; set; }
        public Sensor Sensor { get; set; }
    }
}
