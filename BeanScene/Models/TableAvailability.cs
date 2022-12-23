using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class TableAvailability
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }  

        public virtual Reservation Reservation { get; set; }

        [Required]
        public int TableId { get; set; }    

        public virtual Table Table { get; set; }

        public StatusEnum Availability { get; set; }

        public enum StatusEnum
        {
            Available,
            Reserved
        }

    }
}
