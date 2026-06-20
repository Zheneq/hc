// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// missing in reactor
public class SorceressDamageFieldEffect : Effect
{
	private GridPos m_gridTargetPos;
	private Vector3 m_freePos;
	private Vector3 m_sequencePosition;
	private List<ActorData> m_actorsHitThisTurn;
	private List<ActorData> m_actorsHitThisTurn_fake;
	private List<BoardSquare> m_affectedSquares;

	public bool m_penetrateLoS;
	public AbilityAreaShape m_shape;
	public int m_damage;
	public int m_healing;

	private StandardEffectInfo m_effectOnEnemies;
	private StandardEffectInfo m_effectOnAllies;
	private GameObject m_hittingEnemyPrefab;
	private GameObject m_hittingAllyPrefab;
	private GameObject m_persistentGroundPrefab;
	private GameObject m_onHitPulsePrefab;

	public List<BoardSquare> AffectedSquares
	{
		get
		{
			return m_affectedSquares;
		}
	}

	public SorceressDamageFieldEffect(
		EffectSource parent,
		ActorData caster,
		GridPos gridTargetPos,
		Vector3 freePos,
		int duration,
		bool penetrateLoS,
		AbilityAreaShape shape,
		int damage,
		int healing,
		StandardEffectInfo effectOnEnemies,
		StandardEffectInfo effectOnAllies,
		List<ActorData> actorsAlreadyHit,
		GameObject hittingEnemyPrefab,
		GameObject hittingAllyPrefab,
		GameObject persistentGroundPrefab,
		GameObject hitPulsePrefab,
		SequenceSource parentSequenceSource)
		: base(parent, null, null, caster)
	{
		m_gridTargetPos = gridTargetPos;
		m_freePos = freePos;
		m_time.duration = duration;
		m_penetrateLoS = penetrateLoS;
		m_shape = shape;
		m_damage = damage;
		m_healing = healing;
		m_effectOnEnemies = effectOnEnemies;
		m_effectOnAllies = effectOnAllies;
		m_actorsHitThisTurn = actorsAlreadyHit;
		m_actorsHitThisTurn_fake = new List<ActorData>();
		m_hittingEnemyPrefab = hittingEnemyPrefab;
		m_hittingAllyPrefab = hittingAllyPrefab;
		m_persistentGroundPrefab = persistentGroundPrefab;
		m_onHitPulsePrefab = hitPulsePrefab;
		HitPhase = AbilityPriority.Combat_Damage;
		m_effectName = "Ion Cloud";
		m_affectedSquares = new List<BoardSquare>();
		SequenceSource = new SequenceSource(OnHit_Base, OnHit_Base, false, parentSequenceSource);
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_persistentGroundPrefab,
			Board.Get().GetSquare(m_gridTargetPos),
			null,
			Caster,
			SequenceSource);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] hitActorsArray = m_effectResults.HitActorsArray();
		List<ActorData> hitEnemies = new List<ActorData>();
		List<ActorData> hitAllies = new List<ActorData>();
		foreach (ActorData actorData in hitActorsArray)
		{
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				hitAllies.Add(actorData);
			}
			else
			{
				hitEnemies.Add(actorData);
			}
		}
		SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
		if (AddActorAnimEntryIfHasHits(HitPhase))
		{
			shallowCopy.SetWaitForClientEnable(true);
		}
		BoardSquare square = Board.Get().GetSquare(m_gridTargetPos);
		if (m_hittingAllyPrefab != null && hitAllies.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_hittingAllyPrefab, square, hitAllies.ToArray(), Caster, shallowCopy));
		}
		if (m_hittingEnemyPrefab != null && hitEnemies.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_hittingEnemyPrefab, square, hitEnemies.ToArray(), Caster, shallowCopy));
		}
		if (m_onHitPulsePrefab != null && hitActorsArray.Length != 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_onHitPulsePrefab, m_sequencePosition, hitActorsArray, Caster, shallowCopy));
		}
		return list;
	}

	public override void OnStart()
	{
		BoardSquare square = Board.Get().GetSquare(m_gridTargetPos);
		m_sequencePosition = square.ToVector3();
		m_affectedSquares = AreaEffectUtils.GetSquaresInShape(m_shape, m_freePos, square, m_penetrateLoS, Caster);
	}

	private void SetupHitResults(ActorData onActor, ActorHitResults hitRes)
	{
		if (onActor.GetTeam() == Caster.GetTeam())
		{
			hitRes.AddBaseHealing(m_healing);
			hitRes.AddStandardEffectInfo(m_effectOnAllies);
		}
		else
		{
			hitRes.AddBaseDamage(m_damage);
			hitRes.AddStandardEffectInfo(m_effectOnEnemies);
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(m_gridTargetPos);
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		if (m_damage > 0 || m_effectOnEnemies.m_applyEffect)
		{
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				m_shape,
				m_freePos,
				targetSquare,
				m_penetrateLoS,
				Caster,
				Caster.GetOtherTeams(),
				nonActorTargetInfo);
			foreach (ActorData actorData in actorsInShape)
			{
				if (!GetActorsHitThisTurn(isReal).Contains(actorData))
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, m_freePos));
					SetupHitResults(actorData, actorHitResults);
					effectResults.StoreActorHit(actorHitResults);
					AddActorHitThisTurn(actorData, isReal);
				}
			}
		}
		if (m_healing > 0 || m_effectOnAllies.m_applyEffect)
		{
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				m_shape,
				m_freePos,
				targetSquare,
				m_penetrateLoS,
				Caster,
				Caster.GetTeam(),
				nonActorTargetInfo);
			foreach (ActorData actorData in actorsInShape)
			{
				if (!GetActorsHitThisTurn(isReal).Contains(actorData))
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, m_freePos));
					SetupHitResults(actorData, actorHitResults);
					effectResults.StoreActorHit(actorHitResults);
					AddActorHitThisTurn(actorData, isReal);
				}
			}
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	public override ActorData GetActorAnimationActor()
	{
		foreach (ActorData actorData in m_effectResults.HitActorsArray())
		{
			if (actorData != null && !actorData.IsDead())
			{
				return actorData;
			}
		}
		return Caster;
	}

	public override void OnTurnStart()
	{
		m_actorsHitThisTurn.Clear();
		m_actorsHitThisTurn_fake.Clear();
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_actorsHitThisTurn.Clear();
			m_actorsHitThisTurn_fake.Clear();
		}
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		List<ServerAbilityUtils.TriggeringPathInfo> triggeringPathInfos = new List<ServerAbilityUtils.TriggeringPathInfo>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			ActorData mover = movementInstance.m_mover;
			if (!m_actorsHitThisTurn.Contains(mover) && !m_affectedSquares.Contains(mover.GetCurrentBoardSquare()))
			{
				for (BoardSquarePathInfo step = movementInstance.m_path; step != null; step = step.next)
				{
					if ((movementInstance.m_groundBased || step.IsPathEndpoint())
					    && !step.IsPathStartPoint()
					    && m_affectedSquares.Contains(step.square))
					{
						triggeringPathInfos.Add(new ServerAbilityUtils.TriggeringPathInfo(mover, step));
						break;
					}
				}
			}
		}
		foreach (ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo in triggeringPathInfos)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
			SetupHitResults(triggeringPathInfo.m_mover, actorHitResults);
			GameObject sequencePrefab = triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam()
				? m_hittingEnemyPrefab
				: m_hittingAllyPrefab;
			MovementResults movementResults = new MovementResults(movement.m_movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, actorHitResults);
			movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, SequenceSource);
			movementResultsList.Add(movementResults);
			m_actorsHitThisTurn.Add(triggeringPathInfo.m_mover);
		}
	}

	public override void GatherMovementResultsFromSegment(
		ActorData mover,
		MovementInstance movementInstance,
		MovementStage movementStage,
		BoardSquarePathInfo sourcePath,
		BoardSquarePathInfo destPath,
		ref List<MovementResults> movementResultsList)
	{
		if (m_actorsHitThisTurn.Contains(mover)
		    || m_affectedSquares.Contains(sourcePath.square)
		    || !m_affectedSquares.Contains(destPath.square)
		    || !movementInstance.m_groundBased && !destPath.IsPathEndpoint())
		{
			return;
		}
		ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo = new ServerAbilityUtils.TriggeringPathInfo(mover, destPath);
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
		SetupHitResults(triggeringPathInfo.m_mover, actorHitResults);
		GameObject sequencePrefab = triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam()
			? m_hittingEnemyPrefab
			: m_hittingAllyPrefab;
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(triggeringPathInfo);
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, SequenceSource);
		movementResultsList.Add(movementResults);
		m_actorsHitThisTurn.Add(triggeringPathInfo.m_mover);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && m_affectedSquares != null)
		{
			squaresToAvoid.UnionWith(m_affectedSquares);
		}
	}

	private List<ActorData> GetActorsHitThisTurn(bool isReal)
	{
		return isReal ? m_actorsHitThisTurn : m_actorsHitThisTurn_fake;
	}

	private void AddActorHitThisTurn(ActorData actor, bool isReal)
	{
		if (isReal)
		{
			m_actorsHitThisTurn.Add(actor);
		}
		else
		{
			m_actorsHitThisTurn_fake.Add(actor);
		}
	}
}
#endif
