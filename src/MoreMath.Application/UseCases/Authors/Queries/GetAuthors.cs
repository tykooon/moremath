using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;


namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorsQuery(string? FirstName = null, string? LastName = null, string? SlugName = null) : IRequest<ResultWrap<IEnumerable<AuthorDto>>>;

public class GetAuthorsHandler(IAppDbContext context):
    AbstractHandler<GetAuthorsQuery, ResultWrap<IEnumerable<AuthorDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<AuthorDto>>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _context.Authors.Where(a =>
            (request.FirstName == null || a.FirstName == request.FirstName) &&
            (request.LastName == null || a.LastName == request.LastName) &&
            (request.SlugName == null || a.SlugName == request.SlugName)).ToListAsync(cancellationToken);

        var response = authors.Select(a => a.ToDto());
        return ResultWrap<IEnumerable<AuthorDto>>.Success(response);
    }
}
