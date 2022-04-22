using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Test;

public class traverseComponents  :  AddInPlugin
{
    public override int Execute(params string[] parameters)
    {
        StringBuilder sb = new StringBuilder();
        foreach (ModelItem item in

                 Autodesk.Navisworks.Api.Application

                     .ActiveDocument.Models.RootItemDescendantsAndSelf)
        {

            sb.Append(

                "Display Name: " + item.DisplayName +

                "; Class Display Name: " + item.ClassDisplayName +

                "\n");

        }

        string tempPath = Path.GetTempPath();
        string path  = Path.Combine(tempPath, "test.txt");
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(sb.ToString());
            streamWriter.Close();
        }
        MessageBox.Show(sb.Length.ToString());
        Process.Start(path);
        return 0;
    }

}