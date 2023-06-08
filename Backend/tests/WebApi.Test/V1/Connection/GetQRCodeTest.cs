using Moq;
using RecipeBook.API.WebSockets;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using WebApi.Test.V1.Connection.Builder;
using Xunit;

namespace WebApi.Test.V1.Connection;
public class GetQRCodeTest
{
    [Fact]
    public async Task Validate_Success()
    {
        (var mockHubContext, 
         var mockClientProxy, 
         var mockClients, 
         var mockHubCallerContext) = MockWebSocketConnectionsBuilder.Build();

        var qrCode = Guid.NewGuid().ToString();

        var useCaseGenerateQRCodeBuilder = GenerateQRCodeBuilder(qrCode);

        var hub = new Socket(null, null, useCaseGenerateQRCodeBuilder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        // 1 - function
        // 2 - how many times

        mockClientProxy.Verify(
            c => c.SendCoreAsync("QR Code",
            It.Is<object[]>(response => response != null && response.Length == 1 && response.First().Equals(qrCode)), 
            default), Times.Once);
    }

    [Fact]
    public async Task Validate_Unkown_Error_Failure()
    {
        (var mockHubContext,
         var mockClientProxy,
         var mockClients,
         var mockHubCallerContext) = MockWebSocketConnectionsBuilder.Build();

        var qrCode = Guid.NewGuid().ToString();

        var useCaseGenerateQRCodeBuilder = GenerateQRCodeUnkownErrorBuilder();

        var hub = new Socket(null, null, useCaseGenerateQRCodeBuilder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

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

        var qrCode = Guid.NewGuid().ToString();

        var useCaseGenerateQRCodeBuilder = GenerateQRRecipeBookExceptionBuilder(ResourceErrorMessages.WITHOUT_PERMISSION);

        var hub = new Socket(null, null, useCaseGenerateQRCodeBuilder, mockHubContext.Object, null)
        {
            Context = mockHubCallerContext.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        mockClientProxy.Verify(
            c => c.SendCoreAsync("Error",
            It.Is<object[]>(response => response != null && response.Length == 1 && response.First()
                .Equals(ResourceErrorMessages.WITHOUT_PERMISSION)), default), Times.Once);
    }

    private static IGenerateQRCodeUseCase GenerateQRCodeBuilder(string qrCode)
    {
        var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute()).ReturnsAsync((qrCode ,"userId"));

        return useCaseMock.Object;
    }
    
    private static IGenerateQRCodeUseCase GenerateQRCodeUnkownErrorBuilder()
    {
        var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute()).ThrowsAsync(new ArgumentNullException());

        return useCaseMock.Object;
    }

    private static IGenerateQRCodeUseCase GenerateQRRecipeBookExceptionBuilder(string message)
    {
        var useCaseMock = new Mock<IGenerateQRCodeUseCase>();

        useCaseMock.Setup(u => u.Execute()).ThrowsAsync(new RecipeBookException(message));

        return useCaseMock.Object;
    }
}
