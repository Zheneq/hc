using System.Collections.Generic;

public class AbilityUtil_Targeter_VariableLaser : AbilityUtil_Targeter_Laser
{
	private List<LaserLogicInfo> m_infoValues;

	private TargeterUtils.VariableType m_thresholdType;

	private int m_scoreBonus;

	private AbilityModPropertyFloat m_lengthMod;

	private AbilityModPropertyFloat m_widthMod;

	public AbilityUtil_Targeter_VariableLaser(Ability ability, List<LaserLogicInfo> infoValues, TargeterUtils.VariableType thresholdType, bool affectsAllies = false, bool affectsCaster = false, int scoreBonus = 0)
		: base(ability, infoValues[0].m_width, infoValues[0].m_distance, infoValues[0].m_penetrateLoS, infoValues[0].m_maxTargets, affectsAllies, affectsCaster)
	{
		m_infoValues = infoValues;
		m_thresholdType = thresholdType;
		m_scoreBonus = scoreBonus;
		m_lengthMod = new AbilityModPropertyFloat();
		m_widthMod = new AbilityModPropertyFloat();
	}

	public void SetLengthMod(AbilityModPropertyFloat lengthMod)
	{
		m_lengthMod = lengthMod;
	}

	public void SetWidthMod(AbilityModPropertyFloat widthMod)
	{
		m_widthMod = widthMod;
	}

	protected virtual void UpdateValues(ActorData targetingActor)
	{
		int num = m_scoreBonus + GameplayUtils.GetScoreForVariableType(targetingActor, m_thresholdType);
		int index = 0;
		for (int i = 1; i < m_infoValues.Count; i++)
		{
			if (num >= m_infoValues[i].m_threshold)
			{
				index = i;
			}
		}
		m_width = m_widthMod.GetModifiedValue(m_infoValues[index].m_width);
		m_distance = m_lengthMod.GetModifiedValue(m_infoValues[index].m_distance);
		m_penetrateLoS = m_infoValues[index].m_penetrateLoS;
		m_maxTargets = m_infoValues[index].m_maxTargets;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateValues(targetingActor);
		base.UpdateTargeting(currentTarget, targetingActor);
	}
}
