using AutoMapper;
using BiblioTar.Entities;
using BiblioTar.DTOs;

namespace BiblioTar.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {   //Address config
            CreateMap<Address, AddressCreateDto>().ReverseMap();
            CreateMap<Address, AddressGetDto>().ReverseMap();

            //User config
            CreateMap<User, UserCreateDto>().ReverseMap(); //Vissza fele nem lehet mert a Createben nincs benne az AddressId
            CreateMap<User, EmployeeCreateDto>().ReverseMap();
            
            //UserDto config
            CreateMap<UserDtoToUpdateFunc, UserUpdateInformationDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            //Fine config
            CreateMap<Fine, FineCreateDto>().ReverseMap();
            CreateMap<Fine, FineGetDto>().ReverseMap();

            //Book config
            CreateMap<Book, BookCreateDto>().ReverseMap();
            CreateMap<Book,BookGetDto>().ReverseMap();
            CreateMap<Book,BookUpdateDto>().ReverseMap();
            


            //Employee config

            //Reservation config
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title)).ReverseMap();
            CreateMap<Reservation, ReservationLOggedCreateDto>().ReverseMap();

            
            CreateMap<UserGetDto, User>().ReverseMap();
            
            //Reservation config
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            
            //Borrow config
            CreateMap<Borrow, BorrowDto>().ReverseMap();
            CreateMap<Borrow, BorrowGetDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title)).ReverseMap();
            
            

            
            
        }
    }
}
