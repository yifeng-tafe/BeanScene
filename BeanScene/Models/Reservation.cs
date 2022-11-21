﻿using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReservationMadeTime { get; set; }

        public string? MemberId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReservationDate { get; set; }

        [Required]
        public int ReservationTimeId { get; set; }
        [Required]
        public ReservationTime ReserveTime { get; set; }

        [Required]
        public int NumberOfGuest { get; set; }

        public string? Requirement { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string? RequestSource { get; set; }

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
