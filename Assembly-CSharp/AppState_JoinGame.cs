using System.Text;
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
		if (!ClientGameManager.Get().IsCharacterAvailable(
			    ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter,
			    gameInfo.GameConfig.GameType))
		{
			foreach (CharacterResourceLink characterResourceLink in GameWideData.Get().m_characterResourceLinks)
			{
				if (characterResourceLink.m_characterType != CharacterType.None
				    && !characterResourceLink.m_isHidden
				    && ClientGameManager.Get().IsCharacterAvailable(characterResourceLink.m_characterType,gameInfo.GameConfig.GameType))
				{
					ClientGameManager.Get().UpdateSelectedCharacter(characterResourceLink.m_characterType);
					break;
				}
			}
		}
		s_joinPending = true;

		ClientGameManager.Get().JoinGame(gameInfo, asSpectator, delegate(JoinGameResponse response)
		{
			s_joinPending = false;
			if (response.Success)
			{
				AppState_CharacterSelect.Get().Enter();
			}
			else if (response.LocalizedFailure != null)
			{
				UIDialogPopupManager.OpenOneButtonDialog(
					string.Empty,
					response.LocalizedFailure.ToString(),
					StringUtil.TR("Ok", "Global"));
			}
			else
			{
				UIDialogPopupManager.OpenOneButtonDialog(
					string.Empty,
					new StringBuilder().Append(response.ErrorMessage).Append("#NeedsLocalization").ToString(),
					StringUtil.TR("Ok", "Global"));

			}
		});
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
