using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Smite : AbilityUtil_Targeter
{
	public float m_coneAngleDegrees;

	public float m_coneLengthRadiusInSquares;

	public float m_coneBackwardOffsetInSquares;

	public bool m_conePenetrateLoS;

	public NanoSmithBoltInfo m_boltInfo;

	public float m_boltAngle = 45f;

	public int m_boltCount = 3;

	public AbilityUtil_Targeter_Smite(Ability ability, float coneAngleDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool conePenetrateLoS, NanoSmithBoltInfo boltInfo, float boltAngle, int boltCount)
		: base(ability)
	{
		m_coneAngleDegrees = coneAngleDegrees;
		m_coneLengthRadiusInSquares = coneLengthRadiusInSquares;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_conePenetrateLoS = conePenetrateLoS;
		m_boltInfo = boltInfo;
		m_boltAngle = boltAngle;
		m_boltCount = boltCount;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
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
		Vector3 vec = vector;
		float num = VectorUtils.HorizontalAngle_Deg(vec);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, num, m_coneAngleDegrees, m_coneLengthRadiusInSquares, m_coneBackwardOffsetInSquares, m_conePenetrateLoS, targetingActor, targetingActor.GetOpposingTeam(), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, travelBoardSquareWorldPositionForLos, targetingActor);
			}
		}
		Vector3 vector2 = VectorUtils.AngleDegreesToVector(num);
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = travelBoardSquareWorldPositionForLos + new Vector3(0f, y, 0f) - vector2 * d;
		if (m_highlights != null)
		{
			if (m_highlights.Count > m_boltCount)
			{
				goto IL_01d4;
			}
		}
		m_highlights = new List<GameObject>();
		float radiusInWorld = (m_coneLengthRadiusInSquares + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
		m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneAngleDegrees));
		for (int i = 0; i < m_boltCount; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		}
		goto IL_01d4;
		IL_01d4:
		m_highlights[0].transform.position = position;
		m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
		float squareSize = Board.Get().squareSize;
		float widthInWorld = m_boltInfo.width * squareSize;
		float maxDistanceInWorld = m_boltInfo.range * squareSize;
		float angle = -0.5f * (float)(m_boltCount - 1) * m_boltAngle;
		Vector3 point = Quaternion.AngleAxis(angle, Vector3.up) * vector2;
		int num2 = 0;
		while (num2 < m_boltCount)
		{
			Vector3 vector3 = Quaternion.AngleAxis((float)num2 * m_boltAngle, Vector3.up) * point;
			Vector3 vector4 = travelBoardSquareWorldPositionForLos + (m_coneLengthRadiusInSquares + m_coneBackwardOffsetInSquares) * squareSize * vector3 - m_coneBackwardOffsetInSquares * squareSize * vector2;
			VectorUtils.LaserCoords endPoints;
			List<ActorData> actors2 = m_boltInfo.GetActorsHitByBolt(vector4, vector3, targetingActor, AbilityPriority.Combat_Damage, out endPoints, null, true, false, true);
			for (int num3 = actors2.Count - 1; num3 >= 0; num3--)
			{
				ActorData item = actors2[num3];
				if (actors.Contains(item))
				{
					actors2.Remove(item);
				}
			}
			while (true)
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
				using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData current2 = enumerator2.Current;
						if (current2.GetTeam() == targetingActor.GetTeam())
						{
							AddActorInRange(current2, vector4, targetingActor, AbilityTooltipSubject.Ally);
						}
						else
						{
							AddActorInRange(current2, vector4, targetingActor, AbilityTooltipSubject.Secondary);
						}
					}
				}
				VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(vector4, vector3, maxDistanceInWorld, widthInWorld, m_boltInfo.penetrateLineOfSight, targetingActor);
				VectorUtils.LaserCoords laserCoords = laserCoordinates;
				if (actors2.Count > 0)
				{
					laserCoords = TargeterUtils.GetLaserCoordsToFarthestTarget(laserCoordinates, actors2);
				}
				float magnitude = (laserCoords.end - laserCoords.start).magnitude;
				if (magnitude > 0f)
				{
					int index = num2 + 1;
					m_highlights[index].transform.position = vector4 + new Vector3(0f, y, 0f);
					m_highlights[index].transform.rotation = Quaternion.LookRotation(vector3);
					HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, m_highlights[index]);
				}
				num2++;
				goto IL_04ad;
			}
			IL_04ad:;
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
