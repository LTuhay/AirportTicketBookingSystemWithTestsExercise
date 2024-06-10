using AirportTicketBookingSystem.Model;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.test.DataFactory
{
    public interface ITestDataFactory
    {

        Passenger CreatePassengerData(int? id = null, string name = null,string email = null);
        Flight CreateFlightData(string flightNumber = null,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        decimal? economyPrice = null,
        decimal? businessPrice = null,
        decimal? firstClassPrice = null);
        Booking CreateBookingData(Passenger? passenger = null, Flight? flight = null, string? bookingId = null, BookingClass? bookingClass = null, DateTime? bookingDate = null);

    }
}
