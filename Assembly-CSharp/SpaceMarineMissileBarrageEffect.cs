// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server only, added in rogues
#if SERVER
public class SpaceMarineMissileBarrageEffect : Effect
{
	private StandardEffectInfo m_effectOnTargets;
	private GameObject m_buffSequencePrefab;
	private GameObject m_missileSequencePrefab;
	private int m_launchAnimationIndex;
	private float m_radius;
	private bool m_penetrateLoS;
	private bool m_includeInvisibles;
	private int m_damage;
	private int m_maxHitsPerVolley;
	private int m_extraDamagePerTarget;
	private int m_cinematicsRequested;
	private AbilityModCooldownReduction m_cooldownReductions;
	private bool m_considerDamageAsDirect;

	public SpaceMarineMissileBarrageEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		string effectName,
		float radius,
		bool penetrateLoS,
		bool includeInvisibles,
		int damage,
		int maxHitsPerVolley,
		StandardEffectInfo effectOnTargets,
		int turnsActive,
		GameObject buffSequencePrefab,
		GameObject missileSequencePrefab,
		int launchAnimationIndex,
		int cinematicsRequested,
		AbilityModCooldownReduction cooldownReductions,
		int extraDamagePerTarget,
		bool considerDamageAsDirect)
		: base(parent, targetSquare, caster, caster)
	{
		m_effectName = effectName;
		m_radius = radius;
		m_penetrateLoS = penetrateLoS;
		m_includeInvisibles = includeInvisibles;
		m_damage = damage;
		m_maxHitsPerVolley = maxHitsPerVolley;
		m_effectOnTargets = effectOnTargets;
		m_time.duration = 1 + turnsActive;
		m_buffSequencePrefab = buffSequencePrefab;
		m_missileSequencePrefab = missileSequencePrefab;
		m_launchAnimationIndex = launchAnimationIndex;
		m_cinematicsRequested = cinematicsRequested;
		m_cooldownReductions = cooldownReductions;
		m_extraDamagePerTarget = extraDamagePerTarget;
		m_considerDamageAsDirect = considerDamageAsDirect;
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_buffSequencePrefab, Caster.GetCurrentBoardSquare(), Caster.AsArray(), Caster, SequenceSource);
	}

	private List<ActorData> FindHitActors()
	{
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			Caster.GetFreePos(), m_radius, m_penetrateLoS, Caster, Caster.GetOtherTeams(), null);
		List<ActorData> hitActors = new List<ActorData>(actorsInRadius.Count);
		foreach (ActorData actorData in actorsInRadius)
		{
			if (m_includeInvisibles || actorData.IsActorVisibleToActor(Caster))
			{
				hitActors.Add(actorData);
			}
		}
		if (m_maxHitsPerVolley > 0 && hitActors.Count > m_maxHitsPerVolley)
		{
			BoardSquare casterSquare = Caster.GetCurrentBoardSquare();
			hitActors.Sort(delegate(ActorData hitActor1, ActorData hitActor2)
			{
				float num = hitActor1.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(casterSquare);
				float value = hitActor2.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(casterSquare);
				return num.CompareTo(value);
			});
			int count = hitActors.Count - m_maxHitsPerVolley;
			hitActors.RemoveRange(m_maxHitsPerVolley, count);
		}
		return hitActors;
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		return phaseIndex == HitPhase
		       && m_time.age >= 1
		       && (m_cinematicsRequested > 0 || (m_effectResults.GatheredResults && m_effectResults.HitActorsArray().Length != 0))
			? m_launchAnimationIndex
			: 0;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		return GetCasterAnimationIndex(phaseIndex) > 0
			? m_cinematicsRequested
			: -1;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age >= 1)
		{
			ActorData[] array = m_effectResults.HitActorsArray();
			for (int i = 0; i < array.Length; i++)
			{
				ActorData actorData = array[i];
				Sequence.IExtraSequenceParams[] extraParams = {
					new SplineProjectileSequence.DelayedProjectileExtraParams
					{
						startDelay = 0.25f * i,
						curIndex = i,
						maxIndex = m_maxHitsPerVolley
					}
				};
				list.Add(new ServerClientUtils.SequenceStartData(
					m_missileSequencePrefab,
					actorData.GetCurrentBoardSquare(),
					actorData.AsArray(),
					Caster,
					SequenceSource,
					extraParams));
			}
		}
		return list;
	}

	public override bool HitsCanBeReactedTo()
	{
		return m_considerDamageAsDirect;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age < 1)
		{
			return;
		}
		List<ActorData> hitActors = FindHitActors();
		Vector3 origin = Caster.GetCurrentBoardSquare().ToVector3();
		int damage = m_damage + m_extraDamagePerTarget * (hitActors.Count - 1);
		foreach (ActorData target in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, origin);
			effectResults.StoreActorHit(new ActorHitResults(damage, HitActionType.Damage, m_effectOnTargets, hitParams));
		}
		bool hasCooldownReduction = m_cooldownReductions != null && m_cooldownReductions.HasCooldownReduction();
		if (hasCooldownReduction && hitActors.Count > 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			effectResults.StoreActorHit(actorHitResults);
			if (hasCooldownReduction)
			{
				m_cooldownReductions.AppendCooldownMiscEvents(actorHitResults, false, 0, hitActors.Count);
			}
		}
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() == Caster.GetTeam())
		{
			return;
		}
		BoardSquare casterPos = Board.Get().GetSquareFromVec3(Caster.GetFreePos());
		if (casterPos != null)
		{
			List<BoardSquare> squaresInRadius = AreaEffectUtils.GetSquaresInRadius(casterPos, m_radius + 0.5f, true, Caster);
			squaresToAvoid.UnionWith(squaresInRadius);
		}
	}
}
#endif
