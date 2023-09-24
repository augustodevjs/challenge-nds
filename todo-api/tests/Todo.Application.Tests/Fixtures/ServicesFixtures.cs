using AutoMapper;
using Todo.Application.AutoMapper;

namespace Todo.Application.Tests.Fixtures;

public class ServicesFixtures
{
    public readonly IMapper Mapper = CreateMapper();

    private static IMapper CreateMapper()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });

        return mappingConfig.CreateMapper();
    }
}