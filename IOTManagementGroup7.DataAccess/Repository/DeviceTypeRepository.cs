using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class DeviceTypeRepository : Repository<DeviceType>, IDeviceTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public DeviceTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DeviceType deviceType)
        {
            _db.Update(deviceType);
        }
    }
}
