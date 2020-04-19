using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using UnityEngine;

public class AppState_GroupCharacterSelect : AppState
{
	private static AppState_GroupCharacterSelect s_instance;

	private static Dictionary<GameType, DateTime> m_paroleTimesWarned = new Dictionary<GameType, DateTime>();

	private bool m_pendingJoinQueueRequest;

	private bool m_pendingLeaveQueueRequest;

	private bool m_inQueue;

	private bool m_readyForSoloGame;

	private bool m_receivedGameInfoNotification;

	private bool m_gameSelecting;

	private bool m_wasRequeued;

	private UIDialogBox m_messageBox;

	public static AppState_GroupCharacterSelect Get()
	{
		return AppState_GroupCharacterSelect.s_instance;
	}

	private bool IsSelfReady()
	{
		if (this.m_readyForSoloGame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.IsSelfReady()).MethodHandle;
			}
			return true;
		}
		if (ClientGameManager.Get() != null && ClientGameManager.Get().GroupInfo != null)
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
			for (int i = 0; i < ClientGameManager.Get().GroupInfo.Members.Count; i++)
			{
				if (ClientGameManager.Get().GroupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
					if (!ClientGameManager.Get().GroupInfo.Members[i].IsReady)
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
						if (ClientGameManager.Get().PlayerInfo != null)
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
							if (ClientGameManager.Get().PlayerInfo.IsSpectator)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									return true;
								}
							}
						}
						break;
					}
					return true;
				}
			}
		}
		return false;
	}

	public static void Create()
	{
		AppState.Create<AppState_GroupCharacterSelect>();
	}

	public void SelectedGameMode(GameType gameType)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestedGameType = new GameType?(gameType)
		});
		ClientGameManager.Get().RequestLoadouts(AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked);
	}

	private void CheckForQueueParole()
	{
		LocalizationArg_TimeSpan localizationArg_TimeSpan = null;
		LocalizationArg_GameType localizationArg_GameType = null;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null && clientGameManager.GameTypeAvailabilies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.CheckForQueueParole()).MethodHandle;
			}
			foreach (KeyValuePair<GameType, GameTypeAvailability> keyValuePair in clientGameManager.GameTypeAvailabilies)
			{
				GameType key = keyValuePair.Key;
				DateTime? paroleTimeout = keyValuePair.Value.ParoleTimeout;
				if (paroleTimeout != null)
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
					if (!AppState_GroupCharacterSelect.m_paroleTimesWarned.ContainsKey(key))
					{
						AppState_GroupCharacterSelect.m_paroleTimesWarned[key] = paroleTimeout.Value;
						localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(paroleTimeout.Value - DateTime.UtcNow);
						localizationArg_GameType = LocalizationArg_GameType.Create(key);
						break;
					}
					if (AppState_GroupCharacterSelect.m_paroleTimesWarned[key] != paroleTimeout.Value)
					{
						AppState_GroupCharacterSelect.m_paroleTimesWarned[key] = paroleTimeout.Value;
						localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(paroleTimeout.Value - DateTime.UtcNow);
						localizationArg_GameType = LocalizationArg_GameType.Create(key);
						break;
					}
				}
				else if (AppState_GroupCharacterSelect.m_paroleTimesWarned.ContainsKey(keyValuePair.Key))
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
					AppState_GroupCharacterSelect.m_paroleTimesWarned.Remove(keyValuePair.Key);
				}
			}
		}
		if (localizationArg_TimeSpan != null)
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
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("RecentlyLeftGame", "Global"), LocalizationPayload.Create("RecentlyLeftGameDesc", "Global", new LocalizationArg[]
			{
				localizationArg_GameType,
				localizationArg_TimeSpan
			}).ToString(), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	public override void Enter()
	{
		base.Enter();
		UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
	}

	public void ReEnter(bool wasRequeued)
	{
		this.m_wasRequeued = wasRequeued;
		this.Enter();
	}

	public bool InQueue()
	{
		if (!this.m_inQueue)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.InQueue()).MethodHandle;
			}
			if (!this.m_wasRequeued)
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
				return this.m_pendingJoinQueueRequest;
			}
		}
		return true;
	}

	public bool IsReady()
	{
		bool result;
		if (!this.IsSelfReady())
		{
			if (this.m_inQueue)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.IsReady()).MethodHandle;
				}
				result = !this.m_pendingLeaveQueueRequest;
			}
			else
			{
				result = false;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void CreateGame(GameType gameType, string mapName = null)
	{
		LobbyGameConfig gameConfig = new LobbyGameConfig();
		gameConfig.GameType = gameType;
		gameConfig.Map = mapName;
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		if (gameType != GameType.Solo)
		{
			if (gameType != GameType.Coop)
			{
				goto IL_6B;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.CreateGame(GameType, string)).MethodHandle;
			}
		}
		selectedBotSkillTeamA = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay;
		selectedBotSkillTeamB = (BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay;
		IL_6B:
		ClientGameManager.Get().CreateGame(gameConfig, ReadyState.Ready, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse response)
		{
			if (!response.Success)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GroupCharacterSelect.<CreateGame>c__AnonStorey0.<>m__0(CreateGameResponse)).MethodHandle;
				}
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
				else if (!response.ErrorMessage.IsNullOrEmpty())
				{
					description = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
				}
				else
				{
					description = StringUtil.TR("UnknownErrorTryAgain", "Frontend");
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"), null, -1, false);
				if (gameConfig.GameType != GameType.Practice)
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
							return;
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!gameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial))
						{
							return;
						}
					}
				}
				this.m_readyForSoloGame = false;
			}
		});
	}

	public void NotifyDroppedGroup()
	{
		if (UICharacterSelectScreenController.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.NotifyDroppedGroup()).MethodHandle;
			}
			UICharacterSelectScreenController.Get().NotifyDroppedGroup();
		}
	}

	public void NotifyJoinedNewGroup()
	{
		if (this.m_inQueue)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.NotifyJoinedNewGroup()).MethodHandle;
			}
			this.NotifyQueueDrop();
		}
	}

	public void NotifyQueueDrop()
	{
		this.m_pendingLeaveQueueRequest = false;
		this.m_inQueue = false;
		UICharacterSelectScreenController uicharacterSelectScreenController = UICharacterSelectScreenController.Get();
		if (uicharacterSelectScreenController != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.NotifyQueueDrop()).MethodHandle;
			}
			UIManager.SetGameObjectActive(uicharacterSelectScreenController.m_readyBtn, true, null);
			UICharacterSelectWorldObjects.Get().SetCharacterReady(0, false);
			uicharacterSelectScreenController.m_readyBtn.spriteController.SetClickable(true);
			UIManager.SetGameObjectActive(NavigationBar.Get().m_cancelBtn, false, null);
			NavigationBar.Get().m_cancelBtn.spriteController.SetClickable(false);
			NavigationBar.Get().m_searchQueueText.text = string.Empty;
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				ClientRequestToServerSelectCharacter = new CharacterType?(UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay)
			});
		}
		UICharacterSelectWorldObjects uicharacterSelectWorldObjects = UICharacterSelectWorldObjects.Get();
		if (uicharacterSelectWorldObjects != null)
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
			if (!ClientGameManager.Get().GroupInfo.InAGroup)
			{
				uicharacterSelectWorldObjects.m_ringAnimations[0].PlayAnimation("ReadyOut");
			}
		}
	}

	public void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse)).MethodHandle;
			}
			UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.ErrorMessage, StringUtil.TR("Ok", "Global"), null, -1, false);
			return;
		}
		if (response.PlayerInfo != null && response.PlayerInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
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
			UICharacterSelectScreenController.Get().UpdateCharacters(response.PlayerInfo, null, GameManager.Get().GameplayOverrides);
		}
	}

	private void HandleGameLaunched(GameType gameType)
	{
		this.m_pendingLeaveQueueRequest = false;
		this.m_inQueue = false;
		AppState_GameLoading.Get().Enter(gameType);
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("FailedStartGameServer", "Frontend"), StringUtil.TR("Ok", "Global"), null, -1, false);
	}

	public void UpdateReadyState(bool ready)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
		if (groupInfo == null)
		{
			return;
		}
		for (int i = 0; i < groupInfo.Members.Count; i++)
		{
			if (groupInfo.Members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.UpdateReadyState(bool)).MethodHandle;
				}
				groupInfo.Members[i].IsReady = ready;
				break;
			}
		}
		BotDifficulty? allyDifficulty = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay);
		BotDifficulty? enemyDifficulty = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay);
		if (ready)
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
			if (enemyDifficulty != null)
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
				PlayerPrefs.SetInt("CoopDifficulty", (int)(1 + enemyDifficulty.Value));
			}
		}
		ReadyState readyState;
		if (ready)
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
			readyState = ReadyState.Ready;
		}
		else
		{
			readyState = ReadyState.Accepted;
		}
		ReadyState readyState2 = readyState;
		if (groupInfo.SelectedQueueType != GameType.Practice)
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
			if (groupInfo.SelectedQueueType != GameType.Solo)
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
				if (groupInfo.InAGroup)
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
					if (!groupInfo.IsLeader)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_13B;
						}
					}
				}
				if (groupInfo.SelectedQueueType.IsQueueable())
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
					if (ready)
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
						if (!this.m_inQueue)
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
							if (groupInfo.IsLeader)
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
								clientGameManager.UpdateReadyState(readyState2, allyDifficulty, enemyDifficulty, new Action<PlayerInfoUpdateResponse>(this.HandlePlayerGroupInfoUpdateResponse));
							}
							else
							{
								this.JoinQueue(ClientGameManager.Get().GroupInfo.SelectedQueueType);
							}
						}
					}
					else if (this.m_inQueue)
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
						Log.Info("Sending Leave Queue Request because setting myself not ready", new object[0]);
						this.LeaveQueue();
					}
					else
					{
						clientGameManager.UpdateReadyState(readyState2, allyDifficulty, enemyDifficulty, new Action<PlayerInfoUpdateResponse>(this.HandlePlayerGroupInfoUpdateResponse));
					}
					return;
				}
				Log.Error("Don't know what to do with a ready change when you're the leader of a group for a {0} game", new object[]
				{
					groupInfo.SelectedQueueType
				});
				return;
			}
		}
		IL_13B:
		if (!ready)
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
			if (groupInfo.InAGroup)
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
				this.m_pendingLeaveQueueRequest = true;
			}
		}
		if (groupInfo.SelectedQueueType != GameType.Practice)
		{
			if (groupInfo.SelectedQueueType != GameType.Solo)
			{
				goto IL_195;
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
		this.m_readyForSoloGame = true;
		UICharacterScreen.Get().DoRefreshFunctions(0x80);
		IL_195:
		clientGameManager.UpdateReadyState(readyState2, allyDifficulty, enemyDifficulty, new Action<PlayerInfoUpdateResponse>(this.HandlePlayerGroupInfoUpdateResponse));
	}

	public void HandlePlayerGroupInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		if (!response.Success)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.HandlePlayerGroupInfoUpdateResponse(PlayerInfoUpdateResponse)).MethodHandle;
			}
			Log.Info("HandlePlayerGroupInfoUpdateResponse failed response", new object[0]);
			if (response.LocalizedFailure != null)
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
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, response.LocalizedFailure.ToString(), StringUtil.TR("Ok", "Global"), null, -1, false);
			}
			else if (!response.ErrorMessage.IsNullOrEmpty())
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
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format("{0}#NeedsLocalization", response.ErrorMessage), StringUtil.TR("Ok", "Global"), null, -1, false);
			}
			else
			{
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, StringUtil.TR("UnknownErrorTryAgain", "Frontend"), StringUtil.TR("Ok", "Global"), null, -1, false);
			}
			return;
		}
		if (response.OriginalPlayerInfoUpdate != null)
		{
			if (response.OriginalPlayerInfoUpdate.ContextualReadyState != null)
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
				if (response.OriginalPlayerInfoUpdate.ContextualReadyState.Value.ReadyState == ReadyState.Ready)
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
					if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Practice)
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
						if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Solo)
						{
							goto IL_189;
						}
					}
					this.CreateGame(ClientGameManager.Get().GroupInfo.SelectedQueueType, null);
				}
			}
			IL_189:;
		}
		else
		{
			Log.Error("HandlePlayerGroupInfoUpdateResponse :: OriginalPlayerInfoUpdate is null for player {0}!! This should never happen!", new object[]
			{
				ClientGameManager.Get().Handle
			});
		}
	}

	private void LeaveQueue()
	{
		if (this.m_pendingLeaveQueueRequest)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.LeaveQueue()).MethodHandle;
			}
			return;
		}
		this.m_pendingLeaveQueueRequest = true;
		ClientGameManager.Get().LobbyInterface.LeaveQueue(delegate(LeaveMatchmakingQueueResponse response)
		{
			this.m_pendingLeaveQueueRequest = false;
			if (response.Success)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GroupCharacterSelect.<LeaveQueue>m__0(LeaveMatchmakingQueueResponse)).MethodHandle;
				}
				this.m_inQueue = false;
				if (this.IsReady())
				{
					UICharacterSelectScreenController.Get().SetReady(false, true);
				}
			}
		});
	}

	public void ForceJoinQueue()
	{
		this.m_pendingLeaveQueueRequest = false;
		this.m_inQueue = true;
		NavigationBar.Get().UpdateStatusMessage();
	}

	public unsafe bool CanJoinQueue(out string reason)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
		GameType selectedQueueType = groupInfo.SelectedQueueType;
		if (selectedQueueType.IsQueueable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.CanJoinQueue(string*)).MethodHandle;
			}
			if (!groupInfo.InAGroup && this.m_inQueue)
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
				reason = StringUtil.TR("AlreadyQueued", "Frontend");
				return false;
			}
		}
		reason = string.Empty;
		return true;
	}

	public void JoinQueue(GameType gameType)
	{
		if (this.m_inQueue)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.JoinQueue(GameType)).MethodHandle;
			}
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), StringUtil.TR("CantJoinAlreadyQueued", "Frontend"), StringUtil.TR("Ok", "Global"), null, -1, false);
			return;
		}
		if (this.m_pendingJoinQueueRequest)
		{
			return;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		BotDifficulty? allyDifficulty = null;
		BotDifficulty? enemyDifficulty = null;
		if (!gameType.IsHumanVsHumanGame())
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
			allyDifficulty = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().AllyBotDifficultyToDisplay);
			enemyDifficulty = new BotDifficulty?((BotDifficulty)UICharacterScreen.GetCurrentSpecificState().EnemyBotDifficultyToDisplay);
		}
		this.m_pendingJoinQueueRequest = true;
		clientGameManager.JoinQueue(gameType, allyDifficulty, enemyDifficulty, delegate(JoinMatchmakingQueueResponse response)
		{
			this.m_pendingJoinQueueRequest = false;
			if (response.Success)
			{
				this.ForceJoinQueue();
			}
			else
			{
				string description;
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GroupCharacterSelect.<JoinQueue>m__1(JoinMatchmakingQueueResponse)).MethodHandle;
					}
					description = response.LocalizedFailure.ToString();
				}
				else if (response.ErrorMessage.IsNullOrEmpty())
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
					description = StringUtil.TR("UnknownErrorTryAgain", "Global");
				}
				else
				{
					description = string.Format("{0}#NeedsLocalization", response.ErrorMessage);
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, description, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
		});
	}

	public void NotifyEnteredCharacterSelect()
	{
		this.m_pendingLeaveQueueRequest = false;
		this.m_inQueue = false;
		this.m_readyForSoloGame = false;
	}

	public void NotifyEnteredRankModeDraft()
	{
		this.m_inQueue = false;
	}

	protected override void OnEnter()
	{
		GameManager gameManager = GameManager.Get();
		this.m_receivedGameInfoNotification = false;
		this.m_gameSelecting = false;
		this.m_readyForSoloGame = false;
		AudioManager.GetMixerSnapshotManager().SetMix_Menu();
		AppState_GroupCharacterSelect.ShowScreen();
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification += this.HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer += this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification += this.HandleGameInfoNotification;
		clientGameManager.OnLobbyStatusNotification += this.HandleStatusNotification;
		clientGameManager.OnServerQueueConfigurationUpdateNotification += this.HandleGameTypesAvailability;
		gameManager.OnGameAssembling += this.HandleGameAssembling;
		gameManager.OnGameSelecting += this.HandleGameSelecting;
		gameManager.OnGameLaunched += this.HandleGameLaunched;
		gameManager.OnGameStopped += this.HandleGameStopped;
		if (this.m_wasRequeued)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.OnEnter()).MethodHandle;
			}
			this.m_inQueue = true;
			this.m_wasRequeued = false;
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Requeued", "Global"), StringUtil.TR("SomeoneDroppedReaddedQueue", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
			UICharacterSelectScreenController.Get().SetReady(true, true);
		}
		else
		{
			this.CheckForPreviousGame();
		}
		this.CheckForQueueParole();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndSelectionChatterCue, null);
	}

	internal static void ShowScreen()
	{
		GameManager gameManager = GameManager.Get();
		UIFrontEnd.Get().m_frontEndNavPanel.SetPlayMenuCatgeoryVisible(true);
		UICharacterSelectScreenController.Get().QuickPlaySetup(gameManager.GameInfo);
		UICharacterSelectScreenController.Get().QuickPlayUpdateCharacters(gameManager.GameplayOverrides, false, false);
		UICharacterSelectScreen.Get().SetGameSettingsButtonVisibility(false);
		UICharacterSelectScreenController.Get().NotifyGroupUpdate();
		UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect, false);
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.ShowScreen()).MethodHandle;
			}
			if (ClientGameManager.Get().GroupInfo != null)
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
				if (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
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
					UICharacterSelectScreenController.Get().SetupForRanked(true);
				}
			}
		}
	}

	public void HandleGameTypesAvailability(ServerQueueConfigurationUpdateNotification notification)
	{
		UIPlayCategoryMenu.Get().UpdateGameTypeAvailability(notification.GameTypeAvailabilies);
	}

	public void HandleStatusNotification(LobbyStatusNotification notification)
	{
		UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
	}

	public void HandleQueueStatusNotification(MatchmakingQueueStatusNotification notification)
	{
		NavigationBar.Get().UpdateStatusMessage();
	}

	private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
	{
		this.m_inQueue = false;
		AppState_LandingPage.Get().Enter(lastLobbyErrorMessage, false);
	}

	private void HandleGameAssembling()
	{
		bool flag;
		if (!AppState.IsInGame() && GameManager.Get().GameInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.HandleGameAssembling()).MethodHandle;
			}
			if (GameManager.Get().GameInfo.IsCustomGame)
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
				flag = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
				goto IL_68;
			}
		}
		flag = false;
		IL_68:
		bool flag2 = flag;
		if (GameManager.Get().GameInfo.AcceptTimeout != TimeSpan.Zero)
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
		if (flag2)
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
			AppState_CharacterSelect.Get().Enter();
		}
	}

	private void HandleGameInfoNotification(GameInfoNotification notification)
	{
		this.m_receivedGameInfoNotification = true;
		if (this.m_receivedGameInfoNotification && this.m_gameSelecting)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.HandleGameInfoNotification(GameInfoNotification)).MethodHandle;
			}
			if (UIMatchStartPanel.Get() != null)
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
				UIMatchStartPanel.Get().HandleGameStatusChanged(notification);
			}
			if (ClientGameManager.Get().GroupInfo.SelectedQueueType != GameType.Ranked)
			{
				AppState_CharacterSelect.Get().Enter();
			}
		}
		if (notification.GameInfo.GameConfig.GameType == GameType.Coop)
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
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SelectedEnemyBotDifficulty = new int?((int)notification.GameInfo.SelectedBotSkillTeamB)
			});
		}
	}

	private void HandleGameSelecting()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.GroupInfo.SelectedQueueType.IsQueueable() && !GameManager.Get().GameInfo.IsCustomGame)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.HandleGameSelecting()).MethodHandle;
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.NotifyMatchFound);
			WinUtils.FlashWindow();
			WinUtils.BringApplicationToFront();
			QuestListPanel.Get().SetVisible(false, false, false);
			this.m_gameSelecting = true;
		}
		if (this.m_receivedGameInfoNotification && this.m_gameSelecting)
		{
			AppState_CharacterSelect.Get().Enter();
		}
		if (GameManager.Get().GameInfo.IsCustomGame)
		{
			AppState_CharacterSelect.Get().Enter();
			UIRankedModeSelectScreen.Get().SetVisible(false);
		}
	}

	protected override void OnLeave()
	{
		GameManager gameManager = GameManager.Get();
		gameManager.OnGameLaunched -= this.HandleGameLaunched;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		clientGameManager.OnQueueStatusNotification -= this.HandleQueueStatusNotification;
		clientGameManager.OnDisconnectedFromLobbyServer -= this.HandleDisconnectedFromLobbyServer;
		clientGameManager.OnGameInfoNotification -= this.HandleGameInfoNotification;
		clientGameManager.OnLobbyStatusNotification -= this.HandleStatusNotification;
		clientGameManager.OnServerQueueConfigurationUpdateNotification -= this.HandleGameTypesAvailability;
		gameManager.OnGameAssembling -= this.HandleGameAssembling;
		gameManager.OnGameSelecting -= this.HandleGameSelecting;
		gameManager.OnGameLaunched -= this.HandleGameLaunched;
		gameManager.OnGameStopped -= this.HandleGameStopped;
		this.m_readyForSoloGame = false;
		if (this.m_messageBox != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.OnLeave()).MethodHandle;
			}
			this.m_messageBox.Close();
			this.m_messageBox = null;
		}
		UICharacterScreen.Get().DoRefreshFunctions(0x80);
	}

	private void CheckForPreviousGame()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AppState_GroupCharacterSelect.CheckForPreviousGame()).MethodHandle;
			}
			if (clientGameManager.IsRegistered)
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
				if (clientGameManager.IsReady)
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
					clientGameManager.RequestPreviousGameInfo(delegate(PreviousGameInfoResponse response)
					{
						if (AppState.GetCurrent() != this)
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(AppState_GroupCharacterSelect.<CheckForPreviousGame>m__2(PreviousGameInfoResponse)).MethodHandle;
							}
							return;
						}
						if (response.PreviousGameInfo != null)
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
							if (response.PreviousGameInfo.IsQueuedGame || response.PreviousGameInfo.IsCustomGame)
							{
								if (response.PreviousGameInfo.GameConfig.TotalHumanPlayers >= 2)
								{
									this.PromptToRejoinGame(response.PreviousGameInfo);
									return;
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
					});
				}
			}
		}
	}

	private void PromptToRejoinGame(LobbyGameInfo previousGameInfo)
	{
		UINewUserFlowManager.HideDisplay();
		this.m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("Reconnect", "Global"), string.Format(StringUtil.TR("ReconnectUnderDevelopment", "Global"), previousGameInfo.GameConfig.GameType.GetDisplayName()), StringUtil.TR("Reconnect", "Global"), StringUtil.TR("Cancel", "Global"), delegate(UIDialogBox UIDialogBox)
		{
			Log.Info("Attempting to reconnect!", new object[0]);
			ClientGameManager.Get().RejoinGame(true, null);
			this.m_messageBox = null;
		}, delegate(UIDialogBox UIDialogBox)
		{
			Log.Info("Decided not to reconnect!", new object[0]);
			ClientGameManager.Get().RejoinGame(false, null);
			this.m_messageBox = null;
		}, false, false);
	}

	private void Awake()
	{
		AppState_GroupCharacterSelect.s_instance = this;
	}
}
