using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BattleMonkUltimate : AbilityUtil_Targeter_Shape
{
	private AbilityAreaShape m_enemyShape;

	private bool m_enemyShapePenetratesLoS;

	private bool m_groundBasedMovement;

	private bool m_allowChargeThroughInvalidSquares = true;

	private GameObject EnemyHighlight
	{
		get
		{
			if (m_highlights != null && m_highlights.Count > 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return m_highlights[1];
					}
				}
			}
			return null;
		}
		set
		{
			if (m_highlights == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_highlights = new List<GameObject>();
			}
			while (m_highlights.Count <= 1)
			{
				m_highlights.Add(null);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_highlights[1] != null)
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
					DestroyObjectAndMaterials(m_highlights[1]);
					m_highlights[1] = null;
				}
				m_highlights[1] = value;
				return;
			}
		}
	}

	public AbilityUtil_Targeter_BattleMonkUltimate(Ability ability, AbilityAreaShape allyShape, bool allyShapePenetratesLoS, AbilityAreaShape enemyShape, bool enemyShapePenetratesLoS, bool groundBasedMovement)
		: base(ability, allyShape, allyShapePenetratesLoS, DamageOriginType.CenterOfShape, true, true, AffectsActor.Always)
	{
		m_enemyShape = enemyShape;
		m_enemyShapePenetratesLoS = enemyShapePenetratesLoS;
		m_groundBasedMovement = groundBasedMovement;
		SetAffectedGroups(true, true, true);
		m_showArcToShape = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (m_affectsEnemies)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (EnemyHighlight == null)
			{
				EnemyHighlight = HighlightUtils.Get().CreateShapeCursor(m_enemyShape, targetingActor == GameFlowData.Get().activeOwnedActorData);
			}
			EnemyHighlight.transform.position = base.Highlight.transform.position;
			EnemyHighlight.SetActive(true);
			BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
			if (gameplayRefSquare != null)
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
				List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_enemyShape, currentTarget.FreePos, gameplayRefSquare, m_enemyShapePenetratesLoS, targetingActor, targetingActor.GetOpposingTeam(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
				foreach (ActorData item in actors)
				{
					if (!(item != targetingActor))
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
						if (m_affectsCaster != AffectsActor.Possible)
						{
							continue;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					AddActorInRange(item, highlightGoalPos, targetingActor);
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (m_groundBasedMovement)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, currentBoardSquare, m_allowChargeThroughInvalidSquares);
		}
		else
		{
			boardSquarePathInfo = new BoardSquarePathInfo();
			boardSquarePathInfo.square = currentBoardSquare;
			BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
			boardSquarePathInfo2.square = boardSquareSafe;
			boardSquarePathInfo.next = boardSquarePathInfo2;
			boardSquarePathInfo2.prev = boardSquarePathInfo;
		}
		AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		if (potentialTarget.GetTeam() != targetingActor.GetTeam())
		{
			return false;
		}
		return base.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin, bestTarget);
	}
}
