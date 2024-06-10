using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.test.DataFactory;

namespace AirportTicketBookingSystem.test.FlightsTest
{
    public class FlightRepositoryTests
    {
        private readonly FlightRepository _flightRepository;
        private readonly string _testFilePath;
        private readonly ITestDataFactory testDataFactory;

        public FlightRepositoryTests()
        {
            testDataFactory = new TestDataFactory();
            _testFilePath = Path.GetTempFileName();
            File.WriteAllText(_testFilePath, GetTestFlightData());
            _flightRepository = new FlightRepository();
        }

        [Fact]
        public void BatchFlightUpload_ShouldLoadFlightsFromFile()
        {


            _flightRepository.BatchFlightUpload(_testFilePath);

            var flights = _flightRepository.GetAllFlights();
            Assert.NotEmpty(flights);
            Assert.Equal(3, flights.Count);
        }

        [Fact]
        public void GetFlightByNumber_ShouldReturnCorrectFlight()
        {

            _flightRepository.BatchFlightUpload(_testFilePath);
            var expectedFlightNumber = "AB123";


            var flight = _flightRepository.GetFlightByNumber(expectedFlightNumber);


            Assert.NotNull(flight);
            Assert.Equal(expectedFlightNumber, flight.FlightNumber);
        }

        [Fact]
        public void GetFlightByParams_ShouldReturnFilteredFlights()
        {

            _flightRepository.BatchFlightUpload(_testFilePath);
            decimal minPrice = 200;
            decimal maxPrice = 500;
            string departureCountry = "USA";
            string destinationCountry = "UK";
            DateTime departureDate = new DateTime(2023, 6, 15);
            string departureAirport = "JFK";
            string arrivalAirport = "LHR";
            string travelClass = "Economy";


            var flights = _flightRepository.GetFlightByParams(minPrice, maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, travelClass);


            Assert.Single(flights);
            Assert.Equal("FL123", flights.First().FlightNumber);
        }

        private string GetTestFlightData()
        {
            var flights = new List<Flight>();
            var flight1 = testDataFactory.CreateFlightData(flightNumber: "AB123");
            var flight2 = testDataFactory.CreateFlightData(
                flightNumber: "FL123",
                departureCountry: "USA",
                destinationCountry: "UK",
                departureDate: new DateTime(2023, 6, 15),
                departureAirport: "JFK",
                arrivalAirport: "LHR",
                economyPrice: 300
            );
            var flight3 = testDataFactory.CreateFlightData();

            flights.Add(flight1);
            flights.Add(flight2);
            flights.Add(flight3);

            FlightStringBuilder flightStringBuilder = new FlightStringBuilder();

            return flightStringBuilder.StringBuild(flights);

        }
    }
}
