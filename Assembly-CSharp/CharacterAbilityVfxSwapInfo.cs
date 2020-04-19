using System;

[Serializable]
public struct CharacterAbilityVfxSwapInfo
{
	public int VfxSwapForAbility0;

	public int VfxSwapForAbility1;

	public int VfxSwapForAbility2;

	public int VfxSwapForAbility3;

	public int VfxSwapForAbility4;

	public static int AbilityCount = 5;

	public void Reset()
	{
		this.VfxSwapForAbility0 = 0;
		this.VfxSwapForAbility1 = 0;
		this.VfxSwapForAbility2 = 0;
		this.VfxSwapForAbility3 = 0;
		this.VfxSwapForAbility4 = 0;
	}

	public int GetAbilityVfxSwapIdForAbility(int abilityIndex)
	{
		switch (abilityIndex)
		{
		case 0:
			return this.VfxSwapForAbility0;
		case 1:
			return this.VfxSwapForAbility1;
		case 2:
			return this.VfxSwapForAbility2;
		case 3:
			return this.VfxSwapForAbility3;
		case 4:
			return this.VfxSwapForAbility4;
		default:
			return -1;
		}
	}

	public void SetAbilityVfxSwapIdForAbility(int abilityIndex, int vfxSwapUniqueId)
	{
		switch (abilityIndex)
		{
		case 0:
			this.VfxSwapForAbility0 = vfxSwapUniqueId;
			break;
		case 1:
			this.VfxSwapForAbility1 = vfxSwapUniqueId;
			break;
		case 2:
			this.VfxSwapForAbility2 = vfxSwapUniqueId;
			break;
		case 3:
			this.VfxSwapForAbility3 = vfxSwapUniqueId;
			break;
		case 4:
			this.VfxSwapForAbility4 = vfxSwapUniqueId;
			break;
		}
	}

	public string ToIdString()
	{
		return string.Format("{0}/{1}/{2}/{3}/{4}", new object[]
		{
			this.VfxSwapForAbility0.ToString(),
			this.VfxSwapForAbility1.ToString(),
			this.VfxSwapForAbility2.ToString(),
			this.VfxSwapForAbility3.ToString(),
			this.VfxSwapForAbility4.ToString()
		});
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterAbilityVfxSwapInfo))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterAbilityVfxSwapInfo.Equals(object)).MethodHandle;
			}
			return false;
		}
		CharacterAbilityVfxSwapInfo characterAbilityVfxSwapInfo = (CharacterAbilityVfxSwapInfo)obj;
		if (this.VfxSwapForAbility0 == characterAbilityVfxSwapInfo.VfxSwapForAbility0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.VfxSwapForAbility1 == characterAbilityVfxSwapInfo.VfxSwapForAbility1 && this.VfxSwapForAbility2 == characterAbilityVfxSwapInfo.VfxSwapForAbility2)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.VfxSwapForAbility3 == characterAbilityVfxSwapInfo.VfxSwapForAbility3)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.VfxSwapForAbility4 == characterAbilityVfxSwapInfo.VfxSwapForAbility4;
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.VfxSwapForAbility0.GetHashCode() ^ this.VfxSwapForAbility1.GetHashCode() ^ this.VfxSwapForAbility2.GetHashCode() ^ this.VfxSwapForAbility3.GetHashCode() ^ this.VfxSwapForAbility4.GetHashCode();
	}
}
