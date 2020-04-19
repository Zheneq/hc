using System;

[Serializable]
public class LocalizationArg_Freelancer : LocalizationArg
{
	public CharacterType m_characterType;

	public static LocalizationArg_Freelancer Create(CharacterType characterType)
	{
		return new LocalizationArg_Freelancer
		{
			m_characterType = characterType
		};
	}

	public override string TR()
	{
		return StringUtil.TR_CharacterName(this.m_characterType.ToString());
	}
}
