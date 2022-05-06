using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Test.Helpers;

namespace Test;

public class GetPropertiesFromClashTest  : INavisCommand
{
    List<string> Categories = new List<string>();
    public override void Action()
    {
        GetPropertysFromClashTest();
        IEnumerable<string> distinct = Categories.Distinct();
        Logger.WriteLine("=========================================================");
        Logger.WriteLine("Result Report:");
        Logger.WriteLine($"Total Categories:{distinct.Count()}");
        Logger.WriteLine($"Categorys:{string.Join(",", distinct)}");
        Logger.Open();
    }

    /// <summary>
    /// case user want get all properties full from clash
    /// </summary>
    void GetPropertysFromClashTest()
    {
        StringBuilder sb = new StringBuilder();
        Document doc = Application.ActiveDocument;
        DocumentClash documentClash = doc.GetClash();
        foreach (SavedItem savedItem in documentClash.TestsData.Tests)
        {
            ClashTest clashTest = (ClashTest) savedItem;
            SavedItemCollection savedItemCollection = clashTest.Children;
            foreach (SavedItem item in savedItemCollection)
            {
                ClashResult clashResult = item as ClashResult;
                if (clashResult != null)
                {
                    PropertyCategoryCollection categories = clashResult.CompositeItem1.PropertyCategories;
                    foreach (PropertyCategory category in categories)
                    {
                        GetPropertiesClashTest(category);
                    }
                    PropertyCategoryCollection categories2 = clashResult.CompositeItem2.PropertyCategories;
                    foreach (PropertyCategory category in categories2)
                    {
                        GetPropertiesClashTest(category);
                    }
                    //Only test for one clash result
                    break;
                }
               
            }

        }

    }
    void GetPropertiesClashTest(PropertyCategory propertyCategory)
    {
        Logger.WriteLine(propertyCategory.DisplayName);
        Categories.Add(propertyCategory.DisplayName);
        foreach (DataProperty dataProperty in propertyCategory.Properties)
        {
            Logger.Write($"Name:{dataProperty.Name}");
            Logger.Write($"║ DisplayName:{dataProperty.DisplayName}");
            Logger.Write($"║ Value:{dataProperty.Value.ToVarDisplayString()}");
            Logger.Write($"║ CombinedName:{dataProperty.CombinedName}");
            Logger.WriteLine("");

        }

    }
}