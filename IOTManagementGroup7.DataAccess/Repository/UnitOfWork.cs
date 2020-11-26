using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Device = new DeviceRepository(_db);
            DeviceType = new DeviceTypeRepository(_db);
            Project = new ProjectRepository(_db);
            Sensor = new SensorRepository(_db);
            SP_Call = new SP_Call(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IDeviceRepository Device { get; private set; }
        public IDeviceTypeRepository DeviceType { get; private set; }
        public IProjectRepository Project { get; private set; }
        public ISensorRepository Sensor { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
