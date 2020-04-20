using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScampHug : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	private bool m_directHitIgnoreCover;

	public AbilityUtil_Targeter_ScampHug.IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private ScampHug.TargetingMode m_targetingMode;

	private Scamp_SyncComponent m_syncComp;

	private float m_enemyKnockbackDist;

	private KnockbackType m_enemyKnockbackType;

	public AbilityUtil_Targeter_ScampHug(Ability ability, Scamp_SyncComponent syncComp, ScampHug.TargetingMode targetingMode, float dashWidthInSquares, float dashRangeInSquares, AbilityAreaShape aoeShape, bool directHitIgnoreCover, float enemyKnockbackDist, KnockbackType enemyKnockbackType) : base(ability)
	{
		this.m_syncComp = syncComp;
		this.m_targetingMode = targetingMode;
		this.m_dashWidthInSquares = dashWidthInSquares;
		this.m_dashRangeInSquares = dashRangeInSquares;
		this.m_aoeShape = aoeShape;
		this.m_directHitIgnoreCover = directHitIgnoreCover;
		this.m_enemyKnockbackDist = enemyKnockbackDist;
		this.m_enemyKnockbackType = enemyKnockbackType;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public int LastUpdatePathSquareCount { get; set; }

	private bool IsInKnockbackMode()
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.LastUpdatePathSquareCount = 0;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 3)
			{
				goto IL_CD;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		IL_CD:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		GameObject gameObject3 = this.m_highlights[2];
		if (this.IsInKnockbackMode())
		{
			bool flag = this.m_targetingMode == ScampHug.TargetingMode.Laser;
			ActorData actorData;
			List<ActorData> list;
			BoardSquare boardSquare;
			ScampHug.GetHitActorsAndKnockbackDestinationStatic(currentTarget, targetingActor, this.m_targetingMode, false, this.m_dashWidthInSquares, this.m_dashRangeInSquares, this.m_aoeShape, out actorData, out list, out boardSquare);
			bool active = false;
			Vector3 damageOrigin = Vector3.zero;
			if (actorData != null)
			{
				Vector3 travelBoardSquareWorldPosition;
				if (this.m_directHitIgnoreCover)
				{
					travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
				}
				else
				{
					travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
				}
				Vector3 damageOrigin2 = travelBoardSquareWorldPosition;
				base.AddActorInRange(actorData, damageOrigin2, targetingActor, AbilityTooltipSubject.Primary, false);
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				active = true;
				Vector3 position = currentBoardSquare.ToVector3();
				position.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position;
				damageOrigin = actorData.GetCurrentBoardSquare().ToVector3();
			}
			else if (!flag)
			{
				active = true;
				Vector3 position2 = boardSquare.ToVector3();
				position2.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position2;
				damageOrigin = boardSquare.ToVector3();
			}
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Secondary, false);
				}
			}
			if (this.m_affectCasterDelegate != null)
			{
				if (this.m_affectCasterDelegate(targetingActor, this.GetVisibleActorsInRange()))
				{
					base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
				}
			}
			gameObject2.SetActive(active);
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vector = boardSquare.ToVector3();
			Vector3 aimDir = vector - travelBoardSquareWorldPositionForLos;
			aimDir.y = 0f;
			float distance = aimDir.magnitude / Board.SquareSizeStatic;
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(targetingActor, KnockbackType.ForwardAlongAimDir, aimDir, vector, distance);
			int num = 0;
			base.EnableAllMovementArrows();
			num = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
			if (this.m_enemyKnockbackDist > 0f)
			{
				if (actorData != null)
				{
					if (!list.Contains(actorData))
					{
						list.Add(actorData);
					}
				}
				using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData actorData2 = enumerator2.Current;
						Vector3 aimDir2 = actorData2.GetTravelBoardSquareWorldPosition() - vector;
						aimDir.y = 0f;
						if (aimDir.sqrMagnitude > 0f)
						{
							aimDir.Normalize();
						}
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData2, this.m_enemyKnockbackType, aimDir2, vector, this.m_enemyKnockbackDist);
						num = base.AddMovementArrowWithPrevious(actorData2, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
					}
				}
			}
			base.SetMovementArrowEnabledFromIndex(num, false);
			if (boardSquarePathInfo != null)
			{
				this.LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd(true);
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
				HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, travelBoardSquareWorldPositionForLos, endPos, this.m_dashWidthInSquares);
			}
			else
			{
				gameObject3.transform.position = gameObject2.transform.position;
			}
			gameObject.SetActive(flag);
			gameObject3.SetActive(!flag);
		}
		else
		{
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			if (boardSquareSafe != null)
			{
				Vector3 position3 = boardSquareSafe.ToVector3();
				position3.y = HighlightUtils.GetHighlightHeight();
				gameObject3.transform.position = position3;
				gameObject3.SetActive(true);
				BoardSquarePathInfo path2 = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, targetingActor.GetCurrentBoardSquare(), false);
				base.EnableAllMovementArrows();
				int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, path2, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
				base.SetMovementArrowEnabledFromIndex(fromIndex, false);
			}
			else
			{
				gameObject3.SetActive(false);
			}
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
