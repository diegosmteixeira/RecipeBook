using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Code;

namespace RecipeBook.Application.UseCases.Connection.GenerateQRCode;
public class GenerateQRCodeUseCase : IGenerateQRCodeUseCase
{
    private readonly ICodeWriteOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateQRCodeUseCase(ICodeWriteOnlyRepository repository, 
                                 IUserLogged userLogged, 
                                 IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userLogged = userLogged;
        _unitOfWork = unitOfWork;
    }


    public async Task<string> Execute()
    {
        var userLogged = await _userLogged.UserRecovery();

        var code = new Domain.Entities.Code
        {
            CodeId = Guid.NewGuid().ToString(),
            UserId = userLogged.Id
        };

        await _repository.Register(code);
        await _unitOfWork.Commit();

        return code.CodeId;
    }
}