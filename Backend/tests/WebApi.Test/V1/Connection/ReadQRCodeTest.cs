using Moq;
using RecipeBook.API.WebSockets;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Application.UseCases.Connection.ReadQRCode;
using RecipeBook.Communication.Response;
using RecipeBook.Exception;
using TestsUtilities.Responses;
using WebApi.Test.V1.Connection.Builder;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class ReadQRCodeTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var mockHubContext,
         var mockClientProxy,
         var mockClients,
         var mockHubCallerContext) = MockWebSocketConnectionsBuilder.Build();

        var connectionCode = Guid.NewGuid().ToString();
        var userToConnect = ResponseUserConnectionJsonBuilder.Build(); 

        var useCaseReadQRCode = ReadQRCodeBuilder(userToConnect, connectionCode);
        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);

        var hub = new Socket(null, null, useCaseGenerateQRCoder, mockHubContext.Object, useCaseReadQRCode)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        await hub.ReadQRCode(connectionCode);

        mockClientProxy.Verify(
            c => c.SendCoreAsync("ResultReadQRCode",
                It.Is<object[]>(response => response != null 
                && response.Length == 1 
                && (response.First() as ResponseUserConnectionJson).Name.Equals(userToConnect.Name)
                && (response.First() as ResponseUserConnectionJson).Id.Equals(userToConnect.Id)), 
                default), Times.Once);
    }

    [Fact]
    public async Task Validate_Unkown_Error_Failure()
    {
        (var mockHubContext,
         var mockClientProxy,
         var mockClients,
         var mockHubCallerContext) = MockWebSocketConnectionsBuilder.Build();

        var connectionCode = Guid.NewGuid().ToString();
        var userToConnect = ResponseUserConnectionJsonBuilder.Build();

        var useCaseReadQRCode = ReadQRCodeBuilder_UnkownError(userToConnect, connectionCode);
        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);

        var hub = new Socket(null, null, useCaseGenerateQRCoder, mockHubContext.Object, useCaseReadQRCode)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        await hub.ReadQRCode(connectionCode);

        mockClientProxy.Verify(
                    c => c.SendCoreAsync("Error",
                    It.Is<object[]>(response => response != null && response.Length == 1 && response.First()
                        .Equals(ResourceErrorMessages.UNKNOWN_ERROR)), default), Times.Once);
    }

    [Fact]
    public async Task Validate_RecipeBook_Exception_Failure()
    {
        (var mockHubContext,
         var mockClientProxy,
         var mockClients,
         var mockHubCallerContext) = MockWebSocketConnectionsBuilder.Build();

        var connectionCode = Guid.NewGuid().ToString();
        var userToConnect = ResponseUserConnectionJsonBuilder.Build();

        var useCaseReadQRCode = ReadQRCodeBuilder(userToConnect, connectionCode);
        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);

        var hub = new Socket(null, null, useCaseGenerateQRCoder, mockHubContext.Object, useCaseReadQRCode)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.ReadQRCode(connectionCode);

        mockClientProxy.Verify(c => c.SendCoreAsync("Error",
                    It.Is<object[]>(response => response != null && response.Length == 1 
                    && response.First().Equals(ResourceErrorMessages.USER_NOT_FOUND)), default), Times.Once);
    }

    private static IGenerateQRCodeUseCase GenerateQRCodeBuilder(string qrCode)
    {
        var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute()).ReturnsAsync((qrCode, "userId"));

        return useCaseMock.Object;
    }

    private static IReadQRCodeUseCase ReadQRCodeBuilder(ResponseUserConnectionJson response, string connectionCode)
    {
        var useCaseMock = new Mock<IReadQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute(connectionCode)).ReturnsAsync((response, "userId"));

        return useCaseMock.Object;
    }

    private static IReadQRCodeUseCase ReadQRCodeBuilder_UnkownError(ResponseUserConnectionJson response, string connectionCode)
    {
        var useCaseMock = new Mock<IReadQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute(connectionCode)).ThrowsAsync(new ArgumentNullException());

        return useCaseMock.Object;
    }
}
