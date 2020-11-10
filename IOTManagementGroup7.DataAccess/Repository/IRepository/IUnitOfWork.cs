using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ILightRepository Light { get; }
        ISP_Call SP_Call { get; }
        ITVRepository TV { get; }
        IFridgeRepository Fridge { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
