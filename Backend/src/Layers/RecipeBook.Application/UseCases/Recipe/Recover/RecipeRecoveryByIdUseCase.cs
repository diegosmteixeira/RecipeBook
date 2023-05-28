using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Recover;
public class RecipeRecoveryByIdUseCase : IRecipeRecoveryByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    public RecipeRecoveryByIdUseCase(IRecipeReadOnlyRepository repository,
                            IUserLogged userLogged,
                            IMapper mapper)
    {
        _repository = repository;
        _userLogged = userLogged;
        _mapper = mapper;
    }
    public async Task<ResponseRecipeJson> Execute(long id)
    {
        var userLogged = await _userLogged.UserRecovery();

        var recipe = await _repository.RecipeRecoveryById(id);

        Validate(userLogged, recipe);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    private static void Validate(Domain.Entities.User user, Domain.Entities.Recipe recipe)
    {
        if (recipe == null || recipe.UserId != user.Id)
        {
            throw new ValidatorErrorsException(new List<string> {  ResourceErrorMessages.RECIPE_NOTFOUND });
        }
    }
}
