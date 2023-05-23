using RecipeBook.Exception;
using System.Globalization;

namespace RecipeBook.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IList<string> _locales = new List<string>
    {
        "pt",
        "en"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var culture = new CultureInfo("pt");

        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = context.Request.Headers["Accept-Language"].ToString();

            if (_locales.Any(l => l.Equals(language)))
            {
                culture = new CultureInfo(language);
            }
        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
