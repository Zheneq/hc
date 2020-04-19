using System;

public class SpaceMarineDropBanner : Ability
{
	public int m_duration = 2;

	public float m_radius = 5f;

	public StatType m_allyEffectStat;

	public AbilityStatMod m_allyStatMod;

	public bool m_penetrateLineOfSight;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Drop Banner";
		}
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.m_radius, this.m_penetrateLineOfSight, true, true, -1);
	}
}
