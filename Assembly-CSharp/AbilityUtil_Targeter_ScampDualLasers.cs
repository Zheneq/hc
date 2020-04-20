using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ScampDualLasers : AbilityUtil_Targeter
{
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

	public AbilityUtil_Targeter_ScampDualLasers.LaserCountDelegate m_delegateLaserCount;

	public AbilityUtil_Targeter_ScampDualLasers.ExtraAoeRadiusDelegate m_delegateExtraAoeRadius;

	public AbilityUtil_Targeter_ScampDualLasers(Ability ability, float laserWidth, float minDistFromCaster, float maxDistFromCaster, float laserStartForwardOffset, float laserStartSideOffset, float aoeBaseRadius, float aoeMinRadius, float aoeMaxRadius, float aoeRadiusChangePerUnitFromMin, float aoeRadiusMultIfPartialBlock, bool aoeIgnoreMinCoverDist) : base(ability)
	{
		this.m_laserWidth = laserWidth;
		this.m_minMeetingDistFromCaster = minDistFromCaster;
		this.m_maxMeetingDistFromCaster = maxDistFromCaster;
		this.m_laserStartForwardOffset = laserStartForwardOffset;
		this.m_laserStartSideOffset = laserStartSideOffset;
		this.m_aoeBaseRadius = aoeBaseRadius;
		this.m_aoeMinRadius = aoeMinRadius;
		this.m_aoeMaxRadius = aoeMaxRadius;
		this.m_aoeRadiusChangePerUnitFromMin = aoeRadiusChangePerUnitFromMin;
		this.m_aoeRadiusMultIfPartialBlock = aoeRadiusMultIfPartialBlock;
		this.m_aoeIgnoreMinCoverDist = aoeIgnoreMinCoverDist;
		this.m_laserPart = new TargeterPart_Laser(this.m_laserWidth, 1f, false, -1);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_laserWidth));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_laserWidth));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public bool LasersMetLastUpdate { get; set; }

	public Vector3 LaserMeetPosLastUpdate { get; set; }

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.LasersMetLastUpdate = false;
		if (this.m_highlights.Count < 3)
		{
			this.ClearHighlightCursors(true);
			this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
			this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
			this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(1f, 360f));
		}
		GameObject gameObject = this.m_highlights[2];
		List<SquareInsideChecker_Box> list = new List<SquareInsideChecker_Box>();
		list.Add(this.m_squarePosCheckerList[0] as SquareInsideChecker_Box);
		list.Add(this.m_squarePosCheckerList[1] as SquareInsideChecker_Box);
		SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[2] as SquareInsideChecker_Cone;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		int num = 2;
		if (this.m_delegateLaserCount != null)
		{
			num = this.m_delegateLaserCount(currentTarget, targetingActor);
		}
		List<Vector3> list2;
		if (num > 1)
		{
			list2 = AbilityCommon_DualMeetingLasers.CalcStartingPositions(travelBoardSquareWorldPositionForLos, currentTarget.FreePos, this.m_laserStartForwardOffset, this.m_laserStartSideOffset);
		}
		else
		{
			list2 = new List<Vector3>
			{
				travelBoardSquareWorldPositionForLos
			};
		}
		Vector3 vector = AbilityCommon_DualMeetingLasers.CalcClampedMeetingPos(travelBoardSquareWorldPositionForLos, currentTarget.FreePos, this.m_minMeetingDistFromCaster, this.m_maxMeetingDistFromCaster);
		float num2 = AbilityCommon_DualMeetingLasers.CalcAoeRadius(travelBoardSquareWorldPositionForLos, vector, this.m_aoeBaseRadius, this.m_minMeetingDistFromCaster, this.m_aoeRadiusChangePerUnitFromMin, this.m_aoeMinRadius, this.m_aoeMaxRadius);
		if (this.m_delegateExtraAoeRadius != null)
		{
			num2 += this.m_delegateExtraAoeRadius(currentTarget, targetingActor, this.m_aoeBaseRadius);
		}
		int num3 = -1;
		List<List<ActorData>> list3;
		List<Vector3> list4;
		List<ActorData> list5;
		AbilityCommon_DualMeetingLasers.CalcHitActors(vector, list2, this.m_laserWidth, num2, this.m_aoeRadiusMultIfPartialBlock, targetingActor, base.GetAffectedTeams(targetingActor), false, null, out list3, out list4, out num3, out num2, out list5);
		Vector3 vector2 = vector;
		if (list2.Count < 2)
		{
			vector2 = list4[0];
		}
		float value = AbilityCommon_DualMeetingLasers.CalcMeetingPosDistFromMin(targetingActor.GetTravelBoardSquareWorldPositionForLos(), vector, this.m_minMeetingDistFromCaster);
		if (num3 >= 0)
		{
			using (List<ActorData>.Enumerator enumerator = list5.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					base.AddActorInRange(actorData, vector2, targetingActor, AbilityTooltipSubject.Primary, false);
					if (this.m_aoeIgnoreMinCoverDist)
					{
						base.SetIgnoreCoverMinDist(actorData, this.m_aoeIgnoreMinCoverDist);
					}
					ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
					actorHitContext.symbol_0015.SetFloat(ContextKeys.symbol_0013.GetHash(), value);
					actorHitContext.symbol_0015.SetInt(ContextKeys.symbol_001A.GetHash(), 1);
				}
			}
			HighlightUtils.Get().AdjustDynamicConeMesh(gameObject, num2, 360f);
			Vector3 position = vector2;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = position;
			gameObject.SetActive(true);
			this.LasersMetLastUpdate = true;
			this.LaserMeetPosLastUpdate = vector2;
			squareInsideChecker_Cone.UpdateConeProperties(vector2, 360f, num2, 0f, 0f, targetingActor);
		}
		else
		{
			gameObject.SetActive(false);
		}
		for (int i = 0; i < list3.Count; i++)
		{
			using (List<ActorData>.Enumerator enumerator2 = list3[i].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					base.AddActorInRange(actorData2, list2[i], targetingActor, AbilityTooltipSubject.Secondary, false);
					if (!list5.Contains(actorData2))
					{
						ActorHitContext actorHitContext2 = this.m_actorContextVars[actorData2];
						actorHitContext2.symbol_0015.SetFloat(ContextKeys.symbol_0013.GetHash(), value);
						actorHitContext2.symbol_0015.SetInt(ContextKeys.symbol_001A.GetHash(), 0);
					}
				}
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (j < list2.Count)
			{
				this.m_laserPart.AdjustHighlight(this.m_highlights[j], list2[j], list4[j], false);
				list[j].UpdateBoxProperties(list2[j], list4[j], targetingActor);
				this.m_highlights[j].SetActiveIfNeeded(true);
			}
			else
			{
				this.m_highlights[j].SetActiveIfNeeded(false);
				list[j].UpdateBoxProperties(list2[0], list4[0], targetingActor);
			}
		}
		base.ResetSquareIndicatorIndexToUse();
		for (int k = 0; k < list2.Count; k++)
		{
			Vector3 startPos = list2[k];
			Vector3 endPos = list4[k];
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, this.m_laserWidth, targetingActor, false, null, this.m_squarePosCheckerList, true);
		}
		if (num3 >= 0)
		{
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, vector2, 0f, 360f, num2, 0f, targetingActor, false, this.m_squarePosCheckerList);
		}
		base.HideUnusedSquareIndicators();
	}

	public delegate int LaserCountDelegate(AbilityTarget currentTarget, ActorData targetingActor);

	public delegate float ExtraAoeRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius);
}
