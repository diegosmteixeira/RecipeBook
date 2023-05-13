using AutoMapper;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<Communication.Request.RequestUserRegisterJson, Domain.Entities.User>()
            .ForMember(destinationMember => destinationMember.Password, config => config.Ignore());
    }
}
