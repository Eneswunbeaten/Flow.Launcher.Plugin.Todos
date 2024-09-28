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
            const int baseScore = 1000000000;
            const int scoreDecrement = 1000000;

            var commands = new (string title, string subtitle, string query)[]
            {
                ($"{_query.ActionKeyword} [keyword]", "List todos matching the keyword", $"{_query.ActionKeyword} [keyword]"),
                ($"{_query.ActionKeyword} -l [keyword]", "List all todos, including completed ones", $"{_query.ActionKeyword} -l [keyword]"),
                ($"{_query.ActionKeyword} -a [text]", "Add a new todo item", $"{_query.ActionKeyword} -a [text]"),
                ($"{_query.ActionKeyword} -c [keyword]", "Complete a todo item matching the keyword", $"{_query.ActionKeyword} -c [keyword]"),
                ($"{_query.ActionKeyword} -c --all", "Complete all todos", $"{_query.ActionKeyword} -c --all"),
                ($"{_query.ActionKeyword} -u [keyword]", "Uncheck a completed todo item matching the keyword", $"{_query.ActionKeyword} -u [keyword]"),
                ($"{_query.ActionKeyword} -u --all", "Uncheck all completed todos", $"{_query.ActionKeyword} -u --all"),
                ($"{_query.ActionKeyword} -e [keyword]", "Edit an existing todo item matching the keyword", $"{_query.ActionKeyword} -e [keyword]"),
                ($"{_query.ActionKeyword} -p [keyword]", "Pin to top an uncompleted todo item matching the keyword", $"{_query.ActionKeyword} -p [keyword]"),
                ($"{_query.ActionKeyword} -p --u [keyword]", "Unpin a todo item matching the keyword", $"{_query.ActionKeyword} -p --u [keyword]"),
                ($"{_query.ActionKeyword} -r [keyword]", "Remove todos matching the keyword", $"{_query.ActionKeyword} -r [keyword]"),
                ($"{_query.ActionKeyword} -r --all", "Remove all todos", $"{_query.ActionKeyword} -r --all"),
                ($"{_query.ActionKeyword} -r --done", "Remove all completed todos", $"{_query.ActionKeyword} -r --done"),
                ($"{_query.ActionKeyword} -rl", "Reload todos from the data file", $"{_query.ActionKeyword} -rl"),
                ($"{_query.ActionKeyword} -s --aa", "Sort todos alphabetical ascending", $"{_query.ActionKeyword} -s --aa"),
                ($"{_query.ActionKeyword} -s --ad", "Sort todos alphabetical descending", $"{_query.ActionKeyword} -s --ad"),
                ($"{_query.ActionKeyword} -s --ta", "Sort todos time ascending", $"{_query.ActionKeyword} -s --ta"),
                ($"{_query.ActionKeyword} -s --td", "Sort todos time descending", $"{_query.ActionKeyword} -s --td"),
            };

            var results = new List<Result>();
            int currentScore = baseScore;

            foreach (var (title, subtitle, query) in commands)
            {
                results.Add(CreateResult(title, subtitle, query, currentScore));
                currentScore -= scoreDecrement; // Decrement the score after each addition
            }

            return results;
        }
    }
}
