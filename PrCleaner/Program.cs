using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PrCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetProcesses().ToList().ForEach(prc =>
            {
                try
                {
                    if (prc.ProcessName.Contains(args[0]))
                    {
                        string cmd = GetCommandLine(prc);
                        if (cmd.Contains(args[1]))
                        {                            
                            Process.GetProcessById(prc.Id).Kill();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get commandline arguments for adb process id " + prc.Id);
                    Console.WriteLine(ex.Message);
                }
            });                
        }
        private static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder(process.MainModule.FileName);

            commandLine.Append(" ");
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                    commandLine.Append(" ");
                }
            }

            return commandLine.ToString();
        }
    }
}
