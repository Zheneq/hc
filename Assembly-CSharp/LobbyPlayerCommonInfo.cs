using System;
using System.Collections.Generic;

[Serializable]
public class LobbyPlayerCommonInfo
{
	public long AccountId;
	public int PlayerId;
	public int CustomGameVisualSlot;
	public string Handle;
	public int TitleID;
	public int TitleLevel;
	public int BannerID;
	public int EmblemID;
	public int RibbonID;
	public bool IsGameOwner;
	public bool IsReplayGenerator;
	public BotDifficulty Difficulty;
	public bool BotCanTaunt;
	public Team TeamId;
	public LobbyCharacterInfo CharacterInfo = new LobbyCharacterInfo();
	public List<LobbyCharacterInfo> RemoteCharacterInfos = new List<LobbyCharacterInfo>();
	public ReadyState ReadyState;
	public int ControllingPlayerId;
	public LobbyServerPlayerInfo ControllingPlayerInfo;
	public PlayerGameAccountType GameAccountType;
	public PlayerGameConnectionType GameConnectionType;
	public PlayerGameOptionFlag GameOptionFlags;

	public bool IsRemoteControlled
	{
		get { return ControllingPlayerInfo != null; }
	}

	public bool IsSpectator
	{
		get { return TeamId == Team.Spectator; }
	}

	public CharacterType CharacterType
	{
		get { return CharacterInfo?.CharacterType ?? CharacterType.None; }
	}

	public bool IsReady
	{
		get { return ReadyState == ReadyState.Ready || IsAIControlled || IsRemoteControlled; }
	}

	public bool ReplacedWithBots { get; set; }
	public bool IsAIControlled
	{
		get { return IsNPCBot || IsLoadTestBot || ReplacedWithBots; }
	}

	public bool IsHumanControlled
	{
		get { return !IsAIControlled; }
	}

	public bool IsNPCBot
	{
		get { return GameAccountType == PlayerGameAccountType.None; }
		set
		{
			if (value)
			{
				GameAccountType = PlayerGameAccountType.None;
			}
		}
	}

	public bool IsLoadTestBot
	{
		get { return GameAccountType == PlayerGameAccountType.LoadTest; }
		set
		{
			if (value)
			{
				GameAccountType = PlayerGameAccountType.LoadTest;
			}
		}
	}

	public void SetGameOption(LobbyGameplayOverrides gameplayOverrides)
	{
		if (!IsLoadTestBot)
		{
			return;
		}
		if (gameplayOverrides.UseFakeGameServersForLoadTests || gameplayOverrides.UseFakeClientConnectionsForLoadTests)
		{
			GameConnectionType = PlayerGameConnectionType.None;
		}
		else
		{
			GameConnectionType = PlayerGameConnectionType.RawSocket;
		}
	}

	public void SetGameOption(PlayerGameOptionFlag flag, bool on)
	{
		GameOptionFlags = on
			? GameOptionFlags.WithGameOption(flag)
			: GameOptionFlags.WithoutGameOption(flag);
	}
}
