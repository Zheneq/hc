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
		if (m_affectsEnemies)
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
			if (m_affectsAllies)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
		}
		List<Team> list = new List<Team>();
		if (m_affectsEnemies)
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
			list.Add(caster.GetOpposingTeam());
		}
		if (m_affectsAllies)
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
		return list;
	}

	public ConeTargetingInfo GetModifiedCopy(AbilityModPropertyConeInfo modProp)
	{
		ConeTargetingInfo coneTargetingInfo = new ConeTargetingInfo();
		if (modProp != null)
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
			if (modProp.m_radiusMod == null)
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
				if (Application.isEditor)
				{
					Debug.LogError("mod property not initialized, probably not yet serialized. Inspect the selected mod and save");
				}
				modProp = null;
			}
		}
		float radiusInSquares;
		if (modProp != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			radiusInSquares = modProp.m_radiusMod.GetModifiedValue(m_radiusInSquares);
		}
		else
		{
			radiusInSquares = m_radiusInSquares;
		}
		coneTargetingInfo.m_radiusInSquares = radiusInSquares;
		float widthAngleDeg;
		if (modProp != null)
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
			widthAngleDeg = modProp.m_widthAngleMod.GetModifiedValue(m_widthAngleDeg);
		}
		else
		{
			widthAngleDeg = m_widthAngleDeg;
		}
		coneTargetingInfo.m_widthAngleDeg = widthAngleDeg;
		coneTargetingInfo.m_backwardsOffset = (modProp?.m_backwardsOffsetMod.GetModifiedValue(m_backwardsOffset) ?? m_backwardsOffset);
		coneTargetingInfo.m_penetrateLos = (modProp?.m_penetrateLosMod.GetModifiedValue(m_penetrateLos) ?? m_penetrateLos);
		bool affectsCaster;
		if (modProp != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			affectsCaster = modProp.m_affectsCasterOverride.GetModifiedValue(m_affectsCaster);
		}
		else
		{
			affectsCaster = m_affectsCaster;
		}
		coneTargetingInfo.m_affectsCaster = affectsCaster;
		bool affectsAllies;
		if (modProp != null)
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
			affectsAllies = modProp.m_affectsAllyOverride.GetModifiedValue(m_affectsAllies);
		}
		else
		{
			affectsAllies = m_affectsAllies;
		}
		coneTargetingInfo.m_affectsAllies = affectsAllies;
		coneTargetingInfo.m_affectsEnemies = (modProp?.m_affectsEnemyOverride.GetModifiedValue(m_affectsEnemies) ?? m_affectsEnemies);
		return coneTargetingInfo;
	}
}
