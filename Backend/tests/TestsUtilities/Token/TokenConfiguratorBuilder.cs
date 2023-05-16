using RecipeBook.Application.Services.Token;

namespace TestsUtilities.Token;

public class TokenConfiguratorBuilder
{
    public static TokenConfigurator Instance()
    {
        return new TokenConfigurator(1000, "$2oxYFsg](5Y%yMtKI0|Zx%5V8uKal3aONdr6YPfsg`an*sU:l*el!DCm#EP;iU");
    }
}
