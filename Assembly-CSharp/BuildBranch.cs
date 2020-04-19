using System;

public static class BuildBranch
{
	private const string s_branchName = "stable";

	public static string BranchName
	{
		get
		{
			return "stable".ToUpper();
		}
	}
}
