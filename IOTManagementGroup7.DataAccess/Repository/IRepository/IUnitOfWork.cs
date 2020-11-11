using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ILightRepository Light { get; }
        ITelevisionRepository Television { get; }
        IWashingMachineRepository WashingMachine { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }
        void Save();
    }
}
