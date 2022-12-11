// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithChargeAndCreateBarrier : Ability
{
	[Header("-- Barrier ")]
	public bool m_snapToGrid;
	public StandardBarrierData m_barrierData;
	[Header("-- Sequences ------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Charge and Create Barrier";
		}
		// added in rogues
		// if (m_barrierData == null)
		// {
		// 	m_barrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
		// }
		Targeter = new AbilityUtil_Targeter_BarrierWithCharge(this, m_barrierData.m_width, m_snapToGrid);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomTargetValidation(
		ActorData caster,
		AbilityTarget target,
		int targetIndex,
		List<AbilityTarget> currentTargets)
	{
		BoardSquare square = Board.Get().GetSquare(target.GridPos);
		return KnockbackUtils.BuildStraightLineChargePath(caster, square) != null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

#if SERVER
	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[2];
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.Pivot
		};
		array[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_pos = Board.Get().GetSquare(targets[0].GridPos),
			m_end = BoardSquarePathInfo.ChargeEndType.Miss
		};
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
		array[0].m_segmentMovementSpeed = segmentMovementSpeed;
		array[1].m_segmentMovementSpeed = segmentMovementSpeed;
		return array;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			new ActorData[0],
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(targets[0].FreePos));
		GetBarrierPositionAndFacing(targets, out Vector3 center, out Vector3 facingDir);
		Barrier barrier = new Barrier(m_abilityName, center, facingDir, caster, m_barrierData);
		barrier.SetSourceAbility(this);
		positionHitResults.AddBarrier(barrier);
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	private void GetBarrierPositionAndFacing(List<AbilityTarget> targets, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (m_snapToGrid)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				facing = VectorUtils.GetDirectionToClosestSide(square, targets[0].FreePos);
				position = square.ToVector3() + 0.5f * Board.Get().squareSize * facing;
			}
		}
	}
#endif
}
