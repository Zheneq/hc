using System;
using System.Collections.Generic;

public class AbilityUtil_Targeter_VariableLaser : AbilityUtil_Targeter_Laser
{
	private List<LaserLogicInfo> m_infoValues;

	private TargeterUtils.VariableType m_thresholdType;

	private int m_scoreBonus;

	private AbilityModPropertyFloat m_lengthMod;

	private AbilityModPropertyFloat m_widthMod;

	public AbilityUtil_Targeter_VariableLaser(Ability ability, List<LaserLogicInfo> infoValues, TargeterUtils.VariableType thresholdType, bool affectsAllies = false, bool affectsCaster = false, int scoreBonus = 0) : base(ability, infoValues[0].m_width, infoValues[0].m_distance, infoValues[0].m_penetrateLoS, infoValues[0].m_maxTargets, affectsAllies, affectsCaster)
	{
		this.m_infoValues = infoValues;
		this.m_thresholdType = thresholdType;
		this.m_scoreBonus = scoreBonus;
		this.m_lengthMod = new AbilityModPropertyFloat();
		this.m_widthMod = new AbilityModPropertyFloat();
	}

	public void SetLengthMod(AbilityModPropertyFloat lengthMod)
	{
		this.m_lengthMod = lengthMod;
	}

	public void SetWidthMod(AbilityModPropertyFloat widthMod)
	{
		this.m_widthMod = widthMod;
	}

	protected virtual void UpdateValues(ActorData targetingActor)
	{
		int num = this.m_scoreBonus + GameplayUtils.GetScoreForVariableType(targetingActor, this.m_thresholdType);
		int index = 0;
		for (int i = 1; i < this.m_infoValues.Count; i++)
		{
			if (num >= this.m_infoValues[i].m_threshold)
			{
				index = i;
			}
		}
		this.m_width = this.m_widthMod.GetModifiedValue(this.m_infoValues[index].m_width);
		this.m_distance = this.m_lengthMod.GetModifiedValue(this.m_infoValues[index].m_distance);
		this.m_penetrateLoS = this.m_infoValues[index].m_penetrateLoS;
		this.m_maxTargets = this.m_infoValues[index].m_maxTargets;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateValues(targetingActor);
		base.UpdateTargeting(currentTarget, targetingActor);
	}
}
