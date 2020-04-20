using System;

[Serializable]
public class LocalizationArg_AbilityPing : LocalizationArg
{
	public CharacterType m_characterType;

	public string m_abilityType;

	public string m_abilityName;

	public bool m_isSelectable;

	public int m_remainingCooldown;

	public bool m_isUlt;

	public int m_currentTechPoints;

	public int m_maxTechPoints;

	public static LocalizationArg_AbilityPing Create(CharacterType characterType, Ability ability, bool isSelectable, int remainingCooldown, bool isUlt, int currentTechPoints, int maxTechPoints)
	{
		return new LocalizationArg_AbilityPing
		{
			m_characterType = characterType,
			m_abilityType = ability.GetType().ToString(),
			m_abilityName = ability.m_abilityName,
			m_isSelectable = isSelectable,
			m_remainingCooldown = remainingCooldown,
			m_isUlt = isUlt,
			m_currentTechPoints = currentTechPoints,
			m_maxTechPoints = maxTechPoints
		};
	}

	public override string TR()
	{
		string arg;
		if (this.m_isSelectable)
		{
			arg = StringUtil.TR("Ready!", "Global");
		}
		else if (this.m_remainingCooldown > 0)
		{
			if (this.m_remainingCooldown == 1)
			{
				arg = StringUtil.TR("TurnLeft", "GameModes");
			}
			else
			{
				arg = string.Format(StringUtil.TR("TurnsLeft", "GameModes"), this.m_remainingCooldown);
			}
		}
		else if (this.m_isUlt)
		{
			arg = string.Format(StringUtil.TR("EnergySoFar", "Global"), this.m_currentTechPoints, this.m_maxTechPoints);
		}
		else
		{
			arg = StringUtil.TR("Ready!", "Global");
		}
		return string.Format(StringUtil.TR("AbilityPingMessage", "Global"), StringUtil.TR_CharacterName(this.m_characterType.ToString()), StringUtil.TR_AbilityName(this.m_abilityType, this.m_abilityName), arg);
	}
}
