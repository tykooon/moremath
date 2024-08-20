using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Dtos;
using MoreMath.Application.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;


namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorsQuery(string? FirstName = null, string? LastName = null) : IRequest<ResultWrap<IEnumerable<AuthorDto>>>;

public class GetAuthorsHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetAuthorsQuery, ResultWrap<IEnumerable<AuthorDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<AuthorDto>>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.AuthorRepo.GetFilteredAsync(a =>
            (request.FirstName == null || a.FirstName == request.FirstName) &&
            (request.LastName == null || a.LastName == request.LastName));
        var response = authors.Select(a => a.ToDto());
        return ResultWrap<IEnumerable<AuthorDto>>.Success(response);
    }
}
