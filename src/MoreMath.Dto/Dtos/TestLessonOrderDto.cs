using MoreMath.Shared.Common;

namespace MoreMath.Dto.Dtos;

public record TestLessonOrderDto(
    int Id,
    string FullName,
    string ContactInfo,
    string? Notes,
    int UserId,
    TestLessonOrderStatus Status,
    DateTime Created,
    DateTime Modified);
