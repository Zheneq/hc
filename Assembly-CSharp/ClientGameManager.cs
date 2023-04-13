using System;
using System.Collections;
using System.Collections.Generic;
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
	private ClientGameManagerRequirementSystemInfo m_queueRequirementSystemInfo;
	private OurQueueApplicant m_ourQueueApplicant;
	private GroupQueueApplicant m_scratchGroupQueueApplicant;
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
		OnConnectedToLobbyServerHolder = delegate {};
		OnDisconnectedFromLobbyServerHolder = delegate {};
		OnLobbyServerReadyNotificationHolder = delegate {};
		OnLobbyStatusNotificationHolder = delegate {};		
		OnLobbyCustomGamesNotificationHolder = delegate {};
		OnQueueAssignmentNotificationHolder = delegate {};
		OnQueueStatusNotificationHolder = delegate {};		
		OnQueueEnteredHolder = delegate {};		
		OnQueueLeftHolder = delegate {};		
		OnGameAssignmentNotificationHolder = delegate {};		
		OnGameInfoNotificationHolder = delegate {};		
		OnLobbyServerLockStateChangeHolder = delegate {};		
		OnLobbyServerClientAccessLevelChangeHolder = delegate {};
		OnLobbyGameplayOverridesChangeHolder = delegate {};		
		OnBankBalanceChangeHolder = delegate {};		
		OnModUnlockedHolder = delegate {};		
		OnAccountDataUpdatedHolder = delegate {};		
		OnCharacterDataUpdatedHolder = delegate {};		
		OnInventoryComponentUpdatedHolder = delegate {};		
		OnChatNotificationHolder = delegate {};		
		OnSetDevTagResponseHolder = delegate {};
		OnUseOverconNotificationHolder = delegate {};		
		OnUseGGPackNotificationHolder = delegate {};		
		OnGroupUpdateNotificationHolder = delegate {};		
		OnFriendStatusNotificationHolder = delegate {};		
		OnPlayerTitleChangeHolder = delegate {};		
		OnPlayerBannerChangeHolder = delegate {};
		OnPlayerRibbonChangeHolder = delegate {};		
		OnLoadingScreenBackgroundToggledHolder = delegate {};
		OnQuestCompleteNotificationHolder = delegate {};		
		OnMatchResultsNotificationHolder = delegate {};		
		OnChapterUnlockNotificationHolder = delegate {};		
		OnServerQueueConfigurationUpdateNotificationHolder = delegate {};		
		OnSeasonCompleteNotificationHolder = delegate {};
		OnChapterCompleteNotificationHolder = delegate {};
		OnFactionCompetitionNotificationHolder = delegate {};
		OnTrustBoostUsedNotificationHolder = delegate {};
		OnPlayerFactionContributionChangeNotificationHolder = delegate {};
		OnFactionLoginRewardNotificationHolder = delegate {};
		OnQuestProgressChangedHolder = delegate {};
		OnAlertMissionDataChangeHolder = delegate {};
		OnSeasonChapterQuestsChangeHolder = delegate {};
		OurQueueEntryTime = DateTime.MinValue;
		SoloSubTypeMask = new Dictionary<GameType, ushort>();
		m_loadingProgressUpdateFrequency = 0.5f;
	}

	public static ClientGameManager Get()
	{
		return s_instance;
	}

	public LobbyGameClientInterface LobbyInterface => m_lobbyGameClientInterface;
	public LobbySessionInfo SessionInfo => m_lobbyGameClientInterface?.SessionInfo;
	public string Handle => m_lobbyGameClientInterface?.SessionInfo?.Handle;
	public long AccountId => m_lobbyGameClientInterface?.SessionInfo?.AccountId ?? -1L;

	private Action<RegisterGameClientResponse> OnConnectedToLobbyServerHolder;
	public event Action<RegisterGameClientResponse> OnConnectedToLobbyServer
	{
		add
		{
			Action<RegisterGameClientResponse> action = OnConnectedToLobbyServerHolder;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<RegisterGameClientResponse> action = OnConnectedToLobbyServerHolder;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<string> OnDisconnectedFromLobbyServerHolder;
	public event Action<string> OnDisconnectedFromLobbyServer
	{
		add
		{
			Action<string> action = OnDisconnectedFromLobbyServerHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = OnDisconnectedFromLobbyServerHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyServerReadyNotification> OnLobbyServerReadyNotificationHolder;
	public event Action<LobbyServerReadyNotification> OnLobbyServerReadyNotification
	{
		add
		{
			Action<LobbyServerReadyNotification> action = OnLobbyServerReadyNotificationHolder;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyServerReadyNotification> action = OnLobbyServerReadyNotificationHolder;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyStatusNotification> OnLobbyStatusNotificationHolder;
	public event Action<LobbyStatusNotification> OnLobbyStatusNotification
	{
		add
		{
			Action<LobbyStatusNotification> action = OnLobbyStatusNotificationHolder;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyStatusNotification> action = OnLobbyStatusNotificationHolder;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotificationHolder;
	public event Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotification
	{
		add
		{
			Action<LobbyCustomGamesNotification> action = OnLobbyCustomGamesNotificationHolder;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyCustomGamesNotification> action = OnLobbyCustomGamesNotificationHolder;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Remove(action2, value), action);
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
			Action<MatchmakingQueueStatusNotification> action = OnQueueStatusNotificationHolder;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchmakingQueueStatusNotification> action = OnQueueStatusNotificationHolder;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnQueueEnteredHolder;
	public event Action OnQueueEntered
	{
		add
		{
			Action action = OnQueueEnteredHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueEnteredHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OnQueueEnteredHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueEnteredHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnQueueLeftHolder;
	public event Action OnQueueLeft
	{
		add
		{
			Action action = OnQueueLeftHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueLeftHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OnQueueLeftHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQueueLeftHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameAssignmentNotification> OnGameAssignmentNotificationHolder;
	public event Action<GameAssignmentNotification> OnGameAssignmentNotification
	{
		add
		{
			Action<GameAssignmentNotification> action = OnGameAssignmentNotificationHolder;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameAssignmentNotification> action = OnGameAssignmentNotificationHolder;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameInfoNotification> OnGameInfoNotificationHolder;
	public event Action<GameInfoNotification> OnGameInfoNotification
	{
		add
		{
			Action<GameInfoNotification> action = OnGameInfoNotificationHolder;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameInfoNotification> action = OnGameInfoNotificationHolder;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ServerLockState, ServerLockState> OnLobbyServerLockStateChangeHolder;
	public event Action<ServerLockState, ServerLockState> OnLobbyServerLockStateChange
	{
		add
		{
			Action<ServerLockState, ServerLockState> action = OnLobbyServerLockStateChangeHolder;
			Action<ServerLockState, ServerLockState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ServerLockState, ServerLockState> action = OnLobbyServerLockStateChangeHolder;
			Action<ServerLockState, ServerLockState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ClientAccessLevel, ClientAccessLevel> OnLobbyServerClientAccessLevelChangeHolder;
	public event Action<ClientAccessLevel, ClientAccessLevel> OnLobbyServerClientAccessLevelChange
	{
		add
		{
			Action<ClientAccessLevel, ClientAccessLevel> action = OnLobbyServerClientAccessLevelChangeHolder;
			Action<ClientAccessLevel, ClientAccessLevel> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ClientAccessLevel, ClientAccessLevel> action = OnLobbyServerClientAccessLevelChangeHolder;
			Action<ClientAccessLevel, ClientAccessLevel> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyGameplayOverrides> OnLobbyGameplayOverridesChangeHolder;
	public event Action<LobbyGameplayOverrides> OnLobbyGameplayOverridesChange
	{
		add
		{
			Action<LobbyGameplayOverrides> action = OnLobbyGameplayOverridesChangeHolder;
			Action<LobbyGameplayOverrides> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyGameplayOverrides> action = OnLobbyGameplayOverridesChangeHolder;
			Action<LobbyGameplayOverrides> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<CurrencyData> OnBankBalanceChangeHolder;
	public event Action<CurrencyData> OnBankBalanceChange
	{
		add
		{
			Action<CurrencyData> action = OnBankBalanceChangeHolder;
			Action<CurrencyData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<CurrencyData> action = OnBankBalanceChangeHolder;
			Action<CurrencyData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Remove(action2, value), action);
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
			Action<PersistedAccountData> action = OnAccountDataUpdatedHolder;
			Action<PersistedAccountData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PersistedAccountData> action = OnAccountDataUpdatedHolder;
			Action<PersistedAccountData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<PersistedCharacterData> OnCharacterDataUpdatedHolder;
	public event Action<PersistedCharacterData> OnCharacterDataUpdated
	{
		add
		{
			Action<PersistedCharacterData> action = OnCharacterDataUpdatedHolder;
			Action<PersistedCharacterData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PersistedCharacterData> action = OnCharacterDataUpdatedHolder;
			Action<PersistedCharacterData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<InventoryComponent> OnInventoryComponentUpdatedHolder;
	public event Action<InventoryComponent> OnInventoryComponentUpdated
	{
		add
		{
			Action<InventoryComponent> action = OnInventoryComponentUpdatedHolder;
			Action<InventoryComponent> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<InventoryComponent> action = OnInventoryComponentUpdatedHolder;
			Action<InventoryComponent> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ChatNotification> OnChatNotificationHolder;
	public event Action<ChatNotification> OnChatNotification
	{
		add
		{
			Action<ChatNotification> action = OnChatNotificationHolder;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ChatNotification> action = OnChatNotificationHolder;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<SetDevTagResponse> OnSetDevTagResponseHolder;
	public event Action<SetDevTagResponse> OnSetDevTagResponse
	{
		add
		{
			Action<SetDevTagResponse> action = OnSetDevTagResponseHolder;
			Action<SetDevTagResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<SetDevTagResponse> action = OnSetDevTagResponseHolder;
			Action<SetDevTagResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<UseOverconResponse> OnUseOverconNotificationHolder;
	public event Action<UseOverconResponse> OnUseOverconNotification
	{
		add
		{
			Action<UseOverconResponse> action = OnUseOverconNotificationHolder;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseOverconResponse> action = OnUseOverconNotificationHolder;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<UseGGPackNotification> OnUseGGPackNotificationHolder;
	public event Action<UseGGPackNotification> OnUseGGPackNotification
	{
		add
		{
			Action<UseGGPackNotification> action = OnUseGGPackNotificationHolder;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseGGPackNotification> action = OnUseGGPackNotificationHolder;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action OnGroupUpdateNotificationHolder;
	public event Action OnGroupUpdateNotification
	{
		add
		{
			Action action = OnGroupUpdateNotificationHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGroupUpdateNotificationHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OnGroupUpdateNotificationHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGroupUpdateNotificationHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<FriendStatusNotification> OnFriendStatusNotificationHolder;
	public event Action<FriendStatusNotification> OnFriendStatusNotification
	{
		add
		{
			Action<FriendStatusNotification> action = OnFriendStatusNotificationHolder;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FriendStatusNotification> action = OnFriendStatusNotificationHolder;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<string> OnPlayerTitleChangeHolder;
	public event Action<string> OnPlayerTitleChange
	{
		add
		{
			Action<string> action = OnPlayerTitleChangeHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerTitleChangeHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = OnPlayerTitleChangeHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerTitleChangeHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> OnPlayerBannerChangeHolder;
	public event Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> OnPlayerBannerChange
	{
		add
		{
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action = OnPlayerBannerChangeHolder;
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action = OnPlayerBannerChangeHolder;
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<GameBalanceVars.PlayerRibbon> OnPlayerRibbonChangeHolder;
	public event Action<GameBalanceVars.PlayerRibbon> OnPlayerRibbonChange
	{
		add
		{
			Action<GameBalanceVars.PlayerRibbon> action = OnPlayerRibbonChangeHolder;
			Action<GameBalanceVars.PlayerRibbon> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameBalanceVars.PlayerRibbon> action = OnPlayerRibbonChangeHolder;
			Action<GameBalanceVars.PlayerRibbon> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<int, bool> OnLoadingScreenBackgroundToggledHolder;
	public event Action<int, bool> OnLoadingScreenBackgroundToggled
	{
		add
		{
			Action<int, bool> action = OnLoadingScreenBackgroundToggledHolder;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int, bool> action = OnLoadingScreenBackgroundToggledHolder;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<QuestCompleteNotification> OnQuestCompleteNotificationHolder;
	public event Action<QuestCompleteNotification> OnQuestCompleteNotification
	{
		add
		{
			Action<QuestCompleteNotification> action = OnQuestCompleteNotificationHolder;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<QuestCompleteNotification> action = OnQuestCompleteNotificationHolder;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<MatchResultsNotification> OnMatchResultsNotificationHolder;
	public event Action<MatchResultsNotification> OnMatchResultsNotification
	{
		add
		{
			Action<MatchResultsNotification> action = OnMatchResultsNotificationHolder;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchResultsNotification> action = OnMatchResultsNotificationHolder;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<int, int> OnChapterUnlockNotificationHolder;
	public event Action<int, int> OnChapterUnlockNotification
	{
		add
		{
			Action<int, int> action = OnChapterUnlockNotificationHolder;
			Action<int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int, int> action = OnChapterUnlockNotificationHolder;
			Action<int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotificationHolder;
	public event Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotification
	{
		add
		{
			Action<ServerQueueConfigurationUpdateNotification> action = OnServerQueueConfigurationUpdateNotificationHolder;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ServerQueueConfigurationUpdateNotification> action = OnServerQueueConfigurationUpdateNotificationHolder;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<SeasonStatusNotification> OnSeasonCompleteNotificationHolder;
	public event Action<SeasonStatusNotification> OnSeasonCompleteNotification
	{
		add
		{
			Action<SeasonStatusNotification> action = OnSeasonCompleteNotificationHolder;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<SeasonStatusNotification> action = OnSeasonCompleteNotificationHolder;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Remove(action2, value), action);
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
			Action<FactionCompetitionNotification> action = OnFactionCompetitionNotificationHolder;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionCompetitionNotification> action = OnFactionCompetitionNotificationHolder;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<TrustBoostUsedNotification> OnTrustBoostUsedNotificationHolder;
	public event Action<TrustBoostUsedNotification> OnTrustBoostUsedNotification
	{
		add
		{
			Action<TrustBoostUsedNotification> action = OnTrustBoostUsedNotificationHolder;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<TrustBoostUsedNotification> action = OnTrustBoostUsedNotificationHolder;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChangeNotificationHolder;
	public event Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChangeNotification
	{
		add
		{
			Action<PlayerFactionContributionChangeNotification> action = OnPlayerFactionContributionChangeNotificationHolder;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PlayerFactionContributionChangeNotification> action = OnPlayerFactionContributionChangeNotificationHolder;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<FactionLoginRewardNotification> OnFactionLoginRewardNotificationHolder;
	public event Action<FactionLoginRewardNotification> OnFactionLoginRewardNotification
	{
		add
		{
			Action<FactionLoginRewardNotification> action = OnFactionLoginRewardNotificationHolder;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionLoginRewardNotification> action = OnFactionLoginRewardNotificationHolder;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<QuestProgress[]> OnQuestProgressChangedHolder;
	public event Action<QuestProgress[]> OnQuestProgressChanged
	{
		add
		{
			Action<QuestProgress[]> action = OnQuestProgressChangedHolder;
			Action<QuestProgress[]> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<QuestProgress[]> action = OnQuestProgressChangedHolder;
			Action<QuestProgress[]> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<LobbyAlertMissionDataNotification> OnAlertMissionDataChangeHolder;
	public event Action<LobbyAlertMissionDataNotification> OnAlertMissionDataChange
	{
		add
		{
			Action<LobbyAlertMissionDataNotification> action = OnAlertMissionDataChangeHolder;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyAlertMissionDataNotification> action = OnAlertMissionDataChangeHolder;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<Dictionary<int, SeasonChapterQuests>> OnSeasonChapterQuestsChangeHolder;
	public event Action<Dictionary<int, SeasonChapterQuests>> OnSeasonChapterQuestsChange
	{
		add
		{
			Action<Dictionary<int, SeasonChapterQuests>> action = OnSeasonChapterQuestsChangeHolder;
			Action<Dictionary<int, SeasonChapterQuests>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Dictionary<int, SeasonChapterQuests>> action = OnSeasonChapterQuestsChangeHolder;
			Action<Dictionary<int, SeasonChapterQuests>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool IsConnectedToLobbyServer
	{
		get
		{
			bool result;
			if (m_lobbyGameClientInterface != null)
			{
				result = m_lobbyGameClientInterface.IsConnected;
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
			if (Client == null)
			{
				result = null;
			}
			else
			{
				result = Client.connection;
			}
			return result;
		}
	}

	public MyNetworkClientConnection MyConnection => (Client != null) ? (Client.connection as MyNetworkClientConnection) : null;

	public bool IsConnectedToGameServer
	{
		get
		{
			bool result;
			if (MyConnection != null)
			{
				result = MyConnection.isConnected;
			}
			else if (Connection != null)
			{
				result = Connection.isConnected;
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

	public bool IsLoading => m_loading;

	public bool IsFastForward
	{
		get
		{
			bool result;
			if (!m_withinReconnectReplay)
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

	public bool IsReconnecting => m_withinReconnect;

	public bool IsReconnectingInstantly => m_withinReconnectInstantly;

	public bool SpectatorHideAbilityTargeter { get; set; }

	public bool IsSpectator
	{
		get
		{
			bool result;
			if (PlayerInfo != null)
			{
				result = (PlayerInfo.TeamId == Team.Spectator);
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

	public GameResult GameResult => m_gameResult;

	public bool Reconnected => m_reconnected;

	public bool Observer => m_observer;

	public TierPlacement TierChangeMin { get; private set; }

	public TierPlacement TierChangeMax { get; private set; }

	public TierPlacement TierCurrent { get; private set; }

	public LobbyAlertMissionDataNotification AlertMissionsData { get; private set; }

	public Dictionary<int, SeasonChapterQuests> SeasonChapterQuests { get; private set; }

	public bool IsCharacterInFreeRotation(CharacterType characterType, GameType gameType)
	{
		bool result;
		if (ClientAccessLevel < ClientAccessLevel.Full)
		{
			result = (m_loadedPlayerAccountData.AccountComponent.IsCharacterInFreeRotation(characterType) || IsFreelancerInFreeRotationExtension(characterType, gameType));
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
		PersistedCharacterData playerCharacterData = GetPlayerCharacterData(characterType);
		if (ClientAccessLevel < ClientAccessLevel.Full && !characterType.IsWillFill())
		{
			if (gameType != GameType.Practice)
			{
				if (playerCharacterData != null && playerCharacterData.CharacterComponent.Unlocked)
				{
					return true;
				}
				return IsCharacterInFreeRotation(characterType, gameType);
			}
		}
		return true;
	}

	private List<CharacterVisualInfo> WaitingForSkinSelectResponse { get; set; }

	private FactionLoginRewardNotification LoginRewardNotification { get; set; }

	private List<QuestCompleteNotification> LoginQuestCompleteNotifications { get; set; }

	public DateTime QueueEntryTime => OurQueueEntryTime;

	public bool IsWaitingForSkinResponse()
	{
		return WaitingForSkinSelectResponse.Count > 0;
	}

	public void HandleQueueConfirmation()
	{
		OurQueueEntryTime = DateTime.UtcNow;
	}

	private void OnApplicationQuit()
	{
		ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
		if (clientExceptionDetector != null)
		{
			clientExceptionDetector.FlushErrorsToLobby();
		}
		LeaveGame(false, GameResult.ClientShutDown);
		DiscordClientInterface.Shutdown();
	}

	public List<LobbyGameInfo> CustomGameInfos { get; private set; }

	public LobbyPlayerGroupInfo GroupInfo { get; private set; }

	public EnvironmentType EnvironmentType { get; private set; }

	public ServerMessageOverrides ServerMessageOverrides { get; private set; }

	public ServerLockState ServerLockState { get; private set; }

	public ConnectionQueueInfo ConnectionQueueInfo { get; private set; }

	public AuthTicket AuthTicket => HydrogenConfig.Get().Ticket;

	public ClientAccessLevel ClientAccessLevel => (PlayerInfo == null) ? m_clientAccessLevel : PlayerInfo.EffectiveClientAccessLevel;

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
			if (ClientAccessLevel == ClientAccessLevel.Queued)
			{
				result = (ConnectionQueueInfo != null);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public bool IsServerLocked => ClientAccessLevel == ClientAccessLevel.Locked;

	public bool HasDeveloperAccess()
	{
		if (AuthTicket != null)
		{
			if (AuthTicket.HasEntitlement("DEVELOPER_ACCESS"))
			{
				return true;
			}
		}
		return ClientAccessLevel >= ClientAccessLevel.Admin;
	}

	public bool PurchasingMod { get; private set; }

	public int ModAttemptingToPurchase { get; private set; }

	private void Awake()
	{
		s_instance = this;
		WaitingForCardSelectResponse = -1;
		WaitingForModSelectResponse = -1;
		m_loadLevelOperationBundleSceneNames = new List<KeyValuePair<string, string>>();
		m_loadingCharacterResources = new List<CharacterResourceLink>();
		m_loading = false;
		m_loadingCharacterAssets = false;
		m_assetsLoadingState = new AssetsLoadingState();
		m_lastReceivedMsgSeqNum = 0U;
		m_lastSentMsgSeqNum = 0U;
		m_replay = new Replay();
		m_taskScheduler = new Scheduler();
		m_clientPerformanceCollectTask = delegate
		{
			SendPerformanceReport();
		};
		AllowBadges = false;
		ClearLobbyState();
		LoginQuestCompleteNotifications = new List<QuestCompleteNotification>();
		WaitingForSkinSelectResponse = new List<CharacterVisualInfo>();
	}

	private void Start()
	{
		GameManager.Get().OnGameStopped += HandleGameStopped;
		GameManager.Get().OnGameLaunched += HandleGameLaunched;
		GameManager.Get().OnGameStatusChanged += HandleGameStatusChanged;
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnConnected = (Action<bool>)Delegate.Combine(discordClientInterface.OnConnected, new Action<bool>(HandleDiscordConnected));
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(HandleDiscordDisconnected));
		DiscordClientInterface discordClientInterface3 = DiscordClientInterface.Get();
		discordClientInterface3.OnAuthorized = (Action<string>)Delegate.Combine(discordClientInterface3.OnAuthorized, new Action<string>(HandleDiscordAuthorized));
		DiscordClientInterface discordClientInterface4 = DiscordClientInterface.Get();
		discordClientInterface4.OnAuthenticated = (Action<DiscordUserInfo>)Delegate.Combine(discordClientInterface4.OnAuthenticated, new Action<DiscordUserInfo>(HandleDiscordAuthenticated));
		DiscordClientInterface discordClientInterface5 = DiscordClientInterface.Get();
		discordClientInterface5.OnJoined = (Action)Delegate.Combine(discordClientInterface5.OnJoined, new Action(HandleDiscordJoined));
		DiscordClientInterface discordClientInterface6 = DiscordClientInterface.Get();
		discordClientInterface6.OnLeft = (Action)Delegate.Combine(discordClientInterface6.OnLeft, new Action(HandleDiscordLeft));
		VisualsLoader.OnLoading += HandleVisualsSceneLoading;
		MyNetworkClientConnection.OnSending = (Action<UNetMessage>)Delegate.Combine(MyNetworkClientConnection.OnSending, new Action<UNetMessage>(HandleGameMessageSending));
	}

	private void Update()
	{
		bool flag;
		if (MyConnection != null)
		{
			flag = (PlayerInfo != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
		{
			MyConnection.Update();
		}
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.Update();
		}
		CheckLoaded();
		GroupJoinManager.Get().Update();
		if (!DisplayedMOTDPopup)
		{
			if (HydrogenConfig.Get() != null)
			{
				if (ServerMessageOverrides != null)
				{
					if (ServerMessageOverrides.MOTDPopUpText != null)
					{
						if (UIDialogPopupManager.Get() != null)
						{
							string value = ServerMessageOverrides.MOTDPopUpText.GetValue(HydrogenConfig.Get().Language);
							if (!value.IsNullOrEmpty())
							{
								UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("MessageOfTheDay", "Global"), value, StringUtil.TR("Ok", "Global"));
								DisplayedMOTDPopup = true;
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
			if (LoginRewardNotification != null)
			{
				UINewReward.Get().NotifyNewTrustReward(
					LoginRewardNotification.LogInRewardsGiven,
					-1, 
					string.Empty,
					true);
				LoginRewardNotification = null;
			}
			if (LoginQuestCompleteNotifications.Count > 0)
			{
				foreach (QuestCompleteNotification obj in LoginQuestCompleteNotifications)
				{
					OnQuestCompleteNotificationHolder(obj);
				}
				LoginQuestCompleteNotifications.Clear();
			}
		}
	}

	private void ClearLobbyState()
	{
		IsRegistered = false;
		IsReady = false;
		AllowRelogin = true;
		FriendList = new FriendList();
		IsFriendListInitialized = false;
		CustomGameInfos = new List<LobbyGameInfo>();
		GroupInfo = null;
		m_clientAccessLevel = ClientAccessLevel.Unknown;
		HasPurchasedGame = false;
		ServerLockState = ServerLockState.Unknown;
		ConnectionQueueInfo = new ConnectionQueueInfo();
		CommerceURL = string.Empty;
		ServerMessageOverrides = new ServerMessageOverrides
		{
			MOTDText = string.Empty,
			MOTDPopUpText = string.Empty,
			ReleaseNotesText = string.Empty,
			ReleaseNotesHeader = string.Empty,
			ReleaseNotesDescription = string.Empty,
			WhatsNewText = string.Empty,
			WhatsNewHeader = string.Empty,
			WhatsNewDescription = string.Empty,
			LockScreenText = string.Empty,
			LockScreenButtonText = string.Empty,
			FreeUpsellExternalBrowserUrl = string.Empty,
			FreeUpsellExternalBrowserSteamUrl = string.Empty
		};
		m_discordConnecting = false;
		m_discordJoinSuggested = false;
	}

	public void SetSoloSubGameMask(GameType gameType, ushort subMask)
	{
		SoloSubTypeMask[gameType] = subMask;
	}

	public ushort GetSoloSubGameMask(GameType gameType)
	{
		if (!SoloSubTypeMask.ContainsKey(gameType))
		{
			Dictionary<ushort, GameSubType> gameTypeSubTypes = GetGameTypeSubTypes(gameType);
			if (gameTypeSubTypes != null)
			{
				ushort num = HydrogenConfig.Get().GetSavedSubTypes(gameType, gameTypeSubTypes);
				if (num == 0)
				{
					foreach (KeyValuePair<ushort, GameSubType> keyValuePair in gameTypeSubTypes)
					{
						if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
						{
							num = keyValuePair.Key;
							break;
						}
						if (!keyValuePair.Value.HasMod(GameSubType.SubTypeMods.NotCheckedByDefault))
						{
							num |= keyValuePair.Key;
						}
					}
				}
				SoloSubTypeMask[gameType] = num;
			}
			else
			{
				SoloSubTypeMask[gameType] = 0;
				Log.Error("Unable to find sub types for game {0}", gameType);
			}
		}
		return SoloSubTypeMask[gameType];
	}

	public void ConnectToLobbyServer()
	{
		if (m_lobbyGameClientInterface != null)
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
					hydrogenConfig.Ticket = AuthTicket.Load(hydrogenConfig.TicketFile);
					File.Delete(hydrogenConfig.TicketFile);
					hydrogenConfig.TicketFile = null;
				}
				if (hydrogenConfig.Ticket == null)
				{
					if (hydrogenConfig.PlatformConfig.AllowRequestTickets
					    && !hydrogenConfig.PlatformUserName.IsNullOrEmpty()
					    && !hydrogenConfig.PlatformPassword.IsNullOrEmpty())
					{
						hydrogenConfig.Ticket = AuthTicket.CreateRequestTicket(
							hydrogenConfig.PlatformUserName,
							hydrogenConfig.PlatformPassword,
							"Client");
					}
					else if (hydrogenConfig.PlatformConfig.AllowFakeTickets)
					{
						hydrogenConfig.Ticket = AuthTicket.CreateFakeTicket(
							hydrogenConfig.SystemUserName,
							"Client",
							0,
							"ADMIN_ACCESS;GAME_OWNERSHIP");
					}
				}
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
		Log.Info("Connecting to lobby server from {0} as {1} / {2} [{3}]", 
			hydrogenConfig.HostName,
			hydrogenConfig.Ticket.UserName,
			hydrogenConfig.Ticket.Handle,
			hydrogenConfig.Ticket.AccountId);
		ClearLobbyState();
		Region region = Options_UI.GetRegion();
		m_lobbyGameClientInterface = new LobbyGameClientInterface();
		m_lobbyGameClientInterface.Initialize(
			hydrogenConfig.DirectoryServerAddress,
			hydrogenConfig.Ticket,
			region,
			hydrogenConfig.Language,
			hydrogenConfig.ProcessType,
			hydrogenConfig.PreferredLobbyServerIndex);
		m_lobbyGameClientInterface.OnConnected += HandleConnectedToLobbyServer;
		m_lobbyGameClientInterface.OnDisconnected += HandleDisconnectedFromLobbyServer;
		m_lobbyGameClientInterface.OnLobbyServerReadyNotification += HandleLobbyServerReadyNotification;
		m_lobbyGameClientInterface.OnLobbyStatusNotification += HandleLobbyStatusNotification;
		m_lobbyGameClientInterface.OnLobbyGameplayOverridesNotification += HandleLobbyGameplayOverridesNotification;
		m_lobbyGameClientInterface.OnLobbyCustomGamesNotification += HandleLobbyCustomGamesNotification;
		m_lobbyGameClientInterface.OnQueueStatusNotification += HandleQueueStatusNotification;
		m_lobbyGameClientInterface.OnQueueAssignmentNotification += HandleQueueAssignmentNotification;
		m_lobbyGameClientInterface.OnGameAssignmentNotification += HandleGameAssignmentNotification;
		m_lobbyGameClientInterface.OnGameInfoNotification += HandleGameInfoNotification;
		m_lobbyGameClientInterface.OnGameStatusNotification += HandleGameStatusNotification;
		m_lobbyGameClientInterface.OnForcedCharacterChangeFromServerNotification += HandleForcedCharacterChangeFromServerNotification;
		m_lobbyGameClientInterface.OnCharacterDataUpdateNotification += HandleCharacterDataUpdateNotification;
		m_lobbyGameClientInterface.OnInventoryComponentUpdateNotification += HandleInventoryComponentUpdateNotification;
		m_lobbyGameClientInterface.OnAccountDataUpdated += HandleAccountDataUpdateNotification;
		m_lobbyGameClientInterface.OnBankBalanceChangeNotification += HandleBankBalanceChangeNotification;
		m_lobbyGameClientInterface.OnSeasonStatusNotification += HandleSeasonStatusNotification;
		m_lobbyGameClientInterface.OnChapterStatusNotification += HandleChapterStatusNotification;
		m_lobbyGameClientInterface.OnChatNotification += HandleChatNotification;
		m_lobbyGameClientInterface.OnUseOverconNotification += HandleUseOverconNotification;
		m_lobbyGameClientInterface.OnUseGGPackNotification += HandleGGPackUsedNotification;
		m_lobbyGameClientInterface.OnGroupUpdateNotification += HandleGroupUpdateNotification;
		m_lobbyGameClientInterface.OnFriendStatusNotification += HandleFriendStatusNotification;
		m_lobbyGameClientInterface.OnGroupConfirmation += HandleGroupConfirmationRequest;
		m_lobbyGameClientInterface.OnGroupSuggestion += HandleGroupSuggestionRequest;
		m_lobbyGameClientInterface.OnForceQueueNotification += HandleForceQueueNotification;
		m_lobbyGameClientInterface.OnGameInviteConfirmationRequest += HandleGameInviteConfirmationRequest;
		m_lobbyGameClientInterface.OnQuestCompleteNotification += HandleQuestCompleteNotification;
		m_lobbyGameClientInterface.OnMatchResultsNotification += HandleMatchResultsNotification;
		m_lobbyGameClientInterface.OnServerQueueConfigurationUpdateNotification += HandleServerQueueConfigurationUpdateNotification;
		m_lobbyGameClientInterface.OnRankedOverviewChangeNotification += HandleRankedOverviewChangeNotification;
		m_lobbyGameClientInterface.OnFactionCompetitionNotification += HandleFactionCompetitionNotification;
		m_lobbyGameClientInterface.OnTrustBoostUsedNotification += HandleTrustBoostUsedNotification;
		m_lobbyGameClientInterface.OnFacebookAccessTokenNotification += HandleFacebookAccessTokenNotification;
		m_lobbyGameClientInterface.OnPlayerFactionContributionChange += HandlePlayerFactionContributionChange;
		m_lobbyGameClientInterface.OnFactionLoginRewardNotification += HandleFactionLoginRewardNotification;
		m_lobbyGameClientInterface.OnLobbyAlertMissionDataNotification += HandleLobbyAlertMissionDataNotification;
		m_lobbyGameClientInterface.OnLobbySeasonQuestDataNotification += HandleLobbySeasonQuestDataNotification;
		m_lobbyGameClientInterface.IsCompressed = hydrogenConfig.WebSocketIsCompressed;
		m_lobbyGameClientInterface.IsBinary = hydrogenConfig.WebSocketIsBinary;
		m_lobbyGameClientInterface.HttpPostHandler = HttpPost;
		m_lobbyGameClientInterface.Connect();
	}

	public void DisconnectFromLobbyServer()
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.Disconnect();
			m_lobbyGameClientInterface = null;
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
			GameManager.Get().StopGame();
		}
		NetworkManager.singleton.StopClient();
		DisconnectFromLobbyServer();
		if (GameManager.Get() != null)
		{
			GameManager.Get().OnGameStopped -= HandleGameStopped;
			GameManager.Get().OnGameLaunched -= HandleGameLaunched;
			GameManager.Get().OnGameStatusChanged -= HandleGameStatusChanged;
		}
		MyNetworkManager myNetworkManager = MyNetworkManager.Get();
		if (myNetworkManager != null)
		{
			myNetworkManager.m_OnClientConnect -= HandleNetworkConnect;
			myNetworkManager.m_OnClientDisconnect -= HandleNetworkDisconnect;
			myNetworkManager.m_OnClientError -= HandleNetworkError;
		}
		if (Client != null)
		{
			Client.UnregisterHandler(0x34);
		}
		SinglePlayerManager.UnregisterSpawnHandler();
		s_instance = null;
	}

	public void SubscribeToCustomGames()
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.SubscribeToCustomGames();
		}
	}

	public void UnsubscribeFromCustomGames()
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.UnsubscribeFromCustomGames();
		}
	}

	public void JoinQueue(
		GameType gameType,
		BotDifficulty? allyDifficulty,
		BotDifficulty? enemyDifficulty,
		Action<JoinMatchmakingQueueResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			BotDifficulty allyBotDifficulty = allyDifficulty ?? BotDifficulty.Hard;
			BotDifficulty enemyBotDifficulty = enemyDifficulty ?? BotDifficulty.Easy;
			m_lobbyGameClientInterface.JoinQueue(gameType, allyBotDifficulty, enemyBotDifficulty, onResponseCallback);
		}
		else
		{
			onResponseCallback(new JoinMatchmakingQueueResponse
			{
				Success = false,
				ErrorMessage = "Not connected to Lobby.\nPlease restart client."
			});
		}
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.LeaveQueue(onResponseCallback);
		}
	}

	public void CreateGame(
		LobbyGameConfig gameConfig,
		ReadyState readyState,
		BotDifficulty selectedBotSkillTeamA,
		BotDifficulty selectedBotSkillTeamB,
		Action<CreateGameResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		string processCode = null;
		if (gameConfig.InstanceSubTypeBit == 0)
		{
			Dictionary<ushort, GameSubType> gameTypeSubTypes = GetGameTypeSubTypes(gameConfig.GameType);
			if (!gameTypeSubTypes.IsNullOrEmpty())
			{
				if (gameTypeSubTypes.Count == 1)
				{
					gameConfig.InstanceSubTypeBit = gameTypeSubTypes.First().Key;
					Log.Warning("CreateGame() called without setting InstanceSubTypeIndex. " +
					            "Forcing it to use the only viable one " +
					            $"({gameConfig.GameType}: 0x{gameConfig.InstanceSubTypeBit:x4}: {gameTypeSubTypes.First().Value.GetNameAsPayload()}), " +
					            "but the calling code should consult all possible choices, " +
					            "because although it might currently be configured to only have one choice, " +
					            "that list can be changed dynamically on a running server to be any length.");
					m_lobbyGameClientInterface.CreateGame(gameConfig,
						readyState,
						processCode,
						onResponseCallback,
						selectedBotSkillTeamA,
						selectedBotSkillTeamB);
				}
				else
				{
					List<KeyValuePair<ushort, GameSubType>> pstAsList = gameTypeSubTypes.ToList();
					UIDialogPopupManager.OpenTwoButtonDialog(
						"Brutal Hack",
						"TODO: The calling code did not pick a sub-type for this game type. Please chose:",
						StringUtil.TR(pstAsList[0].Value.LocalizedName),
						StringUtil.TR(pstAsList[1].Value.LocalizedName),
						delegate
					{
						gameConfig.InstanceSubTypeBit = pstAsList[0].Key;
						m_lobbyGameClientInterface.CreateGame(
							gameConfig,
							readyState,
							processCode,
							onResponseCallback,
							selectedBotSkillTeamA,
							selectedBotSkillTeamB);
					}, delegate
					{
						gameConfig.InstanceSubTypeBit = pstAsList[1].Key;
						m_lobbyGameClientInterface.CreateGame(
							gameConfig,
							readyState,
							processCode,
							onResponseCallback,
							selectedBotSkillTeamA,
							selectedBotSkillTeamB);
					});
				}
			}
			else
			{
				Log.Warning($"Huh, why do we not know about the sub-types of game type {gameConfig.GameType}?");
				gameConfig.InstanceSubTypeBit = 1;
				m_lobbyGameClientInterface.CreateGame(
					gameConfig,
					readyState,
					processCode,
					onResponseCallback,
					selectedBotSkillTeamA,
					selectedBotSkillTeamB);
			}
		}
		else
		{
			m_lobbyGameClientInterface.CreateGame(
				gameConfig,
				readyState,
				processCode,
				onResponseCallback,
				selectedBotSkillTeamA,
				selectedBotSkillTeamB);
		}
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.JoinGame(gameInfo, asSpectator, onResponseCallback);
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
			Log.Info("Leaving replay");
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
					TextConsole.Get().Write($"Failed to leave game: {response.ErrorMessage}");
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
					Log.Error("Failed to send LeaveGameNotification");
				}
				Client.Disconnect();
			}
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, MatchFreelancerStats stats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, null, stats, onResponseCallback);
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, PersistedStats stats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, stats, null, onResponseCallback);
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, null, null, onResponseCallback);
		}
	}

	public void UpdateReadyState(ReadyState readyState, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, Action<PlayerInfoUpdateResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
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
				m_lobbyGameClientInterface.UpdateGameCheats(gameOptionFlag, playerGameOptionFlag);
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
			lobbyPlayerInfoUpdate.ContextualReadyState = new ContextualReadyState
			{
				ReadyState = readyState,
				GameProcessCode = ((GameInfo == null) ? null : GameInfo.GameServerProcessCode)
			};
			if (allyDifficulty != null)
			{
				lobbyPlayerInfoUpdate.AllyDifficulty = allyDifficulty.Value;
			}
			if (enemyDifficulty != null)
			{
				lobbyPlayerInfoUpdate.EnemyDifficulty = enemyDifficulty.Value;
			}
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, delegate(PlayerInfoUpdateResponse response)
			{
				if (!response.Success)
				{
					if (shouldResetOnFalure && currentReadyState != readyState && AppState_CharacterSelect.Get() == AppState.GetCurrent())
					{
						if (playerInfo != null)
						{
							playerInfo.ReadyState = currentReadyState;
							Log.Warning("Failure to ready, resetting ready state");
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
					});
				}
			});
		}
		else
		{
			Log.Warning("m_lobbyGameClientInterface == null");
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
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.UpdateGroupGameType(gametype, delegate(PlayerGroupInfoUpdateResponse response)
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
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingFailure", "Global"), description, StringUtil.TR("Ok", "Global"));
				}
			});
		}
	}

	public void UpdateSelectedCharacter(CharacterType character, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterType = character;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleCharacterSelectUpdateResponse);
		}
	}

	private void RecordFailureInCharacterSelectUpdateResponse(PlayerInfoUpdateResponse response, string memberName)
	{
		if (response.LocalizedFailure != null)
		{
			TextConsole.Get().Write(new TextConsole.Message
			{
				Text = string.Format(StringUtil.TR("FailedMessage", "Global"), response.LocalizedFailure),
				MessageType = ConsoleMessageType.SystemMessage
			});
			Log.Error("Lobby Server Error ({0}): {1}", memberName, response.LocalizedFailure.ToString());
		}
		else
		{
			Log.Error("Lobby Server Error ({0}): {1}", memberName, response.ErrorMessage);
		}
	}

	public void HandleCharacterSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleCharacterSelectUpdateResponse");
			return;
		}
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().CharacterSelectionResponseHandler(response);
		}
		if (Get().GroupInfo != null)
		{
			if (response.CharacterInfo != null)
			{
				Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, true);
			}
		}
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent() && response.CharacterInfo != null)
		{
			UICharacterSelectScreenController.Get().NotifyGroupUpdate();
		}
	}

	public void UpdateLoadouts(List<CharacterLoadout> loadouts, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			CharacterLoadoutUpdate value;
			value.CharacterLoadoutChanges = loadouts;
			if (!loadouts.IsNullOrEmpty())
			{
				lobbyPlayerInfoUpdate.RankedLoadoutMods = (loadouts[0].Strictness == ModStrictness.Ranked);
			}
			else
			{
				Log.Error("Client attempting to update invalid loadouts");
			}
			lobbyPlayerInfoUpdate.CharacterLoadoutChanges = value;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleLoadoutUpdateResponse);
		}
	}

	public void RequestLoadouts(bool ranked)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = 0;
			lobbyPlayerInfoUpdate.RankedLoadoutMods = ranked;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleLoadoutUpdateResponse);
		}
	}

	public void HandleLoadoutUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleLoadoutUpdateResponse");
			return;
		}
		if (response.CharacterInfo.CharacterLoadouts.Count > 0 && response.CharacterInfo.CharacterLoadouts[0].Strictness == ModStrictness.Ranked)
		{
			GetPlayerCharacterData(GetPlayerAccountData().AccountComponent.LastCharacter).CharacterComponent.CharacterLoadoutsRanked = response.CharacterInfo.CharacterLoadouts;
		}
		else
		{
			GetPlayerCharacterData(GetPlayerAccountData().AccountComponent.LastCharacter).CharacterComponent.CharacterLoadouts = response.CharacterInfo.CharacterLoadouts;
		}
		UICharacterSelectCharacterSettingsPanel.Get().NotifyLoadoutUpdate(response);
		if (UIRankedCharacterSelectSettingsPanel.Get() != null)
		{
			UIRankedCharacterSelectSettingsPanel.Get().NotifyLoadoutUpdate(response);
		}
	}

	public void UpdateSelectedSkin(CharacterVisualInfo selectedCharacterSkin, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterSkin = selectedCharacterSkin;
			WaitingForSkinSelectResponse.Add(selectedCharacterSkin);
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleSkinSelectUpdateResponse);
		}
	}

	public void HandleSkinSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleSkinSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null && Get().GroupInfo != null)
		{
			GetAllPlayerCharacterData()[response.CharacterInfo.CharacterType].CharacterComponent.LastSkin = response.CharacterInfo.CharacterSkin;
			Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, true);
			if (WaitingForSkinSelectResponse.Count > 0)
			{
				if (WaitingForSkinSelectResponse[0].Equals(response.CharacterInfo.CharacterSkin))
				{
					WaitingForSkinSelectResponse.RemoveAt(0);
					return;
				}
			}
			if (UICharacterSelectWorldObjects.Get() != null)
			{
				UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(response.CharacterInfo.CharacterType, 0, string.Empty, response.CharacterInfo.CharacterSkin, false);
			}
			else
			{
				Log.Warning("Handling skin selection update response when character select is not present");
			}
		}
	}

	public void UpdateSelectedCards(CharacterCardInfo cards, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterCards = cards;
			WaitingForCardSelectResponse = m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleCardSelectUpdateResponse);
		}
	}

	public void ClearWaitingForCardResponse()
	{
		WaitingForCardSelectResponse = -1;
	}

	public void HandleCardSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (WaitingForCardSelectResponse == response.ResponseId)
		{
			ClearWaitingForCardResponse();
		}
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleCardSelectUpdateResponse");
			return;
		}
		if (Get().GroupInfo != null && response.CharacterInfo != null)
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
				UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
			}
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				UIRankedCharacterSelectSettingsPanel.Get().Refresh();
			}
		}
		IL_11D:
		if (response.CharacterInfo != null)
		{
			Get().GroupInfo.SetCharacterInfo(response.CharacterInfo);
			PersistedCharacterData playerCharacterData = GetPlayerCharacterData(response.CharacterInfo.CharacterType);
			if (playerCharacterData != null)
			{
				playerCharacterData.CharacterComponent.LastCards = response.CharacterInfo.CharacterCards;
			}
		}
	}

	public void ClearWaitingForModResponse()
	{
		WaitingForModSelectResponse = -1;
	}

	public void UpdateSelectedMods(CharacterModInfo mods, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterMods = mods;
			lobbyPlayerInfoUpdate.RankedLoadoutMods = (AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked);
			WaitingForModSelectResponse = m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleModSelectUpdateResponse);
		}
	}

	public void UpdateSelectedAbilityVfxSwaps(CharacterAbilityVfxSwapInfo swaps, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterAbilityVfxSwaps = swaps;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleAbilityVfxSwapSelectUpdateResponse);
		}
	}

	public void HandleModSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (WaitingForModSelectResponse == response.ResponseId)
		{
			ClearWaitingForModResponse();
		}
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleModSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null)
		{
			PersistedCharacterData playerCharacterData = Get().GetPlayerCharacterData(response.CharacterInfo.CharacterType);
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
			if (Get().GroupInfo != null)
			{
				Get().GroupInfo.SetCharacterInfo(response.CharacterInfo);
			}
			UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
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
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleAbilityVfxSwapSelectUpdateResponse");
			return;
		}
		if (response.CharacterInfo != null)
		{
			Get().GetPlayerCharacterData(response.CharacterInfo.CharacterType).CharacterComponent.LastAbilityVfxSwaps = response.CharacterInfo.CharacterAbilityVfxSwaps;
			if (Get().GroupInfo != null)
			{
				Get().GroupInfo.SetCharacterInfo(response.CharacterInfo);
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
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.AllyDifficulty = allyDifficulty;
			lobbyPlayerInfoUpdate.EnemyDifficulty = enemyDifficulty;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate);
		}
	}

	public void SendSetRegionRequest(Region region)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendSetRegionRequest(region);
		}
	}

	public void SendRankedTradeRequest_AcceptOrOffer(CharacterType desiredCharacter)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedTradeRequest(desiredCharacter, RankedTradeData.TradeActionType._001D);
		}
	}

	public void SendRankedTradeRequest_Reject(CharacterType desiredCharacter)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedTradeRequest(desiredCharacter, RankedTradeData.TradeActionType._000E);
		}
	}

	public void SendRankedTradeRequest_StopTrading()
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedTradeRequest(CharacterType.None, RankedTradeData.TradeActionType._0012);
		}
	}

	public void SendRankedBanRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedBanRequest(type);
		}
	}

	public void SendRankedSelectRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedSelectionRequest(type);
		}
	}

	public void SendRankedHoverClickRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedHoverClickRequest(type);
		}
	}

	public void UpdateGameInfo(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo)
	{
		UpdateGameInfo(new LobbyGameInfo
		{
			GameConfig = gameConfig
		}, teamInfo);
	}

	public void UpdateGameInfo(LobbyGameInfo gameInfo, LobbyTeamInfo teamInfo)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.UpdateGameInfo(gameInfo, teamInfo);
		}
	}

	public void InvitePlayerToGame(string playerHandle, Action<GameInvitationResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.InvitePlayerToGame(playerHandle, onResponseCallback);
		}
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SpectateGame(playerHandle, onResponseCallback);
		}
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.RequestCrashReportArchiveName(numArchiveBytes, onResponseCallback);
		}
		return false;
	}

	public bool SendStatusReport(ClientStatusReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendStatusReport(report);
		}
		return false;
	}

	public bool SendErrorReport(ClientErrorReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendErrorReport(report);
		}
		return false;
	}

	public bool SendErrorSummary(ClientErrorSummary summary)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendErrorSummary(summary);
		}
		return false;
	}

	public bool SendFeedbackReport(ClientFeedbackReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendFeedbackReport(report);
		}
		return false;
	}

	public bool SendPerformanceReport()
	{
		if (m_lobbyGameClientInterface != null)
		{
			ClientPerformanceReport clientPerformanceReport = new ClientPerformanceReport();
			clientPerformanceReport.PerformanceInfo = ClientPerformanceCollector.Get().Collect();
			return m_lobbyGameClientInterface.SendPerformanceReport(clientPerformanceReport);
		}
		return false;
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		return m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.SendChatNotification(recipientHandle, messageType, text);
	}

	public void SendUseOverconRequest(int id, string overconName, int actorId, int turn)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendUseOverconRequest(id, overconName, actorId, turn);
		}
	}

	public void SendSetDevTagRequest(bool active, Action<SetDevTagResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendSetDevTagRequest(active, onResponseCallback);
		}
	}

	public bool SendUIActionNotification(string context)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendUIActionNotification(context);
		}
		return false;
	}

	private void HandleLobbyCustomGamesNotification(LobbyCustomGamesNotification notification)
	{
		CustomGameInfos = notification.CustomGameInfos;
		OnLobbyCustomGamesNotificationHolder(notification);
	}

	private void HandleGroupUpdateNotification(GroupUpdateNotification notification)
	{
		bool flag = false;
		bool flag2 = notification.Members.Count > 0;
		if (GroupInfo.InAGroup != flag2)
		{
			GroupInfo.InAGroup = flag2;
			if (UIFrontEnd.Get() != null)
			{
				UIFrontEnd.Get().m_frontEndChatConsole.ChangeChatRoom();
			}
		}
		GroupInfo.Members = notification.Members;
		GroupInfo.SelectedQueueType = notification.GameType;
		GroupInfo.SubTypeMask = notification.SubTypeMask;
		if (!GroupInfo.InAGroup)
		{
			GroupInfo.IsLeader = false;
		}
		else if (GroupInfo.InAGroup)
		{
			for (int i = 0; i < notification.Members.Count; i++)
			{
				if (notification.Members[i].IsLeader)
				{
					flag = (notification.Members[i].AccountID == GetPlayerAccountData().AccountId);
					break;
				}
			}
			GroupInfo.IsLeader = flag;
			if (UICharacterScreen.Get() != null)
			{
				if (notification.GameType == GameType.Coop)
				{
					bool value = false;
					UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
					if (AppState.GetCurrent() == AppState_CharacterSelect.Get() || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
					{
						characterSelectSceneStateParameters.AllyBotTeammatesClickable = !flag;
						Dictionary<ushort, GameSubType> gameTypeSubTypes = GetGameTypeSubTypes(GameType.Coop);
						IEnumerable<ushort> enumerable = from p in gameTypeSubTypes.Keys
														 where (p & notification.SubTypeMask) != 0
														 select p;
						if (!enumerable.IsNullOrEmpty())
						{
							GameSubType gameSubType;
							if (gameTypeSubTypes.TryGetValue(enumerable.First(), out gameSubType) && gameSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
							{
								value = true;
							}
						}
					}
					characterSelectSceneStateParameters.SelectedEnemyBotDifficulty = (int)notification.EnemyDifficulty;
					characterSelectSceneStateParameters.SelectedAllyBotDifficulty = (int)notification.AllyDifficulty;
					characterSelectSceneStateParameters.AllyBotTeammatesSelected = value;
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
				if (GroupInfo.SelectedQueueType == GameType.Coop)
				{
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						AllyBotTeammatesSelected = false
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
				if (GroupInfo.InAGroup)
				{
					bool enableAutoJoinDiscord = Options_UI.Get().GetEnableAutoJoinDiscord();
					if (enableAutoJoinDiscord)
					{
						JoinDiscord();
					}
					else if (!m_discordJoinSuggested)
					{
						m_discordJoinSuggested = true;
						string text = string.Format(StringUtil.TR("ClickToJoinDiscordGroupChat", "Global"));
						if (!DiscordClientInterface.IsSdkEnabled)
						{
							TextConsole.Get().Write(text);
						}
					}
				}
				else
				{
					LeaveDiscord();
				}
			}
		}
		OnGroupUpdateNotificationHolder();
	}

	private void HandleGGPackUsedNotification(UseGGPackNotification notification)
	{
		OnUseGGPackNotificationHolder(notification);
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		OnChatNotificationHolder(notification);
	}

	private void HandleOnSetDevTagNotification(SetDevTagResponse response)
	{
		OnSetDevTagResponseHolder(response);
	}

	private void HandleUseOverconNotification(UseOverconResponse notification)
	{
		OnUseOverconNotificationHolder(notification);
	}

	public void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		IsFriendListInitialized = true;
		if (notification.FriendList.IsDelta)
		{
			using (Dictionary<long, FriendInfo>.Enumerator enumerator = notification.FriendList.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, FriendInfo> keyValuePair = enumerator.Current;
					if (FriendList.Friends.ContainsKey(keyValuePair.Key))
					{
						FriendList.Friends[keyValuePair.Key] = keyValuePair.Value;
						if (keyValuePair.Value.FriendStatus == FriendStatus.Removed)
						{
							FriendList.Friends.Remove(keyValuePair.Key);
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
						FriendList.Friends.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}
		else
		{
			FriendList = notification.FriendList;
		}
		OnFriendStatusNotificationHolder(notification);
	}

	private void SendGameInviteConfirmationResponse(bool accepted, GameInviteConfirmationRequest request)
	{
		GameInviteConfirmationResponse gameInviteConfirmationResponse = new GameInviteConfirmationResponse();
		gameInviteConfirmationResponse.Accepted = accepted;
		gameInviteConfirmationResponse.GameCreatorAccountId = request.GameCreatorAccountId;
		gameInviteConfirmationResponse.ResponseId = request.RequestId;
		gameInviteConfirmationResponse.InitialRequestId = request.InitialRequestId;
		Get().LobbyInterface.SendMessage(gameInviteConfirmationResponse);
	}

	private void HandleGameInviteConfirmationRequest(GameInviteConfirmationRequest request)
	{
		string description = string.Format(StringUtil.TR("InviteToCustomGame", "Global"), request.GameCreatorHandle);
		UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("GameInvite", "Global"), description, StringUtil.TR("Join", "Global"), StringUtil.TR("Reject", "Global"), delegate
		{
			SendGameInviteConfirmationResponse(true, request);
		}, delegate
		{
			SendGameInviteConfirmationResponse(false, request);
		});
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
			SendGroupSuggestion(false, false, request);
			return;
		}
		string description = string.Format(StringUtil.TR("InviteToGroupWithYou", "Global"), request.SuggesterAccountName, request.SuggestedAccountFullHandle);
		if (m_currentGroupSuggestDialogBox != null)
		{
			SendGroupSuggestion(false, true, request);
		}
		else
		{
			m_currentGroupSuggestDialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupSuggestion", "Global"), description, StringUtil.TR("Invite", "Global"), StringUtil.TR("Reject", "Global"), null, delegate
			{
				SendGroupSuggestion(true, false, request);
			}, delegate
			{
				SendGroupSuggestion(false, false, request);
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
				Log.Error("Lobby should never send us a ForceQueueNotification({0}) when we're in a {1} game!", notification.Action, gameManager.GameInfo.GameStatus);
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
		else if (Get().GroupInfo.InAGroup)
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
			InviteToGroup(request.SuggestedAccountFullHandle, delegate(GroupInviteResponse r)
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
					});
					groupSuggestionResponse2.SuggestionStatus = GroupSuggestionResponse.Status._000E;
				}
				else
				{
					groupSuggestionResponse2.SuggestionStatus = GroupSuggestionResponse.Status._0012;
				}
				m_lobbyGameClientInterface.SendMessage(groupSuggestionResponse2);
			});
		}
		else
		{
			GroupSuggestionResponse groupSuggestionResponse = new GroupSuggestionResponse();
			groupSuggestionResponse.SuggesterAccountId = request.SuggesterAccountId;
			groupSuggestionResponse.SuggestionStatus = ((!bBusy) ? GroupSuggestionResponse.Status._001D : GroupSuggestionResponse.Status._000E);
			m_lobbyGameClientInterface.SendMessage(groupSuggestionResponse);
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
			if (!IsServer())
			{
				NetworkManager.singleton.StopClient();
			}
		}
		ClientObserverManager component = GetComponent<ClientObserverManager>();
		if (component != null)
		{
			component.HandleGameStopped();
		}
		GameManager.Get().SetGameplayOverridesForCurrentGame(null);
		ResetLoadAssetsState();
		m_loadLevelOperationDone = false;
		m_loadedCharacterResourceCount = 0;
		m_spawnableObjectCount = 0;
		IsRegisteredToGameServer = false;
		m_withinReconnectReplay = false;
		m_withinReconnect = false;
		m_withinReconnectInstantly = false;
		m_lastReceivedMsgSeqNum = 0U;
		m_lastSentMsgSeqNum = 0U;
		m_replay = new Replay();
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			DisconnectFromLobbyServer();
			OnConnectedToLobbyServerHolder(response);
			if (IsConnectedToGameServer)
			{
				TextConsole.Get().Write(StringUtil.TR("FailedToConnectRetrying", "Global"));
				ConnectToLobbyServer();
			}
			else
			{
				GameManager.Get().StopGame();
			}
			return;
		}
		IsRegistered = true;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		m_lobbyGameClientInterface.HeartbeatPeriod = hydrogenConfig.HeartbeatPeriod;
		m_lobbyGameClientInterface.HeartbeatTimeout = hydrogenConfig.HeartbeatTimeout;
		m_lobbyGameClientInterface.MaxSendBufferSize = hydrogenConfig.MaxSendBufferSize;
		m_lobbyGameClientInterface.MaxWaitTime = hydrogenConfig.MaxWaitTime;
		OnConnectedToLobbyServerHolder(response);
		if (IsConnectedToGameServer)
		{
			TextConsole.Get().Write("Reconnected to lobby server");
			LobbyGameInfo previousGameInfo = GameManager.Get().GameInfo;
			RejoinGame(true, delegate(RejoinGameResponse res)
			{
				if (!res.Success)
				{
					Log.Error("{0} Failed to restore the previous game state {1}. Kicked", PlayerInfo.Handle, previousGameInfo.GameServerProcessCode);
					GameResult gameResult = GameResult.ClientKicked;
					LeaveGame(false, gameResult);
					GameManager.Get().StopGame(gameResult);
				}
			});
		}
	}

	private void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		IsReady = true;
		CommerceURL = notification.CommerceURL;
		GroupInfo = notification.GroupInfo;
		EnvironmentType = notification.EnvironmentType;
		if (GroupInfo != null)
		{
			if (GroupInfo.ChararacterInfo != null)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientSelectedVisualInfo = GroupInfo.ChararacterInfo.CharacterSkin
				});
			}
			else
			{
				GroupInfo.SetCharacterInfo(new LobbyCharacterInfo());
			}
			OnGroupUpdateNotificationHolder();
		}
		if (notification.AlertMissionData != null)
		{
			HandleLobbyAlertMissionDataNotification(notification.AlertMissionData);
		}
		if (notification.SeasonChapterQuests != null)
		{
			HandleLobbySeasonQuestDataNotification(notification.SeasonChapterQuests);
		}
		if (notification.Status != null)
		{
			HandleLobbyStatusNotification(notification.Status);
		}
		if (notification.AccountData != null)
		{
			HandleAccountDataUpdated(notification.AccountData);
			if (notification.AccountData.InventoryComponent != null)
			{
				OnInventoryComponentUpdatedHolder(notification.AccountData.InventoryComponent);
			}
			else
			{
				Log.Error("LobbyServerReadyNotification InventoryComponent is null");
			}
		}
		if (notification.CharacterDataList != null)
		{
			HandlePlayerCharacterDataUpdated(notification.CharacterDataList);
		}
		if (notification.FactionCompetitionStatus != null)
		{
			HandleFactionCompetitionNotification(notification.FactionCompetitionStatus);
		}
		OnLobbyServerReadyNotificationHolder(notification);
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		string format = "{0} ({1})";
		object arg = newLevel;
		object arg2;
		if (HasPurchasedGame)
		{
			arg2 = "purchased";
		}
		else
		{
			arg2 = "not purchased";
		}
		string text = string.Format(format, arg, arg2);
		Log.Info("Changed Access Level from {0} to {1}", oldLevel.ToString(), text);
		OnLobbyServerClientAccessLevelChangeHolder(oldLevel, newLevel);
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage, bool allowRelogin, CloseStatusCode code)
	{
		ClientPerformanceCollector.Get().ObserveRTT(null);
		m_lobbyGameClientInterface = null;
		ClearLobbyState();
		bool flag = false;
		if (IsConnectedToGameServer)
		{
			lastLobbyErrorMessage = StringUtil.TR("DisconnectedFromServer", "Disconnected");
			flag = true;
		}
		IsRegistered = false;
		IsReady = false;
		AllowRelogin = allowRelogin;
		if (flag)
		{
			TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnecting", "Disconnected"));
			ConnectToLobbyServer();
		}
		else
		{
			OnDisconnectedFromLobbyServerHolder(lastLobbyErrorMessage);
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
			Log.Info("Unassigned from queue {0}", queueInfo.GameType);
			OnQueueLeftHolder();
			if (matchmakingQueueInfo == null)
			{
				AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
			}
		}
		if (matchmakingQueueInfo != null)
		{
			Log.Info("Assigned to queue {0}", matchmakingQueueInfo.GameType);
			OnQueueEnteredHolder();
		}
		UICharacterScreen.Get().DoRefreshFunctions(0x80);
		NavigationBar.Get().UpdateStatusMessage();
		OnQueueAssignmentNotificationHolder(notification);
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.GameplayOverrides != null)
		{
			SetGameplayOverrides(notification.GameplayOverrides);
		}
		if (notification.ErrorReportRate != null)
		{
			float num = (float)notification.ErrorReportRate.Value.TotalSeconds;
			ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
			if (clientExceptionDetector != null)
			{
				if (num > 0f)
				{
					Log.Info("Will send client errors to the server every {0}.", LocalizationArg_TimeSpan.Create(notification.ErrorReportRate.Value).TR());
					clientExceptionDetector.SecondsBetweenSendingErrorPackets = num;
				}
				else
				{
					clientExceptionDetector.FlushErrorsToLobby();
					Log.Info("Will send client errors to the server immediately");
					clientExceptionDetector.SecondsBetweenSendingErrorPackets = 0f;
				}
			}
			else
			{
				Log.Warning("Failed to configure ClientExceptionDetector to use a {0} second window", num);
			}
		}
		if (notification.ServerMessageOverrides != null)
		{
			ServerMessageOverrides = notification.ServerMessageOverrides;
		}
		if (notification.ClientAccessLevel != ClientAccessLevel.Unknown)
		{
			ClientAccessLevel clientAccessLevel = ClientAccessLevel;
			bool hasPurchasedGame = HasPurchasedGame;
			int highestPurchasedGamePack = HighestPurchasedGamePack;
			m_clientAccessLevel = notification.ClientAccessLevel;
			HasPurchasedGame = notification.HasPurchasedGame;
			HighestPurchasedGamePack = notification.HighestPurchasedGamePack;
			if (clientAccessLevel == ClientAccessLevel)
			{
				if (hasPurchasedGame == HasPurchasedGame)
				{
					if (highestPurchasedGamePack == HighestPurchasedGamePack)
					{
						goto IL_19A;
					}
				}
			}
			HandleLobbyServerClientAccessLevelChange(clientAccessLevel, ClientAccessLevel);
		}
		IL_19A:
		if (notification.ServerLockState != ServerLockState.Unknown)
		{
			ServerLockState serverLockState = ServerLockState;
			ServerLockState = notification.ServerLockState;
			if (serverLockState != ServerLockState)
			{
				OnLobbyServerLockStateChangeHolder(serverLockState, ServerLockState);
			}
		}
		ConnectionQueueInfo = notification.ConnectionQueueInfo;
		if (notification.UtcNow != default(DateTime))
		{
			if (TimeOffset != notification.TimeOffset)
			{
				TextConsole.Get().Write(string.Format("Global Time Offset Is Now: {0}", notification.TimeOffset.ToString()));
			}
			ServerUtcTime = notification.UtcNow;
			ServerPacificTime = notification.PacificNow;
			ClientUtcTime = DateTime.UtcNow;
			TimeOffset = notification.TimeOffset;
		}
		OnLobbyStatusNotificationHolder(notification);
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		SetGameplayOverrides(notification.GameplayOverrides);
	}

	private void SetGameplayOverrides(LobbyGameplayOverrides gameplayOverrides)
	{
		bool enableCardsChanged = GameManager.Get() != null
		             && GameManager.Get().GameplayOverrides != null
		             && GameManager.Get().GameplayOverrides.EnableCards != gameplayOverrides.EnableCards;
		GameManager.Get().SetGameplayOverrides(gameplayOverrides);
		if (enableCardsChanged)
		{
			if (UICharacterSelectScreenController.Get() != null
			    && UICharacterSelectCharacterSettingsPanel.Get() != null)
			{
				if (!gameplayOverrides.EnableCards
				    && UICharacterSelectCharacterSettingsPanel.Get().GetTabPanel() == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts)
				{
					UICharacterSelectCharacterSettingsPanel.Get().OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins);
				}
				UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
			}
			if (UIRankedCharacterSelectSettingsPanel.Get() != null)
			{
				if (UIRankedCharacterSelectSettingsPanel.Get().GetTabPanel() == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts)
				{
					UIRankedCharacterSelectSettingsPanel.Get().OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins);
				}
				UIRankedCharacterSelectSettingsPanel.Get().Refresh();
			}
		}
		ClientPerformanceCollector clientPerformanceCollector = ClientPerformanceCollector.Get();
		if (gameplayOverrides.EnableClientPerformanceCollecting)
		{
			m_taskScheduler.AddTask(m_clientPerformanceCollectTask, gameplayOverrides.ClientPerformanceCollectingFrequency, false);
			clientPerformanceCollector.ObserveRTT(m_lobbyGameClientInterface.WebSocket);
			clientPerformanceCollector.StartCollecting();
		}
		else
		{
			m_taskScheduler.RemoveTask(m_clientPerformanceCollectTask);
			clientPerformanceCollector.ObserveRTT(null);
			clientPerformanceCollector.StopCollecting();
		}
		OnLobbyGameplayOverridesChangeHolder(gameplayOverrides);
	}

	private void HandleLobbyAlertMissionDataNotification(LobbyAlertMissionDataNotification notification)
	{
		AlertMissionsData = notification;
		OnAlertMissionDataChangeHolder(notification);
	}

	private void HandleLobbySeasonQuestDataNotification(LobbySeasonQuestDataNotification notification)
	{
		SeasonChapterQuests = notification.SeasonChapterQuests;
		OnSeasonChapterQuestsChangeHolder(notification.SeasonChapterQuests);
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
				OnQueueStatusNotificationHolder(notification);
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
		m_gameResult = notification.GameResult;
		m_reconnected = notification.Reconnection;
		m_observer = notification.Observer;
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
			Log.Info("Unassigned from game {0}", gameManager.GameInfo.Name);
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
		Log.Info("Assigned to game {0}", gameInfo2.Name);
		gameManager.SetGameplayOverridesForCurrentGame(gameplayOverrides);
		IL_1B9:
		if (!IsServer())
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
					IEnumerator<GameStatus> enumerator = gameInfo2.GameStatus.GetValues().GetEnumerator();
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
								SetGameStatus(gameStatus2);
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
				SetGameStatus(gameInfo2.GameStatus, m_gameResult);
			}
			else
			{
				SetGameStatus(GameStatus.Stopped, m_gameResult);
				ClientAccessLevel clientAccessLevel = ClientAccessLevel;
				gameManager.SetGameInfo(new LobbyGameInfo());
				gameManager.SetPlayerInfo(null);
				gameManager.SetTeamInfo(null);
				gameManager.ForbiddenDevKnowledge = null;
				if (gameManager.TeamPlayerInfo != null)
				{
					gameManager.TeamPlayerInfo.Clear();
				}
				gameManager.GameInfo.GameConfig = new LobbyGameConfig();
				if (clientAccessLevel != ClientAccessLevel)
				{
					HandleLobbyServerClientAccessLevelChange(clientAccessLevel, ClientAccessLevel);
				}
			}
		}
		if (NavigationBar.Get() != null)
		{
			NavigationBar.Get().UpdateStatusMessage();
		}
		if (m_gameResult == GameResult.Requeued)
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
				if (GroupInfo.InAGroup)
				{
					if (!m_discordConnecting)
					{
						if (!m_discordConnected)
						{
							goto IL_530;
						}
					}
					flag6 = (GetDiscordJoinType() == DiscordJoinType._000E);
					goto IL_531;
				}
				IL_530:
				flag6 = false;
				IL_531:
				if (!flag6)
				{
					LeaveDiscord();
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
							JoinDiscord();
						}
						else if (!DiscordClientInterface.IsSdkEnabled)
						{
							string text3 = string.Format(StringUtil.TR("ClickToJoinDiscordTeamChat", "Global"));
							TextConsole.Get().Write(text3);
						}
					}
				}
			}
		}
		IL_53D:
		OnGameAssignmentNotificationHolder(notification);
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = notification.GameInfo;
		LobbyPlayerInfo playerInfo = notification.PlayerInfo;
		LobbyTeamInfo teamInfo = notification.TeamInfo;
		GameType gameType = gameInfo.GameConfig.GameType;
		Log.Info("Received Game Info Notification {0} ({1})", gameInfo.Name, playerInfo.GetHandle());
		if (gameManager.GameInfo.GameServerProcessCode != gameInfo.GameServerProcessCode)
		{
			Log.Warning("Ignoring info({0}) update for game {1}, expected game {2}", notification.GameInfo.GameStatus, gameInfo.GameServerProcessCode, gameManager.GameInfo.GameServerProcessCode);
			return;
		}
		if (!IsServer())
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
						JoinDiscordChannel(userInfo);
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
					gameManager.SetTeamPlayerInfo(teamInfo.TeamInfo(playerInfo.TeamId).ToList());
					goto IL_1D7;
				}
			}
		}
		gameManager.SetTeamPlayerInfo(null);
		IL_1D7:
		if (!IsServer())
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
				SetGameStatus(gameInfo.GameStatus, gameInfo.GameResult);
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
						TierCurrent = notification.TierCurrent;
						string tierName = Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierCurrent.Tier);
						Log.Info("We are currently at tier {0} (points {1})", tierName, notification.TierCurrent.Points);
					}
				}
				if (notification.TierChangeMin != null)
				{
					if (notification.TierChangeMin.Tier != 0)
					{
						TierChangeMin = notification.TierChangeMin;
						string tierName2 = Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierChangeMin.Tier);
						Log.Info("If we lose this game we could fall to tier {0} (points {1})", tierName2, notification.TierChangeMin.Points);
					}
				}
				if (notification.TierChangeMax != null)
				{
					if (notification.TierChangeMax.Tier != 0)
					{
						TierChangeMax = notification.TierChangeMax;
						string tierName3 = Get().GetTierName(notification.GameInfo.GameConfig.GameType, notification.TierChangeMax.Tier);
						Log.Info("If we win this game we could rise to tier {0} (points {1})", tierName3, notification.TierChangeMax.Points);
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
		OnGameInfoNotificationHolder(notification);
	}

	private void HandleGameStatusNotification(GameStatusNotification notification)
	{
		Log.Info("Received Game Status Notification {0} {1}", notification.GameServerProcessCode, notification.GameStatus);
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameInfo.GameServerProcessCode != notification.GameServerProcessCode)
		{
			Log.Warning("Ignoring status({0}) update for game {1}, we believe we're in game {2}", notification.GameStatus, notification.GameServerProcessCode, gameManager.GameInfo.GameServerProcessCode);
			return;
		}
		if (!IsServer())
		{
			gameManager.GameInfo.GameStatus = notification.GameStatus;
			if (notification.GameStatus != gameManager.GameStatus)
			{
				SetGameStatus(notification.GameStatus);
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
		OnGameInfoNotificationHolder(gameInfoNotification);
	}

	private void HandleGameLaunched(GameType gameType)
	{
		ConnectToGameServer();
	}

	private void HandleGameStatusChanged(GameStatus gameStatus)
	{
	}

	private void SetGameStatus(GameStatus gameStatus, GameResult gameResult = GameResult.NoResult, bool notify = true)
	{
		GameManager.Get().SetGameStatus(gameStatus, gameResult, notify);
		if (gameStatus == GameStatus.Loaded)
		{
			WaitingForSkinSelectResponse.Clear();
		}
	}

	public void Replay_SetGameStatus(GameStatus gameStatus)
	{
		SetGameStatus(gameStatus);
	}

	private void ConnectToGameServer()
	{
		if (IsServer())
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		if (ReplayPlayManager.Get() && ReplayPlayManager.Get().IsPlayback())
		{
			ResetLoadAssetsState();
			Log.Info("Stub-connecting to replay system");
			MyNetworkManager.Get().MyStartClientStub();
		}
		else if (gameManager.GameInfo != null && !string.IsNullOrEmpty(gameManager.GameInfo.GameServerAddress))
		{
			if (!Uri.IsWellFormedUriString(gameManager.GameInfo.GameServerAddress, UriKind.Absolute))
			{
				throw new FormatException(
					$"Could not parse game server address {gameManager.GameInfo.GameServerAddress}");
			}

			ResetLoadAssetsState();
			IsRegisteredToGameServer = false;
			Log.Info("Connecting to {0}", gameManager.GameInfo.GameServerAddress);
			MyNetworkManager.Get().MyStartClient(gameManager.GameInfo.GameServerAddress, Handle);
		}
		else
		{
			Log.Error("Game server address is empty");
			return;
		}
		if (!m_registeredHandlers)
		{
			MyNetworkManager myNetworkManager = MyNetworkManager.Get();
			myNetworkManager.m_OnClientConnect += HandleNetworkConnect;
			myNetworkManager.m_OnClientDisconnect += HandleNetworkDisconnect;
			myNetworkManager.m_OnClientError += HandleNetworkError;
			m_registeredHandlers = true;
		}
		Client.RegisterHandler(0x34, HandleLoginResponse);
		Client.RegisterHandler(0x3E, HandleServerAssetsLoadingProgressUpdate);
		Client.RegisterHandler(0x36, HandleSpawningObjectsNotification);
		Client.RegisterHandler(0x30, HandleReplayManagerFile);
		Client.RegisterHandler(0x38, HandleReconnectReplayStatus);
		Client.RegisterHandler(0x44, HandleEndGameNotification);
		ClientObserverManager component = GetComponent<ClientObserverManager>();
		if (component != null)
		{
			component.ConnectingToGameServer();
		}
		SinglePlayerManager.UnregisterSpawnHandler();
		SinglePlayerManager.RegisterSpawnHandler();
		m_withinReconnectReplay = false;
		m_withinReconnect = false;
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
				Log.Info("Reconnecting to game instantly {0}", GameInfo);
				TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnectingGame", "Disconnected"));
				if (IsServer())
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
						if (MyConnection == null)
						{
							return false;
						}
						Log.Info("Reconnecting instantly to {0}", gameManager.GameInfo.GameServerAddress);
						MyConnection.Connect();
						IsRegisteredToGameServer = false;
						m_withinReconnectReplay = false;
						m_withinReconnect = false;
						m_withinReconnectInstantly = true;
						m_lastReceivedMsgSeqNum = disconnectedConnection.lastMessageIncomingSeqNum;
						m_lastSentMsgSeqNum = disconnectedConnection.lastMessageOutgoingSeqNum;
						return true;
					}
				}
				Log.Error("Game server address is empty");
				return false;
			}
		}
		return false;
	}

	public void ReloginToGameServerInstantly(MyNetworkClientConnection reconnectedConnection)
	{
		Log.Info("Relogging in to game instantly {0}", GameInfo);
		TextConsole.Get().Write(StringUtil.TR("ReconnectedGame", "Disconnected"));
		reconnectedConnection.lastMessageIncomingSeqNum = m_lastReceivedMsgSeqNum;
		reconnectedConnection.lastMessageOutgoingSeqNum = m_lastSentMsgSeqNum;
		if (m_lobbyGameClientInterface != null)
		{
			GameManager.LoginRequest loginRequest = new GameManager.LoginRequest();
			loginRequest.AccountId = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.AccountId);
			loginRequest.SessionToken = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.SessionToken);
			loginRequest.PlayerId = PlayerInfo.PlayerId;
			loginRequest.LastReceivedMsgSeqNum = m_lastReceivedMsgSeqNum;
			Client.Send(0x33, loginRequest);
		}
	}

	private void HandleGameMessageSending(UNetMessage message)
	{
		m_replay.RecordRawNetworkMessage(message.Bytes, message.NumBytes);
	}

	private void LoginToGameServer(NetworkConnection conn)
	{
		if (m_lobbyGameClientInterface != null)
		{
			GameManager gameManager = GameManager.Get();
			if (ReplayPlayManager.Get() && ReplayPlayManager.Get().IsPlayback())
			{
				Log.Info("Stub-connected to replay system", gameManager.GameInfo.GameServerAddress);
			}
			else
			{
				Log.Info("Connected to {0}", gameManager.GameInfo.GameServerAddress);
			}
			ClientScene.AddPlayer(conn, 0);
			GameManager.LoginRequest loginRequest = new GameManager.LoginRequest
			{
				AccountId = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.AccountId),
				SessionToken = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.SessionToken),
				PlayerId = gameManager.PlayerInfo.PlayerId,
				LastReceivedMsgSeqNum = m_lastReceivedMsgSeqNum
			};
			Client.Send((int) MyMsgType.LoginRequest, loginRequest);
		}
	}

	private void HandleNetworkConnect(NetworkConnection conn)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Network connect error");
			return;
		}
		LoginToGameServer(conn);
	}

	private void HandleNetworkError(NetworkConnection conn, NetworkError errorCode)
	{
	}

	private void HandleNetworkDisconnect(NetworkConnection conn)
	{
		GameResult gameResult = m_gameResult;
		if (gameResult == GameResult.NoResult && MyConnection != null)
		{
			GameResult gameResult2;
			if (MyConnection.CloseStatusCode == CloseStatusCode.PingTimeout)
			{
				gameResult2 = GameResult.ClientHeartbeatTimeoutToGameServer;
			}
			else
			{
				gameResult2 = GameResult.ClientNetworkErrorToGameServer;
			}
			gameResult = gameResult2;
		}
		Log.Info("Disconnected from game server {0}", gameResult);
		IsRegisteredToGameServer = false;
		LeaveGame(false, gameResult);
		GameManager.Get().StopGame(gameResult);
	}

	private void HandleServerAssetsLoadingProgressUpdate(NetworkMessage msg)
	{
		if (IsServer())
		{
			return;
		}
		GameManager.AssetsLoadingProgress assetsLoadingProgress = msg.ReadMessage<GameManager.AssetsLoadingProgress>();
		Log.Info($"[JSON] {{\"AssetsLoadingProgress\":{DefaultJsonSerializer.Serialize(assetsLoadingProgress)} }}");
		if (assetsLoadingProgress != null)
		{
			float loadingProgress = assetsLoadingProgress.TotalLoadingProgress / 100f;
			UILoadingScreenPanel.Get().SetLoadingProgress(assetsLoadingProgress.PlayerId, loadingProgress, false);
		}
	}

	private void HandleSpawningObjectsNotification(NetworkMessage msg)
	{
		if (IsServer())
		{
			return;
		}
		GameManager.SpawningObjectsNotification spawningObjectsNotification = msg.ReadMessage<GameManager.SpawningObjectsNotification>();
		Log.Info($"[JSON] {{\"SpawningObjectsNotification\":{DefaultJsonSerializer.Serialize(spawningObjectsNotification)} }}");
		if (spawningObjectsNotification != null)
		{
			m_spawnableObjectCount = spawningObjectsNotification.SpawnableObjectCount;
		}
	}

	public static string FormReplayFilename(DateTime time, string gameServerProcessCode, string handle)
	{
		return FormReplayFilename(time.ToString("MM_dd_yyyy__HH_mm_ss"), gameServerProcessCode, handle);
	}

	public static string FormReplayFilename(string timeStr, string gameServerProcessCode, string handle)
	{
		return string.Format("{0}__{1}__{2}__{3}.arr", timeStr, gameServerProcessCode, BuildVersion.MiniVersionString, WWW.EscapeURL(handle));
	}

	public static string RemoveTimeFromReplayFilename(string filename)
	{
		return filename.Substring(0x14);
	}

	private void HandleReplayManagerFile(NetworkMessage msg)
	{
		GameManager.ReplayManagerFile replayManagerFile = msg.ReadMessage<GameManager.ReplayManagerFile>();
		Log.Info($"[JSON] {{\"ReplayManagerFile\":{DefaultJsonSerializer.Serialize(replayManagerFile)} }}");
		if (replayManagerFile == null)
		{
			return;
		}
		if (replayManagerFile.Restart)
		{
			Log.Info("Starting replay save for {0}", GameManager.Get().GameInfo.GameServerProcessCode);
			m_replayManagerAccumulated = string.Empty;
		}
		m_replayManagerAccumulated += replayManagerFile.Fragment;
		if (replayManagerFile.Save)
		{
			try
			{
				Replay replay = JsonUtility.FromJson<Replay>(m_replayManagerAccumulated);
				replay.m_versionMini = BuildVersion.MiniVersionString;
				replay.m_versionFull = BuildVersion.FullVersionString;
				string value = JsonUtility.ToJson(replay);
				string text = Path.Combine(HydrogenConfig.Get().ReplaysPath, FormReplayFilename(DateTime.Now, GameManager.Get().GameInfo.GameServerProcessCode, Handle));
				FileInfo fileInfo = new FileInfo(text);
				fileInfo.Directory.Create();
				StreamWriter streamWriter = new StreamWriter(text);
				streamWriter.WriteLine(value);
				streamWriter.Close();
				Log.Info("Saved replay to {0}", text);
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				Log.Info("Failed to save replay");
			}
			m_replayManagerAccumulated = string.Empty;
		}
	}

	private void HandleReconnectReplayStatus(NetworkMessage msg)
	{
		GameManager.ReconnectReplayStatus reconnectReplayStatus = msg.ReadMessage<GameManager.ReconnectReplayStatus>();
		Log.Info($"[JSON] {{\"ReconnectReplayStatus\":{DefaultJsonSerializer.Serialize(reconnectReplayStatus)} }}");
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
		Debug.LogFormat(format, array);
		if (m_withinReconnectReplay != reconnectReplayStatus.WithinReconnectReplay)
		{
			m_withinReconnectReplay = reconnectReplayStatus.WithinReconnectReplay;
			if (!m_withinReconnectReplay)
			{
				m_withinReconnect = false;
			}
			GameEventManager.ReconnectReplayStateChangedArgs args = new GameEventManager.ReconnectReplayStateChangedArgs(m_withinReconnectReplay);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ReconnectReplayStateChanged, args);
		}
		ClientIdleTimer.Get().ResetIdleTimer();
	}

	private void HandleEndGameNotification(NetworkMessage msg)
	{
		GameManager.EndGameNotification endGameNotification = msg.ReadMessage<GameManager.EndGameNotification>();
		Log.Info($"[JSON] {{\"EndGameNotification\":{DefaultJsonSerializer.Serialize(endGameNotification)} }}");
		if (endGameNotification == null)
		{
			return;
		}
		GameResult gameResult = GameResult.NoResult;
		LeaveGame(true, gameResult);
		GameManager.Get().StopGame(gameResult);
	}

	private void HandleLoginResponse(NetworkMessage msg)
	{
		if (IsServer())
		{
			return;
		}
		GameManager.LoginResponse loginResponse = msg.ReadMessage<GameManager.LoginResponse>();
		Log.Info($"[JSON] {{\"LoginResponse\":{DefaultJsonSerializer.Serialize(loginResponse)} }}");
		if (loginResponse == null)
		{
			return;
		}
		if (loginResponse.Success)
		{
			m_withinReconnect = loginResponse.Reconnecting;
			IsRegisteredToGameServer = true;
			if (m_withinReconnectInstantly)
			{
				m_withinReconnectInstantly = false;
				TextConsole.Get().Write(StringUtil.TR("LoggedIntoGame", "Disconnected"));
				uint num = msg.conn.lastMessageOutgoingSeqNum - loginResponse.LastReceivedMsgSeqNum;
				if (num > 0U)
				{
					uint startSeqNum = loginResponse.LastReceivedMsgSeqNum + 1U;
					uint lastSentMsgSeqNum = m_lastSentMsgSeqNum;
					IEnumerable<Replay.Message> rawNetworkMessages = m_replay.GetRawNetworkMessages(startSeqNum, lastSentMsgSeqNum);
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
		Log.Error(text);
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
		m_loadLevelOperationBundleSceneNames.Add(new KeyValuePair<string, string>(visualsSceneName, visualsSceneName));
	}

	public void LoadAssets()
	{
		if (IsServer())
		{
			return;
		}
		m_loading = true;
		m_loadLevelOperationDone = false;
		m_loadedCharacterResourceCount = 0;
		m_spawnableObjectCount = 0;
		m_assetsLoadingState.Reset();
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
		m_loadLevelOperationBundleSceneNames.Clear();
		m_loadLevelOperationBundleSceneNames.Add(new KeyValuePair<string, string>(map, text));
		m_loadLevelOperation = new AssetBundleManager.LoadSceneAsyncOperation
		{
			sceneName = gameManager.GameConfig.Map,
			bundleName = text,
			loadSceneMode = LoadSceneMode.Single
		};
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(m_loadLevelOperation));
		StartCoroutine(LoadCharacterAssets(gameManager.GameStatus, 0.3f));
	}

	public void UnloadAssets()
	{
		foreach (KeyValuePair<string, string> keyValuePair in m_loadLevelOperationBundleSceneNames)
		{
			string key = keyValuePair.Key;
			string value = keyValuePair.Value;
			AssetBundleManager.Get().UnloadScene(key, value);
		}
		m_loadLevelOperationBundleSceneNames.Clear();
		Get().CleanupMemory();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameObjectsDestroyed, null);
	}

	private IEnumerator LoadCharacterAssets(GameStatus gameStatusForAssets, float delaySeconds)
	{
		bool loading = false;
		GameManager gameManager;
		IEnumerator<LobbyPlayerInfo> enumerator;
		m_loadingCharacterAssets = true;
		gameManager = GameManager.Get();
		enumerator = gameManager.TeamInfo.TeamAPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo teamPlayerInfo = enumerator.Current;
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(teamPlayerInfo.CharacterInfo.CharacterType);
				if (!m_loadingCharacterResources.Contains(characterResourceLink))
				{
					m_loadingCharacterResources.Add(characterResourceLink);
					characterResourceLink.LoadAsync(teamPlayerInfo.CharacterInfo.CharacterSkin, HandleCharacterResourceLoaded, gameStatusForAssets);
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
				if (!m_loadingCharacterResources.Contains(characterResourceLink2))
				{
					m_loadingCharacterResources.Add(characterResourceLink2);
					characterResourceLink2.LoadAsync(teamPlayerInfo2.CharacterInfo.CharacterSkin, HandleCharacterResourceLoaded, gameStatusForAssets);
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
		Log.Info(Log.Category.Loading, "Finished loading character assets");
		m_loadingCharacterAssets = false;
	}

	private void HandleCharacterResourceLoaded(LoadedCharacterSelection loadedCharacter)
	{
		m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
		m_loadedCharacterResourceCount++;
		Log.Info("Loaded CharacterResource {0} (total={1}, remaining={2})", loadedCharacter.resourceLink.name, m_loadedCharacterResourceCount, m_loadingCharacterResources.Count);
	}

	private void ResetLoadAssetsState()
	{
		m_loading = false;
		m_loadingCharacterResources.Clear();
		m_loadingCharacterAssets = false;
	}

	public void LoadCharacterResourceLink(CharacterResourceLink characterResourceLink, CharacterVisualInfo linkVisualInfo = default(CharacterVisualInfo))
	{
		m_loadingCharacterResources.Add(characterResourceLink);
		characterResourceLink.LoadAsync(linkVisualInfo, HandleCharacterResourceLoaded);
	}

	private void UpdateLoadProgress(bool force = false)
	{
		if (PlayerInfo == null || TeamInfo == null)
		{
			return;
		}

		m_assetsLoadingState.LevelLoadProgress = m_loadLevelOperationDone
			? 1f
			: m_loadLevelOperation?.progress ?? 0f;
		m_assetsLoadingState.CharacterLoadProgress = Mathf.Clamp(m_loadedCharacterResourceCount / (float)TeamInfo.TotalPlayerCount, 0f, 1f);
		m_assetsLoadingState.VfxPreloadProgress = ClientVFXLoader.Get() != null ? ClientVFXLoader.Get().Progress : 0f;
		m_assetsLoadingState.SpawningProgress = m_spawnableObjectCount > 0 ? Mathf.Clamp(ClientScene.objects.Count / (float)m_spawnableObjectCount, 0f, 1f) : 0f;
		if (PlayerInfo.PlayerId == 0)
		{
			UILoadingScreenPanel.Get().SetLoadingProgress(PlayerInfo, m_assetsLoadingState.TotalProgress, true);
		}
		else
		{
			UILoadingScreenPanel.Get().SetLoadingProgress(PlayerInfo.PlayerId, m_assetsLoadingState.TotalProgress, true);
		}

		bool isTimeToSend = Time.unscaledTime > m_lastLoadProgressUpdateSent + m_loadingProgressUpdateFrequency;
		if (isTimeToSend || force)
		{
			if (Client != null && Client.isConnected && IsRegisteredToGameServer)
			{
				GameManager.AssetsLoadingProgress assetsLoadingProgress = new GameManager.AssetsLoadingProgress
					{
						AccountId = m_lobbyGameClientInterface.SessionInfo.AccountId,
						PlayerId = PlayerInfo.PlayerId,
						TotalLoadingProgress = (byte)(m_assetsLoadingState.TotalProgress * 100f),
						LevelLoadingProgress = (byte)(m_assetsLoadingState.LevelLoadProgress * 100f),
						CharacterLoadingProgress = (byte)(m_assetsLoadingState.CharacterLoadProgress * 100f),
						VfxLoadingProgress = (byte)(m_assetsLoadingState.VfxPreloadProgress * 100f),
						SpawningProgress = (byte)(m_assetsLoadingState.SpawningProgress * 100f)
					};
				m_lastLoadProgressUpdateSent = Time.unscaledTime;
				Client.Send((short)MyMsgType.ClientAssetsLoadingProgressUpdate, assetsLoadingProgress);
			}
		}
	}

	private void CheckLoaded()
	{
		if (IsServer())
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		bool flag;
		if (m_loadLevelOperationDone)
		{
			flag = (GameFlowData.Get() == null || GameFlowData.Get().gameState < GameState.Deployment);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (!m_loading)
		{
			if (!flag2)
			{
				goto IL_85;
			}
		}
		UpdateLoadProgress();
		IL_85:
		if (m_loading)
		{
			if (m_loadLevelOperation != null)
			{
				if (m_loadLevelOperation.isDone)
				{
					m_loadLevelOperation = null;
					m_loadLevelOperationDone = true;
				}
			}
			if (m_loadLevelOperation == null)
			{
				if (m_loadingCharacterResources.Count == 0 && !m_loadingCharacterAssets)
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
								if (Client != null)
								{
									if (Client.isConnected)
									{
										ResetLoadAssetsState();
										UpdateLoadProgress(true);
										GameManager.AssetsLoadedNotification assetsLoadedNotification = new GameManager.AssetsLoadedNotification();
										assetsLoadedNotification.AccountId = m_lobbyGameClientInterface.SessionInfo.AccountId;
										assetsLoadedNotification.PlayerId = gameManager.PlayerInfo.PlayerId;
										Log.Info(Log.Category.Loading, "Sending asset loaded notification");
										if (!Client.Send(0x35, assetsLoadedNotification))
										{
											Log.Error("Failed to send message AssetsLoadedNotification");
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
			if (m_loadLevelOperation == null && m_loadingCharacterResources.Count == 0)
			{
				if (ClientVFXLoader.Get() != null && !ClientVFXLoader.Get().IsPreloadQueueEmpty())
				{
					if (!ClientVFXLoader.Get().IsPreloadInProgress())
					{
						Log.Info(Log.Category.Loading, "Starting VFX Preload");
						ClientVFXLoader.Get().PreloadQueuedPKFX();
					}
				}
			}
		}
	}

	public bool IsGroupReady()
	{
		bool result = true;
		if (GroupInfo.InAGroup)
		{
			for (int i = 0; i < GroupInfo.Members.Count; i++)
			{
				if (!GroupInfo.Members[i].IsReady)
				{
					return false;
				}
			}
		}
		return result;
	}

	public bool IsPlayerAccountDataAvailable()
	{
		return m_loadedPlayerAccountData != null;
	}

	public bool IsPlayerCharacterDataAvailable(CharacterType charType = CharacterType.None)
	{
		if (m_loadedPlayerCharacterData == null)
		{
			return false;
		}
		return charType == CharacterType.None || m_loadedPlayerCharacterData.ContainsKey(charType);
	}

	public int GetHighestOpenSeasonChapterIndexForActiveSeason()
	{
		int result = -1;
		if (IsPlayerAccountDataAvailable())
		{
			if (SeasonWideData.Get() != null)
			{
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(GetPlayerAccountData().QuestComponent.ActiveSeason);
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
					if (QuestWideData.AreConditionsMet(seasonChapter.Prerequisites.Conditions, seasonChapter.Prerequisites.LogicStatement))
					{
						result = i;
					}
					goto IL_FC;
				}
			}
		}
		return result;
	}

	public PersistedAccountData GetPlayerAccountData()
	{
		if (m_loadedPlayerAccountData == null)
		{
			Log.Error("Player account data not loaded yet");
			return null;
		}
		return m_loadedPlayerAccountData;
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		GameTypeAvailabilies = new Dictionary<GameType, GameTypeAvailability>(notification.GameTypeAvailabilies, default(GameTypeComparer));
		FreeRotationAdditions = new Dictionary<CharacterType, RequirementCollection>(notification.FreeRotationAdditions, default(CharacterTypeComparer));
		AllowBadges = notification.AllowBadges;
		NewPlayerPvPQueueDuration = notification.NewPlayerPvPQueueDuration;
		foreach (GameTypeAvailability gameTypeAvailability in GameTypeAvailabilies.Values)
		{
			foreach (GameSubType gameSubType in gameTypeAvailability.SubTypes)
			{
				List<GameMapConfig> gameMapConfigs = gameSubType.GameMapConfigs;
				
				if (!gameMapConfigs.Exists((p => p.IsActive)))
				{
					if (gameSubType.Requirements == null)
					{
						gameSubType.Requirements = RequirementCollection.Create();
					}
					RequirementCollection requirements = gameSubType.Requirements;
					
					if (!requirements.Exists((p => p is QueueRequirement_Never)))
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
		m_tierInstanceNames = notification.TierInstanceNames;
		OnServerQueueConfigurationUpdateNotificationHolder(notification);
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
		HandleAccountDataUpdated(notification.AccountData);
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification notification)
	{
		if (IsReady)
		{
			OnQuestCompleteNotificationHolder(notification);
		}
		else
		{
			LoginQuestCompleteNotifications.Add(notification);
		}
	}

	private void HandleFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		ActiveFactionCompetition = notification.ActiveIndex;
		FactionScores = notification.Scores;
		OnFactionCompetitionNotificationHolder(notification);
	}

	private void HandleTrustBoostUsedNotification(TrustBoostUsedNotification notification)
	{
		OnTrustBoostUsedNotificationHolder(notification);
	}

	private void HandleFactionLoginRewardNotification(FactionLoginRewardNotification notification)
	{
		if (LoginRewardNotification != null)
		{
			Log.Error("received a second login notification! - should not");
		}
		LoginRewardNotification = notification;
		OnFactionLoginRewardNotificationHolder(notification);
	}

	private void HandlePlayerFactionContributionChange(PlayerFactionContributionChangeNotification notification)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (!m_lobbyGameClientInterface.IsConnected)
			{
			}
			else
			{
				if (!IsPlayerAccountDataAvailable())
				{
					Log.Error("Player Account Data not available for Faction Contribution Change");
					return;
				}
				GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(notification.CompetitionId, notification.FactionId).TotalXP = notification.TotalXP;
				OnPlayerFactionContributionChangeNotificationHolder(notification);
				return;
			}
		}
		Log.Error("Not connected to lobby server for Faction Contribution Change");
	}

	private void HandleFacebookAccessTokenNotification(FacebookAccessTokenNotification notification)
	{
		FacebookClientInterface.Get().UploadScreenshot(notification.AccessToken);
	}

	private void HandleMatchResultsNotification(MatchResultsNotification notification)
	{
		OnMatchResultsNotificationHolder(notification);
	}

	public void QueryPlayerMatchData(Action<PlayerMatchDataResponse> onResponseCallback)
	{
		if (onResponseCallback == null)
		{
			throw new ArgumentNullException("onResponseCallback");
		}
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.GetPlayerMatchData(onResponseCallback);
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
		if (m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet");
			return -1;
		}
		PersistedCharacterData persistedCharacterData;
		if (m_loadedPlayerCharacterData.TryGetValue(character, out persistedCharacterData))
		{
			return persistedCharacterData.ExperienceComponent.Level;
		}
		return -1;
	}

	public PersistedCharacterData GetPlayerCharacterData(CharacterType character)
	{
		if (m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet");
			return null;
		}
		PersistedCharacterData result;
		if (m_loadedPlayerCharacterData.TryGetValue(character, out result))
		{
			return result;
		}
		return null;
	}

	public Dictionary<CharacterType, PersistedCharacterData> GetAllPlayerCharacterData()
	{
		if (m_loadedPlayerCharacterData == null)
		{
			Log.Error("Player character data not loaded yet");
			return null;
		}
		return m_loadedPlayerCharacterData;
	}

	public PersistedCharacterData GetCharacterDataOnInitialLoad(CharacterType charType)
	{
		PersistedCharacterData result;
		if (m_characterDataOnInitialLoad.TryGetValue(charType, out result))
		{
			return result;
		}
		return null;
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				PurchasingMod = true;
				ModAttemptingToPurchase = abilityModID;
				m_lobbyGameClientInterface.PurchaseMod(character, abilityId, abilityModID, HandlePurchaseModResponse);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	private void HandlePurchaseModResponse(PurchaseModResponse response)
	{
		if (response.Success)
		{
			PersistedCharacterData persistedCharacterData;
			if (m_loadedPlayerCharacterData.TryGetValue(response.Character, out persistedCharacterData))
			{
				persistedCharacterData.CharacterComponent.Mods.Add(response.UnlockData);
			}
			OnModUnlockedHolder(response.Character, response.UnlockData);
		}
		else
		{
			Log.Error(string.Format("Failed to unlock Mod {0} for character {1}: {2}", response.UnlockData.ToString(), response.Character, response.ErrorMessage));
		}
		ModAttemptingToPurchase = -1;
		PurchasingMod = false;
	}

	public void PurchaseLoadoutSlot(CharacterType characterType, Action<PurchaseLoadoutSlotResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PurchaseLoadoutSlot(characterType, delegate(PurchaseLoadoutSlotResponse response)
			{
				if (onResponseCallback != null)
				{
					onResponseCallback(response);
				}
			});
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void HandleSelectLoadoutUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			RecordFailureInCharacterSelectUpdateResponse(response, "HandleSelectLoadoutUpdateResponse");
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
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
				lobbyPlayerInfoUpdate.CharacterMods = loadout.ModSet;
				lobbyPlayerInfoUpdate.CharacterAbilityVfxSwaps = loadout.VFXSet;
				lobbyPlayerInfoUpdate.LastSelectedLoadout = loadoutIndex;
				lobbyPlayerInfoUpdate.RankedLoadoutMods = (loadout.Strictness == ModStrictness.Ranked);
				m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleModSelectUpdateResponse);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseModToken(int numToPurchase)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseModToken(numToPurchase, HandlePurchaseModTokenResponse);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void HandlePurchaseModTokenResponse(PurchaseModTokenResponse response)
	{
		if (!response.Success)
		{
			Log.Error("Failed to purchase Mod Token");
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
			});
			Log.Error("Did not use GG pack: {0}", text2);
		}
		HUD_UI.Get().m_mainScreenPanel.m_characterProfile.NotifyReceivedGGPackResponse();
		UIGameOverScreen.Get().NotifySelfGGPackUsed();
	}

	public void RequestToUseGGPack()
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.RequestToUseGGPack(HandleGGPackUseResponse);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void CheckAndSendClientPreparedForGameStartNotification()
	{
		if (PlayerObjectStartedOnClient && InGameUIActivated)
		{
			if (VisualSceneLoaded)
			{
				if (DesignSceneStarted)
				{
					SendClientPreparedForGameStartNotification();
					PlayerObjectStartedOnClient = false;
					InGameUIActivated = false;
					VisualSceneLoaded = false;
					DesignSceneStarted = false;
				}
			}
		}
	}

	public void SendClientPreparedForGameStartNotification()
	{
		Log.Info("SendClientPreparedForGameStartNotification");
		if (NetworkClient.active && !NetworkServer.active)
		{
			if (GameplayData.Get() == null)
			{
				Log.Error("SendClientPreparedForGameStartNotification, but GameplayData is null");
			}
			if (GameFlowData.Get() == null)
			{
				Log.Error("SendClientPreparedForGameStartNotification, but GameFlowData is null");
			}
			GameManager.PlayerObjectStartedOnClientNotification playerObjectStartedOnClientNotification = new GameManager.PlayerObjectStartedOnClientNotification();
			playerObjectStartedOnClientNotification.AccountId = m_lobbyGameClientInterface.SessionInfo.AccountId;
			playerObjectStartedOnClientNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
			Client.Send(0x37, playerObjectStartedOnClientNotification);
		}
	}

	public void UpdateRemoteCharacter(CharacterType character, int remoteSlotIndex, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		UpdateRemoteCharacter(new[]
		{
			character
		}, new[]
		{
			remoteSlotIndex
		}, onResponse);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		if (m_lobbyGameClientInterface == null || m_lobbyGameClientInterface.IsConnected)
		{
			Log.Error("Not connected to lobby server.");
			return;
		}
		if (remoteSlotIndexes.IsNullOrEmpty() || characters.IsNullOrEmpty() || remoteSlotIndexes.Length != characters.Length)
		{
			return;
		}

		if (m_loadedPlayerAccountData != null)
		{
			bool flag = false;
			List<CharacterType> lastRemoteCharacters = m_loadedPlayerAccountData.AccountComponent.LastRemoteCharacters;
						
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
		m_lobbyGameClientInterface.UpdateRemoteCharacter(characters, remoteSlotIndexes, onResponse);	
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (m_loadedPlayerAccountData != null)
				{
					if (m_loadedPlayerAccountData.AccountComponent.SelectedTitleID == newTitleID)
					{
						return;
					}
				}
				m_lobbyGameClientInterface.RequestTitleSelect(newTitleID, delegate(SelectTitleResponse response)
				{
					if (m_loadedPlayerAccountData != null)
					{
						m_loadedPlayerAccountData.AccountComponent.SelectedTitleID = response.CurrentTitleID;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (gameBalanceVars != null)
					{
						OnPlayerTitleChangeHolder(gameBalanceVars.GetTitle(response.CurrentTitleID, string.Empty));
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RequestBannerSelect(int newBannerID, Action<SelectBannerResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (m_loadedPlayerAccountData != null)
				{
					if (m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID != newBannerID)
					{
						if (m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID != newBannerID)
						{
							goto IL_8E;
						}
					}
					return;
				}
				IL_8E:
				m_lobbyGameClientInterface.RequestBannerSelect(newBannerID, delegate(SelectBannerResponse response)
				{
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (m_loadedPlayerAccountData != null)
					{
						if (gameBalanceVars != null)
						{
							m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID = response.ForegroundBannerID;
							m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID = response.BackgroundBannerID;
							OnPlayerBannerChangeHolder(gameBalanceVars.GetBanner(response.ForegroundBannerID), gameBalanceVars.GetBanner(response.BackgroundBannerID));
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
		Log.Error("Not connected to lobby server.");
	}

	public void RequestRibbonSelect(int newRibbonID, Action<SelectRibbonResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (m_loadedPlayerAccountData != null && m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID == newRibbonID)
				{
					return;
				}
				m_lobbyGameClientInterface.RequestRibbonSelect(newRibbonID, delegate(SelectRibbonResponse response)
				{
					if (m_loadedPlayerAccountData != null)
					{
						m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID = response.CurrentRibbonID;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					if (gameBalanceVars != null)
					{
						OnPlayerRibbonChangeHolder(gameBalanceVars.GetRibbon(response.CurrentRibbonID));
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RequestLoadingScreenBackgroundToggle(int loadingScreenId, bool newState, Action<LoadingScreenToggleResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (m_loadedPlayerAccountData != null)
				{
					if (!m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundUnlocked(loadingScreenId))
					{
						return;
					}
					bool flag = m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundActive(loadingScreenId);
					if (flag == newState)
					{
						return;
					}
				}
				m_lobbyGameClientInterface.RequestLoadingScreenBackgroundToggle(loadingScreenId, newState, delegate(LoadingScreenToggleResponse response)
				{
					if (!response.Success)
					{
						return;
					}
					if (m_loadedPlayerAccountData != null)
					{
						m_loadedPlayerAccountData.AccountComponent.ToggleLoadingScreenBackgroundActive(response.LoadingScreenId, response.CurrentState);
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
					OnLoadingScreenBackgroundToggledHolder(response.LoadingScreenId, response.CurrentState);
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RequestRankedLeaderboardOverview(GameType gameType, Action<RankedLeaderboardOverviewResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedLeaderboardOverviewRequest(gameType, onResponse);
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
		if (m_lobbyGameClientInterface != null)
		{
			m_lobbyGameClientInterface.SendRankedLeaderboardOverviewRequest(gameType, groupSize, specification, onResponse);
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
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.RequestUpdateUIState(uiState, stateValue, delegate(UpdateUIStateResponse response)
				{
					if (m_loadedPlayerAccountData != null)
					{
						m_loadedPlayerAccountData.AccountComponent.UIStates[uiState] = stateValue;
					}
					if (onResponse != null)
					{
						onResponse(response);
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void SetPushToTalkKey(int keyType, int keyCode, string keyName)
	{
		if (m_loadedPlayerAccountData != null)
		{
			m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyType = keyType;
			m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyCode = keyCode;
			m_loadedPlayerAccountData.AccountComponent.PushToTalkKeyName = keyName;
		}
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SetPushToTalkKey(keyType, keyCode, keyName);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void symbol_001D(string symbol_001D, string symbol_000E)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.DEBUG_AdminSlashCommandExecuted(symbol_001D, symbol_000E);
			}
		}
	}

	public void symbol_000E()
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
				
				lobbyGameClientInterface.DEBUG_ForceMatchmaking(delegate(DEBUG_ForceMatchmakingResponse response)
					{
						string text = (!response.Success) ? string.Format("Failed to force queue: {0}", response.ErrorMessage) : string.Format("Forced queue {0}", response.GameType);
						TextConsole.Get().Write(text);
						if (response.Success)
						{
							Log.Info(text);
						}
						else
						{
							Log.Error(text);
						}
					});
			}
		}
	}

	public void symbol_0012()
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
				
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
						TextConsole.Get().Write(text2);
						if (response.Success)
						{
							Log.Info(text2);
						}
						else
						{
							Log.Error(text2);
						}
					});
			}
		}
	}

	public void RequestPreviousGameInfo(Action<PreviousGameInfoResponse> onResponseCallback)
	{
		if (IsReady)
		{
			m_lobbyGameClientInterface.SendPreviousGameInfoRequest(onResponseCallback);
		}
	}

	public void RejoinGame(bool accept, Action<RejoinGameResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendRejoinGameRequest(GameInfo, accept, onResponseCallback);
				return;
			}
		}
		string text = string.Format("{0} {1}", StringUtil.TR("FailedToRejoinGame", "Frontend"), StringUtil.TR("NotConnectedToLobbyServer", "Global"));
		TextConsole.Get().Write(text);
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
		DiscordJoinType discordJoinType = GetDiscordJoinType();
		if (discordJoinType == DiscordJoinType._001D)
		{
			return;
		}
		if (m_discordJoinType != discordJoinType)
		{
			Log.Info("Discord | switch joinType {0} => {1} (teamChat={2}, groupChat={3})", m_discordJoinType, discordJoinType, DiscordClientInterface.CanJoinTeamChat, DiscordClientInterface.CanJoinGroupChat);
			LeaveDiscord();
		}
		if (!m_discordConnecting)
		{
			if (!m_discordConnected)
			{
				if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
				{
					m_discordConnecting = true;
					m_discordJoinType = discordJoinType;
					LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
					
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
								DiscordClientInterface.Get().Connect(authInfo);
							}
						});
				}
			}
		}
	}

	private void JoinDiscordChannel(DiscordUserInfo userInfo)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendDiscordJoinServerRequest(userInfo.UserId, userInfo.AccessToken, m_discordJoinType, delegate(DiscordJoinServerResponse response)
				{
					if (response.Success)
					{
						if (m_discordJoinType != DiscordJoinType._001D)
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
						Log.Error("Failed to join discord server {0}", response.ErrorMessage);
						TextConsole.Get().Write(string.Format(StringUtil.TR("FailedToJoinDiscordChat", "Global"), response.ErrorMessage));
						DiscordClientInterface.Get().Disconnect();
					}
				});
			}
		}
	}

	public void CheckDiscord()
	{
		DiscordClientInterface.Get().Connect(null);
	}

	private void HandleDiscordConnected(bool needAuthentication)
	{
		m_discordConnecting = false;
		if (needAuthentication)
		{
			string text = string.Format(StringUtil.TR("ClickToJoinDiscordTeamChat", "Global"));
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				TextConsole.Get().Write(text);
			}
			DiscordClientInterface.Get().Disconnect();
		}
	}

	private void HandleDiscordDisconnected()
	{
		m_discordConnecting = false;
	}

	private void HandleDiscordAuthorized(string rpcCode)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
			
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
		JoinDiscordChannel(userInfo);
	}

	private void HandleDiscordJoined()
	{
		TextConsole.Get().Write("Joined Discord team chat");
	}

	private void HandleDiscordLeft()
	{
		TextConsole.Get().Write("Left Discord team chat");
	}

	public void LeaveDiscord()
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendDiscordLeaveServerRequest(m_discordJoinType, delegate(DiscordLeaveServerResponse response)
				{
					Log.Info("Remove user from Discord server {0} (result {1})", m_discordJoinType, response.Success);
				});
			}
		}
		DiscordClientInterface.Get().Disconnect();
		m_discordConnecting = false;
		m_discordJoinType = DiscordJoinType._001D;
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
		TextConsole.Get().Write(text);
	}

	public void FacebookShareScreenshot(string message = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.SendFacebookGetUserTokenRequest(delegate(FacebookGetUserTokenResponse response)
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
		if (m_loadedPlayerAccountData != null)
		{
			Dictionary<int, QuestProgress> progress = m_loadedPlayerAccountData.QuestComponent.Progress;
			List<QuestProgress> list2 = accountData.QuestComponent.Progress.Values.ToList();
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
		m_loadedPlayerAccountData = accountData;
		OnAccountDataUpdatedHolder(accountData);
		PlayerWallet = new CurrencyWallet(accountData.BankComponent.CurrentAmounts.Data);
		IEnumerator<CurrencyData> enumerator2 = PlayerWallet.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				CurrencyData obj = enumerator2.Current;
				OnBankBalanceChangeHolder(obj);
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
			OnQuestProgressChangedHolder(list.ToArray());
		}
	}

	private void HandlePlayerCharacterDataUpdated(List<PersistedCharacterData> characterDataList)
	{
		m_loadedPlayerCharacterData = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		m_characterDataOnInitialLoad = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		foreach (PersistedCharacterData persistedCharacterData in characterDataList)
		{
			m_loadedPlayerCharacterData.Add(persistedCharacterData.CharacterType, persistedCharacterData);
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
			m_characterDataOnInitialLoad.Add(persistedCharacterData.CharacterType, persistedCharacterData2);
		}
	}

	private void HandleForcedCharacterChangeFromServerNotification(ForcedCharacterChangeFromServerNotification notification)
	{
		if (GroupInfo != null)
		{
			GroupInfo.SetCharacterInfo(notification.ChararacterInfo);
			Log.Info("Server forcing us to switch to {0}", notification.ChararacterInfo.CharacterType.GetDisplayName());
			return;
		}
		throw new Exception(string.Format("The server believes we should be freelancer {0}, but we're not doing anything about that. This is bad?!", notification.ChararacterInfo.CharacterType.GetDisplayName()));
	}

	private void HandleCharacterDataUpdateNotification(PlayerCharacterDataUpdateNotification notification)
	{
		m_loadedPlayerCharacterData[notification.CharacterData.CharacterType] = notification.CharacterData;
		OnCharacterDataUpdatedHolder(notification.CharacterData);
	}

	private void HandleInventoryComponentUpdateNotification(InventoryComponentUpdateNotification notification)
	{
		m_loadedPlayerAccountData.InventoryComponent = notification.InventoryComponent;
		OnInventoryComponentUpdatedHolder(notification.InventoryComponent);
	}

	private void HandleBankBalanceChangeNotification(BankBalanceChangeNotification notification)
	{
		if (notification.Success)
		{
			if (PlayerWallet != null)
			{
				PlayerWallet.SetValue(notification.NewBalance);
				OnBankBalanceChangeHolder(notification.NewBalance);
			}
		}
	}

	public void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		if (m_loadedPlayerAccountData != null)
		{
			m_loadedPlayerAccountData.QuestComponent.ActiveSeason = notification.SeasonStartedIndex;
		}
		OnSeasonCompleteNotificationHolder(notification);
		if (m_loadedPlayerAccountData != null)
		{
			OnAccountDataUpdatedHolder(m_loadedPlayerAccountData);
		}
	}

	public void SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType dialogType)
	{
		m_lobbyGameClientInterface.SendSeasonStatusConfirmed(dialogType);
	}

	public void HandleChapterStatusNotification(ChapterStatusNotification notification)
	{
		if (!notification.Success)
		{
			return;
		}
		if (notification.IsCompleted)
		{
			OnChapterCompleteNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
		}
		else if (notification.IsUnlocked)
		{
			OnChapterUnlockNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
		}
	}

	public void SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendPlayerCharacterFeedback(feedbackData);
			}
		}
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.InviteToGroup(friendHandle, onResponseCallback);
				return;
			}
		}
		GroupInviteResponse groupInviteResponse = new GroupInviteResponse();
		groupInviteResponse.Success = false;
		groupInviteResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void RequestToJoinGroup(string friendHandle, Action<GroupJoinResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.RequestToJoinGroup(friendHandle, onResponseCallback);
		}
		else
		{
			GroupJoinResponse groupJoinResponse = new GroupJoinResponse();
			groupJoinResponse.Success = false;
			groupJoinResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PromoteWithinGroup(name, onResponseCallback);
				return;
			}
		}
		GroupPromoteResponse groupPromoteResponse = new GroupPromoteResponse();
		groupPromoteResponse.Success = false;
		groupPromoteResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void ChatToGroup(string text, Action<GroupChatResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.ChatToGroup(text, onResponseCallback);
		}
		else
		{
			GroupChatResponse groupChatResponse = new GroupChatResponse();
			groupChatResponse.Success = false;
			groupChatResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.");
		}
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.LeaveGroup(onResponseCallback);
				return;
			}
		}
		GroupLeaveResponse groupLeaveResponse = new GroupLeaveResponse();
		groupLeaveResponse.Success = false;
		groupLeaveResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void KickFromGroup(string memberName, Action<GroupKickResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.KickFromGroup(memberName, onResponseCallback);
				return;
			}
		}
		GroupKickResponse groupKickResponse = new GroupKickResponse();
		groupKickResponse.Success = false;
		groupKickResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void UpdateFriend(string friendHandle, long friendAccountId, FriendOperation operation, string strData, Action<FriendUpdateResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.UpdateFriend(friendHandle, friendAccountId, operation, strData, onResponseCallback);
		}
		else if (onResponseCallback != null)
		{
			FriendUpdateResponse friendUpdateResponse = new FriendUpdateResponse();
			friendUpdateResponse.Success = false;
			friendUpdateResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.");
			onResponseCallback(friendUpdateResponse);
		}
	}

	public void UpdatePlayerStatus(string statusString)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			
			Action<PlayerUpdateStatusResponse> onResponseCallback = delegate(PlayerUpdateStatusResponse resonse)
				{
					if (!resonse.Success)
					{
						Log.Warning(resonse.ErrorMessage);
					}
				};
			m_lobbyGameClientInterface.UpdatePlayerStatus(statusString, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseLootMatrixPack(lootMatrixPackIndex, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseGame(int gamePackIndex, long paymentMethodId, Action<PurchaseGameResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PurchaseGame(gamePackIndex, paymentMethodId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseGGPack(int ggPackIndex, long paymentMethodId, Action<PurchaseGGPackResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseGGPack(ggPackIndex, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseCharacter(CurrencyType currencyType, CharacterType characterType, Action<PurchaseCharacterResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseCharacter(currencyType, characterType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseCharacterForCash(CharacterType characterType, long paymentMethodId, Action<PurchaseCharacterForCashResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseCharacterForCash(characterType, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseSkin(CurrencyType currencyType, CharacterType characterType, int skinId, Action<PurchaseSkinResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseSkin(currencyType, characterType, skinId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTexture(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, Action<PurchaseTextureResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseTexture(currencyType, characterType, skinId, textureId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTint(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, int tintId, Action<PurchaseTintResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PurchaseTint(currencyType, characterType, skinId, textureId, tintId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseTintForCash(CharacterType characterType, int skinId, int textureId, int tintId, long paymentMethodId, Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseTintForCash(characterType, skinId, textureId, tintId, paymentMethodId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseStoreItemForCash(int inventoryTemplateId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PurchaseStoreItemForCash(inventoryTemplateId, paymentMethodId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntIndex, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseTaunt(currencyType, characterType, tauntIndex, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PlayerPanelUpdated(int _selectedTitleID, int _selectedForegroundBannerID, int _selectedBackgroundBannerID, int _selectedRibbonID)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PlayerPanelUpdated(_selectedTitleID, _selectedForegroundBannerID, _selectedBackgroundBannerID, _selectedTitleID);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseInventoryItem(int inventoryItemID, CurrencyType currencyType, Action<PurchaseInventoryItemResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseInventoryItem(inventoryItemID, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTitle(int titleId, CurrencyType currencyType, Action<PurchaseTitleResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.PurchaseTitle(currencyType, titleId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void PurchaseBanner(int bannerId, CurrencyType currencyType, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseBannerBackground(currencyType, bannerId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseEmblem(int emblemId, CurrencyType currencyType, Action<PurchaseBannerForegroundResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseBannerForeground(currencyType, emblemId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseEmoticon(int emoticonId, CurrencyType currencyType, Action<PurchaseChatEmojiResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseChatEmoji(currencyType, emoticonId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseAbilityVfx(CharacterType type, int abilityId, int vfxId, CurrencyType currencyType, Action<PurchaseAbilityVfxResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseAbilityVfx(type, abilityId, vfxId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseOvercon(int overconId, CurrencyType currencyType, Action<PurchaseOverconResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseOvercon(overconId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType, Action<PurchaseLoadingScreenBackgroundResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.PurchaseLoadingScreenBackground(loadingScreenBackgroundId, currencyType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PreventNextAccountStatusCheck()
	{
		m_preventNextAccountStatusCheck = true;
	}

	public void SendCheckAccountStatusRequest(Action<CheckAccountStatusResponse> onResponseCallback = null)
	{
		if (m_preventNextAccountStatusCheck)
		{
			m_preventNextAccountStatusCheck = false;
			return;
		}
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.CheckAccountStatus(onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void SendCheckRAFStatusRequest(bool getReferralCode, Action<CheckRAFStatusResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (onResponseCallback == null)
				{
					
					onResponseCallback = delegate {};
				}
				m_lobbyGameClientInterface.CheckRAFStatus(getReferralCode, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void SendRAFReferralEmailsRequest(List<string> emails, Action<SendRAFReferralEmailsResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendRAFReferralEmails(emails, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void SelectDailyQuest(int questId, Action<PickDailyQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SelectDailyQuest(questId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.AbandonDailyQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void ActivateQuestTrigger(QuestTriggerType triggerType, int activationCount, int questId, int questBonusCount, int itemTemplateId, CharacterType charType, Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.ActivateQuestTrigger(triggerType, activationCount, questId, questBonusCount, itemTemplateId, charType, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.BeginQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void CompleteQuest(int questId, Action<CompleteQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.CompleteQuest(questId, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void MarkTutorialSkipped(TutorialVersion progress, Action<MarkTutorialSkippedResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.MarkTutorialSkipped(progress, delegate(MarkTutorialSkippedResponse response)
				{
					m_loadedPlayerAccountData.ExperienceComponent.TutorialProgress = progress;
					if (onResponseCallback != null)
					{
						onResponseCallback(response);
					}
				});
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void GetInventoryItems(Action<GetInventoryItemsResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				Action<GetInventoryItemsResponse> onResponseCallback2 = delegate(GetInventoryItemsResponse response)
				{
					if (response.Success)
					{
						if (m_loadedPlayerAccountData != null)
						{
							m_loadedPlayerAccountData.InventoryComponent.Items = response.Items;
						}
					}
					if (onResponseCallback != null)
					{
						onResponseCallback(response);
					}
				};
				m_lobbyGameClientInterface.GetInventoryItems(onResponseCallback2);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void AddInventoryItems(List<InventoryItem> items, Action<AddInventoryItemsResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.AddInventoryItems(items, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RemoveInventoryItems(List<InventoryItem> items, Action<RemoveInventoryItemsResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.RemoveInventoryItems(items, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void ConsumeInventoryItem(int itemId, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		ConsumeInventoryItem(itemId, 1, toISO, onResponseCallback);
	}

	public void ConsumeInventoryItem(int itemId, int itemCount, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.ConsumeInventoryItem(itemId, itemCount, toISO, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void ConsumeInventoryItems(List<int> itemIds, bool toISO, Action<ConsumeInventoryItemsResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.ConsumeInventoryItems(itemIds, toISO, onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.RerollSeasonQuests(seasonId, chapterId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void SetSeasonQuest(int seasonId, int chapterId, int slotNum, int questId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SetSeasonQuest(seasonId, chapterId, slotNum, questId, onResponseCallback);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void NotifyStoreOpened()
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.NotifyStoreOpened();
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void NotifyCustomKeyBinds(Dictionary<int, KeyCodeData> CustomKeyBinds)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.NotifyCustomKeyBinds(CustomKeyBinds);
				return;
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void NotifyOptions(OptionsNotification notification)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.NotifyOptions(notification);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		if (!SteamManager.UsingSteam)
		{
			if (m_lobbyGameClientInterface != null)
			{
				if (m_lobbyGameClientInterface.IsConnected)
				{
					m_lobbyGameClientInterface.RequestPaymentMethods(onResponseCallback);
					goto IL_5B;
				}
			}
			Log.Error("Not connected to lobby server.");
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
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			m_lobbyGameClientInterface.RequestRefreshBankData(onResponseCallback);
		}
		else
		{
			Log.Error("Not connected to lobby server.");
		}
	}

	public bool IsServer()
	{
		return NetworkServer.active;
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return IsTitleUnlocked(title, out list);
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		GetUnlockStatus(title.m_unlockData, out unlockConditionValues);
		return m_loadedPlayerAccountData.AccountComponent.IsTitleUnlocked(title);
	}

	public bool IsTitleAtMaxLevel(GameBalanceVars.PlayerTitle title)
	{
		if (GameWideData.Get().m_gameBalanceVars != null)
		{
			int maxTitleLevel = GameWideData.Get().m_gameBalanceVars.GetMaxTitleLevel(title.ID);
			int currentTitleLevel = GetCurrentTitleLevel(title.ID);
			return currentTitleLevel >= maxTitleLevel;
		}
		return true;
	}

	public int GetCurrentTitleLevel(int titleID)
	{
		return m_loadedPlayerAccountData.AccountComponent.GetCurrentTitleLevel(titleID);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return IsEmojiUnlocked(emoji, out list);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		GetUnlockStatus(emoji.m_unlockData, out unlockConditionValues);
		return m_loadedPlayerAccountData.AccountComponent.IsChatEmojiUnlocked(emoji);
	}

	public bool IsOverconUnlocked(int overconId)
	{
		return m_loadedPlayerAccountData != null && m_loadedPlayerAccountData.AccountComponent.IsOverconUnlocked(overconId);
	}

	public bool IsBannerUnlocked(GameBalanceVars.PlayerBanner banner, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		GetUnlockStatus(banner.m_unlockData, out unlockConditionValues);
		return m_loadedPlayerAccountData.AccountComponent.IsBannerUnlocked(banner);
	}

	public bool IsRibbonUnlocked(GameBalanceVars.PlayerRibbon ribbon, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		GetUnlockStatus(ribbon.m_unlockData, out unlockConditionValues);
		return m_loadedPlayerAccountData.AccountComponent.IsRibbonUnlocked(ribbon);
	}

	public bool IsLoadingScreenBackgroundUnlocked(int loadingScreenBackgroundPathIndex)
	{
		return m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundUnlocked(loadingScreenBackgroundPathIndex);
	}

	public bool GetUnlockStatus(GameBalanceVars.UnlockData unlock, bool ignorePurchaseCondition = false)
	{
		List<GameBalanceVars.UnlockConditionValue> list;
		return GetUnlockStatus(unlock, out list, ignorePurchaseCondition);
	}

	public bool GetUnlockStatus(GameBalanceVars.UnlockData unlockData, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues, bool ignorePurchaseCondition = false)
	{
		unlockConditionValues = new List<GameBalanceVars.UnlockConditionValue>();
		if (unlockData != null)
		{
			if (!unlockData.UnlockConditions.IsNullOrEmpty())
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
						PersistedCharacterData persistedCharacterData = m_loadedPlayerCharacterData.TryGetValue((CharacterType)unlockCondition.typeSpecificData);
						if (persistedCharacterData != null)
						{
							unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
							unlockConditionValue.typeSpecificData2 = persistedCharacterData.ExperienceComponent.Level;
						}
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.PlayerLevel:
						unlockConditionValue.typeSpecificData = m_loadedPlayerAccountData.ExperienceComponent.Level;
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
						if (ActiveFactionCompetition == unlockCondition.typeSpecificData)
						{
							long factionScore;
							if (FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out factionScore))
							{
								unlockConditionValue.typeSpecificData3 = FactionWideData.Get().GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, factionScore);
							}
						}
						break;
					case GameBalanceVars.UnlockData.UnlockType.TitleLevelReached:
					{
						PersistedAccountData loadedPlayerAccountData = m_loadedPlayerAccountData;
						int typeSpecificData = unlockCondition.typeSpecificData;
						int currentTitleLevel = loadedPlayerAccountData.AccountComponent.GetCurrentTitleLevel(typeSpecificData);
						unlockConditionValue.typeSpecificData = typeSpecificData;
						unlockConditionValue.typeSpecificData2 = currentTitleLevel;
						break;
					}
					case GameBalanceVars.UnlockData.UnlockType.CurrentSeason:
						unlockConditionValue.typeSpecificData = m_loadedPlayerAccountData.QuestComponent.ActiveSeason;
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
		return GetUnlockStatus(unlockData, out list, ignorePurchaseCondition);
	}

	public bool AreUnlockConditionsMet(GameBalanceVars.PlayerUnlockable playerUnlockable, bool ignorePurchaseCondition = false)
	{
		return AreUnlockConditionsMet(playerUnlockable.m_unlockData, ignorePurchaseCondition);
	}

	public int GetCurrentTitleID()
	{
		if (m_loadedPlayerAccountData != null)
		{
			return m_loadedPlayerAccountData.AccountComponent.SelectedTitleID;
		}
		return -1;
	}

	public GameBalanceVars.PlayerBanner GetCurrentBackgroundBanner()
	{
		if (m_loadedPlayerAccountData != null && m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID != -1 && GameWideData.Get() != null && GameWideData.Get().m_gameBalanceVars != null)
		{
			return GameWideData.Get().m_gameBalanceVars.GetBanner(m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID);
		}
		return null;
	}

	public GameBalanceVars.PlayerBanner GetCurrentForegroundBanner()
	{
		if (m_loadedPlayerAccountData != null)
		{
			if (m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID != -1)
			{
				if (GameWideData.Get() != null)
				{
					if (GameWideData.Get().m_gameBalanceVars != null)
					{
						return GameWideData.Get().m_gameBalanceVars.GetBanner(m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID);
					}
				}
			}
		}
		return null;
	}

	public GameBalanceVars.PlayerRibbon GetCurrentRibbon()
	{
		if (m_loadedPlayerAccountData != null)
		{
			if (m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID != -1)
			{
				if (GameWideData.Get() != null)
				{
					if (GameWideData.Get().m_gameBalanceVars != null)
					{
						return GameWideData.Get().m_gameBalanceVars.GetRibbon(m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID);
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
		return GetDisplayedStatString(data.ExperienceComponent.Level, num, data.ExperienceComponent.Wins);
	}

	public string GetDisplayedStatString()
	{
		return GetDisplayedStatString(m_loadedPlayerAccountData);
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
		if (GroupInfo != null)
		{
			if (GroupInfo.InAGroup)
			{
				int num = 0;
				bool flag = false;
				for (int i = 0; i < GroupInfo.Members.Count; i++)
				{
					if (GroupInfo.Members[i].AccountID == Get().GetPlayerAccountData().AccountId)
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
					else if (GroupInfo.Members[i].IsReady)
					{
						num++;
					}
				}
				if (flag)
				{
					return string.Format(StringUtil.TR("WaitingForTeammates", "Global"), num, GroupInfo.Members.Count);
				}
			}
		}
		return string.Empty;
	}

	private void HttpPost(string url, string postString, Action<string, string> callback)
	{
		StartCoroutine(HttpPostCoroutine(url, postString, callback));
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
				client.Dispose();
			}
		}
	}

	public int NumCharacterResourcesCurrentlyLoading
	{
		get
		{
			if (m_loadingCharacterResources == null)
			{
				return 0;
			}
			return m_loadingCharacterResources.Count;
		}
	}

	public bool IsCurrentAlertQuest(int questId)
	{
		if (AlertMissionsData != null)
		{
			if (AlertMissionsData.CurrentAlert != null && AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
			{
				return AlertMissionsData.CurrentAlert.QuestId == questId;
			}
		}
		return false;
	}

	public bool IsTimeOffset()
	{
		return !(TimeOffset == default(TimeSpan));
	}

	public DateTime PacificNow()
	{
		DateTime d = UtcNow();
		return d - (ServerUtcTime - ServerPacificTime);
	}

	public DateTime UtcNow()
	{
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan t = utcNow - ClientUtcTime;
		return ServerUtcTime + t;
	}

	public DateTime UtcToPacific(DateTime dateTime)
	{
		return dateTime - (ServerUtcTime - ServerPacificTime);
	}

	public DateTime PacificToUtc(DateTime dateTime)
	{
		return dateTime + (ServerUtcTime - ServerPacificTime);
	}

	public string GetTierInstanceName(int instanceId)
	{
		if (instanceId < 1)
		{
			return null;
		}
		if (instanceId <= m_tierInstanceNames.Count)
		{
			return m_tierInstanceNames[instanceId - 1].ToString();
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
		if (!GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
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
			if (m_queueRequirementSystemInfo == null)
			{
				m_queueRequirementSystemInfo = new ClientGameManagerRequirementSystemInfo
				{
					m_gtas = GameTypeAvailabilies,
					m_serverUtcTime = ServerUtcTime,
					m_environmentType = EnvironmentType
				};
			}
			else
			{
				m_queueRequirementSystemInfo.m_gtas = GameTypeAvailabilies;
				m_queueRequirementSystemInfo.m_serverUtcTime = ServerUtcTime;
				m_queueRequirementSystemInfo.m_environmentType = EnvironmentType;
			}
			return m_queueRequirementSystemInfo;
		}
	}

	public bool HasLeavingPenalty(GameType gameType)
	{
		GameTypeAvailability gameTypeAvailability;
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && gameTypeAvailability.GameLeavingPenalty != null)
		{
			return gameTypeAvailability.GameLeavingPenalty.PointsGainedForLeaving > 0f;
		}
		return false;
	}

	public LocalizationPayload GetBlockingQueueRestriction(GameType gameType)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails = new QueueBlockOutReasonDetails();
		return GetBlockingQueueRestriction(gameType, out queueBlockOutReasonDetails);
	}

	public LocalizationPayload GetBlockingQueueRestriction(GameType gameType, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		if (GameTypeAvailabilies != null)
		{
			IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
			if (queueRequirementApplicant == null)
			{
				Log.Warning("Player can not be seen as a QueueRequirementApplicant!");
				return LocalizationPayload.Create("UnableToChangeGameMode", "Global");
			}
			bool flag;
			if (GroupInfo != null)
			{
				flag = GroupInfo.InAGroup;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			RequirementMessageContext context = (!flag2) ? RequirementMessageContext.SoloQueueing : RequirementMessageContext.GroupQueueing;
			GameTypeAvailability gameTypeAvailability;
			if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
			{
				if (!gameTypeAvailability.Requirements.IsNullOrEmpty())
				{
					if (!gameTypeAvailability.IsActive)
					{
						return LocalizationPayload.Create((gameType != GameType.Ranked) ? "GameModeUnavailable@Global" : "RankedNotYetAvailable@RankMode");
					}
					IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
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
									Details.CausedBySelf = true;
									return result;
								}
								flag3 = true;
							}
							IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
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
			PersistedAccountData playerAccountData = GetPlayerAccountData();
			if (playerAccountData != null)
			{
				if (m_ourQueueApplicant == null)
				{
					m_ourQueueApplicant = new OurQueueApplicant
					{
						m_pad = playerAccountData,
						AccessLevel = ClientAccessLevel
					};
				}
				else
				{
					m_ourQueueApplicant.m_pad = playerAccountData;
					m_ourQueueApplicant.AccessLevel = ClientAccessLevel;
				}
				return m_ourQueueApplicant;
			}
			return null;
		}
	}

	private GroupQueueApplicant ScratchGroupQueueApplicant
	{
		get
		{
			if (m_scratchGroupQueueApplicant == null)
			{
				m_scratchGroupQueueApplicant = new GroupQueueApplicant();
			}
			return m_scratchGroupQueueApplicant;
		}
	}

	public IEnumerable<IQueueRequirementApplicant> GroupMembersAsQueueApplicants
	{
		get
		{
			bool flag = false;
			List<UpdateGroupMemberData>.Enumerator enumerator;
			if (GroupInfo.Members.IsNullOrEmpty())
			{
				yield break;
			}
			enumerator = GroupInfo.Members.GetEnumerator();
				
			try
			{
				while (enumerator.MoveNext())
				{
					UpdateGroupMemberData member = enumerator.Current;
					yield return new GroupQueueApplicant
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
					enumerator.Dispose();
				}
			}
		}
	}

	public bool MeetsAllQueueRequirements(GameType gameType)
	{
		bool result = false;
		if (GameTypeAvailabilies != null)
		{
			IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
			if (queueRequirementApplicant != null)
			{
				result = true;
				GameTypeAvailability gameTypeAvailability;
				if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
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
							if (!list.IsNullOrEmpty())
							{
								IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
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
										GroupQueueApplicant scratchGroupQueueApplicant = ScratchGroupQueueApplicant;
										foreach (UpdateGroupMemberData member in GroupInfo.Members)
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
		if (!GameTypeAvailabilies.IsNullOrEmpty())
		{
			GameTypeAvailability gameTypeAvailability;
			if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.QueueableGroupSizes.IsNullOrEmpty())
			{
				RequirementCollection requirementCollection;
				if (gameTypeAvailability.QueueableGroupSizes.TryGetValue(groupSize, out requirementCollection))
				{
					if (!requirementCollection.IsNullOrEmpty())
					{
						IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
						IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
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
										IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
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
		if (GameTypeAvailabilies.IsNullOrEmpty())
		{
			Log.Warning("No valid GameTypeAvailabilites loaded yet for {0}", gameType);
			return null;
		}
		GameTypeAvailability gameTypeAvailability;
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.QueueableGroupSizes.IsNullOrEmpty())
		{
			RequirementCollection requirementCollection;
			if (!gameTypeAvailability.QueueableGroupSizes.TryGetValue(groupSize, out requirementCollection))
			{
				return LocalizationPayload.Create("BadGroupSizeForQueue", "Matchmaking", LocalizationArg_Int32.Create(groupSize));
			}
			if (!requirementCollection.IsNullOrEmpty())
			{
				IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
				IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
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
								IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
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
									return LocalizationPayload.Create("GroupSizeRequirementFailure", "matchmaking", localizationArg_Int, localizationArg_GameType, localizationArg_LocalizationPayload);
								}
								return LocalizationPayload.Create("SoloSizeRequirementFailure", "matchmaking", localizationArg_GameType, localizationArg_LocalizationPayload);
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
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.SubTypes.IsNullOrEmpty())
		{
			return GameSubType.CalculatePivotSubTypes(currentMask, GameSubType.SubTypeMods.AntiSocial, gameTypeAvailability.SubTypes);
		}
		return 0;
	}

	public bool IsMapInGameType(GameType gameType, string mapName, out bool isActive)
	{
		bool result = false;
		isActive = false;
		GameTypeAvailability gameTypeAvailability;
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability) && !gameTypeAvailability.SubTypes.IsNullOrEmpty())
		{
			using (List<GameSubType>.Enumerator enumerator = gameTypeAvailability.SubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameSubType gameSubType = enumerator.Current;
					GameMapConfig gameMapConfig = (from p in gameSubType.GameMapConfigs
					where p.Map == mapName
					select p).FirstOrDefault();
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
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
		{
			if (!gameTypeAvailability.SubTypes.IsNullOrEmpty())
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
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SetGameTypeSubMasks(gameType, subGameMask, onResponseCallback);
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
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SetNewSessionLanguage(languageCode);
			}
		}
	}

	public bool IsFreelancerInFreeRotationExtension(CharacterType characterType, GameType gameType, GameSubType gameSubType = null)
	{
		RequirementCollection requirementCollection;
		if (!FreeRotationAdditions.IsNullOrEmpty() && FreeRotationAdditions.TryGetValue(characterType, out requirementCollection))
		{
			IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
			IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
			if (requirementCollection.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, gameSubType))
			{
				return true;
			}
		}
		return false;
	}

	private bool m_discordConnected => DiscordClientInterface.Get().IsConnected;

	private class FreelancerSetQueryInterface : IFreelancerSetQueryInterface
	{
		private FactionWideData m_fwd;

		private GameWideData m_gwd;

		internal FreelancerSetQueryInterface(FactionWideData fwd, GameWideData gwd)
		{
			m_fwd = fwd;
			m_gwd = gwd;
		}

		public HashSet<CharacterType> GetCharacterTypesFromRoles(List<CharacterRole> roles)
		{
			HashSet<CharacterType> retVal = new HashSet<CharacterType>();
			if (!roles.IsNullOrEmpty())
			{
				(from p in m_gwd.m_characterResourceLinks
				where roles.Contains(p.m_characterRole)
				select p).ToList().ForEach(delegate(CharacterResourceLink p)
				{
					retVal.Add(p.m_characterType);
				});
			}
			return retVal;
		}

		public HashSet<CharacterType> GetCharacterTypesFromFractionGroupIds(List<int> groupIds)
		{
			HashSet<CharacterType> retVal = new HashSet<CharacterType>();
			if (!groupIds.IsNullOrEmpty())
			{
				(from p in m_fwd.m_factionGroups
				where groupIds.Contains(p.FactionGroupID)
				select p).ToList().ForEach(delegate(FactionGroup p)
				{
					retVal.UnionWith(p.Characters);
				});
			}
			return retVal;
		}

		public bool DoesCharacterMatchRoles(CharacterType freelancer, List<CharacterRole> roles)
		{
			if (roles.IsNullOrEmpty())
			{
				return false;
			}
			return (from p in m_gwd.m_characterResourceLinks
			where roles.Contains(p.m_characterRole) && p.m_characterType == freelancer
			select p).Count() > 0;
		}

		public bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds)
		{
			bool result;
			if (!groupIds.IsNullOrEmpty())
			{
				result = m_fwd.m_factionGroups.Exists(p => p.Characters.Contains(freelancer) && groupIds.Contains(p.FactionGroupID));
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

		internal FreelancerSetQueryInterface m_freelancerSetQueryInterface;

		public EnvironmentType GetEnvironmentType()
		{
			return m_environmentType;
		}

		public DateTime GetCurrentUTCTime()
		{
			return m_serverUtcTime;
		}

		public IEnumerable<GameType> GetGameTypes()
		{
			return m_gtas.Keys;
		}

		public GameLeavingPenalty GetGameLeavingPenaltyForGameType(GameType gameType)
		{
			GameTypeAvailability gameTypeAvailability;
			if (m_gtas.TryGetValue(gameType, out gameTypeAvailability))
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
			if (!m_gtas.TryGetValue(gameType, out gta))
			{
				yield break;
			}
			if (gta.Requirements.IsNullOrEmpty())
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
		}

		public bool IsCharacterAllowed(CharacterType ct, GameType gameType, GameSubType gst)
		{
			GameManager gameManager = GameManager.Get();
			return gameManager.IsCharacterAllowedForPlayers(ct) && gameManager.IsCharacterAllowedForGameType(ct, gameType, gst, FreelancerSetQueryInterface);
		}

		public IFreelancerSetQueryInterface FreelancerSetQueryInterface
		{
			get
			{
				if (m_freelancerSetQueryInterface == null)
				{
					m_freelancerSetQueryInterface = new FreelancerSetQueryInterface(FactionWideData.Get(), GameWideData.Get());
				}
				return m_freelancerSetQueryInterface;
			}
		}

		public List<SeasonTemplate> Seasons => SeasonWideData.Get().m_seasons;
	}

	private class OurQueueApplicant : IQueueRequirementApplicant
	{
		internal PersistedAccountData m_pad;

		public int TotalMatches => m_pad.ExperienceComponent.Matches;

		public int VsHumanMatches => m_pad.ExperienceComponent.VsHumanMatches;

		public CharacterType CharacterType => m_pad.AccountComponent.LastCharacter;

		public int CharacterMatches
		{
			get
			{
				if (m_pad.CharacterData.ContainsKey(CharacterType))
				{
					return m_pad.CharacterData[CharacterType].ExperienceComponent.Matches;
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
				cgm = Get();
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
			}
		}

		public ClientAccessLevel AccessLevel { get; set; }

		public int SeasonLevel => m_pad.SeasonLevel;

		public LocalizationArg_Handle LocalizedHandle => LocalizationArg_Handle.Create(m_pad.Handle);

		public float GameLeavingPoints => m_pad.AdminComponent.GameLeavingPoints;

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			ClientGameManager clientGameManager = Get();
			GameType selectedQueueType = clientGameManager.GroupInfo.SelectedQueueType;
			return clientGameManager.IsCharacterAvailable(ct, selectedQueueType);
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			return m_pad.GetReactorLevel(seasons);
		}
	}

	private class GroupQueueApplicant : IQueueRequirementApplicant
	{
		internal UpdateGroupMemberData m_member;

		public int TotalMatches => int.MaxValue;

		public int VsHumanMatches => int.MaxValue;

		public CharacterType CharacterType => m_member.MemberDisplayCharacter;

		public int CharacterMatches => int.MaxValue;

		public ClientAccessLevel AccessLevel
		{
			get
			{
				ClientAccessLevel result;
				if (m_member.HasFullAccess)
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

		public int SeasonLevel => int.MaxValue;

		public LocalizationArg_Handle LocalizedHandle => LocalizationArg_Handle.Create(m_member.MemberDisplayName);

		public float GameLeavingPoints => m_member.GameLeavingPoints;

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
			}
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			return int.MaxValue;
		}
	}
}
