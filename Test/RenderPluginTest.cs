using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

[Plugin("RenderPluginTest", "ADSK")]
public class RenderPluginTest : RenderPlugin
{
    public override void OverlayRender(View view, Graphics graphics)
    {
        // the color for the graphics

        Color colorRed = Color.FromByteRGB(255, 0, 0);

        // color and alpha value
        graphics.Color(colorRed, 0.7);
        //set line width
        graphics.LineWidth(5);
        Point2D p1 = new Point2D(20, 20);

        Point2D p2 = new Point2D(view.Width - 20, 20);

        Point2D p3 = new Point2D(view.Width /2.0, view.Height - 20);
        // draw triangle, fill in it

        graphics.Triangle(p1,p2,p3,true);

        // the color for the graphics

        Color colorGreen = Color.FromByteRGB(0, 255, 0);

        // color and alpha value

        graphics.Color(colorGreen, 0.7);

        Point2D p4 = new Point2D(20, 20);

        Point2D p5 = new Point2D(view.Width / 2.0, (view.Height - 20) / 2.0);

        Point2D p6 = new Point2D(view.Width - 20, 20);

        //draw two lines

        graphics.Line(p4, p5);

        graphics.Line(p5, p6);

    } 


    // public override void OverlayRender(View view, Graphics graphics)
    //
    // {
    //
    //     // the color for the graphics
    //
    //     Color colorBlue = Color.FromByteRGB(0, 0, 255);
    //
    //     // color and alpha value
    //
    //     graphics.Color(colorBlue, 1);
    //
    //
    //
    //     Point3D p1 = new Point3D(0, 0,0);
    //
    //     Point3D p2 = new Point3D(0, 0,10);    
    //
    //
    //
    //     // draw Cylinder
    //
    //     graphics.Cylinder(p1,p2,10);
    //
    //
    //
    //     // the color for the graphics
    //
    //     Color colorGrey = Color.FromByteRGB(128, 128, 128);
    //
    //     // color and alpha value
    //
    //     graphics.Color(colorGrey, 1);
    //
    //
    //
    //     Point3D p3 = new Point3D(20, 20,0);    
    //
    //
    //
    //     // draw sphere
    //
    //     graphics.Sphere(p3, 10);
    //
    // }

}