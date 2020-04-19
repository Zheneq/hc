using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactionGroup
{
	public string DisplayName;

	public string LongName;

	public int FactionGroupID;

	public string FilterDisplayStartTime;

	public string FilterDisplayEndTime;

	[Multiline]
	public string LoreDescription;

	public string ColorHex;

	public string BannerPath;

	public string IconPath;

	public string BannerIconPath;

	public string OutlinedIconPath;

	public List<CharacterType> Characters = new List<CharacterType>();

	public static string GetDisplayName(int factionGroupId)
	{
		return StringUtil.TR_FactionGroupName(factionGroupId);
	}
}
