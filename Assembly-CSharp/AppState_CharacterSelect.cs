using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;

public class AppState_CharacterSelect : AppState
{
	private static AppState_CharacterSelect s_instance;

	public static AppState_CharacterSelect Get()
	{
		return AppState_CharacterSelect.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_CharacterSelect>();
	}

	private void Awake()
	{
		AppState_CharacterSelect.s_instance = this;
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
			Log.Warning("Team Info is null", new object[0]);
		}
		bool flag = true;
		if (gameManager.GameConfig.GameType != GameType.Tutorial)
		{
			if (!gameManager.GameConfig.HasGameOption(GameOptionFlag.AutoLaunch))
			{
				if (!hydrogenConfig.AutoLaunchGameType.IsAutoLaunchable())
				{
					goto IL_E6;
				}
				if (!hydrogenConfig.CanAutoLaunchGame())
				{
					goto IL_E6;
				}
			}
		}
		flag = false;
		IL_E6:
		if (flag)
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.CharacterSelect, false);
			CharacterType charType = CharacterType.None;
			if (gameManager.PlayerInfo != null)
			{
				charType = gameManager.PlayerInfo.CharacterType;
				if (!clientGameManager.GroupInfo.InAGroup)
				{
					clientGameManager.GroupInfo.SetCharacterInfo(gameManager.PlayerInfo.CharacterInfo, false);
				}
			}
			UICharacterSelectScreenController.Get().QuickPlaySetup(gameManager.GameInfo);
			UICharacterSelectScreen.Get().Setup(gameManager.GameConfig, gameManager.GameInfo, charType);
			UICharacterSelectScreenController.Get().UpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CustomGamePartyListVisible = new bool?(gameManager.GameInfo.GameConfig.GameType == GameType.Custom)
			});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		GameManager.Get().OnGameStopped += this.HandleGameStopped;
		GameManager.Get().OnGameLoadoutSelecting += this.HandleGameLoadoutSelecting;
		clientGameManager.OnGameInfoNotification += this.HandleGameInfoNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		this.RefreshUI();
		if (gameManager.GameConfig.GameType == GameType.Custom)
		{
			if (UIPlayCategoryMenu.Get() != null)
			{
				UIManager.SetGameObjectActive(UIPlayCategoryMenu.Get(), false, null);
			}
		}
		if (HighlightUtils.Get() != null)
		{
			HighlightUtils.Get().HideCursorHighlights();
		}
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameManager.Get().OnGameStopped -= this.HandleGameStopped;
		GameManager.Get().OnGameLoadoutSelecting -= this.HandleGameLoadoutSelecting;
		clientGameManager.OnGameInfoNotification -= this.HandleGameInfoNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		if (UICharacterScreen.Get() != null)
		{
			UICharacterScreen.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CustomGamePartyListVisible = new bool?(false)
			});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (UICharacterSelectScreenController.Get() != null)
			{
				UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, ClientGameManager.Get().GameTypeAvailabilies[ClientGameManager.Get().GroupInfo.SelectedQueueType].MaxWillFillPerTeam > 0, null);
			}
		}
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo.GameStatus != GameStatus.Stopped)
		{
			if (!AssetBundleManager.Get().SceneExistsInBundle("maps", notification.GameInfo.GameConfig.Map))
			{
				if (!AssetBundleManager.Get().SceneExistsInBundle("testing", notification.GameInfo.GameConfig.Map))
				{
					UICharacterSelectScreenController uicharacterSelectScreenController = UICharacterSelectScreenController.Get();
					if (uicharacterSelectScreenController != null)
					{
						string text = (!GameManager.Get().PlayerInfo.IsGameOwner) ? StringUtil.TR("LeaderSelectedMapNoAccessTo", "Frontend") : StringUtil.TR("NoAccessToThisMap", "Frontend");
						UICharacterSelectScreenController uicharacterSelectScreenController2 = uicharacterSelectScreenController;
						string empty = string.Empty;
						string message = text;
						
						uicharacterSelectScreenController2.OpenOneButtonDialog(empty, message, delegate(UIDialogBox UIDialogBox)
							{
								AppState_GameTeardown.Get().Enter();
							});
					}
					goto IL_11C;
				}
			}
			this.RefreshUI();
			IL_11C:
			if (notification.GameInfo.GameConfig.GameType == GameType.Coop)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					SelectedEnemyBotDifficulty = new int?((int)notification.GameInfo.SelectedBotSkillTeamB)
				});
			}
		}
	}

	public bool NeedForceUpdateCharacters(LobbyPlayerInfo playerInfo, List<LobbyPlayerInfo> teamPlayerInfos, LobbyGameplayOverrides gameplayOverrides)
	{
		bool result = false;
		if (GameManager.Get().GameConfig != null && GameManager.Get().GameConfig.GameType == GameType.PvP)
		{
			if (playerInfo != null)
			{
				if (playerInfo.TeamId != Team.Spectator)
				{
					int num = UICharacterSelectWorldObjects.Get().m_ringAnimations.Length;
					if (teamPlayerInfos != null)
					{
						int num2 = 1;
						foreach (LobbyPlayerInfo lobbyPlayerInfo in teamPlayerInfos)
						{
							if (lobbyPlayerInfo.CharacterType == CharacterType.None)
							{
							}
							else
							{
								bool flag = lobbyPlayerInfo.PlayerId == playerInfo.PlayerId;
								if (flag)
								{
									if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(0) != playerInfo.CharacterInfo.CharacterType)
									{
										result = true;
									}
								}
								else
								{
									if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(num2) != lobbyPlayerInfo.CharacterInfo.CharacterType)
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
						}
					}
					return result;
				}
			}
		}
		return result;
	}

	private void Update()
	{
		GameStatus gameStatus = GameManager.Get().GameStatus;
		if (AppState_CharacterSelect.IsReady())
		{
			if (gameStatus >= GameStatus.Launched)
			{
				if (gameStatus != GameStatus.Stopped)
				{
					AppState_GameLoading.Get().Enter(GameManager.Get().GameInfo.GameConfig.GameType);
				}
			}
		}
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && this.NeedForceUpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides))
		{
			UICharacterSelectScreenController.Get().UpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides);
		}
	}

	private void HandleGameLoadoutSelecting()
	{
		if (UICharacterSelectScreenController.Get() != null)
		{
			CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_characterType;
			if (!characterType.IsWillFill())
			{
				if (!(GameManager.Get() == null))
				{
					if (GameManager.Get().PlayerInfo.TeamId == Team.Spectator)
					{
						goto IL_1AC;
					}
				}
				CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(characterType).CharacterComponent;
				bool flag = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
				bool flag3;
				if (flag)
				{
					bool flag2;
					if (characterComponent.LastRankedMods.ModForAbility0 <= 0)
					{
						if (characterComponent.LastRankedMods.ModForAbility1 <= 0)
						{
							if (characterComponent.LastRankedMods.ModForAbility2 <= 0 && characterComponent.LastRankedMods.ModForAbility3 <= 0)
							{
								flag2 = (characterComponent.LastRankedMods.ModForAbility4 > 0);
								goto IL_121;
							}
						}
					}
					flag2 = true;
					IL_121:
					flag3 = flag2;
				}
				else
				{
					bool flag4;
					if (characterComponent.LastMods.ModForAbility0 <= 0)
					{
						if (characterComponent.LastMods.ModForAbility1 <= 0 && characterComponent.LastMods.ModForAbility2 <= 0)
						{
							if (characterComponent.LastMods.ModForAbility3 <= 0)
							{
								flag4 = (characterComponent.LastMods.ModForAbility4 > 0);
								goto IL_19C;
							}
						}
					}
					flag4 = true;
					IL_19C:
					flag3 = flag4;
				}
				if (!flag3)
				{
					UICharacterSelectScreen.Get().ShowPleaseEquipModsDialog();
				}
			}
			IL_1AC:
			UICharacterSelectScreen.Get().SetGameSettingsButtonVisibility(false);
		}
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		string text = null;
		if (gameResult == GameResult.Requeued)
		{
		}
		else if (gameResult == GameResult.OwnerLeft)
		{
			text = StringUtil.TR("OwnerOfThisGameLeft", "Frontend");
		}
		else if (gameResult == GameResult.ClientKicked)
		{
			text = StringUtil.TR("KickedFromThisGame", "Frontend");
		}
		else if (gameResult == GameResult.ServerCrashed)
		{
			text = StringUtil.TR("FailedStartGameServer", "Frontend");
		}
		if (!text.IsNullOrEmpty() && UICharacterSelectScreenController.Get() != null)
		{
			UICharacterSelectScreenController.Get().OpenOneButtonDialog(string.Empty, text, null);
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}

	private void RefreshUI()
	{
		GameManager gameManager = GameManager.Get();
		LobbyGameInfo gameInfo = gameManager.GameInfo;
		LobbyPlayerInfo playerInfo = gameManager.PlayerInfo;
		LobbyTeamInfo teamInfo = gameManager.TeamInfo;
		if (gameInfo != null && playerInfo != null)
		{
			if (teamInfo != null)
			{
				if (UIFrontEnd.Get() != null)
				{
					if (UICharacterSelectScreen.Get() != null)
					{
						if (!gameManager.PlayerInfo.IsRemoteControlled)
						{
							Team team = gameManager.PlayerInfo.TeamId;
							if (team == Team.Spectator)
							{
								team = Team.TeamA;
							}
							List<LobbyPlayerInfo> teamPlayerInfos = (from ti in gameManager.TeamInfo.TeamInfo(team)
							orderby (ti.PlayerId != playerInfo.PlayerId) ? 1 : 0
							select ti).ToList<LobbyPlayerInfo>();
							List<LobbyPlayerInfo> list = gameManager.TeamInfo.TeamInfo(Team.Spectator).ToList<LobbyPlayerInfo>();
							UICharacterSelectScreenController uicharacterSelectScreenController = UICharacterSelectScreenController.Get();
							uicharacterSelectScreenController.UpdateCharacters(gameManager.PlayerInfo, teamPlayerInfos, gameManager.GameplayOverrides);
							UIManager.SetGameObjectActive(uicharacterSelectScreenController.m_charSettingsPanel.m_skinsSubPanel.m_purchasePanel, false, null);
							UIManager.SetGameObjectActive(uicharacterSelectScreenController.m_charSettingsPanel.m_tauntsSubPanel.m_purchasePanel, false, null);
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
									UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().m_spectatorPartyListPanel, true, null);
									UICharacterSelectScreen.Get().m_spectatorPartyListPanel.UpdateCharacterList(list);
								}
								else
								{
									UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().m_spectatorPartyListPanel, false, null);
								}
							}
							if (playerInfo.IsNPCBot)
							{
								if (playerInfo.TeamId == Team.TeamA)
								{
									if (gameInfo.GameConfig.GameType == GameType.Solo || gameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
									{
										UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
										{
											SelectedAllyBotDifficulty = new int?((int)playerInfo.Difficulty)
										});
									}
								}
								else if (playerInfo.TeamId == Team.TeamB)
								{
									if (gameInfo.GameConfig.GameType != GameType.Solo)
									{
										if (gameInfo.GameConfig.GameType != GameType.Coop)
										{
											goto IL_31B;
										}
									}
									UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
									{
										SelectedEnemyBotDifficulty = new int?((int)playerInfo.Difficulty)
									});
								}
							}
							IL_31B:
							UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, false, null);
						}
					}
				}
				return;
			}
		}
	}

	public void UpdateSelectedSkin(CharacterVisualInfo selectedCharacterSkin)
	{
		ClientGameManager.Get().UpdateSelectedSkin(selectedCharacterSkin, 0);
	}

	public void UpdateSelectedCards(CharacterCardInfo cards)
	{
		ClientGameManager.Get().UpdateSelectedCards(cards, 0);
	}

	public void UpdateSelectedMods(CharacterModInfo mods)
	{
		ClientGameManager.Get().UpdateSelectedMods(mods, 0);
	}

	public void UpdateSelectedAbilityVfxSwaps(CharacterAbilityVfxSwapInfo swaps)
	{
		ClientGameManager.Get().UpdateSelectedAbilityVfxSwaps(swaps, 0);
	}

	public void UpdateReadyState(bool ready)
	{
		this.UpdateReadyState(ready, new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay), new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay));
	}

	public void UpdateReadyState(bool ready, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		ReadyState readyState;
		if (ready)
		{
			readyState = ReadyState.Ready;
		}
		else
		{
			readyState = ReadyState.Accepted;
		}
		clientGameManager.UpdateReadyState(readyState, allyDifficulty, enemyDifficulty, new Action<PlayerInfoUpdateResponse>(this.HandlePlayerInfoUpdateResponse));
	}

	public static bool IsReady()
	{
		if (ClientGameManager.Get() == null)
		{
			return false;
		}
		if (ClientGameManager.Get().PlayerInfo == null)
		{
			return false;
		}
		if (ClientGameManager.Get().PlayerInfo.TeamId != Team.Spectator)
		{
			if (!ClientGameManager.Get().PlayerInfo.IsReady)
			{
				return false;
			}
		}
		return true;
	}

	public void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
		{
			if (response.ErrorMessage != null)
			{
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.ErrorMessage, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
			else if (response.LocalizedFailure != null)
			{
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"), null, -1, false);
			}
			return;
		}
		if (response.PlayerInfo != null)
		{
			if (response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId && UICharacterSelectScreenController.Get() != null)
			{
				UICharacterSelectScreenController.Get().UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
			}
		}
	}

	public void OnShowGameSettingsClicked()
	{
		GameManager gameManager = GameManager.Get();
		if (gameManager != null && gameManager.GameConfig != null)
		{
			if (gameManager.GameConfig.GameType == GameType.Custom)
			{
				if (gameManager.PlayerInfo != null)
				{
					if (UICharacterSelectScreen.Get() != null)
					{
						if (gameManager.PlayerInfo.IsGameOwner)
						{
							UICharacterSelectScreen.Get().ShowGameSettingsPanel(gameManager.GameConfig, gameManager.TeamInfo, gameManager.PlayerInfo);
						}
					}
				}
			}
		}
	}

	public void OnUpdateGameSettingsClicked(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, bool closeSettingsWindow = true)
	{
		LobbyGameInfo lobbyGameInfo = new LobbyGameInfo();
		lobbyGameInfo.GameConfig = gameConfig;
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
