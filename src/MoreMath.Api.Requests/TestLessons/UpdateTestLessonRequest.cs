using MoreMath.Shared.Common;

namespace MoreMath.Api.Requests.TestLessons;

public record UpdateTestLessonRequest(
    string? FullName,
    string? ContactInfo,
    string? Notes,
    TestLessonOrderStatus? Status);