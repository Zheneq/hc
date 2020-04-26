using System;

public static class BuildVersion
{
	public const string BuildDescriptionPrefixString = "Hydrogen Version: ";

	private const int s_releaseNumber = 122;

	private const int s_buildNumber = 100;

	private const string s_buildLetter = "k";

	private const string s_changelistNumber = "265834";

	public static int ReleaseNumber => 122;

	public static int BuildNumber => 100;

	public static string BuildLetter => "k".ToUpper();

	public static string BranchName => BuildBranch.BranchName;

	public static string ChangelistNumber => "265834";

	public static string MiniVersionString => $"{BranchName}-{ReleaseNumber}";

	public static string ShortVersionString => $"{BranchName}-{ReleaseNumber}-{BuildNumber}";

	public static string FullVersionString => $"{BranchName}-{ReleaseNumber}-{BuildNumber}-{BuildLetter}-{ChangelistNumber}";

	public static string GetBuildDescriptionString(DateTime buildDate = default(DateTime), string buildHostName = null)
	{
		string text = string.Format("{0}{1}", "Hydrogen Version: ", FullVersionString);
		if (buildDate != DateTime.MinValue)
		{
			TimeSpan timeSpan = DateTime.UtcNow - buildDate;
			string arg;
			if (timeSpan.TotalHours < 1.0)
			{
				arg = $"{timeSpan.TotalMinutes:0} minutes ago";
			}
			else if (timeSpan.TotalDays < 1.0)
			{
				arg = $"{timeSpan.TotalHours:0} hours ago";
			}
			else
			{
				arg = $"{timeSpan.TotalDays:0} days ago";
			}
			string arg2 = ((DateTimeOffset)buildDate).ToLocalTime().ToString();
			text += $", built {arg} at {arg2}";
		}
		if (!buildHostName.IsNullOrEmpty())
		{
			if (buildHostName != "unknown")
			{
				text += $" on {buildHostName}";
			}
		}
		return text;
	}
}
