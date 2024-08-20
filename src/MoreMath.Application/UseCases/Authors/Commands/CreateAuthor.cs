using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    string AvatarUri,
    string Info,
    string ShortBio) : IRequest<ResultWrap<int>>;



public class CreateAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<CreateAuthorCommand, ResultWrap<int>>(unitOfWork)
{
    public override async Task<ResultWrap<int>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        Author author = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Avatar = command.AvatarUri == null ? null : new Uri(command.AvatarUri, UriKind.RelativeOrAbsolute),
            Info = command.Info,
            ShortBio = command.ShortBio
        };

        await _unitOfWork.AuthorRepo.AddAsync(author);
        await _unitOfWork.CommitAsync();
        return author.Id == 0
            ? ResultWrap.Failure(new Error("Author.Create", "Author was not created"))
            : ResultWrap<int>.Success(author.Id);
    }
}
