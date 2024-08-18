using MediatR;
using MoreMath.Application.Contracts;

namespace MoreMath.Application.UseCases.Abstracts;

public abstract class AbstractHandler<TRequest, TResponse>: IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IUnitOfWork _unitOfWork;

    public AbstractHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public abstract Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken);
}
