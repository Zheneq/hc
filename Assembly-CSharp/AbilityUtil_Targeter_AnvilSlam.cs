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

	public AbilityUtil_Targeter_AnvilSlam(Ability ability, float dashWidthInSquares, float dashRangeInSquares, int boltCount, bool relativeToAim, float boltAngleOffset, NanoSmithBoltInfo boltInfo)
		: base(ability)
	{
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_boltCount = boltCount;
		m_relativeToAim = relativeToAim;
		m_boltAngleOffset = boltAngleOffset;
		m_boltInfo = boltInfo;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
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
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = travelBoardSquareWorldPositionForLos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, m_dashRangeInSquares, m_dashWidthInSquares, targetingActor, targetingActor.GetEnemyTeams(), false, 1, false, false, out laserCoords.end, null);
		Vector3 a;
		if (actorsInLaser.Count > 0)
		{
			a = actorsInLaser[0].GetTravelBoardSquareWorldPosition();
		}
		else
		{
			float d = m_dashRangeInSquares * Board.Get().squareSize;
			Vector3 endPos = travelBoardSquareWorldPositionForLos + vector2 * d;
			a = TargeterUtils.GetEndPointAndLimitToFurthestSquare(travelBoardSquareWorldPositionForLos, endPos, m_dashWidthInSquares, m_dashRangeInSquares, false, targetingActor);
		}
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor);
			}
		}
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_boltCount + 1)
			{
				goto IL_01e5;
			}
		}
		m_highlights = new List<GameObject>(m_boltCount + 1);
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		for (int i = 0; i < m_boltCount; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		}
		goto IL_01e5;
		IL_01e5:
		Vector3 vector3 = a - targetingActor.GetTravelBoardSquareWorldPosition();
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		m_highlights[0].transform.position = targetingActor.GetTravelBoardSquareWorldPosition() + new Vector3(0f, 0.1f, 0f);
		m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
		HighlightUtils.Get().ResizeRectangularCursor(m_dashWidthInSquares * Board.Get().squareSize, magnitude, m_highlights[0]);
		if (actorsInLaser.Count > 0)
		{
			if (m_boltCount > 0)
			{
				Vector3 travelBoardSquareWorldPositionForLos2 = actorsInLaser[0].GetTravelBoardSquareWorldPositionForLos();
				float num = m_boltAngleOffset;
				if (m_relativeToAim)
				{
					num += VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
				}
				float num2 = 360f / (float)m_boltCount;
				float squareSize = Board.Get().squareSize;
				float maxDistanceInWorld = m_boltInfo.range * squareSize;
				float widthInWorld = m_boltInfo.width * squareSize;
				for (int j = 0; j < m_boltCount; j++)
				{
					Vector3 vector4 = VectorUtils.AngleDegreesToVector(num + (float)j * num2);
					VectorUtils.LaserCoords endPoints;
					List<ActorData> actors = m_boltInfo.GetActorsHitByBolt(travelBoardSquareWorldPositionForLos2, vector4, targetingActor, AbilityPriority.Combat_Damage, out endPoints, null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData current2 = enumerator2.Current;
							if (current2.GetTeam() == targetingActor.GetTeam())
							{
								AddActorInRange(current2, travelBoardSquareWorldPositionForLos2, targetingActor, AbilityTooltipSubject.Ally);
							}
							else
							{
								AddActorInRange(current2, travelBoardSquareWorldPositionForLos2, targetingActor, AbilityTooltipSubject.Secondary);
							}
						}
					}
					VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(travelBoardSquareWorldPositionForLos2, vector4, maxDistanceInWorld, widthInWorld, m_boltInfo.penetrateLineOfSight, targetingActor);
					VectorUtils.LaserCoords laserCoords2 = laserCoordinates;
					if (actors.Count > 0)
					{
						laserCoords2 = TargeterUtils.GetLaserCoordsToFarthestTarget(laserCoordinates, actors);
					}
					float magnitude2 = (laserCoords2.end - laserCoords2.start).magnitude;
					int index = j + 1;
					m_highlights[index].SetActive(true);
					m_highlights[index].transform.position = actorsInLaser[0].GetTravelBoardSquareWorldPosition() + new Vector3(0f, 0.1f, 0f);
					m_highlights[index].transform.rotation = Quaternion.LookRotation(vector4);
					HighlightUtils.Get().ResizeRectangularCursor(m_boltInfo.width * squareSize, magnitude2, m_highlights[index]);
				}
				return;
			}
		}
		for (int k = 0; k < m_boltCount; k++)
		{
			int index2 = k + 1;
			m_highlights[index2].SetActive(false);
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float maxDistanceInWorld = m_dashRangeInSquares * Board.Get().squareSize;
		float num = m_dashWidthInSquares * Board.Get().squareSize;
		VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(targetingActor.GetTravelBoardSquareWorldPositionForLos(), currentTarget.AimDirection, maxDistanceInWorld, num, false, targetingActor);
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
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoordinates.start, laserCoordinates.end, m_dashWidthInSquares / 2f, false, targetingActor);
		foreach (BoardSquare item in squaresInBox)
		{
			if (item.IsBaselineHeight())
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere(item.ToVector3(), 0.5f);
		}
	}
}
