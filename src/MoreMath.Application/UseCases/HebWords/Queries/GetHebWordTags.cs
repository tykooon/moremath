using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.HebWords.Queries;

public record GetHebWordTagsQuery(int id) : IRequest<ResultWrap<IEnumerable<TagDto>>>;

public class GetHebWordTagsHandler(IAppDbContext context):
    AbstractHandler<GetHebWordTagsQuery, ResultWrap<IEnumerable<TagDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<TagDto>>> Handle(GetHebWordTagsQuery request, CancellationToken cancellationToken)
    {
        var HebWord = await _context.HebWords.Include(h => h.Tags).FirstOrDefaultAsync(hw => hw.Id.Equals(request.id), cancellationToken);

        return HebWord == null
            ? ResultWrap.Failure(new Error("HebWord.NotFound", "Failed to get HebWord with given Id."))
            : ResultWrap<IEnumerable<TagDto>>.Success(HebWord.Tags.Select(a => a.ToDto()));
    }
}
