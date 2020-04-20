using System;

public static class BuildVersion
{
	public const string BuildDescriptionPrefixString = "Hydrogen Version: ";

	private const int s_releaseNumber = 0x7A;

	private const int s_buildNumber = 0x64;

	private const string s_buildLetter = "k";

	private const string s_changelistNumber = "265834";

	public static int ReleaseNumber
	{
		get
		{
			return 0x7A;
		}
	}

	public static int BuildNumber
	{
		get
		{
			return 0x64;
		}
	}

	public static string BuildLetter
	{
		get
		{
			return "k".ToUpper();
		}
	}

	public static string BranchName
	{
		get
		{
			return BuildBranch.BranchName;
		}
	}

	public static string ChangelistNumber
	{
		get
		{
			return "265834";
		}
	}

	public static string MiniVersionString
	{
		get
		{
			return string.Format("{0}-{1}", BuildVersion.BranchName, BuildVersion.ReleaseNumber);
		}
	}

	public static string ShortVersionString
	{
		get
		{
			return string.Format("{0}-{1}-{2}", BuildVersion.BranchName, BuildVersion.ReleaseNumber, BuildVersion.BuildNumber);
		}
	}

	public static string FullVersionString
	{
		get
		{
			return string.Format("{0}-{1}-{2}-{3}-{4}", new object[]
			{
				BuildVersion.BranchName,
				BuildVersion.ReleaseNumber,
				BuildVersion.BuildNumber,
				BuildVersion.BuildLetter,
				BuildVersion.ChangelistNumber
			});
		}
	}

	public static string GetBuildDescriptionString(DateTime buildDate = default(DateTime), string buildHostName = null)
	{
		string text = string.Format("{0}{1}", "Hydrogen Version: ", BuildVersion.FullVersionString);
		if (buildDate != DateTime.MinValue)
		{
			TimeSpan timeSpan = DateTime.UtcNow - buildDate;
			string arg;
			if (timeSpan.TotalHours < 1.0)
			{
				arg = string.Format("{0:0} minutes ago", timeSpan.TotalMinutes);
			}
			else if (timeSpan.TotalDays < 1.0)
			{
				arg = string.Format("{0:0} hours ago", timeSpan.TotalHours);
			}
			else
			{
				arg = string.Format("{0:0} days ago", timeSpan.TotalDays);
			}
			string arg2 = buildDate.ToLocalTime().ToString();
			text += string.Format(", built {0} at {1}", arg, arg2);
		}
		if (!buildHostName.IsNullOrEmpty())
		{
			if (buildHostName != "unknown")
			{
				text += string.Format(" on {0}", buildHostName);
			}
		}
		return text;
	}
}
