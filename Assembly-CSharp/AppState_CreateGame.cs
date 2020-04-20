using System;
using LobbyGameClientMessages;

public class AppState_CreateGame : AppState
{
	private static AppState_CreateGame s_instance;

	public static AppState_CreateGame Get()
	{
		return AppState_CreateGame.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_CreateGame>();
	}

	private void Awake()
	{
		AppState_CreateGame.s_instance = this;
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.CreateGame, false);
		UIFrontEnd.Get().m_createGameScreen.Setup();
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
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
				goto IL_5F;
			}
		}
		botDifficulty2 = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		IL_5F:
		if (!ClientGameManager.Get().IsCharacterAvailable(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter, gameConfig.GameType))
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
					else if (ClientGameManager.Get().IsCharacterAvailable(characterResourceLink.m_characterType, gameConfig.GameType))
					{
						ClientGameManager.Get().UpdateSelectedCharacter(characterResourceLink.m_characterType, 0);
						goto IL_11F;
					}
				}
				IL_108:
				i++;
				continue;
				goto IL_108;
			}
		}
		IL_11F:
		ClientGameManager clientGameManager = ClientGameManager.Get();
		ReadyState readyState = ReadyState.Accepted;
		BotDifficulty selectedBotSkillTeamA = botDifficulty;
		BotDifficulty selectedBotSkillTeamB = botDifficulty2;
		if (AppState_CreateGame.f__am_cache0 == null)
		{
			AppState_CreateGame.f__am_cache0 = delegate(CreateGameResponse response)
			{
				if (response.Success)
				{
					AppState_CharacterSelect.Get().Enter();
				}
				else
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
					string description = text;
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"), null, -1, false);
				}
			};
		}
		clientGameManager.CreateGame(gameConfig, readyState, selectedBotSkillTeamA, selectedBotSkillTeamB, AppState_CreateGame.f__am_cache0);
	}

	public void OnCancelClicked()
	{
		AppState_JoinGame.Get().Enter();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}
}
