using System.Diagnostics;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;
// Add a button to toggle the InputPlugin example. This can be used to turn the example on and off.
[Plugin("InputAndRenderHandling.EnableInputPluginExample",  // Plugin name
    "ADSK",                                                  // 4 character Developer ID or GUID
    DisplayName = "InputPlugin",                             // Display name for the Plugin in the Ribbon (non-localised if defined here)
    ToolTip = "Enable the InputPlugin example which demonstrates handling mouse and keyboard input.")]  //The tooltip for the item in the ribbon
[AddInPluginAttribute(AddInLocation.AddIn)]                 // The button will appear in the Add-ins tab
public class EnableInputPluginExample : AddInPlugin
{
    public static bool enabled
    { get; private set; }

    public override int Execute(params string[] parameters)
    {
        enabled = !enabled;
        Application.ActiveDocument.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.OverlayRender);
        return 0;
    }

    public override CommandState CanExecute()
    {
        CommandState s = new CommandState();
        s.IsChecked = enabled;
        s.IsVisible = true;
        s.IsEnabled = true;
        return s;
    }
}

// The InputPlugin example.
[Plugin("InputAndRenderHandling.InputPluginExample", "ADSK", DisplayName = "InputPlugin")]
public class InputPluginExample : InputPlugin
{
    public override bool MouseDown(View view, KeyModifiers modifiers, ushort button, int x, int y, double timeOffset)
    {
        if (EnableInputPluginExample.enabled)
        {
            bool doubleClick = modifiers.HasFlag(KeyModifiers.DoubleClick);//Determine if trigered by one of the following: WM_LBUTTONDBLCLK, WM_MBUTTONDBLCLK or WM_RBUTTONDBLCLK.

            PickItemResult itemResult = view.PickItemFromPoint(x, y);
            if (itemResult != null)
            {
                ModelItem modelItem = itemResult.ModelItem;
                Debug.WriteLine(modelItem.ClassDisplayName);
            }
        }
        return false;
    }

    public override bool MouseUp(View view, KeyModifiers modifiers, ushort button, int x, int y, double timeOffset)
    {
        if (EnableInputPluginExample.enabled)
        {

        }
        return false;
    } 
      
    public override bool ContextMenu(View view, int x, int y)
    {
        if (EnableInputPluginExample.enabled)
        {

        }
        return false;
    }

    public override bool KeyDown(View view, KeyModifiers modifier, ushort key, double timeOffset)
    {
        Debug.WriteLine(modifier.ToString() + ", " + key);
        return false;
    }
}
// Add a button to toggle the RenderPlugin example. This can be used to turn the example on and off.
[Plugin("InputAndRenderHandling.EnableRenderPluginExample", "ADSK", DisplayName = "RenderPlugin",
    ToolTip = "Enable the RenderPlugin example which demonstrates handling rendering.")]
[AddInPluginAttribute(AddInLocation.AddIn)]
public class EnableRenderPluginExample : AddInPlugin
{
    public static bool enabled
    { get; private set; }

    public override int Execute(params string[] parameters)
    {
        enabled = !enabled;
        Application.ActiveDocument.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.OverlayRender);
        return 0;
    }

    public override CommandState CanExecute()
    {
        CommandState s = new CommandState();
        s.IsChecked = enabled;
        s.IsVisible = true;
        s.IsEnabled = true;
        return s;
    }
}
