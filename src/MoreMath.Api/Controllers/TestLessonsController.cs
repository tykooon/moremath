using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Api.Requests.Authors;
using MoreMath.Dto.Dtos;
using MoreMath.Application.UseCases.Authors.Commands;
using MoreMath.Application.UseCases.Authors.Queries;
using MoreMath.Shared.Result;
using System.Net;
using MoreMath.Api.Requests.TestLessons;
using MoreMath.Application.UseCases.TestLessons.Commands;
using MoreMath.Application.UseCases.TestLessons.Queries;
using MoreMath.Shared.Common;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestLessonsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateTestLesson(CreateTestLessonRequest request)
    {
        // TODO: UserId should be taken from the token or HttpContext and not from the request
        var res = await _mediator.Send(new CreateTestLessonOrderCommand(
            request.FullName,
            request.ContactInfo,
            request.Notes,
            request.UserId,
            request.Status));
        return res.ToHttpCreated($"/testlessons/{res.Value}");
    }

    [HttpGet("")]
    [ProducesResponseType<IEnumerable<TestLessonOrderDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetTestLessons(string? status)
    {

        TestLessonOrderStatus? statusEnum = status != null && Enum.TryParse(status, ignoreCase:true, out TestLessonOrderStatus value)
            ? value
            : null;
        if(status != null && statusEnum == null)
        {
            var failure = ResultWrap<IEnumerable<TestLessonOrderDto>>.Failure(new Error("TestLessonOrder.InvalidStatus", "Invalid status."));
            return failure.ToHttpResult();
        }

        var res = await _mediator.Send(new GetTestLessonOrdersQuery(statusEnum));
        return res.ToHttpResult();
    }


    [HttpGet("{id}")]
    [ProducesResponseType<TestLessonOrderDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTestLessonById(int id)
    {
        var res = await _mediator.Send(new GetTestLessonOrderByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateTestLesson(int id, UpdateTestLessonRequest request)
    {
        var res = await _mediator.Send(new UpdateTestLessonOrderCommand(
            id,
            request.FullName,
            request.ContactInfo,
            request.Notes,
            request.Status));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteTestLesson(int id)
    {
        var res = await _mediator.Send(new DeleteTestLessonOrderCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}
