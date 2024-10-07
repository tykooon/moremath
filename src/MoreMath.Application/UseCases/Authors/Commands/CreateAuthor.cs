using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    string SlugName,
    string Info,
    string ShortBio,
    string? Phone,
    string? Email,
    string? WhatsApp,
    string? Telegram,
    string? Website,
    string? Options) : IRequest<ResultWrap<int>>;



public class CreateAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<CreateAuthorCommand, ResultWrap<int>>(unitOfWork)
{
    public override async Task<ResultWrap<int>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        Author author = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            SlugName = command.SlugName,
            Info = command.Info,
            ShortBio = command.ShortBio,
            Phone = command.Phone,
            Email = command.Email,
            WhatsApp = command.WhatsApp,
            Telegram = command.Telegram,
            Website = command.Website,
            Options = command.Options
        };

        await _unitOfWork.AuthorRepo.AddAsync(author);
        await _unitOfWork.CommitAsync();
        return author.Id == 0
            ? ResultWrap.Failure(new Error("Author.Create", "Author was not created"))
            : ResultWrap<int>.Success(author.Id);
    }
}
