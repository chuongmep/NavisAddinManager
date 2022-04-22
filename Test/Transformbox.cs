using System.Collections.Generic;
using System.Linq;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;

namespace Test;

public class Transformbox : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        transObjWithBox();
        return 0;
    }


    private void transObjWithBox()

    {
        // get current document

        Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;

        //create the box (volume) you want to check

        BoundingBox3D oNewBox = new BoundingBox3D(
            new Point3D(-1000, -1000, 0),
            new Point3D(1000, 1000, 1000));


        // in the first model, check the items whose boxes are

        // within the specified  box.

        IEnumerable<ModelItem> items =
            oDoc.Models[0].RootItem.DescendantsAndSelf.Where(x =>
                oNewBox.Contains(x.BoundingBox())
            );


        // Select the items in the model that are

        // contained in the collection

        oDoc.CurrentSelection.CopyFrom(items);


        // create a collection to store the items

        ModelItemCollection modelItemCollection = new ModelItemCollection();

        modelItemCollection.CopyFrom(items);


        //transform the objects. Currently this needs COM API

        ComApi.InwOpState10 oState = ComApiBridge.ComApiBridge.State;

        ComApi.InwOpSelection oSel =
            ComApiBridge.ComApiBridge.ToInwOpSelection(modelItemCollection);


        // create transform matrix

        ComApi.InwLTransform3f3 oTrans =
            (ComApi.InwLTransform3f3) oState.ObjectFactory
            (ComApi.nwEObjectType.eObjectType_nwLTransform3f,
                null, null);

        ComApi.InwLVec3f oVec =
            (ComApi.InwLVec3f) oState.ObjectFactory
            (ComApi.nwEObjectType.eObjectType_nwLVec3f,
                null, null);

        oVec.SetValue(5000, 5000, 0);

        oTrans.MakeTranslation(oVec);


        // transform

        oState.OverrideTransform(oSel, oTrans);
    }
}