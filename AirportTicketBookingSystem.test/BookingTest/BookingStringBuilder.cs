using AirportTicketBookingSystem.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AirportTicketBookingSystem.test.BookingTest
{
    public class BookingStringBuilder
    {
        public BookingStringBuilder() { }

        public string StringBuild(List<Booking> bookings)
        {
            StringBuilder resultBuilder = new StringBuilder();

            foreach (var booking in bookings)
            {
                PropertyInfo[] properties = typeof(Booking).GetProperties();
                StringBuilder bookingBuilder = new StringBuilder();

                foreach (var property in properties)
                {
                    var value = property.GetValue(booking);

                    if (value is Passenger passenger)
                    {
                        bookingBuilder.Append($"{passenger.Id}");
                    }
                    else if (value is Flight flight)
                    {
                        bookingBuilder.Append($"{flight.FlightNumber}");
                    }
                    else
                    {
                        bookingBuilder.Append(value);
                    }

                    bookingBuilder.Append(",");
                }

                bookingBuilder.Remove(bookingBuilder.Length - 1, 1);

                resultBuilder.AppendLine(bookingBuilder.ToString());
            }

            return resultBuilder.ToString();
        }
    }
}
