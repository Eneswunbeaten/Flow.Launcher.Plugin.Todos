using System;
using System.Collections.Generic;
using System.IO;
using Flow.Launcher.Plugin;

namespace Wox.Plugin.Todos
{
    public class Help
    {
        private readonly PluginInitContext _context;
        private readonly Query _query;
        private readonly string _iconPath;

        public Help(PluginInitContext context, Query query)
        {
            _context = context;
            _query = query;
            _iconPath = Path.Combine(_context.CurrentPluginMetadata.PluginDirectory, @"ico\app.png");
        }

        public List<Result> Show => GetHelpResults();

        private List<Result> GetHelpResults()
        {
            var results = new List<Result>
            {
                CreateResult(
                    title: $"{_query.ActionKeyword} -a [text]",
                    subtitle: "Add a new todo item",
                    query: $"{_query.ActionKeyword} -a [text]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -c [keyword]",
                    subtitle: "Complete a todo item matching the keyword",
                    query: $"{_query.ActionKeyword} -c [keyword]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -c --all",
                    subtitle: "Complete all todos",
                    query: $"{_query.ActionKeyword} -c --all"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -u [keyword]",
                    subtitle: "Uncheck a completed todo item matching the keyword",
                    query: $"{_query.ActionKeyword} -u [keyword]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -u --all",
                    subtitle: "Uncheck all completed todos",
                    query: $"{_query.ActionKeyword} -u --all"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} [keyword]",
                    subtitle: "List todos matching the keyword",
                    query: $"{_query.ActionKeyword} [keyword]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -l [keyword]",
                    subtitle: "List all todos, including completed ones",
                    query: $"{_query.ActionKeyword} -l [keyword]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r [keyword]",
                    subtitle: "Remove todos matching the keyword",
                    query: $"{_query.ActionKeyword} -r [keyword]"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r --all",
                    subtitle: "Remove all todos",
                    query: $"{_query.ActionKeyword} -r --all"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r --done",
                    subtitle: "Remove all completed todos",
                    query: $"{_query.ActionKeyword} -r --done"
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -rl",
                    subtitle: "Reload todos from the data file",
                    query: $"{_query.ActionKeyword} -rl"
                )
            };

            return results;
        }

        private Result CreateResult(string title, string subtitle, string query)
        {
            return new Result
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = _iconPath,
                Action = _ =>
                {
                    _context.API.ChangeQuery(query);
                    return false;
                }
            };
        }
    }
}
