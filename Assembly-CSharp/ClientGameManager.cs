using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class ClientGameManager : MonoBehaviour
{
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
				m_gwd.m_characterResourceLinks.Where((CharacterResourceLink p) => roles.Contains(p.m_characterRole)).ToList().ForEach(delegate(CharacterResourceLink p)
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
				m_fwd.m_factionGroups.Where((FactionGroup p) => groupIds.Contains(p.FactionGroupID)).ToList().ForEach(delegate(FactionGroup p)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			return m_gwd.m_characterResourceLinks.Where((CharacterResourceLink p) => roles.Contains(p.m_characterRole) && p.m_characterType == freelancer).Count() > 0;
		}

		public bool DoesCharacterMatchFractionGroupIds(CharacterType freelancer, List<int> groupIds)
		{
			int result;
			if (!groupIds.IsNullOrEmpty())
			{
				result = (m_fwd.m_factionGroups.Exists((FactionGroup p) => p.Characters.Contains(freelancer) && groupIds.Contains(p.FactionGroupID)) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	private class ClientGameManagerRequirementSystemInfo : IQueueRequirementSystemInfo
	{
		internal Dictionary<GameType, GameTypeAvailability> m_gtas;

		internal DateTime m_serverUtcTime;

		internal EnvironmentType m_environmentType;

		internal FreelancerSetQueryInterface m_freelancerSetQueryInterface;

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
			if (m_gtas.TryGetValue(gameType, out GameTypeAvailability value))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return value.GameLeavingPenalty;
					}
				}
			}
			return null;
		}

		public IEnumerable<QueueRequirement> GetQueueRequirements(GameType gameType)
		{
			if (!m_gtas.TryGetValue(gameType, out GameTypeAvailability gta))
			{
				yield break;
			}
			while (true)
			{
				if (!gta.Requirements.IsNullOrEmpty())
				{
					while (true)
					{
						IEnumerator<QueueRequirement> enumerator = gta.Requirements.GetEnumerator();
						try
						{
							if (enumerator.MoveNext())
							{
								yield return enumerator.Current;
								/*Error: Unable to find new state assignment for yield return*/;
							}
							while (true)
							{
								switch (5)
								{
								default:
									yield break;
								case 0:
									break;
								}
							}
						}
						finally
						{
							if (enumerator != null)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										enumerator.Dispose();
										goto end_IL_00f8;
									}
								}
							}
							goto end_IL_00f8;
							IL_00fb:
							switch (5)
							{
							default:
								goto end_IL_00f8;
							case 0:
								goto IL_00fb;
							}
							end_IL_00f8:;
						}
					}
				}
				yield break;
			}
		}

		public bool IsCharacterAllowed(CharacterType ct, GameType gameType, GameSubType gst)
		{
			GameManager gameManager = GameManager.Get();
			return gameManager.IsCharacterAllowedForPlayers(ct) && gameManager.IsCharacterAllowedForGameType(ct, gameType, gst, FreelancerSetQueryInterface);
		}
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
				ClientGameManager cgm = Get();
				GameType gameType = cgm.GroupInfo.SelectedQueueType;
				IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						CharacterType ct = (CharacterType)enumerator.Current;
						if (cgm.IsCharacterAvailable(ct, gameType))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									yield return ct;
									/*Error: Unable to find new state assignment for yield return*/;
								}
							}
						}
					}
					while (true)
					{
						switch (7)
						{
						default:
							yield break;
						case 0:
							break;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = disposable = (enumerator as IDisposable);
					if (disposable != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								disposable2.Dispose();
								goto end_IL_00fd;
							}
						}
					}
					goto end_IL_00fd;
					IL_0100:
					switch (5)
					{
					default:
						goto end_IL_00fd;
					case 0:
						goto IL_0100;
					}
					end_IL_00fd:;
				}
			}
		}

		public ClientAccessLevel AccessLevel
		{
			get;
			set;
		}

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
				int result;
				if (m_member.HasFullAccess)
				{
					result = 22;
				}
				else
				{
					result = 20;
				}
				return (ClientAccessLevel)result;
			}
		}

		public int SeasonLevel => int.MaxValue;

		public LocalizationArg_Handle LocalizedHandle => LocalizationArg_Handle.Create(m_member.MemberDisplayName);

		public float GameLeavingPoints => m_member.GameLeavingPoints;

		public IEnumerable<CharacterType> AvailableCharacters
		{
			get
			{
				IEnumerator enumerator = Enum.GetValues(typeof(CharacterType)).GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						yield return (CharacterType)enumerator.Current;
						/*Error: Unable to find new state assignment for yield return*/;
					}
					while (true)
					{
						switch (4)
						{
						default:
							yield break;
						case 0:
							break;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = disposable = (enumerator as IDisposable);
					if (disposable != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								disposable2.Dispose();
								goto end_IL_00b5;
							}
						}
					}
					goto end_IL_00b5;
					IL_00b8:
					switch (7)
					{
					default:
						goto end_IL_00b5;
					case 0:
						goto IL_00b8;
					}
					end_IL_00b5:;
				}
			}
		}

		public bool IsCharacterTypeAvailable(CharacterType ct)
		{
			return true;
		}

		public int GetReactorLevel(List<SeasonTemplate> seasons)
		{
			return int.MaxValue;
		}
	}

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

	public LobbyGameClientInterface LobbyInterface => m_lobbyGameClientInterface;

	public LobbySessionInfo SessionInfo => (m_lobbyGameClientInterface == null) ? null : m_lobbyGameClientInterface.SessionInfo;

	public string Handle => (m_lobbyGameClientInterface == null || m_lobbyGameClientInterface.SessionInfo == null) ? null : m_lobbyGameClientInterface.SessionInfo.Handle;

	public long AccountId
	{
		get
		{
			long result;
			if (m_lobbyGameClientInterface != null)
			{
				if (m_lobbyGameClientInterface.SessionInfo != null)
				{
					result = m_lobbyGameClientInterface.SessionInfo.AccountId;
					goto IL_004a;
				}
			}
			result = -1L;
			goto IL_004a;
			IL_004a:
			return result;
		}
	}

	public bool IsConnectedToLobbyServer
	{
		get
		{
			int result;
			if (m_lobbyGameClientInterface != null)
			{
				result = (m_lobbyGameClientInterface.IsConnected ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public bool IsRegistered
	{
		get;
		private set;
	}

	public bool IsReady
	{
		get;
		private set;
	}

	public bool AllowRelogin
	{
		get;
		private set;
	}

	public NetworkClient Client
	{
		get
		{
			object result;
			if (NetworkManager.singleton == null)
			{
				result = null;
			}
			else
			{
				result = NetworkManager.singleton.client;
			}
			return (NetworkClient)result;
		}
	}

	public NetworkConnection Connection
	{
		get
		{
			object result;
			if (Client == null)
			{
				result = null;
			}
			else
			{
				result = Client.connection;
			}
			return (NetworkConnection)result;
		}
	}

	public MyNetworkClientConnection MyConnection => (Client != null) ? (Client.connection as MyNetworkClientConnection) : null;

	public bool IsConnectedToGameServer
	{
		get
		{
			int result;
			if (MyConnection != null)
			{
				result = (MyConnection.isConnected ? 1 : 0);
			}
			else if (Connection != null)
			{
				result = (Connection.isConnected ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public bool IsRegisteredToGameServer
	{
		get;
		private set;
	}

	public CurrencyWallet PlayerWallet
	{
		get;
		private set;
	}

	public FriendList FriendList
	{
		get;
		private set;
	}

	public bool IsFriendListInitialized
	{
		get;
		private set;
	}

	public Dictionary<int, long> FactionScores
	{
		get;
		private set;
	}

	public int ActiveFactionCompetition
	{
		get;
		private set;
	}

	public bool PlayerObjectStartedOnClient
	{
		get;
		set;
	}

	public bool InGameUIActivated
	{
		get;
		set;
	}

	public bool VisualSceneLoaded
	{
		get;
		set;
	}

	public bool DesignSceneStarted
	{
		get;
		set;
	}

	public bool IsLoading => m_loading;

	public bool IsFastForward
	{
		get
		{
			int result;
			if (!m_withinReconnectReplay)
			{
				if ((bool)ReplayPlayManager.Get())
				{
					result = (ReplayPlayManager.Get().IsFastForward() ? 1 : 0);
				}
				else
				{
					result = 0;
				}
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	public bool IsReconnecting => m_withinReconnect;

	public bool IsReconnectingInstantly => m_withinReconnectInstantly;

	public bool SpectatorHideAbilityTargeter
	{
		get;
		set;
	}

	public bool IsSpectator
	{
		get
		{
			int result;
			if (PlayerInfo != null)
			{
				result = ((PlayerInfo.TeamId == Team.Spectator) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public int WaitingForModSelectResponse
	{
		get;
		private set;
	}

	public int WaitingForCardSelectResponse
	{
		get;
		private set;
	}

	public bool AllowBadges
	{
		get;
		private set;
	}

	public int NewPlayerPvPQueueDuration
	{
		get;
		private set;
	}

	public GameResult GameResult => m_gameResult;

	public bool Reconnected => m_reconnected;

	public bool Observer => m_observer;

	public TierPlacement TierChangeMin
	{
		get;
		private set;
	}

	public TierPlacement TierChangeMax
	{
		get;
		private set;
	}

	public TierPlacement TierCurrent
	{
		get;
		private set;
	}

	public LobbyAlertMissionDataNotification AlertMissionsData
	{
		get;
		private set;
	}

	public Dictionary<int, SeasonChapterQuests> SeasonChapterQuests
	{
		get;
		private set;
	}

	private List<CharacterVisualInfo> WaitingForSkinSelectResponse
	{
		get;
		set;
	}

	private FactionLoginRewardNotification LoginRewardNotification
	{
		get;
		set;
	}

	private List<QuestCompleteNotification> LoginQuestCompleteNotifications
	{
		get;
		set;
	}

	public DateTime QueueEntryTime => OurQueueEntryTime;

	public List<LobbyGameInfo> CustomGameInfos
	{
		get;
		private set;
	}

	public LobbyPlayerGroupInfo GroupInfo
	{
		get;
		private set;
	}

	public EnvironmentType EnvironmentType
	{
		get;
		private set;
	}

	public ServerMessageOverrides ServerMessageOverrides
	{
		get;
		private set;
	}

	public ServerLockState ServerLockState
	{
		get;
		private set;
	}

	public ConnectionQueueInfo ConnectionQueueInfo
	{
		get;
		private set;
	}

	public AuthTicket AuthTicket => HydrogenConfig.Get().Ticket;

	public ClientAccessLevel ClientAccessLevel => (PlayerInfo == null) ? m_clientAccessLevel : PlayerInfo.EffectiveClientAccessLevel;

	public bool HasPurchasedGame
	{
		get;
		private set;
	}

	public int HighestPurchasedGamePack
	{
		get;
		private set;
	}

	public DateTime ServerUtcTime
	{
		get;
		private set;
	}

	public DateTime ServerPacificTime
	{
		get;
		private set;
	}

	public DateTime ClientUtcTime
	{
		get;
		private set;
	}

	public TimeSpan TimeOffset
	{
		get;
		private set;
	}

	public string CommerceURL
	{
		get;
		private set;
	}

	public LobbyGameInfo GameInfo
	{
		get
		{
			object result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().GameInfo;
			}
			else
			{
				result = null;
			}
			return (LobbyGameInfo)result;
		}
	}

	public LobbyPlayerInfo PlayerInfo
	{
		get
		{
			object result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().PlayerInfo;
			}
			else
			{
				result = null;
			}
			return (LobbyPlayerInfo)result;
		}
	}

	public LobbyTeamInfo TeamInfo
	{
		get
		{
			object result;
			if (GameManager.Get() != null)
			{
				result = GameManager.Get().TeamInfo;
			}
			else
			{
				result = null;
			}
			return (LobbyTeamInfo)result;
		}
	}

	public bool IsServerQueued
	{
		get
		{
			int result;
			if (ClientAccessLevel == ClientAccessLevel.Queued)
			{
				result = ((ConnectionQueueInfo != null) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public bool IsServerLocked => ClientAccessLevel == ClientAccessLevel.Locked;

	public bool PurchasingMod
	{
		get;
		private set;
	}

	public int ModAttemptingToPurchase
	{
		get;
		private set;
	}

	public int NumCharacterResourcesCurrentlyLoading
	{
		get
		{
			if (m_loadingCharacterResources == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			return m_loadingCharacterResources.Count;
		}
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
			if (GroupInfo.Members.IsNullOrEmpty())
			{
				yield break;
			}
			while (true)
			{
				using (List<UpdateGroupMemberData>.Enumerator enumerator = GroupInfo.Members.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						UpdateGroupMemberData member = enumerator.Current;
						yield return new GroupQueueApplicant
						{
							m_member = member
						};
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
			}
		}
	}

	private bool m_discordConnected => DiscordClientInterface.Get().IsConnected;

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
				action = Interlocked.CompareExchange(ref this.OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<RegisterGameClientResponse> action = this.OnConnectedToLobbyServerHolder;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectedToLobbyServerHolder, (Action<RegisterGameClientResponse>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<string> action = this.OnDisconnectedFromLobbyServerHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnDisconnectedFromLobbyServerHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotificationHolder;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyServerReadyNotificationHolder, (Action<LobbyServerReadyNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotificationHolder;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyStatusNotificationHolder, (Action<LobbyStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotificationHolder;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyCustomGamesNotificationHolder, (Action<LobbyCustomGamesNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

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
				action = Interlocked.CompareExchange(ref this.OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotificationHolder;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQueueStatusNotificationHolder, (Action<MatchmakingQueueStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnQueueEnteredHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = this.OnQueueEnteredHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQueueEnteredHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnQueueLeftHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnQueueLeftHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQueueLeftHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotificationHolder;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameAssignmentNotificationHolder, (Action<GameAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotificationHolder;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameInfoNotificationHolder, (Action<GameInfoNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ServerLockState, ServerLockState> action = this.OnLobbyServerLockStateChangeHolder;
			Action<ServerLockState, ServerLockState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyServerLockStateChangeHolder, (Action<ServerLockState, ServerLockState>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<ClientAccessLevel, ClientAccessLevel> action = this.OnLobbyServerClientAccessLevelChangeHolder;
			Action<ClientAccessLevel, ClientAccessLevel> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyServerClientAccessLevelChangeHolder, (Action<ClientAccessLevel, ClientAccessLevel>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyGameplayOverrides> action = this.OnLobbyGameplayOverridesChangeHolder;
			Action<LobbyGameplayOverrides> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyGameplayOverridesChangeHolder, (Action<LobbyGameplayOverrides>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<CurrencyData> action = this.OnBankBalanceChangeHolder;
			Action<CurrencyData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnBankBalanceChangeHolder, (Action<CurrencyData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

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
				action = Interlocked.CompareExchange(ref this.OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<PersistedAccountData> action = this.OnAccountDataUpdatedHolder;
			Action<PersistedAccountData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnAccountDataUpdatedHolder, (Action<PersistedAccountData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<PersistedCharacterData> action = this.OnCharacterDataUpdatedHolder;
			Action<PersistedCharacterData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnCharacterDataUpdatedHolder, (Action<PersistedCharacterData>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<InventoryComponent> action = this.OnInventoryComponentUpdatedHolder;
			Action<InventoryComponent> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnInventoryComponentUpdatedHolder, (Action<InventoryComponent>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<ChatNotification> action = this.OnChatNotificationHolder;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnChatNotificationHolder, (Action<ChatNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<SetDevTagResponse> action = this.OnSetDevTagResponseHolder;
			Action<SetDevTagResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSetDevTagResponseHolder, (Action<SetDevTagResponse>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotificationHolder;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnUseOverconNotificationHolder, (Action<UseOverconResponse>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotificationHolder;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnUseGGPackNotificationHolder, (Action<UseGGPackNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGroupUpdateNotificationHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnGroupUpdateNotificationHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGroupUpdateNotificationHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotificationHolder;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFriendStatusNotificationHolder, (Action<FriendStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnPlayerTitleChangeHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<string> action = this.OnPlayerTitleChangeHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnPlayerTitleChangeHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action = this.OnPlayerBannerChangeHolder;
			Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnPlayerBannerChangeHolder, (Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameBalanceVars.PlayerRibbon> action = this.OnPlayerRibbonChangeHolder;
			Action<GameBalanceVars.PlayerRibbon> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnPlayerRibbonChangeHolder, (Action<GameBalanceVars.PlayerRibbon>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<int, bool> action = this.OnLoadingScreenBackgroundToggledHolder;
			Action<int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLoadingScreenBackgroundToggledHolder, (Action<int, bool>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotificationHolder;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQuestCompleteNotificationHolder, (Action<QuestCompleteNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotificationHolder;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMatchResultsNotificationHolder, (Action<MatchResultsNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<int, int> action = this.OnChapterUnlockNotificationHolder;
			Action<int, int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnChapterUnlockNotificationHolder, (Action<int, int>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotificationHolder;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnServerQueueConfigurationUpdateNotificationHolder, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<SeasonStatusNotification> action = this.OnSeasonCompleteNotificationHolder;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSeasonCompleteNotificationHolder, (Action<SeasonStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

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
				action = Interlocked.CompareExchange(ref this.OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotificationHolder;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFactionCompetitionNotificationHolder, (Action<FactionCompetitionNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotificationHolder;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnTrustBoostUsedNotificationHolder, (Action<TrustBoostUsedNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChangeNotificationHolder;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnPlayerFactionContributionChangeNotificationHolder, (Action<PlayerFactionContributionChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotificationHolder;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFactionLoginRewardNotificationHolder, (Action<FactionLoginRewardNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<QuestProgress[]> action = this.OnQuestProgressChangedHolder;
			Action<QuestProgress[]> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQuestProgressChangedHolder, (Action<QuestProgress[]>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnAlertMissionDataChangeHolder;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnAlertMissionDataChangeHolder, (Action<LobbyAlertMissionDataNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<Dictionary<int, SeasonChapterQuests>> action = this.OnSeasonChapterQuestsChangeHolder;
			Action<Dictionary<int, SeasonChapterQuests>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSeasonChapterQuestsChangeHolder, (Action<Dictionary<int, SeasonChapterQuests>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public ClientGameManager()
	{
		
		this.OnConnectedToLobbyServerHolder = delegate
			{
			};
		
		this.OnDisconnectedFromLobbyServerHolder = delegate
			{
			};
		
		this.OnLobbyServerReadyNotificationHolder = delegate
			{
			};
		
		this.OnLobbyStatusNotificationHolder = delegate
			{
			};
		
		this.OnLobbyCustomGamesNotificationHolder = delegate
			{
			};
		this.OnQueueAssignmentNotificationHolder = delegate
		{
		};
		this.OnQueueStatusNotificationHolder = delegate
		{
		};
		
		this.OnQueueEnteredHolder = delegate
			{
			};
		
		this.OnQueueLeftHolder = delegate
			{
			};
		
		this.OnGameAssignmentNotificationHolder = delegate
			{
			};
		
		this.OnGameInfoNotificationHolder = delegate
			{
			};
		
		this.OnLobbyServerLockStateChangeHolder = delegate
			{
			};
		
		this.OnLobbyServerClientAccessLevelChangeHolder = delegate
			{
			};
		this.OnLobbyGameplayOverridesChangeHolder = delegate
		{
		};
		
		this.OnBankBalanceChangeHolder = delegate
			{
			};
		
		this.OnModUnlockedHolder = delegate
			{
			};
		
		this.OnAccountDataUpdatedHolder = delegate
			{
			};
		
		this.OnCharacterDataUpdatedHolder = delegate
			{
			};
		
		this.OnInventoryComponentUpdatedHolder = delegate
			{
			};
		
		this.OnChatNotificationHolder = delegate
			{
			};
		
		this.OnSetDevTagResponseHolder = delegate
			{
			};
		this.OnUseOverconNotificationHolder = delegate
		{
		};
		
		this.OnUseGGPackNotificationHolder = delegate
			{
			};
		
		this.OnGroupUpdateNotificationHolder = delegate
			{
			};
		
		this.OnFriendStatusNotificationHolder = delegate
			{
			};
		
		this.OnPlayerTitleChangeHolder = delegate
			{
			};
		
		this.OnPlayerBannerChangeHolder = delegate
			{
			};
		this.OnPlayerRibbonChangeHolder = delegate
		{
		};
		
		this.OnLoadingScreenBackgroundToggledHolder = delegate
			{
			};
		this.OnQuestCompleteNotificationHolder = delegate
		{
		};
		
		this.OnMatchResultsNotificationHolder = delegate
			{
			};
		
		this.OnChapterUnlockNotificationHolder = delegate
			{
			};
		
		this.OnServerQueueConfigurationUpdateNotificationHolder = delegate
			{
			};
		
		this.OnSeasonCompleteNotificationHolder = delegate
			{
			};
		this.OnChapterCompleteNotificationHolder = delegate
		{
		};
		this.OnFactionCompetitionNotificationHolder = delegate
		{
		};
		this.OnTrustBoostUsedNotificationHolder = delegate
		{
		};
		
		this.OnPlayerFactionContributionChangeNotificationHolder = delegate
			{
			};
		this.OnFactionLoginRewardNotificationHolder = delegate
		{
		};
		
		this.OnQuestProgressChangedHolder = delegate
			{
			};
		this.OnAlertMissionDataChangeHolder = delegate
		{
		};
		this.OnSeasonChapterQuestsChangeHolder = delegate
		{
		};
		OurQueueEntryTime = DateTime.MinValue;
		SoloSubTypeMask = new Dictionary<GameType, ushort>();
		m_loadingProgressUpdateFrequency = 0.5f;
		
	}

	public static ClientGameManager Get()
	{
		return s_instance;
	}

	public bool IsCharacterInFreeRotation(CharacterType characterType, GameType gameType)
	{
		int result;
		if (ClientAccessLevel < ClientAccessLevel.Full)
		{
			result = ((m_loadedPlayerAccountData.AccountComponent.IsCharacterInFreeRotation(characterType) || IsFreelancerInFreeRotationExtension(characterType, gameType)) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsCharacterAvailable(CharacterType characterType, GameType gameType)
	{
		int num;
		if (GameManager.Get() != null)
		{
			num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		CharacterResourceLink characterResourceLink = null;
		int num2 = 0;
		while (true)
		{
			if (num2 < GameWideData.Get().m_characterResourceLinks.Length)
			{
				if (GameWideData.Get().m_characterResourceLinks[num2].m_characterType == characterType)
				{
					characterResourceLink = GameWideData.Get().m_characterResourceLinks[num2];
					break;
				}
				num2++;
				continue;
			}
			break;
		}
		if (characterResourceLink == null)
		{
			goto IL_00b7;
		}
		if (characterResourceLink.m_isHidden)
		{
			if (!flag)
			{
				goto IL_00b7;
			}
		}
		if (!characterResourceLink.m_characterType.IsValidForHumanPreGameSelection())
		{
			return false;
		}
		if (characterResourceLink.m_characterType.IsWillFill())
		{
			if (gameType != GameType.PvP)
			{
				if (gameType != GameType.NewPlayerPvP)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
		}
		PersistedCharacterData playerCharacterData = GetPlayerCharacterData(characterType);
		int result;
		if (ClientAccessLevel < ClientAccessLevel.Full && !characterType.IsWillFill())
		{
			if (gameType != GameType.Practice)
			{
				if (playerCharacterData != null)
				{
					if (playerCharacterData.CharacterComponent.Unlocked)
					{
						goto IL_016a;
					}
				}
				result = (IsCharacterInFreeRotation(characterType, gameType) ? 1 : 0);
				goto IL_016b;
			}
		}
		goto IL_016a;
		IL_00b7:
		return false;
		IL_016a:
		result = 1;
		goto IL_016b;
		IL_016b:
		return (byte)result != 0;
	}

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

	public bool HasDeveloperAccess()
	{
		int result;
		if (AuthTicket != null)
		{
			if (AuthTicket.HasEntitlement("DEVELOPER_ACCESS"))
			{
				result = 1;
				goto IL_004d;
			}
		}
		result = ((ClientAccessLevel >= ClientAccessLevel.Admin) ? 1 : 0);
		goto IL_004d;
		IL_004d:
		return (byte)result != 0;
	}

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
		m_lastReceivedMsgSeqNum = 0u;
		m_lastSentMsgSeqNum = 0u;
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
		int num;
		if (MyConnection != null)
		{
			num = ((PlayerInfo != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num != 0)
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
		bool flag = true;
		if (!(GameManager.Get() == null))
		{
			if (GameManager.Get().GameInfo != null && GameManager.Get().GameInfo.GameConfig != null)
			{
				if (GameManager.Get().GameInfo.GameConfig.GameType == GameType.Tutorial)
				{
					goto IL_01f3;
				}
			}
			if (!(UINewReward.Get() == null))
			{
				if (!UIDialogPopupManager.Get().IsDialogBoxOpen())
				{
					if (!UIFrontendLoadingScreen.Get().IsVisible())
					{
						goto IL_01f5;
					}
				}
			}
		}
		goto IL_01f3;
		IL_01f5:
		if (!flag)
		{
			return;
		}
		while (true)
		{
			if (LoginRewardNotification != null)
			{
				UINewReward.Get().NotifyNewTrustReward(LoginRewardNotification.LogInRewardsGiven, -1, string.Empty, true);
				LoginRewardNotification = null;
			}
			if (LoginQuestCompleteNotifications.Count > 0)
			{
				foreach (QuestCompleteNotification loginQuestCompleteNotification in LoginQuestCompleteNotifications)
				{
					this.OnQuestCompleteNotificationHolder(loginQuestCompleteNotification);
				}
				LoginQuestCompleteNotifications.Clear();
			}
			return;
		}
		IL_01f3:
		flag = false;
		goto IL_01f5;
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
		ServerMessageOverrides = new ServerMessageOverrides();
		ServerMessageOverrides.MOTDText = string.Empty;
		ServerMessageOverrides.MOTDPopUpText = string.Empty;
		ServerMessageOverrides.ReleaseNotesText = string.Empty;
		ServerMessageOverrides.ReleaseNotesHeader = string.Empty;
		ServerMessageOverrides.ReleaseNotesDescription = string.Empty;
		ServerMessageOverrides.WhatsNewText = string.Empty;
		ServerMessageOverrides.WhatsNewHeader = string.Empty;
		ServerMessageOverrides.WhatsNewDescription = string.Empty;
		ServerMessageOverrides.LockScreenText = string.Empty;
		ServerMessageOverrides.LockScreenButtonText = string.Empty;
		ServerMessageOverrides.FreeUpsellExternalBrowserUrl = string.Empty;
		ServerMessageOverrides.FreeUpsellExternalBrowserSteamUrl = string.Empty;
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
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							KeyValuePair<ushort, GameSubType> current = enumerator.Current;
							if (current.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										num = current.Key;
										goto end_IL_0068;
									}
								}
							}
							if (!current.Value.HasMod(GameSubType.SubTypeMods.NotCheckedByDefault))
							{
								num = (ushort)(num | current.Key);
							}
						}
						end_IL_0068:;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							if (hydrogenConfig.PlatformConfig.AllowRequestTickets)
							{
								if (!hydrogenConfig.PlatformUserName.IsNullOrEmpty() && !hydrogenConfig.PlatformPassword.IsNullOrEmpty())
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											hydrogenConfig.Ticket = AuthTicket.CreateRequestTicket(hydrogenConfig.PlatformUserName, hydrogenConfig.PlatformPassword, "Client");
											goto end_IL_0037;
										}
									}
								}
							}
							if (hydrogenConfig.PlatformConfig.AllowFakeTickets)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										hydrogenConfig.Ticket = AuthTicket.CreateFakeTicket(hydrogenConfig.SystemUserName, "Client", 0, "ADMIN_ACCESS;GAME_OWNERSHIP");
										goto end_IL_0037;
									}
								}
							}
							goto end_IL_0037;
						}
					}
				}
				end_IL_0037:;
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				hydrogenConfig.Ticket = null;
			}
		}
		if (hydrogenConfig.Ticket == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					throw new Exception("Could not load auth ticket");
				}
			}
		}
		Log.Info("Connecting to lobby server from {0} as {1} / {2} [{3}]", hydrogenConfig.HostName, hydrogenConfig.Ticket.UserName, hydrogenConfig.Ticket.Handle, hydrogenConfig.Ticket.AccountId);
		ClearLobbyState();
		Region region = Region.US;
		region = Options_UI.GetRegion();
		m_lobbyGameClientInterface = new LobbyGameClientInterface();
		m_lobbyGameClientInterface.Initialize(hydrogenConfig.DirectoryServerAddress, hydrogenConfig.Ticket, region, hydrogenConfig.Language, hydrogenConfig.ProcessType, hydrogenConfig.PreferredLobbyServerIndex);
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.Disconnect();
			m_lobbyGameClientInterface = null;
			return;
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
			Client.UnregisterHandler(52);
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					m_lobbyGameClientInterface.UnsubscribeFromCustomGames();
					return;
				}
			}
			return;
		}
	}

	public void JoinQueue(GameType gameType, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, Action<JoinMatchmakingQueueResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			int num;
			if (allyDifficulty.HasValue)
			{
				num = (int)allyDifficulty.Value;
			}
			else
			{
				num = 3;
			}
			BotDifficulty allyBotDifficulty = (BotDifficulty)num;
			int num2;
			if (enemyDifficulty.HasValue)
			{
				num2 = (int)enemyDifficulty.Value;
			}
			else
			{
				num2 = 1;
			}
			BotDifficulty enemyBotDifficulty = (BotDifficulty)num2;
			m_lobbyGameClientInterface.JoinQueue(gameType, allyBotDifficulty, enemyBotDifficulty, onResponseCallback);
		}
		else
		{
			JoinMatchmakingQueueResponse joinMatchmakingQueueResponse = new JoinMatchmakingQueueResponse();
			joinMatchmakingQueueResponse.Success = false;
			joinMatchmakingQueueResponse.ErrorMessage = "Not connected to Lobby.\nPlease restart client.";
			JoinMatchmakingQueueResponse obj = joinMatchmakingQueueResponse;
			onResponseCallback(obj);
		}
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.LeaveQueue(onResponseCallback);
			return;
		}
	}

	public void CreateGame(LobbyGameConfig gameConfig, ReadyState readyState, BotDifficulty selectedBotSkillTeamA, BotDifficulty selectedBotSkillTeamB, Action<CreateGameResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			string processCode = null;
			if (gameConfig.InstanceSubTypeBit == 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						Dictionary<ushort, GameSubType> gameTypeSubTypes = GetGameTypeSubTypes(gameConfig.GameType);
						if (!gameTypeSubTypes.IsNullOrEmpty())
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
								{
									if (gameTypeSubTypes.Count == 1)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												gameConfig.InstanceSubTypeBit = gameTypeSubTypes.First().Key;
												Log.Warning("CreateGame() called without setting InstanceSubTypeIndex. Forcing it to use the only viable one ({0}: 0x{1:x4}: {2}), but the calling code should consult all possible choices, because although it might currently be configured to only have one choice, that list can be changed dynamically on a running server to be any length.", gameConfig.GameType, gameConfig.InstanceSubTypeBit, gameTypeSubTypes.First().Value.GetNameAsPayload().ToString());
												m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
												return;
											}
										}
									}
									List<KeyValuePair<ushort, GameSubType>> pstAsList = gameTypeSubTypes.ToList();
									UIDialogPopupManager.OpenTwoButtonDialog("Brutal Hack", "TODO: The calling code did not pick a sub-type for this game type. Please chose:", StringUtil.TR(pstAsList[0].Value.LocalizedName), StringUtil.TR(pstAsList[1].Value.LocalizedName), delegate
									{
										gameConfig.InstanceSubTypeBit = pstAsList[0].Key;
										m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
									}, delegate
									{
										gameConfig.InstanceSubTypeBit = pstAsList[1].Key;
										m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
									});
									return;
								}
								}
							}
						}
						Log.Warning("Huh, why do we not know about the sub-types of game type {0}?", gameConfig.GameType);
						gameConfig.InstanceSubTypeBit = 1;
						m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
						return;
					}
					}
				}
			}
			m_lobbyGameClientInterface.CreateGame(gameConfig, readyState, processCode, onResponseCallback, selectedBotSkillTeamA, selectedBotSkillTeamB);
			return;
		}
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.JoinGame(gameInfo, asSpectator, onResponseCallback);
			return;
		}
	}

	public void LeaveGame(bool isPermanent, GameResult gameResult)
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (ReplayPlayManager.Get() != null && ReplayPlayManager.Get().IsPlayback())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Info("Leaving replay");
					m_lobbyGameClientInterface.Replay_RemoveFromGame();
					return;
				}
			}
		}
		if (gameManager.GameInfo == null)
		{
			return;
		}
		while (true)
		{
			if (gameManager.GameInfo.GameServerProcessCode.IsNullOrEmpty())
			{
				return;
			}
			while (true)
			{
				if (m_gameResult != 0)
				{
					return;
				}
				while (true)
				{
					object[] array = new object[2];
					object obj;
					if (isPermanent)
					{
						obj = "permanently";
					}
					else
					{
						obj = "temporarily";
					}
					array[0] = obj;
					array[1] = gameResult;
					Log.Info("Leaving game {0} with result {1}", array);
					m_gameResult = gameResult;
					if (m_lobbyGameClientInterface != null)
					{
						m_lobbyGameClientInterface.LeaveGame(isPermanent, gameResult, delegate(LeaveGameResponse response)
						{
							if (!response.Success)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
									{
										string text = (gameManager.GameInfo == null) ? string.Empty : gameManager.GameInfo.Name;
										string text2 = $"Failed to leave game: {response.ErrorMessage}";
										TextConsole.Get().Write(text2);
										Log.Warning("Request to leave game {0} failed: {1}", text, response.ErrorMessage);
										return;
									}
									}
								}
							}
						});
					}
					if (!NetworkClient.active)
					{
						return;
					}
					while (true)
					{
						if (Client == null)
						{
							return;
						}
						while (true)
						{
							if (!Client.isConnected)
							{
								return;
							}
							while (true)
							{
								if (NetworkServer.active)
								{
									return;
								}
								while (true)
								{
									GameManager.LeaveGameNotification leaveGameNotification = new GameManager.LeaveGameNotification();
									leaveGameNotification.PlayerId = GameManager.Get().PlayerInfo.PlayerId;
									leaveGameNotification.IsPermanent = isPermanent;
									leaveGameNotification.GameResult = gameResult;
									Client.SetMaxDelay(0f);
									if (!Client.Send(67, leaveGameNotification))
									{
										Log.Error("Failed to send LeaveGameNotification");
									}
									Client.Disconnect();
									return;
								}
							}
						}
					}
				}
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, stats, null, onResponseCallback);
			return;
		}
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.CalculateFreelancerStats(bucketType, characterType, null, null, onResponseCallback);
			return;
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
						goto IL_013a;
					}
				}
				m_lobbyGameClientInterface.UpdateGameCheats(gameOptionFlag, playerGameOptionFlag);
			}
			goto IL_013a;
		}
		Log.Warning("m_lobbyGameClientInterface == null");
		goto IL_0243;
		IL_0243:
		if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				GameManager.Get().PlayerInfo.ReadyState = readyState;
			}
			return;
		}
		IL_013a:
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
		if (allyDifficulty.HasValue)
		{
			lobbyPlayerInfoUpdate.AllyDifficulty = allyDifficulty.Value;
		}
		if (enemyDifficulty.HasValue)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						onResponseCallback(response);
						return;
					}
				}
			}
			if (!response.Success)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						object obj;
						if (response.LocalizedFailure == null)
						{
							obj = (response.ErrorMessage.IsNullOrEmpty() ? StringUtil.TR("UnknownErrorTryAgain", "Frontend") : $"{response.ErrorMessage}#NeedsLocalization");
						}
						else
						{
							obj = response.LocalizedFailure.ToString();
						}
						string text = (string)obj;
						TextConsole.Get().Write(new TextConsole.Message
						{
							Text = text,
							MessageType = ConsoleMessageType.SystemMessage
						});
						return;
					}
					}
				}
			}
		});
		goto IL_0243;
	}

	public void UpdateSelectedGameMode(GameType gametype)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.UpdateGroupGameType(gametype, delegate(PlayerGroupInfoUpdateResponse response)
			{
				UICharacterScreen.Get().ReceivedGameTypeChangeResponse();
				if (!response.Success)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							string description;
							if (response.LocalizedFailure != null)
							{
								description = response.LocalizedFailure.ToString();
							}
							else if (!response.ErrorMessage.IsNullOrEmpty())
							{
								description = $"{response.ErrorMessage}#NeedsLocalization";
							}
							else
							{
								description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
							}
							UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingFailure", "Global"), description, StringUtil.TR("Ok", "Global"));
							return;
						}
						}
					}
				}
			});
			return;
		}
	}

	public void UpdateSelectedCharacter(CharacterType character, int playerId = 0)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterType = character;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleCharacterSelectUpdateResponse);
			return;
		}
	}

	private void RecordFailureInCharacterSelectUpdateResponse(PlayerInfoUpdateResponse response, string memberName)
	{
		if (response.LocalizedFailure != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					TextConsole.Get().Write(new TextConsole.Message
					{
						Text = string.Format(StringUtil.TR("FailedMessage", "Global"), response.LocalizedFailure.ToString()),
						MessageType = ConsoleMessageType.SystemMessage
					});
					Log.Error("Lobby Server Error ({0}): {1}", memberName, response.LocalizedFailure.ToString());
					return;
				}
			}
		}
		Log.Error("Lobby Server Error ({0}): {1}", memberName, response.ErrorMessage);
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
		if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()) || response.CharacterInfo == null)
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().NotifyGroupUpdate();
			return;
		}
	}

	public void UpdateLoadouts(List<CharacterLoadout> loadouts, int playerId = 0)
	{
		if (m_lobbyGameClientInterface != null)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			CharacterLoadoutUpdate value = default(CharacterLoadoutUpdate);
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = 0;
			lobbyPlayerInfoUpdate.RankedLoadoutMods = ranked;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleLoadoutUpdateResponse);
			return;
		}
	}

	public void HandleLoadoutUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					RecordFailureInCharacterSelectUpdateResponse(response, "HandleLoadoutUpdateResponse");
					return;
				}
			}
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
		if (!(UIRankedCharacterSelectSettingsPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIRankedCharacterSelectSettingsPanel.Get().NotifyLoadoutUpdate(response);
			return;
		}
	}

	public void UpdateSelectedSkin(CharacterVisualInfo selectedCharacterSkin, int playerId = 0)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterSkin = selectedCharacterSkin;
			WaitingForSkinSelectResponse.Add(selectedCharacterSkin);
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleSkinSelectUpdateResponse);
			return;
		}
	}

	public void HandleSkinSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					RecordFailureInCharacterSelectUpdateResponse(response, "HandleSkinSelectUpdateResponse");
					return;
				}
			}
		}
		if (response.CharacterInfo == null || Get().GroupInfo == null)
		{
			return;
		}
		GetAllPlayerCharacterData()[response.CharacterInfo.CharacterType].CharacterComponent.LastSkin = response.CharacterInfo.CharacterSkin;
		Get().GroupInfo.SetCharacterInfo(response.CharacterInfo, true);
		if (WaitingForSkinSelectResponse.Count > 0)
		{
			if (WaitingForSkinSelectResponse[0].Equals(response.CharacterInfo.CharacterSkin))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						WaitingForSkinSelectResponse.RemoveAt(0);
						return;
					}
				}
			}
		}
		if (UICharacterSelectWorldObjects.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(response.CharacterInfo.CharacterType, 0, string.Empty, response.CharacterInfo.CharacterSkin, false);
					return;
				}
			}
		}
		Log.Warning("Handling skin selection update response when character select is not present");
	}

	public void UpdateSelectedCards(CharacterCardInfo cards, int playerId = 0)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterCards = cards;
			WaitingForCardSelectResponse = m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleCardSelectUpdateResponse);
			return;
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
			while (true)
			{
				RecordFailureInCharacterSelectUpdateResponse(response, "HandleCardSelectUpdateResponse");
				return;
			}
		}
		if (Get().GroupInfo != null && response.CharacterInfo != null)
		{
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				if (GameManager.Get().GameInfo.IsCustomGame)
				{
					if (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
					{
						goto IL_011d;
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
		goto IL_011d;
		IL_011d:
		if (response.CharacterInfo == null)
		{
			return;
		}
		while (true)
		{
			Get().GroupInfo.SetCharacterInfo(response.CharacterInfo);
			PersistedCharacterData playerCharacterData = GetPlayerCharacterData(response.CharacterInfo.CharacterType);
			if (playerCharacterData != null)
			{
				while (true)
				{
					playerCharacterData.CharacterComponent.LastCards = response.CharacterInfo.CharacterCards;
					return;
				}
			}
			return;
		}
	}

	public void ClearWaitingForModResponse()
	{
		WaitingForModSelectResponse = -1;
	}

	public void UpdateSelectedMods(CharacterModInfo mods, int playerId = 0)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.CharacterMods = mods;
			lobbyPlayerInfoUpdate.RankedLoadoutMods = (AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked);
			WaitingForModSelectResponse = m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate, HandleModSelectUpdateResponse);
			return;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					RecordFailureInCharacterSelectUpdateResponse(response, "HandleModSelectUpdateResponse");
					return;
				}
			}
		}
		if (response.CharacterInfo == null)
		{
			return;
		}
		while (true)
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
			if (lastSelectedLoadout.HasValue)
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
			return;
		}
	}

	public void HandleAbilityVfxSwapSelectUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					RecordFailureInCharacterSelectUpdateResponse(response, "HandleAbilityVfxSwapSelectUpdateResponse");
					return;
				}
			}
		}
		if (response.CharacterInfo == null)
		{
			return;
		}
		while (true)
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
			return;
		}
	}

	public void UpdateBotDifficulty(BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty, int playerId = 0)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			LobbyPlayerInfoUpdate lobbyPlayerInfoUpdate = new LobbyPlayerInfoUpdate();
			lobbyPlayerInfoUpdate.PlayerId = playerId;
			lobbyPlayerInfoUpdate.AllyDifficulty = allyDifficulty;
			lobbyPlayerInfoUpdate.EnemyDifficulty = enemyDifficulty;
			m_lobbyGameClientInterface.UpdatePlayerInfo(lobbyPlayerInfoUpdate);
			return;
		}
	}

	public void SendSetRegionRequest(Region region)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendSetRegionRequest(region);
			return;
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendRankedTradeRequest(desiredCharacter, RankedTradeData.TradeActionType._000E);
			return;
		}
	}

	public void SendRankedTradeRequest_StopTrading()
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendRankedTradeRequest(CharacterType.None, RankedTradeData.TradeActionType._0012);
			return;
		}
	}

	public void SendRankedBanRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendRankedBanRequest(type);
			return;
		}
	}

	public void SendRankedSelectRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendRankedSelectionRequest(type);
			return;
		}
	}

	public void SendRankedHoverClickRequest(CharacterType type)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendRankedHoverClickRequest(type);
			return;
		}
	}

	public void UpdateGameInfo(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo)
	{
		LobbyGameInfo lobbyGameInfo = new LobbyGameInfo();
		lobbyGameInfo.GameConfig = gameConfig;
		UpdateGameInfo(lobbyGameInfo, teamInfo);
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.InvitePlayerToGame(playerHandle, onResponseCallback);
			return;
		}
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SpectateGame(playerHandle, onResponseCallback);
			return;
		}
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.RequestCrashReportArchiveName(numArchiveBytes, onResponseCallback);
				}
			}
		}
		return false;
	}

	public bool SendStatusReport(ClientStatusReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.SendStatusReport(report);
				}
			}
		}
		return false;
	}

	public bool SendErrorReport(ClientErrorReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.SendErrorReport(report);
				}
			}
		}
		return false;
	}

	public bool SendErrorSummary(ClientErrorSummary summary)
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.SendErrorSummary(summary);
				}
			}
		}
		return false;
	}

	public bool SendFeedbackReport(ClientFeedbackReport report)
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.SendFeedbackReport(report);
				}
			}
		}
		return false;
	}

	public bool SendPerformanceReport()
	{
		if (m_lobbyGameClientInterface != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					ClientPerformanceReport clientPerformanceReport = new ClientPerformanceReport();
					clientPerformanceReport.PerformanceInfo = ClientPerformanceCollector.Get().Collect();
					return m_lobbyGameClientInterface.SendPerformanceReport(clientPerformanceReport);
				}
				}
			}
		}
		return false;
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		if (m_lobbyGameClientInterface != null)
		{
			return m_lobbyGameClientInterface.SendChatNotification(recipientHandle, messageType, text);
		}
		return false;
	}

	public void SendUseOverconRequest(int id, string overconName, int actorId, int turn)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			m_lobbyGameClientInterface.SendUseOverconRequest(id, overconName, actorId, turn);
			return;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_lobbyGameClientInterface.SendUIActionNotification(context);
				}
			}
		}
		return false;
	}

	private void HandleLobbyCustomGamesNotification(LobbyCustomGamesNotification notification)
	{
		CustomGameInfos = notification.CustomGameInfos;
		this.OnLobbyCustomGamesNotificationHolder(notification);
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
		bool value;
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters;
		if (!GroupInfo.InAGroup)
		{
			GroupInfo.IsLeader = false;
		}
		else if (GroupInfo.InAGroup)
		{
			int num = 0;
			while (true)
			{
				if (num < notification.Members.Count)
				{
					if (notification.Members[num].IsLeader)
					{
						flag = (notification.Members[num].AccountID == GetPlayerAccountData().AccountId);
						break;
					}
					num++;
					continue;
				}
				break;
			}
			GroupInfo.IsLeader = flag;
			if (UICharacterScreen.Get() != null)
			{
				if (notification.GameType == GameType.Coop)
				{
					value = false;
					characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
					if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
					{
						if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
						{
							goto IL_0274;
						}
					}
					characterSelectSceneStateParameters.AllyBotTeammatesClickable = !flag;
					Dictionary<ushort, GameSubType> gameTypeSubTypes = GetGameTypeSubTypes(GameType.Coop);
					IEnumerable<ushort> enumerable = gameTypeSubTypes.Keys.Where((ushort p) => (p & notification.SubTypeMask) != 0);
					if (!enumerable.IsNullOrEmpty())
					{
						if (gameTypeSubTypes.TryGetValue(enumerable.First(), out GameSubType value2) && value2.HasMod(GameSubType.SubTypeMods.AntiSocial))
						{
							value = true;
						}
					}
					goto IL_0274;
				}
				goto IL_02bc;
			}
		}
		else
		{
			if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
			{
				if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
				{
					goto IL_034e;
				}
			}
			if (GroupInfo.SelectedQueueType == GameType.Coop)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					AllyBotTeammatesSelected = false
				});
			}
		}
		goto IL_034e;
		IL_02bc:
		if (!flag)
		{
			UICharacterScreen.Get().UpdateSubTypeMaskChecks(notification.SubTypeMask);
		}
		goto IL_034e;
		IL_03d5:
		if (DiscordClientInterface.IsEnabled)
		{
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				if (!DiscordClientInterface.IsInstalled)
				{
					goto IL_04ad;
				}
			}
			if (GroupInfo.InAGroup)
			{
				if (Options_UI.Get().GetEnableAutoJoinDiscord())
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
		goto IL_04ad;
		IL_04ad:
		this.OnGroupUpdateNotificationHolder();
		return;
		IL_0274:
		characterSelectSceneStateParameters.SelectedEnemyBotDifficulty = (int)notification.EnemyDifficulty;
		characterSelectSceneStateParameters.SelectedAllyBotDifficulty = (int)notification.AllyDifficulty;
		characterSelectSceneStateParameters.AllyBotTeammatesSelected = value;
		UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
		goto IL_02bc;
		IL_034e:
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().DoRefreshFunctions(128);
		}
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
		{
			if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
			{
				goto IL_03d5;
			}
		}
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().DoRefreshFunctions(64);
		}
		goto IL_03d5;
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
		IsFriendListInitialized = true;
		if (notification.FriendList.IsDelta)
		{
			using (Dictionary<long, FriendInfo>.Enumerator enumerator = notification.FriendList.Friends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, FriendInfo> current = enumerator.Current;
					if (FriendList.Friends.ContainsKey(current.Key))
					{
						FriendList.Friends[current.Key] = current.Value;
						if (current.Value.FriendStatus == FriendStatus.Removed)
						{
							FriendList.Friends.Remove(current.Key);
						}
					}
					else
					{
						if (current.Value.FriendStatus != FriendStatus.Friend)
						{
							if (current.Value.FriendStatus != FriendStatus.RequestReceived)
							{
								continue;
							}
						}
						FriendList.Friends.Add(current.Key, current.Value);
					}
				}
			}
		}
		else
		{
			FriendList = notification.FriendList;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					SendGroupSuggestion(false, false, request);
					return;
				}
			}
		}
		string description = string.Format(StringUtil.TR("InviteToGroupWithYou", "Global"), request.SuggesterAccountName, request.SuggestedAccountFullHandle);
		if (m_currentGroupSuggestDialogBox != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SendGroupSuggestion(false, true, request);
					return;
				}
			}
		}
		m_currentGroupSuggestDialogBox = UIDialogPopupManager.OpenPartyInviteDialog(StringUtil.TR("GroupSuggestion", "Global"), description, StringUtil.TR("Invite", "Global"), StringUtil.TR("Reject", "Global"), null, delegate
		{
			SendGroupSuggestion(true, false, request);
		}, delegate
		{
			SendGroupSuggestion(false, false, request);
		});
	}

	private void HandleForceQueueNotification(ForceMatchmakingQueueNotification notification)
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && gameManager.GameInfo != null)
		{
			if (gameManager.GameInfo.GameStatus.IsActiveStatus())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						Log.Error("Lobby should never send us a ForceQueueNotification({0}) when we're in a {1} game!", notification.Action, gameManager.GameInfo.GameStatus);
						return;
					}
				}
			}
		}
		ForceMatchmakingQueueNotification.ActionType action = notification.Action;
		if (action != ForceMatchmakingQueueNotification.ActionType._000E)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (action == ForceMatchmakingQueueNotification.ActionType._0012)
					{
						AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
						return;
					}
					throw new Exception("Unhandled ForceQueueNotification.ActionType");
				}
			}
		}
		if (Get().GroupInfo.InAGroup)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					AppState_GroupCharacterSelect.Get().ForceJoinQueue();
					return;
				}
			}
		}
		AppState_WaitingForGame.Get().Enter();
	}

	private void SendGroupSuggestion(bool bAccepted, bool bBusy, GroupSuggestionRequest request)
	{
		if (bAccepted)
		{
			InviteToGroup(request.SuggestedAccountFullHandle, delegate(GroupInviteResponse r)
			{
				GroupSuggestionResponse groupSuggestionResponse2 = new GroupSuggestionResponse
				{
					SuggesterAccountId = request.SuggesterAccountId
				};
				if (!r.Success)
				{
					string text;
					if (r.LocalizedFailure != null)
					{
						text = r.LocalizedFailure.ToString();
					}
					else if (!r.ErrorMessage.IsNullOrEmpty())
					{
						text = $"Failed: {r.ErrorMessage}#NeedsLocalization";
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
			return;
		}
		GroupSuggestionResponse groupSuggestionResponse = new GroupSuggestionResponse();
		groupSuggestionResponse.SuggesterAccountId = request.SuggesterAccountId;
		groupSuggestionResponse.SuggestionStatus = (bBusy ? GroupSuggestionResponse.Status._000E : GroupSuggestionResponse.Status._001D);
		m_lobbyGameClientInterface.SendMessage(groupSuggestionResponse);
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
		m_lastReceivedMsgSeqNum = 0u;
		m_lastSentMsgSeqNum = 0u;
		m_replay = new Replay();
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			DisconnectFromLobbyServer();
			this.OnConnectedToLobbyServerHolder(response);
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
		this.OnConnectedToLobbyServerHolder(response);
		if (!IsConnectedToGameServer)
		{
			return;
		}
		LobbyGameInfo previousGameInfo;
		while (true)
		{
			TextConsole.Get().Write("Reconnected to lobby server");
			previousGameInfo = GameManager.Get().GameInfo;
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
			return;
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
			this.OnGroupUpdateNotificationHolder();
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
				this.OnInventoryComponentUpdatedHolder(notification.AccountData.InventoryComponent);
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
		this.OnLobbyServerReadyNotificationHolder(notification);
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
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
		string text = $"{arg} ({arg2})";
		Log.Info("Changed Access Level from {0} to {1}", oldLevel.ToString(), text);
		this.OnLobbyServerClientAccessLevelChangeHolder(oldLevel, newLevel);
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnecting", "Disconnected"));
					ConnectToLobbyServer();
					return;
				}
			}
		}
		this.OnDisconnectedFromLobbyServerHolder(lastLobbyErrorMessage);
		int num;
		if (code == CloseStatusCode.PingTimeout)
		{
			num = 15;
		}
		else
		{
			num = 13;
		}
		GameResult gameResult = (GameResult)num;
		GameManager.Get().StopGame(gameResult);
		GameManager.Get().Reset();
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
			this.OnQueueLeftHolder();
			if (matchmakingQueueInfo == null)
			{
				AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
			}
		}
		if (matchmakingQueueInfo != null)
		{
			Log.Info("Assigned to queue {0}", matchmakingQueueInfo.GameType);
			this.OnQueueEnteredHolder();
		}
		UICharacterScreen.Get().DoRefreshFunctions(128);
		NavigationBar.Get().UpdateStatusMessage();
		this.OnQueueAssignmentNotificationHolder(notification);
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.GameplayOverrides != null)
		{
			SetGameplayOverrides(notification.GameplayOverrides);
		}
		if (notification.ErrorReportRate.HasValue)
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
		if (notification.ClientAccessLevel != 0)
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
						goto IL_019a;
					}
				}
			}
			HandleLobbyServerClientAccessLevelChange(clientAccessLevel, ClientAccessLevel);
		}
		goto IL_019a;
		IL_019a:
		if (notification.ServerLockState != 0)
		{
			ServerLockState serverLockState = ServerLockState;
			ServerLockState = notification.ServerLockState;
			if (serverLockState != ServerLockState)
			{
				this.OnLobbyServerLockStateChangeHolder(serverLockState, ServerLockState);
			}
		}
		ConnectionQueueInfo = notification.ConnectionQueueInfo;
		if (notification.UtcNow != default(DateTime))
		{
			if (TimeOffset != notification.TimeOffset)
			{
				TextConsole.Get().Write($"Global Time Offset Is Now: {notification.TimeOffset.ToString()}");
			}
			ServerUtcTime = notification.UtcNow;
			ServerPacificTime = notification.PacificNow;
			ClientUtcTime = DateTime.UtcNow;
			TimeOffset = notification.TimeOffset;
		}
		this.OnLobbyStatusNotificationHolder(notification);
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		SetGameplayOverrides(notification.GameplayOverrides);
	}

	private void SetGameplayOverrides(LobbyGameplayOverrides gameplayOverrides)
	{
		int num;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				num = ((GameManager.Get().GameplayOverrides.EnableCards != gameplayOverrides.EnableCards) ? 1 : 0);
				goto IL_005d;
			}
		}
		num = 0;
		goto IL_005d;
		IL_005d:
		bool flag = (byte)num != 0;
		GameManager.Get().SetGameplayOverrides(gameplayOverrides);
		if (flag)
		{
			if (UICharacterSelectScreenController.Get() != null)
			{
				if (UICharacterSelectCharacterSettingsPanel.Get() != null)
				{
					if (!gameplayOverrides.EnableCards)
					{
						if (UICharacterSelectCharacterSettingsPanel.Get().GetTabPanel() == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts)
						{
							UICharacterSelectCharacterSettingsPanel.Get().OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins);
						}
					}
					UICharacterSelectCharacterSettingsPanel.Get().Refresh(UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay);
				}
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
		this.OnLobbyGameplayOverridesChangeHolder(gameplayOverrides);
	}

	private void HandleLobbyAlertMissionDataNotification(LobbyAlertMissionDataNotification notification)
	{
		AlertMissionsData = notification;
		this.OnAlertMissionDataChangeHolder(notification);
	}

	private void HandleLobbySeasonQuestDataNotification(LobbySeasonQuestDataNotification notification)
	{
		SeasonChapterQuests = notification.SeasonChapterQuests;
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
		object[] array = new object[1];
		object obj;
		if (matchmakingQueueInfo == null)
		{
			obj = "(null)";
		}
		else
		{
			obj = matchmakingQueueInfo.GameType.ToString();
		}
		array[0] = obj;
		Log.Warning("Ignoring status update for queue {0}", array);
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
		int num;
		if (gameInfo != null)
		{
			num = ((!gameInfo.GameServerProcessCode.IsNullOrEmpty()) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool flag2 = gameInfo2 != null && !gameInfo2.GameServerProcessCode.IsNullOrEmpty();
		int num2;
		if (flag)
		{
			if (flag2)
			{
				num2 = ((gameInfo2.GameServerProcessCode != gameInfo.GameServerProcessCode) ? 1 : 0);
				goto IL_00d2;
			}
		}
		num2 = 0;
		goto IL_00d2;
		IL_0531:
		int num3;
		if (num3 == 0)
		{
			LeaveDiscord();
		}
		goto IL_053d;
		IL_053d:
		this.OnGameAssignmentNotificationHolder(notification);
		return;
		IL_0199:
		Log.Info("Assigned to game {0}", gameInfo2.Name);
		gameManager.SetGameplayOverridesForCurrentGame(gameplayOverrides);
		goto IL_01b9;
		IL_00d2:
		bool flag3 = (byte)num2 != 0;
		object[] obj = new object[5]
		{
			text2,
			flag,
			flag2,
			flag3,
			null
		};
		object obj2;
		if (notification.Reconnection)
		{
			obj2 = " (reconnected)";
		}
		else
		{
			obj2 = string.Empty;
		}
		obj[4] = obj2;
		Log.Info("Received Game Assignment Notification {0} (assigned={1} assigning={2} reassigning={3}){4}", obj);
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					goto IL_016f;
				}
			}
			Log.Info("Unassigned from game {0}", gameManager.GameInfo.Name);
			gameManager.SetGameplayOverridesForCurrentGame(null);
		}
		goto IL_016f;
		IL_03e7:
		if (gameInfo2 != null && gameInfo2.GameConfig != null && gameInfo2.GameConfig.GameType != GameType.Practice)
		{
			if (gameInfo2.GameConfig.GameType != GameType.Tutorial)
			{
				if (gameInfo2.GameConfig.GameType != GameType.NewPlayerSolo)
				{
					int num4;
					if (Options_UI.Get() != null)
					{
						num4 = (Options_UI.Get().GetEnableAutoJoinDiscord() ? 1 : 0);
					}
					else
					{
						num4 = 0;
					}
					if (num4 != 0)
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
		goto IL_053d;
		IL_0530:
		num3 = 0;
		goto IL_0531;
		IL_01b9:
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
							GameStatus current = enumerator.Current;
							if (current.IsActiveStatus() && gameInfo2.GameStatus > current)
							{
								if (gameStatus.IsActiveStatus())
								{
									if (gameInfo2.GameStatus <= gameStatus)
									{
										continue;
									}
								}
								SetGameStatus(current);
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_02a1;
								}
							}
						}
						end_IL_02a1:;
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
					goto IL_053d;
				}
			}
			if (!flag)
			{
				if (flag2)
				{
					goto IL_03e7;
				}
			}
			if (flag3)
			{
				goto IL_03e7;
			}
			if (flag)
			{
				if (flag2)
				{
					if (!flag3)
					{
						goto IL_053d;
					}
				}
				if (!GroupInfo.InAGroup)
				{
					goto IL_0530;
				}
				if (!m_discordConnecting)
				{
					if (!m_discordConnected)
					{
						goto IL_0530;
					}
				}
				num3 = ((GetDiscordJoinType() == DiscordJoinType._000E) ? 1 : 0);
				goto IL_0531;
			}
		}
		goto IL_053d;
		IL_016f:
		if (!flag)
		{
			if (flag2)
			{
				goto IL_0199;
			}
		}
		if (flag3)
		{
			goto IL_0199;
		}
		goto IL_01b9;
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
			while (true)
			{
				Log.Warning("Ignoring info({0}) update for game {1}, expected game {2}", notification.GameInfo.GameStatus, gameInfo.GameServerProcessCode, gameManager.GameInfo.GameServerProcessCode);
				return;
			}
		}
		bool flag;
		int num2;
		if (!IsServer() && DiscordClientInterface.Get().ChannelInfo != null)
		{
			int num;
			if (gameInfo != null && gameInfo.GameStatus != GameStatus.Stopped)
			{
				num = (gameInfo.IsCustomGame ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			flag = ((byte)num != 0);
			if (teamInfo != null)
			{
				if (playerInfo != null && gameManager.PlayerInfo != null && playerInfo.TeamId != Team.Invalid)
				{
					num2 = ((playerInfo.TeamId != gameManager.PlayerInfo.TeamId) ? 1 : 0);
					goto IL_0158;
				}
			}
			num2 = 0;
			goto IL_0158;
		}
		goto IL_018c;
		IL_0158:
		bool flag2 = (byte)num2 != 0;
		if (flag)
		{
			if (flag2)
			{
				DiscordUserInfo userInfo = DiscordClientInterface.Get().UserInfo;
				JoinDiscordChannel(userInfo);
			}
		}
		goto IL_018c;
		IL_01d7:
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
		this.OnGameInfoNotificationHolder(notification);
		return;
		IL_018c:
		gameManager.SetPlayerInfo(playerInfo);
		if (playerInfo != null)
		{
			if (teamInfo != null)
			{
				if (teamInfo.TeamPlayerInfo != null)
				{
					gameManager.SetTeamPlayerInfo(teamInfo.TeamInfo(playerInfo.TeamId).ToList());
					goto IL_01d7;
				}
			}
		}
		gameManager.SetTeamPlayerInfo(null);
		goto IL_01d7;
	}

	private void HandleGameStatusNotification(GameStatusNotification notification)
	{
		Log.Info("Received Game Status Notification {0} {1}", notification.GameServerProcessCode, notification.GameStatus);
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameInfo.GameServerProcessCode != notification.GameServerProcessCode)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Warning("Ignoring status({0}) update for game {1}, we believe we're in game {2}", notification.GameStatus, notification.GameServerProcessCode, gameManager.GameInfo.GameServerProcessCode);
					return;
				}
			}
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
		this.OnGameInfoNotificationHolder(gameInfoNotification);
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
		if (gameStatus != GameStatus.Loaded)
		{
			return;
		}
		while (true)
		{
			WaitingForSkinSelectResponse.Clear();
			return;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameManager gameManager = GameManager.Get();
		if ((bool)ReplayPlayManager.Get())
		{
			if (ReplayPlayManager.Get().IsPlayback())
			{
				ResetLoadAssetsState();
				Log.Info("Stub-connecting to replay system");
				MyNetworkManager.Get().MyStartClientStub();
				goto IL_014a;
			}
		}
		if (gameManager.GameInfo != null)
		{
			if (!string.IsNullOrEmpty(gameManager.GameInfo.GameServerAddress))
			{
				if (!Uri.IsWellFormedUriString(gameManager.GameInfo.GameServerAddress, UriKind.Absolute))
				{
					throw new FormatException($"Could not parse game server address {gameManager.GameInfo.GameServerAddress}");
				}
				ResetLoadAssetsState();
				IsRegisteredToGameServer = false;
				Log.Info("Connecting to {0}", gameManager.GameInfo.GameServerAddress);
				MyNetworkManager.Get().MyStartClient(gameManager.GameInfo.GameServerAddress, Handle);
				goto IL_014a;
			}
		}
		Log.Error("Game server address is empty");
		return;
		IL_014a:
		if (!m_registeredHandlers)
		{
			MyNetworkManager myNetworkManager = MyNetworkManager.Get();
			myNetworkManager.m_OnClientConnect += HandleNetworkConnect;
			myNetworkManager.m_OnClientDisconnect += HandleNetworkDisconnect;
			myNetworkManager.m_OnClientError += HandleNetworkError;
			m_registeredHandlers = true;
		}
		Client.RegisterHandler(52, HandleLoginResponse);
		Client.RegisterHandler(62, HandleServerAssetsLoadingProgressUpdate);
		Client.RegisterHandler(54, HandleSpawningObjectsNotification);
		Client.RegisterHandler(48, HandleReplayManagerFile);
		Client.RegisterHandler(56, HandleReconnectReplayStatus);
		Client.RegisterHandler(68, HandleEndGameNotification);
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
			if (gameplayOverrides.AllowReconnectingToGameInstantly)
			{
				Log.Info("Reconnecting to game instantly {0}", GameInfo);
				TextConsole.Get().Write(StringUtil.TR("DisconnectedReconnectingGame", "Disconnected"));
				if (IsServer())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				GameManager gameManager = GameManager.Get();
				if (gameManager.GameInfo != null)
				{
					if (!string.IsNullOrEmpty(gameManager.GameInfo.GameServerAddress))
					{
						if (!Uri.IsWellFormedUriString(gameManager.GameInfo.GameServerAddress, UriKind.Absolute))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									throw new FormatException($"Could not parse game server address {gameManager.GameInfo.GameServerAddress}");
								}
							}
						}
						if (MyConnection == null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
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
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			GameManager.LoginRequest loginRequest = new GameManager.LoginRequest();
			loginRequest.AccountId = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.AccountId);
			loginRequest.SessionToken = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.SessionToken);
			loginRequest.PlayerId = PlayerInfo.PlayerId;
			loginRequest.LastReceivedMsgSeqNum = m_lastReceivedMsgSeqNum;
			Client.Send(51, loginRequest);
			return;
		}
	}

	private void HandleGameMessageSending(UNetMessage message)
	{
		m_replay.RecordRawNetworkMessage(message.Bytes, message.NumBytes);
	}

	private void LoginToGameServer(NetworkConnection conn)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		if ((bool)ReplayPlayManager.Get())
		{
			if (ReplayPlayManager.Get().IsPlayback())
			{
				Log.Info("Stub-connected to replay system", gameManager.GameInfo.GameServerAddress);
				goto IL_008c;
			}
		}
		Log.Info("Connected to {0}", gameManager.GameInfo.GameServerAddress);
		goto IL_008c;
		IL_008c:
		ClientScene.AddPlayer(conn, 0);
		GameManager.LoginRequest loginRequest = new GameManager.LoginRequest();
		loginRequest.AccountId = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.AccountId);
		loginRequest.SessionToken = Convert.ToString(m_lobbyGameClientInterface.SessionInfo.SessionToken);
		loginRequest.PlayerId = gameManager.PlayerInfo.PlayerId;
		loginRequest.LastReceivedMsgSeqNum = m_lastReceivedMsgSeqNum;
		Client.Send(51, loginRequest);
	}

	private void HandleNetworkConnect(NetworkConnection conn)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error("Network connect error");
					return;
				}
			}
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
			int num;
			if (MyConnection.CloseStatusCode == CloseStatusCode.PingTimeout)
			{
				num = 16;
			}
			else
			{
				num = 14;
			}
			gameResult = (GameResult)num;
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
		if (assetsLoadingProgress == null)
		{
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
		float loadingProgress = (float)(int)assetsLoadingProgress.TotalLoadingProgress / 100f;
		UILoadingScreenPanel.Get().SetLoadingProgress(assetsLoadingProgress.PlayerId, loadingProgress, false);
	}

	private void HandleSpawningObjectsNotification(NetworkMessage msg)
	{
		if (IsServer())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameManager.SpawningObjectsNotification spawningObjectsNotification = msg.ReadMessage<GameManager.SpawningObjectsNotification>();
		if (spawningObjectsNotification == null)
		{
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		m_spawnableObjectCount = spawningObjectsNotification.SpawnableObjectCount;
	}

	public static string FormReplayFilename(DateTime time, string gameServerProcessCode, string handle)
	{
		return FormReplayFilename(time.ToString("MM_dd_yyyy__HH_mm_ss"), gameServerProcessCode, handle);
	}

	public static string FormReplayFilename(string timeStr, string gameServerProcessCode, string handle)
	{
		return $"{timeStr}__{gameServerProcessCode}__{BuildVersion.MiniVersionString}__{WWW.EscapeURL(handle)}.arr";
	}

	public static string RemoveTimeFromReplayFilename(string filename)
	{
		return filename.Substring(20);
	}

	private void HandleReplayManagerFile(NetworkMessage msg)
	{
		GameManager.ReplayManagerFile replayManagerFile = msg.ReadMessage<GameManager.ReplayManagerFile>();
		if (replayManagerFile == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
		if (reconnectReplayStatus == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		object[] array = new object[1];
		object obj;
		if (reconnectReplayStatus.WithinReconnectReplay)
		{
			obj = "Entering";
		}
		else
		{
			obj = "Exiting";
		}
		array[0] = obj;
		Debug.LogFormat("{0} reconnection replay phase", array);
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
		if (endGameNotification == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameResult gameResult = GameResult.NoResult;
		LeaveGame(true, gameResult);
		GameManager.Get().StopGame(gameResult);
	}

	private void HandleLoginResponse(NetworkMessage msg)
	{
		if (IsServer())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		GameManager.LoginResponse loginResponse = msg.ReadMessage<GameManager.LoginResponse>();
		if (loginResponse == null)
		{
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (loginResponse.Success)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_withinReconnect = loginResponse.Reconnecting;
					IsRegisteredToGameServer = true;
					if (m_withinReconnectInstantly)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								m_withinReconnectInstantly = false;
								TextConsole.Get().Write(StringUtil.TR("LoggedIntoGame", "Disconnected"));
								if (msg.conn.lastMessageOutgoingSeqNum - loginResponse.LastReceivedMsgSeqNum != 0)
								{
									uint startSeqNum = loginResponse.LastReceivedMsgSeqNum + 1;
									uint lastSentMsgSeqNum = m_lastSentMsgSeqNum;
									IEnumerable<Replay.Message> rawNetworkMessages = m_replay.GetRawNetworkMessages(startSeqNum, lastSentMsgSeqNum);
									IEnumerator<Replay.Message> enumerator = rawNetworkMessages.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											Replay.Message current = enumerator.Current;
											msg.conn.ResendBytes(current.data, current.data.Length, 0);
										}
									}
									finally
									{
										if (enumerator != null)
										{
											while (true)
											{
												switch (3)
												{
												case 0:
													break;
												default:
													enumerator.Dispose();
													goto end_IL_0112;
												}
											}
										}
										end_IL_0112:;
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		string text = $"Login request failed: {loginResponse.ErrorMessage}";
		Log.Error(text);
		AppState_GameTeardown.Get().Enter(GameResult.ClientLoginFailedToGameServer, text);
	}

	public void DisableFrontEnd()
	{
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.Get().Disable();
			return;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_loading = true;
		m_loadLevelOperationDone = false;
		m_loadedCharacterResourceCount = 0;
		m_spawnableObjectCount = 0;
		m_assetsLoadingState.Reset();
		GameManager gameManager = GameManager.Get();
		string map = gameManager.GameConfig.Map;
		string text = (!AssetBundleManager.Get().SceneExistsInBundle("testing", gameManager.GameConfig.Map)) ? "maps" : "testing";
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
		foreach (KeyValuePair<string, string> loadLevelOperationBundleSceneName in m_loadLevelOperationBundleSceneNames)
		{
			string key = loadLevelOperationBundleSceneName.Key;
			string value = loadLevelOperationBundleSceneName.Value;
			AssetBundleManager.Get().UnloadScene(key, value);
		}
		m_loadLevelOperationBundleSceneNames.Clear();
		Get().CleanupMemory();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameObjectsDestroyed, null);
	}

	private IEnumerator LoadCharacterAssets(GameStatus gameStatusForAssets, float delaySeconds)
	{
		m_loadingCharacterAssets = true;
		GameManager gameManager = GameManager.Get();
		using (IEnumerator<LobbyPlayerInfo> enumerator = gameManager.TeamInfo.TeamAPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo teamPlayerInfo2 = enumerator.Current;
				CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(teamPlayerInfo2.CharacterInfo.CharacterType);
				if (!m_loadingCharacterResources.Contains(characterResourceLink2))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							m_loadingCharacterResources.Add(characterResourceLink2);
							characterResourceLink2.LoadAsync(teamPlayerInfo2.CharacterInfo.CharacterSkin, HandleCharacterResourceLoaded, gameStatusForAssets);
							yield return new WaitForSeconds(delaySeconds);
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}
		IEnumerator<LobbyPlayerInfo> enumerator2 = gameManager.TeamInfo.TeamBPlayerInfo.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				LobbyPlayerInfo teamPlayerInfo = enumerator2.Current;
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(teamPlayerInfo.CharacterInfo.CharacterType);
				if (!m_loadingCharacterResources.Contains(characterResourceLink))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							m_loadingCharacterResources.Add(characterResourceLink);
							characterResourceLink.LoadAsync(teamPlayerInfo.CharacterInfo.CharacterSkin, HandleCharacterResourceLoaded, gameStatusForAssets);
							yield return new WaitForSeconds(delaySeconds);
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator2 != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						enumerator2.Dispose();
						goto end_IL_02b6;
					}
				}
			}
			goto end_IL_02b6;
			IL_02b9:
			switch (7)
			{
			default:
				goto end_IL_02b6;
			case 0:
				goto IL_02b9;
			}
			end_IL_02b6:;
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
		if (PlayerInfo == null)
		{
			return;
		}
		while (true)
		{
			if (TeamInfo == null)
			{
				return;
			}
			while (true)
			{
				AssetsLoadingState assetsLoadingState = m_assetsLoadingState;
				float levelLoadProgress;
				if (m_loadLevelOperationDone)
				{
					levelLoadProgress = 1f;
				}
				else if (m_loadLevelOperation != null)
				{
					levelLoadProgress = m_loadLevelOperation.progress;
				}
				else
				{
					levelLoadProgress = 0f;
				}
				assetsLoadingState.LevelLoadProgress = levelLoadProgress;
				m_assetsLoadingState.CharacterLoadProgress = Mathf.Clamp((float)m_loadedCharacterResourceCount / (float)TeamInfo.TotalPlayerCount, 0f, 1f);
				AssetsLoadingState assetsLoadingState2 = m_assetsLoadingState;
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
				AssetsLoadingState assetsLoadingState3 = m_assetsLoadingState;
				float spawningProgress;
				if (m_spawnableObjectCount > 0)
				{
					spawningProgress = Mathf.Clamp((float)ClientScene.objects.Count / (float)m_spawnableObjectCount, 0f, 1f);
				}
				else
				{
					spawningProgress = 0f;
				}
				assetsLoadingState3.SpawningProgress = spawningProgress;
				if (PlayerInfo.PlayerId == 0)
				{
					UILoadingScreenPanel.Get().SetLoadingProgress(PlayerInfo, m_assetsLoadingState.TotalProgress, true);
				}
				else
				{
					UILoadingScreenPanel.Get().SetLoadingProgress(PlayerInfo.PlayerId, m_assetsLoadingState.TotalProgress, true);
				}
				if (!(Time.unscaledTime > m_lastLoadProgressUpdateSent + m_loadingProgressUpdateFrequency))
				{
					if (!force)
					{
						return;
					}
				}
				if (Client == null)
				{
					return;
				}
				while (true)
				{
					if (!Client.isConnected)
					{
						return;
					}
					while (true)
					{
						if (IsRegisteredToGameServer)
						{
							while (true)
							{
								GameManager.AssetsLoadingProgress assetsLoadingProgress = new GameManager.AssetsLoadingProgress();
								assetsLoadingProgress.AccountId = m_lobbyGameClientInterface.SessionInfo.AccountId;
								assetsLoadingProgress.PlayerId = PlayerInfo.PlayerId;
								assetsLoadingProgress.TotalLoadingProgress = (byte)(m_assetsLoadingState.TotalProgress * 100f);
								assetsLoadingProgress.LevelLoadingProgress = (byte)(m_assetsLoadingState.LevelLoadProgress * 100f);
								assetsLoadingProgress.CharacterLoadingProgress = (byte)(m_assetsLoadingState.CharacterLoadProgress * 100f);
								assetsLoadingProgress.VfxLoadingProgress = (byte)(m_assetsLoadingState.VfxPreloadProgress * 100f);
								assetsLoadingProgress.SpawningProgress = (byte)(m_assetsLoadingState.SpawningProgress * 100f);
								m_lastLoadProgressUpdateSent = Time.unscaledTime;
								Client.Send(61, assetsLoadingProgress);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	private void CheckLoaded()
	{
		if (IsServer())
		{
			while (true)
			{
				return;
			}
		}
		GameManager gameManager = GameManager.Get();
		int num;
		if (m_loadLevelOperationDone)
		{
			num = ((GameFlowData.Get() == null || GameFlowData.Get().gameState < GameState.Deployment) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (!m_loading)
		{
			if (!flag)
			{
				goto IL_0085;
			}
		}
		UpdateLoadProgress();
		goto IL_0085;
		IL_0085:
		if (!m_loading)
		{
			return;
		}
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
						goto IL_0240;
					}
				}
				if (!(ClientVFXLoader.Get() == null))
				{
					if (!ClientVFXLoader.Get().IsPreloadQueueEmpty())
					{
						goto IL_0240;
					}
				}
				if (1 == 0)
				{
					return;
				}
				while (true)
				{
					if (ClientScene.localPlayers == null)
					{
						return;
					}
					while (true)
					{
						if (ClientScene.localPlayers.Count <= 0)
						{
							return;
						}
						while (true)
						{
							if (Client == null)
							{
								return;
							}
							while (true)
							{
								if (!Client.isConnected)
								{
									return;
								}
								while (true)
								{
									ResetLoadAssetsState();
									UpdateLoadProgress(true);
									GameManager.AssetsLoadedNotification assetsLoadedNotification = new GameManager.AssetsLoadedNotification();
									assetsLoadedNotification.AccountId = m_lobbyGameClientInterface.SessionInfo.AccountId;
									assetsLoadedNotification.PlayerId = gameManager.PlayerInfo.PlayerId;
									Log.Info(Log.Category.Loading, "Sending asset loaded notification");
									if (!Client.Send(53, assetsLoadedNotification))
									{
										Log.Error("Failed to send message AssetsLoadedNotification");
									}
									return;
								}
							}
						}
					}
				}
			}
		}
		goto IL_0240;
		IL_0240:
		if (m_loadLevelOperation != null || m_loadingCharacterResources.Count != 0)
		{
			return;
		}
		while (true)
		{
			if (!(ClientVFXLoader.Get() != null) || ClientVFXLoader.Get().IsPreloadQueueEmpty())
			{
				return;
			}
			while (true)
			{
				if (!ClientVFXLoader.Get().IsPreloadInProgress())
				{
					Log.Info(Log.Category.Loading, "Starting VFX Preload");
					ClientVFXLoader.Get().PreloadQueuedPKFX();
				}
				return;
			}
		}
	}

	public bool IsGroupReady()
	{
		bool result = true;
		if (GroupInfo.InAGroup)
		{
			int num = 0;
			while (true)
			{
				if (num < GroupInfo.Members.Count)
				{
					if (!GroupInfo.Members[num].IsReady)
					{
						result = false;
						break;
					}
					num++;
					continue;
				}
				break;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (charType == CharacterType.None)
		{
			return true;
		}
		return m_loadedPlayerCharacterData.ContainsKey(charType);
	}

	public int GetHighestOpenSeasonChapterIndexForActiveSeason()
	{
		int result = -1;
		if (IsPlayerAccountDataAvailable())
		{
			if (SeasonWideData.Get() != null)
			{
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(GetPlayerAccountData().QuestComponent.ActiveSeason);
				for (int i = 0; i < seasonTemplate.Chapters.Count; i++)
				{
					SeasonChapter seasonChapter = seasonTemplate.Chapters[i];
					if (seasonChapter.Prerequisites.Conditions.Count != 0)
					{
						if (seasonChapter.Prerequisites.Conditions.Count != 1)
						{
							continue;
						}
						if (seasonChapter.Prerequisites.Conditions[0].ConditionType != QuestConditionType.HasDateTimePassed)
						{
							continue;
						}
					}
					if (QuestWideData.AreConditionsMet(seasonChapter.Prerequisites.Conditions, seasonChapter.Prerequisites.LogicStatement))
					{
						result = i;
					}
				}
			}
		}
		return result;
	}

	public PersistedAccountData GetPlayerAccountData()
	{
		if (m_loadedPlayerAccountData == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Error("Player account data not loaded yet");
					return null;
				}
			}
		}
		return m_loadedPlayerAccountData;
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		GameTypeAvailabilies = new Dictionary<GameType, GameTypeAvailability>(notification.GameTypeAvailabilies, default(GameTypeComparer));
		FreeRotationAdditions = new Dictionary<CharacterType, RequirementCollection>(notification.FreeRotationAdditions, default(CharacterTypeComparer));
		AllowBadges = notification.AllowBadges;
		NewPlayerPvPQueueDuration = notification.NewPlayerPvPQueueDuration;
		foreach (GameTypeAvailability value in GameTypeAvailabilies.Values)
		{
			foreach (GameSubType subType in value.SubTypes)
			{
				List<GameMapConfig> gameMapConfigs = subType.GameMapConfigs;
				
				if (!gameMapConfigs.Exists(((GameMapConfig p) => p.IsActive)))
				{
					if (subType.Requirements == null)
					{
						subType.Requirements = RequirementCollection.Create();
					}
					RequirementCollection requirements = subType.Requirements;
					
					if (!requirements.Exists(((QueueRequirement p) => p is QueueRequirement_Never)))
					{
						subType.Requirements.Add(QueueRequirement_Never.Create(QueueRequirement.RequirementType.AdminDisabled, null));
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
					goto IL_01ec;
				}
			}
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		goto IL_01ec;
		IL_01ec:
		m_tierInstanceNames = notification.TierInstanceNames;
		this.OnServerQueueConfigurationUpdateNotificationHolder(notification);
	}

	private void HandleRankedOverviewChangeNotification(RankedOverviewChangeNotification notification)
	{
		if (notification.GameType != GameType.Ranked)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					throw new Exception($"We do not yet handle RankedOverviewChangeNotification for game type {notification.GameType}");
				}
			}
		}
		UIRankedModeSelectScreen uIRankedModeSelectScreen = UIRankedModeSelectScreen.Get();
		if (!(uIRankedModeSelectScreen != null))
		{
			return;
		}
		while (true)
		{
			uIRankedModeSelectScreen.ProcessTierInfoPerGroupSize(notification.TierInfoPerGroupSize);
			return;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					this.OnQuestCompleteNotificationHolder(notification);
					return;
				}
			}
		}
		LoginQuestCompleteNotifications.Add(notification);
	}

	private void HandleFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		ActiveFactionCompetition = notification.ActiveIndex;
		FactionScores = notification.Scores;
		this.OnFactionCompetitionNotificationHolder(notification);
	}

	private void HandleTrustBoostUsedNotification(TrustBoostUsedNotification notification)
	{
		this.OnTrustBoostUsedNotificationHolder(notification);
	}

	private void HandleFactionLoginRewardNotification(FactionLoginRewardNotification notification)
	{
		if (LoginRewardNotification != null)
		{
			Log.Error("received a second login notification! - should not");
		}
		LoginRewardNotification = notification;
		this.OnFactionLoginRewardNotificationHolder(notification);
	}

	private void HandlePlayerFactionContributionChange(PlayerFactionContributionChangeNotification notification)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				if (!IsPlayerAccountDataAvailable())
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							Log.Error("Player Account Data not available for Faction Contribution Change");
							return;
						}
					}
				}
				GetPlayerAccountData().AccountComponent.GetPlayerCompetitionFactionData(notification.CompetitionId, notification.FactionId).TotalXP = notification.TotalXP;
				this.OnPlayerFactionContributionChangeNotificationHolder(notification);
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
		this.OnMatchResultsNotificationHolder(notification);
	}

	public void QueryPlayerMatchData(Action<PlayerMatchDataResponse> onResponseCallback)
	{
		if (onResponseCallback == null)
		{
			throw new ArgumentNullException("onResponseCallback");
		}
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.GetPlayerMatchData(onResponseCallback);
					return;
				}
			}
		}
		onResponseCallback(new PlayerMatchDataResponse
		{
			Success = false,
			ErrorMessage = "Not connected to lobby server."
		});
	}

	public int GetPlayerCharacterLevel(CharacterType character)
	{
		if (m_loadedPlayerCharacterData == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error("Player character data not loaded yet");
					return -1;
				}
			}
		}
		if (m_loadedPlayerCharacterData.TryGetValue(character, out PersistedCharacterData value))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return value.ExperienceComponent.Level;
				}
			}
		}
		return -1;
	}

	public PersistedCharacterData GetPlayerCharacterData(CharacterType character)
	{
		if (m_loadedPlayerCharacterData == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Player character data not loaded yet");
					return null;
				}
			}
		}
		if (m_loadedPlayerCharacterData.TryGetValue(character, out PersistedCharacterData value))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return null;
	}

	public Dictionary<CharacterType, PersistedCharacterData> GetAllPlayerCharacterData()
	{
		if (m_loadedPlayerCharacterData == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Error("Player character data not loaded yet");
					return null;
				}
			}
		}
		return m_loadedPlayerCharacterData;
	}

	public PersistedCharacterData GetCharacterDataOnInitialLoad(CharacterType charType)
	{
		if (m_characterDataOnInitialLoad.TryGetValue(charType, out PersistedCharacterData value))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return null;
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						PurchasingMod = true;
						ModAttemptingToPurchase = abilityModID;
						m_lobbyGameClientInterface.PurchaseMod(character, abilityId, abilityModID, HandlePurchaseModResponse);
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	private void HandlePurchaseModResponse(PurchaseModResponse response)
	{
		if (response.Success)
		{
			if (m_loadedPlayerCharacterData.TryGetValue(response.Character, out PersistedCharacterData value))
			{
				value.CharacterComponent.Mods.Add(response.UnlockData);
			}
			this.OnModUnlockedHolder(response.Character, response.UnlockData);
		}
		else
		{
			Log.Error($"Failed to unlock Mod {response.UnlockData.ToString()} for character {response.Character}: {response.ErrorMessage}");
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					RecordFailureInCharacterSelectUpdateResponse(response, "HandleSelectLoadoutUpdateResponse");
					return;
				}
			}
		}
		UICharacterSelectCharacterSettingsPanel.Get().NotifyLoadoutUpdate(response);
		if (!(UIRankedCharacterSelectSettingsPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIRankedCharacterSelectSettingsPanel.Get().NotifyLoadoutUpdate(response);
			return;
		}
	}

	public void RequestToSelectLoadout(CharacterLoadout loadout, int loadoutIndex)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
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
				}
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
		if (response.Success)
		{
			return;
		}
		while (true)
		{
			Log.Error("Failed to purchase Mod Token");
			return;
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
				text = $"{response.ErrorMessage}#NeedsLocalization";
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
		if (!PlayerObjectStartedOnClient || !InGameUIActivated)
		{
			return;
		}
		while (true)
		{
			if (!VisualSceneLoaded)
			{
				return;
			}
			while (true)
			{
				if (DesignSceneStarted)
				{
					while (true)
					{
						SendClientPreparedForGameStartNotification();
						PlayerObjectStartedOnClient = false;
						InGameUIActivated = false;
						VisualSceneLoaded = false;
						DesignSceneStarted = false;
						return;
					}
				}
				return;
			}
		}
	}

	public void SendClientPreparedForGameStartNotification()
	{
		Log.Info("SendClientPreparedForGameStartNotification");
		if (!NetworkClient.active || NetworkServer.active)
		{
			return;
		}
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
		Client.Send(55, playerObjectStartedOnClientNotification);
	}

	public void UpdateRemoteCharacter(CharacterType character, int remoteSlotIndex, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		UpdateRemoteCharacter(new CharacterType[1]
		{
			character
		}, new int[1]
		{
			remoteSlotIndex
		}, onResponse);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponse = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (!remoteSlotIndexes.IsNullOrEmpty())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									if (!characters.IsNullOrEmpty())
									{
										if (remoteSlotIndexes.Length != characters.Length)
										{
											while (true)
											{
												switch (3)
												{
												default:
													return;
												case 0:
													break;
												}
											}
										}
										if (m_loadedPlayerAccountData != null)
										{
											bool flag = false;
											List<CharacterType> lastRemoteCharacters = m_loadedPlayerAccountData.AccountComponent.LastRemoteCharacters;
											int num = 0;
											while (true)
											{
												if (num < characters.Length)
												{
													if (lastRemoteCharacters.Count > remoteSlotIndexes[num])
													{
														if (lastRemoteCharacters[remoteSlotIndexes[num]] == characters[num])
														{
															num++;
															continue;
														}
													}
													flag = true;
												}
												else
												{
												}
												break;
											}
											if (!flag)
											{
												while (true)
												{
													switch (6)
													{
													default:
														return;
													case 0:
														break;
													}
												}
											}
										}
										m_lobbyGameClientInterface.UpdateRemoteCharacter(characters, remoteSlotIndexes, onResponse);
									}
									return;
								}
							}
						}
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponse)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (m_loadedPlayerAccountData != null)
						{
							if (m_loadedPlayerAccountData.AccountComponent.SelectedTitleID == newTitleID)
							{
								while (true)
								{
									switch (7)
									{
									default:
										return;
									case 0:
										break;
									}
								}
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
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										this.OnPlayerTitleChangeHolder(gameBalanceVars.GetTitle(response.CurrentTitleID, string.Empty));
										return;
									}
								}
							}
						});
						return;
					}
				}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (m_loadedPlayerAccountData != null)
						{
							if (m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID == newBannerID)
							{
								return;
							}
							if (m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID == newBannerID)
							{
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
						m_lobbyGameClientInterface.RequestBannerSelect(newBannerID, delegate(SelectBannerResponse response)
						{
							GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
							if (m_loadedPlayerAccountData != null)
							{
								if (gameBalanceVars != null)
								{
									m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID = response.ForegroundBannerID;
									m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID = response.BackgroundBannerID;
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
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (m_loadedPlayerAccountData == null || m_loadedPlayerAccountData.AccountComponent.SelectedRibbonID != newRibbonID)
						{
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
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											this.OnPlayerRibbonChangeHolder(gameBalanceVars.GetRibbon(response.CurrentRibbonID));
											return;
										}
									}
								}
							});
						}
						return;
					}
				}
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_loadedPlayerAccountData != null)
						{
							if (!m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundUnlocked(loadingScreenId))
							{
								while (true)
								{
									switch (6)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
							bool flag = m_loadedPlayerAccountData.AccountComponent.IsLoadingScreenBackgroundActive(loadingScreenId);
							if (flag == newState)
							{
								while (true)
								{
									switch (5)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
						}
						m_lobbyGameClientInterface.RequestLoadingScreenBackgroundToggle(loadingScreenId, newState, delegate(LoadingScreenToggleResponse response)
						{
							if (!response.Success)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										return;
									}
								}
							}
							if (m_loadedPlayerAccountData != null)
							{
								m_loadedPlayerAccountData.AccountComponent.ToggleLoadingScreenBackgroundActive(response.LoadingScreenId, response.CurrentState);
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.SendRankedLeaderboardOverviewRequest(gameType, groupSize, specification, onResponse);
					return;
				}
			}
		}
		onResponse(new RankedLeaderboardSpecificResponse
		{
			ErrorMessage = "Not connected to a lobby server",
			Success = false
		});
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
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								onResponse(response);
								return;
							}
						}
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

	public void _001D(string _001D, string _000E)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface._001D(_001D, _000E);
			}
			return;
		}
	}

	public void _000E()
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (!m_lobbyGameClientInterface.IsConnected)
			{
				return;
			}
			while (true)
			{
				LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
				
				lobbyGameClientInterface._001D(delegate(DEBUG_ForceMatchmakingResponse response)
					{
						string text = (!response.Success) ? $"Failed to force queue: {response.ErrorMessage}" : $"Forced queue {response.GameType}";
						TextConsole.Get().Write(text);
						if (response.Success)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									Log.Info(text);
									return;
								}
							}
						}
						Log.Error(text);
					});
				return;
			}
		}
	}

	public void _0012()
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (!m_lobbyGameClientInterface.IsConnected)
			{
				return;
			}
			LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
			
			lobbyGameClientInterface._001D(delegate(DEBUG_TakeSnapshotResponse response)
				{
					string text;
					if (response.Success)
					{
						text = $"Snapshot taken {response.SnapshotId}";
					}
					else
					{
						text = $"Failed to take snapshot: {response.ErrorMessage}";
					}
					string text2 = text;
					TextConsole.Get().Write(text2);
					if (response.Success)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								Log.Info(text2);
								return;
							}
						}
					}
					Log.Error(text2);
				});
			return;
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
		if (m_discordConnecting)
		{
			return;
		}
		while (true)
		{
			if (m_discordConnected)
			{
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
			if (m_lobbyGameClientInterface == null || !m_lobbyGameClientInterface.IsConnected)
			{
				return;
			}
			while (true)
			{
				m_discordConnecting = true;
				m_discordJoinType = discordJoinType;
				LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
				
				lobbyGameClientInterface.SendDiscordGetRpcTokenRequest(delegate(DiscordGetRpcTokenResponse response)
					{
						if (response.Success)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
								{
									DiscordAuthInfo authInfo = new DiscordAuthInfo
									{
										ClientId = response.DiscordClientId,
										RpcToken = response.DiscordRpcToken,
										RpcOrigin = response.DiscordRpcOrigin
									};
									DiscordClientInterface.Get().Connect(authInfo);
									return;
								}
								}
							}
						}
					});
				return;
			}
		}
	}

	private void JoinDiscordChannel(DiscordUserInfo userInfo)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				m_lobbyGameClientInterface.SendDiscordJoinServerRequest(userInfo.UserId, userInfo.AccessToken, m_discordJoinType, delegate(DiscordJoinServerResponse response)
				{
					if (response.Success)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (m_discordJoinType != 0)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
										{
											DiscordChannelInfo channelInfo = new DiscordChannelInfo
											{
												ServerId = response.DiscordServerId,
												VoiceChannelId = response.DiscordVoiceChannelId
											};
											DiscordClientInterface.Get().SelectVoiceChannel(channelInfo);
											return;
										}
										}
									}
								}
								return;
							}
						}
					}
					Log.Error("Failed to join discord server {0}", response.ErrorMessage);
					TextConsole.Get().Write(string.Format(StringUtil.TR("FailedToJoinDiscordChat", "Global"), response.ErrorMessage));
					DiscordClientInterface.Get().Disconnect();
				});
			}
			return;
		}
	}

	public void CheckDiscord()
	{
		DiscordClientInterface.Get().Connect(null);
	}

	private void HandleDiscordConnected(bool needAuthentication)
	{
		m_discordConnecting = false;
		if (!needAuthentication)
		{
			return;
		}
		string text = string.Format(StringUtil.TR("ClickToJoinDiscordTeamChat", "Global"));
		if (!DiscordClientInterface.IsSdkEnabled)
		{
			TextConsole.Get().Write(text);
		}
		DiscordClientInterface.Get().Disconnect();
	}

	private void HandleDiscordDisconnected()
	{
		m_discordConnecting = false;
	}

	private void HandleDiscordAuthorized(string rpcCode)
	{
		if (m_lobbyGameClientInterface == null || !m_lobbyGameClientInterface.IsConnected)
		{
			return;
		}
		while (true)
		{
			LobbyGameClientInterface lobbyGameClientInterface = m_lobbyGameClientInterface;
			
			lobbyGameClientInterface.SendDiscordGetAccessTokenRequest(rpcCode, delegate(DiscordGetAccessTokenResponse response)
				{
					if (response.Success)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
							{
								DiscordUserInfo userInfo = new DiscordUserInfo
								{
									AccessToken = response.DiscordAccessToken
								};
								DiscordClientInterface.Get().Authenticate(userInfo);
								return;
							}
							}
						}
					}
				});
			return;
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
		int value;
		if (autoJoin)
		{
			value = 1;
		}
		else
		{
			value = 0;
		}
		PlayerPrefs.SetInt("AutoJoinDiscord", value);
		string format = StringUtil.TR("ConfiguredDiscordAutojoin", "Global");
		string arg;
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
		if (m_lobbyGameClientInterface == null || !m_lobbyGameClientInterface.IsConnected)
		{
			return;
		}
		while (true)
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
			return;
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
							while (true)
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
								KeyValuePair<int, int> current = enumerator.Current;
								if (!questProgress.ObjectiveProgress.TryGetValue(current.Key, out int value))
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											flag = true;
											goto end_IL_00d3;
										}
									}
								}
								if (current.Value != value)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											flag = true;
											goto end_IL_00d3;
										}
									}
								}
							}
							end_IL_00d3:;
						}
					}
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
		this.OnAccountDataUpdatedHolder(accountData);
		PlayerWallet = new CurrencyWallet(accountData.BankComponent.CurrentAmounts.Data);
		IEnumerator<CurrencyData> enumerator2 = PlayerWallet.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				CurrencyData current2 = enumerator2.Current;
				this.OnBankBalanceChangeHolder(current2);
			}
		}
		finally
		{
			if (enumerator2 != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						enumerator2.Dispose();
						goto end_IL_01e4;
					}
				}
			}
			end_IL_01e4:;
		}
		if (list.Count > 0)
		{
			this.OnQuestProgressChangedHolder(list.ToArray());
		}
	}

	private void HandlePlayerCharacterDataUpdated(List<PersistedCharacterData> characterDataList)
	{
		m_loadedPlayerCharacterData = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		m_characterDataOnInitialLoad = new Dictionary<CharacterType, PersistedCharacterData>(default(CharacterTypeComparer));
		foreach (PersistedCharacterData characterData in characterDataList)
		{
			m_loadedPlayerCharacterData.Add(characterData.CharacterType, characterData);
			PersistedCharacterData persistedCharacterData = characterData.Clone() as PersistedCharacterData;
			persistedCharacterData.CharacterComponent = new CharacterComponent();
			using (List<PlayerSkinData>.Enumerator enumerator2 = characterData.CharacterComponent.Skins.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PlayerSkinData current2 = enumerator2.Current;
					persistedCharacterData.CharacterComponent.Skins.Add(current2.GetDeepCopy());
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_008b;
					}
				}
				end_IL_008b:;
			}
			m_characterDataOnInitialLoad.Add(characterData.CharacterType, persistedCharacterData);
		}
	}

	private void HandleForcedCharacterChangeFromServerNotification(ForcedCharacterChangeFromServerNotification notification)
	{
		if (GroupInfo != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					GroupInfo.SetCharacterInfo(notification.ChararacterInfo);
					Log.Info("Server forcing us to switch to {0}", notification.ChararacterInfo.CharacterType.GetDisplayName());
					return;
				}
			}
		}
		throw new Exception($"The server believes we should be freelancer {notification.ChararacterInfo.CharacterType.GetDisplayName()}, but we're not doing anything about that. This is bad?!");
	}

	private void HandleCharacterDataUpdateNotification(PlayerCharacterDataUpdateNotification notification)
	{
		m_loadedPlayerCharacterData[notification.CharacterData.CharacterType] = notification.CharacterData;
		this.OnCharacterDataUpdatedHolder(notification.CharacterData);
	}

	private void HandleInventoryComponentUpdateNotification(InventoryComponentUpdateNotification notification)
	{
		m_loadedPlayerAccountData.InventoryComponent = notification.InventoryComponent;
		this.OnInventoryComponentUpdatedHolder(notification.InventoryComponent);
	}

	private void HandleBankBalanceChangeNotification(BankBalanceChangeNotification notification)
	{
		if (!notification.Success)
		{
			return;
		}
		while (true)
		{
			if (PlayerWallet != null)
			{
				while (true)
				{
					PlayerWallet.SetValue(notification.NewBalance);
					this.OnBankBalanceChangeHolder(notification.NewBalance);
					return;
				}
			}
			return;
		}
	}

	public void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		if (m_loadedPlayerAccountData != null)
		{
			m_loadedPlayerAccountData.QuestComponent.ActiveSeason = notification.SeasonStartedIndex;
		}
		this.OnSeasonCompleteNotificationHolder(notification);
		if (m_loadedPlayerAccountData != null)
		{
			this.OnAccountDataUpdatedHolder(m_loadedPlayerAccountData);
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (notification.IsCompleted)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					this.OnChapterCompleteNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
					return;
				}
			}
		}
		if (!notification.IsUnlocked)
		{
			return;
		}
		while (true)
		{
			this.OnChapterUnlockNotificationHolder(notification.SeasonIndex, notification.ChapterIndex + 1);
			return;
		}
	}

	public void SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					m_lobbyGameClientInterface.SendPlayerCharacterFeedback(feedbackData);
					return;
				}
			}
			return;
		}
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.InviteToGroup(friendHandle, onResponseCallback);
						return;
					}
				}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.RequestToJoinGroup(friendHandle, onResponseCallback);
					return;
				}
			}
		}
		GroupJoinResponse groupJoinResponse = new GroupJoinResponse();
		groupJoinResponse.Success = false;
		groupJoinResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PromoteWithinGroup(name, onResponseCallback);
						return;
					}
				}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.ChatToGroup(text, onResponseCallback);
					return;
				}
			}
		}
		GroupChatResponse groupChatResponse = new GroupChatResponse();
		groupChatResponse.Success = false;
		groupChatResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
		Log.Error("Not connected to lobby server.");
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.LeaveGroup(onResponseCallback);
						return;
					}
				}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.UpdateFriend(friendHandle, friendAccountId, operation, strData, onResponseCallback);
					return;
				}
			}
		}
		if (onResponseCallback == null)
		{
			return;
		}
		while (true)
		{
			FriendUpdateResponse friendUpdateResponse = new FriendUpdateResponse();
			friendUpdateResponse.Success = false;
			friendUpdateResponse.ErrorMessage = StringUtil.TR("NotConnectedToLobbyServer", "Global");
			Log.Error("Not connected to lobby server.");
			onResponseCallback(friendUpdateResponse);
			return;
		}
	}

	public void UpdatePlayerStatus(string statusString)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					
					Action<PlayerUpdateStatusResponse> onResponseCallback = delegate(PlayerUpdateStatusResponse resonse)
						{
							if (!resonse.Success)
							{
								Log.Warning(resonse.ErrorMessage);
							}
						};
					m_lobbyGameClientInterface.UpdatePlayerStatus(statusString, onResponseCallback);
					return;
				}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseLootMatrixPack(lootMatrixPackIndex, paymentMethodId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseGGPack(ggPackIndex, paymentMethodId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseCharacter(currencyType, characterType, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseCharacterForCash(characterType, paymentMethodId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseSkin(currencyType, characterType, skinId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseTexture(currencyType, characterType, skinId, textureId, onResponseCallback);
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTint(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, int tintId, Action<PurchaseTintResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.PurchaseTint(currencyType, characterType, skinId, textureId, tintId, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTintForCash(CharacterType characterType, int skinId, int textureId, int tintId, long paymentMethodId, Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseTintForCash(characterType, skinId, textureId, tintId, paymentMethodId, onResponseCallback);
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseStoreItemForCash(int inventoryTemplateId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.PurchaseStoreItemForCash(inventoryTemplateId, paymentMethodId, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntIndex, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseTaunt(currencyType, characterType, tauntIndex, onResponseCallback);
						return;
					}
				}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.PurchaseTitle(currencyType, titleId, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void PurchaseBanner(int bannerId, CurrencyType currencyType, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseBannerBackground(currencyType, bannerId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseChatEmoji(currencyType, emoticonId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseAbilityVfx(type, abilityId, vfxId, currencyType, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseOvercon(overconId, currencyType, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.PurchaseLoadingScreenBackground(loadingScreenBackgroundId, currencyType, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.CheckAccountStatus(onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (onResponseCallback == null)
						{
							
							onResponseCallback = delegate
								{
								};
						}
						m_lobbyGameClientInterface.CheckRAFStatus(getReferralCode, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.SendRAFReferralEmails(emails, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.SelectDailyQuest(questId, onResponseCallback);
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.AbandonDailyQuest(questId, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void ActivateQuestTrigger(QuestTriggerType triggerType, int activationCount, int questId, int questBonusCount, int itemTemplateId, CharacterType charType, Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.ActivateQuestTrigger(triggerType, activationCount, questId, questBonusCount, itemTemplateId, charType, onResponseCallback);
						return;
					}
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.BeginQuest(questId, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.MarkTutorialSkipped(progress, delegate(MarkTutorialSkippedResponse response)
						{
							m_loadedPlayerAccountData.ExperienceComponent.TutorialProgress = progress;
							if (onResponseCallback != null)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										onResponseCallback(response);
										return;
									}
								}
							}
						});
						return;
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
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
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										onResponseCallback(response);
										return;
									}
								}
							}
						};
						m_lobbyGameClientInterface.GetInventoryItems(onResponseCallback2);
						return;
					}
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.AddInventoryItems(items, onResponseCallback);
						return;
					}
				}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.ConsumeInventoryItems(itemIds, toISO, onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		if (m_lobbyGameClientInterface != null)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.RerollSeasonQuests(seasonId, chapterId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.SetSeasonQuest(seasonId, chapterId, slotNum, questId, onResponseCallback);
						return;
					}
				}
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.NotifyStoreOpened();
						return;
					}
				}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.NotifyOptions(notification);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		if (!SteamManager.UsingSteam)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (m_lobbyGameClientInterface != null)
					{
						if (m_lobbyGameClientInterface.IsConnected)
						{
							m_lobbyGameClientInterface.RequestPaymentMethods(onResponseCallback);
							return;
						}
					}
					Log.Error("Not connected to lobby server.");
					return;
				}
			}
		}
		PaymentMethod paymentMethod = new PaymentMethod();
		paymentMethod.id = -1L;
		paymentMethod.specificType = "Steam Wallet";
		paymentMethod.generalType = "Steam Wallet";
		paymentMethod.tier = "1";
		paymentMethod.maskedPaymentInfo = string.Empty;
		paymentMethod.expirationDate = string.Empty;
		paymentMethod.isDefault = true;
		PaymentMethodsResponse paymentMethodsResponse = new PaymentMethodsResponse();
		paymentMethodsResponse.PaymentMethodList = new PaymentMethodList();
		paymentMethodsResponse.PaymentMethodList.IsError = false;
		paymentMethodsResponse.PaymentMethodList.PaymentMethods.Add(paymentMethod);
		paymentMethodsResponse.Success = true;
		onResponseCallback(paymentMethodsResponse);
	}

	public void RequestRefreshBankData(Action<RefreshBankDataResponse> onResponseCallback)
	{
		if (m_lobbyGameClientInterface != null && m_lobbyGameClientInterface.IsConnected)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_lobbyGameClientInterface.RequestRefreshBankData(onResponseCallback);
					return;
				}
			}
		}
		Log.Error("Not connected to lobby server.");
	}

	public bool IsServer()
	{
		return NetworkServer.active;
	}

	public bool IsTitleUnlocked(GameBalanceVars.PlayerTitle title)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
		return IsTitleUnlocked(title, out unlockConditionValues);
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					int maxTitleLevel = GameWideData.Get().m_gameBalanceVars.GetMaxTitleLevel(title.ID);
					int currentTitleLevel = GetCurrentTitleLevel(title.ID);
					return currentTitleLevel >= maxTitleLevel;
				}
				}
			}
		}
		return true;
	}

	public int GetCurrentTitleLevel(int titleID)
	{
		return m_loadedPlayerAccountData.AccountComponent.GetCurrentTitleLevel(titleID);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
		return IsEmojiUnlocked(emoji, out unlockConditionValues);
	}

	public bool IsEmojiUnlocked(GameBalanceVars.ChatEmoticon emoji, out List<GameBalanceVars.UnlockConditionValue> unlockConditionValues)
	{
		GetUnlockStatus(emoji.m_unlockData, out unlockConditionValues);
		return m_loadedPlayerAccountData.AccountComponent.IsChatEmojiUnlocked(emoji);
	}

	public bool IsOverconUnlocked(int overconId)
	{
		if (m_loadedPlayerAccountData != null)
		{
			return m_loadedPlayerAccountData.AccountComponent.IsOverconUnlocked(overconId);
		}
		return false;
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
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
		return GetUnlockStatus(unlock, out unlockConditionValues, ignorePurchaseCondition);
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
				while (true)
				{
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
						{
							unlockConditionValue.typeSpecificData = unlockCondition.typeSpecificData;
							unlockConditionValue.typeSpecificData2 = unlockCondition.typeSpecificData2;
							if (ActiveFactionCompetition != unlockCondition.typeSpecificData)
							{
								break;
							}
							if (FactionScores.TryGetValue(unlockCondition.typeSpecificData2, out long value))
							{
								unlockConditionValue.typeSpecificData3 = FactionWideData.Get().GetCompetitionFactionTierReached(unlockCondition.typeSpecificData, unlockCondition.typeSpecificData2, value);
							}
							break;
						}
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
					while (true)
					{
						return unlockData.AreUnlockConditionsMet(unlockConditionValues, ignorePurchaseCondition);
					}
				}
			}
		}
		return false;
	}

	public bool AreUnlockConditionsMet(GameBalanceVars.UnlockData unlockData, bool ignorePurchaseCondition = false)
	{
		List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
		return GetUnlockStatus(unlockData, out unlockConditionValues, ignorePurchaseCondition);
	}

	public bool AreUnlockConditionsMet(GameBalanceVars.PlayerUnlockable playerUnlockable, bool ignorePurchaseCondition = false)
	{
		return AreUnlockConditionsMet(playerUnlockable.m_unlockData, ignorePurchaseCondition);
	}

	public int GetCurrentTitleID()
	{
		if (m_loadedPlayerAccountData != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_loadedPlayerAccountData.AccountComponent.SelectedTitleID;
				}
			}
		}
		return -1;
	}

	public GameBalanceVars.PlayerBanner GetCurrentBackgroundBanner()
	{
		if (m_loadedPlayerAccountData != null && m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID != -1 && GameWideData.Get() != null && GameWideData.Get().m_gameBalanceVars != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return GameWideData.Get().m_gameBalanceVars.GetBanner(m_loadedPlayerAccountData.AccountComponent.SelectedBackgroundBannerID);
				}
			}
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
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return GameWideData.Get().m_gameBalanceVars.GetBanner(m_loadedPlayerAccountData.AccountComponent.SelectedForegroundBannerID);
							}
						}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		int num = 0;
		using (Dictionary<int, ExperienceComponent>.ValueCollection.Enumerator enumerator = data.QuestComponent.SeasonExperience.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ExperienceComponent current = enumerator.Current;
				num += current.Level;
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
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
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
		switch (GameManager.Get().GameStatus)
		{
		case GameStatus.LoadoutSelecting:
			return StringUtil.TR("SelectLoadout", "Global");
		case GameStatus.FreelancerSelecting:
			while (true)
			{
				int num2;
				if (GameManager.Get().GameConfig != null)
				{
					num2 = (int)GameManager.Get().GameConfig.GameType;
				}
				else
				{
					num2 = -1;
				}
				GameType gameType = (GameType)num2;
				if (gameType != GameType.Practice)
				{
					if (gameType != GameType.Solo)
					{
						if (gameType != GameType.Duel)
						{
							if (gameType != 0)
							{
								return StringUtil.TR("DuplicateFreelancers", "Global");
							}
						}
					}
				}
				if (gameType != GameType.Practice)
				{
					if (gameType != GameType.Solo)
					{
						return StringUtil.TR("LockedIn", "Global");
					}
				}
				return string.Empty;
			}
		default:
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
							else
							{
								if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
								{
									continue;
								}
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
	}

	private void HttpPost(string url, string postString, Action<string, string> callback)
	{
		StartCoroutine(HttpPostCoroutine(url, postString, callback));
	}

	private IEnumerator HttpPostCoroutine(string url, string postString, Action<string, string> callback)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		byte[] postBytes = utf8Encoding.GetBytes(postString);
		WWW client = new WWW(url, postBytes);
		try
		{
			yield return client;
			if (!client.error.IsNullOrEmpty())
			{
				while (true)
				{
					string arg;
					switch (3)
					{
					case 0:
						break;
					default:
						{
							int num = client.error.IndexOf(": ");
							if (client.error.StartsWith("Failed to connect to"))
							{
								if (num > 0)
								{
									if (num + 2 < client.error.Length)
									{
										arg = client.error.Substring(num + 2, client.error.Length - num - 2);
										goto IL_0165;
									}
								}
							}
							arg = client.error;
							goto IL_0165;
						}
						IL_0165:
						callback(null, arg);
						yield break;
					}
				}
			}
			callback(client.text, null);
		}
		finally
		{
			base._003C_003E__Finally0();
			goto end_IL_018f;
			IL_0192:
			switch (2)
			{
			default:
				goto end_IL_018f;
			case 0:
				goto IL_0192;
			}
			end_IL_018f:;
		}
	}

	public bool IsCurrentAlertQuest(int questId)
	{
		int result;
		if (AlertMissionsData != null)
		{
			if (AlertMissionsData.CurrentAlert != null && AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
			{
				result = ((AlertMissionsData.CurrentAlert.QuestId == questId) ? 1 : 0);
				goto IL_0061;
			}
		}
		result = 0;
		goto IL_0061;
		IL_0061:
		return (byte)result != 0;
	}

	public bool IsTimeOffset()
	{
		if (TimeOffset == default(TimeSpan))
		{
			return false;
		}
		return true;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		if (instanceId <= m_tierInstanceNames.Count)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_tierInstanceNames[instanceId - 1].ToString();
				}
			}
		}
		string text = string.Empty;
		switch (instanceId % 23)
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
		case 10:
			text += "Lucky ";
			break;
		case 11:
			text += "Noble ";
			break;
		case 12:
			text += "Merciful ";
			break;
		case 13:
			text += "Wise ";
			break;
		case 14:
			text += "Divine ";
			break;
		case 15:
			text += "Modest ";
			break;
		case 16:
			text += "Atomic ";
			break;
		case 17:
			text += "Electric ";
			break;
		case 18:
			text += "Nightmare ";
			break;
		case 19:
			text += "Red ";
			break;
		case 20:
			text += "Green ";
			break;
		case 21:
			text += "Dangerous ";
			break;
		case 22:
			text += "Mercurial ";
			break;
		}
		switch (instanceId % 29)
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
		case 10:
			text += "Greed";
			break;
		case 11:
			text += "Moon";
			break;
		case 12:
			text += "Star";
			break;
		case 13:
			text += "Mountain";
			break;
		case 14:
			text += "River";
			break;
		case 15:
			text += "Fate";
			break;
		case 16:
			text += "Doom";
			break;
		case 17:
			text += "Loot";
			break;
		case 18:
			text += "Panda";
			break;
		case 19:
			text += "Eagle";
			break;
		case 20:
			text += "Guardian";
			break;
		case 21:
			text += "Thunder";
			break;
		case 22:
			text += "Future";
			break;
		case 23:
			text += "Armor";
			break;
		case 24:
			text += "Shield";
			break;
		case 25:
			text += "Sword";
			break;
		case 26:
			text += "Payday";
			break;
		case 27:
			text += "Axe";
			break;
		case 28:
			text += "Briefcase";
			break;
		}
		return text;
	}

	public string GetTierName(GameType gameType, int tier)
	{
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
		{
			if (tier <= value.PerTierDefinitions.Count)
			{
				if (tier < 1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return StringUtil.TR("Placement", "RankMode");
						}
					}
				}
				TierDefinitions tierDefinitions = value.PerTierDefinitions[tier - 1];
				LocalizationPayload nameLocalization = tierDefinitions.NameLocalization;
				if (nameLocalization != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return nameLocalization.ToString();
						}
					}
				}
				return $"[Tier{tier}NotLoc'd]";
			}
			return $"[BadTier={tier}]";
		}
		return $"[BadGameType={gameType.ToString()}]";
	}

	public bool HasLeavingPenalty(GameType gameType)
	{
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value) && value.GameLeavingPenalty != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return value.GameLeavingPenalty.PointsGainedForLeaving > 0f;
				}
			}
		}
		return false;
	}

	public LocalizationPayload GetBlockingQueueRestriction(GameType gameType)
	{
		QueueBlockOutReasonDetails Details = new QueueBlockOutReasonDetails();
		return GetBlockingQueueRestriction(gameType, out Details);
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
			int num;
			if (GroupInfo != null)
			{
				num = (GroupInfo.InAGroup ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			RequirementMessageContext context = (num == 0) ? RequirementMessageContext.SoloQueueing : RequirementMessageContext.GroupQueueing;
			if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
			{
				if (!value.Requirements.IsNullOrEmpty())
				{
					if (!value.IsActive)
					{
						while (true)
						{
							return LocalizationPayload.Create((gameType != GameType.Ranked) ? "GameModeUnavailable@Global" : "RankedNotYetAvailable@RankMode");
						}
					}
					IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
					IEnumerator<QueueRequirement> enumerator = value.Requirements.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							QueueRequirement current = enumerator.Current;
							bool flag = false;
							if (!current.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
							{
								if (!current.AnyGroupMember)
								{
									LocalizationPayload result = current.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, context, out Details);
									Details.CausedBySelf = true;
									return result;
								}
								flag = true;
							}
							IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									IQueueRequirementApplicant current2 = enumerator2.Current;
									if (!current.DoesApplicantPass(queueRequirementSystemInfo, current2, gameType, null))
									{
										if (!current.AnyGroupMember)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													return current.GenerateFailure(queueRequirementSystemInfo, current2, context, out Details);
												}
											}
										}
									}
									else if (current.AnyGroupMember)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												break;
											default:
												flag = false;
												goto end_IL_013c;
											}
										}
									}
								}
								end_IL_013c:;
							}
							finally
							{
								if (enumerator2 != null)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											enumerator2.Dispose();
											goto end_IL_01b8;
										}
									}
								}
								end_IL_01b8:;
							}
							if (flag)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										return current.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, context, out Details);
									}
								}
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_0208;
								}
							}
						}
						end_IL_0208:;
					}
				}
			}
		}
		return null;
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
				if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
				{
					if (value.Requirements != null)
					{
						if (!value.IsActive)
						{
							result = false;
						}
						else
						{
							List<QueueRequirement> list = value.Requirements.ToList();
							if (!list.IsNullOrEmpty())
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
									{
										IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
										using (List<QueueRequirement>.Enumerator enumerator = list.GetEnumerator())
										{
											while (enumerator.MoveNext())
											{
												QueueRequirement current = enumerator.Current;
												bool flag = false;
												if (!current.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
												{
													if (!current.AnyGroupMember)
													{
														while (true)
														{
															switch (6)
															{
															case 0:
																break;
															default:
																return false;
															}
														}
													}
													flag = true;
												}
												GroupQueueApplicant scratchGroupQueueApplicant = ScratchGroupQueueApplicant;
												foreach (UpdateGroupMemberData member in GroupInfo.Members)
												{
													UpdateGroupMemberData updateGroupMemberData = scratchGroupQueueApplicant.m_member = member;
													if (!current.DoesApplicantPass(queueRequirementSystemInfo, scratchGroupQueueApplicant, gameType, null))
													{
														if (!current.AnyGroupMember)
														{
															while (true)
															{
																switch (5)
																{
																case 0:
																	break;
																default:
																	result = false;
																	goto end_IL_0119;
																}
															}
														}
													}
													else if (current.AnyGroupMember)
													{
														while (true)
														{
															switch (5)
															{
															case 0:
																break;
															default:
																flag = false;
																goto end_IL_0119;
															}
														}
													}
												}
												if (flag)
												{
													result = false;
												}
											}
											while (true)
											{
												switch (5)
												{
												case 0:
													break;
												default:
													return result;
												}
											}
										}
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
			if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value) && !value.QueueableGroupSizes.IsNullOrEmpty())
			{
				if (value.QueueableGroupSizes.TryGetValue(groupSize, out RequirementCollection value2))
				{
					if (!value2.IsNullOrEmpty())
					{
						IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
						IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
						using (IEnumerator<QueueRequirement> enumerator = value2.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								QueueRequirement current = enumerator.Current;
								if (!current.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
								{
									bool flag = false;
									if (current.AnyGroupMember)
									{
										IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												IQueueRequirementApplicant current2 = enumerator2.Current;
												if (current.DoesApplicantPass(queueRequirementSystemInfo, current2, gameType, null))
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
												while (true)
												{
													switch (5)
													{
													case 0:
														break;
													default:
														enumerator2.Dispose();
														goto end_IL_011c;
													}
												}
											}
											end_IL_011c:;
										}
									}
									if (!flag)
									{
										result = false;
									}
								}
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return result;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Warning("No valid GameTypeAvailabilites loaded yet for {0}", gameType);
					return null;
				}
			}
		}
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value) && !value.QueueableGroupSizes.IsNullOrEmpty())
		{
			if (!value.QueueableGroupSizes.TryGetValue(groupSize, out RequirementCollection value2))
			{
				return LocalizationPayload.Create("BadGroupSizeForQueue", "Matchmaking", LocalizationArg_Int32.Create(groupSize));
			}
			if (!value2.IsNullOrEmpty())
			{
				IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
				IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
				IEnumerator<QueueRequirement> enumerator = value2.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						QueueRequirement current = enumerator.Current;
						if (!current.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, null))
						{
							bool flag = false;
							if (current.AnyGroupMember)
							{
								IEnumerator<IQueueRequirementApplicant> enumerator2 = GroupMembersAsQueueApplicants.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										IQueueRequirementApplicant current2 = enumerator2.Current;
										if (current.DoesApplicantPass(queueRequirementSystemInfo, current2, gameType, null))
										{
											while (true)
											{
												switch (3)
												{
												case 0:
													break;
												default:
													flag = true;
													goto end_IL_00ff;
												}
											}
										}
									}
									end_IL_00ff:;
								}
								finally
								{
									if (enumerator2 != null)
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												enumerator2.Dispose();
												goto end_IL_0136;
											}
										}
									}
									end_IL_0136:;
								}
							}
							if (!flag)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
									{
										int num;
										if (groupSize > 1)
										{
											num = 2;
										}
										else
										{
											num = 1;
										}
										RequirementMessageContext requirementMessageContext = (RequirementMessageContext)num;
										LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(groupSize);
										LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType);
										LocalizationPayload payload = current.GenerateFailure(queueRequirementSystemInfo, queueRequirementApplicant, requirementMessageContext);
										LocalizationArg_LocalizationPayload localizationArg_LocalizationPayload = LocalizationArg_LocalizationPayload.Create(payload);
										if (requirementMessageContext == RequirementMessageContext.GroupQueueing)
										{
											return LocalizationPayload.Create("GroupSizeRequirementFailure", "matchmaking", localizationArg_Int, localizationArg_GameType, localizationArg_LocalizationPayload);
										}
										return LocalizationPayload.Create("SoloSizeRequirementFailure", "matchmaking", localizationArg_GameType, localizationArg_LocalizationPayload);
									}
									}
								}
							}
						}
					}
				}
				finally
				{
					if (enumerator != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								enumerator.Dispose();
								goto end_IL_020e;
							}
						}
					}
					end_IL_020e:;
				}
			}
		}
		return null;
	}

	public ushort GenerateGameSubTypeMaskForToggledAntiSocial(GameType gameType, ushort currentMask)
	{
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value) && !value.SubTypes.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GameSubType.CalculatePivotSubTypes(currentMask, GameSubType.SubTypeMods.AntiSocial, value.SubTypes);
				}
			}
		}
		return 0;
	}

	public bool IsMapInGameType(GameType gameType, string mapName, out bool isActive)
	{
		bool result = false;
		isActive = false;
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value) && !value.SubTypes.IsNullOrEmpty())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					using (List<GameSubType>.Enumerator enumerator = value.SubTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameSubType current = enumerator.Current;
							GameMapConfig gameMapConfig = current.GameMapConfigs.Where((GameMapConfig p) => p.Map == mapName).FirstOrDefault();
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
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	public Dictionary<ushort, GameSubType> GetGameTypeSubTypes(GameType gameType)
	{
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
		{
			if (!value.SubTypes.IsNullOrEmpty())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
						ushort num = 1;
						using (List<GameSubType>.Enumerator enumerator = value.SubTypes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GameSubType current = enumerator.Current;
								dictionary.Add(num, current);
								num = (ushort)(num << 1);
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return dictionary;
								}
							}
						}
					}
					}
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_lobbyGameClientInterface.SetGameTypeSubMasks(gameType, subGameMask, onResponseCallback);
						return;
					}
				}
			}
		}
		if (onResponseCallback == null)
		{
			return;
		}
		while (true)
		{
			onResponseCallback(new SetGameSubTypeResponse
			{
				Success = false,
				ErrorMessage = "Not connected to lobby"
			});
			return;
		}
	}

	public void SetNewSessionLanguage(string languageCode)
	{
		if (m_lobbyGameClientInterface == null)
		{
			return;
		}
		while (true)
		{
			if (m_lobbyGameClientInterface.IsConnected)
			{
				while (true)
				{
					m_lobbyGameClientInterface.SetNewSessionLanguage(languageCode);
					return;
				}
			}
			return;
		}
	}

	public bool IsFreelancerInFreeRotationExtension(CharacterType characterType, GameType gameType, GameSubType gameSubType = null)
	{
		if (!FreeRotationAdditions.IsNullOrEmpty() && FreeRotationAdditions.TryGetValue(characterType, out RequirementCollection value))
		{
			IQueueRequirementApplicant queueRequirementApplicant = QueueRequirementApplicant;
			IQueueRequirementSystemInfo queueRequirementSystemInfo = QueueRequirementSystemInfo;
			if (value.DoesApplicantPass(queueRequirementSystemInfo, queueRequirementApplicant, gameType, gameSubType))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}
}
