// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class MantaDirtyFightingEffect : StandardActorEffect
{
	public int m_damageAmount;
	public StandardActorEffectData m_effectOnExplosion;
	public StandardActorEffectData m_effectOnExpire;

	private bool m_explodeOnlyFromCasterDamage;
	private int m_techPointGainPerExplosion;
	private int m_healAmountPerExplosion;
	private GameObject m_explosionSequencePrefab;
	private bool m_wasHitThisTurn;
	private bool m_wasHitThisTurnByCaster;
	private bool m_wasHitByNonCasterAllyThisTurn;
	private bool m_wasHitThisTurn_fake;
	private bool m_wasHitThisTurnByCaster_fake;
	private bool m_wasHitByNonCasterAllyThisTurn_fake;
	private bool m_explosionDone;
	private Manta_SyncComponent m_syncComp;
	private bool m_removedTargetActorFromSyncComp;

	public bool IsActive()
	{
		return !m_explosionDone;
	}

	public MantaDirtyFightingEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData effectData,
		int damageOnDetonation,
		StandardActorEffectData effectOnExplode,
		GameObject explosionSequencePrefab,
		bool explodeOnlyFromCasterDamage,
		int techPointGainPerExplosion,
		int healPerExplosion,
		StandardActorEffectData effectOnExpireWithoutExplosion)
		: base(parent, targetSquare, target, caster, effectData)
	{
		m_effectName = "Manta Dirty Fighting Effect";
		m_damageAmount = damageOnDetonation;
		m_effectOnExplosion = effectOnExplode;
		m_effectOnExpire = effectOnExpireWithoutExplosion;
		m_techPointGainPerExplosion = techPointGainPerExplosion;
		m_healAmountPerExplosion = healPerExplosion;
		m_explodeOnlyFromCasterDamage = explodeOnlyFromCasterDamage;
		m_explosionSequencePrefab = explosionSequencePrefab;
	}

	public override void OnStart()
	{
		base.OnStart();
		m_syncComp = Caster.GetComponent<Manta_SyncComponent>();
		if (m_syncComp != null && Target != null)
		{
			m_syncComp.AddDirtyFightingActor(Target);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (!m_removedTargetActorFromSyncComp && m_syncComp != null && Target != null)
		{
			m_syncComp.RemoveDirtyFightingActor(Target);
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_wasHitThisTurn = false;
		m_wasHitThisTurn_fake = false;
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
		if (phase == HitPhase && m_wasHitThisTurn && !m_explosionDone)
		{
			m_explosionDone = true;
			if (m_syncComp != null && Target != null)
			{
				m_syncComp.RemoveDirtyFightingActor(Target);
				m_removedTargetActorFromSyncComp = true;
			}
		}
		if (phase == AbilityPriority.Combat_Final
		    && m_time.age == m_time.duration - 1
		    && m_effectOnExpire != null
		    && !m_explosionDone)
		{
			ActorHitParameters hitParams = new ActorHitParameters(Target, Caster.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(hitParams);
			actorHitResults.AddEffect(new StandardActorEffect(hitParams, m_effectOnExpire));
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Target, Caster, actorHitResults, Parent.Ability);
		}
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
		if (!GetWasHitThisTurn(isReal) && (!m_explodeOnlyFromCasterDamage || incomingHit.m_hitParameters.Caster == Caster))
		{
			SetWasHitThisTurn(true, isReal);
			AbilityResults_Reaction abilityResults = new AbilityResults_Reaction();
			ActorHitParameters hitParams = new ActorHitParameters(Target, Target.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(hitParams);
			actorHitResults.CanBeReactedTo = false;
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			actorHitResults.AddBaseDamage(m_damageAmount);
			if (m_effectOnExplosion != null)
			{
				actorHitResults.AddEffect(new StandardActorEffect(hitParams, m_effectOnExplosion));
			}
			actorHitResults.AddTechPointGainOnCaster(m_techPointGainPerExplosion);
			if (m_data.m_sequencePrefabs != null && m_data.m_sequencePrefabs.Length != 0)
			{
				foreach (GameObject sequencePrefab in m_data.m_sequencePrefabs)
				{
					actorHitResults.AddEffectSequenceToEnd(sequencePrefab, m_guid);
				}
			}
			abilityResults.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
			abilityResults.SetupSequenceData(m_explosionSequencePrefab, Target.GetCurrentBoardSquare(), SequenceSource);
			abilityResults.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageIfReactOnAttacker);
			reactions.Add(abilityResults);
			if (m_healAmountPerExplosion > 0)
			{
				AbilityResults_Reaction abilityResultsOnCaster = new AbilityResults_Reaction();
				ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
				casterHitResults.AddBaseHealing(m_healAmountPerExplosion);
				abilityResultsOnCaster.SetupGameplayData(this, casterHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
				abilityResultsOnCaster.SetupSequenceData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), Caster.GetCurrentBoardSquare(), SequenceSource);
				abilityResultsOnCaster.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageIfReactOnAttacker);
				reactions.Add(abilityResultsOnCaster);
			}
		}
		if (incomingHit.m_hitParameters.Caster != Caster)
		{
			SetWasHitByNonCasterAllyThisTurn(true, isReal);
			return;
		}
		SetWasHitByCasterThisTurn(true, isReal);
	}

	private bool GetWasHitThisTurn(bool isReal)
	{
		return isReal ? m_wasHitThisTurn : m_wasHitThisTurn_fake;
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
		return isReal ? m_wasHitByNonCasterAllyThisTurn : m_wasHitByNonCasterAllyThisTurn_fake;
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

	private bool GetWasHitByCasterThisTurn(bool isReal)
	{
		return isReal ? m_wasHitThisTurnByCaster : m_wasHitThisTurnByCaster_fake;
	}

	private void SetWasHitByCasterThisTurn(bool wasHitByCasterThisTurn, bool isReal)
	{
		if (isReal)
		{
			m_wasHitThisTurnByCaster = wasHitByCasterThisTurn;
		}
		else
		{
			m_wasHitThisTurnByCaster_fake = wasHitByCasterThisTurn;
		}
	}
}
#endif
