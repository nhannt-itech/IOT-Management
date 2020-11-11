using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
    }
}