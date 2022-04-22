using System;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;
using COMApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace Test;

[Plugin("NETInteropTest", "ADSK", ToolTip = "NETInteropTest", DisplayName = "NETInteropTest")]
public class ABasicPlugin : AddInPlugin
{
    void walkNode(COMApi.InwOaNode parentNode, bool bFoundFirst)
    {
        if (parentNode.IsGroup)
        {
            COMApi.InwOaGroup group = (COMApi.InwOaGroup) parentNode;
            long subNodesCount = group.Children().Count;

            for (long subNodeIndex = 1; subNodeIndex <= subNodesCount; subNodeIndex++)
            {
                object newNode = group.Children()[subNodeIndex];
                object child = group.Children()[subNodeIndex];

                if ((!bFoundFirst) && (subNodesCount > 1))
                {
                    bFoundFirst = true;
                }

                COMApi.InwOaNode? nwOaNode = newNode as COMApi.InwOaNode;
                walkNode(nwOaNode, bFoundFirst);
            }
        }
        else if (parentNode.IsGeometry)
        {
            long fragsCount = parentNode.Fragments().Count;
            System.Diagnostics.Debug.WriteLine("frags count:" + fragsCount.ToString());

            for (long fragindex = 1; fragindex <= fragsCount; fragindex++)
            {
                CallbackGeomListener callbkListener =
                    new CallbackGeomListener();

                COMApi.InwNodeFragsColl fragsColl = parentNode.Fragments();
                object frag = fragsColl[fragindex];
                COMApi.InwOaFragment3? nwOaFragment3 = frag as COMApi.InwOaFragment3;
                nwOaFragment3.GenerateSimplePrimitives(
                    COMApi.nwEVertexProperty.eNORMAL,
                    callbkListener);
            }

            fragCount += fragsCount;
            geoNodeCount += 1;
        }
    }

    DateTime dt = DateTime.Now;
    long geoNodeCount = 0;
    long fragCount = 0;

    public override int Execute(params string[] parameters)
    {
        geoNodeCount = 0;
        fragCount = 0;
        dt = DateTime.Now;

        //convert to COM selection 
        COMApi.InwOpState oState = ComBridge.State;
        walkNode(oState.CurrentPartition, false);

        TimeSpan ts = DateTime.Now - dt;
        MessageBox.Show("Geometry Nodes Count: " + geoNodeCount + " Fragments Count: " + fragCount + "Time: " +
                        ts.TotalMilliseconds.ToString() + "ms");
        return 0;
    }
}

class CallbackGeomListener : COMApi.InwSimplePrimitivesCB
{
    public void Line(COMApi.InwSimpleVertex v1,
        COMApi.InwSimpleVertex v2)

    {
        // do your work 
    }

    public void Point(COMApi.InwSimpleVertex v1)
    {
        // do your work  
    }

    public void SnapPoint(COMApi.InwSimpleVertex v1)
    {
        // do your work  
    }

    public void Triangle(COMApi.InwSimpleVertex v1,
        COMApi.InwSimpleVertex v2,
        COMApi.InwSimpleVertex v3)
    {
        // do your work  
    }
}