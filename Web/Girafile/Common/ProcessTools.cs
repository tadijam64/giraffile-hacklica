using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Girafile.Common
{
    public class ProcessTools
    {
        // priprema okruženja za pokretanje python skripti
        public static System.Diagnostics.ProcessStartInfo pokreniProcesRelativnaPutanja()
        {
            return new System.Diagnostics.ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false
            };
        }

        public static void pokreniProces(string proces, string argumenti)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = pokreniProcesRelativnaPutanja();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = proces;
            startInfo.Arguments = argumenti;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}