using System;

[Serializable]
public class LocalizationArg_FirstRank : LocalizationArg
{
	public int m_rank;

	public static LocalizationArg_FirstRank Create(int rank)
	{
		LocalizationArg_FirstRank localizationArg_FirstRank = new LocalizationArg_FirstRank();
		localizationArg_FirstRank.m_rank = rank;
		return localizationArg_FirstRank;
	}

	public override string TR()
	{
		if (m_rank == 1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return StringUtil.TR("First", "FirstRank");
				}
			}
		}
		if (m_rank == 2)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return StringUtil.TR("Second", "FirstRank");
				}
			}
		}
		if (m_rank == 3)
		{
			return StringUtil.TR("Third", "FirstRank");
		}
		if (m_rank == 4)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return StringUtil.TR("Fourth", "FirstRank");
				}
			}
		}
		return StringUtil.TR("Fifth", "FirstRank");
	}
}
