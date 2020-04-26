using System;

[Serializable]
public class MatchResultsStatline : ICloneable
{
	[Serializable]
	public struct AbilityEntry
	{
		public int AbilityModId;
	}

	[NonSerialized]
	public object Actor;

	public int PlayerId;

	public long AccountID;

	public string DisplayName;

	public CharacterType Character;

	public bool IsPerspective;

	public bool IsAlly;

	public bool IsHumanControlled;

	public bool IsBotMasqueradingAsHuman;

	public bool HumanReplacedByBot;

	public int TitleID;

	public int TitleLevel;

	public int BannerID;

	public int EmblemID;

	public int RibbonID;

	public AbilityEntry[] AbilityEntries;

	public bool CatalystHasPrepPhase;

	public bool CatalystHasDashPhase;

	public bool CatalystHasBlastPhase;

	public int TotalPlayerKills;

	public int TotalDeaths;

	public int TotalPlayerDamage;

	public int TotalPlayerAssists;

	public int TotalPlayerHealingFromAbility;

	public int TotalPlayerAbsorb;

	public int TotalPlayerDamageReceived;

	public int TotalPlayerTurns;

	public float TotalPlayerLockInTime;

	public int TotalPlayerContribution;

	public object Clone()
	{
		return MemberwiseClone();
	}
}
