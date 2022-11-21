using BeanScene.Models;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.ViewModels
{
    public class TableAvailabilityViewModel
    {
        public IEnumerable<Reservation> Reservations { get; set; }

        public IEnumerable<Table> Tables { get; set; }

        public IEnumerable<TableAvailability.StatusEnum> StatusEnums { get; set; }
        public TableAvailability TableAvailability { get; set; }

    }
}
