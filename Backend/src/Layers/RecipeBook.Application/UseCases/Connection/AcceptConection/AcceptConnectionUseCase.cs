using HashidsNet;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories.Code;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Connection;

namespace RecipeBook.Application.UseCases.Connection.AcceptConection;
public class AcceptConnectionUseCase : IAcceptConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _codeRepository;
    private readonly IConnectionWriteOnlyRepository _writeRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashids _hashids;


    public AcceptConnectionUseCase(IConnectionWriteOnlyRepository writeRepository,
                                   ICodeWriteOnlyRepository codeRepository,
                                   IUserLogged userLogged,
                                   IUnitOfWork unitOfWork,
                                   IHashids hashids)
    {
        _writeRepository = writeRepository;
        _codeRepository = codeRepository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
        _hashids = hashids;
    }

    public async Task<string> Execute(string connectedWithUserId)
    {
        var userLogged = await _userLogged.UserRecovery();

        await _codeRepository.Delete(userLogged.Id);

        var userQRCodeReader = _hashids.DecodeLong(connectedWithUserId).First();

        await _writeRepository.Register(new Domain.Entities.Connection
        {
            UserId = userLogged.Id,
            ConnectedWithUserId = userQRCodeReader
        });

        await _writeRepository.Register(new Domain.Entities.Connection
        {
            UserId = userQRCodeReader,
            ConnectedWithUserId = userLogged.Id
        });

        await _unitOfWork.Commit();

        return _hashids.EncodeLong(userLogged.Id);
    }
}
