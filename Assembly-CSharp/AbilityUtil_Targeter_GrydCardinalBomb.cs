using System;
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

	public AbilityUtil_Targeter_GrydCardinalBomb(Ability ability, float maxTrunkDist, float maxBranchDist, bool splitOnWall, bool splitOnActor, bool continueAfterActorHit, int maxSplits) : base(ability)
	{
		this.m_maxTrunkDist = maxTrunkDist;
		this.m_maxBranchDist = maxBranchDist;
		this.m_splitOnWall = splitOnWall;
		this.m_splitOnActor = splitOnActor;
		this.m_trunkContinueAfterActorHit = continueAfterActorHit;
		this.m_maxNumSplits = maxSplits;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.m_actorToHitContext.Clear();
		List<GrydCardinalSegmentInfo> list = new List<GrydCardinalSegmentInfo>();
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		int num = 0;
		if (boardSquare != null)
		{
			list.Add(new GrydCardinalSegmentInfo(boardSquare, Vector3.forward));
			list.Add(new GrydCardinalSegmentInfo(boardSquare, Vector3.back));
			list.Add(new GrydCardinalSegmentInfo(boardSquare, Vector3.left));
			list.Add(new GrydCardinalSegmentInfo(boardSquare, Vector3.right));
			ActorData actorData = AreaEffectUtils.GetTargetableActorOnSquare(boardSquare, true, false, targetingActor);
			if (actorData != null && !actorData.\u0018())
			{
				actorData = null;
			}
			List<ActorData> list2 = new List<ActorData>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].m_hitActorsMap = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
				list2.Clear();
				GrydCardinalSegmentInfo.CalculateSegmentInfo(list[i], this.m_maxTrunkDist, this.m_maxBranchDist, this.m_maxNumSplits, this.m_splitOnWall, this.m_splitOnActor, this.m_trunkContinueAfterActorHit, false, 0, targetingActor, targetingActor.\u0015(), null, list[i].m_hitActorsMap, list2);
				GrydCardinalSegmentInfo.HandleTargeterHighlights(list[i], false, this.m_highlights, ref num);
				list[i].TrackActorHitInfo(this.m_actorToHitContext);
			}
			if (actorData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_GrydCardinalBomb.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
				}
				if (this.m_actorToHitContext.ContainsKey(actorData))
				{
					this.m_actorToHitContext[actorData].m_numHits++;
				}
				else
				{
					ActorMultiHitContext actorMultiHitContext = new ActorMultiHitContext();
					actorMultiHitContext.m_numHits = 1;
					actorMultiHitContext.m_numHitsFromCover = 0;
					actorMultiHitContext.m_hitOrigin = actorData.\u0016();
					this.m_actorToHitContext[actorData] = actorMultiHitContext;
				}
			}
			foreach (KeyValuePair<ActorData, ActorMultiHitContext> keyValuePair in this.m_actorToHitContext)
			{
				base.AddActorInRange(keyValuePair.Key, keyValuePair.Value.m_hitOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		for (int j = num; j < this.m_highlights.Count; j++)
		{
			this.m_highlights[j].SetActive(false);
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
