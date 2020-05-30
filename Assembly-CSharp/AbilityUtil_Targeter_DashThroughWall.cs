using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_DashThroughWall : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private float m_maxWallThicknessInSquares;

	public float m_extraTotalDistanceIfThroughWalls = 1.5f;

	private float m_coneWidth;

	private float m_coneLength;

	private float m_throughWallConeWidth;

	private float m_throughWallConeLength;

	private bool m_directHitIgnoreCover;

	private Color m_normalHighlightColor = Color.green;

	private Color m_throughWallsHighlightColor = Color.yellow;

	private bool m_throughWallConeClampedToWall;

	private bool m_aoeWithMiss;

	private float m_coneBackwardOffset;

	private UIDynamicCone m_coneHighlightMesh;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public int LastUpdatePathSquareCount
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_DashThroughWall(Ability ability, float dashWidthInSquares, float dashRangeInSquares, float wallThicknessInSquares, float coneWidthDegrees, float coneThroughWallsWidthDegrees, float coneLengthInSquares, float coneThroughWallsLengthInSquares, float extraTotalRangeIfThroughWalls, float coneBackwardOffset, bool directHitIgnoreCover, bool throughWallConeClampedToWall, bool aoeWithoutDirectHit)
		: base(ability)
	{
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_maxWallThicknessInSquares = wallThicknessInSquares;
		m_coneWidth = coneWidthDegrees;
		m_throughWallConeWidth = coneThroughWallsWidthDegrees;
		m_coneLength = coneLengthInSquares;
		m_throughWallConeLength = coneThroughWallsLengthInSquares;
		m_directHitIgnoreCover = directHitIgnoreCover;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_extraTotalDistanceIfThroughWalls = extraTotalRangeIfThroughWalls;
		m_coneBackwardOffset = coneBackwardOffset;
		m_throughWallConeClampedToWall = throughWallConeClampedToWall;
		m_aoeWithMiss = aoeWithoutDirectHit;
		MantaDashThroughWall mantaDashThroughWall = ability as MantaDashThroughWall;
		if (mantaDashThroughWall != null)
		{
			m_normalHighlightColor = mantaDashThroughWall.m_normalHighlightColor;
			m_throughWallsHighlightColor = mantaDashThroughWall.m_throughWallsHighlightColor;
		}
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		LastUpdatePathSquareCount = 0;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_00c1;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(m_coneWidth, m_coneLength * Board.Get().squareSize, false));
		m_coneHighlightMesh = m_highlights[1].GetComponent<UIDynamicCone>();
		goto IL_00c1;
		IL_00c1:
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 chargeEndPoint;
		bool traveledFullDistance;
		List<ActorData> chargeHitActors = GetChargeHitActors(currentTarget.AimDirection, travelBoardSquareWorldPositionForLos, targetingActor, out chargeEndPoint, out traveledFullDistance);
		Vector3 vector = chargeEndPoint - travelBoardSquareWorldPositionForLos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		BoardSquare boardSquare = null;
		int num;
		if (chargeHitActors.Count == 0)
		{
			num = ((!traveledFullDistance) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool flag2 = false;
		bool active = false;
		if (!flag)
		{
			if (chargeHitActors.Count <= 0)
			{
				if (!m_aoeWithMiss)
				{
					goto IL_03f1;
				}
			}
			float d = Mathf.Min(0.5f, magnitude / 2f);
			Vector3 vector2 = chargeEndPoint - vector * d;
			BoardSquare boardSquare2 = Board.Get().GetSquare(vector2);
			if (chargeHitActors.Count > 0)
			{
				Vector3 travelBoardSquareWorldPosition;
				if (m_directHitIgnoreCover)
				{
					travelBoardSquareWorldPosition = chargeHitActors[0].GetTravelBoardSquareWorldPosition();
				}
				else
				{
					travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
				}
				Vector3 damageOrigin = travelBoardSquareWorldPosition;
				AddActorInRange(chargeHitActors[0], damageOrigin, targetingActor);
				BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, chargeHitActors[0].GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare(), true);
				boardSquare2 = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, chargeHitActors[0].GetCurrentBoardSquare(), pathToDesired);
			}
			else
			{
				boardSquare2 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector2, true);
				BoardSquare boardSquare3 = Board.Get().GetSquare(vector2);
				if (boardSquare3 == null)
				{
					flag2 = true;
				}
			}
			if (boardSquare2 != null)
			{
				Vector3 vector3 = chargeEndPoint - vector.normalized * m_coneBackwardOffset * Board.Get().squareSize;
				if (flag2)
				{
					vector3 = boardSquare2.ToVector3();
				}
				float num2 = VectorUtils.HorizontalAngle_Deg(vector);
				List<ActorData> actors = AreaEffectUtils.GetActorsInCone(vector3, num2, m_coneWidth, m_coneLength, 0f, false, targetingActor, targetingActor.GetOpposingTeam(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						AddActorInRange(current, boardSquare2.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary);
					}
				}
				active = true;
				vector3.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = vector3;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector);
				if (m_coneHighlightMesh != null)
				{
					m_coneHighlightMesh.AdjustConeMeshVertices(m_coneWidth, m_coneLength * Board.Get().squareSize);
				}
				boardSquare = boardSquare2;
				DrawInvalidSquareIndicators(targetingActor, vector3, num2, m_coneLength, m_coneWidth);
			}
		}
		goto IL_03f1;
		IL_03f1:
		BoardSquare boardSquare4 = null;
		if (flag)
		{
			float a = m_maxWallThicknessInSquares * Board.Get().squareSize;
			a = Mathf.Min(a, (m_dashRangeInSquares + m_extraTotalDistanceIfThroughWalls) * Board.Get().squareSize - magnitude);
			Vector3 vector4 = chargeEndPoint + vector * a;
			Vector3 coneStartPos = vector4;
			Vector3 vector5 = vector;
			Vector3 perpendicularFromWall = vector5;
			boardSquare4 = MantaDashThroughWall.GetSquareBeyondWall(travelBoardSquareWorldPositionForLos, vector4, targetingActor, a, ref coneStartPos, ref perpendicularFromWall);
			if (m_throughWallConeClampedToWall)
			{
				vector5 = perpendicularFromWall;
			}
			if (boardSquare4 != null)
			{
				float num3 = VectorUtils.HorizontalAngle_Deg(vector5);
				List<ActorData> actors2 = AreaEffectUtils.GetActorsInCone(coneStartPos, num3, m_throughWallConeWidth, m_throughWallConeLength, 0f, false, targetingActor, targetingActor.GetOpposingTeam(), null);
				if (actors2 != null)
				{
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
					using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData current2 = enumerator2.Current;
							AddActorInRange(current2, boardSquare4.ToVector3(), targetingActor, AbilityTooltipSubject.Tertiary);
						}
					}
				}
				active = true;
				coneStartPos.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = coneStartPos;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector5);
				if (m_coneHighlightMesh != null)
				{
					m_coneHighlightMesh.AdjustConeMeshVertices(m_throughWallConeWidth, m_throughWallConeLength * Board.Get().squareSize);
				}
				SetHighlightColor(gameObject, m_throughWallsHighlightColor);
				DrawInvalidSquareIndicators(targetingActor, coneStartPos, num3, m_throughWallConeLength, m_throughWallConeWidth);
			}
			else
			{
				SetHighlightColor(gameObject, m_normalHighlightColor);
			}
		}
		else
		{
			SetHighlightColor(gameObject, m_normalHighlightColor);
		}
		gameObject2.SetActive(active);
		if (boardSquare4 == null)
		{
			float d2 = Mathf.Min(0.5f, magnitude / 2f);
			chargeEndPoint -= vector * d2;
			if (boardSquare != null)
			{
				boardSquare4 = boardSquare;
			}
			else
			{
				boardSquare4 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, chargeEndPoint, true);
				if (boardSquare4 == null)
				{
					boardSquare4 = targetingActor.GetCurrentBoardSquare();
				}
			}
		}
		if (chargeHitActors.Count > 0)
		{
			boardSquare4 = chargeHitActors[0].GetCurrentBoardSquare();
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		bool flag3 = false;
		if (boardSquare4 != null)
		{
			if (boardSquare4.OccupantActor != null)
			{
				if (boardSquare4.OccupantActor != targetingActor)
				{
					if (boardSquare4.OccupantActor.IsVisibleToClient())
					{
						BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare4, boardSquarePathInfo);
						if (chargeDestination != boardSquare4)
						{
							boardSquare4 = chargeDestination;
							flag3 = true;
						}
					}
				}
			}
		}
		if (flag3)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		arrowIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, arrowIndex);
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		if (boardSquarePathInfo != null)
		{
			LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd();
		}
		Vector3 a2 = chargeEndPoint;
		if (flag)
		{
			if (boardSquarePathInfo != null)
			{
				BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
				a2 = pathEndpoint.square.ToVector3();
			}
		}
		if (flag2)
		{
			a2 = boardSquare4.ToVector3();
		}
		Vector3 lhs = a2 - travelBoardSquareWorldPositionForLos;
		lhs.y = 0f;
		float d3 = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = travelBoardSquareWorldPositionForLos + d3 * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, travelBoardSquareWorldPositionForLos, endPos, m_dashWidthInSquares);
	}

	private List<ActorData> GetChargeHitActors(Vector3 aimDir, Vector3 startPos, ActorData caster, out Vector3 chargeEndPoint, out bool traveledFullDistance)
	{
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, aimDir, m_dashRangeInSquares, m_dashWidthInSquares, caster, caster.GetOpposingTeams(), false, 1, false, false, out chargeEndPoint, null, null, true);
		float num = (m_dashRangeInSquares - 0.25f) * Board.Get().squareSize;
		traveledFullDistance = ((startPos - chargeEndPoint).magnitude >= num);
		return actorsInLaser;
	}

	private void SetHighlightColor(GameObject highlight, Color color)
	{
		Renderer component = highlight.GetComponent<Renderer>();
		if (component != null)
		{
			if (component.material != null)
			{
				component.material.SetColor("_TintColor", color);
			}
		}
		Renderer[] componentsInChildren = highlight.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer != null))
			{
				continue;
			}
			if (renderer.material != null)
			{
				renderer.material.SetColor("_TintColor", color);
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStartPos, forwardDir_degrees, coneWidthDegrees, coneLengthSquares, 0f, targetingActor, false);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
