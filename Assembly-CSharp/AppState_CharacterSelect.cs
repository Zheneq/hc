using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;

public class AppState_CharacterSelect : AppState
{
	private static AppState_CharacterSelect s_instance;

	public static AppState_CharacterSelect Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_CharacterSelect>();
	}

	private void Awake()
	{
		s_instance = this;
	}

	protected override void OnEnter()
	{
		AppState_GroupCharacterSelect.Get().NotifyEnteredCharacterSelect();
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		GameManager gameManager = GameManager.Get();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (gameManager.GameInfo == null)
		{
			throw new Exception("GameInfo must be set before entering app state");
		}
		if (gameManager.GameConfig == null)
		{
			throw new Exception("GameConfig must be set before entering app state");
		}
		if (gameManager.TeamInfo == null)
		{
			Log.Warning("Team Info is null");
		}
		if (gameManager.GameConfig.GameType != GameType.Tutorial
		    && !gameManager.GameConfig.HasGameOption(GameOptionFlag.AutoLaunch)
		    && (!hydrogenConfig.AutoLaunchGameType.IsAutoLaunchable() || !hydrogenConfig.CanAutoLaunchGame()))
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.CharacterSelect);
			CharacterType charType = CharacterType.None;
			if (gameManager.PlayerInfo != null)
			{
				charType = gameManager.PlayerInfo.CharacterType;
				if (!clientGameManager.GroupInfo.InAGroup)
				{
					clientGameManager.GroupInfo.SetCharacterInfo(gameManager.PlayerInfo.CharacterInfo);
				}
			}
			UICharacterSelectScreenController.Get().QuickPlaySetup(gameManager.GameInfo);
			UICharacterSelectScreen.Get().Setup(gameManager.GameConfig, gameManager.GameInfo, charType);
			UICharacterSelectScreenController.Get().UpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CustomGamePartyListVisible = gameManager.GameInfo.GameConfig.GameType == GameType.Custom
			});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		GameManager.Get().OnGameStopped += HandleGameStopped;
		GameManager.Get().OnGameLoadoutSelecting += HandleGameLoadoutSelecting;
		clientGameManager.OnGameInfoNotification += HandleGameInfoNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		RefreshUI();
		if (gameManager.GameConfig.GameType == GameType.Custom && UIPlayCategoryMenu.Get() != null)
		{
			UIManager.SetGameObjectActive(UIPlayCategoryMenu.Get(), false);
		}
		if (HighlightUtils.Get() != null)
		{
			HighlightUtils.Get().HideCursorHighlights();
		}
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameManager.Get().OnGameStopped -= HandleGameStopped;
		GameManager.Get().OnGameLoadoutSelecting -= HandleGameLoadoutSelecting;
		clientGameManager.OnGameInfoNotification -= HandleGameInfoNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().HandleNewSceneStateParameter(
				new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					CustomGamePartyListVisible = false
				});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		if (ClientGameManager.Get().GroupInfo != null && UICharacterSelectScreenController.Get() != null)
		{
			UIManager.SetGameObjectActive(
				UICharacterSelectScreenController.Get().m_miscCharSelectButtons,
				ClientGameManager.Get().GameTypeAvailabilies[ClientGameManager.Get().GroupInfo.SelectedQueueType].MaxWillFillPerTeam > 0);
		}
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			return;
		}
		if (!AssetBundleManager.Get().SceneExistsInBundle("maps", notification.GameInfo.GameConfig.Map)
		    && !AssetBundleManager.Get().SceneExistsInBundle("testing", notification.GameInfo.GameConfig.Map))
		{
			UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
			if (uICharacterSelectScreenController != null)
			{
				uICharacterSelectScreenController.OpenOneButtonDialog(
					string.Empty, 
					GameManager.Get().PlayerInfo.IsGameOwner
						? StringUtil.TR("NoAccessToThisMap", "Frontend")
						: StringUtil.TR("LeaderSelectedMapNoAccessTo", "Frontend"),
					delegate
					{
						AppState_GameTeardown.Get().Enter();
					});
			}
		}
		else
		{
			RefreshUI();
		}
		if (notification.GameInfo.GameConfig.GameType == GameType.Coop)
		{
			UIManager.Get().HandleNewSceneStateParameter(
				new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					SelectedEnemyBotDifficulty = (int)notification.GameInfo.SelectedBotSkillTeamB
				});
		}
	}

	public bool NeedForceUpdateCharacters(LobbyPlayerInfo playerInfo, List<LobbyPlayerInfo> teamPlayerInfos, LobbyGameplayOverrides gameplayOverrides)
	{
		bool result = false;
		if (GameManager.Get().GameConfig == null
		    || GameManager.Get().GameConfig.GameType != GameType.PvP
		    || playerInfo == null
		    || playerInfo.TeamId == Team.Spectator)
		{
			return false;
		}
		int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
		if (teamPlayerInfos == null)
		{
			return false;
		}
		int num2 = 1;
		foreach (LobbyPlayerInfo teamPlayerInfo in teamPlayerInfos)
		{
			if (teamPlayerInfo.CharacterType == CharacterType.None)
			{
				continue;
			}
			if (teamPlayerInfo.PlayerId == playerInfo.PlayerId)
			{
				if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(0) != playerInfo.CharacterInfo.CharacterType)
				{
					result = true;
				}
			}
			else
			{
				if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(num2) != teamPlayerInfo.CharacterInfo.CharacterType)
				{
					result = true;
				}

				num2++;
			}

			if (num2 >= num)
			{
				break;
			}
		}
		return result;
	}

	private void Update()
	{
		GameStatus gameStatus = GameManager.Get().GameStatus;
		if (IsReady()
		    && gameStatus >= GameStatus.Launched
		    && gameStatus != GameStatus.Stopped)
		{
			AppState_GameLoading.Get().Enter(GameManager.Get().GameInfo.GameConfig.GameType);
		}
		GameManager gameManager = GameManager.Get();
		if (gameManager != null
		    && NeedForceUpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides))
		{
			UICharacterSelectScreenController.Get().UpdateCharacters(
				gameManager.PlayerInfo,
				gameManager.TeamPlayerInfo,
				gameManager.GameplayOverrides);
			
		}
	}

	private void HandleGameLoadoutSelecting()
	{
		if (UICharacterSelectScreenController.Get() == null)
		{
			return;
		}
		CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_characterType;
		if (!characterType.IsWillFill())
		{
			if (GameManager.Get() == null || GameManager.Get().PlayerInfo.TeamId != Team.Spectator)
			{
				CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(characterType).CharacterComponent;
				bool isRanked = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
				bool areModsSelected = false;
				if (isRanked)
				{
					areModsSelected = characterComponent.LastRankedMods.ModForAbility0 > 0
					        || characterComponent.LastRankedMods.ModForAbility1 > 0
					        || characterComponent.LastRankedMods.ModForAbility2 > 0
					        || characterComponent.LastRankedMods.ModForAbility3 > 0
					        || characterComponent.LastRankedMods.ModForAbility4 > 0;
				}
				else
				{
					areModsSelected = characterComponent.LastMods.ModForAbility0 > 0
					        || characterComponent.LastMods.ModForAbility1 > 0
					        || characterComponent.LastMods.ModForAbility2 > 0
					        || characterComponent.LastMods.ModForAbility3 > 0
					        || characterComponent.LastMods.ModForAbility4 > 0;
				}

				if (!areModsSelected)
				{
					UICharacterSelectScreen.Get().ShowPleaseEquipModsDialog();
				}
			}
		}
		UICharacterSelectScreen.Get().SetGameSettingsButtonVisibility(false);
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		string text = null;
		switch (gameResult)
		{
			case GameResult.Requeued:
				break;
			case GameResult.OwnerLeft:
				text = StringUtil.TR("OwnerOfThisGameLeft", "Frontend");
				break;
			case GameResult.ClientKicked:
				text = StringUtil.TR("KickedFromThisGame", "Frontend");
				break;
			case GameResult.ServerCrashed:
				text = StringUtil.TR("FailedStartGameServer", "Frontend");
				break;
		}
		if (!text.IsNullOrEmpty() && UICharacterSelectScreenController.Get() != null)
		{
			UICharacterSelectScreenController.Get().OpenOneButtonDialog(string.Empty, text);
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}

	private void RefreshUI()
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
		LobbyTeamInfo teamInfo = gameManager.TeamInfo;
		if (gameInfo == null
		    || playerInfo == null
		    || teamInfo == null
		    || UIFrontEnd.Get() == null
		    || UICharacterSelectScreen.Get() == null
		    || gameManager.PlayerInfo.IsRemoteControlled)
		{
			return;
		}
		Team team = gameManager.PlayerInfo.TeamId;
		if (team == Team.Spectator)
		{
			team = Team.TeamA;
		}
		List<LobbyPlayerInfo> teamPlayerInfos = (
			from ti in gameManager.TeamInfo.TeamInfo(team)
			orderby ti.PlayerId != playerInfo.PlayerId ? 1 : 0
			select ti).ToList();
		List<LobbyPlayerInfo> list = gameManager.TeamInfo.TeamInfo(Team.Spectator).ToList();
		UICharacterSelectScreenController controller = UICharacterSelectScreenController.Get();
		controller.UpdateCharacters(
			gameManager.PlayerInfo,
			teamPlayerInfos,
			gameManager.GameplayOverrides);
		UIManager.SetGameObjectActive(controller.m_charSettingsPanel.m_skinsSubPanel.m_purchasePanel, false);
		UIManager.SetGameObjectActive(controller.m_charSettingsPanel.m_tauntsSubPanel.m_purchasePanel, false);
		UICharacterScreen.Get().m_partyListPanel.UpdateCharacterList(playerInfo, teamInfo, gameInfo);
		UICharacterSelectScreen.Get().m_simplePartyListPanel.UpdateCharacterList(playerInfo, teamInfo, gameInfo);
		UIGameSettingsPanel.Get().UpdateCharacterList(playerInfo, teamInfo, gameInfo);
		if (UIMatchStartPanel.Get() != null)
		{
			UIMatchStartPanel.Get().UpdateCharacterList();
		}
		if (UICharacterSelectScreen.Get().m_spectatorPartyListPanel != null)
		{
			if (list.Count > 0)
			{
				UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().m_spectatorPartyListPanel, true);
				UICharacterSelectScreen.Get().m_spectatorPartyListPanel.UpdateCharacterList(list);
			}
			else
			{
				UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().m_spectatorPartyListPanel, false);
			}
		}
		if (playerInfo.IsNPCBot)
		{
			if (playerInfo.TeamId == Team.TeamA)
			{
				if (gameInfo.GameConfig.GameType == GameType.Solo
				    || gameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
				{
					UIManager.Get().HandleNewSceneStateParameter(
						new UICharacterScreen.CharacterSelectSceneStateParameters
						{
							SelectedAllyBotDifficulty = (int)playerInfo.Difficulty
						});
				}
			}
			else if (playerInfo.TeamId == Team.TeamB)
			{
				if (gameInfo.GameConfig.GameType == GameType.Solo
				    || gameInfo.GameConfig.GameType == GameType.Coop)
				{
					UIManager.Get().HandleNewSceneStateParameter(
						new UICharacterScreen.CharacterSelectSceneStateParameters
						{
							SelectedEnemyBotDifficulty = (int)playerInfo.Difficulty
						});
				}
			}
		}
		UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, false);
	}

	public void UpdateSelectedSkin(CharacterVisualInfo selectedCharacterSkin)
	{
		ClientGameManager.Get().UpdateSelectedSkin(selectedCharacterSkin);
	}

	public void UpdateSelectedCards(CharacterCardInfo cards)
	{
		ClientGameManager.Get().UpdateSelectedCards(cards);
	}

	public void UpdateSelectedMods(CharacterModInfo mods)
	{
		ClientGameManager.Get().UpdateSelectedMods(mods);
	}

	public void UpdateSelectedAbilityVfxSwaps(CharacterAbilityVfxSwapInfo swaps)
	{
		ClientGameManager.Get().UpdateSelectedAbilityVfxSwaps(swaps);
	}

	public void UpdateReadyState(bool ready)
	{
		UpdateReadyState(
			ready,
			(BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay,
			(BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay);
	}

	public void UpdateReadyState(bool ready, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty)
	{
		ReadyState readyState = ready ? ReadyState.Ready : ReadyState.Accepted;
		ClientGameManager.Get().UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, HandlePlayerInfoUpdateResponse);
	}

	public static bool IsReady()
	{
		return ClientGameManager.Get() != null
		       && ClientGameManager.Get().PlayerInfo != null
		       && (ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator
		           || ClientGameManager.Get().PlayerInfo.IsReady);
	}

	public void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (response.Success)
		{
			if (response.PlayerInfo != null
			    && response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId
			    && UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get()
					.UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
			}
		}
		else if (response.ErrorMessage != null)
		{
			UIDialogPopupManager.OpenOneButtonDialog(
				string.Empty, 
				response.ErrorMessage,
				StringUtil.TR("Ok", "Global"));
		}
		else if (response.LocalizedFailure != null)
		{
			UIDialogPopupManager.OpenOneButtonDialog(
				string.Empty, 
				response.LocalizedFailure.ToString(),
				StringUtil.TR("Ok", "Global"));
		}
	}

	public void OnShowGameSettingsClicked()
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager != null
		    && gameManager.GameConfig != null
		    && gameManager.GameConfig.GameType == GameType.Custom
		    && gameManager.PlayerInfo != null
		    && UICharacterSelectScreen.Get() != null
		    && gameManager.PlayerInfo.IsGameOwner)
		{
			UICharacterSelectScreen.Get().ShowGameSettingsPanel(
				gameManager.GameConfig,
				gameManager.TeamInfo,
				gameManager.PlayerInfo);
		}
	}

	public void OnUpdateGameSettingsClicked(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, bool closeSettingsWindow = true)
	{
		LobbyGameInfo lobbyGameInfo = new LobbyGameInfo
		{
			GameConfig = gameConfig
		};
		ClientGameManager.Get().UpdateGameInfo(lobbyGameInfo, teamInfo);
		if (closeSettingsWindow)
		{
			UICharacterSelectScreen.Get().HideGameSettingsPanel(gameConfig);
		}
	}

	public void OnCancelGameSettingsClicked()
	{
		UICharacterSelectScreen.Get().HideGameSettingsPanel(GameManager.Get().GameConfig);
	}
}
