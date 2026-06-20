// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, added in rogues
#if SERVER
public class FishManRoamingDebuffEffect : StandardActorEffect
{
	private float m_jumpRadius;
	private bool m_jumpIgnoresLoS;
	private int m_jumpsRemaining;
	private GameObject m_jumpSequencePrefab;
	private int m_jumpAnimationIndex;
	private bool m_canJumpToEnemies;
	private bool m_canJumpToAllies;
	private bool m_canJumpToInvisibleTargets;
	private FishManRoamingDebuff.RoamingDebuffJumpPreference m_primaryJumpPreference;
	private FishManRoamingDebuff.RoamingDebuffJumpPreference m_secondaryJumpPreference;
	private FishManRoamingDebuff.RoamingDebuffJumpPreference m_tiebreakerJumpPreference;
	private int m_damageToEnemiesOnJump;
	private int m_healingToAlliesOnJump;
	// TODO FISHMAN always 0, actual value pre-added to m_damageToEnemiesOnJump.
	// Actual logic matches in-game description:
	//	Deals an additional [DamageIncreasePerJump_Final] damage when it jumps.
	private int m_damageIncreasePerJump;
	private StandardEffectInfo m_effectWhileOnEnemy;
	private StandardEffectInfo m_effectWhileOnAlly;
	private bool m_addTheatricsEntry;
	private bool m_shouldEndEarly;

	public FishManRoamingDebuffEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData myEffectData,
		float jumpRadius,
		bool jumpIgnoresLoS,
		int jumpsRemaining,
		GameObject jumpSequencePrefab,
		int jumpAnimationIndex,
		bool canJumpToEnemies,
		bool canJumpToAllies,
		bool canJumpToInvisibleTargets,
		FishManRoamingDebuff.RoamingDebuffJumpPreference primaryJumpPreference,
		FishManRoamingDebuff.RoamingDebuffJumpPreference secondaryJumpPreference,
		FishManRoamingDebuff.RoamingDebuffJumpPreference tiebreakerJumpPreference,
		int damageToEnemiesOnJump,
		int healingToAlliesOnJump,
		int damageIncreasePerJump,
		StandardEffectInfo effectWhileOnEnemy,
		StandardEffectInfo effectWhileOnAlly,
		bool addTheatricsEntry)
		: base(parent, targetSquare, target, caster, myEffectData)
	{
		m_jumpRadius = jumpRadius;
		m_jumpIgnoresLoS = jumpIgnoresLoS;
		m_jumpsRemaining = jumpsRemaining;
		m_jumpSequencePrefab = jumpSequencePrefab;
		m_jumpAnimationIndex = jumpAnimationIndex;
		m_canJumpToEnemies = canJumpToEnemies;
		m_canJumpToAllies = canJumpToAllies;
		m_canJumpToInvisibleTargets = canJumpToInvisibleTargets;
		m_primaryJumpPreference = primaryJumpPreference;
		m_secondaryJumpPreference = secondaryJumpPreference;
		m_tiebreakerJumpPreference = tiebreakerJumpPreference;
		m_damageToEnemiesOnJump = damageToEnemiesOnJump;
		m_healingToAlliesOnJump = healingToAlliesOnJump;
		m_damageIncreasePerJump = damageIncreasePerJump;
		m_effectWhileOnEnemy = effectWhileOnEnemy;
		m_effectWhileOnAlly = effectWhileOnAlly;
		m_addTheatricsEntry = addTheatricsEntry;
	}

	private List<ActorData> GetPotentialJumpTargetActors()
	{
		Vector3 targetPos = Target.GetLoSCheckPos();
		List<Team> affectedTeams = new List<Team>();
		if (m_canJumpToAllies)
		{
			affectedTeams.Add(Caster.GetTeam());
		}
		if (m_canJumpToEnemies)
		{
			affectedTeams.AddRange(Caster.GetOtherTeams());
		}
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			targetPos,
			m_jumpRadius,
			m_jumpIgnoresLoS,
			Caster,
			affectedTeams,
			null);
		if (actorsInRadius.Contains(Target))
		{
			actorsInRadius.Remove(Target);
		}
		if (!m_canJumpToInvisibleTargets)
		{
			for (int i = actorsInRadius.Count - 1; i >= 0; i--)
			{
				if (!actorsInRadius[i].IsActorVisibleIgnoringFogOfWar(Caster))
				{
					actorsInRadius.RemoveAt(i);
				}
			}
		}
		return actorsInRadius;
	}

	// rogues
	// public override bool CanExecuteForTeam_FCFS(Team team)
	// {
	// 	return Caster.GetTeam() != team;
	// }

	private bool CanJump(AbilityPriority phaseIndex)
	{
		return phaseIndex == HitPhase && m_jumpsRemaining > 0 && GetPotentialJumpTargetActors().Count > 0;
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (Caster.IsDead() || !CanJump(phaseIndex))
		{
			return 0;
		}
		return m_jumpAnimationIndex;
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return m_addTheatricsEntry && phaseIndex == HitPhase && CanJump(phaseIndex);
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_jumpsRemaining <= 0)
		{
			return;
		}
		Vector3 targetPos = Target.GetLoSCheckPos();
		List<ActorData> potentialJumpTargetActors = GetPotentialJumpTargetActors();
		if (potentialJumpTargetActors.Count == 0)
		{
			base.GatherEffectResults(ref effectResults, isReal);
			return;
		}
		JumpTargetSorter comparer = new JumpTargetSorter(
			Caster.GetTeam(),
			Target.GetCurrentBoardSquare(),
			m_primaryJumpPreference,
			m_secondaryJumpPreference,
			m_tiebreakerJumpPreference);
		potentialJumpTargetActors.Sort(comparer);
		ActorData actorData = potentialJumpTargetActors[0];
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, targetPos));
		if (actorData.GetTeam() == Caster.GetTeam())
		{
			actorHitResults.SetBaseHealing(m_healingToAlliesOnJump);
			actorHitResults.AddEffect(new FishManRoamingDebuffEffect(
				Parent,
				actorData.GetCurrentBoardSquare(),
				actorData,
				Caster,
				m_effectWhileOnAlly.m_effectData,
				m_jumpRadius,
				m_jumpIgnoresLoS,
				m_jumpsRemaining - 1,
				m_jumpSequencePrefab,
				m_jumpAnimationIndex,
				m_canJumpToEnemies,
				m_canJumpToAllies,
				m_canJumpToInvisibleTargets,
				m_primaryJumpPreference,
				m_secondaryJumpPreference,
				m_tiebreakerJumpPreference,
				m_damageToEnemiesOnJump + m_damageIncreasePerJump,
				m_healingToAlliesOnJump,
				m_damageIncreasePerJump,
				m_effectWhileOnEnemy,
				m_effectWhileOnAlly,
				m_addTheatricsEntry));
		}
		else
		{
			actorHitResults.SetBaseDamage(m_damageToEnemiesOnJump);
			actorHitResults.AddEffect(new FishManRoamingDebuffEffect(
				Parent,
				actorData.GetCurrentBoardSquare(),
				actorData,
				Caster,
				m_effectWhileOnEnemy.m_effectData,
				m_jumpRadius,
				m_jumpIgnoresLoS,
				m_jumpsRemaining - 1,
				m_jumpSequencePrefab,
				m_jumpAnimationIndex,
				m_canJumpToEnemies,
				m_canJumpToAllies,
				m_canJumpToInvisibleTargets,
				m_primaryJumpPreference,
				m_secondaryJumpPreference,
				m_tiebreakerJumpPreference,
				m_damageToEnemiesOnJump + m_damageIncreasePerJump,
				m_healingToAlliesOnJump,
				m_damageIncreasePerJump,
				m_effectWhileOnEnemy,
				m_effectWhileOnAlly,
				m_addTheatricsEntry));
		}
		actorHitResults.AddEffectForRemoval(this, ServerEffectManager.Get().GetActorEffects(Target));
		effectResults.StoreActorHit(actorHitResults);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(Vector3.zero));
		foreach (GameObject prefab in m_data.m_sequencePrefabs)
		{
			if (prefab != null)
			{
				positionHitResults.AddEffectSequenceToEnd(prefab, m_guid);
			}
		}
		effectResults.StorePositionHit(positionHitResults);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_data.m_sequencePrefabs.Length == 0)
		{
			return list;
		}
		foreach (GameObject prefab in m_data.m_sequencePrefabs)
		{
			Sequence.IExtraSequenceParams[] extraParams = null;
			if (m_jumpsRemaining <= 0)
			{
				extraParams = new Sequence.PhaseTimingExtraParams
				{
					m_turnDelayEndOverride = 0
				}.ToArray();
			}

			list.Add(new ServerClientUtils.SequenceStartData(
				prefab, TargetSquare, Target.AsArray(), Caster, SequenceSource, extraParams));
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		ActorData[] array = m_effectResults.HitActorsArray();
		if (array.Length == 0)
		{
			return new List<ServerClientUtils.SequenceStartData>();
		}
		SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
		if (AddActorAnimEntryIfHasHits(HitPhase))
		{
			shallowCopy.SetWaitForClientEnable(true);
		}
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_jumpSequencePrefab,
				array[0].GetFreePos(),
				m_effectResults.HitActorsArray(),
				Caster,
				shallowCopy,
				new SplineProjectileSequence.DelayedProjectileExtraParams
				{
					useOverrideStartPos = true,
					overrideStartPos = Target.GetFreePos()
				}.ToArray()),
			new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				Vector3.zero,
				null,
				Caster,
				shallowCopy)
		};
	}

	public override List<Vector3> CalcPointsOfInterestForCamera()
	{
		List<Vector3> list = new List<Vector3>();
		ActorData[] array = m_effectResults.HitActorsArray();
		if (Target != null && Target.GetCurrentBoardSquare() != null)
		{
			list.Add(Target.GetFreePos());
		}
		if (array.Length != 0)
		{
			list.Add(array[0].GetFreePos());
		}
		return list;
	}

	public override ActorData GetActorAnimationActor()
	{
		return Target;
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (phase == AbilityPriority.Combat_Final
		    && m_time.age >= m_time.duration - 1
		    && m_time.duration > 0)
		{
			m_shouldEndEarly = true;
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_shouldEndEarly;
	}

	public class JumpTargetSorter : IComparer<ActorData>
	{
		private Team m_casterTeam;
		private BoardSquare m_jumpFromSquare;
		private FishManRoamingDebuff.RoamingDebuffJumpPreference m_primaryJumpPreference;
		private FishManRoamingDebuff.RoamingDebuffJumpPreference m_secondaryJumpPreference;
		private FishManRoamingDebuff.RoamingDebuffJumpPreference m_tiebreakerJumpPreference;

		public JumpTargetSorter(
			Team casterTeam,
			BoardSquare jumpFromSquare,
			FishManRoamingDebuff.RoamingDebuffJumpPreference primaryJumpPreference,
			FishManRoamingDebuff.RoamingDebuffJumpPreference secondaryJumpPreference,
			FishManRoamingDebuff.RoamingDebuffJumpPreference tiebreakerJumpPreference)
		{
			m_casterTeam = casterTeam;
			m_jumpFromSquare = jumpFromSquare;
			m_primaryJumpPreference = primaryJumpPreference;
			m_secondaryJumpPreference = secondaryJumpPreference;
			m_tiebreakerJumpPreference = tiebreakerJumpPreference;
		}

		public int Compare(ActorData a, ActorData b)
		{
			int num = CompareActorsWrtPreference(a, b, m_primaryJumpPreference);
			if (num == 0)
			{
				num = CompareActorsWrtPreference(a, b, m_secondaryJumpPreference);
			}
			if (num == 0)
			{
				num = CompareActorsWrtPreference(a, b, m_tiebreakerJumpPreference);
			}
			return num;
		}

		private int CompareActorsWrtPreference(ActorData a, ActorData b, FishManRoamingDebuff.RoamingDebuffJumpPreference jumpPreference)
		{
			float distA = a.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(m_jumpFromSquare);
			float distB = b.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(m_jumpFromSquare);
			bool enemyA = a.GetTeam() != m_casterTeam;
			bool enemyB = b.GetTeam() != m_casterTeam;
			int hitPointsA = a.HitPoints;
			int hitPointsB = b.HitPoints;
			switch (jumpPreference)
			{
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.Closest:
					return distA.CompareTo(distB);
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.Farthest:
					return distB.CompareTo(distA);
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.Ally:
					return enemyA.CompareTo(enemyB);
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.Enemy:
					return enemyB.CompareTo(enemyA);
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.LeastHP:
					return hitPointsA.CompareTo(hitPointsB);
				case FishManRoamingDebuff.RoamingDebuffJumpPreference.MostHP:
					return hitPointsB.CompareTo(hitPointsA);
				default:
					return 0;
			}
		}
	}
}
#endif
