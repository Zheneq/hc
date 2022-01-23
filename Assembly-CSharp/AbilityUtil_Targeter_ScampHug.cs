using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScampHug : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	private bool m_directHitIgnoreCover;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private ScampHug.TargetingMode m_targetingMode;

	private Scamp_SyncComponent m_syncComp;

	private float m_enemyKnockbackDist;

	private KnockbackType m_enemyKnockbackType;

	public int LastUpdatePathSquareCount
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_ScampHug(Ability ability, Scamp_SyncComponent syncComp, ScampHug.TargetingMode targetingMode, float dashWidthInSquares, float dashRangeInSquares, AbilityAreaShape aoeShape, bool directHitIgnoreCover, float enemyKnockbackDist, KnockbackType enemyKnockbackType)
		: base(ability)
	{
		m_syncComp = syncComp;
		m_targetingMode = targetingMode;
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_aoeShape = aoeShape;
		m_directHitIgnoreCover = directHitIgnoreCover;
		m_enemyKnockbackDist = enemyKnockbackDist;
		m_enemyKnockbackType = enemyKnockbackType;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	private bool IsInKnockbackMode()
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		LastUpdatePathSquareCount = 0;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 3)
			{
				goto IL_00cd;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		goto IL_00cd;
		IL_00cd:
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		GameObject gameObject3 = m_highlights[2];
		if (IsInKnockbackMode())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					bool flag = m_targetingMode == ScampHug.TargetingMode.Laser;
					ScampHug.GetHitActorsAndKnockbackDestinationStatic(currentTarget, targetingActor, m_targetingMode, false, m_dashWidthInSquares, m_dashRangeInSquares, m_aoeShape, out ActorData firstHitActor, out List<ActorData> aoeHitActors, out BoardSquare knockbackDestSquare);
					bool active = false;
					Vector3 damageOrigin = Vector3.zero;
					if (firstHitActor != null)
					{
						Vector3 travelBoardSquareWorldPosition;
						if (m_directHitIgnoreCover)
						{
							travelBoardSquareWorldPosition = firstHitActor.GetFreePos();
						}
						else
						{
							travelBoardSquareWorldPosition = targetingActor.GetFreePos();
						}
						Vector3 damageOrigin2 = travelBoardSquareWorldPosition;
						AddActorInRange(firstHitActor, damageOrigin2, targetingActor);
						BoardSquare currentBoardSquare = firstHitActor.GetCurrentBoardSquare();
						active = true;
						Vector3 position = currentBoardSquare.ToVector3();
						position.y = HighlightUtils.GetHighlightHeight();
						gameObject2.transform.position = position;
						damageOrigin = firstHitActor.GetCurrentBoardSquare().ToVector3();
					}
					else if (!flag)
					{
						active = true;
						Vector3 position2 = knockbackDestSquare.ToVector3();
						position2.y = HighlightUtils.GetHighlightHeight();
						gameObject2.transform.position = position2;
						damageOrigin = knockbackDestSquare.ToVector3();
					}
					using (List<ActorData>.Enumerator enumerator = aoeHitActors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Secondary);
						}
					}
					if (m_affectCasterDelegate != null)
					{
						if (m_affectCasterDelegate(targetingActor, GetVisibleActorsInRange()))
						{
							AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
						}
					}
					gameObject2.SetActive(active);
					Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
					Vector3 vector = knockbackDestSquare.ToVector3();
					Vector3 aimDir = vector - travelBoardSquareWorldPositionForLos;
					aimDir.y = 0f;
					float distance = aimDir.magnitude / Board.SquareSizeStatic;
					BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(targetingActor, KnockbackType.ForwardAlongAimDir, aimDir, vector, distance);
					int arrowIndex = 0;
					EnableAllMovementArrows();
					arrowIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Knockback, arrowIndex);
					if (m_enemyKnockbackDist > 0f)
					{
						if (firstHitActor != null)
						{
							if (!aoeHitActors.Contains(firstHitActor))
							{
								aoeHitActors.Add(firstHitActor);
							}
						}
						using (List<ActorData>.Enumerator enumerator2 = aoeHitActors.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ActorData current2 = enumerator2.Current;
								Vector3 aimDir2 = current2.GetFreePos() - vector;
								aimDir.y = 0f;
								if (aimDir.sqrMagnitude > 0f)
								{
									aimDir.Normalize();
								}
								BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current2, m_enemyKnockbackType, aimDir2, vector, m_enemyKnockbackDist);
								arrowIndex = AddMovementArrowWithPrevious(current2, path, TargeterMovementType.Knockback, arrowIndex);
							}
						}
					}
					SetMovementArrowEnabledFromIndex(arrowIndex, false);
					if (boardSquarePathInfo != null)
					{
						LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd();
					}
					if (flag)
					{
						Vector3 a = vector;
						if (boardSquarePathInfo != null)
						{
							BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
							a = pathEndpoint.square.ToVector3();
						}
						Vector3 lhs = a - travelBoardSquareWorldPositionForLos;
						lhs.y = 0f;
						float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
						Vector3 endPos = travelBoardSquareWorldPositionForLos + d * currentTarget.AimDirection;
						endPos.y = HighlightUtils.GetHighlightHeight();
						HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, travelBoardSquareWorldPositionForLos, endPos, m_dashWidthInSquares);
					}
					else
					{
						gameObject3.transform.position = gameObject2.transform.position;
					}
					gameObject.SetActive(flag);
					gameObject3.SetActive(!flag);
					return;
				}
				}
			}
		}
		gameObject.SetActive(false);
		gameObject2.SetActive(false);
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (boardSquareSafe != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					Vector3 position3 = boardSquareSafe.ToVector3();
					position3.y = HighlightUtils.GetHighlightHeight();
					gameObject3.transform.position = position3;
					gameObject3.SetActive(true);
					BoardSquarePathInfo path2 = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, targetingActor.GetCurrentBoardSquare(), false);
					EnableAllMovementArrows();
					int fromIndex = AddMovementArrowWithPrevious(targetingActor, path2, TargeterMovementType.Movement, 0);
					SetMovementArrowEnabledFromIndex(fromIndex, false);
					return;
				}
				}
			}
		}
		gameObject3.SetActive(false);
	}
}
