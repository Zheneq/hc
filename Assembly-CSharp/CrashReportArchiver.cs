using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

public static class CrashReportArchiver
{
    private const int MAX_ARCHIVE_SIZE = 4194304;
    internal const int MAX_LOG_SIZE = 262144;

    internal static bool CreateArchiveFromCrashDumpDirectory(
        out string resultPath,
        out int resultArchiveNumBytes,
        string crashDumpDirectoryPath,
        string persistentDataPath,
        Dictionary<string, string> userKeyValues,
        BugReportType bugReportType = BugReportType.Crash)
    {
        resultPath = null;
        resultArchiveNumBytes = 0;
        try
        {
            string extraDirectoryPath = Path.Combine(crashDumpDirectoryPath, "extra");
            DirectoryInfo directoryInfo = Directory.CreateDirectory(extraDirectoryPath);
            if (!directoryInfo.Exists)
            {
                Log.Error("Failed to create " + extraDirectoryPath);
                return false;
            }

            string[] files = Directory.GetFiles(crashDumpDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string path = fileName.ToLower();
                if (Path.GetFileNameWithoutExtension(path) != "system_info" && Path.GetExtension(path) != ".dmp")
                {
                    string destFileName = Path.Combine(extraDirectoryPath, fileName);
                    File.Move(file, destFileName);
                }
            }

            files = Directory.GetFiles(extraDirectoryPath);
            string fullVersionString = null;
            foreach (string file in files)
            {
                switch (bugReportType)
                {
                    case BugReportType.Crash:
                    {
                        if (file.EndsWith("output_log.txt"))
                        {
                            fullVersionString = string.Empty;
                            using (StreamReader streamReader = new StreamReader(file))
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                string line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line.StartsWith("Hydrogen Version: "))
                                    {
                                        fullVersionString = line.Substring("Hydrogen Version: ".Length);
                                        string[] versionParts = fullVersionString.Split(',', ' ');
                                        if (versionParts.Length > 0 && !string.IsNullOrEmpty(versionParts[0]))
                                        {
                                            fullVersionString = versionParts[0];
                                        }
                                    }

                                    if (line.StartsWith("========== OUTPUTING STACK TRACE"))
                                    {
                                        break;
                                    }
                                }

                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    string trimmedLine = line.Trim();
                                    if (trimmedLine.Length > 0)
                                    {
                                        if (!line.StartsWith(" "))
                                        {
                                            stringBuilder.Append(trimmedLine);
                                        }
                                    }
                                    else if (stringBuilder.Length > 0)
                                    {
                                        break;
                                    }
                                }

                                string stackTrace = stringBuilder.ToString();
                                userKeyValues["StackTraceHash"] = $"{StringUtil.CaseInsensitiveHash(stackTrace):X8}";
                            }
                        }

                        break;
                    }
                    case BugReportType.Exception:
                    case BugReportType.Error:
                    {
                        if (file.EndsWith("UserMessage.txt"))
                        {
                            using (StreamReader streamReader = new StreamReader(file))
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                string line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    stringBuilder.Append(line.Trim());
                                }

                                string userMessage = stringBuilder.ToString();
                                userKeyValues["StackTraceHash"] = $"{StringUtil.CaseInsensitiveHash(userMessage):X8}";
                            }
                        }

                        break;
                    }
                }
            }

            CreateSystemInfoFile(crashDumpDirectoryPath, userKeyValues, bugReportType, fullVersionString);
            CompressResult compressResult = CompressDirectoryToPersistentDataPath(
                out resultPath,
                out resultArchiveNumBytes,
                crashDumpDirectoryPath,
                persistentDataPath,
                true);
            if (compressResult == CompressResult.FailureExceedsMaxArchiveSize)
            {
                compressResult = CompressDirectoryToPersistentDataPath(
                    out resultPath,
                    out resultArchiveNumBytes,
                    crashDumpDirectoryPath,
                    persistentDataPath,
                    false);
            }

            if (compressResult == CompressResult.Success)
            {
                Log.Info("Created crash report archive: " + resultPath);
                return true;
            }

            Log.Error("CrashReportArchiver.CompressDirectoryToPersistentDataPath failed: " + compressResult);
            return false;
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }

        return false;
    }

    internal static void DeleteArchives(string persistentDataPath)
    {
        try
        {
            FileInfo[] files = new DirectoryInfo(persistentDataPath).GetFiles("Channel150*.zip");
            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception exception)
                {
                    Log.Exception(exception);
                }
            }
        }
        catch (Exception exception2)
        {
            Log.Exception(exception2);
        }
    }

    private static string GetProcessName()
    {
        string text = Process.GetCurrentProcess().ProcessName;
        if (text == "Unity")
        {
            text = "AtlasReactor_UnityEditor";
        }

        return text;
    }

    private static CompressResult CompressDirectoryToPersistentDataPath(
        out string resultPath,
        out int resultArchiveBytes,
        string sourceDirectoryPath,
        string destinationDirectoryPath,
        bool includeLogFiles)
    {
        resultPath = null;
        resultArchiveBytes = 0;
        if (!Directory.Exists(sourceDirectoryPath))
        {
            Log.Warning("Cannot find directory '{0}'", sourceDirectoryPath);
            return CompressResult.Failure;
        }

        if (!Directory.Exists(destinationDirectoryPath))
        {
            Log.Warning("Cannot find directory '{0}'", destinationDirectoryPath);
            return CompressResult.Failure;
        }

        try
        {
            string[] files = Directory.GetFiles(sourceDirectoryPath);
            string[] directories = Directory.GetDirectories(sourceDirectoryPath);
            string processName = GetProcessName();
            string zipFileName = $"Channel150_{processName}_temp.zip";
            string archivePath = Path.Combine(destinationDirectoryPath, zipFileName);
            long archiveLength;
            using (FileStream fileStream = File.Create(archivePath))
            {
                using (ZipOutputStream zipOutputStream = new ZipOutputStream(fileStream))
                {
                    zipOutputStream.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (string file in files)
                    {
                        WriteFileToZip(file, Path.GetFileName(file), zipOutputStream, buffer);
                    }

                    if (includeLogFiles)
                    {
                        foreach (string logDirectoryPath in directories)
                        {
                            string logDirectoryName = Path.GetFileName(logDirectoryPath);
                            string[] logFiles = Directory.GetFiles(logDirectoryPath);
                            foreach (string logFile in logFiles)
                            {
                                string zipPath = logFile.LastIndexOf(".txt") < 0 && logFile.LastIndexOf(".htm") < 0
                                    ? Path.Combine(logDirectoryName, Path.GetFileName(logFile) + ".txt")
                                    : Path.Combine(logDirectoryName, Path.GetFileName(logFile));
                                WriteFileToZip(logFile, zipPath, zipOutputStream, buffer);
                            }
                        }
                    }

                    zipOutputStream.Finish();
                    archiveLength = fileStream.Length;
                    zipOutputStream.Close();
                }
            }

            if (archiveLength > MAX_ARCHIVE_SIZE)
            {
                return CompressResult.FailureExceedsMaxArchiveSize;
            }

            if (archiveLength <= 0)
            {
                return CompressResult.Failure;
            }

            resultPath = archivePath;
            resultArchiveBytes = (int)archiveLength;
            return CompressResult.Success;
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
        }

        return CompressResult.Failure;
    }

    private static void WriteFileToZip(string file, string fileZipPath, ZipOutputStream zipOutputStream, byte[] buffer)
    {
        ZipEntry zipEntry = new ZipEntry(fileZipPath)
        {
            DateTime = DateTime.Now
        };
        zipOutputStream.PutNextEntry(zipEntry);
        using (FileStream fileStream = File.OpenRead(file))
        {
            int bytesRead;
            do
            {
                bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                zipOutputStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }
    }

    private static bool CreateSystemInfoFile(
        string directoryPath,
        Dictionary<string, string> userKeyValues,
        BugReportType bugReportType,
        string fullVersionString)
    {
        string filePath = Path.Combine(directoryPath, "system_info.xml");
        Log.Info($"Creating system info file: {filePath}");
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, false, new UnicodeEncoding(false, true)))
            {
                streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>");
                streamWriter.WriteLine("<CrashCollector xmlns=\"http://www.trionworld.com\">");
                PopulateFiles(streamWriter);
                PopulateProcessInfo(streamWriter, userKeyValues);
                PopulateEnvironmentVariables(streamWriter);
                PopulateSystemInfo(streamWriter, userKeyValues, bugReportType, fullVersionString);
                PopulateKeyValueData(streamWriter, userKeyValues);
                streamWriter.WriteLine("</CrashCollector>");
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Error creating system info file. Exception {ex.ToString()}.");
            return false;
        }
    }

    private static void PopulateFiles(StreamWriter text)
    {
        text.WriteLine("  <IncludedFiles>");
        text.WriteLine("    <Item>crash.dmp</Item>");
        text.WriteLine("  </IncludedFiles>");
    }

    private static string TryGetMemoryCounter(Dictionary<string, string> userKeyValues, string key)
    {
        if (!userKeyValues.TryGetValue(key, out string value) || string.IsNullOrEmpty(value))
        {
            value = "0";
        }

        return SanitizeXml(value);
    }

    private static void PopulateProcessInfo(StreamWriter text, Dictionary<string, string> userKeyValues)
    {
        text.WriteLine("  <Process>");
        text.WriteLine(
            "    <PageFaultCount>" + TryGetMemoryCounter(userKeyValues, "MemoryPageFaultCount") + "</PageFaultCount>");
        text.WriteLine(
            "    <WorkingSetSize>" + TryGetMemoryCounter(userKeyValues, "MemoryWorkingSetSize") + "</WorkingSetSize>");
        text.WriteLine(
            "    <PeakWorkingSetSize>" + TryGetMemoryCounter(userKeyValues, "MemoryPeakWorkingSetSize")
                                       + "</PeakWorkingSetSize>");
        text.WriteLine(
            "    <PagefileUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPagefileUsage") + "</PagefileUsage>");
        text.WriteLine(
            "    <PeakPagefileUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPeakPagefileUsage")
                                      + "</PeakPagefileUsage>");
        text.WriteLine(
            "    <PrivateUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPrivateUsage") + "</PrivateUsage>");
        text.WriteLine(
            "    <VirtualMemoryAvailable>" + TryGetMemoryCounter(userKeyValues, "MemoryTotalVirtual")
                                           + "</VirtualMemoryAvailable>");
        text.WriteLine(
            "    <VirtualMemoryUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryUsedVirtual")
                                       + "</VirtualMemoryUsage>");
        text.WriteLine("  </Process>");
    }

    private static void PopulateEnvironmentVariables(StreamWriter text)
    {
        text.WriteLine("  <Environment>");
        text.WriteLine("    <Variables>");
        text.WriteLine("    </Variables>");
        text.WriteLine("  </Environment>");
    }

    private static void PopulateSystemInfo(
        StreamWriter text,
        Dictionary<string, string> userKeyValues,
        BugReportType bugReportType,
        string fullVersionString)
    {
        string buildNum = string.Empty;
        if (fullVersionString == null)
        {
            fullVersionString = BuildVersion.FullVersionString;
        }

        string[] versionParts = fullVersionString.Split('-');
        if (versionParts.Length > 1 && !string.IsNullOrEmpty(versionParts[1]))
        {
            buildNum = versionParts[1];
        }

        text.WriteLine("  <System>");
        text.WriteLine("    <MachineName>" + SanitizeXml(Environment.MachineName) + "</MachineName>");
        text.WriteLine("    <Channel>" + SanitizeXml("Channel_150") + "</Channel>");
        text.WriteLine("    <BuildVersion>" + SanitizeXml(fullVersionString) + "</BuildVersion>");
        text.WriteLine("    <BuildType>" + SanitizeXml(HydrogenConfig.Get().EnvironmentName) + "</BuildType>");
        string timeString = DateTime.Now.ToString("MM/dd/yy hh:mm:ss");
        text.WriteLine("    <Time>" + SanitizeXml(timeString) + "</Time>");
        text.WriteLine("    <BuildNum>" + SanitizeXml(buildNum) + "</BuildNum>");
        text.WriteLine("    <Application>" + SanitizeXml(GetProcessName()) + "</Application>");
        if (bugReportType == BugReportType.Crash)
        {
            text.WriteLine("    <ProcessName>" + SanitizeXml(GetProcessName()) + "</ProcessName>");
        }
        else
        {
            text.WriteLine(
                "    <ProcessName>" + SanitizeXml($"{GetProcessName()}_{bugReportType.ToString()}Report")
                                    + "</ProcessName>");
        }

        text.WriteLine("    <CorrelationID>" + SanitizeXml(Guid.NewGuid().ToString()) + "</CorrelationID>");
        string userName = HydrogenConfig.Get().Ticket != null ? HydrogenConfig.Get().Ticket.UserName : "NULL";
        text.WriteLine("    <UserID>" + SanitizeXml(userName) + "</UserID>");
        text.WriteLine(
            "    <TotalMemory>" + TryGetMemoryCounter(userKeyValues, "MemoryTotalPhysical") + "</TotalMemory>");
        text.WriteLine("    <UsedMemory>" + TryGetMemoryCounter(userKeyValues, "MemoryUsedPhysical") + "</UsedMemory>");
        PopulateNetworkCards(text);
        text.WriteLine("  </System>");
    }

    private static void PopulateNetworkCards(StreamWriter text)
    {
        text.WriteLine("    <NetworkCards>");
        try
        {
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in allNetworkInterfaces)
            {
                text.WriteLine("      <Card>");
                text.WriteLine("        <Address>");
                PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
                byte[] addressBytes = physicalAddress.GetAddressBytes();
                foreach (byte addressByte in addressBytes)
                {
                    text.Write(addressByte.ToString("x2"));
                }

                text.WriteLine("</Address>");
                text.WriteLine(
                    "        <Description>" + FormatEscapedXml("{0}", networkInterface.Description) + "</Description>");
                text.WriteLine("      </Card>");
            }
        }
        catch (Exception ex)
        {
            Log.Error("Exception trying to enumerate net cards for exception reporting: {0}:{1}", ex, ex.Message);
        }

        text.WriteLine("    </NetworkCards>");
    }

    private static void PopulateKeyValueData(StreamWriter text, Dictionary<string, string> userKeyValues)
    {
        text.WriteLine("  <UserKeyValue>");
        foreach (KeyValuePair<string, string> pair in userKeyValues)
        {
            text.WriteLine("    <Pair>");
            text.WriteLine("      <Key>" + SanitizeXml(pair.Key) + "</Key>");
            text.WriteLine("      <Value>" + SanitizeXml(pair.Value) + "</Value>");
            text.WriteLine("    </Pair>");
        }

        text.WriteLine("  </UserKeyValue>");
    }

    private static string SanitizeXml(string input)
    {
        string text = "XML sanitization error";
        try
        {
            text = input != null
                ? input
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&apos;")
                : "NULL";
            string pattern = "[^\\x09\\x0A\\x0D\\x20-\\xD7FF\\xE000-\\xFFFD\\x10000-x10FFFF]";
            text = Regex.Replace(text, pattern, string.Empty);
            if (text.Length >= 2048)
            {
                text = $"{text.Substring(0, 2044)}...";
            }

            return text;
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
            return text;
        }
    }

    private static string FormatEscapedXml<T1>(string format, T1 t1)
    {
        return SanitizeXml(string.Format(format, t1));
    }
}