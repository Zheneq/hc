using System;

public class AbilityUtil_Targeter_ExoTether : AbilityUtil_Targeter_Laser
{
	private ExoTetherTrap m_tetherAbility;

	private LaserTargetingInfo m_anchoredLaserTargetingInfo;

	public AbilityUtil_Targeter_ExoTether(Ability ability, LaserTargetingInfo laserTargetingInfo, LaserTargetingInfo anchoredLaserTargetingInfo) : base(ability, laserTargetingInfo)
	{
		this.m_anchoredLaserTargetingInfo = anchoredLaserTargetingInfo;
		this.m_tetherAbility = (ability as ExoTetherTrap);
	}

	public override float GetWidth()
	{
		if (this.m_tetherAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ExoTether.GetWidth()).MethodHandle;
			}
			return this.m_anchoredLaserTargetingInfo.width;
		}
		return base.GetWidth();
	}

	public override float GetDistance()
	{
		if (this.m_tetherAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ExoTether.GetDistance()).MethodHandle;
			}
			return this.m_anchoredLaserTargetingInfo.range;
		}
		return this.m_distance;
	}

	public override bool GetPenetrateLoS()
	{
		if (this.m_tetherAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ExoTether.GetPenetrateLoS()).MethodHandle;
			}
			return this.m_anchoredLaserTargetingInfo.penetrateLos;
		}
		return this.m_penetrateLoS;
	}

	public override int GetMaxTargets()
	{
		if (this.m_tetherAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ExoTether.GetMaxTargets()).MethodHandle;
			}
			return this.m_anchoredLaserTargetingInfo.maxTargets;
		}
		return this.m_maxTargets;
	}
}
