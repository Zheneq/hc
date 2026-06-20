using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertyLaserInfo
{
	public AbilityModPropertyFloat m_rangeMod = new AbilityModPropertyFloat();

	public AbilityModPropertyFloat m_widthMod = new AbilityModPropertyFloat();

	public AbilityModPropertyInt m_maxTargetsMod = new AbilityModPropertyInt();

	public AbilityModPropertyBool m_penetrateLosOverride = new AbilityModPropertyBool();

	[Space(10f)]
	public AbilityModPropertyBool m_affectsEnemyOverride = new AbilityModPropertyBool();

	public AbilityModPropertyBool m_affectsAllyOverride = new AbilityModPropertyBool();

	public AbilityModPropertyBool m_affectsCasterOverride = new AbilityModPropertyBool();

	public LaserTargetingInfo GetModifiedValue(LaserTargetingInfo info)
	{
		if (info != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return info.GetModifiedCopy(this);
				}
			}
		}
		if (Application.isEditor)
		{
			Debug.LogError("null passed in to generate modified modded LaserTargetingInfo, returning an instance with default values");
		}
		return new LaserTargetingInfo();
	}

	public float GetRange(LaserTargetingInfo info)
	{
		return m_rangeMod.GetModifiedValue(info.range);
	}

	public float GetWidth(LaserTargetingInfo info)
	{
		return m_widthMod.GetModifiedValue(info.width);
	}

	public int GetMaxTargets(LaserTargetingInfo info)
	{
		return m_maxTargetsMod.GetModifiedValue(info.maxTargets);
	}

	public bool GetPenetrateLos(LaserTargetingInfo info)
	{
		return m_penetrateLosOverride.GetModifiedValue(info.penetrateLos);
	}

	public bool GetAffectsEnemy(LaserTargetingInfo info)
	{
		return m_affectsEnemyOverride.GetModifiedValue(info.affectsEnemies);
	}

	public bool GetAffectsAlly(LaserTargetingInfo info)
	{
		return m_affectsAllyOverride.GetModifiedValue(info.affectsAllies);
	}

	public bool GetAffectsCaster(LaserTargetingInfo info)
	{
		return m_affectsCasterOverride.GetModifiedValue(info.affectsCaster);
	}
}
