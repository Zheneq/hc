// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class ArcherHealingReactionEffect : StandardActorEffect
{
	private ArcherHealingDebuffArrow m_parentAbility;
	private GameObject m_reactionSequencePrefab;
	private int m_reactionHealing;
	private int m_reactionHealingForCaster;
	private int m_lessHealOnSubsequentReactions;
	private int m_extraHealBelowHealthThreshold;
	private float m_healthThresholdForExtraHealing;
	private StandardEffectInfo m_reactionEffect;
	private int m_reactionsPerAlly;
	private int m_techPointsPerHeal;
	private List<ActorData> m_actorsReactedToThisTurn = new List<ActorData>();
	private List<ActorData> m_actorsReactedToThisTurnFake = new List<ActorData>();
	private Dictionary<ActorData, int> m_actorsReactedTo = new Dictionary<ActorData, int>();
	private int m_totalHealingReactions;
	private Archer_SyncComponent m_syncComp;
	
	// custom
	public int ReactionHealing => m_reactionHealing;

	public ArcherHealingReactionEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData baseEffectData,
		int reactionHealing,
		int reactionHealingForCaster,
		int lessHealOnSubsequentReactions,
		int extraHealBelowHealthThreshold,
		float healthThresholdForExtraHealing,
		StandardEffectInfo reactionEffect,
		int reactionsPerAlly,
		int techPointsPerHeal,
		GameObject reactionProjectileSequencePrefab)
		: base(parent, targetSquare, target, caster, baseEffectData)
	{
		m_parentAbility = parent.Ability as ArcherHealingDebuffArrow;
		m_effectName = m_parentAbility.m_abilityName;
		m_reactionHealing = reactionHealing;
		m_reactionHealingForCaster = reactionHealingForCaster;
		m_lessHealOnSubsequentReactions = lessHealOnSubsequentReactions;
		m_extraHealBelowHealthThreshold = extraHealBelowHealthThreshold;
		m_healthThresholdForExtraHealing = healthThresholdForExtraHealing;
		m_reactionEffect = reactionEffect;
		m_reactionsPerAlly = reactionsPerAlly;
		m_techPointsPerHeal = techPointsPerHeal;
		m_reactionSequencePrefab = reactionProjectileSequencePrefab;
		m_syncComp = caster.GetComponent<Archer_SyncComponent>();
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_actorsReactedToThisTurn.Clear();
		m_actorsReactedToThisTurnFake.Clear();
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_actorsReactedToThisTurn.Clear();
			m_actorsReactedToThisTurnFake.Clear();
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp != null)
		{
			m_syncComp.ClearUsedHealReactionActors();
			m_syncComp.Networkm_healReactionTargetActor = -1;
			if (m_totalHealingReactions == 0)
			{
				AbilityModCooldownReduction cooldownReductionIfNoHeals = m_parentAbility.GetCooldownReductionIfNoHeals();
				if (cooldownReductionIfNoHeals != null && cooldownReductionIfNoHeals.HasCooldownReduction())
				{
					ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
					cooldownReductionIfNoHeals.AppendCooldownMiscEvents(hitRes, true, 0, 0);
					MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Caster, Caster, hitRes, m_parentAbility);
				}
			}
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		base.GatherEffectResults(ref effectResults, isReal);
		if (isReal && m_syncComp != null)
		{
			m_syncComp.Networkm_healReactionTargetActor = Target.ActorIndex;
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (!incomingHit.HasDamage)
		{
			return;
		}
		AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
		ActorData caster = incomingHit.m_hitParameters.Caster;
		if (caster.IsDead())
		{
			return;
		}
		bool reacted = true;
		if (isReal)
		{
			if (m_actorsReactedToThisTurn.Contains(caster))
			{
				reacted = false;
			}
			else
			{
				m_actorsReactedToThisTurn.Add(caster);
				m_totalHealingReactions++;
			}
		}
		else if (m_actorsReactedToThisTurnFake.Contains(caster))
		{
			reacted = false;
		}
		else
		{
			m_actorsReactedToThisTurnFake.Add(caster);
		}
		if (m_actorsReactedTo.ContainsKey(caster) && m_reactionsPerAlly <= m_actorsReactedTo[caster])
		{
			reacted = false;
		}
		if (isReal && GameFlowData.Get().IsInResolveState() && reacted)
		{
			if (!m_actorsReactedTo.ContainsKey(caster))
			{
				m_actorsReactedTo.Add(caster, 0);
			}
			m_actorsReactedTo[caster]++;
			if (m_syncComp != null)
			{
				if (m_reactionsPerAlly == m_actorsReactedTo[caster])
				{
					m_syncComp.AddExpendedHealReactionActor(caster);
				}
				m_syncComp.AddUsedHealReactionActor(caster);
			}
		}
		if (reacted)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, incomingHit.m_hitParameters.Origin));
			int healing = caster == Caster ? m_reactionHealingForCaster : m_reactionHealing;
			if (m_actorsReactedTo.ContainsKey(caster) && m_actorsReactedTo[caster] > 1)
			{
				healing -= m_lessHealOnSubsequentReactions;
			}
			if (m_syncComp != null && m_syncComp.ActorIsShieldGeneratorTarget(caster))
			{
				healing += m_parentAbility.GetExtraHealOnShieldGeneratorTargets();
			}
			if (incomingHit.m_hitParameters.Ability != null && incomingHit.m_hitParameters.Ability is ArcherBendingArrow arrow)
			{
				healing += arrow.GetExtraHealingFromHealingDebuffTarget();
			}
			if (caster.GetHpPortionInServerResolution() <= m_healthThresholdForExtraHealing)
			{
				healing += m_extraHealBelowHealthThreshold;
			}
			actorHitResults.SetBaseHealing(healing);
			actorHitResults.AddStandardEffectInfo(m_reactionEffect);
			actorHitResults.AddTechPointGainOnCaster(m_techPointsPerHeal);
			abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
			abilityResults_Reaction.SetSequenceCaster(Target);
			abilityResults_Reaction.SetupSequenceData(
				m_reactionSequencePrefab,
				caster.GetCurrentBoardSquare(),
				SequenceSource,
				new Sequence.ActorIndexExtraParam
				{
					m_actorIndex = (short)Caster.ActorIndex
				}.ToArray());
			abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageIfReactOnAttacker);
			reactions.Add(abilityResults_Reaction);
		}
	}
}
#endif
