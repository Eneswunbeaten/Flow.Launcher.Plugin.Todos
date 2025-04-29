using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.Todos
{
    public class Main : IPlugin, ISettingProvider
    {
        private static Todos _todos;
        private static Settings _setting;

        public void Init(PluginInitContext context)
        {
            _setting = context.API.LoadSettingJsonStorage<Settings>();
            _todos = new Todos(context, _setting);
        }

        public Control CreateSettingPanel()
        {
            return new FilePathSetting(_setting);
        }

        public List<Result> Query(Query query)
        {
            _todos.Reload();
            _todos.ActionKeyword = query.ActionKeyword;

            if (query.Search.StartsWith("-"))
            {
                return TodoContextMenu.GetMenuOptions(_todos);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var results = new List<Result>();
                results.Add(new Result
                {
                    Title = $"Add new todo: {query.Search}",
                    SubTitle = "Press Enter to add | Esc to cancel",
                    IcoPath = _todos.GetFilePath("ico/app.png"),
                    Score = 100,
                    Action = c =>
                    {
                        _todos.Add(new Todo
                        {
                            Content = query.Search,
                            CreatedTime = System.DateTime.Now
                        });
                        _todos.Context.API.ChangeQuery(query.ActionKeyword + " ", true);
                        return false;
                    }
                });

                var matchingTodos = _todos.Find(t =>
                    t.Content.IndexOf(query.Search, System.StringComparison.OrdinalIgnoreCase) >= 0);

                if (matchingTodos.Any())
                {
                    results.AddRange(matchingTodos.Select(r =>
                    {
                        var originalAction = r.Action;
                        var todo = r.ContextData as Todo;
                        r.Action = c =>
                        {
                            if (c.SpecialKeyState != null && c.SpecialKeyState.CtrlPressed && todo != null)
                            {
                                _todos.Remove(todo);
                                _todos.Context.API.ChangeQuery(query.ActionKeyword + " ", true);
                                return false;
                            }
                            return originalAction != null ? originalAction(c) : false;
                        };
                        return r;
                    }));
                }

                return results;
            }

            var allTodos = _todos.Results;
            if (!allTodos.Any())
            {
                return new List<Result>
                {
                    new Result
                    {
                        Title = "No todos yet",
                        SubTitle = "Type to add your first todo!",
                        IcoPath = _todos.GetFilePath("ico/app.png")
                    }
                };
            }

            return allTodos;
        }
    }
}