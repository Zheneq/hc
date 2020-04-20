using System;
using UnityEngine;

[Serializable]
public class AbilityModPropertyConeInfo
{
	public AbilityModPropertyFloat m_radiusMod = new AbilityModPropertyFloat();

	public AbilityModPropertyFloat m_widthAngleMod = new AbilityModPropertyFloat();

	public AbilityModPropertyFloat m_backwardsOffsetMod = new AbilityModPropertyFloat();

	public AbilityModPropertyBool m_penetrateLosMod = new AbilityModPropertyBool();

	[Space(10f)]
	public AbilityModPropertyBool m_affectsEnemyOverride = new AbilityModPropertyBool();

	public AbilityModPropertyBool m_affectsAllyOverride = new AbilityModPropertyBool();

	public AbilityModPropertyBool m_affectsCasterOverride = new AbilityModPropertyBool();

	public ConeTargetingInfo GetModifiedValue(ConeTargetingInfo info)
	{
		if (info != null)
		{
			return info.GetModifiedCopy(this);
		}
		if (Application.isEditor)
		{
			Debug.LogError("null passed in to generate modified modded LaserTargetingInfo, returning an instance with default values");
		}
		return new ConeTargetingInfo();
	}
}
