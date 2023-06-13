using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Connection.RemoveConnection;
public class RemoveConnectionUseCase : IRemoveConnectionUseCase
{
    private readonly IConnectionReadOnlyRepository _connReadRepository;
    private readonly IConnectionWriteOnlyRepository _connWriteRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveConnectionUseCase(IConnectionReadOnlyRepository connReadRepository,
                                   IConnectionWriteOnlyRepository connWriteRepository,
                                   IUserLogged userLogged,
                                   IUnitOfWork unitOfWork)
    {
        _connReadRepository = connReadRepository;
        _connWriteRepository = connWriteRepository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long userIdToRemove)
    {
        var userLogged = await _userLogged.UserRecovery();

        var connectedUsers = await _connReadRepository.RecoverConnections(userLogged.Id);

        Validate(connectedUsers, userIdToRemove);

        await _connWriteRepository.RemoveConnection(userLogged.Id, userIdToRemove);

        await _unitOfWork.Commit();
    }

    private static void Validate(IList<Domain.Entities.User> connectedUsers, long userIdToRemove)
    {
        if (!connectedUsers.Any(u => u.Id == userIdToRemove))
        {
            throw new ValidatorErrorsException(new List<string> { ResourceErrorMessages.USER_NOT_FOUND });
        }
    }
}
