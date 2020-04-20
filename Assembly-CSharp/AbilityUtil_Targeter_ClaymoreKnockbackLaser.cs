using System;
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

	public AbilityUtil_Targeter_ClaymoreKnockbackLaser(Ability ability, float laserWidth, float distance, bool penetrateLos, bool lengthLgnoreGeo, int maxTargets, float laserMiddleWidth, float knockbackDistance, KnockbackType knockbackType) : base(ability)
	{
		this.m_laserWidth = laserWidth;
		this.m_laserRange = distance;
		this.m_penetrateLos = penetrateLos;
		this.m_lengthIgnoreGeo = lengthLgnoreGeo;
		this.m_maxTargets = maxTargets;
		this.m_laserMiddleWidth = laserMiddleWidth;
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public float GetLaserRange()
	{
		return this.m_laserRange;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float num = this.m_laserWidth * Board.Get().squareSize;
		float num2 = this.m_laserMiddleWidth * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		base.ClearActorsInRange();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, this.m_laserRange, this.m_laserWidth, targetingActor, targetingActor.GetOpposingTeams(), this.m_penetrateLos, this.m_maxTargets, this.m_lengthIgnoreGeo, false, out laserCoords.end, null, null, false, true);
		List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, this.m_laserRange, this.m_laserMiddleWidth, targetingActor, targetingActor.GetOpposingTeams(), this.m_penetrateLos, this.m_maxTargets, this.m_lengthIgnoreGeo, false, out laserCoords.end, null, null, false, true);
		VectorUtils.LaserCoords laserCoords2 = laserCoords;
		using (List<ActorData>.Enumerator enumerator = actorsInLaser2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				base.AddActorInRange(actor, laserCoords2.start, targetingActor, AbilityTooltipSubject.Primary, false);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreKnockbackLaser.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
		}
		int num3 = 0;
		base.EnableAllMovementArrows();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		foreach (ActorData actorData in actorsInLaser)
		{
			if (!actorsInLaser2.Contains(actorData))
			{
				base.AddActorInRange(actorData, laserCoords2.start, targetingActor, AbilityTooltipSubject.Secondary, false);
				if (targetingActor.TechPoints + targetingActor.ReservedTechPoints >= targetingActor.GetActualMaxTechPoints())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, travelBoardSquareWorldPosition, this.m_knockbackDistance);
					num3 = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num3, false);
				}
			}
		}
		if (this.m_affectsTargetingActor)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		base.SetMovementArrowEnabledFromIndex(num3, false);
		if (this.m_highlights != null)
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
			if (this.m_highlights.Count >= 2)
			{
				goto IL_284;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num2, 1f, null));
		IL_284:
		float magnitude = (laserCoords2.end - laserCoords2.start).magnitude;
		Vector3 normalized = (laserCoords2.end - laserCoords2.start).normalized;
		for (int i = 0; i < this.m_highlights.Count; i++)
		{
			float num4;
			if (i == 0)
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
				num4 = num;
			}
			else
			{
				num4 = num2;
			}
			float widthInWorld = num4;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, this.m_highlights[i]);
			this.m_highlights[i].transform.position = laserCoords2.start + new Vector3(0f, y, 0f);
			this.m_highlights[i].transform.rotation = Quaternion.LookRotation(normalized);
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
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords2.start, laserCoords2.end);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreKnockbackLaser.DrawInvalidSquareIndicators(AbilityTarget, ActorData, Vector3, Vector3)).MethodHandle;
			}
			base.ResetSquareIndicatorIndexToUse();
			float widthInSquares = Mathf.Max(this.m_laserWidth, this.m_laserMiddleWidth);
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, widthInSquares, targetingActor, this.m_penetrateLos, null, null, true);
			base.HideUnusedSquareIndicators();
		}
	}
}
