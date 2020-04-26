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

	public bool LasersMetLastUpdate
	{
		get;
		set;
	}

	public Vector3 LaserMeetPosLastUpdate
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_ScampDualLasers(Ability ability, float laserWidth, float minDistFromCaster, float maxDistFromCaster, float laserStartForwardOffset, float laserStartSideOffset, float aoeBaseRadius, float aoeMinRadius, float aoeMaxRadius, float aoeRadiusChangePerUnitFromMin, float aoeRadiusMultIfPartialBlock, bool aoeIgnoreMinCoverDist)
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
		GameObject gameObject = m_highlights[2];
		List<SquareInsideChecker_Box> list = new List<SquareInsideChecker_Box>();
		list.Add(m_squarePosCheckerList[0] as SquareInsideChecker_Box);
		list.Add(m_squarePosCheckerList[1] as SquareInsideChecker_Box);
		SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[2] as SquareInsideChecker_Cone;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		int num = 2;
		if (m_delegateLaserCount != null)
		{
			num = m_delegateLaserCount(currentTarget, targetingActor);
		}
		List<Vector3> list2;
		if (num > 1)
		{
			list2 = AbilityCommon_DualMeetingLasers.CalcStartingPositions(travelBoardSquareWorldPositionForLos, currentTarget.FreePos, m_laserStartForwardOffset, m_laserStartSideOffset);
		}
		else
		{
			List<Vector3> list3 = new List<Vector3>();
			list3.Add(travelBoardSquareWorldPositionForLos);
			list2 = list3;
		}
		Vector3 vector = AbilityCommon_DualMeetingLasers.CalcClampedMeetingPos(travelBoardSquareWorldPositionForLos, currentTarget.FreePos, m_minMeetingDistFromCaster, m_maxMeetingDistFromCaster);
		float finalRadius = AbilityCommon_DualMeetingLasers.CalcAoeRadius(travelBoardSquareWorldPositionForLos, vector, m_aoeBaseRadius, m_minMeetingDistFromCaster, m_aoeRadiusChangePerUnitFromMin, m_aoeMinRadius, m_aoeMaxRadius);
		if (m_delegateExtraAoeRadius != null)
		{
			finalRadius += m_delegateExtraAoeRadius(currentTarget, targetingActor, m_aoeBaseRadius);
		}
		int aoeEndPosIndex = -1;
		AbilityCommon_DualMeetingLasers.CalcHitActors(vector, list2, m_laserWidth, finalRadius, m_aoeRadiusMultIfPartialBlock, targetingActor, GetAffectedTeams(targetingActor), false, null, out List<List<ActorData>> laserHitActorsList, out List<Vector3> laserEndPosList, out aoeEndPosIndex, out finalRadius, out List<ActorData> aoeHitActors);
		Vector3 vector2 = vector;
		if (list2.Count < 2)
		{
			vector2 = laserEndPosList[0];
		}
		float value = AbilityCommon_DualMeetingLasers.CalcMeetingPosDistFromMin(targetingActor.GetTravelBoardSquareWorldPositionForLos(), vector, m_minMeetingDistFromCaster);
		if (aoeEndPosIndex >= 0)
		{
			using (List<ActorData>.Enumerator enumerator = aoeHitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, vector2, targetingActor);
					if (m_aoeIgnoreMinCoverDist)
					{
						SetIgnoreCoverMinDist(current, m_aoeIgnoreMinCoverDist);
					}
					ActorHitContext actorHitContext = m_actorContextVars[current];
					actorHitContext._0015.SetFloat(ContextKeys._0013.GetHash(), value);
					actorHitContext._0015.SetInt(ContextKeys._001A.GetHash(), 1);
				}
			}
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject, finalRadius, 360f);
			Vector3 position = vector2;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = position;
			gameObject.SetActive(true);
			LasersMetLastUpdate = true;
			LaserMeetPosLastUpdate = vector2;
			squareInsideChecker_Cone.UpdateConeProperties(vector2, 360f, finalRadius, 0f, 0f, targetingActor);
		}
		else
		{
			gameObject.SetActive(false);
		}
		for (int i = 0; i < laserHitActorsList.Count; i++)
		{
			using (List<ActorData>.Enumerator enumerator2 = laserHitActorsList[i].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					AddActorInRange(current2, list2[i], targetingActor, AbilityTooltipSubject.Secondary);
					if (!aoeHitActors.Contains(current2))
					{
						ActorHitContext actorHitContext2 = m_actorContextVars[current2];
						actorHitContext2._0015.SetFloat(ContextKeys._0013.GetHash(), value);
						actorHitContext2._0015.SetInt(ContextKeys._001A.GetHash(), 0);
					}
				}
			}
		}
		while (true)
		{
			for (int j = 0; j < 2; j++)
			{
				if (j < list2.Count)
				{
					m_laserPart.AdjustHighlight(m_highlights[j], list2[j], laserEndPosList[j], false);
					list[j].UpdateBoxProperties(list2[j], laserEndPosList[j], targetingActor);
					m_highlights[j].SetActiveIfNeeded(true);
				}
				else
				{
					m_highlights[j].SetActiveIfNeeded(false);
					list[j].UpdateBoxProperties(list2[0], laserEndPosList[0], targetingActor);
				}
			}
			while (true)
			{
				ResetSquareIndicatorIndexToUse();
				for (int k = 0; k < list2.Count; k++)
				{
					Vector3 startPos = list2[k];
					Vector3 endPos = laserEndPosList[k];
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, m_laserWidth, targetingActor, false, null, m_squarePosCheckerList);
				}
				while (true)
				{
					if (aoeEndPosIndex >= 0)
					{
						AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, vector2, 0f, 360f, finalRadius, 0f, targetingActor, false, m_squarePosCheckerList);
					}
					HideUnusedSquareIndicators();
					return;
				}
			}
		}
	}
}
