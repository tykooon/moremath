using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Dtos;
using MoreMath.Application.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorByIdQuery(int id) : IRequest<ResultWrap<AuthorDto?>>;

public class GetAuthorByIdHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetAuthorByIdQuery, ResultWrap<AuthorDto?>>(unitOfWork)
{

    public override async Task<ResultWrap<AuthorDto?>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.AuthorRepo.FindAsync(request.id);

        return author == null
            ? ResultWrap<AuthorDto?>.Failure(new Error("Author.NotFound", "Failed to get author with given Id."))
            : ResultWrap<AuthorDto?>.Success(author.ToDto());
    }
}
