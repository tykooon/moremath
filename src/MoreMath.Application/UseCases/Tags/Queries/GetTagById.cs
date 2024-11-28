using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Queries;

public record GetTagByIdQuery(int Id): IRequest<ResultWrap<TagDto?>>;

public class GetTagByIdHandler(IAppDbContext context) :
    AbstractHandler<GetTagByIdQuery, ResultWrap<TagDto?>>(context)
{
    public override async Task<ResultWrap<TagDto?>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync([query.Id], cancellationToken);
        return tag == null
            ? ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id was not found"))
            : ResultWrap<TagDto?>.Success(tag.ToDto());
    }
}