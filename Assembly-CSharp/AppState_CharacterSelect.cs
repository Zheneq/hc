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
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				throw new Exception("GameInfo must be set before entering app state");
			}
		}
		if (gameManager.GameConfig == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				throw new Exception("GameConfig must be set before entering app state");
			}
		}
		if (gameManager.TeamInfo == null)
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
			Log.Warning("Team Info is null");
		}
		bool flag = true;
		if (gameManager.GameConfig.GameType != GameType.Tutorial)
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
			if (!gameManager.GameConfig.HasGameOption(GameOptionFlag.AutoLaunch))
			{
				if (hydrogenConfig.AutoLaunchGameType.IsAutoLaunchable())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (hydrogenConfig.CanAutoLaunchGame())
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.CharacterSelect);
			CharacterType charType = CharacterType.None;
			if (gameManager.PlayerInfo != null)
			{
				charType = gameManager.PlayerInfo.CharacterType;
				if (!clientGameManager.GroupInfo.InAGroup)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (UIPlayCategoryMenu.Get() != null)
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
				UIManager.SetGameObjectActive(UIPlayCategoryMenu.Get(), false);
			}
		}
		if (!(HighlightUtils.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!AssetBundleManager.Get().SceneExistsInBundle("maps", notification.GameInfo.GameConfig.Map))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!AssetBundleManager.Get().SceneExistsInBundle("testing", notification.GameInfo.GameConfig.Map))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
					if (uICharacterSelectScreenController != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						string message = (!GameManager.Get().PlayerInfo.IsGameOwner) ? StringUtil.TR("LeaderSelectedMapNoAccessTo", "Frontend") : StringUtil.TR("NoAccessToThisMap", "Frontend");
						string empty = string.Empty;
						if (_003C_003Ef__am_0024cache0 == null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
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
					switch (1)
					{
					case 0:
						continue;
					}
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
			while (true)
			{
				switch (2)
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
			if (playerInfo != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
											while (true)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												break;
											}
										}
										else
										{
											if (teamPlayerInfo.PlayerId == playerInfo.PlayerId)
											{
												while (true)
												{
													switch (3)
													{
													case 0:
														continue;
													}
													break;
												}
												if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(0) != playerInfo.CharacterInfo.CharacterType)
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
													result = true;
												}
											}
											else
											{
												if (UICharacterSelectWorldObjects.Get().CharacterTypeInSlot(num2) != teamPlayerInfo.CharacterInfo.CharacterType)
												{
													while (true)
													{
														switch (3)
														{
														case 0:
															continue;
														}
														break;
													}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
			while (true)
			{
				switch (4)
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
			if (gameStatus >= GameStatus.Launched)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameStatus != GameStatus.Stopped)
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
			switch (5)
			{
			case 0:
				continue;
			}
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterResourceLinkOfCharacterTypeToDisplay.m_characterType;
			int num;
			int num2;
			bool flag2;
			if (!characterType.IsWillFill())
			{
				if (!(GameManager.Get() == null))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						CharacterModInfo lastRankedMods2 = characterComponent.LastRankedMods;
						if (lastRankedMods2.ModForAbility1 <= 0)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							CharacterModInfo lastRankedMods3 = characterComponent.LastRankedMods;
							if (lastRankedMods3.ModForAbility2 <= 0)
							{
								CharacterModInfo lastRankedMods4 = characterComponent.LastRankedMods;
								if (lastRankedMods4.ModForAbility3 <= 0)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
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
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					CharacterModInfo lastMods2 = characterComponent.LastMods;
					if (lastMods2.ModForAbility1 <= 0)
					{
						CharacterModInfo lastMods3 = characterComponent.LastMods;
						if (lastMods3.ModForAbility2 <= 0)
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
			while (true)
			{
				switch (6)
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
		}
		else if (gameResult == GameResult.OwnerLeft)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			text = StringUtil.TR("OwnerOfThisGameLeft", "Frontend");
		}
		else if (gameResult == GameResult.ClientKicked)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (!(UICharacterSelectScreen.Get() != null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (gameManager.PlayerInfo.IsRemoteControlled)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					Team team = gameManager.PlayerInfo.TeamId;
					if (team == Team.Spectator)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
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
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (list.Count > 0)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
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
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (playerInfo.TeamId == Team.TeamA)
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
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (gameInfo.GameConfig.GameType != GameType.Solo)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (gameInfo.GameConfig.GameType != GameType.Coop)
								{
									goto IL_031b;
								}
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
			while (true)
			{
				switch (2)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (ClientGameManager.Get().PlayerInfo.TeamId != Team.Spectator)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId && UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (gameManager.GameConfig.GameType != 0)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (gameManager.PlayerInfo == null)
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (!(UICharacterSelectScreen.Get() != null))
					{
						return;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						if (gameManager.PlayerInfo.IsGameOwner)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
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
