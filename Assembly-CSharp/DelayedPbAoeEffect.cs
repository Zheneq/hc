// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class DelayedPbAoeEffect : Effect
{
	protected int m_hitStartDelay;
	protected int m_damageAmount;
	protected StandardEffectInfo m_enemyHitEffect;
	protected int m_selfHealAmount;
	protected StandardEffectInfo m_selfHitEffect;
	protected int m_allyHealAmount;
	protected StandardEffectInfo m_allyHitEffect;
	protected bool m_ignoreSourceEnergyGain;
	protected int m_energyGainPerEnemyHit;
	protected int m_energyGainPerAllyHit;
	protected int m_energyGainOnSelfHit;
	protected float m_radiusInSquares;
	protected bool m_ignoreLos;
	protected int m_animationIndex;
	protected GameObject m_persistentSequencePrefab;
	protected GameObject m_triggerSequencePrefab;

	public DelayedPbAoeEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		int duration,
		int hitStartDelay,
		int damage,
		StandardEffectInfo enemyHitEffect,
		int allyHeal,
		StandardEffectInfo allyHitEffect,
		int selfHeal,
		StandardEffectInfo selfHitEffect,
		bool ignoreSourceEnergyGains,
		int energyGainPerEnemyHit,
		int energyGainPerAllyHit,
		int energyGainOnSelfHit,
		float radiusInSquares,
		bool penetrateLos,
		int animIndex,
		GameObject persistentSequencePrefab,
		GameObject triggerSequencePrefab)
		: base(parent, targetSquare, target, caster)
	{
		m_effectName = "Delayed PbAoe Effect";
		m_hitStartDelay = hitStartDelay;
		m_damageAmount = damage;
		m_enemyHitEffect = enemyHitEffect;
		m_allyHealAmount = allyHeal;
		m_allyHitEffect = allyHitEffect;
		m_selfHealAmount = selfHeal;
		m_selfHitEffect = selfHitEffect;
		m_ignoreSourceEnergyGain = ignoreSourceEnergyGains;
		m_energyGainPerEnemyHit = energyGainPerEnemyHit;
		m_energyGainPerAllyHit = energyGainPerAllyHit;
		m_energyGainOnSelfHit = energyGainOnSelfHit;
		m_radiusInSquares = radiusInSquares;
		m_ignoreLos = penetrateLos;
		m_animationIndex = animIndex;
		m_persistentSequencePrefab = persistentSequencePrefab;
		m_triggerSequencePrefab = triggerSequencePrefab;
		m_time.duration = Mathf.Max(duration, m_hitStartDelay + 1);
		m_time.duration = Mathf.Max(1, m_time.duration);
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public virtual int GetDamage()
	{
		return m_damageAmount;
	}

	public virtual int GetAllyHeal()
	{
		return m_allyHealAmount;
	}

	public virtual int GetSelfHeal()
	{
		return m_selfHealAmount;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		if (m_persistentSequencePrefab == null)
		{
			return null;
		}
		return new ServerClientUtils.SequenceStartData(
			m_persistentSequencePrefab,
			Target.GetCurrentBoardSquare(),
			new[] { Target },
			Target,
			SequenceSource);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age >= m_hitStartDelay)
		{
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			if (AddActorAnimEntryIfHasHits(HitPhase) || GetCasterAnimationIndex(HitPhase) > 0)
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_triggerSequencePrefab,
				Target.GetCurrentBoardSquare(),
				m_effectResults.HitActorsArray(),
				Target,
				shallowCopy));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age >= m_hitStartDelay)
		{
			Vector3 freePos = Target.GetFreePos();
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> hitActors = GetHitActors(nonActorTargetInfo);
			foreach (ActorData actorData in hitActors)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, freePos));
				if (m_ignoreSourceEnergyGain)
				{
					actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				}
				if (actorData.GetTeam() == Caster.GetTeam())
				{
					if (actorData == Target)
					{
						actorHitResults.SetBaseHealing(GetSelfHeal());
						if (m_selfHitEffect != null)
						{
							actorHitResults.AddStandardEffectInfo(m_selfHitEffect);
						}
						actorHitResults.SetTechPointGainOnCaster(m_energyGainOnSelfHit);
					}
					else
					{
						actorHitResults.SetBaseHealing(GetAllyHeal());
						if (m_allyHitEffect != null)
						{
							actorHitResults.AddStandardEffectInfo(m_allyHitEffect);
						}
						actorHitResults.SetTechPointGainOnCaster(m_energyGainPerAllyHit);
					}
				}
				else
				{
					actorHitResults.SetBaseDamage(GetDamage());
					if (m_enemyHitEffect != null)
					{
						actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
					}
					actorHitResults.SetTechPointGainOnCaster(m_energyGainPerEnemyHit);
				}
				effectResults.StoreActorHit(actorHitResults);
			}
			if (hitActors.Count == 0)
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
				actorHitResults2.SetIgnoreTechpointInteractionForHit(true);
				effectResults.StoreActorHit(actorHitResults2);
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare currentBoardSquare = Target.GetCurrentBoardSquare();
		if (currentBoardSquare != null)
		{
			bool includeEnemies = GetDamage() > 0 || (m_enemyHitEffect != null && m_enemyHitEffect.m_applyEffect);
			bool includeAllies = GetAllyHeal() > 0 || (m_allyHitEffect != null && m_allyHitEffect.m_applyEffect);
			bool includeSelf = Target.GetTeam() == Caster.GetTeam() && (GetSelfHeal() > 0 || (m_selfHitEffect != null && m_selfHitEffect.m_applyEffect));
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
				currentBoardSquare.ToVector3(),
				m_radiusInSquares,
				m_ignoreLos,
				Caster,
				TargeterUtils.GetRelevantTeams(Caster, includeAllies, includeEnemies),
				nonActorTargetInfo);
			if (!includeSelf && actorsInRadius.Contains(Target))
			{
				actorsInRadius.Remove(Target);
			}
			else if (includeSelf && !actorsInRadius.Contains(Target))
			{
				actorsInRadius.Add(Target);
			}
			return actorsInRadius;
		}
		return new List<ActorData>();
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (m_time.age >= m_hitStartDelay && phaseIndex == HitPhase)
		{
			return m_animationIndex;
		}
		return 0;
	}

	public override ActorData GetActorAnimationActor()
	{
		return Target;
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return Target != null;
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || Target.IsDead();
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			BoardSquare currentBoardSquare = Target.GetCurrentBoardSquare();
			if (currentBoardSquare != null)
			{
				squaresToAvoid.UnionWith(AreaEffectUtils.GetSquaresInRadius(
					currentBoardSquare, m_radiusInSquares, m_ignoreLos, Caster));
			}
		}
	}
}
#endif
