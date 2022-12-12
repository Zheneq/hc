// ROGUES
// SERVER
using System.Collections.Generic;

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
		Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, m_radius, m_penetrateLineOfSight, true, true);
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetCurrentBoardSquare(),
			caster.AsArray(),
			caster,
			additionalData.m_sequenceSource);
	}
#endif
}
