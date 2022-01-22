using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreKnockbackLaser : AbilityUtil_Targeter
{
	private float m_laserWidth = 1f;

	private float m_laserRange = 15f;

	private bool m_penetrateLos;

	private bool m_lengthIgnoreGeo;

	private int m_maxTargets = -1;

	private float m_laserMiddleWidth;

	private float m_knockbackDistance;

	private KnockbackType m_knockbackType;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ClaymoreKnockbackLaser(Ability ability, float laserWidth, float distance, bool penetrateLos, bool lengthLgnoreGeo, int maxTargets, float laserMiddleWidth, float knockbackDistance, KnockbackType knockbackType)
		: base(ability)
	{
		m_laserWidth = laserWidth;
		m_laserRange = distance;
		m_penetrateLos = penetrateLos;
		m_lengthIgnoreGeo = lengthLgnoreGeo;
		m_maxTargets = maxTargets;
		m_laserMiddleWidth = laserMiddleWidth;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public float GetLaserRange()
	{
		return m_laserRange;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float num = m_laserWidth * Board.Get().squareSize;
		float num2 = m_laserMiddleWidth * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		ClearActorsInRange();
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, m_laserRange, m_laserWidth, targetingActor, targetingActor.GetEnemyTeams(), m_penetrateLos, m_maxTargets, m_lengthIgnoreGeo, false, out laserCoords.end, null);
		List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, m_laserRange, m_laserMiddleWidth, targetingActor, targetingActor.GetEnemyTeams(), m_penetrateLos, m_maxTargets, m_lengthIgnoreGeo, false, out laserCoords.end, null);
		VectorUtils.LaserCoords laserCoords2 = laserCoords;
		using (List<ActorData>.Enumerator enumerator = actorsInLaser2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, laserCoords2.start, targetingActor);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_00e7;
				}
			}
			end_IL_00e7:;
		}
		int num3 = 0;
		EnableAllMovementArrows();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		foreach (ActorData item in actorsInLaser)
		{
			if (!actorsInLaser2.Contains(item))
			{
				AddActorInRange(item, laserCoords2.start, targetingActor, AbilityTooltipSubject.Secondary);
				if (targetingActor.TechPoints + targetingActor.ReservedTechPoints >= targetingActor.GetMaxTechPoints())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(item, m_knockbackType, currentTarget.AimDirection, travelBoardSquareWorldPosition, m_knockbackDistance);
					num3 = AddMovementArrowWithPrevious(item, path, TargeterMovementType.Knockback, num3);
				}
			}
		}
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		}
		SetMovementArrowEnabledFromIndex(num3, false);
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_0284;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, 1f));
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num2, 1f));
		goto IL_0284;
		IL_0284:
		float magnitude = (laserCoords2.end - laserCoords2.start).magnitude;
		Vector3 normalized = (laserCoords2.end - laserCoords2.start).normalized;
		for (int i = 0; i < m_highlights.Count; i++)
		{
			float num4;
			if (i == 0)
			{
				num4 = num;
			}
			else
			{
				num4 = num2;
			}
			float widthInWorld = num4;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, m_highlights[i]);
			m_highlights[i].transform.position = laserCoords2.start + new Vector3(0f, y, 0f);
			m_highlights[i].transform.rotation = Quaternion.LookRotation(normalized);
		}
		while (true)
		{
			DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords2.start, laserCoords2.end);
			return;
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			float widthInSquares = Mathf.Max(m_laserWidth, m_laserMiddleWidth);
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, widthInSquares, targetingActor, m_penetrateLos);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
