using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;

namespace WhiteLagoon.Inrfastructure.Repository
{
    public class VillaReposiroty: IVillaRepository

    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly ApplicationDbContext _db;

        // Defining the constructor for the VillaController class
        public VillaReposiroty(ApplicationDbContext db)
        {
            _db = db; // Assigning the passed in db value to the _db field
        }
        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> Get(Expression<Func<Villa, bool>>? filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Add(Villa entity)
        {
            _db.Add(entity);
        }

        public void Update(Villa entity)
        {
            _db.Update(entity);
        }

        public void Remove(Villa entity)
        {
            _db.Remove(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
