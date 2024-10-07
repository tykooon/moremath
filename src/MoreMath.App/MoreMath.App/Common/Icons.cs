using Microsoft.AspNetCore.Components;
using MoreMath.App.Components.Inline;

namespace MoreMath.App.Common;

public static class Icons
{
    public static RenderFragment Bagrut => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "school");
        builder.CloseComponent();
    };

    public static RenderFragment QuickHelp => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "alarm-light-outline");
        builder.CloseComponent();
    };

    public static RenderFragment Olympics => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "trophy");
        builder.CloseComponent();
    };

    public static RenderFragment Mathematics => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "square-root");
        builder.CloseComponent();
    };

    public static RenderFragment MathHebrew => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "abjad-hebrew");
        builder.CloseComponent();
    };

    public static RenderFragment Blog => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "feather");
        builder.CloseComponent();
    };

    public static RenderFragment Team => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "account-group");
        builder.CloseComponent();
    };

    public static RenderFragment Contacts => (builder) =>
    {
        builder.OpenComponent(0, typeof(MdIcon));
        builder.AddAttribute(1, "Name", "email-fast-outline");
        builder.CloseComponent();
    };
}
