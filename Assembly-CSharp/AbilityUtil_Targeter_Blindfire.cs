using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Blindfire : AbilityUtil_Targeter
{
	public float m_coneAngleDegrees;

	public float m_coneLengthRadiusInSquares;

	public float m_coneBackwardOffsetInSquares;

	public bool m_penetrateLoS;

	public bool m_restrictWithinCover;

	public bool m_includeTargetsInCover = true;

	public int m_maxTargets;

	private List<GameObject> m_boundsHighlights;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_Blindfire(Ability ability, float coneAngleDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool penetrateLoS, bool restrictWithinCover, bool includeTargetsInCover, int maxTargets) : base(ability)
	{
		this.m_coneAngleDegrees = coneAngleDegrees;
		this.m_coneLengthRadiusInSquares = coneLengthRadiusInSquares;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_penetrateLoS = penetrateLoS;
		this.m_restrictWithinCover = restrictWithinCover;
		this.m_includeTargetsInCover = includeTargetsInCover;
		this.m_maxTargets = maxTargets;
		this.m_boundsHighlights = new List<GameObject>();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		base.SetAffectedGroups(true, false, false);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	private void ClearBoundsHighlights()
	{
		using (List<GameObject>.Enumerator enumerator = this.m_boundsHighlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
		this.m_boundsHighlights.Clear();
	}

	protected override void ClearHighlightCursors(bool clearInstantly)
	{
		this.ClearBoundsHighlights();
		base.ClearHighlightCursors(clearInstantly);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		Vector3 casterPos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		ActorCover component = targetingActor.GetComponent<ActorCover>();
		if (this.m_restrictWithinCover)
		{
			this.CreateBoundsHighlights(casterPos, component);
		}
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vector2 = vector;
		if (component.IsDirInCover(vector2) || !this.m_restrictWithinCover)
		{
			float num = VectorUtils.HorizontalAngle_Deg(vector2);
			float num2;
			if (this.m_restrictWithinCover)
			{
				Vector3 vector3;
				component.ClampConeToValidCover(num, this.m_coneAngleDegrees, out num2, out vector3);
			}
			else
			{
				num2 = num;
				Vector3 vector3 = vector2;
			}
			this.CreateConeHighlights(casterPos, num2);
			List<Team> affectedTeams = base.GetAffectedTeams();
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(casterPos, num2, this.m_coneAngleDegrees, this.m_coneLengthRadiusInSquares, this.m_coneBackwardOffsetInSquares, this.m_penetrateLoS, targetingActor, affectedTeams, null, false, default(Vector3));
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
			if (!this.m_includeTargetsInCover)
			{
				actorsInCone.RemoveAll(delegate(ActorData actor)
				{
					bool result;
					if (actor.GetActorCover() != null)
					{
						result = actor.GetActorCover().IsInCoverWrt(casterPos);
					}
					else
					{
						result = false;
					}
					return result;
				});
			}
			if (this.m_maxTargets > 0)
			{
				TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, casterPos);
				TargeterUtils.LimitActorsToMaxNumber(ref actorsInCone, this.m_maxTargets);
			}
			foreach (ActorData actor2 in actorsInCone)
			{
				base.AddActorInRange(actor2, casterPos, targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		else
		{
			this.ClearHighlightCursors(true);
		}
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	public void CreateConeHighlights(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 vector = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - vector * d;
		if (base.Highlight == null)
		{
			float radiusInWorld = (this.m_coneLengthRadiusInSquares + this.m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			base.Highlight = HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneAngleDegrees);
		}
		base.Highlight.transform.position = position;
		base.Highlight.transform.rotation = Quaternion.LookRotation(vector);
	}

	public void CreateBoundsHighlights(Vector3 casterPos, ActorCover actorCover)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f);
		List<CoverRegion> coveredRegions = actorCover.GetCoveredRegions();
		int num = coveredRegions.Count * 2;
		if (this.m_boundsHighlights.Count != num)
		{
			this.ClearBoundsHighlights();
			for (int i = 0; i < coveredRegions.Count; i++)
			{
				GameObject item = HighlightUtils.Get().CreateBoundaryLine(this.m_coneLengthRadiusInSquares, false, true);
				GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(this.m_coneLengthRadiusInSquares, false, false);
				this.m_boundsHighlights.Add(item);
				this.m_boundsHighlights.Add(item2);
			}
		}
		for (int j = 0; j < coveredRegions.Count; j++)
		{
			int num2 = j * 2;
			int index = num2 + 1;
			Vector3 forward = -VectorUtils.AngleDegreesToVector(coveredRegions[j].m_endAngle);
			this.m_boundsHighlights[num2].transform.position = position;
			this.m_boundsHighlights[num2].transform.rotation = Quaternion.LookRotation(forward);
			Vector3 forward2 = -VectorUtils.AngleDegreesToVector(coveredRegions[j].m_startAngle);
			this.m_boundsHighlights[index].transform.position = position;
			this.m_boundsHighlights[index].transform.rotation = Quaternion.LookRotation(forward2);
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vector;
			if (currentTarget == null)
			{
				vector = targetingActor.transform.forward;
			}
			else
			{
				vector = currentTarget.AimDirection;
			}
			Vector3 vec = vector;
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_coneAngleDegrees, this.m_coneLengthRadiusInSquares, this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLoS, null);
			base.HideUnusedSquareIndicators();
		}
	}
}
