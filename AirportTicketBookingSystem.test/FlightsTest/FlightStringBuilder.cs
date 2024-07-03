using AirportTicketBookingSystem.Model;
using System.Reflection;
using System.Text;

namespace AirportTicketBookingSystem.test.FlightsTest
{
    public class FlightStringBuilder
    {
        public FlightStringBuilder() { }

        public string StringBuild(List<Flight> flights)
        {
            StringBuilder resultBuilder = new StringBuilder();

            foreach (var flight in flights)
            {
                PropertyInfo[] properties = typeof(Flight).GetProperties();
                StringBuilder flightBuilder = new StringBuilder();

                foreach (var property in properties)
                {
                    flightBuilder.Append(property.GetValue(flight));
                    flightBuilder.Append(",");
                }

                flightBuilder.Remove(flightBuilder.Length - 1, 1);

                resultBuilder.AppendLine(flightBuilder.ToString());
            }

            return resultBuilder.ToString();
        }
    }
}
