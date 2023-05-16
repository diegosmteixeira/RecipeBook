using AutoMapper;
using RecipeBook.Application.Services.AutoMapper;

namespace TestsUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Instance()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<AutoMapperConfiguration>();
        });

        return configuration.CreateMapper();
    }
}
