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
		LocalizationArg_AbilityPing localizationArg_AbilityPing = new LocalizationArg_AbilityPing();
		localizationArg_AbilityPing.m_characterType = characterType;
		localizationArg_AbilityPing.m_abilityType = ability.GetType().ToString();
		localizationArg_AbilityPing.m_abilityName = ability.m_abilityName;
		localizationArg_AbilityPing.m_isSelectable = isSelectable;
		localizationArg_AbilityPing.m_remainingCooldown = remainingCooldown;
		localizationArg_AbilityPing.m_isUlt = isUlt;
		localizationArg_AbilityPing.m_currentTechPoints = currentTechPoints;
		localizationArg_AbilityPing.m_maxTechPoints = maxTechPoints;
		return localizationArg_AbilityPing;
	}

	public override string TR()
	{
		string arg;
		if (m_isSelectable)
		{
			while (true)
			{
				switch (3)
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
			arg = StringUtil.TR("Ready!", "Global");
		}
		else if (m_remainingCooldown <= 0)
		{
			arg = ((!m_isUlt) ? StringUtil.TR("Ready!", "Global") : string.Format(StringUtil.TR("EnergySoFar", "Global"), m_currentTechPoints, m_maxTechPoints));
		}
		else
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
			if (m_remainingCooldown == 1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				arg = StringUtil.TR("TurnLeft", "GameModes");
			}
			else
			{
				arg = string.Format(StringUtil.TR("TurnsLeft", "GameModes"), m_remainingCooldown);
			}
		}
		return string.Format(StringUtil.TR("AbilityPingMessage", "Global"), StringUtil.TR_CharacterName(m_characterType.ToString()), StringUtil.TR_AbilityName(m_abilityType, m_abilityName), arg);
	}
}
