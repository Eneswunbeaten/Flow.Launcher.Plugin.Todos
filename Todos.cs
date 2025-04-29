using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Flow.Launcher.Plugin;
using Newtonsoft.Json;

namespace Flow.Launcher.Plugin.Todos
{
    public class Todo
    {
        public string Content { get; set; }
        public bool Completed { get; set; }
        public bool Pinned { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class Todos
    {
        private const string DataFileName = @"todos.data.json";
        private string _dataFolderPath;
        private List<Todo> _todoList;

        public PluginInitContext Context { get; }
        public string ActionKeyword { get; set; }
        public List<Result> Results => ToResults(_todoList);

        public Todos(PluginInitContext context, Settings setting)
        {
            Context = context;
            ActionKeyword = context.CurrentPluginMetadata.ActionKeywords?.FirstOrDefault() ?? string.Empty;
            _dataFolderPath = setting.FolderPath;
            Load();
        }

        private void Load()
        {
            if (!Directory.Exists(_dataFolderPath))
            {
                _dataFolderPath = Context.CurrentPluginMetadata.PluginDirectory;
            }

            try
            {
                var filePath = Path.Combine(_dataFolderPath, DataFileName);
                if (File.Exists(filePath))
                {
                    var text = File.ReadAllText(filePath);
                    _todoList = JsonConvert.DeserializeObject<List<Todo>>(text) ?? new List<Todo>();
                }
                else
                {
                    _todoList = new List<Todo>();
                    Save();
                }
            }
            catch (Exception e)
            {
                Context.API.ShowMsg("Error", $"Failed to load todos: {e.Message}");
                _todoList = new List<Todo>();
            }
        }

        private void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_todoList);
                File.WriteAllText(Path.Combine(_dataFolderPath, DataFileName), json);
            }
            catch (Exception e)
            {
                Context.API.ShowMsg("Error", $"Failed to save todos: {e.Message}");
            }
        }

        public string GetFilePath(string icon = "")
        {
            return Path.Combine(Context.CurrentPluginMetadata.PluginDirectory,
                string.IsNullOrEmpty(icon) ? @"ico\app.png" : icon);
        }

        private List<Result> ToResults(IEnumerable<Todo> todos)
        {
            var results = new List<Result>();

            var sortedTodos = todos
                .OrderByDescending(t => t.Pinned)
                .ThenBy(t => t.Completed)
                .ThenByDescending(t => t.CreatedTime);

            foreach (var todo in sortedTodos)
            {
                var icon = todo.Pinned ? "📌 " : "";
                var isDone = todo.Completed;

                var result = new Result
                {
                    Title =$"{icon}{todo.Content}",
                    SubTitle = $"{(isDone ? "✓ Done" : "☐ Todo")} | {ToRelativeTime(todo.CreatedTime)}",
                    IcoPath = GetFilePath(isDone ? @"ico\done.png" : @"ico\todo.png"),
                    ContextData = todo,
                    Action = c =>
                    {
                        ToggleComplete(todo);
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeywords?.FirstOrDefault() + " ", true);
                        return false;
                    }
                };
                results.Add(result);
            }

            return results;
        }

        private static string ToRelativeTime(DateTime value)
        {
            var ts = DateTime.Now.Subtract(value);
            if (ts.TotalMinutes < 1) return "just now";
            if (ts.TotalHours < 1) return $"{ts.Minutes}m ago";
            if (ts.TotalDays < 1) return $"{ts.Hours}h ago";
            if (ts.TotalDays < 7) return $"{ts.Days}d ago";
            return value.ToString("MMM dd");
        }

        public List<Result> Find(Func<Todo, bool> predicate)
        {
            return ToResults(_todoList.Where(predicate));
        }

        public void Add(Todo todo)
        {
            _todoList.Add(todo);
            Save();
        }

        public void Remove(Todo todo)
        {
            _todoList.Remove(todo);
            Save();
        }

        public void ToggleComplete(Todo todo)
        {
            todo.Completed = !todo.Completed;
            Save();
        }

        public void TogglePin(Todo todo)
        {
            todo.Pinned = !todo.Pinned;
            Save();
        }

        public void Edit(Todo todo, string newContent)
        {
            todo.Content = newContent;
            Save();
        }

        public void RemoveCompleted()
        {
            _todoList.RemoveAll(t => t.Completed);
            Save();
        }

        public void Reload() => Load();

        public void ExportToDesktop()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string exportPath = Path.Combine(desktopPath, "todos_export.txt");
            var lines = _todoList.Select(t =>
                $"[{(t.Completed ? "X" : " ")}] {t.Content} (Created: {t.CreatedTime:yyyy-MM-dd HH:mm})");
            File.WriteAllLines(exportPath, lines);
            Context.API.ShowMsg("Exported!", $"Todos exported to {exportPath}");
        }
    }
}
