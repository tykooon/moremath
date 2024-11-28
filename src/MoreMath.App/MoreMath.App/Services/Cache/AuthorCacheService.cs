using MediatR;
using MoreMath.Application.UseCases.Authors.Queries;
using MoreMath.Dto.Dtos;

namespace MoreMath.App.Services.Cache;

public class AuthorCacheService(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;
    private readonly Dictionary<int, AuthorDto> _authors = [];

    public async Task<AuthorDto?> GetAuthorAsync(int id)
    {
        if (_authors.TryGetValue(id, out AuthorDto? author) && author != null )
        {
            return author;
        }
        else
        {
            var query = new GetAuthorByIdQuery(id);
            var res = await _mediator.Send(query);
            return res.IsSuccessfull ? _authors[id] = res.Value! : null;
        }
    }
}
