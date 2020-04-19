using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_NekoDiscsFan : AbilityUtil_Targeter_ThiefFanLaser
{
	private float m_aoeRadiusAtEnd;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private bool m_showEndSquareHighlights = true;

	public AbilityUtil_Targeter_NekoDiscsFan(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, float rangeInSquares, float widthInSquares, float aoeRadiusAtEnd, int maxTargets, int count, bool penetrateLos, float interpStep, float startAngleOffset) : base(ability, minAngle, maxAngle, angleInterpMinDistance, angleInterpMaxDistance, rangeInSquares, widthInSquares, maxTargets, count, penetrateLos, false, false, false, false, 0, interpStep, startAngleOffset)
	{
		this.m_aoeRadiusAtEnd = aoeRadiusAtEnd;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < this.m_count; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(widthInSquares));
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_NekoDiscsFan..ctor(Ability, float, float, float, float, float, float, float, int, int, bool, float, float)).MethodHandle;
		}
		for (int j = 0; j < this.m_count; j++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
	}

	public void SetShowEndSquareHighlight(bool show)
	{
		this.m_showEndSquareHighlights = show;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_NekoDiscsFan.StartConfirmedTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count >= 3 * this.m_count + 1)
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
				this.m_highlights[3 * this.m_count].SetActive(false);
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 vector = targetingActor.\u0015();
		List<BoardSquare> discSquaresFromEndPositions = NekoFanOfDiscs.GetDiscSquaresFromEndPositions(this.m_laserEndPoints, vector);
		for (int i = 0; i < this.m_count; i++)
		{
			if (this.m_highlights.Count <= this.m_count + 2 * i)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_NekoDiscsFan.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
				}
				this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
				this.m_highlights.Add(HighlightUtils.Get().CreateAoECursor(this.m_aoeRadiusAtEnd * Board.SquareSizeStatic, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			GameObject gameObject = this.m_highlights[2 * i + this.m_count];
			GameObject gameObject2 = this.m_highlights[2 * i + 1 + this.m_count];
			Vector3 vector2 = new Vector3(this.m_laserEndPoints[i].x, HighlightUtils.GetHighlightHeight(), this.m_laserEndPoints[i].z);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(vector, this.m_laserEndPoints[i]);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(vector2, this.m_aoeRadiusAtEnd, false, targetingActor, targetingActor.\u0012(), null, true, coneLosCheckPos);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
			base.AddActorsInRange(actorsInRadius, vector, targetingActor, AbilityTooltipSubject.Primary, false);
			int u001D = ContextKeys.\u001A.\u0012();
			for (int j = 0; j < actorsInRadius.Count; j++)
			{
				ActorData key = actorsInRadius[j];
				ActorHitContext actorHitContext = this.m_actorContextVars[key];
				actorHitContext.\u001D = vector;
				actorHitContext.\u0015.\u0016(u001D, 1);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquare boardSquare = discSquaresFromEndPositions[i];
			Vector3 position = boardSquare.\u0012();
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = position;
			gameObject2.transform.position = vector2;
			if (gameObject.activeSelf != this.m_showEndSquareHighlights)
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
				gameObject.SetActive(this.m_showEndSquareHighlights);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (this.m_interpStep > 0f)
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
			if (this.m_highlights.Count < 3 * this.m_count + 1)
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
				float z = (this.m_interpStep + this.m_interpMinDistanceInSquares) * Board.SquareSizeStatic;
				float num = 1.2f;
				GameObject gameObject3 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(num, 0.5f, false, Color.cyan);
				gameObject3.transform.localPosition = new Vector3(-0.5f * Board.\u000E().squareSize * num, 0f, z);
				gameObject3.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
				GameObject gameObject4 = new GameObject();
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.transform.localRotation = Quaternion.identity;
				gameObject3.transform.parent = gameObject4.transform;
				this.m_highlights.Add(gameObject4);
			}
			GameObject gameObject5 = this.m_highlights[this.m_highlights.Count - 1];
			Vector3 position2 = vector;
			position2.y = HighlightUtils.GetHighlightHeight();
			gameObject5.transform.position = position2;
			gameObject5.transform.rotation = Quaternion.LookRotation(currentTarget.AimDirection);
		}
	}

	protected override void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		for (int i = 0; i < this.m_count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[index] as SquareInsideChecker_Box;
			SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[index + this.m_count] as SquareInsideChecker_Cone;
			squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
			squareInsideChecker_Cone.UpdateConeProperties(endPos, 360f, this.m_aoeRadiusAtEnd, 0f, 0f, targetingActor);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
			squareInsideChecker_Cone.SetLosPosOverride(true, coneLosCheckPos, true);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_NekoDiscsFan.UpdateLaserEndPointsForHiddenSquares(Vector3, Vector3, int, ActorData)).MethodHandle;
		}
	}

	protected override void HandleShowHiddenSquares(ActorData targetingActor)
	{
		for (int i = 0; i < this.m_count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), this.m_widthInSquares, targetingActor, this.m_penetrateLos, null, this.m_squarePosCheckerList, true);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, squareInsideChecker_Box.GetEndPos(), 0f, 360f, this.m_aoeRadiusAtEnd, 0f, targetingActor, this.m_penetrateLos, this.m_squarePosCheckerList);
		}
	}
}
