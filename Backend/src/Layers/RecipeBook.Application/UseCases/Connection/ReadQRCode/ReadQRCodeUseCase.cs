using HashidsNet;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Response;
using RecipeBook.Domain.Repositories.Code;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Connection.ReadQRCode;
public class ReadQRCodeUseCase : IReadQRCodeUseCase
{
    private readonly IConnectionReadOnlyRepository _connectionRepository;
    private readonly ICodeReadOnlyRepository _codeRepository;
    private readonly IUserLogged _userLogged;
    private readonly IHashids _hashids;


    public ReadQRCodeUseCase(IConnectionReadOnlyRepository connectionRepository,
                             ICodeReadOnlyRepository repository,
                             IUserLogged userLogged,
                             IHashids hashids)
    {
        _codeRepository = repository;
        _userLogged = userLogged;
        _connectionRepository = connectionRepository;
        _hashids = hashids;
    }

    public async Task<(ResponseUserConnectionJson userToConnect, 
        string idUserWhichGenerateQRCode)> Execute(string connectionCode)
    {
        var userLogged = await _userLogged.UserRecovery();
        var code = await _codeRepository.RecoverEntityCode(connectionCode);

        await Validate(code, userLogged);

        var userToConnect = new ResponseUserConnectionJson
        {
            Name = userLogged.Name
        };

        return (userToConnect, _hashids.EncodeLong(code.UserId));
    }

    private async Task Validate(Domain.Entities.Code code, 
                                Domain.Entities.User userLogged)
    {
        if (code is null)
        {
            throw new RecipeBookException("");
        }
        if (code.UserId == userLogged.Id)
        {
            throw new RecipeBookException("");
        }

        var connectionExists = await _connectionRepository
            .IsConnectedAsync(code.UserId, userLogged.Id);

        if (connectionExists)
        {
            throw new RecipeBookException("");
        }
    }
}