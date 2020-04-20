using System;
using System.Collections.Generic;

[Serializable]
public class PlayerPatternData
{
	public bool Unlocked;

	public PlayerPatternData()
	{
		this.Colors = new List<PlayerColorData>();
	}

	public List<PlayerColorData> Colors { get; set; }

	public PlayerColorData GetColor(int i)
	{
		while (this.Colors.Count <= i)
		{
			this.Colors.Add(new PlayerColorData());
		}
		return this.Colors[i];
	}

	public PlayerPatternData GetDeepCopy()
	{
		PlayerPatternData playerPatternData = new PlayerPatternData();
		playerPatternData.Unlocked = this.Unlocked;
		using (List<PlayerColorData>.Enumerator enumerator = this.Colors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerColorData playerColorData = enumerator.Current;
				playerPatternData.Colors.Add(playerColorData.GetDeepCopy());
			}
		}
		return playerPatternData;
	}
}
