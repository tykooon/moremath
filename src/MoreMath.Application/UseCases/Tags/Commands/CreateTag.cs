using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Commands;

public record CreateTagCommand(string TagName): IRequest<ResultWrap<int>>;

public class CreateTagHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<CreateTagCommand, ResultWrap<int>>(unitOfWork)
{
    public override async Task<ResultWrap<int>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = (await _unitOfWork.TagRepo.GetFilteredAsync(x => x.TagName == command.TagName)).FirstOrDefault();
        if(tag != null)
        {
            return ResultWrap.Failure(Error.Validation(
                typeof(CreateTagCommand).Name,
                nameof(command.TagName),
                "Tag with provided tagName already exists."));
        }

        tag = new Tag()
        {
            TagName = command.TagName
        };

        await _unitOfWork.TagRepo.AddAsync(tag);
        await _unitOfWork.CommitAsync();
        return tag.Id == 0
            ? ResultWrap.Failure(new Error("Tag.CreateError", "Error while processing tag creation"))
            : ResultWrap<int>.Success(tag.Id);
    }
}
