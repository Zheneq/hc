using System;

[Serializable]
public class MapData
{
	public string Name;

	public string DisplayName;

	public string ResourceImageSpriteLocation;

	public string MinimapImageLocation;

	public float MinimapImageXScale = 1f;

	public float MinimapImageYScale = 1f;

	public bool IsValidForCustomGames;

	public string GetDisplayName()
	{
		return StringUtil.TR_MapName(this.Name);
	}
}
