using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Queries;

public record GetUsersQuery(string? Username = null) : IRequest<ResultWrap<IEnumerable<UserDto>>>;

public class GetUsersHandler(IAppDbContext context) :
    AbstractHandler<GetUsersQuery, ResultWrap<IEnumerable<UserDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(a => request.Username == null || a.Username == request.Username)
            .Select(a => a.ToDto())
            .ToListAsync(cancellationToken);
        return ResultWrap<IEnumerable<UserDto>>.Success(users);
    }
}