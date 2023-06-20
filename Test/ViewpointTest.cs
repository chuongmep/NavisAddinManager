using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;
using COMBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using ComAPI = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace Test;

public class ViewpointTest : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        return 0;
    }

    public void CreateViewPointSample()
    {
        // Create a new Viewpoint sample with name Test
        DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;
        using (Transaction tran = new Transaction(Application.ActiveDocument, "Create Viewpoint"))
        {
            // Create a new Viewpoint
            SavedViewpoint savedViewpoint = new SavedViewpoint();
            savedViewpoint.DisplayName = "Test";
            savedViewpoint.Viewpoint.Position = new Point3D(0, 0, 0);
            // Add the Viewpoint to the document
            documentSavedViewpoints.AddCopy(savedViewpoint);
            // Commit the transaction
            tran.Commit();
        }
    }

    /// <summary>
    /// Create A new Viewpoint from Current View
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private SavedViewpoint CreateViewPointCurrentView(string name)
    {
        // Create a new Viewpoint sample with name Test
        DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;
        SavedViewpoint savedViewpoint;
        using (Transaction tran = new Transaction(Application.ActiveDocument, "Create Viewpoint"))
        {
            // Create a new Viewpoint
            DocumentCurrentViewpoint currentViewpoint = Application.ActiveDocument.CurrentViewpoint;
            Viewpoint viewpoint = currentViewpoint.ToViewpoint().CreateCopy();
            savedViewpoint = new SavedViewpoint(viewpoint);
            savedViewpoint.DisplayName = name;
            documentSavedViewpoints.AddCopy(savedViewpoint);
            // Commit the transaction
            tran.Commit();
        }

        return savedViewpoint;
    }

    void TrySetCamera()
    {
        ComAPI.InwOpState10 comState;
        comState = COMBridge.State;
        //find the clash detective plugin
        ComAPI.InwOpClashElement m_clash = null;

        foreach (ComAPI.InwBase oPlugin in comState.Plugins())
        {
            if (oPlugin.ObjectName == "nwOpClashElement")
            {
                m_clash = (ComAPI.InwOpClashElement) oPlugin;
                break;
            }
        }

        if (m_clash == null)
        {
            System.Windows.Forms.MessageBox.Show(@"cannot find clash test plugin!");
            return;
        }

        foreach (ComAPI.InwOclClashTest test in m_clash.Tests())
        {
            string testName = test.name;
            string objectName = test.ObjectName;
            foreach (ComAPI.InwOclTestResult result in test.results())
            {
                ComAPI.InwNvViewPoint vp = result.GetSuitableViewPoint();
                string resultName = result.name;
                if (resultName != "Clash7")
                {
                    continue;
                }

                // Create a new Viewpoint
                using (Transaction tran = new Transaction(Application.ActiveDocument, "Create Viewpoint"))
                {
                    // Create a new Viewpoint
                    SavedViewpoint savedViewpoint = new SavedViewpoint();
                    savedViewpoint.DisplayName = resultName;
                    savedViewpoint.Viewpoint.Position = new Point3D(vp.Camera.Position.data1, vp.Camera.Position.data2,
                        vp.Camera.Position.data3);
                    // Try set rotation
                    double angle = vp.Camera.Rotation.angle;
                    ComAPI.InwLUnitVec3f axis = vp.Camera.Rotation.GetAxis();
                    ComAPI.nwEProjection nwEProjection = vp.Camera.Projection;
                    //set savedViewpoint.Viewpoint
                    UnitVector3D unitVector3D = new UnitVector3D(axis.data1, axis.data2, axis.data3);
                    savedViewpoint.Viewpoint.Rotation = new Rotation3D(unitVector3D, angle);
                    switch (nwEProjection)
                    {
                        case ComAPI.nwEProjection.eProjection_PERSPECTIVE:
                            savedViewpoint.Viewpoint.Projection = ViewpointProjection.Perspective;
                            break;
                        case ComAPI.nwEProjection.eProjection_ORTHOGRAPHIC:
                            savedViewpoint.Viewpoint.Projection = ViewpointProjection.Orthographic;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Application.ActiveDocument.CurrentViewpoint.CopyFrom(savedViewpoint.Viewpoint);
                    // Add the Viewpoint to the document
                    DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;
                    documentSavedViewpoints.AddCopy(savedViewpoint);
                }
            }
        }
    }

    public void ResetActiveView()
    {
        DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;
        using (Transaction tran = new Transaction(Application.ActiveDocument, "Create Default Viewpoint"))
        {
            // Create a new Viewpoint
            SavedViewpoint savedViewpoint = new SavedViewpoint();
            savedViewpoint.DisplayName = "Default";
            SavedItem? savedItem = documentSavedViewpoints.Value.FirstOrDefault(x => x.DisplayName == "Default");
            if (savedItem != null) documentSavedViewpoints.Remove(savedItem);
            // Add the Viewpoint to the document
            documentSavedViewpoints.AddCopy(savedViewpoint);
            Application.ActiveDocument.SavedViewpoints.CurrentSavedViewpoint =
                documentSavedViewpoints.Value.Last<SavedItem>();
            Application.ActiveDocument.Models.ResetAllHidden();
            // Commit the transaction
            tran.Commit();
        }
    }

    public void GenerateViewPointClashResult()
    {
        // Get all Clash Test
        Document doc = Application.ActiveDocument;
        DocumentClash documentClash = doc.GetClash();
        var clashTests = documentClash.TestsData.Tests;
        FolderItem folderItem = new FolderItem();
        folderItem.DisplayName = "My Folder";
        // add the folder to saved viewpoint collection
        DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;
        documentSavedViewpoints.AddCopy(folderItem);
        foreach (SavedItem test in clashTests)
        {
            ClashTest? clashTest = test as ClashTest;
            if (clashTest == null) continue;
            foreach (var savedItem1 in clashTest.Children)
            {
                ModelItemCollection visible = new ModelItemCollection();
                List<ModelItem> AItems = new List<ModelItem>();
                List<ModelItem> BItems = new List<ModelItem>();
                if (savedItem1 is ClashResult)
                {
                    var result = (ClashResult) savedItem1;
                    visible.AddRange(result.Selection1);
                    visible.AddRange(result.Selection2);
                    AItems.Add(result.Item1);
                    BItems.Add(result.Item2);
                    SaveViewClashResult(doc, clashTest, result, visible, AItems, BItems);
                }
                else if (savedItem1 is ClashResultGroup gr)
                {
                    foreach (var savedItem2 in gr.Children)
                    {
                        ModelItemCollection gVisible = new ModelItemCollection();
                        List<ModelItem> AGroupItems = new List<ModelItem>();
                        List<ModelItem> BGroupItems = new List<ModelItem>();
                        if (savedItem2 is ClashResult)
                        {
                            var result = (ClashResult) savedItem2;
                            visible.AddRange(result.Selection1);
                            visible.AddRange(result.Selection2);
                            AItems.Add(result.Item1);
                            BItems.Add(result.Item2);
                            SaveViewClashResult(doc, clashTest, result, gVisible, AGroupItems, BGroupItems);
                        }
                    }
                }
            }
        }

        // reset view
        ResetActiveView();
    }

    public void SaveViewClashResult(Document doc, ClashTest test, ClashResult clashResult, ModelItemCollection visible,
        List<ModelItem> AItems, List<ModelItem> BItems)
    {
        ResetActiveView();
        //hide the remaining items
        visible.Invert(doc);
        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.SetHidden(visible, true);
        // zoom to the visible items
        visible.Invert(doc);
        ComAPI.InwOpSelection comSelectionOut = ComApiBridge.ComApiBridge.ToInwOpSelection(visible);

        // zoom in to the specified selection
        ComAPI.InwOpState10 comState = COMBridge.State;
        comState.ZoomInCurViewOnSel(comSelectionOut);


        // Set Color 
        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverrideTemporaryColor(AItems, Color.Red);
        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverrideTemporaryColor(BItems, Color.Blue);
        visible.Invert(doc);
        // Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverrideTemporaryTransparency(visible, 1);
        // Add Current Viewpoint to the document
        // DocumentCurrentViewpoint currentViewpoint = Application.ActiveDocument.CurrentViewpoint;
        // Viewpoint viewpoint = currentViewpoint.ToViewpoint().CreateCopy();
        // SavedViewpoint newViewPoint = new SavedViewpoint(viewpoint);
        string name = test.DisplayName + ": " + clashResult.DisplayName;
        SavedViewpoint savedViewpoint = CreateViewPointCurrentView(name);
        SetAttributeCurrentView(savedViewpoint);
        MoveCurrentViewPointToFolder();
        // Adjust the camera angle
        doc.ActiveView.LookFromFrontRightTop();
        // Prevent redraw for every test and item
        doc.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.All);
        //oFolder.Children.Add(newViewPoint);
        // documentSavedViewpoints.AddCopy(oNewViewPt1);
    }

    public void TestSaveAttributeViewPoint()
    {
        List<OpView> opViews = new List<OpView>();
        foreach (var obj in COMBridge.State.SavedViews())
        {
            ComAPI.InwOpSavedView inwOpSavedView = (ComAPI.InwOpSavedView) obj;
            if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_View)
            {
                ComAPI.InwOpView nwOpView = (ComAPI.InwOpView) inwOpSavedView;
                opViews.Add(new OpView(nwOpView, String.Empty));
            }

            if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Folder)
            {
                ComAPI.InwOpFolderView inwOpFolderView = (ComAPI.InwOpFolderView) inwOpSavedView;
                string name = inwOpFolderView.name;
                this.recurseGroupView(inwOpFolderView, ref opViews, name);
            }
            // else if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Anim)
            // {
            //     ComAPI.InwOpAnimView inwOpAnimView = (ComAPI.InwOpAnimView)inwOpSavedView;
            //     string folderName = "Animation_" + inwOpAnimView.name;
            //     this.recurseGroupView(inwOpAnimView, opViews, folderName);
            // }
            // else if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Cut)
            // {
            //     ComAPI.InwOpCutView inwOpCutView = (ComAPI.InwOpCutView)inwOpSavedView;
            //     
            // }
        }

        MessageBox.Show(opViews.Count.ToString());
        opViews.First().SetAttribute();
    }

    private void recurseGroupView(ComAPI.InwOpGroupView parentSavedView, ref List<OpView> ilnviews, string folderName)
    {
        try
        {
            if (parentSavedView.SavedViews() == null || parentSavedView.SavedViews().Count == 0) return;
            foreach (object obj in parentSavedView.SavedViews())
            {
                ComAPI.InwOpSavedView inwOpSavedView = (ComAPI.InwOpSavedView) obj;
                if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_View)
                {
                    ComAPI.InwOpView opSavedView = (ComAPI.InwOpView) inwOpSavedView;
                    var opView = new OpView(opSavedView, folderName);
                    ilnviews.Add(opView);
                }

                if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Folder)
                {
                    ComAPI.InwOpFolderView inwOpFolderView = (ComAPI.InwOpFolderView) parentSavedView;
                    string name = inwOpFolderView.name;
                    this.recurseGroupView(inwOpFolderView, ref ilnviews, name);
                }
                // else if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Anim)
                // {
                //     ComAPI.InwOpAnimView inwOpAnimView = (ComAPI.InwOpAnimView)parentSavedView;
                //     string folderName2 = "Animation_" + inwOpAnimView.name;
                //     this.recurseGroupView(inwOpAnimView, ilnviews, folderName2);
                // }
                // else if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_Cut)
                // {
                //     ComAPI.InwOpCutView inwOpCutView = (ComAPI.InwOpCutView)inwOpSavedView;
                // }
            }
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Set ApplyHideAttribs and ApplyMaterialAttribs Current Viewpoint
    /// </summary>
    /// <param name="savedViewpoint"></param>
    public void SetAttributeCurrentView(SavedViewpoint savedViewpoint)
    {
        string vpDisplayName = savedViewpoint.DisplayName;
        foreach (var obj in COMBridge.State.SavedViews())
        {
            ComAPI.InwOpSavedView inwOpSavedView = (ComAPI.InwOpSavedView) obj;
            if (inwOpSavedView.Type == ComAPI.nwESavedViewType.eSavedViewType_View)
            {
                ComAPI.InwOpView nwOpView = (ComAPI.InwOpView) inwOpSavedView;
                if (nwOpView.name == vpDisplayName)
                {
                    var opView = new OpView(nwOpView, String.Empty);
                    opView.SetAttribute();
                }
            }
        }
    }

    private void MoveCurrentViewPointToFolder()
    {
        Document doc = Application.ActiveDocument;
        DocumentSavedViewpoints documentSavedViewpoints = doc.SavedViewpoints;
        DocumentSavedViewpoints oSavePts = documentSavedViewpoints;
        try

        {
            oSavePts.Move(documentSavedViewpoints.RootItem, documentSavedViewpoints.RootItem.Children.Count - 1,
                (GroupItem) doc.SavedViewpoints.Value.ElementAt(documentSavedViewpoints.RootItem.Children.Count - 3),
                0);
        }

        catch (Exception ex)

        {
            MessageBox.Show(ex.ToString());
        }
    }

    public void TestCreateAViewPointInFolder()
    {
        // Create a new folder with name Test and add the Viewpoint to the folder
        // new a folder
        DocumentSavedViewpoints documentSavedViewpoints = Application.ActiveDocument.SavedViewpoints;

        using (Transaction tran = new Transaction(Application.ActiveDocument, "Create Viewpoint"))
        {
            // Create a new folder
            FolderItem oFolder = new FolderItem();
            oFolder.DisplayName = "MyNewFolder";
            // Create a new Viewpoint in the folder
            SavedViewpoint savedViewpoint = new SavedViewpoint();
            savedViewpoint.DisplayName = "Test";
            savedViewpoint.Viewpoint.Position = new Point3D(0, 0, 0);
            // Add the Viewpoint to the folder
            oFolder.Children.Add(savedViewpoint);
            // Commit the transaction
            documentSavedViewpoints.AddCopy(oFolder);
            tran.Commit();
        }
    }

    public void GetJsonCamera()
    {
        DocumentCurrentViewpoint currentViewpoint = Application.ActiveDocument.CurrentViewpoint;
        string? json = currentViewpoint.Value.GetCamera();
        //write to file
        string documentFoler = System.IO.Path.GetDirectoryName(Application.ActiveDocument.FileName);
        string path = System.IO.Path.Combine(documentFoler, "Camera.json");
        System.IO.File.WriteAllText(path, json);
    }
}

public class OpView
{
    /// <summary>
    /// Folder Name of View
    /// </summary>
    public string FolderName { get; set; }


    /// <summary>
    /// View Of Com Api
    /// </summary>
    public ComAPI.InwOpView View { get; set; }

    public OpView(ComAPI.InwOpView opView, string folderName)
    {
        FolderName = folderName;
        View = opView;
    }

    /// <summary>
    /// Apply Set Attribute for this view
    /// </summary>
    public void SetAttribute()
    {
        View.ApplyHideAttribs = true;
        View.ApplyMaterialAttribs = true;
    }
}