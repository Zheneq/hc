using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

public class SlashCommand_Log : SlashCommand
{
    public SlashCommand_Log()
        : base("/log", SlashCommandType.Everywhere)
    {
        PublicFacing = true;
    }

    public override void OnSlashCommand(string arguments)
    {
        OpenLogFile();
    }

    private void OpenLogFile()
    {
        try
        {
            string fileLogCurrentPath = ClientBootstrap.Instance.GetFileLogCurrentPath();
            if (!File.Exists(fileLogCurrentPath))
            {
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(BuildInfo.GetBuildInfoString());
            stringBuilder.AppendLine(fileLogCurrentPath);
            stringBuilder.AppendLine();
            List<string> list = new List<string>
            {
                "Connecting to lobby server from",
                "Assigned to game"
            };

            using (FileStream fileStream = new FileStream(
                       fileLogCurrentPath,
                       FileMode.Open,
                       FileAccess.Read,
                       FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string line = streamReader.ReadLine();
                    string text = list.SingleOrDefault(k => line.Contains(k));
                    if (text != null)
                    {
                        stringBuilder.AppendLine(line);
                    }
                }
            }


            string filePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), ".txt"));
            File.WriteAllText(filePath, stringBuilder.ToString());
            Process.Start(new ProcessStartInfo(filePath));
            WinUtils.OpenContainingFolder(fileLogCurrentPath);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
}