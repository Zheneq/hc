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
			while (true)
			{
				throw new Exception("GameInfo must be set before entering app state");
			}
		}
		if (gameManager.GameConfig == null)
		{
			while (true)
			{
				throw new Exception("GameConfig must be set before entering app state");
			}
		}
		if (gameManager.TeamInfo == null)
		{
			Log.Warning("Team Info is null");
		}
		bool flag = true;
		if (gameManager.GameConfig.GameType != GameType.Tutorial)
		{
			if (!gameManager.GameConfig.HasGameOption(GameOptionFlag.AutoLaunch))
			{
				if (hydrogenConfig.AutoLaunchGameType.IsAutoLaunchable())
				{
					if (hydrogenConfig.CanAutoLaunchGame())
					{
						goto IL_00e4;
					}
				}
				goto IL_00e6;
			}
		}
		goto IL_00e4;
		IL_00e6:
		if (flag)
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
				CustomGamePartyListVisible = (gameManager.GameInfo.GameConfig.GameType == GameType.Custom)
			});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		GameManager.Get().OnGameStopped += HandleGameStopped;
		GameManager.Get().OnGameLoadoutSelecting += HandleGameLoadoutSelecting;
		clientGameManager.OnGameInfoNotification += HandleGameInfoNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		RefreshUI();
		if (gameManager.GameConfig.GameType == GameType.Custom)
		{
			if (UIPlayCategoryMenu.Get() != null)
			{
				UIManager.SetGameObjectActive(UIPlayCategoryMenu.Get(), false);
			}
		}
		if (!(HighlightUtils.Get() != null))
		{
			return;
		}
		while (true)
		{
			HighlightUtils.Get().HideCursorHighlights();
			return;
		}
		IL_00e4:
		flag = false;
		goto IL_00e6;
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
			UICharacterScreen.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CustomGamePartyListVisible = false
			});
			UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
		}
		if (ClientGameManager.Get().GroupInfo == null)
		{
			return;
		}
		while (true)
		{
			if (UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, ClientGameManager.Get().GameTypeAvailabilies[ClientGameManager.Get().GroupInfo.SelectedQueueType].MaxWillFillPerTeam > 0);
					return;
				}
			}
			return;
		}
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			return;
		}
		while (true)
		{
			if (!AssetBundleManager.Get().SceneExistsInBundle("maps", notification.GameInfo.GameConfig.Map))
			{
				if (!AssetBundleManager.Get().SceneExistsInBundle("testing", notification.GameInfo.GameConfig.Map))
				{
					UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
					if (uICharacterSelectScreenController != null)
					{
						string message = (!GameManager.Get().PlayerInfo.IsGameOwner) ? StringUtil.TR("LeaderSelectedMapNoAccessTo", "Frontend") : StringUtil.TR("NoAccessToThisMap", "Frontend");
						string empty = string.Empty;
						if (_003C_003Ef__am_0024cache0 == null)
						{
							_003C_003Ef__am_0024cache0 = delegate
							{
								AppState_GameTeardown.Get().Enter();
							};
						}
						uICharacterSelectScreenController.OpenOneButtonDialog(empty, message, _003C_003Ef__am_0024cache0);
					}
					goto IL_011c;
				}
			}
			RefreshUI();
			goto IL_011c;
			IL_011c:
			if (notification.GameInfo.GameConfig.GameType == GameType.Coop)
			{
				while (true)
				{
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						SelectedEnemyBotDifficulty = (int)notification.GameInfo.SelectedBotSkillTeamB
					});
					return;
				}
			}
			return;
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
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
							{
								int num2 = 1;
								{
									foreach (LobbyPlayerInfo teamPlayerInfo in teamPlayerInfos)
									{
										if (teamPlayerInfo.CharacterType == CharacterType.None)
										{
										}
										else
										{
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
												return result;
											}
										}
									}
									return result;
								}
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
		if (IsReady())
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
		if (!(gameManager != null) || !NeedForceUpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides))
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().UpdateCharacters(gameManager.PlayerInfo, gameManager.TeamPlayerInfo, gameManager.GameplayOverrides);
			return;
		}
	}

	private void HandleGameLoadoutSelecting()
	{
		if (!(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		while (true)
		{
			CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_characterType;
			int num;
			int num2;
			bool flag2;
			if (!characterType.IsWillFill())
			{
				if (!(GameManager.Get() == null))
				{
					if (GameManager.Get().PlayerInfo.TeamId == Team.Spectator)
					{
						goto IL_01ac;
					}
				}
				CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(characterType).CharacterComponent;
				bool flag = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
				flag2 = false;
				if (flag)
				{
					CharacterModInfo lastRankedMods = characterComponent.LastRankedMods;
					if (lastRankedMods.ModForAbility0 <= 0)
					{
						CharacterModInfo lastRankedMods2 = characterComponent.LastRankedMods;
						if (lastRankedMods2.ModForAbility1 <= 0)
						{
							CharacterModInfo lastRankedMods3 = characterComponent.LastRankedMods;
							if (lastRankedMods3.ModForAbility2 <= 0)
							{
								CharacterModInfo lastRankedMods4 = characterComponent.LastRankedMods;
								if (lastRankedMods4.ModForAbility3 <= 0)
								{
									CharacterModInfo lastRankedMods5 = characterComponent.LastRankedMods;
									num = ((lastRankedMods5.ModForAbility4 > 0) ? 1 : 0);
									goto IL_0121;
								}
							}
						}
					}
					num = 1;
					goto IL_0121;
				}
				CharacterModInfo lastMods = characterComponent.LastMods;
				if (lastMods.ModForAbility0 <= 0)
				{
					CharacterModInfo lastMods2 = characterComponent.LastMods;
					if (lastMods2.ModForAbility1 <= 0)
					{
						CharacterModInfo lastMods3 = characterComponent.LastMods;
						if (lastMods3.ModForAbility2 <= 0)
						{
							CharacterModInfo lastMods4 = characterComponent.LastMods;
							if (lastMods4.ModForAbility3 <= 0)
							{
								CharacterModInfo lastMods5 = characterComponent.LastMods;
								num2 = ((lastMods5.ModForAbility4 > 0) ? 1 : 0);
								goto IL_019c;
							}
						}
					}
				}
				num2 = 1;
				goto IL_019c;
			}
			goto IL_01ac;
			IL_01ac:
			UICharacterSelectScreen.Get().SetGameSettingsButtonVisibility(false);
			return;
			IL_019d:
			if (!flag2)
			{
				UICharacterSelectScreen.Get().ShowPleaseEquipModsDialog();
			}
			goto IL_01ac;
			IL_0121:
			flag2 = ((byte)num != 0);
			goto IL_019d;
			IL_019c:
			flag2 = ((byte)num2 != 0);
			goto IL_019d;
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
		if (text.IsNullOrEmpty() || !(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().OpenOneButtonDialog(string.Empty, text);
			return;
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
		if (gameInfo == null || playerInfo == null)
		{
			return;
		}
		if (teamInfo == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(UICharacterSelectScreen.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (gameManager.PlayerInfo.IsRemoteControlled)
				{
					return;
				}
				while (true)
				{
					Team team = gameManager.PlayerInfo.TeamId;
					if (team == Team.Spectator)
					{
						team = Team.TeamA;
					}
					List<LobbyPlayerInfo> teamPlayerInfos = (from ti in gameManager.TeamInfo.TeamInfo(team)
						orderby (ti.PlayerId != playerInfo.PlayerId) ? 1 : 0
						select ti).ToList();
					List<LobbyPlayerInfo> list = gameManager.TeamInfo.TeamInfo(Team.Spectator).ToList();
					UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
					uICharacterSelectScreenController.UpdateCharacters(gameManager.PlayerInfo, teamPlayerInfos, gameManager.GameplayOverrides);
					UIManager.SetGameObjectActive(uICharacterSelectScreenController.m_charSettingsPanel.m_skinsSubPanel.m_purchasePanel, false);
					UIManager.SetGameObjectActive(uICharacterSelectScreenController.m_charSettingsPanel.m_tauntsSubPanel.m_purchasePanel, false);
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
							if (gameInfo.GameConfig.GameType == GameType.Solo || gameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
							{
								UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
								{
									SelectedAllyBotDifficulty = (int)playerInfo.Difficulty
								});
							}
						}
						else if (playerInfo.TeamId == Team.TeamB)
						{
							if (gameInfo.GameConfig.GameType != GameType.Solo)
							{
								if (gameInfo.GameConfig.GameType != GameType.Coop)
								{
									goto IL_031b;
								}
							}
							UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
							{
								SelectedEnemyBotDifficulty = (int)playerInfo.Difficulty
							});
						}
					}
					goto IL_031b;
					IL_031b:
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, false);
					return;
				}
			}
		}
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
		UpdateReadyState(ready, (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay, (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay);
	}

	public void UpdateReadyState(bool ready, BotDifficulty? allyDifficulty, BotDifficulty? enemyDifficulty)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		int readyState;
		if (ready)
		{
			readyState = 3;
		}
		else
		{
			readyState = 1;
		}
		clientGameManager.UpdateReadyState((ReadyState)readyState, allyDifficulty, enemyDifficulty, HandlePlayerInfoUpdateResponse);
	}

	public static bool IsReady()
	{
		if (ClientGameManager.Get() == null)
		{
			return false;
		}
		if (ClientGameManager.Get().PlayerInfo == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (response.ErrorMessage != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.ErrorMessage, StringUtil.TR("Ok", "Global"));
								return;
							}
						}
					}
					if (response.LocalizedFailure != null)
					{
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"));
					}
					return;
				}
			}
		}
		if (response.PlayerInfo == null)
		{
			return;
		}
		while (true)
		{
			if (response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId && UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					UICharacterSelectScreenController.Get().UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
					return;
				}
			}
			return;
		}
	}

	public void OnShowGameSettingsClicked()
	{
		GameManager gameManager = GameManager.Get();
		if (!(gameManager != null) || gameManager.GameConfig == null)
		{
			return;
		}
		while (true)
		{
			if (gameManager.GameConfig.GameType != 0)
			{
				return;
			}
			while (true)
			{
				if (gameManager.PlayerInfo == null)
				{
					return;
				}
				while (true)
				{
					if (!(UICharacterSelectScreen.Get() != null))
					{
						return;
					}
					while (true)
					{
						if (gameManager.PlayerInfo.IsGameOwner)
						{
							while (true)
							{
								UICharacterSelectScreen.Get().ShowGameSettingsPanel(gameManager.GameConfig, gameManager.TeamInfo, gameManager.PlayerInfo);
								return;
							}
						}
						return;
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
