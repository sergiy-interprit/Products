using AutoMapper;
using Products.API.Models;
using Products.Core.Models;

namespace Products.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Account, AccountDto>();
            CreateMap<Product, ProductDto>();

            // DTO to Domain
            CreateMap<AccountDto, Account>();
            CreateMap<SaveAccountDto, Account>();

            CreateMap<ProductDto, Product>();
            CreateMap<SaveProductDto, Product>();
        }
    }
}