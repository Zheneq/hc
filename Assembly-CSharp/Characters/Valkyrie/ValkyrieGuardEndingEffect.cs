// ROGUES
// SERVER
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
	
	// custom
	public ActorCover.CoverDirections CoverDirection => m_shieldFacing;
	// custom
	public bool CoverIgnoresMinDist => m_shieldCoverIgnoreMinDist;

	public ValkyrieGuardEndingEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		GameObject removeShieldSequencePrefab,
		ActorCover.CoverDirections shieldFacing,
		bool shieldCoverIgnoreMinDist,
		int techPointGain,
		int techPointGainTooNearForCover,
		StandardEffectInfo reactionEffect,
		StandardEffectInfo reactionEffectTooNearForCover)
		: base(parent, targetSquare, target, caster, data)
	{
		HitPhase = AbilityPriority.Combat_Final;
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
			m_passive = passiveData.GetPassiveOfType(typeof(Passive_Valkyrie)) as Passive_Valkyrie;
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (GameObject prefab in m_data.m_sequencePrefabs)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				prefab,
				Caster.GetCurrentBoardSquare(),
				new[] { Caster },
				Caster,
				SequenceSource,
				new Sequence.IExtraSequenceParams[] {
					new ValkyrieDirectionalShieldSequence.ExtraParams
					{
						m_aimDirection = (sbyte)m_shieldFacing
					}
				}));
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age == m_time.duration - 2)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_removeShieldSequencePrefab,
				Caster.GetCurrentBoardSquare(), 
				new[] { Caster },
				Caster,
				SequenceSource, 
				new Sequence.IExtraSequenceParams[] {
					new ValkyrieDirectionalShieldSequence.ExtraParams
					{
						m_aimDirection = (sbyte)m_shieldFacing
					}
				}));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age == m_time.duration - 2)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			if (!m_data.m_sequencePrefabs.IsNullOrEmpty())
			{
				actorHitResults.AddEffectSequenceToEnd(m_data.m_sequencePrefabs[0], GetEffectGuid());
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
		if (Caster.GetActorCover() != null)
		{
			Caster.GetActorCover().RemoveTempCoverProvider(m_shieldFacing, m_shieldCoverIgnoreMinDist);
		}
	}

	public override void OnStart()
	{
		base.OnStart();
		if (Caster.GetActorCover() != null)
		{
			Caster.GetActorCover().AddTempCoverProvider(m_shieldFacing, m_shieldCoverIgnoreMinDist);
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (m_techPointGainPerCoveredHit == 0
		    || incomingHit.GetDamageCalcScratch().m_damageAfterIncomingBuffDebuffWithCover <= 0)
		{
			return;
		}
		bool tooNearForCover = false;
		if (!m_passive.IsDamageCoveredByGuard(incomingHit.m_hitParameters.DamageSource, ref tooNearForCover))
		{
			return;
		}
		AbilityResults_Reaction abilityResults = new AbilityResults_Reaction();
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
		if (tooNearForCover)
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
		abilityResults.SetupGameplayData(
			this,
			actorHitResults,
			incomingHit.m_reactionDepth,
			null,
			isReal,
			incomingHit);
		abilityResults.SetupSequenceData(null, Caster.GetCurrentBoardSquare(), SequenceSource);
		reactions.Add(abilityResults);
		if (isReal && GameFlowData.Get().IsInResolveState())
		{
			m_triggeredReactionOnce = true;
		}
	}
}
#endif
