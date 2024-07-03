using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.test.PassengersTest
{
    public class PassengerStringBuilder
    {
        public PassengerStringBuilder() { }

        public string StringBuild(List<Passenger> passengers)
        {
            StringBuilder resultBuilder = new StringBuilder();

            foreach (var passenger in passengers)
            {
                PropertyInfo[] properties = typeof(Passenger).GetProperties();
                StringBuilder passengerBuilder = new StringBuilder();

                foreach (var property in properties)
                {
                    passengerBuilder.Append(property.GetValue(passenger));
                    passengerBuilder.Append(",");
                }

                passengerBuilder.Remove(passengerBuilder.Length - 1, 1);

                resultBuilder.AppendLine(passengerBuilder.ToString());
            }

            return resultBuilder.ToString();
        }
    }
}
