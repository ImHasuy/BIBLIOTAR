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
        
            //Fine config
            CreateMap<Fine, FineCreateDto>().ReverseMap();
            CreateMap<Fine, FineGetDto>().ReverseMap();





        }
    }
}
