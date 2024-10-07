using MoreMath.Core.Abstracts;
using MoreMath.Shared.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMath.Core.Entities;

public class TestLessonOrder: EntityWithDates<int>
{
    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string ContactInfo { get; set; } = "";

    public string Notes { get; set; } = "";

    public User? User { get; set; }

    [ForeignKey("User")]
    public int? UserId { get; set; }

    public TestLessonOrderStatus Status { get; set; } = TestLessonOrderStatus.New;
}
