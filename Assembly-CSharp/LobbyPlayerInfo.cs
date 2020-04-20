using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

	public LobbyPlayerInfo Clone()
	{
		return (LobbyPlayerInfo)base.MemberwiseClone();
	}

	public bool ReplacedWithBots { get; set; }

	[JsonIgnore]
	public bool IsRemoteControlled
	{
		get
		{
			return this.ControllingPlayerId != 0;
		}
	}

	[JsonIgnore]
	public bool IsSpectator
	{
		get
		{
			return this.TeamId == Team.Spectator;
		}
	}

	[JsonIgnore]
	public CharacterType CharacterType
	{
		get
		{
			CharacterType result;
			if (this.CharacterInfo == null)
			{
				result = CharacterType.None;
			}
			else
			{
				result = this.CharacterInfo.CharacterType;
			}
			return result;
		}
	}

	[JsonIgnore]
	public bool IsReady
	{
		get
		{
			bool result;
			if (this.ReadyState != ReadyState.Ready && !this.IsAIControlled)
			{
				result = this.IsRemoteControlled;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}

	[JsonIgnore]
	public bool IsAIControlled
	{
		get
		{
			return this.IsNPCBot || this.IsLoadTestBot;
		}
	}

	public string GetHandle()
	{
		if (this.IsRemoteControlled)
		{
			return string.Format("{0} ({1})", StringUtil.TR_CharacterName(this.CharacterInfo.CharacterType.ToString()), this.Handle);
		}
		if (this.IsNPCBot)
		{
			if (!this.BotsMasqueradeAsHumans)
			{
				return StringUtil.TR_CharacterName(this.CharacterInfo.CharacterType.ToString());
			}
		}
		return this.Handle;
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
						LobbyCharacterInfo lobbyCharacterInfo = enumerator.Current;
						list.Add(lobbyCharacterInfo.Clone());
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
			LobbyPlayerInfo lobbyPlayerInfo3 = lobbyPlayerInfo2;
			bool botsMasqueradeAsHumans;
			if (queueConfig != null)
			{
				botsMasqueradeAsHumans = queueConfig.BotsMasqueradeAsHumans;
			}
			else
			{
				botsMasqueradeAsHumans = false;
			}
			lobbyPlayerInfo3.BotsMasqueradeAsHumans = botsMasqueradeAsHumans;
			lobbyPlayerInfo2.Difficulty = serverInfo.Difficulty;
			lobbyPlayerInfo2.BotCanTaunt = serverInfo.BotCanTaunt;
			lobbyPlayerInfo2.TeamId = serverInfo.TeamId;
			lobbyPlayerInfo2.CharacterInfo = ((serverInfo.CharacterInfo == null) ? null : serverInfo.CharacterInfo.Clone());
			lobbyPlayerInfo2.RemoteCharacterInfos = list;
			lobbyPlayerInfo2.ReadyState = serverInfo.ReadyState;
			LobbyPlayerInfo lobbyPlayerInfo4 = lobbyPlayerInfo2;
			int controllingPlayerId;
			if (serverInfo.IsRemoteControlled)
			{
				controllingPlayerId = serverInfo.ControllingPlayerInfo.PlayerId;
			}
			else
			{
				controllingPlayerId = 0;
			}
			lobbyPlayerInfo4.ControllingPlayerId = controllingPlayerId;
			lobbyPlayerInfo2.EffectiveClientAccessLevel = serverInfo.EffectiveClientAccessLevel;
			lobbyPlayerInfo = lobbyPlayerInfo2;
			if (serverInfo.AccountLevel >= maxPlayerLevel)
			{
				lobbyPlayerInfo.DisplayedStat = LocalizationPayload.Create("TotalSeasonLevelStatNumber", "Global", new LocalizationArg[]
				{
					LocalizationArg_Int32.Create(serverInfo.TotalLevel)
				});
			}
			else
			{
				lobbyPlayerInfo.DisplayedStat = LocalizationPayload.Create("LevelStatNumber", "Global", new LocalizationArg[]
				{
					LocalizationArg_Int32.Create(serverInfo.AccountLevel)
				});
			}
		}
		return lobbyPlayerInfo;
	}
}
