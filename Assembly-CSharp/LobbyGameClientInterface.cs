using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LobbyGameClientMessages;
using Steamworks;
using UnityEngine;
using WebSocketSharp;

public class LobbyGameClientInterface : WebSocketInterface
{
	public static bool BlockSendingGroupUpdates;

	protected string m_lobbyServerAddress;

	protected string m_directoryServerAddress;

	protected int m_preferredLobbyServerIndex;

	protected LobbySessionInfo m_sessionInfo;

	protected AuthTicket m_ticket;

	protected bool m_registered;

	private string m_lastLobbyErrorMessage;

	private bool m_allowRelogin;

	protected WebSocketMessageDispatcher<LobbyGameClientInterface> m_messageDispatcher;

	public LobbyGameClientInterface() : base(Factory.Get())
    {
		this.OnConnected = delegate(RegisterGameClientResponse A_0)
		{
		};
		
		this.OnDisconnected = delegate(string A_0, bool A_1, CloseStatusCode A_2)
			{
			};
		this.OnLobbyServerReadyNotification = delegate(LobbyServerReadyNotification A_0)
		{
		};
		
		this.OnLobbyStatusNotification = delegate(LobbyStatusNotification A_0)
			{
			};
		
		this.OnLobbyGameplayOverridesNotification = delegate(LobbyGameplayOverridesNotification A_0)
			{
			};
		
		this.OnLobbyCustomGamesNotification = delegate(LobbyCustomGamesNotification A_0)
			{
			};
		this.OnQueueAssignmentNotification = delegate(MatchmakingQueueAssignmentNotification A_0)
		{
		};
		
		this.OnQueueStatusNotification = delegate(MatchmakingQueueStatusNotification A_0)
			{
			};
		
		this.OnGameAssignmentNotification = delegate(GameAssignmentNotification A_0)
			{
			};
		
		this.OnGameInfoNotification = delegate(GameInfoNotification A_0)
			{
			};
		
		this.OnGameStatusNotification = delegate(GameStatusNotification A_0)
			{
			};
		
		this.OnAccountDataUpdated = delegate(PlayerAccountDataUpdateNotification A_0)
			{
			};
		
		this.OnForcedCharacterChangeFromServerNotification = delegate(ForcedCharacterChangeFromServerNotification A_0)
			{
			};
		
		this.OnCharacterDataUpdateNotification = delegate(PlayerCharacterDataUpdateNotification A_0)
			{
			};
		
		this.OnInventoryComponentUpdateNotification = delegate(InventoryComponentUpdateNotification A_0)
			{
			};
		
		this.OnBankBalanceChangeNotification = delegate(BankBalanceChangeNotification A_0)
			{
			};
		
		this.OnSeasonStatusNotification = delegate(SeasonStatusNotification A_0)
			{
			};
		this.OnChapterStatusNotification = delegate(ChapterStatusNotification A_0)
		{
		};
		
		this.OnGroupUpdateNotification = delegate(GroupUpdateNotification A_0)
			{
			};
		
		this.OnUseGGPackNotification = delegate(UseGGPackNotification A_0)
			{
			};
		
		this.OnChatNotification = delegate(ChatNotification A_0)
			{
			};
		
		this.OnUseOverconNotification = delegate(UseOverconResponse A_0)
			{
			};
		
		this.OnFriendStatusNotification = delegate(FriendStatusNotification A_0)
			{
			};
		this.OnGroupConfirmation = delegate(GroupConfirmationRequest A_0)
		{
		};
		
		this.OnGroupSuggestion = delegate(GroupSuggestionRequest A_0)
			{
			};
		
		this.OnForceQueueNotification = delegate(ForceMatchmakingQueueNotification A_0)
			{
			};
		
		this.OnGameInviteConfirmationRequest = delegate(GameInviteConfirmationRequest A_0)
			{
			};
		
		this.OnQuestCompleteNotification = delegate(QuestCompleteNotification A_0)
			{
			};
		
		this.OnMatchResultsNotification = delegate(MatchResultsNotification A_0)
			{
			};
		
		this.OnServerQueueConfigurationUpdateNotification = delegate(ServerQueueConfigurationUpdateNotification A_0)
			{
			};
		
		this.OnRankedOverviewChangeNotification = delegate(RankedOverviewChangeNotification A_0)
			{
			};
		this.OnFactionCompetitionNotification = delegate(FactionCompetitionNotification A_0)
		{
		};
		this.OnTrustBoostUsedNotification = delegate(TrustBoostUsedNotification A_0)
		{
		};
		
		this.OnFacebookAccessTokenNotification = delegate(FacebookAccessTokenNotification A_0)
			{
			};
		
		this.OnPlayerFactionContributionChange = delegate(PlayerFactionContributionChangeNotification A_0)
			{
			};
		
		this.OnFactionLoginRewardNotification = delegate(FactionLoginRewardNotification A_0)
			{
			};
		
		this.OnLobbyAlertMissionDataNotification = delegate(LobbyAlertMissionDataNotification A_0)
			{
			};
		
		this.OnLobbySeasonQuestDataNotification = delegate(LobbySeasonQuestDataNotification A_0)
			{
			};
		this.m_registered = false;
		this.m_sessionInfo = new LobbySessionInfo();
		base.ConnectionTimeout = 30f;
	}

	public event Action<RegisterGameClientResponse> OnConnected
	{
		add
		{
			Action<RegisterGameClientResponse> action = this.OnConnected;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RegisterGameClientResponse>>(ref this.OnConnected, (Action<RegisterGameClientResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<RegisterGameClientResponse> action = this.OnConnected;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RegisterGameClientResponse>>(ref this.OnConnected, (Action<RegisterGameClientResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<string, bool, CloseStatusCode> OnDisconnected
	{
		add
		{
			Action<string, bool, CloseStatusCode> action = this.OnDisconnected;
			Action<string, bool, CloseStatusCode> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string, bool, CloseStatusCode>>(ref this.OnDisconnected, (Action<string, bool, CloseStatusCode>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, bool, CloseStatusCode> action = this.OnDisconnected;
			Action<string, bool, CloseStatusCode> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string, bool, CloseStatusCode>>(ref this.OnDisconnected, (Action<string, bool, CloseStatusCode>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbyServerReadyNotification> OnLobbyServerReadyNotification
	{
		add
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotification;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyServerReadyNotification>>(ref this.OnLobbyServerReadyNotification, (Action<LobbyServerReadyNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotification;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyServerReadyNotification>>(ref this.OnLobbyServerReadyNotification, (Action<LobbyServerReadyNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbyStatusNotification> OnLobbyStatusNotification
	{
		add
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotification;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyStatusNotification>>(ref this.OnLobbyStatusNotification, (Action<LobbyStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotification;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyStatusNotification>>(ref this.OnLobbyStatusNotification, (Action<LobbyStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbyGameplayOverridesNotification> OnLobbyGameplayOverridesNotification
	{
		add
		{
			Action<LobbyGameplayOverridesNotification> action = this.OnLobbyGameplayOverridesNotification;
			Action<LobbyGameplayOverridesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyGameplayOverridesNotification>>(ref this.OnLobbyGameplayOverridesNotification, (Action<LobbyGameplayOverridesNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyGameplayOverridesNotification> action = this.OnLobbyGameplayOverridesNotification;
			Action<LobbyGameplayOverridesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyGameplayOverridesNotification>>(ref this.OnLobbyGameplayOverridesNotification, (Action<LobbyGameplayOverridesNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotification
	{
		add
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotification;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyCustomGamesNotification>>(ref this.OnLobbyCustomGamesNotification, (Action<LobbyCustomGamesNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotification;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyCustomGamesNotification>>(ref this.OnLobbyCustomGamesNotification, (Action<LobbyCustomGamesNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<MatchmakingQueueAssignmentNotification> OnQueueAssignmentNotification
	{
		add
		{
			Action<MatchmakingQueueAssignmentNotification> action = this.OnQueueAssignmentNotification;
			Action<MatchmakingQueueAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueAssignmentNotification>>(ref this.OnQueueAssignmentNotification, (Action<MatchmakingQueueAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchmakingQueueAssignmentNotification> action = this.OnQueueAssignmentNotification;
			Action<MatchmakingQueueAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueAssignmentNotification>>(ref this.OnQueueAssignmentNotification, (Action<MatchmakingQueueAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<MatchmakingQueueStatusNotification> OnQueueStatusNotification
	{
		add
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotification;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueStatusNotification>>(ref this.OnQueueStatusNotification, (Action<MatchmakingQueueStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotification;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchmakingQueueStatusNotification>>(ref this.OnQueueStatusNotification, (Action<MatchmakingQueueStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GameAssignmentNotification> OnGameAssignmentNotification
	{
		add
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotification;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameAssignmentNotification>>(ref this.OnGameAssignmentNotification, (Action<GameAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotification;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameAssignmentNotification>>(ref this.OnGameAssignmentNotification, (Action<GameAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GameInfoNotification> OnGameInfoNotification
	{
		add
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotification;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameInfoNotification>>(ref this.OnGameInfoNotification, (Action<GameInfoNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotification;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameInfoNotification>>(ref this.OnGameInfoNotification, (Action<GameInfoNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GameStatusNotification> OnGameStatusNotification
	{
		add
		{
			Action<GameStatusNotification> action = this.OnGameStatusNotification;
			Action<GameStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameStatusNotification>>(ref this.OnGameStatusNotification, (Action<GameStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameStatusNotification> action = this.OnGameStatusNotification;
			Action<GameStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GameStatusNotification>>(ref this.OnGameStatusNotification, (Action<GameStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<PlayerAccountDataUpdateNotification> OnAccountDataUpdated
	{
		add
		{
			Action<PlayerAccountDataUpdateNotification> action = this.OnAccountDataUpdated;
			Action<PlayerAccountDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerAccountDataUpdateNotification>>(ref this.OnAccountDataUpdated, (Action<PlayerAccountDataUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PlayerAccountDataUpdateNotification> action = this.OnAccountDataUpdated;
			Action<PlayerAccountDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerAccountDataUpdateNotification>>(ref this.OnAccountDataUpdated, (Action<PlayerAccountDataUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<ForcedCharacterChangeFromServerNotification> OnForcedCharacterChangeFromServerNotification;

	public event Action<PlayerCharacterDataUpdateNotification> OnCharacterDataUpdateNotification
	{
		add
		{
			Action<PlayerCharacterDataUpdateNotification> action = this.OnCharacterDataUpdateNotification;
			Action<PlayerCharacterDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerCharacterDataUpdateNotification>>(ref this.OnCharacterDataUpdateNotification, (Action<PlayerCharacterDataUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PlayerCharacterDataUpdateNotification> action = this.OnCharacterDataUpdateNotification;
			Action<PlayerCharacterDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerCharacterDataUpdateNotification>>(ref this.OnCharacterDataUpdateNotification, (Action<PlayerCharacterDataUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<InventoryComponentUpdateNotification> OnInventoryComponentUpdateNotification
	{
		add
		{
			Action<InventoryComponentUpdateNotification> action = this.OnInventoryComponentUpdateNotification;
			Action<InventoryComponentUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<InventoryComponentUpdateNotification>>(ref this.OnInventoryComponentUpdateNotification, (Action<InventoryComponentUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<InventoryComponentUpdateNotification> action = this.OnInventoryComponentUpdateNotification;
			Action<InventoryComponentUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<InventoryComponentUpdateNotification>>(ref this.OnInventoryComponentUpdateNotification, (Action<InventoryComponentUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<BankBalanceChangeNotification> OnBankBalanceChangeNotification
	{
		add
		{
			Action<BankBalanceChangeNotification> action = this.OnBankBalanceChangeNotification;
			Action<BankBalanceChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<BankBalanceChangeNotification>>(ref this.OnBankBalanceChangeNotification, (Action<BankBalanceChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<BankBalanceChangeNotification> action = this.OnBankBalanceChangeNotification;
			Action<BankBalanceChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<BankBalanceChangeNotification>>(ref this.OnBankBalanceChangeNotification, (Action<BankBalanceChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<SeasonStatusNotification> OnSeasonStatusNotification
	{
		add
		{
			Action<SeasonStatusNotification> action = this.OnSeasonStatusNotification;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SeasonStatusNotification>>(ref this.OnSeasonStatusNotification, (Action<SeasonStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<SeasonStatusNotification> action = this.OnSeasonStatusNotification;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<SeasonStatusNotification>>(ref this.OnSeasonStatusNotification, (Action<SeasonStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ChapterStatusNotification> OnChapterStatusNotification
	{
		add
		{
			Action<ChapterStatusNotification> action = this.OnChapterStatusNotification;
			Action<ChapterStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChapterStatusNotification>>(ref this.OnChapterStatusNotification, (Action<ChapterStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ChapterStatusNotification> action = this.OnChapterStatusNotification;
			Action<ChapterStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChapterStatusNotification>>(ref this.OnChapterStatusNotification, (Action<ChapterStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GroupUpdateNotification> OnGroupUpdateNotification
	{
		add
		{
			Action<GroupUpdateNotification> action = this.OnGroupUpdateNotification;
			Action<GroupUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupUpdateNotification>>(ref this.OnGroupUpdateNotification, (Action<GroupUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GroupUpdateNotification> action = this.OnGroupUpdateNotification;
			Action<GroupUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupUpdateNotification>>(ref this.OnGroupUpdateNotification, (Action<GroupUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<UseGGPackNotification> OnUseGGPackNotification
	{
		add
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotification;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseGGPackNotification>>(ref this.OnUseGGPackNotification, (Action<UseGGPackNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotification;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseGGPackNotification>>(ref this.OnUseGGPackNotification, (Action<UseGGPackNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ChatNotification> OnChatNotification
	{
		add
		{
			Action<ChatNotification> action = this.OnChatNotification;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChatNotification>>(ref this.OnChatNotification, (Action<ChatNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ChatNotification> action = this.OnChatNotification;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ChatNotification>>(ref this.OnChatNotification, (Action<ChatNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<UseOverconResponse> OnUseOverconNotification
	{
		add
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotification;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseOverconResponse>>(ref this.OnUseOverconNotification, (Action<UseOverconResponse>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotification;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<UseOverconResponse>>(ref this.OnUseOverconNotification, (Action<UseOverconResponse>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<FriendStatusNotification> OnFriendStatusNotification
	{
		add
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotification;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FriendStatusNotification>>(ref this.OnFriendStatusNotification, (Action<FriendStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotification;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FriendStatusNotification>>(ref this.OnFriendStatusNotification, (Action<FriendStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GroupConfirmationRequest> OnGroupConfirmation
	{
		add
		{
			Action<GroupConfirmationRequest> action = this.OnGroupConfirmation;
			Action<GroupConfirmationRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupConfirmationRequest>>(ref this.OnGroupConfirmation, (Action<GroupConfirmationRequest>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GroupConfirmationRequest> action = this.OnGroupConfirmation;
			Action<GroupConfirmationRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupConfirmationRequest>>(ref this.OnGroupConfirmation, (Action<GroupConfirmationRequest>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GroupSuggestionRequest> OnGroupSuggestion
	{
		add
		{
			Action<GroupSuggestionRequest> action = this.OnGroupSuggestion;
			Action<GroupSuggestionRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupSuggestionRequest>>(ref this.OnGroupSuggestion, (Action<GroupSuggestionRequest>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GroupSuggestionRequest> action = this.OnGroupSuggestion;
			Action<GroupSuggestionRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<GroupSuggestionRequest>>(ref this.OnGroupSuggestion, (Action<GroupSuggestionRequest>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ForceMatchmakingQueueNotification> OnForceQueueNotification
	{
		add
		{
			Action<ForceMatchmakingQueueNotification> action = this.OnForceQueueNotification;
			Action<ForceMatchmakingQueueNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ForceMatchmakingQueueNotification>>(ref this.OnForceQueueNotification, (Action<ForceMatchmakingQueueNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ForceMatchmakingQueueNotification> action = this.OnForceQueueNotification;
			Action<ForceMatchmakingQueueNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ForceMatchmakingQueueNotification>>(ref this.OnForceQueueNotification, (Action<ForceMatchmakingQueueNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<GameInviteConfirmationRequest> OnGameInviteConfirmationRequest;

	public event Action<QuestCompleteNotification> OnQuestCompleteNotification
	{
		add
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotification;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestCompleteNotification>>(ref this.OnQuestCompleteNotification, (Action<QuestCompleteNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotification;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<QuestCompleteNotification>>(ref this.OnQuestCompleteNotification, (Action<QuestCompleteNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<MatchResultsNotification> OnMatchResultsNotification
	{
		add
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotification;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchResultsNotification>>(ref this.OnMatchResultsNotification, (Action<MatchResultsNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotification;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<MatchResultsNotification>>(ref this.OnMatchResultsNotification, (Action<MatchResultsNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotification
	{
		add
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotification;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerQueueConfigurationUpdateNotification>>(ref this.OnServerQueueConfigurationUpdateNotification, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotification;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<ServerQueueConfigurationUpdateNotification>>(ref this.OnServerQueueConfigurationUpdateNotification, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<RankedOverviewChangeNotification> OnRankedOverviewChangeNotification
	{
		add
		{
			Action<RankedOverviewChangeNotification> action = this.OnRankedOverviewChangeNotification;
			Action<RankedOverviewChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RankedOverviewChangeNotification>>(ref this.OnRankedOverviewChangeNotification, (Action<RankedOverviewChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<RankedOverviewChangeNotification> action = this.OnRankedOverviewChangeNotification;
			Action<RankedOverviewChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<RankedOverviewChangeNotification>>(ref this.OnRankedOverviewChangeNotification, (Action<RankedOverviewChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<FactionCompetitionNotification> OnFactionCompetitionNotification
	{
		add
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotification;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionCompetitionNotification>>(ref this.OnFactionCompetitionNotification, (Action<FactionCompetitionNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotification;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionCompetitionNotification>>(ref this.OnFactionCompetitionNotification, (Action<FactionCompetitionNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<TrustBoostUsedNotification> OnTrustBoostUsedNotification
	{
		add
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotification;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TrustBoostUsedNotification>>(ref this.OnTrustBoostUsedNotification, (Action<TrustBoostUsedNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotification;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<TrustBoostUsedNotification>>(ref this.OnTrustBoostUsedNotification, (Action<TrustBoostUsedNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<FacebookAccessTokenNotification> OnFacebookAccessTokenNotification
	{
		add
		{
			Action<FacebookAccessTokenNotification> action = this.OnFacebookAccessTokenNotification;
			Action<FacebookAccessTokenNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FacebookAccessTokenNotification>>(ref this.OnFacebookAccessTokenNotification, (Action<FacebookAccessTokenNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FacebookAccessTokenNotification> action = this.OnFacebookAccessTokenNotification;
			Action<FacebookAccessTokenNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FacebookAccessTokenNotification>>(ref this.OnFacebookAccessTokenNotification, (Action<FacebookAccessTokenNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChange
	{
		add
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChange;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerFactionContributionChangeNotification>>(ref this.OnPlayerFactionContributionChange, (Action<PlayerFactionContributionChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChange;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<PlayerFactionContributionChangeNotification>>(ref this.OnPlayerFactionContributionChange, (Action<PlayerFactionContributionChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<FactionLoginRewardNotification> OnFactionLoginRewardNotification
	{
		add
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotification;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionLoginRewardNotification>>(ref this.OnFactionLoginRewardNotification, (Action<FactionLoginRewardNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotification;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<FactionLoginRewardNotification>>(ref this.OnFactionLoginRewardNotification, (Action<FactionLoginRewardNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbyAlertMissionDataNotification> OnLobbyAlertMissionDataNotification
	{
		add
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnLobbyAlertMissionDataNotification;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyAlertMissionDataNotification>>(ref this.OnLobbyAlertMissionDataNotification, (Action<LobbyAlertMissionDataNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnLobbyAlertMissionDataNotification;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbyAlertMissionDataNotification>>(ref this.OnLobbyAlertMissionDataNotification, (Action<LobbyAlertMissionDataNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<LobbySeasonQuestDataNotification> OnLobbySeasonQuestDataNotification
	{
		add
		{
			Action<LobbySeasonQuestDataNotification> action = this.OnLobbySeasonQuestDataNotification;
			Action<LobbySeasonQuestDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbySeasonQuestDataNotification>>(ref this.OnLobbySeasonQuestDataNotification, (Action<LobbySeasonQuestDataNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<LobbySeasonQuestDataNotification> action = this.OnLobbySeasonQuestDataNotification;
			Action<LobbySeasonQuestDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<LobbySeasonQuestDataNotification>>(ref this.OnLobbySeasonQuestDataNotification, (Action<LobbySeasonQuestDataNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool IsConnected
	{
		get
		{
			return base.State == global::WebSocket.SocketState.Open && this.m_registered;
		}
	}

	public LobbySessionInfo SessionInfo
	{
		get
		{
			return this.m_sessionInfo;
		}
	}

	public void Initialize(string directoryServerAddress, AuthTicket ticket, Region region, string languageCode, ProcessType processType, int preferredLobbyServerIndex = 0)
	{
		this.m_lobbyServerAddress = null;
		this.m_directoryServerAddress = directoryServerAddress;
		this.m_sessionInfo.BuildVersion = BuildVersion.ShortVersionString;
		this.m_sessionInfo.ProtocolVersion = Factory.Get().ProtocolVersion;
		this.m_sessionInfo.AccountId = ticket.AccountId;
		this.m_sessionInfo.UserName = ticket.UserName;
		this.m_sessionInfo.Handle = ticket.Handle;
		this.m_sessionInfo.ProcessType = processType;
		this.m_sessionInfo.Region = region;
		this.m_sessionInfo.LanguageCode = languageCode;
		this.m_ticket = ticket;
		this.m_preferredLobbyServerIndex = preferredLobbyServerIndex;
		this.m_messageDispatcher = new WebSocketMessageDispatcher<LobbyGameClientInterface>();
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionOpenedNotification>(new Action<ConnectionOpenedNotification>(this.HandleConnectionOpenedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionClosedNotification>(new Action<ConnectionClosedNotification>(this.HandleConnectionClosedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionErrorNotification>(new Action<ConnectionErrorNotification>(this.HandleConnectionErrorNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbyServerReadyNotification>(new Action<LobbyServerReadyNotification>(this.HandleLobbyServerReadyNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbyStatusNotification>(new Action<LobbyStatusNotification>(this.HandleLobbyStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbyGameplayOverridesNotification>(new Action<LobbyGameplayOverridesNotification>(this.HandleLobbyGameplayOverridesNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbyCustomGamesNotification>(new Action<LobbyCustomGamesNotification>(this.HandleLobbyCustomGamesNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueStatusNotification>(new Action<MatchmakingQueueStatusNotification>(this.HandleQueueStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueAssignmentNotification>(new Action<MatchmakingQueueAssignmentNotification>(this.HandleQueueAssignmentNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GameAssignmentNotification>(new Action<GameAssignmentNotification>(this.HandleGameAssignmentNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GameInfoNotification>(new Action<GameInfoNotification>(this.HandleGameInfoNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GameStatusNotification>(new Action<GameStatusNotification>(this.HandleGameStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<SynchronizeWithClientOutOfGameRequest>(new Action<SynchronizeWithClientOutOfGameRequest>(this.HandleSynchronizeWithClientOutOfGameRequest));
		this.m_messageDispatcher.RegisterMessageDelegate<ForcedCharacterChangeFromServerNotification>(new Action<ForcedCharacterChangeFromServerNotification>(this.HandleRequeueForceCharacterNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<PlayerCharacterDataUpdateNotification>(new Action<PlayerCharacterDataUpdateNotification>(this.HandleCharacterDataUpdateNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<InventoryComponentUpdateNotification>(new Action<InventoryComponentUpdateNotification>(this.HandleInventoryComponentUpdateNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<BankBalanceChangeNotification>(new Action<BankBalanceChangeNotification>(this.HandleBankBalanceChangeNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<PlayerAccountDataUpdateNotification>(new Action<PlayerAccountDataUpdateNotification>(this.HandleAccountDataUpdateNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<SeasonStatusNotification>(new Action<SeasonStatusNotification>(this.HandleSeasonStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ChapterStatusNotification>(new Action<ChapterStatusNotification>(this.HandleChapterStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GroupUpdateNotification>(new Action<GroupUpdateNotification>(this.HandleGroupUpdateNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<UseGGPackNotification>(new Action<UseGGPackNotification>(this.HandleGGPackUsedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ChatNotification>(new Action<ChatNotification>(this.HandleChatNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<UseOverconResponse>(new Action<UseOverconResponse>(this.HandleUseOverconResponse));
		this.m_messageDispatcher.RegisterMessageDelegate<FriendStatusNotification>(new Action<FriendStatusNotification>(this.HandleFriendStatusNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GroupConfirmationRequest>(new Action<GroupConfirmationRequest>(this.HandleGroupConfirmationRequest));
		this.m_messageDispatcher.RegisterMessageDelegate<GroupSuggestionRequest>(new Action<GroupSuggestionRequest>(this.HandleGroupSuggestionRequest));
		this.m_messageDispatcher.RegisterMessageDelegate<GameInviteConfirmationRequest>(new Action<GameInviteConfirmationRequest>(this.HandleGameInviteConfirmationRequest));
		this.m_messageDispatcher.RegisterMessageDelegate<QuestCompleteNotification>(new Action<QuestCompleteNotification>(this.HandleQuestCompleteNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<MatchResultsNotification>(new Action<MatchResultsNotification>(this.HandleMatchResultsNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<PendingPurchaseResult>(new Action<PendingPurchaseResult>(this.HandlePendingPurchaseResult));
		this.m_messageDispatcher.RegisterMessageDelegate<ForceMatchmakingQueueNotification>(new Action<ForceMatchmakingQueueNotification>(this.HandleForceQueueNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<FreelancerUnavailableNotification>(new Action<FreelancerUnavailableNotification>(this.HandleFreelancerUnavailableNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<EnterFreelancerResolutionPhaseNotification>(new Action<EnterFreelancerResolutionPhaseNotification>(this.HandleResolvingDuplicateFreelancerNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueToPlayersNotification>(new Action<MatchmakingQueueToPlayersNotification>(this.HandleQueueToPlayerNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ServerQueueConfigurationUpdateNotification>(new Action<ServerQueueConfigurationUpdateNotification>(this.HandleServerQueueConfigurationUpdateNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<RankedOverviewChangeNotification>(new Action<RankedOverviewChangeNotification>(this.HandleRankedOverviewChangeNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<GameDestroyedByPlayerNotification>(new Action<GameDestroyedByPlayerNotification>(this.HandleGameDestroyedByPlayerNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<FactionCompetitionNotification>(new Action<FactionCompetitionNotification>(this.HandleFactionCompetitionNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<TrustBoostUsedNotification>(new Action<TrustBoostUsedNotification>(this.HandleTrustBoostUsedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<FacebookAccessTokenNotification>(new Action<FacebookAccessTokenNotification>(this.HandleFacebookAccessTokenNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<PlayerFactionContributionChangeNotification>(new Action<PlayerFactionContributionChangeNotification>(this.HandlePlayerFactionContributionChangeNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<FactionLoginRewardNotification>(new Action<FactionLoginRewardNotification>(this.HandleFactionLoginRewardNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbyAlertMissionDataNotification>(new Action<LobbyAlertMissionDataNotification>(this.HandleLobbyAlertMissionDataNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<LobbySeasonQuestDataNotification>(new Action<LobbySeasonQuestDataNotification>(this.HandleLobbySeasonQuestDataNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ErrorReportSummaryRequest>(new Action<ErrorReportSummaryRequest>(this.HandleErrorReportSummaryRequest));
		this.m_lastLobbyErrorMessage = null;
		this.m_allowRelogin = true;
	}

	public void AssignGameClient(string directoryServerAddress)
	{
		int requestId = this.m_messageDispatcher.GetRequestId();
		this.m_messageDispatcher.RegisterMessageDelegate<RegisterGameClientResponse>(new Action<RegisterGameClientResponse>(this.HandleRegisterGameClientResponse), requestId);
		RegisterGameClientResponse registerResponse = new RegisterGameClientResponse();
		registerResponse.Success = false;
		registerResponse.ResponseId = requestId;
		Action handleNetworkException = delegate()
		{
			registerResponse.Success = false;
			registerResponse.LocalizedFailure = LocalizationPayload.Create("NetworkError", "Global");
			this.m_messageDispatcher.HandleMessage(this, registerResponse);
		};
		Action<string> handleRequestFailure = delegate(string message)
		{
			registerResponse.Success = false;
			registerResponse.ErrorMessage = message;
			this.m_messageDispatcher.HandleMessage(this, registerResponse);
		};
		try
		{
			this.m_overallConnectionTimer.Start();
			AssignGameClientRequest assignGameClientRequest = new AssignGameClientRequest();
			assignGameClientRequest.RequestId = this.m_messageDispatcher.GetRequestId();
			assignGameClientRequest.SessionInfo = this.m_sessionInfo;
			assignGameClientRequest.AuthInfo = this.m_ticket.AuthInfo;
			assignGameClientRequest.PreferredLobbyServerIndex = this.m_preferredLobbyServerIndex;
			if (directoryServerAddress.IndexOf("://") == -1)
			{
				directoryServerAddress = "ws://" + directoryServerAddress;
			}
			Uri uri = new Uri(directoryServerAddress);
			UriBuilder newDirectoryServerUri = new UriBuilder();
			int num = 0x17A2;
			string str = "DirectorySessionManager";
			UriBuilder newDirectoryServerUri4 = newDirectoryServerUri;
			string scheme;
			if (!(uri.Scheme == "ws"))
			{
				if (!(uri.Scheme == "http"))
				{
					if (!(uri.Scheme == "wss"))
					{
						if (!(uri.Scheme == "https"))
						{
							scheme = newDirectoryServerUri.Scheme;
							goto IL_1AC;
						}
					}
					scheme = "https";
					goto IL_1AC;
				}
			}
			scheme = "http";
			IL_1AC:
			newDirectoryServerUri4.Scheme = scheme;
			newDirectoryServerUri.Host = NetUtil.GetIPv4Address(uri.Host).ToString();
			UriBuilder newDirectoryServerUri2 = newDirectoryServerUri;
			int port;
			if (uri.Port > 0)
			{
				if (!uri.IsDefaultPort)
				{
					port = uri.Port;
					goto IL_211;
				}
			}
			port = num;
			IL_211:
			newDirectoryServerUri2.Port = port;
			UriBuilder newDirectoryServerUri3 = newDirectoryServerUri;
			string path;
			if (uri.AbsolutePath != "/")
			{
				path = uri.AbsolutePath;
			}
			else
			{
				path = "/" + str;
			}
			newDirectoryServerUri3.Path = path;
			newDirectoryServerUri.Query = string.Format("messageType={0}", assignGameClientRequest.GetType().Name);
			base.Logger.Info("Requesting lobby server assignment from {0}", new object[]
			{
				newDirectoryServerUri
			});
			base.SendHttpRequest<AssignGameClientResponse>(newDirectoryServerUri.ToString(), assignGameClientRequest, delegate(AssignGameClientResponse assignResponse, Exception exception)
			{
				try
				{
					if (exception != null)
					{
						if (this.m_overallConnectionTimer.Elapsed.TotalSeconds >= (double)this.ConnectionTimeout)
						{
							this.m_overallConnectionTimer.Reset();
							throw exception;
						}
						this.Logger.Info("Re-requesting lobby server assignment from {0}: {1}", new object[]
						{
							newDirectoryServerUri,
							exception.Message.Trim()
						});
						this.Reconnect();
					}
					else
					{
						if (!assignResponse.Success)
						{
							throw new ClientRequestFailed(assignResponse.ErrorMessage);
						}
						if (assignResponse.LobbyServerAddress.IsNullOrEmpty())
						{
							throw new ClientRequestFailed("Empty response from server");
						}
						this.m_lobbyServerAddress = assignResponse.LobbyServerAddress;
						this.m_sessionInfo = assignResponse.SessionInfo;
						this.Connect();
					}
				}
				catch (ClientRequestFailed clientRequestFailed)
				{
					handleRequestFailure(clientRequestFailed.Message);
				}
				catch (Exception)
				{
					handleNetworkException();
				}
			});
		}
		catch (Exception)
		{
			handleNetworkException();
		}
	}

	public override void Connect()
	{
		this.m_registered = false;
		this.m_sessionInfo.IsBinary = base.IsBinary;
		if (this.m_lobbyServerAddress == null)
		{
			if (this.m_directoryServerAddress == null)
			{
				throw new Exception("Directory server address must be specified");
			}
			this.AssignGameClient(this.m_directoryServerAddress);
		}
		else
		{
			base.InitializeSocket(this.m_lobbyServerAddress, 0x17AC, "LobbyGameClientSessionManager");
			base.Connect();
		}
	}

	public override void Update()
	{
		base.Update();
	}

	private void HandleForceQueueNotification(ForceMatchmakingQueueNotification notification)
	{
		this.OnForceQueueNotification(notification);
	}

	private void HandleQueueToPlayerNotification(MatchmakingQueueToPlayersNotification notification)
	{
		if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage.symbol_0012)
		{
			AppState_GroupCharacterSelect.Get().ReEnter(true);
			UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true, null);
		}
		else if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage.symbol_0015)
		{
			string description = StringUtil.TR("RuinedGameStartSoThrownOutOfQueue", "Global");
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), description, StringUtil.TR("Ok", "Global"), null, -1, false);
			Log.Info("Updating ready state to false because ruined game, thrown out of queue, current Appstate: " + AppState.GetCurrent().ToString(), new object[0]);
			AppState_GroupCharacterSelect.Get().UpdateReadyState(false);
		}
		else if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage.symbol_000E)
		{
			ClientGameManager.Get().HandleQueueConfirmation();
		}
		else
		{
			string description2 = StringUtil.TR("UnknownQueueManagerBug", "Global");
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), description2, StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		this.OnServerQueueConfigurationUpdateNotification(notification);
	}

	private void HandleGameDestroyedByPlayerNotification(GameDestroyedByPlayerNotification notification)
	{
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), notification.LocalizedMessage.ToString(), StringUtil.TR("Ok", "Global"), null, -1, false);
	}

	private void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		if (notification.SubPhase == FreelancerResolutionPhaseSubType.DUPLICATE_FREELANCER)
		{
			UIMatchStartPanel.Get().NotifyDuplicateFreelancer(true);
		}
		else if (notification.RankedData != null)
		{
			if (notification.SubPhase != FreelancerResolutionPhaseSubType.WAITING_FOR_ALL_PLAYERS)
			{
				if (UIRankedModeDraftScreen.Get() == null)
				{
					return;
				}
				if (AppState_RankModeDraft.Get() != AppState.GetCurrent())
				{
					AppState_RankModeDraft.Get().Enter();
				}
				UIRankedModeDraftScreen.Get().HandleResolvingDuplicateFreelancerNotification(notification);
				if (!notification.SubPhase.IsPickBanSubPhase())
				{
					if (!notification.SubPhase.IsPickFreelancerSubPhase())
					{
						return;
					}
				}
				RankedResolutionPhaseData value = notification.RankedData.Value;
				int ourPlayerId = UIRankedModeDraftScreen.Get().OurPlayerId;
				if (value.symbol_001D(ourPlayerId))
				{
					System.Random rnd = new System.Random();
					List<CharacterType> list = new List<CharacterType>();
					for (CharacterType characterType = CharacterType.None; characterType < CharacterType.Last; characterType++)
					{
						list.Add(characterType);
					}
					list.Shuffle(rnd);
					list.Remove(CharacterType.None);
					list.Remove(CharacterType.PunchingDummy);
					using (List<CharacterType>.Enumerator enumerator = value.FriendlyBans.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CharacterType item = enumerator.Current;
							list.Remove(item);
						}
					}
					using (List<CharacterType>.Enumerator enumerator2 = value.EnemyBans.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CharacterType item2 = enumerator2.Current;
							list.Remove(item2);
						}
					}
					foreach (CharacterType item3 in value.EnemyTeamSelections.Values)
					{
						list.Remove(item3);
					}
					foreach (CharacterType item4 in value.FriendlyTeamSelections.Values)
					{
						list.Remove(item4);
					}
				}
			}
		}
	}

	internal void WriteErrorToConsole(LocalizationPayload localizedFailure, string unlocalizedFailure)
	{
		string text;
		if (localizedFailure != null)
		{
			text = localizedFailure.ToString();
		}
		else if (!unlocalizedFailure.IsNullOrEmpty())
		{
			text = string.Format("{0}#NeedsLocalization", unlocalizedFailure);
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
	}

	public void SendRankedTradeRequest(CharacterType desiredCharacter, RankedTradeData.TradeActionType tradeAction)
	{
		RankedTradeRequest rankedTradeRequest = new RankedTradeRequest();
		rankedTradeRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		rankedTradeRequest.Trade.DesiredCharacter = desiredCharacter;
		rankedTradeRequest.Trade.TradeAction = tradeAction;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage<RankedTradeResponse>(rankedTradeRequest, delegate(RankedTradeResponse response)
		{
			if (!response.Success)
			{
				this.WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedBanRequest(CharacterType selection)
	{
		RankedBanRequest rankedBanRequest = new RankedBanRequest();
		rankedBanRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		rankedBanRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage<RankedBanResponse>(rankedBanRequest, delegate(RankedBanResponse response)
		{
			if (!response.Success)
			{
				this.WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedSelectionRequest(CharacterType selection)
	{
		RankedSelectionRequest rankedSelectionRequest = new RankedSelectionRequest();
		rankedSelectionRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		rankedSelectionRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage<RankedSelectionResponse>(rankedSelectionRequest, delegate(RankedSelectionResponse response)
		{
			if (!response.Success)
			{
				this.WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedHoverClickRequest(CharacterType selection)
	{
		RankedHoverClickRequest rankedHoverClickRequest = new RankedHoverClickRequest();
		rankedHoverClickRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		rankedHoverClickRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage<RankedHoverClickResponse>(rankedHoverClickRequest, delegate(RankedHoverClickResponse response)
		{
			if (!response.Success)
			{
				this.WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	private void HandleFreelancerUnavailableNotification(FreelancerUnavailableNotification notification)
	{
		CharacterResourceLink characterResourceLink = (notification.oldCharacterType == CharacterType.None) ? null : GameWideData.Get().GetCharacterResourceLink(notification.oldCharacterType);
		string text;
		if (characterResourceLink != null)
		{
			text = characterResourceLink.GetDisplayName();
		}
		else
		{
			text = StringUtil.TR("YourPreferredFreelancer", "Global");
		}
		string arg = text;
		string title;
		string description;
		if (notification.ItsTooLateToChange)
		{
			title = StringUtil.TR("AutomaticSelection", "Global");
			description = LocalizationPayload.Create("ForcedToPlayFreelancer", "Global", new LocalizationArg[]
			{
				LocalizationArg_Freelancer.Create(notification.oldCharacterType),
				LocalizationArg_Freelancer.Create(notification.newCharacterType)
			}).ToString();
		}
		else
		{
			title = StringUtil.TR("SelectNewFreelancer", "Global");
			string text2;
			if (notification.thiefName.IsNullOrEmpty())
			{
				text2 = string.Format(StringUtil.TR("AlreadyClaimedChooseNewFreelancer", "Global"), arg);
			}
			else
			{
				text2 = string.Format(StringUtil.TR("AlreadyClaimedByTeammateChooseNewFreelancer", "Global"), arg, notification.thiefName);
			}
			description = text2;
			UICharacterSelectScreen.Get().AllowCharacterSwapForConflict();
		}
		if (!notification.oldCharacterType.IsWillFill())
		{
			UIDialogPopupManager.OpenOneButtonDialog(title, description, StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	private void HandleConnectionOpenedNotification(ConnectionOpenedNotification notification)
	{
		this.RegisterGameClient();
	}

	private void HandleConnectionClosedNotification(ConnectionClosedNotification notification)
	{
		if (this.m_registered)
		{
			base.Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", new object[]
			{
				this.m_serverAddress,
				notification.Message.Trim(),
				notification.Code
			});
			this.OnDisconnected(this.m_lastLobbyErrorMessage, this.m_allowRelogin, notification.Code);
			this.m_lastLobbyErrorMessage = null;
			this.m_allowRelogin = true;
		}
		else if (this.m_overallConnectionTimer.IsRunning)
		{
			if (this.m_overallConnectionTimer.Elapsed.TotalSeconds < (double)base.ConnectionTimeout)
			{
				base.Logger.Info("Retrying connection to {0}: {1} CloseStatusCode={2}", new object[]
				{
					this.m_serverAddress,
					notification.Message.Trim(),
					notification.Code
				});
				base.Reconnect();
			}
			else
			{
				base.Logger.Info("Failed to connect to {0}: {1} CloseStatusCode={2}", new object[]
				{
					this.m_serverAddress,
					notification.Message.Trim(),
					notification.Code
				});
				this.m_overallConnectionTimer.Reset();
				RegisterGameClientResponse registerGameClientResponse = new RegisterGameClientResponse();
				registerGameClientResponse.Success = false;
				registerGameClientResponse.LocalizedFailure = LocalizationPayload.Create("NetworkError", "Global");
				this.OnConnected(registerGameClientResponse);
			}
		}
	}

	private void HandleConnectionErrorNotification(ConnectionErrorNotification notification)
	{
		Log.Error("Communication error to lobby server: {0}", new object[]
		{
			notification.ErrorMessage
		});
	}

	protected override void HandleMessage(WebSocketMessage message)
	{
		base.HandleMessage(message);
		this.m_messageDispatcher.HandleMessage(this, message);
	}

	public bool SendRequestMessage<ResponseType>(WebSocketMessage request, Action<ResponseType> callback) where ResponseType : WebSocketResponseMessage, new()
	{
		return base.SendRequestMessage<ResponseType, LobbyGameClientInterface>(request, callback, this.m_messageDispatcher);
	}

	public void UnregisterRequest<ResponseType>(int requestId) where ResponseType : WebSocketResponseMessage, new()
	{
		this.m_messageDispatcher.UnregisterMessageDelegate<ResponseType>(requestId);
	}

	private void RegisterGameClient()
	{
		RegisterGameClientRequest registerGameClientRequest = new RegisterGameClientRequest();
		registerGameClientRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		registerGameClientRequest.SessionInfo = this.m_sessionInfo;
		registerGameClientRequest.AuthInfo = this.m_ticket.AuthInfo;
		if (SteamManager.UsingSteam)
		{
			registerGameClientRequest.SteamUserId = SteamUser.GetSteamID().ToString();
		}
		else
		{
			registerGameClientRequest.SteamUserId = "0";
		}
		registerGameClientRequest.SystemInfo = new LobbyGameClientSystemInfo();
		registerGameClientRequest.SystemInfo.GraphicsDeviceName = SystemInfo.graphicsDeviceName;
		this.SendRequestMessage<RegisterGameClientResponse>(registerGameClientRequest, new Action<RegisterGameClientResponse>(this.HandleRegisterGameClientResponse));
	}

	private void HandleRegisterGameClientResponse(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			base.Logger.Error("Failed to register game client with lobby server: {0}", new object[]
			{
				response.ErrorMessage
			});
			this.m_registered = false;
			this.OnConnected(response);
			this.Disconnect();
		}
		else
		{
			base.Logger.Info("Registered game client with lobby server", new object[0]);
			this.m_registered = true;
			this.m_overallConnectionTimer.Reset();
			if (response.SessionInfo != null)
			{
				this.m_sessionInfo = response.SessionInfo;
			}
			else if (response.AuthInfo != null)
			{
				this.m_sessionInfo.AccountId = response.AuthInfo.AccountId;
			}
			if (response.Status != null)
			{
				this.HandleLobbyStatusNotification(response.Status);
			}
			if (response.AuthInfo != null)
			{
				if (!string.IsNullOrEmpty(response.AuthInfo.Handle))
				{
					this.m_ticket.AuthInfo = response.AuthInfo;
				}
			}
			this.OnConnected(response);
		}
	}

	private void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		this.OnLobbyServerReadyNotification(notification);
		if (notification.Success)
		{
			base.Logger.Info("Lobby server is ready", new object[0]);
			if (notification.FriendStatus != null)
			{
				this.HandleFriendStatusNotification(notification.FriendStatus);
			}
			if (notification.ServerQueueConfiguration != null)
			{
				this.HandleServerQueueConfigurationUpdateNotification(notification.ServerQueueConfiguration);
			}
		}
		else
		{
			base.Logger.Error("Lobby server failed to become ready: {0}", new object[]
			{
				notification.ErrorMessage
			});
		}
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.LocalizedFailure != null)
		{
			base.Logger.Error("Error from lobby server: {0}", new object[]
			{
				notification.LocalizedFailure.ToString()
			});
			this.m_lastLobbyErrorMessage = notification.LocalizedFailure.ToString();
			this.m_allowRelogin = notification.AllowRelogin;
			this.Disconnect();
			return;
		}
		this.OnLobbyStatusNotification(notification);
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		this.OnLobbyGameplayOverridesNotification(notification);
	}

	public void SubscribeToCustomGames()
	{
		base.SendMessage(new SubscribeToCustomGamesRequest());
	}

	public void UnsubscribeFromCustomGames()
	{
		base.SendMessage(new UnsubscribeFromCustomGamesRequest());
	}

	private void HandleLobbyCustomGamesNotification(LobbyCustomGamesNotification notification)
	{
		this.OnLobbyCustomGamesNotification(notification);
	}

	public void SetGameTypeSubMasks(GameType gameType, ushort subGameMask, Action<SetGameSubTypeResponse> onResponseCallback)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - SetGameTypeSubMasks \r\nCall Stack: {0}", new object[]
			{
				"n/a"
			});
			return;
		}
		this.SendRequestMessage<SetGameSubTypeResponse>(new SetGameSubTypeRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			gameType = gameType,
			SubTypeMask = subGameMask
		}, onResponseCallback);
	}

	public void JoinQueue(GameType gameType, BotDifficulty allyBotDifficulty, BotDifficulty enemyBotDifficulty, Action<JoinMatchmakingQueueResponse> callback)
	{
		this.SendRequestMessage<JoinMatchmakingQueueResponse>(new JoinMatchmakingQueueRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameType = gameType,
			AllyBotDifficulty = allyBotDifficulty,
			EnemyBotDifficulty = enemyBotDifficulty
		}, callback);
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		this.SendRequestMessage<LeaveMatchmakingQueueResponse>(new LeaveMatchmakingQueueRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void UpdateQueueEnemyBotDifficulty(LobbyMatchmakingQueueInfo queueInfo, BotDifficulty enemyDifficulty)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateQueueEnemyBotDifficulty", new object[0]);
			return;
		}
		base.SendMessage(new UpdateMatchmakingQueueRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			EnemyDifficulty = enemyDifficulty
		});
	}

	public void CreateGame(LobbyGameConfig gameConfig, ReadyState readyState, string processCode, Action<CreateGameResponse> onResponseCallback, BotDifficulty botSkillTeamA = BotDifficulty.Easy, BotDifficulty botSkillTeamB = BotDifficulty.Easy)
	{
		this.SendRequestMessage<CreateGameResponse>(new CreateGameRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameConfig = gameConfig,
			ReadyState = readyState,
			ProcessCode = processCode,
			SelectedBotSkillTeamA = botSkillTeamA,
			SelectedBotSkillTeamB = botSkillTeamB
		}, onResponseCallback);
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		this.SendRequestMessage<JoinGameResponse>(new JoinGameRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameServerProcessCode = gameInfo.GameServerProcessCode,
			AsSpectator = asSpectator
		}, onResponseCallback);
	}

	public void LeaveGame(bool isPermanent, GameResult gameResult, Action<LeaveGameResponse> onResponseCallback)
	{
		this.SendRequestMessage<LeaveGameResponse>(new LeaveGameRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			IsPermanent = isPermanent,
			GameResult = gameResult
		}, onResponseCallback);
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, PersistedStats stats, MatchFreelancerStats matchFreelancerStats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		this.SendRequestMessage<CalculateFreelancerStatsResponse>(new CalculateFreelancerStatsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			PersistedStatBucket = bucketType,
			CharacterType = characterType,
			PersistedStats = stats,
			MatchFreelancerStats = matchFreelancerStats
		}, onResponseCallback);
	}

	private void HandleQueueAssignmentNotification(MatchmakingQueueAssignmentNotification notification)
	{
		this.OnQueueAssignmentNotification(notification);
	}

	private void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
		this.OnQueueStatusNotification(notification);
	}

	private void HandleGameAssignmentNotification(GameAssignmentNotification notification)
	{
		this.OnGameAssignmentNotification(notification);
	}

	public void Replay_RemoveFromGame()
	{
		this.HandleGameAssignmentNotification(new GameAssignmentNotification());
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		this.OnGameInfoNotification(notification);
	}

	private void HandleGameStatusNotification(GameStatusNotification notification)
	{
		this.OnGameStatusNotification(notification);
	}

	public void HandleSynchronizeWithClientOutOfGameRequest(SynchronizeWithClientOutOfGameRequest request)
	{
		Log.Info("The servers are checking with us to confirm we're out of game {0}", new object[]
		{
			request.GameServerProcessCode
		});
		base.SendMessage(new SynchronizeWithClientOutOfGameResponse
		{
			GameServerProcessCode = request.GameServerProcessCode
		});
	}

	public int UpdatePlayerInfo(LobbyPlayerInfoUpdate playerInfoUpdate, Action<PlayerInfoUpdateResponse> onResponseCallback = null)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdatePlayerInfo \r\nCall stack: {0}", new object[]
			{
				"n/a"
			});
			return 0;
		}
		PlayerInfoUpdateRequest playerInfoUpdateRequest = new PlayerInfoUpdateRequest();
		playerInfoUpdateRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		playerInfoUpdateRequest.PlayerInfoUpdate = playerInfoUpdate;
		playerInfoUpdateRequest.GameType = new GameType?(ClientGameManager.Get().GroupInfo.SelectedQueueType);
		this.SendRequestMessage<PlayerInfoUpdateResponse>(playerInfoUpdateRequest, onResponseCallback);
		return playerInfoUpdateRequest.RequestId;
	}

	public void UpdateGameCheats(GameOptionFlag gameOptionFlags, PlayerGameOptionFlag playerGameOptionFlags, Action<GameCheatUpdateResponse> onResponseCallback = null)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameCheats", new object[0]);
			return;
		}
		this.SendRequestMessage<GameCheatUpdateResponse>(new GameCheatUpdateRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameOptionFlags = gameOptionFlags,
			PlayerGameOptionFlags = playerGameOptionFlags
		}, onResponseCallback);
	}

	public void UpdateGroupGameType(GameType gameType, Action<PlayerGroupInfoUpdateResponse> onResponseCallback)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGroupGameType", new object[0]);
			return;
		}
		this.SendRequestMessage<PlayerGroupInfoUpdateResponse>(new PlayerGroupInfoUpdateRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameType = gameType
		}, onResponseCallback);
	}

	public void UpdateGameInfo(LobbyGameInfo gameInfo, LobbyTeamInfo teamInfo)
	{
		if (LobbyGameClientInterface.BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameInfo", new object[0]);
			return;
		}
		GameInfoUpdateRequest gameInfoUpdateRequest = new GameInfoUpdateRequest();
		gameInfoUpdateRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		gameInfoUpdateRequest.GameInfo = gameInfo;
		gameInfoUpdateRequest.TeamInfo = teamInfo;
		Action<GameInfoUpdateResponse> callback = delegate(GameInfoUpdateResponse response)
		{
			if (!response.Success)
			{
				this.WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		};
		this.SendRequestMessage<GameInfoUpdateResponse>(gameInfoUpdateRequest, callback);
	}

	public void InvitePlayerToGame(string playerHandle, Action<GameInvitationResponse> onResponseCallback)
	{
		this.SendRequestMessage<GameInvitationResponse>(new GameInvitationRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			InviteeHandle = playerHandle
		}, onResponseCallback);
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		this.SendRequestMessage<GameSpectatorResponse>(new GameSpectatorRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			InviteeHandle = playerHandle
		}, onResponseCallback);
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<CrashReportArchiveNameResponse>(new CrashReportArchiveNameRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			NumArchiveBytes = numArchiveBytes
		}, onResponseCallback);
		return true;
	}

	public bool SendStatusReport(ClientStatusReport report)
	{
		report.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(report);
	}

	public bool SendErrorReport(ClientErrorReport report)
	{
		report.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(report);
	}

	public bool SendErrorSummary(ClientErrorSummary summary)
	{
		summary.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(summary);
	}

	public bool SendFeedbackReport(ClientFeedbackReport report)
	{
		report.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(report);
	}

	public bool SendPerformanceReport(ClientPerformanceReport report)
	{
		report.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(report);
	}

	public bool SendSetRegionRequest(Region region)
	{
		return base.SendMessage(new SetRegionRequest
		{
			Region = region
		});
	}

	private void HandleAccountDataUpdateNotification(PlayerAccountDataUpdateNotification notification)
	{
		this.OnAccountDataUpdated(notification);
	}

	private void HandleRequeueForceCharacterNotification(ForcedCharacterChangeFromServerNotification notification)
	{
		this.OnForcedCharacterChangeFromServerNotification(notification);
	}

	private void HandleCharacterDataUpdateNotification(PlayerCharacterDataUpdateNotification notification)
	{
		this.OnCharacterDataUpdateNotification(notification);
	}

	private void HandleInventoryComponentUpdateNotification(InventoryComponentUpdateNotification notification)
	{
		this.OnInventoryComponentUpdateNotification(notification);
	}

	private void HandleRankedOverviewChangeNotification(RankedOverviewChangeNotification notification)
	{
		this.OnRankedOverviewChangeNotification(notification);
	}

	public void GetPlayerMatchData(Action<PlayerMatchDataResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PlayerMatchDataResponse>(new PlayerMatchDataRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void PurchaseLoadoutSlot(CharacterType characterType, Action<PurchaseLoadoutSlotResponse> onResponseCallback)
	{
		this.SendRequestMessage<PurchaseLoadoutSlotResponse>(new PurchaseLoadoutSlotRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Character = characterType
		}, onResponseCallback);
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID, Action<PurchaseModResponse> onResponseCallback)
	{
		PurchaseModRequest purchaseModRequest = new PurchaseModRequest();
		purchaseModRequest.RequestId = this.m_messageDispatcher.GetRequestId();
		purchaseModRequest.Character = character;
		purchaseModRequest.UnlockData.AbilityId = abilityId;
		purchaseModRequest.UnlockData.AbilityModID = abilityModID;
		this.SendRequestMessage<PurchaseModResponse>(purchaseModRequest, onResponseCallback);
	}

	public void PurchaseModToken(int numToPurchase, Action<PurchaseModTokenResponse> onResponseCallback)
	{
		this.SendRequestMessage<PurchaseModTokenResponse>(new PurchaseModTokenRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			NumToPurchase = numToPurchase
		}, onResponseCallback);
	}

	public void RequestBalancedTeam(BalancedTeamRequest request, Action<BalancedTeamResponse> onResponseCallback)
	{
		request.RequestId = this.m_messageDispatcher.GetRequestId();
		this.SendRequestMessage<BalancedTeamResponse>(request, onResponseCallback);
	}

	public void RequestRefreshBankData(Action<RefreshBankDataResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<RefreshBankDataResponse>(new RefreshBankDataRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	private void HandleBankBalanceChangeNotification(BankBalanceChangeNotification notification)
	{
		this.OnBankBalanceChangeNotification(notification);
	}

	private void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		this.OnSeasonStatusNotification(notification);
	}

	public bool SendSeasonStatusConfirmed(SeasonStatusConfirmed.DialogType dialogType)
	{
		return base.SendMessage(new SeasonStatusConfirmed
		{
			Dialog = dialogType
		});
	}

	private void HandleChapterStatusNotification(ChapterStatusNotification notification)
	{
		this.OnChapterStatusNotification(notification);
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupInviteResponse>(new GroupInviteRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle
		}, onResponseCallback);
	}

	public void RequestToJoinGroup(string friendHandle, Action<GroupJoinResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupJoinResponse>(new GroupJoinRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle
		}, onResponseCallback);
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupPromoteResponse>(new GroupPromoteRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Name = name
		}, onResponseCallback);
	}

	public void ChatToGroup(string text, Action<GroupChatResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupChatResponse>(new GroupChatRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Text = text,
			RequestedEmojis = ChatEmojiManager.Get().GetAllEmojisInString(text)
		}, onResponseCallback);
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupLeaveResponse>(new GroupLeaveRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void KickFromGroup(string memberName, Action<GroupKickResponse> onResponseCallback)
	{
		this.SendRequestMessage<GroupKickResponse>(new GroupKickRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			MemberName = memberName
		}, onResponseCallback);
	}

	public void UpdateFriend(string friendHandle, long friendAccountId, FriendOperation operation, string strData, Action<FriendUpdateResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<FriendUpdateResponse>(new FriendUpdateRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle,
			FriendAccountId = friendAccountId,
			FriendOperation = operation,
			StringData = strData
		}, onResponseCallback);
	}

	public void UpdatePlayerStatus(string statusString, Action<PlayerUpdateStatusResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PlayerUpdateStatusResponse>(new PlayerUpdateStatusRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			AccountId = this.SessionInfo.AccountId,
			StatusString = statusString
		}, onResponseCallback);
	}

	public void NotifyStoreOpened()
	{
		StoreOpenedMessage message = new StoreOpenedMessage();
		base.SendMessage(message);
	}

	public void NotifyCustomKeyBinds(Dictionary<int, KeyCodeData> CustomKeyBinds)
	{
		base.SendMessage(new CustomKeyBindNotification
		{
			CustomKeyBinds = CustomKeyBinds
		});
	}

	public bool NotifyOptions(OptionsNotification notification)
	{
		notification.RequestId = this.m_messageDispatcher.GetRequestId();
		return base.SendMessage(notification);
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		this.SendRequestMessage<PaymentMethodsResponse>(new PaymentMethodsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void RequestPrices(Action<PricesResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PricesResponse>(new PricesRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void SendSteamMtxConfirm(bool authorized, ulong orderId)
	{
		base.SendMessage(new SteamMtxConfirm
		{
			authorized = authorized,
			orderId = orderId
		});
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0L)
		{
			return;
		}
		this.SendRequestMessage<PurchaseLootMatrixPackResponse>(new PurchaseLootMatrixPackRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			LootMatrixPackIndex = lootMatrixPackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseGame(int gamePackIndex, long paymentMethodId, Action<PurchaseGameResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0L)
		{
			return;
		}
		this.SendRequestMessage<PurchaseGameResponse>(new PurchaseGameRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GamePackIndex = gamePackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseGGPack(int ggPackIndex, long paymentMethodId, Action<PurchaseGGPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0L)
		{
			return;
		}
		this.SendRequestMessage<PurchaseGGPackResponse>(new PurchaseGGPackRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GGPackIndex = ggPackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseCharacter(CurrencyType currencyType, CharacterType characterType, Action<PurchaseCharacterResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseCharacterResponse>(new PurchaseCharacterRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType
		}, onResponseCallback);
	}

	public void PurchaseCharacterForCash(CharacterType characterType, long paymentMethodId, Action<PurchaseCharacterForCashResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0L)
		{
			return;
		}
		this.SendRequestMessage<PurchaseCharacterForCashResponse>(new PurchaseCharacterForCashRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CharacterType = characterType,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseSkin(CurrencyType currencyType, CharacterType characterType, int skinId, Action<PurchaseSkinResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseSkinResponse>(new PurchaseSkinRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId
		}, onResponseCallback);
	}

	public void PurchaseTexture(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, Action<PurchaseTextureResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseTextureResponse>(new PurchaseTextureRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId
		}, onResponseCallback);
	}

	public void PurchaseTint(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, int tintId, Action<PurchaseTintResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseTintResponse>(new PurchaseTintRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId,
			TintId = tintId
		}, onResponseCallback);
	}

	public void PurchaseTintForCash(CharacterType characterType, int skinId, int textureId, int tintId, long paymentMethodId, Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseTintForCashResponse>(new PurchaseTintForCashRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			PaymentMethodId = paymentMethodId,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId,
			TintId = tintId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseStoreItemForCash(int inventoryItemId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseStoreItemForCashResponse>(new PurchaseStoreItemForCashRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			PaymentMethodId = paymentMethodId,
			InventoryTemplateId = inventoryItemId,
			AccountCurrency = this.m_ticket.AccountCurrency
		}, onResponseCallback);
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntId, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseTauntResponse>(new PurchaseTauntRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			TauntId = tauntId
		}, onResponseCallback);
	}

	public void PurchaseTitle(CurrencyType currencyType, int titleId, Action<PurchaseTitleResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseTitleResponse>(new PurchaseTitleRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			TitleId = titleId
		}, onResponseCallback);
	}

	public void PurchaseChatEmoji(CurrencyType currencyType, int emojiID, Action<PurchaseChatEmojiResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseChatEmojiResponse>(new PurchaseChatEmojiRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			EmojiID = emojiID
		}, onResponseCallback);
	}

	public void PurchaseBannerBackground(CurrencyType currencyType, int bannerBackgroundId, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseBannerBackgroundResponse>(new PurchaseBannerBackgroundRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			BannerBackgroundId = bannerBackgroundId
		}, onResponseCallback);
	}

	public void PurchaseBannerForeground(CurrencyType currencyType, int bannerForegroundId, Action<PurchaseBannerForegroundResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseBannerForegroundResponse>(new PurchaseBannerForegroundRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			BannerForegroundId = bannerForegroundId
		}, onResponseCallback);
	}

	public void PurchaseAbilityVfx(CharacterType charType, int abilityId, int vfxId, CurrencyType currencyType, Action<PurchaseAbilityVfxResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseAbilityVfxResponse>(new PurchaseAbilityVfxRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = charType,
			AbilityId = abilityId,
			VfxId = vfxId
		}, onResponseCallback);
	}

	public void PurchaseOvercon(int overconId, CurrencyType currencyType, Action<PurchaseOverconResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseOverconResponse>(new PurchaseOverconRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			OverconId = overconId
		}, onResponseCallback);
	}

	public void PurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType, Action<PurchaseLoadingScreenBackgroundResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseLoadingScreenBackgroundResponse>(new PurchaseLoadingScreenBackgroundRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			LoadingScreenBackgroundId = loadingScreenBackgroundId
		}, onResponseCallback);
	}

	public void PlayerPanelUpdated(int _SelectedTitleID, int _SelectedForegroundBannerID, int _SelectedBackgroundBannerID, int _SelectedRibbonID)
	{
		base.SendMessage(new PlayerPanelUpdatedNotification
		{
			originalSelectedTitleID = _SelectedTitleID,
			originalSelectedForegroundBannerID = _SelectedForegroundBannerID,
			originalSelectedBackgroundBannerID = _SelectedBackgroundBannerID,
			originalSelectedRibbonID = _SelectedRibbonID
		});
	}

	public void PurchaseInventoryItem(int inventoryItemID, CurrencyType currencyType, Action<PurchaseInventoryItemResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PurchaseInventoryItemResponse>(new PurchaseInventoryItemRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			InventoryItemID = inventoryItemID
		}, onResponseCallback);
	}

	public void CheckAccountStatus(Action<CheckAccountStatusResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<CheckAccountStatusResponse>(new CheckAccountStatusRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void CheckRAFStatus(bool getReferralCode, Action<CheckRAFStatusResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<CheckRAFStatusResponse>(new CheckRAFStatusRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GetReferralCode = getReferralCode
		}, onResponseCallback);
	}

	public void SendRAFReferralEmails(List<string> emails, Action<SendRAFReferralEmailsResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SendRAFReferralEmailsResponse>(new SendRAFReferralEmailsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Emails = emails
		}, onResponseCallback);
	}

	public void SelectDailyQuest(int questId, Action<PickDailyQuestResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PickDailyQuestResponse>(new PickDailyQuestRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			questId = questId
		}, onResponseCallback);
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<AbandonDailyQuestResponse>(new AbandonDailyQuestRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			questId = questId
		}, onResponseCallback);
	}

	public void ActivateQuestTrigger(QuestTriggerType triggerType, int activationCount, int questId, int questBonusCount, int itemTemplateId, CharacterType charType, Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<ActivateQuestTriggerResponse>(new ActivateQuestTriggerRequest
		{
			TriggerType = triggerType,
			ActivationCount = activationCount,
			QuestId = questId,
			QuestBonusCount = questBonusCount,
			ItemTemplateId = itemTemplateId,
			charType = charType,
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<BeginQuestResponse>(new BeginQuestRequest
		{
			QuestId = questId,
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void CompleteQuest(int questId, Action<CompleteQuestResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<CompleteQuestResponse>(new CompleteQuestRequest
		{
			QuestId = questId,
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void MarkTutorialSkipped(TutorialVersion progress, Action<MarkTutorialSkippedResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<MarkTutorialSkippedResponse>(new MarkTutorialSkippedRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Progress = progress
		}, onResponseCallback);
	}

	public void GetInventoryItems(Action<GetInventoryItemsResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<GetInventoryItemsResponse>(new GetInventoryItemsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void AddInventoryItems(List<InventoryItem> items, Action<AddInventoryItemsResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<AddInventoryItemsResponse>(new AddInventoryItemsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Items = items
		}, onResponseCallback);
	}

	public void RemoveInventoryItems(List<InventoryItem> items, Action<RemoveInventoryItemsResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<RemoveInventoryItemsResponse>(new RemoveInventoryItemsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Items = items
		}, onResponseCallback);
	}

	public void ConsumeInventoryItem(int itemId, int itemCount, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<ConsumeInventoryItemResponse>(new ConsumeInventoryItemRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			ItemId = itemId,
			ItemCount = itemCount,
			ToISO = toISO
		}, onResponseCallback);
	}

	public void ConsumeInventoryItems(List<int> itemIds, bool toISO, Action<ConsumeInventoryItemsResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<ConsumeInventoryItemsResponse>(new ConsumeInventoryItemsRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			ItemIds = itemIds,
			ToISO = toISO
		}, onResponseCallback);
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SeasonQuestActionResponse>(new SeasonQuestActionRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Action = SeasonQuestActionRequest.ActionType.symbol_001D,
			SeasonId = seasonId,
			ChapterId = chapterId
		}, onResponseCallback);
	}

	public void SetSeasonQuest(int seasonId, int chapterId, int slotNum, int questId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SeasonQuestActionResponse>(new SeasonQuestActionRequest
		{
			Action = SeasonQuestActionRequest.ActionType.symbol_000E,
			RequestId = this.m_messageDispatcher.GetRequestId(),
			SeasonId = seasonId,
			ChapterId = chapterId,
			SlotNum = slotNum,
			QuestId = questId
		}, onResponseCallback);
	}

	public bool SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		return base.SendMessage(new PlayerCharacterFeedbackRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			FeedbackData = feedbackData
		});
	}

	public void SendRejoinGameRequest(LobbyGameInfo previousGameInfo, bool accept, Action<RejoinGameResponse> onResponseCallback)
	{
		this.SendRequestMessage<RejoinGameResponse>(new RejoinGameRequest
		{
			PreviousGameInfo = previousGameInfo,
			Accept = accept
		}, onResponseCallback);
	}

	public void SendDiscordGetRpcTokenRequest(Action<DiscordGetRpcTokenResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<DiscordGetRpcTokenResponse>(new DiscordGetRpcTokenRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void SendDiscordGetAccessTokenRequest(string rpcCode, Action<DiscordGetAccessTokenResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<DiscordGetAccessTokenResponse>(new DiscordGetAccessTokenRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			DiscordRpcCode = rpcCode
		}, onResponseCallback);
	}

	public void SendDiscordJoinServerRequest(ulong discordUserId, string discordUserAccessToken, DiscordJoinType joinType, Action<DiscordJoinServerResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<DiscordJoinServerResponse>(new DiscordJoinServerRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			DiscordUserId = discordUserId,
			DiscordUserAccessToken = discordUserAccessToken,
			JoinType = joinType
		}, onResponseCallback);
	}

	public void SendDiscordLeaveServerRequest(DiscordJoinType joinType, Action<DiscordLeaveServerResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<DiscordLeaveServerResponse>(new DiscordLeaveServerRequest
		{
			JoinType = joinType,
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void SendFacebookGetUserTokenRequest(Action<FacebookGetUserTokenResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<FacebookGetUserTokenResponse>(new FacebookGetUserTokenRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void SendPreviousGameInfoRequest(Action<PreviousGameInfoResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<PreviousGameInfoResponse>(new PreviousGameInfoRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		ChatNotification chatNotification = new ChatNotification();
		chatNotification.RecipientHandle = recipientHandle;
		chatNotification.ConsoleMessageType = messageType;
		chatNotification.Text = text;
		chatNotification.EmojisAllowed = ChatEmojiManager.Get().GetAllEmojisInString(chatNotification.Text);
		return base.SendMessage(chatNotification);
	}

	public void SendSetDevTagRequest(bool active, Action<SetDevTagResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SetDevTagResponse>(new SetDevTagRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			active = active
		}, onResponseCallback);
	}

	public void SendUseOverconRequest(int overconId, string overconName, int actorId, int turn)
	{
		this.SendRequestMessage<UseOverconResponse>(new UseOverconRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			OverconId = overconId,
			OverconName = overconName,
			ActorId = actorId,
			Turn = turn
		}, new Action<UseOverconResponse>(this.HandleUseOverconResponse));
	}

	public bool SendUIActionNotification(string context)
	{
		return base.SendMessage(new UIActionNotification
		{
			Context = context
		});
	}

	private void HandleGroupUpdateNotification(GroupUpdateNotification notification)
	{
		try
		{
			LobbyGameClientInterface.BlockSendingGroupUpdates = true;
			this.OnGroupUpdateNotification(notification);
		}
		finally
		{
			LobbyGameClientInterface.BlockSendingGroupUpdates = false;
		}
	}

	private void HandleGGPackUsedNotification(UseGGPackNotification notification)
	{
		this.OnUseGGPackNotification(notification);
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		this.OnChatNotification(notification);
	}

	private void HandleUseOverconResponse(UseOverconResponse notification)
	{
		this.OnUseOverconNotification(notification);
	}

	private void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		this.OnFriendStatusNotification(notification);
	}

	private void HandleGroupConfirmationRequest(GroupConfirmationRequest request)
	{
		this.OnGroupConfirmation(request);
	}

	private void HandleGroupSuggestionRequest(GroupSuggestionRequest request)
	{
		this.OnGroupSuggestion(request);
	}

	private void HandleGameInviteConfirmationRequest(GameInviteConfirmationRequest request)
	{
		this.OnGameInviteConfirmationRequest(request);
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification request)
	{
		this.OnQuestCompleteNotification(request);
	}

	private void HandleMatchResultsNotification(MatchResultsNotification request)
	{
		this.OnMatchResultsNotification(request);
	}

	private void HandleFactionCompetitionNotification(FactionCompetitionNotification request)
	{
		this.OnFactionCompetitionNotification(request);
	}

	private void HandleTrustBoostUsedNotification(TrustBoostUsedNotification request)
	{
		this.OnTrustBoostUsedNotification(request);
	}

	private void HandleFacebookAccessTokenNotification(FacebookAccessTokenNotification request)
	{
		this.OnFacebookAccessTokenNotification(request);
	}

	private void HandleFactionLoginRewardNotification(FactionLoginRewardNotification notification)
	{
		this.OnFactionLoginRewardNotification(notification);
	}

	private void HandlePlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		this.OnPlayerFactionContributionChange(notification);
	}

	private void HandleLobbyAlertMissionDataNotification(LobbyAlertMissionDataNotification notification)
	{
		this.OnLobbyAlertMissionDataNotification(notification);
	}

	private void HandleLobbySeasonQuestDataNotification(LobbySeasonQuestDataNotification notification)
	{
		this.OnLobbySeasonQuestDataNotification(notification);
	}

	private void HandleErrorReportSummaryRequest(ErrorReportSummaryRequest request)
	{
		ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
		if (clientExceptionDetector != null)
		{
			ClientErrorReport clientErrorReport;
			if (clientExceptionDetector.GetClientErrorReport(request.CrashReportHash, out clientErrorReport))
			{
				Log.Info("Informing lobby about error report {0}: {1}", new object[]
				{
					request.CrashReportHash,
					clientErrorReport.LogString
				});
				base.SendMessage(new ErrorReportSummaryResponse
				{
					ClientErrorReport = clientErrorReport
				});
			}
			else
			{
				Log.Warning("Lobby asked us to describe error {0}, but we've never seen it!", new object[]
				{
					request.CrashReportHash
				});
			}
		}
	}

	private void HandlePendingPurchaseResult(PendingPurchaseResult resultMsg)
	{
		if (UIStorePanel.Get() != null)
		{
			UIStorePanel.Get().HandlePendingPurchaseResult(resultMsg);
		}
	}

	private void symbol_001D(DEBUG_ResetCompletedChaptersResponse symbol_001D)
	{
		string text = string.Empty;
		if (symbol_001D.Success)
		{
			text = "Cleared completed chapters list";
			Log.Info(text, new object[0]);
		}
		else
		{
			text = "Unable to reset completed chapters list";
			Log.Error(text, new object[0]);
		}
		TextConsole.Get().Write(text, ConsoleMessageType.SystemMessage);
	}

	public void RequestToUseGGPack(Action<UseGGPackResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<UseGGPackResponse>(new UseGGPackRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, onResponseCallback);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<UpdateRemoteCharacterResponse>(new UpdateRemoteCharacterRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			Characters = characters,
			RemoteSlotIndexes = remoteSlotIndexes
		}, onResponseCallback);
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SelectTitleResponse>(new SelectTitleRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			TitleID = newTitleID
		}, onResponseCallback);
	}

	public void RequestBannerSelect(int newBannerID, Action<SelectBannerResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SelectBannerResponse>(new SelectBannerRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			BannerID = newBannerID
		}, onResponseCallback);
	}

	public void RequestLoadingScreenBackgroundToggle(int loadingScreenId, bool newState, Action<LoadingScreenToggleResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<LoadingScreenToggleResponse>(new LoadingScreenToggleRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			LoadingScreenId = loadingScreenId,
			NewState = newState
		}, onResponseCallback);
	}

	public void RequestRibbonSelect(int newRibbonID, Action<SelectRibbonResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<SelectRibbonResponse>(new SelectRibbonRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			RibbonID = newRibbonID
		}, onResponseCallback);
	}

	public void RequestUpdateUIState(AccountComponent.UIStateIdentifier uiState, int stateValue, Action<UpdateUIStateResponse> onResponseCallback = null)
	{
		this.SendRequestMessage<UpdateUIStateResponse>(new UpdateUIStateRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			UIState = uiState,
			StateValue = stateValue
		}, onResponseCallback);
	}

	public void SetPushToTalkKey(int keyType, int keyCode, string keyName)
	{
		this.SendRequestMessage<UpdatePushToTalkKeyResponse>(new UpdatePushToTalkKeyRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			KeyType = keyType,
			KeyCode = keyCode,
			KeyName = keyName
		}, null);
	}

	public void SendRankedLeaderboardOverviewRequest(GameType gameType, Action<RankedLeaderboardOverviewResponse> onResponseCallback)
	{
		this.SendRequestMessage<RankedLeaderboardOverviewResponse>(new RankedLeaderboardOverviewRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameType = gameType
		}, onResponseCallback);
	}

	public void SendRankedLeaderboardOverviewRequest(GameType gameType, int groupSize, RankedLeaderboardSpecificRequest.RequestSpecificationType specification, Action<RankedLeaderboardSpecificResponse> onResponseCallback)
	{
		this.SendRequestMessage<RankedLeaderboardSpecificResponse>(new RankedLeaderboardSpecificRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			GameType = gameType,
			GroupSize = groupSize,
			Specification = specification
		}, onResponseCallback);
	}

	public void SetNewSessionLanguage(string languageCode)
	{
		base.SendMessage(new OverrideSessionLanguageCodeNotification
		{
			RequestId = this.m_messageDispatcher.GetRequestId(),
			LanguageCode = languageCode
		});
	}

	public void symbol_001D(string symbol_001D, string symbol_000E)
	{
		base.SendMessage(new DEBUG_AdminSlashCommandNotification
		{
			Command = symbol_001D,
			Arguments = symbol_000E
		});
	}

	public void symbol_001D(Action<DEBUG_ForceMatchmakingResponse> symbol_001D)
	{
		this.SendRequestMessage<DEBUG_ForceMatchmakingResponse>(new DEBUG_ForceMatchmakingRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, symbol_001D);
	}

	public void symbol_001D(Action<DEBUG_TakeSnapshotResponse> symbol_001D)
	{
		this.SendRequestMessage<DEBUG_TakeSnapshotResponse>(new DEBUG_TakeSnapshotRequest
		{
			RequestId = this.m_messageDispatcher.GetRequestId()
		}, symbol_001D);
	}
}
