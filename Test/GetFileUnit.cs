using System.Windows;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace Test;

//https://adndevblog.typepad.com/aec/2012/06/accessing-the-file-units-and-location-information-using-navisworks-net-api.html
public class GetFileUnit : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        ComApi.InwOpState10 state;

        state = ComApiBridge.ComApiBridge.State;
        ComApi.InwOaPartition part;

        part = state.CurrentPartition;


        ComApi.InwNodeAttributesColl atts;

        atts = part.Attributes();


        foreach (ComApi.InwOaAttribute att in atts)

        {
            ComApi.InwOaTransform trans = att as ComApi.InwOaTransform;

            if (null != trans)

            {
                ComApi.InwLTransform3f tra = trans.GetTransform();

                object traMatrix = tra.Matrix;
                MessageBox.Show(traMatrix.GetType().ToString());
                break;
            }
        }

        return 0;
    }
}