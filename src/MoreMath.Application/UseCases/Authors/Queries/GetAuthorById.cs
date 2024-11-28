using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorByIdQuery(int Id) : IRequest<ResultWrap<AuthorDto?>>;

public class GetAuthorByIdHandler(IAppDbContext context):
    AbstractHandler<GetAuthorByIdQuery, ResultWrap<AuthorDto?>>(context)
{

    public override async Task<ResultWrap<AuthorDto?>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.AsNoTracking().Where(a => a.Id.Equals(request.Id)).FirstOrDefaultAsync(cancellationToken);

        return author == null
            ? ResultWrap<AuthorDto?>.Failure(new Error("Author.NotFound", "Failed to get author with given Id."))
            : ResultWrap<AuthorDto?>.Success(author.ToDto());
    }
}
