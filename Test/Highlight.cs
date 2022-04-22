using System.Linq;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace Test;

public class Highlight : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        highlight();
        return 0;
    }

    private void highlight()
    {
        Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;

        // assume we get two model items

        ModelItem oItem1 =
            oDoc.Models[0].RootItem.DescendantsAndSelf.ElementAt<ModelItem>(1);

        ModelItem oItem2 =
            oDoc.Models[0].RootItem.DescendantsAndSelf.ElementAt<ModelItem>(2);


        // create ModelItemCollection

        ModelItemCollection oSel_Net =
            new ModelItemCollection();

        oSel_Net.Add(oItem1);

        oSel_Net.Add(oItem2);


        // highlight by .NET API

        oDoc.CurrentSelection.SelectedItems.CopyFrom(oSel_Net);


        //highlight by COM API

        ComApi.InwOpState10 state =
            ComApiBridge.ComApiBridge.State;

        ComApi.InwOpSelection comSelectionOut =
            ComApiBridge.ComApiBridge.ToInwOpSelection(oSel_Net);


        state.CurrentSelection = comSelectionOut;
    }
}