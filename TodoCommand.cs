using System.ComponentModel;

namespace Wox.Plugin.Todos
{
    public enum TodoCommand
    {
        [Description("Add")]
        A,
        [Description("Complete")]
        C,
        [Description("Edit")] // TODO  
        E,
        [Description("EditResult")] // TODO  
        EE,
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
