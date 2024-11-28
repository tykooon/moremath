using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;
using MoreMath.Dto.Responses;

namespace MoreMath.Application.UseCases.HebWords.Queries;

public record GetHebWordByIdQuery(int Id) : IRequest<ResultWrap<HebWordDto?>>;

public class GetHebWordByIdHandler(IAppDbContext context):
    AbstractHandler<GetHebWordByIdQuery, ResultWrap<HebWordDto?>>(context)
{

    public override async Task<ResultWrap<HebWordDto?>> Handle(GetHebWordByIdQuery request, CancellationToken cancellationToken)
    {
        var HebWord = await _context.HebWords.Include(h => h.Tags).FirstOrDefaultAsync(hw => hw.Id.Equals(request.Id), cancellationToken);

        return HebWord == null
            ? ResultWrap<HebWordDto?>.Failure(new Error("HebWord.NotFound", "Failed to get HebWord with given Id."))
            : ResultWrap<HebWordDto?>.Success(HebWord.ToDto());
    }
}
