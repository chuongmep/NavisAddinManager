//Add two new namespaces

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace Test;

//https://adndevblog.typepad.com/aec/2012/05/accessing-clash-report-information-using-net-api.html
public class AccessClashReport : AddInPlugin
{
    public override int Execute(params string[] parameters)

    {
        var oState = ComApiBridge.ComApiBridge.State;


        //find the clash detective plugin

        ComApi.InwOpClashElement m_clash = null;

        foreach (ComApi.InwBase oPlugin in oState.Plugins())

        {
            if (oPlugin.ObjectName == "nwOpClashElement")

            {
                m_clash = (ComApi.InwOpClashElement) oPlugin;

                break;
            }
        }


        if (m_clash == null)

        {
            System.Windows.Forms.MessageBox.Show(
                "cannot find clash test plugin!");

            return 0;
        }

        //Run all stored clash test

        m_clash.RunAllTests(null);


        // attach one custom porperty to the result

        // paths of one result of one test

        foreach (ComApi.InwOclClashTest clashTest
                 in m_clash.Tests())

        {
            if (clashTest.name == "Test 1")

            {
                foreach (ComApi.InwOclTestResult clashResult
                         in clashTest.results())

                {
                    // get a clash test named "Clash1"

                    if (clashResult.name == "Clash1")

                    {
                        //add property to one path

                        ComApi.InwOaPath3 oPath1 =
                            (ComApi.InwOaPath3) clashResult.Path1;


                        ComApi.InwGUIPropertyNode2 propn =
                            (ComApi.InwGUIPropertyNode2) oState.GetGUIPropertyNode(oPath1, true);


                        ComApi.InwOaPropertyVec newPvec =
                            (ComApi.InwOaPropertyVec) oState.ObjectFactory(
                                ComApi.nwEObjectType.eObjectType_nwOaPropertyVec,
                                null,
                                null);


                        ComApi.InwOaProperty newP =
                            (ComApi.InwOaProperty) oState.ObjectFactory(
                                ComApi.nwEObjectType.eObjectType_nwOaProperty,
                                null,
                                null);

                        newP.name = "demo_Property_Name";

                        newP.UserName = "demo_Property_UserName";

                        newP.value = "demo_Property_Value";


                        newPvec.Properties().Add(newP);


                        propn.SetUserDefined(0,
                            "demo_PropertyTab_Name",
                            "demo_PropertyTab_InteralName",
                            newPvec);


                        //path2.. if you need add property, too

                        ComApi.InwOaPath3 oPath2 =
                            (ComApi.InwOaPath3) clashResult.Path2;

                        //.....
                    }
                }
            }
        }


        //write reports to XML

        //........


        return 0;
    }
}