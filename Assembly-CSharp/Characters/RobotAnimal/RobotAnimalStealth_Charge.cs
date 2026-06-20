// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalStealth_Charge : Ability
{
	[Header("-- Anim")]
	public float m_recoveryTime = 0.5f;
#if SERVER
	// added in rogues
	private BoardSquare m_desiredDestination;
#endif

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

#if SERVER
	// added in rogues
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[chargeSegments.Length - 1].m_pos;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] ProcessChargeDodge(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerEvadeUtils.ChargeInfo charge,
		List<ServerEvadeUtils.EvadeInfo> evades)
	{
		return ServerEvadeUtils.ProcessChargeDodgeForStopOnTargetHit(
			Board.Get().GetSquare(targets[0].GridPos),
			targets,
			caster,
			charge,
			evades);
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ServerEvadeUtils.ChargeSegment[] segments = ServerEvadeUtils.GetChargeSegmentForStopOnTargetHit(
			caster, 
			new List<Vector3>
			{
				square.ToVector3()
			},
			square,
			m_recoveryTime);
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(segments));
		foreach (ServerEvadeUtils.ChargeSegment segment in segments)
		{
			if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
			{
				segment.m_segmentMovementSpeed = segmentMovementSpeed;
			}
		}
		return segments;
	}

	// added in rogues
	public override void OnAbilityAnimationReleaseFocus(ActorData caster)
	{
		if (m_desiredDestination != null && m_desiredDestination.IsValidForGameplay())
		{
			caster.GetActorMovement().UpdateSquaresCanMoveTo();
			BoardSquare closestMoveableSquareTo = caster.GetActorMovement().GetClosestMoveableSquareTo(m_desiredDestination, true);
			if (closestMoveableSquareTo)
			{
				ServerActionBuffer.Get().StoreMovementRequest(closestMoveableSquareTo.x, closestMoveableSquareTo.y, caster);
				caster.GetActorBehavior().CurrentTurn.MoveDestination = closestMoveableSquareTo;
			}
		}
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (targets.Count > 1)
		{
			m_desiredDestination = Board.Get().GetSquare(targets[1].GridPos);
			return;
		}
		m_desiredDestination = null;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
	}
#endif
}
