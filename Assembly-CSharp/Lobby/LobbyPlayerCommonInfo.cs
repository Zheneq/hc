using System;
using System.Collections.Generic;
using UnityEngine.Networking;

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
	public LobbyServerPlayerInfo ControllingPlayerInfo; // TODO not initialized
	public PlayerGameAccountType GameAccountType;
	public PlayerGameConnectionType GameConnectionType;
	public PlayerGameOptionFlag GameOptionFlags;

#if SERVER
	// custom - ControllingPlayerInfo is not initialized
	public bool IsRemoteControlled => ControllingPlayerId != 0;
#else
	// reactor
	public bool IsRemoteControlled => ControllingPlayerInfo != null;
#endif
	
	public bool IsSpectator => TeamId == Team.Spectator;
	public CharacterType CharacterType => CharacterInfo?.CharacterType ?? CharacterType.None;
	public bool IsReady => ReadyState == ReadyState.Ready || IsAIControlled || IsRemoteControlled;
	public bool ReplacedWithBots { get; set; }
	public bool IsAIControlled => IsNPCBot || IsLoadTestBot || ReplacedWithBots;
	public bool IsHumanControlled => !IsAIControlled;

	public bool IsNPCBot
	{
		get => GameAccountType == PlayerGameAccountType.None;
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
		get => GameAccountType == PlayerGameAccountType.LoadTest;
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

#if SERVER
	// added in rogues
	public virtual void Deserialize(NetworkReader reader)
	{
		AccountId = reader.ReadInt64();
		PlayerId = reader.ReadInt32();
		CustomGameVisualSlot = reader.ReadInt32();
		Handle = reader.ReadString();
		TitleID = reader.ReadInt32();
		TitleLevel = reader.ReadInt32();
		BannerID = reader.ReadInt32();
		EmblemID = reader.ReadInt32();
		RibbonID = reader.ReadInt32();
		IsGameOwner = reader.ReadBoolean();
		Difficulty = (BotDifficulty)reader.ReadSByte();
		BotCanTaunt = reader.ReadBoolean();
		TeamId = (Team)reader.ReadSByte();
		AllianceMessageBase.DeserializeObject(out CharacterInfo, reader);
		ReadyState = (ReadyState)reader.ReadSByte();
		ControllingPlayerId = reader.ReadInt32();
		GameAccountType = (PlayerGameAccountType)reader.ReadSByte();
		GameConnectionType = (PlayerGameConnectionType)reader.ReadSByte();
		GameOptionFlags = (PlayerGameOptionFlag)reader.ReadSByte();
	}

	// added in rogues
	public virtual void Serialize(NetworkWriter writer)
	{
		writer.Write(AccountId);
		writer.Write(PlayerId);
		writer.Write(CustomGameVisualSlot);
		writer.Write(Handle);
		writer.Write(TitleID);
		writer.Write(TitleLevel);
		writer.Write(BannerID);
		writer.Write(EmblemID);
		writer.Write(RibbonID);
		writer.Write(IsGameOwner);
		writer.Write((sbyte)Difficulty);
		writer.Write(BotCanTaunt);
		writer.Write((sbyte)TeamId);
		AllianceMessageBase.SerializeObject(CharacterInfo, writer);
		writer.Write((sbyte)ReadyState);
		if (ControllingPlayerId == 0 && ControllingPlayerInfo != null)
		{
			ControllingPlayerId = ControllingPlayerInfo.PlayerId;
		}
		writer.Write(ControllingPlayerId);
		writer.Write((sbyte)GameAccountType);
		writer.Write((sbyte)GameConnectionType);
		writer.Write((sbyte)GameOptionFlags);
	}
#endif
}
