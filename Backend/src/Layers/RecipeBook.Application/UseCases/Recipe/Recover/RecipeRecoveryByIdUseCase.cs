using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Recover;
public class RecipeRecoveryByIdUseCase : IRecipeRecoveryByIdUseCase
{
    private readonly IConnectionReadOnlyRepository _connRepository;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    public RecipeRecoveryByIdUseCase(IConnectionReadOnlyRepository connRepository,
                                     IRecipeReadOnlyRepository repository,
                                     IUserLogged userLogged,
                                     IMapper mapper)
    {
        _connRepository = connRepository;
        _repository = repository;
        _userLogged = userLogged;
        _mapper = mapper;
    }
    public async Task<ResponseRecipeJson> Execute(long id)
    {
        var userLogged = await _userLogged.UserRecovery();

        var recipe = await _repository.RecipeRecoveryById(id);

        await Validate(userLogged, recipe);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    private async Task Validate(Domain.Entities.User user, Domain.Entities.Recipe recipe)
    {
        var connectedUsers = await _connRepository.RecoverConnections(user.Id);

        if (recipe is null || recipe.UserId != user.Id && !connectedUsers.Any(u => u.Id == recipe.UserId))

        {
            throw new ValidatorErrorsException(new List<string> {  ResourceErrorMessages.RECIPE_NOTFOUND });
        }
    }
}
