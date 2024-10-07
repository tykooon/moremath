using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Queries;

public record GetTestLessonOrderByIdQuery(int id) : IRequest<ResultWrap<TestLessonOrderDto?>>;

public class GetTestLessonOrderByIdHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetTestLessonOrderByIdQuery, ResultWrap<TestLessonOrderDto?>>(unitOfWork)
{

    public override async Task<ResultWrap<TestLessonOrderDto?>> Handle(GetTestLessonOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var testLessonOrder = await _unitOfWork.TestLessonRepo.FindAsync(request.id);

        return testLessonOrder == null
            ? ResultWrap<TestLessonOrderDto?>.Failure(new Error("TestLessonOrder.NotFound", "Failed to get Test Lesson Order with given Id."))
            : ResultWrap<TestLessonOrderDto?>.Success(testLessonOrder.ToDto());
    }
}
