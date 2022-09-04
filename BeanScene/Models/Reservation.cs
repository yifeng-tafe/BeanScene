namespace BeanScene.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int TableId { get; set; }
        public int UserId { get; set; }

        public DateTime? ResearveDate { get; set; }   
        public DateTime? ConfirmDate { get; set; }   
        
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Requested,
            Approve,
            Rejected,
            Seated,
            Ordered,
            Served,
            Paid,
        }
    }
}
