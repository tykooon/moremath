using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Queries;

public record GetTestLessonOrderByIdQuery(int id) : IRequest<ResultWrap<TestLessonOrderDto?>>;

public class GetTestLessonOrderByIdHandler(IAppDbContext context) :
    AbstractHandler<GetTestLessonOrderByIdQuery, ResultWrap<TestLessonOrderDto?>>(context)
{

    public override async Task<ResultWrap<TestLessonOrderDto?>> Handle(GetTestLessonOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var testLessonOrder = await _context.TestLessonOrders.FindAsync([request.id], cancellationToken);

        return testLessonOrder == null
            ? ResultWrap<TestLessonOrderDto?>.Failure(new Error("TestLessonOrder.NotFound", "Failed to get Test Lesson Order with given Id."))
            : ResultWrap<TestLessonOrderDto?>.Success(testLessonOrder.ToDto());
    }
}
