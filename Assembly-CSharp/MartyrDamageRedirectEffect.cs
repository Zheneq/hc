// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// added in rogues
#if SERVER
public class MartyrDamageRedirectEffect : StandardActorEffect
{
	private bool m_redirectIndirectDamage;
	private float m_damageReductionPercent = 0.5f;
	private float m_damageRedirectPercent = 0.5f;
	private int m_techPointGainPerDamageRedirect;
	private float m_maxRange;
	private GameObject m_effectSequencePrefab;
	private GameObject m_reactionSequencePrefab;
	private List<ActorData> m_redirectRecipients = new List<ActorData>();
	private bool m_shouldRemove;

	public MartyrDamageRedirectEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, bool redirectIndirectDamage, List<ActorData> recipients, StandardActorEffectData standardActorEffectData, float damageReduction, float damageRedirect, int techPointGain, float maxRange = 0f, GameObject effectSequencePrefab = null, GameObject reactionSequencePrefab = null)
		: base(parent, targetSquare, target, caster, standardActorEffectData)
	{
		m_redirectIndirectDamage = redirectIndirectDamage;
		m_redirectRecipients = recipients;
		m_damageReductionPercent = damageReduction;
		m_damageRedirectPercent = damageRedirect;
		m_techPointGainPerDamageRedirect = techPointGain;
		m_maxRange = maxRange;
		m_effectSequencePrefab = effectSequencePrefab;
		m_reactionSequencePrefab = reactionSequencePrefab;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (Target != null)
		{
			AbilityStatMod abilityStatMod = new AbilityStatMod
			{
				stat = StatType.IncomingDamage,
				modType = ModType.Multiplier,
				modValue = m_damageReductionPercent
			};
			Target.GetActorStats().AddStatMod(abilityStatMod);
		}
	}

	public override void OnEnd()
	{
		if (Target != null)
		{
			AbilityStatMod abilityStatMod = new AbilityStatMod
			{
				stat = StatType.IncomingDamage,
				modType = ModType.Multiplier,
				modValue = m_damageReductionPercent
			};
			Target.GetActorStats().RemoveStatMod(abilityStatMod);
		}
		base.OnEnd();
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (phase == AbilityPriority.Combat_Final)
		{
			m_shouldRemove = true;
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_shouldRemove;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = base.GetEffectStartSeqDataList();
		if (list == null)
		{
			list = new List<ServerClientUtils.SequenceStartData>();
		}
		if (m_effectSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_effectSequencePrefab, Target.GetFreePos(), new ActorData[]
			{
				Target
			}, Caster, SequenceSource, null));
		}
		return list;
	}

	public override bool ShouldForceReactToHit(ActorHitResults incomingHit)
	{
		return m_redirectIndirectDamage;
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		foreach (ActorData actorData in m_redirectRecipients)
		{
			if (incomingHit.GetDamageCalcScratch().m_damageAfterIncomingBuffDebuffWithCover > 0 && !actorData.IsDead() && (m_maxRange <= 0f || Target.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(actorData.GetCurrentBoardSquare()) <= m_maxRange))
			{
				int num = Mathf.RoundToInt(incomingHit.GetDamageCalcScratch().m_damageAfterIncomingBuffDebuffWithCover * m_damageRedirectPercent);
				Effect effect = incomingHit.m_hitParameters.Effect;
				if (effect == null || !(effect is MartyrDamageRedirectEffect) || num <= 0)
				{
					AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
					ActorHitParameters hitParams = new ActorHitParameters(actorData, actorData.GetFreePos());
					ActorHitResults actorHitResults = new ActorHitResults(num, HitActionType.Damage, hitParams);
					if (m_techPointGainPerDamageRedirect != 0)
					{
						actorHitResults.AddTechPointGainOnCaster(m_techPointGainPerDamageRedirect);
					}
					ActorData caster = Caster;
					if (actorData.GetTeam() == Caster.GetTeam())
					{
						caster = incomingHit.m_hitParameters.Caster;
					}
					abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, caster, isReal, incomingHit);
					abilityResults_Reaction.SetupSequenceData(m_reactionSequencePrefab, actorData.GetCurrentBoardSquare(), SequenceSource, null);
					if (incomingHit.ForMovementStage != MovementStage.Evasion && incomingHit.ForMovementStage != MovementStage.Knockback)
					{
						abilityResults_Reaction.SetSequenceCaster(Target);
					}
					reactions.Add(abilityResults_Reaction);
				}
			}
		}
	}
}
#endif
