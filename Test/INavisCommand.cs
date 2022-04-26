using System;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

public abstract class INavisCommand : AddInPlugin
{
    public abstract void Action();
    public override int Execute(params string[] parameters)
    {
        try
        {
            Logger.Clear();
            Action();
        }
        catch (Exception e)
        {
            Logger.Write(e.ToString()).Open();
            Clipboard.SetText(e.Message);
        }
        return 0;
    }
    public Logger Logger = new Logger();
}