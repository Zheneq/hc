using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LobbyGameClientMessages;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
	private static GameManager s_instance;

	private bool s_quitting;

	private GameStatus m_gameStatus;

	private LobbyGameplayOverrides m_gameplayOverrides;

	private LobbyGameplayOverrides m_gameplayOverridesForCurrentGame;

	public Dictionary<int, ForbiddenDevKnowledge> ForbiddenDevKnowledge;

	public GameManager()
	{
		
		this.OnGameAssemblingHolder = delegate()
			{
			};
		this.OnGameSelectingHolder = delegate()
		{
		};
		this.OnGameLoadoutSelectingHolder = delegate()
		{
		};
		this.OnGameLaunchedHolder = delegate(GameType A_0)
		{
		};
		this.OnGameLoadedHolder = delegate()
		{
		};
		
		this.OnGameStartedHolder = delegate()
			{
			};
		this.OnGameStoppedHolder = delegate(GameResult A_0)
		{
		};
		
		this.OnGameStatusChangedHolder = delegate(GameStatus A_0)
			{
			};
		
	}

	public static GameManager Get()
	{
		return GameManager.s_instance;
	}

	private Action OnGameAssemblingHolder;
	public event Action OnGameAssembling
	{
		add
		{
			Action action = this.OnGameAssemblingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameAssemblingHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameAssemblingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameAssemblingHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGameSelectingHolder;
	public event Action OnGameSelecting
	{
		add
		{
			Action action = this.OnGameSelectingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameSelectingHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameSelectingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameSelectingHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGameLoadoutSelectingHolder;
	public event Action OnGameLoadoutSelecting
	{
		add
		{
			Action action = this.OnGameLoadoutSelectingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadoutSelectingHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameLoadoutSelectingHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadoutSelectingHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameType> OnGameLaunchedHolder;
	public event Action<GameType> OnGameLaunched
	{
		add
		{
			Action<GameType> action = this.OnGameLaunchedHolder;
			Action<GameType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameType>>(ref this.OnGameLaunchedHolder, (Action<GameType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameType> action = this.OnGameLaunchedHolder;
			Action<GameType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameType>>(ref this.OnGameLaunchedHolder, (Action<GameType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGameLoadedHolder;
	public event Action OnGameLoaded
	{
		add
		{
			Action action = this.OnGameLoadedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadedHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameLoadedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadedHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGameStartedHolder;
	public event Action OnGameStarted
	{
		add
		{
			Action action = this.OnGameStartedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameStartedHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameStartedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameStartedHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameResult> OnGameStoppedHolder;
	public event Action<GameResult> OnGameStopped
	{
		add
		{
			Action<GameResult> action = this.OnGameStoppedHolder;
			Action<GameResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameResult>>(ref this.OnGameStoppedHolder, (Action<GameResult>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameResult> action = this.OnGameStoppedHolder;
			Action<GameResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameResult>>(ref this.OnGameStoppedHolder, (Action<GameResult>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<GameStatus> OnGameStatusChangedHolder;
	public event Action<GameStatus> OnGameStatusChanged;

	public LobbyGameInfo GameInfo { get; private set; }

	public LobbyTeamInfo TeamInfo { get; private set; }

	public LobbyGameConfig GameConfig
	{
		get
		{
			return this.GameInfo.GameConfig;
		}
	}

	public LobbyGameplayOverrides GameplayOverrides
	{
		get
		{
			LobbyGameplayOverrides result;
			if (this.m_gameplayOverridesForCurrentGame != null)
			{
				result = this.m_gameplayOverridesForCurrentGame;
			}
			else
			{
				result = this.m_gameplayOverrides;
			}
			return result;
		}
	}

	public LobbyMatchmakingQueueInfo QueueInfo { get; private set; }

	public List<LobbyPlayerInfo> TeamPlayerInfo { get; private set; }

	public LobbyPlayerInfo PlayerInfo { get; private set; }

	public LobbyGameSummary GameSummary { get; private set; }

	public LobbyGameSummaryOverrides GameSummaryOverrides { get; private set; }

	public bool EnableHiddenGameItems { get; set; }

	public GameStatus GameStatus
	{
		get
		{
			return this.m_gameStatus;
		}
	}

	public float GameStatusTime { get; private set; }

	private void Awake()
	{
		GameManager.s_instance = this;
		this.Reset();
	}

	private void Start()
	{
		this.Reset();
	}

	internal static bool IsEditorAndNotGame()
	{
		return Application.isEditor && GameManager.s_instance == null;
	}

	public void Reset()
	{
		this.GameInfo = new LobbyGameInfo();
		this.TeamInfo = new LobbyTeamInfo();
		this.m_gameplayOverrides = new LobbyGameplayOverrides();
		this.m_gameStatus = GameStatus.Stopped;
		this.QueueInfo = null;
		this.ForbiddenDevKnowledge = null;
		if (GameWideData.Get() != null)
		{
			this.GameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
		}
	}

	private void OnDestroy()
	{
		GameManager.s_instance = null;
	}

	private void OnApplicationQuit()
	{
		this.s_quitting = true;
	}

	internal void SetGameStatus(GameStatus gameStatus, GameResult gameResult = GameResult.NoResult, bool notify = true)
	{
		if (gameStatus != this.m_gameStatus)
		{
			this.m_gameStatus = gameStatus;
			this.GameStatusTime = Time.unscaledTime;
			if (!this.s_quitting)
			{
				if (notify)
				{
					if (!this.GameInfo.GameServerProcessCode.IsNullOrEmpty())
					{
						if (this.GameInfo.GameConfig != null)
						{
							if (gameResult == GameResult.NoResult)
							{
								Log.Info("Game {0} is {1}", new object[]
								{
									this.GameInfo.Name,
									gameStatus.ToString().ToLower()
								});
							}
							else
							{
								Log.Info("Game {0} is {1} with result {2}", new object[]
								{
									this.GameInfo.Name,
									gameStatus.ToString().ToLower(),
									gameResult.ToString()
								});
							}
						}
					}
					switch (gameStatus)
					{
					case GameStatus.Assembling:
						this.OnGameAssemblingHolder();
						break;
					case GameStatus.FreelancerSelecting:
						this.OnGameSelectingHolder();
						break;
					case GameStatus.LoadoutSelecting:
						this.OnGameLoadoutSelectingHolder();
						break;
					case GameStatus.Launched:
						this.OnGameLaunchedHolder(this.GameInfo.GameConfig.GameType);
						break;
					case GameStatus.Loaded:
						this.OnGameLoadedHolder();
						break;
					case GameStatus.Started:
						this.OnGameStartedHolder();
						break;
					case GameStatus.Stopped:
						this.OnGameStoppedHolder(gameResult);
						break;
					}
					this.OnGameStatusChangedHolder(gameStatus);
				}
			}
		}
	}

	public void SetGameInfo(LobbyGameInfo gameInfo)
	{
		this.GameInfo = gameInfo;
	}

	public void SetQueueInfo(LobbyMatchmakingQueueInfo queueInfo)
	{
		this.QueueInfo = queueInfo;
	}

	public void SetTeamInfo(LobbyTeamInfo teamInfo)
	{
		this.TeamInfo = teamInfo;
	}

	public void SetPlayerInfo(LobbyPlayerInfo playerInfo)
	{
		this.PlayerInfo = playerInfo;
	}

	public void SetTeamPlayerInfo(List<LobbyPlayerInfo> teamPlayerInfo)
	{
		this.TeamPlayerInfo = teamPlayerInfo;
	}

	public void SetGameSummary(LobbyGameSummary gameSummary)
	{
		this.GameSummary = gameSummary;
	}

	public void SetGameSummaryOverrides(LobbyGameSummaryOverrides gameSummaryOverrides)
	{
		this.GameSummaryOverrides = gameSummaryOverrides;
	}

	public void StopGame(GameResult gameResult = GameResult.NoResult)
	{
		this.SetGameStatus(GameStatus.Stopped, gameResult, true);
		GameTime.scale = 1f;
	}

	public void SetGameplayOverrides(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides gameplayOverrides2 = this.GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		this.m_gameplayOverrides = gameplayOverrides;
		using (List<string>.Enumerator enumerator = gameplayOverrides2.GetDifferences(this.GameplayOverrides).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string message = enumerator.Current;
				Log.Notice(message, new object[0]);
			}
		}
	}

	public void SetGameplayOverridesForCurrentGame(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides gameplayOverrides2 = this.GameplayOverrides;
		if (gameplayOverrides != null)
		{
			gameplayOverrides.SetBaseCharacterConfigs(GameWideData.Get());
			gameplayOverrides.SetFactionConfigs(FactionWideData.Get());
		}
		this.m_gameplayOverridesForCurrentGame = gameplayOverrides;
		using (List<string>.Enumerator enumerator = gameplayOverrides2.GetDifferences(this.GameplayOverrides).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string message = enumerator.Current;
				Log.Notice(message, new object[0]);
			}
		}
	}

	public bool IsCharacterAllowedForPlayers(CharacterType characterType)
	{
		return this.GameplayOverrides.IsCharacterAllowedForPlayers(characterType);
	}

	public bool IsCharacterAllowedForBots(CharacterType characterType)
	{
		return this.GameplayOverrides.IsCharacterAllowedForBots(characterType);
	}

	public bool IsValidForHumanPreGameSelection(CharacterType characterType)
	{
		return this.GameplayOverrides.IsValidForHumanPreGameSelection(characterType);
	}

	public bool IsCharacterAllowedForGameType(CharacterType characterType, GameType gameType, GameSubType gameSubType, IFreelancerSetQueryInterface qi)
	{
		return this.GameplayOverrides.IsCharacterAllowedForGameType(characterType, gameType, gameSubType, qi);
	}

	public bool IsCharacterVisible(CharacterType characterType)
	{
		return this.GameplayOverrides.IsCharacterVisible(characterType);
	}

	public bool IsGameLoading()
	{
		bool result = false;
		if (this.GameInfo != null)
		{
			if (this.GameInfo.GameConfig != null)
			{
				if (this.GameInfo.GameStatus != GameStatus.Stopped)
				{
					if (this.GameInfo.GameConfig.GameType != GameType.Custom)
					{
						if (this.GameInfo.GameStatus >= GameStatus.Assembling)
						{
							result = true;
						}
					}
					else if (this.GameInfo.GameStatus.IsPostLaunchStatus())
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
		if (gameType != GameType.Tutorial)
		{
			if (gameType != GameType.Practice)
			{
				return gameType != GameType.Custom;
			}
		}
		return false;
	}

	public bool IsAllowingPlayerRequestedPause()
	{
		if (this.GameConfig != null)
		{
			if (this.GameConfig.GameType == GameType.Custom)
			{
				if (this.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowPausing))
				{
					goto IL_CB;
				}
			}
			if (this.GameConfig.GameType != GameType.Practice && this.GameConfig.GameType != GameType.Solo)
			{
				if (this.GameConfig.GameType == GameType.Coop)
				{
					if (this.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						goto IL_CB;
					}
				}
				return this.GameConfig.GameType == GameType.NewPlayerSolo;
			}
			IL_CB:
			return true;
		}
		return false;
	}

	public bool IsBotMasqueradingAsHuman(int playerId)
	{
		return this.TeamInfo.TeamPlayerInfo.Exists(delegate(LobbyPlayerInfo p)
		{
			bool result;
			if (p.PlayerId == playerId)
			{
				result = p.BotsMasqueradeAsHumans;
			}
			else
			{
				result = false;
			}
			return result;
		});
	}

	public bool IsFreelancerConflictPossible(bool sameTeam)
	{
		if (this.GameConfig == null)
		{
			throw new Exception("GameConfig not set");
		}
		if (this.GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
		{
			return false;
		}
		FreelancerDuplicationRuleTypes freelancerDuplicationRuleTypes = FreelancerDuplicationRuleTypes.byGameType;
		bool flag;
		if (this.GameConfig.HasSelectedSubType)
		{
			flag = this.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection);
			freelancerDuplicationRuleTypes = this.GameConfig.InstanceSubType.DuplicationRule;
		}
		else
		{
			flag = (this.GameConfig.GameType == GameType.Ranked);
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
			throw new Exception(string.Format("Unhandled FreelancerDuplicationRuleTypes {0}", freelancerDuplicationRuleTypes));
		}
	}

	public class LoginRequest : MessageBase
	{
		public string AccountId;

		public string SessionToken;

		public int PlayerId;

		public uint LastReceivedMsgSeqNum;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(this.AccountId);
			writer.Write(this.SessionToken);
			writer.WritePackedUInt32((uint)this.PlayerId);
			writer.WritePackedUInt32(this.LastReceivedMsgSeqNum);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.AccountId = reader.ReadString();
			this.SessionToken = reader.ReadString();
			this.PlayerId = (int)reader.ReadPackedUInt32();
			this.LastReceivedMsgSeqNum = reader.ReadPackedUInt32();
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
			writer.Write(this.Success);
			writer.Write(this.Reconnecting);
			writer.Write(this.ErrorMessage);
			writer.WritePackedUInt32(this.LastReceivedMsgSeqNum);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.Success = reader.ReadBoolean();
			this.Reconnecting = reader.ReadBoolean();
			this.ErrorMessage = reader.ReadString();
			this.LastReceivedMsgSeqNum = reader.ReadPackedUInt32();
		}
	}

	public class ReplayManagerFile : MessageBase
	{
		public string Fragment;

		public bool Restart;

		public bool Save;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(this.Fragment);
			writer.Write(this.Restart);
			writer.Write(this.Save);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.Fragment = reader.ReadString();
			this.Restart = reader.ReadBoolean();
			this.Save = reader.ReadBoolean();
		}
	}

	public class AssetsLoadedNotification : MessageBase
	{
		public long AccountId;

		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt64((ulong)this.AccountId);
			writer.WritePackedUInt32((uint)this.PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.AccountId = (long)reader.ReadPackedUInt64();
			this.PlayerId = (int)reader.ReadPackedUInt32();
		}
	}

	public class PlayerObjectStartedOnClientNotification : MessageBase
	{
		public long AccountId;

		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt64((ulong)this.AccountId);
			writer.WritePackedUInt32((uint)this.PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.AccountId = (long)reader.ReadPackedUInt64();
			this.PlayerId = (int)reader.ReadPackedUInt32();
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
			writer.WritePackedUInt64((ulong)this.AccountId);
			writer.WritePackedUInt32((uint)this.PlayerId);
			writer.WritePackedUInt32((uint)this.TotalLoadingProgress);
			writer.WritePackedUInt32((uint)this.LevelLoadingProgress);
			writer.WritePackedUInt32((uint)this.CharacterLoadingProgress);
			writer.WritePackedUInt32((uint)this.VfxLoadingProgress);
			writer.WritePackedUInt32((uint)this.SpawningProgress);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.AccountId = (long)reader.ReadPackedUInt64();
			this.PlayerId = (int)reader.ReadPackedUInt32();
			this.TotalLoadingProgress = (byte)reader.ReadPackedUInt32();
			this.LevelLoadingProgress = (byte)reader.ReadPackedUInt32();
			this.CharacterLoadingProgress = (byte)reader.ReadPackedUInt32();
			this.VfxLoadingProgress = (byte)reader.ReadPackedUInt32();
			this.SpawningProgress = (byte)reader.ReadPackedUInt32();
		}
	}

	public class SpawningObjectsNotification : MessageBase
	{
		public int PlayerId;

		public int SpawnableObjectCount;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.PlayerId);
			writer.WritePackedUInt32((uint)this.SpawnableObjectCount);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.PlayerId = (int)reader.ReadPackedUInt32();
			this.SpawnableObjectCount = (int)reader.ReadPackedUInt32();
		}
	}

	public class LeaveGameNotification : MessageBase
	{
		public int PlayerId;

		public bool IsPermanent;

		public GameResult GameResult;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.PlayerId);
			writer.Write(this.IsPermanent);
			writer.Write((int)this.GameResult);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.PlayerId = (int)reader.ReadPackedUInt32();
			this.IsPermanent = reader.ReadBoolean();
			this.GameResult = (GameResult)reader.ReadInt32();
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
			writer.Write(this.WithinReconnectReplay);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.WithinReconnectReplay = reader.ReadBoolean();
		}
	}

	public class ObserverMessage : MessageBase
	{
		public Replay.Message Message;

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteMessage_Replay(writer, this.Message);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.Message = GeneratedNetworkCode._ReadMessage_Replay(reader);
		}
	}

	public class FakeActionRequest : MessageBase
	{
		public int PlayerId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.PlayerId = (int)reader.ReadPackedUInt32();
		}
	}

	public class FakeActionResponse : MessageBase
	{
		public int msgSize;

		public byte[] msgData;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WriteBytesAndSize(this.msgData, this.msgSize);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.msgSize = (int)reader.ReadPackedUInt32();
			this.msgData = reader.ReadBytesAndSize();
		}
	}
}
