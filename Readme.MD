# Navisworks Add-in Manager
![Navis API](https://img.shields.io/badge/Navisworks%20API%202024-blue.svg) ![Platform](https://img.shields.io/badge/platform-Windows-lightgray.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![ReSharper](https://img.shields.io/badge/ReSharper-2023.0.0-yellow) ![Rider](https://img.shields.io/badge/Rider-2023.0.0-yellow) ![Visual Studio 2022](https://img.shields.io/badge/Visual_Studio_2022-yellow) ![.NET Framework](https://img.shields.io/badge/.NET_6.0-yellow)

[![Publish](../../actions/workflows/Workflow.yml/badge.svg)](../../actions)
[![Github All Releases](https://img.shields.io/github/downloads/chuongmep/navisaddinmanager/total?color=blue&label=Download)]()
[![HitCount](https://hits.dwyl.com/chuongmep/navisaddinmanager.svg?style=flat-square)](http://hits.dwyl.com/chuongmep/navisaddinmanager)
<a href="https://twitter.com/intent/follow?screen_name=chuongmep">
<img src="https://img.shields.io/twitter/follow/chuongmep?style=social&logo=twitter"
alt="follow on Twitter"></a>
## Summary

![AddinManager](pic/Addin.png)

Usually, when developing and debugging an addin with [Navisworks](https://www.autodesk.com/products/navisworks/overview) API, user has to close & re-open [Navisworks](https://www.autodesk.com/products/navisworks/overview) each time
he/she modifies the addin code and wants to re-compile. But with **Add-In Manager**, user can modify and run the Addin.
directly without closing & re-opening [Navisworks](https://www.autodesk.com/products/navisworks/overview) again and again.This tool enables users to test plugin code within Navisworks without having to create an entire plugin first.This is a next branch from [Revit Addin Manager](https://github.com/chuongmep/RevitAddInManager)
to support multiple platform API for developer. With current technology, you can use [Hot Reload](https://docs.microsoft.com/en-us/visualstudio/debugger/hot-reload?view=vs-2022) to speed it up.But try this project it will be faster with any Addin.

- [x] [Revit Add-In Manager](https://github.com/chuongmep/RevitAddInManager)

- [x] [Navis Add-In Manager](https://github.com/chuongmep/NavisAddInManager)

- [x] [Cad Add-In Manager](https://github.com/chuongmep/CadAddInManager)

- [x] More...

![](pic/7aF7wDel5L.gif)

### Some feature include:

- A newbie can create a new Add-in and explore **Navisworks API** easy with Add-in manager.
- Add-in tester(use Debug and Trace to test your add-in).
- **Debug / Trace** : Support Show result use [Debug](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug?view=net-6.0) or [Trace](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace?view=net-6.0) to show result in Navisworks.
- Add-in manager
    - AddInManager Manual : Use for normal process load/unload addin
    - AddInManager Faceless : use for load last process before addin without UI
- Allow user know whether plugin can load successfully or not.
- No depend on any library from outside **Navisworks API**.
- <kbd>F5</kbd> - Load fresh plugin don't need load again.
- <kbd>F1</kbd> - Go link open source report some error,bug or feature request.
- <kbd>Delete</kbd> - Quick remove by use right click or use from keyboard.
- <kbd>Crt + MouseWheel</kbd> - Zoom in/out by use mouse wheel in command plugin.
- <kbd>Arrow Up</kbd> - Move from TreeView to search.
- <kbd>Arrow Down</kbd> - Move from search to TreeView.
- <kbd>Esc</kbd> - Quick Close Add-in Manager.
- <kbd>Enter</kbd> - Quick Run Execute a command selected in Add-in Manager.
## Add-In Manager

![](pic/AddinManager.png)

### Command

- [x] AddInManagerManual
- [x] AddInManagerFaceless

### Trace/Debug

- [x] Show/Hide result inside **Dockpanel Navisworks**.
- [x] Show Result history inside **Navis Addin Manager**.
- [x] Clear Result.
- [X] Show Result by color.

![](pic/Trace-Debug.png)

A sample command to execute:

- Way one : use full with Add-in Manager
``` csharp
[Plugin("HelloWorld", "ChuongMep",DisplayName = "HelloWorld", ToolTip = "HelloWorld Navisworks AddinManager")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class HelloWorld : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            //Code Here
            MessageBox.Show("Hello World",Application.Title);
            return 0;
        }
    }
   
```
- Way two  : use one quick run with Add-in Manager
``` csharp
    /// <summary>
    /// Only work for in AddinManager
    /// </summary>
    public class TestDontNeedUseAttClass : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            //Code Here
            MessageBox.Show("Hello World",Application.Title);
            return 0;
        }
    }
```
---
- Quick Example to Debug or Trace result output :

```cs
using System.Diagnostics;
using Autodesk.Navisworks.Api.Plugins;
public class DebugTrace : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        Trace.WriteLine($"Warning: This is a warning");
        Trace.WriteLine($"Error: This is a error");
        Trace.WriteLine($"Add: This is a add");
        Trace.WriteLine($"Modify: This is a modify");
        Trace.WriteLine($"Delete: This is a delete");
        return 0;
    }
}
```

## Installation

Please follow last release at section [Release](https://github.com/chuongmep/NavisAddInManager/releases/latest)

Support Navisworks Version : 2020, 2021, 2022, 2023,2024.
---

## Guideline

You can visit to wiki and read document or access by this [link](https://github.com/chuongmep/NavisAddInManager/wiki).

---

## Author

Originally implemented by [ChuongHo](https://github.com/chuongmep) with platform **WPF** and add more feature to fit
with the progressive development of modern technology. His focus is to work smarter and achieve the most effective
practices to enhance data utilization and digital collaboration among AEC parties.

---

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see
the [License](License.md) file for full details.

---

## Contribute

**Add-In Manager** is an open-source project and would _be_ nothing without its community. You can make suggestions or
track and submit bugs via
Github [issues](https://docs.github.com/en/issues/tracking-your-work-with-issues/creating-an-issue). You can submit your
own code to the **Add-In Manager** project via a
Github [pull request](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/about-pull-requests)
.

Many Thanks all contributors for this repository. Feel free to contribute!
Please refer to the [CONTRIBUTING](CONTRIBUTING.md) for details.

<a href = "https://github.com/chuongmep/NavisAddInManager/graphs/contributors">
  <img src = "https://contrib.rocks/image?repo=chuongmep/NavisAddInManager"/>
</a>

---

## Sponsors

![](pic/jetbrains.png)

Thanks for providing a free All product IDE for this project

---

## Credits

- Credit to [icons8.com](https://icons8.com) for the Navis Add-in Manager icons.
- Credit to [Nice3point](https://github.com/Nice3point) for the CI/CD template.
- Website helped me resize icon : [https://resizeappicon.com/](https://resizeappicon.com/)

--- 

## Open Source Recomment

- [NavisAppInfo](https://github.com/chuongmep/NavisAppInfo) : Allow user snoop properties, method, track event. Easy to explore Navsiworks.

## Learning Resouces

- [Navisworks Developer Center](https://www.autodesk.com/developer-network/platform-technologies/navisworks)
- [adndevblog](https://adndevblog.typepad.com/aec/navisworks/)
- [Autodesk Navisworks Forum](https://forums.autodesk.com/t5/navisworks/ct-p/4)
- [Navisworks API Docs](https://apidocs.co/apps/navisworks/2018/87317537-2911-4c08-b492-6496c82b3ed0.htm)
- [Navisworks API Open Source](https://github.com/topics/navisworks-api)
- [Navisworks Nuget](https://www.nuget.org/packages?q=chuongmep.navis)
