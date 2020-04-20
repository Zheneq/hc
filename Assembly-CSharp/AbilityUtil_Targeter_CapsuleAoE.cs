using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_CapsuleAoE : AbilityUtil_Targeter
{
	private float m_radiusAroundStart = 2f;

	private float m_radiusAroundEnd = 2f;

	private float m_rangeFromLine = 2f;

	private bool m_penetrateLoS;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public AbilityUtil_Targeter_CapsuleAoE.StartSquareDelegate GetDefaultStartSquare;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_CapsuleAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS) : base(ability)
	{
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_radiusAroundEnd = radiusAroundEnd;
		this.m_rangeFromLine = rangeFromDir;
		this.m_penetrateLoS = penetrateLoS;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		BoardSquare boardSquare;
		if (currentTargetIndex > 0)
		{
			boardSquare = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
		}
		else
		{
			boardSquare = boardSquareSafe;
		}
		BoardSquare boardSquare2 = boardSquare;
		if (this.m_ability.GetExpectedNumberOfTargeters() == 1)
		{
			boardSquare2 = targetingActor.GetCurrentBoardSquare();
		}
		else if (this.m_ability.GetExpectedNumberOfTargeters() == 0)
		{
			if (this.GetDefaultStartSquare != null)
			{
				boardSquare2 = this.GetDefaultStartSquare();
			}
		}
		Vector3 vector = boardSquare2.ToVector3();
		Vector3 vector2 = boardSquareSafe.ToVector3();
		bool flag = this.m_rangeFromLine > 0f;
		bool flag2 = this.m_radiusAroundStart > 0f;
		bool flag3 = this.m_radiusAroundEnd > 0f;
		if (flag)
		{
			float widthInSquares = this.m_rangeFromLine * 2f;
			if (this.m_highlights.Count == 0)
			{
				GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
				this.m_highlights.Add(item);
			}
			if (vector == vector2)
			{
				if (this.m_highlights.Count > 0)
				{
					if (this.m_highlights[0] != null)
					{
						this.m_highlights[0].SetActive(false);
					}
				}
			}
			else
			{
				if (this.m_highlights.Count > 0 && this.m_highlights[0] != null)
				{
					this.m_highlights[0].SetActive(true);
				}
				TargeterUtils.RefreshLaserBoxHighlight(this.m_highlights[0], vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
			}
		}
		if (flag2)
		{
			if (this.m_highlights.Count != 1)
			{
				if (this.m_highlights.Count != 0 || flag)
				{
					goto IL_25E;
				}
			}
			GameObject item2 = TargeterUtils.CreateCircleHighlight(vector, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
			this.m_highlights.Add(item2);
			IL_25E:
			int index = (!flag) ? 0 : 1;
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[index], vector, TargeterUtils.HeightAdjustType.FromPathArrow);
		}
		if (flag3)
		{
			if (this.m_highlights.Count == 2)
			{
				if (flag2)
				{
					if (flag)
					{
						goto IL_31B;
					}
				}
			}
			if (this.m_highlights.Count == 1)
			{
				if (flag2 ^ flag)
				{
					goto IL_31B;
				}
			}
			if (this.m_highlights.Count != 0)
			{
				goto IL_34F;
			}
			if (flag2)
			{
				goto IL_34F;
			}
			if (flag)
			{
				goto IL_34F;
			}
			IL_31B:
			GameObject item3 = TargeterUtils.CreateCircleHighlight(vector2, this.m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
			this.m_highlights.Add(item3);
			IL_34F:
			int num = 0;
			if (flag)
			{
				num++;
			}
			if (flag2)
			{
				num++;
			}
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[num], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
		}
		List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector, vector2, this.m_radiusAroundStart, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor, null, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadiusOfLine, targetingActor.GetTravelBoardSquareWorldPosition());
		foreach (ActorData actorData in actorsInRadiusOfLine)
		{
			if (base.GetAffectsTarget(actorData, targetingActor))
			{
				Vector3 damageOrigin = vector;
				if (this.UseEndPosAsDamageOriginIfOverlap)
				{
					Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
					travelBoardSquareWorldPosition.y = vector2.y;
					if ((travelBoardSquareWorldPosition - vector2).sqrMagnitude <= Mathf.Epsilon)
					{
						damageOrigin = vector2;
					}
				}
				base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
				if (this.m_radiusAroundStart > 0f)
				{
					float num2 = VectorUtils.HorizontalPlaneDistInSquares(vector, actorData.GetTravelBoardSquareWorldPosition());
					if (num2 <= this.m_radiusAroundStart * Board.Get().squareSize)
					{
						base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Near, true);
					}
				}
				if (this.m_radiusAroundEnd > 0f)
				{
					float num3 = VectorUtils.HorizontalPlaneDistInSquares(vector2, actorData.GetTravelBoardSquareWorldPosition());
					if (num3 <= this.m_radiusAroundEnd * Board.Get().squareSize)
					{
						base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
					}
				}
			}
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, vector, vector2, this.m_radiusAroundStart, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate BoardSquare StartSquareDelegate();
}
