// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class StandardGroundEffect : Effect
{
	public GroundEffectField m_fieldInfo;
	protected HashSet<BoardSquare> m_affectedSquares;
	protected List<StandardGroundEffect> m_linkedGroundEffects;
	protected HashSet<ActorData> m_actorsHitThisTurn;
	protected HashSet<ActorData> m_actorsHitThisTurn_fake;
	public Vector3 m_shapeFreePos;
	private Quaternion m_sequenceOrientation = Quaternion.identity;

	public StandardGroundEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		Vector3 shapeFreePos,
		ActorData target,
		ActorData caster,
		GroundEffectField fieldInfo)
		: base(parent, targetSquare, target, caster)
	{
		m_time.duration = fieldInfo.duration;
		m_fieldInfo = fieldInfo;
		m_shapeFreePos = shapeFreePos;
		HitPhase = AbilityPriority.Combat_Damage;
		m_affectedSquares = new HashSet<BoardSquare>();
		m_actorsHitThisTurn = new HashSet<ActorData>();
		m_actorsHitThisTurn_fake = new HashSet<ActorData>();
	}

	public void SetLinkedGroundEffects(List<StandardGroundEffect> linkedGroundEffects)
	{
		m_linkedGroundEffects = linkedGroundEffects;
	}

	public override void OnStart()
	{
		CalculateAffectedSquares();
	}

	protected void CalculateAffectedSquares()
	{
		foreach (BoardSquare square in GetSquaresInShape())
		{
			m_affectedSquares.Add(square);
		}
	}
	
	// custom
	public HashSet<BoardSquare> AffectedSquares => m_affectedSquares;

	// custom
	public List<BoardSquare> GetSquaresInShape()
	{
		return AreaEffectUtils.GetSquaresInShape(m_fieldInfo.shape, m_shapeFreePos, TargetSquare, m_fieldInfo.penetrateLos, Caster);
	}

	public void AddToActorsHitThisTurn(List<ActorData> newActorsToExcludeThisTurn)
	{
		foreach (ActorData item in newActorsToExcludeThisTurn)
		{
			m_actorsHitThisTurn.Add(item);
			m_actorsHitThisTurn_fake.Add(item);
		}
	}

	public void SetSequenceOrientation(Quaternion orientation)
	{
		m_sequenceOrientation = orientation;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_fieldInfo.perSquareSequences)
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_fieldInfo.shape, m_shapeFreePos, TargetSquare, m_fieldInfo.penetrateLos, Caster);
			foreach (BoardSquare boardSquare in squaresInShape)
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					m_fieldInfo.persistentSequencePrefab,
					boardSquare.ToVector3(),
					m_sequenceOrientation,
					null,
					Caster,
					SequenceSource));
			}
		}
		else
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_fieldInfo.persistentSequencePrefab,
				GetShapeCenter(),
				m_sequenceOrientation,
				null,
				Caster,
				SequenceSource));
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (!ShouldHitThisTurn())
		{
			return list;
		}
		Vector3 shapeCenter = GetShapeCenter();
		ActorData[] hitActors = m_effectResults.HitActorsArray();
		List<ActorData> enemiesHit = new List<ActorData>();
		List<ActorData> alliesHit = new List<ActorData>();
		foreach (ActorData actorData in hitActors)
		{
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				alliesHit.Add(actorData);
			}
			else
			{
				enemiesHit.Add(actorData);
			}
		}
		SequenceSource sequence = SequenceSource.GetShallowCopy();
		if (AddActorAnimEntryIfHasHits(HitPhase))
		{
			sequence.SetWaitForClientEnable(true);
		}
		if (m_fieldInfo.allyHitSequencePrefab != null && alliesHit.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_fieldInfo.allyHitSequencePrefab, TargetSquare, alliesHit.ToArray(), Caster, sequence));
		}
		if (m_fieldInfo.enemyHitSequencePrefab != null && enemiesHit.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_fieldInfo.enemyHitSequencePrefab, TargetSquare, enemiesHit.ToArray(), Caster, sequence));
		}
		GameObject prefab = m_fieldInfo.hitPulseSequencePrefab;
		if (prefab == null
		    && m_fieldInfo.allyHitSequencePrefab == null
		    && m_fieldInfo.enemyHitSequencePrefab == null)
		{
			prefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
		}
		if (prefab != null && hitActors.Length != 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(prefab, shapeCenter, hitActors, Caster, sequence));
		}
		return list;
	}

	public Vector3 GetShapeCenter()
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_fieldInfo.shape, m_shapeFreePos, TargetSquare);
		centerOfShape.y = Board.Get().BaselineHeight;
		return centerOfShape;
	}

	public virtual bool ShouldHitThisTurn()
	{
		return m_fieldInfo.hitDelayTurns <= 0
		       || m_time.age >= m_fieldInfo.hitDelayTurns;
	}

	public override void OnTurnStart()
	{
		// rogues
		// if (base.Caster.GetTeam() == GameFlowData.Get().ActingTeam)
		// {
			m_actorsHitThisTurn.Clear();
			m_actorsHitThisTurn_fake.Clear();
		// }
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		// rogues
		// Log.Error("Effect Phase Start called");
	}

	public virtual void SetupActorHitResults(ref ActorHitResults actorHitRes, BoardSquare targetSquare)
	{
		if (ShouldHitThisTurn())
		{
			m_fieldInfo.SetupActorHitResult(ref actorHitRes, Caster, targetSquare);
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ShouldHitThisTurn())
		{
			return;
		}
		bool includeEnemies = m_fieldInfo.IncludeEnemies();
		bool includeAllies = m_fieldInfo.IncludeAllies();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(Caster, includeAllies, includeEnemies);
		if (relevantTeams.Count <= 0)
		{
			return;
		}

		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_fieldInfo.shape, m_shapeFreePos, TargetSquare, m_fieldInfo.penetrateLos, Caster, relevantTeams, null);
		foreach (ActorData actorData in actorsInShape)
		{
			if (!IsActorHitThisTurn(actorData, isReal) && m_fieldInfo.CanBeAffected(actorData, Caster))
			{
				ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
				SetupActorHitResults(ref hitResults, TargetSquare);
				effectResults.StoreActorHit(hitResults);
				AddActorHitThisTurn(actorData, isReal);
			}
		}
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return !Caster.IsDead();
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

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly()
		       || (m_fieldInfo.endIfHasDoneHits
		           && m_actorsHitThisTurn.Count > 0
		           && ServerActionBuffer.Get().AbilityPhase >= HitPhase);
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_fieldInfo.ignoreMovementHits || !ShouldHitThisTurn())
		{
			return;
		}
		bool includeEnemies = m_fieldInfo.IncludeEnemies();
		bool includeAllies = m_fieldInfo.IncludeAllies();
		List<ServerAbilityUtils.TriggeringPathInfo> triggeringPaths = new List<ServerAbilityUtils.TriggeringPathInfo>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			ActorData mover = movementInstance.m_mover;
			bool isEnemy = mover.GetTeam() != Caster.GetTeam();
			if (m_fieldInfo.CanBeAffected(mover, Caster)
			    && !m_affectedSquares.Contains(mover.GetCurrentBoardSquare()))
			{
				for (BoardSquarePathInfo step = movementInstance.m_path; step != null; step = step.next)
				{
					BoardSquare square = step.square;
					if ((movementInstance.m_groundBased || step.IsPathEndpoint())
					    && !step.IsPathStartPoint()
					    && ((isEnemy && includeEnemies) || (!isEnemy && includeAllies))
					    && m_affectedSquares.Contains(square)
					    && !m_actorsHitThisTurn.Contains(mover))
					{
						triggeringPaths.Add(new ServerAbilityUtils.TriggeringPathInfo(mover, step));
						m_actorsHitThisTurn.Add(mover);
					}
				}
			}
		}
		foreach (ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo in triggeringPaths)
		{
			ActorHitResults reactionHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
			SetupActorHitResults(ref reactionHitResults, triggeringPathInfo.m_triggeringPathSegment.square);
			GameObject sequencePrefab = triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam()
				? m_fieldInfo.enemyHitSequencePrefab
				: m_fieldInfo.allyHitSequencePrefab;
			MovementResults movementResults = new MovementResults(movement.m_movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, reactionHitResults);
			movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, SequenceSource);
			movementResultsList.Add(movementResults);
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
		if (m_fieldInfo.ignoreMovementHits
		    || !ShouldHitThisTurn()
		    || m_actorsHitThisTurn.Contains(mover)
		    || m_affectedSquares.Contains(sourcePath.square)
		    || !m_affectedSquares.Contains(destPath.square)
		    || !m_fieldInfo.CanBeAffected(mover, Caster)
		    || (!movementInstance.m_groundBased && !destPath.IsPathEndpoint()))
		{
			return;
		}
		ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo = new ServerAbilityUtils.TriggeringPathInfo(mover, destPath);
		ActorHitResults reactionHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
		SetupActorHitResults(ref reactionHitResults, triggeringPathInfo.m_triggeringPathSegment.square);
		GameObject sequencePrefab = triggeringPathInfo.m_mover.GetTeam() != Caster.GetTeam()
			? m_fieldInfo.enemyHitSequencePrefab
			: m_fieldInfo.allyHitSequencePrefab;
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(triggeringPathInfo);
		movementResults.SetupGameplayData(this, reactionHitResults);
		movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, SequenceSource);
		movementResultsList.Add(movementResults);
		m_actorsHitThisTurn.Add(mover);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && m_affectedSquares != null)
		{
			squaresToAvoid.UnionWith(m_affectedSquares);
		}
	}

	public override bool IsMovementBlockedOnEnterSquare(ActorData mover, BoardSquare movingFrom, BoardSquare movingTo)
	{
		return mover.GetTeam() != Caster.GetTeam()
		       && m_affectedSquares != null
		       && (m_fieldInfo.stopMovementInField && m_affectedSquares.Contains(movingTo)
		           || m_fieldInfo.stopMovementOutOfField && m_affectedSquares.Contains(movingFrom));
	}

	public bool IsActorHitThisTurn(ActorData actor, bool isReal) // private in rogues
	{
		bool isActorHit = isReal
			? m_actorsHitThisTurn.Contains(actor)
			: m_actorsHitThisTurn_fake.Contains(actor);

		if (isActorHit || m_linkedGroundEffects == null)
		{
			return isActorHit;
		}
		foreach (StandardGroundEffect groundEffect in m_linkedGroundEffects)
		{
			if (groundEffect == this || groundEffect == null)
			{
				continue;
			}
			isActorHit = isReal
				? groundEffect.m_actorsHitThisTurn.Contains(actor)
				: groundEffect.m_actorsHitThisTurn_fake.Contains(actor);
			if (isActorHit)
			{
				break;
			}
		}
		return isActorHit;
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
