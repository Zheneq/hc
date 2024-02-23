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

	public AbilityUtil_Targeter_TricksterLaser(Ability ability, TricksterAfterImageNetworkBehaviour syncComp, LaserTargetingInfo laserTargetingInfo, int maxAfterImages)
		: base(ability)
	{
		m_width = laserTargetingInfo.width;
		m_distance = laserTargetingInfo.range;
		m_penetrateLoS = laserTargetingInfo.penetrateLos;
		m_maxTargets = laserTargetingInfo.maxTargets;
		m_affectsAllies = laserTargetingInfo.affectsAllies;
		m_affectsTargetingActor = laserTargetingInfo.affectsCaster;
		m_afterImageSyncComp = syncComp;
		m_maxLasers = maxAfterImages + 1;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < m_maxLasers; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_width));
		}
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			if (m_highlights.Count != m_maxLasers * 2)
			{
				return;
			}
			while (true)
			{
				for (int i = m_maxLasers; i < m_highlights.Count; i++)
				{
					m_highlights[i].SetActive(false);
				}
				return;
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_maxLasers * 2)
			{
				goto IL_00e2;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_maxLasers; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		}
		for (int j = 0; j < m_maxLasers; j++)
		{
			GameObject item = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, 0.3f, true, Color.cyan);
			m_highlights.Add(item);
		}
		goto IL_00e2;
		IL_00e2:
		int num;
		if (GameFlowData.Get() != null)
		{
			num = ((GameFlowData.Get().activeOwnedActorData == targetingActor) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool active = (byte)num != 0;
		for (int k = 0; k < m_maxLasers; k++)
		{
			if (k < validAfterImages.Count + 1)
			{
				m_highlights[k].SetActive(true);
				m_highlights[m_maxLasers + k].SetActive(active);
			}
			else
			{
				m_highlights[k].SetActive(false);
				m_highlights[m_maxLasers + k].SetActive(false);
			}
		}
		List<ActorData> list = new List<ActorData>();
		list.Add(targetingActor);
		list.AddRange(validAfterImages);
		float widthInWorld = m_width * Board.Get().squareSize;
		Vector3 foo;
		Vector3 freePosForAim;
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(currentTarget.FreePos, targetingActor, list, false, out foo, out freePosForAim);
		Dictionary<ActorData, Vector3> dictionary = new Dictionary<ActorData, Vector3>();
		m_actorToHitCount.Clear();
		m_actorToCoverCount.Clear();
		List<VectorUtils.LaserCoords> list2 = new List<VectorUtils.LaserCoords>();
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		for (int l = 0; l < list.Count; l++)
		{
			ActorData actorData = list[l];
			Vector3 dir = freePosForAim - list[l].GetFreePos();
			dir.y = 0f;
			float magnitude = dir.magnitude;
			dir.Normalize();
			laserCoords.start = actorData.GetLoSCheckPos();
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, dir, m_distance, m_width, actorData, GetAffectedTeams(), m_penetrateLoS, m_maxTargets, false, false, out laserCoords.end, null);
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
					ActorData current = enumerator.Current;
					ActorCover actorCover = current.GetActorCover();
					bool flag = actorCover.IsInCoverWrt(laserCoords2.start);
					if (dictionary.ContainsKey(current))
					{
						m_actorToHitCount[current]++;
						Dictionary<ActorData, int> actorToCoverCount;
						Dictionary<ActorData, int> dictionary2 = actorToCoverCount = m_actorToCoverCount;
						ActorData key;
						ActorData key2 = key = current;
						int num2 = actorToCoverCount[key];
						int num3;
						if (flag)
						{
							num3 = 1;
						}
						else
						{
							num3 = 0;
						}
						dictionary2[key2] = num2 + num3;
						if (actorCover != null && !actorCover.IsInCoverWrt(dictionary[current]))
						{
							if (actorCover.IsInCoverWrt(laserCoords2.start))
							{
								dictionary[current] = laserCoords2.start;
							}
						}
					}
					else
					{
						m_actorToHitCount[current] = 1;
						Dictionary<ActorData, int> actorToCoverCount2 = m_actorToCoverCount;
						int value;
						if (flag)
						{
							value = 1;
						}
						else
						{
							value = 0;
						}
						actorToCoverCount2[current] = value;
						dictionary[current] = laserCoords2.start;
					}
				}
			}
			if (m_affectsTargetingActor)
			{
				AddActorInRange(targetingActor, laserCoords2.start, targetingActor);
			}
			float y = 0.1f - BoardSquare.s_LoSHeightOffset;
			float magnitude2 = (laserCoords2.end - laserCoords2.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude2, m_highlights[l]);
			Vector3 normalized = (laserCoords2.end - laserCoords2.start).normalized;
			Vector3 position = laserCoords2.start + new Vector3(0f, y, 0f);
			m_highlights[l].transform.position = position;
			m_highlights[l].transform.rotation = Quaternion.LookRotation(normalized);
			m_highlights[m_maxLasers + l].transform.position = position;
			m_highlights[m_maxLasers + l].transform.rotation = Quaternion.LookRotation(normalized);
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
			HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_highlights[m_maxLasers + l], magnitude / Board.Get().squareSize, color2);
			if (l == list.Count - 1)
			{
				if (l < m_squarePosCheckerList.Count - 1)
				{
					for (int m = l; m < m_squarePosCheckerList.Count; m++)
					{
						SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[m] as SquareInsideChecker_Box;
						squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
					}
					continue;
				}
			}
			SquareInsideChecker_Box squareInsideChecker_Box2 = m_squarePosCheckerList[l] as SquareInsideChecker_Box;
			squareInsideChecker_Box2.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
		}
		foreach (KeyValuePair<ActorData, Vector3> item2 in dictionary)
		{
			ActorData key3 = item2.Key;
			for (int n = 0; n < m_actorToHitCount[key3]; n++)
			{
				AddActorInRange(key3, item2.Value, targetingActor, AbilityTooltipSubject.Primary, true);
			}
		}
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			for (int num4 = 0; num4 < list.Count; num4++)
			{
				if (num4 < m_maxLasers)
				{
					OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler = m_indicatorHandler;
					VectorUtils.LaserCoords laserCoords3 = list2[num4];
					Vector3 start = laserCoords3.start;
					VectorUtils.LaserCoords laserCoords4 = list2[num4];
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, start, laserCoords4.end, m_width, targetingActor, m_penetrateLoS, null, m_squarePosCheckerList);
					continue;
				}
				break;
			}
			HideUnusedSquareIndicators();
			return;
		}
	}
}
