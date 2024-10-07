using MoreMath.Shared.Common;

namespace MoreMath.Api.Requests.TestLessons;

public record CreateTestLessonRequest(
    string FullName,
    string ContactInfo,
    string? Notes,
    int? UserId,
    TestLessonOrderStatus Status );