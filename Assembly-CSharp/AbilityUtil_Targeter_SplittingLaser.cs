using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SplittingLaser : AbilityUtil_Targeter
{
	private float m_minAngle;

	private float m_maxAngle;

	private float m_interpMinDistanceInSquares;

	private float m_interpMaxDistanceInSquares;

	private int m_numSplitLaserPairs;

	private bool m_alwaysSplit;

	private float m_primaryLaserLengthInSquares;

	private float m_primaryLaserWidthInSquares;

	private bool m_primaryLaserPenetratesLoS;

	private int m_primaryLaserMaxTargets;

	private bool m_primaryLaserAffectsEnemies;

	private bool m_primaryLaserAffectsAllies;

	private float m_splitLaserLengthInSquares;

	private float m_splitLaserWidthInSquares;

	private bool m_splitLaserPenetratesLoS;

	private int m_splitLaserMaxTargets;

	private bool m_splitLaserAffectsEnemies;

	private bool m_splitLaserAffectsAllies;

	public AbilityUtil_Targeter_SplittingLaser(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, int numSplitLaserPairs, bool alwaysSplit, float primaryLaserLengthInSquare, float primaryLaserWidthInSquares, bool primaryLaserPenetratesLoS, int primaryLaserMaxTargets, bool primaryLaserAffectsEnemies, bool primaryLaserAffectsAllies, float splitLaserLengthInSquare, float splitLaserWidthInSquares, bool splitLaserPenetratesLoS, int splitLaserMaxTargets, bool splitLaserAffectsEnemies, bool splitLaserAffectsAllies)
		: base(ability)
	{
		m_minAngle = Mathf.Max(0f, minAngle);
		m_maxAngle = maxAngle;
		m_interpMinDistanceInSquares = angleInterpMinDistance;
		m_interpMaxDistanceInSquares = angleInterpMaxDistance;
		m_numSplitLaserPairs = numSplitLaserPairs;
		m_alwaysSplit = alwaysSplit;
		m_primaryLaserLengthInSquares = primaryLaserLengthInSquare;
		m_primaryLaserWidthInSquares = primaryLaserWidthInSquares;
		m_primaryLaserPenetratesLoS = primaryLaserPenetratesLoS;
		m_primaryLaserMaxTargets = primaryLaserMaxTargets;
		m_primaryLaserAffectsEnemies = primaryLaserAffectsEnemies;
		m_primaryLaserAffectsAllies = primaryLaserAffectsAllies;
		m_splitLaserLengthInSquares = splitLaserLengthInSquare;
		m_splitLaserWidthInSquares = splitLaserWidthInSquares;
		m_splitLaserPenetratesLoS = splitLaserPenetratesLoS;
		m_splitLaserMaxTargets = splitLaserMaxTargets;
		m_splitLaserAffectsEnemies = splitLaserAffectsEnemies;
		m_splitLaserAffectsAllies = splitLaserAffectsAllies;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<Team> GetPrimaryLaserAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			if (m_primaryLaserAffectsEnemies)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				list.Add(caster.GetOpposingTeam());
			}
			if (m_primaryLaserAffectsAllies)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(caster.GetTeam());
			}
		}
		return list;
	}

	public List<Team> GetSplitLaserAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_splitLaserAffectsEnemies)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(caster.GetOpposingTeam());
			}
			if (m_splitLaserAffectsAllies)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(caster.GetTeam());
			}
		}
		return list;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, m_primaryLaserLengthInSquares, m_primaryLaserWidthInSquares, targetingActor, GetPrimaryLaserAffectedTeams(targetingActor), m_primaryLaserPenetratesLoS, m_primaryLaserMaxTargets, false, false, out laserCoords.end, null);
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		for (int i = 0; i < actorsInLaser.Count; i++)
		{
			if (actorsInLaser[i].GetTeam() == targetingActor.GetTeam())
			{
				list2.Add(actorsInLaser[i]);
			}
			else
			{
				list.Add(actorsInLaser[i]);
			}
		}
		AddActorsInRange(list, laserCoords.start, targetingActor);
		AddActorsInRange(list2, laserCoords.start, targetingActor, AbilityTooltipSubject.Tertiary);
		SetupLaserHighlight(laserCoords, 0);
		if (!m_alwaysSplit)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (actorsInLaser.Count <= 0)
			{
				for (int j = 1; j < m_highlights.Count; j++)
				{
					DisableLaserHighlight(j);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		float num = CalculateSplitAngleDegrees(currentTarget, targetingActor);
		float num2 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		int num3 = 0;
		VectorUtils.LaserCoords laserCoords2 = default(VectorUtils.LaserCoords);
		VectorUtils.LaserCoords laserCoords3 = default(VectorUtils.LaserCoords);
		while (num3 < m_numSplitLaserPairs)
		{
			float angle = num2 + num * (float)(num3 + 1);
			float angle2 = num2 - num * (float)(num3 + 1);
			Vector3 dir = VectorUtils.AngleDegreesToVector(angle);
			Vector3 dir2 = VectorUtils.AngleDegreesToVector(angle2);
			laserCoords2.start = laserCoords.end;
			laserCoords3.start = laserCoords.end;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords2.start, dir, m_splitLaserLengthInSquares, m_splitLaserWidthInSquares, targetingActor, GetSplitLaserAffectedTeams(targetingActor), m_splitLaserPenetratesLoS, m_splitLaserMaxTargets, false, false, out laserCoords2.end, null, actorsInLaser);
			List<ActorData> actorsInLaser3 = AreaEffectUtils.GetActorsInLaser(laserCoords3.start, dir2, m_splitLaserLengthInSquares, m_splitLaserWidthInSquares, targetingActor, GetSplitLaserAffectedTeams(targetingActor), m_splitLaserPenetratesLoS, m_splitLaserMaxTargets, false, false, out laserCoords3.end, null, actorsInLaser);
			List<ActorData> list3 = new List<ActorData>();
			List<ActorData> list4 = new List<ActorData>();
			for (int k = 0; k < actorsInLaser2.Count; k++)
			{
				if (actorsInLaser2[k].GetTeam() == targetingActor.GetTeam())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					list4.Add(actorsInLaser2[k]);
				}
				else
				{
					list3.Add(actorsInLaser2[k]);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				List<ActorData> list5 = new List<ActorData>();
				List<ActorData> list6 = new List<ActorData>();
				for (int l = 0; l < actorsInLaser3.Count; l++)
				{
					if (actorsInLaser3[l].GetTeam() == targetingActor.GetTeam())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						list6.Add(actorsInLaser3[l]);
					}
					else
					{
						list5.Add(actorsInLaser3[l]);
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_02e8;
					}
					continue;
					end_IL_02e8:
					break;
				}
				AddActorsInRange(list3, laserCoords2.start, targetingActor, AbilityTooltipSubject.Secondary);
				AddActorsInRange(list5, laserCoords3.start, targetingActor, AbilityTooltipSubject.Secondary);
				AddActorsInRange(list4, laserCoords2.start, targetingActor, AbilityTooltipSubject.Quaternary);
				AddActorsInRange(list6, laserCoords3.start, targetingActor, AbilityTooltipSubject.Quaternary);
				SetupLaserHighlight(laserCoords2, 1 + num3 * 2, false);
				SetupLaserHighlight(laserCoords3, 2 + num3 * 2, false);
				num3++;
				goto IL_035e;
			}
			IL_035e:;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SetupLaserHighlight(VectorUtils.LaserCoords laserCoords, int highlightIndex, bool primary = true)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		float num;
		if (primary)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_primaryLaserWidthInSquares;
		}
		else
		{
			num = m_splitLaserWidthInSquares;
		}
		float widthInWorld = num * Board.Get().squareSize;
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		while (m_highlights.Count <= highlightIndex)
		{
			m_highlights.Add(null);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_highlights[highlightIndex] == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_highlights[highlightIndex] = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude);
			}
			GameObject gameObject = m_highlights[highlightIndex];
			gameObject.SetActive(true);
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject);
			gameObject.transform.position = laserCoords.start + new Vector3(0f, y, 0f);
			gameObject.transform.rotation = Quaternion.LookRotation(normalized);
			return;
		}
	}

	private void DisableLaserHighlight(int highlightIndex)
	{
		if (m_highlights != null && m_highlights.Count > highlightIndex && m_highlights[highlightIndex] != null)
		{
			m_highlights[highlightIndex].SetActive(false);
		}
	}

	private float CalculateSplitAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float num = m_interpMaxDistanceInSquares - m_interpMinDistanceInSquares;
		if (num <= 0f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_minAngle;
				}
			}
		}
		float value = (currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude / Board.Get().squareSize;
		float num2 = Mathf.Clamp(value, m_interpMinDistanceInSquares, m_interpMaxDistanceInSquares) - m_interpMinDistanceInSquares;
		float num3 = num2 / num;
		float num4 = 1f - num3;
		float num5 = m_maxAngle - m_minAngle;
		return m_minAngle + num5 * num4;
	}
}
