using System;

[Serializable]
public class LocalizationArg_GameType : LocalizationArg
{
	public GameType m_gameType;

	public static LocalizationArg_GameType Create(GameType gameType)
	{
		LocalizationArg_GameType localizationArg_GameType = new LocalizationArg_GameType();
		localizationArg_GameType.m_gameType = gameType;
		return localizationArg_GameType;
	}

	public override string TR()
	{
		return m_gameType.GetDisplayName();
	}
}
