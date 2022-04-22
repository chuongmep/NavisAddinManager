using System.Diagnostics;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;
// The InputPlugin example.

    [Plugin("MyInputPlugin", "ADSK")]

    public class InputPluginExample : InputPlugin

    {  

        public override bool MouseDown(

            //the current view when mouse down

            View view, 

            //Enumerates key modifiers used in input:

            //None, Ctrl,Alt,Shift

            KeyModifiers modifiers,

            //left mouse button:1,

            //middle mouse button:2,

            //right mouse button:3

            ushort button,

            //screen coordinate x

            int x,

            //screen coordinate y

            int y, 

            // not clear to me :-( 

            double timeOffset)

        {

            // key modifiers used in input

            Debug.Print(modifiers.ToString());

            //left/middle mouse

            Debug.Print(button.ToString());

            //timeOffset

            Debug.Print(timeOffset.ToString());

 

            // get info of selecting

            PickItemResult itemResult =
                         view.PickItemFromPoint(x, y);

 

            if (itemResult != null)

            { 

                //selected point in WCS

               string oStr = string.Format(
                "{0},{1},{2}", itemResult.Point.X,

                              itemResult.Point.Y,

                              itemResult.Point.Z);

               Debug.Print(oStr);

 

                //selected object

                ModelItem modelItem = itemResult.ModelItem;

                System.Windows.Forms.MessageBox.Show (
                              modelItem.ClassDisplayName);

            }

 

            return false;

        }

 

        public override bool KeyDown(

            //the current view when mouse down

            View view,

            //Enumerates key modifiers used in input:

            //None, Ctrl,Alt,Shift

            KeyModifiers modifier,

            //the key pressed

            ushort key,

            // not clear to me :-( 

            double timeOffset)

        {

            //  key modifiers + pressed key

            Debug.Print(modifier.ToString() + ", " + key);

            return false;

        }

    }