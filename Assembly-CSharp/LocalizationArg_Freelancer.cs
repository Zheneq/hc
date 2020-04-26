using System;

[Serializable]
public class LocalizationArg_Freelancer : LocalizationArg
{
	public CharacterType m_characterType;

	public static LocalizationArg_Freelancer Create(CharacterType characterType)
	{
		LocalizationArg_Freelancer localizationArg_Freelancer = new LocalizationArg_Freelancer();
		localizationArg_Freelancer.m_characterType = characterType;
		return localizationArg_Freelancer;
	}

	public override string TR()
	{
		return StringUtil.TR_CharacterName(m_characterType.ToString());
	}
}
