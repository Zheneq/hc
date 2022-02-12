// ROGUES
// SERVER
using System;
using System.Collections.Generic;
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

	public StandardGroundEffect(EffectSource parent, BoardSquare targetSquare, Vector3 shapeFreePos, ActorData target, ActorData caster, GroundEffectField fieldInfo)
		: base(parent, targetSquare, target, caster)
	{
		this.m_time.duration = fieldInfo.duration;
		this.m_fieldInfo = fieldInfo;
		this.m_shapeFreePos = shapeFreePos;
		base.HitPhase = AbilityPriority.Combat_Damage;
		this.m_affectedSquares = new HashSet<BoardSquare>();
		this.m_actorsHitThisTurn = new HashSet<ActorData>();
		this.m_actorsHitThisTurn_fake = new HashSet<ActorData>();
	}

	public void SetLinkedGroundEffects(List<StandardGroundEffect> linkedGroundEffects)
	{
		this.m_linkedGroundEffects = linkedGroundEffects;
	}

	public override void OnStart()
	{
		this.CalculateAffectedSquares();
	}

	protected void CalculateAffectedSquares()
	{
		foreach (BoardSquare item in AreaEffectUtils.GetSquaresInShape(this.m_fieldInfo.shape, this.m_shapeFreePos, base.TargetSquare, this.m_fieldInfo.penetrateLos, base.Caster))
		{
			this.m_affectedSquares.Add(item);
		}
	}

	public void AddToActorsHitThisTurn(List<ActorData> newActorsToExcludeThisTurn)
	{
		foreach (ActorData item in newActorsToExcludeThisTurn)
		{
			this.m_actorsHitThisTurn.Add(item);
			this.m_actorsHitThisTurn_fake.Add(item);
		}
	}

	public void SetSequenceOrientation(Quaternion orientation)
	{
		this.m_sequenceOrientation = orientation;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (this.m_fieldInfo.perSquareSequences)
		{
			using (List<BoardSquare>.Enumerator enumerator = AreaEffectUtils.GetSquaresInShape(this.m_fieldInfo.shape, this.m_shapeFreePos, base.TargetSquare, this.m_fieldInfo.penetrateLos, base.Caster).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare boardSquare = enumerator.Current;
					ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(this.m_fieldInfo.persistentSequencePrefab, boardSquare.ToVector3(), this.m_sequenceOrientation, null, base.Caster, base.SequenceSource, null);
					list.Add(item);
				}
				return list;
			}
		}
		list.Add(new ServerClientUtils.SequenceStartData(this.m_fieldInfo.persistentSequencePrefab, this.GetShapeCenter(), this.m_sequenceOrientation, null, base.Caster, base.SequenceSource, null));
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (this.ShouldHitThisTurn())
		{
			Vector3 shapeCenter = this.GetShapeCenter();
			ActorData[] array = this.m_effectResults.HitActorsArray();
			List<ActorData> list2 = new List<ActorData>();
			List<ActorData> list3 = new List<ActorData>();
			foreach (ActorData actorData in array)
			{
				if (actorData.GetTeam() == base.Caster.GetTeam())
				{
					list3.Add(actorData);
				}
				else
				{
					list2.Add(actorData);
				}
			}
			SequenceSource shallowCopy = base.SequenceSource.GetShallowCopy();
			if (this.AddActorAnimEntryIfHasHits(base.HitPhase))
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			if (this.m_fieldInfo.allyHitSequencePrefab != null && list3.Count > 0)
			{
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(this.m_fieldInfo.allyHitSequencePrefab, base.TargetSquare, list3.ToArray(), base.Caster, shallowCopy, null);
				list.Add(item);
			}
			if (this.m_fieldInfo.enemyHitSequencePrefab != null && list2.Count > 0)
			{
				ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(this.m_fieldInfo.enemyHitSequencePrefab, base.TargetSquare, list2.ToArray(), base.Caster, shallowCopy, null);
				list.Add(item2);
			}
			GameObject gameObject = this.m_fieldInfo.hitPulseSequencePrefab;
			if (gameObject == null && this.m_fieldInfo.allyHitSequencePrefab == null && this.m_fieldInfo.enemyHitSequencePrefab == null)
			{
				gameObject = SequenceLookup.Get().GetSimpleHitSequencePrefab();
			}
			if (gameObject != null && array.Length != 0)
			{
				ServerClientUtils.SequenceStartData item3 = new ServerClientUtils.SequenceStartData(gameObject, shapeCenter, array, base.Caster, shallowCopy, null);
				list.Add(item3);
			}
		}
		return list;
	}

	public Vector3 GetShapeCenter()
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_fieldInfo.shape, this.m_shapeFreePos, base.TargetSquare);
		centerOfShape.y = (float)Board.Get().BaselineHeight;
		return centerOfShape;
	}

	public virtual bool ShouldHitThisTurn()
	{
		return this.m_fieldInfo.hitDelayTurns <= 0 || this.m_time.age >= this.m_fieldInfo.hitDelayTurns;
	}

	public override void OnTurnStart()
	{
		// rogues
		//if (base.Caster.GetTeam() == GameFlowData.Get().ActingTeam)
		//{
			this.m_actorsHitThisTurn.Clear();
			this.m_actorsHitThisTurn_fake.Clear();
		//}
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		Log.Error("Effect Phase Start called");
	}

	public virtual void SetupActorHitResults(ref ActorHitResults actorHitRes, BoardSquare targetSquare)
	{
		if (this.ShouldHitThisTurn())
		{
			this.m_fieldInfo.SetupActorHitResult(ref actorHitRes, base.Caster, targetSquare, 1);
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!this.ShouldHitThisTurn())
		{
			return;
		}
		bool includeEnemies = this.m_fieldInfo.IncludeEnemies();
		bool includeAllies = this.m_fieldInfo.IncludeAllies();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(base.Caster, includeAllies, includeEnemies);
		if (relevantTeams.Count > 0)
		{
			foreach (ActorData actorData in AreaEffectUtils.GetActorsInShape(this.m_fieldInfo.shape, this.m_shapeFreePos, base.TargetSquare, this.m_fieldInfo.penetrateLos, base.Caster, relevantTeams, null))
			{
				if (!this.IsActorHitThisTurn(actorData, isReal) && this.m_fieldInfo.CanBeAffected(actorData, base.Caster))
				{
					ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
					this.SetupActorHitResults(ref hitResults, base.TargetSquare);
					effectResults.StoreActorHit(hitResults);
					this.AddActorHitThisTurn(actorData, isReal);
				}
			}
		}
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return !base.Caster.IsDead();
	}

	public override ActorData GetActorAnimationActor()
	{
		foreach (ActorData actorData in this.m_effectResults.HitActorsArray())
		{
			if (actorData != null && !actorData.IsDead())
			{
				return actorData;
			}
		}
		return base.Caster;
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || (this.m_fieldInfo.endIfHasDoneHits && this.m_actorsHitThisTurn.Count > 0 && ServerActionBuffer.Get().AbilityPhase >= base.HitPhase);
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (this.m_fieldInfo.ignoreMovementHits || !this.ShouldHitThisTurn())
		{
			return;
		}
		bool flag = this.m_fieldInfo.IncludeEnemies();
		bool flag2 = this.m_fieldInfo.IncludeAllies();
		List<ServerAbilityUtils.TriggeringPathInfo> list = new List<ServerAbilityUtils.TriggeringPathInfo>();
		foreach (MovementInstance movementInstance in movement.m_movementInstances)
		{
			ActorData mover = movementInstance.m_mover;
			bool flag3 = mover.GetTeam() != base.Caster.GetTeam();
			bool flag4 = !flag3;
			bool flag5 = this.m_fieldInfo.CanBeAffected(mover, base.Caster);
			bool flag6 = !this.m_affectedSquares.Contains(mover.GetCurrentBoardSquare());
			if (flag5 && flag6)
			{
				for (BoardSquarePathInfo boardSquarePathInfo = movementInstance.m_path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
				{
					BoardSquare square = boardSquarePathInfo.square;
					if ((movementInstance.m_groundBased || boardSquarePathInfo.IsPathEndpoint()) && !boardSquarePathInfo.IsPathStartPoint() && ((flag3 && flag) || (flag4 && flag2)) && this.m_affectedSquares.Contains(square) && !this.m_actorsHitThisTurn.Contains(mover))
					{
						ServerAbilityUtils.TriggeringPathInfo item = new ServerAbilityUtils.TriggeringPathInfo(mover, boardSquarePathInfo);
						list.Add(item);
						this.m_actorsHitThisTurn.Add(mover);
					}
				}
			}
		}
		foreach (ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo in list)
		{
			ActorHitResults reactionHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
			this.SetupActorHitResults(ref reactionHitResults, triggeringPathInfo.m_triggeringPathSegment.square);
			GameObject sequencePrefab;
			if (triggeringPathInfo.m_mover.GetTeam() != base.Caster.GetTeam())
			{
				sequencePrefab = this.m_fieldInfo.enemyHitSequencePrefab;
			}
			else
			{
				sequencePrefab = this.m_fieldInfo.allyHitSequencePrefab;
			}
			MovementResults movementResults = new MovementResults(movement.m_movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, reactionHitResults);
			movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, base.SequenceSource, null, true);
			movementResultsList.Add(movementResults);
		}
	}

	public override void GatherMovementResultsFromSegment(ActorData mover, MovementInstance movementInstance, MovementStage movementStage, BoardSquarePathInfo sourcePath, BoardSquarePathInfo destPath, ref List<MovementResults> movementResultsList)
	{
		if (!this.m_fieldInfo.ignoreMovementHits
			&& this.ShouldHitThisTurn()
			&& !this.m_actorsHitThisTurn.Contains(mover)
			&& !this.m_affectedSquares.Contains(sourcePath.square)
			&& this.m_affectedSquares.Contains(destPath.square)
			&& this.m_fieldInfo.CanBeAffected(mover, base.Caster)
			&& (movementInstance.m_groundBased || destPath.IsPathEndpoint()))
		{
			ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo = new ServerAbilityUtils.TriggeringPathInfo(mover, destPath);
			ActorHitResults reactionHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
			this.SetupActorHitResults(ref reactionHitResults, triggeringPathInfo.m_triggeringPathSegment.square);
			GameObject sequencePrefab;
			if (triggeringPathInfo.m_mover.GetTeam() != base.Caster.GetTeam())
			{
				sequencePrefab = this.m_fieldInfo.enemyHitSequencePrefab;
			}
			else
			{
				sequencePrefab = this.m_fieldInfo.allyHitSequencePrefab;
			}
			MovementResults movementResults = new MovementResults(movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, reactionHitResults);
			movementResults.SetupSequenceData(sequencePrefab, triggeringPathInfo.m_triggeringPathSegment.square, base.SequenceSource, null, true);
			movementResultsList.Add(movementResults);
			this.m_actorsHitThisTurn.Add(mover);
		}
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != base.Caster.GetTeam() && this.m_affectedSquares != null)
		{
			squaresToAvoid.UnionWith(this.m_affectedSquares);
		}
	}

	public override bool IsMovementBlockedOnEnterSquare(ActorData mover, BoardSquare movingFrom, BoardSquare movingTo)
	{
		if (mover.GetTeam() != base.Caster.GetTeam() && this.m_affectedSquares != null)
		{
			if (this.m_fieldInfo.stopMovementInField && this.m_affectedSquares.Contains(movingTo))
			{
				return true;
			}
			if (this.m_fieldInfo.stopMovementOutOfField && this.m_affectedSquares.Contains(movingFrom))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsActorHitThisTurn(ActorData actor, bool isReal)
	{
		bool flag = false;
		if (isReal)
		{
			flag = this.m_actorsHitThisTurn.Contains(actor);
		}
		else
		{
			flag = this.m_actorsHitThisTurn_fake.Contains(actor);
		}
		if (!flag && this.m_linkedGroundEffects != null)
		{
			foreach (StandardGroundEffect standardGroundEffect in this.m_linkedGroundEffects)
			{
				if (standardGroundEffect != this && standardGroundEffect != null)
				{
					if (isReal)
					{
						flag = standardGroundEffect.m_actorsHitThisTurn.Contains(actor);
					}
					else
					{
						flag = standardGroundEffect.m_actorsHitThisTurn_fake.Contains(actor);
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
		return flag;
	}

	private void AddActorHitThisTurn(ActorData actor, bool isReal)
	{
		if (isReal)
		{
			this.m_actorsHitThisTurn.Add(actor);
			return;
		}
		this.m_actorsHitThisTurn_fake.Add(actor);
	}
}
#endif
