using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

 [Plugin("SearchComparisonPlugIn.SearchComparisonPlugIn", "ADSK",
      DisplayName="Search Comparison",
      ToolTip="Compares the time taken for different search techniques on a Navisworks Document")]
   public class SearchComparisonPlugIn : AddInPlugin
   {
      public override int Execute(params string[] parameters)
      {
         StringBuilder output = new StringBuilder(1000);
         output.Append("Searching for Visible ModelItems which contain geometry, returning a ModelItemCollection\n");
         output.Append("Time format is (mm:ss.fff)\n");

         /////////////////////////////////////////////////////////////////////////////
         //iterative method
         DateTime start = DateTime.Now;

         ModelItemCollection searchResults = new ModelItemCollection();
         foreach (ModelItem modelItem in Autodesk.Navisworks.Api.Application.ActiveDocument.Models.CreateCollectionFromRootItems().DescendantsAndSelf)
         {
            //find the model items with the Geometry, not hidden
            if (modelItem.HasGeometry && !modelItem.IsHidden)
               searchResults.Add(modelItem);
         }

         DateTime end = DateTime.Now;
         DateTime taken = new DateTime(end.Ticks - start.Ticks);
         output.Append("Iterative method\t");
         output.Append(taken.ToString("mm:ss.fff"));
         output.Append(" to find ");
         output.Append(searchResults.Count);
         output.Append(" ModelItems\n");

         /////////////////////////////////////////////////////////////////////////////
         //using LINQ style query

         start = DateTime.Now;

         ModelItemCollection searchResults1 = new ModelItemCollection();
         searchResults1.CopyFrom(
            Autodesk.Navisworks.Api.Application.ActiveDocument.Models.RootItemDescendantsAndSelf.
            Where(x =>
               //find the ModelItems with the Geometry, not hidden
               (x.HasGeometry && !x.IsHidden)));

         end = DateTime.Now;
         taken = new DateTime(end.Ticks - start.Ticks);
         output.Append("LINQ query\t");
         output.Append(taken.ToString("mm:ss.fff"));
         output.Append(" to find ");
         output.Append(searchResults1.Count);
         output.Append(" ModelItems\n");

         /////////////////////////////////////////////////////////////////////////////
         //Searching purely with Search class

         start = DateTime.Now;

         //Create a new search
         Search s2 = new Search();

         //Add a search condition for ModelItems with Geometry only...
         s2.SearchConditions.Add(SearchCondition.HasCategoryByName(PropertyCategoryNames.Geometry));

         //...and not hidden
         s2.SearchConditions.Add(SearchCondition.HasPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemHidden).EqualValue(VariantData.FromBoolean(false)));

         //set the selection to everything
         s2.Selection.SelectAll();
         s2.Locations = SearchLocations.DescendantsAndSelf;

         //get the resulting collection by applying this search
         ModelItemCollection searchResults2 = s2.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);

         end = DateTime.Now;
         taken = new DateTime(end.Ticks - start.Ticks);
         output.Append("Search query\t");
         output.Append(taken.ToString("mm:ss.fff"));
         output.Append(" to find ");
         output.Append(searchResults2.Count);
         output.Append(" ModelItems\n");


         /////////////////////////////////////////////////////////////////////////////
         //Mixture search
         start = DateTime.Now;

         //Create a new search
         Search s3 = new Search();

         //Add a search condition for those ModelItems with Geometry
         s3.SearchConditions.Add(SearchCondition.HasCategoryByName(PropertyCategoryNames.Geometry));

         //set the selection to everything
         s3.Selection.SelectAll();
         s3.Locations = SearchLocations.DescendantsAndSelf;

         //get the resulting collection by applying this search
         ModelItemCollection searchResults3 = new ModelItemCollection();
         searchResults3.CopyFrom(
            s3.FindIncremental(Autodesk.Navisworks.Api.Application.ActiveDocument, false)
            .Where(x =>
               //find the model items, not hidden
               (!x.IsHidden)));

         end = DateTime.Now;
         taken = new DateTime(end.Ticks - start.Ticks);
         output.Append("Mixed query\t");
         output.Append(taken.ToString("mm:ss.fff"));
         output.Append(" to find ");
         output.Append(searchResults3.Count);
         output.Append(" ModelItems\n");

         MessageBox.Show(output.ToString());
         return 0;
      }
   }