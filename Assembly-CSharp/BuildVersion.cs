using System;
using System.Text;

public static class BuildVersion
{
	public const string BuildDescriptionPrefixString = "Hydrogen Version: ";

	private const int s_releaseNumber = 122;

	private const int s_buildNumber = 100;

	private const string s_buildLetter = "k";

	private const string s_changelistNumber = "265834";

	public static int ReleaseNumber
	{
		get { return 122; }
	}

	public static int BuildNumber
	{
		get { return 100; }
	}

	public static string BuildLetter
	{
		get { return "k".ToUpper(); }
	}

	public static string BranchName
	{
		get { return BuildBranch.BranchName; }
	}

	public static string ChangelistNumber
	{
		get { return "265834"; }
	}

	public static string MiniVersionString
	{
		get { return new StringBuilder().Append(BranchName).Append("-").Append(ReleaseNumber).ToString(); }
	}

	public static string ShortVersionString
	{
		get { return new StringBuilder().Append(BranchName).Append("-").Append(ReleaseNumber).Append("-").Append(BuildNumber).ToString(); }
	}

	public static string FullVersionString
	{
		get { return new StringBuilder().Append(BranchName).Append("-").Append(ReleaseNumber).Append("-").Append(BuildNumber).Append("-").Append(BuildLetter).Append("-").Append(ChangelistNumber).ToString(); }
	}

	public static string GetBuildDescriptionString(DateTime buildDate = default(DateTime), string buildHostName = null)
	{
		string text = string.Format("{0}{1}", "Hydrogen Version: ", FullVersionString);
		if (buildDate != DateTime.MinValue)
		{
			TimeSpan timeSpan = DateTime.UtcNow - buildDate;
			string arg;
			if (timeSpan.TotalHours < 1.0)
			{
				arg = new StringBuilder().AppendFormat("{0:0}", timeSpan.TotalMinutes).Append(" minutes ago").ToString();
			}
			else if (timeSpan.TotalDays < 1.0)
			{
				arg = new StringBuilder().AppendFormat("{0:0}", timeSpan.TotalHours).Append(" hours ago").ToString();
			}
			else
			{
				arg = new StringBuilder().AppendFormat("{0:0}", timeSpan.TotalDays).Append(" days ago").ToString();
			}
			string arg2 = ((DateTimeOffset)buildDate).ToLocalTime().ToString();
			text += new StringBuilder().Append(", built ").Append(arg).Append(" at ").Append(arg2).ToString();
		}
		if (!buildHostName.IsNullOrEmpty())
		{
			if (buildHostName != "unknown")
			{
				text += new StringBuilder().Append(" on ").Append(buildHostName).ToString();
			}
		}
		return text;
	}
}
