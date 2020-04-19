using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreSlam : AbilityUtil_Targeter
{
	public float m_laserRange;

	private float m_laserWidth;

	private int m_laserMaxTargets;

	private float m_coneAngleDegrees;

	private float m_coneLengthRadius;

	private bool m_penetrateLos;

	private float m_coneBackwardOffsetInSquares;

	private bool m_appendTooltipForDuplicates;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_ClaymoreSlam(Ability ability, float laserRange, float laserWidth, int laserMaxTargets, float coneAngleDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool penetrateLos, bool affectEnemies = true, bool affectAllies = false, bool affectCaster = false, bool appendTooltipForDuplicates = false) : base(ability)
	{
		this.m_laserRange = laserRange;
		this.m_laserWidth = laserWidth;
		this.m_laserMaxTargets = laserMaxTargets;
		this.m_coneAngleDegrees = coneAngleDegrees;
		this.m_coneLengthRadius = coneLengthRadius;
		this.m_penetrateLos = penetrateLos;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_affectsEnemies = affectEnemies;
		this.m_affectsAllies = affectAllies;
		this.m_affectsTargetingActor = affectCaster;
		this.m_appendTooltipForDuplicates = appendTooltipForDuplicates;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreSlam..ctor(Ability, float, float, int, float, float, float, bool, bool, bool, bool, bool)).MethodHandle;
			}
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_laserWidth));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.AllocateHighlights();
		Vector3 vector = targetingActor.\u0015();
		Vector3 aimDirection = currentTarget.AimDirection;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = targetingActor.\u0015();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, aimDirection, this.m_laserRange, this.m_laserWidth, targetingActor, relevantTeams, this.m_penetrateLos, this.m_laserMaxTargets, false, false, out laserCoords.end, null, null, false, true);
		VectorUtils.LaserCoords laserCoords2 = laserCoords;
		if (this.m_affectsTargetingActor)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreSlam.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (!actorsInLaser.Contains(targetingActor))
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
				base.AddActorInRange(targetingActor, vector, targetingActor, AbilityTooltipSubject.Self, false);
			}
		}
		int num = 0;
		Vector3 b = targetingActor.\u0016();
		float squareSizeStatic = Board.SquareSizeStatic;
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (this.ShouldAddActor(actorData, targetingActor))
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
					float u000E = (actorData.\u0016() - b).magnitude / squareSizeStatic;
					base.AddActorInRange(actorData, vector, targetingActor, AbilityTooltipSubject.Primary, false);
					ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
					actorHitContext.\u001D = laserCoords2.start;
					actorHitContext.\u0015.\u0016(ContextKeys.\u0011.\u0012(), num);
					actorHitContext.\u0015.\u0015(ContextKeys.\u0018.\u0012(), u000E);
					num++;
				}
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
		}
		float num2 = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector, num2, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, this.m_penetrateLos, targetingActor, targetingActor.\u0012(), null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		using (List<ActorData>.Enumerator enumerator2 = actorsInCone.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData actorData2 = enumerator2.Current;
				if (this.ShouldAddActor(actorData2, targetingActor))
				{
					base.AddActorInRange(actorData2, vector, targetingActor, AbilityTooltipSubject.Secondary, this.m_appendTooltipForDuplicates);
					if (!actorsInLaser.Contains(actorData2))
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorData2 != targetingActor)
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
							float u000E2 = (actorData2.\u0016() - b).magnitude / squareSizeStatic;
							ActorHitContext actorHitContext2 = this.m_actorContextVars[actorData2];
							actorHitContext2.\u001D = laserCoords2.start;
							actorHitContext2.\u0015.\u0015(ContextKeys.\u0018.\u0012(), u000E2);
						}
					}
				}
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
		}
		GameObject gameObject = this.m_highlights[0];
		float d = this.m_coneBackwardOffsetInSquares * Board.\u000E().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = vector + new Vector3(0f, y, 0f) - aimDirection * d;
		gameObject.transform.position = position;
		gameObject.transform.rotation = Quaternion.LookRotation(aimDirection);
		HighlightUtils.Get().RotateAndResizeRectangularCursor(this.m_highlights[1], vector, laserCoords2.end, this.m_laserWidth);
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[0] as SquareInsideChecker_Box;
			SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[1] as SquareInsideChecker_Cone;
			squareInsideChecker_Box.UpdateBoxProperties(vector, laserCoords2.end, targetingActor);
			squareInsideChecker_Cone.UpdateConeProperties(vector, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, num2, targetingActor);
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, vector, laserCoords2.end, this.m_laserWidth, targetingActor, this.m_penetrateLos, null, this.m_squarePosCheckerList, true);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, vector, num2, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLos, this.m_squarePosCheckerList);
			base.HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = this.m_affectsTargetingActor;
		}
		else
		{
			if (actor.\u000E() == caster.\u000E())
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreSlam.ShouldAddActor(ActorData, ActorData)).MethodHandle;
				}
				if (this.m_affectsAllies)
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
					return true;
				}
			}
			if (actor.\u000E() != caster.\u000E() && this.m_affectsEnemies)
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
				result = true;
			}
		}
		return result;
	}

	private void AllocateHighlights()
	{
		if (this.m_highlights != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreSlam.AllocateHighlights()).MethodHandle;
			}
			if (this.m_highlights.Count >= 2)
			{
				return;
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
		this.m_highlights = new List<GameObject>();
		float radiusInWorld = (this.m_coneLengthRadius + this.m_coneBackwardOffsetInSquares) * Board.\u000E().squareSize;
		this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneAngleDegrees));
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(this.m_laserWidth, 1f, null));
	}
}
