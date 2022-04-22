using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

public class FocusItem : AddInPlugin
{
    public override int Execute(params string[] parameters)

    {
        //Create hidden collection

        ModelItemCollection hidden = new ModelItemCollection();

        //create a store for the visible items

        ModelItemCollection visible = new ModelItemCollection();


        //Add all the items that are visible to the visible collection

        foreach (ModelItem item in
                 Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems)

        {
            if (item.AncestorsAndSelf != null)

                visible.AddRange(item.AncestorsAndSelf);

            if (item.Descendants != null)

                visible.AddRange(item.Descendants);
        }


        //mark as invisible all the siblings of the visible items

        foreach (ModelItem toShow in visible)

        {
            if (toShow.Parent != null)

            {
                hidden.AddRange(toShow.Parent.Children);
            }
        }


        //remove the visible items from the collection

        foreach (ModelItem toShow in visible)

        {
            hidden.Remove(toShow);
        }


        //hide the remaining items

        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.SetHidden(hidden, true);


        return 0;
    }
}