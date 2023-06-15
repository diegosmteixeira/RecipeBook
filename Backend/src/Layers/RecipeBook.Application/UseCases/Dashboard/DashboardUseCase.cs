using AutoMapper;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Communication.Request;
using RecipeBook.Communication.Response;

using RecipeBook.Domain.Extension;
using RecipeBook.Domain.Repositories.Connection;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Application.UseCases.Dashboard;
public class DashboardUseCase : IDashboardUseCase
{

    private readonly IConnectionReadOnlyRepository _connRepository;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IUserLogged _userLogged;
    private readonly IMapper _mapper;
    public DashboardUseCase(IConnectionReadOnlyRepository connRepository,
                            IRecipeReadOnlyRepository repository,
                            IUserLogged userLogged,
                            IMapper mapper)
    {
        _connRepository = connRepository;
        _repository = repository;
        _userLogged = userLogged;
        _mapper = mapper;
    }

    public async Task<ResponseDashboardJson> Execute(RequestDashboardJson request)
    {
        var userLogged = await _userLogged.UserRecovery();

        var recipes = await _repository.RecipeRecovery(userLogged.Id);

        recipes = Filter(request, recipes);

        var recipeUsersConnectedWith = await RecipesUsersConnected(request, userLogged);

        recipes = recipes.Concat(recipeUsersConnectedWith).ToList();

        return new ResponseDashboardJson
        {
            Recipes = _mapper.Map<List<ResponseRecipeDashboardJson>>(recipes)
        };
    }

    private async Task<IList<Domain.Entities.Recipe>> RecipesUsersConnected(RequestDashboardJson request, Domain.Entities.User userLogged)
    {
        var connections = await _connRepository.RecoverConnections(userLogged.Id);

        var connectedUsers = connections.Select(u => u.Id).ToList();

        var recipeUsersConnectedWith = await _repository.RecipeRecoveryUsersConnectedWith(connectedUsers);

        return Filter(request, recipeUsersConnectedWith);
    }

    private static IList<Domain.Entities.Recipe> Filter(RequestDashboardJson request, IList<Domain.Entities.Recipe> recipes)
    {
        if (recipes is null)
            return new List<Domain.Entities.Recipe>();

        var filtredRecipes = recipes;

        if (request.Category.HasValue)
        {
            recipes = recipes.Where(r => r.Category == (Domain.Enum.Category)request.Category.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.TitleOrIngredient))
        {

            /*filtredRecipes = recipes.Where(r => r.Title.Contains(request.TitleOrIngredient) || 
                r.Ingredients.Any(ingredient => ingredient.Name.Contains(request.TitleOrIngredient))).ToList();*/
            
            filtredRecipes = recipes.Where(r => r.Title.CustomComparer(request.TitleOrIngredient) || 
                r.Ingredients.Any(ingredient => ingredient.Name.CustomComparer(request.TitleOrIngredient))).ToList();
        }

        return filtredRecipes.OrderBy(r => r.Title).ToList();
    }
}
