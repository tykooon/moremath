using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Queries;

public record GetTagsQuery(string? searchString) : IRequest<ResultWrap<IEnumerable<TagDto>>>;

public class GetTagsHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetTagsQuery, ResultWrap<IEnumerable<TagDto>>>(unitOfWork)
{
    public override async Task<ResultWrap<IEnumerable<TagDto>>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var tags = await _unitOfWork.TagRepo.GetFilteredAsync(x =>
            query.searchString == null ||
            EF.Functions.Like(x.TagName, $"%{query.searchString}%"));

        return tags == null
            ? ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id was not found"))
            : ResultWrap<IEnumerable<TagDto>>.Success(tags.Select(t => t.ToDto()));
    }
}