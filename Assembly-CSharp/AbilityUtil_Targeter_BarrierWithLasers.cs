using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BarrierWithLasers : AbilityUtil_Targeter_Barrier
{
	public float m_laserRangeFront;

	public float m_laserRangeBack;

	public bool m_laserIgnoreLos;

	public bool m_laserLengthIgnoreLos;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private TargeterPart_Laser m_laserPartFront;

	private TargeterPart_Laser m_laserPartBack;

	public AbilityUtil_Targeter_BarrierWithLasers(Ability ability, float width, float laserRangeFront, float laserRangeBack, bool laserIgnoreLos, bool laserLengthIgnoreLos) : base(ability, width, true, false, true)
	{
		this.m_laserRangeFront = laserRangeFront;
		this.m_laserRangeBack = laserRangeBack;
		this.m_laserIgnoreLos = laserIgnoreLos;
		this.m_laserLengthIgnoreLos = laserLengthIgnoreLos;
		this.m_laserPartFront = new TargeterPart_Laser(this.m_width, this.m_laserRangeFront, this.m_laserIgnoreLos, -1);
		this.m_laserPartFront.m_lengthIgnoreWorldGeo = this.m_laserLengthIgnoreLos;
		this.m_laserPartFront.m_ignoreStartOffset = true;
		this.m_laserPartBack = new TargeterPart_Laser(this.m_width, this.m_laserRangeBack, this.m_laserIgnoreLos, -1);
		this.m_laserPartBack.m_lengthIgnoreWorldGeo = this.m_laserLengthIgnoreLos;
		this.m_laserPartBack.m_ignoreStartOffset = true;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		int num = 1;
		if (this.m_snapToBorder)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BarrierWithLasers.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			num = 2;
		}
		if (this.m_highlights.Count < num + 2)
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
			this.m_highlights.Add(this.m_laserPartFront.CreateHighlightObject(this));
			this.m_highlights.Add(this.m_laserPartBack.CreateHighlightObject(this));
		}
		GameObject gameObject = this.m_highlights[num];
		GameObject gameObject2 = this.m_highlights[num + 1];
		bool flag = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (flag)
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
			base.ResetSquareIndicatorIndexToUse();
		}
		Vector3 barrierCenterPos = this.m_barrierCenterPos;
		barrierCenterPos.y = (float)Board.\u000E().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
		if (this.m_laserRangeFront > 0f)
		{
			Vector3 endPos;
			List<ActorData> hitActors = this.m_laserPartFront.GetHitActors(barrierCenterPos, this.m_barrierDir, targetingActor, relevantTeams, out endPos);
			using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, barrierCenterPos, targetingActor, AbilityTooltipSubject.Primary, false);
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag)
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
				this.m_laserPartFront.ShowHiddenSquares(this.m_indicatorHandler, barrierCenterPos, endPos, targetingActor, this.m_laserIgnoreLos);
			}
			this.m_laserPartFront.AdjustHighlight(gameObject, barrierCenterPos, endPos, false);
		}
		else
		{
			gameObject.SetActiveIfNeeded(false);
		}
		if (this.m_laserRangeBack > 0f)
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
			Vector3 endPos2;
			List<ActorData> hitActors2 = this.m_laserPartBack.GetHitActors(barrierCenterPos, -1f * this.m_barrierDir, targetingActor, relevantTeams, out endPos2);
			using (List<ActorData>.Enumerator enumerator2 = hitActors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					base.AddActorInRange(actor2, barrierCenterPos, targetingActor, AbilityTooltipSubject.Primary, false);
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
			if (flag)
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
				this.m_laserPartBack.ShowHiddenSquares(this.m_indicatorHandler, barrierCenterPos, endPos2, targetingActor, this.m_laserIgnoreLos);
			}
			this.m_laserPartBack.AdjustHighlight(gameObject2, barrierCenterPos, endPos2, false);
		}
		else
		{
			gameObject2.SetActiveIfNeeded(false);
		}
		if (flag)
		{
			base.HideAllSquareIndicators();
		}
		if (this.m_laserRangeFront <= 0f)
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
			if (this.m_laserRangeBack <= 0f)
			{
				return;
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
		}
		this.m_highlights[0].gameObject.SetActiveIfNeeded(false);
	}
}
