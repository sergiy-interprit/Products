using AutoMapper;
using Products.API.Dto;
using Products.Domain.Models;
using System.Linq;

namespace Products.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Account, AccountDto>()
                //.ForMember(x => x.Products.Select(y => y.AccountId), opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Account, AccountWithoutProductsDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            // DTO to Domain
            CreateMap<SaveAccountDto, Account>().ReverseMap();
            CreateMap<SaveProductDto, Product>().ReverseMap();
        }
    }
}