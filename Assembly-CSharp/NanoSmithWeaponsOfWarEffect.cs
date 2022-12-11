// SERVER
// ROGUES

using System.Collections.Generic;
using UnityEngine;

// added in rogues
#if SERVER
public class NanoSmithWeaponsOfWarEffect : Effect
{
	private int m_sweepDamageAmount;
	private AbilityAreaShape m_sweepShape;
	private bool m_sweepPenetrateLos;
	private bool m_sweepIncludeTarget;
	private bool m_includeEnemies = true;
	private bool m_includeAllies;
	private int m_sweepDamageDelay;

	public StandardEffectInfo m_enemyOnHitEffect;
	public StandardEffectInfo m_allyOnHitEffect;

	private GameObject m_persistentSequencePrefab;
	private GameObject m_rangeIndicatorSequencePrefab;
	private GameObject m_bladeSequencePrefab;
	private bool m_absorbedDamage;

	public NanoSmithWeaponsOfWarEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		int sweepDuration,
		int sweepDamageAmount,
		AbilityAreaShape sweepShape,
		bool sweepPenetrateLos,
		bool sweepIncludeTarget,
		bool includeEnemies,
		bool includeAllies,
		int sweepDamageDelay,
		StandardEffectInfo enemyOnHitEffect,
		StandardEffectInfo allyOnHitEffect,
		GameObject persistentSequencePrefab,
		GameObject rangeIndicatorPrefab,
		GameObject bladeSequencePrefab)
		: base(parent, targetSquare, target, caster)
	{
		m_time.duration = sweepDuration;
		m_sweepDamageAmount = sweepDamageAmount;
		m_sweepShape = sweepShape;
		m_sweepPenetrateLos = sweepPenetrateLos;
		m_sweepIncludeTarget = sweepIncludeTarget;
		m_includeEnemies = includeEnemies;
		m_includeAllies = includeAllies;
		HitPhase = AbilityPriority.Combat_Damage;
		m_sweepDamageDelay = sweepDamageDelay;
		m_enemyOnHitEffect = enemyOnHitEffect;
		m_allyOnHitEffect = allyOnHitEffect;
		m_persistentSequencePrefab = persistentSequencePrefab;
		m_rangeIndicatorSequencePrefab = rangeIndicatorPrefab;
		m_bladeSequencePrefab = bladeSequencePrefab;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		list.Add(new ServerClientUtils.SequenceStartData(m_persistentSequencePrefab, Target.GetFreePos(), Target.AsArray(), Target, SequenceSource));
		if (m_rangeIndicatorSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_rangeIndicatorSequencePrefab, Target.GetFreePos(), Target.AsArray(), Target, SequenceSource));
		}
		return list;
	}

	public override ServerClientUtils.SequenceStartData GetEffectHitSeqData()
	{
		if (m_time.age >= m_sweepDamageDelay)
		{
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			if (AddActorAnimEntryIfHasHits(HitPhase))
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			return new ServerClientUtils.SequenceStartData(m_bladeSequencePrefab, Target.GetFreePos(), m_effectResults.HitActorsArray(), Target, shallowCopy);
		}
		return null;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age >= m_sweepDamageDelay)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> targetsInSweepShape = GetTargetsInSweepShape(nonActorTargetInfo);
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_sweepShape, Target.GetFreePos(), Target.GetCurrentBoardSquare());
			foreach (ActorData actorData in targetsInSweepShape)
			{
				ActorHitParameters hitParams = new ActorHitParameters(actorData, centerOfShape);
				if (actorData.GetTeam() != Caster.GetTeam())
				{
					effectResults.StoreActorHit(new ActorHitResults(m_sweepDamageAmount, HitActionType.Damage, m_enemyOnHitEffect, hitParams));
				}
				else
				{
					effectResults.StoreActorHit(new ActorHitResults(m_allyOnHitEffect, hitParams));
				}
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
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
		return m_reasonsToEndEarly.Count > 0 || Target == null || Target.IsDead();
	}

	private List<ActorData> GetTargetsInSweepShape(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> list = new List<Team>();
		if (m_includeEnemies)
		{
			list.AddRange(Caster.GetOtherTeams());
		}
		if (m_includeAllies)
		{
			list.Add(Caster.GetTeam());
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_sweepShape, Target.GetFreePos(), Target.GetCurrentBoardSquare(), m_sweepPenetrateLos, Target, list, nonActorTargetInfo);
		if (!m_sweepIncludeTarget && actorsInShape.Contains(Target))
		{
			actorsInShape.Remove(Target);
		}
		return actorsInShape;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && Target != null && Target.GetCurrentBoardSquare() != null)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_sweepShape, Target.GetFreePos(), Target.GetCurrentBoardSquare(), true, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}

	public override void OnStart()
	{
		Passive_Nanosmith component = Caster.GetComponent<Passive_Nanosmith>();
		if (component != null)
		{
			component.OnShieldApplied();
		}
		base.OnStart();
	}

	public override void OnAbsorbedDamage(int damageAbsorbed)
	{
		if (!m_absorbedDamage)
		{
			m_absorbedDamage = true;
			Passive_Nanosmith component = Caster.GetComponent<Passive_Nanosmith>();
			if (component != null)
			{
				component.OnShieldAbsorbedDamage();
			}
		}
		base.OnAbsorbedDamage(damageAbsorbed);
	}
}
#endif
