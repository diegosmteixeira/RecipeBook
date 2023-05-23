using RecipeBook.Application.Services.Token;

namespace TestsUtilities.Token;

public class TokenConfiguratorBuilder
{
    public static TokenConfigurator Instance()
    {
        return new TokenConfigurator(1000, "eA+sawaEtcnnq%N_1<-Sh0E'Yk4.hMyiM;fIp0;~]z\\u4ofM2j)2yYKhMbF^ti=");
    }

    public static TokenConfigurator ExpiredToken()
    {
        return new TokenConfigurator(0.0166667, "eA+sawaEtcnnq%N_1<-Sh0E'Yk4.hMyiM;fIp0;~]z\\u4ofM2j)2yYKhMbF^ti=");
    }
}

