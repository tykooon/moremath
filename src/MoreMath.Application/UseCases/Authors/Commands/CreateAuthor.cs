using FluentValidation;
using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    string AvatarUri,
    string Info,
    string ShortBio) : IRequest<Result<int>>;

public class  CreateAuthorHandler: IRequestHandler<CreateAuthorCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateAuthorCommand> _validator;

    public CreateAuthorHandler(IUnitOfWork unitOfWork, IValidator<CreateAuthorCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<int>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        //var res = await _validator.ValidateAsync(command, cancellationToken);
        //if(!res.IsValid)
        //{
        //    var result = Result.Failure(new Error("Author.Create.Validation"));
        //    foreach(var err in res.Errors)
        //    {
        //        result.AppendError(new Error(err.ErrorCode, err.ErrorMessage));
        //    }
        //    return result;
        //}
         
        Author author = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Avatar = new Uri(command.AvatarUri),
            Info = command.Info,
            ShortBio = command.ShortBio
        };

        await _unitOfWork.AuthorRepo.AddAsync(author);
        await _unitOfWork.CommitAsync();
        return author.Id == 0
            ? Result.Failure(new("Author.Create", "Author was not created"))
            : Result<int>.Success(author.Id);
    }
}
