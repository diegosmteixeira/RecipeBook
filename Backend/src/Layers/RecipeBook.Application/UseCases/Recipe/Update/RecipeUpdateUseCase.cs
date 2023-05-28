using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Request;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception.ExceptionsBase;
using RecipeBook.Exception;
using RecipeBook.Domain.Repositories;
using RecipeBook.Application.UseCases.Recipe.Register;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public class RecipeUpdateUseCase : IRecipeUpdateUseCase
{
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public RecipeUpdateUseCase(IRecipeUpdateOnlyRepository repository,
                               IUserLogged userLogged,
                               IMapper mapper,
                               IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userLogged = userLogged;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id, RequestRecipeJson request)
    {
        var userLogged = await _userLogged.UserRecovery();

        var recipe = await _repository.RecipeRecoveryById(id);

        Validate(userLogged, recipe, request);

        _mapper.Map(request, recipe);

        _repository.Update(recipe);

        await _unitOfWork.Commit();
    }

    private static void Validate(Domain.Entities.User user, 
                                 Domain.Entities.Recipe recipe,
                                 RequestRecipeJson request)
    {
        if (recipe == null || recipe.UserId != user.Id)
        {
            throw new ValidatorErrorsException(new List<string> { ResourceErrorMessages.RECIPE_NOTFOUND });
        }

        var validator = new RecipeUpdateValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var messages = result.Errors.Select(result => result.ErrorMessage).ToList();
            throw new ValidatorErrorsException(messages);
        }
    }
}
