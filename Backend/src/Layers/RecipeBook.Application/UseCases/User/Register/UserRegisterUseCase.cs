using AutoMapper;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class UserRegisterUseCase : IUserRegisterUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnly;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IMapper _autoMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Encryption _encryption;
    private readonly TokenConfigurator _tokenConfigurator;

    public UserRegisterUseCase(IUserWriteOnlyRepository repository,
                               IMapper autoMapper,
                               IUnitOfWork unitOfWork,
                               Encryption encryption,
                               TokenConfigurator tokenConfigurator,
                               IUserReadOnlyRepository userReadOnly)
    {
        _repository = repository;
        _autoMapper = autoMapper;
        _unitOfWork = unitOfWork;
        _encryption = encryption;
        _tokenConfigurator = tokenConfigurator;
        _userReadOnly = userReadOnly;
    }
    public async Task<ResponseUserRegisterJson> Execute(RequestUserRegisterJson request)
    {
        await Validate(request);

        var user = _autoMapper.Map<Domain.Entities.User>(request);
        user.Password = _encryption.PasswordEncryption(request.Password);

        await _repository.Add(user);
        await _unitOfWork.Commit();

        var token = _tokenConfigurator.GenerateToken(user.Email);

        return new ResponseUserRegisterJson
        {
            Token = token
        };
    }

    private async Task Validate(RequestUserRegisterJson request)
    {
        var validator = new UserRegisterValidator();
        var result = validator.Validate(request);

        var userAlreadyExists = await _userReadOnly.CheckIfUserExists(request.Email);
        if (userAlreadyExists)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", ResourceErrorMessages.EMAIL_EXISTS));
        }

        if (!result.IsValid) 
        {
            var errorMessage = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidatorErrorsException(errorMessage);
        }
    }
}
