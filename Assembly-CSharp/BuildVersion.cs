using System;

public static class BuildVersion
{
	public const string BuildDescriptionPrefixString = "Hydrogen Version: ";

	private const int s_releaseNumber = 122;
	private const int s_buildNumber = 100;
	private const string s_buildLetter = "k";
	private const string s_changelistNumber = "265834";
	
	// NOTE custom
	public const string s_version =
		ThisAssembly.Git.BaseVersion.Major + "." + 
		ThisAssembly.Git.BaseVersion.Minor + 
#if SERVER
		(ThisAssembly.Git.BaseVersion.Patch != "0" || ThisAssembly.Git.Commits != "0" ? "." + ThisAssembly.Git.BaseVersion.Patch : "") +
		(ThisAssembly.Git.Commits != "0" ? "." + ThisAssembly.Git.Commits : "") +
		ThisAssembly.Git.SemVer.DashLabel
#else
		(ThisAssembly.Git.SemVer.Patch != "0" ? "." + ThisAssembly.Git.SemVer.Patch : "") +
		(ThisAssembly.Git.SemVer.DashLabel != "-client" ? ThisAssembly.Git.SemVer.DashLabel : "")
#endif
#if VANILLA
		+ "-vanilla"
#endif
		;

	public static int ReleaseNumber => s_releaseNumber;
	public static int BuildNumber => s_buildNumber;
	public static string BuildLetter => s_buildLetter.ToUpper();
	public static string BranchName => BuildBranch.BranchName;
	public static string ChangelistNumber => s_changelistNumber;
#if SERVER
	public static string BuildTag => s_version; // custom
#endif
	public static string MiniVersionString => $"{BranchName}-{ReleaseNumber}";
	
#if VANILLA
	public static string ShortVersionString => $"{BranchName}-{ReleaseNumber}-{BuildNumber}";
#else
	public static string ShortVersionString => $"{BranchName}-{ReleaseNumber}-{BuildNumber}_{s_version}";
#endif
	
	public static string FullVersionString => $"{BranchName}-{ReleaseNumber}-{BuildNumber}-{BuildLetter}-{ChangelistNumber}";

	public static string GetBuildDescriptionString(DateTime buildDate = default(DateTime), string buildHostName = null)
	{
		string text = $"{BuildDescriptionPrefixString}{FullVersionString}";
		if (buildDate != DateTime.MinValue)
		{
			TimeSpan timeSpan = DateTime.UtcNow - buildDate;
			string timespanString;
			if (timeSpan.TotalHours < 1.0)
			{
				timespanString = $"{timeSpan.TotalMinutes:0} minutes ago";
			}
			else if (timeSpan.TotalDays < 1.0)
			{
				timespanString = $"{timeSpan.TotalHours:0} hours ago";
			}
			else
			{
				timespanString = $"{timeSpan.TotalDays:0} days ago";
			}
			string dateString = ((DateTimeOffset)buildDate).ToLocalTime().ToString();
			text += $", built {timespanString} at {dateString}";
		}
		if (!buildHostName.IsNullOrEmpty() && buildHostName != "unknown")
		{
			text += $" on {buildHostName}";
		}
		return text;
	}
}
