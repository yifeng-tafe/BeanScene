using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }
        public int TableId { get; set; }   
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReserveDate { get; set; }

        [Required]
        public int ReservationTypeId { get; set; }
        [Required]
        public ReservationType ReserveType { get; set; }

        [Required]
        public int NumberOfGuest { get; set; }

        public string Requirement { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Requested,
            Comfirmed,
            Rejected,
            Seated,
            Ordered,
            Paid,
            Completed
        }







  

    }
}
