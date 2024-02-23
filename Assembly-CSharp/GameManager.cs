﻿using System;
using System.Collections.Generic;
using System.Text;
using LobbyGameClientMessages;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
	public class LoginRequest : MessageBase
	{
		public string AccountId;
		public string SessionToken;
		public int PlayerId;
		public uint LastReceivedMsgSeqNum;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(AccountId);
			writer.Write(SessionToken);
			writer.WritePackedUInt32((uint)PlayerId);
			writer.WritePackedUInt32(LastReceivedMsgSeqNum);
		}

		public override void Deserialize(NetworkReader reader)
		{
			AccountId = reader.ReadString();
			SessionToken = reader.ReadString();
			PlayerId = (int)reader.ReadPackedUInt32();
			LastReceivedMsgSeqNum = reader.ReadPackedUInt32();
		}
	}

	public class LoginResponse : MessageBase
	{
		public bool Success;
		public bool Reconnecting;
		public string ErrorMessage;
		public uint LastReceivedMsgSeqNum;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Success);
			writer.Write(Reconnecting);
			writer.Write(ErrorMessage);
			writer.WritePackedUInt32(LastReceivedMsgSeqNum);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Success = reader.ReadBoolean();
			Reconnecting = reader.ReadBoolean();
			ErrorMessage = reader.ReadString();
			LastReceivedMsgSeqNum = reader.ReadPackedUInt32();
		}
	}

	public class ReplayManagerFile : MessageBase
	{
		public string Fragment;
		public bool Restart;
		public bool Save;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Fragment);
			writer.Write(Restart);
			writer.Write(Save);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Fragment = reader.ReadString();
			Restart = reader.ReadBoolean();
			Save = reader.ReadBoolean();
		}
	}

	public class AssetsLoadedNotification : MessageBase
	{
		public long AccountId;
		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt64((ulong)AccountId);
			writer.WritePackedUInt32((uint)PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			AccountId = (long)reader.ReadPackedUInt64();
			PlayerId = (int)reader.ReadPackedUInt32();
		}
	}

	public class PlayerObjectStartedOnClientNotification : MessageBase
	{
		public long AccountId;
		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt64((ulong)AccountId);
			writer.WritePackedUInt32((uint)PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			AccountId = (long)reader.ReadPackedUInt64();
			PlayerId = (int)reader.ReadPackedUInt32();
		}
	}

	public class AssetsLoadingProgress : MessageBase
	{
		public long AccountId;
		public int PlayerId;
		public byte TotalLoadingProgress;
		public byte LevelLoadingProgress;
		public byte CharacterLoadingProgress;
		public byte VfxLoadingProgress;
		public byte SpawningProgress;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt64((ulong)AccountId);
			writer.WritePackedUInt32((uint)PlayerId);
			writer.WritePackedUInt32(TotalLoadingProgress);
			writer.WritePackedUInt32(LevelLoadingProgress);
			writer.WritePackedUInt32(CharacterLoadingProgress);
			writer.WritePackedUInt32(VfxLoadingProgress);
			writer.WritePackedUInt32(SpawningProgress);
		}

		public override void Deserialize(NetworkReader reader)
		{
			AccountId = (long)reader.ReadPackedUInt64();
			PlayerId = (int)reader.ReadPackedUInt32();
			TotalLoadingProgress = (byte)reader.ReadPackedUInt32();
			LevelLoadingProgress = (byte)reader.ReadPackedUInt32();
			CharacterLoadingProgress = (byte)reader.ReadPackedUInt32();
			VfxLoadingProgress = (byte)reader.ReadPackedUInt32();
			SpawningProgress = (byte)reader.ReadPackedUInt32();
		}
	}

	public class SpawningObjectsNotification : MessageBase
	{
		public int PlayerId;
		public int SpawnableObjectCount;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)PlayerId);
			writer.WritePackedUInt32((uint)SpawnableObjectCount);
		}

		public override void Deserialize(NetworkReader reader)
		{
			PlayerId = (int)reader.ReadPackedUInt32();
			SpawnableObjectCount = (int)reader.ReadPackedUInt32();
		}
	}

	public class LeaveGameNotification : MessageBase
	{
		public int PlayerId;
		public bool IsPermanent;
		public GameResult GameResult;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)PlayerId);
			writer.Write(IsPermanent);
			writer.Write((int)GameResult);
		}

		public override void Deserialize(NetworkReader reader)
		{
			PlayerId = (int)reader.ReadPackedUInt32();
			IsPermanent = reader.ReadBoolean();
			GameResult = (GameResult)reader.ReadInt32();
		}
	}

	public class EndGameNotification : MessageBase
	{
	}

	public class ReconnectReplayStatus : MessageBase
	{
		public bool WithinReconnectReplay;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(WithinReconnectReplay);
		}

		public override void Deserialize(NetworkReader reader)
		{
			WithinReconnectReplay = reader.ReadBoolean();
		}
	}

	public class ObserverMessage : MessageBase
	{
		public Replay.Message Message;

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteMessage_Replay(writer, Message);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Message = GeneratedNetworkCode._ReadMessage_Replay(reader);
		}
	}

	public class FakeActionRequest : MessageBase
	{
		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			PlayerId = (int)reader.ReadPackedUInt32();
		}
	}

	public class FakeActionResponse : MessageBase
	{
		public int msgSize;
		public byte[] msgData;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WriteBytesAndSize(msgData, msgSize);
		}

		public override void Deserialize(NetworkReader reader)
		{
			msgSize = (int)reader.ReadPackedUInt32();
			msgData = reader.ReadBytesAndSize();
		}
	}

	private static GameManager s_instance;
	
	private bool s_quitting;
	private GameStatus m_gameStatus;
	private LobbyGameplayOverrides m_gameplayOverrides;
	private LobbyGameplayOverrides m_gameplayOverridesForCurrentGame;

	public Dictionary<int, ForbiddenDevKnowledge> ForbiddenDevKnowledge;

	public LobbyGameInfo GameInfo { get; private set; }

	public LobbyTeamInfo TeamInfo { get; private set; }

	public LobbyGameConfig GameConfig
	{
		get { return GameInfo.GameConfig; }
	}

	public LobbyGameplayOverrides GameplayOverrides
	{
		get { return m_gameplayOverridesForCurrentGame ?? m_gameplayOverrides; }
	}

	public LobbyMatchmakingQueueInfo QueueInfo { get; private set; }

	public List<LobbyPlayerInfo> TeamPlayerInfo { get; private set; }

	public LobbyPlayerInfo PlayerInfo { get; private set; }

	public LobbyGameSummary GameSummary { get; private set; }

	public LobbyGameSummaryOverrides GameSummaryOverrides { get; private set; }

	public bool EnableHiddenGameItems { get; set; }

	public GameStatus GameStatus
	{
		get { return m_gameStatus; }
	}

	public float GameStatusTime { get; private set; }
	
	public event Action OnGameAssembling = delegate {};
	public event Action OnGameSelecting = delegate {};
	public event Action OnGameLoadoutSelecting = delegate {};
	public event Action<GameType> OnGameLaunched = delegate {};
	public event Action OnGameLoaded = delegate {};
	public event Action OnGameStarted = delegate {};
	public event Action<GameResult> OnGameStopped = delegate {};
	public event Action<GameStatus> OnGameStatusChanged = delegate {};

	public static GameManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		Reset();
	}

	private void Start()
	{
		Reset();
	}

	internal static bool IsEditorAndNotGame()
	{
		return Application.isEditor && s_instance == null;
	}

	public void Reset()
	{
		GameInfo = new LobbyGameInfo();
		TeamInfo = new LobbyTeamInfo();
		m_gameplayOverrides = new LobbyGameplayOverrides();
		m_gameStatus = GameStatus.Stopped;
		QueueInfo = null;
		ForbiddenDevKnowledge = null;
		if (GameWideData.Get() != null)
		{
			GameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void OnApplicationQuit()
	{
		s_quitting = true;
	}

	internal void SetGameStatus(GameStatus gameStatus, GameResult gameResult = GameResult.NoResult, bool notify = true)
	{
		if (gameStatus == m_gameStatus)
		{
			return;
		}
		m_gameStatus = gameStatus;
		GameStatusTime = Time.unscaledTime;
		if (s_quitting || !notify)
		{
			return;
		}
		if (!GameInfo.GameServerProcessCode.IsNullOrEmpty() && GameInfo.GameConfig != null)
		{
			if (gameResult == GameResult.NoResult)
			{
				Log.Info("Game {0} is {1}", GameInfo.Name, gameStatus.ToString().ToLower());
			}
			else
			{
				Log.Info("Game {0} is {1} with result {2}", GameInfo.Name, gameStatus.ToString().ToLower(), gameResult.ToString());
			}
		}
		switch (gameStatus)
		{
			case GameStatus.Assembling:
				OnGameAssembling();
				break;
			case GameStatus.FreelancerSelecting:
				OnGameSelecting();
				break;
			case GameStatus.LoadoutSelecting:
				OnGameLoadoutSelecting();
				break;
			case GameStatus.Launched:
				OnGameLaunched(GameInfo.GameConfig.GameType);
				break;
			case GameStatus.Loaded:
				OnGameLoaded();
				break;
			case GameStatus.Started:
				OnGameStarted();
				break;
			case GameStatus.Stopped:
				OnGameStopped(gameResult);
				break;
		}
		OnGameStatusChanged(gameStatus);
	}

	public void SetGameInfo(LobbyGameInfo gameInfo)
	{
		GameInfo = gameInfo;
	}

	public void SetQueueInfo(LobbyMatchmakingQueueInfo queueInfo)
	{
		QueueInfo = queueInfo;
	}

	public void SetTeamInfo(LobbyTeamInfo teamInfo)
	{
		TeamInfo = teamInfo;
	}

	public void SetPlayerInfo(LobbyPlayerInfo playerInfo)
	{
		PlayerInfo = playerInfo;
	}

	public void SetTeamPlayerInfo(List<LobbyPlayerInfo> teamPlayerInfo)
	{
		TeamPlayerInfo = teamPlayerInfo;
	}

	public void SetGameSummary(LobbyGameSummary gameSummary)
	{
		GameSummary = gameSummary;
	}

	public void SetGameSummaryOverrides(LobbyGameSummaryOverrides gameSummaryOverrides)
	{
		GameSummaryOverrides = gameSummaryOverrides;
	}

	public void StopGame(GameResult gameResult = GameResult.NoResult)
	{
		SetGameStatus(GameStatus.Stopped, gameResult);
		GameTime.scale = 1f;
	}

	public void SetGameplayOverrides(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides oldOverrides = GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		m_gameplayOverrides = gameplayOverrides;
		foreach (string message in oldOverrides.GetDifferences(GameplayOverrides))
		{
			Log.Notice(message);
		}
	}

	public void SetGameplayOverridesForCurrentGame(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides oldGameplayOverrides = GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		m_gameplayOverridesForCurrentGame = gameplayOverrides;
		foreach (string message in oldGameplayOverrides.GetDifferences(GameplayOverrides))
		{
			Log.Notice(message);
		}
	}

	public bool IsCharacterAllowedForPlayers(CharacterType characterType)
	{
		return GameplayOverrides.IsCharacterAllowedForPlayers(characterType);
	}

	public bool IsCharacterAllowedForBots(CharacterType characterType)
	{
		return GameplayOverrides.IsCharacterAllowedForBots(characterType);
	}

	public bool IsValidForHumanPreGameSelection(CharacterType characterType)
	{
		return GameplayOverrides.IsValidForHumanPreGameSelection(characterType);
	}

	public bool IsCharacterAllowedForGameType(
		CharacterType characterType, GameType gameType, GameSubType gameSubType, IFreelancerSetQueryInterface qi)
	{
		return GameplayOverrides.IsCharacterAllowedForGameType(characterType, gameType, gameSubType, qi);
	}

	public bool IsCharacterVisible(CharacterType characterType)
	{
		return GameplayOverrides.IsCharacterVisible(characterType);
	}

	public bool IsGameLoading()
	{
		return GameInfo != null
		       && GameInfo.GameConfig != null
		       && GameInfo.GameStatus != GameStatus.Stopped
		       && (GameInfo.GameConfig.GameType != GameType.Custom
			       ? GameInfo.GameStatus >= GameStatus.Assembling
			       : GameInfo.GameStatus.IsPostLaunchStatus());
	}

	public static bool IsGameTypeValidForGGPack(GameType gameType)
	{
		return gameType != GameType.Tutorial
		       && gameType != GameType.Practice
		       && gameType != GameType.Custom;
	}

	public bool IsAllowingPlayerRequestedPause()
	{
		if (GameConfig == null)
		{
			return false;
		}
		if (GameConfig.GameType == GameType.Custom && GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowPausing))
		{
			return true;
		}
		if (GameConfig.GameType == GameType.Practice || GameConfig.GameType == GameType.Solo)
		{
			return true;
		}
		if (GameConfig.GameType == GameType.Coop && GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
		{
			return true;
		}
		return GameConfig.GameType == GameType.NewPlayerSolo;
	}

	public bool IsBotMasqueradingAsHuman(int playerId)
	{
		return TeamInfo.TeamPlayerInfo.Exists(p => p.PlayerId == playerId && p.BotsMasqueradeAsHumans);
	}

	public bool IsFreelancerConflictPossible(bool sameTeam)
	{
		if (GameConfig == null)
		{
			throw new Exception("GameConfig not set");
		}
		if (GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
		{
			return false;
		}
		bool isRankedSelection;
		FreelancerDuplicationRuleTypes freelancerDuplicationRuleTypes = FreelancerDuplicationRuleTypes.byGameType;
		if (GameConfig.HasSelectedSubType)
		{
			isRankedSelection = GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection);
			freelancerDuplicationRuleTypes = GameConfig.InstanceSubType.DuplicationRule;
		}
		else
		{
			isRankedSelection = GameConfig.GameType == GameType.Ranked;
		}
		switch (freelancerDuplicationRuleTypes)
		{
			case FreelancerDuplicationRuleTypes.byGameType:
				return sameTeam || isRankedSelection;
			case FreelancerDuplicationRuleTypes.noneInGame:
				return true;
			case FreelancerDuplicationRuleTypes.noneInTeam:
				return sameTeam;
			case FreelancerDuplicationRuleTypes.allow:
				return false;
			case FreelancerDuplicationRuleTypes.alwaysDupAcrossTeam:
				return !sameTeam;
			case FreelancerDuplicationRuleTypes.alwaysDupAcrossGame:
				return false;
			default:
				throw new Exception(new StringBuilder().Append("Unhandled FreelancerDuplicationRuleTypes ").Append(freelancerDuplicationRuleTypes).ToString());
		}
	}
}
