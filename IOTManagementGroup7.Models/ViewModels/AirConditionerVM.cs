using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;




namespace IOTManagementGroup7.Models.ViewModels
{
    public class AirConditionerVM
    {
        public AirConditioner AirConditioner { get; set; }
        public IEnumerable<SelectListItem> ApplicationUserList { get; set; }
    }
}