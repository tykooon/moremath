using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Common;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Queries;

public record GetTestLessonOrdersQuery(TestLessonOrderStatus? Status = null) : IRequest<ResultWrap<IEnumerable<TestLessonOrderDto>>>;

public class GetTestLessonOrdersHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetTestLessonOrdersQuery, ResultWrap<IEnumerable<TestLessonOrderDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<TestLessonOrderDto>>> Handle(GetTestLessonOrdersQuery request, CancellationToken cancellationToken)
    {
        var testLessonOrders = await _unitOfWork.TestLessonRepo.GetFilteredAsync(a =>
            request.Status == null || a.Status == request.Status);
        var response = testLessonOrders.Select(a => a.ToDto());
        return ResultWrap<IEnumerable<TestLessonOrderDto>>.Success(response);
    }
}