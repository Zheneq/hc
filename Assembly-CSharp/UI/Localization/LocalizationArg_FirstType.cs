using System;

[Serializable]
public class LocalizationArg_FirstType : LocalizationArg
{
	public int m_firstType;
	public int m_id;

	public static LocalizationArg_FirstType Create(int firstType, int id)
	{
		return new LocalizationArg_FirstType
		{
			m_firstType = firstType,
			m_id = id
		};
	}

	public override string TR()
	{
		switch (m_firstType)
		{
			case -1:
				return StringUtil.TR_QuestDescription(m_id);
			case -2:
				return string.Format(
					StringUtil.TR("ReachLevelTwenty", "FirstType"),
					StringUtil.TR_CharacterName(((CharacterType)m_id).ToString()));
			default:
				return "do the unknown";
		}
	}
}
