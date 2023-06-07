using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Code;

namespace TestsUtilities.Repositories;
public class CodeReadOnlyRepositoryBuilder
{
    private static CodeReadOnlyRepositoryBuilder _instance;
    private readonly Mock<ICodeReadOnlyRepository> _repository;

    private CodeReadOnlyRepositoryBuilder()
    {
        if (_repository is null)
            _repository = new Mock<ICodeReadOnlyRepository>();
    }

    public static CodeReadOnlyRepositoryBuilder Instance()
    {
        _instance = new CodeReadOnlyRepositoryBuilder();
        return _instance;
    }

    public CodeReadOnlyRepositoryBuilder RecoverEntityCode(Code code) 
    {
        _repository.Setup(x => x.RecoverEntityCode(code.CodeId)).ReturnsAsync(code);
        return this;
    }

    public ICodeReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
