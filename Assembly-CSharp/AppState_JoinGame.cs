using System;
using LobbyGameClientMessages;

public class AppState_JoinGame : AppState
{
	private static AppState_JoinGame s_instance;

	private static bool s_joinPending;

	public static AppState_JoinGame Get()
	{
		return AppState_JoinGame.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_JoinGame>();
	}

	private void Awake()
	{
		AppState_JoinGame.s_instance = this;
	}

	protected override void OnEnter()
	{
		AppState_JoinGame.s_joinPending = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.SubscribeToCustomGames();
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.JoinGame, false);
	}

	protected override void OnLeave()
	{
		AppState_JoinGame.s_joinPending = false;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.UnsubscribeFromCustomGames();
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
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
		if (!AppState_JoinGame.s_joinPending)
		{
			if (!ClientGameManager.Get().IsCharacterAvailable(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter, gameInfo.GameConfig.GameType))
			{
				CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
				int i = 0;
				while (i < characterResourceLinks.Length)
				{
					CharacterResourceLink characterResourceLink = characterResourceLinks[i];
					if (characterResourceLink.m_characterType != CharacterType.None)
					{
						if (characterResourceLink.m_isHidden)
						{
						}
						else if (ClientGameManager.Get().IsCharacterAvailable(characterResourceLink.m_characterType, gameInfo.GameConfig.GameType))
						{
							ClientGameManager.Get().UpdateSelectedCharacter(characterResourceLink.m_characterType, 0);
							goto IL_CA;
						}
					}
					IL_B6:
					i++;
					continue;
					goto IL_B6;
				}
			}
			IL_CA:
			AppState_JoinGame.s_joinPending = true;
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (AppState_JoinGame.f__am_cache0 == null)
			{
				AppState_JoinGame.f__am_cache0 = delegate(JoinGameResponse response)
				{
					AppState_JoinGame.s_joinPending = false;
					if (response.Success)
					{
						AppState_CharacterSelect.Get().Enter();
					}
					else if (response.LocalizedFailure != null)
					{
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"), null, -1, false);
					}
					else
					{
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format("{0}#NeedsLocalization", response.ErrorMessage), StringUtil.TR("Ok", "Global"), null, -1, false);
					}
				};
			}
			clientGameManager.JoinGame(gameInfo, asSpectator, AppState_JoinGame.f__am_cache0);
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}
}
