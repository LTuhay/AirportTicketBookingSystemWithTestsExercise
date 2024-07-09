using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.DTO
{
    public record FlightDTO(string FlightNumber, string DepartureCountry, string DestinationCountry, DateTime DepartureDate, string DepartureAirport, string ArrivalAirport, decimal EconomyPrice, decimal BusinessPrice, decimal FirstClassPrice)
    {
        }
   

}
