using System;

[Serializable]
public struct CharacterModInfo
{
	public int ModForAbility0;

	public int ModForAbility1;

	public int ModForAbility2;

	public int ModForAbility3;

	public int ModForAbility4;

	public static int AbilityCount = 5;

	public void Reset()
	{
		this.ModForAbility0 = -1;
		this.ModForAbility1 = -1;
		this.ModForAbility2 = -1;
		this.ModForAbility3 = -1;
		this.ModForAbility4 = -1;
	}

	public int GetModForAbility(int abilityIndex)
	{
		switch (abilityIndex)
		{
		case 0:
			return this.ModForAbility0;
		case 1:
			return this.ModForAbility1;
		case 2:
			return this.ModForAbility2;
		case 3:
			return this.ModForAbility3;
		case 4:
			return this.ModForAbility4;
		default:
			return -1;
		}
	}

	public void SetModForAbility(int abilityIndex, int mod)
	{
		switch (abilityIndex)
		{
		case 0:
			this.ModForAbility0 = mod;
			break;
		case 1:
			this.ModForAbility1 = mod;
			break;
		case 2:
			this.ModForAbility2 = mod;
			break;
		case 3:
			this.ModForAbility3 = mod;
			break;
		case 4:
			this.ModForAbility4 = mod;
			break;
		}
	}

	public string ToIdString()
	{
		return string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
		{
			this.ModForAbility0.ToString(),
			this.ModForAbility1.ToString(),
			this.ModForAbility2.ToString(),
			this.ModForAbility3.ToString(),
			this.ModForAbility4.ToString()
		});
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterModInfo))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterModInfo.Equals(object)).MethodHandle;
			}
			return false;
		}
		CharacterModInfo characterModInfo = (CharacterModInfo)obj;
		if (this.ModForAbility0 == characterModInfo.ModForAbility0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.ModForAbility1 == characterModInfo.ModForAbility1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.ModForAbility2 == characterModInfo.ModForAbility2 && this.ModForAbility3 == characterModInfo.ModForAbility3)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.ModForAbility4 == characterModInfo.ModForAbility4;
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.ModForAbility0.GetHashCode() ^ this.ModForAbility1.GetHashCode() ^ this.ModForAbility2.GetHashCode() ^ this.ModForAbility3.GetHashCode() ^ this.ModForAbility4.GetHashCode();
	}
}
