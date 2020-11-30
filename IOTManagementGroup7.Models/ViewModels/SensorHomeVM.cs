using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.Models.ViewModels
{
    public class SensorHomeVM
    {
        public IEnumerable<Sensor> Sensors { get; set; }
        public Project Project { get; set; }
    }
}
