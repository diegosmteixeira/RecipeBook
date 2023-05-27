using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Dashboard;
public interface IDashboardUseCase
{
    Task<ResponseDashboardJson> Execute(RequestDashboardJson request);
}
