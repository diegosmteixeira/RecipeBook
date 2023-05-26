using Moq;
using RecipeBook.Domain.Repositories.Recipe;

namespace TestsUtilities.Repositories;
public class RecipeWriteOnlyRepositoryBuilder
{
    private static RecipeWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IRecipeWriteOnlyRepository> _repository;
    public RecipeWriteOnlyRepositoryBuilder()
    {
        if (_repository is null)
        {
            _repository = new Mock<IRecipeWriteOnlyRepository>();
        }
    }

    public static RecipeWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new RecipeWriteOnlyRepositoryBuilder();
        return _instance;
    }
    
    public IRecipeWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}
