using System.Text;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;
using Application = Autodesk.Navisworks.Api.Application;

namespace Test
{
    
    [Plugin("HelloWorld", "ChuongMep",DisplayName = "HelloWorld", ToolTip = "HelloWorld Navisworks AddinManager")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class HelloWorld : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            MessageBox.Show("Hello World",Application.Title);
            return 0;
        }
        
    }
    
    /// <summary>
    /// Only work for in AddinManager
    /// </summary>
    public class TestDontNeedUseAttClass : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            MessageBox.Show("Hello World",Application.Title);
            return 0;
        }
    }
    
    [Plugin("AddinManagerManual", "ChuongMep",DisplayName = "AddinManager Manual", ToolTip = "Addin Manager Manual")]
    [AddInPlugin(AddInLocation.AddIn)]
    public class TestAddInManagerManual : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            Test();
            return 0;
        }
        void Test()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Title:{Application.Title}");
            sb.AppendLine($"Version:{Application.Version}");
            sb.AppendLine($"ApiMajor:{Application.Version.ApiMajor}");
            sb.AppendLine($"ApiMajor:{Application.Version.ApiMajor}");
            sb.AppendLine($"IsApiStable:{Application.Version.IsApiStable}");
            sb.AppendLine($"SignInUserId:{Application.Gui.SignInUserId}");
            sb.AppendLine($"SignInUserName:{Application.Gui.SignInUserName}");
            sb.AppendLine($"Bim360 IsSignedIn: {Application.Bim360.IsSignedIn}");
            sb.AppendLine($"Build: {Application.Version.Build.ToString()}");
            sb.AppendLine($"Document Title: {Application.MainDocument.Title}");
            sb.AppendLine($"Document File Name: {Application.ActiveDocument.FileName}");
            sb.AppendLine($"Document File Name: {Application.ActiveDocument.Database.Value.State}");
            MessageBox.Show(sb.ToString());
        }
    }
}