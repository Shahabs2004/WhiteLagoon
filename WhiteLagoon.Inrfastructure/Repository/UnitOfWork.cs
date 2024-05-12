using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IVillaRepository   Villa   { get; private set; }
        public IAmenityRepository Amenity { get; }

        public IBookingRepository         Booking         { get; private set; }

        public IApplicationUserRepository User { get; private set; }

        public IVillaNumberRepository VillaNumber { get; }
        public void Save()
        {
            _db.SaveChanges();
        }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaReposiroty(_db);
            Amenity = new AmenityRepository(_db);
            VillaNumber = new VillaNumberReposiroty(_db);
            Booking = new BookingRepository(_db);
            User = new ApplicationUserRepository(_db);

        }
    }
}
