using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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

	public bool ReplacedWithBots { get; set; }
	[JsonIgnore]
	public bool IsRemoteControlled
	{
		get { return ControllingPlayerId != 0; }
	}

	[JsonIgnore]
	public bool IsSpectator
	{
		get { return TeamId == Team.Spectator; }
	}

	[JsonIgnore]
	public CharacterType CharacterType
	{
		get { return CharacterInfo != null ? CharacterInfo.CharacterType : CharacterType.None; }
	}

	[JsonIgnore]
	public bool IsReady
	{
		get { return ReadyState == ReadyState.Ready || IsAIControlled || IsRemoteControlled; }
	}

	[JsonIgnore]
	public bool IsAIControlled
	{
		get { return IsNPCBot || IsLoadTestBot; }
	}

	public LobbyPlayerInfo Clone()
	{
		return (LobbyPlayerInfo)MemberwiseClone();
	}

	public string GetHandle()
	{
		if (IsRemoteControlled)
		{
			return new StringBuilder().Append(StringUtil.TR_CharacterName(CharacterInfo.CharacterType.ToString())).Append(" (").Append(Handle).Append(")").ToString();
		}
		if (IsNPCBot && !BotsMasqueradeAsHumans)
		{
			return StringUtil.TR_CharacterName(CharacterInfo.CharacterType.ToString());
		}
		return Handle;
	}

	public static LobbyPlayerInfo FromServer(LobbyServerPlayerInfo serverInfo, int maxPlayerLevel, MatchmakingQueueConfig queueConfig)
	{
		if (serverInfo == null)
		{
			return null;
		}
		List<LobbyCharacterInfo> list = null;
		if (serverInfo.RemoteCharacterInfos != null)
		{
			list = new List<LobbyCharacterInfo>();
			foreach (LobbyCharacterInfo current in serverInfo.RemoteCharacterInfos)
			{
				list.Add(current.Clone());
			}
		}

		return new LobbyPlayerInfo
		{
			AccountId = serverInfo.AccountId,
			PlayerId = serverInfo.PlayerId,
			CustomGameVisualSlot = serverInfo.CustomGameVisualSlot,
			Handle = serverInfo.Handle,
			TitleID = serverInfo.TitleID,
			TitleLevel = serverInfo.TitleLevel,
			BannerID = serverInfo.BannerID,
			EmblemID = serverInfo.EmblemID,
			RibbonID = serverInfo.RibbonID,
			IsGameOwner = serverInfo.IsGameOwner,
			ReplacedWithBots = serverInfo.ReplacedWithBots,
			IsNPCBot = serverInfo.IsNPCBot,
			IsLoadTestBot = serverInfo.IsLoadTestBot,
			BotsMasqueradeAsHumans = queueConfig != null && queueConfig.BotsMasqueradeAsHumans,
			Difficulty = serverInfo.Difficulty,
			BotCanTaunt = serverInfo.BotCanTaunt,
			TeamId = serverInfo.TeamId,
			CharacterInfo = serverInfo.CharacterInfo != null ? serverInfo.CharacterInfo.Clone() : null,
			RemoteCharacterInfos = list,
			ReadyState = serverInfo.ReadyState,
			ControllingPlayerId = serverInfo.IsRemoteControlled ? serverInfo.ControllingPlayerInfo.PlayerId : 0,
			EffectiveClientAccessLevel = serverInfo.EffectiveClientAccessLevel,
			DisplayedStat = serverInfo.AccountLevel >= maxPlayerLevel
				? LocalizationPayload.Create("TotalSeasonLevelStatNumber", "Global", LocalizationArg_Int32.Create(serverInfo.TotalLevel))
				: LocalizationPayload.Create("LevelStatNumber", "Global", LocalizationArg_Int32.Create(serverInfo.AccountLevel))
		};
	}
}
