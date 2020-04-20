using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_FanReverseCones : TargetSelect_FanCones
{
	public float m_perConeHorizontalOffset;

	protected override bool UseCasterPosForLoS()
	{
		return true;
	}

	public override List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		List<Vector3> coneDirections = this.GetConeDirections(currentTarget, targeterFreePos, caster);
		Vector3 vector = targeterFreePos - travelBoardSquareWorldPositionForLos;
		vector.Normalize();
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		float d = this.m_coneStartOffsetInConeDir * Board.SquareSizeStatic;
		Vector3 a = travelBoardSquareWorldPositionForLos + d * vector;
		for (int i = 0; i < coneDirections.Count; i++)
		{
			float d2 = this.m_perConeHorizontalOffset * (float)(i - coneDirections.Count / 2);
			Vector3 vector2 = a + normalized * d2;
			vector2 -= this.m_coneInfo.m_radiusInSquares * Board.SquareSizeStatic * coneDirections[i];
			list.Add(vector2);
		}
		return list;
	}

	public override List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> coneDirections = base.GetConeDirections(currentTarget, targeterFreePos, caster);
		for (int i = 0; i < this.m_coneCount; i++)
		{
			coneDirections[i] = -coneDirections[i];
		}
		return coneDirections;
	}

	protected override bool CustomLoS(ActorData actor, ActorData caster)
	{
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		return caster.GetCurrentBoardSquare().symbol_0013(currentBoardSquare.x, currentBoardSquare.y);
	}
}
