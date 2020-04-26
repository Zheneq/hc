public class SpaceMarineDropBanner : Ability
{
	public int m_duration = 2;

	public float m_radius = 5f;

	public StatType m_allyEffectStat;

	public AbilityStatMod m_allyStatMod;

	public bool m_penetrateLineOfSight;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Drop Banner";
		}
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, m_radius, m_penetrateLineOfSight, true, true);
	}
}
