using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class ReservationTime
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ReservationTypeId { get; set; }

        [Required]
        public virtual ReservationType ReservationTypes { get; set; }
    }
}
