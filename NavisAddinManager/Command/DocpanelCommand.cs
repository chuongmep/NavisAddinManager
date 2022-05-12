using Autodesk.Navisworks.Api.ApplicationParts;
using Autodesk.Navisworks.Api.Plugins;
using NavisAddinManager.Model;

namespace NavisAddinManager.Command;

public class DockPanelCommand : IAddinCommand
{
    public override int Action(params string[] parameters)
    {
        string PluginId = $"AddinManager.ShowHidePanel.ChuongMep";
        if (Autodesk.Navisworks.Api.Application.IsAutomated)
        {
            throw new InvalidOperationException("Invalid when running using Automation");
        }
        ApplicationPlugins applicationPlugins = Autodesk.Navisworks.Api.Application.Plugins;
        PluginRecord pr = applicationPlugins.FindPlugin(PluginId);
        if (pr != null && pr is DockPanePluginRecord && pr.IsEnabled)
        {
            //check if it needs loading
            if (pr.LoadedPlugin == null)
            {
                pr.LoadPlugin();
            }

            if (pr.LoadedPlugin is DockPanePlugin dpp)
            {
                //switch the Visible flag
                dpp.Visible = !dpp.Visible;
            }
        }

        return 0;
    }
}