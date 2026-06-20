// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// added in rogues
#if SERVER
public class NanoSmithBlastShieldEffect : StandardActorEffect
{
	private GameObject m_shieldSquencePrefab;
	private int m_healOnEndIfHasRemainingAbsorb;
	private bool m_absorbedDamage;

	public NanoSmithBlastShieldEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData standardActorEffectData,
		int healOnEndIfHasRemainingAbsorb,
		GameObject shieldSequencePrefab)
		: base(parent, targetSquare, target, caster, standardActorEffectData)
	{
		m_effectName = parent.Ability.m_abilityName;
		m_healOnEndIfHasRemainingAbsorb = healOnEndIfHasRemainingAbsorb;
		m_shieldSquencePrefab = shieldSequencePrefab;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_shieldSquencePrefab, Target.GetFreePos(), Target.AsArray(), Target, SequenceSource);
		list.Add(item);
		return list;
	}

	public override void OnAbilityAndMovementDone()
	{
		if (m_healOnEndIfHasRemainingAbsorb > 0
		    && m_time.age + 1 == m_time.duration && Absorbtion.m_absorbRemaining > 0)
		{
			ServerCombatManager.Get().Heal(Parent, Caster, Target, m_healOnEndIfHasRemainingAbsorb, ServerCombatManager.HealingType.Effect);
		}
	}

	public override void OnStart()
	{
		Passive_Nanosmith component = Caster.GetComponent<Passive_Nanosmith>();
		if (component != null)
		{
			component.OnShieldApplied();
		}
		base.OnStart();
	}

	public override void OnAbsorbedDamage(int damageAbsorbed)
	{
		if (!m_absorbedDamage)
		{
			m_absorbedDamage = true;
			Passive_Nanosmith component = Caster.GetComponent<Passive_Nanosmith>();
			if (component != null)
			{
				component.OnShieldAbsorbedDamage();
			}
		}
		base.OnAbsorbedDamage(damageAbsorbed);
	}
}
#endif
