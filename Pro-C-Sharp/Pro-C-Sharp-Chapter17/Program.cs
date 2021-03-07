﻿using System;
using System.Diagnostics;
using System.Linq;

namespace Pro_C_Sharp_Chapter17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Processes *****\n");
            ListAllRunningProcesses();
            // Prompt user for a PID and print out the set of active threads.
            Console.WriteLine("***** Enter PID of process to investigate *****");
            Console.Write("PID: ");
            string pID = Console.ReadLine();
            int theProcID = int.Parse(pID);
            EnumThreadsForPid(theProcID);
            EnumModsForPid(theProcID);
            StartAndKillProcess();
            Console.ReadLine();
        }

        static void ListAllRunningProcesses()
        {
            // Get all the processes on the local machine, ordered by
            // PID.
            var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
            // Print out PID and name of each process.
            foreach (var p in runningProcs)
            {
                string info = $"-> PID: {p.Id}\tName: {p.ProcessName}";
                Console.WriteLine(info);
            }
            Console.WriteLine("************************************\n");
        }
        static void GetSpecificProcess()
        {
            Process theProc = null;
            try
            {
                theProc = Process.GetProcessById(987);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void EnumThreadsForPid(int pID)
        {
            Process theProc = null;
            try
            {
                theProc = Process.GetProcessById(pID);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            // List out stats for each thread in the specified process.
            Console.WriteLine("Here are the threads used by: {0}", theProc.ProcessName);
            ProcessThreadCollection theThreads = theProc.Threads;
            foreach (ProcessThread pt in theThreads)
            {
                string info =
                    $"-> Thread ID: {pt.Id}\tStart Time: {pt.StartTime.ToShortTimeString()}\tPriority:" +
                    $"{ pt.PriorityLevel}";
                Console.WriteLine(info);
            }
            Console.WriteLine("************************************\n");
        }

        static void EnumModsForPid(int pID)
        {
            Process theProc = null;
            try
            {
                theProc = Process.GetProcessById(pID);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Here are the loaded modules for: {0}", theProc.ProcessName);
            ProcessModuleCollection theMods = theProc.Modules;
            foreach (ProcessModule pm in theMods)
            {
                string info = $"-> Mod Name: {pm.ModuleName}";
                Console.WriteLine(info);
            }
            Console.WriteLine("************************************\n");
        }

        static void StartAndKillProcess()
        {
            Process ffProc = null;
            // Launch Firefox, and go to Facebook!
            try
            {
                ffProc = Process.Start("FireFox.exe", "www.facebook.com");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Write("--> Hit enter to kill {0}...", ffProc.ProcessName);
            Console.ReadLine();
            // Kill the iexplore.exe process.
            try
            {
                ffProc.Kill();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
