using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile() 
        {
            CreateMap<Model.Booking, DTO.BookingDTO>();
            CreateMap<DTO.BookingDTO, Model.Booking>();
        }
    }
}
