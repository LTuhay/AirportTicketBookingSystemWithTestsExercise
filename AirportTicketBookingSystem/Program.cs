using AirportTicketBookingSystem.Controller;
using AirportTicketBookingSystem.Controllers;
using Microsoft.Extensions.DependencyInjection;
//using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem

{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<PassengerController>()
                .AddSingleton<BookingController>()
                .AddAutoMapper(typeof(Program))
                .BuildServiceProvider();

            var pc = serviceProvider.GetService<PassengerController>();
            var bc = serviceProvider.GetService<BookingController>();




        }
    }
}


