﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IDeviceRepository Device { get; }
        IDeviceTypeRepository DeviceType { get; }
        IProjectRepository Project { get; }
        ISensorRepository Sensor { get; }
        ISP_Call SP_Call { get; }
        void Save();
    }
}
