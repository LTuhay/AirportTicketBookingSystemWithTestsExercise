
using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.test.BookingTest;
using AirportTicketBookingSystem.test.DataFactory;
using Moq;
using FluentAssertions;


namespace AirportTicketBookingSystem.Repository.Tests
{
    public class BookingRepositoryTests
    {
        private readonly BookingRepository bookingRepository;
        private readonly Mock<IPassengerRepository> mockPassengerRepository;
        private readonly Mock<IFlightRepository> mockFlightRepository;
        private readonly string testFilePath;
        private readonly ITestDataFactory testDataFactory;

        public BookingRepositoryTests()
        {
            testDataFactory = new TestDataFactory();
            testFilePath = Path.GetTempFileName();
            mockPassengerRepository = new Mock<IPassengerRepository>();
            mockFlightRepository = new Mock<IFlightRepository>();

            bookingRepository = new BookingRepository(
                mockPassengerRepository.Object,
                mockFlightRepository.Object,
                testFilePath);
        }

        [Fact]
        public void BatchBookingUpload_ShouldUploadBookingsFromFile()
        {

            Passenger passenger = testDataFactory.CreatePassengerData();
            Flight flight = testDataFactory.CreateFlightData();

            mockPassengerRepository.Setup(pr => pr.GetPassengerById(It.IsAny<int>())).Returns(passenger);
            mockFlightRepository.Setup(fr => fr.GetFlightByNumber(It.IsAny<string>())).Returns(flight);

            Booking booking = testDataFactory.CreateBookingData(passenger, flight);

            List<Booking> bookings = new List<Booking> { booking };
            BookingStringBuilder bookingStringBuilder = new BookingStringBuilder();
            string testData = bookingStringBuilder.StringBuild(bookings);
            File.WriteAllText(testFilePath, testData);


            bookingRepository.BatchBookingUpload();


            var allBookings = bookingRepository.GetAllBookings();
            allBookings.Should().NotBeNull();
            allBookings.Should().ContainSingle();
            allBookings.Should().Contain(b => b.BookingId == booking.BookingId &&
                                              b.Passenger.Id == booking.Passenger.Id &&
                                              b.Flight.FlightNumber == booking.Flight.FlightNumber &&
                                              b.BookingClass == booking.BookingClass &&
                                              b.Price == booking.Price);
        }



        [Fact]
        public void AddBooking_ShouldAddBookingCorrectly()
        {
            Passenger passenger = testDataFactory.CreatePassengerData();
            Flight flight = testDataFactory.CreateFlightData();

            Booking newBooking = testDataFactory.CreateBookingData(passenger, flight);

            bookingRepository.AddBooking(newBooking);

            var result = bookingRepository.GetBookingByID(newBooking.BookingId);
            result.Should().NotBeNull();
            result.Passenger.Id.Should().Be(newBooking.Passenger.Id);
            result.Flight.FlightNumber.Should().Be(newBooking.Flight.FlightNumber);
            result.BookingClass.Should().Be(newBooking.BookingClass);
            result.Price.Should().Be(newBooking.Price);

        }

        [Fact]
        public void DeleteBooking_ShouldDeleteBookingCorrectly()
        {
            Passenger passenger = testDataFactory.CreatePassengerData();
            Flight flight = testDataFactory.CreateFlightData();

            Booking newBooking = testDataFactory.CreateBookingData(passenger, flight);

            bookingRepository.DeleteBooking(newBooking.BookingId);

            var result = bookingRepository.GetBookingByID(newBooking.BookingId);
            result.Should().BeNull();
        }

        [Fact]
        public void GetBookingsByPassenger_ShouldReturnCorrectBookings()
        {
            Passenger passenger1 = testDataFactory.CreatePassengerData();
            Flight flight1 = testDataFactory.CreateFlightData();
            Flight flight2 = testDataFactory.CreateFlightData();

            Booking newBooking1 = testDataFactory.CreateBookingData(passenger1, flight1);
            Booking newBooking2 = testDataFactory.CreateBookingData(passenger1, flight2);

            bookingRepository.AddBooking(newBooking1);
            bookingRepository.AddBooking(newBooking2);

            var result = bookingRepository.GetBookingsByPassenger(passenger1.Id);

            result.Should().NotBeNull();
            result.Should().Contain(b => b.BookingId == newBooking1.BookingId && b.Passenger.Id == passenger1.Id && b.Flight.FlightNumber == flight1.FlightNumber);
            result.Should().Contain(b => b.BookingId == newBooking2.BookingId && b.Passenger.Id == passenger1.Id && b.Flight.FlightNumber == flight2.FlightNumber);
            result.Count.Should().Be(2);
        }

        [Fact]
        public void UpdateBooking_ShouldUpdateBookingCorrectly()
        {
            Passenger passenger = testDataFactory.CreatePassengerData();
            Flight flight = testDataFactory.CreateFlightData(economyPrice: 100);

            Booking newBooking = testDataFactory.CreateBookingData(passenger, flight, bookingId: "id12");

            bookingRepository.AddBooking(newBooking);

            Passenger passenger2 = testDataFactory.CreatePassengerData();

            Booking updatedBooking = testDataFactory.CreateBookingData(passenger2, flight, bookingId: "id12", bookingClass: BookingClass.Economy);


            bookingRepository.UpdateBooking(updatedBooking);

            var result = bookingRepository.GetBookingByID("id12");
            result.Should().NotBeNull();
            result.Passenger.Should().Be(updatedBooking.Passenger);
            result.Price.Should().Be(updatedBooking.Price);

        }

        [Fact]
        public void GetBookingByID_ShouldReturnCorrectBooking()
        {


            Passenger passenger = testDataFactory.CreatePassengerData();
            Flight flight = testDataFactory.CreateFlightData();

            Booking booking = testDataFactory.CreateBookingData(passenger, flight, bookingId: "id12");

            bookingRepository.AddBooking(booking);


            var result = bookingRepository.GetBookingByID("id12");

            result.Should().NotBeNull();
            result.BookingId.Should().Be(booking.BookingId);
            result.Passenger.Id.Should().Be(booking.Passenger.Id);
            result.Flight.FlightNumber.Should().Be(booking.Flight.FlightNumber);
        }

        [Fact]
        public void GetBookingByParams_ShouldReturnFilteredBookings()
        {
            var passenger1 = testDataFactory.CreatePassengerData(id: 1, name: "John Doe", email: "john.doe@example.com");
            var passenger2 = testDataFactory.CreatePassengerData(id: 2, name: "Jane Smith", email: "jane.smith@example.com");
            var flight1 = testDataFactory.CreateFlightData(flightNumber: "AA123", departureCountry: "USA", destinationCountry: "Canada", departureDate: new DateTime(2022, 12, 12), departureAirport: "JFK", arrivalAirport: "YYZ", economyPrice: 100, businessPrice: 200, firstClassPrice: 300);
            var flight2 = testDataFactory.CreateFlightData(flightNumber: "BB456", departureCountry: "USA", destinationCountry: "UK", departureDate: new DateTime(2023, 01, 15), departureAirport: "LAX", arrivalAirport: "LHR", economyPrice: 100, businessPrice: 200, firstClassPrice: 300);

            mockPassengerRepository.Setup(pr => pr.GetPassengerById(1)).Returns(passenger1);
            mockPassengerRepository.Setup(pr => pr.GetPassengerById(2)).Returns(passenger2);
            mockFlightRepository.Setup(fr => fr.GetFlightByNumber("AA123")).Returns(flight1);
            mockFlightRepository.Setup(fr => fr.GetFlightByNumber("BB456")).Returns(flight2);
            var booking1 = testDataFactory.CreateBookingData(bookingId: "B8", passenger: passenger1, flight: flight1, bookingClass: BookingClass.Economy, bookingDate: DateTime.Now);
            var booking2 = testDataFactory.CreateBookingData(bookingId: "B9", passenger: passenger2, flight: flight2, bookingClass: BookingClass.Business, bookingDate: DateTime.Now);
               
            bookingRepository.AddBooking(booking1);
            bookingRepository.AddBooking(booking2);

            var result = bookingRepository.GetBookingByParams(50, 150, "USA", "Canada", new DateTime(2022, 12, 12), "JFK", "YYZ", "Economy");

            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.Should().Contain(b => b.BookingId == "B8");

        }

        [Fact]
        public void GetBookingByFlightNumber_ShouldReturnCorrectBookings()
        {
            var passenger1 = testDataFactory.CreatePassengerData(id: 1, name: "John Doe", email: "john.doe@example.com");
            var passenger2 = testDataFactory.CreatePassengerData(id: 2, name: "Jane Smith", email: "jane.smith@example.com");
            var flight1 = testDataFactory.CreateFlightData(flightNumber: "AA123", departureCountry: "USA", destinationCountry: "Canada", departureDate: new DateTime(2022, 12, 12), departureAirport: "JFK", arrivalAirport: "YYZ");

            mockPassengerRepository.Setup(pr => pr.GetPassengerById(1)).Returns(passenger1);
            mockPassengerRepository.Setup(pr => pr.GetPassengerById(2)).Returns(passenger2);
            mockFlightRepository.Setup(fr => fr.GetFlightByNumber("AA123")).Returns(flight1);

            var booking1 = testDataFactory.CreateBookingData(bookingId: "B8", passenger: passenger1, flight: flight1, bookingClass: BookingClass.Economy, bookingDate: DateTime.Now);
            var booking2 = testDataFactory.CreateBookingData(bookingId: "B9", passenger: passenger2, flight: flight1, bookingClass: BookingClass.Business, bookingDate: DateTime.Now);

            bookingRepository.AddBooking(booking1);
            bookingRepository.AddBooking(booking2);

            var result = bookingRepository.GetBookingByFlightNumber("AA123");

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.Should().Contain(b => b.BookingId == "B8");
            result.Should().Contain(b => b.BookingId == "B9");

        }


    }
}
