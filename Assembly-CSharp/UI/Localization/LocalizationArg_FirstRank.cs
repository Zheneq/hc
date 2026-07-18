using System;

[Serializable]
public class LocalizationArg_FirstRank : LocalizationArg
{
	public int m_rank;

	public static LocalizationArg_FirstRank Create(int rank)
	{
		return new LocalizationArg_FirstRank
		{
			m_rank = rank
		};
	}

	public override string TR()
	{
		switch (m_rank)
		{
			case 1:
				return StringUtil.TR("First", "FirstRank");
			case 2:
				return StringUtil.TR("Second", "FirstRank");
			case 3:
				return StringUtil.TR("Third", "FirstRank");
			case 4:
				return StringUtil.TR("Fourth", "FirstRank");
			default:
				return StringUtil.TR("Fifth", "FirstRank");
		}
	}
}
