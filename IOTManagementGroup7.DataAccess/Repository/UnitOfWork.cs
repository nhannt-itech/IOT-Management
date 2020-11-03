﻿using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
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
            Light = new LightRepository(_db);
            SP_Call = new SP_Call(_db);
        }


        public ILightRepository Light { get; private set; }
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