﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Optimizer
{
    public static class HostsHelper
    {
        internal static string nl = Environment.NewLine;
        internal static readonly string HostsFile = CleanHelper.System32Folder + "\\drivers\\etc\\hosts";

        internal static void RestoreDefaultHosts()
        {
            try
            {
                if (File.Exists(HostsFile))
                {
                    File.Delete(HostsFile);
                }

                File.WriteAllBytes(HostsFile, Properties.Resources.hosts);
            }
            catch { }
        }

        internal static string[] ReadHosts()
        {
            return File.ReadAllLines(HostsFile);
        }

        internal static void LocateHosts()
        {
            CleanHelper.FindFile(HostsFile);
        }

        internal static void SaveHosts(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].StartsWith("#") && (!string.IsNullOrEmpty(lines[i])))
                {
                    lines[i] = SanitizeEntry(lines[i]);
                }
            }

            File.WriteAllText(HostsFile, string.Empty);
            File.WriteAllLines(HostsFile, lines);
        }

        internal static List<string> GetEntries()
        {
            List<string> entries = new List<string>();

            string[] lines = File.ReadAllLines(HostsFile);

            foreach (string line in lines)
            {
                if (!line.StartsWith("#") && (!string.IsNullOrEmpty(line)))
                {
                    entries.Add(line.Replace(" ", " : "));
                }
            }

            return entries;
        }

        internal static void AddEntry(string ipdomain)
        {
            try
            {
                File.AppendAllText(HostsFile, nl + ipdomain);
            }
            catch { }
        }

        internal static void RemoveEntry(string ipdomain)
        {
            try
            {
                File.WriteAllLines(HostsFile, File.ReadLines(HostsFile).Where(l => l != ipdomain).ToList());
            }
            catch { }
        }

        internal static void RemoveAllEntries(List<string> collection)
        {
            try
            {
                foreach (string text in collection)
                {
                    File.WriteAllLines(HostsFile, File.ReadLines(HostsFile).Where(l => l != text).ToList());
                }
            }
            catch { }
        }

        internal static string SanitizeEntry(string entry)
        {
            // remove multiple white spaces and keep only one
            return Regex.Replace(entry, @"\s{2,}", " ");
        }
    }
}
