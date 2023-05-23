using FluentValidation;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Request;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly Encryption _encryption;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUseCase(IUserUpdateOnlyRepository repository,
                                 IUserLogged userLogged,
                                 Encryption encryption,
                                 IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userLogged = userLogged;
        _encryption = encryption;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var userLogged = await _userLogged.UserRecovery();

        var user = await _repository.UserRecoveryById(userLogged.Id);

        Validate(request, user);

        user.Password = _encryption.PasswordEncryption(request.NewPassword);
        _repository.Update(user);

        await _unitOfWork.Commit();

    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var currentEncryptedPassword = _encryption.PasswordEncryption(request.CurrentPassword);

        if (!user.Password.Equals(currentEncryptedPassword)) 
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("Current Password", 
                ResourceErrorMessages.INVALID_CURRENT_PASSWORD));
        }



        if (!result.IsValid) 
        {
            var messages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidatorErrorsException(messages);
        }
    }
}
