using System;

[Serializable]
public class LocalizationArg_FirstType : LocalizationArg
{
	public int m_firstType;

	public int m_id;

	public static LocalizationArg_FirstType Create(int firstType, int id)
	{
		LocalizationArg_FirstType localizationArg_FirstType = new LocalizationArg_FirstType();
		localizationArg_FirstType.m_firstType = firstType;
		localizationArg_FirstType.m_id = id;
		return localizationArg_FirstType;
	}

	public override string TR()
	{
		if (m_firstType == -1)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return StringUtil.TR_QuestDescription(m_id);
				}
			}
		}
		if (m_firstType == -2)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					CharacterType id = (CharacterType)m_id;
					string arg = StringUtil.TR_CharacterName(id.ToString());
					return string.Format(StringUtil.TR("ReachLevelTwenty", "FirstType"), arg);
				}
				}
			}
		}
		return "do the unknown";
	}
}
