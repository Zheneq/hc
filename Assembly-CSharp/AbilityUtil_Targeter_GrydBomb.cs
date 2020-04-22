using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GrydBomb : AbilityUtil_Targeter_Shape
{
	public bool m_lockToCardinalDirs = true;

	public bool m_showArrowHighlight = true;

	public float m_bombMoveRange;

	public float m_heightOffset = 0.1f;

	private GrydPlaceOrMoveBomb m_bombAbility;

	public AbilityUtil_Targeter_GrydBomb(Ability ability, float moveRange)
		: base(ability, AbilityAreaShape.SingleSquare, false)
	{
		m_bombMoveRange = moveRange;
		m_bombAbility = (ability as GrydPlaceOrMoveBomb);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_bombAbility != null && m_bombAbility.HasPlacedBomb())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ClearActorsInRange();
					SetShowArcToShape(false);
					Vector3 worldPositionForLoS = Board.Get().GetBoardSquareSafe(m_bombAbility.GetPlacedBomb()).GetWorldPositionForLoS();
					Vector3 vector = currentTarget.FreePos - worldPositionForLoS;
					if (m_lockToCardinalDirs)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						vector = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector)));
					}
					if (m_highlights != null && m_highlights.Count < 1)
					{
						m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(Board.Get().squareSize * 0.75f, m_bombMoveRange * Board.Get().squareSize));
					}
					Vector3 position = worldPositionForLoS;
					position.y = HighlightUtils.GetHighlightHeight();
					m_highlights[0].transform.position = position;
					m_highlights[0].transform.rotation = Quaternion.LookRotation(vector);
					Vector3 laserEndPos;
					List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(worldPositionForLoS, vector, m_bombMoveRange, 0.75f, targetingActor, targetingActor.GetOpposingTeams(), false, 1, false, false, out laserEndPos, null);
					AddActorsInRange(actorsInLaser, worldPositionForLoS, targetingActor);
					return;
				}
				}
			}
		}
		SetShowArcToShape(true);
		base.UpdateTargeting(currentTarget, targetingActor);
	}
}
