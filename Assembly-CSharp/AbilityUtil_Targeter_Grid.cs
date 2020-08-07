using UnityEngine;

public class AbilityUtil_Targeter_Grid : AbilityUtil_Targeter
{
	public AbilityGridPattern m_pattern;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	private float m_scale = 1f;

	public AbilityUtil_Targeter_Grid(Ability ability, AbilityGridPattern pattern, float trapScale)
		: base(ability)
	{
		m_pattern = pattern;
		m_scale = trapScale;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		GridPos gridPos = (GetCurrentRangeInSquares() == 0f)
			? targetingActor.GetGridPosWithIncrementedHeight()
			: currentTarget.GridPos;
		return Board.Get().GetSquare(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(m_pattern, currentTarget.FreePos, gameplayRefSquare);
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			centerOfGridPattern.y = travelBoardSquareWorldPosition.y + m_heightOffset;
			return centerOfGridPattern;
		}
		return Vector3.zero;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare == null)
		{
			return;
		}

		Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
		if (Highlight == null)
		{
			Highlight = HighlightUtils.Get().CreateGridPatternHighlight(m_pattern, m_scale);
			Highlight.transform.position = highlightGoalPos;
		}
		else
		{
			Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, Highlight, ref m_curSpeed);
		}
		Highlight.SetActive(true);
	}
}
