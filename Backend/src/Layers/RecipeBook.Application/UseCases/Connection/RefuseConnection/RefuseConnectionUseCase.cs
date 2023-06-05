using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories.Code;
using RecipeBook.Domain.Repositories;
using HashidsNet;

namespace RecipeBook.Application.UseCases.Connection.RefuseConnection;
public class RefuseConnectionUseCase : IRefuseConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashids _hashids;

    public RefuseConnectionUseCase(ICodeWriteOnlyRepository repository,
                                   IUserLogged userLogged,
                                   IUnitOfWork unitOfWork,
                                   IHashids hashids)
    {
        _repository = repository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
        _hashids = hashids;
    }

    public async Task<string> Execute()
    {
        var userLogged = await _userLogged.UserRecovery();

        await _repository.Delete(userLogged.Id);

        await _unitOfWork.Commit();

        return _hashids.EncodeLong(userLogged.Id);
    }
}
