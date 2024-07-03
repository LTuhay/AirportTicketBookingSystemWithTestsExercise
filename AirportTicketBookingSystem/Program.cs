using AirportTicketBookingSystem.Controller;
using AirportTicketBookingSystem.Controllers;
using Microsoft.Extensions.DependencyInjection;
using AirportTicketBookingSystem.Utilities;
using AirportTicketBookingSystem.Repository;

namespace AirportTicketBookingSystem

{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPassengerRepository, PassengerRepository>() 
                .AddSingleton<IFlightRepository, FlightRepository>()     
                .AddSingleton<IBookingRepository, BookingRepository>()
                .AddAutoMapper(typeof(Program))
                .AddSingleton<PassengerController>()
                .AddSingleton<BookingController>()
                .BuildServiceProvider();

            var pc = serviceProvider.GetService<PassengerController>();
            var bc = serviceProvider.GetService<BookingController>();

            Menu menu = new Menu(pc, bc);
            menu.StartMenu();




        }
    }
}


