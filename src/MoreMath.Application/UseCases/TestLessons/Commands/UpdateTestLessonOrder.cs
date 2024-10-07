using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Common;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Commands;

public record UpdateTestLessonOrderCommand(
    int Id,
    string? FullName,
    string? ContactInfo,
    string? Notes,
    TestLessonOrderStatus? Status
) : IRequest<ResultWrap>;

public class UpdateTestLessonOrderHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<UpdateTestLessonOrderCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(UpdateTestLessonOrderCommand command, CancellationToken cancellationToken)
    {
        var testLessonOrder = await _unitOfWork.TestLessonRepo.FindAsync(command.Id);

        if (testLessonOrder == null)
        {
            return ResultWrap.Failure(new Error("TestLessonOrder.NotFound", "Failed to update Test Lesson Order with given Id. Order wasn't found."));
        }

        testLessonOrder.FullName = command.FullName ?? testLessonOrder.FullName;
        testLessonOrder.ContactInfo = command.ContactInfo ?? testLessonOrder.ContactInfo;
        testLessonOrder.Notes = command.Notes ?? testLessonOrder.Notes;
        testLessonOrder.Status = command.Status ?? testLessonOrder.Status;

        _unitOfWork.TestLessonRepo.Update(testLessonOrder);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
