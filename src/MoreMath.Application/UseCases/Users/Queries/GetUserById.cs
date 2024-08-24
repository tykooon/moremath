using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Queries;

public record GetUserByIdQuery(int id) : IRequest<ResultWrap<UserDto?>>;

public class GetUserByIdHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetUserByIdQuery, ResultWrap<UserDto?>>(unitOfWork)
{

    public override async Task<ResultWrap<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var User = await _unitOfWork.UserRepo.FindAsync(request.id);

        return User == null
            ? ResultWrap<UserDto?>.Failure(new Error("User.NotFound", "Failed to get User with given Id."))
            : ResultWrap<UserDto?>.Success(User.ToDto());
    }
}
