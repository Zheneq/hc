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
		if (this.affectsEnemies && this.affectsAllies)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LaserTargetingInfo.GetAffectedTeams(ActorData)).MethodHandle;
			}
			return null;
		}
		List<Team> list = new List<Team>();
		if (this.affectsEnemies)
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
		if (this.affectsAllies)
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
			list.Add(caster.GetTeam());
		}
		return list;
	}

	public LaserTargetingInfo GetModifiedCopy(AbilityModPropertyLaserInfo mod)
	{
		LaserTargetingInfo laserTargetingInfo = new LaserTargetingInfo();
		if (mod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LaserTargetingInfo.GetModifiedCopy(AbilityModPropertyLaserInfo)).MethodHandle;
			}
			if (mod.m_rangeMod == null)
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
				mod = null;
			}
		}
		laserTargetingInfo.range = ((mod == null) ? this.range : mod.GetRange(this));
		LaserTargetingInfo laserTargetingInfo2 = laserTargetingInfo;
		float num;
		if (mod != null)
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
			num = mod.GetWidth(this);
		}
		else
		{
			num = this.width;
		}
		laserTargetingInfo2.width = num;
		LaserTargetingInfo laserTargetingInfo3 = laserTargetingInfo;
		int num2;
		if (mod != null)
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
			num2 = mod.GetMaxTargets(this);
		}
		else
		{
			num2 = this.maxTargets;
		}
		laserTargetingInfo3.maxTargets = num2;
		LaserTargetingInfo laserTargetingInfo4 = laserTargetingInfo;
		bool flag;
		if (mod != null)
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
			flag = mod.GetPenetrateLos(this);
		}
		else
		{
			flag = this.penetrateLos;
		}
		laserTargetingInfo4.penetrateLos = flag;
		LaserTargetingInfo laserTargetingInfo5 = laserTargetingInfo;
		bool affectsEnemy;
		if (mod != null)
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
			affectsEnemy = mod.GetAffectsEnemy(this);
		}
		else
		{
			affectsEnemy = this.affectsEnemies;
		}
		laserTargetingInfo5.affectsEnemies = affectsEnemy;
		LaserTargetingInfo laserTargetingInfo6 = laserTargetingInfo;
		bool affectsAlly;
		if (mod != null)
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
			affectsAlly = mod.GetAffectsAlly(this);
		}
		else
		{
			affectsAlly = this.affectsAllies;
		}
		laserTargetingInfo6.affectsAllies = affectsAlly;
		LaserTargetingInfo laserTargetingInfo7 = laserTargetingInfo;
		bool flag2;
		if (mod != null)
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
			flag2 = mod.GetAffectsCaster(this);
		}
		else
		{
			flag2 = this.affectsCaster;
		}
		laserTargetingInfo7.affectsCaster = flag2;
		return laserTargetingInfo;
	}
}
