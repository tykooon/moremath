using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoreMath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController(IMediator mediator) : ControllerBase
    {
        protected IMediator _mediator = mediator;
    }
}
