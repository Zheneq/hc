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
		if (this.m_firstType == -1)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationArg_FirstType.TR()).MethodHandle;
			}
			return StringUtil.TR_QuestDescription(this.m_id);
		}
		if (this.m_firstType == -2)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterType id = (CharacterType)this.m_id;
			string arg = StringUtil.TR_CharacterName(id.ToString());
			return string.Format(StringUtil.TR("ReachLevelTwenty", "FirstType"), arg);
		}
		return "do the unknown";
	}
}
