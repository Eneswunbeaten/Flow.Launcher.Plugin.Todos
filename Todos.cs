using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Flow.Launcher.Plugin;
using Newtonsoft.Json;

namespace Wox.Plugin.Todos
{
    // Represents a single Todo item
    public class Todo
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    // Manages a collection of Todo items
    public class Todos
    {
        // Constants
        private const string DataFileName = @"todos.data.json";

        // Private Fields
        private string _dataFolderPath;
        private List<Todo> _todoList;

        // Public Properties
        public PluginInitContext Context { get; }
        public string ActionKeyword { get; set; }
        public List<Result> Results => ToResults(_todoList);

        // Private Properties
        private int MaxId => _todoList?.Max(t => t.Id) ?? 0;

        // Constructor
        public Todos(PluginInitContext context, Settings setting)
        {
            Context = context;
            ActionKeyword = context.CurrentPluginMetadata.ActionKeywords?.FirstOrDefault() ?? string.Empty;
            _dataFolderPath = setting.FolderPath;
            Load();
        }

        // Initialization Methods
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
                throw new Exception($"Failed to read data file: {e.Message}");
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
                throw new Exception($"Failed to write data file: {e.Message}");
            }
        }

        // GetFilePath and Alert Functions
        public string GetFilePath(string icon = "")
        {
            return Path.Combine(Context.CurrentPluginMetadata.PluginDirectory, string.IsNullOrEmpty(icon) ? @"ico\app.png" : icon);
        }

        public void Alert(string title, string content)
        {
            Context.API.ShowMsg(title, content, GetFilePath());
        }

        // Methods for Generating Results
        private bool PerformDefaultAction(Todo todo)
        {
            try
            {
                Clipboard.SetText(todo.Content);
                return true;
            }
            catch (Exception)
            {
                Alert("Failed", "Copy failed, please try again later.");
                return false;
            }
        }

        private static string ToRelativeTime(DateTime value)
        {
            var ts = DateTime.Now.Subtract(value);
            var seconds = ts.TotalSeconds;

            if (seconds < 60)
                return seconds < 2 ? "one second ago" : $"{ts.Seconds} seconds ago";

            if (seconds < 3600)
                return ts.Minutes == 1 ? "one minute ago" : $"{ts.Minutes} minutes ago";

            if (seconds < 86400)
                return ts.Hours == 1 ? "an hour ago" : $"{ts.Hours} hours ago";

            if (seconds < 172800)
                return "yesterday";

            if (seconds < 2592000)
                return ts.Days == 1 ? "one day ago" : $"{ts.Days} days ago";

            if (seconds < 31536000)
                return ts.Days / 30 <= 1 ? "one month ago" : $"{ts.Days / 30} months ago";

            return ts.Days / 365 <= 1 ? "one year ago" : $"{ts.Days / 365} years ago";
        }

        private List<Result> ToResults(IEnumerable<Todo> todos, Func<Todo, string> subTitleFormatter = null, Func<ActionContext, Todo, bool> itemAction = null)
        {
            var results = todos.OrderByDescending(t => t.CreatedTime)
                .Select(t => new Result
                {
                    Title = t.Content,
                    SubTitle = subTitleFormatter?.Invoke(t) ?? $"{ToRelativeTime(t.CreatedTime)} | Copy to clipboard",
                    IcoPath = GetFilePath(t.Completed ? @"ico\done.png" : @"ico\todo.png"),
                    Action = c =>
                    {
                        return itemAction?.Invoke(c, t) ?? PerformDefaultAction(t);
                    }
                }).ToList();

            if (!results.Any())
            {
                results.Add(new Result
                {
                    Title = "No results",
                    SubTitle = "Click to view help",
                    IcoPath = GetFilePath(),
                    Action = c =>
                    {
                        Context.API.ChangeQuery($"{ActionKeyword} -h");
                        return false;
                    }
                });
            }
            return results;
        }

        // Search and Filtering Methods
        public List<Result> Find(Func<Todo, bool> predicate, Func<Todo, string> subTitleFormatter = null, Func<ActionContext, Todo, bool> itemAction = null)
        {
            return ToResults(_todoList.Where(predicate), subTitleFormatter, itemAction);
        }

        // Todo Management Methods
        public Todos Add(Todo todo, Action callback = null)
        {
            if (string.IsNullOrEmpty(todo.Content)) return this;

            todo.Id = MaxId + 1;
            _todoList.Add(todo);
            Save();
            callback?.Invoke();
            Context.API.ChangeQuery($"{ActionKeyword} -a ");
            return this;
        }

        public Todos Remove(Todo todo, Action callback = null)
        {
            var item = _todoList.FirstOrDefault(t => t.Id == todo.Id);
            if (item != null)
            {
                _todoList.Remove(item);
                Save();
                callback?.Invoke();
            }
            return this;
        }

        public Todos RemoveAll(Action callback = null)
        {
            _todoList.Clear();
            Save();
            Alert("Success", "All todos removed!");
            callback?.Invoke();
            return this;
        }

        public Todos RemoveAllCompletedTodos(Action callback = null)
        {
            _todoList.RemoveAll(t => t.Completed);
            Save();
            Alert("Success", "All completed todos removed!");
            callback?.Invoke();
            return this;
        }

        public Todos Complete(Todo todo, Action callback = null)
        {
            var item = _todoList.FirstOrDefault(t => t.Id == todo.Id);
            if (item != null)
            {
                item.Completed = true;
                Save();
                callback?.Invoke();
            }
            return this;
        }

        public Todos Uncheck(Todo todo, Action callback = null)
        {
            var item = _todoList.FirstOrDefault(t => t.Id == todo.Id);
            if (item != null)
            {
                item.Completed = false;
                Save();
                callback?.Invoke();
            }
            return this;
        }

        public Todos CompleteAll(Action callback = null)
        {
            _todoList.ForEach(t => t.Completed = true);
            Save();
            Alert("Success", "All todos marked as done!");
            callback?.Invoke();
            return this;
        }

        public Todos UncheckAll(Action callback = null)
        {
            _todoList.ForEach(t => t.Completed = false);
            Save();
            Alert("Success", "All todos unchecked!");
            callback?.Invoke();
            return this;
        }

        public void Reload() => Load();
    }
}
