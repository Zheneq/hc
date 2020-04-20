using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AnvilSlam : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	private float m_dashRangeInSquares;

	private int m_boltCount;

	private bool m_relativeToAim;

	private float m_boltAngleOffset;

	private NanoSmithBoltInfo m_boltInfo;

	public AbilityUtil_Targeter_AnvilSlam(Ability ability, float dashWidthInSquares, float dashRangeInSquares, int boltCount, bool relativeToAim, float boltAngleOffset, NanoSmithBoltInfo boltInfo) : base(ability)
	{
		this.m_dashWidthInSquares = dashWidthInSquares;
		this.m_dashRangeInSquares = dashRangeInSquares;
		this.m_boltCount = boltCount;
		this.m_relativeToAim = relativeToAim;
		this.m_boltAngleOffset = boltAngleOffset;
		this.m_boltInfo = boltInfo;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vector2 = vector;
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = travelBoardSquareWorldPositionForLos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, this.m_dashRangeInSquares, this.m_dashWidthInSquares, targetingActor, targetingActor.GetOpposingTeams(), false, 1, false, false, out laserCoords.end, null, null, false, true);
		Vector3 a;
		if (actorsInLaser.Count > 0)
		{
			a = actorsInLaser[0].GetTravelBoardSquareWorldPosition();
		}
		else
		{
			float d = this.m_dashRangeInSquares * Board.Get().squareSize;
			Vector3 endPos = travelBoardSquareWorldPositionForLos + vector2 * d;
			a = TargeterUtils.GetEndPointAndLimitToFurthestSquare(travelBoardSquareWorldPositionForLos, endPos, this.m_dashWidthInSquares, this.m_dashRangeInSquares, false, targetingActor, 0.71f);
		}
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				base.AddActorInRange(actor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_boltCount + 1)
			{
				goto IL_1E5;
			}
		}
		this.m_highlights = new List<GameObject>(this.m_boltCount + 1);
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		for (int i = 0; i < this.m_boltCount; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		}
		IL_1E5:
		Vector3 vector3 = a - targetingActor.GetTravelBoardSquareWorldPosition();
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		this.m_highlights[0].transform.position = targetingActor.GetTravelBoardSquareWorldPosition() + new Vector3(0f, 0.1f, 0f);
		this.m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
		HighlightUtils.Get().ResizeRectangularCursor(this.m_dashWidthInSquares * Board.Get().squareSize, magnitude, this.m_highlights[0]);
		if (actorsInLaser.Count > 0)
		{
			if (this.m_boltCount > 0)
			{
				Vector3 travelBoardSquareWorldPositionForLos2 = actorsInLaser[0].GetTravelBoardSquareWorldPositionForLos();
				float num = this.m_boltAngleOffset;
				if (this.m_relativeToAim)
				{
					num += VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
				}
				float num2 = 360f / (float)this.m_boltCount;
				float squareSize = Board.Get().squareSize;
				float maxDistanceInWorld = this.m_boltInfo.range * squareSize;
				float widthInWorld = this.m_boltInfo.width * squareSize;
				for (int j = 0; j < this.m_boltCount; j++)
				{
					Vector3 vector4 = VectorUtils.AngleDegreesToVector(num + (float)j * num2);
					VectorUtils.LaserCoords laserCoords2;
					List<ActorData> actorsHitByBolt = this.m_boltInfo.GetActorsHitByBolt(travelBoardSquareWorldPositionForLos2, vector4, targetingActor, AbilityPriority.Combat_Damage, out laserCoords2, null, false, false, false);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsHitByBolt);
					using (List<ActorData>.Enumerator enumerator2 = actorsHitByBolt.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData actorData = enumerator2.Current;
							if (actorData.GetTeam() == targetingActor.GetTeam())
							{
								base.AddActorInRange(actorData, travelBoardSquareWorldPositionForLos2, targetingActor, AbilityTooltipSubject.Ally, false);
							}
							else
							{
								base.AddActorInRange(actorData, travelBoardSquareWorldPositionForLos2, targetingActor, AbilityTooltipSubject.Secondary, false);
							}
						}
					}
					VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(travelBoardSquareWorldPositionForLos2, vector4, maxDistanceInWorld, widthInWorld, this.m_boltInfo.penetrateLineOfSight, targetingActor, null);
					VectorUtils.LaserCoords laserCoords3 = laserCoordinates;
					if (actorsHitByBolt.Count > 0)
					{
						laserCoords3 = TargeterUtils.GetLaserCoordsToFarthestTarget(laserCoordinates, actorsHitByBolt);
					}
					float magnitude2 = (laserCoords3.end - laserCoords3.start).magnitude;
					int index = j + 1;
					this.m_highlights[index].SetActive(true);
					this.m_highlights[index].transform.position = actorsInLaser[0].GetTravelBoardSquareWorldPosition() + new Vector3(0f, 0.1f, 0f);
					this.m_highlights[index].transform.rotation = Quaternion.LookRotation(vector4);
					HighlightUtils.Get().ResizeRectangularCursor(this.m_boltInfo.width * squareSize, magnitude2, this.m_highlights[index]);
				}
				return;
			}
		}
		for (int k = 0; k < this.m_boltCount; k++)
		{
			int index2 = k + 1;
			this.m_highlights[index2].SetActive(false);
		}
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float maxDistanceInWorld = this.m_dashRangeInSquares * Board.Get().squareSize;
		float num = this.m_dashWidthInSquares * Board.Get().squareSize;
		VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(targetingActor.GetTravelBoardSquareWorldPositionForLos(), currentTarget.AimDirection, maxDistanceInWorld, num, false, targetingActor, null);
		float num2 = 0.1f;
		Vector3 vector = laserCoordinates.start + new Vector3(0f, num2, 0f);
		Vector3 vector2 = laserCoordinates.end + new Vector3(0f, num2, 0f);
		Vector3 vector3 = vector2 - vector;
		float magnitude = vector3.magnitude;
		vector3.Normalize();
		Vector3 a = Vector3.Cross(vector3, Vector3.up);
		Vector3 a2 = (vector + vector2) * 0.5f;
		a2.y = (float)Board.Get().BaselineHeight + num2;
		Vector3 b = a * (num / 2f);
		Vector3 b2 = vector3 * (magnitude / 2f);
		Vector3 vector4 = a2 - b - b2;
		Vector3 vector5 = a2 - b + b2;
		Vector3 vector6 = a2 + b - b2;
		Vector3 vector7 = a2 + b + b2;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector5, vector7);
		Gizmos.DrawLine(vector7, vector6);
		Gizmos.DrawLine(vector6, vector4);
		Gizmos.color = Color.yellow;
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoordinates.start, laserCoordinates.end, this.m_dashWidthInSquares / 2f, false, targetingActor);
		foreach (BoardSquare boardSquare in squaresInBox)
		{
			if (boardSquare.IsBaselineHeight())
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere(boardSquare.ToVector3(), 0.5f);
		}
	}
}
