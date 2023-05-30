using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception.ExceptionsBase;
using RecipeBook.Exception;
using RecipeBook.Domain.Repositories;

namespace RecipeBook.Application.UseCases.Recipe.Delete;
public class RecipeDeleteUseCase : IRecipeDeleteUseCase
{
    private readonly IRecipeWriteOnlyRepository _writeRepository;
    private readonly IRecipeReadOnlyRepository _readRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;
    public RecipeDeleteUseCase(IRecipeWriteOnlyRepository writeRepository,
                               IRecipeReadOnlyRepository readRepository,
                               IUserLogged userLogged,
                               IMapper mapper,
                               IUnitOfWork unitOfWork)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {
        var userLogged = await _userLogged.UserRecovery();

        var recipe = await _readRepository.RecipeRecoveryById(id);

        Validate(userLogged, recipe);

        await _writeRepository.Delete(id);

        await _unitOfWork.Commit();
    }

    private static void Validate(Domain.Entities.User user, Domain.Entities.Recipe recipe)
    {
        if (recipe is null || recipe.UserId != user.Id)

        {
            throw new ValidatorErrorsException(new List<string> { ResourceErrorMessages.RECIPE_NOTFOUND });
        }
    }
}
