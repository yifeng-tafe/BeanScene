using BeanScene.Models;

namespace BeanScene.ViewModels
{
    public class ReservationTimeViewModel
    {
        public IEnumerable<ReservationType> ReservationTypes { get; set; }
        public ReservationTime ReservationTime { get; set; }
    }
}
