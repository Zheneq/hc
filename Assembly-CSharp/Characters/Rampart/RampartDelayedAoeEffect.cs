// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class RampartDelayedAoeEffect : Effect
{
	private int m_delayTurns = 1;
	private AbilityAreaShape m_aoeShape;
	private int m_damageAmount;
	private StandardEffectInfo m_enemyHitEffect;
	private bool m_penetrateLos;
	private GameObject m_persistentMarkerSequencePrefab;

	public RampartDelayedAoeEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, int delayTurns, AbilityAreaShape aoeShape, int damageAmount, StandardEffectInfo enemyHitEffect, bool penetrateLos, GameObject persistentMarkerSequencePrefab, GameObject onHitSequencePrefab) : base(parent, targetSquare, target, caster)
	{
		m_effectName = "Rampart Delayed Aoe Effect";
		m_delayTurns = delayTurns;
		m_aoeShape = aoeShape;
		m_damageAmount = damageAmount;
		m_enemyHitEffect = enemyHitEffect;
		m_penetrateLos = penetrateLos;
		m_persistentMarkerSequencePrefab = persistentMarkerSequencePrefab;
		m_time.duration = Mathf.Max(1, m_delayTurns + 1);
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_persistentMarkerSequencePrefab, GetAoeCenterSquare(), null, Target, SequenceSource);
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age >= m_delayTurns)
		{
			BoardSquare aoeCenterSquare = GetAoeCenterSquare();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_aoeShape, aoeCenterSquare.ToVector3(), aoeCenterSquare);
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			foreach (ActorData target in GetHitActors(nonActorTargetInfo))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, centerOfShape));
				actorHitResults.SetBaseDamage(m_damageAmount);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				effectResults.StoreActorHit(actorHitResults);
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare aoeCenterSquare = GetAoeCenterSquare();
		return AreaEffectUtils.GetActorsInShape(m_aoeShape, aoeCenterSquare.ToVector3(), aoeCenterSquare, m_penetrateLos, Caster, Caster.GetOtherTeams(), nonActorTargetInfo);
	}

	private BoardSquare GetAoeCenterSquare()
	{
		return Target.GetCurrentBoardSquare();
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			BoardSquare aoeCenterSquare = GetAoeCenterSquare();
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_aoeShape, aoeCenterSquare.ToVector3(), aoeCenterSquare, true, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}
}
#endif
