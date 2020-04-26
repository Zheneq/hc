using LobbyGameClientMessages;

public class AppState_CreateGame : AppState
{
	private static AppState_CreateGame s_instance;

	public static AppState_CreateGame Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_CreateGame>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.CreateGame);
		UIFrontEnd.Get().m_createGameScreen.Setup();
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
	}

	public void OnCreateClicked(LobbyGameConfig gameConfig)
	{
		BotDifficulty botDifficulty = BotDifficulty.Easy;
		BotDifficulty botDifficulty2 = BotDifficulty.Easy;
		if (gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
		{
			botDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		}
		if (gameConfig.GameType != GameType.Solo)
		{
			if (gameConfig.GameType != GameType.Coop)
			{
				goto IL_005f;
			}
		}
		botDifficulty2 = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		goto IL_005f;
		IL_005f:
		if (!ClientGameManager.Get().IsCharacterAvailable(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter, gameConfig.GameType))
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
						if (characterResourceLink.m_isHidden)
						{
						}
						else if (ClientGameManager.Get().IsCharacterAvailable(characterResourceLink.m_characterType, gameConfig.GameType))
						{
							ClientGameManager.Get().UpdateSelectedCharacter(characterResourceLink.m_characterType);
							break;
						}
					}
					num++;
					continue;
				}
				break;
			}
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		BotDifficulty selectedBotSkillTeamA = botDifficulty;
		BotDifficulty selectedBotSkillTeamB = botDifficulty2;
		
		clientGameManager.CreateGame(gameConfig, ReadyState.Accepted, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse response)
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
							AppState_CharacterSelect.Get().Enter();
							return;
						}
					}
				}
				object obj;
				if (response.LocalizedFailure == null)
				{
					obj = (response.ErrorMessage.IsNullOrEmpty() ? StringUtil.TR("UnknownErrorTryAgain", "Frontend") : $"{response.ErrorMessage}#NeedsLocalization");
				}
				else
				{
					obj = response.LocalizedFailure.ToString();
				}
				string description = (string)obj;
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"));
			});
	}

	public void OnCancelClicked()
	{
		AppState_JoinGame.Get().Enter();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
