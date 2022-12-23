using BeanScene.Models;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.ViewModels
{
    public class ReservationViewModel
    {
        public IEnumerable<ReservationTime>? ReservationTimes { get; set; }
        
        public IEnumerable<ReservationType>? ReservationTypes { get; set; }
        [Required]
        public Reservation Reservation { get; set; }

        public TableAvailability? TableAvailability { get; set; }
        
        public Table? Table { get; set; }
        

        public IEnumerable<Table>? TablesAssigned { get; set; }
        
        public IEnumerable<Table>? TablesAvailable { get; set; }
        
        public int? TotalAvailableTables { get; set; }

    }
}
