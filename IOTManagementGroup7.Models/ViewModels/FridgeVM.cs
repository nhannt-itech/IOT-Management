using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;


namespace IOTManagementGroup7.Models.ViewModels
{
    public class FridgeVM
    {
        public Fridge Fridge { get; set; }
        public IEnumerable<SelectListItem> ApplicationUserList { get; set; }
    }
}
