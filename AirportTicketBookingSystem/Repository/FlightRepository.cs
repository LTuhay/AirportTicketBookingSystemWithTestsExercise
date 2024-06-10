using AirportTicketBookingSystem.Model;
using System.ComponentModel.DataAnnotations;


namespace AirportTicketBookingSystem.Repository
{
    public class FlightRepository : IFlightRepository
    {

        private string _filePath;
        private List<Flight> flights;

        public FlightRepository(string? filePath = null)
        {
            _filePath = filePath ?? @"..\..\..\Data\FlightData.txt";
            flights = new List<Flight>();
            FlightUpload(filePath);
        }

        public void BatchFlightUpload(string fp) // Uploads flight data from file
        {
            FlightUpload(fp);
        }

        public void BatchFlightUpload()
        {
            FlightUpload(_filePath);
        }


        private void FlightUpload(string fp)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fp))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 9)
                        {
                            Flight flight = new Flight
                            {
                                FlightNumber = parts[0],
                                DepartureCountry = parts[1],
                                DestinationCountry = parts[2],
                                DepartureDate = DateTime.Parse(parts[3]),
                                DepartureAirport = parts[4],
                                ArrivalAirport = parts[5],
                                EconomyPrice = decimal.Parse(parts[6]),
                                BusinessPrice = decimal.Parse(parts[7]),
                                FirstClassPrice = decimal.Parse(parts[8])
                            };

                            var validationContext = new ValidationContext(flight, serviceProvider: null, items: null);
                            var validationResults = new List<ValidationResult>();


                            bool isValid = Validator.TryValidateObject(flight, validationContext, validationResults, validateAllProperties: true);

                            if (isValid)
                            {
                                bool flightAlreadyExists = flights.Any(existingFlight => existingFlight.FlightNumber == flight.FlightNumber);
                                if (!flightAlreadyExists)
                                {
                                    flights.Add(flight);
                                }
                                else
                                {
                                    Console.WriteLine($"Flight {flight.FlightNumber} already exists.");
                                }
                            }
                            else
                            {
                                foreach (var validationResult in validationResults)
                                {
                                    Console.WriteLine(validationResult.ErrorMessage);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid flight format");
                        }
                          
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }




        public List<Flight> GetAllFlights()
        {
            return flights;
        }

       public  Flight? GetFlightByNumber(string flightNumber)
        {
            return flights.FirstOrDefault(flight => flight.FlightNumber == flightNumber);
        }

        public List<Flight> GetFlightByParams(decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass)
        {
            return flights
                .Where(flight =>
                    (departure == "" || flight.DepartureCountry == departure) &&
                    (destination == "" || flight.DestinationCountry == destination) &&
                    (departureDate == DateTime.MinValue || flight.DepartureDate == departureDate) &&
                    (departureAirport == "" || flight.DepartureAirport == departureAirport) &&
                    (destinationAirport == "" || flight.ArrivalAirport == destinationAirport) &&
                    (
                        (minPrice == -1 && maxPrice == -1) ||
                        (
                            (minPrice == -1 || flight.EconomyPrice >= minPrice) &&
                            (maxPrice == -1 || flight.EconomyPrice <= maxPrice)
                        ) ||
                        (
                            (minPrice == -1 || flight.BusinessPrice >= minPrice) &&
                            (maxPrice == -1 || flight.BusinessPrice <= maxPrice)
                        ) ||
                        (
                            (minPrice == -1 || flight.FirstClassPrice >= minPrice) &&
                            (maxPrice == -1 || flight.FirstClassPrice <= maxPrice)
                        )
                    ) &&
                    (
                        (travelClass == "" && (flight.EconomyPrice > 0 || flight.BusinessPrice > 0 || flight.FirstClassPrice > 0)) ||
                        (travelClass == "Economy" && flight.EconomyPrice > 0) ||
                        (travelClass == "Business" && flight.BusinessPrice > 0) ||
                        (travelClass == "First Class" && flight.FirstClassPrice > 0)
                    )
                )
                .Distinct()
                .ToList();
        }


    }
}
