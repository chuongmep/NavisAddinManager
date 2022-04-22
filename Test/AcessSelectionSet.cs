using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

public class AcessSelectionSet : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        test();
        return 0;
    }

    private void test()
    {
        ComApi.InwOpState10 state;

        state = ComBridge.State;


// model items collection to store the

// items of selection set

        ModelItemCollection modelItemCollectionIn =
            new ModelItemCollection();

// Selection Set Collection

        ComApi.InwSelectionSetExColl oSSExColl =
            state.SelectionSetsEx();


        for (int i = 1; i <= oSSExColl.Count; i++)

        {
            // if this is a selection set

            if (oSSExColl[i] is ComApi.InwOpSelectionSet)

            {
                ComApi.InwOpSelectionSet oSet =
                    (ComApi.InwOpSelectionSet) oSSExColl[i];

                state.CurrentSelection =
                    oSet.selection;

                ComApi.InwOpSelection oCopySelection =
                    (ComApi.InwOpSelection) oSet.selection.Copy();


                foreach (ModelItem oItem in
                         ComBridge.ToModelItemCollection(oSet.selection))

                {
                    // store each item

                    modelItemCollectionIn.Add(oItem);
                }


                //this is just an example on gatehouse.nwd.

                // we checked two sets only

                if (i > 2)

                    break;
            }

            else if (oSSExColl[i] is ComApi.InwSelectionSetFolder)

            {
                //if this is folder.

                ComApi.InwSelectionSetFolder oSSFolder =
                    (ComApi.InwSelectionSetFolder) oSSExColl[i];

                // recurse the folder

                recurseFolderSS(oSSFolder);
            }

            else

            {
            }
        }


//set the current selection

// highlight the selection of the set

        Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.CopyFrom(modelItemCollectionIn);
    }


// recurse a selection set folder

    private void recurseFolderSS(ComApi.InwSelectionSetFolder folder)

    {
        ComApi.InwOpState10 state;

        state = ComBridge.State;


        ComApi.InwSelectionSetExColl oSSExColl =
            folder.SelectionSets();


        for (int i = 1; i <= oSSExColl.Count; i++)

        {
            if (oSSExColl[i] is
                ComApi.InwOpSelectionSet)

            {
                ComApi.InwOpSelectionSet oSet =
                    (ComApi.InwOpSelectionSet) oSSExColl[i];


                //you can collect the sets from the folder.

                // .....
            }

            else if (oSSExColl[i] is ComApi.InwSelectionSetFolder)

            {
                // if this is a folder, recurse

                ComApi.InwSelectionSetFolder oSSFolder =
                    (ComApi.InwSelectionSetFolder) oSSExColl[i];

                recurseFolderSS(oSSFolder);
            }

            else

            {
            }
        }
    }
}