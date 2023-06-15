using Microsoft.AspNetCore.SignalR;
using Moq;
using RecipeBook.API.WebSockets;

namespace WebApi.Test.V1.Connection.Builder;
public class MockWebSocketConnectionsBuilder
{
    public static 
        (Mock<IHubContext<Socket>> mockHubContext, 
        Mock<IClientProxy> mockClientProxy, 
        Mock<IHubCallerClients> mockClients,
        Mock<HubCallerContext> mockHubCallerContext) Build()
    {
        var mockClientProxy = new Mock<IClientProxy>();

        var mockClients = new Mock<IHubCallerClients>();
        mockClients.Setup(c => c.Caller).Returns(mockClientProxy.Object);
        mockClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockClientProxy.Object);

        var mockHubCallerContext = new Mock<HubCallerContext>();
        mockHubCallerContext.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString());

        var mockHubClients = new Mock<IHubClients>();
        mockHubClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockClientProxy.Object);

        var mockHubContext = new Mock<IHubContext<Socket>>();
        mockHubContext.Setup(c => c.Clients).Returns(mockHubClients.Object);

        return (mockHubContext, mockClientProxy, mockClients, mockHubCallerContext);
    }
}
