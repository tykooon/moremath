using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Api.Requests.Users;
using MoreMath.Dto.Dtos;
using MoreMath.Application.UseCases.Users.Commands;
using MoreMath.Application.UseCases.Users.Queries;
using MoreMath.Shared.Result;
using System.Net;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("")]
    [ProducesResponseType<IEnumerable<UserDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetUsers(string? username)
    {
        var res = await _mediator.Send(new GetUsersQuery(username));
        return res.ToHttpResult();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetUserById(int id)
    {
        var res = await _mediator.Send(new GetUserByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateUser(CreateUserRequest request)
    {
        var res = await _mediator.Send(new CreateUserCommand(
            request.Username,
            request.IsActive));
        return res.ToHttpCreated($"/Users/{res.Value}");
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateUser(int id, UpdateUserRequest request)
    {
        var res = await _mediator.Send(new UpdateUserCommand(
            id,
            request.Username,
            request.IsActive));
        return res.ToHttpResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteUser(int id)
    {
        var res = await _mediator.Send(new DeleteUserCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

}

