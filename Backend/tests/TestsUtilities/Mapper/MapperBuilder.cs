using AutoMapper;
using RecipeBook.Application.Services.AutoMapper;
using TestsUtilities.Hashids;

namespace TestsUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Instance()
    {
        var hashids = HashidsBuilder.Instance().Build();

        var mockMapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapperConfiguration(hashids));
        });

        return mockMapper.CreateMapper();
    }
}
