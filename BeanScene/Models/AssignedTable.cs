using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class AssignedTable
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReservationMadeTime { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public int TableId { get; set; }

        [Required]
        public Table Table { get; set; }
    }
}
