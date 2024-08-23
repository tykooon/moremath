using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Authentication.ApiKey;
using MoreMath.Api.Extensions;
using MoreMath.Api.Requests.Categories;
using MoreMath.Application.UseCases.Categories.Commands;
using MoreMath.Application.UseCases.Categories.Queries;
using MoreMath.Dto.Dtos;
using MoreMath.Shared.Result;
using System.Net;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[ApiKey]
public class CategoriesController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("")]
    [ProducesResponseType<IEnumerable<CategoryDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetCategories(string? searchString)
    {
        var res = await _mediator.Send(new GetCategoriesQuery(searchString));
        return res.ToHttpResult();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetCategoryById(int id)
    {
        var res = await _mediator.Send(new GetCategoryByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateCategory(CreateCategoryRequest request)
    {
        var res = await _mediator.Send(new CreateCategoryCommand(request.CategoryName,request.Description ));
        return res.ToHttpCreated($"/categoties/{res.Value}");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteCategory(int id)
    {
        var res = await _mediator.Send(new DeleteCategoryCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

}
