using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Commands;

public record CreateTagCommand(string TagName): IRequest<ResultWrap<int>>;

public class CreateTagHandler(IAppDbContext context) :
    AbstractHandler<CreateTagCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.Where(x => x.TagName == command.TagName).FirstOrDefaultAsync(cancellationToken);
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

        await _context.Tags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return tag.Id == 0
            ? ResultWrap.Failure(new Error("Tag.CreateError", "Error while processing tag creation"))
            : ResultWrap<int>.Success(tag.Id);
    }
}
