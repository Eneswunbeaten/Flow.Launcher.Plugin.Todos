using System.Collections.Generic;

namespace Flow.Launcher.Plugin.Todos
{
    public class Help
    {
        private readonly Todos _todos;

        public Help(Todos todos)
        {
            _todos = todos;
        }

        public List<Result> Show => new List<Result>
        {
            new Result
            {
                Title = $"{_todos.ActionKeyword}",
                SubTitle = "List all todos",
                IcoPath = _todos.GetFilePath("ico/app.png")
            },
            new Result
            {
                Title = $"{_todos.ActionKeyword} <text>",
                SubTitle = "Add new todo or search existing todos",
                IcoPath = _todos.GetFilePath("ico/app.png")
            },
            new Result
            {
                Title = $"{_todos.ActionKeyword} /",
                SubTitle = "Show menu options",
                IcoPath = _todos.GetFilePath("ico/app.png")
            },
            new Result
            {
                Title = "Todo Item Actions:",
                SubTitle = "Click: Toggle completion | Right-click: Show menu",
                IcoPath = _todos.GetFilePath("ico/app.png")
            },
            new Result
            {
                Title = "Export to Desktop",
                SubTitle = "Export all todos as a text file to your desktop",
                IcoPath = _todos.GetFilePath("ico/app.png"),
                Action = c => { _todos.ExportToDesktop(); return false; }
            }
        };
    }
}