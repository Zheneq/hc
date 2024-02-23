using System;
using System.Collections.Generic;
using System.Text;
using LobbyGameClientMessages;
using Steamworks;
using UnityEngine;
using WebSocketSharp;
using Random = System.Random;

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

	public bool IsConnected
	{
		get { return State == WebSocket.SocketState.Open && m_registered; }
	}

	public LobbySessionInfo SessionInfo
	{
		get { return m_sessionInfo; }
	}

	public event Action<RegisterGameClientResponse> OnConnected = delegate {};
	public event Action<string, bool, CloseStatusCode> OnDisconnected = delegate {};
	public event Action<LobbyServerReadyNotification> OnLobbyServerReadyNotification = delegate {};
	public event Action<LobbyStatusNotification> OnLobbyStatusNotification = delegate {};
	public event Action<LobbyGameplayOverridesNotification> OnLobbyGameplayOverridesNotification = delegate {};
	public event Action<LobbyCustomGamesNotification> OnLobbyCustomGamesNotification = delegate {};
	public event Action<MatchmakingQueueAssignmentNotification> OnQueueAssignmentNotification = delegate {};
	public event Action<MatchmakingQueueStatusNotification> OnQueueStatusNotification = delegate {};
	public event Action<GameAssignmentNotification> OnGameAssignmentNotification = delegate {};
	public event Action<GameInfoNotification> OnGameInfoNotification = delegate {};
	public event Action<GameStatusNotification> OnGameStatusNotification = delegate {};
	public event Action<PlayerAccountDataUpdateNotification> OnAccountDataUpdated = delegate {};
	public event Action<ForcedCharacterChangeFromServerNotification> OnForcedCharacterChangeFromServerNotification = delegate {};
	public event Action<PlayerCharacterDataUpdateNotification> OnCharacterDataUpdateNotification = delegate {};
	public event Action<InventoryComponentUpdateNotification> OnInventoryComponentUpdateNotification = delegate {};
	public event Action<BankBalanceChangeNotification> OnBankBalanceChangeNotification = delegate {};
	public event Action<SeasonStatusNotification> OnSeasonStatusNotification = delegate {};
	public event Action<ChapterStatusNotification> OnChapterStatusNotification = delegate {};
	public event Action<GroupUpdateNotification> OnGroupUpdateNotification = delegate {};
	public event Action<UseGGPackNotification> OnUseGGPackNotification = delegate {};
	public event Action<ChatNotification> OnChatNotification = delegate {};
	public event Action<UseOverconResponse> OnUseOverconNotification = delegate {};
	public event Action<FriendStatusNotification> OnFriendStatusNotification = delegate {};
	public event Action<GroupConfirmationRequest> OnGroupConfirmation = delegate {};
	public event Action<GroupSuggestionRequest> OnGroupSuggestion = delegate {};
	public event Action<ForceMatchmakingQueueNotification> OnForceQueueNotification = delegate {};
	public event Action<GameInviteConfirmationRequest> OnGameInviteConfirmationRequest = delegate {};
	public event Action<QuestCompleteNotification> OnQuestCompleteNotification = delegate {};
	public event Action<MatchResultsNotification> OnMatchResultsNotification = delegate {};
	public event Action<ServerQueueConfigurationUpdateNotification> OnServerQueueConfigurationUpdateNotification = delegate {};
	public event Action<RankedOverviewChangeNotification> OnRankedOverviewChangeNotification = delegate {};
	public event Action<FactionCompetitionNotification> OnFactionCompetitionNotification = delegate {};
	public event Action<TrustBoostUsedNotification> OnTrustBoostUsedNotification = delegate {};
	public event Action<FacebookAccessTokenNotification> OnFacebookAccessTokenNotification = delegate {};
	public event Action<PlayerFactionContributionChangeNotification> OnPlayerFactionContributionChange= delegate {};
	public event Action<FactionLoginRewardNotification> OnFactionLoginRewardNotification = delegate {};
	public event Action<LobbyAlertMissionDataNotification> OnLobbyAlertMissionDataNotification = delegate {};
	public event Action<LobbySeasonQuestDataNotification> OnLobbySeasonQuestDataNotification = delegate {};

	public LobbyGameClientInterface() : base(Factory.Get())
	{
		m_registered = false;
		m_sessionInfo = new LobbySessionInfo();
		ConnectionTimeout = 30f;
	}

	public void Initialize(
		string directoryServerAddress,
		AuthTicket ticket,
		Region region,
		string languageCode,
		ProcessType processType,
		int preferredLobbyServerIndex = 0)
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
		RegisterGameClientResponse registerResponse = new RegisterGameClientResponse
		{
			Success = false,
			ResponseId = requestId
		};
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
			AssignGameClientRequest assignGameClientRequest = new AssignGameClientRequest
			{
				RequestId = m_messageDispatcher.GetRequestId(),
				SessionInfo = m_sessionInfo,
				AuthInfo = m_ticket.AuthInfo,
				PreferredLobbyServerIndex = m_preferredLobbyServerIndex
			};
			if (directoryServerAddress.IndexOf("://") == -1)
			{
				directoryServerAddress = new StringBuilder().Append("ws://").Append(directoryServerAddress).ToString();
			}
			Uri uri = new Uri(directoryServerAddress);
			UriBuilder newDirectoryServerUri = new UriBuilder();
			int num = 6050;
			string str = "DirectorySessionManager";
			string scheme;
			switch (uri.Scheme)
			{
				case "ws":
				case "http":
					scheme = "http";
					break;
				case "wss":
				case "https":
					scheme = "https";
					break;
				default:
					scheme = newDirectoryServerUri.Scheme;
					break;
			}
			newDirectoryServerUri.Scheme = scheme;
			newDirectoryServerUri.Host = NetUtil.GetIPv4Address(uri.Host).ToString();
			newDirectoryServerUri.Port = uri.Port > 0 && !uri.IsDefaultPort ? uri.Port : num;
			newDirectoryServerUri.Path = uri.AbsolutePath != "/" ? uri.AbsolutePath : new StringBuilder().Append("/").Append(str).ToString();
			newDirectoryServerUri.Query = new StringBuilder().Append("messageType=").Append(assignGameClientRequest.GetType().Name).ToString();
			Logger.Info("Requesting lobby server assignment from {0}", newDirectoryServerUri);
			SendHttpRequest(newDirectoryServerUri.ToString(), assignGameClientRequest, delegate(AssignGameClientResponse assignResponse, Exception exception)
			{
				try
				{
					if (exception != null)
					{
						if (m_overallConnectionTimer.Elapsed.TotalSeconds < ConnectionTimeout)
						{
							Logger.Info("Re-requesting lobby server assignment from {0}: {1}", newDirectoryServerUri, exception.Message.Trim());
							Reconnect();
						}
						else
						{
							m_overallConnectionTimer.Reset();
							throw exception;
						}
					}
					if (!assignResponse.Success)
					{
						throw new ClientRequestFailed(assignResponse.ErrorMessage);
					}
					if (assignResponse.LobbyServerAddress.IsNullOrEmpty())
					{
						throw new ClientRequestFailed("Empty response from server");
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
		m_sessionInfo.IsBinary = IsBinary;
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
		OnForceQueueNotification(notification);
	}

	private void HandleQueueToPlayerNotification(MatchmakingQueueToPlayersNotification notification)
	{
		switch (notification.MessageToSend)
		{
			case MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._0012:
				AppState_GroupCharacterSelect.Get().ReEnter(true);
				UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true);
				return;
			case MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._0015:
			{
				string desc = StringUtil.TR("RuinedGameStartSoThrownOutOfQueue", "Global");
				UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), desc, StringUtil.TR("Ok", "Global"));
				Log.Info(new StringBuilder().Append("Updating ready state to false because ruined game, thrown out of queue, current Appstate: ").Append(AppState.GetCurrent()).ToString());
				AppState_GroupCharacterSelect.Get().UpdateReadyState(false);
				return;
			}
			case MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage._000E:
				ClientGameManager.Get().HandleQueueConfirmation();
				return;
			default:
			{
				string desc = StringUtil.TR("UnknownQueueManagerBug", "Global");
				UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), desc, StringUtil.TR("Ok", "Global"));
				break;
			}
		}
	}

	private void HandleServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		OnServerQueueConfigurationUpdateNotification(notification);
	}

	private void HandleGameDestroyedByPlayerNotification(GameDestroyedByPlayerNotification notification)
	{
		UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("QueuingNotification", "Global"), notification.LocalizedMessage.ToString(), StringUtil.TR("Ok", "Global"));
	}

	private void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		if (notification.SubPhase == FreelancerResolutionPhaseSubType.DUPLICATE_FREELANCER)
		{
			UIMatchStartPanel.Get().NotifyDuplicateFreelancer(true);
			return;
		}
		if (!notification.RankedData.HasValue)
		{
			return;
		}
		if (notification.SubPhase == FreelancerResolutionPhaseSubType.WAITING_FOR_ALL_PLAYERS)
		{
			return;
		}
		if (UIRankedModeDraftScreen.Get() == null)
		{
			return;
		}
		if (AppState_RankModeDraft.Get() != AppState.GetCurrent())
		{
			AppState_RankModeDraft.Get().Enter();
		}
		UIRankedModeDraftScreen.Get().HandleResolvingDuplicateFreelancerNotification(notification);
		if (!notification.SubPhase.IsPickBanSubPhase() && !notification.SubPhase.IsPickFreelancerSubPhase())
		{
			return;
		}
		RankedResolutionPhaseData value = notification.RankedData.Value;
		int ourPlayerId = UIRankedModeDraftScreen.Get().OurPlayerId;
		if (value._001D(ourPlayerId))
		{
			Random rnd = new Random();
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
			text = new StringBuilder().Append(unlocalizedFailure).Append("#NeedsLocalization").ToString();
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
		RankedTradeRequest rankedTradeRequest = new RankedTradeRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		rankedTradeRequest.Trade.DesiredCharacter = desiredCharacter;
		rankedTradeRequest.Trade.TradeAction = tradeAction;
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedTradeRequest, delegate(RankedTradeResponse response)
		{
			if (!response.Success)
			{
				WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedBanRequest(CharacterType selection)
	{
		RankedBanRequest rankedBanRequest = new RankedBanRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Selection = selection
		};
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedBanRequest, delegate(RankedBanResponse response)
		{
			if (!response.Success)
			{
				WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedSelectionRequest(CharacterType selection)
	{
		RankedSelectionRequest rankedSelectionRequest = new RankedSelectionRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Selection = selection
		};
		ClientGameManager.Get().LobbyInterface.SendRequestMessage(rankedSelectionRequest, delegate(RankedSelectionResponse response)
		{
			if (!response.Success)
			{
				WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		});
	}

	public void SendRankedHoverClickRequest(CharacterType selection)
	{
		RankedHoverClickRequest rankedHoverClickRequest = new RankedHoverClickRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Selection = selection
		};
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
		CharacterResourceLink characterResourceLink = notification.oldCharacterType != CharacterType.None
			? GameWideData.Get().GetCharacterResourceLink(notification.oldCharacterType)
			: null;
		string name = characterResourceLink != null
			? characterResourceLink.GetDisplayName()
			: StringUtil.TR("YourPreferredFreelancer", "Global");
		string title;
		string description;
		if (notification.ItsTooLateToChange)
		{
			title = StringUtil.TR("AutomaticSelection", "Global");
			description = LocalizationPayload.Create(
				"ForcedToPlayFreelancer", 
				"Global", 
				LocalizationArg_Freelancer.Create(notification.oldCharacterType),
				LocalizationArg_Freelancer.Create(notification.newCharacterType)
			).ToString();
		}
		else
		{
			title = StringUtil.TR("SelectNewFreelancer", "Global");
			description = notification.thiefName.IsNullOrEmpty()
				? string.Format(StringUtil.TR("AlreadyClaimedChooseNewFreelancer", "Global"), name)
				: string.Format(StringUtil.TR("AlreadyClaimedByTeammateChooseNewFreelancer", "Global"), name, notification.thiefName);
			UICharacterSelectScreen.Get().AllowCharacterSwapForConflict();
		}
		if (!notification.oldCharacterType.IsWillFill())
		{
			UIDialogPopupManager.OpenOneButtonDialog(title, description, StringUtil.TR("Ok", "Global"));
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
			Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
			OnDisconnected(m_lastLobbyErrorMessage, m_allowRelogin, notification.Code);
			m_lastLobbyErrorMessage = null;
			m_allowRelogin = true;
		}
		else if (m_overallConnectionTimer.IsRunning)
		{
			if (m_overallConnectionTimer.Elapsed.TotalSeconds < ConnectionTimeout)
			{
				Logger.Info("Retrying connection to {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
				Reconnect();
			}
			else
			{
				Logger.Info("Failed to connect to {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
				m_overallConnectionTimer.Reset();
				RegisterGameClientResponse registerGameClientResponse = new RegisterGameClientResponse
				{
					Success = false,
					LocalizedFailure = LocalizationPayload.Create("NetworkError", "Global")
				};
				OnConnected(registerGameClientResponse);
			}
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

	public bool SendRequestMessage<ResponseType>(WebSocketMessage request, Action<ResponseType> callback)
		where ResponseType : WebSocketResponseMessage, new()
	{
		return SendRequestMessage(request, callback, m_messageDispatcher);
	}

	public void UnregisterRequest<ResponseType>(int requestId) where ResponseType : WebSocketResponseMessage, new()
	{
		m_messageDispatcher.UnregisterMessageDelegate<ResponseType>(requestId);
	}

	private void RegisterGameClient()
	{
		RegisterGameClientRequest registerGameClientRequest = new RegisterGameClientRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			SessionInfo = m_sessionInfo,
			AuthInfo = m_ticket.AuthInfo,
			SteamUserId = SteamManager.UsingSteam ? SteamUser.GetSteamID().ToString() : "0",
			SystemInfo = new LobbyGameClientSystemInfo
			{
				GraphicsDeviceName = SystemInfo.graphicsDeviceName
			}
		};
		SendRequestMessage<RegisterGameClientResponse>(registerGameClientRequest, HandleRegisterGameClientResponse);
	}

	private void HandleRegisterGameClientResponse(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			Logger.Error("Failed to register game client with lobby server: {0}", response.ErrorMessage);
			m_registered = false;
			OnConnected(response);
			Disconnect();
			return;
		}
		Logger.Info("Registered game client with lobby server");
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
		if (response.AuthInfo != null && !string.IsNullOrEmpty(response.AuthInfo.Handle))
		{
			m_ticket.AuthInfo = response.AuthInfo;
		}
		OnConnected(response);
	}

	private void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		OnLobbyServerReadyNotification(notification);
		if (!notification.Success)
		{
			Logger.Error("Lobby server failed to become ready: {0}", notification.ErrorMessage);
			return;
		}
		Logger.Info("Lobby server is ready");
		if (notification.FriendStatus != null)
		{
			HandleFriendStatusNotification(notification.FriendStatus);
		}
		if (notification.ServerQueueConfiguration != null)
		{
			HandleServerQueueConfigurationUpdateNotification(notification.ServerQueueConfiguration);
		}
	}

	private void HandleLobbyStatusNotification(LobbyStatusNotification notification)
	{
		if (notification.LocalizedFailure != null)
		{
			Logger.Error("Error from lobby server: {0}", notification.LocalizedFailure.ToString());
			m_lastLobbyErrorMessage = notification.LocalizedFailure.ToString();
			m_allowRelogin = notification.AllowRelogin;
			Disconnect();
			return;
		}
		OnLobbyStatusNotification(notification);
	}

	private void HandleLobbyGameplayOverridesNotification(LobbyGameplayOverridesNotification notification)
	{
		OnLobbyGameplayOverridesNotification(notification);
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
		OnLobbyCustomGamesNotification(notification);
	}

	public void SetGameTypeSubMasks(GameType gameType, ushort subGameMask, Action<SetGameSubTypeResponse> onResponseCallback)
	{
		if (BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - SetGameTypeSubMasks \r\nCall Stack: {0}", "n/a");
			return;
		}
		SetGameSubTypeRequest setGameSubTypeRequest = new SetGameSubTypeRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			gameType = gameType,
			SubTypeMask = subGameMask
		};
		SendRequestMessage(setGameSubTypeRequest, onResponseCallback);
	}

	public void JoinQueue(GameType gameType, BotDifficulty allyBotDifficulty, BotDifficulty enemyBotDifficulty, Action<JoinMatchmakingQueueResponse> callback)
	{
		JoinMatchmakingQueueRequest joinMatchmakingQueueRequest = new JoinMatchmakingQueueRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameType = gameType,
			AllyBotDifficulty = allyBotDifficulty,
			EnemyBotDifficulty = enemyBotDifficulty
		};
		SendRequestMessage(joinMatchmakingQueueRequest, callback);
	}

	public void LeaveQueue(Action<LeaveMatchmakingQueueResponse> onResponseCallback)
	{
		LeaveMatchmakingQueueRequest leaveMatchmakingQueueRequest = new LeaveMatchmakingQueueRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(leaveMatchmakingQueueRequest, onResponseCallback);
	}

	public void UpdateQueueEnemyBotDifficulty(LobbyMatchmakingQueueInfo queueInfo, BotDifficulty enemyDifficulty)
	{
		if (BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateQueueEnemyBotDifficulty");
			return;
		}
		UpdateMatchmakingQueueRequest updateMatchmakingQueueRequest = new UpdateMatchmakingQueueRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			EnemyDifficulty = enemyDifficulty
		};
		SendMessage(updateMatchmakingQueueRequest);
	}

	public void CreateGame(
		LobbyGameConfig gameConfig,
		ReadyState readyState,
		string processCode,
		Action<CreateGameResponse> onResponseCallback,
		BotDifficulty botSkillTeamA = BotDifficulty.Easy,
		BotDifficulty botSkillTeamB = BotDifficulty.Easy)
	{
		CreateGameRequest createGameRequest = new CreateGameRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameConfig = gameConfig,
			ReadyState = readyState,
			ProcessCode = processCode,
			SelectedBotSkillTeamA = botSkillTeamA,
			SelectedBotSkillTeamB = botSkillTeamB
		};
		SendRequestMessage(createGameRequest, onResponseCallback);
	}

	public void JoinGame(LobbyGameInfo gameInfo, bool asSpectator, Action<JoinGameResponse> onResponseCallback)
	{
		JoinGameRequest joinGameRequest = new JoinGameRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameServerProcessCode = gameInfo.GameServerProcessCode,
			AsSpectator = asSpectator
		};
		SendRequestMessage(joinGameRequest, onResponseCallback);
	}

	public void LeaveGame(bool isPermanent, GameResult gameResult, Action<LeaveGameResponse> onResponseCallback)
	{
		LeaveGameRequest leaveGameRequest = new LeaveGameRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			IsPermanent = isPermanent,
			GameResult = gameResult
		};
		SendRequestMessage(leaveGameRequest, onResponseCallback);
	}

	public void CalculateFreelancerStats(
		PersistedStatBucket bucketType,
		CharacterType characterType,
		PersistedStats stats,
		MatchFreelancerStats matchFreelancerStats,
		Action<CalculateFreelancerStatsResponse> onResponseCallback)
	{
		CalculateFreelancerStatsRequest calculateFreelancerStatsRequest = new CalculateFreelancerStatsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			PersistedStatBucket = bucketType,
			CharacterType = characterType,
			PersistedStats = stats,
			MatchFreelancerStats = matchFreelancerStats
		};
		SendRequestMessage(calculateFreelancerStatsRequest, onResponseCallback);
	}

	private void HandleQueueAssignmentNotification(MatchmakingQueueAssignmentNotification notification)
	{
		OnQueueAssignmentNotification(notification);
	}

	private void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
		OnQueueStatusNotification(notification);
	}

	private void HandleGameAssignmentNotification(GameAssignmentNotification notification)
	{
		OnGameAssignmentNotification(notification);
	}

	public void Replay_RemoveFromGame()
	{
		HandleGameAssignmentNotification(new GameAssignmentNotification());
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		OnGameInfoNotification(notification);
	}

	private void HandleGameStatusNotification(GameStatusNotification notification)
	{
		OnGameStatusNotification(notification);
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
			Log.Error("Attempted to send a group update in response to one - bad! - UpdatePlayerInfo \r\nCall stack: {0}", "n/a");
			return 0;
		}
		PlayerInfoUpdateRequest playerInfoUpdateRequest = new PlayerInfoUpdateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			PlayerInfoUpdate = playerInfoUpdate,
			GameType = ClientGameManager.Get().GroupInfo.SelectedQueueType
		};
		SendRequestMessage(playerInfoUpdateRequest, onResponseCallback);
		return playerInfoUpdateRequest.RequestId;
	}

	public void UpdateGameCheats(GameOptionFlag gameOptionFlags, PlayerGameOptionFlag playerGameOptionFlags, Action<GameCheatUpdateResponse> onResponseCallback = null)
	{
		if (BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameCheats");
			return;
		}
		GameCheatUpdateRequest gameCheatUpdateRequest = new GameCheatUpdateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameOptionFlags = gameOptionFlags,
			PlayerGameOptionFlags = playerGameOptionFlags
		};
		SendRequestMessage(gameCheatUpdateRequest, onResponseCallback);
	}

	public void UpdateGroupGameType(GameType gameType, Action<PlayerGroupInfoUpdateResponse> onResponseCallback)
	{
		if (BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGroupGameType");
			return;
		}
		PlayerGroupInfoUpdateRequest playerGroupInfoUpdateRequest = new PlayerGroupInfoUpdateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameType = gameType
		};
		SendRequestMessage(playerGroupInfoUpdateRequest, onResponseCallback);
	}

	public void UpdateGameInfo(LobbyGameInfo gameInfo, LobbyTeamInfo teamInfo)
	{
		if (BlockSendingGroupUpdates)
		{
			Log.Error("Attempted to send a group update in response to one - bad! - UpdateGameInfo");
			return;
		}
		GameInfoUpdateRequest gameInfoUpdateRequest = new GameInfoUpdateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameInfo = gameInfo,
			TeamInfo = teamInfo
		};
		Action<GameInfoUpdateResponse> callback = delegate(GameInfoUpdateResponse response)
		{
			if (!response.Success)
			{
				WriteErrorToConsole(response.LocalizedFailure, response.ErrorMessage);
			}
		};
		SendRequestMessage(gameInfoUpdateRequest, callback);
	}

	public void InvitePlayerToGame(string playerHandle, Action<GameInvitationResponse> onResponseCallback)
	{
		GameInvitationRequest gameInvitationRequest = new GameInvitationRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			InviteeHandle = playerHandle
		};
		SendRequestMessage(gameInvitationRequest, onResponseCallback);
	}

	public void SpectateGame(string playerHandle, Action<GameSpectatorResponse> onResponseCallback)
	{
		GameSpectatorRequest gameSpectatorRequest = new GameSpectatorRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			InviteeHandle = playerHandle
		};
		SendRequestMessage(gameSpectatorRequest, onResponseCallback);
	}

	public bool RequestCrashReportArchiveName(int numArchiveBytes, Action<CrashReportArchiveNameResponse> onResponseCallback = null)
	{
		CrashReportArchiveNameRequest crashReportArchiveNameRequest = new CrashReportArchiveNameRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			NumArchiveBytes = numArchiveBytes
		};
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
		SetRegionRequest setRegionRequest = new SetRegionRequest
		{
			Region = region
		};
		return SendMessage(setRegionRequest);
	}

	private void HandleAccountDataUpdateNotification(PlayerAccountDataUpdateNotification notification)
	{
		OnAccountDataUpdated(notification);
	}

	private void HandleRequeueForceCharacterNotification(ForcedCharacterChangeFromServerNotification notification)
	{
		OnForcedCharacterChangeFromServerNotification(notification);
	}

	private void HandleCharacterDataUpdateNotification(PlayerCharacterDataUpdateNotification notification)
	{
		OnCharacterDataUpdateNotification(notification);
	}

	private void HandleInventoryComponentUpdateNotification(InventoryComponentUpdateNotification notification)
	{
		OnInventoryComponentUpdateNotification(notification);
	}

	private void HandleRankedOverviewChangeNotification(RankedOverviewChangeNotification notification)
	{
		OnRankedOverviewChangeNotification(notification);
	}

	public void GetPlayerMatchData(Action<PlayerMatchDataResponse> onResponseCallback = null)
	{
		PlayerMatchDataRequest playerMatchDataRequest = new PlayerMatchDataRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(playerMatchDataRequest, onResponseCallback);
	}

	public void PurchaseLoadoutSlot(CharacterType characterType, Action<PurchaseLoadoutSlotResponse> onResponseCallback)
	{
		PurchaseLoadoutSlotRequest purchaseLoadoutSlotRequest = new PurchaseLoadoutSlotRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Character = characterType
		};
		SendRequestMessage(purchaseLoadoutSlotRequest, onResponseCallback);
	}

	public void PurchaseMod(CharacterType character, int abilityId, int abilityModID, Action<PurchaseModResponse> onResponseCallback)
	{
		PurchaseModRequest purchaseModRequest = new PurchaseModRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Character = character
		};
		purchaseModRequest.UnlockData.AbilityId = abilityId;
		purchaseModRequest.UnlockData.AbilityModID = abilityModID;
		SendRequestMessage(purchaseModRequest, onResponseCallback);
	}

	public void PurchaseModToken(int numToPurchase, Action<PurchaseModTokenResponse> onResponseCallback)
	{
		PurchaseModTokenRequest purchaseModTokenRequest = new PurchaseModTokenRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			NumToPurchase = numToPurchase
		};
		SendRequestMessage(purchaseModTokenRequest, onResponseCallback);
	}

	public void RequestBalancedTeam(BalancedTeamRequest request, Action<BalancedTeamResponse> onResponseCallback)
	{
		request.RequestId = m_messageDispatcher.GetRequestId();
		SendRequestMessage(request, onResponseCallback);
	}

	public void RequestRefreshBankData(Action<RefreshBankDataResponse> onResponseCallback = null)
	{
		RefreshBankDataRequest refreshBankDataRequest = new RefreshBankDataRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(refreshBankDataRequest, onResponseCallback);
	}

	private void HandleBankBalanceChangeNotification(BankBalanceChangeNotification notification)
	{
		OnBankBalanceChangeNotification(notification);
	}

	private void HandleSeasonStatusNotification(SeasonStatusNotification notification)
	{
		OnSeasonStatusNotification(notification);
	}

	public bool SendSeasonStatusConfirmed(SeasonStatusConfirmed.DialogType dialogType)
	{
		SeasonStatusConfirmed seasonStatusConfirmed = new SeasonStatusConfirmed
		{
			Dialog = dialogType
		};
		return SendMessage(seasonStatusConfirmed);
	}

	private void HandleChapterStatusNotification(ChapterStatusNotification notification)
	{
		OnChapterStatusNotification(notification);
	}

	public void InviteToGroup(string friendHandle, Action<GroupInviteResponse> onResponseCallback)
	{
		GroupInviteRequest groupInviteRequest = new GroupInviteRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle
		};
		SendRequestMessage(groupInviteRequest, onResponseCallback);
	}

	public void RequestToJoinGroup(string friendHandle, Action<GroupJoinResponse> onResponseCallback)
	{
		GroupJoinRequest groupJoinRequest = new GroupJoinRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle
		};
		SendRequestMessage(groupJoinRequest, onResponseCallback);
	}

	public void PromoteWithinGroup(string name, Action<GroupPromoteResponse> onResponseCallback)
	{
		GroupPromoteRequest groupPromoteRequest = new GroupPromoteRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Name = name
		};
		SendRequestMessage(groupPromoteRequest, onResponseCallback);
	}

	public void ChatToGroup(string text, Action<GroupChatResponse> onResponseCallback)
	{
		GroupChatRequest groupChatRequest = new GroupChatRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Text = text,
			RequestedEmojis = ChatEmojiManager.Get().GetAllEmojisInString(text)
		};
		SendRequestMessage(groupChatRequest, onResponseCallback);
	}

	public void LeaveGroup(Action<GroupLeaveResponse> onResponseCallback)
	{
		GroupLeaveRequest groupLeaveRequest = new GroupLeaveRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(groupLeaveRequest, onResponseCallback);
	}

	public void KickFromGroup(string memberName, Action<GroupKickResponse> onResponseCallback)
	{
		GroupKickRequest groupKickRequest = new GroupKickRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			MemberName = memberName
		};
		SendRequestMessage(groupKickRequest, onResponseCallback);
	}

	public void UpdateFriend(
		string friendHandle,
		long friendAccountId,
		FriendOperation operation,
		string strData,
		Action<FriendUpdateResponse> onResponseCallback = null)
	{
		FriendUpdateRequest friendUpdateRequest = new FriendUpdateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			FriendHandle = friendHandle,
			FriendAccountId = friendAccountId,
			FriendOperation = operation,
			StringData = strData
		};
		SendRequestMessage(friendUpdateRequest, onResponseCallback);
	}

	public void UpdatePlayerStatus(string statusString, Action<PlayerUpdateStatusResponse> onResponseCallback = null)
	{
		PlayerUpdateStatusRequest playerUpdateStatusRequest = new PlayerUpdateStatusRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			AccountId = SessionInfo.AccountId,
			StatusString = statusString
		};
		SendRequestMessage(playerUpdateStatusRequest, onResponseCallback);
	}

	public void NotifyStoreOpened()
	{
		StoreOpenedMessage message = new StoreOpenedMessage();
		SendMessage(message);
	}

	public void NotifyCustomKeyBinds(Dictionary<int, KeyCodeData> CustomKeyBinds)
	{
		CustomKeyBindNotification customKeyBindNotification = new CustomKeyBindNotification
		{
			CustomKeyBinds = CustomKeyBinds
		};
		SendMessage(customKeyBindNotification);
	}

	public bool NotifyOptions(OptionsNotification notification)
	{
		notification.RequestId = m_messageDispatcher.GetRequestId();
		return SendMessage(notification);
	}

	public void RequestPaymentMethods(Action<PaymentMethodsResponse> onResponseCallback)
	{
		PaymentMethodsRequest paymentMethodsRequest = new PaymentMethodsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(paymentMethodsRequest, onResponseCallback);
	}

	public void RequestPrices(Action<PricesResponse> onResponseCallback = null)
	{
		PricesRequest pricesRequest = new PricesRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(pricesRequest, onResponseCallback);
	}

	public void SendSteamMtxConfirm(bool authorized, ulong orderId)
	{
		SteamMtxConfirm steamMtxConfirm = new SteamMtxConfirm
		{
			authorized = authorized,
			orderId = orderId
		};
		SendMessage(steamMtxConfirm);
	}

	public void PurchaseLootMatrixPack(int lootMatrixPackIndex, long paymentMethodId, Action<PurchaseLootMatrixPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
		{
			return;
		}
		PurchaseLootMatrixPackRequest purchaseLootMatrixPackRequest = new PurchaseLootMatrixPackRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			LootMatrixPackIndex = lootMatrixPackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseLootMatrixPackRequest, onResponseCallback);
	}

	public void PurchaseGame(int gamePackIndex, long paymentMethodId, Action<PurchaseGameResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
		{
			return;
		}
		PurchaseGameRequest purchaseGameRequest = new PurchaseGameRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GamePackIndex = gamePackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseGameRequest, onResponseCallback);
	}

	public void PurchaseGGPack(int ggPackIndex, long paymentMethodId, Action<PurchaseGGPackResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
		{
			return;
		}
		PurchaseGGPackRequest purchaseGGPackRequest = new PurchaseGGPackRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GGPackIndex = ggPackIndex,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseGGPackRequest, onResponseCallback);
	}

	public void PurchaseCharacter(CurrencyType currencyType, CharacterType characterType, Action<PurchaseCharacterResponse> onResponseCallback = null)
	{
		PurchaseCharacterRequest purchaseCharacterRequest = new PurchaseCharacterRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType
		};
		SendRequestMessage(purchaseCharacterRequest, onResponseCallback);
	}

	public void PurchaseCharacterForCash(CharacterType characterType, long paymentMethodId, Action<PurchaseCharacterForCashResponse> onResponseCallback = null)
	{
		if (paymentMethodId == 0)
		{
			return;
		}
		PurchaseCharacterForCashRequest purchaseCharacterForCashRequest = new PurchaseCharacterForCashRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CharacterType = characterType,
			PaymentMethodId = paymentMethodId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseCharacterForCashRequest, onResponseCallback);
	}

	public void PurchaseSkin(CurrencyType currencyType, CharacterType characterType, int skinId, Action<PurchaseSkinResponse> onResponseCallback = null)
	{
		PurchaseSkinRequest purchaseSkinRequest = new PurchaseSkinRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId
		};
		SendRequestMessage(purchaseSkinRequest, onResponseCallback);
	}

	public void PurchaseTexture(
		CurrencyType currencyType,
		CharacterType characterType,
		int skinId,
		int textureId,
		Action<PurchaseTextureResponse> onResponseCallback = null)
	{
		PurchaseTextureRequest purchaseTextureRequest = new PurchaseTextureRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId
		};
		SendRequestMessage(purchaseTextureRequest, onResponseCallback);
	}

	public void PurchaseTint(
		CurrencyType currencyType,
		CharacterType characterType,
		int skinId,
		int textureId,
		int tintId,
		Action<PurchaseTintResponse> onResponseCallback = null)
	{
		PurchaseTintRequest purchaseTintRequest = new PurchaseTintRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId,
			TintId = tintId
		};
		SendRequestMessage(purchaseTintRequest, onResponseCallback);
	}

	public void PurchaseTintForCash(
		CharacterType characterType,
		int skinId,
		int textureId,
		int tintId,
		long paymentMethodId,
		Action<PurchaseTintForCashResponse> onResponseCallback = null)
	{
		PurchaseTintForCashRequest purchaseTintForCashRequest = new PurchaseTintForCashRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			PaymentMethodId = paymentMethodId,
			CharacterType = characterType,
			SkinId = skinId,
			TextureId = textureId,
			TintId = tintId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseTintForCashRequest, onResponseCallback);
	}

	public void PurchaseStoreItemForCash(int inventoryItemId, long paymentMethodId, Action<PurchaseStoreItemForCashResponse> onResponseCallback = null)
	{
		PurchaseStoreItemForCashRequest purchaseStoreItemForCashRequest = new PurchaseStoreItemForCashRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			PaymentMethodId = paymentMethodId,
			InventoryTemplateId = inventoryItemId,
			AccountCurrency = m_ticket.AccountCurrency
		};
		SendRequestMessage(purchaseStoreItemForCashRequest, onResponseCallback);
	}

	public void PurchaseTaunt(CurrencyType currencyType, CharacterType characterType, int tauntId, Action<PurchaseTauntResponse> onResponseCallback = null)
	{
		PurchaseTauntRequest purchaseTauntRequest = new PurchaseTauntRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = characterType,
			TauntId = tauntId
		};
		SendRequestMessage(purchaseTauntRequest, onResponseCallback);
	}

	public void PurchaseTitle(CurrencyType currencyType, int titleId, Action<PurchaseTitleResponse> onResponseCallback = null)
	{
		PurchaseTitleRequest purchaseTitleRequest = new PurchaseTitleRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			TitleId = titleId
		};
		SendRequestMessage(purchaseTitleRequest, onResponseCallback);
	}

	public void PurchaseChatEmoji(CurrencyType currencyType, int emojiID, Action<PurchaseChatEmojiResponse> onResponseCallback = null)
	{
		PurchaseChatEmojiRequest purchaseChatEmojiRequest = new PurchaseChatEmojiRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			EmojiID = emojiID
		};
		SendRequestMessage(purchaseChatEmojiRequest, onResponseCallback);
	}

	public void PurchaseBannerBackground(CurrencyType currencyType, int bannerBackgroundId, Action<PurchaseBannerBackgroundResponse> onResponseCallback = null)
	{
		PurchaseBannerBackgroundRequest purchaseBannerBackgroundRequest = new PurchaseBannerBackgroundRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			BannerBackgroundId = bannerBackgroundId
		};
		SendRequestMessage(purchaseBannerBackgroundRequest, onResponseCallback);
	}

	public void PurchaseBannerForeground(CurrencyType currencyType, int bannerForegroundId, Action<PurchaseBannerForegroundResponse> onResponseCallback = null)
	{
		PurchaseBannerForegroundRequest purchaseBannerForegroundRequest = new PurchaseBannerForegroundRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			BannerForegroundId = bannerForegroundId
		};
		SendRequestMessage(purchaseBannerForegroundRequest, onResponseCallback);
	}

	public void PurchaseAbilityVfx(
		CharacterType charType,
		int abilityId,
		int vfxId,
		CurrencyType currencyType,
		Action<PurchaseAbilityVfxResponse> onResponseCallback = null)
	{
		PurchaseAbilityVfxRequest purchaseAbilityVfxRequest = new PurchaseAbilityVfxRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			CharacterType = charType,
			AbilityId = abilityId,
			VfxId = vfxId
		};
		SendRequestMessage(purchaseAbilityVfxRequest, onResponseCallback);
	}

	public void PurchaseOvercon(int overconId, CurrencyType currencyType, Action<PurchaseOverconResponse> onResponseCallback = null)
	{
		PurchaseOverconRequest purchaseOverconRequest = new PurchaseOverconRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			OverconId = overconId
		};
		SendRequestMessage(purchaseOverconRequest, onResponseCallback);
	}

	public void PurchaseLoadingScreenBackground(
		int loadingScreenBackgroundId,
		CurrencyType currencyType,
		Action<PurchaseLoadingScreenBackgroundResponse> onResponseCallback = null)
	{
		PurchaseLoadingScreenBackgroundRequest purchaseLoadingScreenBackgroundRequest = new PurchaseLoadingScreenBackgroundRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			LoadingScreenBackgroundId = loadingScreenBackgroundId
		};
		SendRequestMessage(purchaseLoadingScreenBackgroundRequest, onResponseCallback);
	}

	public void PlayerPanelUpdated(int _SelectedTitleID, int _SelectedForegroundBannerID, int _SelectedBackgroundBannerID, int _SelectedRibbonID)
	{
		PlayerPanelUpdatedNotification playerPanelUpdatedNotification = new PlayerPanelUpdatedNotification
		{
			originalSelectedTitleID = _SelectedTitleID,
			originalSelectedForegroundBannerID = _SelectedForegroundBannerID,
			originalSelectedBackgroundBannerID = _SelectedBackgroundBannerID,
			originalSelectedRibbonID = _SelectedRibbonID
		};
		SendMessage(playerPanelUpdatedNotification);
	}

	public void PurchaseInventoryItem(int inventoryItemID, CurrencyType currencyType, Action<PurchaseInventoryItemResponse> onResponseCallback = null)
	{
		PurchaseInventoryItemRequest purchaseInventoryItemRequest = new PurchaseInventoryItemRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			CurrencyType = currencyType,
			InventoryItemID = inventoryItemID
		};
		SendRequestMessage(purchaseInventoryItemRequest, onResponseCallback);
	}

	public void CheckAccountStatus(Action<CheckAccountStatusResponse> onResponseCallback = null)
	{
		CheckAccountStatusRequest checkAccountStatusRequest = new CheckAccountStatusRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(checkAccountStatusRequest, onResponseCallback);
	}

	public void CheckRAFStatus(bool getReferralCode, Action<CheckRAFStatusResponse> onResponseCallback = null)
	{
		CheckRAFStatusRequest checkRAFStatusRequest = new CheckRAFStatusRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GetReferralCode = getReferralCode
		};
		SendRequestMessage(checkRAFStatusRequest, onResponseCallback);
	}

	public void SendRAFReferralEmails(List<string> emails, Action<SendRAFReferralEmailsResponse> onResponseCallback = null)
	{
		SendRAFReferralEmailsRequest sendRAFReferralEmailsRequest = new SendRAFReferralEmailsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Emails = emails
		};
		SendRequestMessage(sendRAFReferralEmailsRequest, onResponseCallback);
	}

	public void SelectDailyQuest(int questId, Action<PickDailyQuestResponse> onResponseCallback = null)
	{
		PickDailyQuestRequest pickDailyQuestRequest = new PickDailyQuestRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			questId = questId
		};
		SendRequestMessage(pickDailyQuestRequest, onResponseCallback);
	}

	public void AbandonDailyQuest(int questId, Action<AbandonDailyQuestResponse> onResponseCallback = null)
	{
		AbandonDailyQuestRequest abandonDailyQuestRequest = new AbandonDailyQuestRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			questId = questId
		};
		SendRequestMessage(abandonDailyQuestRequest, onResponseCallback);
	}

	public void ActivateQuestTrigger(
		QuestTriggerType triggerType,
		int activationCount,
		int questId,
		int questBonusCount,
		int itemTemplateId,
		CharacterType charType,
		Action<ActivateQuestTriggerResponse> onResponseCallback = null)
	{
		ActivateQuestTriggerRequest activateQuestTriggerRequest = new ActivateQuestTriggerRequest
		{
			TriggerType = triggerType,
			ActivationCount = activationCount,
			QuestId = questId,
			QuestBonusCount = questBonusCount,
			ItemTemplateId = itemTemplateId,
			charType = charType,
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(activateQuestTriggerRequest, onResponseCallback);
	}

	public void BeginQuest(int questId, Action<BeginQuestResponse> onResponseCallback = null)
	{
		BeginQuestRequest beginQuestRequest = new BeginQuestRequest
		{
			QuestId = questId,
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(beginQuestRequest, onResponseCallback);
	}

	public void CompleteQuest(int questId, Action<CompleteQuestResponse> onResponseCallback = null)
	{
		CompleteQuestRequest completeQuestRequest = new CompleteQuestRequest
		{
			QuestId = questId,
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(completeQuestRequest, onResponseCallback);
	}

	public void MarkTutorialSkipped(TutorialVersion progress, Action<MarkTutorialSkippedResponse> onResponseCallback = null)
	{
		MarkTutorialSkippedRequest markTutorialSkippedRequest = new MarkTutorialSkippedRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Progress = progress
		};
		SendRequestMessage(markTutorialSkippedRequest, onResponseCallback);
	}

	public void GetInventoryItems(Action<GetInventoryItemsResponse> onResponseCallback = null)
	{
		GetInventoryItemsRequest getInventoryItemsRequest = new GetInventoryItemsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(getInventoryItemsRequest, onResponseCallback);
	}

	public void AddInventoryItems(List<InventoryItem> items, Action<AddInventoryItemsResponse> onResponseCallback = null)
	{
		AddInventoryItemsRequest addInventoryItemsRequest = new AddInventoryItemsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Items = items
		};
		SendRequestMessage(addInventoryItemsRequest, onResponseCallback);
	}

	public void RemoveInventoryItems(List<InventoryItem> items, Action<RemoveInventoryItemsResponse> onResponseCallback = null)
	{
		RemoveInventoryItemsRequest removeInventoryItemsRequest = new RemoveInventoryItemsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Items = items
		};
		SendRequestMessage(removeInventoryItemsRequest, onResponseCallback);
	}

	public void ConsumeInventoryItem(int itemId, int itemCount, bool toISO, Action<ConsumeInventoryItemResponse> onResponseCallback = null)
	{
		ConsumeInventoryItemRequest consumeInventoryItemRequest = new ConsumeInventoryItemRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			ItemId = itemId,
			ItemCount = itemCount,
			ToISO = toISO
		};
		SendRequestMessage(consumeInventoryItemRequest, onResponseCallback);
	}

	public void ConsumeInventoryItems(List<int> itemIds, bool toISO, Action<ConsumeInventoryItemsResponse> onResponseCallback = null)
	{
		ConsumeInventoryItemsRequest consumeInventoryItemsRequest = new ConsumeInventoryItemsRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			ItemIds = itemIds,
			ToISO = toISO
		};
		SendRequestMessage(consumeInventoryItemsRequest, onResponseCallback);
	}

	public void RerollSeasonQuests(int seasonId, int chapterId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		SeasonQuestActionRequest seasonQuestActionRequest = new SeasonQuestActionRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Action = SeasonQuestActionRequest.ActionType.RerollSeasonQuests,
			SeasonId = seasonId,
			ChapterId = chapterId
		};
		SendRequestMessage(seasonQuestActionRequest, onResponseCallback);
	}

	public void SetSeasonQuest(int seasonId, int chapterId, int slotNum, int questId, Action<SeasonQuestActionResponse> onResponseCallback = null)
	{
		SeasonQuestActionRequest seasonQuestActionRequest = new SeasonQuestActionRequest
		{
			Action = SeasonQuestActionRequest.ActionType.SetSeasonQuest,
			RequestId = m_messageDispatcher.GetRequestId(),
			SeasonId = seasonId,
			ChapterId = chapterId,
			SlotNum = slotNum,
			QuestId = questId
		};
		SendRequestMessage(seasonQuestActionRequest, onResponseCallback);
	}

	public bool SendPlayerCharacterFeedback(PlayerFeedbackData feedbackData)
	{
		PlayerCharacterFeedbackRequest playerCharacterFeedbackRequest = new PlayerCharacterFeedbackRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			FeedbackData = feedbackData
		};
		return SendMessage(playerCharacterFeedbackRequest);
	}

	public void SendRejoinGameRequest(LobbyGameInfo previousGameInfo, bool accept, Action<RejoinGameResponse> onResponseCallback)
	{
		RejoinGameRequest rejoinGameRequest = new RejoinGameRequest
		{
			PreviousGameInfo = previousGameInfo,
			Accept = accept
		};
		SendRequestMessage(rejoinGameRequest, onResponseCallback);
	}

	public void SendDiscordGetRpcTokenRequest(Action<DiscordGetRpcTokenResponse> onResponseCallback = null)
	{
		DiscordGetRpcTokenRequest discordGetRpcTokenRequest = new DiscordGetRpcTokenRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(discordGetRpcTokenRequest, onResponseCallback);
	}

	public void SendDiscordGetAccessTokenRequest(string rpcCode, Action<DiscordGetAccessTokenResponse> onResponseCallback = null)
	{
		DiscordGetAccessTokenRequest discordGetAccessTokenRequest = new DiscordGetAccessTokenRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			DiscordRpcCode = rpcCode
		};
		SendRequestMessage(discordGetAccessTokenRequest, onResponseCallback);
	}

	public void SendDiscordJoinServerRequest(ulong discordUserId, string discordUserAccessToken, DiscordJoinType joinType, Action<DiscordJoinServerResponse> onResponseCallback = null)
	{
		DiscordJoinServerRequest discordJoinServerRequest = new DiscordJoinServerRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			DiscordUserId = discordUserId,
			DiscordUserAccessToken = discordUserAccessToken,
			JoinType = joinType
		};
		SendRequestMessage(discordJoinServerRequest, onResponseCallback);
	}

	public void SendDiscordLeaveServerRequest(DiscordJoinType joinType, Action<DiscordLeaveServerResponse> onResponseCallback = null)
	{
		DiscordLeaveServerRequest discordLeaveServerRequest = new DiscordLeaveServerRequest
		{
			JoinType = joinType,
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(discordLeaveServerRequest, onResponseCallback);
	}

	public void SendFacebookGetUserTokenRequest(Action<FacebookGetUserTokenResponse> onResponseCallback = null)
	{
		FacebookGetUserTokenRequest facebookGetUserTokenRequest = new FacebookGetUserTokenRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(facebookGetUserTokenRequest, onResponseCallback);
	}

	public void SendPreviousGameInfoRequest(Action<PreviousGameInfoResponse> onResponseCallback = null)
	{
		PreviousGameInfoRequest previousGameInfoRequest = new PreviousGameInfoRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(previousGameInfoRequest, onResponseCallback);
	}

	public bool SendChatNotification(string recipientHandle, ConsoleMessageType messageType, string text)
	{
		ChatNotification chatNotification = new ChatNotification
		{
			RecipientHandle = recipientHandle,
			ConsoleMessageType = messageType,
			Text = text
		};
		chatNotification.EmojisAllowed = ChatEmojiManager.Get().GetAllEmojisInString(chatNotification.Text);
		return SendMessage(chatNotification);
	}

	public void SendSetDevTagRequest(bool active, Action<SetDevTagResponse> onResponseCallback = null)
	{
		SetDevTagRequest setDevTagRequest = new SetDevTagRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			active = active
		};
		SendRequestMessage(setDevTagRequest, onResponseCallback);
	}

	public void SendUseOverconRequest(int overconId, string overconName, int actorId, int turn)
	{
		UseOverconRequest useOverconRequest = new UseOverconRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			OverconId = overconId,
			OverconName = overconName,
			ActorId = actorId,
			Turn = turn
		};
		SendRequestMessage<UseOverconResponse>(useOverconRequest, HandleUseOverconResponse);
	}

	public bool SendUIActionNotification(string context)
	{
		UIActionNotification uIActionNotification = new UIActionNotification
		{
			Context = context
		};
		return SendMessage(uIActionNotification);
	}

	private void HandleGroupUpdateNotification(GroupUpdateNotification notification)
	{
		try
		{
			BlockSendingGroupUpdates = true;
			OnGroupUpdateNotification(notification);
		}
		finally
		{
			BlockSendingGroupUpdates = false;
		}
	}

	private void HandleGGPackUsedNotification(UseGGPackNotification notification)
	{
		OnUseGGPackNotification(notification);
	}

	private void HandleChatNotification(ChatNotification notification)
	{
		OnChatNotification(notification);
	}

	private void HandleUseOverconResponse(UseOverconResponse notification)
	{
		OnUseOverconNotification(notification);
	}

	private void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		OnFriendStatusNotification(notification);
	}

	private void HandleGroupConfirmationRequest(GroupConfirmationRequest request)
	{
		OnGroupConfirmation(request);
	}

	private void HandleGroupSuggestionRequest(GroupSuggestionRequest request)
	{
		OnGroupSuggestion(request);
	}

	private void HandleGameInviteConfirmationRequest(GameInviteConfirmationRequest request)
	{
		OnGameInviteConfirmationRequest(request);
	}

	private void HandleQuestCompleteNotification(QuestCompleteNotification request)
	{
		OnQuestCompleteNotification(request);
	}

	private void HandleMatchResultsNotification(MatchResultsNotification request)
	{
		OnMatchResultsNotification(request);
	}

	private void HandleFactionCompetitionNotification(FactionCompetitionNotification request)
	{
		OnFactionCompetitionNotification(request);
	}

	private void HandleTrustBoostUsedNotification(TrustBoostUsedNotification request)
	{
		OnTrustBoostUsedNotification(request);
	}

	private void HandleFacebookAccessTokenNotification(FacebookAccessTokenNotification request)
	{
		OnFacebookAccessTokenNotification(request);
	}

	private void HandleFactionLoginRewardNotification(FactionLoginRewardNotification notification)
	{
		OnFactionLoginRewardNotification(notification);
	}

	private void HandlePlayerFactionContributionChangeNotification(PlayerFactionContributionChangeNotification notification)
	{
		OnPlayerFactionContributionChange(notification);
	}

	private void HandleLobbyAlertMissionDataNotification(LobbyAlertMissionDataNotification notification)
	{
		OnLobbyAlertMissionDataNotification(notification);
	}

	private void HandleLobbySeasonQuestDataNotification(LobbySeasonQuestDataNotification notification)
	{
		OnLobbySeasonQuestDataNotification(notification);
	}

	private void HandleErrorReportSummaryRequest(ErrorReportSummaryRequest request)
	{
		ClientExceptionDetector clientExceptionDetector = ClientExceptionDetector.Get();
		if (clientExceptionDetector == null)
		{
			return;
		}
		if (!clientExceptionDetector.GetClientErrorReport(request.CrashReportHash, out ClientErrorReport clientErrorReport))
		{
			Log.Warning("Lobby asked us to describe error {0}, but we've never seen it!", request.CrashReportHash);
			return;
		}
		Log.Info("Informing lobby about error report {0}: {1}", request.CrashReportHash, clientErrorReport.LogString);
		SendMessage(new ErrorReportSummaryResponse
		{
			ClientErrorReport = clientErrorReport
		});
	}

	private void HandlePendingPurchaseResult(PendingPurchaseResult resultMsg)
	{
		if (UIStorePanel.Get() != null)
		{
			UIStorePanel.Get().HandlePendingPurchaseResult(resultMsg);
		}
	}

	private void DEBUG_HandleResetCompletedChaptersResponse(DEBUG_ResetCompletedChaptersResponse response)  // _001D
	{
		if (response.Success)
		{
			Log.Info("Cleared completed chapters list");
		}
		else
		{
			Log.Error("Unable to reset completed chapters list");
		}
		TextConsole.Get().Write(string.Empty);
	}

	public void RequestToUseGGPack(Action<UseGGPackResponse> onResponseCallback = null)
	{
		UseGGPackRequest useGGPackRequest = new UseGGPackRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(useGGPackRequest, onResponseCallback);
	}

	public void UpdateRemoteCharacter(CharacterType[] characters, int[] remoteSlotIndexes, Action<UpdateRemoteCharacterResponse> onResponseCallback = null)
	{
		UpdateRemoteCharacterRequest updateRemoteCharacterRequest = new UpdateRemoteCharacterRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			Characters = characters,
			RemoteSlotIndexes = remoteSlotIndexes
		};
		SendRequestMessage(updateRemoteCharacterRequest, onResponseCallback);
	}

	public void RequestTitleSelect(int newTitleID, Action<SelectTitleResponse> onResponseCallback = null)
	{
		SelectTitleRequest selectTitleRequest = new SelectTitleRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			TitleID = newTitleID
		};
		SendRequestMessage(selectTitleRequest, onResponseCallback);
	}

	public void RequestBannerSelect(int newBannerID, Action<SelectBannerResponse> onResponseCallback = null)
	{
		SelectBannerRequest selectBannerRequest = new SelectBannerRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			BannerID = newBannerID
		};
		SendRequestMessage(selectBannerRequest, onResponseCallback);
	}

	public void RequestLoadingScreenBackgroundToggle(int loadingScreenId, bool newState, Action<LoadingScreenToggleResponse> onResponseCallback = null)
	{
		LoadingScreenToggleRequest loadingScreenToggleRequest = new LoadingScreenToggleRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			LoadingScreenId = loadingScreenId,
			NewState = newState
		};
		SendRequestMessage(loadingScreenToggleRequest, onResponseCallback);
	}

	public void RequestRibbonSelect(int newRibbonID, Action<SelectRibbonResponse> onResponseCallback = null)
	{
		SelectRibbonRequest selectRibbonRequest = new SelectRibbonRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			RibbonID = newRibbonID
		};
		SendRequestMessage(selectRibbonRequest, onResponseCallback);
	}

	public void RequestUpdateUIState(AccountComponent.UIStateIdentifier uiState, int stateValue, Action<UpdateUIStateResponse> onResponseCallback = null)
	{
		UpdateUIStateRequest updateUIStateRequest = new UpdateUIStateRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			UIState = uiState,
			StateValue = stateValue
		};
		SendRequestMessage(updateUIStateRequest, onResponseCallback);
	}

	public void SetPushToTalkKey(int keyType, int keyCode, string keyName)
	{
		UpdatePushToTalkKeyRequest updatePushToTalkKeyRequest = new UpdatePushToTalkKeyRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			KeyType = keyType,
			KeyCode = keyCode,
			KeyName = keyName
		};
		SendRequestMessage<UpdatePushToTalkKeyResponse>(updatePushToTalkKeyRequest, null);
	}

	public void SendRankedLeaderboardOverviewRequest(GameType gameType, Action<RankedLeaderboardOverviewResponse> onResponseCallback)
	{
		RankedLeaderboardOverviewRequest rankedLeaderboardOverviewRequest = new RankedLeaderboardOverviewRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameType = gameType
		};
		SendRequestMessage(rankedLeaderboardOverviewRequest, onResponseCallback);
	}

	public void SendRankedLeaderboardOverviewRequest(
		GameType gameType,
		int groupSize,
		RankedLeaderboardSpecificRequest.RequestSpecificationType specification,
		Action<RankedLeaderboardSpecificResponse> onResponseCallback)
	{
		RankedLeaderboardSpecificRequest rankedLeaderboardSpecificRequest = new RankedLeaderboardSpecificRequest
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			GameType = gameType,
			GroupSize = groupSize,
			Specification = specification
		};
		SendRequestMessage(rankedLeaderboardSpecificRequest, onResponseCallback);
	}

	public void SetNewSessionLanguage(string languageCode)
	{
		SendMessage(new OverrideSessionLanguageCodeNotification
		{
			RequestId = m_messageDispatcher.GetRequestId(),
			LanguageCode = languageCode
		});
	}

	public void DEBUG_AdminSlashCommandExecuted(string command, string arguments) // _001D
	{
		SendMessage(new DEBUG_AdminSlashCommandNotification
		{
			Command = command,
			Arguments = arguments
		});
	}

	public void DEBUG_ForceMatchmaking(Action<DEBUG_ForceMatchmakingResponse> onResponseCallback) // _001D
	{
		DEBUG_ForceMatchmakingRequest forceMatchmakingRequest = new DEBUG_ForceMatchmakingRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(forceMatchmakingRequest, onResponseCallback);
	}

	public void DEBUG_TakeSnapshot(Action<DEBUG_TakeSnapshotResponse> onResponseCallback) // _001D
	{
		DEBUG_TakeSnapshotRequest takeSnapshotRequest = new DEBUG_TakeSnapshotRequest
		{
			RequestId = m_messageDispatcher.GetRequestId()
		};
		SendRequestMessage(takeSnapshotRequest, onResponseCallback);
	}
}
