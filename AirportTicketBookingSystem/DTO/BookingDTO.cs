using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.DTO
{
    public record BookingDTO(string BookingId, PassengerDTO Passenger, FlightDTO Flight, string BookingClass, DateTime BookingDate, decimal BookingPrice)
    {    }

}
