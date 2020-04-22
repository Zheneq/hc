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
		base.PublicFacing = true;
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
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(BuildInfo.GetBuildInfoString());
			stringBuilder.AppendLine(fileLogCurrentPath);
			stringBuilder.AppendLine();
			List<string> list = new List<string>();
			list.Add("Connecting to lobby server from");
			list.Add("Assigned to game");
			FileStream fileStream = new FileStream(fileLogCurrentPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			try
			{
				StreamReader streamReader = new StreamReader(fileStream);
				try
				{
					while (streamReader.Peek() >= 0)
					{
						string line = streamReader.ReadLine();
						string text = list.Where((string k) => line.Contains(k)).SingleOrDefault();
						if (text != null)
						{
							stringBuilder.AppendLine(line);
						}
					}
				}
				finally
				{
					if (streamReader != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								((IDisposable)streamReader).Dispose();
								goto end_IL_00db;
							}
						}
					}
					end_IL_00db:;
				}
			}
			finally
			{
				if (fileStream != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							((IDisposable)fileStream).Dispose();
							goto end_IL_00f3;
						}
					}
				}
				end_IL_00f3:;
			}
			string text2 = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), ".txt"));
			File.WriteAllText(text2, stringBuilder.ToString());
			Process.Start(new ProcessStartInfo(text2));
			WinUtils.OpenContainingFolder(fileLogCurrentPath);
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
}
