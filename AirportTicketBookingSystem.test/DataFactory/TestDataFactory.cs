
using AirportTicketBookingSystem.Model;
using AutoFixture;

namespace AirportTicketBookingSystem.test.DataFactory
{
    public class TestDataFactory : ITestDataFactory
    {
        private readonly IFixture fixture;

        public TestDataFactory()
        {
            fixture = new Fixture();

        }
        public Booking CreateBookingData(Passenger? passenger = null, Flight? flight = null, string? bookingId = null, BookingClass? bookingClass = null, DateTime? bookingDate = null)
        {

            Booking booking = fixture.Build<Booking>()
                .With(b => b.BookingId, bookingId?? fixture.Create<string>())
                .With(b => b.Passenger, passenger?? fixture.Create<Passenger>())
                .With(b => b.Flight, flight ?? fixture.Create<Flight>())
                .With(b => b.BookingClass, bookingClass ?? fixture.Create<BookingClass>())
                .With(b => b.BookingDate, bookingDate ?? fixture.Create<DateTime>())
                .Create();

            return booking;
        }

        public Flight CreateFlightData(string? flightNumber = null, string? departureCountry = null, string? destinationCountry = null, DateTime? departureDate = null, string? departureAirport = null, string? arrivalAirport = null, decimal? economyPrice = null, decimal? businessPrice = null, decimal? firstClassPrice = null)
        {
            return fixture.Build<Flight>()
                .With(f => f.FlightNumber, flightNumber ?? fixture.Create<string>().Substring(0, 2) + fixture.Create<int>().ToString("D3"))
                .With(f => f.DepartureCountry, departureCountry ?? fixture.Create<string>().Substring(0, 20))
                .With(f => f.DestinationCountry, destinationCountry ?? fixture.Create<string>().Substring(0, 20))
                .With(f => f.DepartureDate, departureDate ?? fixture.Create<DateTime>())
                .With(f => f.DepartureAirport, departureAirport ?? fixture.Create<string>().Substring(0, 20))
                .With(f => f.ArrivalAirport, arrivalAirport ?? fixture.Create<string>().Substring(0, 20))
                .With(f => f.EconomyPrice, economyPrice ?? fixture.Create<decimal>())
                .With(f => f.BusinessPrice, businessPrice ?? fixture.Create<decimal>())
                .With(f => f.FirstClassPrice, firstClassPrice ?? fixture.Create<decimal>())
                .Create();
        }

        public Passenger CreatePassengerData(int? id = null, string? name = null, string? email = null)
        {
            return fixture.Build<Passenger>()
                .With(p => p.Id, id ?? fixture.Create<int>())
                .With(p => p.Name, name ?? fixture.Create<string>().Substring(0, 20))
                .With(p => p.Email, email ?? fixture.Create<string>().Substring(0, 20))
                .Create();
        }
    }
}
