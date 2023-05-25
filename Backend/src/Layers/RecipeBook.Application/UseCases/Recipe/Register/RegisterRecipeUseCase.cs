using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Register;
public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserLogged _userLogged;
    private readonly IRecipeWriteOnlyRepository _repository;
    public RegisterRecipeUseCase(IMapper mapper,
                                 IUnitOfWork unitOfWork,
                                 IUserLogged userLogged, 
                                 IRecipeWriteOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userLogged = userLogged;
        _repository = repository;
    }
    public async Task<ResponseRecipeJson> Execute(RequestRecipeRegisterJson request)
    {
        Validate(request);

        var user = await _userLogged.UserRecovery();
        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = user.Id;

        await _repository.AddRecipe(recipe);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    private void Validate(RequestRecipeRegisterJson request)
    {
        var validator = new RegisterRecipeValidator();
        var result = validator.Validate(request);

        if (!result.IsValid) 
        {
            var messages = result.Errors.Select(result => result.ErrorMessage).ToList();
            throw new ValidatorErrorsException(messages);
        }
    }
}
