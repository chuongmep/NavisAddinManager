using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

public class SetHidden : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        // Create hidden collection

        ModelItemCollection hidden = new ModelItemCollection();

        // Add all the items that are visible to visible collection

        hidden.AddRange(Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems);
        // hide the remaining items

        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.SetHidden(hidden, true);
        return 0;
    }
}