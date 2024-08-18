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
    string ShortBio) : IRequest<Result<int>>;



public class CreateAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<CreateAuthorCommand, Result<int>>(unitOfWork)
{
    public override async Task<Result<int>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        Author author = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Avatar = new Uri(command.AvatarUri, UriKind.RelativeOrAbsolute),
            Info = command.Info,
            ShortBio = command.ShortBio
        };

        await _unitOfWork.AuthorRepo.AddAsync(author);
        await _unitOfWork.CommitAsync();
        return author.Id == 0
            ? Result.Failure([new Error("Author.Create", "Author was not created")])
            : Result<int>.Success(author.Id);
    }
}
