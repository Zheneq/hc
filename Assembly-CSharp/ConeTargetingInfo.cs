using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConeTargetingInfo
{
	public float m_radiusInSquares = 1f;

	public float m_widthAngleDeg = 90f;

	public float m_backwardsOffset;

	public bool m_penetrateLos;

	[Space(10f)]
	public bool m_affectsEnemies = true;

	public bool m_affectsAllies;

	public bool m_affectsCaster;

	public List<Team> GetAffectedTeams(ActorData caster)
	{
		if (this.m_affectsEnemies)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConeTargetingInfo.GetAffectedTeams(ActorData)).MethodHandle;
			}
			if (this.m_affectsAllies)
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
				return null;
			}
		}
		List<Team> list = new List<Team>();
		if (this.m_affectsEnemies)
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
			list.Add(caster.GetOpposingTeam());
		}
		if (this.m_affectsAllies)
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
		return list;
	}

	public ConeTargetingInfo GetModifiedCopy(AbilityModPropertyConeInfo modProp)
	{
		ConeTargetingInfo coneTargetingInfo = new ConeTargetingInfo();
		if (modProp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConeTargetingInfo.GetModifiedCopy(AbilityModPropertyConeInfo)).MethodHandle;
			}
			if (modProp.m_radiusMod == null)
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
				if (Application.isEditor)
				{
					Debug.LogError("mod property not initialized, probably not yet serialized. Inspect the selected mod and save");
				}
				modProp = null;
			}
		}
		ConeTargetingInfo coneTargetingInfo2 = coneTargetingInfo;
		float radiusInSquares;
		if (modProp != null)
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
			radiusInSquares = modProp.m_radiusMod.GetModifiedValue(this.m_radiusInSquares);
		}
		else
		{
			radiusInSquares = this.m_radiusInSquares;
		}
		coneTargetingInfo2.m_radiusInSquares = radiusInSquares;
		ConeTargetingInfo coneTargetingInfo3 = coneTargetingInfo;
		float widthAngleDeg;
		if (modProp != null)
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
			widthAngleDeg = modProp.m_widthAngleMod.GetModifiedValue(this.m_widthAngleDeg);
		}
		else
		{
			widthAngleDeg = this.m_widthAngleDeg;
		}
		coneTargetingInfo3.m_widthAngleDeg = widthAngleDeg;
		coneTargetingInfo.m_backwardsOffset = ((modProp == null) ? this.m_backwardsOffset : modProp.m_backwardsOffsetMod.GetModifiedValue(this.m_backwardsOffset));
		coneTargetingInfo.m_penetrateLos = ((modProp == null) ? this.m_penetrateLos : modProp.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos));
		ConeTargetingInfo coneTargetingInfo4 = coneTargetingInfo;
		bool affectsCaster;
		if (modProp != null)
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
			affectsCaster = modProp.m_affectsCasterOverride.GetModifiedValue(this.m_affectsCaster);
		}
		else
		{
			affectsCaster = this.m_affectsCaster;
		}
		coneTargetingInfo4.m_affectsCaster = affectsCaster;
		ConeTargetingInfo coneTargetingInfo5 = coneTargetingInfo;
		bool affectsAllies;
		if (modProp != null)
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
			affectsAllies = modProp.m_affectsAllyOverride.GetModifiedValue(this.m_affectsAllies);
		}
		else
		{
			affectsAllies = this.m_affectsAllies;
		}
		coneTargetingInfo5.m_affectsAllies = affectsAllies;
		coneTargetingInfo.m_affectsEnemies = ((modProp == null) ? this.m_affectsEnemies : modProp.m_affectsEnemyOverride.GetModifiedValue(this.m_affectsEnemies));
		return coneTargetingInfo;
	}
}
