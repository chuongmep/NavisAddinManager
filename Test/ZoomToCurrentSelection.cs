using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace Test;

public class ZoomToCurrentSelection : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        ZoomTo();
        return 0;
    }

    public void ZoomTo()

    {
        ComApi.InwOpState10 comState = ComApiBridge.ComApiBridge.State;

        //Create a collection

        ModelItemCollection modelItemCollectionIn =
            new ModelItemCollection(Application.ActiveDocument.CurrentSelection.SelectedItems);

        ComApi.InwOpSelection comSelectionOut = ComApiBridge.ComApiBridge.ToInwOpSelection(modelItemCollectionIn);

        // zoom in to the specified selection

        comState.ZoomInCurViewOnSel(comSelectionOut);

        // zoom in to the current selection

        //comState.ZoomInCurViewOnCurSel();
    }
}