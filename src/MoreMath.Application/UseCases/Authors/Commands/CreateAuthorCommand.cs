using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Requests;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record CreateAuthorCommand(CreateAuthorRequest Request): IRequest<Result<int>>;

public class  CreateAuthorCommandHandler: IRequestHandler<CreateAuthorCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAuthorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author author = new()
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Avatar = new Uri(request.Request.AvatarUri),
            Info = request.Request.Info,
            ShortBio = request.Request.ShortBio
        };

        await _unitOfWork.AuthorRepo.AddAsync(author);
        await _unitOfWork.CommitAsync();
        return author.Id == 0
            ? Result.Failure(new("Author.Create", "Author was not created"))
            : Result<int>.Success(author.Id);
    }
}
