// ROGUES
// SERVER
using System.Collections.Generic;

// server-only, added in rogues
#if SERVER
public class SniperGhillieSuitEffect : StandardActorEffect
{
	private bool m_asToggle;
	private int m_cost;
	private bool m_proximityBasedInvisibility = true;
	private bool m_unsuppressInvisOnPhaseEnd;
	private bool m_shouldEnd;

	public SniperGhillieSuitEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		StandardActorEffectData standardActorEffectData,
		bool asToggle,
		int cost,
		bool proximityBasedInvisibility,
		bool unsuppressInvisOnPhaseEnd) : base(parent,
		targetSquare,
		caster,
		caster,
		standardActorEffectData)
	{
		m_asToggle = asToggle;
		m_cost = cost;
		m_proximityBasedInvisibility = proximityBasedInvisibility;
		m_unsuppressInvisOnPhaseEnd = unsuppressInvisOnPhaseEnd;
		m_shouldEnd = false;
	}

	public override void OnStart()
	{
		if (m_proximityBasedInvisibility)
		{
			Target.GetActorStatus().AddStatus(StatusType.ProximityBasedInvisibility, m_data.m_duration);
		}
		else
		{
			Target.GetActorStatus().AddStatus(StatusType.InvisibleToEnemies, m_data.m_duration);
		}
		base.OnStart();
		if (m_asToggle)
		{
			AbilityData abilityData = Caster.GetAbilityData();
			abilityData.SetToggledAction(abilityData.GetActionTypeOfAbility(Parent.Ability), true);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_proximityBasedInvisibility)
		{
			Target.GetActorStatus().RemoveStatus(StatusType.ProximityBasedInvisibility);
		}
		else
		{
			Target.GetActorStatus().RemoveStatus(StatusType.InvisibleToEnemies);
		}
		if (m_asToggle)
		{
			AbilityData abilityData = Caster.GetAbilityData();
			abilityData.SetToggledAction(abilityData.GetActionTypeOfAbility(Parent.Ability), false);
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_shouldEnd = (Target.TechPoints < m_cost);
		if (!m_shouldEnd)
		{
			Target.SetTechPoints(Target.TechPoints - m_cost);
		}
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (m_unsuppressInvisOnPhaseEnd)
		{
			Target.GetAbilityData().UnsuppressInvisibility();
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_shouldEnd;
	}

	public override void OnBreakInvisibility()
	{
		Target.GetAbilityData().SuppressInvisibility();
	}

	public override List<StatusType> GetStatuses()
	{
		List<StatusType> list = base.GetStatuses();
		if (list == null)
		{
			list = new List<StatusType>();
		}
		if (m_proximityBasedInvisibility)
		{
			list.Add(StatusType.ProximityBasedInvisibility);
		}
		else
		{
			list.Add(StatusType.InvisibleToEnemies);
		}
		return list;
	}
}
#endif
