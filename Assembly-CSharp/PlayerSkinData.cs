using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSkinData
{
	public PlayerSkinData()
	{
		this.Patterns = new List<PlayerPatternData>();
	}

	public bool Unlocked { get; set; }

	public List<PlayerPatternData> Patterns { get; set; }

	public PlayerPatternData GetPattern(int i)
	{
		while (this.Patterns.Count <= i)
		{
			this.Patterns.Add(new PlayerPatternData());
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSkinData.GetPattern(int)).MethodHandle;
		}
		return this.Patterns[i];
	}

	public PlayerSkinData GetDeepCopy()
	{
		PlayerSkinData playerSkinData = new PlayerSkinData();
		playerSkinData.Unlocked = this.Unlocked;
		using (List<PlayerPatternData>.Enumerator enumerator = this.Patterns.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerPatternData playerPatternData = enumerator.Current;
				playerSkinData.Patterns.Add(playerPatternData.GetDeepCopy());
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerSkinData.GetDeepCopy()).MethodHandle;
			}
		}
		return playerSkinData;
	}
}
