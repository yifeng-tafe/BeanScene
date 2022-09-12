using BeanScene.Models;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.ViewModels
{
    public class ReservationViewModel
    {
        
        //Table details to be displayed on Reservation Index view
        public int Id { get; set; }
        public int TableId { set; get; }
        public string Name { get; set; }
        public int AreaID { get; set; }
        public Area Areas { get; set; }

        //Table reservation details
        [DataType(DataType.Date)]
        public DateTime? ReserveDate { get; set; }
        public string ReserveType { get; set; }
        public string ReserveTime { get; set; }
        public string ReserveArea { get; set; }
        public int NumberOfGuest { get; set; }
        public string Requirement { get; set; }
        public string Status { get; set; }

        //User details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }
    }
}
