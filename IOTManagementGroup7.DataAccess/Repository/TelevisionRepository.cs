using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class TelevisionRepository : Repository<Television>, ITelevisionRepository
    {
        private readonly ApplicationDbContext _db;
        public TelevisionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Television television)
        {
            _db.Update(television);
        }
    }
}
