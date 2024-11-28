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

public class UpdateTestLessonOrderHandler(IAppDbContext context) :
    AbstractHandler<UpdateTestLessonOrderCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(UpdateTestLessonOrderCommand command, CancellationToken cancellationToken)
    {
        var testLessonOrder = await _context.TestLessonOrders.FindAsync(command.Id);

        if (testLessonOrder == null)
        {
            return ResultWrap.Failure(new Error("TestLessonOrder.NotFound", "Failed to update Test Lesson Order with given Id. Order wasn't found."));
        }

        testLessonOrder.FullName = command.FullName ?? testLessonOrder.FullName;
        testLessonOrder.ContactInfo = command.ContactInfo ?? testLessonOrder.ContactInfo;
        testLessonOrder.Notes = command.Notes ?? testLessonOrder.Notes;
        testLessonOrder.Status = command.Status ?? testLessonOrder.Status;

        testLessonOrder.UpdateTimeMark();        
        _context.TestLessonOrders.Update(testLessonOrder);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
