using AirportTicketBookingSystem.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Model
{
    public class Booking
    {
        [Required(ErrorMessage = "Booking ID is required.")]
        public string? BookingId { get; set; }

        [Required(ErrorMessage = "Passenger is required.")]
        public Passenger? Passenger { get; set; }

        [Required(ErrorMessage = "Fligh is required.")]
        public Flight? Flight { get; set; }
        [Required(ErrorMessage = "Booking class is required.")]

        private BookingClass bookingClass;
        [Required(ErrorMessage = "Booking class is required.")]
        public BookingClass BookingClass
        {
            get => bookingClass;
            set
            {
                if (!Enum.IsDefined(typeof(BookingClass), value))
                {
                    throw new ArgumentException("Invalid booking class.");
                }
                bookingClass = value;
            }
        }

        [Required(ErrorMessage = "Date time is required.")]
        public DateTime BookingDate { get; set; }

        public decimal Price => GetPriceForBookingClass();



        private decimal GetPriceForBookingClass()
        {
            if (Flight == null)
                throw new InvalidOperationException("Flight information is required to calculate the price.");

            return bookingClass switch
            {
                BookingClass.Economy => Flight.EconomyPrice,
                BookingClass.Business => Flight.BusinessPrice,
                BookingClass.FirstClass => Flight.FirstClassPrice,
                _ => throw new ArgumentException("Invalid booking class.")
            };
        }


    }
}
