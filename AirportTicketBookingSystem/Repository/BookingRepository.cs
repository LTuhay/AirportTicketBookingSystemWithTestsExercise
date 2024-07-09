using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirportTicketBookingSystem.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private string _filePath = @"..\..\..\Data\BookingsData.txt";
        private List<Booking> bookings;
        private IPassengerRepository _passengerRepository;
        private IFlightRepository _flightRepository;

        public BookingRepository(IPassengerRepository passengerRepository, IFlightRepository flightRepository, string? filePath = null)
        {
            _passengerRepository = passengerRepository ?? throw new ArgumentNullException(nameof(passengerRepository));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _filePath = filePath ?? @"..\..\..\Data\BookingsData.txt";

            bookings = new List<Booking>();
            BookingUpload(filePath);
        }

        public void BatchBookingUpload() // Uploads bookings from file
        {
            BookingUpload(_filePath);
        }

        private void BookingUpload (string fp)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fp))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length >= 6)
                        {
                            Booking booking = new Booking
                            {
                                BookingId = parts[0],
                                Passenger = _passengerRepository.GetPassengerById(int.Parse(parts[1])),
                                Flight = _flightRepository.GetFlightByNumber(parts[2]),
                                BookingClass = (BookingClass)Enum.Parse(typeof(BookingClass), parts[3]),
                                BookingDate = DateTime.Parse(parts[4]),
                //                Price = decimal.Parse(parts[5])
                            };
                            bookings.Add(booking);
                        }
                        else
                        {
                            Console.WriteLine("Invalid booking format.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while uploading bookings: {ex.Message}");
            }
        }

        public void AddBooking(Booking booking)
        {
            bookings.Add(booking);
            string line = $"{booking.BookingId},{booking.Passenger.Id},{booking.Flight.FlightNumber},{booking.BookingClass},{booking.BookingDate},{booking.Price}";

            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(line);
            }
        }

        public void DeleteBooking(string bookingId)
        {
            string tempFile = Path.GetTempFileName();

            using (var reader = new StreamReader(_filePath))
            using (var writer = new StreamWriter(tempFile))
            {
                string ? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith(bookingId))
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            File.Delete(_filePath);
            File.Move(tempFile, _filePath);

            bookings.RemoveAll(booking => booking.BookingId == bookingId);
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            return bookings.Where(booking => booking.Passenger.Id == passengerId).ToList();
        }

        public void UpdateBooking(Booking booking)
        {
            try
            {
                string tempFile = Path.GetTempFileName();

                using (var reader = new StreamReader(_filePath))
                using (var writer = new StreamWriter(tempFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 5 && parts[0] == booking.BookingId)
                        {
                            writer.WriteLine($"{booking.BookingId},{booking.Passenger.Id},{booking.Flight.FlightNumber},{booking.BookingClass},{booking.BookingDate},{booking.Price}");
                            int index = bookings.FindIndex(b => b.BookingId == booking.BookingId);
                            if (index != -1)
                            {
                                bookings[index] = booking;
                            }
                        }
                        else
                        {
                            writer.WriteLine(line); 
                        }
                    }
                }

                File.Delete(_filePath);
                File.Move(tempFile, _filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the file: {ex.Message}");
            }
        }


        public List<Booking> GetAllBookings()
        {
            return bookings;
        }

        public Booking? GetBookingByID(string id)
        {
            return bookings.FirstOrDefault(booking => booking.BookingId == id);
        }

        public List<Booking> GetBookingByParams(decimal minPrice, decimal maxPrice, string? departure, string? destination, DateTime departureDate, string? departureAirport, string? destinationAirport, string? travelClass)
        {

            var filteredBookings = bookings
                .Where(booking => (departure == "" || booking.Flight.DepartureCountry == departure)
                    && (destination == "" || booking.Flight.DestinationCountry == destination)
                    && (departureDate == DateTime.MinValue || booking.Flight.DepartureDate == departureDate)
                    && (departureAirport == "" || booking.Flight.DepartureAirport == departureAirport)
                    && (destinationAirport == "" || booking.Flight.ArrivalAirport == destinationAirport)
                    && ((minPrice == -1 && maxPrice == -1) ||
                        ((minPrice == -1 || booking.Price >= minPrice) &&
                        (maxPrice == -1 || booking.Price <= maxPrice)))
                    && (travelClass == "" || booking.BookingClass.ToString().Equals(travelClass, StringComparison.OrdinalIgnoreCase)))
                .Distinct()
                .ToList();

            return filteredBookings;
        }

        public List<Booking> GetBookingByFlightNumber(string FlightNumber)
        {
            return bookings.Where(booking => booking.Flight.FlightNumber == FlightNumber).ToList();
        }
    }
}
