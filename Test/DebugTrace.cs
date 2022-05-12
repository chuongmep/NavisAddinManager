using System.Diagnostics;
using Autodesk.Navisworks.Api.Plugins;


public class DebugTrace  : AddInPlugin
{
    public override int Execute(params string[] parameters)
    {

        Trace.WriteLine($"Warning: This is a warning");
        Trace.WriteLine($"Error: This is a error");
        Trace.WriteLine($"Add: This is a add");
        Trace.WriteLine($"Modify: This is a modify");
        Trace.WriteLine($"Delete: This is a delete");
        return 0;
    }
}