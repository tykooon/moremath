using MediatR;
using MoreMath.Application.Contracts;

namespace MoreMath.Application.UseCases.Abstracts;

public abstract class AbstractHandler<TRequest, TResponse>(IAppDbContext context) : 
    IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected readonly IAppDbContext _context = context;

    public abstract Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken);
}
