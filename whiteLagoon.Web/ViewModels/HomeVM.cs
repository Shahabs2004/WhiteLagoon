using WhiteLagoon.Domain.Entities;

namespace whiteLagoon.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? villaList { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }

        public int Nights { get; set; }
    }
}
