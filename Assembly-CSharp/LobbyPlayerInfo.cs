using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LobbyPlayerInfo
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

	public LocalizationPayload DisplayedStat;

	public bool IsGameOwner;

	public bool IsLoadTestBot;

	public bool IsNPCBot;

	public bool BotsMasqueradeAsHumans;

	public BotDifficulty Difficulty;

	public bool BotCanTaunt;

	public Team TeamId;

	public LobbyCharacterInfo CharacterInfo = new LobbyCharacterInfo();

	public List<LobbyCharacterInfo> RemoteCharacterInfos = new List<LobbyCharacterInfo>();

	public ReadyState ReadyState;

	public int ControllingPlayerId;

	public ClientAccessLevel EffectiveClientAccessLevel;

	public bool ReplacedWithBots
	{
		get;
		set;
	}

	[JsonIgnore]
	public bool IsRemoteControlled => ControllingPlayerId != 0;

	[JsonIgnore]
	public bool IsSpectator => TeamId == Team.Spectator;

	[JsonIgnore]
	public CharacterType CharacterType
	{
		get
		{
			int result;
			if (CharacterInfo == null)
			{
				result = 0;
			}
			else
			{
				result = (int)CharacterInfo.CharacterType;
			}
			return (CharacterType)result;
		}
	}

	[JsonIgnore]
	public bool IsReady
	{
		get
		{
			int result;
			if (ReadyState != ReadyState.Ready && !IsAIControlled)
			{
				result = (IsRemoteControlled ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	[JsonIgnore]
	public bool IsAIControlled => IsNPCBot || IsLoadTestBot;

	public LobbyPlayerInfo Clone()
	{
		return (LobbyPlayerInfo)MemberwiseClone();
	}

	public string GetHandle()
	{
		if (IsRemoteControlled)
		{
			return $"{StringUtil.TR_CharacterName(CharacterInfo.CharacterType.ToString())} ({Handle})";
		}
		if (IsNPCBot)
		{
			if (!BotsMasqueradeAsHumans)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return StringUtil.TR_CharacterName(CharacterInfo.CharacterType.ToString());
					}
				}
			}
		}
		return Handle;
	}

	public static LobbyPlayerInfo FromServer(LobbyServerPlayerInfo serverInfo, int maxPlayerLevel, MatchmakingQueueConfig queueConfig)
	{
		LobbyPlayerInfo lobbyPlayerInfo = null;
		if (serverInfo != null)
		{
			List<LobbyCharacterInfo> list = null;
			if (serverInfo.RemoteCharacterInfos != null)
			{
				list = new List<LobbyCharacterInfo>();
				using (List<LobbyCharacterInfo>.Enumerator enumerator = serverInfo.RemoteCharacterInfos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LobbyCharacterInfo current = enumerator.Current;
						list.Add(current.Clone());
					}
				}
			}
			LobbyPlayerInfo lobbyPlayerInfo2 = new LobbyPlayerInfo();
			lobbyPlayerInfo2.AccountId = serverInfo.AccountId;
			lobbyPlayerInfo2.PlayerId = serverInfo.PlayerId;
			lobbyPlayerInfo2.CustomGameVisualSlot = serverInfo.CustomGameVisualSlot;
			lobbyPlayerInfo2.Handle = serverInfo.Handle;
			lobbyPlayerInfo2.TitleID = serverInfo.TitleID;
			lobbyPlayerInfo2.TitleLevel = serverInfo.TitleLevel;
			lobbyPlayerInfo2.BannerID = serverInfo.BannerID;
			lobbyPlayerInfo2.EmblemID = serverInfo.EmblemID;
			lobbyPlayerInfo2.RibbonID = serverInfo.RibbonID;
			lobbyPlayerInfo2.IsGameOwner = serverInfo.IsGameOwner;
			lobbyPlayerInfo2.ReplacedWithBots = serverInfo.ReplacedWithBots;
			lobbyPlayerInfo2.IsNPCBot = serverInfo.IsNPCBot;
			lobbyPlayerInfo2.IsLoadTestBot = serverInfo.IsLoadTestBot;
			int botsMasqueradeAsHumans;
			if (queueConfig != null)
			{
				botsMasqueradeAsHumans = (queueConfig.BotsMasqueradeAsHumans ? 1 : 0);
			}
			else
			{
				botsMasqueradeAsHumans = 0;
			}
			lobbyPlayerInfo2.BotsMasqueradeAsHumans = ((byte)botsMasqueradeAsHumans != 0);
			lobbyPlayerInfo2.Difficulty = serverInfo.Difficulty;
			lobbyPlayerInfo2.BotCanTaunt = serverInfo.BotCanTaunt;
			lobbyPlayerInfo2.TeamId = serverInfo.TeamId;
			lobbyPlayerInfo2.CharacterInfo = ((serverInfo.CharacterInfo == null) ? null : serverInfo.CharacterInfo.Clone());
			lobbyPlayerInfo2.RemoteCharacterInfos = list;
			lobbyPlayerInfo2.ReadyState = serverInfo.ReadyState;
			int controllingPlayerId;
			if (serverInfo.IsRemoteControlled)
			{
				controllingPlayerId = serverInfo.ControllingPlayerInfo.PlayerId;
			}
			else
			{
				controllingPlayerId = 0;
			}
			lobbyPlayerInfo2.ControllingPlayerId = controllingPlayerId;
			lobbyPlayerInfo2.EffectiveClientAccessLevel = serverInfo.EffectiveClientAccessLevel;
			lobbyPlayerInfo = lobbyPlayerInfo2;
			if (serverInfo.AccountLevel >= maxPlayerLevel)
			{
				lobbyPlayerInfo.DisplayedStat = LocalizationPayload.Create("TotalSeasonLevelStatNumber", "Global", LocalizationArg_Int32.Create(serverInfo.TotalLevel));
			}
			else
			{
				lobbyPlayerInfo.DisplayedStat = LocalizationPayload.Create("LevelStatNumber", "Global", LocalizationArg_Int32.Create(serverInfo.AccountLevel));
			}
		}
		return lobbyPlayerInfo;
	}
}
