using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Profiles
{
    public class PassengerProfile : Profile
    {
        public PassengerProfile() 
        {
            CreateMap<Model.Passenger, DTO.PassengerDTO>();
            CreateMap<DTO.PassengerDTO, Model.Passenger>();
        }
    }
}
