using System.ComponentModel;

namespace Flow.Launcher.Plugin.Todos
{
    public enum TodoCommand
    {
        [Description("Add")]
        A,
        [Description("Complete")]
        C,
        [Description("Edit")]
        E,
        [Description("Help")]
        H,
        [Description("List")]
        L,
        [Description("Reload")]
        Rl,
        [Description("Remove")]
        R,
        [Description("Sort")]
        S,
        [Description("Uncheck")]
        U
    }
}
