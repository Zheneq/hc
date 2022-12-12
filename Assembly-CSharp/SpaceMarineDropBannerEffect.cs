// ROGUES
// SERVER
using System.Collections.Generic;

// server only, added in rogues, not used
#if SERVER
public class SpaceMarineDropBannerEffect : Effect
{
	private SpaceMarineDropBanner m_parentAbility;
	private List<ActorData> m_affectedActors = new List<ActorData>(1);

	public SpaceMarineDropBannerEffect(EffectSource parent, BoardSquare targetSquare, ActorData caster) : base(parent, targetSquare, caster, caster)
	{
		m_parentAbility = parent.Ability as SpaceMarineDropBanner;
		m_time.duration = m_parentAbility.m_duration;
		m_effectName = "Lead by Example";
	}

	private void SendCombatText(AbilityStatMod statMod, ActorData target, CombatTextCategory textCat)
	{
		string combatText = $"{statMod}";
		string format = textCat == CombatTextCategory.BuffLoss || textCat == CombatTextCategory.DebuffLoss
			? "{0}'s stats:{1} removed from {2}"
			: "{0} applied stats:{1} to {2}";
		string logText = string.Format(format, Caster.DisplayName, statMod, target.DisplayName);
		target.CallRpcCombatText(combatText, logText, textCat, BuffIconToDisplay.Damage);
	}

	private void ApplyToRadius()
	{
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			Caster.GetLoSCheckPos(),
			m_parentAbility.m_radius,
			m_parentAbility.m_penetrateLineOfSight,
			Caster,
			Caster.GetTeam(),
			null);
		foreach (ActorData actorData in actorsInRadius)
		{
			if (!m_affectedActors.Contains(actorData))
			{
				actorData.gameObject.GetComponent<ActorStats>().AddStatMod(m_parentAbility.m_allyStatMod);
				SendCombatText(m_parentAbility.m_allyStatMod, actorData, CombatTextCategory.BuffGain);
				m_affectedActors.Add(actorData);
			}
		}
		List<ActorData> actorsLeftRadius = new List<ActorData>(m_affectedActors.Count);
		foreach (ActorData actorData in m_affectedActors)
		{
			if (!actorsInRadius.Contains(actorData))
			{
				actorsLeftRadius.Add(actorData);
			}
		}
		foreach (ActorData actorData in actorsLeftRadius)
		{
			RemoveStatMod(actorData);
			m_affectedActors.Remove(actorData);
		}
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			SequenceLookup.Get().GetSimpleHitSequencePrefab(),
			TargetSquare, 
			null,
			Caster,
			SequenceSource);
	}

	public override void OnStart()
	{
		string combatText = "Lead by Example";
		string logText = $"{Caster.DisplayName} applied {m_effectName} to ({TargetSquare.x}, {TargetSquare.y})";
		Caster.CallRpcCombatText(combatText, logText, CombatTextCategory.BuffGain, BuffIconToDisplay.Damage);
		ApplyToRadius();
	}

	public override void OnTurnStart()
	{
		ApplyToRadius();
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		ApplyToRadius();
	}

	private void RemoveStatMod(ActorData actor)
	{
		if (actor.GetTeam() == Caster.GetTeam())
		{
			actor.gameObject.GetComponent<ActorStats>().RemoveStatMod(m_parentAbility.m_allyStatMod);
		}
	}

	public override void OnEnd()
	{
		foreach (ActorData actor in m_affectedActors)
		{
			RemoveStatMod(actor);
		}
	}
}
#endif
