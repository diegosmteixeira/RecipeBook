using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Code;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace RecipeBook.Infrastructure.Repository;
public class CodeRepository : ICodeWriteOnlyRepository, ICodeReadOnlyRepository
{
    private readonly RecipeBookContext _context;
    public CodeRepository(RecipeBookContext context)
    {
        _context = context;
    }

    public async Task Delete(long userId)
    {
        var codes = await _context.Codes.Where(c => c.UserId == userId).ToListAsync();

        if (codes.Any())
        {
            _context.Codes.RemoveRange(codes);
        }
    }

    public async Task<Code> RecoverEntityCode(string code)
    {
        return await _context.Codes.AsNoTracking().FirstOrDefaultAsync(c => c.CodeId == code);
    }

    public async Task Register(Code code)
    {
        var codeDB = await _context.Codes.FirstOrDefaultAsync(c => c.UserId == code.UserId);

        if (codeDB is not null) 
        {
            codeDB.CodeId = code.CodeId;
            _context.Codes.Update(codeDB);
        }
        else
        {
            await _context.Codes.AddAsync(code);
        }
    }
}
