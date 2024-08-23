using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Queries;

public record GetCategoriesQuery(string? searchString) : IRequest<ResultWrap<IEnumerable<CategoryDto>>>;

public class GetCategoriesHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetCategoriesQuery, ResultWrap<IEnumerable<CategoryDto>>>(unitOfWork)
{
    public override async Task<ResultWrap<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepo.GetFilteredAsync(x =>
            query.searchString == null ||
            EF.Functions.Like(x.CategoryName, $"%{query.searchString}%"));

        return categories == null
            ? ResultWrap.Failure(new Error("Category.NotFound", "Category with provided Id was not found"))
            : ResultWrap<IEnumerable<CategoryDto>>.Success(categories.Select(c => c.ToDto()));
    }
}