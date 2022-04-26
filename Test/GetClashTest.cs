using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Interop;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;

namespace Test;

public class GetClashTest : INavisCommand
{

    public override void Action()
    {
       
        GetInfoClash();
    }
    void GetInfoClash()
    {
        StringBuilder sb = new StringBuilder();
        Document doc = Application.ActiveDocument;
        List<ClashTest> savedItems = new List<ClashTest>();
        DocumentClash documentClash = doc.GetClash();
        foreach (SavedItem savedItem in documentClash.TestsData.Tests)
        {
            ClashTest clashTest = (ClashTest) savedItem;
            SavedItemCollection savedItemCollection = clashTest.Children;
            foreach (SavedItem item in savedItemCollection)
            {
                ClashResult clashResult = item as ClashResult;
                PropertyCategoryCollection categories = clashResult.Item1.PropertyCategories;
                foreach (PropertyCategory category in categories)
                {
                    GetPropertiesClashTest(category);
                }
                break;
            }

        }

        Logger.Open();

    }

    void GetPropertiesClashTest(PropertyCategory propertyCategory)
    {
        
        foreach (DataProperty dataProperty in propertyCategory.Properties)
        {
            Logger.Write($"||Name:{dataProperty.Name}");
            Logger.Write($"||DisplayName:{dataProperty.DisplayName}");
            Logger.Write($"||DisplayName:{dataProperty.Value}");
            Logger.Write("\n");

        }

    }
}