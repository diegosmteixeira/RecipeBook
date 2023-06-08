using Moq;
using RecipeBook.API.WebSockets;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Application.UseCases.Connection.RefuseConnection;
using RecipeBook.Communication.Response;
using RecipeBook.Exception;
using TestsUtilities.Responses;
using WebApi.Test.V1.Connection.Builder;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class RefuseConnectionTest
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
        var useCaseRefuseConnection = RefuseConnectionBuilder();

        var hub = new Socket(useCaseRefuseConnection, null, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        await hub.RefuseConnection();

        mockClientProxy.Verify(c => c.SendCoreAsync("OnConnectionRefused", 
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
        var useCaseRefuseConnection = RefuseConnectionBuilder_UnkownError();

        var hub = new Socket(useCaseRefuseConnection, null, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        await hub.RefuseConnection();

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
        var useCaseRefuseConnection = RefuseConnectionBuilder();

        var hub = new Socket(useCaseRefuseConnection, null, useCaseGenerateQRCoder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.RefuseConnection();

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

    private static IRefuseConnectionUseCase RefuseConnectionBuilder()
    {
        var useCaseMock = new Mock<IRefuseConnectionUseCase>();

        useCaseMock.Setup(u => u.Execute()).ReturnsAsync(("userId"));

        return useCaseMock.Object;
    }

    private static IRefuseConnectionUseCase RefuseConnectionBuilder_UnkownError()
    {
        var useCaseMock = new Mock<IRefuseConnectionUseCase>();

        useCaseMock.Setup(u => u.Execute()).ThrowsAsync(new ArgumentNullException());

        return useCaseMock.Object;
    }
}
