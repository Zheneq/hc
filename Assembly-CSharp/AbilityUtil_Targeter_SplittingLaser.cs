using System;
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

	public AbilityUtil_Targeter_SplittingLaser(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, int numSplitLaserPairs, bool alwaysSplit, float primaryLaserLengthInSquare, float primaryLaserWidthInSquares, bool primaryLaserPenetratesLoS, int primaryLaserMaxTargets, bool primaryLaserAffectsEnemies, bool primaryLaserAffectsAllies, float splitLaserLengthInSquare, float splitLaserWidthInSquares, bool splitLaserPenetratesLoS, int splitLaserMaxTargets, bool splitLaserAffectsEnemies, bool splitLaserAffectsAllies) : base(ability)
	{
		this.m_minAngle = Mathf.Max(0f, minAngle);
		this.m_maxAngle = maxAngle;
		this.m_interpMinDistanceInSquares = angleInterpMinDistance;
		this.m_interpMaxDistanceInSquares = angleInterpMaxDistance;
		this.m_numSplitLaserPairs = numSplitLaserPairs;
		this.m_alwaysSplit = alwaysSplit;
		this.m_primaryLaserLengthInSquares = primaryLaserLengthInSquare;
		this.m_primaryLaserWidthInSquares = primaryLaserWidthInSquares;
		this.m_primaryLaserPenetratesLoS = primaryLaserPenetratesLoS;
		this.m_primaryLaserMaxTargets = primaryLaserMaxTargets;
		this.m_primaryLaserAffectsEnemies = primaryLaserAffectsEnemies;
		this.m_primaryLaserAffectsAllies = primaryLaserAffectsAllies;
		this.m_splitLaserLengthInSquares = splitLaserLengthInSquare;
		this.m_splitLaserWidthInSquares = splitLaserWidthInSquares;
		this.m_splitLaserPenetratesLoS = splitLaserPenetratesLoS;
		this.m_splitLaserMaxTargets = splitLaserMaxTargets;
		this.m_splitLaserAffectsEnemies = splitLaserAffectsEnemies;
		this.m_splitLaserAffectsAllies = splitLaserAffectsAllies;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<Team> GetPrimaryLaserAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			if (this.m_primaryLaserAffectsEnemies)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SplittingLaser.GetPrimaryLaserAffectedTeams(ActorData)).MethodHandle;
				}
				list.Add(caster.GetOpposingTeam());
			}
			if (this.m_primaryLaserAffectsAllies)
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SplittingLaser.GetSplitLaserAffectedTeams(ActorData)).MethodHandle;
			}
			if (this.m_splitLaserAffectsEnemies)
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
				list.Add(caster.GetOpposingTeam());
			}
			if (this.m_splitLaserAffectsAllies)
			{
				for (;;)
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
		base.ClearActorsInRange();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, currentTarget.AimDirection, this.m_primaryLaserLengthInSquares, this.m_primaryLaserWidthInSquares, targetingActor, this.GetPrimaryLaserAffectedTeams(targetingActor), this.m_primaryLaserPenetratesLoS, this.m_primaryLaserMaxTargets, false, false, out laserCoords.end, null, null, false, true);
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
		base.AddActorsInRange(list, laserCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
		base.AddActorsInRange(list2, laserCoords.start, targetingActor, AbilityTooltipSubject.Tertiary, false);
		this.SetupLaserHighlight(laserCoords, 0, true);
		if (!this.m_alwaysSplit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SplittingLaser.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (actorsInLaser.Count > 0)
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
			}
			else
			{
				for (int j = 1; j < this.m_highlights.Count; j++)
				{
					this.DisableLaserHighlight(j);
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					return;
				}
			}
		}
		float num = this.CalculateSplitAngleDegrees(currentTarget, targetingActor);
		float num2 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		for (int k = 0; k < this.m_numSplitLaserPairs; k++)
		{
			float angle = num2 + num * (float)(k + 1);
			float angle2 = num2 - num * (float)(k + 1);
			Vector3 dir = VectorUtils.AngleDegreesToVector(angle);
			Vector3 dir2 = VectorUtils.AngleDegreesToVector(angle2);
			VectorUtils.LaserCoords laserCoords2;
			laserCoords2.start = laserCoords.end;
			VectorUtils.LaserCoords laserCoords3;
			laserCoords3.start = laserCoords.end;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(laserCoords2.start, dir, this.m_splitLaserLengthInSquares, this.m_splitLaserWidthInSquares, targetingActor, this.GetSplitLaserAffectedTeams(targetingActor), this.m_splitLaserPenetratesLoS, this.m_splitLaserMaxTargets, false, false, out laserCoords2.end, null, actorsInLaser, false, true);
			List<ActorData> actorsInLaser3 = AreaEffectUtils.GetActorsInLaser(laserCoords3.start, dir2, this.m_splitLaserLengthInSquares, this.m_splitLaserWidthInSquares, targetingActor, this.GetSplitLaserAffectedTeams(targetingActor), this.m_splitLaserPenetratesLoS, this.m_splitLaserMaxTargets, false, false, out laserCoords3.end, null, actorsInLaser, false, true);
			List<ActorData> list3 = new List<ActorData>();
			List<ActorData> list4 = new List<ActorData>();
			for (int l = 0; l < actorsInLaser2.Count; l++)
			{
				if (actorsInLaser2[l].GetTeam() == targetingActor.GetTeam())
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
					list4.Add(actorsInLaser2[l]);
				}
				else
				{
					list3.Add(actorsInLaser2[l]);
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
			List<ActorData> list5 = new List<ActorData>();
			List<ActorData> list6 = new List<ActorData>();
			for (int m = 0; m < actorsInLaser3.Count; m++)
			{
				if (actorsInLaser3[m].GetTeam() == targetingActor.GetTeam())
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					list6.Add(actorsInLaser3[m]);
				}
				else
				{
					list5.Add(actorsInLaser3[m]);
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
			base.AddActorsInRange(list3, laserCoords2.start, targetingActor, AbilityTooltipSubject.Secondary, false);
			base.AddActorsInRange(list5, laserCoords3.start, targetingActor, AbilityTooltipSubject.Secondary, false);
			base.AddActorsInRange(list4, laserCoords2.start, targetingActor, AbilityTooltipSubject.Quaternary, false);
			base.AddActorsInRange(list6, laserCoords3.start, targetingActor, AbilityTooltipSubject.Quaternary, false);
			this.SetupLaserHighlight(laserCoords2, 1 + k * 2, false);
			this.SetupLaserHighlight(laserCoords3, 2 + k * 2, false);
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

	private void SetupLaserHighlight(VectorUtils.LaserCoords laserCoords, int highlightIndex, bool primary = true)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		float num;
		if (primary)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SplittingLaser.SetupLaserHighlight(VectorUtils.LaserCoords, int, bool)).MethodHandle;
			}
			num = this.m_primaryLaserWidthInSquares;
		}
		else
		{
			num = this.m_splitLaserWidthInSquares;
		}
		float widthInWorld = num * Board.Get().squareSize;
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		while (this.m_highlights.Count <= highlightIndex)
		{
			this.m_highlights.Add(null);
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
		if (this.m_highlights[highlightIndex] == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_highlights[highlightIndex] = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude, null);
		}
		GameObject gameObject = this.m_highlights[highlightIndex];
		gameObject.SetActive(true);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject);
		gameObject.transform.position = laserCoords.start + new Vector3(0f, y, 0f);
		gameObject.transform.rotation = Quaternion.LookRotation(normalized);
	}

	private void DisableLaserHighlight(int highlightIndex)
	{
		if (this.m_highlights != null && this.m_highlights.Count > highlightIndex && this.m_highlights[highlightIndex] != null)
		{
			this.m_highlights[highlightIndex].SetActive(false);
		}
	}

	private float CalculateSplitAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float num = this.m_interpMaxDistanceInSquares - this.m_interpMinDistanceInSquares;
		if (num <= 0f)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SplittingLaser.CalculateSplitAngleDegrees(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_minAngle;
		}
		float value = (currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude / Board.Get().squareSize;
		float num2 = Mathf.Clamp(value, this.m_interpMinDistanceInSquares, this.m_interpMaxDistanceInSquares) - this.m_interpMinDistanceInSquares;
		float num3 = num2 / num;
		float num4 = 1f - num3;
		float num5 = this.m_maxAngle - this.m_minAngle;
		return this.m_minAngle + num5 * num4;
	}
}
