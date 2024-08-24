using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Queries;

public record GetUsersQuery(string? Username = null) : IRequest<ResultWrap<IEnumerable<UserDto>>>;

public class GetUsersHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetUsersQuery, ResultWrap<IEnumerable<UserDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var Users = await _unitOfWork.UserRepo.GetFilteredAsync(a =>
            request.Username == null || a.Username == request.Username);
        var response = Users.Select(a => a.ToDto());
        return ResultWrap<IEnumerable<UserDto>>.Success(response);
    }
}