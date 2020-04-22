using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Threading;
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

	public LobbyGameInfo GameInfo
	{
		get;
		private set;
	}

	public LobbyTeamInfo TeamInfo
	{
		get;
		private set;
	}

	public LobbyGameConfig GameConfig => GameInfo.GameConfig;

	public LobbyGameplayOverrides GameplayOverrides
	{
		get
		{
			LobbyGameplayOverrides result;
			if (m_gameplayOverridesForCurrentGame != null)
			{
				result = m_gameplayOverridesForCurrentGame;
			}
			else
			{
				result = m_gameplayOverrides;
			}
			return result;
		}
	}

	public LobbyMatchmakingQueueInfo QueueInfo
	{
		get;
		private set;
	}

	public List<LobbyPlayerInfo> TeamPlayerInfo
	{
		get;
		private set;
	}

	public LobbyPlayerInfo PlayerInfo
	{
		get;
		private set;
	}

	public LobbyGameSummary GameSummary
	{
		get;
		private set;
	}

	public LobbyGameSummaryOverrides GameSummaryOverrides
	{
		get;
		private set;
	}

	public bool EnableHiddenGameItems
	{
		get;
		set;
	}

	public GameStatus GameStatus => m_gameStatus;

	public float GameStatusTime
	{
		get;
		private set;
	}

	public event Action OnGameAssembling
	{
		add
		{
			Action action = this.OnGameAssembling;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameAssembling, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnGameAssembling;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameAssembling, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action OnGameSelecting
	{
		add
		{
			Action action = this.OnGameSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameSelecting, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = this.OnGameSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameSelecting, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action OnGameLoadoutSelecting
	{
		add
		{
			Action action = this.OnGameLoadoutSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLoadoutSelecting, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnGameLoadoutSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLoadoutSelecting, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action<GameType> OnGameLaunched
	{
		add
		{
			Action<GameType> action = this.OnGameLaunched;
			Action<GameType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLaunched, (Action<GameType>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameType> action = this.OnGameLaunched;
			Action<GameType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLaunched, (Action<GameType>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action OnGameLoaded
	{
		add
		{
			Action action = this.OnGameLoaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLoaded, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnGameLoaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameLoaded, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnGameStarted
	{
		add
		{
			Action action = this.OnGameStarted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameStarted, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = this.OnGameStarted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameStarted, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action<GameResult> OnGameStopped
	{
		add
		{
			Action<GameResult> action = this.OnGameStopped;
			Action<GameResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameStopped, (Action<GameResult>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameResult> action = this.OnGameStopped;
			Action<GameResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameStopped, (Action<GameResult>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public event Action<GameStatus> OnGameStatusChanged;

	public GameManager()
	{
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = delegate
			{
			};
		}
		this.OnGameAssembling = _003C_003Ef__am_0024cache0;
		this.OnGameSelecting = delegate
		{
		};
		this.OnGameLoadoutSelecting = delegate
		{
		};
		this.OnGameLaunched = delegate
		{
		};
		this.OnGameLoaded = delegate
		{
		};
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = delegate
			{
			};
		}
		this.OnGameStarted = _003C_003Ef__am_0024cache5;
		this.OnGameStopped = delegate
		{
		};
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = delegate
			{
			};
		}
		this.OnGameStatusChanged = _003C_003Ef__am_0024cache7;
		
	}

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
		if (!(GameWideData.Get() != null))
		{
			return;
		}
		while (true)
		{
			GameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			return;
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
		if (s_quitting)
		{
			return;
		}
		while (true)
		{
			if (!notify)
			{
				return;
			}
			while (true)
			{
				if (!GameInfo.GameServerProcessCode.IsNullOrEmpty())
				{
					if (GameInfo.GameConfig != null)
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
				}
				switch (gameStatus)
				{
				case GameStatus.Assembling:
					this.OnGameAssembling();
					break;
				case GameStatus.FreelancerSelecting:
					this.OnGameSelecting();
					break;
				case GameStatus.LoadoutSelecting:
					this.OnGameLoadoutSelecting();
					break;
				case GameStatus.Launched:
					this.OnGameLaunched(GameInfo.GameConfig.GameType);
					break;
				case GameStatus.Loaded:
					this.OnGameLoaded();
					break;
				case GameStatus.Started:
					this.OnGameStarted();
					break;
				case GameStatus.Stopped:
					this.OnGameStopped(gameResult);
					break;
				}
				this.OnGameStatusChanged(gameStatus);
				return;
			}
		}
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
		LobbyGameplayOverrides gameplayOverrides2 = GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		m_gameplayOverrides = gameplayOverrides;
		using (List<string>.Enumerator enumerator = gameplayOverrides2.GetDifferences(GameplayOverrides).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				Log.Notice(current);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	public void SetGameplayOverridesForCurrentGame(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides gameplayOverrides2 = GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		m_gameplayOverridesForCurrentGame = gameplayOverrides;
		using (List<string>.Enumerator enumerator = gameplayOverrides2.GetDifferences(GameplayOverrides).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				Log.Notice(current);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
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

	public bool IsCharacterAllowedForGameType(CharacterType characterType, GameType gameType, GameSubType gameSubType, IFreelancerSetQueryInterface qi)
	{
		return GameplayOverrides.IsCharacterAllowedForGameType(characterType, gameType, gameSubType, qi);
	}

	public bool IsCharacterVisible(CharacterType characterType)
	{
		return GameplayOverrides.IsCharacterVisible(characterType);
	}

	public bool IsGameLoading()
	{
		bool result = false;
		if (GameInfo != null)
		{
			if (GameInfo.GameConfig != null)
			{
				if (GameInfo.GameStatus != GameStatus.Stopped)
				{
					if (GameInfo.GameConfig.GameType != 0)
					{
						if (GameInfo.GameStatus >= GameStatus.Assembling)
						{
							result = true;
						}
					}
					else if (GameInfo.GameStatus.IsPostLaunchStatus())
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool IsGameTypeValidForGGPack(GameType gameType)
	{
		int result;
		if (gameType != GameType.Tutorial)
		{
			if (gameType != GameType.Practice)
			{
				result = ((gameType != GameType.Custom) ? 1 : 0);
				goto IL_0025;
			}
		}
		result = 0;
		goto IL_0025;
		IL_0025:
		return (byte)result != 0;
	}

	public bool IsAllowingPlayerRequestedPause()
	{
		if (GameConfig != null)
		{
			while (true)
			{
				int result;
				switch (5)
				{
				case 0:
					break;
				default:
					{
						if (GameConfig.GameType == GameType.Custom)
						{
							if (GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowPausing))
							{
								goto IL_00cb;
							}
						}
						if (GameConfig.GameType == GameType.Practice || GameConfig.GameType == GameType.Solo)
						{
							goto IL_00cb;
						}
						if (GameConfig.GameType == GameType.Coop)
						{
							if (GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
							{
								goto IL_00cb;
							}
						}
						result = ((GameConfig.GameType == GameType.NewPlayerSolo) ? 1 : 0);
						goto IL_00cc;
					}
					IL_00cc:
					return (byte)result != 0;
					IL_00cb:
					result = 1;
					goto IL_00cc;
				}
			}
		}
		return false;
	}

	public bool IsBotMasqueradingAsHuman(int playerId)
	{
		return TeamInfo.TeamPlayerInfo.Exists(delegate(LobbyPlayerInfo p)
		{
			int result;
			if (p.PlayerId == playerId)
			{
				result = (p.BotsMasqueradeAsHumans ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		});
	}

	public bool IsFreelancerConflictPossible(bool sameTeam)
	{
		if (GameConfig == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					throw new Exception("GameConfig not set");
				}
			}
		}
		if (GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
		{
			return false;
		}
		bool flag = false;
		FreelancerDuplicationRuleTypes freelancerDuplicationRuleTypes = FreelancerDuplicationRuleTypes.byGameType;
		if (GameConfig.HasSelectedSubType)
		{
			flag = GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection);
			freelancerDuplicationRuleTypes = GameConfig.InstanceSubType.DuplicationRule;
		}
		else
		{
			flag = (GameConfig.GameType == GameType.Ranked);
		}
		switch (freelancerDuplicationRuleTypes)
		{
		case FreelancerDuplicationRuleTypes.byGameType:
			return sameTeam || flag;
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
			throw new Exception($"Unhandled FreelancerDuplicationRuleTypes {freelancerDuplicationRuleTypes}");
		}
	}
}
