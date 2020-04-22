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
		ModForAbility0 = -1;
		ModForAbility1 = -1;
		ModForAbility2 = -1;
		ModForAbility3 = -1;
		ModForAbility4 = -1;
	}

	public int GetModForAbility(int abilityIndex)
	{
		switch (abilityIndex)
		{
		case 0:
			return ModForAbility0;
		case 1:
			return ModForAbility1;
		case 2:
			return ModForAbility2;
		case 3:
			return ModForAbility3;
		case 4:
			return ModForAbility4;
		default:
			return -1;
		}
	}

	public void SetModForAbility(int abilityIndex, int mod)
	{
		switch (abilityIndex)
		{
		case 0:
			ModForAbility0 = mod;
			break;
		case 1:
			ModForAbility1 = mod;
			break;
		case 2:
			ModForAbility2 = mod;
			break;
		case 3:
			ModForAbility3 = mod;
			break;
		case 4:
			ModForAbility4 = mod;
			break;
		}
	}

	public string ToIdString()
	{
		return $"{ModForAbility0.ToString()}/{ModForAbility1.ToString()}/{ModForAbility2.ToString()}/{ModForAbility3.ToString()}/{ModForAbility4.ToString()}";
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterModInfo))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		CharacterModInfo characterModInfo = (CharacterModInfo)obj;
		int result;
		if (ModForAbility0 == characterModInfo.ModForAbility0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (ModForAbility1 == characterModInfo.ModForAbility1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ModForAbility2 == characterModInfo.ModForAbility2 && ModForAbility3 == characterModInfo.ModForAbility3)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					result = ((ModForAbility4 == characterModInfo.ModForAbility4) ? 1 : 0);
					goto IL_0090;
				}
			}
		}
		result = 0;
		goto IL_0090;
		IL_0090:
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return ModForAbility0.GetHashCode() ^ ModForAbility1.GetHashCode() ^ ModForAbility2.GetHashCode() ^ ModForAbility3.GetHashCode() ^ ModForAbility4.GetHashCode();
	}
}
