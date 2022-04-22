using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;
using ComApi=Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge=Autodesk.Navisworks.Api.ComApi;
namespace Test;

[Plugin("APICallsCOMPlugin.APICallsCOMPlugin",                    //Plugin name
    "ADSK",                                                   //4 character Developer ID or GUID
    ToolTip = "Demonstrates using the COM API within a .NET API Plugin", //The tooltip for the item in the ribbon
    DisplayName = ".NET_COM")]                                     //Display name for the Plugin in the Ribbon

public class APICallsCOMPlugin : AddInPlugin                               //Derives from AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        // NOTE: All methods called from Navisworks should catch handle 
        //       their own excepetions.
        try
        {
            #region Using_ComApiBridge_State
            ComApi.InwOpState10 state;
            state = ComApiBridge.ComApiBridge.State;

            foreach (ComApi.InwOaPath path in state.CurrentSelection.Paths())
            {
                ComApi.InwOaNode node;
                node = path.Nodes().Last() as ComApi.InwOaNode;
                MessageBox.Show(Autodesk.Navisworks.Api.Application.Gui.MainWindow, "UserName=" + node.UserName);
            }
            #endregion
        }
        catch { }
        return 0;
    }
}