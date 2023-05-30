using System.Globalization;
using System.Text;

namespace RecipeBook.Domain.Extension;
public static class StringExtension
{
    public static bool CustomComparer(this string source, string search)
    {
        var index = CultureInfo.CurrentCulture.CompareInfo
            .IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

        return index >= 0;
    }

    public static string AccentRemover(this string source)
    {
        return new string(source.Normalize(NormalizationForm.FormD)
            .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}
