using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record UpdateAuthorCommand(
    int Id,
    string? FirstName,
    string? LastName,
    string? SlugName,
    string? Info,
    string? ShortBio,
    string? Phone,
    string? Email,
    string? WhatsApp,
    string? Telegram,
    string? Website,
    string? Options) : IRequest<ResultWrap>;



public class  UpdateAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<UpdateAuthorCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.AuthorRepo.FindAsync(command.Id);

        if (author == null)
        {
            return ResultWrap.Failure([new Error("Author.NotFound", "Failed to update author with given Id. Author wasn't found.")]);
        }

        author.FirstName = command.FirstName ?? author.FirstName;
        author.LastName = command.LastName ?? author.LastName;
        author.SlugName = command.SlugName ?? author.SlugName;
        author.Info = command.Info ?? author.Info;
        author.ShortBio = command.ShortBio ?? author.ShortBio;
        author.Phone = command.Phone ?? author.Phone;
        author.Email = command.Email ?? author.Email;
        author.WhatsApp = command.WhatsApp ?? author.WhatsApp;
        author.Telegram = command.Telegram ?? author.Telegram;
        author.Website = command.Website ?? author.Website;
        author.Options = command.Options ?? author.Options;

        _unitOfWork.AuthorRepo.Update(author);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
