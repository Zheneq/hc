using System;
using System.Collections.Generic;

[Serializable]
public class PlayerPatternData
{
	public bool Unlocked;

	public List<PlayerColorData> Colors
	{
		get;
		set;
	}

	public PlayerPatternData()
	{
		Colors = new List<PlayerColorData>();
	}

	public PlayerColorData GetColor(int i)
	{
		while (Colors.Count <= i)
		{
			Colors.Add(new PlayerColorData());
		}
		while (true)
		{
			return Colors[i];
		}
	}

	public PlayerPatternData GetDeepCopy()
	{
		PlayerPatternData playerPatternData = new PlayerPatternData();
		playerPatternData.Unlocked = Unlocked;
		using (List<PlayerColorData>.Enumerator enumerator = Colors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerColorData current = enumerator.Current;
				playerPatternData.Colors.Add(current.GetDeepCopy());
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return playerPatternData;
					}
					/*OpCode not supported: LdMemberToken*/;
					return playerPatternData;
				}
			}
		}
	}
}
