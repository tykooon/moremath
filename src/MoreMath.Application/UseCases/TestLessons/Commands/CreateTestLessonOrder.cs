using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MediatR;
using MoreMath.Core.Entities;
using MoreMath.Shared.Common;

namespace MoreMath.Application.UseCases.TestLessons.Commands;

public record CreateTestLessonOrderCommand(
    string FullName,
    string ContactInfo,
    string? Notes,
    int? UserId,
    TestLessonOrderStatus Status) : IRequest<ResultWrap<int>>;


public class CreateTestLessonOrderHandler(IAppDbContext context) :
    AbstractHandler<CreateTestLessonOrderCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(CreateTestLessonOrderCommand command, CancellationToken cancellationToken)
    {
        var testLessonOrder = new TestLessonOrder
        {
            FullName = command.FullName,
            ContactInfo = command.ContactInfo,
            Notes = command.Notes ?? "",
            Status = command.Status
        };

        if (command.UserId.HasValue)
        {
            var user = await _context.Users.FindAsync([command.UserId.Value], cancellationToken);
            testLessonOrder.User = user;
        }

        await _context.TestLessonOrders.AddAsync(testLessonOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return testLessonOrder.Id == 0
            ? ResultWrap.Failure(new Error("TestLesson.Create", "Test Lesson was not created"))
            : ResultWrap<int>.Success(testLessonOrder.Id);
    }
}

