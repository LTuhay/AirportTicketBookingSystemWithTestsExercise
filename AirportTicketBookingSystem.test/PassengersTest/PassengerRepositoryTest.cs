
using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.test.DataFactory;

namespace AirportTicketBookingSystem.test.PassengersTest

{
    public class PassengerRepositoryTests
    {
        private readonly PassengerRepository passengerRepository;
        private readonly string testFilePath;
        private readonly ITestDataFactory testDataFactory;

        public PassengerRepositoryTests()
        {
            testDataFactory = new TestDataFactory();
            testFilePath = Path.GetTempFileName();
            File.WriteAllText(testFilePath, GetTestPassengerData());
            passengerRepository = new PassengerRepository(testFilePath);
        }

        [Fact]
        public void BatchPassengerUpload_ShouldLoadPassengersFromFile()
        {

            passengerRepository.BatchPassengerUpload();

            var passengers = passengerRepository.GetAllPassengers();
            Assert.NotEmpty(passengers);
            Assert.Equal(3, passengers.Count);
            Assert.Contains(passengers, p => p.Name == "John Doe");
        }

        [Fact]
        public void AddPassenger_ShouldAddPassengerCorrectly()
        {

            passengerRepository.BatchPassengerUpload();
            var newPassenger = testDataFactory.CreatePassengerData(id: 4);
                
            passengerRepository.AddPassenger(newPassenger);

            var retrievedPassenger = passengerRepository.GetPassengerById(4);
            Assert.NotNull(retrievedPassenger);
            Assert.Equal(newPassenger.Id, retrievedPassenger.Id);
            Assert.Equal(newPassenger.Name, retrievedPassenger.Name);
            Assert.Equal(newPassenger.Email, retrievedPassenger.Email);

            var lines = File.ReadAllLines(testFilePath);
            Assert.Contains(lines, line => line == newPassenger.Id+","+newPassenger.Name+","+  newPassenger.Email);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnCorrectPassenger()
        {

            passengerRepository.BatchPassengerUpload();
            var expectedPassengerId = 1;
            var expectedPassengerName = "John Doe";
            var expectedPassengerEmail = "john.doe@example.com";

            var passenger = passengerRepository.GetPassengerById(expectedPassengerId);

            Assert.NotNull(passenger);
            Assert.Equal(expectedPassengerId, passenger.Id);
            Assert.Equal(expectedPassengerName, passenger.Name);
            Assert.Equal(expectedPassengerEmail, passenger.Email);
        }



        private string GetTestPassengerData()
        {
            var passengers = new List<Passenger>();
            var passenger1 = testDataFactory.CreatePassengerData(id:1, name: "John Doe", email: "john.doe@example.com");
            var passenger2 = testDataFactory.CreatePassengerData();
            var passenger3 = testDataFactory.CreatePassengerData();
            passengers.Add(passenger1);
            passengers.Add(passenger2);
            passengers.Add(passenger3);

            PassengerStringBuilder passengerStringBuilder = new PassengerStringBuilder();
            return passengerStringBuilder.StringBuild(passengers);
        }
    }

}
