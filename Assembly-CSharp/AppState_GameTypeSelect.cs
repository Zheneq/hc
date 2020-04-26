using LobbyGameClientMessages;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppState_GameTypeSelect : AppState
{
	private static AppState_GameTypeSelect s_instance;

	private GameType m_autoCreateGameType;

	private ushort m_autoCreateGameSubType;

	private string m_autoCreateMapName;

	private AutoLaunchGameConfig m_autoCreateGameConfig;

	private float m_autoCreateGameAtTime;

	private UIOneButtonDialog m_messageBox;

	public static AppState_GameTypeSelect Get()
	{
		return s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_GameTypeSelect>();
	}

	private void Awake()
	{
		s_instance = this;
		m_autoCreateGameType = GameType.None;
		m_autoCreateGameSubType = 0;
	}

	public void Enter(AutoLaunchGameConfig autoCreateGameConfig)
	{
		m_autoCreateGameType = autoCreateGameConfig.GameConfig.GameType;
		m_autoCreateMapName = autoCreateGameConfig.GameConfig.Map;
		m_autoCreateGameConfig = autoCreateGameConfig;
		m_autoCreateGameAtTime = Time.unscaledTime;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(m_autoCreateGameType);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			m_autoCreateGameSubType = gameTypeSubTypes.Keys.First();
			m_autoCreateGameConfig.GameConfig.SubTypes = gameTypeSubTypes.Values.ToList();
			if (gameTypeSubTypes.Count() > 1)
			{
				Log.Error("More than one sub-type available for {0}, selecting 0x{1:X4} by default", m_autoCreateGameType, m_autoCreateGameSubType);
			}
		}
		m_autoCreateGameConfig.GameConfig.SetGameOption(GameOptionFlag.AutoLaunch, true);
		base.Enter();
	}

	public void Enter(GameType autoCreateGameType, string autoCreateMapName = null)
	{
		m_autoCreateGameType = autoCreateGameType;
		m_autoCreateMapName = autoCreateMapName;
		m_autoCreateGameAtTime = Time.unscaledTime;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(autoCreateGameType);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			m_autoCreateGameSubType = gameTypeSubTypes.Keys.First();
			if (gameTypeSubTypes.Count() > 1)
			{
				Log.Error("More than one sub-type available for {0}, selecting 0x{1:X4} by default", autoCreateGameType, m_autoCreateGameSubType);
			}
		}
		base.Enter();
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification += HandleGameInfoNotification;
		if (m_autoCreateGameType != GameType.None)
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GameTypeSelect);
			return;
		}
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (m_messageBox != null)
		{
			m_messageBox.Close();
			m_messageBox = null;
		}
		clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification -= HandleGameInfoNotification;
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None);
			return;
		}
	}

	protected void Update()
	{
		if (m_autoCreateGameType == GameType.None)
		{
			return;
		}
		while (true)
		{
			if (!(m_autoCreateGameAtTime <= Time.unscaledTime))
			{
				return;
			}
			if (m_autoCreateGameConfig != null)
			{
				if (m_autoCreateGameConfig.GameConfig != null)
				{
					if (m_autoCreateGameConfig.TeamInfo != null)
					{
						IEnumerable<LobbyPlayerInfo> teamAPlayerInfo = m_autoCreateGameConfig.TeamInfo.TeamAPlayerInfo;
						if (ClientGameManager.Get() != null)
						{
							IEnumerator<LobbyPlayerInfo> enumerator = teamAPlayerInfo.GetEnumerator();
							try
							{
								while (true)
								{
									if (!enumerator.MoveNext())
									{
										break;
									}
									LobbyPlayerInfo current = enumerator.Current;
									if (current.IsGameOwner)
									{
										if (current.CharacterType.IsValidForHumanGameplay())
										{
											while (true)
											{
												switch (6)
												{
												case 0:
													break;
												default:
													ClientGameManager.Get().UpdateSelectedSkin(current.CharacterInfo.CharacterSkin);
													ClientGameManager.Get().UpdateSelectedCharacter(current.CharacterType);
													goto end_IL_009e;
												}
											}
										}
									}
								}
								end_IL_009e:;
							}
							finally
							{
								if (enumerator != null)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											enumerator.Dispose();
											goto end_IL_0119;
										}
									}
								}
								end_IL_0119:;
							}
						}
						if (ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(GameType.Custom))
						{
							GameTypeAvailability gameTypeAvailability = ClientGameManager.Get().GameTypeAvailabilies[GameType.Custom];
							ushort num = 1;
							using (List<GameSubType>.Enumerator enumerator2 = gameTypeAvailability.SubTypes.GetEnumerator())
							{
								while (true)
								{
									if (!enumerator2.MoveNext())
									{
										break;
									}
									GameSubType current2 = enumerator2.Current;
									if (current2.GameMapConfigs.Exists(delegate(GameMapConfig i)
									{
										int result;
										if (i.IsActive)
										{
											result = ((i.Map == m_autoCreateGameConfig.GameConfig.Map) ? 1 : 0);
										}
										else
										{
											result = 0;
										}
										return (byte)result != 0;
									}))
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit = num;
												goto end_IL_0171;
											}
										}
									}
									num = (ushort)(num << 1);
								}
								end_IL_0171:;
							}
						}
						if (m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit == 0)
						{
							m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit = 1;
						}
						CreateGame(m_autoCreateGameConfig.GameConfig, m_autoCreateGameConfig.TeamInfo);
						goto IL_0247;
					}
				}
			}
			CreateGame(m_autoCreateGameType, m_autoCreateGameSubType, m_autoCreateMapName);
			goto IL_0247;
			IL_0247:
			m_autoCreateGameType = GameType.None;
			m_autoCreateGameSubType = 0;
			m_autoCreateMapName = null;
			m_autoCreateGameAtTime = 0f;
			return;
		}
	}

	public void OnSoloClicked()
	{
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Coop);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ushort, GameSubType> current = enumerator.Current;
					if (current.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								CreateGame(GameType.Coop, current.Key);
								return;
							}
						}
					}
				}
			}
		}
		CreateGame(GameType.Solo, 1);
	}

	public void OnCoopClicked()
	{
		JoinQueue(GameType.Coop);
	}

	public void OnPvPClicked()
	{
		JoinQueue(GameType.PvP);
	}

	public void OnPracticeClicked()
	{
		CreateGame(GameType.Practice, 1);
	}

	public void OnTutorialClicked()
	{
		CreateGame(GameType.Tutorial, 1);
	}

	public void OnCustomClicked()
	{
		AppState_JoinGame.Get().Enter();
	}

	public void OnCancelClicked()
	{
		AppState_LandingPage.Get().Enter();
	}

	public void JoinQueue(GameType gameType)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		BotDifficulty? allyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		BotDifficulty? enemyDifficulty = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		
		clientGameManager.JoinQueue(gameType, allyDifficulty, enemyDifficulty, delegate(JoinMatchmakingQueueResponse response)
			{
				if (response.Success)
				{
					AppState_WaitingForGame.Get().Enter();
				}
				else
				{
					string description;
					if (response.LocalizedFailure != null)
					{
						description = response.LocalizedFailure.ToString();
					}
					else if (response.ErrorMessage.IsNullOrEmpty())
					{
						description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
					}
					else
					{
						description = $"{response.ErrorMessage}#NeedsLocalization";
					}
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"));
				}
			});
	}

	public void CreateGame(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo = null)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameType gameType = gameConfig.GameType;
		string mapName = gameConfig.Map;
		ushort subTypeBit = gameConfig.InstanceSubTypeBit;
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		if (gameType == GameType.NewPlayerSolo)
		{
			selectedBotSkillTeamA = BotDifficulty.Hard;
			selectedBotSkillTeamB = BotDifficulty.Tutorial;
		}
		else
		{
			if (gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
			{
				selectedBotSkillTeamA = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
			}
			if (gameType != GameType.Solo)
			{
				if (gameType != GameType.Coop)
				{
					goto IL_00c1;
				}
			}
			selectedBotSkillTeamB = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		}
		goto IL_00c1;
		IL_00c1:
		clientGameManager.CreateGame(gameConfig, ReadyState.Accepted, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse response)
		{
			if (response.Success)
			{
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial && GameManager.Get().GameConfig.GameType != GameType.NewPlayerSolo)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							AppState_CharacterSelect.Get().Enter();
							if (GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.AutoLaunch))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										ClientGameManager.Get().UpdateGameInfo(GameManager.Get().GameConfig, teamInfo);
										AppState_CharacterSelect.Get().UpdateReadyState(true, null, null);
										return;
									}
								}
							}
							return;
						}
					}
				}
			}
			else
			{
				if (response.AllowRetry)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							m_autoCreateGameType = gameType;
							m_autoCreateMapName = mapName;
							m_autoCreateGameAtTime = Time.unscaledTime + 5f;
							m_autoCreateGameSubType = subTypeBit;
							UIFrontendLoadingScreen.Get().StartDisplayLoading(response.LocalizedFailure.ToString());
							return;
						}
					}
				}
				UIFrontendLoadingScreen.Get().SetVisible(false);
				string description;
				if (response.LocalizedFailure != null)
				{
					description = response.LocalizedFailure.ToString();
				}
				else if (gameType != GameType.Tutorial)
				{
					description = (response.ErrorMessage.IsNullOrEmpty() ? StringUtil.TR("UnknownErrorTryAgain", "Frontend") : $"{response.ErrorMessage}#NeedsLocalization");
				}
				else
				{
					description = StringUtil.TR("UnableToLoadTutorial", "Frontend");
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"));
				if (gameType == GameType.Tutorial)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							AppState_LandingPage.Get().Enter();
							return;
						}
					}
				}
			}
		});
	}

	public void CreateGame(GameType gameType, ushort subTypeBit, string mapName = null)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		lobbyGameConfig.GameType = gameType;
		lobbyGameConfig.Map = mapName;
		lobbyGameConfig.InstanceSubTypeBit = subTypeBit;
		lobbyGameConfig.SubTypes = clientGameManager.GetGameTypeSubTypes(gameType).Values.ToList();
		CreateGame(lobbyGameConfig);
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			if (GameManager.Get().GameConfig.GameType != GameType.NewPlayerSolo)
			{
				return;
			}
		}
		if (GameManager.Get().PlayerInfo == null || GameManager.Get().GameInfo == null)
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().TeamInfo != null)
			{
				AppState_GameLoading.Get().Enter(GameManager.Get().GameConfig.GameType);
			}
			return;
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage);
	}
}
