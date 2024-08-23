using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Commands;

public record DeleteTagCommand(int Id) : IRequest<ResultWrap>;

public class DeleteTagHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<DeleteTagCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _unitOfWork.TagRepo.FindAsync(command.Id);
        if (tag == null)
        {
            return ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id doesn't exist."));
        }

        _unitOfWork.TagRepo.Delete(tag);
        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
