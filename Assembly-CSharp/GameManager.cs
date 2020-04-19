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
		if (GameManager.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager..ctor()).MethodHandle;
			}
			GameManager.<>f__am$cache0 = delegate()
			{
			};
		}
		this.OnGameAssembling = GameManager.<>f__am$cache0;
		this.OnGameSelecting = delegate()
		{
		};
		this.OnGameLoadoutSelecting = delegate()
		{
		};
		this.OnGameLaunched = delegate(GameType A_0)
		{
		};
		this.OnGameLoaded = delegate()
		{
		};
		if (GameManager.<>f__am$cache5 == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			GameManager.<>f__am$cache5 = delegate()
			{
			};
		}
		this.OnGameStarted = GameManager.<>f__am$cache5;
		this.OnGameStopped = delegate(GameResult A_0)
		{
		};
		if (GameManager.<>f__am$cache7 == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			GameManager.<>f__am$cache7 = delegate(GameStatus A_0)
			{
			};
		}
		this.OnGameStatusChanged = GameManager.<>f__am$cache7;
		base..ctor();
	}

	public static GameManager Get()
	{
		return GameManager.s_instance;
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
				action = Interlocked.CompareExchange<Action>(ref this.OnGameAssembling, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.add_OnGameAssembling(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnGameAssembling;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameAssembling, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameAssembling(Action)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action>(ref this.OnGameSelecting, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameSelecting, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameSelecting(Action)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadoutSelecting, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.add_OnGameLoadoutSelecting(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnGameLoadoutSelecting;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoadoutSelecting, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameLoadoutSelecting(Action)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action<GameType>>(ref this.OnGameLaunched, (Action<GameType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.add_OnGameLaunched(Action<GameType>)).MethodHandle;
			}
		}
		remove
		{
			Action<GameType> action = this.OnGameLaunched;
			Action<GameType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameType>>(ref this.OnGameLaunched, (Action<GameType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameLaunched(Action<GameType>)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoaded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.add_OnGameLoaded(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnGameLoaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameLoaded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action>(ref this.OnGameStarted, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGameStarted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGameStarted, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameStarted(Action)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action<GameResult>>(ref this.OnGameStopped, (Action<GameResult>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.add_OnGameStopped(Action<GameResult>)).MethodHandle;
			}
		}
		remove
		{
			Action<GameResult> action = this.OnGameStopped;
			Action<GameResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameResult>>(ref this.OnGameStopped, (Action<GameResult>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.remove_OnGameStopped(Action<GameResult>)).MethodHandle;
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.get_GameplayOverrides()).MethodHandle;
				}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.Reset()).MethodHandle;
			}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.SetGameStatus(GameStatus, GameResult, bool)).MethodHandle;
				}
				if (notify)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.GameInfo.GameServerProcessCode.IsNullOrEmpty())
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.GameInfo.GameConfig != null)
						{
							if (gameResult == GameResult.NoResult)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
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
						this.OnGameAssembling();
						break;
					case GameStatus.FreelancerSelecting:
						this.OnGameSelecting();
						break;
					case GameStatus.LoadoutSelecting:
						this.OnGameLoadoutSelecting();
						break;
					case GameStatus.Launched:
						this.OnGameLaunched(this.GameInfo.GameConfig.GameType);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.SetGameplayOverrides(LobbyGameplayOverrides)).MethodHandle;
			}
		}
	}

	public void SetGameplayOverridesForCurrentGame(LobbyGameplayOverrides gameplayOverrides)
	{
		LobbyGameplayOverrides gameplayOverrides2 = this.GameplayOverrides;
		if (gameplayOverrides != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.SetGameplayOverridesForCurrentGame(LobbyGameplayOverrides)).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.IsGameLoading()).MethodHandle;
			}
			if (this.GameInfo.GameConfig != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GameInfo.GameStatus != GameStatus.Stopped)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.GameInfo.GameConfig.GameType != GameType.Custom)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.GameInfo.GameStatus >= GameStatus.Assembling)
						{
							result = true;
						}
					}
					else if (this.GameInfo.GameStatus.IsPostLaunchStatus())
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.IsGameTypeValidForGGPack(GameType)).MethodHandle;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.IsAllowingPlayerRequestedPause()).MethodHandle;
			}
			if (this.GameConfig.GameType == GameType.Custom)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GameConfig.GameOptionFlags.HasGameOption(GameOptionFlag.AllowPausing))
				{
					goto IL_CB;
				}
			}
			if (this.GameConfig.GameType != GameType.Practice && this.GameConfig.GameType != GameType.Solo)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GameConfig.GameType == GameType.Coop)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						goto IL_CB;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.<IsBotMasqueradingAsHuman>c__AnonStorey0.<>m__0(LobbyPlayerInfo)).MethodHandle;
				}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameManager.IsFreelancerConflictPossible(bool)).MethodHandle;
			}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
