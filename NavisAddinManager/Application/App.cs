using Autodesk.Navisworks.Api.Plugins;
using NavisAddinManager.Command;
using NavisAddinManager.Properties;
using Test = NavisAddinManager.Command.Test;

namespace NavisAddinManager;

[Plugin("AddinManager", "ChuongMep", DisplayName = "AddinManager")]
[RibbonLayout("AddinManagerRibbon.xaml")]
[RibbonTab("ID_AddinManager_TAB",DisplayName = "AddinManager")]
[Command("ID_ButtonAddinManagerManual", DisplayName = "Addin Manager \n Manual", Icon = "Resources\\dev16x16.png", LargeIcon = "Resources\\dev32x32.png",ToolTip = "Addin Manager Manual")]
[Command("ID_ButtonAddinManagerFaceless", DisplayName = "Addin Manager \n Faceless",Icon = "Resources\\dev16x16.png", LargeIcon = "Resources\\dev32x32.png", ToolTip = "Addin Manager Faceless")]
[Command("ID_ButtonDockPanelCommand", DisplayName = "Show/Hide \n Trace-Debug Output",Icon = "Resources\\dev16x16.png", LargeIcon = "Resources\\dev32x32.png", ToolTip = "Show/Hide Trace-Debug Output")]
[Command("ID_ButtonTest", DisplayName = "Test",Icon = "Resources\\lab16x16.png", LargeIcon = "Resources\\lab32x32.png", ToolTip = "Test")]
public class App  : CommandHandlerPlugin
{
    public override int ExecuteCommand(string name, params string[] parameters)
    {
        switch (name)
        {
            case "ID_ButtonAddinManagerManual":
                AddInManagerManual addInManagerManual = new AddInManagerManual();
                addInManagerManual.Execute();
                break;
            case "ID_ButtonAddinManagerFaceless":
                AddInManagerFaceLess addInManagerFaceless = new AddInManagerFaceLess();
                addInManagerFaceless.Execute();
                break;
            case "ID_ButtonDockPanelCommand":
                DockPanelCommand dockPanelCommand = new DockPanelCommand();
                dockPanelCommand.Execute();
                break;
            case "ID_ButtonTest":
                Test test = new Test();
                test.Execute();
                break;
        }
        return 0;
    }
}