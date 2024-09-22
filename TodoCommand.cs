using System.ComponentModel;

namespace Flow.Launcher.Plugin.Todos
{
    public enum TodoCommand
    {
        [Description("Add")]
        A,
        [Description("Complete")]
        C,
        [Description("Edit")] // TODO  
        E,
        [Description("Help")]
        H,
        [Description("List")]
        L,
        [Description("Reload")]
        Rl,
        [Description("Remove")]
        R,
        [Description("Uncheck")]
        U
    }
}
