// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class ExoTetherEffect : StandardActorEffect
{
	private float m_tetherDistance;
	private bool m_abilityQueued;
	private bool m_shouldEnd;
	private GameObject m_tetherBreakHitSequence;
	private StandardEffectInfo m_tetherBreakEffect;
	public int m_baseBreakDamage; // private in rogues
	private float m_extraBreakDamagePerMoveDist;
	private int m_maxExtraBreakDamage;
	private bool m_breakTetherOnNonGroundMovment;
	private bool m_moveLeftGround;
	private bool m_hittingTarget;

	public ExoTetherEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData standardActorEffectData,
		float tetherDistance,
		int baseBreakDamage,
		float extraBreakDamagePerMoveDist,
		int maxExtraBreakDamage,
		StandardEffectInfo tetherBreakEffect,
		bool breakTetherOnNonGroundBasedMovement,
		GameObject tetherBreakHitSequencePrefab)
		: base(parent, targetSquare, target, caster, standardActorEffectData)
	{
		m_shouldEnd = false;
		m_tetherDistance = tetherDistance;
		m_baseBreakDamage = baseBreakDamage;
		m_extraBreakDamagePerMoveDist = extraBreakDamagePerMoveDist;
		m_maxExtraBreakDamage = maxExtraBreakDamage;
		m_tetherBreakEffect = tetherBreakEffect;
		m_breakTetherOnNonGroundMovment = breakTetherOnNonGroundBasedMovement;
		m_tetherBreakHitSequence = tetherBreakHitSequencePrefab;
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_hittingTarget)
		{
			return;
		}
		if (m_baseBreakDamage > 0 || m_tetherBreakEffect != null)
		{
			foreach (MovementInstance movementInstance in movement.m_movementInstances)
			{
				if (movementInstance.m_mover == Target)
				{
					m_moveLeftGround = !movementInstance.m_groundBased;
					BoardSquarePathInfo boardSquarePathInfo = movementInstance.m_path;
					while (boardSquarePathInfo != null && !m_hittingTarget)
					{
						SetUpMovementHit(boardSquarePathInfo, movement.m_movementStage, ref movementResultsList);
						boardSquarePathInfo = boardSquarePathInfo.next;
					}
				}
			}
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
		if (m_hittingTarget
		    || mover != Target
		    || m_baseBreakDamage <= 0 && m_tetherBreakEffect == null)
		{
			return;
		}
		m_moveLeftGround = !movementInstance.m_groundBased;
		SetUpMovementHit(destPath, movementStage, ref movementResultsList);
	}

	private void SetUpMovementHit(BoardSquarePathInfo destPathNode, MovementStage movementStage, ref List<MovementResults> movementResultsList)
	{
		if (!(destPathNode.square.HorizontalDistanceInSquaresTo(TargetSquare) > m_tetherDistance)
		    && (!m_breakTetherOnNonGroundMovment || !m_moveLeftGround))
		{
			return;
		}
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, TargetSquare.ToVector3()));
		int num = m_baseBreakDamage;
		if (m_extraBreakDamagePerMoveDist > 0f)
		{
			float num2 = destPathNode.GetPathEndpoint().square.HorizontalDistanceInSquaresTo(TargetSquare);
			int num3 = Mathf.RoundToInt(m_extraBreakDamagePerMoveDist * num2);
			if (m_maxExtraBreakDamage > 0)
			{
				num3 = Mathf.Min(m_maxExtraBreakDamage, num3);
			}
			num += num3;
		}
		if (num > 0)
		{
			actorHitResults.AddBaseDamage(num);
		}
		if (m_tetherBreakEffect != null)
		{
			actorHitResults.AddStandardEffectInfo(m_tetherBreakEffect);
			foreach (GameObject sequencePrefab in m_data.m_sequencePrefabs)
			{
				actorHitResults.AddEffectSequenceToEnd(sequencePrefab, m_guid);
			}
		}
		movementResultsList.Add(new MovementResults(
			movementStage,
			this,
			actorHitResults,
			Target,
			destPathNode,
			m_tetherBreakHitSequence,
			TargetSquare,
			SequenceSource));
		m_hittingTarget = true;
	}

	// TODO EXO unused
	public float GetTotalDistanceFromTarget()
	{
		return Target.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(TargetSquare);
	}

	public override void OnStart()
	{
		base.OnStart();
		if (Target != null)
		{
			Caster.GetComponent<SparkBeamTrackerComponent>().AddBeamActorByIndex(Target.ActorIndex);
		}
	}

	public void SetAbilityQueued(bool queued)
	{
		m_abilityQueued = queued;
	}

	// TODO EXO unused
	public bool IsSkippingGatheringResults()
	{
		return m_abilityQueued;
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (Target != null)
		{
			Caster.GetComponent<SparkBeamTrackerComponent>().RemoveBeamActorByIndex(Target.ActorIndex);
		}
		if (!m_hittingTarget)
		{
			PassiveData component = Caster.GetComponent<PassiveData>();
			if (component != null)
			{
				Passive_Exo passive_Exo = component.GetPassiveOfType(typeof(Passive_Exo)) as Passive_Exo;
				if (passive_Exo != null)
				{
					passive_Exo.m_tetherEndedWithoutBreaking = true;
				}
			}
		}
	}

	private void CheckTetherDistance()
	{
		if (Target.IsDead() || Caster.IsDead() || m_hittingTarget)
		{
			m_shouldEnd = true;
		}
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		CheckTetherDistance();
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		CheckTetherDistance();
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_shouldEnd;
	}
}
#endif
