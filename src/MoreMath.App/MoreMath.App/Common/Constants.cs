using Syncfusion.Blazor;

namespace MoreMath.App.Common;

public static class Constants
{
    public static class Roots
    {
        public const string Bagrut = "lessons/bagrut";
        public const string QuickHelp = "lessons/quickhelp";
        public const string Olympics = "lessons/olympics";
        public const string Mathematics = "articles/mathematics";
        public const string MathHebrew = "articles/math-hebrew";
        public const string Blog = "articles/blog";
        public const string Search = "articles/search";
        public const string News = "about/news";
        public const string Team = "about/team";
        public const string Contacts = "about/contacts";
        public const string JoinTeam = "about/team/join";
        public const string TestLesson = "lessons/letstry";
        public const string AlexTykoun = "about/team/alextykoun";
        public const string OlgaPindrik = "about/team/olgapindrik";
    }

    public static readonly List<MediaBreakpoint> MediaBreakPoints = 
        [   
            new MediaBreakpoint() { Breakpoint = "Xs", MediaQuery = "(max-width: 320px)" },
            new MediaBreakpoint() { Breakpoint = "Xss", MediaQuery = "(min-width: 375px) and  (max-width: 575)" },
            new MediaBreakpoint() { Breakpoint = "Sm", MediaQuery = "(min-width: 576px) and  (max-width: 767)" },
            new MediaBreakpoint() { Breakpoint = "Md", MediaQuery = "(min-width: 768px) and  (max-width: 1023px)" },
            new MediaBreakpoint() { Breakpoint = "Lg", MediaQuery = "(min-width: 1024px) and (max-width: 1279px)" },
            new MediaBreakpoint() { Breakpoint = "Xl", MediaQuery = "(min-width: 1280px)" }
         ];

public static string BLOB_STORAGE { get; set; } = "https://stmoremathdev001.blob.core.windows.net/images";
}