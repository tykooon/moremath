using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Common;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Queries;

public record GetTestLessonOrdersQuery(TestLessonOrderStatus? Status = null) : IRequest<ResultWrap<IEnumerable<TestLessonOrderDto>>>;

public class GetTestLessonOrdersHandler(IAppDbContext context) :
    AbstractHandler<GetTestLessonOrdersQuery, ResultWrap<IEnumerable<TestLessonOrderDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<TestLessonOrderDto>>> Handle(GetTestLessonOrdersQuery request, CancellationToken cancellationToken)
    {
        var testLessonOrders = await _context.TestLessonOrders.Where(a =>
            request.Status == null || a.Status == request.Status).Select(a => a.ToDto()).ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<TestLessonOrderDto>>.Success(testLessonOrders);
    }
}