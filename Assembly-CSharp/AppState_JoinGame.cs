using LobbyGameClientMessages;

public class AppState_JoinGame : AppState
{
	private static AppState_JoinGame s_instance;

	private static bool s_joinPending;

	public static AppState_JoinGame Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_JoinGame>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		s_joinPending = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.SubscribeToCustomGames();
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.JoinGame);
	}

	protected override void OnLeave()
	{
		s_joinPending = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.UnsubscribeFromCustomGames();
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
	}

	public void OnCancelClicked()
	{
		UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
	}

	public void OnCreateClicked()
	{
		AppState_CreateGame.Get().Enter();
	}

	public void OnJoinClicked(LobbyGameInfo gameInfo, bool asSpectator)
	{
		if (s_joinPending)
		{
			return;
		}
		if (!ClientGameManager.Get().IsCharacterAvailable(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter, gameInfo.GameConfig.GameType))
		{
			CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
			int num = 0;
			while (true)
			{
				if (num < characterResourceLinks.Length)
				{
					CharacterResourceLink characterResourceLink = characterResourceLinks[num];
					if (characterResourceLink.m_characterType != 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (characterResourceLink.m_isHidden)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else if (ClientGameManager.Get().IsCharacterAvailable(characterResourceLink.m_characterType, gameInfo.GameConfig.GameType))
						{
							ClientGameManager.Get().UpdateSelectedCharacter(characterResourceLink.m_characterType);
							break;
						}
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		s_joinPending = true;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache0 = delegate(JoinGameResponse response)
			{
				s_joinPending = false;
				if (response.Success)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							AppState_CharacterSelect.Get().Enter();
							return;
						}
					}
				}
				if (response.LocalizedFailure != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"));
							return;
						}
					}
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, $"{response.ErrorMessage}#NeedsLocalization", StringUtil.TR("Ok", "Global"));
			};
		}
		clientGameManager.JoinGame(gameInfo, asSpectator, _003C_003Ef__am_0024cache0);
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
