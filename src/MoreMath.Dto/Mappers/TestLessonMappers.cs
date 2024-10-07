using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class TestLessonMappers
{
    public static TestLessonOrderDto ToDto(this TestLessonOrder order) => new(
        order.Id,
        order.FullName,
        order.ContactInfo,
        order.Notes,
        order.UserId ?? 0,
        order.Status,
        order.Created,
        order.Modified);

}
