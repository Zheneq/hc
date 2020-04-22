public class AbilityUtil_Targeter_ExoTether : AbilityUtil_Targeter_Laser
{
	private ExoTetherTrap m_tetherAbility;

	private LaserTargetingInfo m_anchoredLaserTargetingInfo;

	public AbilityUtil_Targeter_ExoTether(Ability ability, LaserTargetingInfo laserTargetingInfo, LaserTargetingInfo anchoredLaserTargetingInfo)
		: base(ability, laserTargetingInfo)
	{
		m_anchoredLaserTargetingInfo = anchoredLaserTargetingInfo;
		m_tetherAbility = (ability as ExoTetherTrap);
	}

	public override float GetWidth()
	{
		if (m_tetherAbility != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_anchoredLaserTargetingInfo.width;
				}
			}
		}
		return base.GetWidth();
	}

	public override float GetDistance()
	{
		if (m_tetherAbility != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_anchoredLaserTargetingInfo.range;
				}
			}
		}
		return m_distance;
	}

	public override bool GetPenetrateLoS()
	{
		if (m_tetherAbility != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_anchoredLaserTargetingInfo.penetrateLos;
				}
			}
		}
		return m_penetrateLoS;
	}

	public override int GetMaxTargets()
	{
		if (m_tetherAbility != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_anchoredLaserTargetingInfo.maxTargets;
				}
			}
		}
		return m_maxTargets;
	}
}
