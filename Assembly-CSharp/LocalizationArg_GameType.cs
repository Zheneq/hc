using System;

[Serializable]
public class LocalizationArg_GameType : LocalizationArg
{
	public GameType m_gameType;

	public static LocalizationArg_GameType Create(GameType gameType)
	{
		return new LocalizationArg_GameType
		{
			m_gameType = gameType
		};
	}

	public override string TR()
	{
		return m_gameType.GetDisplayName();
	}
}
