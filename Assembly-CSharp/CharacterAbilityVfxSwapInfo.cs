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
		VfxSwapForAbility0 = 0;
		VfxSwapForAbility1 = 0;
		VfxSwapForAbility2 = 0;
		VfxSwapForAbility3 = 0;
		VfxSwapForAbility4 = 0;
	}

	public int GetAbilityVfxSwapIdForAbility(int abilityIndex)
	{
		switch (abilityIndex)
		{
		case 0:
			return VfxSwapForAbility0;
		case 1:
			return VfxSwapForAbility1;
		case 2:
			return VfxSwapForAbility2;
		case 3:
			return VfxSwapForAbility3;
		case 4:
			return VfxSwapForAbility4;
		default:
			return -1;
		}
	}

	public void SetAbilityVfxSwapIdForAbility(int abilityIndex, int vfxSwapUniqueId)
	{
		switch (abilityIndex)
		{
		case 0:
			VfxSwapForAbility0 = vfxSwapUniqueId;
			break;
		case 1:
			VfxSwapForAbility1 = vfxSwapUniqueId;
			break;
		case 2:
			VfxSwapForAbility2 = vfxSwapUniqueId;
			break;
		case 3:
			VfxSwapForAbility3 = vfxSwapUniqueId;
			break;
		case 4:
			VfxSwapForAbility4 = vfxSwapUniqueId;
			break;
		}
	}

	public string ToIdString()
	{
		return $"{VfxSwapForAbility0.ToString()}/{VfxSwapForAbility1.ToString()}/{VfxSwapForAbility2.ToString()}/{VfxSwapForAbility3.ToString()}/{VfxSwapForAbility4.ToString()}";
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CharacterAbilityVfxSwapInfo))
		{
			while (true)
			{
				return false;
			}
		}
		CharacterAbilityVfxSwapInfo characterAbilityVfxSwapInfo = (CharacterAbilityVfxSwapInfo)obj;
		int result;
		if (VfxSwapForAbility0 == characterAbilityVfxSwapInfo.VfxSwapForAbility0)
		{
			if (VfxSwapForAbility1 == characterAbilityVfxSwapInfo.VfxSwapForAbility1 && VfxSwapForAbility2 == characterAbilityVfxSwapInfo.VfxSwapForAbility2)
			{
				if (VfxSwapForAbility3 == characterAbilityVfxSwapInfo.VfxSwapForAbility3)
				{
					result = ((VfxSwapForAbility4 == characterAbilityVfxSwapInfo.VfxSwapForAbility4) ? 1 : 0);
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
		return VfxSwapForAbility0.GetHashCode() ^ VfxSwapForAbility1.GetHashCode() ^ VfxSwapForAbility2.GetHashCode() ^ VfxSwapForAbility3.GetHashCode() ^ VfxSwapForAbility4.GetHashCode();
	}
}
