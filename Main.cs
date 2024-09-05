using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Flow.Launcher.Plugin;
using Flow.Launcher.Infrastructure.Storage;

namespace Wox.Plugin.Todos
{
    public class Main : IPlugin, ISettingProvider, Flow.Launcher.Plugin.ISavable
    {
        private static Todos _todos;
        private static Todo _todo_to_edit = null;
        private readonly PluginJsonStorage<Settings> _storage;
        private readonly Settings _setting;

        public Main()
        {
            _storage = new PluginJsonStorage<Settings>();
            _setting = _storage.Load();
        }

        public List<Result> Query(Query query)
        {
            _todos.Reload();
            _todos.ActionKeyword = query.ActionKeyword;

            var help = new Help(_todos.Context, query);

            if (query.FirstSearch.Equals("-"))
            {
                _todo_to_edit = null; // enforce reset of _todo_to_edit if user exited edit process early
                return help.Show;
            }

            if (!query.FirstSearch.StartsWith("-"))
            {
                return Search(query.Search, t => !t.Completed);
            }

            if (!Enum.TryParse(query.FirstSearch.TrimStart('-'), true, out TodoCommand op))
            {
                return Search(query.Search, t => !t.Completed);
            }

            switch (op)
            {
                case TodoCommand.H:
                    return help.Show;
                case TodoCommand.U:
                    return HandleUncheck(query);
                case TodoCommand.C:
                    return HandleComplete(query);
                case TodoCommand.R:
                    return HandleRemove(query);
                case TodoCommand.A:
                    return new List<Result> { AddResult(query.SecondToEndSearch) };
                case TodoCommand.E:
                    if (_todo_to_edit == null)
                    {
                        return HandleEdit(query);
                    }
                    else
                    {
                        return new List<Result> { EditResult(query.SecondToEndSearch) };
                    }
                case TodoCommand.L:
                    return Search(query.SecondToEndSearch);
                case TodoCommand.Rl:
                    return new List<Result> {
                        new Result {
                            Title = "Reload todos from data file?",
                            SubTitle = "Click to reload",
                            IcoPath = _todos.GetFilePath(),
                            Action = c => {
                                _todos.Reload();
                                _todos.Context.API.ChangeQuery($"{query.ActionKeyword} ", true);
                                return false;
                            }
                        }
                    };
                default:
                    return Search(query.Search, t => !t.Completed);
            }
        }

        public void Init(PluginInitContext context)
        {
            _todos = new Todos(context, _setting);
        }

        public Control CreateSettingPanel()
        {
            return new FilePathSetting(_setting, _storage);
        }

        public void Save()
        {
            _storage.Save();
        }

        private List<Result> Search(string search, Func<Todo, bool> conditions = null)
        {
            var results = _todos.Find(t =>
                t.Content.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                && (conditions?.Invoke(t) ?? true));

            if (!results.Any() && !string.IsNullOrEmpty(search))
            {
                results.Insert(0, AddResult(search));
            }
            return results;
        }

        private Result AddResult(string content)
        {
            return new Result
            {
                Title = $"Add new item \"{content}\"",
                SubTitle = "",
                IcoPath = _todos.GetFilePath(),
                Action = c =>
                {
                    _todos.Add(new Todo
                    {
                        Content = content,
                        Completed = false,
                        CreatedTime = DateTime.Now
                    });
                    return false;
                }
            };
        }

        private Result EditResult(string content)
        {
            return new Result
            {
                Title = $"Enter new title: \"{content}\"",
                SubTitle = $"Current title: {_todo_to_edit.Content}. Press Enter when done.",
                IcoPath = _todos.GetFilePath(),
                Action = c =>
                {
                    _todos.Edit(_todo_to_edit, content);
                    _todo_to_edit = null; // Reset to null after editing is done
                    return false;
                }
            };
        }

        private List<Result> HandleUncheck(Query query)
        {
            if (query.SecondSearch.Equals("--all", StringComparison.OrdinalIgnoreCase))
            {
                return new List<Result> {
                    new Result {
                        Title = "Mark all todos as not done?",
                        SubTitle = "Click to mark all todos as not done",
                        IcoPath = _todos.GetFilePath(),
                        Action = c => {
                            _todos.UncheckAll();
                            _todos.Context.API.ChangeQuery($"{query.ActionKeyword} ", true);
                            return false;
                        }
                    }
                };
            }

            return _todos.Find(
                t => t.Content.IndexOf(query.SecondToEndSearch, StringComparison.OrdinalIgnoreCase) >= 0 && t.Completed,
                t => "Click to mark todo as not done",
                (c, t) => {
                    _todos.Uncheck(t);
                    _todos.Context.API.ChangeQuery($"{query.ActionKeyword} -u ", true);
                    return false;
                });
        }

        private List<Result> HandleComplete(Query query)
        {
            if (query.SecondSearch.Equals("--all", StringComparison.OrdinalIgnoreCase))
            {
                return new List<Result> {
                    new Result {
                        Title = "Mark all todos as done?",
                        SubTitle = "Click to mark all todos as done",
                        IcoPath = _todos.GetFilePath(),
                        Action = c => {
                            _todos.CompleteAll();
                            _todos.Context.API.ChangeQuery($"{query.ActionKeyword} ", true);
                            return false;
                        }
                    }
                };
            }

            return _todos.Find(
                t => t.Content.IndexOf(query.SecondToEndSearch, StringComparison.OrdinalIgnoreCase) >= 0 && !t.Completed,
                t => "Click to mark todo as done",
                (c, t) => {
                    _todos.Complete(t);
                    _todos.Context.API.ChangeQuery($"{query.ActionKeyword} -c ", true);
                    return false;
                });
        }

        private List<Result> HandleRemove(Query query)
        {
            if (query.SecondSearch.Equals("--all", StringComparison.OrdinalIgnoreCase))
            {
                return new List<Result> {
                    new Result {
                        Title = "Remove all todos?",
                        SubTitle = "Click to remove all todos",
                        IcoPath = _todos.GetFilePath(),
                        Action = c => {
                            _todos.RemoveAll();
                            _todos.Context.API.ChangeQuery($"{query.ActionKeyword} ", true);
                            return false;
                        }
                    }
                };
            }

            if (query.SecondSearch.Equals("--done", StringComparison.OrdinalIgnoreCase))
            {
                return new List<Result> {
                    new Result {
                        Title = "Remove all completed todos?",
                        SubTitle = "Click to remove all completed todos",
                        IcoPath = _todos.GetFilePath(),
                        Action = c => {
                            _todos.RemoveAllCompletedTodos();
                            _todos.Context.API.ChangeQuery($"{query.ActionKeyword} ", true);
                            return false;
                        }
                    }
                };
            }

            return _todos.Find(
                t => t.Content.IndexOf(query.SecondToEndSearch, StringComparison.OrdinalIgnoreCase) >= 0,
                t => "Click to remove todo",
                (c, t) => {
                    _todos.Remove(t);
                    _todos.Context.API.ChangeQuery($"{query.ActionKeyword} -r ", true);
                    return false;
                });
        }

        private List<Result> HandleEdit(Query query)
        {
            _todo_to_edit = null; // Ensure _todo_to_edit is reset for the next operation
            return _todos.Find(
                t => t.Content.IndexOf(query.SecondToEndSearch, StringComparison.OrdinalIgnoreCase) >= 0 && !t.Completed,
                t => "Click to edit todo",
                (c, t) => {
                    _todo_to_edit = t;
                    _todos.Context.API.ChangeQuery($"{query.ActionKeyword} -e {t.Content}", true);
                    return false;
                });
        }
    }
}