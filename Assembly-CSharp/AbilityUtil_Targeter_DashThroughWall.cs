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
		this.m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(this.m_coneWidth, this.m_coneLength * Board.\u000E().squareSize, false, null));
		this.m_coneHighlightMesh = this.m_highlights[1].GetComponent<UIDynamicCone>();
		IL_C1:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		Vector3 vector = targetingActor.\u0015();
		Vector3 vector2;
		bool flag;
		List<ActorData> chargeHitActors = this.GetChargeHitActors(currentTarget.AimDirection, vector, targetingActor, out vector2, out flag);
		Vector3 vector3 = vector2 - vector;
		float magnitude = vector3.magnitude;
		vector3.Normalize();
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
			Vector3 vector4 = vector2 - vector3 * d;
			BoardSquare boardSquare2 = Board.\u000E().\u000E(vector4);
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
				Vector3 vector5;
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
					vector5 = chargeHitActors[0].\u0016();
				}
				else
				{
					vector5 = targetingActor.\u0016();
				}
				Vector3 damageOrigin = vector5;
				base.AddActorInRange(chargeHitActors[0], damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
				BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, chargeHitActors[0].\u0012(), targetingActor.\u0012(), true);
				boardSquare2 = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, chargeHitActors[0].\u0012(), pathToDesired);
			}
			else
			{
				boardSquare2 = KnockbackUtils.GetLastValidBoardSquareInLine(vector, vector4, true, false, float.MaxValue);
				BoardSquare x = Board.\u000E().\u000E(vector4);
				if (x == null)
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
				Vector3 vector6 = vector2 - vector3.normalized * this.m_coneBackwardOffset * Board.\u000E().squareSize;
				if (flag4)
				{
					vector6 = boardSquare2.ToVector3();
				}
				float num = VectorUtils.HorizontalAngle_Deg(vector3);
				List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector6, num, this.m_coneWidth, this.m_coneLength, 0f, false, targetingActor, targetingActor.\u0012(), null, false, default(Vector3));
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
				vector6.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = vector6;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector3);
				if (this.m_coneHighlightMesh != null)
				{
					this.m_coneHighlightMesh.AdjustConeMeshVertices(this.m_coneWidth, this.m_coneLength * Board.\u000E().squareSize);
				}
				boardSquare = boardSquare2;
				this.DrawInvalidSquareIndicators(targetingActor, vector6, num, this.m_coneLength, this.m_coneWidth);
			}
		}
		IL_3F1:
		BoardSquare boardSquare3 = null;
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
			float num2 = this.m_maxWallThicknessInSquares * Board.\u000E().squareSize;
			num2 = Mathf.Min(num2, (this.m_dashRangeInSquares + this.m_extraTotalDistanceIfThroughWalls) * Board.\u000E().squareSize - magnitude);
			Vector3 vector7 = vector2 + vector3 * num2;
			Vector3 vector8 = vector7;
			Vector3 vector9 = vector3;
			Vector3 vector10 = vector9;
			boardSquare3 = MantaDashThroughWall.GetSquareBeyondWall(vector, vector7, targetingActor, num2, ref vector8, ref vector10);
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
				vector9 = vector10;
			}
			if (boardSquare3 != null)
			{
				float num3 = VectorUtils.HorizontalAngle_Deg(vector9);
				List<ActorData> actorsInCone2 = AreaEffectUtils.GetActorsInCone(vector8, num3, this.m_throughWallConeWidth, this.m_throughWallConeLength, 0f, false, targetingActor, targetingActor.\u0012(), null, false, default(Vector3));
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
							base.AddActorInRange(actor2, boardSquare3.ToVector3(), targetingActor, AbilityTooltipSubject.Tertiary, false);
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
				vector8.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = vector8;
				gameObject2.transform.rotation = Quaternion.LookRotation(vector9);
				if (this.m_coneHighlightMesh != null)
				{
					this.m_coneHighlightMesh.AdjustConeMeshVertices(this.m_throughWallConeWidth, this.m_throughWallConeLength * Board.\u000E().squareSize);
				}
				this.SetHighlightColor(gameObject, this.m_throughWallsHighlightColor);
				this.DrawInvalidSquareIndicators(targetingActor, vector8, num3, this.m_throughWallConeLength, this.m_throughWallConeWidth);
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
		if (boardSquare3 == null)
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
			vector2 -= vector3 * d2;
			if (boardSquare != null)
			{
				boardSquare3 = boardSquare;
			}
			else
			{
				boardSquare3 = KnockbackUtils.GetLastValidBoardSquareInLine(vector, vector2, true, false, float.MaxValue);
				if (boardSquare3 == null)
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
					boardSquare3 = targetingActor.\u0012();
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
			boardSquare3 = chargeHitActors[0].\u0012();
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, targetingActor.\u0012(), true);
		bool flag5 = false;
		if (boardSquare3 != null)
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
			if (boardSquare3.OccupantActor != null)
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
				if (boardSquare3.OccupantActor != targetingActor)
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
					if (boardSquare3.OccupantActor.\u0018())
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
						BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare3, boardSquarePathInfo);
						if (chargeDestination != boardSquare3)
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
							boardSquare3 = chargeDestination;
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
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, targetingActor.\u0012(), true);
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
		Vector3 a = vector2;
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
			a = boardSquare3.ToVector3();
		}
		Vector3 lhs = a - vector;
		lhs.y = 0f;
		float d3 = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = vector + d3 * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, vector, endPos, this.m_dashWidthInSquares);
	}

	private List<ActorData> GetChargeHitActors(Vector3 aimDir, Vector3 startPos, ActorData caster, out Vector3 chargeEndPoint, out bool traveledFullDistance)
	{
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, aimDir, this.m_dashRangeInSquares, this.m_dashWidthInSquares, caster, caster.\u0015(), false, 1, false, false, out chargeEndPoint, null, null, true, true);
		float num = (this.m_dashRangeInSquares - 0.25f) * Board.\u000E().squareSize;
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
