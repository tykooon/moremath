using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Dtos;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorsQuery() : IRequest<Result<IEnumerable<AuthorDto>>>;

public class GetAuthorsHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetAuthorsQuery, Result<IEnumerable<AuthorDto>>>(unitOfWork)
{

    public override async Task<Result<IEnumerable<AuthorDto>>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.AuthorRepo.GetAllAsync();
        var response = authors.Select(a => new AuthorDto(a.Id, a.FirstName, a.LastName, a.Avatar?.ToString(), a.Info, a.ShortBio, a.Created, a.Modified));
        return Result<IEnumerable<AuthorDto>>.Success(response);
    }
}
