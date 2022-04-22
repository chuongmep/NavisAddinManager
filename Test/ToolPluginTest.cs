using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

[Plugin("EnableToolPluginExample", "ADSK",
    DisplayName = "EnableToolPlugin")]
public class EnableToolPluginExample : AddInPlugin

{
    static bool enable = false;

    public override int Execute(params string[] parameters)

    {
        if (enable)

        {
            //switch to the native tool
            Application.MainDocument.Tool.Value = Tool.Select;
        }

        else

        {
            //switch to custom tool

            ToolPluginRecord toolPluginRecord =
                (ToolPluginRecord) Application.Plugins.FindPlugin(
                    "ToolPluginTest.ADSK");

            Application.MainDocument.Tool.SetCustomToolPlugin(toolPluginRecord.LoadPlugin());
        }

        enable = !enable;

        return 0;
    }
}

[Plugin("ToolPluginTest", "ADSK")]
public class ToolPluginTest : ToolPlugin

{
    ModelItem clickedModel;

    public override bool MouseDown(View view,
        KeyModifiers modifiers,
        ushort button,
        int x,
        int y,
        double timeOffset)

    {
        // get current selection
        PickItemResult itemResult =
            view.PickItemFromPoint(x, y);

        if (itemResult != null)

        {
            clickedModel =
                itemResult.ModelItem;

            Application.ActiveDocument.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.Render);
        }

        return false;
    }


    public void OverlayRenderModel(View view, Graphics graphics)

    {
        if (clickedModel != null)

        {
            //color for graphics

            Color yellow = Color.FromByteRGB(255, 255, 0);

            graphics.Color(yellow, 0.8);


            //boundingbox of the current selection

            BoundingBox3D boundingBox = clickedModel.BoundingBox();


            Point3D origin = boundingBox.Min;

            Vector3D xVector =
                new Vector3D((boundingBox.Max -
                              boundingBox.Min).X, 0, 0);

            Vector3D yVector =
                new Vector3D(0, (boundingBox.Max -
                                 boundingBox.Min).Y, 0);

            Vector3D zVector =
                new Vector3D(0, 0, (boundingBox.Max -
                                    boundingBox.Min).Z);


            //draw a cuboid

            graphics.Cuboid(origin, xVector, yVector, zVector, true);
        }
    }
}