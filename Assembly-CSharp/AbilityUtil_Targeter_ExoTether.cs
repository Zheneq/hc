public class AbilityUtil_Targeter_ExoTether : AbilityUtil_Targeter_Laser
{
	private ExoTetherTrap m_tetherAbility;
	private LaserTargetingInfo m_anchoredLaserTargetingInfo;

	public AbilityUtil_Targeter_ExoTether(
		Ability ability,
		LaserTargetingInfo laserTargetingInfo,
		LaserTargetingInfo anchoredLaserTargetingInfo)
		: base(ability, laserTargetingInfo)
	{
		m_anchoredLaserTargetingInfo = anchoredLaserTargetingInfo;
		m_tetherAbility = ability as ExoTetherTrap;
	}

	public override float GetWidth()
	{
		return m_tetherAbility != null
			? m_anchoredLaserTargetingInfo.width
			: base.GetWidth();
	}

	public override float GetDistance()
	{
		return m_tetherAbility != null
			? m_anchoredLaserTargetingInfo.range
			: m_distance;
	}

	public override bool GetPenetrateLoS()
	{
		return m_tetherAbility != null
			? m_anchoredLaserTargetingInfo.penetrateLos
			: m_penetrateLoS;
	}

	public override int GetMaxTargets()
	{
		return m_tetherAbility != null
			? m_anchoredLaserTargetingInfo.maxTargets
			: m_maxTargets;
	}
}
