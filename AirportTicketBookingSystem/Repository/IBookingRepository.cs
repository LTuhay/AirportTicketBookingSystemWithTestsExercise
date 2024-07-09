using AirportTicketBookingSystem.Model;

namespace AirportTicketBookingSystem.Repository
{
    public interface IBookingRepository
    {
        List<Booking> GetBookingsByPassenger(int passengerId);

        Booking? GetBookingByID(string id);
        List<Booking> GetAllBookings();
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(string bookingId);

        List<Booking> GetBookingByParams(decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass);

        List<Booking> GetBookingByFlightNumber(string FlightNumber);

        void BatchBookingUpload();


    }
}
