using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;


namespace IOTManagementGroup7.Models.ViewModels
{
    public class TelevisionVM
    {
        public Television Television { get; set; }
        public IEnumerable<SelectListItem> ApplicationUserList { get; set; }
    }
}
