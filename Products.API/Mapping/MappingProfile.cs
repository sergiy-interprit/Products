using AutoMapper;
using Products.API.Models;
using Products.Domain.Models;
using System.Linq;

namespace Products.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO
            CreateMap<Account, AccountDto>()
                //.ForMember(x => x.Products.Select(y => y.AccountId), opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Account, AccountWithoutProductsDto>();

            CreateMap<Product, ProductDto>().ReverseMap();

            // DTO to Domain
            //CreateMap<AccountDto, Account>();
            CreateMap<SaveAccountDto, Account>().ReverseMap();

            //CreateMap<ProductDto, Product>();
            CreateMap<SaveProductDto, Product>();
        }
    }
}