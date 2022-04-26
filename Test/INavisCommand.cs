using Autodesk.Navisworks.Api.Plugins;

namespace Test;

public abstract class INavisCommand : AddInPlugin
{
    public abstract void Action();
    public override int Execute(params string[] parameters)
    {
        Action();
        return 0;
    }
    public Logger Logger = new Logger();
}