using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Queries;

public record GetCategoryByIdQuery(int Id): IRequest<ResultWrap<CategoryDto?>>;

public class GetCategoryByIdHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetCategoryByIdQuery, ResultWrap<CategoryDto?>>(unitOfWork)
{
    public override async Task<ResultWrap<CategoryDto?>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepo.FindAsync(query.Id);
        return category == null
            ? ResultWrap.Failure(new Error("Category.NotFound", "Category with provided Id was not found"))
            : ResultWrap<CategoryDto?>.Success(category.ToDto());
    }
}