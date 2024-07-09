using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Profiles
{
    public class FlightProfile : Profile
    {
        public FlightProfile() 
        {
            CreateMap<Model.Flight, DTO.FlightDTO>();
            CreateMap<DTO.FlightDTO, Model.Flight>();
        }

    }
}
