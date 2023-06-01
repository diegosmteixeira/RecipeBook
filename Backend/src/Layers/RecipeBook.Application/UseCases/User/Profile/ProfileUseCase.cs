using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.User.Profile;
public class ProfileUseCase : IProfileUseCase
{
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;

    public ProfileUseCase(IUserLogged userLogged,
                          IMapper mapper)
    {
        _mapper = mapper;
        _userLogged = userLogged;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _userLogged.UserRecovery();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
