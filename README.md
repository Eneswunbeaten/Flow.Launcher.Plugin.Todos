# Flow Launcher Plugin: Todos

## Description
A simple and efficient todo management plugin for Flow Launcher that allows users to create, edit, sort, and pin todos. Based on the Wox plugin ([by cayoue](https://github.com/caoyue/Wox.Plugin.Todos)), this version introduces new functionality and optimizations for seamless user experience.

## Key Features
- **Toggle Completion**: Quickly toggle todo items between completed and uncompleted states.
- **Edit Functionality**: Modify existing todos directly within Flow Launcher.
- **Pin/Unpin Items**: Pin important items to the top for easy access.
- **Sorting Options**: Sort todos alphabetically or by creation time (ascending/descending).
- **Efficient Action Retention**: Repeat actions (add, complete, remove) without clearing the command for quick successive actions.

## How to Install
1. Download and install [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher/releases/latest).
2. Install the plugin by typing `pm install todos` in Flow Launcher.
3. Optionally, configure a folder to store your todos.json file in the settings (`Settings -> Plugin -> Todos`).

## Available Commands
| Command        | Description |
|----------------|-------------|
| `td -h`        | Show help options. |
| `td -a`        | Add a new todo. |
| `td -r`        | Remove a todo. |
| `td -c`        | Complete a todo. |
| `td -u`        | Uncheck (uncomplete) a todo. |
| `td -e`        | Edit an existing todo. |
| `td -p`        | Pin a todo to the top. |
| `td -p --u`    | Unpin a todo. |
| `td -s --aa`   | Sort todos alphabetically (A-Z). |
| `td -s --ad`   | Sort todos alphabetically (Z-A). |
| `td -s --ta`   | Sort todos by time (oldest first). |
| `td -s --td`   | Sort todos by time (newest first). |

## Pro Tips
- **Quick Access**: Pinned items stay at the top regardless of other sorting options.
- **Efficient Workflows**: You can add and update multiple todos without clearing the command input.
- **Filter Help**: You can filter through available commands using `td -h <search>` for quick lookup of specific help items.
- **Sync Across Devices**: Configure your todo storage folder in a cloud service like OneDrive, Dropbox, or Google Drive to sync todos file across all your computers.

## Contributing
If you'd like to propose new features, improve existing ones, or report bugs, please open an issue or submit a pull request on this repository!

## Original Wox.Plugin.Todos ReadMe
-------------------
Wox.Plugin.Todos
--------------------------
[![Build status](https://ci.appveyor.com/api/projects/status/hbaa5n2oo940lwyl/branch/master?svg=true)](https://ci.appveyor.com/project/caoyue/wox-plugin-todos/branch/master)

A simple todo app for [Wox](https://github.com/Wox-launcher/Wox)

![demo.gif](https://raw.githubusercontent.com/caoyue/Wox.Plugin.Todos/master/todos.gif)

### usage
- type `td -h` to view supported commands

### sync your todo list
- go to "Settings -> Plugin -> Todos", choose a synced folder you want to store data file
- restart wox to take effect
