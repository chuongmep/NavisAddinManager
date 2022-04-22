using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace Test;
//https://adndevblog.typepad.com/aec/2013/02/create-quick-properties.html
public class CreateQuickProperties : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        QuickProperties();
        return 0;
    }

    void QuickProperties()

    {
        ComApi.InwOpState4 oState = ComApiBridge.State;
        //get the current selection

        ModelItemCollection oModelColl =
            Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems;


        // simple demo: check the first model item only

        if (oModelColl.Count > 0)

        {
            ModelItem oFirstItem = oModelColl[0];

            ComApi.InwOaPath oPath =
                ComApiBridge.ToInwOaPath(oFirstItem);


            // get the content of the display string

            MessageBox.Show(oState.SmartTagText(oPath));
        }


        // re-construct the definitions


        // create a new property tag property

        ComApi.InwSmartTagsOpts oSmartTagOpt =
            (ComApi.InwSmartTagsOpts) oState.ObjectFactory(
                ComApi.nwEObjectType.eObjectType_nwSmartTagsOpts,
                null,
                null);


        // the search condition of the property

        ComApi.InwOpFindCondition oOpFindCond =
            (ComApi.InwOpFindCondition) oState.ObjectFactory(
                ComApi.nwEObjectType.eObjectType_nwOpFindCondition,
                null,
                null);


        // clear the conditions if you re-use the existing InwSmartTagsOpts

        //oSmartTagOpt.Conditions().Clear();


        // demo: to display the property:

        // Entity Handle >> Value


        //set attribute name: use the internal name and user name together!          

        oOpFindCond.SetAttributeNames("LcOpDwgEntityAttrib", "Entity Handle");

        //set property name: use the internal name and user name together!          

        oOpFindCond.SetPropertyNames("LcOaNat64AttributeValue", "Value");

        //search condition

        oOpFindCond.Condition = ComApi.nwEFindCondition.eFind_HAS_PROP;

        //add the condition to the InwSmartTagsOpts

        oSmartTagOpt.Conditions().Add(oOpFindCond);

        // add the InwSmartTagsOpts to global options

        oState.GlobalProperties().SetSmartTagsOpts(oSmartTagOpt);


        // display the properties

        oState.SmartTagsEnabled = true;
    }
}