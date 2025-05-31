using AutoMapper;

using Contracts;

using SearchService.Consumers;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreatedContract, Item>();
        CreateMap<AuctionUpdatedContract, Item>();
    }
}
