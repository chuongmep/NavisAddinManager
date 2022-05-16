using System.IO;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;
using NavisAddinManager.Model;
using NavisAddinManager.View;
using NavisAddinManager.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace NavisAddinManager.Command;

public sealed class AddinManagerBase
{
    public int ExecuteCommand(bool faceless, params string[] parameters)
    {
        var vm = new AddInManagerViewModel();
        if (_activeCmd != null && faceless)
        {
            return RunActiveCommand(vm, parameters);
        }
        var FrmAddInManager = new FrmAddInManager(vm);
        FrmAddInManager.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        FrmAddInManager.SetNavisAsWindowOwner();
        FrmAddInManager.ShowDialog();
        return 0;
    }

    public string ActiveTempFolder
    {
        get => _activeTempFolder;
        set => _activeTempFolder = value;
    }

    public int RunActiveCommand(AddInManagerViewModel vm, params string[] parameters)
    {
        var filePath = _activeCmd.FilePath;
        if (!File.Exists(filePath))
        {
            MessageBox.Show("File not found: " + filePath,DefaultSetting.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            return 0;
        }
        int result;
        try
        {
            vm.AssemLoader.HookAssemblyResolve();
            var assembly = vm.AssemLoader.LoadAddinsToTempFolder(filePath, false);
            if (null == assembly)
            {
                result = 0;
            }
            else
            {
                _activeTempFolder = vm.AssemLoader.TempFolder;
                if (assembly.CreateInstance(_activeCmdItem.FullClassName) is not AddInPlugin AddInPlugin)
                {
                    result = 0;
                }
                else
                {
                    _activeEc = AddInPlugin;
                    return _activeEc.Execute(parameters);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
            result = 0;
        }
        finally
        {
            vm.AssemLoader.UnhookAssemblyResolve();
            vm.AssemLoader.CopyGeneratedFilesBack();
        }

        return result;
    }

    public static AddinManagerBase Instance
    {
        get
        {
            if (_instance == null)
            {
#pragma warning disable RCS1059 // Avoid locking on publicly accessible instance.
                lock (typeof(AddinManagerBase))
                {
                    if (_instance == null)
                    {
                        _instance = new AddinManagerBase();
                    }
                }
#pragma warning restore RCS1059 // Avoid locking on publicly accessible instance.
            }

            return _instance;
        }
    }

    private AddinManagerBase()
    {
        _addinManager = new AddinManager();
        _activeCmd = null;
        _activeCmdItem = null;
        _activeApp = null;
        _activeAppItem = null;
    }

    public AddInPlugin ActiveEC
    {
        get => _activeEc;
        set => _activeEc = value;
    }

    public Addin ActiveCmd
    {
        get => _activeCmd;
        set => _activeCmd = value;
    }

    public AddinItem ActiveCmdItem
    {
        get => _activeCmdItem;
        set => _activeCmdItem = value;
    }

    public Addin ActiveApp
    {
        get => _activeApp;
        set => _activeApp = value;
    }

    public AddinItem ActiveAppItem
    {
        get => _activeAppItem;
        set => _activeAppItem = value;
    }

    public AddinManager AddinManager
    {
        get => _addinManager;
        set => _addinManager = value;
    }

    private string _activeTempFolder = string.Empty;

    private static volatile AddinManagerBase _instance;

    private AddInPlugin _activeEc;

    private Addin _activeCmd;

    private AddinItem _activeCmdItem;

    private Addin _activeApp;

    private AddinItem _activeAppItem;

    private AddinManager _addinManager;
}