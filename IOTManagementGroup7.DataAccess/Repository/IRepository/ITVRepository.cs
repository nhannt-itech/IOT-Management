using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface ITVRepository :IRepository<TV>
    {
        void Update(TV tV);
    }
}
