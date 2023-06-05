using AutoMapper;
using HashidsNet;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapperConfiguration : Profile
{
    private readonly IHashids _hashids;

    public AutoMapperConfiguration(IHashids hashids)
    {
        _hashids = hashids;

        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<Communication.Request.RequestUserRegisterJson, Domain.Entities.User>()
            .ForMember(destinationMember => destinationMember.Password, config => config.Ignore());

        CreateMap<Communication.Request.RequestRecipeJson, Domain.Entities.Recipe>();

        CreateMap<Communication.Request.RequestIngredientJson, Domain.Entities.Ingredient>();
    }

    private void EntityToResponse()
    {
        CreateMap<Domain.Entities.Recipe, Communication.Response.ResponseRecipeJson>()
            .ForMember(destinationMember => destinationMember.Id, config => 
                config.MapFrom(origin => _hashids.EncodeLong(origin.Id)));
        
        CreateMap<Domain.Entities.Ingredient, Communication.Response.ResponseIngredientJson>()
            .ForMember(destinationMember => destinationMember.Id, config =>
                config.MapFrom(origin => _hashids.EncodeLong(origin.Id)));

        CreateMap<Domain.Entities.Recipe, Communication.Response.ResponseRecipeDashboardJson>()
            .ForMember(destinationMember => destinationMember.Id, config =>
                config.MapFrom(origin => _hashids.EncodeLong(origin.Id)))
            .ForMember(destinationMember => destinationMember.QuantityIngredients, 
                config => config.MapFrom(origin => origin.Ingredients.Count));

        CreateMap<Domain.Entities.User, Communication.Response.ResponseUserProfileJson>();

        CreateMap<Domain.Entities.User, Communication.Response.ResponseUserConnectionJson>();
    }
}
