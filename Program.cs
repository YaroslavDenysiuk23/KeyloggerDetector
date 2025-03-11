using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Net;

class KeyloggerDetector
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    static HashSet<string> trustedIPs = new HashSet<string>
    {
        "8.8.8.8", "8.8.4.4", 
        "1.1.1.1", "1.0.0.1",
        "208.67.222.222", "208.67.220.220" 
    };

    static void Main()
    {
        Console.WriteLine("🔍 Keylogger Detector started...\n");

        Thread processMonitor = new Thread(MonitorProcesses);
        processMonitor.Start();

        Thread keyloggerCheck = new Thread(DetectKeyloggingBehavior);
        keyloggerCheck.Start();

        Thread networkMonitor = new Thread(MonitorNetworkActivity);
        networkMonitor.Start();
    }

    static void MonitorProcesses()
    {
        string[] suspiciousProcesses = { "keylog", "logger", "stealer", "sniffer", "spy", "monitor", "recorder" };
        var detectedProcesses = new HashSet<string>();

        while (true)
        {
            var runningProcesses = Process.GetProcesses();
            foreach (var process in runningProcesses)
            {
                if (suspiciousProcesses.Any(p => process.ProcessName.ToLower().Contains(p)) && !detectedProcesses.Contains(process.ProcessName))
                {
                    Console.WriteLine($"⚠ Suspicious process detected: {process.ProcessName}");
                    detectedProcesses.Add(process.ProcessName);
                }
            }
            Thread.Sleep(5000);
        }
    }

    static void DetectKeyloggingBehavior()
    {
        int keyPressCount = 0;
        DateTime startTime = DateTime.Now;

        while (true)
        {
            for (int i = 1; i < 255; i++)
            {
                if (GetAsyncKeyState(i) != 0)
                {
                    keyPressCount++;
                }
            }

            if ((DateTime.Now - startTime).TotalSeconds > 10)
            {
                if (keyPressCount > 50)
                {
                    Console.WriteLine("⚠ High keyboard activity detected! Potential keylogger.");
                }
                keyPressCount = 0;
                startTime = DateTime.Now;
            }
            Thread.Sleep(100);
        }
    }

    static void MonitorNetworkActivity()
    {
        string[] suspiciousPorts = { "21", "23", "445", "3389", "8080", "9001" };
        var detectedConnections = new HashSet<string>();

        while (true)
        {
            Process netstat = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c netstat -ano",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            netstat.Start();
            string output = netstat.StandardOutput.ReadToEnd();
            netstat.WaitForExit();

            foreach (string line in output.Split('\n'))
            {
                var parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 5) continue;

                string localAddress = parts[1];
                string remoteAddress = parts[2].Split(':')[0];
                string port = parts[2].Split(':').Last();

                if (!trustedIPs.Contains(remoteAddress) && suspiciousPorts.Any(p => port == p) && !detectedConnections.Contains(remoteAddress))
                {
                    Console.WriteLine("⚠ Suspicious network connection detected: " + remoteAddress + ":" + port);
                    detectedConnections.Add(remoteAddress);
                }
            }
            Thread.Sleep(10000);
        }
    }
}