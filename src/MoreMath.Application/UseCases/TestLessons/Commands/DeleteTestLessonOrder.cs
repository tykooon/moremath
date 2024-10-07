using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.TestLessons.Commands;

public record DeleteTestLessonOrderCommand(int Id) : IRequest<ResultWrap>;

public class DeleteTestLessonOrderHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<DeleteTestLessonOrderCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteTestLessonOrderCommand command, CancellationToken cancellationToken)
    {
        var testLessonOrder = await _unitOfWork.TestLessonRepo.FindAsync(command.Id);

        if (testLessonOrder == null)
        {
            return ResultWrap.Failure(new Error("TestLessonOrder.NotFound", "Failed to delete TestLessonOrder with given Id. TestLessonOrder wasn't found."));
        }

        _unitOfWork.TestLessonRepo.Delete(testLessonOrder);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
