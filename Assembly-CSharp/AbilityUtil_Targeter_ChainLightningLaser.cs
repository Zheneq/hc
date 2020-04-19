using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ChainLightningLaser : AbilityUtil_Targeter
{
	private float m_width = 1f;

	public float m_distance = 15f;

	private bool m_penetrateLos;

	private int m_maxTargets = -1;

	private int m_maxChainHits = -1;

	private float m_chainRadius = 3f;

	private int m_numChainHighlights = 3;

	private float m_chainHighlightWidthInSquares = 0.1f;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_ChainLightningLaser(Ability ability, float width, float distance, bool penetrateLos, int maxTargets, bool affectsAllies, int maxChainHits, float chainRadius) : base(ability)
	{
		this.m_width = width;
		this.m_distance = distance;
		this.m_penetrateLos = penetrateLos;
		this.m_maxTargets = maxTargets;
		this.m_affectsAllies = affectsAllies;
		this.m_maxChainHits = maxChainHits;
		this.m_chainRadius = chainRadius;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_width));
		for (int i = 0; i < this.m_maxChainHits; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_chainHighlightWidthInSquares));
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.AllocateHighlights();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, true);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = targetingActor.\u0015();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, this.m_distance, this.m_width, targetingActor, relevantTeams, this.m_penetrateLos, this.m_maxTargets, false, false, out laserCoords.end, null, null, false, true);
		GameObject highlightObj = this.m_highlights[0];
		SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[0] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
		this.AdjustLaserHighlight(highlightObj, laserCoords.start, laserCoords.end, this.m_width);
		List<ActorData> list = new List<ActorData>();
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				base.AddActorInRange(actorData, laserCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
				list.Add(actorData);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ChainLightningLaser.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
		}
		int num = 0;
		if (actorsInLaser.Count > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			ActorData actorData2 = actorsInLaser[actorsInLaser.Count - 1];
			while (actorData2 != null)
			{
				if (this.m_maxChainHits > 0 && num >= this.m_maxChainHits)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_255;
					}
				}
				else
				{
					ActorData actorData3 = this.FindChainHitActor(actorData2, targetingActor, list);
					if (actorData3 != null)
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
						base.AddActorInRange(actorData3, actorData2.\u0015(), targetingActor, AbilityTooltipSubject.Secondary, false);
						list.Add(actorData3);
						if (num + 1 < this.m_highlights.Count)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							GameObject gameObject = this.m_highlights[1 + num];
							this.AdjustLaserHighlight(gameObject, actorData2.\u0016(), actorData3.\u0016(), this.m_chainHighlightWidthInSquares);
							gameObject.SetActive(true);
							SquareInsideChecker_Box squareInsideChecker_Box2 = this.m_squarePosCheckerList[1 + num] as SquareInsideChecker_Box;
							squareInsideChecker_Box2.UpdateBoxProperties(actorData2.\u0016(), actorData3.\u0016(), targetingActor);
						}
						num++;
					}
					actorData2 = actorData3;
				}
			}
		}
		IL_255:
		for (int i = num + 1; i < this.m_highlights.Count; i++)
		{
			this.m_highlights[i].SetActive(false);
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = num + 1; j < this.m_squarePosCheckerList.Count; j++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box3 = this.m_squarePosCheckerList[j] as SquareInsideChecker_Box;
			squareInsideChecker_Box3.UpdateBoxProperties(squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), targetingActor);
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, laserCoords.start, laserCoords.end, this.m_width, targetingActor, this.m_penetrateLos, null, this.m_squarePosCheckerList, true);
			for (int k = 0; k < num; k++)
			{
				SquareInsideChecker_Box squareInsideChecker_Box4 = this.m_squarePosCheckerList[1 + k] as SquareInsideChecker_Box;
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, squareInsideChecker_Box4.GetStartPos(), squareInsideChecker_Box4.GetEndPos(), this.m_chainHighlightWidthInSquares, targetingActor, true, null, this.m_squarePosCheckerList, true);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			base.HideUnusedSquareIndicators();
		}
	}

	private ActorData FindChainHitActor(ActorData fromActor, ActorData caster, List<ActorData> actorsAddedSoFar)
	{
		ActorData result = null;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.m_affectsAllies, true);
		Vector3 vector = fromActor.\u0015();
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(vector, this.m_chainRadius, this.m_penetrateLos, caster, relevantTeams, null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadius, vector);
		using (List<ActorData>.Enumerator enumerator = actorsInRadius.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!actorsAddedSoFar.Contains(actorData))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ChainLightningLaser.FindChainHitActor(ActorData, ActorData, List<ActorData>)).MethodHandle;
					}
					return actorData;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	private void AllocateHighlights()
	{
		float widthInWorld = this.m_width * Board.\u000E().squareSize;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ChainLightningLaser.AllocateHighlights()).MethodHandle;
			}
			if (this.m_highlights.Count >= this.m_numChainHighlights + 1)
			{
				return;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, 1f, null));
		for (int i = 0; i < this.m_numChainHighlights; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(this.m_chainHighlightWidthInSquares, 1f, null));
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void AdjustLaserHighlight(GameObject highlightObj, Vector3 startPos, Vector3 endPos, float widthInSquares)
	{
		startPos.y = HighlightUtils.GetHighlightHeight();
		endPos.y = startPos.y;
		float widthInWorld = widthInSquares * Board.\u000E().squareSize;
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
		Vector3 normalized = vector.normalized;
		highlightObj.transform.position = startPos;
		highlightObj.transform.rotation = Quaternion.LookRotation(normalized);
	}
}
