using System;
using UnityEngine;

public class AbilityUtil_Targeter_Grid : AbilityUtil_Targeter
{
	public AbilityGridPattern m_pattern;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	private float m_scale = 1f;

	public AbilityUtil_Targeter_Grid(Ability ability, AbilityGridPattern pattern, float trapScale) : base(ability)
	{
		this.m_pattern = pattern;
		this.m_scale = trapScale;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		GridPos gridPos;
		if (this.GetCurrentRangeInSquares() != 0f)
		{
			gridPos = currentTarget.GridPos;
		}
		else
		{
			gridPos = targetingActor.GetGridPosWithIncrementedHeight();
		}
		return Board.Get().GetBoardSquareSafe(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(this.m_pattern, currentTarget.FreePos, gameplayRefSquare);
			centerOfGridPattern.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfGridPattern;
		}
		return Vector3.zero;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = this.GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				base.Highlight = HighlightUtils.Get().CreateGridPatternHighlight(this.m_pattern, this.m_scale);
				base.Highlight.transform.position = highlightGoalPos;
			}
			else
			{
				base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref this.m_curSpeed);
			}
			base.Highlight.SetActive(true);
		}
	}
}
