using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
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
		return AppState_GameTypeSelect.s_instance;
	}

	public static void Create()
	{
		AppState.Create<AppState_GameTypeSelect>();
	}

	private void Awake()
	{
		AppState_GameTypeSelect.s_instance = this;
		this.m_autoCreateGameType = GameType.None;
		this.m_autoCreateGameSubType = 0;
	}

	public void Enter(AutoLaunchGameConfig autoCreateGameConfig)
	{
		this.m_autoCreateGameType = autoCreateGameConfig.GameConfig.GameType;
		this.m_autoCreateMapName = autoCreateGameConfig.GameConfig.Map;
		this.m_autoCreateGameConfig = autoCreateGameConfig;
		this.m_autoCreateGameAtTime = Time.unscaledTime;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(this.m_autoCreateGameType);
		if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.Enter(AutoLaunchGameConfig)).MethodHandle;
			}
			this.m_autoCreateGameSubType = gameTypeSubTypes.Keys.First<ushort>();
			this.m_autoCreateGameConfig.GameConfig.SubTypes = gameTypeSubTypes.Values.ToList<GameSubType>();
			if (gameTypeSubTypes.Count<KeyValuePair<ushort, GameSubType>>() > 1)
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
				Log.Error("More than one sub-type available for {0}, selecting 0x{1:X4} by default", new object[]
				{
					this.m_autoCreateGameType,
					this.m_autoCreateGameSubType
				});
			}
		}
		this.m_autoCreateGameConfig.GameConfig.SetGameOption(GameOptionFlag.AutoLaunch, true);
		base.Enter();
	}

	public void Enter(GameType autoCreateGameType, string autoCreateMapName = null)
	{
		this.m_autoCreateGameType = autoCreateGameType;
		this.m_autoCreateMapName = autoCreateMapName;
		this.m_autoCreateGameAtTime = Time.unscaledTime;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(autoCreateGameType);
		if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
		{
			this.m_autoCreateGameSubType = gameTypeSubTypes.Keys.First<ushort>();
			if (gameTypeSubTypes.Count<KeyValuePair<ushort, GameSubType>>() > 1)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.Enter(GameType, string)).MethodHandle;
				}
				Log.Error("More than one sub-type available for {0}, selecting 0x{1:X4} by default", new object[]
				{
					autoCreateGameType,
					this.m_autoCreateGameSubType
				});
			}
		}
		base.Enter();
	}

	protected override void OnEnter()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification += this.HandleGameInfoNotification;
		if (this.m_autoCreateGameType == GameType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.OnEnter()).MethodHandle;
			}
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GameTypeSelect, false);
		}
	}

	protected override void OnLeave()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (this.m_messageBox != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.OnLeave()).MethodHandle;
			}
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification -= this.HandleGameInfoNotification;
		if (UIFrontEnd.Get() != null)
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
			UIFrontEnd.Get().ShowScreen(FrontEndScreenState.None, false);
		}
	}

	protected void Update()
	{
		if (this.m_autoCreateGameType != GameType.None)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.Update()).MethodHandle;
			}
			if (this.m_autoCreateGameAtTime <= Time.unscaledTime)
			{
				if (this.m_autoCreateGameConfig != null)
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
					if (this.m_autoCreateGameConfig.GameConfig != null)
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
						if (this.m_autoCreateGameConfig.TeamInfo != null)
						{
							IEnumerable<LobbyPlayerInfo> teamAPlayerInfo = this.m_autoCreateGameConfig.TeamInfo.TeamAPlayerInfo;
							if (ClientGameManager.Get() != null)
							{
								IEnumerator<LobbyPlayerInfo> enumerator = teamAPlayerInfo.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
										if (lobbyPlayerInfo.IsGameOwner)
										{
											for (;;)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											if (lobbyPlayerInfo.CharacterType.IsValidForHumanGameplay())
											{
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													break;
												}
												ClientGameManager.Get().UpdateSelectedSkin(lobbyPlayerInfo.CharacterInfo.CharacterSkin, 0);
												ClientGameManager.Get().UpdateSelectedCharacter(lobbyPlayerInfo.CharacterType, 0);
												goto IL_12D;
											}
										}
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
								finally
								{
									if (enumerator != null)
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
										enumerator.Dispose();
									}
								}
							}
							IL_12D:
							if (ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(GameType.Custom))
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
								GameTypeAvailability gameTypeAvailability = ClientGameManager.Get().GameTypeAvailabilies[GameType.Custom];
								ushort num = 1;
								using (List<GameSubType>.Enumerator enumerator2 = gameTypeAvailability.SubTypes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										GameSubType gameSubType = enumerator2.Current;
										if (gameSubType.GameMapConfigs.Exists(delegate(GameMapConfig i)
										{
											bool result;
											if (i.IsActive)
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
													RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GameTypeSelect.<Update>m__0(GameMapConfig)).MethodHandle;
												}
												result = (i.Map == this.m_autoCreateGameConfig.GameConfig.Map);
											}
											else
											{
												result = false;
											}
											return result;
										}))
										{
											for (;;)
											{
												switch (6)
												{
												case 0:
													continue;
												}
												break;
											}
											this.m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit = num;
											goto IL_1E4;
										}
										num = (ushort)(num << 1);
									}
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
								}
							}
							IL_1E4:
							if (this.m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit == 0)
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
								this.m_autoCreateGameConfig.GameConfig.InstanceSubTypeBit = 1;
							}
							this.CreateGame(this.m_autoCreateGameConfig.GameConfig, this.m_autoCreateGameConfig.TeamInfo);
							goto IL_247;
						}
					}
				}
				this.CreateGame(this.m_autoCreateGameType, this.m_autoCreateGameSubType, this.m_autoCreateMapName);
				IL_247:
				this.m_autoCreateGameType = GameType.None;
				this.m_autoCreateGameSubType = 0;
				this.m_autoCreateMapName = null;
				this.m_autoCreateGameAtTime = 0f;
			}
		}
	}

	public void OnSoloClicked()
	{
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Coop);
		if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.OnSoloClicked()).MethodHandle;
			}
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
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
						this.CreateGame(GameType.Coop, keyValuePair.Key, null);
						return;
					}
				}
				for (;;)
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
		this.CreateGame(GameType.Solo, 1, null);
	}

	public void OnCoopClicked()
	{
		this.JoinQueue(GameType.Coop);
	}

	public void OnPvPClicked()
	{
		this.JoinQueue(GameType.PvP);
	}

	public void OnPracticeClicked()
	{
		this.CreateGame(GameType.Practice, 1, null);
	}

	public void OnTutorialClicked()
	{
		this.CreateGame(GameType.Tutorial, 1, null);
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
		BotDifficulty? botDifficulty = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay);
		BotDifficulty? botDifficulty2 = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay);
		ClientGameManager clientGameManager2 = clientGameManager;
		BotDifficulty? allyDifficulty = botDifficulty;
		BotDifficulty? enemyDifficulty = botDifficulty2;
		if (AppState_GameTypeSelect.<>f__am$cache0 == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.JoinQueue(GameType)).MethodHandle;
			}
			AppState_GameTypeSelect.<>f__am$cache0 = delegate(JoinMatchmakingQueueResponse response)
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
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GameTypeSelect.<JoinQueue>m__1(JoinMatchmakingQueueResponse)).MethodHandle;
						}
						description = response.LocalizedFailure.ToString();
					}
					else if (response.ErrorMessage.IsNullOrEmpty())
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
						description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
					}
					else
					{
						description = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
					}
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"), null, -1, false);
				}
			};
		}
		clientGameManager2.JoinQueue(gameType, allyDifficulty, enemyDifficulty, AppState_GameTypeSelect.<>f__am$cache0);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.CreateGame(LobbyGameConfig, LobbyTeamInfo)).MethodHandle;
			}
			selectedBotSkillTeamA = BotDifficulty.Hard;
			selectedBotSkillTeamB = BotDifficulty.Tutorial;
		}
		else
		{
			bool flag = gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
			if (flag)
			{
				selectedBotSkillTeamA = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
			}
			if (gameType != GameType.Solo)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (gameType != GameType.Coop)
				{
					goto IL_C1;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			selectedBotSkillTeamB = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		}
		IL_C1:
		clientGameManager.CreateGame(gameConfig, ReadyState.Accepted, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse response)
		{
			if (response.Success)
			{
				if (GameManager.Get().GameConfig.GameType != GameType.Tutorial && GameManager.Get().GameConfig.GameType != GameType.NewPlayerSolo)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GameTypeSelect.<CreateGame>c__AnonStorey0.<>m__0(CreateGameResponse)).MethodHandle;
					}
					AppState_CharacterSelect.Get().Enter();
					bool flag2 = GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.AutoLaunch);
					if (flag2)
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
						ClientGameManager.Get().UpdateGameInfo(GameManager.Get().GameConfig, teamInfo);
						AppState_CharacterSelect.Get().UpdateReadyState(true, null, null);
					}
				}
			}
			else if (response.AllowRetry)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_autoCreateGameType = gameType;
				this.m_autoCreateMapName = mapName;
				this.m_autoCreateGameAtTime = Time.unscaledTime + 5f;
				this.m_autoCreateGameSubType = subTypeBit;
				UIFrontendLoadingScreen.Get().StartDisplayLoading(response.LocalizedFailure.ToString());
			}
			else
			{
				UIFrontendLoadingScreen.Get().SetVisible(false);
				string description;
				if (response.LocalizedFailure != null)
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
					description = response.LocalizedFailure.ToString();
				}
				else if (gameType == GameType.Tutorial)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					description = StringUtil.TR("UnableToLoadTutorial", "Frontend");
				}
				else if (!response.ErrorMessage.IsNullOrEmpty())
				{
					description = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
				}
				else
				{
					description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"), null, -1, false);
				if (gameType == GameType.Tutorial)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					AppState_LandingPage.Get().Enter();
				}
			}
		});
	}

	public void CreateGame(GameType gameType, ushort subTypeBit, string mapName = null)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		this.CreateGame(new LobbyGameConfig
		{
			GameType = gameType,
			Map = mapName,
			InstanceSubTypeBit = subTypeBit,
			SubTypes = clientGameManager.GetGameTypeSubTypes(gameType).Values.ToList<GameSubType>()
		}, null);
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
		{
			if (GameManager.Get().GameConfig.GameType != GameType.NewPlayerSolo)
			{
				return;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GameTypeSelect.HandleGameInfoNotification(GameInfoNotification)).MethodHandle;
			}
		}
		if (GameManager.Get().PlayerInfo != null && GameManager.Get().GameInfo != null)
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
			if (GameManager.Get().TeamInfo != null)
			{
				AppState_GameLoading.Get().Enter(GameManager.Get().GameConfig.GameType);
			}
		}
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}
}
