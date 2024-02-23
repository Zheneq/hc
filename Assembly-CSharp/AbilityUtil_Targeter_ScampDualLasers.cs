using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScampDualLasers : AbilityUtil_Targeter
{
	public delegate int LaserCountDelegate(AbilityTarget currentTarget, ActorData targetingActor);
	public delegate float ExtraAoeRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius);

	private float m_laserWidth;
	private float m_minMeetingDistFromCaster;
	internal float m_maxMeetingDistFromCaster;
	private float m_laserStartForwardOffset;
	private float m_laserStartSideOffset;
	private float m_aoeBaseRadius;
	private float m_aoeMinRadius;
	private float m_aoeMaxRadius;
	private float m_aoeRadiusChangePerUnitFromMin;
	private float m_aoeRadiusMultIfPartialBlock;
	private bool m_aoeIgnoreMinCoverDist;
	private TargeterPart_Laser m_laserPart;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();
	public LaserCountDelegate m_delegateLaserCount;
	public ExtraAoeRadiusDelegate m_delegateExtraAoeRadius;

	public bool LasersMetLastUpdate { get; set; }

	public Vector3 LaserMeetPosLastUpdate { get; set; }

	public AbilityUtil_Targeter_ScampDualLasers(
		Ability ability,
		float laserWidth,
		float minDistFromCaster,
		float maxDistFromCaster,
		float laserStartForwardOffset,
		float laserStartSideOffset,
		float aoeBaseRadius,
		float aoeMinRadius,
		float aoeMaxRadius,
		float aoeRadiusChangePerUnitFromMin,
		float aoeRadiusMultIfPartialBlock,
		bool aoeIgnoreMinCoverDist)
		: base(ability)
	{
		m_laserWidth = laserWidth;
		m_minMeetingDistFromCaster = minDistFromCaster;
		m_maxMeetingDistFromCaster = maxDistFromCaster;
		m_laserStartForwardOffset = laserStartForwardOffset;
		m_laserStartSideOffset = laserStartSideOffset;
		m_aoeBaseRadius = aoeBaseRadius;
		m_aoeMinRadius = aoeMinRadius;
		m_aoeMaxRadius = aoeMaxRadius;
		m_aoeRadiusChangePerUnitFromMin = aoeRadiusChangePerUnitFromMin;
		m_aoeRadiusMultIfPartialBlock = aoeRadiusMultIfPartialBlock;
		m_aoeIgnoreMinCoverDist = aoeIgnoreMinCoverDist;
		m_laserPart = new TargeterPart_Laser(m_laserWidth, 1f, false, -1);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_laserWidth));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_laserWidth));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		LasersMetLastUpdate = false;
		if (m_highlights.Count < 3)
		{
			ClearHighlightCursors();
			m_highlights.Add(m_laserPart.CreateHighlightObject(this));
			m_highlights.Add(m_laserPart.CreateHighlightObject(this));
			m_highlights.Add(HighlightUtils.Get().CreateConeCursor(1f, 360f));
		}
		GameObject coneHighlight = m_highlights[2];
		List<SquareInsideChecker_Box> areaBoxes = new List<SquareInsideChecker_Box>
		{
			m_squarePosCheckerList[0] as SquareInsideChecker_Box,
			m_squarePosCheckerList[1] as SquareInsideChecker_Box
		};
		SquareInsideChecker_Cone areaCone = m_squarePosCheckerList[2] as SquareInsideChecker_Cone;
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		int num = 2;
		if (m_delegateLaserCount != null)
		{
			num = m_delegateLaserCount(currentTarget, targetingActor);
		}
		List<Vector3> laserStartPosList;
		if (num > 1)
		{
			laserStartPosList = AbilityCommon_DualMeetingLasers.CalcStartingPositions(
				losCheckPos,
				currentTarget.FreePos,
				m_laserStartForwardOffset,
				m_laserStartSideOffset);
		}
		else
		{
			laserStartPosList = new List<Vector3> { losCheckPos };
		}
		Vector3 targetPos = AbilityCommon_DualMeetingLasers.CalcClampedMeetingPos(
			losCheckPos,
			currentTarget.FreePos,
			m_minMeetingDistFromCaster,
			m_maxMeetingDistFromCaster);
		float aoeRadius = AbilityCommon_DualMeetingLasers.CalcAoeRadius(
			losCheckPos,
			targetPos,
			m_aoeBaseRadius,
			m_minMeetingDistFromCaster,
			m_aoeRadiusChangePerUnitFromMin,
			m_aoeMinRadius,
			m_aoeMaxRadius);
		if (m_delegateExtraAoeRadius != null)
		{
			aoeRadius += m_delegateExtraAoeRadius(currentTarget, targetingActor, m_aoeBaseRadius);
		}

		List<List<ActorData>> laserHitActorsList;
		List<Vector3> laserEndPosList;
		int aoeEndPosIndex;
		List<ActorData> aoeHitActors;
		AbilityCommon_DualMeetingLasers.CalcHitActors(
			targetPos,
			laserStartPosList,
			m_laserWidth,
			aoeRadius,
			m_aoeRadiusMultIfPartialBlock,
			targetingActor,
			GetAffectedTeams(targetingActor),
			false,
			null,
			out laserHitActorsList,
			out laserEndPosList,
			out aoeEndPosIndex,
			out aoeRadius,
			out aoeHitActors);
		Vector3 damageOrigin = laserStartPosList.Count < 2 ? laserEndPosList[0] : targetPos;
		float distFromMin = AbilityCommon_DualMeetingLasers.CalcMeetingPosDistFromMin(
			targetingActor.GetLoSCheckPos(), targetPos, m_minMeetingDistFromCaster);
		if (aoeEndPosIndex >= 0)
		{
			foreach (ActorData hitActor in aoeHitActors)
			{
				AddActorInRange(hitActor, damageOrigin, targetingActor);
				if (m_aoeIgnoreMinCoverDist)
				{
					SetIgnoreCoverMinDist(hitActor, m_aoeIgnoreMinCoverDist);
				}
				ActorHitContext actorHitContext = m_actorContextVars[hitActor];
				actorHitContext.m_contextVars.SetValue(ContextKeys.s_DistFromMin.GetKey(), distFromMin);
				actorHitContext.m_contextVars.SetValue(ContextKeys.s_InAoe.GetKey(), 1);
			}
			HighlightUtils.Get().AdjustDynamicConeMesh(coneHighlight, aoeRadius, 360f);
			Vector3 position = damageOrigin;
			position.y = HighlightUtils.GetHighlightHeight();
			coneHighlight.transform.position = position;
			coneHighlight.SetActive(true);
			LasersMetLastUpdate = true;
			LaserMeetPosLastUpdate = damageOrigin;
			areaCone.UpdateConeProperties(damageOrigin, 360f, aoeRadius, 0f, 0f, targetingActor);
		}
		else
		{
			coneHighlight.SetActive(false);
		}
		for (int i = 0; i < laserHitActorsList.Count; i++)
		{
			foreach (ActorData hitActor in laserHitActorsList[i])
			{
				AddActorInRange(hitActor, laserStartPosList[i], targetingActor, AbilityTooltipSubject.Secondary);
				if (!aoeHitActors.Contains(hitActor))
				{
					ActorHitContext actorHitContext = m_actorContextVars[hitActor];
					actorHitContext.m_contextVars.SetValue(ContextKeys.s_DistFromMin.GetKey(), distFromMin);
					actorHitContext.m_contextVars.SetValue(ContextKeys.s_InAoe.GetKey(), 0);
				}
			}
		}
		for (int i = 0; i < 2; i++)
		{
			if (i < laserStartPosList.Count)
			{
				m_laserPart.AdjustHighlight(m_highlights[i], laserStartPosList[i], laserEndPosList[i], false);
				areaBoxes[i].UpdateBoxProperties(laserStartPosList[i], laserEndPosList[i], targetingActor);
				m_highlights[i].SetActiveIfNeeded(true);
			}
			else
			{
				m_highlights[i].SetActiveIfNeeded(false);
				areaBoxes[i].UpdateBoxProperties(laserStartPosList[0], laserEndPosList[0], targetingActor);
			}
		}
		ResetSquareIndicatorIndexToUse();
		for (int i = 0; i < laserStartPosList.Count; i++)
		{
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				m_indicatorHandler,
				laserStartPosList[i],
				laserEndPosList[i],
				m_laserWidth,
				targetingActor,
				false,
				null,
				m_squarePosCheckerList);
		}
		if (aoeEndPosIndex >= 0)
		{
			AreaEffectUtils.OperateOnSquaresInCone(
				m_indicatorHandler,
				damageOrigin,
				0f,
				360f,
				aoeRadius,
				0f,
				targetingActor,
				false,
				m_squarePosCheckerList);
		}
		HideUnusedSquareIndicators();
	}
}
