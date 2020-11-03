using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class LightRepository : Repository<Light>, ILightRepository
    {
        private readonly ApplicationDbContext _db;
        public LightRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Light light)
        {
            _db.Update(light);
        }
    }
}
