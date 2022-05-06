using System.Diagnostics;
using System.IO;

namespace Test.Helpers;

public class Logger
{
    private static string logPath { get; set; }

    public Logger()
    {
        logPath = Path.Combine(Path.GetTempPath(), "log.txt");
    }

    public Logger Clear()
    {
        File.Delete(logPath);
        return this;
    }

    public Logger WriteLine(string message)
    {
        if (!File.Exists(logPath)) File.Create(logPath).Close();
        using (StreamWriter st = new StreamWriter(logPath, true))
        {
            st.WriteLine(message);
            st.Close();
        }

        return this;
    }

    public Logger Write(string message)
    {
        if (!File.Exists(logPath)) File.Create(logPath).Close();
        using (StreamWriter st = new StreamWriter(logPath, true))
        {
            st.Write(message);
            st.Close();
        }

        return this;
    }

    public Logger Open()
    {
        if (File.Exists(logPath))
        {
            Process.Start(logPath);
            return this;
        }
        return this;
    }
}