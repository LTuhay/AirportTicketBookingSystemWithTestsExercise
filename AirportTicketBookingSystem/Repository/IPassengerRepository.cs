using AirportTicketBookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Repository
{
    public interface IPassengerRepository
    {

        void BatchPassengerUpload();
        List<Passenger> GetAllPassengers();
        Passenger? GetPassengerById(int passengerId);
        void AddPassenger(Passenger passenger);


    }
}
