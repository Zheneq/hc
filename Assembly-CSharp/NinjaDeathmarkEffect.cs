// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class NinjaDeathmarkEffect : StandardActorEffect
{
	private Ninja_SyncComponent m_syncComp;
	private int m_damageAmount;
	private int m_healOnAttackerOnDetonate;
	private int m_tpGainOnAttackerOnDetonate;
	private StandardEffectInfo m_effectOnTargetOnDetonate;
	private StandardEffectInfo m_effectOnAttackerOnDetonate;
	private GameObject m_onTriggerSequencePrefab;
	private GameObject m_persistemtSequencePrefab;
	private bool m_triggeredThisTurn;
	private bool m_triggeredThisTurn_fake;

	public NinjaDeathmarkEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData standardActorEffectData,
		Ninja_SyncComponent syncComp,
		int damageAmount,
		int healOnAttackerOnDetonate,
		int tpGainOnAttackerOnDetonate,
		StandardEffectInfo effectOnTargetOnDetonate,
		StandardEffectInfo effectOnAttackerOnDetonate,
		GameObject onTriggerSequencePrefab,
		GameObject persistentSequencePrefab) : base(parent,
		targetSquare,
		target,
		caster,
		standardActorEffectData)
	{
		m_syncComp = syncComp;
		m_damageAmount = damageAmount;
		m_healOnAttackerOnDetonate = healOnAttackerOnDetonate;
		m_tpGainOnAttackerOnDetonate = tpGainOnAttackerOnDetonate;
		m_effectOnTargetOnDetonate = effectOnTargetOnDetonate;
		m_effectOnAttackerOnDetonate = effectOnAttackerOnDetonate;
		m_onTriggerSequencePrefab = onTriggerSequencePrefab;
		m_persistemtSequencePrefab = persistentSequencePrefab;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (m_syncComp != null)
		{
			m_syncComp.AddDeathmarkActorIndex(Target.ActorIndex);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp != null)
		{
			m_syncComp.RemoveDeathmarkActorIndex(Target.ActorIndex);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> effectStartSeqDataList = base.GetEffectStartSeqDataList();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_persistemtSequencePrefab, Target.GetFreePos(), null, Target, SequenceSource);
		effectStartSeqDataList.Add(item);
		return effectStartSeqDataList;
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_triggeredThisTurn = false;
			m_triggeredThisTurn_fake = false;
		}
		base.OnAbilityPhaseStart(phase);
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (!incomingHit.HasDamage
		    || incomingHit.m_hitParameters.Caster != Caster
		    || Caster.IsDead()
		    || incomingHit.m_hitParameters.GetRelevantAbility() == null
		    || GetTriggeredThisTurn(isReal))
		{
			return;
		}
		if (m_syncComp != null)
		{
			AbilityData.ActionType actionTypeOfAbility = Caster.GetAbilityData().GetActionTypeOfAbility(incomingHit.m_hitParameters.GetRelevantAbility());
			if (m_syncComp.m_blacklistForDeathmark.Contains(actionTypeOfAbility))
			{
				return;
			}
		}
		Ability relevantAbility = incomingHit.m_hitParameters.GetRelevantAbility();
		if (relevantAbility is NinjaShurikenOrDash dash && !dash.CanTriggerDeathmark())
		{
			return;
		}
		SetTriggeredThisTurn(true, isReal);
		NinjaOmnidash ninjaOmnidash = null;
		if (relevantAbility is NinjaOmnidash omnidash)
		{
			ninjaOmnidash = omnidash;
		}
		AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
		int damage = m_damageAmount;
		if (ninjaOmnidash != null && ninjaOmnidash.GetMod() != null)
		{
			damage = ninjaOmnidash.GetMod().m_deathmarkDamageMod.GetModifiedValue(damage);
		}
		actorHitResults.SetBaseDamage(damage);
		actorHitResults.AddStandardEffectInfo(m_effectOnTargetOnDetonate);
		actorHitResults.SetIgnoreTechpointInteractionForHit(true);
		actorHitResults.AddHitResultsTag(HitResultsTags.DeathmarkDetonation);
		abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
		abilityResults_Reaction.SetupSequenceData(m_onTriggerSequencePrefab, Target.GetCurrentBoardSquare(), SequenceSource);
		reactions.Add(abilityResults_Reaction);
		AbilityResults_Reaction abilityResults_Reaction2 = new AbilityResults_Reaction();
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Target.GetFreePos()));
		int healing = m_healOnAttackerOnDetonate;
		if (ninjaOmnidash != null && ninjaOmnidash.GetMod() != null)
		{
			healing = ninjaOmnidash.GetMod().m_deathmarkCasterHealMod.GetModifiedValue(healing);
		}
		if (relevantAbility is Ninja360Attack ninja360Attack && ninja360Attack.GetSelfHealOnMarkedHit() > 0)
		{
			healing += ninja360Attack.GetSelfHealOnMarkedHit();
		}
		casterHitResults.SetBaseHealing(healing);
		casterHitResults.AddTechPointGain(m_tpGainOnAttackerOnDetonate);
		casterHitResults.AddStandardEffectInfo(m_effectOnAttackerOnDetonate);
		casterHitResults.AddEffectForRemoval(this, ServerEffectManager.Get().GetActorEffects(Target));
		casterHitResults.SetIgnoreTechpointInteractionForHit(true);
		abilityResults_Reaction2.SetupGameplayData(this, casterHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
		abilityResults_Reaction2.SetupSequenceData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), Caster.GetCurrentBoardSquare(), SequenceSource);
		reactions.Add(abilityResults_Reaction2);
	}

	private bool GetTriggeredThisTurn(bool isReal)
	{
		return isReal ? m_triggeredThisTurn : m_triggeredThisTurn_fake;
	}

	private void SetTriggeredThisTurn(bool triggeredThisTurn, bool isReal)
	{
		if (isReal)
		{
			m_triggeredThisTurn = triggeredThisTurn;
		}
		else
		{
			m_triggeredThisTurn_fake = triggeredThisTurn;
		}
	}
}
#endif
