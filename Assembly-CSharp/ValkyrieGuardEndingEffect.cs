using System;
using System.Collections.Generic;
using UnityEngine;

#if SERVER
//Added in rouges
public class ValkyrieGuardEndingEffect : StandardActorEffect
{
	private GameObject m_removeShieldSequencePrefab;
	private ActorCover.CoverDirections m_shieldFacing = ActorCover.CoverDirections.INVALID;
	private bool m_shieldCoverIgnoreMinDist = true;
	private int m_techPointGainPerCoveredHit;
	private int m_techPointGainPerTooCloseForCoverHit;
	private StandardEffectInfo m_reactionEffect;
	private StandardEffectInfo m_reactionEffectTooNearForCover;
	private Passive_Valkyrie m_passive;
	private bool m_triggeredReactionOnce;
	private bool m_readyToEnd;

	public ValkyrieGuardEndingEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData data, GameObject removeShieldSequencePrefab, ActorCover.CoverDirections shieldFacing, bool shieldCoverIgnoreMinDist, int techPointGain, int techPointGainTooNearForCover, StandardEffectInfo reactionEffect, StandardEffectInfo reactionEffectTooNearForCover) : base(parent, targetSquare, target, caster, data)
	{
		base.HitPhase = AbilityPriority.Combat_Final;
		m_removeShieldSequencePrefab = removeShieldSequencePrefab;
		m_shieldFacing = shieldFacing;
		m_shieldCoverIgnoreMinDist = shieldCoverIgnoreMinDist;
		m_techPointGainPerCoveredHit = techPointGain;
		m_techPointGainPerTooCloseForCoverHit = techPointGainTooNearForCover;
		m_reactionEffect = reactionEffect;
		m_reactionEffectTooNearForCover = reactionEffectTooNearForCover;
		PassiveData passiveData = caster.GetPassiveData();
		if (passiveData != null)
		{
			m_passive = (passiveData.GetPassiveOfType(typeof(Passive_Valkyrie)) as Passive_Valkyrie);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (GameObject gameObject in m_data.m_sequencePrefabs)
		{
			ValkyrieDirectionalShieldSequence.ExtraParams extraParams = new ValkyrieDirectionalShieldSequence.ExtraParams();
			extraParams.m_aimDirection = (sbyte)m_shieldFacing;
			List<ServerClientUtils.SequenceStartData> list2 = list;
			GameObject prefab = gameObject;
			BoardSquare currentBoardSquare = base.Caster.GetCurrentBoardSquare();
			ActorData[] targetActorArray = new ActorData[]
			{
				base.Caster
			};
			ActorData caster = base.Caster;
			SequenceSource sequenceSource = base.SequenceSource;
			Sequence.IExtraSequenceParams[] extraParams2 = new ValkyrieDirectionalShieldSequence.ExtraParams[]
			{
				extraParams
			};
			list2.Add(new ServerClientUtils.SequenceStartData(prefab, currentBoardSquare, targetActorArray, caster, sequenceSource, extraParams2));
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age == m_time.duration - 2)
		{
			ValkyrieDirectionalShieldSequence.ExtraParams extraParams = new ValkyrieDirectionalShieldSequence.ExtraParams();
			extraParams.m_aimDirection = (sbyte)m_shieldFacing;
			List<ServerClientUtils.SequenceStartData> list2 = list;
			GameObject removeShieldSequencePrefab = m_removeShieldSequencePrefab;
			BoardSquare currentBoardSquare = base.Caster.GetCurrentBoardSquare();
			ActorData[] targetActorArray = new ActorData[]
			{
				base.Caster
			};
			ActorData caster = base.Caster;
			SequenceSource sequenceSource = base.SequenceSource;
			Sequence.IExtraSequenceParams[] extraParams2 = new ValkyrieDirectionalShieldSequence.ExtraParams[]
			{
				extraParams
			};
			list2.Add(new ServerClientUtils.SequenceStartData(removeShieldSequencePrefab, currentBoardSquare, targetActorArray, caster, sequenceSource, extraParams2));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age == m_time.duration - 2)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(base.Target, base.Target.GetFreePos()));
			if (!m_data.m_sequencePrefabs.IsNullOrEmpty<GameObject>())
			{
				actorHitResults.AddEffectSequenceToEnd(m_data.m_sequencePrefabs[0], base.GetEffectGuid());
			}
			effectResults.StoreActorHit(actorHitResults);
		}
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_time.age == m_time.duration - 1)
		{
			m_readyToEnd = true;
		}
	}

	public override bool ShouldEndEarly()
	{
		return m_readyToEnd;
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (base.Caster.GetActorCover() != null)
		{
			base.Caster.GetActorCover().RemoveTempCoverProvider(m_shieldFacing, m_shieldCoverIgnoreMinDist);
		}
	}

	public override void OnStart()
	{
		base.OnStart();
		if (base.Caster.GetActorCover() != null)
		{
			base.Caster.GetActorCover().AddTempCoverProvider(m_shieldFacing, m_shieldCoverIgnoreMinDist);
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (m_techPointGainPerCoveredHit != 0 && incomingHit.GetDamageCalcScratch().m_damageAfterIncomingBuffDebuffWithCover > 0)
		{
			bool flag = false;
			if (m_passive.IsDamageCoveredByGuard(incomingHit.m_hitParameters.DamageSource, ref flag))
			{
				AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(base.Caster, base.Caster.GetFreePos()));
				if (flag)
				{
					actorHitResults.AddTechPointGainOnCaster(m_techPointGainPerTooCloseForCoverHit);
					if (!m_triggeredReactionOnce)
					{
						actorHitResults.AddStandardEffectInfo(m_reactionEffectTooNearForCover);
					}
				}
				else
				{
					actorHitResults.AddTechPointGainOnCaster(m_techPointGainPerCoveredHit);
					if (!m_triggeredReactionOnce)
					{
						actorHitResults.AddStandardEffectInfo(m_reactionEffect);
					}
				}
				abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
				abilityResults_Reaction.SetupSequenceData(null, base.Caster.GetCurrentBoardSquare(), base.SequenceSource, null);
				reactions.Add(abilityResults_Reaction);
				if (isReal && GameFlowData.Get().IsInResolveState())
				{
					m_triggeredReactionOnce = true;
				}
			}
		}
	}
}
#endif