using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_Laser : AbilityUtil_Targeter
{
	public float m_width = 1f;

	public float m_distance = 15f;

	public bool m_penetrateLoS;

	public int m_maxTargets = -1;

	private bool m_lengthIgnoreWorldGeo;

	protected Vector3 m_lastCalculatedLaserEndPos;

	public AbilityUtil_Targeter_Laser.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_Laser.MaxTargetsDelegate m_customMaxTargetsDelegate;

	public AbilityUtil_Targeter_Laser.ClampedAimDirectionDelegate m_getClampedAimDirection;

	private List<AbilityUtil_Targeter_Laser.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_Laser.HitActorContext>();

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_Laser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false) : base(ability)
	{
		this.m_width = width;
		this.m_distance = distance;
		this.m_penetrateLoS = penetrateLoS;
		this.m_maxTargets = maxTargets;
		this.m_affectsAllies = affectsAllies;
		base.SetAffectedGroups(true, this.m_affectsAllies, affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_laserPart = new TargeterPart_Laser(this.m_width, this.m_distance, this.m_penetrateLoS, this.m_maxTargets);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public AbilityUtil_Targeter_Laser(Ability ability, LaserTargetingInfo laserTargetingInfo) : base(ability)
	{
		this.m_width = laserTargetingInfo.width;
		this.m_distance = laserTargetingInfo.range;
		this.m_penetrateLoS = laserTargetingInfo.penetrateLos;
		this.m_maxTargets = laserTargetingInfo.maxTargets;
		this.m_affectsEnemies = laserTargetingInfo.affectsEnemies;
		this.m_affectsAllies = laserTargetingInfo.affectsAllies;
		base.SetAffectedGroups(this.m_affectsEnemies, this.m_affectsAllies, laserTargetingInfo.affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_laserPart = new TargeterPart_Laser(this.m_width, this.m_distance, this.m_penetrateLoS, this.m_maxTargets);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public virtual float GetWidth()
	{
		return this.m_width;
	}

	public virtual float GetDistance()
	{
		return this.m_distance;
	}

	public virtual bool GetPenetrateLoS()
	{
		return this.m_penetrateLoS;
	}

	public virtual int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public bool LengthIgnoreWorldGeo
	{
		get
		{
			return this.m_lengthIgnoreWorldGeo;
		}
		set
		{
			this.m_lengthIgnoreWorldGeo = value;
		}
	}

	public Vector3 GetLastLaserEndPos()
	{
		return this.m_lastCalculatedLaserEndPos;
	}

	public List<AbilityUtil_Targeter_Laser.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext;
	}

	public virtual Vector3 GetStartLosPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return targetingActor.GetTravelBoardSquareWorldPositionForLos();
	}

	public virtual Vector3 GetAimDirection(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return currentTarget.AimDirection;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		int maxTargets = this.GetMaxTargets();
		if (this.m_customMaxTargetsDelegate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Laser.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			maxTargets = this.m_customMaxTargetsDelegate(targetingActor);
		}
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = this.GetStartLosPos(currentTarget, targetingActor);
		Vector3 vector = this.GetAimDirection(currentTarget, targetingActor);
		if (currentTargetIndex > 0 && this.m_getClampedAimDirection != null)
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
			vector = this.m_getClampedAimDirection(vector, targets[currentTargetIndex - 1].AimDirection);
		}
		this.m_laserPart.UpdateDimensions(this.GetWidth(), this.GetDistance());
		this.m_laserPart.m_maxTargets = maxTargets;
		this.m_laserPart.m_lengthIgnoreWorldGeo = this.LengthIgnoreWorldGeo;
		List<ActorData> hitActors = this.m_laserPart.GetHitActors(laserCoords.start, vector, targetingActor, base.GetAffectedTeams(), out laserCoords.end);
		this.m_lastCalculatedLaserEndPos = laserCoords.end;
		if (hitActors.Contains(targetingActor))
		{
			hitActors.Remove(targetingActor);
		}
		if (base.Highlight == null)
		{
			base.Highlight = this.m_laserPart.CreateHighlightObject(this);
		}
		this.m_laserPart.AdjustHighlight(base.Highlight, laserCoords.start, laserCoords.end, true);
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float squareSize = Board.Get().squareSize;
		using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				base.AddActorInRange(actorData, laserCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
				list.Add(actorData);
				float num2 = (actorData.GetTravelBoardSquareWorldPosition() - travelBoardSquareWorldPosition).magnitude / squareSize;
				AbilityUtil_Targeter_Laser.HitActorContext item;
				item.actor = actorData;
				item.hitOrderIndex = num;
				item.squaresFromCaster = num2;
				this.m_hitActorContext.Add(item);
				ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
				actorHitContext.\u001D = laserCoords.start;
				actorHitContext.\u0015.SetInt(ContextKeys.\u0011.GetHash(), num);
				actorHitContext.\u0015.SetFloat(ContextKeys.\u0018.GetHash(), num2);
				num++;
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
		if (this.m_affectsTargetingActor)
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
			if (this.m_affectCasterDelegate != null)
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
				if (!this.m_affectCasterDelegate(targetingActor, list))
				{
					goto IL_2ED;
				}
			}
			base.AddActorInRange(targetingActor, laserCoords.start, targetingActor, AbilityTooltipSubject.Secondary, false);
			AbilityUtil_Targeter_Laser.HitActorContext item2;
			item2.actor = targetingActor;
			item2.hitOrderIndex = num;
			item2.squaresFromCaster = 0f;
			this.m_hitActorContext.Add(item2);
		}
		IL_2ED:
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
	}

	protected virtual void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Laser.DrawInvalidSquareIndicators(AbilityTarget, ActorData, Vector3, Vector3)).MethodHandle;
			}
			base.ResetSquareIndicatorIndexToUse();
			this.m_laserPart.ShowHiddenSquares(this.m_indicatorHandler, startPos, endPos, targetingActor, this.GetPenetrateLoS());
			base.HideUnusedSquareIndicators();
		}
	}

	public VectorUtils.LaserCoords CurrentLaserCoordinatesForGizmo(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 startLosPos = this.GetStartLosPos(currentTarget, targetingActor);
		Vector3 aimDirection = this.GetAimDirection(currentTarget, targetingActor);
		float maxDistanceInWorld = this.GetDistance() * Board.Get().squareSize;
		float widthInWorld = this.GetWidth() * Board.Get().squareSize;
		bool penetrateLoS = this.GetPenetrateLoS() || this.m_lengthIgnoreWorldGeo;
		VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(startLosPos, aimDirection, maxDistanceInWorld, widthInWorld, penetrateLoS, targetingActor, null);
		float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
		if (laserInitialOffsetInSquares > 0f)
		{
			laserCoordinates.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoordinates.start, laserCoordinates.end, laserInitialOffsetInSquares);
		}
		return laserCoordinates;
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		VectorUtils.LaserCoords laserCoords = this.CurrentLaserCoordinatesForGizmo(currentTarget, targetingActor);
		float widthInWorld = this.GetWidth() * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = laserCoords.start + new Vector3(0f, y, 0f);
		Vector3 vector2 = laserCoords.end + new Vector3(0f, y, 0f);
		TargeterUtils.DrawGizmo_LaserBox(vector, vector2, widthInWorld, Color.red);
		Gizmos.color = Color.yellow;
		List<BoardSquare> list = (!GameWideData.Get().UseActorRadiusForLaser()) ? AreaEffectUtils.GetSquaresInBox(vector, vector2, this.GetWidth() / 2f, this.GetPenetrateLoS(), targetingActor) : AreaEffectUtils.GetSquaresInBoxByActorRadius(vector, vector2, this.GetWidth(), this.GetPenetrateLoS(), targetingActor, null);
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.IsBaselineHeight())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Laser.DrawGizmos(AbilityTarget, ActorData)).MethodHandle;
					}
					Gizmos.DrawWireCube(boardSquare.ToVector3(), new Vector3(0.5f, 0.5f, 0.5f));
				}
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
		Gizmos.color = Color.white;
		List<BoardSquare> list2;
		if (GameWideData.Get().UseActorRadiusForLaser())
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
			list2 = AreaEffectUtils.GetSquaresInBoxByActorRadius(vector, vector2, this.GetWidth(), true, targetingActor, null);
		}
		else
		{
			list2 = AreaEffectUtils.GetSquaresInBox(vector, vector2, this.GetWidth() / 2f, true, targetingActor);
		}
		List<BoardSquare> list3 = list2;
		using (List<BoardSquare>.Enumerator enumerator2 = list3.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				BoardSquare boardSquare2 = enumerator2.Current;
				if (boardSquare2.IsBaselineHeight())
				{
					Gizmos.DrawWireSphere(boardSquare2.ToVector3(), 0.2f);
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
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate int MaxTargetsDelegate(ActorData caster);

	public delegate Vector3 ClampedAimDirectionDelegate(Vector3 currentAimDir, Vector3 prevAimDir);

	public struct HitActorContext
	{
		public ActorData actor;

		public int hitOrderIndex;

		public float squaresFromCaster;
	}
}
