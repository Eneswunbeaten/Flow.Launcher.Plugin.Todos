Reason for Forking Source
--------------------------
This was project was originally forked to add ability to toggle todo completions on and off which seemed like a simple tweak. The source project appeared to be quite stale so forking seemed the best option. This forked version includes updated NuGet packages as well as .NET framework. The source project contains outdated NuGet packages, one of which has a high severity vulnerability. After adding in the missing capability to uncheck completed items, many more improvements were added as described below.

### Updated Backend
- Updated .NET framework to latest version utilized by Flow-Launcher
- Updated NuGet packages which resolved high severity vulnerability
### Optimized Codebase
- Restructured codebase for optimized performance, readability, and maintainability 
### New Capabilities
- Added uncheck ( -u ) capability so todos marked completed can be marked uncompleted
- Added editing ( -e ) capbility for existing todo items allowing user to tweak todo descriptions as needed
### Improved Behavior
- Rather than clearing commands each time add, complete, remove, or uncheck is performed, the corresponding action is retained allowing for quick repeat actions (i.e., adding/completing/removing/unchecking multiple items can be performed in quick succession)

Original ReadMe from forked source
--------------------------
This is a port of the Wox plugin Wox.Plugin.Todos created by caoyue (@caoyue).

This port is intended to be used for [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher). It will not work for Wox.

To download and use this plugin, from [Flow](https://github.com/Flow-Launcher/Flow.Launcher/releases/latest) type `pm install todos`.

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
