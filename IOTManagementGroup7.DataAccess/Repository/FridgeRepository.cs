﻿using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOTManagementGroup7.DataAccess.Repository
{
    public class FridgeRepository : Repository<Fridge>, IFridgeRepository
    {
        private readonly ApplicationDbContext _db;
        public FridgeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Fridge fridge)
        {
            _db.Update(fridge);
        }
    }
}