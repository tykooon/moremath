using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Queries;

public record GetCategoriesQuery(string? searchString) : IRequest<ResultWrap<IEnumerable<CategoryDto>>>;

public class GetCategoriesHandler(IAppDbContext context) :
    AbstractHandler<GetCategoriesQuery, ResultWrap<IEnumerable<CategoryDto>>>(context)
{
    public override async Task<ResultWrap<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Where(x =>
            query.searchString == null ||
            EF.Functions.Like(x.CategoryName, $"%{query.searchString}%")).ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<CategoryDto>>.Success(categories.Select(c => c.ToDto()));
    }
}