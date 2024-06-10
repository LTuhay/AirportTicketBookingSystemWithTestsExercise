using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Repository
{
    public interface IFlightRepository
    {
        void BatchFlightUpload(string filePath);

        void BatchFlightUpload();
        Flight? GetFlightByNumber(string flightNumber);

        List<Flight> GetFlightByParams (decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass);
        List<Flight> GetAllFlights();

    }

}
