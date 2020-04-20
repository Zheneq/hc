using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TricksterLaser : AbilityUtil_Targeter
{
	private float m_width = 1f;

	public float m_distance = 15f;

	private bool m_penetrateLoS;

	private int m_maxTargets = -1;

	private int m_maxLasers;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	public Dictionary<ActorData, int> m_actorToHitCount = new Dictionary<ActorData, int>();

	public Dictionary<ActorData, int> m_actorToCoverCount = new Dictionary<ActorData, int>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_TricksterLaser(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, LaserTargetingInfo laserTargetingInfo, int maxAfterImages) : base(ability)
	{
		this.m_width = laserTargetingInfo.width;
		this.m_distance = laserTargetingInfo.range;
		this.m_penetrateLoS = laserTargetingInfo.penetrateLos;
		this.m_maxTargets = laserTargetingInfo.maxTargets;
		this.m_affectsAllies = laserTargetingInfo.affectsAllies;
		this.m_affectsTargetingActor = laserTargetingInfo.affectsCaster;
		this.m_afterImageSyncComp = syncComp;
		this.m_maxLasers = maxAfterImages + 1;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < this.m_maxLasers; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_width));
		}
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count == this.m_maxLasers * 2)
			{
				for (int i = this.m_maxLasers; i < this.m_highlights.Count; i++)
				{
					this.m_highlights[i].SetActive(false);
				}
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_maxLasers * 2)
			{
				goto IL_E2;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_maxLasers; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		}
		for (int j = 0; j < this.m_maxLasers; j++)
		{
			GameObject item = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, 0.3f, true, Color.cyan);
			this.m_highlights.Add(item);
		}
		IL_E2:
		bool flag;
		if (GameFlowData.Get() != null)
		{
			flag = (GameFlowData.Get().activeOwnedActorData == targetingActor);
		}
		else
		{
			flag = false;
		}
		bool active = flag;
		for (int k = 0; k < this.m_maxLasers; k++)
		{
			if (k < validAfterImages.Count + 1)
			{
				this.m_highlights[k].SetActive(true);
				this.m_highlights[this.m_maxLasers + k].SetActive(active);
			}
			else
			{
				this.m_highlights[k].SetActive(false);
				this.m_highlights[this.m_maxLasers + k].SetActive(false);
			}
		}
		List<ActorData> list = new List<ActorData>();
		list.Add(targetingActor);
		list.AddRange(validAfterImages);
		float widthInWorld = this.m_width * Board.Get().squareSize;
		Vector3 vector;
		Vector3 a;
		this.m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(currentTarget.FreePos, targetingActor, list, false, out vector, out a);
		Dictionary<ActorData, Vector3> dictionary = new Dictionary<ActorData, Vector3>();
		this.m_actorToHitCount.Clear();
		this.m_actorToCoverCount.Clear();
		List<VectorUtils.LaserCoords> list2 = new List<VectorUtils.LaserCoords>();
		int l = 0;
		while (l < list.Count)
		{
			ActorData actorData = list[l];
			Vector3 dir = a - list[l].GetTravelBoardSquareWorldPosition();
			dir.y = 0f;
			float magnitude = dir.magnitude;
			dir.Normalize();
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = actorData.GetTravelBoardSquareWorldPositionForLos();
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, dir, this.m_distance, this.m_width, actorData, base.GetAffectedTeams(), this.m_penetrateLoS, this.m_maxTargets, false, false, out laserCoords.end, null, null, false, true);
			list2.Add(laserCoords);
			VectorUtils.LaserCoords laserCoords2 = laserCoords;
			if (actorsInLaser.Contains(targetingActor))
			{
				actorsInLaser.Remove(targetingActor);
			}
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData2 = enumerator.Current;
					ActorCover actorCover = actorData2.GetActorCover();
					bool flag2 = actorCover.IsInCoverWrt(laserCoords2.start);
					if (dictionary.ContainsKey(actorData2))
					{
						Dictionary<ActorData, int> dictionary2;
						ActorData key;
						(dictionary2 = this.m_actorToHitCount)[key = actorData2] = dictionary2[key] + 1;
						Dictionary<ActorData, int> dictionary3 = dictionary2 = this.m_actorToCoverCount;
						ActorData key3;
						ActorData key2 = key3 = actorData2;
						int num = dictionary2[key3];
						int num2;
						if (flag2)
						{
							num2 = 1;
						}
						else
						{
							num2 = 0;
						}
						dictionary3[key2] = num + num2;
						if (actorCover != null && !actorCover.IsInCoverWrt(dictionary[actorData2]))
						{
							if (actorCover.IsInCoverWrt(laserCoords2.start))
							{
								dictionary[actorData2] = laserCoords2.start;
							}
						}
					}
					else
					{
						this.m_actorToHitCount[actorData2] = 1;
						Dictionary<ActorData, int> actorToCoverCount = this.m_actorToCoverCount;
						ActorData key4 = actorData2;
						int value;
						if (flag2)
						{
							value = 1;
						}
						else
						{
							value = 0;
						}
						actorToCoverCount[key4] = value;
						dictionary[actorData2] = laserCoords2.start;
					}
				}
			}
			if (this.m_affectsTargetingActor)
			{
				base.AddActorInRange(targetingActor, laserCoords2.start, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			float y = 0.1f - BoardSquare.s_LoSHeightOffset;
			float magnitude2 = (laserCoords2.end - laserCoords2.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude2, this.m_highlights[l]);
			Vector3 normalized = (laserCoords2.end - laserCoords2.start).normalized;
			Vector3 position = laserCoords2.start + new Vector3(0f, y, 0f);
			this.m_highlights[l].transform.position = position;
			this.m_highlights[l].transform.rotation = Quaternion.LookRotation(normalized);
			this.m_highlights[this.m_maxLasers + l].transform.position = position;
			this.m_highlights[this.m_maxLasers + l].transform.rotation = Quaternion.LookRotation(normalized);
			Color color;
			if (actorsInLaser.Count > 0)
			{
				color = Color.red;
			}
			else
			{
				color = Color.cyan;
			}
			Color color2 = color;
			HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_highlights[this.m_maxLasers + l], magnitude / Board.Get().squareSize, color2);
			if (l != list.Count - 1)
			{
				goto IL_627;
			}
			if (l >= this.m_squarePosCheckerList.Count - 1)
			{
				goto IL_627;
			}
			for (int m = l; m < this.m_squarePosCheckerList.Count; m++)
			{
				SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[m] as SquareInsideChecker_Box;
				squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
			}
			IL_653:
			l++;
			continue;
			IL_627:
			SquareInsideChecker_Box squareInsideChecker_Box2 = this.m_squarePosCheckerList[l] as SquareInsideChecker_Box;
			squareInsideChecker_Box2.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
			goto IL_653;
		}
		foreach (KeyValuePair<ActorData, Vector3> keyValuePair in dictionary)
		{
			ActorData key5 = keyValuePair.Key;
			for (int n = 0; n < this.m_actorToHitCount[key5]; n++)
			{
				base.AddActorInRange(key5, keyValuePair.Value, targetingActor, AbilityTooltipSubject.Primary, true);
			}
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			int num3 = 0;
			while (num3 < list.Count)
			{
				if (num3 >= this.m_maxLasers)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_78A;
					}
				}
				else
				{
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, list2[num3].start, list2[num3].end, this.m_width, targetingActor, this.m_penetrateLoS, null, this.m_squarePosCheckerList, true);
					num3++;
				}
			}
			IL_78A:
			base.HideUnusedSquareIndicators();
		}
	}
}
