using HashidsNet;
using QRCoder;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Code;
using System.Drawing;

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


    public async Task<(byte[] qrCode, string idUser)> Execute()
    {
        var userLogged = await _userLogged.UserRecovery();

        var code = new Domain.Entities.Code
        {
            CodeId = Guid.NewGuid().ToString(),
            UserId = userLogged.Id
        };

        await _repository.Register(code);
        await _unitOfWork.Commit();

        return (GenerateQRCodeImage(code.CodeId), _hashids.EncodeLong(userLogged.Id));
    }

    private static byte[] GenerateQRCodeImage(string code)
    {
        var qrCodeGenerator = new QRCodeGenerator();

        var qrCodeData = qrCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new QRCode(qrCodeData);

        var bitmap = qrCode.GetGraphic(5, Color.Black, Color.Transparent, true);

        using var stream = new MemoryStream();
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

        return stream.ToArray();
    }
}