// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
public class ClaymoreDirtyFightingTargetEffect : StandardActorEffect
{
	public int m_damageAmount;  // float in rogues
	
	private bool m_explodeUpToOncePerTurn;
	private GameObject m_explosionSequencePrefab;
	private bool m_wasHitThisTurn;
	private bool m_wasHitByNonCasterAllyThisTurn;
	private bool m_wasHitThisTurn_fake;
	private bool m_wasHitByNonCasterAllyThisTurn_fake;
	private bool m_explosionDone;

	// custom
	private int m_explosionCooldownReduction;
	private bool m_explosionReduceCooldownOnlyIfHitByAlly;
	private Claymore_SyncComponent m_syncComp;
	private Passive_Claymore m_passive;
	// end custom

	public ClaymoreDirtyFightingTargetEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData effectData,
		int damageOnDetonation,  // float in rogues
		GameObject explosionSequencePrefab,
		bool explodeUpToOncePerTurn,
		int explosionCooldownReduction, // custom
		bool explosionReduceCooldownOnlyIfHitByAlly) // custom
		: base(parent, targetSquare, target, caster, effectData)
	{
		m_effectName = "Claymore Dirty Fighting Target Effect";
		m_damageAmount = damageOnDetonation;
		m_explodeUpToOncePerTurn = explodeUpToOncePerTurn;
		m_explosionSequencePrefab = explosionSequencePrefab;
		
		// custom
		m_explosionCooldownReduction = explosionCooldownReduction;
		m_explosionReduceCooldownOnlyIfHitByAlly = explosionReduceCooldownOnlyIfHitByAlly;
		m_syncComp = parent.Ability.GetComponent<Claymore_SyncComponent>();
		m_passive = parent.Ability.GetComponent<Passive_Claymore>();
		// end custom
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_wasHitThisTurn = false;
		m_wasHitThisTurn_fake = false;
		if (m_explodeUpToOncePerTurn)
		{
			m_explosionDone = false;
		}
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_wasHitThisTurn = false;
			m_wasHitThisTurn_fake = false;
			m_wasHitByNonCasterAllyThisTurn = false;
			m_wasHitByNonCasterAllyThisTurn_fake = false;
		}
		base.OnAbilityPhaseStart(phase);
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (GetWasHitThisTurn(true) && !m_explosionDone)
		{
			m_explosionDone = true;
		}
		
		// custom
		bool applyCdr = m_explosionReduceCooldownOnlyIfHitByAlly
			? GetWasHitByNonCasterAllyThisTurn(true)
			: GetWasHitThisTurn(true);
		if (applyCdr)
		{
			m_passive.SetPendingCdrDaggerTrigger(m_explosionCooldownReduction, AbilityData.ActionType.ABILITY_3);
		}
		// end custom
	}

	// custom
	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly()
		       || m_explosionDone && !m_explodeUpToOncePerTurn;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>();
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (!incomingHit.HasDamage || m_explosionDone)
		{
			return;
		}
		if (!GetWasHitThisTurn(isReal))
		{
			SetWasHitThisTurn(true, isReal);
			AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
			ActorHitParameters hitParameters = new ActorHitParameters(Target, Target.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(m_damageAmount, HitActionType.Damage, (StandardEffectInfo) null, hitParameters);
			actorHitResults.CanBeReactedTo = false;
			// rogues
			// actorHitResults.ModifyDamageCoeff(m_damageAmount, m_damageAmount);
			if (m_data.m_sequencePrefabs != null && m_data.m_sequencePrefabs.Length != 0)
			{
				for (int i = 0; i < m_data.m_sequencePrefabs.Length; i++)
				{
					actorHitResults.AddEffectSequenceToEnd(m_data.m_sequencePrefabs[i], m_guid);
				}
			}
			abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
			abilityResults_Reaction.SetupSequenceData(m_explosionSequencePrefab, Target.GetCurrentBoardSquare(), SequenceSource);
			abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.ClientExecuteOnFirstDamagingHit);
			reactions.Add(abilityResults_Reaction);
		}
		if (incomingHit.m_hitParameters.Caster != Caster)
		{
			SetWasHitByNonCasterAllyThisTurn(true, isReal);
		}
	}

	private bool GetWasHitThisTurn(bool isReal)
	{
		return isReal
			? m_wasHitThisTurn
			: m_wasHitThisTurn_fake;
	}

	private void SetWasHitThisTurn(bool wasHitThisTurn, bool isReal)
	{
		if (isReal)
		{
			m_wasHitThisTurn = wasHitThisTurn;
		}
		else
		{
			m_wasHitThisTurn_fake = wasHitThisTurn;
		}
	}

	private bool GetWasHitByNonCasterAllyThisTurn(bool isReal)
	{
		return isReal
			? m_wasHitByNonCasterAllyThisTurn
			: m_wasHitByNonCasterAllyThisTurn_fake;
	}

	private void SetWasHitByNonCasterAllyThisTurn(bool wasHitByNonCasterAllyThisTurn, bool isReal)
	{
		if (isReal)
		{
			m_wasHitByNonCasterAllyThisTurn = wasHitByNonCasterAllyThisTurn;
		}
		else
		{
			m_wasHitByNonCasterAllyThisTurn_fake = wasHitByNonCasterAllyThisTurn;
		}
	}
	
	// custom
	public override void OnEnd()
	{
		base.OnEnd();
		m_syncComp.ResetDirtyFightingData();
	}
}
#endif
