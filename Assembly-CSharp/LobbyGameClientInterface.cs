using LobbyGameClientMessages;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading;
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

	public bool IsConnected => base.State == WebSocket.SocketState.Open && m_registered;

	public LobbySessionInfo SessionInfo => m_sessionInfo;

	public event Action<RegisterGameClientResponse> OnConnected
	{
		add
		{
			Action<RegisterGameClientResponse> action = this.OnConnected;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnected, (Action<RegisterGameClientResponse>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<RegisterGameClientResponse> action = this.OnConnected;
			Action<RegisterGameClientResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnected, (Action<RegisterGameClientResponse>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnDisconnected, (Action<string, bool, CloseStatusCode>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<string, bool, CloseStatusCode> action = this.OnDisconnected;
			Action<string, bool, CloseStatusCode> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnDisconnected, (Action<string, bool, CloseStatusCode>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnLobbyServerReadyNotification, (Action<LobbyServerReadyNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyServerReadyNotification> action = this.OnLobbyServerReadyNotification;
			Action<LobbyServerReadyNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyServerReadyNotification, (Action<LobbyServerReadyNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnLobbyStatusNotification, (Action<LobbyStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<LobbyStatusNotification> action = this.OnLobbyStatusNotification;
			Action<LobbyStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyStatusNotification, (Action<LobbyStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyGameplayOverridesNotification, (Action<LobbyGameplayOverridesNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyGameplayOverridesNotification> action = this.OnLobbyGameplayOverridesNotification;
			Action<LobbyGameplayOverridesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyGameplayOverridesNotification, (Action<LobbyGameplayOverridesNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyCustomGamesNotification, (Action<LobbyCustomGamesNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyCustomGamesNotification> action = this.OnLobbyCustomGamesNotification;
			Action<LobbyCustomGamesNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyCustomGamesNotification, (Action<LobbyCustomGamesNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnQueueAssignmentNotification, (Action<MatchmakingQueueAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<MatchmakingQueueAssignmentNotification> action = this.OnQueueAssignmentNotification;
			Action<MatchmakingQueueAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQueueAssignmentNotification, (Action<MatchmakingQueueAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnQueueStatusNotification, (Action<MatchmakingQueueStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<MatchmakingQueueStatusNotification> action = this.OnQueueStatusNotification;
			Action<MatchmakingQueueStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQueueStatusNotification, (Action<MatchmakingQueueStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGameAssignmentNotification, (Action<GameAssignmentNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameAssignmentNotification> action = this.OnGameAssignmentNotification;
			Action<GameAssignmentNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameAssignmentNotification, (Action<GameAssignmentNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGameInfoNotification, (Action<GameInfoNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<GameInfoNotification> action = this.OnGameInfoNotification;
			Action<GameInfoNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameInfoNotification, (Action<GameInfoNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGameStatusNotification, (Action<GameStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GameStatusNotification> action = this.OnGameStatusNotification;
			Action<GameStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGameStatusNotification, (Action<GameStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnAccountDataUpdated, (Action<PlayerAccountDataUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<PlayerAccountDataUpdateNotification> action = this.OnAccountDataUpdated;
			Action<PlayerAccountDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnAccountDataUpdated, (Action<PlayerAccountDataUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

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
				action = Interlocked.CompareExchange(ref this.OnCharacterDataUpdateNotification, (Action<PlayerCharacterDataUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<PlayerCharacterDataUpdateNotification> action = this.OnCharacterDataUpdateNotification;
			Action<PlayerCharacterDataUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnCharacterDataUpdateNotification, (Action<PlayerCharacterDataUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnInventoryComponentUpdateNotification, (Action<InventoryComponentUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<InventoryComponentUpdateNotification> action = this.OnInventoryComponentUpdateNotification;
			Action<InventoryComponentUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnInventoryComponentUpdateNotification, (Action<InventoryComponentUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnBankBalanceChangeNotification, (Action<BankBalanceChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<BankBalanceChangeNotification> action = this.OnBankBalanceChangeNotification;
			Action<BankBalanceChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnBankBalanceChangeNotification, (Action<BankBalanceChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnSeasonStatusNotification, (Action<SeasonStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<SeasonStatusNotification> action = this.OnSeasonStatusNotification;
			Action<SeasonStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnSeasonStatusNotification, (Action<SeasonStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnChapterStatusNotification, (Action<ChapterStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<ChapterStatusNotification> action = this.OnChapterStatusNotification;
			Action<ChapterStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnChapterStatusNotification, (Action<ChapterStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGroupUpdateNotification, (Action<GroupUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GroupUpdateNotification> action = this.OnGroupUpdateNotification;
			Action<GroupUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGroupUpdateNotification, (Action<GroupUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnUseGGPackNotification, (Action<UseGGPackNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<UseGGPackNotification> action = this.OnUseGGPackNotification;
			Action<UseGGPackNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnUseGGPackNotification, (Action<UseGGPackNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnChatNotification, (Action<ChatNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ChatNotification> action = this.OnChatNotification;
			Action<ChatNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnChatNotification, (Action<ChatNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnUseOverconNotification, (Action<UseOverconResponse>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<UseOverconResponse> action = this.OnUseOverconNotification;
			Action<UseOverconResponse> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnUseOverconNotification, (Action<UseOverconResponse>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFriendStatusNotification, (Action<FriendStatusNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<FriendStatusNotification> action = this.OnFriendStatusNotification;
			Action<FriendStatusNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFriendStatusNotification, (Action<FriendStatusNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGroupConfirmation, (Action<GroupConfirmationRequest>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GroupConfirmationRequest> action = this.OnGroupConfirmation;
			Action<GroupConfirmationRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGroupConfirmation, (Action<GroupConfirmationRequest>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnGroupSuggestion, (Action<GroupSuggestionRequest>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<GroupSuggestionRequest> action = this.OnGroupSuggestion;
			Action<GroupSuggestionRequest> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnGroupSuggestion, (Action<GroupSuggestionRequest>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnForceQueueNotification, (Action<ForceMatchmakingQueueNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ForceMatchmakingQueueNotification> action = this.OnForceQueueNotification;
			Action<ForceMatchmakingQueueNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnForceQueueNotification, (Action<ForceMatchmakingQueueNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

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
				action = Interlocked.CompareExchange(ref this.OnQuestCompleteNotification, (Action<QuestCompleteNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<QuestCompleteNotification> action = this.OnQuestCompleteNotification;
			Action<QuestCompleteNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnQuestCompleteNotification, (Action<QuestCompleteNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnMatchResultsNotification, (Action<MatchResultsNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<MatchResultsNotification> action = this.OnMatchResultsNotification;
			Action<MatchResultsNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMatchResultsNotification, (Action<MatchResultsNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.OnServerQueueConfigurationUpdateNotification, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ServerQueueConfigurationUpdateNotification> action = this.OnServerQueueConfigurationUpdateNotification;
			Action<ServerQueueConfigurationUpdateNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnServerQueueConfigurationUpdateNotification, (Action<ServerQueueConfigurationUpdateNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnRankedOverviewChangeNotification, (Action<RankedOverviewChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<RankedOverviewChangeNotification> action = this.OnRankedOverviewChangeNotification;
			Action<RankedOverviewChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnRankedOverviewChangeNotification, (Action<RankedOverviewChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFactionCompetitionNotification, (Action<FactionCompetitionNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FactionCompetitionNotification> action = this.OnFactionCompetitionNotification;
			Action<FactionCompetitionNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFactionCompetitionNotification, (Action<FactionCompetitionNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnTrustBoostUsedNotification, (Action<TrustBoostUsedNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<TrustBoostUsedNotification> action = this.OnTrustBoostUsedNotification;
			Action<TrustBoostUsedNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnTrustBoostUsedNotification, (Action<TrustBoostUsedNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFacebookAccessTokenNotification, (Action<FacebookAccessTokenNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FacebookAccessTokenNotification> action = this.OnFacebookAccessTokenNotification;
			Action<FacebookAccessTokenNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFacebookAccessTokenNotification, (Action<FacebookAccessTokenNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnPlayerFactionContributionChange, (Action<PlayerFactionContributionChangeNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<PlayerFactionContributionChangeNotification> action = this.OnPlayerFactionContributionChange;
			Action<PlayerFactionContributionChangeNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnPlayerFactionContributionChange, (Action<PlayerFactionContributionChangeNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnFactionLoginRewardNotification, (Action<FactionLoginRewardNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<FactionLoginRewardNotification> action = this.OnFactionLoginRewardNotification;
			Action<FactionLoginRewardNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnFactionLoginRewardNotification, (Action<FactionLoginRewardNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbyAlertMissionDataNotification, (Action<LobbyAlertMissionDataNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbyAlertMissionDataNotification> action = this.OnLobbyAlertMissionDataNotification;
			Action<LobbyAlertMissionDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbyAlertMissionDataNotification, (Action<LobbyAlertMissionDataNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
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
				action = Interlocked.CompareExchange(ref this.OnLobbySeasonQuestDataNotification, (Action<LobbySeasonQuestDataNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<LobbySeasonQuestDataNotification> action = this.OnLobbySeasonQuestDataNotification;
			Action<LobbySeasonQuestDataNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLobbySeasonQuestDataNotification, (Action<LobbySeasonQuestDataNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public LobbyGameClientInterface()
	{
		this.OnConnected = delegate
		{
		};
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = delegate
			{
			};
		}
		this.OnDisconnected = _003C_003Ef__am_0024cache1;
		this.OnLobbyServerReadyNotification = delegate
		{
		};
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = delegate
			{
			};
		}
		this.OnLobbyStatusNotification = _003C_003Ef__am_0024cache3;
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = delegate
			{
			};
		}
		this.OnLobbyGameplayOverridesNotification = _003C_003Ef__am_0024cache4;
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = delegate
			{
			};
		}
		this.OnLobbyCustomGamesNotification = _003C_003Ef__am_0024cache5;
		this.OnQueueAssignmentNotification = delegate
		{
		};
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = delegate
			{
			};
		}
		this.OnQueueStatusNotification = _003C_003Ef__am_0024cache7;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = delegate
			{
			};
		}
		this.OnGameAssignmentNotification = _003C_003Ef__am_0024cache8;
		if (_003C_003Ef__am_0024cache9 == null)
		{
			_003C_003Ef__am_0024cache9 = delegate
			{
			};
		}
		this.OnGameInfoNotification = _003C_003Ef__am_0024cache9;
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = delegate
			{
			};
		}
		this.OnGameStatusNotification = _003C_003Ef__am_0024cacheA;
		if (_003C_003Ef__am_0024cacheB == null)
		{
			_003C_003Ef__am_0024cacheB = delegate
			{
			};
		}
		this.OnAccountDataUpdated = _003C_003Ef__am_0024cacheB;
		if (_003C_003Ef__am_0024cacheC == null)
		{
			_003C_003Ef__am_0024cacheC = delegate
			{
			};
		}
		this.OnForcedCharacterChangeFromServerNotification = _003C_003Ef__am_0024cacheC;
		if (_003C_003Ef__am_0024cacheD == null)
		{
			_003C_003Ef__am_0024cacheD = delegate
			{
			};
		}
		this.OnCharacterDataUpdateNotification = _003C_003Ef__am_0024cacheD;
		if (_003C_003Ef__am_0024cacheE == null)
		{
			_003C_003Ef__am_0024cacheE = delegate
			{
			};
		}
		this.OnInventoryComponentUpdateNotification = _003C_003Ef__am_0024cacheE;
		if (_003C_003Ef__am_0024cacheF == null)
		{
			_003C_003Ef__am_0024cacheF = delegate
			{
			};
		}
		this.OnBankBalanceChangeNotification = _003C_003Ef__am_0024cacheF;
		if (_003C_003Ef__am_0024cache10 == null)
		{
			_003C_003Ef__am_0024cache10 = delegate
			{
			};
		}
		this.OnSeasonStatusNotification = _003C_003Ef__am_0024cache10;
		this.OnChapterStatusNotification = delegate
		{
		};
		if (_003C_003Ef__am_0024cache12 == null)
		{
			_003C_003Ef__am_0024cache12 = delegate
			{
			};
		}
		this.OnGroupUpdateNotification = _003C_003Ef__am_0024cache12;
		if (_003C_003Ef__am_0024cache13 == null)
		{
			_003C_003Ef__am_0024cache13 = delegate
			{
			};
		}
		this.OnUseGGPackNotification = _003C_003Ef__am_0024cache13;
		if (_003C_003Ef__am_0024cache14 == null)
		{
			_003C_003Ef__am_0024cache14 = delegate
			{
			};
		}
		this.OnChatNotification = _003C_003Ef__am_0024cache14;
		if (_003C_003Ef__am_0024cache15 == null)
		{
			_003C_003Ef__am_0024cache15 = delegate
			{
			};
		}
		this.OnUseOverconNotification = _003C_003Ef__am_0024cache15;
		if (_003C_003Ef__am_0024cache16 == null)
		{
			_003C_003Ef__am_0024cache16 = delegate
			{
			};
		}
		this.OnFriendStatusNotification = _003C_003Ef__am_0024cache16;
		this.OnGroupConfirmation = delegate
		{
		};
		if (_003C_003Ef__am_0024cache18 == null)
		{
			_003C_003Ef__am_0024cache18 = delegate
			{
			};
		}
		this.OnGroupSuggestion = _003C_003Ef__am_0024cache18;
		if (_003C_003Ef__am_0024cache19 == null)
		{
			_003C_003Ef__am_0024cache19 = delegate
			{
			};
		}
		this.OnForceQueueNotification = _003C_003Ef__am_0024cache19;
		if (_003C_003Ef__am_0024cache1A == null)
		{
			_003C_003Ef__am_0024cache1A = delegate
			{
			};
		}
		this.OnGameInviteConfirmationRequest = _003C_003Ef__am_0024cache1A;
		if (_003C_003Ef__am_0024cache1B == null)
		{
			_003C_003Ef__am_0024cache1B = delegate
			{
			};
		}
		this.OnQuestCompleteNotification = _003C_003Ef__am_0024cache1B;
		if (_003C_003Ef__am_0024cache1C == null)
		{
			_003C_003Ef__am_0024cache1C = delegate
			{
			};
		}
		this.OnMatchResultsNotification = _003C_003Ef__am_0024cache1C;
		if (_003C_003Ef__am_0024cache1D == null)
		{
			_003C_003Ef__am_0024cache1D = delegate
			{
			};
		}
		this.OnServerQueueConfigurationUpdateNotification = _003C_003Ef__am_0024cache1D;
		if (_003C_003Ef__am_0024cache1E == null)
		{
			_003C_003Ef__am_0024cache1E = delegate
			{
			};
		}
		this.OnRankedOverviewChangeNotification = _003C_003Ef__am_0024cache1E;
		this.OnFactionCompetitionNotification = delegate
		{
		};
		this.OnTrustBoostUsedNotification = delegate
		{
		};
		if (_003C_003Ef__am_0024cache21 == null)
		{
			_003C_003Ef__am_0024cache21 = delegate
			{
			};
		}
		this.OnFacebookAccessTokenNotification = _003C_003Ef__am_0024cache21;
		if (_003C_003Ef__am_0024cache22 == null)
		{
			_003C_003Ef__am_0024cache22 = delegate
			{
			};
		}
		this.OnPlayerFactionContributionChange = _003C_003Ef__am_0024cache22;
		if (_003C_003Ef__am_0024cache23 == null)
		{
			_003C_003Ef__am_0024cache23 = delegate
			{
			};
		}
		this.OnFactionLoginRewardNotification = _003C_003Ef__am_0024cache23;
		if (_003C_003Ef__am_0024cache24 == null)
		{
			_003C_003Ef__am_0024cache24 = delegate
			{
			};
		}
		this.OnLobbyAlertMissionDataNotification = _003C_003Ef__am_0024cache24;
		if (_003C_003Ef__am_0024cache25 == null)
		{
			_003C_003Ef__am_0024cache25 = delegate
			{
			};
		}
		this.OnLobbySeasonQuestDataNotification = _003C_003Ef__am_0024cache25;
		base._002Ector(Factory.Get());
		m_registered = false;
		m_sessionInfo = new LobbySessionInfo();
		base.ConnectionTimeout = 30f;
	}

	public void Initialize(string directoryServerAddress, AuthTicket ticket, Region region, string languageCode, ProcessType processType, int preferredLobbyServerIndex = 0)
	{
		m_lobbyServerAddress = null;
		m_directoryServerAddress = directoryServerAddress;
		m_sessionInfo.BuildVersion = BuildVersion.ShortVersionString;
		m_sessionInfo.ProtocolVersion = Factory.Get().ProtocolVersion;
		m_sessionInfo.AccountId = ticket.AccountId;
		m_sessionInfo.UserName = ticket.UserName;
		m_sessionInfo.Handle = ticket.Handle;
		m_sessionInfo.ProcessType = processType;
		m_sessionInfo.Region = region;
		m_sessionInfo.LanguageCode = languageCode;
		m_ticket = ticket;
		m_preferredLobbyServerIndex = preferredLobbyServerIndex;
		m_messageDispatcher = new WebSocketMessageDispatcher<LobbyGameClientInterface>();
		m_messageDispatcher.RegisterMessageDelegate<ConnectionOpenedNotification>(HandleConnectionOpenedNotification);
		m_messageDispatcher.RegisterMessageDelegate<ConnectionClosedNotification>(HandleConnectionClosedNotification);
		m_messageDispatcher.RegisterMessageDelegate<ConnectionErrorNotification>(HandleConnectionErrorNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbyServerReadyNotification>(HandleLobbyServerReadyNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbyStatusNotification>(HandleLobbyStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbyGameplayOverridesNotification>(HandleLobbyGameplayOverridesNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbyCustomGamesNotification>(HandleLobbyCustomGamesNotification);
		m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueStatusNotification>(HandleQueueStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueAssignmentNotification>(HandleQueueAssignmentNotification);
		m_messageDispatcher.RegisterMessageDelegate<GameAssignmentNotification>(HandleGameAssignmentNotification);
		m_messageDispatcher.RegisterMessageDelegate<GameInfoNotification>(HandleGameInfoNotification);
		m_messageDispatcher.RegisterMessageDelegate<GameStatusNotification>(HandleGameStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<SynchronizeWithClientOutOfGameRequest>(HandleSynchronizeWithClientOutOfGameRequest);
		m_messageDispatcher.RegisterMessageDelegate<ForcedCharacterChangeFromServerNotification>(HandleRequeueForceCharacterNotification);
		m_messageDispatcher.RegisterMessageDelegate<PlayerCharacterDataUpdateNotification>(HandleCharacterDataUpdateNotification);
		m_messageDispatcher.RegisterMessageDelegate<InventoryComponentUpdateNotification>(HandleInventoryComponentUpdateNotification);
		m_messageDispatcher.RegisterMessageDelegate<BankBalanceChangeNotification>(HandleBankBalanceChangeNotification);
		m_messageDispatcher.RegisterMessageDelegate<PlayerAccountDataUpdateNotification>(HandleAccountDataUpdateNotification);
		m_messageDispatcher.RegisterMessageDelegate<SeasonStatusNotification>(HandleSeasonStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<ChapterStatusNotification>(HandleChapterStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<GroupUpdateNotification>(HandleGroupUpdateNotification);
		m_messageDispatcher.RegisterMessageDelegate<UseGGPackNotification>(HandleGGPackUsedNotification);
		m_messageDispatcher.RegisterMessageDelegate<ChatNotification>(HandleChatNotification);
		m_messageDispatcher.RegisterMessageDelegate<UseOverconResponse>(HandleUseOverconResponse);
		m_messageDispatcher.RegisterMessageDelegate<FriendStatusNotification>(HandleFriendStatusNotification);
		m_messageDispatcher.RegisterMessageDelegate<GroupConfirmationRequest>(HandleGroupConfirmationRequest);
		m_messageDispatcher.RegisterMessageDelegate<GroupSuggestionRequest>(HandleGroupSuggestionRequest);
		m_messageDispatcher.RegisterMessageDelegate<GameInviteConfirmationRequest>(HandleGameInviteConfirmationRequest);
		m_messageDispatcher.RegisterMessageDelegate<QuestCompleteNotification>(HandleQuestCompleteNotification);
		m_messageDispatcher.RegisterMessageDelegate<MatchResultsNotification>(HandleMatchResultsNotification);
		m_messageDispatcher.RegisterMessageDelegate<PendingPurchaseResult>(HandlePendingPurchaseResult);
		m_messageDispatcher.RegisterMessageDelegate<ForceMatchmakingQueueNotification>(HandleForceQueueNotification);
		m_messageDispatcher.RegisterMessageDelegate<FreelancerUnavailableNotification>(HandleFreelancerUnavailableNotification);
		m_messageDispatcher.RegisterMessageDelegate<EnterFreelancerResolutionPhaseNotification>(HandleResolvingDuplicateFreelancerNotification);
		m_messageDispatcher.RegisterMessageDelegate<MatchmakingQueueToPlayersNotification>(HandleQueueToPlayerNotification);
		m_messageDispatcher.RegisterMessageDelegate<ServerQueueConfigurationUpdateNotification>(HandleServerQueueConfigurationUpdateNotification);
		m_messageDispatcher.RegisterMessageDelegate<RankedOverviewChangeNotification>(HandleRankedOverviewChangeNotification);
		m_messageDispatcher.RegisterMessageDelegate<GameDestroyedByPlayerNotification>(HandleGameDestroyedByPlayerNotification);
		m_messageDispatcher.RegisterMessageDelegate<FactionCompetitionNotification>(HandleFactionCompetitionNotification);
		m_messageDispatcher.RegisterMessageDelegate<TrustBoostUsedNotification>(HandleTrustBoostUsedNotification);
		m_messageDispatcher.RegisterMessageDelegate<FacebookAccessTokenNotification>(HandleFacebookAccessTokenNotification);
		m_messageDispatcher.RegisterMessageDelegate<PlayerFactionContributionChangeNotification>(HandlePlayerFactionContributionChangeNotification);
		m_messageDispatcher.RegisterMessageDelegate<FactionLoginRewardNotification>(HandleFactionLoginRewardNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbyAlertMissionDataNotification>(HandleLobbyAlertMissionDataNotification);
		m_messageDispatcher.RegisterMessageDelegate<LobbySeasonQuestDataNotification>(HandleLobbySeasonQuestDataNotification);
		m_messageDispatcher.RegisterMessageDelegate<ErrorReportSummaryRequest>(HandleErrorReportSummaryRequest);
		m_lastLobbyErrorMessage = null;
		m_allowRelogin = true;
	}

	public void AssignGameClient(string directoryServerAddress)
	{
		int requestId = m_messageDispatcher.GetRequestId();
		m_messageDispatcher.RegisterMessageDelegate<RegisterGameClientResponse>(HandleRegisterGameClientResponse, requestId);
		RegisterGameClientResponse registerResponse = new RegisterGameClientResponse();
		registerResponse.Success = false;
		registerResponse.ResponseId = requestId;
		Action handleNetworkException = delegate
		{
			registerResponse.Success = false;
			registerResponse.LocalizedFailure = LocalizationPayload.Create("NetworkError", "Global");
			m_messageDispatcher.HandleMessage(this, registerResponse);
		};
		Action<string> handleRequestFailure = delegate(string message)
		{
			registerResponse.Success = false;
			registerResponse.ErrorMessage = message;
			m_messageDispatcher.HandleMessage(this, registerResponse);
		};
		try
		{
			m_overallConnectionTimer.Start();
			AssignGameClientRequest assignGameClientRequest = new AssignGameClientRequest();
			assignGameClientRequest.RequestId = m_messageDispatcher.GetRequestId();
			assignGameClientRequest.SessionInfo = m_sessionInfo;
			assignGameClientRequest.AuthInfo = m_ticket.AuthInfo;
			assignGameClientRequest.PreferredLobbyServerIndex = m_preferredLobbyServerIndex;
			if (directoryServerAddress.IndexOf("://") == -1)
			{
				directoryServerAddress = "ws://" + directoryServerAddress;
			}
			Uri uri = new Uri(directoryServerAddress);
			UriBuilder newDirectoryServerUri = new UriBuilder();
			int num = 6050;
			string str = "DirectorySessionManager";
			UriBuilder uriBuilder = newDirectoryServerUri;
			object scheme;
			if (!(uri.Scheme == "ws"))
			{
				if (!(uri.Scheme == "http"))
				{
					if (!(uri.Scheme == "wss"))
					{
						if (!(uri.Scheme == "https"))
						{
							scheme = newDirectoryServerUri.Scheme;
							goto IL_01ac;
						}
					}
					scheme = "https";
					goto IL_01ac;
				}
			}
			scheme = "http";
			goto IL_01ac;
			IL_01ac:
			uriBuilder.Scheme = (string)scheme;
			newDirectoryServerUri.Host = NetUtil.GetIPv4Address(uri.Host).ToString();
			UriBuilder uriBuilder2 = newDirectoryServerUri;
			int port;
			if (uri.Port > 0)
			{
				if (!uri.IsDefaultPort)
				{
					port = uri.Port;
					goto IL_0211;
				}
			}
			port = num;
			goto IL_0211;
			IL_0211:
			uriBuilder2.Port = port;
			UriBuilder uriBuilder3 = newDirectoryServerUri;
			string path;
			if (uri.AbsolutePath != "/")
			{
				path = uri.AbsolutePath;
			}
			else
			{
				path = "/" + str;
			}
			uriBuilder3.Path = path;
			newDirectoryServerUri.Query = $"messageType={assignGameClientRequest.GetType().Name}";
			base.Logger.Info("Requesting lobby server assignment from {0}", newDirectoryServerUri);
			SendHttpRequest(newDirectoryServerUri.ToString(), assignGameClientRequest, delegate(AssignGameClientResponse assignResponse, Exception exception)
			{
				try
				{
					if (exception != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								if (m_overallConnectionTimer.Elapsed.TotalSeconds < (double)base.ConnectionTimeout)
								{
									base.Logger.Info("Re-requesting lobby server assignment from {0}: {1}", newDirectoryServerUri, exception.Message.Trim());
									Reconnect();
									return;
								}
								m_overallConnectionTimer.Reset();
								throw exception;
							}
						}
					}
					if (!assignResponse.Success)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								throw new ClientRequestFailed(assignResponse.ErrorMessage);
							}
						}
					}
					if (assignResponse.LobbyServerAddress.IsNullOrEmpty())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								throw new ClientRequestFailed("Empty response from server");
							}
						}
					}
					m_lobbyServerAddress = assignResponse.LobbyServerAddress;
					m_sessionInfo = assignResponse.SessionInfo;
					Connect();
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
		m_registered = false;
		m_sessionInfo.IsBinary = base.IsBinary;
		if (m_lobbyServerAddress == null)
		{
			if (m_directoryServerAddress == null)
			{
				throw new Exception("Directory server address must be specified");
			}
			AssignGameClient(m_directoryServerAddress);
		}
		else
		{
			InitializeSocket(m_lobbyServerAddress, 6060, "LobbyGameClientSessionManager");
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
		if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._0012)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					AppState_GroupCharacterSelect.Get().ReEnter(true);
					UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true);
					return;
				}
			}
		}
		if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._0015)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					string description = StringUtil.TR("RuinedGameStartSoThrownOutOfQueue", "Global");
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), description, StringUtil.TR("Ok", "Global"));
					Log.Info("Updating ready state to false because ruined game, thrown out of queue, current Appstate: " + AppState.GetCurrent().ToString());
					AppState_GroupCharacterSelect.Get().UpdateReadyState(false);
					return;
				}
				}
			}
		}
		if (notification.MessageToSend == MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._000E)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					ClientGameManager.Get().HandleQueueConfirmation();
					return;
				}
			}
		}
		string description2 = StringUtil.TR("UnknownQueueManagerBug", "Global");
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), description2, StringUtil.TR("Ok", "Global"));
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		this.OnServerQueueConfigurationUpdateNotification(notification);
	}

	private void HandleGameDestroyedByPlayerNotification(GameDestroyedByPlayerNotification notification)
	{
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), notification.LocalizedMessage.ToString(), StringUtil.TR("Ok", "Global"));
	}

	private void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		if (notification.SubPhase == FreelancerResolutionPhaseSubType.DUPLICATE_FREELANCER)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIMatchStartPanel.Get().NotifyDuplicateFreelancer(true);
					return;
				}
			}
		}
		if (!notification.RankedData.HasValue)
		{
			return;
		}
		while (true)
		{
			if (notification.SubPhase == FreelancerResolutionPhaseSubType.WAITING_FOR_ALL_PLAYERS)
			{
				return;
			}
			while (true)
			{
				if (UIRankedModeDraftScreen.Get() == null)
				{
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
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
				if (value._001D(ourPlayerId))
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
							CharacterType current = enumerator.Current;
							list.Remove(current);
						}
					}
					using (List<CharacterType>.Enumerator enumerator2 = value.EnemyBans.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CharacterType current2 = enumerator2.Current;
							list.Remove(current2);
						}
					}
					foreach (CharacterType value2 in value.EnemyTeamSelections.Values)
					{
						list.Remove(value2);
					}
					foreach (CharacterType value3 in value.FriendlyTeamSelections.Values)
					{
						list.Remove(value3);
					}
				}
				return;
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
			text = $"{unlocalizedFailure}#NeedsLocalization";
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
	}

	public void SendRankedTradeRequest(CharacterType desiredCharacter, RankedTradeData.TradeActionType tradeAction)
	{
		RankedTradeRequest rankedTradeRequest = new RankedTradeRequest();
		rankedTradeRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedTradeRequest.Trade.DesiredCharacter = desiredCharacter;
		rankedTradeRequest.Trade.TradeAction = tradeAction;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedTradeRequest, delegate(RankedTradeResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
						return;
					}
				}
			}
		});
	}

	public void SendRankedBanRequest(CharacterType selection)
	{
		RankedBanRequest rankedBanRequest = new RankedBanRequest();
		rankedBanRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedBanRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedBanRequest, delegate(RankedBanResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
						return;
					}
				}
			}
		});
	}

	public void SendRankedSelectionRequest(CharacterType selection)
	{
		RankedSelectionRequest rankedSelectionRequest = new RankedSelectionRequest();
		rankedSelectionRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedSelectionRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedSelectionRequest, delegate(RankedSelectionResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
						return;
					}
				}
			}
		});
	}

	public void SendRankedHoverClickRequest(CharacterType selection)
	{
		RankedHoverClickRequest rankedHoverClickRequest = new RankedHoverClickRequest();
		rankedHoverClickRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedHoverClickRequest.Selection = selection;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedHoverClickRequest, delegate(RankedHoverClickResponse response)
		{
			if (!response.Success)
			{
				WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
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
			description = LocalizationPayload.Create("ForcedToPlayFreelancer", "Global", LocalizationArg_Freelancer.Create(notification.oldCharacterType), LocalizationArg_Freelancer.Create(notification.newCharacterType)).ToString();
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
		if (notification.oldCharacterType.IsWillFill())
		{
			return;
		}
		while (true)
		{
			UIDialogPopupManager.OpenOneButtonDialog(title, description, StringUtil.TR("Ok", "Global"));
			return;
		}
	}

	private void HandleConnectionOpenedNotification(ConnectionOpenedNotification notification)
	{
		RegisterGameClient();
	}

	private void HandleConnectionClosedNotification(ConnectionClosedNotification notification)
	{
		if (m_registered)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					base.Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
					this.OnDisconnected(m_lastLobbyErrorMessage, m_allowRelogin, notification.Code);
					m_lastLobbyErrorMessage = null;
					m_allowRelogin = true;
					return;
				}
			}
		}
		if (!m_overallConnectionTimer.IsRunning)
		{
			return;
		}
		while (true)
		{
			if (m_overallConnectionTimer.Elapsed.TotalSeconds < (double)base.ConnectionTimeout)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						base.Logger.Info("Retrying connection to {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
						Reconnect();
						return;
					}
				}
			}
			base.Logger.Info("Failed to connect to {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
			m_overallConnectionTimer.Reset();
			RegisterGameClientResponse registerGameClientResponse = new RegisterGameClientResponse();
			registerGameClientResponse.Success = false;
			registerGameClientResponse.LocalizedFailure = LocalizationPayload.Create("NetworkError", "Global");
			this.OnConnected(registerGameClientResponse);
			return;
		}
	}

	private void HandleConnectionErrorNotification(ConnectionErrorNotification notification)
	{
		Log.Error("Communication error to lobby server: {0}", notification.ErrorMessage);
	}

	protected override void HandleMessage(WebSocketMessage message)
	{
		base.HandleMessage(message);
		m_messageDispatcher.HandleMessage(this, message);
	}

	public bool SendRequestMessage<ResponseType>(WebSocketMessage request, Action<ResponseType> callback) where ResponseType : WebSocketResponseMessage, new()
	{
		return SendRequestMessage(request, callback, m_messageDispatcher);
	}

	public void UnregisterRequest<ResponseType>(int requestId) where ResponseType : WebSocketResponseMessage, new()
	{
		m_messageDispatcher.UnregisterMessageDelegate<ResponseType>(requestId);
	}

	private void RegisterGameClient()
	{
		RegisterGameClientRequest registerGameClientRequest = new RegisterGameClientRequest();
		registerGameClientRequest.RequestId = m_messageDispatcher.GetRequestId();
		registerGameClientRequest.SessionInfo = m_sessionInfo;
		registerGameClientRequest.AuthInfo = m_ticket.AuthInfo;
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
		SendRequestMessage<RegisterGameClientResponse>(registerGameClientRequest, HandleRegisterGameClientResponse);
	}

	private void HandleRegisterGameClientResponse(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			base.Logger.Error("Failed to register game client with lobby server: {0}", response.ErrorMessage);
			m_registered = false;
			this.OnConnected(response);
			Disconnect();
			return;
		}
		base.Logger.Info("Registered game client with lobby server");
		m_registered = true;
		m_overallConnectionTimer.Reset();
		if (response.SessionInfo != null)
		{
			m_sessionInfo = response.SessionInfo;
		}
		else if (response.AuthInfo != null)
		{
			m_sessionInfo.AccountId = response.AuthInfo.AccountId;
		}
		if (response.Status != null)
		{
			HandleLobbyStatusNotification(response.Status);
		}
		if (response.AuthInfo != null)
		{
			if (!string.IsNullOrEmpty(response.AuthInfo.Handle))
			{
				m_ticket.AuthInfo = response.AuthInfo;
			}
		}
		this.OnConnected(response);
	}

	private void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		this.OnLobbyServerReadyNotification(notification);
		if (notification.Success)
		{
			base.Logger.Info("Lobby server is ready");
			if (notification.FriendStatus != null)
			{
				HandleFriendStatusNotification(notification.FriendStatus);
			}
			if (notification.ServerQueueConfiguration == null)
			{
				return;
			}
			while (true)
			{
				HandleServerQueueConfigurationUpdateNotification(notification.ServerQueueConfiguration);
				return;
			}
		}
		base.Logger.Error("Lobby server failed to become ready: {0}", notification.ErrorMessage);
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.LocalizedFailure != null)
		{
			base.Logger.Error("Error from lobby server: {0}", notification.LocalizedFailure.ToString());
			m_lastLobbyErrorMessage = notification.LocalizedFailure.ToString();
			m_allowRelogin = notification.AllowRelogin;
			Disconnect();
		}
		else
		{
			this.OnLobbyStatusNotification(notification);
		}
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		this.OnLobbyGameplayOverridesNotification(notification);
	}

	public void SubscribeToCustomGames()
	{
		SendMessage(new SubscribeToCustomGamesRequest());
	}

	public void UnsubscribeFromCustomGames()
	{
		SendMessage(new UnsubscribeFromCustomGamesRequest());
	}

	private void HandleLobbyCustomGamesNotification(LobbyCustomGamesNotification notification)
	{
		this.OnLobbyCustomGamesNotification(notification);
	}

	public void SetGameTypeSubMasks(GameType gameType, ushort subGameMask, Action<SetGameSubTypeResponse> onResponseCallback)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - SetGameTypeSubMasks \r\nCall Stack: {0}", "n/a");
					return;
				}
			}
		}
		SetGameSubTypeRequest setGameSubTypeRequest = new SetGameSubTypeRequest();
		setGameSubTypeRequest.RequestId = m_messageDispatcher.GetRequestId();
		setGameSubTypeRequest.gameType = gameType;
		setGameSubTypeRequest.SubTypeMask = subGameMask;
		SendRequestMessage(setGameSubTypeRequest, onResponseCallback);
	}

	public void JoinQueue(GameType gameType, BotDifficulty allyBotDifficulty, BotDifficulty enemyBotDifficulty, Action<JoinMatchmakingQueueResponse> callback)
	{
		JoinMatchmakingQueueRequest joinMatchmakingQueueRequest = new JoinMatchmakingQueueRequest();
		joinMatchmakingQueueRequest.RequestId = m_messageDispatcher.GetRequestId();
		joinMatchmakingQueueRequest.GameType = gameType;
		joinMatchmakingQueueRequest.AllyBotDifficulty = allyBotDifficulty;
		joinMatchmakingQueueRequest.EnemyBotDifficulty = enemyBotDifficulty;
		SendRequestMessage(joinMatchmakingQueueRequest, callback);
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		LeaveMatchmakingQueueRequest leaveMatchmakingQueueRequest = new LeaveMatchmakingQueueRequest();
		leaveMatchmakingQueueRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(leaveMatchmakingQueueRequest, onResponseCallback);
	}

	public void UpdateQueueEnemyBotDifficulty(LobbyMatchmakingQueueInfo queueInfo, BotDifficulty enemyDifficulty)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - UpdateQueueEnemyBotDifficulty");
					return;
				}
			}
		}
		UpdateMatchmakingQueueRequest updateMatchmakingQueueRequest = new UpdateMatchmakingQueueRequest();
		updateMatchmakingQueueRequest.RequestId = m_messageDispatcher.GetRequestId();
		updateMatchmakingQueueRequest.EnemyDifficulty = enemyDifficulty;
		SendMessage(updateMatchmakingQueueRequest);
	}

	public void CreateGame(LobbyGameConfig gameConfig, ReadyState readyState, string processCode, Action<CreateGameResponse> onResponseCallback, BotDifficulty botSkillTeamA = BotDifficulty.Easy, BotDifficulty botSkillTeamB = BotDifficulty.Easy)
	{
		CreateGameRequest createGameRequest = new CreateGameRequest();
		createGameRequest.RequestId = m_messageDispatcher.GetRequestId();
		createGameRequest.GameConfig = gameConfig;
		createGameRequest.ReadyState = readyState;
		createGameRequest.ProcessCode = processCode;
		createGameRequest.SelectedBotSkillTeamA = botSkillTeamA;
		createGameRequest.SelectedBotSkillTeamB = botSkillTeamB;
		SendRequestMessage(createGameRequest, onResponseCallback);
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		JoinGameRequest joinGameRequest = new JoinGameRequest();
		joinGameRequest.RequestId = m_messageDispatcher.GetRequestId();
		joinGameRequest.GameServerProcessCode = gameInfo.GameServerProcessCode;
		joinGameRequest.AsSpectator = asSpectator;
		SendRequestMessage(joinGameRequest, onResponseCallback);
	}

	public void LeaveGame(bool isPermanent, GameResult gameResult, Action<LeaveGameResponse> onResponseCallback)
	{
		LeaveGameRequest leaveGameRequest = new LeaveGameRequest();
		leaveGameRequest.RequestId = m_messageDispatcher.GetRequestId();
		leaveGameRequest.IsPermanent = isPermanent;
		leaveGameRequest.GameResult = gameResult;
		SendRequestMessage(leaveGameRequest, onResponseCallback);
	}

	public void CalculateFreelancerStats(PersistedStatBucket bucketType, CharacterType characterType, PersistedStats stats, MatchFreelancerStats matchFreelancerStats, Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		CalculateFreelancerStatsRequest calculateFreelancerStatsRequest = new CalculateFreelancerStatsRequest();
		calculateFreelancerStatsRequest.RequestId = m_messageDispatcher.GetRequestId();
		calculateFreelancerStatsRequest.PersistedStatBucket = bucketType;
		calculateFreelancerStatsRequest.CharacterType = characterType;
		calculateFreelancerStatsRequest.PersistedStats = stats;
		calculateFreelancerStatsRequest.MatchFreelancerStats = matchFreelancerStats;
		SendRequestMessage(calculateFreelancerStatsRequest, onResponseCallback);
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
		HandleGameAssignmentNotification(new GameAssignmentNotification());
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
		Log.Info("The servers are checking with us to confirm we're out of game {0}", request.GameServerProcessCode);
		SendMessage(new SynchronizeWithClientOutOfGameResponse
		{
			GameServerProcessCode = request.GameServerProcessCode
		});
	}

	public int UpdatePlayerInfo(LobbyPlayerInfoUpdate playerInfoUpdate, Action<PlayerInfoUpdateResponse> onResponseCallback = null)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - UpdatePlayerInfo \r\nCall stack: {0}", "n/a");
					return 0;
				}
			}
		}
		PlayerInfoUpdateRequest playerInfoUpdateRequest = new PlayerInfoUpdateRequest();
		playerInfoUpdateRequest.RequestId = m_messageDispatcher.GetRequestId();
		playerInfoUpdateRequest.PlayerInfoUpdate = playerInfoUpdate;
		playerInfoUpdateRequest.GameType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
		SendRequestMessage(playerInfoUpdateRequest, onResponseCallback);
		return playerInfoUpdateRequest.RequestId;
	}

	public void UpdateGameCheats(GameOptionFlag gameOptionFlags, PlayerGameOptionFlag playerGameOptionFlags, Action<GameCheatUpdateResponse> onResponseCallback = null)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameCheats");
					return;
				}
			}
		}
		GameCheatUpdateRequest gameCheatUpdateRequest = new GameCheatUpdateRequest();
		gameCheatUpdateRequest.RequestId = m_messageDispatcher.GetRequestId();
		gameCheatUpdateRequest.GameOptionFlags = gameOptionFlags;
		gameCheatUpdateRequest.PlayerGameOptionFlags = playerGameOptionFlags;
		SendRequestMessage(gameCheatUpdateRequest, onResponseCallback);
	}

	public void UpdateGroupGameType(GameType gameType, Action<PlayerGroupInfoUpdateResponse> onResponseCallback)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - UpdateGroupGameType");
					return;
				}
			}
		}
		PlayerGroupInfoUpdateRequest playerGroupInfoUpdateRequest = new PlayerGroupInfoUpdateRequest();
		playerGroupInfoUpdateRequest.RequestId = m_messageDispatcher.GetRequestId();
		playerGroupInfoUpdateRequest.GameType = gameType;
		SendRequestMessage(playerGroupInfoUpdateRequest, onResponseCallback);
	}

	public void UpdateGameInfo(LobbyGameInfo gameInfo, LobbyTeamInfo teamInfo)
	{
		if (BlockSendingGroupUpdates)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameInfo");
					return;
				}
			}
		}
		GameInfoUpdateRequest gameInfoUpdateRequest = new GameInfoUpdateRequest();
		gameInfoUpdateRequest.RequestId = m_messageDispatcher.GetRequestId();
		gameInfoUpdateRequest.GameInfo = gameInfo;
		gameInfoUpdateRequest.TeamInfo = teamInfo;
		Action<GameInfoUpdateResponse> callback = delegate(GameInfoUpdateResponse response)
		{
			if (!response.Success)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
						return;
					}
				}
			}
		};
		SendRequestMessage(gameInfoUpdateRequest, callback);
	}

	public void InvitePlayerToGame(string playerHandle, Action<GameInvitationResponse> onResponseCallback)
	{
		GameInvitationRequest gameInvitationRequest = new GameInvitationRequest();
		gameInvitationRequest.RequestId = m_messageDispatcher.GetRequestId();
		gameInvitationRequest.InviteeHandle = playerHandle;
		SendRequestMessage(gameInvitationRequest, onResponseCallback);
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		GameSpectatorRequest gameSpectatorRequest = new GameSpectatorRequest();
		gameSpectatorRequest.RequestId = m_messageDispatcher.GetRequestId();
		gameSpectatorRequest.InviteeHandle = playerHandle;
		SendRequestMessage(gameSpectatorRequest, onResponseCallback);
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		CrashReportArchiveNameRequest crashReportArchiveNameRequest = new CrashReportArchiveNameRequest();
		crashReportArchiveNameRequest.RequestId = m_messageDispatcher.GetRequestId();
		crashReportArchiveNameRequest.NumArchiveBytes = numArchiveBytes;
		SendRequestMessage(crashReportArchiveNameRequest, onResponseCallback);
		return true;
	}

	public bool SendStatusReport(ClientStatusReport report)
	{
		report.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(report);
	}

	public bool SendErrorReport(ClientErrorReport report)
	{
		report.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(report);
	}

	public bool SendErrorSummary(ClientErrorSummary summary)
	{
		summary.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(summary);
	}

	public bool SendFeedbackReport(ClientFeedbackReport report)
	{
		report.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(report);
	}

	public bool SendPerformanceReport(ClientPerformanceReport report)
	{
		report.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(report);
	}

	public bool SendSetRegionRequest(Region region)
	{
		SetRegionRequest setRegionRequest = new SetRegionRequest();
		setRegionRequest.Region = region;
		return SendMessage(setRegionRequest);
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
		PlayerMatchDataRequest playerMatchDataRequest = new PlayerMatchDataRequest();
		playerMatchDataRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(playerMatchDataRequest, onResponseCallback);
	}

	public void PurchaseLoadoutSlot(CharacterType characterType, Action<PurchaseLoadoutSlotResponse> onResponseCallback)
	{
		PurchaseLoadoutSlotRequest purchaseLoadoutSlotRequest = new PurchaseLoadoutSlotRequest();
		purchaseLoadoutSlotRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseLoadoutSlotRequest.Character = characterType;
		SendRequestMessage(purchaseLoadoutSlotRequest, onResponseCallback);
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID, Action<PurchaseModResponse> onResponseCallback)
	{
		PurchaseModRequest purchaseModRequest = new PurchaseModRequest();
		purchaseModRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseModRequest.Character = character;
		purchaseModRequest.UnlockData.AbilityId = abilityId;
		purchaseModRequest.UnlockData.AbilityModID = abilityModID;
		SendRequestMessage(purchaseModRequest, onResponseCallback);
	}

	public void PurchaseModToken(int numToPurchase, Action<PurchaseModTokenResponse> onResponseCallback)
	{
		PurchaseModTokenRequest purchaseModTokenRequest = new PurchaseModTokenRequest();
		purchaseModTokenRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseModTokenRequest.NumToPurchase = numToPurchase;
		SendRequestMessage(purchaseModTokenRequest, onResponseCallback);
	}

	public void RequestBalancedTeam(BalancedTeamRequest request, Action<BalancedTeamResponse> onResponseCallback)
	{
		request.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(request, onResponseCallback);
	}

	public void RequestRefreshBankData(Action<RefreshBankDataResponse> onResponseCallback = null)
	{
		RefreshBankDataRequest refreshBankDataRequest = new RefreshBankDataRequest();
		refreshBankDataRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(refreshBankDataRequest, onResponseCallback);
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
		SeasonStatusConfirmed seasonStatusConfirmed = new SeasonStatusConfirmed();
		seasonStatusConfirmed.Dialog = dialogType;
		return SendMessage(seasonStatusConfirmed);
	}

	private void HandleChapterStatusNotification(ChapterStatusNotification notification)
	{
		this.OnChapterStatusNotification(notification);
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		GroupInviteRequest groupInviteRequest = new GroupInviteRequest();
		groupInviteRequest.RequestId = m_messageDispatcher.GetRequestId();
		groupInviteRequest.FriendHandle = friendHandle;
		SendRequestMessage(groupInviteRequest, onResponseCallback);
	}

	public void RequestToJoinGroup(string friendHandle, Action<GroupJoinResponse> onResponseCallback)
	{
		GroupJoinRequest groupJoinRequest = new GroupJoinRequest();
		groupJoinRequest.RequestId = m_messageDispatcher.GetRequestId();
		groupJoinRequest.FriendHandle = friendHandle;
		SendRequestMessage(groupJoinRequest, onResponseCallback);
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		GroupPromoteRequest groupPromoteRequest = new GroupPromoteRequest();
		groupPromoteRequest.RequestId = m_messageDispatcher.GetRequestId();
		groupPromoteRequest.Name = name;
		SendRequestMessage(groupPromoteRequest, onResponseCallback);
	}

	public void ChatToGroup(string text, Action<GroupChatResponse> onResponseCallback)
	{
		GroupChatRequest groupChatRequest = new GroupChatRequest();
		groupChatRequest.RequestId = m_messageDispatcher.GetRequestId();
		groupChatRequest.Text = text;
		groupChatRequest.RequestedEmojis = ChatEmojiManager.Get().GetAllEmojisInString(text);
		SendRequestMessage(groupChatRequest, onResponseCallback);
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		GroupLeaveRequest groupLeaveRequest = new GroupLeaveRequest();
		groupLeaveRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(groupLeaveRequest, onResponseCallback);
	}

	public void KickFromGroup(string memberName, Action<GroupKickResponse> onResponseCallback)
	{
		GroupKickRequest groupKickRequest = new GroupKickRequest();
		groupKickRequest.RequestId = m_messageDispatcher.GetRequestId();
		groupKickRequest.MemberName = memberName;
		SendRequestMessage(groupKickRequest, onResponseCallback);
	}

	public void UpdateFriend(string friendHandle, long friendAccountId, FriendOperation operation, string strData, Action<FriendUpdateResponse> onResponseCallback = null)
	{
		FriendUpdateRequest friendUpdateRequest = new FriendUpdateRequest();
		friendUpdateRequest.RequestId = m_messageDispatcher.GetRequestId();
		friendUpdateRequest.FriendHandle = friendHandle;
		friendUpdateRequest.FriendAccountId = friendAccountId;
		friendUpdateRequest.FriendOperation = operation;
		friendUpdateRequest.StringData = strData;
		SendRequestMessage(friendUpdateRequest, onResponseCallback);
	}

	public void UpdatePlayerStatus(string statusString, Action<PlayerUpdateStatusResponse> onResponseCallback = null)
	{
		PlayerUpdateStatusRequest playerUpdateStatusRequest = new PlayerUpdateStatusRequest();
		playerUpdateStatusRequest.RequestId = m_messageDispatcher.GetRequestId();
		playerUpdateStatusRequest.AccountId = SessionInfo.AccountId;
		playerUpdateStatusRequest.StatusString = statusString;
		SendRequestMessage(playerUpdateStatusRequest, onResponseCallback);
	}

	public void NotifyStoreOpened()
	{
		StoreOpenedMessage message = new StoreOpenedMessage();
		SendMessage(message);
	}

	public void NotifyCustomKeyBinds(Dictionary<int, KeyCodeData> CustomKeyBinds)
	{
		CustomKeyBindNotification customKeyBindNotification = new CustomKeyBindNotification();
		customKeyBindNotification.CustomKeyBinds = CustomKeyBinds;
		SendMessage(customKeyBindNotification);
	}

	public bool NotifyOptions(OptionsNotification notification)
	{
		notification.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(notification);
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		PaymentMethodsRequest paymentMethodsRequest = new PaymentMethodsRequest();
		paymentMethodsRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(paymentMethodsRequest, onResponseCallback);
	}

	public void RequestPrices(Action<PricesResponse> onResponseCallback = null)
	{
		PricesRequest pricesRequest = new PricesRequest();
		pricesRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(pricesRequest, onResponseCallback);
	}

	public void SendSteamMtxConfirm(bool authorized, ulong orderId)
	{
		SteamMtxConfirm steamMtxConfirm = new SteamMtxConfirm();
		steamMtxConfirm.authorized = authorized;
		steamMtxConfirm.orderId = orderId;
		SendMessage(steamMtxConfirm);
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
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
		PurchaseLootMatrixPackRequest purchaseLootMatrixPackRequest = new PurchaseLootMatrixPackRequest();
		purchaseLootMatrixPackRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseLootMatrixPackRequest.LootMatrixPackIndex = lootMatrixPackIndex;
		purchaseLootMatrixPackRequest.PaymentMethodId = paymentMethodId;
		purchaseLootMatrixPackRequest.AccountCurrency = m_ticket.AccountCurrency;
		SendRequestMessage(purchaseLootMatrixPackRequest, onResponseCallback);
	}

	public void PurchaseGame(int gamePackIndex, long paymentMethodId, Action<PurchaseGameResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
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
		PurchaseGameRequest purchaseGameRequest = new PurchaseGameRequest();
		purchaseGameRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseGameRequest.GamePackIndex = gamePackIndex;
		purchaseGameRequest.PaymentMethodId = paymentMethodId;
		purchaseGameRequest.AccountCurrency = m_ticket.AccountCurrency;
		SendRequestMessage(purchaseGameRequest, onResponseCallback);
	}

	public void PurchaseGGPack(int ggPackIndex, long paymentMethodId, Action<PurchaseGGPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
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
		PurchaseGGPackRequest purchaseGGPackRequest = new PurchaseGGPackRequest();
		purchaseGGPackRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseGGPackRequest.GGPackIndex = ggPackIndex;
		purchaseGGPackRequest.PaymentMethodId = paymentMethodId;
		purchaseGGPackRequest.AccountCurrency = m_ticket.AccountCurrency;
		SendRequestMessage(purchaseGGPackRequest, onResponseCallback);
	}

	public void PurchaseCharacter(CurrencyType currencyType, CharacterType characterType, Action<PurchaseCharacterResponse> onResponseCallback = null)
	{
		PurchaseCharacterRequest purchaseCharacterRequest = new PurchaseCharacterRequest();
		purchaseCharacterRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseCharacterRequest.CurrencyType = currencyType;
		purchaseCharacterRequest.CharacterType = characterType;
		SendRequestMessage(purchaseCharacterRequest, onResponseCallback);
	}

	public void PurchaseCharacterForCash(CharacterType characterType, long paymentMethodId, Action<PurchaseCharacterForCashResponse> onResponseCallback = null)
	{
		if (paymentMethodId != 0)
		{
			PurchaseCharacterForCashRequest purchaseCharacterForCashRequest = new PurchaseCharacterForCashRequest();
			purchaseCharacterForCashRequest.RequestId = m_messageDispatcher.GetRequestId();
			purchaseCharacterForCashRequest.CharacterType = characterType;
			purchaseCharacterForCashRequest.PaymentMethodId = paymentMethodId;
			purchaseCharacterForCashRequest.AccountCurrency = m_ticket.AccountCurrency;
			SendRequestMessage(purchaseCharacterForCashRequest, onResponseCallback);
		}
	}

	public void PurchaseSkin(CurrencyType currencyType, CharacterType characterType, int skinId, Action<PurchaseSkinResponse> onResponseCallback = null)
	{
		PurchaseSkinRequest purchaseSkinRequest = new PurchaseSkinRequest();
		purchaseSkinRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseSkinRequest.CurrencyType = currencyType;
		purchaseSkinRequest.CharacterType = characterType;
		purchaseSkinRequest.SkinId = skinId;
		SendRequestMessage(purchaseSkinRequest, onResponseCallback);
	}

	public void PurchaseTexture(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, Action<PurchaseTextureResponse> onResponseCallback = null)
	{
		PurchaseTextureRequest purchaseTextureRequest = new PurchaseTextureRequest();
		purchaseTextureRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseTextureRequest.CurrencyType = currencyType;
		purchaseTextureRequest.CharacterType = characterType;
		purchaseTextureRequest.SkinId = skinId;
		purchaseTextureRequest.TextureId = textureId;
		SendRequestMessage(purchaseTextureRequest, onResponseCallback);
	}

	public void PurchaseTint(CurrencyType currencyType, CharacterType characterType, int skinId, int textureId, int tintId, Action<PurchaseTintResponse> onResponseCallback = null)
	{
		PurchaseTintRequest purchaseTintRequest = new PurchaseTintRequest();
		purchaseTintRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseTintRequest.CurrencyType = currencyType;
		purchaseTintRequest.CharacterType = characterType;
		purchaseTintRequest.SkinId = skinId;
		purchaseTintRequest.TextureId = textureId;
		purchaseTintRequest.TintId = tintId;
		SendRequestMessage(purchaseTintRequest, onResponseCallback);
	}

	public void PurchaseTintForCash(CharacterType characterType, int skinId, int textureId, int tintId, long paymentMethodId, Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		PurchaseTintForCashRequest purchaseTintForCashRequest = new PurchaseTintForCashRequest();
		purchaseTintForCashRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseTintForCashRequest.PaymentMethodId = paymentMethodId;
		purchaseTintForCashRequest.CharacterType = characterType;
		purchaseTintForCashRequest.SkinId = skinId;
		purchaseTintForCashRequest.TextureId = textureId;
		purchaseTintForCashRequest.TintId = tintId;
		purchaseTintForCashRequest.AccountCurrency = m_ticket.AccountCurrency;
		SendRequestMessage(purchaseTintForCashRequest, onResponseCallback);
	}

	public void PurchaseStoreItemForCash(int inventoryItemId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		PurchaseStoreItemForCashRequest purchaseStoreItemForCashRequest = new PurchaseStoreItemForCashRequest();
		purchaseStoreItemForCashRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseStoreItemForCashRequest.PaymentMethodId = paymentMethodId;
		purchaseStoreItemForCashRequest.InventoryTemplateId = inventoryItemId;
		purchaseStoreItemForCashRequest.AccountCurrency = m_ticket.AccountCurrency;
		SendRequestMessage(purchaseStoreItemForCashRequest, onResponseCallback);
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntId, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		PurchaseTauntRequest purchaseTauntRequest = new PurchaseTauntRequest();
		purchaseTauntRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseTauntRequest.CurrencyType = currencyType;
		purchaseTauntRequest.CharacterType = characterType;
		purchaseTauntRequest.TauntId = tauntId;
		SendRequestMessage(purchaseTauntRequest, onResponseCallback);
	}

	public void PurchaseTitle(CurrencyType currencyType, int titleId, Action<PurchaseTitleResponse> onResponseCallback = null)
	{
		PurchaseTitleRequest purchaseTitleRequest = new PurchaseTitleRequest();
		purchaseTitleRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseTitleRequest.CurrencyType = currencyType;
		purchaseTitleRequest.TitleId = titleId;
		SendRequestMessage(purchaseTitleRequest, onResponseCallback);
	}

	public void PurchaseChatEmoji(CurrencyType currencyType, int emojiID, Action<PurchaseChatEmojiResponse> onResponseCallback = null)
	{
		PurchaseChatEmojiRequest purchaseChatEmojiRequest = new PurchaseChatEmojiRequest();
		purchaseChatEmojiRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseChatEmojiRequest.CurrencyType = currencyType;
		purchaseChatEmojiRequest.EmojiID = emojiID;
		SendRequestMessage(purchaseChatEmojiRequest, onResponseCallback);
	}

	public void PurchaseBannerBackground(CurrencyType currencyType, int bannerBackgroundId, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		PurchaseBannerBackgroundRequest purchaseBannerBackgroundRequest = new PurchaseBannerBackgroundRequest();
		purchaseBannerBackgroundRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseBannerBackgroundRequest.CurrencyType = currencyType;
		purchaseBannerBackgroundRequest.BannerBackgroundId = bannerBackgroundId;
		SendRequestMessage(purchaseBannerBackgroundRequest, onResponseCallback);
	}

	public void PurchaseBannerForeground(CurrencyType currencyType, int bannerForegroundId, Action<PurchaseBannerForegroundResponse> onResponseCallback = null)
	{
		PurchaseBannerForegroundRequest purchaseBannerForegroundRequest = new PurchaseBannerForegroundRequest();
		purchaseBannerForegroundRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseBannerForegroundRequest.CurrencyType = currencyType;
		purchaseBannerForegroundRequest.BannerForegroundId = bannerForegroundId;
		SendRequestMessage(purchaseBannerForegroundRequest, onResponseCallback);
	}

	public void PurchaseAbilityVfx(CharacterType charType, int abilityId, int vfxId, CurrencyType currencyType, Action<PurchaseAbilityVfxResponse> onResponseCallback = null)
	{
		PurchaseAbilityVfxRequest purchaseAbilityVfxRequest = new PurchaseAbilityVfxRequest();
		purchaseAbilityVfxRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseAbilityVfxRequest.CurrencyType = currencyType;
		purchaseAbilityVfxRequest.CharacterType = charType;
		purchaseAbilityVfxRequest.AbilityId = abilityId;
		purchaseAbilityVfxRequest.VfxId = vfxId;
		SendRequestMessage(purchaseAbilityVfxRequest, onResponseCallback);
	}

	public void PurchaseOvercon(int overconId, CurrencyType currencyType, Action<PurchaseOverconResponse> onResponseCallback = null)
	{
		PurchaseOverconRequest purchaseOverconRequest = new PurchaseOverconRequest();
		purchaseOverconRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseOverconRequest.CurrencyType = currencyType;
		purchaseOverconRequest.OverconId = overconId;
		SendRequestMessage(purchaseOverconRequest, onResponseCallback);
	}

	public void PurchaseLoadingScreenBackground(int loadingScreenBackgroundId, CurrencyType currencyType, Action<PurchaseLoadingScreenBackgroundResponse> onResponseCallback = null)
	{
		PurchaseLoadingScreenBackgroundRequest purchaseLoadingScreenBackgroundRequest = new PurchaseLoadingScreenBackgroundRequest();
		purchaseLoadingScreenBackgroundRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseLoadingScreenBackgroundRequest.CurrencyType = currencyType;
		purchaseLoadingScreenBackgroundRequest.LoadingScreenBackgroundId = loadingScreenBackgroundId;
		SendRequestMessage(purchaseLoadingScreenBackgroundRequest, onResponseCallback);
	}

	public void PlayerPanelUpdated(int _SelectedTitleID, int _SelectedForegroundBannerID, int _SelectedBackgroundBannerID, int _SelectedRibbonID)
	{
		PlayerPanelUpdatedNotification playerPanelUpdatedNotification = new PlayerPanelUpdatedNotification();
		playerPanelUpdatedNotification.originalSelectedTitleID = _SelectedTitleID;
		playerPanelUpdatedNotification.originalSelectedForegroundBannerID = _SelectedForegroundBannerID;
		playerPanelUpdatedNotification.originalSelectedBackgroundBannerID = _SelectedBackgroundBannerID;
		playerPanelUpdatedNotification.originalSelectedRibbonID = _SelectedRibbonID;
		SendMessage(playerPanelUpdatedNotification);
	}

	public void PurchaseInventoryItem(int inventoryItemID, CurrencyType currencyType, Action<PurchaseInventoryItemResponse> onResponseCallback = null)
	{
		PurchaseInventoryItemRequest purchaseInventoryItemRequest = new PurchaseInventoryItemRequest();
		purchaseInventoryItemRequest.RequestId = m_messageDispatcher.GetRequestId();
		purchaseInventoryItemRequest.CurrencyType = currencyType;
		purchaseInventoryItemRequest.InventoryItemID = inventoryItemID;
		SendRequestMessage(purchaseInventoryItemRequest, onResponseCallback);
	}

	public void CheckAccountStatus(Action<CheckAccountStatusResponse> onResponseCallback = null)
	{
		CheckAccountStatusRequest checkAccountStatusRequest = new CheckAccountStatusRequest();
		checkAccountStatusRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(checkAccountStatusRequest, onResponseCallback);
	}

	public void CheckRAFStatus(bool getReferralCode, Action<CheckRAFStatusResponse> onResponseCallback = null)
	{
		CheckRAFStatusRequest checkRAFStatusRequest = new CheckRAFStatusRequest();
		checkRAFStatusRequest.RequestId = m_messageDispatcher.GetRequestId();
		checkRAFStatusRequest.GetReferralCode = getReferralCode;
		SendRequestMessage(checkRAFStatusRequest, onResponseCallback);
	}

	public void SendRAFReferralEmails(List<string> emails, Action<SendRAFReferralEmailsResponse> onResponseCallback = null)
	{
		SendRAFReferralEmailsRequest sendRAFReferralEmailsRequest = new SendRAFReferralEmailsRequest();
		sendRAFReferralEmailsRequest.RequestId = m_messageDispatcher.GetRequestId();
		sendRAFReferralEmailsRequest.Emails = emails;
		SendRequestMessage(sendRAFReferralEmailsRequest, onResponseCallback);
	}

	public void SelectDailyQuest(int questId, Action<PickDailyQuestResponse> onResponseCallback = null)
	{
		PickDailyQuestRequest pickDailyQuestRequest = new PickDailyQuestRequest();
		pickDailyQuestRequest.RequestId = m_messageDispatcher.GetRequestId();
		pickDailyQuestRequest.questId = questId;
		SendRequestMessage(pickDailyQuestRequest, onResponseCallback);
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		AbandonDailyQuestRequest abandonDailyQuestRequest = new AbandonDailyQuestRequest();
		abandonDailyQuestRequest.RequestId = m_messageDispatcher.GetRequestId();
		abandonDailyQuestRequest.questId = questId;
		SendRequestMessage(abandonDailyQuestRequest, onResponseCallback);
	}

	public void ActivateQuestTrigger(QuestTriggerType triggerType, int activationCount, int questId, int questBonusCount, int itemTemplateId, CharacterType charType, Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		ActivateQuestTriggerRequest activateQuestTriggerRequest = new ActivateQuestTriggerRequest();
		activateQuestTriggerRequest.TriggerType = triggerType;
		activateQuestTriggerRequest.ActivationCount = activationCount;
		activateQuestTriggerRequest.QuestId = questId;
		activateQuestTriggerRequest.QuestBonusCount = questBonusCount;
		activateQuestTriggerRequest.ItemTemplateId = itemTemplateId;
		activateQuestTriggerRequest.charType = charType;
		activateQuestTriggerRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(activateQuestTriggerRequest, onResponseCallback);
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		BeginQuestRequest beginQuestRequest = new BeginQuestRequest();
		beginQuestRequest.QuestId = questId;
		beginQuestRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(beginQuestRequest, onResponseCallback);
	}

	public void CompleteQuest(int questId, Action<CompleteQuestResponse> onResponseCallback = null)
	{
		CompleteQuestRequest completeQuestRequest = new CompleteQuestRequest();
		completeQuestRequest.QuestId = questId;
		completeQuestRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(completeQuestRequest, onResponseCallback);
	}

	public void MarkTutorialSkipped(TutorialVersion progress, Action<MarkTutorialSkippedResponse> onResponseCallback = null)
	{
		MarkTutorialSkippedRequest markTutorialSkippedRequest = new MarkTutorialSkippedRequest();
		markTutorialSkippedRequest.RequestId = m_messageDispatcher.GetRequestId();
		markTutorialSkippedRequest.Progress = progress;
		SendRequestMessage(markTutorialSkippedRequest, onResponseCallback);
	}

	public void GetInventoryItems(Action<GetInventoryItemsResponse> onResponseCallback = null)
	{
		GetInventoryItemsRequest getInventoryItemsRequest = new GetInventoryItemsRequest();
		getInventoryItemsRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(getInventoryItemsRequest, onResponseCallback);
	}

	public void AddInventoryItems(List<InventoryItem> items, Action<AddInventoryItemsResponse> onResponseCallback = null)
	{
		AddInventoryItemsRequest addInventoryItemsRequest = new AddInventoryItemsRequest();
		addInventoryItemsRequest.RequestId = m_messageDispatcher.GetRequestId();
		addInventoryItemsRequest.Items = items;
		SendRequestMessage(addInventoryItemsRequest, onResponseCallback);
	}

	public void RemoveInventoryItems(List<InventoryItem> items, Action<RemoveInventoryItemsResponse> onResponseCallback = null)
	{
		RemoveInventoryItemsRequest removeInventoryItemsRequest = new RemoveInventoryItemsRequest();
		removeInventoryItemsRequest.RequestId = m_messageDispatcher.GetRequestId();
		removeInventoryItemsRequest.Items = items;
		SendRequestMessage(removeInventoryItemsRequest, onResponseCallback);
	}

	public void ConsumeInventoryItem(int itemId, int itemCount, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		ConsumeInventoryItemRequest consumeInventoryItemRequest = new ConsumeInventoryItemRequest();
		consumeInventoryItemRequest.RequestId = m_messageDispatcher.GetRequestId();
		consumeInventoryItemRequest.ItemId = itemId;
		consumeInventoryItemRequest.ItemCount = itemCount;
		consumeInventoryItemRequest.ToISO = toISO;
		SendRequestMessage(consumeInventoryItemRequest, onResponseCallback);
	}

	public void ConsumeInventoryItems(List<int> itemIds, bool toISO, Action<ConsumeInventoryItemsResponse> onResponseCallback = null)
	{
		ConsumeInventoryItemsRequest consumeInventoryItemsRequest = new ConsumeInventoryItemsRequest();
		consumeInventoryItemsRequest.RequestId = m_messageDispatcher.GetRequestId();
		consumeInventoryItemsRequest.ItemIds = itemIds;
		consumeInventoryItemsRequest.ToISO = toISO;
		SendRequestMessage(consumeInventoryItemsRequest, onResponseCallback);
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		SeasonQuestActionRequest seasonQuestActionRequest = new SeasonQuestActionRequest();
		seasonQuestActionRequest.RequestId = m_messageDispatcher.GetRequestId();
		seasonQuestActionRequest.Action = SeasonQuestActionRequest.ActionType._001D;
		seasonQuestActionRequest.SeasonId = seasonId;
		seasonQuestActionRequest.ChapterId = chapterId;
		SendRequestMessage(seasonQuestActionRequest, onResponseCallback);
	}

	public void SetSeasonQuest(int seasonId, int chapterId, int slotNum, int questId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		SeasonQuestActionRequest seasonQuestActionRequest = new SeasonQuestActionRequest();
		seasonQuestActionRequest.Action = SeasonQuestActionRequest.ActionType._000E;
		seasonQuestActionRequest.RequestId = m_messageDispatcher.GetRequestId();
		seasonQuestActionRequest.SeasonId = seasonId;
		seasonQuestActionRequest.ChapterId = chapterId;
		seasonQuestActionRequest.SlotNum = slotNum;
		seasonQuestActionRequest.QuestId = questId;
		SendRequestMessage(seasonQuestActionRequest, onResponseCallback);
	}

	public bool SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		PlayerCharacterFeedbackRequest playerCharacterFeedbackRequest = new PlayerCharacterFeedbackRequest();
		playerCharacterFeedbackRequest.RequestId = m_messageDispatcher.GetRequestId();
		playerCharacterFeedbackRequest.FeedbackData = feedbackData;
		return SendMessage(playerCharacterFeedbackRequest);
	}

	public void SendRejoinGameRequest(LobbyGameInfo previousGameInfo, bool accept, Action<RejoinGameResponse> onResponseCallback)
	{
		RejoinGameRequest rejoinGameRequest = new RejoinGameRequest();
		rejoinGameRequest.PreviousGameInfo = previousGameInfo;
		rejoinGameRequest.Accept = accept;
		SendRequestMessage(rejoinGameRequest, onResponseCallback);
	}

	public void SendDiscordGetRpcTokenRequest(Action<DiscordGetRpcTokenResponse> onResponseCallback = null)
	{
		DiscordGetRpcTokenRequest discordGetRpcTokenRequest = new DiscordGetRpcTokenRequest();
		discordGetRpcTokenRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(discordGetRpcTokenRequest, onResponseCallback);
	}

	public void SendDiscordGetAccessTokenRequest(string rpcCode, Action<DiscordGetAccessTokenResponse> onResponseCallback = null)
	{
		DiscordGetAccessTokenRequest discordGetAccessTokenRequest = new DiscordGetAccessTokenRequest();
		discordGetAccessTokenRequest.RequestId = m_messageDispatcher.GetRequestId();
		discordGetAccessTokenRequest.DiscordRpcCode = rpcCode;
		SendRequestMessage(discordGetAccessTokenRequest, onResponseCallback);
	}

	public void SendDiscordJoinServerRequest(ulong discordUserId, string discordUserAccessToken, DiscordJoinType joinType, Action<DiscordJoinServerResponse> onResponseCallback = null)
	{
		DiscordJoinServerRequest discordJoinServerRequest = new DiscordJoinServerRequest();
		discordJoinServerRequest.RequestId = m_messageDispatcher.GetRequestId();
		discordJoinServerRequest.DiscordUserId = discordUserId;
		discordJoinServerRequest.DiscordUserAccessToken = discordUserAccessToken;
		discordJoinServerRequest.JoinType = joinType;
		SendRequestMessage(discordJoinServerRequest, onResponseCallback);
	}

	public void SendDiscordLeaveServerRequest(DiscordJoinType joinType, Action<DiscordLeaveServerResponse> onResponseCallback = null)
	{
		DiscordLeaveServerRequest discordLeaveServerRequest = new DiscordLeaveServerRequest();
		discordLeaveServerRequest.JoinType = joinType;
		discordLeaveServerRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(discordLeaveServerRequest, onResponseCallback);
	}

	public void SendFacebookGetUserTokenRequest(Action<FacebookGetUserTokenResponse> onResponseCallback = null)
	{
		FacebookGetUserTokenRequest facebookGetUserTokenRequest = new FacebookGetUserTokenRequest();
		facebookGetUserTokenRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(facebookGetUserTokenRequest, onResponseCallback);
	}

	public void SendPreviousGameInfoRequest(Action<PreviousGameInfoResponse> onResponseCallback = null)
	{
		PreviousGameInfoRequest previousGameInfoRequest = new PreviousGameInfoRequest();
		previousGameInfoRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(previousGameInfoRequest, onResponseCallback);
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		ChatNotification chatNotification = new ChatNotification();
		chatNotification.RecipientHandle = recipientHandle;
		chatNotification.ConsoleMessageType = messageType;
		chatNotification.Text = text;
		chatNotification.EmojisAllowed = ChatEmojiManager.Get().GetAllEmojisInString(chatNotification.Text);
		return SendMessage(chatNotification);
	}

	public void SendSetDevTagRequest(bool active, Action<SetDevTagResponse> onResponseCallback = null)
	{
		SetDevTagRequest setDevTagRequest = new SetDevTagRequest();
		setDevTagRequest.RequestId = m_messageDispatcher.GetRequestId();
		setDevTagRequest.active = active;
		SendRequestMessage(setDevTagRequest, onResponseCallback);
	}

	public void SendUseOverconRequest(int overconId, string overconName, int actorId, int turn)
	{
		UseOverconRequest useOverconRequest = new UseOverconRequest();
		useOverconRequest.RequestId = m_messageDispatcher.GetRequestId();
		useOverconRequest.OverconId = overconId;
		useOverconRequest.OverconName = overconName;
		useOverconRequest.ActorId = actorId;
		useOverconRequest.Turn = turn;
		SendRequestMessage<UseOverconResponse>(useOverconRequest, HandleUseOverconResponse);
	}

	public bool SendUIActionNotification(string context)
	{
		UIActionNotification uIActionNotification = new UIActionNotification();
		uIActionNotification.Context = context;
		return SendMessage(uIActionNotification);
	}

	private void HandleGroupUpdateNotification(GroupUpdateNotification notification)
	{
		try
		{
			BlockSendingGroupUpdates = true;
			this.OnGroupUpdateNotification(notification);
		}
		finally
		{
			BlockSendingGroupUpdates = false;
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
		if (!(clientExceptionDetector != null))
		{
			return;
		}
		while (true)
		{
			if (clientExceptionDetector.GetClientErrorReport(request.CrashReportHash, out ClientErrorReport clientErrorReport))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						Log.Info("Informing lobby about error report {0}: {1}", request.CrashReportHash, clientErrorReport.LogString);
						SendMessage(new ErrorReportSummaryResponse
						{
							ClientErrorReport = clientErrorReport
						});
						return;
					}
				}
			}
			Log.Warning("Lobby asked us to describe error {0}, but we've never seen it!", request.CrashReportHash);
			return;
		}
	}

	private void HandlePendingPurchaseResult(PendingPurchaseResult resultMsg)
	{
		if (!(UIStorePanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIStorePanel.Get().HandlePendingPurchaseResult(resultMsg);
			return;
		}
	}

	private void _001D(DEBUG_ResetCompletedChaptersResponse _001D)
	{
		string empty = string.Empty;
		if (_001D.Success)
		{
			empty = "Cleared completed chapters list";
			Log.Info(empty);
		}
		else
		{
			empty = "Unable to reset completed chapters list";
			Log.Error(empty);
		}
		TextConsole.Get().Write(empty);
	}

	public void RequestToUseGGPack(Action<UseGGPackResponse> onResponseCallback = null)
	{
		UseGGPackRequest useGGPackRequest = new UseGGPackRequest();
		useGGPackRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(useGGPackRequest, onResponseCallback);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponseCallback = null)
	{
		UpdateRemoteCharacterRequest updateRemoteCharacterRequest = new UpdateRemoteCharacterRequest();
		updateRemoteCharacterRequest.RequestId = m_messageDispatcher.GetRequestId();
		updateRemoteCharacterRequest.Characters = characters;
		updateRemoteCharacterRequest.RemoteSlotIndexes = remoteSlotIndexes;
		SendRequestMessage(updateRemoteCharacterRequest, onResponseCallback);
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponseCallback = null)
	{
		SelectTitleRequest selectTitleRequest = new SelectTitleRequest();
		selectTitleRequest.RequestId = m_messageDispatcher.GetRequestId();
		selectTitleRequest.TitleID = newTitleID;
		SendRequestMessage(selectTitleRequest, onResponseCallback);
	}

	public void RequestBannerSelect(int newBannerID, Action<SelectBannerResponse> onResponseCallback = null)
	{
		SelectBannerRequest selectBannerRequest = new SelectBannerRequest();
		selectBannerRequest.RequestId = m_messageDispatcher.GetRequestId();
		selectBannerRequest.BannerID = newBannerID;
		SendRequestMessage(selectBannerRequest, onResponseCallback);
	}

	public void RequestLoadingScreenBackgroundToggle(int loadingScreenId, bool newState, Action<LoadingScreenToggleResponse> onResponseCallback = null)
	{
		LoadingScreenToggleRequest loadingScreenToggleRequest = new LoadingScreenToggleRequest();
		loadingScreenToggleRequest.RequestId = m_messageDispatcher.GetRequestId();
		loadingScreenToggleRequest.LoadingScreenId = loadingScreenId;
		loadingScreenToggleRequest.NewState = newState;
		SendRequestMessage(loadingScreenToggleRequest, onResponseCallback);
	}

	public void RequestRibbonSelect(int newRibbonID, Action<SelectRibbonResponse> onResponseCallback = null)
	{
		SelectRibbonRequest selectRibbonRequest = new SelectRibbonRequest();
		selectRibbonRequest.RequestId = m_messageDispatcher.GetRequestId();
		selectRibbonRequest.RibbonID = newRibbonID;
		SendRequestMessage(selectRibbonRequest, onResponseCallback);
	}

	public void RequestUpdateUIState(AccountComponent.UIStateIdentifier uiState, int stateValue, Action<UpdateUIStateResponse> onResponseCallback = null)
	{
		UpdateUIStateRequest updateUIStateRequest = new UpdateUIStateRequest();
		updateUIStateRequest.RequestId = m_messageDispatcher.GetRequestId();
		updateUIStateRequest.UIState = uiState;
		updateUIStateRequest.StateValue = stateValue;
		SendRequestMessage(updateUIStateRequest, onResponseCallback);
	}

	public void SetPushToTalkKey(int keyType, int keyCode, string keyName)
	{
		UpdatePushToTalkKeyRequest updatePushToTalkKeyRequest = new UpdatePushToTalkKeyRequest();
		updatePushToTalkKeyRequest.RequestId = m_messageDispatcher.GetRequestId();
		updatePushToTalkKeyRequest.KeyType = keyType;
		updatePushToTalkKeyRequest.KeyCode = keyCode;
		updatePushToTalkKeyRequest.KeyName = keyName;
		SendRequestMessage<UpdatePushToTalkKeyResponse>(updatePushToTalkKeyRequest, null);
	}

	public void SendRankedLeaderboardOverviewRequest(GameType gameType, Action<RankedLeaderboardOverviewResponse> onResponseCallback)
	{
		RankedLeaderboardOverviewRequest rankedLeaderboardOverviewRequest = new RankedLeaderboardOverviewRequest();
		rankedLeaderboardOverviewRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedLeaderboardOverviewRequest.GameType = gameType;
		SendRequestMessage(rankedLeaderboardOverviewRequest, onResponseCallback);
	}

	public void SendRankedLeaderboardOverviewRequest(GameType gameType, int groupSize, RankedLeaderboardSpecificRequest.RequestSpecificationType specification, Action<RankedLeaderboardSpecificResponse> onResponseCallback)
	{
		RankedLeaderboardSpecificRequest rankedLeaderboardSpecificRequest = new RankedLeaderboardSpecificRequest();
		rankedLeaderboardSpecificRequest.RequestId = m_messageDispatcher.GetRequestId();
		rankedLeaderboardSpecificRequest.GameType = gameType;
		rankedLeaderboardSpecificRequest.GroupSize = groupSize;
		rankedLeaderboardSpecificRequest.Specification = specification;
		SendRequestMessage(rankedLeaderboardSpecificRequest, onResponseCallback);
	}

	public void SetNewSessionLanguage(string languageCode)
	{
		OverrideSessionLanguageCodeNotification overrideSessionLanguageCodeNotification = new OverrideSessionLanguageCodeNotification();
		overrideSessionLanguageCodeNotification.RequestId = m_messageDispatcher.GetRequestId();
		overrideSessionLanguageCodeNotification.LanguageCode = languageCode;
		SendMessage(overrideSessionLanguageCodeNotification);
	}

	public void _001D(string _001D, string _000E)
	{
		DEBUG_AdminSlashCommandNotification dEBUG_AdminSlashCommandNotification = new DEBUG_AdminSlashCommandNotification();
		dEBUG_AdminSlashCommandNotification.Command = _001D;
		dEBUG_AdminSlashCommandNotification.Arguments = _000E;
		SendMessage(dEBUG_AdminSlashCommandNotification);
	}

	public void _001D(Action<DEBUG_ForceMatchmakingResponse> _001D)
	{
		DEBUG_ForceMatchmakingRequest dEBUG_ForceMatchmakingRequest = new DEBUG_ForceMatchmakingRequest();
		dEBUG_ForceMatchmakingRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(dEBUG_ForceMatchmakingRequest, _001D);
	}

	public void _001D(Action<DEBUG_TakeSnapshotResponse> _001D)
	{
		DEBUG_TakeSnapshotRequest dEBUG_TakeSnapshotRequest = new DEBUG_TakeSnapshotRequest();
		dEBUG_TakeSnapshotRequest.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(dEBUG_TakeSnapshotRequest, _001D);
	}
}
