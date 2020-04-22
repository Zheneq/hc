using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSkinData
{
	public bool Unlocked
	{
		get;
		set;
	}

	public List<PlayerPatternData> Patterns
	{
		get;
		set;
	}

	public PlayerSkinData()
	{
		Patterns = new List<PlayerPatternData>();
	}

	public PlayerPatternData GetPattern(int i)
	{
		while (Patterns.Count <= i)
		{
			Patterns.Add(new PlayerPatternData());
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return Patterns[i];
		}
	}

	public PlayerSkinData GetDeepCopy()
	{
		PlayerSkinData playerSkinData = new PlayerSkinData();
		playerSkinData.Unlocked = Unlocked;
		using (List<PlayerPatternData>.Enumerator enumerator = Patterns.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerPatternData current = enumerator.Current;
				playerSkinData.Patterns.Add(current.GetDeepCopy());
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return playerSkinData;
					}
					/*OpCode not supported: LdMemberToken*/;
					return playerSkinData;
				}
			}
		}
	}
}
