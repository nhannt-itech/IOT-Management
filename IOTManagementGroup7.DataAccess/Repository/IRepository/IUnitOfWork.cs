using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ILightRepository Light { get; }

        ICameraRepository Camera { get; }
        ISP_Call SP_Call { get; }

        ITVRepository TV { get; }
        IFridgeRepository Fridge { get; }
        void Save();
    }
}
