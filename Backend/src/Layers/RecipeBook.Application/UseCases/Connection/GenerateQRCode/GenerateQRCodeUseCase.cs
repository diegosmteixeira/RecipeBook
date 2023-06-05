using HashidsNet;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Code;

namespace RecipeBook.Application.UseCases.Connection.GenerateQRCode;
public class GenerateQRCodeUseCase : IGenerateQRCodeUseCase
{
    private readonly ICodeWriteOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashids _hashids;

    public GenerateQRCodeUseCase(ICodeWriteOnlyRepository repository,
                                 IUserLogged userLogged,
                                 IUnitOfWork unitOfWork,
                                 IHashids hashids)
    {
        _repository = repository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
        _hashids = hashids;
    }


    public async Task<(string qrCode, string idUser)> Execute()
    {
        var userLogged = await _userLogged.UserRecovery();

        var code = new Domain.Entities.Code
        {
            CodeId = Guid.NewGuid().ToString(),
            UserId = userLogged.Id
        };

        await _repository.Register(code);
        await _unitOfWork.Commit();

        return (code.CodeId, _hashids.EncodeLong(userLogged.Id));
    }
}