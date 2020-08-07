using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LaserTargetingInfo
{
	public float range = 5f;

	public float width = 1f;

	public bool penetrateLos;

	public int maxTargets = 1;

	public bool affectsEnemies = true;

	public bool affectsAllies;

	public bool affectsCaster;

	public List<Team> GetAffectedTeams(ActorData caster)
	{
		if (affectsEnemies && affectsAllies)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		List<Team> list = new List<Team>();
		if (affectsEnemies)
		{
			list.Add(caster.GetEnemyTeam());
		}
		if (affectsAllies)
		{
			list.Add(caster.GetTeam());
		}
		return list;
	}

	public LaserTargetingInfo GetModifiedCopy(AbilityModPropertyLaserInfo mod)
	{
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		if (mod != null)
		{
			if (mod.m_rangeMod == null)
			{
				if (Application.isEditor)
				{
					Debug.LogError("mod property not initialized, probably not yet serialized. Inspect the selected mod and save");
				}
				mod = null;
			}
		}
		laserTargetingInfo.range = (mod?.GetRange(this) ?? range);
		float num;
		if (mod != null)
		{
			num = mod.GetWidth(this);
		}
		else
		{
			num = width;
		}
		laserTargetingInfo.width = num;
		int num2;
		if (mod != null)
		{
			num2 = mod.GetMaxTargets(this);
		}
		else
		{
			num2 = maxTargets;
		}
		laserTargetingInfo.maxTargets = num2;
		bool num3;
		if (mod != null)
		{
			num3 = mod.GetPenetrateLos(this);
		}
		else
		{
			num3 = penetrateLos;
		}
		laserTargetingInfo.penetrateLos = num3;
		bool affectsEnemy;
		if (mod != null)
		{
			affectsEnemy = mod.GetAffectsEnemy(this);
		}
		else
		{
			affectsEnemy = affectsEnemies;
		}
		laserTargetingInfo.affectsEnemies = affectsEnemy;
		bool affectsAlly;
		if (mod != null)
		{
			affectsAlly = mod.GetAffectsAlly(this);
		}
		else
		{
			affectsAlly = affectsAllies;
		}
		laserTargetingInfo.affectsAllies = affectsAlly;
		bool num4;
		if (mod != null)
		{
			num4 = mod.GetAffectsCaster(this);
		}
		else
		{
			num4 = affectsCaster;
		}
		laserTargetingInfo.affectsCaster = num4;
		return laserTargetingInfo;
	}
}
