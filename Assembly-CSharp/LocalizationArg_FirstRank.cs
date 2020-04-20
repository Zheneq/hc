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
		if (this.m_rank == 1)
		{
			return StringUtil.TR("First", "FirstRank");
		}
		if (this.m_rank == 2)
		{
			return StringUtil.TR("Second", "FirstRank");
		}
		if (this.m_rank == 3)
		{
			return StringUtil.TR("Third", "FirstRank");
		}
		if (this.m_rank == 4)
		{
			return StringUtil.TR("Fourth", "FirstRank");
		}
		return StringUtil.TR("Fifth", "FirstRank");
	}
}
