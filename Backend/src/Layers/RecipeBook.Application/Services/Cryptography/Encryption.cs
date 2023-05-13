using System.Security.Cryptography;
using System.Text;

namespace RecipeBook.Application.Services.Cryptography;

public class Encryption
{
    private readonly string _key;

    public Encryption(string key)
    {
        _key = key;
        
    }
    public string PasswordEncryption(string password)
    {
        var passwordKey = $"{password}{_key}";

        var bytes = Encoding.UTF8.GetBytes(passwordKey);
        var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(bytes);

        return StringBytes(hashBytes);
    }

    public static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
