using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {


        ICameraRepository Camera { get; }
        ISP_Call SP_Call { get; }
        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
