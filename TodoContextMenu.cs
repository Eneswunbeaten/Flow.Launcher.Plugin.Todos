using System.Collections.Generic;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.Todos
{
    public static class TodoContextMenu
    {
        public static List<Result> GetMenuOptions(Todos todos)
        {
            return new List<Result>
            {
                new Result
                {
                    Title = "🧹 Clear completed todos",
                    SubTitle = "Remove all completed todos",
                    IcoPath = todos.GetFilePath("ico/todo.png"),
                    Action = c =>
                    {
                        todos.RemoveCompleted();
                        todos.Context.API.ChangeQuery(todos.ActionKeyword + " ", true);
                        return false;
                    }
                },
                new Result
                {
                    Title = "🔄 Reload todos",
                    SubTitle = "Reload todos from data file",
                    IcoPath = todos.GetFilePath("ico/todo.png"),
                    Action = c =>
                    {
                        todos.Reload();
                        todos.Context.API.ChangeQuery(todos.ActionKeyword + " ", true);
                        return false;
                    }
                },
                new Result
                {
                    Title = "Export to Desktop",
                    SubTitle = "Export all todos as a text file to your desktop",
                    IcoPath = todos.GetFilePath("ico/app.png"),
                    Action = c =>
                    {
                        todos.ExportToDesktop();
                        return false;
                    }
                }
            };
        }
    }
}
