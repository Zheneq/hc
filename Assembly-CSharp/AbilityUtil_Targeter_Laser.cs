using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Laser : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate int MaxTargetsDelegate(ActorData caster);

	public delegate Vector3 ClampedAimDirectionDelegate(Vector3 currentAimDir, Vector3 prevAimDir);

	public struct HitActorContext
	{
		public ActorData actor;

		public int hitOrderIndex;

		public float squaresFromCaster;
	}

	public float m_width = 1f;

	public float m_distance = 15f;

	public bool m_penetrateLoS;

	public int m_maxTargets = -1;

	private bool m_lengthIgnoreWorldGeo;

	protected Vector3 m_lastCalculatedLaserEndPos;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	public MaxTargetsDelegate m_customMaxTargetsDelegate;

	public ClampedAimDirectionDelegate m_getClampedAimDirection;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public bool LengthIgnoreWorldGeo
	{
		get
		{
			return m_lengthIgnoreWorldGeo;
		}
		set
		{
			m_lengthIgnoreWorldGeo = value;
		}
	}

	public AbilityUtil_Targeter_Laser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false)
		: base(ability)
	{
		m_width = width;
		m_distance = distance;
		m_penetrateLoS = penetrateLoS;
		m_maxTargets = maxTargets;
		m_affectsAllies = affectsAllies;
		SetAffectedGroups(true, m_affectsAllies, affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_laserPart = new TargeterPart_Laser(m_width, m_distance, m_penetrateLoS, m_maxTargets);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public AbilityUtil_Targeter_Laser(Ability ability, LaserTargetingInfo laserTargetingInfo)
		: base(ability)
	{
		m_width = laserTargetingInfo.width;
		m_distance = laserTargetingInfo.range;
		m_penetrateLoS = laserTargetingInfo.penetrateLos;
		m_maxTargets = laserTargetingInfo.maxTargets;
		m_affectsEnemies = laserTargetingInfo.affectsEnemies;
		m_affectsAllies = laserTargetingInfo.affectsAllies;
		SetAffectedGroups(m_affectsEnemies, m_affectsAllies, laserTargetingInfo.affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_laserPart = new TargeterPart_Laser(m_width, m_distance, m_penetrateLoS, m_maxTargets);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public virtual float GetWidth()
	{
		return m_width;
	}

	public virtual float GetDistance()
	{
		return m_distance;
	}

	public virtual bool GetPenetrateLoS()
	{
		return m_penetrateLoS;
	}

	public virtual int GetMaxTargets()
	{
		return m_maxTargets;
	}

	public Vector3 GetLastLaserEndPos()
	{
		return m_lastCalculatedLaserEndPos;
	}

	public List<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext;
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
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		m_hitActorContext.Clear();
		int maxTargets = GetMaxTargets();
		if (m_customMaxTargetsDelegate != null)
		{
			maxTargets = m_customMaxTargetsDelegate(targetingActor);
		}
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = GetStartLosPos(currentTarget, targetingActor);
		Vector3 vector = GetAimDirection(currentTarget, targetingActor);
		if (currentTargetIndex > 0 && m_getClampedAimDirection != null)
		{
			vector = m_getClampedAimDirection(vector, targets[currentTargetIndex - 1].AimDirection);
		}
		m_laserPart.UpdateDimensions(GetWidth(), GetDistance());
		m_laserPart.m_maxTargets = maxTargets;
		m_laserPart.m_lengthIgnoreWorldGeo = LengthIgnoreWorldGeo;
		List<ActorData> hitActors = m_laserPart.GetHitActors(laserCoords.start, vector, targetingActor, GetAffectedTeams(), out laserCoords.end);
		m_lastCalculatedLaserEndPos = laserCoords.end;
		if (hitActors.Contains(targetingActor))
		{
			hitActors.Remove(targetingActor);
		}
		if (base.Highlight == null)
		{
			base.Highlight = m_laserPart.CreateHighlightObject(this);
		}
		m_laserPart.AdjustHighlight(base.Highlight, laserCoords.start, laserCoords.end);
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float squareSize = Board.Get().squareSize;
		using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
		{
			HitActorContext item = default(HitActorContext);
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, laserCoords.start, targetingActor);
				list.Add(current);
				float num2 = (current.GetTravelBoardSquareWorldPosition() - travelBoardSquareWorldPosition).magnitude / squareSize;
				item.actor = current;
				item.hitOrderIndex = num;
				item.squaresFromCaster = num2;
				m_hitActorContext.Add(item);
				ActorHitContext actorHitContext = m_actorContextVars[current];
				actorHitContext.m_hitOrigin = laserCoords.start;
				actorHitContext.m_contextVars.SetValue(ContextKeys._0011.GetKey(), num);
				actorHitContext.m_contextVars.SetValue(ContextKeys._0018.GetKey(), num2);
				num++;
			}
		}
		if (m_affectsTargetingActor)
		{
			if (m_affectCasterDelegate != null)
			{
				if (!m_affectCasterDelegate(targetingActor, list))
				{
					goto IL_02ed;
				}
			}
			AddActorInRange(targetingActor, laserCoords.start, targetingActor, AbilityTooltipSubject.Secondary);
			HitActorContext item2 = default(HitActorContext);
			item2.actor = targetingActor;
			item2.hitOrderIndex = num;
			item2.squaresFromCaster = 0f;
			m_hitActorContext.Add(item2);
		}
		goto IL_02ed;
		IL_02ed:
		DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
	}

	protected virtual void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			m_laserPart.ShowHiddenSquares(m_indicatorHandler, startPos, endPos, targetingActor, GetPenetrateLoS());
			HideUnusedSquareIndicators();
			return;
		}
	}

	public VectorUtils.LaserCoords CurrentLaserCoordinatesForGizmo(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 startLosPos = GetStartLosPos(currentTarget, targetingActor);
		Vector3 aimDirection = GetAimDirection(currentTarget, targetingActor);
		float maxDistanceInWorld = GetDistance() * Board.Get().squareSize;
		float widthInWorld = GetWidth() * Board.Get().squareSize;
		bool penetrateLoS = GetPenetrateLoS() || m_lengthIgnoreWorldGeo;
		VectorUtils.LaserCoords laserCoordinates = VectorUtils.GetLaserCoordinates(startLosPos, aimDirection, maxDistanceInWorld, widthInWorld, penetrateLoS, targetingActor);
		float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
		if (laserInitialOffsetInSquares > 0f)
		{
			laserCoordinates.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoordinates.start, laserCoordinates.end, laserInitialOffsetInSquares);
		}
		return laserCoordinates;
	}

	public override void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		VectorUtils.LaserCoords laserCoords = CurrentLaserCoordinatesForGizmo(currentTarget, targetingActor);
		float widthInWorld = GetWidth() * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = laserCoords.start + new Vector3(0f, y, 0f);
		Vector3 vector2 = laserCoords.end + new Vector3(0f, y, 0f);
		TargeterUtils.DrawGizmo_LaserBox(vector, vector2, widthInWorld, Color.red);
		Gizmos.color = Color.yellow;
		List<BoardSquare> list = (!GameWideData.Get().UseActorRadiusForLaser()) ? AreaEffectUtils.GetSquaresInBox(vector, vector2, GetWidth() / 2f, GetPenetrateLoS(), targetingActor) : AreaEffectUtils.GetSquaresInBoxByActorRadius(vector, vector2, GetWidth(), GetPenetrateLoS(), targetingActor);
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.IsBaselineHeight())
				{
					Gizmos.DrawWireCube(current.ToVector3(), new Vector3(0.5f, 0.5f, 0.5f));
				}
			}
		}
		Gizmos.color = Color.white;
		List<BoardSquare> list2;
		if (GameWideData.Get().UseActorRadiusForLaser())
		{
			list2 = AreaEffectUtils.GetSquaresInBoxByActorRadius(vector, vector2, GetWidth(), true, targetingActor);
		}
		else
		{
			list2 = AreaEffectUtils.GetSquaresInBox(vector, vector2, GetWidth() / 2f, true, targetingActor);
		}
		List<BoardSquare> list3 = list2;
		using (List<BoardSquare>.Enumerator enumerator2 = list3.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				BoardSquare current2 = enumerator2.Current;
				if (current2.IsBaselineHeight())
				{
					Gizmos.DrawWireSphere(current2.ToVector3(), 0.2f);
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
