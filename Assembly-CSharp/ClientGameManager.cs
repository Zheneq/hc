using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class ClientGameManager : MonoBehaviour
{
	private static ClientGameManager s_instance;

	public Dictionary<GameType, GameTypeAvailability> GameTypeAvailabilies;

	public Dictionary<CharacterType, RequirementCollection> FreeRotationAdditions;

	private List<LocalizationPayload> m_tierInstanceNames;

	private DateTime OurQueueEntryTime;

	private bool m_preventNextAccountStatusCheck;

	private bool DisplayedMOTDPopup;

	public Dictionary<GameType, ushort> SoloSubTypeMask;

	private string m_replayManagerAccumulated;

	private float m_lastLoadProgressUpdateSent;

	private float m_loadingProgressUpdateFrequency;

	private ClientGameManager.ClientGameManagerRequirementSystemInfo m_queueRequirementSystemInfo;

	private ClientGameManager.OurQueueApplicant m_ourQueueApplicant;

	private ClientGameManager.GroupQueueApplicant m_scratchGroupQueueApplicant;

	private LobbyGameClientInterface m_lobbyGameClientInterface;

	private bool m_registeredHandlers;

	private bool m_loading;

	private bool m_loadLevelOperationDone;

	private int m_loadedCharacterResourceCount;

	private bool m_loadingCharacterAssets;

	private int m_spawnableObjectCount;

	private AssetsLoadingState m_assetsLoadingState;

	private bool m_withinReconnectReplay;

	private bool m_withinReconnect;

	private bool m_withinReconnectInstantly;

	private uint m_lastReceivedMsgSeqNum;

	private uint m_lastSentMsgSeqNum;

	private Replay m_replay;

	private GameResult m_gameResult;

	private bool m_reconnected;

	private bool m_observer;

	private ClientAccessLevel m_clientAccessLevel;

	private AssetBundleManager.LoadSceneAsyncOperation m_loadLevelOperation;

	private List<KeyValuePair<string, string>> m_loadLevelOperationBundleSceneNames;

	private List<CharacterResourceLink> m_loadingCharacterResources;

	private PersistedAccountData m_loadedPlayerAccountData;

	private Dictionary<CharacterType, PersistedCharacterData> m_loadedPlayerCharacterData;

	private Dictionary<CharacterType, PersistedCharacterData> m_characterDataOnInitialLoad;

	private UIPartyInvitePopDialogBox m_currentGroupSuggestDialogBox;

	private UIPartyInvitePopDialogBox m_currentJoinGroupDialogBox;

	private bool m_discordJoinSuggested;

	private bool m_discordConnecting;

	private DiscordJoinType m_discordJoinType;

	private Scheduler m_taskScheduler;

	private Action m_clientPerformanceCollectTask;

	public ClientGameManager()
	{
		
		this.OnConnectedToLobbyServerHolder = delegate(RegisterGameClientResponse A_0)
			{
			};
		
		this.OnDisconnectedFromLobbyServerHolder = delegate(string A_0)
			{
			};
		
		this.OnLobbyServerReadyNotificationHolder = delegate(LobbyServerReadyNotification A_0)
			{
			};
		
		this.OnLobbyStatusNotificationHolder = delegate(LobbyStatusNotification A_0)
			{
			};
		
		this.OnLobbyCustomGamesNotificationHolder = delegate(LobbyCustomGamesNotification A_0)
			{
			};
		this.OnQueueAssignmentNotificationHolder = delegate(MatchmakingQueueAssignmentNotification A_0)
		{
		};
		this.OnQueueStatusNotificationHolder = delegate(MatchmakingQueueStatusNotification A_0)
		{
		};
		
		this.OnQueueEnteredHolder = delegate()
			{
			};
		
		this.OnQueueLeftHolder = delegate()
			{
			};
		
		this.OnGameAssignmentNotificationHolder = delegate(GameAssignmentNotification A_0)
			{
			};
		
		this.OnGameInfoNotificationHolder = delegate(GameInfoNotification A_0)
			{
			};
		
		this.OnLobbyServerLockStateChangeHolder = delegate(ServerLockState A_0, ServerLockState A_1)
			{
			};
		
		this.OnLobbyServerClientAccessLevelChangeHolder = delegate(ClientAccessLevel A_0, ClientAccessLevel A_1)
			{
			};
		this.OnLobbyGameplayOverridesChangeHolder = delegate(LobbyGameplayOverrides A_0)
		{
		};
		
		this.OnBankBalanceChangeHolder = delegate(CurrencyData A_0)
			{
			};
		
		this.OnModUnlockedHolder = delegate(CharacterType A_0, PlayerModData A_1)
			{
			};
		
		this.OnAccountDataUpdatedHolder = delegate(PersistedAccountData A_0)
			{
			};
		
		this.OnCharacterDataUpdatedHolder = delegate(PersistedCharacterData A_0)
			{
			};
		
		this.OnInventoryComponentUpdatedHolder = delegate(InventoryComponent A_0)
			{
			};
		
		this.OnChatNotificationHolder = delegate(ChatNotification A_0)
			{
			};
		
		this.OnSetDevTagResponseHolder = delegate(SetDevTagResponse A_0)
			{
			};
		this.OnUseOverconNotificationHolder = delegate(UseOverconResponse A_0)
		{
		};
		
		this.OnUseGGPackNotificationHolder = delegate(UseGGPackNotification A_0)
			{
			};
		
		this.OnGroupUpdateNotificationHolder = delegate()
			{
			};
		
		this.OnFriendStatusNotificationHolder = delegate(FriendStatusNotification A_0)
			{
			};
		
		this.OnPlayerTitleChangeHolder = delegate(string A_0)
			{
			};
		
		this.OnPlayerBannerChangeHolder = delegate(GameBalanceVars.PlayerBanner A_0, GameBalanceVars.PlayerBanner A_1)
			{
			};
		this.OnPlayerRibbonChangeHolder = delegate(GameBalanceVars.PlayerRibbon A_0)
		{
		};
		
		this.OnLoadingScreenBackgroundToggledHolder = delegate(int A_0, bool A_1)
			{
			};
		this.OnQuestCompleteNotificationHolder = delegate(QuestCompleteNotification A_0)
		{
		};
		
		this.OnMatchResultsNotificationHolder = delegate(MatchResultsNotification A_0)
			{
			};
		
		this.OnChapterUnlockNotificationHolder = delegate(int A_0, int A_1)
			{
			};
		
		this.OnServerQueueConfigurationUpdateNotificationHolder = delegate(ServerQueueConfigurationUpdateNotification A_0)
			{
			};
		
		this.OnSeasonCompleteNotificationHolder = delegate(SeasonStatusNotification A_0)
			{
			};
		this.OnChapterCompleteNotificationHolder = delegate(int A_0, int A_1)
		{
		};
		this.OnFactionCompetitionNotificationHolder = delegate(FactionCompetitionNotification A_0)
		{
		};
		this.OnTrustBoostUsedNotificationHolder = delegate(TrustBoostUsedNotification A_0)
		{
		};
		
		this.OnPlayerFactionContributionChangeNotificationHolder = delegate(PlayerFactionContributionChangeNotification A_0)
			{
			};
		this.OnFactionLoginRewardNotificationHolder = delegate(FactionLoginRewardNotification A_0)
		{
		};
		
		this.OnQuestProgressChangedHolder = delegate(QuestProgress[] A_0)
			{
			};
		this.OnAlertMissionDataChangeHolder = delegate(LobbyAlertMissionDataNotification A_0)
		{
		};
		this.OnSeasonChapterQuestsChangeHolder = delegate(Dictionary<int, SeasonChapterQuests> A_0)
		{
		};
		this.OurQueueEntryTime = DateTime.MinValue;
		this.SoloSubTypeMask = new Dictionary<GameType, ushort>();
		this.m_loadingProgressUpdateFrequency = 0.5f;
		
	}

	public static ClientGameManager Get()
	{
		return ClientGameManager.s_instance;
	}

	public LobbyGameClientInterface LobbyInterface
	{
		get
		{
			return this.m_lobbyGameClientInterface;
		}
	}

	public LobbySessionInfo SessionInfo
	{
		get
		{
			return (this.m_lobbyGameClientInterface == null) ? null : this.m_lobbyGameClientInterface.SessionInfo;
		}
	}

	public string Handle
	{
		get
		{
			return (this.m_lobbyGameClientInterface == null || this.m_lobbyGameClientInterface.SessionInfo == null) ? null : this.m_lobbyGameClientInterface.SessionInfo.Handle;
		}
	}

	public long AccountId
	{
		get
		{
			if (this.m_lobbyGameClientInterface != null)
			{
				if (this.m_lobbyGameClientInterface.SessionInfo != null)
				{
					return this.m_lobbyGameClientInterface.SessionInfo.AccountId;
				}
			}
			return -1L;
		}
	}

	private Action<RegisterGameClientResponse> OnConnectedToLobbyServerHolder;
	public event Action<RegisterGameClientResponse> OnConnectedToLobbyServer
	{
		add
		{
			Action<RegisterGameClientResponse> action = this.OnConnectedToLobbyServerHolder;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RegisterGameClientResponse>>(ref this.OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<RegisterGameClientResponse> action = this.OnConnectedToLobbyServerHolder;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RegisterGameClientResponse>>(ref this.OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<string> OnDisconnectedFromLobbyServerHolder;
	public event Action<string> OnDisconnectedFromLobbyServer
	{
		add
		{
			Action<string> action = this.OnDisconnectedFromLobbyServerHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = this.OnDisconnectedFromLobbyServerHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyServerReadyNotification> OnLobbyServerReadyNotificationHolder;
	public event Action<LobbyServerReadyNotification> OnLobbyServerReadyNotification
	{
		add
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotificationHolder;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyServerReadyNotification>>(ref this.OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotificationHolder;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyServerReadyNotification>>(ref this.OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyStatusNotification> OnLobbyStatusNotificationHolder;
	public event Action<LobbyStatusNotification> OnLobbyStatusNotification
	{
		add
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotificationHolder;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyStatusNotification>>(ref this.OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotificationHolder;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyStatusNotification>>(ref this.OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotificationHolder;
	public event Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotification
	{
		add
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotificationHolder;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyCustomGamesNotification>>(ref this.OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotificationHolder;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyCustomGamesNotification>>(ref this.OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<MatchmakingQueueAssignmentNotification> OnQueueAssignmentNotificationHolder;
	public event Action<MatchmakingQueueAssignmentNotification> OnQueueAssignmentNotification;

	private Action<MatchmakingQueueStatusNotification> OnQueueStatusNotificationHolder;
	public event Action<MatchmakingQueueStatusNotification> OnQueueStatusNotification
	{
		add
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotificationHolder;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueStatusNotification>>(ref this.OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotificationHolder;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueStatusNotification>>(ref this.OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnQueueEnteredHolder;
	public event Action OnQueueEntered
	{
		add
		{
			Action action = this.OnQueueEnteredHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnQueueEnteredHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnQueueEnteredHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnQueueEnteredHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnQueueLeftHolder;
	public event Action OnQueueLeft
	{
		add
		{
			Action action = this.OnQueueLeftHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnQueueLeftHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnQueueLeftHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnQueueLeftHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameAssignmentNotification> OnGameAssignmentNotificationHolder;
	public event Action<GameAssignmentNotification> OnGameAssignmentNotification
	{
		add
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotificationHolder;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameAssignmentNotification>>(ref this.OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotificationHolder;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameAssignmentNotification>>(ref this.OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameInfoNotification> OnGameInfoNotificationHolder;
	public event Action<GameInfoNotification> OnGameInfoNotification
	{
		add
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotificationHolder;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameInfoNotification>>(ref this.OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotificationHolder;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameInfoNotification>>(ref this.OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ServerLockState, ServerLockState> OnLobbyServerLockStateChangeHolder;
	public event Action<ServerLockState, ServerLockState> OnLobbyServerLockStateChange
	{
		add
		{
			Action<ServerLockState, ServerLockState> action = this.OnLobbyServerLockStateChangeHolder;
			Action<ServerLockState, ServerLockState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerLockState, ServerLockState>>(ref this.OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ServerLockState, ServerLockState> action = this.OnLobbyServerLockStateChangeHolder;
			Action<ServerLockState, ServerLockState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerLockState, ServerLockState>>(ref this.OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ClientAccessLevel, ClientAccessLevel> OnLobbyServerClientAccessLevelChangeHolder;
	public event Action<ClientAccessLevel, ClientAccessLevel> OnLobbyServerClientAccessLevelChange
	{
		add
		{
			Action<ClientAccessLevel, ClientAccessLevel> action = this.OnLobbyServerClientAccessLevelChangeHolder;
			Action<ClientAccessLevel, ClientAccessLevel> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ClientAccessLevel, ClientAccessLevel>>(ref this.OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ClientAccessLevel, ClientAccessLevel> action = this.OnLobbyServerClientAccessLevelChangeHolder;
			Action<ClientAccessLevel, ClientAccessLevel> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ClientAccessLevel, ClientAccessLevel>>(ref this.OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyGameplayOverrides> OnLobbyGameplayOverridesChangeHolder;
	public event Action<LobbyGameplayOverrides> OnLobbyGameplayOverridesChange
	{
		add
		{
			Action<LobbyGameplayOverrides> action = this.OnLobbyGameplayOverridesChangeHolder;
			Action<LobbyGameplayOverrides> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyGameplayOverrides>>(ref this.OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyGameplayOverrides> action = this.OnLobbyGameplayOverridesChangeHolder;
			Action<LobbyGameplayOverrides> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyGameplayOverrides>>(ref this.OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<CurrencyData> OnBankBalanceChangeHolder;
	public event Action<CurrencyData> OnBankBalanceChange
	{
		add
		{
			Action<CurrencyData> action = this.OnBankBalanceChangeHolder;
			Action<CurrencyData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<CurrencyData>>(ref this.OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<CurrencyData> action = this.OnBankBalanceChangeHolder;
			Action<CurrencyData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<CurrencyData>>(ref this.OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<CharacterType, PlayerModData> OnModUnlockedHolder;
	public event Action<CharacterType, PlayerModData> OnModUnlocked;

	private Action<PersistedAccountData> OnAccountDataUpdatedHolder;
	public event Action<PersistedAccountData> OnAccountDataUpdated
	{
		add
		{
			Action<PersistedAccountData> action = this.OnAccountDataUpdatedHolder;
			Action<PersistedAccountData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PersistedAccountData>>(ref this.OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PersistedAccountData> action = this.OnAccountDataUpdatedHolder;
			Action<PersistedAccountData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PersistedAccountData>>(ref this.OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<PersistedCharacterData> OnCharacterDataUpdatedHolder;
	public event Action<PersistedCharacterData> OnCharacterDataUpdated
	{
		add
		{
			Action<PersistedCharacterData> action = this.OnCharacterDataUpdatedHolder;
			Action<PersistedCharacterData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PersistedCharacterData>>(ref this.OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PersistedCharacterData> action = this.OnCharacterDataUpdatedHolder;
			Action<PersistedCharacterData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PersistedCharacterData>>(ref this.OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<InventoryComponent> OnInventoryComponentUpdatedHolder;
	public event Action<InventoryComponent> OnInventoryComponentUpdated
	{
		add
		{
			Action<InventoryComponent> action = this.OnInventoryComponentUpdatedHolder;
			Action<InventoryComponent> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<InventoryComponent>>(ref this.OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<InventoryComponent> action = this.OnInventoryComponentUpdatedHolder;
			Action<InventoryComponent> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<InventoryComponent>>(ref this.OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ChatNotification> OnChatNotificationHolder;
	public event Action<ChatNotification> OnChatNotification
	{
		add
		{
			Action<ChatNotification> action = this.OnChatNotificationHolder;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChatNotification>>(ref this.OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ChatNotification> action = this.OnChatNotificationHolder;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChatNotification>>(ref this.OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<SetDevTagResponse> OnSetDevTagResponseHolder;
	public event Action<SetDevTagResponse> OnSetDevTagResponse
	{
		add
		{
			Action<SetDevTagResponse> action = this.OnSetDevTagResponseHolder;
			Action<SetDevTagResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SetDevTagResponse>>(ref this.OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<SetDevTagResponse> action = this.OnSetDevTagResponseHolder;
			Action<SetDevTagResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SetDevTagResponse>>(ref this.OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<UseOverconResponse> OnUseOverconNotificationHolder;
	public event Action<UseOverconResponse> OnUseOverconNotification
	{
		add
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotificationHolder;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseOverconResponse>>(ref this.OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotificationHolder;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseOverconResponse>>(ref this.OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<UseGGPackNotification> OnUseGGPackNotificationHolder;
	public event Action<UseGGPackNotification> OnUseGGPackNotification
	{
		add
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotificationHolder;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseGGPackNotification>>(ref this.OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotificationHolder;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseGGPackNotification>>(ref this.OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGroupUpdateNotificationHolder;
	public event Action OnGroupUpdateNotification
	{
		add
		{
			Action action = this.OnGroupUpdateNotificationHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGroupUpdateNotificationHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnGroupUpdateNotificationHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnGroupUpdateNotificationHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<FriendStatusNotification> OnFriendStatusNotificationHolder;
	public event Action<FriendStatusNotification> OnFriendStatusNotification
	{
		add
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotificationHolder;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FriendStatusNotification>>(ref this.OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotificationHolder;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FriendStatusNotification>>(ref this.OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<string> OnPlayerTitleChangeHolder;
	public event Action<string> OnPlayerTitleChange
	{
		add
		{
			Action<string> action = this.OnPlayerTitleChangeHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnPlayerTitleChangeHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = this.OnPlayerTitleChangeHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnPlayerTitleChangeHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> OnPlayerBannerChangeHolder;
	public event Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> OnPlayerBannerChange
	{
		add
		{
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action = this.OnPlayerBannerChangeHolder;
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>>(ref this.OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action = this.OnPlayerBannerChangeHolder;
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>>(ref this.OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameBalanceVars.PlayerRibbon> OnPlayerRibbonChangeHolder;
	public event Action<GameBalanceVars.PlayerRibbon> OnPlayerRibbonChange
	{
		add
		{
			Action<GameBalanceVars.PlayerRibbon> action = this.OnPlayerRibbonChangeHolder;
			Action<GameBalanceVars.PlayerRibbon> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameBalanceVars.PlayerRibbon>>(ref this.OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameBalanceVars.PlayerRibbon> action = this.OnPlayerRibbonChangeHolder;
			Action<GameBalanceVars.PlayerRibbon> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameBalanceVars.PlayerRibbon>>(ref this.OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<int, bool> OnLoadingScreenBackgroundToggledHolder;
	public event Action<int, bool> OnLoadingScreenBackgroundToggled
	{
		add
		{
			Action<int, bool> action = this.OnLoadingScreenBackgroundToggledHolder;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<int, bool>>(ref this.OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int, bool> action = this.OnLoadingScreenBackgroundToggledHolder;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<int, bool>>(ref this.OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<QuestCompleteNotification> OnQuestCompleteNotificationHolder;
	public event Action<QuestCompleteNotification> OnQuestCompleteNotification
	{
		add
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotificationHolder;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestCompleteNotification>>(ref this.OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotificationHolder;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestCompleteNotification>>(ref this.OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<MatchResultsNotification> OnMatchResultsNotificationHolder;
	public event Action<MatchResultsNotification> OnMatchResultsNotification
	{
		add
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotificationHolder;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchResultsNotification>>(ref this.OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotificationHolder;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchResultsNotification>>(ref this.OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<int, int> OnChapterUnlockNotificationHolder;
	public event Action<int, int> OnChapterUnlockNotification
	{
		add
		{
			Action<int, int> action = this.OnChapterUnlockNotificationHolder;
			Action<int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<int, int>>(ref this.OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int, int> action = this.OnChapterUnlockNotificationHolder;
			Action<int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<int, int>>(ref this.OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotificationHolder;
	public event Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotification
	{
		add
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotificationHolder;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerQueueConfigurationUpdateNotification>>(ref this.OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotificationHolder;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerQueueConfigurationUpdateNotification>>(ref this.OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<SeasonStatusNotification> OnSeasonCompleteNotificationHolder;
	public event Action<SeasonStatusNotification> OnSeasonCompleteNotification
	{
		add
		{
			Action<SeasonStatusNotification> action = this.OnSeasonCompleteNotificationHolder;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SeasonStatusNotification>>(ref this.OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<SeasonStatusNotification> action = this.OnSeasonCompleteNotificationHolder;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SeasonStatusNotification>>(ref this.OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<int, int> OnChapterCompleteNotificationHolder;
	public event Action<int, int> OnChapterCompleteNotification;

	private Action<FactionCompetitionNotification> OnFactionCompetitionNotificationHolder;
	public event Action<FactionCompetitionNotification> OnFactionCompetitionNotification
	{
		add
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotificationHolder;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionCompetitionNotification>>(ref this.OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotificationHolder;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionCompetitionNotification>>(ref this.OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<TrustBoostUsedNotification> OnTrustBoostUsedNotificationHolder;
	public event Action<TrustBoostUsedNotification> OnTrustBoostUsedNotification
	{
		add
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotificationHolder;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TrustBoostUsedNotification>>(ref this.OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotificationHolder;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TrustBoostUsedNotification>>(ref this.OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChangeNotificationHolder;
	public event Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChangeNotification
	{
		add
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChangeNotificationHolder;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerFactionContributionChangeNotification>>(ref this.OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChangeNotificationHolder;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerFactionContributionChangeNotification>>(ref this.OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<FactionLoginRewardNotification> OnFactionLoginRewardNotificationHolder;
	public event Action<FactionLoginRewardNotification> OnFactionLoginRewardNotification
	{
		add
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotificationHolder;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionLoginRewardNotification>>(ref this.OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotificationHolder;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionLoginRewardNotification>>(ref this.OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<QuestProgress[]> OnQuestProgressChangedHolder;
	public event Action<QuestProgress[]> OnQuestProgressChanged
	{
		add
		{
			Action<QuestProgress[]> action = this.OnQuestProgressChangedHolder;
			Action<QuestProgress[]> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestProgress[]>>(ref this.OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<QuestProgress[]> action = this.OnQuestProgressChangedHolder;
			Action<QuestProgress[]> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestProgress[]>>(ref this.OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyAlertMissionDataNotification> OnAlertMissionDataChangeHolder;
	public event Action<LobbyAlertMissionDataNotification> OnAlertMissionDataChange
	{
		add
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnAlertMissionDataChangeHolder;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyAlertMissionDataNotification>>(ref this.OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnAlertMissionDataChangeHolder;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyAlertMissionDataNotification>>(ref this.OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<Dictionary<int, SeasonChapterQuests>> OnSeasonChapterQuestsChangeHolder;
	public event Action<Dictionary<int, SeasonChapterQuests>> OnSeasonChapterQuestsChange
	{
		add
		{
			Action<Dictionary<int, SeasonChapterQuests>> action = this.OnSeasonChapterQuestsChangeHolder;
			Action<Dictionary<int, SeasonChapterQuests>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Dictionary<int, SeasonChapterQuests>>>(ref this.OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Dictionary<int, SeasonChapterQuests>> action = this.OnSeasonChapterQuestsChangeHolder;
			Action<Dictionary<int, SeasonChapterQuests>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Dictionary<int, SeasonChapterQuests>>>(ref this.OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool IsConnectedToLobbyServer
	{
		get
		{
			bool result;
			if (this.m_lobbyGameClientInterface != null)
			{
				result = this.m_lobbyGameClientInterface.IsConnected;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public bool IsRegistered { get; private set; }

	public bool IsReady { get; private set; }

	public bool AllowRelogin { get; private set; }

	public NetworkClient Client
	{
		get
		{
			NetworkClient result;
			if (NetworkManager.singleton == null)
			{
				result = null;
			}
			else
			{
				result = NetworkManager.singleton.client;
			}
			return result;
		}
	}

	public NetworkConnection Connection
	{
		get
		{
			NetworkConnection result;
			if (this.Client == null)
			{
				result = null;
			}
			else
			{
				result = this.Client.connection;
			}
			return result;
		}
	}

	public MyNetworkClientConnection MyConnection
	{
		get
		{
			return (this.Client != null) ? (this.Client.connection as MyNetworkClientConnection) : null;
		}
	}

	public bool IsConnectedToGameServer
	{
		get
		{
			bool result;
			if (this.MyConnection != null)
			{
				result = this.MyConnection.isConnected;
			}
			else if (this.Connection != null)
			{
				result = this.Connection.isConnected;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public bool IsRegisteredToGameServer { get; private set; }

	public CurrencyWallet PlayerWallet { get; private set; }

	public FriendList FriendList { get; private set; }

	public bool IsFriendListInitialized { get; private set; }

	public Dictionary<int, long> FactionScores { get; private set; }

	public int ActiveFactionCompetition { get; private set; }

	public bool PlayerObjectStartedOnClient { get; set; }

	public bool InGameUIActivated { get; set; }

	public bool VisualSceneLoaded { get; set; }

	public bool DesignSceneStarted { get; set; }

	public bool IsLoading
	{
		get
		{
			return this.m_loading;
		}
	}

	public bool IsFastForward
	{
		get
		{
			bool result;
			if (!this.m_withinReconnectReplay)
			{
				if (ReplayPlayManager.Get())
				{
					result = ReplayPlayManager.Get().IsFastForward();
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}
	}

	public bool IsReconnecting
	{
		get
		{
			return this.m_withinReconnect;
		}
	}

	public bool IsReconnectingInstantly
	{
		get
		{
			return this.m_withinReconnectInstantly;
		}
	}

	public bool SpectatorHideAbilityTargeter { get; set; }

	public bool IsSpectator
	{
		get
		{
			bool result;
			if (this.PlayerInfo != null)
			{
				result = (this.PlayerInfo.TeamId == Team.Spectator);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public int WaitingForModSelectResponse { get; private set; }

	public int WaitingForCardSelectResponse { get; private set; }

	public bool AllowBadges { get; private set; }

	public int NewPlayerPvPQueueDuration { get; private set; }

	public GameResult GameResult
	{
		get
		{
			return this.m_gameResult;
		}
	}

	public bool Reconnected
	{
		get
		{
			return this.m_reconnected;
		}
	}

	public bool Observer
	{
		get
		{
			return this.m_observer;
		}
	}

	public TierPlacement TierChangeMin { get; private set; }

	public TierPlacement TierChangeMax { get; private set; }

	public TierPlacement TierCurrent { get; private set; }

	public LobbyAlertMissionDataNotification AlertMissionsData { get; private set; }

	public Dictionary<int, SeasonChapterQuests> SeasonChapterQuests { get; private set; }

	public bool IsCharacterInFreeRotation(CharacterType characterType, GameType gameType)
	{
		bool result;
		if (this.ClientAccessLevel < ClientAccessLevel.Full)
		{
			result = (this.m_loadedPlayerAccountData.AccountComponent.IsCharacterInFreeRotation(characterType) || this.IsFreelancerInFreeRotationExtension(characterType, gameType, null));
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsCharacterAvailable(CharacterType characterType, GameType gameType)
	{
		bool enableHiddenCharacters = GameManager.Get()?.GameplayOverrides.EnableHiddenCharacters ?? false;
		CharacterResourceLink characterResourceLink = null;
		foreach (CharacterResourceLink crl in GameWideData.Get().m_characterResourceLinks)
		{
			if (crl.m_characterType == characterType)
			{
				characterResourceLink = crl;
				break;
			}
		}

		if (characterResourceLink == null)
		{
			return false;
		}
		if (characterResourceLink.m_isHidden && !enableHiddenCharacters)
		{
			return false;
		}
		if (!characterResourceLink.m_characterType.IsValidForHumanPreGameSelection())
		{
			return false;
		}
		if (characterResourceLink.m_characterType.IsWillFill() && gameType != GameType.PvP && gameType != GameType.NewPlayerPvP)
		{
			return false;
		}
		PersistedCharacterData playerCharacterData = this.GetPlayerCharacterData(characterType);
		if (this.ClientAccessLevel < ClientAccessLevel.Full && !characterType.IsWillFill())
		{
			if (gameType != GameType.Practice)
			{
				if (playerCharacterData != null && playerCharacterData.CharacterComponent.Unlocked)
				{
					return true;
				}
				return this.IsCharacterInFreeRotation(characterType, gameType);
			}
		}
		return true;
	}

	private List<CharacterVisualInfo> WaitingForSkinSelectResponse { get; set; }

	private FactionLoginRewardNotification LoginRewardNotification { get; set; }

	private List<QuestCompleteNotification> LoginQuestCompleteNotifications { get; set; }

	public DateTime QueueEntryTime
	{
		get
		{
			return this.OurQueueEntryTime;
		}
	}

	public bool IsWaitingForSkinResponse()
	{
		return this.WaitingForSkinSelectResponse.Count > 0;
	}

	public void HandleQueueConfirmation()
	{
		this.OurQueueEntryTime = DateTime.UtcNow;
	}

	private void OnApplicationQuit()
	{
		ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
		if (clientExceptionDetector != null)
		{
			clientExceptionDetector.FlushErrorsToLobby();
		}
		this.LeaveGame(false, GameResult.ClientShutDown);
		DiscordClientInterface.Shutdown();
	}

	public List<LobbyGameInfo> CustomGameInfos { get; private set; }

	public LobbyPlayerGroupInfo GroupInfo { get; private set; }

	public EnvironmentType EnvironmentType { get; private set; }

	public ServerMessageOverrides ServerMessageOverrides { get; private set; }

	public ServerLockState ServerLockState { get; private set; }

	public ConnectionQueueInfo ConnectionQueueInfo { get; private set; }

	public AuthTicket AuthTicket
	{
		get
		{
			return HydrogenConfig.Get().Ticket;
		}
	}

	public ClientAccessLevel ClientAccessLevel
	{
		get
		{
			return (this.PlayerInfo == null) ? this.m_clientAccessLevel : this.PlayerInfo.EffectiveClientAccessLevel;
		}
	}

	public bool HasPurchasedGame { get; private set; }

	public int HighestPurchasedGamePack { get; private set; }

	public DateTime ServerUtcTime { get; private set; }

	public DateTime ServerPacificTime { get; private set; }

	public DateTime ClientUtcTime { get; private set; }

	public TimeSpan TimeOffset { get; private set; }

	public string CommerceURL { get; private set; }

	public LobbyGameInfo GameInfo
	{
		get
		{
			LobbyGameInfo result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().GameInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	public LobbyPlayerInfo PlayerInfo
	{
		get
		{
			LobbyPlayerInfo result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().PlayerInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	public LobbyTeamInfo TeamInfo
	{
		get
		{
			LobbyTeamInfo result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().TeamInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	public bool IsServerQueued
	{
		get
		{
			bool result;
			if (this.ClientAccessLevel == ClientAccessLevel.Queued)
			{
				result = (this.ConnectionQueueInfo != null);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public bool IsServerLocked
	{
		get
		{
			return this.ClientAccessLevel == ClientAccessLevel.Locked;
		}
	}

	public bool HasDeveloperAccess()
	{
		if (this.AuthTicket != null)
		{
			if (this.AuthTicket.HasEntitlement("DEVELOPER_ACCESS"))
			{
				return true;
			}
		}
		return this.ClientAccessLevel >= ClientAccessLevel.Admin;
	}

	public bool PurchasingMod { get; private set; }

	public int ModAttemptingToPurchase { get; private set; }

	private void Awake()
	{
		ClientGameManager.s_instance = this;
		this.WaitingForCardSelectResponse = -1;
		this.WaitingForModSelectResponse = -1;
		this.m_loadLevelOperationBundleSceneNames = new List<KeyValuePair<string, string>>();
		this.m_loadingCharacterResources = new List<CharacterResourceLink>();
		this.m_loading = false;
		this.m_loadingCharacterAssets = false;
		this.m_assetsLoadingState = new AssetsLoadingState();
		this.m_lastReceivedMsgSeqNum = 0U;
		this.m_lastSentMsgSeqNum = 0U;
		this.m_replay = new Replay();
		this.m_taskScheduler = new Scheduler();
		this.m_clientPerformanceCollectTask = delegate()
		{
			this.SendPerformanceReport();
		};
		this.AllowBadges = false;
		this.ClearLobbyState();
		this.LoginQuestCompleteNotifications = new List<QuestCompleteNotification>();
		this.WaitingForSkinSelectResponse = new List<CharacterVisualInfo>();
	}

	private void Start()
	{
		GameManager.Get().OnGameStopped += this.HandleGameStopped;
		GameManager.Get().OnGameLaunched += this.HandleGameLaunched;
		GameManager.Get().OnGameStatusChanged += this.HandleGameStatusChanged;
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnConnected = (Action<bool>)Delegate.Combine(discordClientInterface.OnConnected, new Action<bool>(this.HandleDiscordConnected));
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(this.HandleDiscordDisconnected));
		DiscordClientInterface discordClientInterface3 = DiscordClientInterface.Get();
		discordClientInterface3.OnAuthorized = (Action<string>)Delegate.Combine(discordClientInterface3.OnAuthorized, new Action<string>(this.HandleDiscordAuthorized));
		DiscordClientInterface discordClientInterface4 = DiscordClientInterface.Get();
		discordClientInterface4.OnAuthenticated = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface4.OnAuthenticated, new Action<DiscordUserInfo>(this.HandleDiscordAuthenticated));
		DiscordClientInterface discordClientInterface5 = DiscordClientInterface.Get();
		discordClientInterface5.OnJoined = (Action)Delegate.Combine(discordClientInterface5.OnJoined, new Action(this.HandleDiscordJoined));
		DiscordClientInterface discordClientInterface6 = DiscordClientInterface.Get();
		discordClientInterface6.OnLeft = (Action)Delegate.Combine(discordClientInterface6.OnLeft, new Action(this.HandleDiscordLeft));
		VisualsLoader.OnLoading += this.HandleVisualsSceneLoading;
		MyNetworkClientConnection.OnSending = (Action<UNetMessage>)Delegate.Combine(MyNetworkClientConnection.OnSending, new Action<UNetMessage>(this.HandleGameMessageSending));
	}

	private void Update()
	{
		bool flag;
		if (this.MyConnection != null)
		{
			flag = (this.PlayerInfo != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
		{
			this.MyConnection.Update();
		}
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.Update();
		}
		this.CheckLoaded();
		GroupJoinManager.Get().Update();
		if (!this.DisplayedMOTDPopup)
		{
			if (HydrogenConfig.Get() != null)
			{
				if (this.ServerMessageOverrides != null)
				{
					if (this.ServerMessageOverrides.MOTDPopUpText != null)
					{
						if (UIDialogPopupManager.Get() != null)
						{
							string value = this.ServerMessageOverrides.MOTDPopUpText.GetValue(HydrogenConfig.Get().Language);
							if (!value.IsNullOrEmpty())
							{
								UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("MessageOfTheDay", "Global"), value, StringUtil.TR("Ok", "Global"), null, -1, false);
								this.DisplayedMOTDPopup = true;
							}
						}
					}
				}
			}
		}
		bool flag3 = true;
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameConfig != null)
			{
				if (GameManager.Get().GameInfo.GameConfig.GameType == GameType.Tutorial)
				{
					goto IL_1F3;
				}
			}
			if (!(UINewReward.Get() == null))
			{
				if (!UIDialogPopupManager.Get().IsDialogBoxOpen())
				{
					if (!UIFrontendLoadingScreen.Get().IsVisible())
					{
						goto IL_1F5;
					}
				}
			}
		}
		IL_1F3:
		flag3 = false;
		IL_1F5:
		if (flag3)
		{
			if (this.LoginRewardNotification != null)
			{
				UINewReward.Get().NotifyNewTrustReward(this.LoginRewardNotification.LogInRewardsGiven, -1, string.Empty, true);
				this.LoginRewardNotification = null;
			}
			if (this.LoginQuestCompleteNotifications.Count > 0)
			{
				foreach (QuestCompleteNotification obj in this.LoginQuestCompleteNotifications)
				{
					this.OnQuestCompleteNotificationHolder(obj);
				}
				this.LoginQuestCompleteNotifications.Clear();
			}
		}
	}

	private void ClearLobbyState()
	{
		this.IsRegistered = false;
		this.IsReady = false;
		this.AllowRelogin = true;
		this.FriendList = new FriendList();
		this.IsFriendListInitialized = false;
		this.CustomGameInfos = new List<LobbyGameInfo>();
		this.GroupInfo = null;
		this.m_clientAccessLevel = ClientAccessLevel.Unknown;
		this.HasPurchasedGame = false;
		this.ServerLockState = ServerLockState.Unknown;
		this.ConnectionQueueInfo = new ConnectionQueueInfo();
		this.CommerceURL = string.Empty;
		this.ServerMessageOverrides = new ServerMessageOverrides();
		this.ServerMessageOverrides.MOTDText = string.Empty;
		this.ServerMessageOverrides.MOTDPopUpText = string.Empty;
		this.ServerMessageOverrides.ReleaseNotesText = string.Empty;
		this.ServerMessageOverrides.ReleaseNotesHeader = string.Empty;
		this.ServerMessageOverrides.ReleaseNotesDescription = string.Empty;
		this.ServerMessageOverrides.WhatsNewText = string.Empty;
		this.ServerMessageOverrides.WhatsNewHeader = string.Empty;
		this.ServerMessageOverrides.WhatsNewDescription = string.Empty;
		this.ServerMessageOverrides.LockScreenText = string.Empty;
		this.ServerMessageOverrides.LockScreenButtonText = string.Empty;
		this.ServerMessageOverrides.FreeUpsellExternalBrowserUrl = string.Empty;
		this.ServerMessageOverrides.FreeUpsellExternalBrowserSteamUrl = string.Empty;
		this.m_discordConnecting = false;
		this.m_discordJoinSuggested = false;
	}

	public void SetSoloSubGameMask(GameType gameType, ushort subMask)
	{
		this.SoloSubTypeMask[gameType] = subMask;
	}

	public ushort GetSoloSubGameMask(GameType gameType)
	{
		if (!this.SoloSubTypeMask.ContainsKey(gameType))
		{
			Dictionary<ushort, GameSubType> gameTypeSubTypes = this.GetGameTypeSubTypes(gameType);
			if (gameTypeSubTypes != null)
			{
				ushort num = HydrogenConfig.Get().GetSavedSubTypes(gameType, gameTypeSubTypes);
				if (num == 0)
				{
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
							if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
							{
								num = keyValuePair.Key;
								goto IL_E8;
							}
							if (!keyValuePair.Value.HasMod(GameSubType.SubTypeMods.NotCheckedByDefault))
							{
								num |= keyValuePair.Key;
							}
						}
					}
				}
				IL_E8:
				this.SoloSubTypeMask[gameType] = num;
			}
			else
			{
				this.SoloSubTypeMask[gameType] = 0;
				Log.Error("Unable to find sub types for game {0}", new object[]
				{
					gameType
				});
			}
		}
		return this.SoloSubTypeMask[gameType];
	}

	public void ConnectToLobbyServer()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return;
		}
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		if (hydrogenConfig.Ticket == null)
		{
			try
			{
				if (!hydrogenConfig.TicketFile.IsNullOrEmpty())
				{
					hydrogenConfig.Ticket = AuthTicket.Load(hydrogenConfig.TicketFile, null);
					File.Delete(hydrogenConfig.TicketFile);
					hydrogenConfig.TicketFile = null;
				}
				if (hydrogenConfig.Ticket == null)
				{
					if (hydrogenConfig.PlatformConfig.AllowRequestTickets)
					{
						if (!hydrogenConfig.PlatformUserName.IsNullOrEmpty() && !hydrogenConfig.PlatformPassword.IsNullOrEmpty())
						{
							hydrogenConfig.Ticket = AuthTicket.CreateRequestTicket(hydrogenConfig.PlatformUserName, hydrogenConfig.PlatformPassword, "Client");
							goto IL_126;
						}
					}
					if (hydrogenConfig.PlatformConfig.AllowFakeTickets)
					{
						hydrogenConfig.Ticket = AuthTicket.CreateFakeTicket(hydrogenConfig.SystemUserName, "Client", 0, new string[]
						{
							"ADMIN_ACCESS;GAME_OWNERSHIP"
						});
					}
				}
				IL_126:;
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				hydrogenConfig.Ticket = null;
			}
		}
		if (hydrogenConfig.Ticket == null)
		{
			throw new Exception("Could not load auth ticket");
		}
		Log.Info("Connecting to lobby server from {0} as {1} / {2} [{3}]", new object[]
		{
			hydrogenConfig.HostName,
			hydrogenConfig.Ticket.UserName,
			hydrogenConfig.Ticket.Handle,
			hydrogenConfig.Ticket.AccountId
		});
		this.ClearLobbyState();
		Region region = Options_UI.GetRegion();
		this.m_lobbyGameClientInterface = new LobbyGameClientInterface();
		this.m_lobbyGameClientInterface.Initialize(hydrogenConfig.DirectoryServerAddress, hydrogenConfig.Ticket, region, hydrogenConfig.Language, hydrogenConfig.ProcessType, hydrogenConfig.PreferredLobbyServerIndex);
		this.m_lobbyGameClientInterface.OnConnected += this.HandleConnectedToLobbyServer;
		this.m_lobbyGameClientInterface.OnDisconnected += this.HandleDisconnectedFromLobbyServer;
		this.m_lobbyGameClientInterface.OnLobbyServerReadyNotification += this.HandleLobbyServerReadyNotification;
		this.m_lobbyGameClientInterface.OnLobbyStatusNotification += this.HandleLobbyStatusNotification;
		this.m_lobbyGameClientInterface.OnLobbyGameplayOverridesNotification += this.HandleLobbyGameplayOverridesNotification;
		this.m_lobbyGameClientInterface.OnLobbyCustomGamesNotification += this.HandleLobbyCustomGamesNotification;
		this.m_lobbyGameClientInterface.OnQueueStatusNotification += this.HandleQueueStatusNotification;
		this.m_lobbyGameClientInterface.OnQueueAssignmentNotification += this.HandleQueueAssignmentNotification;
		this.m_lobbyGameClientInterface.OnGameAssignmentNotification += this.HandleGameAssignmentNotification;
		this.m_lobbyGameClientInterface.OnGameInfoNotification += this.HandleGameInfoNotification;
		this.m_lobbyGameClientInterface.OnGameStatusNotification += this.HandleGameStatusNotification;
		this.m_lobbyGameClientInterface.OnForcedCharacterChangeFromServerNotification += this.HandleForcedCharacterChangeFromServerNotification;
		this.m_lobbyGameClientInterface.OnCharacterDataUpdateNotification += this.HandleCharacterDataUpdateNotification;
		this.m_lobbyGameClientInterface.OnInventoryComponentUpdateNotification += this.HandleInventoryComponentUpdateNotification;
		this.m_lobbyGameClientInterface.OnAccountDataUpdated += this.HandleAccountDataUpdateNotification;
		this.m_lobbyGameClientInterface.OnBankBalanceChangeNotification += this.HandleBankBalanceChangeNotification;
		this.m_lobbyGameClientInterface.OnSeasonStatusNotification += this.HandleSeasonStatusNotification;
		this.m_lobbyGameClientInterface.OnChapterStatusNotification += this.HandleChapterStatusNotification;
		this.m_lobbyGameClientInterface.OnChatNotification += this.HandleChatNotification;
		this.m_lobbyGameClientInterface.OnUseOverconNotification += this.HandleUseOverconNotification;
		this.m_lobbyGameClientInterface.OnUseGGPackNotification += this.HandleGGPackUsedNotification;
		this.m_lobbyGameClientInterface.OnGroupUpdateNotification += this.HandleGroupUpdateNotification;
		this.m_lobbyGameClientInterface.OnFriendStatusNotification += this.HandleFriendStatusNotification;
		this.m_lobbyGameClientInterface.OnGroupConfirmation += this.HandleGroupConfirmationRequest;
		this.m_lobbyGameClientInterface.OnGroupSuggestion += this.HandleGroupSuggestionRequest;
		this.m_lobbyGameClientInterface.OnForceQueueNotification += this.HandleForceQueueNotification;
		this.m_lobbyGameClientInterface.OnGameInviteConfirmationRequest += this.HandleGameInviteConfirmationRequest;
		this.m_lobbyGameClientInterface.OnQuestCompleteNotification += this.HandleQuestCompleteNotification;
		this.m_lobbyGameClientInterface.OnMatchResultsNotification += this.HandleMatchResultsNotification;
		this.m_lobbyGameClientInterface.OnServerQueueConfigurationUpdateNotification += this.HandleServerQueueConfigurationUpdateNotification;
		this.m_lobbyGameClientInterface.OnRankedOverviewChangeNotification += this.HandleRankedOverviewChangeNotification;
		this.m_lobbyGameClientInterface.OnFactionCompetitionNotification += this.HandleFactionCompetitionNotification;
		this.m_lobbyGameClientInterface.OnTrustBoostUsedNotification += this.HandleTrustBoostUsedNotification;
		this.m_lobbyGameClientInterface.OnFacebookAccessTokenNotification += this.HandleFacebookAccessTokenNotification;
		this.m_lobbyGameClientInterface.OnPlayerFactionContributionChange += this.HandlePlayerFactionContributionChange;
		this.m_lobbyGameClientInterface.OnFactionLoginRewardNotification += this.HandleFactionLoginRewardNotification;
		this.m_lobbyGameClientInterface.OnLobbyAlertMissionDataNotification += this.HandleLobbyAlertMissionDataNotification;
		this.m_lobbyGameClientInterface.OnLobbySeasonQuestDataNotification += this.HandleLobbySeasonQuestDataNotification;
		this.m_lobbyGameClientInterface.IsCompressed = hydrogenConfig.WebSocketIsCompressed;
		this.m_lobbyGameClientInterface.IsBinary = hydrogenConfig.WebSocketIsBinary;
		this.m_lobbyGameClientInterface.HttpPostHandler = new Action<string, string, Action<string, string>>(this.HttpPost);
		this.m_lobbyGameClientInterface.Connect();
	}

	public void DisconnectFromLobbyServer()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.Disconnect();
			this.m_lobbyGameClientInterface = null;
		}
	}

	internal void CleanupMemory()
	{
		CharacterResourceLink.UnloadAll();
		PrefabResourceLink.UnloadAll();
		SequenceLookup.UnloadAll();
		SequenceSource.ClearStaticData();
	}

	private void OnDestroy()
	{
		if (GameManager.Get() != null)
		{
			GameManager.Get().StopGame(GameResult.NoResult);
		}
		NetworkManager.singleton.StopClient();
		this.DisconnectFromLobbyServer();
		if (GameManager.Get() != null)
		{
			GameManager.Get().OnGameStopped -= this.HandleGameStopped;
			GameManager.Get().OnGameLaunched -= this.HandleGameLaunched;
			GameManager.Get().OnGameStatusChanged -= this.HandleGameStatusChanged;
		}
		MyNetworkManager myNetworkManager = MyNetworkManager.Get();
		if (myNetworkManager != null)
		{
			myNetworkManager.m_OnClientConnect -= this.HandleNetworkConnect;
			myNetworkManager.m_OnClientDisconnect -= this.HandleNetworkDisconnect;
			myNetworkManager.m_OnClientError -= this.HandleNetworkError;
		}
		if (this.Client != null)
		{
			this.Client.UnregisterHandler(0x34);
		}
		SinglePlayerManager.UnregisterSpawnHandler();
		ClientGameManager.s_instance = null;
	}

	public void SubscribeToCustomGames()
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.SubscribeToCustomGames();
		}
	}

	public void UnsubscribeFromCustomGames()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.UnsubscribeFromCustomGames();
			}
		}
	}

	public void JoinQueue(GameType gameType, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, Action<JoinMatchmakingQueueResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			BotDifficulty botDifficulty;
			if (allyDifficulty != null)
			{
				botDifficulty = allyDifficulty.Value;
			}
			else
			{
				botDifficulty = BotDifficulty.Hard;
			}
			BotDifficulty allyBotDifficulty = botDifficulty;
			BotDifficulty botDifficulty2;
			if (enemyDifficulty != null)
			{
				botDifficulty2 = enemyDifficulty.Value;
			}
			else
			{
				botDifficulty2 = BotDifficulty.Easy;
			}
			BotDifficulty enemyBotDifficulty = botDifficulty2;
			this.m_lobbyGameClientInterface.JoinQueue(gameType, allyBotDifficulty, enemyBotDifficulty, onResponseCallback);
		}
		else
		{
			JoinMatchmakingQueueResponse obj = new JoinMatchmakingQueueResponse
			{
				Success = false,
				ErrorMessage = "Not connected to Lobby.\nPlease restart client."
			};
			onResponseCallback(obj);
		}
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.LeaveQueue(onResponseCallback);
		}
	}

	public void CreateGame(LobbyGameConfig gameConfig, ReadyState readyState, BotDifficulty selectedBotSkillTeamA, BotDifficulty selectedBotSkillTeamB, Action<CreateGameResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			string processCode = null;
			if (gameConfig.InstanceSubTypeBit == 0)
			{
				Dictionary<ushort, GameSubType> gameTypeSubTypes = this.GetGameTypeSubTypes(gameConfig.GameType);
				if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
				{
					if (gameTypeSubTypes.Count == 1)
					{
						gameConfig.InstanceSubTypeBit = gameTypeSubTypes.First<KeyValuePair<ushort, GameSubType>>().Key;
						Log.Warning("CreateGame() called without setting InstanceSubTypeIndex. Forcing it to use the only viable one ({0}: 0x{1:x4}: {2}), but the calling code should consult all possible choices, because although it might currently be configured to only have one choice, that list can be changed dynamically on a running server to be any length.", new object[]
						{
							gameConfig.GameType,
							gameConfig.InstanceSubTypeBit,
							gameTypeSubTypes.First<KeyValuePair<ushort, GameSubType>>().Value.GetNameAsPayload().ToString()
						});
						this.m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
					}
					else
					{
						List<KeyValuePair<ushort, GameSubType>> pstAsList = gameTypeSubTypes.ToList<KeyValuePair<ushort, GameSubType>>();
						UIDialogPopupManager.OpenTwoButtonDialog("Brutal Hack", "TODO: The calling code did not pick a sub-type for this game type. Please chose:", StringUtil.TR(pstAsList[0].Value.LocalizedName), StringUtil.TR(pstAsList[1].Value.LocalizedName), delegate(UIDialogBox UIDialogBox)
						{
							gameConfig.InstanceSubTypeBit = pstAsList[0].Key;
							this.m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
						}, delegate(UIDialogBox UIDialogBox)
						{
							gameConfig.InstanceSubTypeBit = pstAsList[1].Key;
							this.m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
						}, false, false);
					}
				}
				else
				{
					Log.Warning("Huh, why do we not know about the sub-types of game type {0}?", new object[]
					{
						gameConfig.GameType
					});
					gameConfig.InstanceSubTypeBit = 1;
					this.m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
				}
			}
			else
			{
				this.m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
			}
		}
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.JoinGame(gameInfo, asSpectator, onResponseCallback);
		}
	}

	public void LeaveGame(bool isPermanent, GameResult gameResult)
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager == null)
		{
			return;
		}
		if (ReplayPlayManager.Get() != null && ReplayPlayManager.Get().IsPlayback())
		{
			Log.Info("Leaving replay", new object[0]);
			m_lobbyGameClientInterface.Replay_RemoveFromGame();
			return;
		}
		if (gameManager.GameInfo != null && !gameManager.GameInfo.GameServerProcessCode.IsNullOrEmpty() && m_gameResult == GameResult.NoResult)
		{
			Log.Info($"Leaving game {(isPermanent ? "permanently" : "temporarily")} with result {gameResult}");
			m_gameResult = gameResult;
			m_lobbyGameClientInterface?.LeaveGame(isPermanent, gameResult, delegate(LeaveGameResponse response)
			{
				if (!response.Success)
				{
					TextConsole.Get().Write($"Failed to leave game: {response.ErrorMessage}", ConsoleMessageType.SystemMessage);
					Log.Warning($"Request to leave game {gameManager.GameInfo?.Name ?? string.Empty} failed: {response.ErrorMessage}");
				}
			});
					
			if (NetworkClient.active && Client != null && Client.isConnected && !NetworkServer.active)
			{
				GameManager.LeaveGameNotification leaveGameNotification = new GameManager.LeaveGameNotification();
				leaveGameNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
				leaveGameNotification.IsPermanent = isPermanent;
				leaveGameNotification.GameResult = gameResult;
				Client.SetMaxDelay(0f);
				if (!Client.Send(0x43, leaveGameNotification))
				{
					Log.Error("Failed to send LeaveGameNotification", new object[0]);
				}
				Client.Disconnect();
			}
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, MatchFreelancerStats stats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, null, stats, onResponseCallback);
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, PersistedStats stats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, stats, null, onResponseCallback);
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, null, null, onResponseCallback);
		}
	}

	public void UpdateReadyState(ReadyState readyState, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, Action<PlayerInfoUpdateResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (readyState == ReadyState.Ready)
			{
				GameOptionFlag gameOptionFlag = GameOptionFlag.None;
				if (DebugParameters.Get() != null)
				{
					if (DebugParameters.Get().GetParameterAsBool("ReplaceHumansWithBots"))
					{
						gameOptionFlag = gameOptionFlag.WithGameOption(GameOptionFlag.ReplaceHumansWithBots);
					}
				}
				if (DebugParameters.Get() != null)
				{
					if (DebugParameters.Get().GetParameterAsBool("SkipEndOfGameCheck"))
					{
						gameOptionFlag = gameOptionFlag.WithGameOption(GameOptionFlag.SkipEndOfGameCheck);
					}
				}
				if (DebugParameters.Get() != null)
				{
					if (DebugParameters.Get().GetParameterAsBool("EnableTeamAIOutput"))
					{
						gameOptionFlag = gameOptionFlag.WithGameOption(GameOptionFlag.EnableTeamAIOutput);
					}
				}
				PlayerGameOptionFlag playerGameOptionFlag = PlayerGameOptionFlag.None;
				if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("ReplaceHumanWithBot"))
				{
					playerGameOptionFlag = playerGameOptionFlag.WithGameOption(PlayerGameOptionFlag.ReplaceHumanWithBot);
				}
				if (gameOptionFlag == GameOptionFlag.None)
				{
					if (playerGameOptionFlag == PlayerGameOptionFlag.None)
					{
						goto IL_13A;
					}
				}
				this.m_lobbyGameClientInterface.UpdateGameCheats(gameOptionFlag, playerGameOptionFlag, null);
			}
			IL_13A:
			LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
			bool shouldResetOnFalure = false;
			ReadyState currentReadyState = ReadyState.Ready;
			if (playerInfo != null)
			{
				shouldResetOnFalure = true;
				currentReadyState = playerInfo.ReadyState;
			}
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.ContextualReadyState = new ContextualReadyState?(new ContextualReadyState
			{
				ReadyState = readyState,
				GameProcessCode = ((this.GameInfo == null) ? null : this.GameInfo.GameServerProcessCode)
			});
			if (allyDifficulty != null)
			{
				lobbyPlayerInfoUpdate.AllyDifficulty = new BotDifficulty?(allyDifficulty.Value);
			}
			if (enemyDifficulty != null)
			{
				lobbyPlayerInfoUpdate.EnemyDifficulty = new BotDifficulty?(enemyDifficulty.Value);
			}
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, delegate(PlayerInfoUpdateResponse response)
			{
				if (!response.Success)
				{
					if (shouldResetOnFalure && currentReadyState != readyState && AppState_CharacterSelect.Get() == AppState.GetCurrent())
					{
						if (playerInfo != null)
						{
							playerInfo.ReadyState = currentReadyState;
							Log.Warning("Failure to ready, resetting ready state", new object[0]);
						}
					}
				}
				if (onResponseCallback != null)
				{
					onResponseCallback(response);
				}
				else if (!response.Success)
				{
					string text;
					if (response.LocalizedFailure != null)
					{
						text = response.LocalizedFailure.ToString();
					}
					else
					{
						text = (response.ErrorMessage.IsNullOrEmpty() ? StringUtil.TR("UnknownErrorTryAgain", "Frontend") : string.Format("{0}#NeedsLocalization", response.ErrorMessage));
					}
					string text2 = text;
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = text2,
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
				}
			});
		}
		else
		{
			Log.Warning("m_lobbyGameClientInterface == null", new object[0]);
		}
		if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				GameManager.Get().PlayerInfo.ReadyState = readyState;
			}
		}
	}

	public void UpdateSelectedGameMode(GameType gametype)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.UpdateGroupGameType(gametype, delegate(PlayerGroupInfoUpdateResponse response)
			{
				UICharacterScreen.Get().ReceivedGameTypeChangeResponse();
				if (!response.Success)
				{
					string description;
					if (response.LocalizedFailure != null)
					{
						description = response.LocalizedFailure.ToString();
					}
					else if (!response.ErrorMessage.IsNullOrEmpty())
					{
						description = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
					}
					else
					{
						description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
					}
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingFailure", "Global"), description, StringUtil.TR("Ok", "Global"), null, -1, false);
				}
			});
		}
	}

	public void UpdateSelectedCharacter(CharacterType character, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterType = new CharacterType?(character);
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleCharacterSelectUpdateResponse));
		}
	}

	private void RecordFailureInCharacterSelectUpdateResponse(PlayerInfoUpdateResponse response, string memberName)
	{
		if (response.LocalizedFailure != null)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = string.Format(StringUtil.TR("FailedMessage", "Global"), response.LocalizedFailure.ToString()),
				MessageType = ConsoleMessageType.SystemMessage
			}, null);
			Log.Error("Lobby Server Error ({0}): {1}", new object[]
			{
				memberName,
				response.LocalizedFailure.ToString()
			});
		}
		else
		{
			Log.Error("Lobby Server Error ({0}): {1}", new object[]
			{
				memberName,
				response.ErrorMessage
			});
		}
	}

	public void HandleCharacterSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleCharacterSelectUpdateResponse");
			return;
		}
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().CharacterSelectionResponseHandler(response);
		}
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (response.CharacterInfo != null)
			{
				ClientGameManager.Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, true);
			}
		}
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent() && response.CharacterInfo != null)
		{
			UICharacterSelectScreenController.Get().NotifyGroupUpdate();
		}
	}

	public void UpdateLoadouts(List<CharacterLoadout> loadouts, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			CharacterLoadoutUpdate value;
			value.CharacterLoadoutChanges = loadouts;
			if (!loadouts.IsNullOrEmpty<CharacterLoadout>())
			{
				lobbyPlayerInfoUpdate.RankedLoadoutMods = (loadouts[0].Strictness == ModStrictness.Ranked);
			}
			else
			{
				Log.Error("Client attempting to update invalid loadouts", new object[0]);
			}
			lobbyPlayerInfoUpdate.CharacterLoadoutChanges = new CharacterLoadoutUpdate?(value);
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleLoadoutUpdateResponse));
		}
	}

	public void RequestLoadouts(bool ranked)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = 0;
			lobbyPlayerInfoUpdate.RankedLoadoutMods = ranked;
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleLoadoutUpdateResponse));
		}
	}

	public void HandleLoadoutUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleLoadoutUpdateResponse");
			return;
		}
		if (response.CharacterInfo.CharacterLoadouts.Count > 0 && response.CharacterInfo.CharacterLoadouts[0].Strictness == ModStrictness.Ranked)
		{
			this.GetPlayerCharacterData(this.GetPlayerAccountData().AccountComponent.LastCharacter).CharacterComponent.CharacterLoadoutsRanked = response.CharacterInfo.CharacterLoadouts;
		}
		else
		{
			this.GetPlayerCharacterData(this.GetPlayerAccountData().AccountComponent.LastCharacter).CharacterComponent.CharacterLoadouts = response.CharacterInfo.CharacterLoadouts;
		}
		UICharacterSelectCharacterSettingsPanel.Get().NotifyLoadoutUpdate(response);
		if (UIRankedCharacterSelectSettingsPanel.Get() != null)
		{
			UIRankedCharacterSelectSettingsPanel.Get().NotifyLoadoutUpdate(response);
		}
	}

	public void UpdateSelectedSkin(CharacterVisualInfo selectedCharacterSkin, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterSkin = new CharacterVisualInfo?(selectedCharacterSkin);
			this.WaitingForSkinSelectResponse.Add(selectedCharacterSkin);
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleSkinSelectUpdateResponse));
		}
	}

	public void HandleSkinSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleSkinSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null && ClientGameManager.Get().GroupInfo != null)
		{
			this.GetAllPlayerCharacterData()[response.CharacterInfo.CharacterType].CharacterComponent.LastSkin = response.CharacterInfo.CharacterSkin;
			ClientGameManager.Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, true);
			if (this.WaitingForSkinSelectResponse.Count > 0)
			{
				if (this.WaitingForSkinSelectResponse[0].Equals(response.CharacterInfo.CharacterSkin))
				{
					this.WaitingForSkinSelectResponse.RemoveAt(0);
					return;
				}
			}
			if (UICharacterSelectWorldObjects.Get() != null)
			{
				UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(response.CharacterInfo.CharacterType, 0, string.Empty, response.CharacterInfo.CharacterSkin, false);
			}
			else
			{
				Log.Warning("Handling skin selection update response when character select is not present", new object[0]);
			}
		}
	}

	public void UpdateSelectedCards(CharacterCardInfo cards, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterCards = new CharacterCardInfo?(cards);
			this.WaitingForCardSelectResponse = this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleCardSelectUpdateResponse));
		}
	}

	public void ClearWaitingForCardResponse()
	{
		this.WaitingForCardSelectResponse = -1;
	}

	public void HandleCardSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (this.WaitingForCardSelectResponse == response.ResponseId)
		{
			this.ClearWaitingForCardResponse();
		}
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleCardSelectUpdateResponse");
			return;
		}
		if (ClientGameManager.Get().GroupInfo != null && response.CharacterInfo != null)
		{
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				if (GameManager.Get().GameInfo.IsCustomGame)
				{
					if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
					{
						goto IL_11D;
					}
				}
			}
			if (UICharacterSelectCharacterSettingsPanel.Get() != null)
			{
				UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
			}
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				UIRankedCharacterSelectSettingsPanel.Get().Refresh();
			}
		}
		IL_11D:
		if (response.CharacterInfo != null)
		{
			ClientGameManager.Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, false);
			PersistedCharacterData playerCharacterData = this.GetPlayerCharacterData(response.CharacterInfo.CharacterType);
			if (playerCharacterData != null)
			{
				playerCharacterData.CharacterComponent.LastCards = response.CharacterInfo.CharacterCards;
			}
		}
	}

	public void ClearWaitingForModResponse()
	{
		this.WaitingForModSelectResponse = -1;
	}

	public void UpdateSelectedMods(CharacterModInfo mods, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterMods = new CharacterModInfo?(mods);
			lobbyPlayerInfoUpdate.RankedLoadoutMods = (AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked);
			this.WaitingForModSelectResponse = this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleModSelectUpdateResponse));
		}
	}

	public void UpdateSelectedAbilityVfxSwaps(CharacterAbilityVfxSwapInfo swaps, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterAbilityVfxSwaps = new CharacterAbilityVfxSwapInfo?(swaps);
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleAbilityVfxSwapSelectUpdateResponse));
		}
	}

	public void HandleModSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (this.WaitingForModSelectResponse == response.ResponseId)
		{
			this.ClearWaitingForModResponse();
		}
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleModSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(response.CharacterInfo.CharacterType);
			bool rankedLoadoutMods = response.OriginalPlayerInfoUpdate.RankedLoadoutMods;
			if (rankedLoadoutMods)
			{
				playerCharacterData.CharacterComponent.LastRankedMods = response.CharacterInfo.CharacterMods;
			}
			else
			{
				playerCharacterData.CharacterComponent.LastMods = response.CharacterInfo.CharacterMods;
			}
			int? lastSelectedLoadout = response.OriginalPlayerInfoUpdate.LastSelectedLoadout;
			if (lastSelectedLoadout != null)
			{
				if (rankedLoadoutMods)
				{
					CharacterComponent characterComponent = playerCharacterData.CharacterComponent;
					int? lastSelectedLoadout2 = response.OriginalPlayerInfoUpdate.LastSelectedLoadout;
					characterComponent.LastSelectedRankedLoadout = lastSelectedLoadout2.Value;
				}
				else
				{
					CharacterComponent characterComponent2 = playerCharacterData.CharacterComponent;
					int? lastSelectedLoadout3 = response.OriginalPlayerInfoUpdate.LastSelectedLoadout;
					characterComponent2.LastSelectedLoadout = lastSelectedLoadout3.Value;
				}
			}
			if (ClientGameManager.Get().GroupInfo != null)
			{
				ClientGameManager.Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, false);
			}
			UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				UIRankedCharacterSelectSettingsPanel.Get().Refresh();
			}
		}
	}

	public void HandleAbilityVfxSwapSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleAbilityVfxSwapSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null)
		{
			ClientGameManager.Get().GetPlayerCharacterData(response.CharacterInfo.CharacterType).CharacterComponent.LastAbilityVfxSwaps = response.CharacterInfo.CharacterAbilityVfxSwaps;
			if (ClientGameManager.Get().GroupInfo != null)
			{
				ClientGameManager.Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, false);
			}
			UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.RefreshSelectedVfxSwaps();
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				UIRankedCharacterSelectSettingsPanel.Get().m_abilitiesSubPanel.RefreshSelectedVfxSwaps();
			}
		}
	}

	public void UpdateBotDifficulty(BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, int playerId = 0)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.AllyDifficulty = allyDifficulty;
			lobbyPlayerInfoUpdate.EnemyDifficulty = enemyDifficulty;
			this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, null);
		}
	}

	public void SendSetRegionRequest(Region region)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendSetRegionRequest(region);
		}
	}

	public void SendRankedTradeRequest_AcceptOrOffer(CharacterType desiredCharacter)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedTradeRequest(desiredCharacter, RankedTradeData.TradeActionType._001D);
		}
	}

	public void SendRankedTradeRequest_Reject(CharacterType desiredCharacter)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedTradeRequest(desiredCharacter, RankedTradeData.TradeActionType._000E);
		}
	}

	public void SendRankedTradeRequest_StopTrading()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedTradeRequest(CharacterType.None, RankedTradeData.TradeActionType._0012);
		}
	}

	public void SendRankedBanRequest(CharacterType type)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedBanRequest(type);
		}
	}

	public void SendRankedSelectRequest(CharacterType type)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedSelectionRequest(type);
		}
	}

	public void SendRankedHoverClickRequest(CharacterType type)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedHoverClickRequest(type);
		}
	}

	public void UpdateGameInfo(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo)
	{
		this.UpdateGameInfo(new LobbyGameInfo
		{
			GameConfig = gameConfig
		}, teamInfo);
	}

	public void UpdateGameInfo(LobbyGameInfo gameInfo, LobbyTeamInfo teamInfo)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.UpdateGameInfo(gameInfo, teamInfo);
		}
	}

	public void InvitePlayerToGame(string playerHandle, Action<GameInvitationResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.InvitePlayerToGame(playerHandle, onResponseCallback);
		}
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SpectateGame(playerHandle, onResponseCallback);
		}
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.RequestCrashReportArchiveName(numArchiveBytes, onResponseCallback);
		}
		return false;
	}

	public bool SendStatusReport(ClientStatusReport report)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.SendStatusReport(report);
		}
		return false;
	}

	public bool SendErrorReport(ClientErrorReport report)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.SendErrorReport(report);
		}
		return false;
	}

	public bool SendErrorSummary(ClientErrorSummary summary)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.SendErrorSummary(summary);
		}
		return false;
	}

	public bool SendFeedbackReport(ClientFeedbackReport report)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.SendFeedbackReport(report);
		}
		return false;
	}

	public bool SendPerformanceReport()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			ClientPerformanceReport clientPerformanceReport = new ClientPerformanceReport();
			clientPerformanceReport.PerformanceInfo = ClientPerformanceCollector.Get().Collect();
			return this.m_lobbyGameClientInterface.SendPerformanceReport(clientPerformanceReport);
		}
		return false;
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		return this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.SendChatNotification(recipientHandle, messageType, text);
	}

	public void SendUseOverconRequest(int id, string overconName, int actorId, int turn)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendUseOverconRequest(id, overconName, actorId, turn);
		}
	}

	public void SendSetDevTagRequest(bool active, Action<SetDevTagResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendSetDevTagRequest(active, onResponseCallback);
		}
	}

	public bool SendUIActionNotification(string context)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			return this.m_lobbyGameClientInterface.SendUIActionNotification(context);
		}
		return false;
	}

	private void HandleLobbyCustomGamesNotification(LobbyCustomGamesNotification notification)
	{
		this.CustomGameInfos = notification.CustomGameInfos;
		this.OnLobbyCustomGamesNotificationHolder(notification);
	}

	private void HandleGroupUpdateNotification(GroupUpdateNotification notification)
	{
		bool flag = false;
		bool flag2 = notification.Members.Count > 0;
		if (this.GroupInfo.InAGroup != flag2)
		{
			this.GroupInfo.InAGroup = flag2;
			if (UIFrontEnd.Get() != null)
			{
				UIFrontEnd.Get().m_frontEndChatConsole.ChangeChatRoom();
			}
		}
		this.GroupInfo.Members = notification.Members;
		this.GroupInfo.SelectedQueueType = notification.GameType;
		this.GroupInfo.SubTypeMask = notification.SubTypeMask;
		if (!this.GroupInfo.InAGroup)
		{
			this.GroupInfo.IsLeader = false;
		}
		else if (this.GroupInfo.InAGroup)
		{
			for (int i = 0; i < notification.Members.Count; i++)
			{
				if (notification.Members[i].IsLeader)
				{
					flag = (notification.Members[i].AccountID == this.GetPlayerAccountData().AccountId);
					break;
				}
			}
			this.GroupInfo.IsLeader = flag;
			if (UICharacterScreen.Get() != null)
			{
				if (notification.GameType == GameType.Coop)
				{
					bool value = false;
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
					if (AppState.GetCurrent() == AppState_CharacterSelect.Get() || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
					{
						characterSelectSceneStateParameters.AllyBotTeammatesClickable = new bool?(!flag);
						Dictionary<ushort, GameSubType> gameTypeSubTypes = this.GetGameTypeSubTypes(GameType.Coop);
						IEnumerable<ushort> enumerable = from p in gameTypeSubTypes.Keys
														 where (p & notification.SubTypeMask) != 0
														 select p;
						if (!enumerable.IsNullOrEmpty<ushort>())
						{
							GameSubType gameSubType;
							if (gameTypeSubTypes.TryGetValue(enumerable.First<ushort>(), out gameSubType) && gameSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
							{
								value = true;
							}
						}
					}
					characterSelectSceneStateParameters.SelectedEnemyBotDifficulty = new int?((int)notification.EnemyDifficulty);
					characterSelectSceneStateParameters.SelectedAllyBotDifficulty = new int?((int)notification.AllyDifficulty);
					characterSelectSceneStateParameters.AllyBotTeammatesSelected = new bool?(value);
					UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
				}
				if (!flag)
				{
					UICharacterScreen.Get().UpdateSubTypeMaskChecks(notification.SubTypeMask);
				}
			}
		}
		else
		{
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get() || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
			{
				if (this.GroupInfo.SelectedQueueType == GameType.Coop)
				{
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						AllyBotTeammatesSelected = new bool?(false)
					});
				}
			}
		}
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().DoRefreshFunctions(0x80);
		}
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get() || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
		{
			if (UICharacterScreen.Get() != null)
			{
				UICharacterScreen.Get().DoRefreshFunctions(0x40);
			}
		}
		if (DiscordClientInterface.IsEnabled)
		{
			if (DiscordClientInterface.IsSdkEnabled || DiscordClientInterface.IsInstalled)
			{
				if (this.GroupInfo.InAGroup)
				{
					bool enableAutoJoinDiscord = Options_UI.Get().GetEnableAutoJoinDiscord();
					if (enableAutoJoinDiscord)
					{
						this.JoinDiscord();
					}
					else if (!this.m_discordJoinSuggested)
					{
						this.m_discordJoinSuggested = true;
						string text = string.Format(StringUtil.TR("ClickToJoinDiscordGroupChat", "Global"), new object[0]);
						if (!DiscordClientInterface.IsSdkEnabled)
						{
							TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
						}
					}
				}
				else
				{
					this.LeaveDiscord();
				}
			}
		}
		this.OnGroupUpdateNotificationHolder();
	}

	private void HandleGGPackUsedNotification(UseGGPackNotification notification)
	{
		this.OnUseGGPackNotificationHolder(notification);
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		this.OnChatNotificationHolder(notification);
	}

	private void HandleOnSetDevTagNotification(SetDevTagResponse response)
	{
		this.OnSetDevTagResponseHolder(response);
	}

	private void HandleUseOverconNotification(UseOverconResponse notification)
	{
		this.OnUseOverconNotificationHolder(notification);
	}

	public void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		this.IsFriendListInitialized = true;
		if (notification.FriendList.IsDelta)
		{
			using (Dictionary<long, FriendInfo>.Enumerator enumerator = notification.FriendList.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, FriendInfo> keyValuePair = enumerator.Current;
					if (this.FriendList.Friends.ContainsKey(keyValuePair.Key))
					{
						this.FriendList.Friends[keyValuePair.Key] = keyValuePair.Value;
						if (keyValuePair.Value.FriendStatus == FriendStatus.Removed)
						{
							this.FriendList.Friends.Remove(keyValuePair.Key);
						}
					}
					else
					{
						if (keyValuePair.Value.FriendStatus != FriendStatus.Friend)
						{
							if (keyValuePair.Value.FriendStatus != FriendStatus.RequestReceived)
							{
								continue;
							}
						}
						this.FriendList.Friends.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}
		else
		{
			this.FriendList = notification.FriendList;
		}
		this.OnFriendStatusNotificationHolder(notification);
	}

	private void SendGameInviteConfirmationResponse(bool accepted, GameInviteConfirmationRequest request)
	{
		GameInviteConfirmationResponse gameInviteConfirmationResponse = new GameInviteConfirmationResponse();
		gameInviteConfirmationResponse.Accepted = accepted;
		gameInviteConfirmationResponse.GameCreatorAccountId = request.GameCreatorAccountId;
		gameInviteConfirmationResponse.ResponseId = request.RequestId;
		gameInviteConfirmationResponse.InitialRequestId = request.InitialRequestId;
		ClientGameManager.Get().LobbyInterface.SendMessage(gameInviteConfirmationResponse);
	}

	private void HandleGameInviteConfirmationRequest(GameInviteConfirmationRequest request)
	{
		string description = string.Format(StringUtil.TR("InviteToCustomGame", "Global"), request.GameCreatorHandle);
		UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("GameInvite", "Global"), description, StringUtil.TR("Join", "Global"), StringUtil.TR("Reject", "Global"), delegate(UIDialogBox UIDialogBox)
		{
			this.SendGameInviteConfirmationResponse(true, request);
		}, delegate(UIDialogBox UIDialogBox)
		{
			this.SendGameInviteConfirmationResponse(false, request);
		}, false, false);
	}

	private void HandleGroupConfirmationRequest(GroupConfirmationRequest request)
	{
		GroupJoinManager.Get().AddRequest(request);
	}

	private void HandleGroupSuggestionRequest(GroupSuggestionRequest request)
	{
		if (UIFrontEnd.Get() == null)
		{
			return;
		}
		if (UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame)
		{
			this.SendGroupSuggestion(false, false, request);
			return;
		}
		string description = string.Format(StringUtil.TR("InviteToGroupWithYou", "Global"), request.SuggesterAccountName, request.SuggestedAccountFullHandle);
		if (this.m_currentGroupSuggestDialogBox != null)
		{
			this.SendGroupSuggestion(false, true, request);
		}
		else
		{
			this.m_currentGroupSuggestDialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupSuggestion", "Global"), description, StringUtil.TR("Invite", "Global"), StringUtil.TR("Reject", "Global"), null, delegate(UIDialogBox UIDialogBox)
			{
				this.SendGroupSuggestion(true, false, request);
			}, delegate(UIDialogBox UIDialogBox)
			{
				this.SendGroupSuggestion(false, false, request);
			});
		}
	}

	private void HandleForceQueueNotification(ForceMatchmakingQueueNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && gameManager.GameInfo != null)
		{
			if (gameManager.GameInfo.GameStatus.IsActiveStatus())
			{
				Log.Error("Lobby should never send us a ForceQueueNotification({0}) when we're in a {1} game!", new object[]
				{
					notification.Action,
					gameManager.GameInfo.GameStatus
				});
				return;
			}
		}
		ForceMatchmakingQueueNotification.ActionType action = notification.Action;
		if (action != ForceMatchmakingQueueNotification.ActionType._000E)
		{
			if (action != ForceMatchmakingQueueNotification.ActionType._0012)
			{
				throw new Exception("Unhandled ForceQueueNotification.ActionType");
			}
			AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
		}
		else if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			AppState_GroupCharacterSelect.Get().ForceJoinQueue();
		}
		else
		{
			AppState_WaitingForGame.Get().Enter();
		}
	}

	private void SendGroupSuggestion(bool bAccepted, bool bBusy, GroupSuggestionRequest request)
	{
		if (bAccepted)
		{
			this.InviteToGroup(request.SuggestedAccountFullHandle, delegate(GroupInviteResponse r)
			{
				GroupSuggestionResponse groupSuggestionResponse2 = new GroupSuggestionResponse();
				groupSuggestionResponse2.SuggesterAccountId = request.SuggesterAccountId;
				if (!r.Success)
				{
					string text;
					if (r.LocalizedFailure != null)
					{
						text = r.LocalizedFailure.ToString();
					}
					else if (!r.ErrorMessage.IsNullOrEmpty())
					{
						text = string.Format("Failed: {0}#NeedsLocalization", r.ErrorMessage);
					}
					else
					{
						text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
					}
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = text,
						MessageType = ConsoleMessageType.SystemMessage
					}, null);
					groupSuggestionResponse2.SuggestionStatus = GroupSuggestionResponse.Status._000E;
				}
				else
				{
					groupSuggestionResponse2.SuggestionStatus = GroupSuggestionResponse.Status._0012;
				}
				this.m_lobbyGameClientInterface.SendMessage(groupSuggestionResponse2);
			});
		}
		else
		{
			GroupSuggestionResponse groupSuggestionResponse = new GroupSuggestionResponse();
			groupSuggestionResponse.SuggesterAccountId = request.SuggesterAccountId;
			groupSuggestionResponse.SuggestionStatus = ((!bBusy) ? GroupSuggestionResponse.Status._001D : GroupSuggestionResponse.Status._000E);
			this.m_lobbyGameClientInterface.SendMessage(groupSuggestionResponse);
		}
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		if (SequenceManager.Get() != null)
		{
			SequenceManager.Get().HandleOnGameStopped();
		}
		if (NetworkClient.active)
		{
			if (!this.IsServer())
			{
				NetworkManager.singleton.StopClient();
			}
		}
		ClientObserverManager component = base.GetComponent<ClientObserverManager>();
		if (component != null)
		{
			component.HandleGameStopped();
		}
		GameManager.Get().SetGameplayOverridesForCurrentGame(null);
		this.ResetLoadAssetsState();
		this.m_loadLevelOperationDone = false;
		this.m_loadedCharacterResourceCount = 0;
		this.m_spawnableObjectCount = 0;
		this.IsRegisteredToGameServer = false;
		this.m_withinReconnectReplay = false;
		this.m_withinReconnect = false;
		this.m_withinReconnectInstantly = false;
		this.m_lastReceivedMsgSeqNum = 0U;
		this.m_lastSentMsgSeqNum = 0U;
		this.m_replay = new Replay();
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			this.DisconnectFromLobbyServer();
			this.OnConnectedToLobbyServerHolder(response);
			if (this.IsConnectedToGameServer)
			{
				TextConsole.Get().Write(StringUtil.TR("FailedToConnectRetrying", "Global"), ConsoleMessageType.SystemMessage);
				this.ConnectToLobbyServer();
			}
			else
			{
				GameManager.Get().StopGame(GameResult.NoResult);
			}
			return;
		}
		this.IsRegistered = true;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		this.m_lobbyGameClientInterface.HeartbeatPeriod = hydrogenConfig.HeartbeatPeriod;
		this.m_lobbyGameClientInterface.HeartbeatTimeout = hydrogenConfig.HeartbeatTimeout;
		this.m_lobbyGameClientInterface.MaxSendBufferSize = hydrogenConfig.MaxSendBufferSize;
		this.m_lobbyGameClientInterface.MaxWaitTime = hydrogenConfig.MaxWaitTime;
		this.OnConnectedToLobbyServerHolder(response);
		if (this.IsConnectedToGameServer)
		{
			TextConsole.Get().Write("Reconnected to lobby server", ConsoleMessageType.SystemMessage);
			LobbyGameInfo previousGameInfo = GameManager.Get().GameInfo;
			this.RejoinGame(true, delegate(RejoinGameResponse res)
			{
				if (!res.Success)
				{
					Log.Error("{0} Failed to restore the previous game state {1}. Kicked", new object[]
					{
						this.PlayerInfo.Handle,
						previousGameInfo.GameServerProcessCode
					});
					GameResult gameResult = GameResult.ClientKicked;
					this.LeaveGame(false, gameResult);
					GameManager.Get().StopGame(gameResult);
				}
			});
		}
	}

	private void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		this.IsReady = true;
		this.CommerceURL = notification.CommerceURL;
		this.GroupInfo = notification.GroupInfo;
		this.EnvironmentType = notification.EnvironmentType;
		if (this.GroupInfo != null)
		{
			if (this.GroupInfo.ChararacterInfo != null)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientSelectedVisualInfo = new CharacterVisualInfo?(this.GroupInfo.ChararacterInfo.CharacterSkin)
				});
			}
			else
			{
				this.GroupInfo.SetCharacterInfo(new LobbyCharacterInfo(), false);
			}
			this.OnGroupUpdateNotificationHolder();
		}
		if (notification.AlertMissionData != null)
		{
			this.HandleLobbyAlertMissionDataNotification(notification.AlertMissionData);
		}
		if (notification.SeasonChapterQuests != null)
		{
			this.HandleLobbySeasonQuestDataNotification(notification.SeasonChapterQuests);
		}
		if (notification.Status != null)
		{
			this.HandleLobbyStatusNotification(notification.Status);
		}
		if (notification.AccountData != null)
		{
			this.HandleAccountDataUpdated(notification.AccountData);
			if (notification.AccountData.InventoryComponent != null)
			{
				this.OnInventoryComponentUpdatedHolder(notification.AccountData.InventoryComponent);
			}
			else
			{
				Log.Error("LobbyServerReadyNotification InventoryComponent is null", new object[0]);
			}
		}
		if (notification.CharacterDataList != null)
		{
			this.HandlePlayerCharacterDataUpdated(notification.CharacterDataList);
		}
		if (notification.FactionCompetitionStatus != null)
		{
			this.HandleFactionCompetitionNotification(notification.FactionCompetitionStatus);
		}
		this.OnLobbyServerReadyNotificationHolder(notification);
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		string format = "{0} ({1})";
		object arg = newLevel;
		object arg2;
		if (this.HasPurchasedGame)
		{
			arg2 = "purchased";
		}
		else
		{
			arg2 = "not purchased";
		}
		string text = string.Format(format, arg, arg2);
		Log.Info("Changed Access Level from {0} to {1}", new object[]
		{
			oldLevel.ToString(),
			text
		});
		this.OnLobbyServerClientAccessLevelChangeHolder(oldLevel, newLevel);
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage, bool allowRelogin, CloseStatusCode code)
	{
		ClientPerformanceCollector.Get().ObserveRTT(null);
		this.m_lobbyGameClientInterface = null;
		this.ClearLobbyState();
		bool flag = false;
		if (this.IsConnectedToGameServer)
		{
			lastLobbyErrorMessage = StringUtil.TR("DisconnectedFromServer", "Disconnected");
			flag = true;
		}
		this.IsRegistered = false;
		this.IsReady = false;
		this.AllowRelogin = allowRelogin;
		if (flag)
		{
			TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnecting", "Disconnected"), ConsoleMessageType.SystemMessage);
			this.ConnectToLobbyServer();
		}
		else
		{
			this.OnDisconnectedFromLobbyServerHolder(lastLobbyErrorMessage);
			GameResult gameResult;
			if (code == CloseStatusCode.PingTimeout)
			{
				gameResult = GameResult.ClientHeartbeatTimeoutToLobbyServer;
			}
			else
			{
				gameResult = GameResult.ClientNetworkErrorToLobbyServer;
			}
			GameResult gameResult2 = gameResult;
			GameManager.Get().StopGame(gameResult2);
			GameManager.Get().Reset();
		}
	}

	private void HandleQueueAssignmentNotification(MatchmakingQueueAssignmentNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		LobbyMatchmakingQueueInfo matchmakingQueueInfo = notification.MatchmakingQueueInfo;
		LobbyMatchmakingQueueInfo queueInfo = gameManager.QueueInfo;
		gameManager.SetQueueInfo(matchmakingQueueInfo);
		if (matchmakingQueueInfo == null)
		{
			if (queueInfo.GameConfig.GameType == GameType.Ranked)
			{
				UICharacterSelectWorldObjects.Get().SetCharacterReady(0, false);
			}
		}
		if (queueInfo != null)
		{
			Log.Info("Unassigned from queue {0}", new object[]
			{
				queueInfo.GameType
			});
			this.OnQueueLeftHolder();
			if (matchmakingQueueInfo == null)
			{
				AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
			}
		}
		if (matchmakingQueueInfo != null)
		{
			Log.Info("Assigned to queue {0}", new object[]
			{
				matchmakingQueueInfo.GameType
			});
			this.OnQueueEnteredHolder();
		}
		UICharacterScreen.Get().DoRefreshFunctions(0x80);
		NavigationBar.Get().UpdateStatusMessage();
		this.OnQueueAssignmentNotificationHolder(notification);
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.GameplayOverrides != null)
		{
			this.SetGameplayOverrides(notification.GameplayOverrides);
		}
		if (notification.ErrorReportRate != null)
		{
			float num = (float)notification.ErrorReportRate.Value.TotalSeconds;
			ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
			if (clientExceptionDetector != null)
			{
				if (num > 0f)
				{
					Log.Info("Will send client errors to the server every {0}.", new object[]
					{
						LocalizationArg_TimeSpan.Create(notification.ErrorReportRate.Value).TR()
					});
					clientExceptionDetector.SecondsBetweenSendingErrorPackets = num;
				}
				else
				{
					clientExceptionDetector.FlushErrorsToLobby();
					Log.Info("Will send client errors to the server immediately", new object[0]);
					clientExceptionDetector.SecondsBetweenSendingErrorPackets = 0f;
				}
			}
			else
			{
				Log.Warning("Failed to configure ClientExceptionDetector to use a {0} second window", new object[]
				{
					num
				});
			}
		}
		if (notification.ServerMessageOverrides != null)
		{
			this.ServerMessageOverrides = notification.ServerMessageOverrides;
		}
		if (notification.ClientAccessLevel != ClientAccessLevel.Unknown)
		{
			ClientAccessLevel clientAccessLevel = this.ClientAccessLevel;
			bool hasPurchasedGame = this.HasPurchasedGame;
			int highestPurchasedGamePack = this.HighestPurchasedGamePack;
			this.m_clientAccessLevel = notification.ClientAccessLevel;
			this.HasPurchasedGame = notification.HasPurchasedGame;
			this.HighestPurchasedGamePack = notification.HighestPurchasedGamePack;
			if (clientAccessLevel == this.ClientAccessLevel)
			{
				if (hasPurchasedGame == this.HasPurchasedGame)
				{
					if (highestPurchasedGamePack == this.HighestPurchasedGamePack)
					{
						goto IL_19A;
					}
				}
			}
			this.HandleLobbyServerClientAccessLevelChange(clientAccessLevel, this.ClientAccessLevel);
		}
		IL_19A:
		if (notification.ServerLockState != ServerLockState.Unknown)
		{
			ServerLockState serverLockState = this.ServerLockState;
			this.ServerLockState = notification.ServerLockState;
			if (serverLockState != this.ServerLockState)
			{
				this.OnLobbyServerLockStateChangeHolder(serverLockState, this.ServerLockState);
			}
		}
		this.ConnectionQueueInfo = notification.ConnectionQueueInfo;
		if (notification.UtcNow != default(DateTime))
		{
			if (this.TimeOffset != notification.TimeOffset)
			{
				TextConsole.Get().Write(string.Format("Global Time Offset Is Now: {0}", notification.TimeOffset.ToString()), ConsoleMessageType.SystemMessage);
			}
			this.ServerUtcTime = notification.UtcNow;
			this.ServerPacificTime = notification.PacificNow;
			this.ClientUtcTime = DateTime.UtcNow;
			this.TimeOffset = notification.TimeOffset;
		}
		this.OnLobbyStatusNotificationHolder(notification);
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		this.SetGameplayOverrides(notification.GameplayOverrides);
	}

	private void SetGameplayOverrides(LobbyGameplayOverrides gameplayOverrides)
	{
		bool flag;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				flag = (GameManager.Get().GameplayOverrides.EnableCards != gameplayOverrides.EnableCards);
				goto IL_5D;
			}
		}
		flag = false;
		IL_5D:
		bool flag2 = flag;
		GameManager.Get().SetGameplayOverrides(gameplayOverrides);
		if (flag2)
		{
			if (UICharacterSelectScreenController.Get() != null)
			{
				if (UICharacterSelectCharacterSettingsPanel.Get() != null)
				{
					if (!gameplayOverrides.EnableCards)
					{
						if (UICharacterSelectCharacterSettingsPanel.Get().GetTabPanel() == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts)
						{
							UICharacterSelectCharacterSettingsPanel.Get().OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins, false);
						}
					}
					UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay, false, false);
				}
			}
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				if (UIRankedCharacterSelectSettingsPanel.Get().GetTabPanel() == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts)
				{
					UIRankedCharacterSelectSettingsPanel.Get().OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins, false);
				}
				UIRankedCharacterSelectSettingsPanel.Get().Refresh();
			}
		}
		ClientPerformanceCollector clientPerformanceCollector = ClientPerformanceCollector.Get();
		if (gameplayOverrides.EnableClientPerformanceCollecting)
		{
			this.m_taskScheduler.AddTask(this.m_clientPerformanceCollectTask, gameplayOverrides.ClientPerformanceCollectingFrequency, false);
			clientPerformanceCollector.ObserveRTT(this.m_lobbyGameClientInterface.WebSocket);
			clientPerformanceCollector.StartCollecting();
		}
		else
		{
			this.m_taskScheduler.RemoveTask(this.m_clientPerformanceCollectTask);
			clientPerformanceCollector.ObserveRTT(null);
			clientPerformanceCollector.StopCollecting();
		}
		this.OnLobbyGameplayOverridesChangeHolder(gameplayOverrides);
	}

	private void HandleLobbyAlertMissionDataNotification(LobbyAlertMissionDataNotification notification)
	{
		this.AlertMissionsData = notification;
		this.OnAlertMissionDataChangeHolder(notification);
	}

	private void HandleLobbySeasonQuestDataNotification(LobbySeasonQuestDataNotification notification)
	{
		this.SeasonChapterQuests = notification.SeasonChapterQuests;
		this.OnSeasonChapterQuestsChangeHolder(notification.SeasonChapterQuests);
	}

	private void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		LobbyMatchmakingQueueInfo matchmakingQueueInfo = notification.MatchmakingQueueInfo;
		if (gameManager.QueueInfo != null)
		{
			if (gameManager.QueueInfo.GameType == matchmakingQueueInfo.GameType)
			{
				GameManager.Get().SetQueueInfo(notification.MatchmakingQueueInfo);
				if (GameManager.Get().QueueInfo != null)
				{
					UICharacterSelectScreenController.Get().NotifiedEnteredQueue();
				}
				NavigationBar.Get().UpdateStatusMessage();
				this.OnQueueStatusNotificationHolder(notification);
				return;
			}
		}
		string message = "Ignoring status update for queue {0}";
		object[] array = new object[1];
		int num = 0;
		object obj;
		if (matchmakingQueueInfo == null)
		{
			obj = "(null)";
		}
		else
		{
			obj = matchmakingQueueInfo.GameType.ToString();
		}
		array[num] = obj;
		Log.Warning(message, array);
	}

	private void HandleGameAssignmentNotification(GameAssignmentNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyGameInfo gameInfo2 = notification.GameInfo;
		LobbyGameplayOverrides gameplayOverrides = notification.GameplayOverrides;
		this.m_gameResult = notification.GameResult;
		this.m_reconnected = notification.Reconnection;
		this.m_observer = notification.Observer;
		string text;
		if (gameInfo2 != null)
		{
			text = gameInfo2.Name;
		}
		else
		{
			text = string.Empty;
		}
		string text2 = text;
		bool flag;
		if (gameInfo != null)
		{
			flag = !gameInfo.GameServerProcessCode.IsNullOrEmpty();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3 = gameInfo2 != null && !gameInfo2.GameServerProcessCode.IsNullOrEmpty();
		bool flag4;
		if (flag2)
		{
			if (flag3)
			{
				flag4 = (gameInfo2.GameServerProcessCode != gameInfo.GameServerProcessCode);
				goto IL_D2;
			}
		}
		flag4 = false;
		IL_D2:
		bool flag5 = flag4;
		string message = "Received Game Assignment Notification {0} (assigned={1} assigning={2} reassigning={3}){4}";
		object[] array = new object[5];
		array[0] = text2;
		array[1] = flag2;
		array[2] = flag3;
		array[3] = flag5;
		int num = 4;
		object obj;
		if (notification.Reconnection)
		{
			obj = " (reconnected)";
		}
		else
		{
			obj = string.Empty;
		}
		array[num] = obj;
		Log.Info(message, array);
		if (flag2)
		{
			if (flag3)
			{
				if (!flag5)
				{
					goto IL_16F;
				}
			}
			Log.Info("Unassigned from game {0}", new object[]
			{
				gameManager.GameInfo.Name
			});
			gameManager.SetGameplayOverridesForCurrentGame(null);
		}
		IL_16F:
		if (!flag2)
		{
			if (flag3)
			{
				goto IL_199;
			}
		}
		if (!flag5)
		{
			goto IL_1B9;
		}
		IL_199:
		Log.Info("Assigned to game {0}", new object[]
		{
			gameInfo2.Name
		});
		gameManager.SetGameplayOverridesForCurrentGame(gameplayOverrides);
		IL_1B9:
		if (!this.IsServer())
		{
			if (notification.PlayerInfo != null)
			{
				gameManager.SetPlayerInfo(notification.PlayerInfo);
			}
			if (gameInfo2 != null)
			{
				GameStatus gameStatus = gameManager.GameStatus;
				gameManager.SetGameInfo(gameInfo2);
				if (gameInfo2.GameStatus.IsActiveStatus())
				{
					IEnumerator<GameStatus> enumerator = gameInfo2.GameStatus.GetValues<GameStatus>().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							GameStatus gameStatus2 = enumerator.Current;
							if (gameStatus2.IsActiveStatus() && gameInfo2.GameStatus > gameStatus2)
							{
								if (gameStatus.IsActiveStatus())
								{
									if (gameInfo2.GameStatus <= gameStatus)
									{
										continue;
									}
								}
								this.SetGameStatus(gameStatus2, GameResult.NoResult, true);
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.SetGameStatus(gameInfo2.GameStatus, this.m_gameResult, true);
			}
			else
			{
				this.SetGameStatus(GameStatus.Stopped, this.m_gameResult, true);
				ClientAccessLevel clientAccessLevel = this.ClientAccessLevel;
				gameManager.SetGameInfo(new LobbyGameInfo());
				gameManager.SetPlayerInfo(null);
				gameManager.SetTeamInfo(null);
				gameManager.ForbiddenDevKnowledge = null;
				if (gameManager.TeamPlayerInfo != null)
				{
					gameManager.TeamPlayerInfo.Clear();
				}
				gameManager.GameInfo.GameConfig = new LobbyGameConfig();
				if (clientAccessLevel != this.ClientAccessLevel)
				{
					this.HandleLobbyServerClientAccessLevelChange(clientAccessLevel, this.ClientAccessLevel);
				}
			}
		}
		if (NavigationBar.Get() != null)
		{
			NavigationBar.Get().UpdateStatusMessage();
		}
		if (this.m_gameResult == GameResult.Requeued)
		{
		}
		if (DiscordClientInterface.IsEnabled)
		{
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				if (!DiscordClientInterface.IsInstalled)
				{
					goto IL_53D;
				}
			}
			if (!flag2)
			{
				if (flag3)
				{
					goto IL_3E7;
				}
			}
			if (flag5)
			{
			}
			else
			{
				if (!flag2)
				{
					goto IL_53D;
				}
				if (flag3)
				{
					if (!flag5)
					{
						goto IL_53D;
					}
				}
				bool flag6;
				if (this.GroupInfo.InAGroup)
				{
					if (!this.m_discordConnecting)
					{
						if (!this.m_discordConnected)
						{
							goto IL_530;
						}
					}
					flag6 = (this.GetDiscordJoinType() == DiscordJoinType._000E);
					goto IL_531;
				}
				IL_530:
				flag6 = false;
				IL_531:
				if (!flag6)
				{
					this.LeaveDiscord();
					goto IL_53D;
				}
				goto IL_53D;
			}
			IL_3E7:
			if (gameInfo2 != null && gameInfo2.GameConfig != null && gameInfo2.GameConfig.GameType != GameType.Practice)
			{
				if (gameInfo2.GameConfig.GameType != GameType.Tutorial)
				{
					if (gameInfo2.GameConfig.GameType != GameType.NewPlayerSolo)
					{
						bool flag7;
						if (Options_UI.Get() != null)
						{
							flag7 = Options_UI.Get().GetEnableAutoJoinDiscord();
						}
						else
						{
							flag7 = false;
						}
						bool flag8 = flag7;
						if (flag8)
						{
							this.JoinDiscord();
						}
						else if (!DiscordClientInterface.IsSdkEnabled)
						{
							string text3 = string.Format(StringUtil.TR("ClickToJoinDiscordTeamChat", "Global"), new object[0]);
							TextConsole.Get().Write(text3, ConsoleMessageType.SystemMessage);
						}
					}
				}
			}
		}
		IL_53D:
		this.OnGameAssignmentNotificationHolder(notification);
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = notification.GameInfo;
		LobbyPlayerInfo playerInfo = notification.PlayerInfo;
		LobbyTeamInfo teamInfo = notification.TeamInfo;
		GameType gameType = gameInfo.GameConfig.GameType;
		Log.Info("Received Game Info Notification {0} ({1})", new object[]
		{
			gameInfo.Name,
			playerInfo.GetHandle()
		});
		if (gameManager.GameInfo.GameServerProcessCode != gameInfo.GameServerProcessCode)
		{
			Log.Warning("Ignoring info({0}) update for game {1}, expected game {2}", new object[]
			{
				notification.GameInfo.GameStatus,
				gameInfo.GameServerProcessCode,
				gameManager.GameInfo.GameServerProcessCode
			});
			return;
		}
		if (!this.IsServer())
		{
			bool flag = DiscordClientInterface.Get().ChannelInfo != null;
			if (flag)
			{
				bool flag2;
				if (gameInfo != null && gameInfo.GameStatus != GameStatus.Stopped)
				{
					flag2 = gameInfo.IsCustomGame;
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				bool flag4;
				if (teamInfo != null)
				{
					if (playerInfo != null && gameManager.PlayerInfo != null && playerInfo.TeamId != Team.Invalid)
					{
						flag4 = (playerInfo.TeamId != gameManager.PlayerInfo.TeamId);
						goto IL_158;
					}
				}
				flag4 = false;
				IL_158:
				bool flag5 = flag4;
				if (flag3)
				{
					if (flag5)
					{
						DiscordUserInfo userInfo = DiscordClientInterface.Get().UserInfo;
						this.JoinDiscordChannel(userInfo);
					}
				}
			}
		}
		gameManager.SetPlayerInfo(playerInfo);
		if (playerInfo != null)
		{
			if (teamInfo != null)
			{
				if (teamInfo.TeamPlayerInfo != null)
				{
					gameManager.SetTeamPlayerInfo(teamInfo.TeamInfo(playerInfo.TeamId).ToList<LobbyPlayerInfo>());
					goto IL_1D7;
				}
			}
		}
		gameManager.SetTeamPlayerInfo(null);
		IL_1D7:
		if (!this.IsServer())
		{
			if (gameInfo.GameConfig == null)
			{
				gameInfo.GameConfig = new LobbyGameConfig();
			}
			gameManager.SetGameInfo(gameInfo);
			gameManager.SetTeamInfo(teamInfo);
			gameManager.ForbiddenDevKnowledge = notification.DevOnly;
			UITextConsole.AddToTeamMatesToAutoComplete(teamInfo);
			if (gameInfo.GameStatus != gameManager.GameStatus)
			{
				this.SetGameStatus(gameInfo.GameStatus, gameInfo.GameResult, true);
			}
		}
		if (gameManager.GameInfo != null)
		{
			if (gameManager.GameInfo.GameConfig != null)
			{
				if (notification.TierCurrent != null)
				{
					if (notification.TierCurrent.Tier != 0)
					{
						this.TierCurrent = notification.TierCurrent;
						string tierName = ClientGameManager.Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierCurrent.Tier);
						Log.Info("We are currently at tier {0} (points {1})", new object[]
						{
							tierName,
							notification.TierCurrent.Points
						});
					}
				}
				if (notification.TierChangeMin != null)
				{
					if (notification.TierChangeMin.Tier != 0)
					{
						this.TierChangeMin = notification.TierChangeMin;
						string tierName2 = ClientGameManager.Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierChangeMin.Tier);
						Log.Info("If we lose this game we could fall to tier {0} (points {1})", new object[]
						{
							tierName2,
							notification.TierChangeMin.Points
						});
					}
				}
				if (notification.TierChangeMax != null)
				{
					if (notification.TierChangeMax.Tier != 0)
					{
						this.TierChangeMax = notification.TierChangeMax;
						string tierName3 = ClientGameManager.Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierChangeMax.Tier);
						Log.Info("If we win this game we could rise to tier {0} (points {1})", new object[]
						{
							tierName3,
							notification.TierChangeMax.Points
						});
					}
				}
			}
		}
		if (NavigationBar.Get() != null)
		{
			NavigationBar.Get().UpdateStatusMessage();
		}
		if (AppState.GetCurrent() == AppState_GameLoading.Get())
		{
			if (gameType != GameType.Tutorial)
			{
				if (UILoadingScreenPanel.Get() != null)
				{
					UILoadingScreenPanel.Get().ShowTeams();
				}
			}
		}
		this.OnGameInfoNotificationHolder(notification);
	}

	private void HandleGameStatusNotification(GameStatusNotification notification)
	{
		Log.Info("Received Game Status Notification {0} {1}", new object[]
		{
			notification.GameServerProcessCode,
			notification.GameStatus
		});
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameInfo.GameServerProcessCode != notification.GameServerProcessCode)
		{
			Log.Warning("Ignoring status({0}) update for game {1}, we believe we're in game {2}", new object[]
			{
				notification.GameStatus,
				notification.GameServerProcessCode,
				gameManager.GameInfo.GameServerProcessCode
			});
			return;
		}
		if (!this.IsServer())
		{
			gameManager.GameInfo.GameStatus = notification.GameStatus;
			if (notification.GameStatus != gameManager.GameStatus)
			{
				this.SetGameStatus(notification.GameStatus, GameResult.NoResult, true);
			}
		}
		if (NavigationBar.Get() != null)
		{
			NavigationBar.Get().UpdateStatusMessage();
		}
		GameInfoNotification gameInfoNotification = new GameInfoNotification();
		gameInfoNotification.GameInfo = gameManager.GameInfo;
		gameInfoNotification.PlayerInfo = gameManager.PlayerInfo;
		gameInfoNotification.TeamInfo = gameManager.TeamInfo;
		this.OnGameInfoNotificationHolder(gameInfoNotification);
	}

	private void HandleGameLaunched(GameType gameType)
	{
		this.ConnectToGameServer();
	}

	private void HandleGameStatusChanged(GameStatus gameStatus)
	{
	}

	private void SetGameStatus(GameStatus gameStatus, GameResult gameResult = GameResult.NoResult, bool notify = true)
	{
		GameManager.Get().SetGameStatus(gameStatus, gameResult, notify);
		if (gameStatus == GameStatus.Loaded)
		{
			this.WaitingForSkinSelectResponse.Clear();
		}
	}

	public void Replay_SetGameStatus(GameStatus gameStatus)
	{
		this.SetGameStatus(gameStatus, GameResult.NoResult, true);
	}

	private void ConnectToGameServer()
	{
		if (this.IsServer())
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		if (ReplayPlayManager.Get())
		{
			if (ReplayPlayManager.Get().IsPlayback())
			{
				this.ResetLoadAssetsState();
				Log.Info("Stub-connecting to replay system", new object[0]);
				MyNetworkManager.Get().MyStartClientStub();
				goto IL_14A;
			}
		}
		if (gameManager.GameInfo != null)
		{
			if (string.IsNullOrEmpty(gameManager.GameInfo.GameServerAddress))
			{
			}
			else
			{
				if (!Uri.IsWellFormedUriString(gameManager.GameInfo.GameServerAddress, UriKind.Absolute))
				{
					throw new FormatException(string.Format("Could not parse game server address {0}", gameManager.GameInfo.GameServerAddress));
				}
				this.ResetLoadAssetsState();
				this.IsRegisteredToGameServer = false;
				Log.Info("Connecting to {0}", new object[]
				{
					gameManager.GameInfo.GameServerAddress
				});
				MyNetworkManager.Get().MyStartClient(gameManager.GameInfo.GameServerAddress, this.Handle);
				goto IL_14A;
			}
		}
		Log.Error("Game server address is empty", new object[0]);
		return;
		IL_14A:
		if (!this.m_registeredHandlers)
		{
			MyNetworkManager myNetworkManager = MyNetworkManager.Get();
			myNetworkManager.m_OnClientConnect += this.HandleNetworkConnect;
			myNetworkManager.m_OnClientDisconnect += this.HandleNetworkDisconnect;
			myNetworkManager.m_OnClientError += this.HandleNetworkError;
			this.m_registeredHandlers = true;
		}
		this.Client.RegisterHandler(0x34, new NetworkMessageDelegate(this.HandleLoginResponse));
		this.Client.RegisterHandler(0x3E, new NetworkMessageDelegate(this.HandleServerAssetsLoadingProgressUpdate));
		this.Client.RegisterHandler(0x36, new NetworkMessageDelegate(this.HandleSpawningObjectsNotification));
		this.Client.RegisterHandler(0x30, new NetworkMessageDelegate(this.HandleReplayManagerFile));
		this.Client.RegisterHandler(0x38, new NetworkMessageDelegate(this.HandleReconnectReplayStatus));
		this.Client.RegisterHandler(0x44, new NetworkMessageDelegate(this.HandleEndGameNotification));
		ClientObserverManager component = base.GetComponent<ClientObserverManager>();
		if (component != null)
		{
			component.ConnectingToGameServer();
		}
		SinglePlayerManager.UnregisterSpawnHandler();
		SinglePlayerManager.RegisterSpawnHandler();
		this.m_withinReconnectReplay = false;
		this.m_withinReconnect = false;
	}

	public bool ReconnectToGameServerInstantly(MyNetworkClientConnection disconnectedConnection)
	{
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		if (gameplayOverrides != null)
		{
			if (!gameplayOverrides.AllowReconnectingToGameInstantly)
			{
			}
			else
			{
				Log.Info("Reconnecting to game instantly {0}", new object[]
				{
					this.GameInfo
				});
				TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnectingGame", "Disconnected"), ConsoleMessageType.SystemMessage);
				if (this.IsServer())
				{
					return false;
				}
				GameManager gameManager = GameManager.Get();
				if (gameManager.GameInfo != null)
				{
					if (string.IsNullOrEmpty(gameManager.GameInfo.GameServerAddress))
					{
					}
					else
					{
						if (!Uri.IsWellFormedUriString(gameManager.GameInfo.GameServerAddress, UriKind.Absolute))
						{
							throw new FormatException(string.Format("Could not parse game server address {0}", gameManager.GameInfo.GameServerAddress));
						}
						if (this.MyConnection == null)
						{
							return false;
						}
						Log.Info("Reconnecting instantly to {0}", new object[]
						{
							gameManager.GameInfo.GameServerAddress
						});
						this.MyConnection.Connect();
						this.IsRegisteredToGameServer = false;
						this.m_withinReconnectReplay = false;
						this.m_withinReconnect = false;
						this.m_withinReconnectInstantly = true;
						this.m_lastReceivedMsgSeqNum = disconnectedConnection.lastMessageIncomingSeqNum;
						this.m_lastSentMsgSeqNum = disconnectedConnection.lastMessageOutgoingSeqNum;
						return true;
					}
				}
				Log.Error("Game server address is empty", new object[0]);
				return false;
			}
		}
		return false;
	}

	public void ReloginToGameServerInstantly(MyNetworkClientConnection reconnectedConnection)
	{
		Log.Info("Relogging in to game instantly {0}", new object[]
		{
			this.GameInfo
		});
		TextConsole.Get().Write(StringUtil.TR("ReconnectedGame", "Disconnected"), ConsoleMessageType.SystemMessage);
		reconnectedConnection.lastMessageIncomingSeqNum = this.m_lastReceivedMsgSeqNum;
		reconnectedConnection.lastMessageOutgoingSeqNum = this.m_lastSentMsgSeqNum;
		if (this.m_lobbyGameClientInterface != null)
		{
			GameManager.LoginRequest loginRequest = new GameManager.LoginRequest();
			loginRequest.AccountId = Convert.ToString(this.m_lobbyGameClientInterface.SessionInfo.AccountId);
			loginRequest.SessionToken = Convert.ToString(this.m_lobbyGameClientInterface.SessionInfo.SessionToken);
			loginRequest.PlayerId = this.PlayerInfo.PlayerId;
			loginRequest.LastReceivedMsgSeqNum = this.m_lastReceivedMsgSeqNum;
			this.Client.Send(0x33, loginRequest);
		}
	}

	private void HandleGameMessageSending(UNetMessage message)
	{
		this.m_replay.RecordRawNetworkMessage(message.Bytes, message.NumBytes);
	}

	private void LoginToGameServer(NetworkConnection conn)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			GameManager gameManager = GameManager.Get();
			if (ReplayPlayManager.Get())
			{
				if (ReplayPlayManager.Get().IsPlayback())
				{
					Log.Info("Stub-connected to replay system", new object[]
					{
						gameManager.GameInfo.GameServerAddress
					});
					goto IL_8C;
				}
			}
			Log.Info("Connected to {0}", new object[]
			{
				gameManager.GameInfo.GameServerAddress
			});
			IL_8C:
			ClientScene.AddPlayer(conn, 0);
			GameManager.LoginRequest loginRequest = new GameManager.LoginRequest();
			loginRequest.AccountId = Convert.ToString(this.m_lobbyGameClientInterface.SessionInfo.AccountId);
			loginRequest.SessionToken = Convert.ToString(this.m_lobbyGameClientInterface.SessionInfo.SessionToken);
			loginRequest.PlayerId = gameManager.PlayerInfo.PlayerId;
			loginRequest.LastReceivedMsgSeqNum = this.m_lastReceivedMsgSeqNum;
			this.Client.Send(0x33, loginRequest);
		}
	}

	private void HandleNetworkConnect(NetworkConnection conn)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Network connect error", new object[0]);
			return;
		}
		this.LoginToGameServer(conn);
	}

	private void HandleNetworkError(NetworkConnection conn, NetworkError errorCode)
	{
	}

	private void HandleNetworkDisconnect(NetworkConnection conn)
	{
		GameResult gameResult = this.m_gameResult;
		if (gameResult == GameResult.NoResult && this.MyConnection != null)
		{
			GameResult gameResult2;
			if (this.MyConnection.CloseStatusCode == CloseStatusCode.PingTimeout)
			{
				gameResult2 = GameResult.ClientHeartbeatTimeoutToGameServer;
			}
			else
			{
				gameResult2 = GameResult.ClientNetworkErrorToGameServer;
			}
			gameResult = gameResult2;
		}
		Log.Info("Disconnected from game server {0}", new object[]
		{
			gameResult
		});
		this.IsRegisteredToGameServer = false;
		this.LeaveGame(false, gameResult);
		GameManager.Get().StopGame(gameResult);
	}

	private void HandleServerAssetsLoadingProgressUpdate(NetworkMessage msg)
	{
		if (this.IsServer())
		{
			return;
		}
		GameManager.AssetsLoadingProgress assetsLoadingProgress = msg.ReadMessage<GameManager.AssetsLoadingProgress>();
		if (assetsLoadingProgress == null)
		{
			return;
		}
		float loadingProgress = (float)assetsLoadingProgress.TotalLoadingProgress / 100f;
		UILoadingScreenPanel.Get().SetLoadingProgress(assetsLoadingProgress.PlayerId, loadingProgress, false);
	}

	private void HandleSpawningObjectsNotification(NetworkMessage msg)
	{
		if (this.IsServer())
		{
			return;
		}
		GameManager.SpawningObjectsNotification spawningObjectsNotification = msg.ReadMessage<GameManager.SpawningObjectsNotification>();
		if (spawningObjectsNotification == null)
		{
			return;
		}
		this.m_spawnableObjectCount = spawningObjectsNotification.SpawnableObjectCount;
	}

	public static string FormReplayFilename(DateTime time, string gameServerProcessCode, string handle)
	{
		return ClientGameManager.FormReplayFilename(time.ToString("MM_dd_yyyy__HH_mm_ss"), gameServerProcessCode, handle);
	}

	public static string FormReplayFilename(string timeStr, string gameServerProcessCode, string handle)
	{
		return string.Format("{0}__{1}__{2}__{3}.arr", new object[]
		{
			timeStr,
			gameServerProcessCode,
			BuildVersion.MiniVersionString,
			WWW.EscapeURL(handle)
		});
	}

	public static string RemoveTimeFromReplayFilename(string filename)
	{
		return filename.Substring(0x14);
	}

	private void HandleReplayManagerFile(NetworkMessage msg)
	{
		GameManager.ReplayManagerFile replayManagerFile = msg.ReadMessage<GameManager.ReplayManagerFile>();
		if (replayManagerFile == null)
		{
			return;
		}
		if (replayManagerFile.Restart)
		{
			Log.Info("Starting replay save for {0}", new object[]
			{
				GameManager.Get().GameInfo.GameServerProcessCode
			});
			this.m_replayManagerAccumulated = string.Empty;
		}
		this.m_replayManagerAccumulated += replayManagerFile.Fragment;
		if (replayManagerFile.Save)
		{
			try
			{
				Replay replay = JsonUtility.FromJson<Replay>(this.m_replayManagerAccumulated);
				replay.m_versionMini = BuildVersion.MiniVersionString;
				replay.m_versionFull = BuildVersion.FullVersionString;
				string value = JsonUtility.ToJson(replay);
				string text = Path.Combine(HydrogenConfig.Get().ReplaysPath, ClientGameManager.FormReplayFilename(DateTime.Now, GameManager.Get().GameInfo.GameServerProcessCode, this.Handle));
				FileInfo fileInfo = new FileInfo(text);
				fileInfo.Directory.Create();
				StreamWriter streamWriter = new StreamWriter(text);
				streamWriter.WriteLine(value);
				streamWriter.Close();
				Log.Info("Saved replay to {0}", new object[]
				{
					text
				});
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				Log.Info("Failed to save replay", new object[0]);
			}
			this.m_replayManagerAccumulated = string.Empty;
		}
	}

	private void HandleReconnectReplayStatus(NetworkMessage msg)
	{
		GameManager.ReconnectReplayStatus reconnectReplayStatus = msg.ReadMessage<GameManager.ReconnectReplayStatus>();
		if (reconnectReplayStatus == null)
		{
			return;
		}
		string format = "{0} reconnection replay phase";
		object[] array = new object[1];
		int num = 0;
		object obj;
		if (reconnectReplayStatus.WithinReconnectReplay)
		{
			obj = "Entering";
		}
		else
		{
			obj = "Exiting";
		}
		array[num] = obj;
		UnityEngine.Debug.LogFormat(format, array);
		if (this.m_withinReconnectReplay != reconnectReplayStatus.WithinReconnectReplay)
		{
			this.m_withinReconnectReplay = reconnectReplayStatus.WithinReconnectReplay;
			if (!this.m_withinReconnectReplay)
			{
				this.m_withinReconnect = false;
			}
			GameEventManager.ReconnectReplayStateChangedArgs args = new GameEventManager.ReconnectReplayStateChangedArgs(this.m_withinReconnectReplay);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ReconnectReplayStateChanged, args);
		}
		ClientIdleTimer.Get().ResetIdleTimer();
	}

	private void HandleEndGameNotification(NetworkMessage msg)
	{
		if (msg.ReadMessage<GameManager.EndGameNotification>() == null)
		{
			return;
		}
		GameResult gameResult = GameResult.NoResult;
		this.LeaveGame(true, gameResult);
		GameManager.Get().StopGame(gameResult);
	}

	private void HandleLoginResponse(NetworkMessage msg)
	{
		if (this.IsServer())
		{
			return;
		}
		GameManager.LoginResponse loginResponse = msg.ReadMessage<GameManager.LoginResponse>();
		if (loginResponse == null)
		{
			return;
		}
		if (loginResponse.Success)
		{
			this.m_withinReconnect = loginResponse.Reconnecting;
			this.IsRegisteredToGameServer = true;
			if (this.m_withinReconnectInstantly)
			{
				this.m_withinReconnectInstantly = false;
				TextConsole.Get().Write(StringUtil.TR("LoggedIntoGame", "Disconnected"), ConsoleMessageType.SystemMessage);
				uint num = msg.conn.lastMessageOutgoingSeqNum - loginResponse.LastReceivedMsgSeqNum;
				if (num > 0U)
				{
					uint startSeqNum = loginResponse.LastReceivedMsgSeqNum + 1U;
					uint lastSentMsgSeqNum = this.m_lastSentMsgSeqNum;
					IEnumerable<Replay.Message> rawNetworkMessages = this.m_replay.GetRawNetworkMessages(startSeqNum, lastSentMsgSeqNum);
					IEnumerator<Replay.Message> enumerator = rawNetworkMessages.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Replay.Message message = enumerator.Current;
							msg.conn.ResendBytes(message.data, message.data.Length, 0);
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
			}
			return;
		}
		string text = string.Format("Login request failed: {0}", loginResponse.ErrorMessage);
		Log.Error(text, new object[0]);
		AppState_GameTeardown.Get().Enter(GameResult.ClientLoginFailedToGameServer, text);
	}

	public void DisableFrontEnd()
	{
		if (UIFrontEnd.Get() != null)
		{
			UIFrontEnd.Get().Disable();
		}
	}

	private void HandleVisualsSceneLoading(string visualsSceneName)
	{
		this.m_loadLevelOperationBundleSceneNames.Add(new KeyValuePair<string, string>(visualsSceneName, visualsSceneName));
	}

	public void LoadAssets()
	{
		if (this.IsServer())
		{
			return;
		}
		this.m_loading = true;
		this.m_loadLevelOperationDone = false;
		this.m_loadedCharacterResourceCount = 0;
		this.m_spawnableObjectCount = 0;
		this.m_assetsLoadingState.Reset();
		GameManager gameManager = GameManager.Get();
		string map = gameManager.GameConfig.Map;
		string text;
		if (AssetBundleManager.Get().SceneExistsInBundle("testing", gameManager.GameConfig.Map))
		{
			text = "testing";
		}
		else
		{
			text = "maps";
		}
		this.m_loadLevelOperationBundleSceneNames.Clear();
		this.m_loadLevelOperationBundleSceneNames.Add(new KeyValuePair<string, string>(map, text));
		this.m_loadLevelOperation = new AssetBundleManager.LoadSceneAsyncOperation
		{
			sceneName = gameManager.GameConfig.Map,
			bundleName = text,
			loadSceneMode = LoadSceneMode.Single
		};
		base.StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(this.m_loadLevelOperation));
		base.StartCoroutine(this.LoadCharacterAssets(gameManager.GameStatus, 0.3f));
	}

	public void UnloadAssets()
	{
		foreach (KeyValuePair<string, string> keyValuePair in this.m_loadLevelOperationBundleSceneNames)
		{
			string key = keyValuePair.Key;
			string value = keyValuePair.Value;
			AssetBundleManager.Get().UnloadScene(key, value);
		}
		this.m_loadLevelOperationBundleSceneNames.Clear();
		ClientGameManager.Get().CleanupMemory();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameObjectsDestroyed, null);
	}

	private IEnumerator LoadCharacterAssets(GameStatus gameStatusForAssets, float delaySeconds)
	{
		bool loading = false;
		GameManager gameManager;
		IEnumerator<LobbyPlayerInfo> enumerator;
		this.m_loadingCharacterAssets = true;
		gameManager = GameManager.Get();
		enumerator = gameManager.TeamInfo.TeamAPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo teamPlayerInfo = enumerator.Current;
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(teamPlayerInfo.CharacterInfo.CharacterType);
				if (!this.m_loadingCharacterResources.Contains(characterResourceLink))
				{
					this.m_loadingCharacterResources.Add(characterResourceLink);
					characterResourceLink.LoadAsync(teamPlayerInfo.CharacterInfo.CharacterSkin, new CharacterResourceLink.CharacterResourceDelegate(this.HandleCharacterResourceLoaded), gameStatusForAssets);
					yield return new WaitForSeconds(delaySeconds);
					loading = true;
				}
			}
		}
		finally
		{
			if (!loading && enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		IEnumerator<LobbyPlayerInfo> enumerator2 = gameManager.TeamInfo.TeamBPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				LobbyPlayerInfo teamPlayerInfo2 = enumerator2.Current;
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(teamPlayerInfo2.CharacterInfo.CharacterType);
				if (!this.m_loadingCharacterResources.Contains(characterResourceLink2))
				{
					this.m_loadingCharacterResources.Add(characterResourceLink2);
					characterResourceLink2.LoadAsync(teamPlayerInfo2.CharacterInfo.CharacterSkin, new CharacterResourceLink.CharacterResourceDelegate(this.HandleCharacterResourceLoaded), gameStatusForAssets);
					yield return new WaitForSeconds(delaySeconds);
					loading = true;
				}
			}
		}
		finally
		{
			if (!loading && enumerator2 != null)
			{
				enumerator2.Dispose();
			}
		}
		Log.Info(Log.Category.Loading, "Finished loading character assets", new object[0]);
		this.m_loadingCharacterAssets = false;
		yield break;
	}

	private void HandleCharacterResourceLoaded(LoadedCharacterSelection loadedCharacter)
	{
		this.m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
		this.m_loadedCharacterResourceCount++;
		Log.Info("Loaded CharacterResource {0} (total={1}, remaining={2})", new object[]
		{
			loadedCharacter.resourceLink.name,
			this.m_loadedCharacterResourceCount,
			this.m_loadingCharacterResources.Count
		});
	}

	private void ResetLoadAssetsState()
	{
		this.m_loading = false;
		this.m_loadingCharacterResources.Clear();
		this.m_loadingCharacterAssets = false;
	}

	public void LoadCharacterResourceLink(CharacterResourceLink characterResourceLink, CharacterVisualInfo linkVisualInfo = default(CharacterVisualInfo))
	{
		this.m_loadingCharacterResources.Add(characterResourceLink);
		characterResourceLink.LoadAsync(linkVisualInfo, new CharacterResourceLink.CharacterResourceDelegate(this.HandleCharacterResourceLoaded));
	}

	private void UpdateLoadProgress(bool force = false)
	{
		if (this.PlayerInfo != null)
		{
			if (this.TeamInfo != null)
			{
				AssetsLoadingState assetsLoadingState = this.m_assetsLoadingState;
				float levelLoadProgress;
				if (this.m_loadLevelOperationDone)
				{
					levelLoadProgress = 1f;
				}
				else if (this.m_loadLevelOperation != null)
				{
					levelLoadProgress = this.m_loadLevelOperation.progress;
				}
				else
				{
					levelLoadProgress = 0f;
				}
				assetsLoadingState.LevelLoadProgress = levelLoadProgress;
				this.m_assetsLoadingState.CharacterLoadProgress = Mathf.Clamp((float)this.m_loadedCharacterResourceCount / (float)this.TeamInfo.TotalPlayerCount, 0f, 1f);
				AssetsLoadingState assetsLoadingState2 = this.m_assetsLoadingState;
				float vfxPreloadProgress;
				if (ClientVFXLoader.Get() != null)
				{
					vfxPreloadProgress = ClientVFXLoader.Get().Progress;
				}
				else
				{
					vfxPreloadProgress = 0f;
				}
				assetsLoadingState2.VfxPreloadProgress = vfxPreloadProgress;
				AssetsLoadingState assetsLoadingState3 = this.m_assetsLoadingState;
				float spawningProgress;
				if (this.m_spawnableObjectCount > 0)
				{
					spawningProgress = Mathf.Clamp((float)ClientScene.objects.Count / (float)this.m_spawnableObjectCount, 0f, 1f);
				}
				else
				{
					spawningProgress = 0f;
				}
				assetsLoadingState3.SpawningProgress = spawningProgress;
				if (this.PlayerInfo.PlayerId == 0)
				{
					UILoadingScreenPanel.Get().SetLoadingProgress(this.PlayerInfo, this.m_assetsLoadingState.TotalProgress, true);
				}
				else
				{
					UILoadingScreenPanel.Get().SetLoadingProgress(this.PlayerInfo.PlayerId, this.m_assetsLoadingState.TotalProgress, true);
				}
				if (Time.unscaledTime <= this.m_lastLoadProgressUpdateSent + this.m_loadingProgressUpdateFrequency)
				{
					if (!force)
					{
						return;
					}
				}
				if (this.Client != null)
				{
					if (this.Client.isConnected)
					{
						if (this.IsRegisteredToGameServer)
						{
							GameManager.AssetsLoadingProgress assetsLoadingProgress = new GameManager.AssetsLoadingProgress();
							assetsLoadingProgress.AccountId = this.m_lobbyGameClientInterface.SessionInfo.AccountId;
							assetsLoadingProgress.PlayerId = this.PlayerInfo.PlayerId;
							assetsLoadingProgress.TotalLoadingProgress = (byte)(this.m_assetsLoadingState.TotalProgress * 100f);
							assetsLoadingProgress.LevelLoadingProgress = (byte)(this.m_assetsLoadingState.LevelLoadProgress * 100f);
							assetsLoadingProgress.CharacterLoadingProgress = (byte)(this.m_assetsLoadingState.CharacterLoadProgress * 100f);
							assetsLoadingProgress.VfxLoadingProgress = (byte)(this.m_assetsLoadingState.VfxPreloadProgress * 100f);
							assetsLoadingProgress.SpawningProgress = (byte)(this.m_assetsLoadingState.SpawningProgress * 100f);
							this.m_lastLoadProgressUpdateSent = Time.unscaledTime;
							this.Client.Send(0x3D, assetsLoadingProgress);
						}
					}
				}
			}
		}
	}

	private void CheckLoaded()
	{
		if (this.IsServer())
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		bool flag;
		if (this.m_loadLevelOperationDone)
		{
			flag = (GameFlowData.Get() == null || GameFlowData.Get().gameState < GameState.Deployment);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (!this.m_loading)
		{
			if (!flag2)
			{
				goto IL_85;
			}
		}
		this.UpdateLoadProgress(false);
		IL_85:
		if (this.m_loading)
		{
			if (this.m_loadLevelOperation != null)
			{
				if (this.m_loadLevelOperation.isDone)
				{
					this.m_loadLevelOperation = null;
					this.m_loadLevelOperationDone = true;
				}
			}
			if (this.m_loadLevelOperation == null)
			{
				if (this.m_loadingCharacterResources.Count == 0 && !this.m_loadingCharacterAssets)
				{
					if (!(VisualsLoader.Get() == null))
					{
						if (!VisualsLoader.Get().LevelLoaded())
						{
							goto IL_240;
						}
					}
					if (!(ClientVFXLoader.Get() == null))
					{
						if (!ClientVFXLoader.Get().IsPreloadQueueEmpty())
						{
							goto IL_240;
						}
					}
					bool flag3 = true;
					if (flag3)
					{
						if (ClientScene.localPlayers != null)
						{
							if (ClientScene.localPlayers.Count > 0)
							{
								if (this.Client != null)
								{
									if (this.Client.isConnected)
									{
										this.ResetLoadAssetsState();
										this.UpdateLoadProgress(true);
										GameManager.AssetsLoadedNotification assetsLoadedNotification = new GameManager.AssetsLoadedNotification();
										assetsLoadedNotification.AccountId = this.m_lobbyGameClientInterface.SessionInfo.AccountId;
										assetsLoadedNotification.PlayerId = gameManager.PlayerInfo.PlayerId;
										Log.Info(Log.Category.Loading, "Sending asset loaded notification", new object[0]);
										if (!this.Client.Send(0x35, assetsLoadedNotification))
										{
											Log.Error("Failed to send message AssetsLoadedNotification", new object[0]);
										}
									}
								}
							}
						}
					}
					return;
				}
			}
			IL_240:
			if (this.m_loadLevelOperation == null && this.m_loadingCharacterResources.Count == 0)
			{
				if (ClientVFXLoader.Get() != null && !ClientVFXLoader.Get().IsPreloadQueueEmpty())
				{
					if (!ClientVFXLoader.Get().IsPreloadInProgress())
					{
						Log.Info(Log.Category.Loading, "Starting VFX Preload", new object[0]);
						ClientVFXLoader.Get().PreloadQueuedPKFX();
					}
				}
			}
		}
	}

	public bool IsGroupReady()
	{
		bool result = true;
		if (this.GroupInfo.InAGroup)
		{
			for (int i = 0; i < this.GroupInfo.Members.Count; i++)
			{
				if (!this.GroupInfo.Members[i].IsReady)
				{
					return false;
				}
			}
		}
		return result;
	}

	public bool IsPlayerAccountDataAvailable()
	{
		return this.m_loadedPlayerAccountData != null;
	}

	public bool IsPlayerCharacterDataAvailable(CharacterType charType = CharacterType.None)
	{
		if (this.m_loadedPlayerCharacterData == null)
		{
			return false;
		}
		return charType == CharacterType.None || this.m_loadedPlayerCharacterData.ContainsKey(charType);
	}

	public int GetHighestOpenSeasonChapterIndexForActiveSeason()
	{
		int result = -1;
		if (this.IsPlayerAccountDataAvailable())
		{
			if (SeasonWideData.Get() != null)
			{
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(this.GetPlayerAccountData().QuestComponent.ActiveSeason);
				int i = 0;
				while (i < seasonTemplate.Chapters.Count)
				{
					SeasonChapter seasonChapter = seasonTemplate.Chapters[i];
					if (seasonChapter.Prerequisites.Conditions.Count == 0)
					{
						goto IL_D0;
					}
					if (seasonChapter.Prerequisites.Conditions.Count == 1)
					{
						if (seasonChapter.Prerequisites.Conditions[0].ConditionType == QuestConditionType.HasDateTimePassed)
						{
							goto IL_D0;
						}
					}
					IL_FC:
					i++;
					continue;
					IL_D0:
					if (QuestWideData.AreConditionsMet(seasonChapter.Prerequisites.Conditions, seasonChapter.Prerequisites.LogicStatement, false))
					{
						result = i;
						goto IL_FC;
					}
					goto IL_FC;
				}
			}
		}
		return result;
	}

	public PersistedAccountData GetPlayerAccountData()
	{
		if (this.m_loadedPlayerAccountData == null)
		{
			Log.Error("Player account data not loaded yet", new object[0]);
			return null;
		}
		return this.m_loadedPlayerAccountData;
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		this.GameTypeAvailabilies = new Dictionary<GameType, GameTypeAvailability>(notification.GameTypeAvailabilies, default(GameTypeComparer));
		this.FreeRotationAdditions = new Dictionary<CharacterType, RequirementCollection>(notification.FreeRotationAdditions, default(CharacterTypeComparer));
		this.AllowBadges = notification.AllowBadges;
		this.NewPlayerPvPQueueDuration = notification.NewPlayerPvPQueueDuration;
		foreach (GameTypeAvailability gameTypeAvailability in this.GameTypeAvailabilies.Values)
		{
			foreach (GameSubType gameSubType in gameTypeAvailability.SubTypes)
			{
				List<GameMapConfig> gameMapConfigs = gameSubType.GameMapConfigs;
				
				if (!gameMapConfigs.Exists(((GameMapConfig p) => p.IsActive)))
				{
					if (gameSubType.Requirements == null)
					{
						gameSubType.Requirements = RequirementCollection.Create();
					}
					RequirementCollection requirements = gameSubType.Requirements;
					
					if (!requirements.Exists(((QueueRequirement p) => p is QueueRequirement_Never)))
					{
						gameSubType.Requirements.Add(QueueRequirement_Never.Create(QueueRequirement.RequirementType.AdminDisabled, null));
					}
				}
			}
		}
		if (UICharacterScreen.Get() != null)
		{
			if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
			{
				if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
				{
					goto IL_1EC;
				}
			}
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		IL_1EC:
		this.m_tierInstanceNames = notification.TierInstanceNames;
		this.OnServerQueueConfigurationUpdateNotificationHolder(notification);
	}

	private void HandleRankedOverviewChangeNotification(RankedOverviewChangeNotification notification)
	{
		if (notification.GameType != GameType.Ranked)
		{
			throw new Exception(string.Format("We do not yet handle RankedOverviewChangeNotification for game type {0}", notification.GameType));
		}
		UIRankedModeSelectScreen uirankedModeSelectScreen = UIRankedModeSelectScreen.Get();
		if (uirankedModeSelectScreen != null)
		{
			uirankedModeSelectScreen.ProcessTierInfoPerGroupSize(notification.TierInfoPerGroupSize);
		}
	}

	private void HandleAccountDataUpdateNotification(PlayerAccountDataUpdateNotification notification)
	{
		this.HandleAccountDataUpdated(notification.AccountData);
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification notification)
	{
		if (this.IsReady)
		{
			this.OnQuestCompleteNotificationHolder(notification);
		}
		else
		{
			this.LoginQuestCompleteNotifications.Add(notification);
		}
	}

	private void HandleFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		this.ActiveFactionCompetition = notification.ActiveIndex;
		this.FactionScores = notification.Scores;
		this.OnFactionCompetitionNotificationHolder(notification);
	}

	private void HandleTrustBoostUsedNotification(TrustBoostUsedNotification notification)
	{
		this.OnTrustBoostUsedNotificationHolder(notification);
	}

	private void HandleFactionLoginRewardNotification(FactionLoginRewardNotification notification)
	{
		if (this.LoginRewardNotification != null)
		{
			Log.Error("received a second login notification! - should not", new object[0]);
		}
		this.LoginRewardNotification = notification;
		this.OnFactionLoginRewardNotificationHolder(notification);
	}

	private void HandlePlayerFactionContributionChange(PlayerFactionContributionChangeNotification notification)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (!this.m_lobbyGameClientInterface.IsConnected)
			{
			}
			else
			{
				if (!this.IsPlayerAccountDataAvailable())
				{
					Log.Error("Player Account Data not available for Faction Contribution Change", new object[0]);
					return;
				}
				this.GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(notification.CompetitionId, notification.FactionId).TotalXP = notification.TotalXP;
				this.OnPlayerFactionContributionChangeNotificationHolder(notification);
				return;
			}
		}
		Log.Error("Not connected to lobby server for Faction Contribution Change", new object[0]);
	}

	private void HandleFacebookAccessTokenNotification(FacebookAccessTokenNotification notification)
	{
		FacebookClientInterface.Get().UploadScreenshot(notification.AccessToken);
	}

	private void HandleMatchResultsNotification(MatchResultsNotification notification)
	{
		this.OnMatchResultsNotificationHolder(notification);
	}

	public void QueryPlayerMatchData(Action<PlayerMatchDataResponse> onResponseCallback)
	{
		if (onResponseCallback == null)
		{
			throw new ArgumentNullException("onResponseCallback");
		}
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.GetPlayerMatchData(onResponseCallback);
		}
		else
		{
			onResponseCallback(new PlayerMatchDataResponse
			{
				Success = false,
				ErrorMessage = "Not connected to lobby server."
			});
		}
	}

	public int GetPlayerCharacterLevel(CharacterType character)
	{
		if (this.m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet", new object[0]);
			return -1;
		}
		PersistedCharacterData persistedCharacterData;
		if (this.m_loadedPlayerCharacterData.TryGetValue(character, out persistedCharacterData))
		{
			return persistedCharacterData.ExperienceComponent.Level;
		}
		return -1;
	}

	public PersistedCharacterData GetPlayerCharacterData(CharacterType character)
	{
		if (this.m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet", new object[0]);
			return null;
		}
		PersistedCharacterData result;
		if (this.m_loadedPlayerCharacterData.TryGetValue(character, out result))
		{
			return result;
		}
		return null;
	}

	public Dictionary<CharacterType, PersistedCharacterData> GetAllPlayerCharacterData()
	{
		if (this.m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet", new object[0]);
			return null;
		}
		return this.m_loadedPlayerCharacterData;
	}

	public PersistedCharacterData GetCharacterDataOnInitialLoad(CharacterType charType)
	{
		PersistedCharacterData result;
		if (this.m_characterDataOnInitialLoad.TryGetValue(charType, out result))
		{
			return result;
		}
		return null;
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.PurchasingMod = true;
				this.ModAttemptingToPurchase = abilityModID;
				this.m_lobbyGameClientInterface.PurchaseMod(character, abilityId, abilityModID, new Action<PurchaseModResponse>(this.HandlePurchaseModResponse));
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	private void HandlePurchaseModResponse(PurchaseModResponse response)
	{
		if (response.Success)
		{
			PersistedCharacterData persistedCharacterData;
			if (this.m_loadedPlayerCharacterData.TryGetValue(response.Character, out persistedCharacterData))
			{
				persistedCharacterData.CharacterComponent.Mods.Add(response.UnlockData);
			}
			this.OnModUnlockedHolder(response.Character, response.UnlockData);
		}
		else
		{
			Log.Error(string.Format("Failed to unlock Mod {0} for character {1}: {2}", response.UnlockData.ToString(), response.Character, response.ErrorMessage), new object[0]);
		}
		this.ModAttemptingToPurchase = -1;
		this.PurchasingMod = false;
	}

	public void PurchaseLoadoutSlot(CharacterType characterType, Action<PurchaseLoadoutSlotResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PurchaseLoadoutSlot(characterType, delegate(PurchaseLoadoutSlotResponse response)
			{
				if (onResponseCallback != null)
				{
					onResponseCallback(response);
				}
			});
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void HandleSelectLoadoutUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			this.RecordFailureInCharacterSelectUpdateResponse(response, "HandleSelectLoadoutUpdateResponse");
			return;
		}
		UICharacterSelectCharacterSettingsPanel.Get().NotifyLoadoutUpdate(response);
		if (UIRankedCharacterSelectSettingsPanel.Get() != null)
		{
			UIRankedCharacterSelectSettingsPanel.Get().NotifyLoadoutUpdate(response);
		}
	}

	public void RequestToSelectLoadout(CharacterLoadout loadout, int loadoutIndex)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
				lobbyPlayerInfoUpdate.CharacterMods = new CharacterModInfo?(loadout.ModSet);
				lobbyPlayerInfoUpdate.CharacterAbilityVfxSwaps = new CharacterAbilityVfxSwapInfo?(loadout.VFXSet);
				lobbyPlayerInfoUpdate.LastSelectedLoadout = new int?(loadoutIndex);
				lobbyPlayerInfoUpdate.RankedLoadoutMods = (loadout.Strictness == ModStrictness.Ranked);
				this.m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, new Action<PlayerInfoUpdateResponse>(this.HandleModSelectUpdateResponse));
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseModToken(int numToPurchase)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseModToken(numToPurchase, new Action<PurchaseModTokenResponse>(this.HandlePurchaseModTokenResponse));
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void HandlePurchaseModTokenResponse(PurchaseModTokenResponse response)
	{
		if (!response.Success)
		{
			Log.Error("Failed to purchase Mod Token", new object[0]);
		}
	}

	public void HandleGGPackUseResponse(UseGGPackResponse response)
	{
		if (response.Success)
		{
			HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NotifyGGPackUsed(response);
		}
		else
		{
			string text;
			if (response.LocalizedFailure != null)
			{
				text = response.LocalizedFailure.ToString();
			}
			else if (!response.ErrorMessage.IsNullOrEmpty())
			{
				text = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
			}
			else
			{
				text = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
			}
			string text2 = text;
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = text2,
				MessageType = ConsoleMessageType.SystemMessage
			}, null);
			Log.Error("Did not use GG pack: {0}", new object[]
			{
				text2
			});
		}
		HUD_UI.Get().m_mainScreenPanel.m_characterProfile.NotifyReceivedGGPackResponse();
		UIGameOverScreen.Get().NotifySelfGGPackUsed();
	}

	public void RequestToUseGGPack()
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.RequestToUseGGPack(new Action<UseGGPackResponse>(this.HandleGGPackUseResponse));
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void CheckAndSendClientPreparedForGameStartNotification()
	{
		if (this.PlayerObjectStartedOnClient && this.InGameUIActivated)
		{
			if (this.VisualSceneLoaded)
			{
				if (this.DesignSceneStarted)
				{
					this.SendClientPreparedForGameStartNotification();
					this.PlayerObjectStartedOnClient = false;
					this.InGameUIActivated = false;
					this.VisualSceneLoaded = false;
					this.DesignSceneStarted = false;
				}
			}
		}
	}

	public void SendClientPreparedForGameStartNotification()
	{
		Log.Info("SendClientPreparedForGameStartNotification", new object[0]);
		if (NetworkClient.active && !NetworkServer.active)
		{
			if (GameplayData.Get() == null)
			{
				Log.Error("SendClientPreparedForGameStartNotification, but GameplayData is null", new object[0]);
			}
			if (GameFlowData.Get() == null)
			{
				Log.Error("SendClientPreparedForGameStartNotification, but GameFlowData is null", new object[0]);
			}
			GameManager.PlayerObjectStartedOnClientNotification playerObjectStartedOnClientNotification = new GameManager.PlayerObjectStartedOnClientNotification();
			playerObjectStartedOnClientNotification.AccountId = this.m_lobbyGameClientInterface.SessionInfo.AccountId;
			playerObjectStartedOnClientNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
			this.Client.Send(0x37, playerObjectStartedOnClientNotification);
		}
	}

	public void UpdateRemoteCharacter(CharacterType character, int remoteSlotIndex, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		this.UpdateRemoteCharacter(new CharacterType[]
		{
			character
		}, new int[]
		{
			remoteSlotIndex
		}, onResponse);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		if (this.m_lobbyGameClientInterface == null || this.m_lobbyGameClientInterface.IsConnected)
		{
			Log.Error("Not connected to lobby server.", new object[0]);
			return;
		}
		if (remoteSlotIndexes.IsNullOrEmpty<int>() || characters.IsNullOrEmpty<CharacterType>() || remoteSlotIndexes.Length != characters.Length)
		{
			return;
		}

		if (this.m_loadedPlayerAccountData != null)
		{
			bool flag = false;
			List<CharacterType> lastRemoteCharacters = this.m_loadedPlayerAccountData.AccountComponent.LastRemoteCharacters;
						
			for (int i = 0; i < characters.Length; i++)
			{
				if (!(lastRemoteCharacters.Count > remoteSlotIndexes[i] && lastRemoteCharacters[remoteSlotIndexes[i]] == characters[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		this.m_lobbyGameClientInterface.UpdateRemoteCharacter(characters, remoteSlotIndexes, onResponse);	
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				if (this.m_loadedPlayerAccountData != null)
				{
					if (this.m_loadedPlayerAccountData.AccountComponent.SelectedTitleID == newTitleID)
					{
						return;
					}
				}
				this.m_lobbyGameClientInterface.RequestTitleSelect(newTitleID, delegate(SelectTitleResponse response)
				{
					if (this.m_loadedPlayerAccountData != null)
					{
						this.m_loadedPlayerAccountData.AccountComponent.SelectedTitleID = response.CurrentTitleID;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (gameBalanceVars != null)
					{
						this.OnPlayerTitleChangeHolder(gameBalanceVars.GetTitle(response.CurrentTitleID, string.Empty, -1));
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RequestBannerSelect(int newBannerID, Action<SelectBannerResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				if (this.m_loadedPlayerAccountData != null)
				{
					if (this.m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID != newBannerID)
					{
						if (this.m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID != newBannerID)
						{
							goto IL_8E;
						}
					}
					return;
				}
				IL_8E:
				this.m_lobbyGameClientInterface.RequestBannerSelect(newBannerID, delegate(SelectBannerResponse response)
				{
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (this.m_loadedPlayerAccountData != null)
					{
						if (gameBalanceVars != null)
						{
							this.m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID = response.ForegroundBannerID;
							this.m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID = response.BackgroundBannerID;
							this.OnPlayerBannerChangeHolder(gameBalanceVars.GetBanner(response.ForegroundBannerID), gameBalanceVars.GetBanner(response.BackgroundBannerID));
						}
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RequestRibbonSelect(int newRibbonID, Action<SelectRibbonResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				if (this.m_loadedPlayerAccountData != null && this.m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID == newRibbonID)
				{
					return;
				}
				this.m_lobbyGameClientInterface.RequestRibbonSelect(newRibbonID, delegate(SelectRibbonResponse response)
				{
					if (this.m_loadedPlayerAccountData != null)
					{
						this.m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID = response.CurrentRibbonID;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (gameBalanceVars != null)
					{
						this.OnPlayerRibbonChangeHolder(gameBalanceVars.GetRibbon(response.CurrentRibbonID));
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RequestLoadingScreenBackgroundToggle(int loadingScreenId, bool newState, Action<LoadingScreenToggleResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				if (this.m_loadedPlayerAccountData != null)
				{
					if (!this.m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundUnlocked(loadingScreenId))
					{
						return;
					}
					bool flag = this.m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundActive(loadingScreenId);
					if (flag == newState)
					{
						return;
					}
				}
				this.m_lobbyGameClientInterface.RequestLoadingScreenBackgroundToggle(loadingScreenId, newState, delegate(LoadingScreenToggleResponse response)
				{
					if (!response.Success)
					{
						return;
					}
					if (this.m_loadedPlayerAccountData != null)
					{
						this.m_loadedPlayerAccountData.AccountComponent.ToggleLoadingScreenBackgroundActive(response.LoadingScreenId, response.CurrentState);
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					this.OnLoadingScreenBackgroundToggledHolder(response.LoadingScreenId, response.CurrentState);
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RequestRankedLeaderboardOverview(GameType gameType, Action<RankedLeaderboardOverviewResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedLeaderboardOverviewRequest(gameType, onResponse);
		}
		else
		{
			onResponse(new RankedLeaderboardOverviewResponse
			{
				ErrorMessage = "Not connected to a lobby server",
				Success = false
			});
		}
	}

	public void RequestRankedLeaderboardSpecific(GameType gameType, int groupSize, RankedLeaderboardSpecificRequest.RequestSpecificationType specification, Action<RankedLeaderboardSpecificResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			this.m_lobbyGameClientInterface.SendRankedLeaderboardOverviewRequest(gameType, groupSize, specification, onResponse);
		}
		else
		{
			onResponse(new RankedLeaderboardSpecificResponse
			{
				ErrorMessage = "Not connected to a lobby server",
				Success = false
			});
		}
	}

	public void RequestUpdateUIState(AccountComponent.UIStateIdentifier uiState, int stateValue, Action<UpdateUIStateResponse> onResponse)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.RequestUpdateUIState(uiState, stateValue, delegate(UpdateUIStateResponse response)
				{
					if (this.m_loadedPlayerAccountData != null)
					{
						this.m_loadedPlayerAccountData.AccountComponent.UIStates[uiState] = stateValue;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void SetPushToTalkKey(int keyType, int keyCode, string keyName)
	{
		if (this.m_loadedPlayerAccountData != null)
		{
			this.m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyType = keyType;
			this.m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyCode = keyCode;
			this.m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyName = keyName;
		}
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SetPushToTalkKey(keyType, keyCode, keyName);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void symbol_001D(string symbol_001D, string symbol_000E)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.DEBUG_AdminSlashCommandExecuted(symbol_001D, symbol_000E);
			}
		}
	}

	public void symbol_000E()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				LobbyGameClientInterface lobbyGameClientInterface = this.m_lobbyGameClientInterface;
				
				lobbyGameClientInterface.DEBUG_ForceMatchmaking(delegate(DEBUG_ForceMatchmakingResponse response)
					{
						string text = (!response.Success) ? string.Format("Failed to force queue: {0}", response.ErrorMessage) : string.Format("Forced queue {0}", response.GameType);
						TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
						if (response.Success)
						{
							Log.Info(text, new object[0]);
						}
						else
						{
							Log.Error(text, new object[0]);
						}
					});
			}
		}
	}

	public void symbol_0012()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				LobbyGameClientInterface lobbyGameClientInterface = this.m_lobbyGameClientInterface;
				
				lobbyGameClientInterface.DEBUG_TakeSnapshot(delegate(DEBUG_TakeSnapshotResponse response)
					{
						string text;
						if (response.Success)
						{
							text = string.Format("Snapshot taken {0}", response.SnapshotId);
						}
						else
						{
							text = string.Format("Failed to take snapshot: {0}", response.ErrorMessage);
						}
						string text2 = text;
						TextConsole.Get().Write(text2, ConsoleMessageType.SystemMessage);
						if (response.Success)
						{
							Log.Info(text2, new object[0]);
						}
						else
						{
							Log.Error(text2, new object[0]);
						}
					});
			}
		}
	}

	public void RequestPreviousGameInfo(Action<PreviousGameInfoResponse> onResponseCallback)
	{
		if (this.IsReady)
		{
			this.m_lobbyGameClientInterface.SendPreviousGameInfoRequest(onResponseCallback);
		}
	}

	public void RejoinGame(bool accept, Action<RejoinGameResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SendRejoinGameRequest(this.GameInfo, accept, onResponseCallback);
				return;
			}
		}
		string text = string.Format("{0} {1}", StringUtil.TR("FailedToRejoinGame", "Frontend"), StringUtil.TR("NotConnectedToLobbyServer", "Global"));
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
	}

	public DiscordJoinType GetDiscordJoinType()
	{
		DiscordJoinType result = DiscordJoinType._001D;
		if (DiscordClientInterface.IsSdkEnabled)
		{
			SettingsState.VoiceChatMode voiceChatMode = SettingsState.VoiceChatMode.None;
			if (DiscordClientInterface.CanJoinTeamChat)
			{
				voiceChatMode = Options_UI.Get().GetGameModeVoiceChat();
				if (voiceChatMode != SettingsState.VoiceChatMode.Team && !DiscordClientInterface.CanJoinGroupChat)
				{
					voiceChatMode = SettingsState.VoiceChatMode.Team;
				}
			}
			if (voiceChatMode == SettingsState.VoiceChatMode.None && DiscordClientInterface.CanJoinGroupChat)
			{
				voiceChatMode = SettingsState.VoiceChatMode.Group;
			}
			if (voiceChatMode == SettingsState.VoiceChatMode.Team)
			{
				result = DiscordJoinType._0012;
			}
			else if (voiceChatMode == SettingsState.VoiceChatMode.Group)
			{
				result = DiscordJoinType._000E;
			}
		}
		else
		{
			SettingsState.VoiceChatMode voiceChatMode2 = SettingsState.VoiceChatMode.None;
			if (DiscordClientInterface.CanJoinTeamChat)
			{
				voiceChatMode2 = Options_UI.Get().GetGameModeVoiceChat();
			}
			else if (DiscordClientInterface.CanJoinGroupChat)
			{
				voiceChatMode2 = SettingsState.VoiceChatMode.Group;
			}
			if (voiceChatMode2 == SettingsState.VoiceChatMode.Group)
			{
				result = DiscordJoinType._000E;
			}
			else if (voiceChatMode2 == SettingsState.VoiceChatMode.Team)
			{
				result = DiscordJoinType._0012;
			}
		}
		return result;
	}

	public void JoinDiscord()
	{
		DiscordJoinType discordJoinType = this.GetDiscordJoinType();
		if (discordJoinType == DiscordJoinType._001D)
		{
			return;
		}
		if (this.m_discordJoinType != discordJoinType)
		{
			Log.Info("Discord | switch joinType {0} => {1} (teamChat={2}, groupChat={3})", new object[]
			{
				this.m_discordJoinType,
				discordJoinType,
				DiscordClientInterface.CanJoinTeamChat,
				DiscordClientInterface.CanJoinGroupChat
			});
			this.LeaveDiscord();
		}
		if (!this.m_discordConnecting)
		{
			if (!this.m_discordConnected)
			{
				if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
				{
					this.m_discordConnecting = true;
					this.m_discordJoinType = discordJoinType;
					LobbyGameClientInterface lobbyGameClientInterface = this.m_lobbyGameClientInterface;
					
					lobbyGameClientInterface.SendDiscordGetRpcTokenRequest(delegate(DiscordGetRpcTokenResponse response)
						{
							if (response.Success)
							{
								DiscordAuthInfo authInfo = new DiscordAuthInfo
								{
									ClientId = response.DiscordClientId,
									RpcToken = response.DiscordRpcToken,
									RpcOrigin = response.DiscordRpcOrigin
								};
								DiscordClientInterface.Get().Connect(authInfo, 0);
							}
						});
				}
				return;
			}
		}
	}

	private void JoinDiscordChannel(DiscordUserInfo userInfo)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SendDiscordJoinServerRequest(userInfo.UserId, userInfo.AccessToken, this.m_discordJoinType, delegate(DiscordJoinServerResponse response)
				{
					if (response.Success)
					{
						if (this.m_discordJoinType != DiscordJoinType._001D)
						{
							DiscordChannelInfo channelInfo = new DiscordChannelInfo
							{
								ServerId = response.DiscordServerId,
								VoiceChannelId = response.DiscordVoiceChannelId
							};
							DiscordClientInterface.Get().SelectVoiceChannel(channelInfo);
						}
					}
					else
					{
						Log.Error("Failed to join discord server {0}", new object[]
						{
							response.ErrorMessage
						});
						TextConsole.Get().Write(string.Format(StringUtil.TR("FailedToJoinDiscordChat", "Global"), response.ErrorMessage), ConsoleMessageType.SystemMessage);
						DiscordClientInterface.Get().Disconnect();
					}
				});
			}
		}
	}

	public void CheckDiscord()
	{
		DiscordClientInterface.Get().Connect(null, 0);
	}

	private void HandleDiscordConnected(bool needAuthentication)
	{
		this.m_discordConnecting = false;
		if (needAuthentication)
		{
			string text = string.Format(StringUtil.TR("ClickToJoinDiscordTeamChat", "Global"), new object[0]);
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
			}
			DiscordClientInterface.Get().Disconnect();
		}
	}

	private void HandleDiscordDisconnected()
	{
		this.m_discordConnecting = false;
	}

	private void HandleDiscordAuthorized(string rpcCode)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			LobbyGameClientInterface lobbyGameClientInterface = this.m_lobbyGameClientInterface;
			
			lobbyGameClientInterface.SendDiscordGetAccessTokenRequest(rpcCode, delegate(DiscordGetAccessTokenResponse response)
				{
					if (response.Success)
					{
						DiscordUserInfo userInfo = new DiscordUserInfo
						{
							AccessToken = response.DiscordAccessToken
						};
						DiscordClientInterface.Get().Authenticate(userInfo);
					}
				});
		}
	}

	private void HandleDiscordAuthenticated(DiscordUserInfo userInfo)
	{
		this.JoinDiscordChannel(userInfo);
	}

	private void HandleDiscordJoined()
	{
		TextConsole.Get().Write("Joined Discord team chat", ConsoleMessageType.SystemMessage);
	}

	private void HandleDiscordLeft()
	{
		TextConsole.Get().Write("Left Discord team chat", ConsoleMessageType.SystemMessage);
	}

	public void LeaveDiscord()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SendDiscordLeaveServerRequest(this.m_discordJoinType, delegate(DiscordLeaveServerResponse response)
				{
					Log.Info("Remove user from Discord server {0} (result {1})", new object[]
					{
						this.m_discordJoinType,
						response.Success
					});
				});
			}
		}
		DiscordClientInterface.Get().Disconnect();
		this.m_discordConnecting = false;
		this.m_discordJoinType = DiscordJoinType._001D;
	}

	public void ConfigureDiscord(bool autoJoin)
	{
		Options_UI.Get().SetEnableAutoJoinDiscord(autoJoin);
		string key = "AutoJoinDiscord";
		int value;
		if (autoJoin)
		{
			value = 1;
		}
		else
		{
			value = 0;
		}
		PlayerPrefs.SetInt(key, value);
		string format = StringUtil.TR("ConfiguredDiscordAutojoin", "Global");
		object arg;
		if (autoJoin)
		{
			arg = StringUtil.TR("Enabled", "Global");
		}
		else
		{
			arg = StringUtil.TR("Disabled", "Global");
		}
		string text = string.Format(format, arg);
		if (DiscordClientInterface.s_debugOutput)
		{
			text += ((!DiscordClientInterface.IsSdkEnabled) ? " (DesktopApp)" : " (SDK)");
		}
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
	}

	public void FacebookShareScreenshot(string message = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.SendFacebookGetUserTokenRequest(delegate(FacebookGetUserTokenResponse response)
			{
				if (response.Success)
				{
					string language = HydrogenConfig.Get().Language;
					if (message.IsNullOrEmpty())
					{
						message = "Atlas Reactor at " + DateTime.Now;
					}
					FacebookClientInterface.Get().Connect(response.OAuthInfo, language, message);
				}
			});
		}
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		List<QuestProgress> list = new List<QuestProgress>();
		if (this.m_loadedPlayerAccountData != null)
		{
			Dictionary<int, QuestProgress> progress = this.m_loadedPlayerAccountData.QuestComponent.Progress;
			List<QuestProgress> list2 = accountData.QuestComponent.Progress.Values.ToList<QuestProgress>();
			for (int i = 0; i < list2.Count; i++)
			{
				bool flag = false;
				QuestProgress questProgress = list2[i];
				if (progress.ContainsKey(questProgress.Id))
				{
					QuestProgress questProgress2 = progress[questProgress.Id];
					if (questProgress2.ObjectiveProgress.Count != questProgress.ObjectiveProgress.Count)
					{
						flag = true;
					}
					if (!flag)
					{
						using (Dictionary<int, int>.Enumerator enumerator = questProgress2.ObjectiveProgress.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<int, int> keyValuePair = enumerator.Current;
								int num;
								if (!questProgress.ObjectiveProgress.TryGetValue(keyValuePair.Key, out num))
								{
									flag = true;
								}
								else
								{
									if (keyValuePair.Value == num)
									{
										continue;
									}
									flag = true;
								}
								goto IL_149;
							}
						}
					}
					IL_149:;
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					list.Add(questProgress);
				}
			}
		}
		this.m_loadedPlayerAccountData = accountData;
		this.OnAccountDataUpdatedHolder(accountData);
		this.PlayerWallet = new CurrencyWallet(accountData.BankComponent.CurrentAmounts.Data);
		IEnumerator<CurrencyData> enumerator2 = this.PlayerWallet.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				CurrencyData obj = enumerator2.Current;
				this.OnBankBalanceChangeHolder(obj);
			}
		}
		finally
		{
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
		}
		if (list.Count > 0)
		{
			this.OnQuestProgressChangedHolder(list.ToArray());
		}
	}

	private void HandlePlayerCharacterDataUpdated(List<PersistedCharacterData> characterDataList)
	{
		this.m_loadedPlayerCharacterData = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		this.m_characterDataOnInitialLoad = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		foreach (PersistedCharacterData persistedCharacterData in characterDataList)
		{
			this.m_loadedPlayerCharacterData.Add(persistedCharacterData.CharacterType, persistedCharacterData);
			PersistedCharacterData persistedCharacterData2 = persistedCharacterData.Clone() as PersistedCharacterData;
			persistedCharacterData2.CharacterComponent = new CharacterComponent();
			using (List<PlayerSkinData>.Enumerator enumerator2 = persistedCharacterData.CharacterComponent.Skins.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PlayerSkinData playerSkinData = enumerator2.Current;
					persistedCharacterData2.CharacterComponent.Skins.Add(playerSkinData.GetDeepCopy());
				}
			}
			this.m_characterDataOnInitialLoad.Add(persistedCharacterData.CharacterType, persistedCharacterData2);
		}
	}

	private void HandleForcedCharacterChangeFromServerNotification(ForcedCharacterChangeFromServerNotification notification)
	{
		if (this.GroupInfo != null)
		{
			this.GroupInfo.SetCharacterInfo(notification.ChararacterInfo, false);
			Log.Info("Server forcing us to switch to {0}", new object[]
			{
				notification.ChararacterInfo.CharacterType.GetDisplayName()
			});
			return;
		}
		throw new Exception(string.Format("The server believes we should be freelancer {0}, but we're not doing anything about that. This is bad?!", notification.ChararacterInfo.CharacterType.GetDisplayName()));
	}

	private void HandleCharacterDataUpdateNotification(PlayerCharacterDataUpdateNotification notification)
	{
		this.m_loadedPlayerCharacterData[notification.CharacterData.CharacterType] = notification.CharacterData;
		this.OnCharacterDataUpdatedHolder(notification.CharacterData);
	}

	private void HandleInventoryComponentUpdateNotification(InventoryComponentUpdateNotification notification)
	{
		this.m_loadedPlayerAccountData.InventoryComponent = notification.InventoryComponent;
		this.OnInventoryComponentUpdatedHolder(notification.InventoryComponent);
	}

	private void HandleBankBalanceChangeNotification(BankBalanceChangeNotification notification)
	{
		if (notification.Success)
		{
			if (this.PlayerWallet != null)
			{
				this.PlayerWallet.SetValue(notification.NewBalance);
				this.OnBankBalanceChangeHolder(notification.NewBalance);
			}
		}
	}

	public void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		if (this.m_loadedPlayerAccountData != null)
		{
			this.m_loadedPlayerAccountData.QuestComponent.ActiveSeason = notification.SeasonStartedIndex;
		}
		this.OnSeasonCompleteNotificationHolder(notification);
		if (this.m_loadedPlayerAccountData != null)
		{
			this.OnAccountDataUpdatedHolder(this.m_loadedPlayerAccountData);
		}
	}

	public void SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType dialogType)
	{
		this.m_lobbyGameClientInterface.SendSeasonStatusConfirmed(dialogType);
	}

	public void HandleChapterStatusNotification(ChapterStatusNotification notification)
	{
		if (!notification.Success)
		{
			return;
		}
		if (notification.IsCompleted)
		{
			this.OnChapterCompleteNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
		}
		else if (notification.IsUnlocked)
		{
			this.OnChapterUnlockNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
		}
	}

	public void SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SendPlayerCharacterFeedback(feedbackData);
			}
		}
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.InviteToGroup(friendHandle, onResponseCallback);
				return;
			}
		}
		GroupInviteResponse groupInviteResponse = new GroupInviteResponse();
		groupInviteResponse.Success = false;
		groupInviteResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RequestToJoinGroup(string friendHandle, Action<GroupJoinResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.RequestToJoinGroup(friendHandle, onResponseCallback);
		}
		else
		{
			GroupJoinResponse groupJoinResponse = new GroupJoinResponse();
			groupJoinResponse.Success = false;
			groupJoinResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PromoteWithinGroup(name, onResponseCallback);
				return;
			}
		}
		GroupPromoteResponse groupPromoteResponse = new GroupPromoteResponse();
		groupPromoteResponse.Success = false;
		groupPromoteResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void ChatToGroup(string text, Action<GroupChatResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.ChatToGroup(text, onResponseCallback);
		}
		else
		{
			GroupChatResponse groupChatResponse = new GroupChatResponse();
			groupChatResponse.Success = false;
			groupChatResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.LeaveGroup(onResponseCallback);
				return;
			}
		}
		GroupLeaveResponse groupLeaveResponse = new GroupLeaveResponse();
		groupLeaveResponse.Success = false;
		groupLeaveResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void KickFromGroup(string memberName, Action<GroupKickResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.KickFromGroup(memberName, onResponseCallback);
				return;
			}
		}
		GroupKickResponse groupKickResponse = new GroupKickResponse();
		groupKickResponse.Success = false;
		groupKickResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void UpdateFriend(string friendHandle, long friendAccountId, FriendOperation operation, string strData, Action<FriendUpdateResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.UpdateFriend(friendHandle, friendAccountId, operation, strData, onResponseCallback);
		}
		else if (onResponseCallback != null)
		{
			FriendUpdateResponse friendUpdateResponse = new FriendUpdateResponse();
			friendUpdateResponse.Success = false;
			friendUpdateResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.", new object[0]);
			onResponseCallback(friendUpdateResponse);
		}
	}

	public void UpdatePlayerStatus(string statusString)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			
			Action<PlayerUpdateStatusResponse> onResponseCallback = delegate(PlayerUpdateStatusResponse resonse)
				{
					if (!resonse.Success)
					{
						Log.Warning(resonse.ErrorMessage, new object[0]);
					}
				};
			this.m_lobbyGameClientInterface.UpdatePlayerStatus(statusString, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseLootMatrixPack(lootMatrixPackIndex, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseGame(int gamePackIndex, long paymentMethodId, Action<PurchaseGameResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PurchaseGame(gamePackIndex, paymentMethodId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseGGPack(int ggPackIndex, long paymentMethodId, Action<PurchaseGGPackResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseGGPack(ggPackIndex, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseCharacter(CurrencyType currencyType, CharacterType characterType, Action<PurchaseCharacterResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseCharacter(currencyType, characterType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseCharacterForCash(CharacterType characterType, long paymentMethodId, Action<PurchaseCharacterForCashResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseCharacterForCash(characterType, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseSkin(CurrencyType currencyType, CharacterType characterType, int skinId, Action<PurchaseSkinResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseSkin(currencyType, characterType, skinId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseTexture(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, Action<PurchaseTextureResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseTexture(currencyType, characterType, skinId, textureId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseTint(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, int tintId, Action<PurchaseTintResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PurchaseTint(currencyType, characterType, skinId, textureId, tintId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseTintForCash(CharacterType characterType, int skinId, int textureId, int tintId, long paymentMethodId, Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseTintForCash(characterType, skinId, textureId, tintId, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseStoreItemForCash(int inventoryTemplateId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PurchaseStoreItemForCash(inventoryTemplateId, paymentMethodId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntIndex, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseTaunt(currencyType, characterType, tauntIndex, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PlayerPanelUpdated(int _selectedTitleID, int _selectedForegroundBannerID, int _selectedBackgroundBannerID, int _selectedRibbonID)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PlayerPanelUpdated(_selectedTitleID, _selectedForegroundBannerID, _selectedBackgroundBannerID, _selectedTitleID);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseInventoryItem(int inventoryItemID, CurrencyType currencyType, Action<PurchaseInventoryItemResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseInventoryItem(inventoryItemID, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseTitle(int titleId, CurrencyType currencyType, Action<PurchaseTitleResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.PurchaseTitle(currencyType, titleId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void PurchaseBanner(int bannerId, CurrencyType currencyType, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseBannerBackground(currencyType, bannerId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseEmblem(int emblemId, CurrencyType currencyType, Action<PurchaseBannerForegroundResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseBannerForeground(currencyType, emblemId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseEmoticon(int emoticonId, CurrencyType currencyType, Action<PurchaseChatEmojiResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseChatEmoji(currencyType, emoticonId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseAbilityVfx(CharacterType type, int abilityId, int vfxId, CurrencyType currencyType, Action<PurchaseAbilityVfxResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseAbilityVfx(type, abilityId, vfxId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseOvercon(int overconId, CurrencyType currencyType, Action<PurchaseOverconResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseOvercon(overconId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType, Action<PurchaseLoadingScreenBackgroundResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.PurchaseLoadingScreenBackground(loadingScreenBackgroundId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void PreventNextAccountStatusCheck()
	{
		this.m_preventNextAccountStatusCheck = true;
	}

	public void SendCheckAccountStatusRequest(Action<CheckAccountStatusResponse> onResponseCallback = null)
	{
		if (this.m_preventNextAccountStatusCheck)
		{
			this.m_preventNextAccountStatusCheck = false;
			return;
		}
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.CheckAccountStatus(onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void SendCheckRAFStatusRequest(bool getReferralCode, Action<CheckRAFStatusResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				if (onResponseCallback == null)
				{
					
					onResponseCallback = delegate(CheckRAFStatusResponse r)
						{
						};
				}
				this.m_lobbyGameClientInterface.CheckRAFStatus(getReferralCode, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void SendRAFReferralEmailsRequest(List<string> emails, Action<SendRAFReferralEmailsResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SendRAFReferralEmails(emails, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void SelectDailyQuest(int questId, Action<PickDailyQuestResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SelectDailyQuest(questId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.AbandonDailyQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void ActivateQuestTrigger(QuestTriggerType triggerType, int activationCount, int questId, int questBonusCount, int itemTemplateId, CharacterType charType, Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.ActivateQuestTrigger(triggerType, activationCount, questId, questBonusCount, itemTemplateId, charType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.BeginQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void CompleteQuest(int questId, Action<CompleteQuestResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.CompleteQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void MarkTutorialSkipped(TutorialVersion progress, Action<MarkTutorialSkippedResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.MarkTutorialSkipped(progress, delegate(MarkTutorialSkippedResponse response)
				{
					this.m_loadedPlayerAccountData.ExperienceComponent.TutorialProgress = progress;
					if (onResponseCallback != null)
					{
						onResponseCallback(response);
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void GetInventoryItems(Action<GetInventoryItemsResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				Action<GetInventoryItemsResponse> onResponseCallback2 = delegate(GetInventoryItemsResponse response)
				{
					if (response.Success)
					{
						if (this.m_loadedPlayerAccountData != null)
						{
							this.m_loadedPlayerAccountData.InventoryComponent.Items = response.Items;
						}
					}
					if (onResponseCallback != null)
					{
						onResponseCallback(response);
					}
				};
				this.m_lobbyGameClientInterface.GetInventoryItems(onResponseCallback2);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void AddInventoryItems(List<InventoryItem> items, Action<AddInventoryItemsResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.AddInventoryItems(items, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void RemoveInventoryItems(List<InventoryItem> items, Action<RemoveInventoryItemsResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.RemoveInventoryItems(items, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void ConsumeInventoryItem(int itemId, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		this.ConsumeInventoryItem(itemId, 1, toISO, onResponseCallback);
	}

	public void ConsumeInventoryItem(int itemId, int itemCount, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.ConsumeInventoryItem(itemId, itemCount, toISO, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void ConsumeInventoryItems(List<int> itemIds, bool toISO, Action<ConsumeInventoryItemsResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.ConsumeInventoryItems(itemIds, toISO, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.RerollSeasonQuests(seasonId, chapterId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void SetSeasonQuest(int seasonId, int chapterId, int slotNum, int questId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SetSeasonQuest(seasonId, chapterId, slotNum, questId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void NotifyStoreOpened()
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.NotifyStoreOpened();
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void NotifyCustomKeyBinds(Dictionary<int, KeyCodeData> CustomKeyBinds)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.NotifyCustomKeyBinds(CustomKeyBinds);
				return;
			}
		}
		Log.Error("Not connected to lobby server.", new object[0]);
	}

	public void NotifyOptions(OptionsNotification notification)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.NotifyOptions(notification);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		if (!SteamManager.UsingSteam)
		{
			if (this.m_lobbyGameClientInterface != null)
			{
				if (this.m_lobbyGameClientInterface.IsConnected)
				{
					this.m_lobbyGameClientInterface.RequestPaymentMethods(onResponseCallback);
					goto IL_5B;
				}
			}
			Log.Error("Not connected to lobby server.", new object[0]);
			IL_5B:;
		}
		else
		{
			PaymentMethod paymentMethod = new PaymentMethod();
			paymentMethod.id = -1L;
			paymentMethod.specificType = "Steam Wallet";
			paymentMethod.generalType = "Steam Wallet";
			paymentMethod.tier = "1";
			paymentMethod.maskedPaymentInfo = string.Empty;
			paymentMethod.expirationDate = string.Empty;
			paymentMethod.isDefault = true;
			onResponseCallback(new PaymentMethodsResponse
			{
				// TODO DECOMP ClientGameManager::RequestPaymentMethods
				//PaymentMethodList = new PaymentMethodList(),
				PaymentMethodList = 
				{
					IsError = false,
					PaymentMethods = 
					{
						paymentMethod
					}
				},
				Success = true
			});
		}
	}

	public void RequestRefreshBankData(Action<RefreshBankDataResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null && this.m_lobbyGameClientInterface.IsConnected)
		{
			this.m_lobbyGameClientInterface.RequestRefreshBankData(onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.", new object[0]);
		}
	}

	public bool IsServer()
	{
		return NetworkServer.active;
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return this.IsTitleUnlocked(title, out list);
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		this.GetUnlockStatus(title.m_unlockData, out unlockConditionValues, false);
		return this.m_loadedPlayerAccountData.AccountComponent.IsTitleUnlocked(title);
	}

	public bool IsTitleAtMaxLevel(GameBalanceVars.PlayerTitle title)
	{
		if (GameWideData.Get().m_gameBalanceVars != null)
		{
			int maxTitleLevel = GameWideData.Get().m_gameBalanceVars.GetMaxTitleLevel(title.ID);
			int currentTitleLevel = this.GetCurrentTitleLevel(title.ID);
			return currentTitleLevel >= maxTitleLevel;
		}
		return true;
	}

	public int GetCurrentTitleLevel(int titleID)
	{
		return this.m_loadedPlayerAccountData.AccountComponent.GetCurrentTitleLevel(titleID);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return this.IsEmojiUnlocked(emoji, out list);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		this.GetUnlockStatus(emoji.m_unlockData, out unlockConditionValues, false);
		return this.m_loadedPlayerAccountData.AccountComponent.IsChatEmojiUnlocked(emoji);
	}

	public bool IsOverconUnlocked(int overconId)
	{
		return this.m_loadedPlayerAccountData != null && this.m_loadedPlayerAccountData.AccountComponent.IsOverconUnlocked(overconId);
	}

	public bool IsBannerUnlocked(GameBalanceVars.PlayerBanner banner, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		this.GetUnlockStatus(banner.m_unlockData, out unlockConditionValues, false);
		return this.m_loadedPlayerAccountData.AccountComponent.IsBannerUnlocked(banner);
	}

	public bool IsRibbonUnlocked(GameBalanceVars.PlayerRibbon ribbon, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		this.GetUnlockStatus(ribbon.m_unlockData, out unlockConditionValues, false);
		return this.m_loadedPlayerAccountData.AccountComponent.IsRibbonUnlocked(ribbon);
	}

	public bool IsLoadingScreenBackgroundUnlocked(int loadingScreenBackgroundPathIndex)
	{
		return this.m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundPathIndex);
	}

	public bool GetUnlockStatus(GameBalanceVars.UnlockData unlock, bool ignorePurchaseCondition = false)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return this.GetUnlockStatus(unlock, out list, ignorePurchaseCondition);
	}

	public unsafe bool GetUnlockStatus(GameBalanceVars.UnlockData unlockData, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool ignorePurchaseCondition = false)
	{
		unlockConditionValues = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty<GameBalanceVars.UnlockCondition>())
			{
				for (int i = 0; i < unlockData.UnlockConditions.Length; i++)
				{
					unlockConditionValues.Add(new GameBalanceVars.UnlockConditionValue
					{
						ConditionType = unlockData.UnlockConditions[i].ConditionType
					});
				}
				for (int j = 0; j < unlockData.UnlockConditions.Length; j++)
				{
					GameBalanceVars.UnlockCondition unlockCondition = unlockData.UnlockConditions[j];
					GameBalanceVars.UnlockConditionValue unlockConditionValue = unlockConditionValues[j];
					switch (unlockCondition.ConditionType)
					{
					case GameBalanceVars.UnlockData.UnlockType.CharacterLevel:
					{
						PersistedCharacterData persistedCharacterData = this.m_loadedPlayerCharacterData.TryGetValue((CharacterType)unlockCondition.typeSpecificData);
						if (persistedCharacterData != null)
						{
							unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
							unlockConditionValue.typeSpecificData2 = persistedCharacterData.ExperienceComponent.Level;
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.PlayerLevel:
						unlockConditionValue.typeSpecificData = this.m_loadedPlayerAccountData.ExperienceComponent.Level;
						break;
					case GameBalanceVars.UnlockData.UnlockType.ELO:
						unlockConditionValue.typeSpecificData = 0;
						break;
					case GameBalanceVars.UnlockData.UnlockType.Quest:
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						QuestItem.GetQuestProgress(unlockCondition.typeSpecificData, out unlockConditionValue.typeSpecificData2, out unlockConditionValue.typeSpecificData3);
						break;
					case GameBalanceVars.UnlockData.UnlockType.FactionTierReached:
						unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
						unlockConditionValue.typeSpecificData2 = unlockCondition.typeSpecificData2;
						if (this.ActiveFactionCompetition == unlockCondition.typeSpecificData)
						{
							long factionScore;
							if (this.FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out factionScore))
							{
								unlockConditionValue.typeSpecificData3 = FactionWideData.Get().GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, factionScore);
							}
						}
						break;
					case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
					{
						PersistedAccountData loadedPlayerAccountData = this.m_loadedPlayerAccountData;
						int typeSpecificData = unlockCondition.typeSpecificData;
						int currentTitleLevel = loadedPlayerAccountData.AccountComponent.GetCurrentTitleLevel(typeSpecificData);
						unlockConditionValue.typeSpecificData = typeSpecificData;
						unlockConditionValue.typeSpecificData2 = currentTitleLevel;
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
						unlockConditionValue.typeSpecificData = this.m_loadedPlayerAccountData.QuestComponent.ActiveSeason;
						break;
					}
				}
				return unlockData.AreUnlockConditionsMet(unlockConditionValues, ignorePurchaseCondition);
			}
		}
		return false;
	}

	public bool AreUnlockConditionsMet(GameBalanceVars.UnlockData unlockData, bool ignorePurchaseCondition = false)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return this.GetUnlockStatus(unlockData, out list, ignorePurchaseCondition);
	}

	public bool AreUnlockConditionsMet(GameBalanceVars.PlayerUnlockable playerUnlockable, bool ignorePurchaseCondition = false)
	{
		return this.AreUnlockConditionsMet(playerUnlockable.m_unlockData, ignorePurchaseCondition);
	}

	public int GetCurrentTitleID()
	{
		if (this.m_loadedPlayerAccountData != null)
		{
			return this.m_loadedPlayerAccountData.AccountComponent.SelectedTitleID;
		}
		return -1;
	}

	public GameBalanceVars.PlayerBanner GetCurrentBackgroundBanner()
	{
		if (this.m_loadedPlayerAccountData != null && this.m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID != -1 && GameWideData.Get() != null && GameWideData.Get().m_gameBalanceVars != null)
		{
			return GameWideData.Get().m_gameBalanceVars.GetBanner(this.m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID);
		}
		return null;
	}

	public GameBalanceVars.PlayerBanner GetCurrentForegroundBanner()
	{
		if (this.m_loadedPlayerAccountData != null)
		{
			if (this.m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID != -1)
			{
				if (GameWideData.Get() != null)
				{
					if (GameWideData.Get().m_gameBalanceVars != null)
					{
						return GameWideData.Get().m_gameBalanceVars.GetBanner(this.m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID);
					}
				}
			}
		}
		return null;
	}

	public GameBalanceVars.PlayerRibbon GetCurrentRibbon()
	{
		if (this.m_loadedPlayerAccountData != null)
		{
			if (this.m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID != -1)
			{
				if (GameWideData.Get() != null)
				{
					if (GameWideData.Get().m_gameBalanceVars != null)
					{
						return GameWideData.Get().m_gameBalanceVars.GetRibbon(this.m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID);
					}
				}
			}
		}
		return null;
	}

	public string GetDisplayedStatString(LobbyPlayerInfo info)
	{
		return info.DisplayedStat.ToString();
	}

	public string GetDisplayedStatString(PersistedAccountData data)
	{
		if (data == null)
		{
			return string.Empty;
		}
		int num = 0;
		using (Dictionary<int, ExperienceComponent>.ValueCollection.Enumerator enumerator = data.QuestComponent.SeasonExperience.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ExperienceComponent experienceComponent = enumerator.Current;
				num += experienceComponent.Level;
			}
		}
		return this.GetDisplayedStatString(data.ExperienceComponent.Level, num, data.ExperienceComponent.Wins);
	}

	public string GetDisplayedStatString()
	{
		return this.GetDisplayedStatString(this.m_loadedPlayerAccountData);
	}

	public string GetDisplayedStatString(int accountLevel, int seasonLevel, int numWins)
	{
		if (GameBalanceVars.Get() == null)
		{
			return string.Empty;
		}
		if (accountLevel > seasonLevel)
		{
			return string.Format(StringUtil.TR("TotalSeasonLevelStatNumber", "Global"), accountLevel);
		}
		return string.Format(StringUtil.TR("LevelStatNumber", "Global"), seasonLevel);
	}

	public string GenerateQueueLabel()
	{
		LobbyMatchmakingQueueInfo queueInfo = GameManager.Get().QueueInfo;
		if (queueInfo != null)
		{
			return StringUtil.TR("Searching", "Frontend") + string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)Mathf.Max(0f, (float)queueInfo.AverageWaitTime.TotalSeconds));
		}
		GameStatus gameStatus = GameManager.Get().GameStatus;
		if (gameStatus == GameStatus.LoadoutSelecting)
		{
			return StringUtil.TR("SelectLoadout", "Global");
		}
		if (gameStatus == GameStatus.FreelancerSelecting)
		{
			GameType gameType;
			if (GameManager.Get().GameConfig != null)
			{
				gameType = GameManager.Get().GameConfig.GameType;
			}
			else
			{
				gameType = GameType.None;
			}
			GameType gameType2 = gameType;
			if (gameType2 != GameType.Practice)
			{
				if (gameType2 != GameType.Solo)
				{
					if (gameType2 != GameType.Duel)
					{
						if (gameType2 != GameType.Custom)
						{
							return StringUtil.TR("DuplicateFreelancers", "Global");
						}
					}
				}
			}
			if (gameType2 != GameType.Practice)
			{
				if (gameType2 != GameType.Solo)
				{
					return StringUtil.TR("LockedIn", "Global");
				}
			}
			return string.Empty;
		}
		if (this.GroupInfo != null)
		{
			if (this.GroupInfo.InAGroup)
			{
				int num = 0;
				bool flag = false;
				for (int i = 0; i < this.GroupInfo.Members.Count; i++)
				{
					if (this.GroupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
					{
						if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent() && AppState_GroupCharacterSelect.Get().IsReady())
						{
							flag = true;
							num++;
						}
						else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
						{
							if (AppState_CharacterSelect.IsReady())
							{
								flag = true;
								num++;
							}
						}
					}
					else if (this.GroupInfo.Members[i].IsReady)
					{
						num++;
					}
				}
				if (flag)
				{
					return string.Format(StringUtil.TR("WaitingForTeammates", "Global"), num, this.GroupInfo.Members.Count);
				}
			}
		}
		return string.Empty;
	}

	private void HttpPost(string url, string postString, Action<string, string> callback)
	{
		base.StartCoroutine(this.HttpPostCoroutine(url, postString, callback));
	}

	private IEnumerator HttpPostCoroutine(string url, string postString, Action<string, string> callback)
	{
		bool flag = false;
		WWW client;
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		byte[] postBytes = utf8Encoding.GetBytes(postString);
		client = new WWW(url, postBytes);

		try
		{
			yield return client;
			flag = true;
			if (!client.error.IsNullOrEmpty())
			{
				int num2 = client.error.IndexOf(": ");
				string arg;
				if (client.error.StartsWith("Failed to connect to") && num2 > 0 && num2 + 2 < client.error.Length)
				{
					arg = client.error.Substring(num2 + 2, client.error.Length - num2 - 2);
				}
				else
				{
					arg = client.error;
				}
				callback(null, arg);
			}
			else
			{
				callback(client.text, null);
			}
		}
		finally
		{
			if (flag)
			{
			}
			else if (client != null)
			{
				((IDisposable)client).Dispose();
			}
		}
		yield break;
	}

	public int NumCharacterResourcesCurrentlyLoading
	{
		get
		{
			if (this.m_loadingCharacterResources == null)
			{
				return 0;
			}
			return this.m_loadingCharacterResources.Count;
		}
	}

	public bool IsCurrentAlertQuest(int questId)
	{
		if (this.AlertMissionsData != null)
		{
			if (this.AlertMissionsData.CurrentAlert != null && this.AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
			{
				return this.AlertMissionsData.CurrentAlert.QuestId == questId;
			}
		}
		return false;
	}

	public bool IsTimeOffset()
	{
		return !(this.TimeOffset == default(TimeSpan));
	}

	public DateTime PacificNow()
	{
		DateTime d = this.UtcNow();
		return d - (this.ServerUtcTime - this.ServerPacificTime);
	}

	public DateTime UtcNow()
	{
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan t = utcNow - this.ClientUtcTime;
		return this.ServerUtcTime + t;
	}

	public DateTime UtcToPacific(DateTime dateTime)
	{
		return dateTime - (this.ServerUtcTime - this.ServerPacificTime);
	}

	public DateTime PacificToUtc(DateTime dateTime)
	{
		return dateTime + (this.ServerUtcTime - this.ServerPacificTime);
	}

	public string GetTierInstanceName(int instanceId)
	{
		if (instanceId < 1)
		{
			return null;
		}
		if (instanceId <= this.m_tierInstanceNames.Count)
		{
			return this.m_tierInstanceNames[instanceId - 1].ToString();
		}
		string text = string.Empty;
		switch (instanceId % 0x17)
		{
		case 0:
			text += "Iron ";
			break;
		case 1:
			text += "Grim ";
			break;
		case 2:
			text += "Golden ";
			break;
		case 3:
			text += "Strong ";
			break;
		case 4:
			text += "Quick ";
			break;
		case 5:
			text += "Sudden ";
			break;
		case 6:
			text += "Righteous ";
			break;
		case 7:
			text += "Black ";
			break;
		case 8:
			text += "Diamond ";
			break;
		case 9:
			text += "Metal ";
			break;
		case 0xA:
			text += "Lucky ";
			break;
		case 0xB:
			text += "Noble ";
			break;
		case 0xC:
			text += "Merciful ";
			break;
		case 0xD:
			text += "Wise ";
			break;
		case 0xE:
			text += "Divine ";
			break;
		case 0xF:
			text += "Modest ";
			break;
		case 0x10:
			text += "Atomic ";
			break;
		case 0x11:
			text += "Electric ";
			break;
		case 0x12:
			text += "Nightmare ";
			break;
		case 0x13:
			text += "Red ";
			break;
		case 0x14:
			text += "Green ";
			break;
		case 0x15:
			text += "Dangerous ";
			break;
		case 0x16:
			text += "Mercurial ";
			break;
		}
		switch (instanceId % 0x1D)
		{
		case 0:
			text += "Tiger";
			break;
		case 1:
			text += "Snake";
			break;
		case 2:
			text += "Justice";
			break;
		case 3:
			text += "Lion";
			break;
		case 4:
			text += "Shark";
			break;
		case 5:
			text += "Badger";
			break;
		case 6:
			text += "Wolverine";
			break;
		case 7:
			text += "Dragon";
			break;
		case 8:
			text += "Phoenix";
			break;
		case 9:
			text += "Honor";
			break;
		case 0xA:
			text += "Greed";
			break;
		case 0xB:
			text += "Moon";
			break;
		case 0xC:
			text += "Star";
			break;
		case 0xD:
			text += "Mountain";
			break;
		case 0xE:
			text += "River";
			break;
		case 0xF:
			text += "Fate";
			break;
		case 0x10:
			text += "Doom";
			break;
		case 0x11:
			text += "Loot";
			break;
		case 0x12:
			text += "Panda";
			break;
		case 0x13:
			text += "Eagle";
			break;
		case 0x14:
			text += "Guardian";
			break;
		case 0x15:
			text += "Thunder";
			break;
		case 0x16:
			text += "Future";
			break;
		case 0x17:
			text += "Armor";
			break;
		case 0x18:
			text += "Shield";
			break;
		case 0x19:
			text += "Sword";
			break;
		case 0x1A:
			text += "Payday";
			break;
		case 0x1B:
			text += "Axe";
			break;
		case 0x1C:
			text += "Briefcase";
			break;
		}
		return text;
	}

	public string GetTierName(GameType gameType, int tier)
	{
		GameTypeAvailability gameTypeAvailability;
		if (!this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
		{
			return string.Format("[BadGameType={0}]", gameType.ToString());
		}
		if (tier > gameTypeAvailability.PerTierDefinitions.Count)
		{
			return string.Format("[BadTier={0}]", tier);
		}
		if (tier < 1)
		{
			return StringUtil.TR("Placement", "RankMode");
		}
		LocalizationPayload nameLocalization = gameTypeAvailability.PerTierDefinitions[tier - 1].NameLocalization;
		if (nameLocalization != null)
		{
			return nameLocalization.ToString();
		}
		return string.Format("[Tier{0}NotLoc'd]", tier);
	}

	public IQueueRequirementSystemInfo QueueRequirementSystemInfo
	{
		get
		{
			if (this.m_queueRequirementSystemInfo == null)
			{
				this.m_queueRequirementSystemInfo = new ClientGameManager.ClientGameManagerRequirementSystemInfo
				{
					m_gtas = this.GameTypeAvailabilies,
					m_serverUtcTime = this.ServerUtcTime,
					m_environmentType = this.EnvironmentType
				};
			}
			else
			{
				this.m_queueRequirementSystemInfo.m_gtas = this.GameTypeAvailabilies;
				this.m_queueRequirementSystemInfo.m_serverUtcTime = this.ServerUtcTime;
				this.m_queueRequirementSystemInfo.m_environmentType = this.EnvironmentType;
			}
			return this.m_queueRequirementSystemInfo;
		}
	}

	public bool HasLeavingPenalty(GameType gameType)
	{
		GameTypeAvailability gameTypeAvailability;
		if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && gameTypeAvailability.GameLeavingPenalty != null)
		{
			return gameTypeAvailability.GameLeavingPenalty.PointsGainedForLeaving > 0f;
		}
		return false;
	}

	public LocalizationPayload GetBlockingQueueRestriction(GameType gameType)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails = new QueueBlockOutReasonDetails();
		return this.GetBlockingQueueRestriction(gameType, out queueBlockOutReasonDetails);
	}

	public unsafe LocalizationPayload GetBlockingQueueRestriction(GameType gameType, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		if (this.GameTypeAvailabilies != null)
		{
			IQueueRequirementApplicant queueRequirementApplicant = this.QueueRequirementApplicant;
			if (queueRequirementApplicant == null)
			{
				Log.Warning("Player can not be seen as a QueueRequirementApplicant!", new object[0]);
				return LocalizationPayload.Create("UnableToChangeGameMode", "Global");
			}
			bool flag;
			if (this.GroupInfo != null)
			{
				flag = this.GroupInfo.InAGroup;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			RequirementMessageContext context = (!flag2) ? RequirementMessageContext.SoloQueueing : RequirementMessageContext.GroupQueueing;
			GameTypeAvailability gameTypeAvailability;
			if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
			{
				if (!gameTypeAvailability.Requirements.IsNullOrEmpty<QueueRequirement>())
				{
					if (!gameTypeAvailability.IsActive)
					{
						return LocalizationPayload.Create((gameType != GameType.Ranked) ? "GameModeUnavailable@Global" : "RankedNotYetAvailable@RankMode");
					}
					IQueueRequirementSystemInfo queueRequirementSystemInfo = this.QueueRequirementSystemInfo;
					IEnumerator<QueueRequirement> enumerator = gameTypeAvailability.Requirements.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							QueueRequirement queueRequirement = enumerator.Current;
							bool flag3 = false;
							if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
							{
								if (!queueRequirement.AnyGroupMember)
								{
									LocalizationPayload result = queueRequirement.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, context, out Details);
									Details.CausedBySelf = new bool?(true);
									return result;
								}
								flag3 = true;
							}
							IEnumerator<IQueueRequirementApplicant> enumerator2 = this.GroupMembersAsQueueApplicants.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									IQueueRequirementApplicant applicant = enumerator2.Current;
									if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, applicant, gameType, null))
									{
										if (!queueRequirement.AnyGroupMember)
										{
											return queueRequirement.GenerateFailure(queueRequirementSystemInfo, applicant, context, out Details);
										}
									}
									else if (queueRequirement.AnyGroupMember)
									{
										flag3 = false;
										break;
									}
								}
							}
							finally
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
							if (flag3)
							{
								return queueRequirement.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, context, out Details);
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
			}
		}
		return null;
	}

	public IQueueRequirementApplicant QueueRequirementApplicant
	{
		get
		{
			PersistedAccountData playerAccountData = this.GetPlayerAccountData();
			if (playerAccountData != null)
			{
				if (this.m_ourQueueApplicant == null)
				{
					this.m_ourQueueApplicant = new ClientGameManager.OurQueueApplicant
					{
						m_pad = playerAccountData,
						AccessLevel = this.ClientAccessLevel
					};
				}
				else
				{
					this.m_ourQueueApplicant.m_pad = playerAccountData;
					this.m_ourQueueApplicant.AccessLevel = this.ClientAccessLevel;
				}
				return this.m_ourQueueApplicant;
			}
			return null;
		}
	}

	private ClientGameManager.GroupQueueApplicant ScratchGroupQueueApplicant
	{
		get
		{
			if (this.m_scratchGroupQueueApplicant == null)
			{
				this.m_scratchGroupQueueApplicant = new ClientGameManager.GroupQueueApplicant();
			}
			return this.m_scratchGroupQueueApplicant;
		}
	}

	public IEnumerable<IQueueRequirementApplicant> GroupMembersAsQueueApplicants
	{
		get
		{
			bool flag = false;
			List<UpdateGroupMemberData>.Enumerator enumerator;
			if (this.GroupInfo.Members.IsNullOrEmpty<UpdateGroupMemberData>())
			{
				yield break;
			}
			enumerator = this.GroupInfo.Members.GetEnumerator();
				
			try
			{
				while (enumerator.MoveNext())
				{
					UpdateGroupMemberData member = enumerator.Current;
					yield return new ClientGameManager.GroupQueueApplicant
					{
						m_member = member
					};
					flag = true;
				}
			}
			finally
			{
				if (flag)
				{
				}
				else
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			yield break;
		}
	}

	public bool MeetsAllQueueRequirements(GameType gameType)
	{
		bool result = false;
		if (this.GameTypeAvailabilies != null)
		{
			IQueueRequirementApplicant queueRequirementApplicant = this.QueueRequirementApplicant;
			if (queueRequirementApplicant != null)
			{
				result = true;
				GameTypeAvailability gameTypeAvailability;
				if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
				{
					if (gameTypeAvailability.Requirements != null)
					{
						if (!gameTypeAvailability.IsActive)
						{
							result = false;
						}
						else
						{
							List<QueueRequirement> list = gameTypeAvailability.Requirements.ToList();
							if (!list.IsNullOrEmpty<QueueRequirement>())
							{
								IQueueRequirementSystemInfo queueRequirementSystemInfo = this.QueueRequirementSystemInfo;
								using (List<QueueRequirement>.Enumerator enumerator = list.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										QueueRequirement queueRequirement = enumerator.Current;
										bool flag = false;
										if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
										{
											if (!queueRequirement.AnyGroupMember)
											{
												return false;
											}
											flag = true;
										}
										ClientGameManager.GroupQueueApplicant scratchGroupQueueApplicant = this.ScratchGroupQueueApplicant;
										foreach (UpdateGroupMemberData member in this.GroupInfo.Members)
										{
											scratchGroupQueueApplicant.m_member = member;
											if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, scratchGroupQueueApplicant, gameType, null))
											{
												if (!queueRequirement.AnyGroupMember)
												{
													result = false;
													break;
												}
											}
											else if (queueRequirement.AnyGroupMember)
											{
												flag = false;
												break;
											}
										}
										if (flag)
										{
											result = false;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	public bool MeetsGroupSizeRequirement(GameType gameType, int groupSize)
	{
		bool result = true;
		if (!this.GameTypeAvailabilies.IsNullOrEmpty<KeyValuePair<GameType, GameTypeAvailability>>())
		{
			GameTypeAvailability gameTypeAvailability;
			if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.QueueableGroupSizes.IsNullOrEmpty<KeyValuePair<int, RequirementCollection>>())
			{
				RequirementCollection requirementCollection;
				if (gameTypeAvailability.QueueableGroupSizes.TryGetValue(groupSize, out requirementCollection))
				{
					if (!requirementCollection.IsNullOrEmpty<QueueRequirement>())
					{
						IQueueRequirementSystemInfo queueRequirementSystemInfo = this.QueueRequirementSystemInfo;
						IQueueRequirementApplicant queueRequirementApplicant = this.QueueRequirementApplicant;
						using (IEnumerator<QueueRequirement> enumerator = requirementCollection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								QueueRequirement queueRequirement = enumerator.Current;
								if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
								{
									bool flag = false;
									if (queueRequirement.AnyGroupMember)
									{
										IEnumerator<IQueueRequirementApplicant> enumerator2 = this.GroupMembersAsQueueApplicants.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												IQueueRequirementApplicant applicant = enumerator2.Current;
												if (queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, applicant, gameType, null))
												{
													flag = true;
													break;
												}
											}
										}
										finally
										{
											if (enumerator2 != null)
											{
												enumerator2.Dispose();
											}
										}
									}
									if (!flag)
									{
										result = false;
									}
								}
							}
						}
					}
				}
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	public LocalizationPayload GetReasonGroupSizeCantQueue(GameType gameType, int groupSize)
	{
		if (this.GameTypeAvailabilies.IsNullOrEmpty<KeyValuePair<GameType, GameTypeAvailability>>())
		{
			Log.Warning("No valid GameTypeAvailabilites loaded yet for {0}", new object[]
			{
				gameType
			});
			return null;
		}
		GameTypeAvailability gameTypeAvailability;
		if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.QueueableGroupSizes.IsNullOrEmpty<KeyValuePair<int, RequirementCollection>>())
		{
			RequirementCollection requirementCollection;
			if (!gameTypeAvailability.QueueableGroupSizes.TryGetValue(groupSize, out requirementCollection))
			{
				return LocalizationPayload.Create("BadGroupSizeForQueue", "Matchmaking", new LocalizationArg[]
				{
					LocalizationArg_Int32.Create(groupSize)
				});
			}
			if (!requirementCollection.IsNullOrEmpty<QueueRequirement>())
			{
				IQueueRequirementSystemInfo queueRequirementSystemInfo = this.QueueRequirementSystemInfo;
				IQueueRequirementApplicant queueRequirementApplicant = this.QueueRequirementApplicant;
				IEnumerator<QueueRequirement> enumerator = requirementCollection.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						QueueRequirement queueRequirement = enumerator.Current;
						if (!queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
						{
							bool flag = false;
							if (queueRequirement.AnyGroupMember)
							{
								IEnumerator<IQueueRequirementApplicant> enumerator2 = this.GroupMembersAsQueueApplicants.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										IQueueRequirementApplicant applicant = enumerator2.Current;
										if (queueRequirement.DoesApplicantPass(queueRequirementSystemInfo, applicant, gameType, null))
										{
											flag = true;
											break;
										}
									}
								}
								finally
								{
									if (enumerator2 != null)
									{
										enumerator2.Dispose();
									}
								}
							}
							if (!flag)
							{
								RequirementMessageContext requirementMessageContext;
								if (groupSize > 1)
								{
									requirementMessageContext = RequirementMessageContext.GroupQueueing;
								}
								else
								{
									requirementMessageContext = RequirementMessageContext.SoloQueueing;
								}
								RequirementMessageContext requirementMessageContext2 = requirementMessageContext;
								LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(groupSize);
								LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType);
								LocalizationPayload payload = queueRequirement.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, requirementMessageContext2);
								LocalizationArg_LocalizationPayload localizationArg_LocalizationPayload = LocalizationArg_LocalizationPayload.Create(payload);
								if (requirementMessageContext2 == RequirementMessageContext.GroupQueueing)
								{
									return LocalizationPayload.Create("GroupSizeRequirementFailure", "matchmaking", new LocalizationArg[]
									{
										localizationArg_Int,
										localizationArg_GameType,
										localizationArg_LocalizationPayload
									});
								}
								return LocalizationPayload.Create("SoloSizeRequirementFailure", "matchmaking", new LocalizationArg[]
								{
									localizationArg_GameType,
									localizationArg_LocalizationPayload
								});
							}
						}
					}
				}
				finally
				{
					if (enumerator != null)
					{
						enumerator.Dispose();
					}
				}
			}
		}
		return null;
	}

	public ushort GenerateGameSubTypeMaskForToggledAntiSocial(GameType gameType, ushort currentMask)
	{
		GameTypeAvailability gameTypeAvailability;
		if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.SubTypes.IsNullOrEmpty<GameSubType>())
		{
			return GameSubType.CalculatePivotSubTypes(currentMask, GameSubType.SubTypeMods.AntiSocial, gameTypeAvailability.SubTypes);
		}
		return 0;
	}

	public unsafe bool IsMapInGameType(GameType gameType, string mapName, out bool isActive)
	{
		bool result = false;
		isActive = false;
		GameTypeAvailability gameTypeAvailability;
		if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.SubTypes.IsNullOrEmpty<GameSubType>())
		{
			using (List<GameSubType>.Enumerator enumerator = gameTypeAvailability.SubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameSubType gameSubType = enumerator.Current;
					GameMapConfig gameMapConfig = (from p in gameSubType.GameMapConfigs
					where p.Map == mapName
					select p).FirstOrDefault<GameMapConfig>();
					if (gameMapConfig != null)
					{
						result = true;
						if (gameMapConfig.IsActive)
						{
							isActive = true;
							return result;
						}
					}
				}
			}
		}
		return result;
	}

	public Dictionary<ushort, GameSubType> GetGameTypeSubTypes(GameType gameType)
	{
		GameTypeAvailability gameTypeAvailability;
		if (this.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
		{
			if (!gameTypeAvailability.SubTypes.IsNullOrEmpty<GameSubType>())
			{
				Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
				ushort num = 1;
				using (List<GameSubType>.Enumerator enumerator = gameTypeAvailability.SubTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameSubType value = enumerator.Current;
						dictionary.Add(num, value);
						num = (ushort)(num << 1);
					}
				}
				return dictionary;
			}
		}
		return null;
	}

	public void SetGameTypeSubMasks(GameType gameType, ushort subGameMask, Action<SetGameSubTypeResponse> onResponseCallback)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SetGameTypeSubMasks(gameType, subGameMask, onResponseCallback);
				return;
			}
		}
		if (onResponseCallback != null)
		{
			onResponseCallback(new SetGameSubTypeResponse
			{
				Success = false,
				ErrorMessage = "Not connected to lobby"
			});
		}
	}

	public void SetNewSessionLanguage(string languageCode)
	{
		if (this.m_lobbyGameClientInterface != null)
		{
			if (this.m_lobbyGameClientInterface.IsConnected)
			{
				this.m_lobbyGameClientInterface.SetNewSessionLanguage(languageCode);
			}
		}
	}

	public bool IsFreelancerInFreeRotationExtension(CharacterType characterType, GameType gameType, GameSubType gameSubType = null)
	{
		RequirementCollection requirementCollection;
		if (!this.FreeRotationAdditions.IsNullOrEmpty<KeyValuePair<CharacterType, RequirementCollection>>() && this.FreeRotationAdditions.TryGetValue(characterType, out requirementCollection))
		{
			IQueueRequirementApplicant queueRequirementApplicant = this.QueueRequirementApplicant;
			IQueueRequirementSystemInfo queueRequirementSystemInfo = this.QueueRequirementSystemInfo;
			if (requirementCollection.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, gameSubType))
			{
				return true;
			}
		}
		return false;
	}

	private bool m_discordConnected
	{
		get
		{
			return DiscordClientInterface.Get().IsConnected;
		}
	}

	private class FreelancerSetQueryInterface : IFreelancerSetQueryInterface
	{
		private FactionWideData m_fwd;

		private GameWideData m_gwd;

		internal FreelancerSetQueryInterface(FactionWideData fwd, GameWideData gwd)
		{
			this.m_fwd = fwd;
			this.m_gwd = gwd;
		}

		public HashSet<CharacterType> GetCharacterTypesFromRoles(List<CharacterRole> roles)
		{
			HashSet<CharacterType> retVal = new HashSet<CharacterType>();
			if (!roles.IsNullOrEmpty<CharacterRole>())
			{
				(from p in this.m_gwd.m_characterResourceLinks
				where roles.Contains(p.m_characterRole)
				select p).ToList<CharacterResourceLink>().ForEach(delegate(CharacterResourceLink p)
				{
					retVal.Add(p.m_characterType);
				});
			}
			return retVal;
		}

		public HashSet<CharacterType> GetCharacterTypesFromFractionGroupIds(List<int> groupIds)
		{
			HashSet<CharacterType> retVal = new HashSet<CharacterType>();
			if (!groupIds.IsNullOrEmpty<int>())
			{
				(from p in this.m_fwd.m_factionGroups
				where groupIds.Contains(p.FactionGroupID)
				select p).ToList<FactionGroup>().ForEach(delegate(FactionGroup p)
				{
					retVal.UnionWith(p.Characters);
				});
			}
			return retVal;
		}

		public bool DoesCharacterMatchRoles(CharacterType freelancer, List<CharacterRole> roles)
		{
			if (roles.IsNullOrEmpty<CharacterRole>())
			{
				return false;
			}
			return (from p in this.m_gwd.m_characterResourceLinks
			where roles.Contains(p.m_characterRole) && p.m_characterType == freelancer
			select p).Count<CharacterResourceLink>() > 0;
		}

		public bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds)
		{
			bool result;
			if (!groupIds.IsNullOrEmpty<int>())
			{
				result = this.m_fwd.m_factionGroups.Exists((FactionGroup p) => p.Characters.Contains(freelancer) && groupIds.Contains(p.FactionGroupID));
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	private class ClientGameManagerRequirementSystemInfo : IQueueRequirementSystemInfo
	{
		internal Dictionary<GameType, GameTypeAvailability> m_gtas;

		internal DateTime m_serverUtcTime;

		internal EnvironmentType m_environmentType;

		internal ClientGameManager.FreelancerSetQueryInterface m_freelancerSetQueryInterface;

		public EnvironmentType GetEnvironmentType()
		{
			return this.m_environmentType;
		}

		public DateTime GetCurrentUTCTime()
		{
			return this.m_serverUtcTime;
		}

		public IEnumerable<GameType> GetGameTypes()
		{
			return this.m_gtas.Keys;
		}

		public GameLeavingPenalty GetGameLeavingPenaltyForGameType(GameType gameType)
		{
			GameTypeAvailability gameTypeAvailability;
			if (this.m_gtas.TryGetValue(gameType, out gameTypeAvailability))
			{
				return gameTypeAvailability.GameLeavingPenalty;
			}
			return null;
		}

		public IEnumerable<QueueRequirement> GetQueueRequirements(GameType gameType)
		{
			bool flag = false;
			IEnumerator<QueueRequirement> enumerator;
			GameTypeAvailability gta;
			if (!this.m_gtas.TryGetValue(gameType, out gta))
			{
				yield break;
			}
			if (gta.Requirements.IsNullOrEmpty<QueueRequirement>())
			{
				yield break;
			}
			enumerator = gta.Requirements.GetEnumerator();
			
			try
			{
				while (enumerator.MoveNext())
				{
					QueueRequirement req = enumerator.Current;
					yield return req;
					flag = true;
				}
			}
			finally
			{
				if (flag)
				{
				}
				else if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			yield break;
		}

		public bool IsCharacterAllowed(CharacterType ct, GameType gameType, GameSubType gst)
		{
			GameManager gameManager = GameManager.Get();
			return gameManager.IsCharacterAllowedForPlayers(ct) && gameManager.IsCharacterAllowedForGameType(ct, gameType, gst, this.FreelancerSetQueryInterface);
		}

		public IFreelancerSetQueryInterface FreelancerSetQueryInterface
		{
			get
			{
				if (this.m_freelancerSetQueryInterface == null)
				{
					this.m_freelancerSetQueryInterface = new ClientGameManager.FreelancerSetQueryInterface(FactionWideData.Get(), GameWideData.Get());
				}
				return this.m_freelancerSetQueryInterface;
			}
		}

		public List<SeasonTemplate> Seasons
		{
			get
			{
				return SeasonWideData.Get().m_seasons;
			}
		}
	}

	private class OurQueueApplicant : IQueueRequirementApplicant
	{
		internal PersistedAccountData m_pad;

		public int TotalMatches
		{
			get
			{
				return this.m_pad.ExperienceComponent.Matches;
			}
		}

		public int VsHumanMatches
		{
			get
			{
				return this.m_pad.ExperienceComponent.VsHumanMatches;
			}
		}

		public CharacterType CharacterType
		{
			get
			{
				return this.m_pad.AccountComponent.LastCharacter;
			}
		}

		public int CharacterMatches
		{
			get
			{
				if (this.m_pad.CharacterData.ContainsKey(this.CharacterType))
				{
					return this.m_pad.CharacterData[this.CharacterType].ExperienceComponent.Matches;
				}
				return 0;
			}
		}

		public IEnumerable<CharacterType> AvailableCharacters
		{
			get
			{
				bool flag = false;
				ClientGameManager cgm;
				GameType gameType;
				IEnumerator enumerator;
				cgm = ClientGameManager.Get();
				gameType = cgm.GroupInfo.SelectedQueueType;
				enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CharacterType ct = (CharacterType)obj;
						if (cgm.IsCharacterAvailable(ct, gameType))
						{
							yield return ct;
							flag = true;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if (flag)
					{
					}
					else if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				yield break;
			}
		}

		public ClientAccessLevel AccessLevel { get; set; }

		public int SeasonLevel
		{
			get
			{
				return this.m_pad.SeasonLevel;
			}
		}

		public LocalizationArg_Handle LocalizedHandle
		{
			get
			{
				return LocalizationArg_Handle.Create(this.m_pad.Handle);
			}
		}

		public float GameLeavingPoints
		{
			get
			{
				return this.m_pad.AdminComponent.GameLeavingPoints;
			}
		}

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			GameType selectedQueueType = clientGameManager.GroupInfo.SelectedQueueType;
			return clientGameManager.IsCharacterAvailable(ct, selectedQueueType);
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			return this.m_pad.GetReactorLevel(seasons);
		}
	}

	private class GroupQueueApplicant : IQueueRequirementApplicant
	{
		internal UpdateGroupMemberData m_member;

		public int TotalMatches
		{
			get
			{
				return int.MaxValue;
			}
		}

		public int VsHumanMatches
		{
			get
			{
				return int.MaxValue;
			}
		}

		public CharacterType CharacterType
		{
			get
			{
				return this.m_member.MemberDisplayCharacter;
			}
		}

		public int CharacterMatches
		{
			get
			{
				return int.MaxValue;
			}
		}

		public ClientAccessLevel AccessLevel
		{
			get
			{
				ClientAccessLevel result;
				if (this.m_member.HasFullAccess)
				{
					result = ClientAccessLevel.Full;
				}
				else
				{
					result = ClientAccessLevel.Free;
				}
				return result;
			}
		}

		public int SeasonLevel
		{
			get
			{
				return int.MaxValue;
			}
		}

		public LocalizationArg_Handle LocalizedHandle
		{
			get
			{
				return LocalizationArg_Handle.Create(this.m_member.MemberDisplayName);
			}
		}

		public float GameLeavingPoints
		{
			get
			{
				return this.m_member.GameLeavingPoints;
			}
		}

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			return true;
		}

		public IEnumerable<CharacterType> AvailableCharacters
		{
			get
			{
				bool flag = false;
				IEnumerator enumerator;
				enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CharacterType ct = (CharacterType)obj;
						yield return ct;
						flag = true;
					}
				}
				finally
				{
					IDisposable disposable;
					if (flag)
					{
					}
					else if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				yield break;
			}
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			return int.MaxValue;
		}
	}
}
