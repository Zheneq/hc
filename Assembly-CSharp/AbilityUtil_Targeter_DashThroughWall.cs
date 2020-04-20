using System;
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

	public AbilityUtil_Targeter_DashThroughWall(Ability ability, float dashWidthInSquares, float dashRangeInSquares, float wallThicknessInSquares, float coneWidthDegrees, float coneThroughWallsWidthDegrees, float coneLengthInSquares, float coneThroughWallsLengthInSquares, float extraTotalRangeIfThroughWalls, float coneBackwardOffset, bool directHitIgnoreCover, bool throughWallConeClampedToWall, bool aoeWithoutDirectHit) : base(ability)
	{
		this.m_dashWidthInSquares = dashWidthInSquares;
		this.m_dashRangeInSquares = dashRangeInSquares;
		this.m_maxWallThicknessInSquares = wallThicknessInSquares;
		this.m_coneWidth = coneWidthDegrees;
		this.m_throughWallConeWidth = coneThroughWallsWidthDegrees;
		this.m_coneLength = coneLengthInSquares;
		this.m_throughWallConeLength = coneThroughWallsLengthInSquares;
		this.m_directHitIgnoreCover = directHitIgnoreCover;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_extraTotalDistanceIfThroughWalls = extraTotalRangeIfThroughWalls;
		this.m_coneBackwardOffset = coneBackwardOffset;
		this.m_throughWallConeClampedToWall = throughWallConeClampedToWall;
		this.m_aoeWithMiss = aoeWithoutDirectHit;
		MantaDashThroughWall mantaDashThroughWall = ability as MantaDashThroughWall;
		if (mantaDashThroughWall != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_DashThroughWall..ctor(Ability, float, float, float, float, float, float, float, float, float, bool, bool, bool)).MethodHandle;
			}
			this.m_normalHighlightColor = mantaDashThroughWall.m_normalHighlightColor;
			this.m_throughWallsHighlightColor = mantaDashThroughWall.m_throughWallsHighlightColor;
		}
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public int LastUpdatePathSquareCount { get; set; }

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.LastUpdatePathSquareCount = 0;
		if (this.m_highlights != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_DashThroughWall.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count >= 2)
			{
				goto IL_C1;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(this.m_coneWidth, this.m_coneLength * Board.Get().squareSize, false, null));
		this.m_coneHighlightMesh = this.m_highlights[1].GetComponent<UIDynamicCone>();
		IL_C1:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		bool flag;
		List<ActorData> chargeHitActors = this.GetChargeHitActors(currentTarget.AimDirection, travelBoardSquareWorldPositionForLos, targetingActor, out vector, out flag);
		Vector3 vector2 = vector - travelBoardSquareWorldPositionForLos;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		BoardSquare boardSquare = null;
		bool flag2;
		if (chargeHitActors.Count == 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag2 = !flag;
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4 = false;
		bool active = false;
		if (!flag3)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (chargeHitActors.Count <= 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_aoeWithMiss)
				{
					goto IL_3F1;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			float d = Mathf.Min(0.5f, magnitude / 2f);
			Vector3 vector3 = vector - vector2 * d;
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(vector3);
			if (chargeHitActors.Count > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 travelBoardSquareWorldPosition;
				if (this.m_directHitIgnoreCover)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					travelBoardSquareWorldPosition = chargeHitActors[0].GetTravelBoardSquareWorldPosition();
				}
				else
				{
					travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
				}
				Vector3 damageOrigin = travelBoardSquareWorldPosition;
				base.AddActorInRange(chargeHitActors[0], damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
				BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, chargeHitActors[0].GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare(), true);
				boardSquare2 = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, chargeHitActors[0].GetCurrentBoardSquare(), pathToDesired);
			}
			else
			{
				boardSquare2 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector3, true, false, float.MaxValue);
				BoardSquare boardSquare3 = Board.Get().GetBoardSquare(vector3);
				if (boardSquare3 == null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag4 = true;
				}
			}
			if (boardSquare2 != null)
			{
				Vector3 vector4 = vector - vector2.normalized * this.m_coneBackwardOffset * Board.Get().squareSize;
				if (flag4)
				{
					vector4 = boardSquare2.ToVector3();
				}
				float num = VectorUtils.HorizontalAngle_Deg(vector2);
				List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector4, num, this.m_coneWidth, this.m_coneLength, 0f, false, targetingActor, targetingActor.GetOpposingTeam(), null, false, default(Vector3));
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
				using (List<ActorData>.Enumerator enumerator = actorsInCone.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actor = enumerator.Current;
						base.AddActorInRange(actor, boardSquare2.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary, false);
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				active = true;
				vector4.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = vector4;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector2);
				if (this.m_coneHighlightMesh != null)
				{
					this.m_coneHighlightMesh.AdjustConeMeshVertices(this.m_coneWidth, this.m_coneLength * Board.Get().squareSize);
				}
				boardSquare = boardSquare2;
				this.DrawInvalidSquareIndicators(targetingActor, vector4, num, this.m_coneLength, this.m_coneWidth);
			}
		}
		IL_3F1:
		BoardSquare boardSquare4 = null;
		if (flag3)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			float num2 = this.m_maxWallThicknessInSquares * Board.Get().squareSize;
			num2 = Mathf.Min(num2, (this.m_dashRangeInSquares + this.m_extraTotalDistanceIfThroughWalls) * Board.Get().squareSize - magnitude);
			Vector3 vector5 = vector + vector2 * num2;
			Vector3 vector6 = vector5;
			Vector3 vector7 = vector2;
			Vector3 vector8 = vector7;
			boardSquare4 = MantaDashThroughWall.GetSquareBeyondWall(travelBoardSquareWorldPositionForLos, vector5, targetingActor, num2, ref vector6, ref vector8);
			if (this.m_throughWallConeClampedToWall)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				vector7 = vector8;
			}
			if (boardSquare4 != null)
			{
				float num3 = VectorUtils.HorizontalAngle_Deg(vector7);
				List<ActorData> actorsInCone2 = AreaEffectUtils.GetActorsInCone(vector6, num3, this.m_throughWallConeWidth, this.m_throughWallConeLength, 0f, false, targetingActor, targetingActor.GetOpposingTeam(), null, false, default(Vector3));
				if (actorsInCone2 != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone2);
					using (List<ActorData>.Enumerator enumerator2 = actorsInCone2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData actor2 = enumerator2.Current;
							base.AddActorInRange(actor2, boardSquare4.ToVector3(), targetingActor, AbilityTooltipSubject.Tertiary, false);
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				active = true;
				vector6.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = vector6;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector7);
				if (this.m_coneHighlightMesh != null)
				{
					this.m_coneHighlightMesh.AdjustConeMeshVertices(this.m_throughWallConeWidth, this.m_throughWallConeLength * Board.Get().squareSize);
				}
				this.SetHighlightColor(gameObject, this.m_throughWallsHighlightColor);
				this.DrawInvalidSquareIndicators(targetingActor, vector6, num3, this.m_throughWallConeLength, this.m_throughWallConeWidth);
			}
			else
			{
				this.SetHighlightColor(gameObject, this.m_normalHighlightColor);
			}
		}
		else
		{
			this.SetHighlightColor(gameObject, this.m_normalHighlightColor);
		}
		gameObject2.SetActive(active);
		if (boardSquare4 == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			float d2 = Mathf.Min(0.5f, magnitude / 2f);
			vector -= vector2 * d2;
			if (boardSquare != null)
			{
				boardSquare4 = boardSquare;
			}
			else
			{
				boardSquare4 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector, true, false, float.MaxValue);
				if (boardSquare4 == null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					boardSquare4 = targetingActor.GetCurrentBoardSquare();
				}
			}
		}
		if (chargeHitActors.Count > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			boardSquare4 = chargeHitActors[0].GetCurrentBoardSquare();
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		bool flag5 = false;
		if (boardSquare4 != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquare4.OccupantActor != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare4.OccupantActor != targetingActor)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (boardSquare4.OccupantActor.IsVisibleToClient())
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare4, boardSquarePathInfo);
						if (chargeDestination != boardSquare4)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							boardSquare4 = chargeDestination;
							flag5 = true;
						}
					}
				}
			}
		}
		if (flag5)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		}
		int num4 = 0;
		base.EnableAllMovementArrows();
		num4 = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, num4, false);
		base.SetMovementArrowEnabledFromIndex(num4, false);
		if (boardSquarePathInfo != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd(true);
		}
		Vector3 a = vector;
		if (flag3)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquarePathInfo != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
				a = pathEndpoint.square.ToVector3();
			}
		}
		if (flag4)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			a = boardSquare4.ToVector3();
		}
		Vector3 lhs = a - travelBoardSquareWorldPositionForLos;
		lhs.y = 0f;
		float d3 = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = travelBoardSquareWorldPositionForLos + d3 * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, travelBoardSquareWorldPositionForLos, endPos, this.m_dashWidthInSquares);
	}

	private List<ActorData> GetChargeHitActors(Vector3 aimDir, Vector3 startPos, ActorData caster, out Vector3 chargeEndPoint, out bool traveledFullDistance)
	{
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, aimDir, this.m_dashRangeInSquares, this.m_dashWidthInSquares, caster, caster.GetOpposingTeams(), false, 1, false, false, out chargeEndPoint, null, null, true, true);
		float num = (this.m_dashRangeInSquares - 0.25f) * Board.Get().squareSize;
		traveledFullDistance = ((startPos - chargeEndPoint).magnitude >= num);
		return actorsInLaser;
	}

	private void SetHighlightColor(GameObject highlight, Color color)
	{
		Renderer component = highlight.GetComponent<Renderer>();
		if (component != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_DashThroughWall.SetHighlightColor(GameObject, Color)).MethodHandle;
			}
			if (component.material != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				component.material.SetColor("_TintColor", color);
			}
		}
		foreach (Renderer renderer in highlight.GetComponentsInChildren<Renderer>())
		{
			if (renderer != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (renderer.material != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					renderer.material.SetColor("_TintColor", color);
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_DashThroughWall.DrawInvalidSquareIndicators(ActorData, Vector3, float, float, float)).MethodHandle;
			}
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, coneStartPos, forwardDir_degrees, coneWidthDegrees, coneLengthSquares, 0f, targetingActor, false, null);
			base.HideUnusedSquareIndicators();
		}
	}
}
