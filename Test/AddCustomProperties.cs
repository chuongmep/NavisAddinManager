using System.Linq;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace Test;

/// <summary>
/// Try add custom property to a document
/// </summary>
public class AddCustomProperties : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        addProToAllItems();
        return 0;
    }

    private void addProToAllItems()

    {
        //get state object of COM API

        ComApi.InwOpState3 oState = ComBridge.State;


        // get the current selection

        ModelItemCollection oCurrentMC =
            Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems;


        // add property to the selected paths

        foreach (ModelItem oEachSelectedItem in oCurrentMC)

        {
            //convert the .NET collection to COM object

            ComApi.InwOaPath oPath =
                ComBridge.ToInwOaPath(oEachSelectedItem);


            // get properties collection of the path

            ComApi.InwGUIPropertyNode2 propn =
                (ComApi.InwGUIPropertyNode2) oState.GetGUIPropertyNode(oPath, true);


            // create new property category

            // (new tab in the properties dialog)

            ComApi.InwOaPropertyVec newPvec =
                (ComApi.InwOaPropertyVec) oState.ObjectFactory(
                    ComApi.nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

            // create new property

            ComApi.InwOaProperty newP =
                (ComApi.InwOaProperty) oState.ObjectFactory(
                    ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);


            // set the name, username and value of the new property     

            newP.name = "demo_Property_Name";

            newP.UserName = "demo_Property_UserName";

            newP.value = "demo_Property_Value";


            // add the new property to the new property category

            newPvec.Properties().Add(newP);


            // add the new property category to the path

            propn.SetUserDefined(0, "MyAttribute_InternalName",
                "MyAttribute", newPvec);


            // add the attribute to all child items


            // check if oEachItem.Children is empty

            if (oEachSelectedItem.Children.Count<ModelItem>() > 0)

            {
                //*********

                // add attribute to all child items

                foreach (ModelItem oEachChild in oEachSelectedItem.Children)

                {
                    //check if each child node is within the current selection

                    // we will ignore this item because we've added already

                    if (!oCurrentMC.Contains(oEachChild))

                    {
                        // convert the child node to a path e.g.

                        ComApi.InwOaPath oAdditionalPath =
                            ComBridge.ToInwOaPath(oEachChild);


                        // get properties collection of the path

                        ComApi.InwGUIPropertyNode2 propn2 =
                            (ComApi.InwGUIPropertyNode2) oState.GetGUIPropertyNode(
                                oAdditionalPath, true);


                        // add the new property category to the path

                        propn2.SetUserDefined(0, "MyAttribute_InternalName",
                            "MyAttribute", newPvec);
                    }
                }


                //**********

                //or add the attribute to the specific child item

                // e.g. the first 'geometry' node

                if (oEachSelectedItem.HasGeometry)

                {
                    ModelItem oFirstGeoItem = oEachSelectedItem.FindFirstGeometry().Item;

                    //check if each child node is within the current selection

                    // we will ignore this item because we've added already

                    if (!oCurrentMC.Contains(oFirstGeoItem))

                    {
                        // convert the child node to a path e.g.

                        ComApi.InwOaPath oAdditionalPath =
                            ComBridge.ToInwOaPath(oFirstGeoItem);


                        // get properties collection of the path

                        ComApi.InwGUIPropertyNode2 propn3 =
                            (ComApi.InwGUIPropertyNode2) oState.GetGUIPropertyNode(
                                oAdditionalPath, true);


                        // add the new property category to the path

                        propn3.SetUserDefined(0, "MyAttribute_InternalName",
                            "MyAttribute", newPvec);
                    }
                }

                // ......
            }
        }
    }
}