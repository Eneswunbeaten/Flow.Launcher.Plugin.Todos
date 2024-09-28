using System;
using System.Collections.Generic;
using System.IO;
using Flow.Launcher.Plugin;
using Flow.Launcher.Plugin.SharedModels;

namespace Flow.Launcher.Plugin.Todos
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
        private Result CreateResult(string title, string subtitle, string query, int score)
        {
            return new Result
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = _iconPath,
                Score = score,
                Action = _ =>
                {
                    _context.API.ChangeQuery(query);
                    return false;
                }
            };
        }

        public List<Result> Show => GetHelpResults();

        private List<Result> GetHelpResults()
        {
            var results = new List<Result>
            {
                CreateResult(
                    title: $"{_query.ActionKeyword} -a [text]",
                    subtitle: "Add a new todo item",
                    query: $"{_query.ActionKeyword} -a [text]",
                    score: 1000000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -c [keyword]",
                    subtitle: "Complete a todo item matching the keyword",
                    query: $"{_query.ActionKeyword} -c [keyword]",
                    score: 999000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -c --all",
                    subtitle: "Complete all todos",
                    query: $"{_query.ActionKeyword} -c --all",
                    score: 998000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -u [keyword]",
                    subtitle: "Uncheck a completed todo item matching the keyword",
                    query: $"{_query.ActionKeyword} -u [keyword]",
                    score: 997000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -u --all",
                    subtitle: "Uncheck all completed todos",
                    query: $"{_query.ActionKeyword} -u --all",
                    score: 996000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -e [keyword]",
                    subtitle: "Edit an existing todo item matching the keyword",
                    query: $"{_query.ActionKeyword} -e [keyword]",
                    score: 995000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r [keyword]",
                    subtitle: "Remove todos matching the keyword",
                    query: $"{_query.ActionKeyword} -r [keyword]",
                    score: 994000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r --all",
                    subtitle: "Remove all todos",
                    query: $"{_query.ActionKeyword} -r --all",
                    score: 993000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -r --done",
                    subtitle: "Remove all completed todos",
                    query: $"{_query.ActionKeyword} -r --done",
                    score: 992000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -rl",
                    subtitle: "Reload todos from the data file",
                    query: $"{_query.ActionKeyword} -rl",
                    score: 991000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -s --aa",
                    subtitle: "Sort todos alphabetical ascending",
                    query: $"{_query.ActionKeyword} -s --aa",
                    score: 990000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -s --ad",
                    subtitle: "Sort todos alphabetical descending",
                    query: $"{_query.ActionKeyword} -s --ad",
                    score: 989000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -s --ta",
                    subtitle: "Sort todos time ascending",
                    query: $"{_query.ActionKeyword} -s --ta",
                    score: 988000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -s --td",
                    subtitle: "Sort todos time descending",
                    query: $"{_query.ActionKeyword} -s --td",
                    score: 987000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} -l [keyword]",
                    subtitle: "List all todos, including completed ones",
                    query: $"{_query.ActionKeyword} -l [keyword]",
                    score: 986000000
                ),
                CreateResult(
                    title: $"{_query.ActionKeyword} [keyword]",
                    subtitle: "List todos matching the keyword",
                    query: $"{_query.ActionKeyword} [keyword]",
                    score: 985000000
                )
            };

            return results;
        }
    }
}
