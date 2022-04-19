using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisAddinManager.Model;

public static class StaticUtil
{
    public static void ShowWarning(string msg)
    {
        MessageBox.Show(msg, DefaultSetting.AppName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
    public static string CommandFullName = typeof(AddInPlugin).FullName;
}