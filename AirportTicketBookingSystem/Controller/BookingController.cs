using AirportTicketBookingSystem.DTO;
using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.Repository;
using AutoMapper;

namespace AirportTicketBookingSystem.Controller
{
    public class BookingController
    {

        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;


        private BookingController(IFlightRepository flightRepository, IPassengerRepository passengerRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _passengerRepository = passengerRepository ?? throw new ArgumentNullException(nameof(passengerRepository));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public void BatchFlightUpload(string filePath)
        {
            _flightRepository.BatchFlightUpload(filePath);
        }


        public List<FlightDTO> GetAllFlights()
        {
            List<Flight>flights = _flightRepository.GetAllFlights();
            return _mapper.Map<List<FlightDTO>>(flights);

        }

        public List<BookingDTO> GetBookingsByPassenger(int passengerId)
        {
            List<Booking> bookings = _bookingRepository.GetBookingsByPassenger(passengerId);
            return _mapper.Map<List<BookingDTO>>(bookings);
        }

        public FlightDTO? GetFlightsByNumber(string number)
        {
            Flight? flight = _flightRepository.GetFlightByNumber(number);
            if (flight != null)
            {
                return _mapper.Map<FlightDTO>(flight);
            }

            return null;
        }
        public List<FlightDTO> GetFlightsByParams(decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass)
        {
            List<Flight> flights = _flightRepository.GetFlightByParams(minPrice, maxPrice, departure, destination, departureDate, departureAirport, destinationAirport, travelClass);
            return _mapper.Map<List<FlightDTO>>(flights);
        }

        public List<BookingDTO> GetBookingsByParams(decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass)
        {
            List<Booking> bookings = _bookingRepository.GetBookingByParams(minPrice, maxPrice, departure, destination, departureDate, departureAirport, destinationAirport, travelClass);
            return _mapper.Map<List<BookingDTO>>(bookings);
        }

        public List<BookingDTO> GetAllBookings() 
        { 
            List<Booking> bookings = _bookingRepository.GetAllBookings();
            return _mapper.Map<List<BookingDTO>>(bookings);
        }

        public Guid GenerateUniqueBookingId()
        {

            Guid bookingId = Guid.NewGuid();
            List<Booking> bookings = _bookingRepository.GetAllBookings();

            while (bookings.Exists(booking => booking.BookingId.Equals(bookingId)))
            {
                bookingId = Guid.NewGuid();
            }

            return bookingId;
        }

        public void AddBooking (BookingDTO booking)
        {
            
            _bookingRepository.AddBooking(_mapper.Map<Booking>(booking));
        }


        public List<BookingDTO> GetBookingsByFlightNumber(string flightNumber)
        {
            List<Booking> bookings = _bookingRepository.GetBookingByFlightNumber(flightNumber);
            return _mapper.Map<List<BookingDTO>>(bookings);
        }

        public BookingDTO? GetBookingById(string id)
        {
            Booking? booking = _bookingRepository.GetBookingByID(id);
            if (booking != null)
            {
                return _mapper.Map<BookingDTO>(booking);
            }
            return null;
        }

        public void DeleteBookingById(string id)
        {
            _bookingRepository.DeleteBooking(id);
        }

        public void UpdateBooking(BookingDTO booking)
        {
            _bookingRepository.UpdateBooking(_mapper.Map<Booking>(booking));
        }

        public void ImportConstraints()
        {
            Console.WriteLine("Flight Import Constraints:");
            Console.WriteLine();

            Console.WriteLine("1. Flight Number:");
            Console.WriteLine("   - Required: Each flight must have a unique flight number.");
            Console.WriteLine();

            Console.WriteLine("2. Departure and Destination Country:");
            Console.WriteLine("   - Required: Departure and destination country must be specified for each flight.");
            Console.WriteLine("   - Length Limit: Country names cannot exceed 20 characters.");
            Console.WriteLine();

            Console.WriteLine("3. Departure Date:");
            Console.WriteLine("   - Required: Departure date for each flight is mandatory.");
            Console.WriteLine("   - Format: Date should be in the correct format (DD/MM/YYYY HH:MM).");
            Console.WriteLine("   - Allowed Range: Departure date must be in the future.");
            Console.WriteLine();

            Console.WriteLine("4. Departure and Arrival Airport:");
            Console.WriteLine("   - Required: Departure and arrival airport must be provided for each flight.");
            Console.WriteLine("   - Length Limit: Airport names cannot exceed 20 characters.");
            Console.WriteLine();

            Console.WriteLine("5. Economy, Business, and First Class Price:");
            Console.WriteLine("   - Optional: If a class is not available on a flight, the price should be set to 0.");
            Console.WriteLine("   - Price Range: Prices must be non-negative numbers.");
        }




    }
}
