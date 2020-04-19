using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TrackerDrone : AbilityUtil_Targeter
{
	protected TrackerDroneTrackerComponent m_droneTrackerComponent;

	protected float m_radiusAroundStart = 2f;

	protected float m_radiusAroundEnd = 2f;

	protected float m_rangeFromLine = 2f;

	protected bool m_addTargets = true;

	protected bool m_penetrateLoS;

	protected bool m_hitUntracked;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_TrackerDrone(Ability ability, TrackerDroneTrackerComponent droneTracker, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, bool addTargets, bool hitUntrackedTargets) : base(ability)
	{
		this.m_droneTrackerComponent = droneTracker;
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_radiusAroundEnd = radiusAroundEnd;
		this.m_rangeFromLine = rangeFromDir;
		this.m_addTargets = addTargets;
		this.m_penetrateLoS = penetrateLoS;
		this.m_hitUntracked = hitUntrackedTargets;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TrackerDrone..ctor(Ability, TrackerDroneTrackerComponent, float, float, float, int, bool, bool, bool, bool)).MethodHandle;
			}
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
	}

	protected AbilityUtil_Targeter_TrackerDrone(Ability ability) : base(ability)
	{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TrackerDrone..ctor(Ability)).MethodHandle;
			}
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
	}

	public int GetNumHighlights()
	{
		return 3;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		if (boardSquare == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TrackerDrone.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			return;
		}
		Vector3 vector = boardSquare.ToVector3();
		Vector3 vector2 = targetingActor.\u0016();
		if (this.m_droneTrackerComponent != null && this.m_droneTrackerComponent.DroneIsActive())
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
			BoardSquare boardSquare2 = Board.\u000E().\u0016(this.m_droneTrackerComponent.BoardX(), this.m_droneTrackerComponent.BoardY());
			vector2 = boardSquare2.ToVector3();
		}
		bool flag = this.m_droneTrackerComponent != null && this.m_droneTrackerComponent.DroneIsActive();
		float num;
		if (flag)
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
			num = this.m_radiusAroundStart;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		if (this.m_addTargets)
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
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector, num2, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor, null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
			using (List<ActorData>.Enumerator enumerator = actorsInRadiusOfLine.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					bool flag2;
					if (this.m_droneTrackerComponent != null)
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
						flag2 = this.m_droneTrackerComponent.IsTrackingActor(actorData.ActorIndex);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (actorData.\u000E() != targetingActor.\u000E())
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
						if (!this.m_hitUntracked)
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
							if (!flag3)
							{
								continue;
							}
						}
						ActorData actor = actorData;
						Vector3 damageOrigin = vector2;
						AbilityTooltipSubject subjectType;
						if (flag3)
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
							subjectType = AbilityTooltipSubject.Primary;
						}
						else
						{
							subjectType = AbilityTooltipSubject.Secondary;
						}
						base.AddActorInRange(actor, damageOrigin, targetingActor, subjectType, false);
					}
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
			}
		}
		bool flag4 = this.m_rangeFromLine > 0f;
		bool flag5 = num2 > 0f;
		bool flag6 = this.m_radiusAroundEnd > 0f;
		int index = 0;
		int index2 = 1;
		int index3 = 2;
		float widthInSquares = this.m_rangeFromLine * 2f;
		if (this.m_highlights != null)
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
			if (this.m_highlights.Count >= this.GetNumHighlights())
			{
				goto IL_2CC;
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
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(TargeterUtils.CreateLaserBoxHighlight(vector2, vector, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight));
		this.m_highlights.Add(TargeterUtils.CreateCircleHighlight(vector2, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
		this.m_highlights.Add(TargeterUtils.CreateCircleHighlight(vector, this.m_radiusAroundEnd, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
		IL_2CC:
		Vector3 vector3 = vector2;
		vector3.y = HighlightUtils.GetHighlightHeight();
		Vector3 vector4 = vector;
		vector4.y = HighlightUtils.GetHighlightHeight();
		if (flag4)
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
			if (vector2 != vector)
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
				this.m_highlights[index].SetActive(true);
				TargeterUtils.RefreshLaserBoxHighlight(this.m_highlights[index], vector3, vector4, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				goto IL_354;
			}
		}
		this.m_highlights[index].SetActive(false);
		IL_354:
		if (flag5)
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
			this.m_highlights[index2].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[index2], vector3, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			this.m_highlights[index2].SetActive(false);
		}
		if (flag6)
		{
			this.m_highlights[index3].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[index3], vector4, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			this.m_highlights[index3].SetActive(false);
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, vector2, vector, num2, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor);
			base.HideUnusedSquareIndicators();
		}
	}
}
