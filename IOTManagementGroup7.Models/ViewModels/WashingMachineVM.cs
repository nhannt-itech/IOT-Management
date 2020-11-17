using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;


namespace IOTManagementGroup7.Models.ViewModels
{
    public class WashingMachineVM
    {
        public WashingMachine WashingMachine { get; set; }
        public IEnumerable<SelectListItem> ApplicationUserList { get; set; }
    }
}
