using Moq;
using RecipeBook.API.WebSockets;
using RecipeBook.Application.UseCases.Connection.AcceptConection;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Application.UseCases.Connection.RefuseConnection;
using RecipeBook.Exception;
using TestsUtilities.Responses;
using WebApi.Test.V1.Connection.Builder;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class AcceptConnectionTest
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

        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);
        var useCaseAcceptConnection = AcceptConnectionBuilder(userToConnect.Id);

        var hub = new Socket(null, useCaseAcceptConnection, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        await hub.AcceptConnection(userToConnect.Id);

        mockClientProxy.Verify(c => c.SendCoreAsync("OnConnectionAccepted",
            It.Is<object[]>(response => response != null && response.Length == 0), default), Times.Once);
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

        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);
        var useCaseAcceptConnection = AcceptConnectionBuilder_UnkownError(userToConnect.Id);

        var hub = new Socket(null, useCaseAcceptConnection, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.AcceptConnection(userToConnect.Id);

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

        var useCaseGenerateQRCoder = GenerateQRCodeBuilder(connectionCode);
        var useCaseAcceptConnection = AcceptConnectionBuilder(userToConnect.Id);

        var hub = new Socket(null, useCaseAcceptConnection, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.AcceptConnection(userToConnect.Id);

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

    private static IAcceptConnectionUseCase AcceptConnectionBuilder(string connectedWithUserId)
    {
        var useCaseMock = new Mock<IAcceptConnectionUseCase>();

        useCaseMock.Setup(u => u.Execute(connectedWithUserId)).ReturnsAsync(("userId"));

        return useCaseMock.Object;
    }

    private static IAcceptConnectionUseCase AcceptConnectionBuilder_UnkownError(string connectedWithUserId)
    {
        var useCaseMock = new Mock<IAcceptConnectionUseCase>();

        useCaseMock.Setup(u => u.Execute(connectedWithUserId)).ThrowsAsync(new ArgumentNullException());

        return useCaseMock.Object;
    }
}
