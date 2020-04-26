using System;

[Serializable]
public class PlayerColorData
{
	public bool Unlocked
	{
		get;
		set;
	}

	public PlayerColorData GetDeepCopy()
	{
		return MemberwiseClone() as PlayerColorData;
	}
}
