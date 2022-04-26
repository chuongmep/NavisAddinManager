using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Test;

public class Logger
{
    private static string logPath => Path.Combine(Path.GetTempPath(), "log.txt");
    
    public Logger WriteLine(string message)
    {
        if(!File.Exists(logPath))  File.Create(logPath).Close();
        using (StreamWriter st = new StreamWriter(logPath,true))
        {
           st.WriteLine(message);    
           st.Close();
        }

        return this;
    }
    public Logger Write(string message)
    {
        if(!File.Exists(logPath))  File.Create(logPath).Close();
        using (StreamWriter st = new StreamWriter(logPath,true))
        {
            st.Write(message);    
            st.Close();
        }

        return this;
    }

    public Logger Open()
    {
        Process.Start(logPath);
        return this;
    }
}