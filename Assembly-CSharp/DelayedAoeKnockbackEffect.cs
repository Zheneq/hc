// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
public class DelayedAoeKnockbackEffect : Effect
{
	public enum KnockbackCenterType
	{
		FromTargetSquare,
		FromTargetActor
	}
	
	private KnockbackCenterType m_knockbackCenterType;
	private int m_knockbackDelay;
	private int m_damageAmount;
	private StandardEffectInfo m_enemyHitEffect;
	private AbilityAreaShape m_knockbackShape = AbilityAreaShape.Three_x_Three;
	private bool m_bombPenetrateLineOfSight;
	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	private float m_knockbackDistance = 2f;
	private bool m_knockbackAdjacentActorsIfPull;
	private AbilityData.ActionType m_cooldownActionType = AbilityData.ActionType.INVALID_ACTION;
	private int m_cooldownChangePerHit;
	private GameObject m_knockbackMarkerSequencePrefab;
	private GameObject m_onKnockbackSequencePrefab;

	public DelayedAoeKnockbackEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, KnockbackCenterType knockbackCenterType, int knockbackDelay, int bombAmount, StandardEffectInfo enemyHitEffect, AbilityAreaShape knockbackShape, bool penetrateLos, KnockbackType knockbackType, float knockbackDistance, bool knockbackAdjacentActorsIfPull, GameObject knockbackMarkerSequencePrefab, GameObject onKnockbackSequencePrefab)
		: base(parent, targetSquare, target, caster)
	{
		m_effectName = "Delayed Knockback Effect";
		m_knockbackCenterType = knockbackCenterType;
		m_knockbackDelay = knockbackDelay;
		m_damageAmount = bombAmount;
		m_enemyHitEffect = enemyHitEffect;
		m_knockbackShape = knockbackShape;
		m_bombPenetrateLineOfSight = penetrateLos;
		m_knockbackType = knockbackType;
		m_knockbackDistance = knockbackDistance;
		m_knockbackAdjacentActorsIfPull = knockbackAdjacentActorsIfPull;
		m_knockbackMarkerSequencePrefab = knockbackMarkerSequencePrefab;
		m_onKnockbackSequencePrefab = onKnockbackSequencePrefab;
		m_time.duration = Mathf.Max(1, m_knockbackDelay + 1);
		HitPhase = AbilityPriority.Combat_Knockback;
	}

	public void SetCooldownOnHitConfig(AbilityData.ActionType actionType, int cooldownChangePerHit)
	{
		m_cooldownActionType = actionType;
		m_cooldownChangePerHit = cooldownChangePerHit;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_knockbackMarkerSequencePrefab, GetKnockbackCenterSquare(), new[]
		{
			Target
		}, Target, SequenceSource);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_time.age >= m_knockbackDelay)
		{
			BoardSquare targetSquare = m_knockbackCenterType == KnockbackCenterType.FromTargetSquare
				? TargetSquare
				: Target.GetCurrentBoardSquare();
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			if (AddActorAnimEntryIfHasHits(HitPhase))
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_onKnockbackSequencePrefab, targetSquare, m_effectResults.HitActorsArray(), Target, shallowCopy);
			list.Add(item);
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age < m_knockbackDelay)
		{
			return;
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		BoardSquare knockbackCenterSquare = GetKnockbackCenterSquare();
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_knockbackShape, knockbackCenterSquare.ToVector3(), knockbackCenterSquare);
		bool flag = false;
		List<ActorData> hitActors = GetHitActors(nonActorTargetInfo);
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			actorHitResults.SetBaseDamage(m_damageAmount);
			actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			KnockbackHitData knockbackData;
			if (m_knockbackType == KnockbackType.PullToSource && m_knockbackAdjacentActorsIfPull && Board.Get().GetSquaresAreAdjacent(actorData.GetCurrentBoardSquare(), Target.GetCurrentBoardSquare()))
			{
				Vector3 aimDir = Target.GetFreePos() - actorData.GetFreePos();
				aimDir.y = 0f;
				float distance = 2f;
				if (Board.Get().GetSquaresAreDiagonallyAdjacent(actorData.GetCurrentBoardSquare(), Target.GetCurrentBoardSquare()))
				{
					distance = 2.82f;
				}
				knockbackData = new KnockbackHitData(actorData, Target, KnockbackType.ForwardAlongAimDir, aimDir, centerOfShape, distance);
			}
			else
			{
				knockbackData = new KnockbackHitData(actorData, Target, m_knockbackType, Vector3.forward, centerOfShape, m_knockbackDistance);
			}
			actorHitResults.AddKnockbackData(knockbackData);
			if (!flag && m_cooldownChangePerHit != 0 && m_cooldownActionType != AbilityData.ActionType.INVALID_ACTION)
			{
				int addAmount = m_cooldownChangePerHit * hitActors.Count;
				MiscHitEventData_AddToCasterCooldown hitEvent = new MiscHitEventData_AddToCasterCooldown(m_cooldownActionType, addAmount);
				actorHitResults.AddMiscHitEvent(hitEvent);
				flag = true;
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

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare knockbackCenterSquare = GetKnockbackCenterSquare();
		return AreaEffectUtils.GetActorsInShape(m_knockbackShape, knockbackCenterSquare.ToVector3(), knockbackCenterSquare, m_bombPenetrateLineOfSight, Caster, Caster.GetOtherTeams(), nonActorTargetInfo);
	}

	private BoardSquare GetKnockbackCenterSquare()
	{
		return m_knockbackCenterType == KnockbackCenterType.FromTargetSquare
			? TargetSquare
			: Target.GetCurrentBoardSquare();
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
			BoardSquare knockbackCenterSquare = GetKnockbackCenterSquare();
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_knockbackShape, knockbackCenterSquare.ToVector3(), knockbackCenterSquare, true, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}
}
#endif
