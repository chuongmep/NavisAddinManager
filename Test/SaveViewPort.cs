using System.Diagnostics;
using System.Windows;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;

namespace Test;

public class SaveViewPort : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        savedVPs();
        createSavedVP_Folder();
        return 0;
    }


    void savedVPs()

    {
        Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;

// the saved viewpoint we will set it to current

        SavedViewpoint oSVP_to_SetCurrent = null;


// iterate the collection of saved viewpoints

        foreach (SavedItem oSVP in
                 oDoc.SavedViewpoints.Value)

        {
            // if it is a folder/animation 

            if (oSVP.IsGroup)

                recurse(oSVP);

            else

            {
                // Access the properties of the saved

                //viewpoint               

                SavedViewpoint oThisSVP =
                    oSVP as SavedViewpoint;


                if (oThisSVP != null)

                {
                    MessageBox.Show("Display Name" +
                                oThisSVP.DisplayName);

                    Viewpoint oVP =
                        oThisSVP.Viewpoint;


                    // dump the properties of Viewpoint

                    // ......


                    if (oThisSVP.DisplayName ==
                        "mysavedviewpoint1")

                        oSVP_to_SetCurrent = oThisSVP;
                }
            }
        }

// current selected saved viewpoint

        SavedViewpoint oCurViewPt =
            oDoc.SavedViewpoints.CurrentSavedViewpoint
                as SavedViewpoint;


        Viewpoint oCurrentVP =
            oCurViewPt.Viewpoint;


        // dump the properties of current Viewpoint

        // ......


        // switch the current viewpoint to "myviewpoint1"

        oDoc.SavedViewpoints.CurrentSavedViewpoint =
            oSVP_to_SetCurrent;
    }
    void recurse(SavedItem oFolder)

    {
        foreach (SavedViewpoint oSVP in ((GroupItem)
                     oFolder).Children)

        {
            // if it is a folder/animation                           

            if (oSVP.IsGroup)

                recurse(oSVP);

            else

            {
                // Access the properties of the saved

                //viewpoint                                
            }
        }
    }
    

    void createSavedVP_Folder()

    {

        Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        
        // Create a saved viewpoint with current viewpoint

        SavedViewpoint oNewViewPt1 = new SavedViewpoint(oDoc.CurrentViewpoint.ToViewpoint());
        
        oNewViewPt1.DisplayName = "MySavedView1";

        // Create a viewpoint for the

        // second saved viewpoint
        
        Viewpoint oNewVP = new Viewpoint();

        // Based on the current viewpoint,

        // move camera along X+ with value 10

        oNewVP = oDoc.CurrentViewpoint.ToViewpoint().CreateCopy();

        Point3D oNewPos = new Point3D(oNewVP.Position.X + 10, oNewVP.Position.Y, oNewVP.Position.Z);

        oNewVP.Position = oNewPos;

        // Create the saved viewpoint

        // with the new viewpoint

        SavedViewpoint oNewViewPt2 = new SavedViewpoint(oNewVP);

        oNewViewPt2.DisplayName =

            "MySavedView2";

        // Add the saved viewpoints to the collection

        oDoc.SavedViewpoints.AddCopy(oNewViewPt1);

        oDoc.SavedViewpoints.AddCopy(oNewViewPt2);
        
        FolderItem oNewViewPtFolder1 = new FolderItem();

        oNewViewPtFolder1.DisplayName = "Group1";

 

        // put the saved viewpoint1 to group1

        oNewViewPtFolder1.Children.Add(oNewViewPt1);

 

        // add the group

        oDoc.SavedViewpoints.AddCopy(oNewViewPtFolder1);

        // add saved viewpoint

        oDoc.SavedViewpoints.AddCopy(oNewViewPt2);

    }

}