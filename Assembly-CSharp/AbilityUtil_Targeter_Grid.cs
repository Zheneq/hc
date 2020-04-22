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
		GridPos gridPos = (GetCurrentRangeInSquares() == 0f) ? targetingActor.GetGridPosWithIncrementedHeight() : currentTarget.GridPos;
		return Board.Get().GetBoardSquareSafe(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(m_pattern, currentTarget.FreePos, gameplayRefSquare);
					Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
					centerOfGridPattern.y = travelBoardSquareWorldPosition.y + m_heightOffset;
					return centerOfGridPattern;
				}
				}
			}
		}
		return Vector3.zero;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (!(gameplayRefSquare != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				base.Highlight = HighlightUtils.Get().CreateGridPatternHighlight(m_pattern, m_scale);
				base.Highlight.transform.position = highlightGoalPos;
			}
			else
			{
				base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref m_curSpeed);
			}
			base.Highlight.SetActive(true);
			return;
		}
	}
}
