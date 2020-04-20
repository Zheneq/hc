using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AoE_Smooth_FixedOffset : AbilityUtil_Targeter_AoE_Smooth
{
	public float m_minOffsetFromCaster;

	public float m_maxOffsetFromCaster;

	private float m_knockbackDist;

	private float m_connectLaserWidth;

	private TargeterPart_Laser m_laserPart;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private SquareInsideChecker_Box m_laserChecker;

	private AbilityUtil_Targeter_AoE_Smooth_FixedOffset.SquareInsideChecker_AoeFixedOffset m_coneChecker;

	public AbilityUtil_Targeter_AoE_Smooth_FixedOffset.IsSquareInLosDelegate m_delegateIsSquareInLos;

	public AbilityUtil_Targeter_AoE_Smooth_FixedOffset(Ability ability, float minOffsetFromCaster, float maxOffsetFromCaster, float radius, bool penetrateLoS, float knockbackDist, KnockbackType knockbackType, float connectLaserWidth, bool affectsEnemies = true, bool affectsAllies = false, int maxTargets = -1) : base(ability, radius, penetrateLoS, affectsEnemies, affectsAllies, maxTargets)
	{
		this.m_minOffsetFromCaster = minOffsetFromCaster;
		this.m_maxOffsetFromCaster = maxOffsetFromCaster;
		this.m_knockbackDist = knockbackDist;
		this.m_knockbackType = knockbackType;
		this.m_connectLaserWidth = connectLaserWidth;
		this.m_laserPart = new TargeterPart_Laser(this.m_connectLaserWidth, 1f, false, -1);
		this.m_coneChecker = new AbilityUtil_Targeter_AoE_Smooth_FixedOffset.SquareInsideChecker_AoeFixedOffset(this);
		this.m_laserChecker = new SquareInsideChecker_Box(this.m_connectLaserWidth);
		this.m_squarePosCheckerList.Add(this.m_coneChecker);
		if (this.m_connectLaserWidth > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset..ctor(Ability, float, float, float, bool, float, KnockbackType, float, bool, bool, int)).MethodHandle;
			}
			this.m_squarePosCheckerList.Add(this.m_laserChecker);
		}
	}

	public static Vector3 GetClampedFreePos(Vector3 freePos, ActorData caster, float minDistInSquares, float maxDistInSquares)
	{
		float squareSize = Board.Get().squareSize;
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector = freePos - travelBoardSquareWorldPositionForLos;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num = minDistInSquares * squareSize;
		float num2 = maxDistInSquares * squareSize;
		if (magnitude < num)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset.GetClampedFreePos(Vector3, ActorData, float, float)).MethodHandle;
			}
			return travelBoardSquareWorldPositionForLos + vector.normalized * num;
		}
		if (magnitude > num2)
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
			return travelBoardSquareWorldPositionForLos + vector.normalized * num2;
		}
		return freePos;
	}

	protected override Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return AbilityUtil_Targeter_AoE_Smooth_FixedOffset.GetClampedFreePos(currentTarget.FreePos, targetingActor, this.m_minOffsetFromCaster, this.m_maxOffsetFromCaster);
	}

	protected override Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return targetingActor.GetTravelBoardSquareWorldPositionForLos();
	}

	public override void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		base.CreateHighlightObjectsIfNeeded(radiusInSquares, targetingActor);
		if (this.m_connectLaserWidth > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset.CreateHighlightObjectsIfNeeded(float, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count < 2)
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
				this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
		if (this.m_knockbackDist > 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			int num = 0;
			base.EnableAllMovementArrows();
			Vector3 refPos = this.GetRefPos(currentTarget, targetingActor, 0f);
			using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, refPos, this.m_knockbackDist);
					num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
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
			base.SetMovementArrowEnabledFromIndex(num, false);
		}
		Vector3 refPos2 = this.GetRefPos(currentTarget, targetingActor, 0f);
		Vector3 laserEnd = refPos2;
		if (this.m_connectLaserWidth > 0f)
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
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vector = refPos2;
			vector.y = travelBoardSquareWorldPositionForLos.y;
			Vector3 dir = vector - travelBoardSquareWorldPositionForLos;
			float num2 = dir.magnitude / Board.SquareSizeStatic;
			float num3 = num2 - this.m_radius;
			laserEnd = travelBoardSquareWorldPositionForLos + Board.SquareSizeStatic * num3 * dir.normalized;
			if (num3 > 0f)
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
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, dir, num3, this.m_connectLaserWidth, targetingActor, base.GetAffectedTeams(), false, -1, true, false, out vector, null, null, false, true);
				using (List<ActorData>.Enumerator enumerator2 = actorsInLaser.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData actorData2 = enumerator2.Current;
						if (!visibleActorsInRange.Contains(actorData2))
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
							base.AddActorInRange(actorData2, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Secondary, false);
						}
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
				this.m_laserPart.AdjustHighlight(this.m_highlights[1], travelBoardSquareWorldPositionForLos, vector, false);
				this.m_highlights[1].SetActiveIfNeeded(true);
			}
			else
			{
				this.m_highlights[1].SetActiveIfNeeded(false);
			}
		}
		this.CustomHandleHiddenSquareIndicators(targetingActor, refPos2, targetingActor.GetTravelBoardSquareWorldPositionForLos(), laserEnd);
	}

	protected override void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
	}

	private void CustomHandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos, Vector3 laserStart, Vector3 laserEnd)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset.CustomHandleHiddenSquareIndicators(ActorData, Vector3, Vector3, Vector3)).MethodHandle;
			}
			this.m_laserChecker.UpdateBoxProperties(laserStart, laserEnd, targetingActor);
			this.m_coneChecker.UpdateConeProperties(centerPos, 360f, this.m_radius, 0f, 0f, targetingActor);
			base.ResetSquareIndicatorIndexToUse();
			if (this.m_connectLaserWidth > 0f)
			{
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, laserStart, laserEnd, this.m_connectLaserWidth, targetingActor, this.GetPenetrateLoS(), null, this.m_squarePosCheckerList, true);
			}
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, centerPos, 0f, 360f, this.m_radius, 0f, targetingActor, this.GetPenetrateLoS(), this.m_squarePosCheckerList);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate bool IsSquareInLosDelegate(BoardSquare testSquare, Vector3 aoeCenterPos, ActorData caster);

	public class SquareInsideChecker_AoeFixedOffset : SquareInsideChecker_Cone
	{
		private AbilityUtil_Targeter_AoE_Smooth_FixedOffset m_targeter;

		public SquareInsideChecker_AoeFixedOffset(AbilityUtil_Targeter_AoE_Smooth_FixedOffset targeter)
		{
			this.m_targeter = targeter;
		}

		public unsafe override bool IsSquareInside(BoardSquare square, out bool inLos)
		{
			bool flag = base.IsSquareInside(square, out inLos);
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth_FixedOffset.SquareInsideChecker_AoeFixedOffset.IsSquareInside(BoardSquare, bool*)).MethodHandle;
				}
				if (this.m_targeter.m_delegateIsSquareInLos != null)
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
					inLos = this.m_targeter.m_delegateIsSquareInLos(square, this.m_coneStart, this.m_caster);
				}
			}
			return flag;
		}
	}
}
