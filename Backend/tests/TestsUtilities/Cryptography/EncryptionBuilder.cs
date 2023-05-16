using RecipeBook.Application.Services.Cryptography;

namespace TestsUtilities.Cryptography;

public class EncryptionBuilder
{
    public static Encryption Instance()
    {
        return new Encryption("ABCD123");
    }
}
