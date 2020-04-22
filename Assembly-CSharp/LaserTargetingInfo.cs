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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		List<Team> list = new List<Team>();
		if (affectsEnemies)
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
		if (affectsAllies)
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
			list.Add(caster.GetTeam());
		}
		return list;
	}

	public LaserTargetingInfo GetModifiedCopy(AbilityModPropertyLaserInfo mod)
	{
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		if (mod != null)
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
			if (mod.m_rangeMod == null)
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
				mod = null;
			}
		}
		laserTargetingInfo.range = (mod?.GetRange(this) ?? range);
		float num;
		if (mod != null)
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
