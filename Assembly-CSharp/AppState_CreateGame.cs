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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_CreateGame.OnCreateClicked(LobbyGameConfig)).MethodHandle;
			}
			botDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		}
		if (gameConfig.GameType != GameType.Solo)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (gameConfig.GameType != GameType.Coop)
			{
				goto IL_5F;
			}
		}
		botDifficulty2 = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		IL_5F:
		if (!ClientGameManager.Get().IsCharacterAvailable(ClientGameManager.Get().GetPlayerAccountData().AccountComponent.LastCharacter, gameConfig.GameType))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
			int i = 0;
			while (i < characterResourceLinks.Length)
			{
				CharacterResourceLink characterResourceLink = characterResourceLinks[i];
				if (characterResourceLink.m_characterType != CharacterType.None)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (characterResourceLink.m_isHidden)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_11F:
		ClientGameManager clientGameManager = ClientGameManager.Get();
		ReadyState readyState = ReadyState.Accepted;
		BotDifficulty selectedBotSkillTeamA = botDifficulty;
		BotDifficulty selectedBotSkillTeamB = botDifficulty2;
		if (AppState_CreateGame.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			AppState_CreateGame.<>f__am$cache0 = delegate(CreateGameResponse response)
			{
				if (response.Success)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_CreateGame.<OnCreateClicked>m__0(CreateGameResponse)).MethodHandle;
					}
					AppState_CharacterSelect.Get().Enter();
				}
				else
				{
					string text;
					if (response.LocalizedFailure != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
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
		clientGameManager.CreateGame(gameConfig, readyState, selectedBotSkillTeamA, selectedBotSkillTeamB, AppState_CreateGame.<>f__am$cache0);
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
