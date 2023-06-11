using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Application.UseCases.Connection.RecoverConnection;
public class RecoverConnectionUseCase : IRecoverConnectionUseCase
{
    private readonly IUserLogged _userLogged;
    private readonly IConnectionReadOnlyRepository _connRepository;
    private readonly IRecipeReadOnlyRepository _readRepository;
    private readonly IMapper _mapper;
    public RecoverConnectionUseCase(IUserLogged userLogged,
                                    IConnectionReadOnlyRepository connRepository,
                                    IRecipeReadOnlyRepository readRepository,
                                    IMapper mapper)
    {
        _userLogged = userLogged;
        _connRepository = connRepository;
        _readRepository = readRepository;
        _mapper = mapper;
    }
    public async Task<ResponseUserConnectedListJson> Execute()
    {
        var userLogged = await _userLogged.UserRecovery();

        var connections = await _connRepository.RecoverConnections(userLogged.Id);

        var tasks = connections.Select(async user =>
        {
            var recipeQuantity = await _readRepository.RecipeRecoveryCount(user.Id);

            var userJson = _mapper.Map<ResponseUserConnectedWithJson>(user);
            userJson.RecipesQuantity = recipeQuantity;

            return userJson;
        });

        return new ResponseUserConnectedListJson
        {
            Users = await Task.WhenAll(tasks)
        };
    }
}
