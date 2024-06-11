using AirportTicketBookingSystem.Model;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.DTO;
using AutoMapper;

namespace AirportTicketBookingSystem.Controllers
{
    public class PassengerController
    {

        private readonly IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;


        public PassengerController(IPassengerRepository passengerRepository, IMapper mapper)
        {
            _passengerRepository = passengerRepository ?? throw new ArgumentNullException(nameof(passengerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public List<PassengerDTO> GetAllPassengers()
        {
            List<Passenger> passengers = _passengerRepository.GetAllPassengers();
            List<PassengerDTO> dtos = new List<PassengerDTO>();
            return _mapper.Map<List<PassengerDTO>>(passengers);
        }

        public PassengerDTO? GetPassengerById (int id)
        {
            Passenger? passenger = _passengerRepository.GetPassengerById(id);
            if (passenger != null)
            {
                return _mapper.Map<PassengerDTO>(passenger);
            }
            return null;
        }

        public PassengerDTO AddPassenger(int id, string name, string email)
        {
            Passenger? passenger;
            if (_passengerRepository.GetPassengerById(id) != null)
            {
                Console.WriteLine($"Already exists a passenger with id {id}");
                passenger= _passengerRepository.GetPassengerById(id);
            }
            else
            {
                passenger = new Passenger
                {
                    Id = id,
                    Name = name,
                    Email = email
                };
                _passengerRepository.AddPassenger(passenger);
            }
            return _mapper.Map<PassengerDTO>(passenger);
        }


    }
}
