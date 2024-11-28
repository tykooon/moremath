using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Commands;

public record DeleteTagCommand(int Id) : IRequest<ResultWrap>;

public class DeleteTagHandler(IAppDbContext context) :
    AbstractHandler<DeleteTagCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(command.Id);
        if (tag == null)
        {
            return ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id doesn't exist."));
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
