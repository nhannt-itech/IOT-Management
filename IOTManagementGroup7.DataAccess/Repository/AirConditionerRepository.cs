using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class AirConditionerRepository : Repository<AirConditioner>, IAirConditionerRepository
    {
        private readonly ApplicationDbContext _db;
        public AirConditionerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(AirConditioner airConditioner)
        {
            _db.Update(airConditioner);
        }
    }
}
