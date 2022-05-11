using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisAddinManager.Command;

public abstract class IAddinCommand
{
    public abstract int Action(params string[] parameters);

    public void Execute(params string[] parameters)
    {
        try
        {
            Action(parameters);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
/// <summary>
/// Executer the command Addin Manager Manual
/// </summary>
public class AddInManagerManual : IAddinCommand
{
    public override int Action(params string[] parameters)
    {
        MyListener myListener = new MyListener();
        Debug.Listeners.Add(myListener);
        Trace.Listeners.Add(myListener);
        return AddinManagerBase.Instance.ExecuteCommand(false, parameters);
    }
}

/// <summary>
/// Execute the command Addin Manager Faceless
/// </summary>
public class AddInManagerFaceLess : IAddinCommand
{
    public override int Action(params string[] parameters)
    {
        return AddinManagerBase.Instance.ExecuteCommand(true, parameters);
    }
}

public class Test : IAddinCommand
{
    public override int Action(params string[] parameters)
    {
        MessageBox.Show("Hello World");
        return 0;
    }
}
public class MyListener : TraceListener
{
    public string path = @"C:\Users\vcho\OneDrive - ONG&ONG Pte Ltd\Documents\journal.txt";

    public override void Write(string message)
    {
        using (StreamWriter st = new StreamWriter(path, true))
        {
            st.Write(message);
            st.Close();
        }
    }

    public override void WriteLine(string message)
    {
        using (StreamWriter st = new StreamWriter(path, true))
        {
            st.WriteLine(message);
            st.Close();
        }
    }
}

