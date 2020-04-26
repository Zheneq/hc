using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GrydCardinalBomb : AbilityUtil_Targeter
{
	private float m_maxTrunkDist;

	private float m_maxBranchDist;

	private bool m_splitOnWall;

	private bool m_splitOnActor;

	private bool m_trunkContinueAfterActorHit;

	private int m_maxNumSplits;

	public Dictionary<ActorData, ActorMultiHitContext> m_actorToHitContext = new Dictionary<ActorData, ActorMultiHitContext>();

	public AbilityUtil_Targeter_GrydCardinalBomb(Ability ability, float maxTrunkDist, float maxBranchDist, bool splitOnWall, bool splitOnActor, bool continueAfterActorHit, int maxSplits)
		: base(ability)
	{
		m_maxTrunkDist = maxTrunkDist;
		m_maxBranchDist = maxBranchDist;
		m_splitOnWall = splitOnWall;
		m_splitOnActor = splitOnActor;
		m_trunkContinueAfterActorHit = continueAfterActorHit;
		m_maxNumSplits = maxSplits;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		m_actorToHitContext.Clear();
		List<GrydCardinalSegmentInfo> list = new List<GrydCardinalSegmentInfo>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		int nextHighlightIndex = 0;
		if (boardSquareSafe != null)
		{
			list.Add(new GrydCardinalSegmentInfo(boardSquareSafe, Vector3.forward));
			list.Add(new GrydCardinalSegmentInfo(boardSquareSafe, Vector3.back));
			list.Add(new GrydCardinalSegmentInfo(boardSquareSafe, Vector3.left));
			list.Add(new GrydCardinalSegmentInfo(boardSquareSafe, Vector3.right));
			ActorData actorData = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, targetingActor);
			if (actorData != null && !actorData.IsVisibleToClient())
			{
				actorData = null;
			}
			List<ActorData> list2 = new List<ActorData>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].m_hitActorsMap = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
				list2.Clear();
				GrydCardinalSegmentInfo.CalculateSegmentInfo(list[i], m_maxTrunkDist, m_maxBranchDist, m_maxNumSplits, m_splitOnWall, m_splitOnActor, m_trunkContinueAfterActorHit, false, 0, targetingActor, targetingActor.GetOpposingTeams(), null, list[i].m_hitActorsMap, list2);
				GrydCardinalSegmentInfo.HandleTargeterHighlights(list[i], false, m_highlights, ref nextHighlightIndex);
				list[i].TrackActorHitInfo(m_actorToHitContext);
			}
			if (actorData != null)
			{
				if (m_actorToHitContext.ContainsKey(actorData))
				{
					m_actorToHitContext[actorData].m_numHits++;
				}
				else
				{
					ActorMultiHitContext actorMultiHitContext = new ActorMultiHitContext();
					actorMultiHitContext.m_numHits = 1;
					actorMultiHitContext.m_numHitsFromCover = 0;
					actorMultiHitContext.m_hitOrigin = actorData.GetTravelBoardSquareWorldPosition();
					m_actorToHitContext[actorData] = actorMultiHitContext;
				}
			}
			foreach (KeyValuePair<ActorData, ActorMultiHitContext> item in m_actorToHitContext)
			{
				AddActorInRange(item.Key, item.Value.m_hitOrigin, targetingActor);
			}
		}
		for (int j = nextHighlightIndex; j < m_highlights.Count; j++)
		{
			m_highlights[j].SetActive(false);
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
