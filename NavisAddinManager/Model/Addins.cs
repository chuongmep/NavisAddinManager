using System.IO;
using System.Reflection;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisAddinManager.Model;

public abstract class Addins
{
    public SortedDictionary<string, Addin> AddinDict
    {
        get => addinDict;
        set => addinDict = value;
    }

    public int Count => addinDict.Count;

    public Addins()
    {
        addinDict = new SortedDictionary<string, Addin>();
    }

    public void SortAddin()
    {
        foreach (var addin in addinDict.Values)
        {
            addin.SortAddinItem();
        }
    }

    public void AddAddIn(Addin addin)
    {
        var fileName = Path.GetFileName(addin.FilePath);
        if (addinDict.ContainsKey(fileName))
        {
            addinDict.Remove(fileName);
        }
        addinDict[fileName] = addin;
    }

    public bool RemoveAddIn(Addin addin)
    {
        var fileName = Path.GetFileName(addin.FilePath);
        if (addinDict.ContainsKey(fileName))
        {
            addinDict.Remove(fileName);
            return true;
        }
        return false;
    }

    public void AddItem(AddinItem item)
    {
        var assemblyName = item.AssemblyName;
        if (!addinDict.ContainsKey(assemblyName))
        {
            addinDict[assemblyName] = new Addin(item.AssemblyPath);
        }
        addinDict[assemblyName].ItemList.Add(item);
    }

    public List<AddinItem> LoadItems(Assembly assembly, string fullName, string originalAssemblyFilePath, AddinType type)
    {
        var list = new List<AddinItem>();
        Type[] array;
        try
        {
            array = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            array = ex.Types;
            if (array == null)
            {
                return list;
            }
        }
        foreach (var type2 in array)
        {
            try
            {
                if (!(null == type2) && type2.IsSubclassOf(typeof(AddInPlugin)))
                {
                    var item = new AddinItem(originalAssemblyFilePath, Guid.NewGuid(), type2.FullName, type);
                    list.Add(item);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }
        return list;
    }

    protected SortedDictionary<string, Addin> addinDict;

    protected int maxCount = 100;

    protected int count;
}