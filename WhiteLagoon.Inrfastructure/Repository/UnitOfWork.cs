using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Inrfastructure.Data;

namespace WhiteLagoon.Inrfastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IVillaRepository Villa { get; private set; }
        public IVillaNumberRepository VillaNumber { get; }
        public void Save()
        {
            _db.SaveChanges();
        }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaReposiroty(_db);
            VillaNumber = new VillaNumberReposiroty(_db);

        }
    }
}
