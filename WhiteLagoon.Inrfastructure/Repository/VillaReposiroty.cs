﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;

namespace WhiteLagoon.Inrfastructure.Repository
{
    public class VillaReposiroty:Repository<Villa>, IVillaRepository

    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly ApplicationDbContext _db;

        // Defining the constructor for the VillaController class
        public VillaReposiroty(ApplicationDbContext db) :base(db)
        {
            _db = db; // Assigning the passed in db value to the _db field
        }
        public void Update(Villa entity)
        {
            _db.Update(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
