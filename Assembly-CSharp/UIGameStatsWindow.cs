using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameStatsWindow : UIScene, IGameEventListener
{
	public RectTransform m_container;

	public RectTransform m_resizePanel;

	public _SelectableBtn m_StatsBtn;

	public _SelectableBtn m_ModsBtn;

	public _SelectableBtn m_closeBtn;

	public GameObject m_debugLabels;

	public GameObject m_debugStatus;

	public TextMeshProUGUI m_debugStatusString;

	public UIGameOverPlayerEntry[] m_friendlyTeam;

	public UIGameOverPlayerEntry[] m_enemyTeam;

	public RectTransform m_statsLabelHeaders;

	public RectTransform m_modLabelHeaders;

	public RectTransform m_panelOuterContainer;

	public RectTransform m_frontendPosition;

	public RectTransform m_ingamePosition;

	public GameObject m_frontendHeader;

	public GameObject m_frontendHeaderVictory;

	public GameObject m_frontendHeaderDefeat;

	public TextMeshProUGUI m_frontendHeaderRedTeamScore;

	public TextMeshProUGUI m_frontendHeaderBlueTeamScore;

	public TextMeshProUGUI m_frontendHeaderTurnTime;

	public TextMeshProUGUI m_frontendHeaderStage;

	public TextMeshProUGUI m_frontendHeaderObjective;

	private UIGameStatsWindow.StatsPage m_currentPage;

	private float m_fullPanelHeight;

	private static UIGameStatsWindow s_instance;

	public static UIGameStatsWindow Get()
	{
		return UIGameStatsWindow.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameStats;
	}

	public override void Awake()
	{
		UIGameStatsWindow.s_instance = this;
		this.m_StatsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnStatsBtnClicked);
		this.m_ModsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnModsBtnClicked);
		this.m_closeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnCloseBtnClicked);
		this.m_closeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		this.SetToggleStatsVisible(false, false);
		this.m_fullPanelHeight = this.m_resizePanel.sizeDelta.y;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		base.Awake();
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
	}

	public void Refresh()
	{
		this.SetStatePage(this.m_currentPage);
	}

	public void SetStatePage(UIGameStatsWindow.StatsPage page)
	{
		if (page == UIGameStatsWindow.StatsPage.Mods)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.SetStatePage(UIGameStatsWindow.StatsPage)).MethodHandle;
			}
			this.m_StatsBtn.SetSelected(false, false, string.Empty, string.Empty);
			this.m_ModsBtn.SetSelected(true, false, string.Empty, string.Empty);
			UIManager.SetGameObjectActive(this.m_statsLabelHeaders, false, null);
			UIManager.SetGameObjectActive(this.m_modLabelHeaders, true, null);
		}
		else if (page == UIGameStatsWindow.StatsPage.Numbers)
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
			this.m_StatsBtn.SetSelected(true, false, string.Empty, string.Empty);
			this.m_ModsBtn.SetSelected(false, false, string.Empty, string.Empty);
			UIManager.SetGameObjectActive(this.m_statsLabelHeaders, true, null);
			UIManager.SetGameObjectActive(this.m_modLabelHeaders, false, null);
		}
		this.m_currentPage = page;
		foreach (UIGameOverPlayerEntry uigameOverPlayerEntry in this.m_friendlyTeam)
		{
			uigameOverPlayerEntry.SetStatPage(this.m_currentPage);
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
		foreach (UIGameOverPlayerEntry uigameOverPlayerEntry2 in this.m_enemyTeam)
		{
			uigameOverPlayerEntry2.SetStatPage(this.m_currentPage);
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void OnStatsBtnClicked(BaseEventData data)
	{
		this.SetStatePage(UIGameStatsWindow.StatsPage.Numbers);
	}

	public void OnModsBtnClicked(BaseEventData data)
	{
		this.SetStatePage(UIGameStatsWindow.StatsPage.Mods);
	}

	public void OnCloseBtnClicked(BaseEventData data)
	{
		this.ToggleStatsWindow();
	}

	public void OnStatToggleClicked(BaseEventData data)
	{
		if (this.m_currentPage == UIGameStatsWindow.StatsPage.Mods)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.OnStatToggleClicked(BaseEventData)).MethodHandle;
			}
			this.SetStatePage(UIGameStatsWindow.StatsPage.Numbers);
		}
		else if (this.m_currentPage == UIGameStatsWindow.StatsPage.Numbers)
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
			this.SetStatePage(UIGameStatsWindow.StatsPage.Mods);
		}
	}

	public void SetupTeamMemberList(PersistedCharacterMatchData matchData)
	{
		if (matchData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.SetupTeamMemberList(PersistedCharacterMatchData)).MethodHandle;
			}
			return;
		}
		MatchResultsStats matchResults = matchData.MatchDetailsComponent.MatchResults;
		this.SetupTeamMemberList(matchResults);
		UIManager.SetGameObjectActive(this.m_frontendHeaderVictory, matchData.MatchComponent.Result == PlayerGameResult.Win, null);
		UIManager.SetGameObjectActive(this.m_frontendHeaderDefeat, matchData.MatchComponent.Result == PlayerGameResult.Lose, null);
		this.m_frontendHeaderStage.text = GameWideData.Get().GetMapDisplayName(matchData.MatchComponent.MapName);
		if (matchResults.GameTime == 0f)
		{
			this.m_frontendHeaderTurnTime.text = string.Empty;
		}
		else
		{
			this.m_frontendHeaderTurnTime.text = string.Format(StringUtil.TR("StatsTurnTime", "Frontend"), matchData.MatchComponent.NumOfTurns, StringUtil.FormatTime((int)matchResults.GameTime));
		}
	}

	public void SetupTeamMemberList(MatchResultsStats stats)
	{
		if (stats != null && stats.FriendlyStatlines != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.SetupTeamMemberList(MatchResultsStats)).MethodHandle;
			}
			if (stats.EnemyStatlines != null)
			{
				for (int i = 0; i < Math.Min(stats.FriendlyStatlines.Length, this.m_friendlyTeam.Length); i++)
				{
					UIManager.SetGameObjectActive(this.m_friendlyTeam[i], true, null);
					this.m_friendlyTeam[i].Setup(stats.FriendlyStatlines[i]);
				}
				for (int j = Math.Min(stats.FriendlyStatlines.Length, this.m_friendlyTeam.Length); j < this.m_friendlyTeam.Length; j++)
				{
					UIManager.SetGameObjectActive(this.m_friendlyTeam[j], false, null);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int k = 0; k < Math.Min(stats.EnemyStatlines.Length, this.m_enemyTeam.Length); k++)
				{
					UIManager.SetGameObjectActive(this.m_enemyTeam[k], true, null);
					this.m_enemyTeam[k].Setup(stats.EnemyStatlines[k]);
				}
				for (int l = Math.Min(stats.EnemyStatlines.Length, this.m_enemyTeam.Length); l < this.m_enemyTeam.Length; l++)
				{
					UIManager.SetGameObjectActive(this.m_enemyTeam[l], false, null);
				}
				Vector2 sizeDelta = this.m_resizePanel.sizeDelta;
				sizeDelta.y = this.m_fullPanelHeight - 50f * (float)(5 - stats.EnemyStatlines.Length);
				this.m_resizePanel.sizeDelta = sizeDelta;
				if (stats.RedScore == 0)
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
					if (stats.BlueScore == 0)
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
						this.m_frontendHeaderRedTeamScore.text = string.Empty;
						this.m_frontendHeaderBlueTeamScore.text = string.Empty;
						goto IL_21C;
					}
				}
				this.m_frontendHeaderRedTeamScore.text = string.Format(StringUtil.TR("StatsRedTeamScore", "Frontend"), stats.RedScore.ToString());
				this.m_frontendHeaderBlueTeamScore.text = string.Format(StringUtil.TR("StatsBlueTeamScore", "Frontend"), stats.BlueScore.ToString());
				IL_21C:
				if (this.m_frontendHeaderObjective != null)
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
					if (stats.VictoryCondition.IsNullOrEmpty())
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
						this.m_frontendHeaderObjective.text = string.Empty;
					}
					else
					{
						this.m_frontendHeaderObjective.text = string.Format(StringUtil.TR(stats.VictoryCondition), stats.VictoryConditionTurns);
					}
				}
				UIManager.SetGameObjectActive(this.m_frontendHeaderVictory, false, null);
				UIManager.SetGameObjectActive(this.m_frontendHeaderDefeat, false, null);
				this.m_frontendHeaderStage.text = string.Empty;
				this.m_frontendHeaderTurnTime.text = string.Empty;
				bool secretButtonClicked = Options_UI.Get().m_secretButtonClicked;
				if (this.m_debugStatus)
				{
					UIManager.SetGameObjectActive(this.m_debugStatus, secretButtonClicked, null);
				}
				if (this.m_debugLabels)
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
					UIManager.SetGameObjectActive(this.m_debugLabels, secretButtonClicked, null);
				}
				if (secretButtonClicked)
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
					if (this.m_debugStatusString)
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
						Team teamId = GameManager.Get().PlayerInfo.TeamId;
						Team team;
						if (teamId == Team.TeamB)
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
							team = Team.TeamB;
						}
						else
						{
							team = Team.TeamA;
						}
						Team team2 = team;
						Team team3 = team2.OtherTeam();
						string arg = string.Empty;
						float num = -1f;
						if (ObjectivePoints.Get() != null)
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
							num = ObjectivePoints.Get().GetTotalMinutesOnMatchEnd() * 60f;
						}
						if (num > 0f)
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
							int num2 = (int)(num / 60f);
							int num3 = (int)num % 0x3C;
							if (num3 < 0xA)
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
								arg = string.Format("{0}:0{1}", num2, num3);
							}
							else
							{
								arg = string.Format("{0}:{1}", num2, num3);
							}
						}
						else if (UITimerPanel.Get() != null)
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
							arg = UITimerPanel.Get().m_timeLabel.text;
						}
						int currentTurn = GameFlowData.Get().CurrentTurn;
						int num4 = 0;
						int num5 = 0;
						if (ObjectivePoints.Get() != null)
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
							num4 = ObjectivePoints.Get().GetPointsForTeam(team2);
							num5 = ObjectivePoints.Get().GetPointsForTeam(team3);
						}
						int num6 = 0;
						int num7 = 0;
						List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team2);
						List<ActorData> playerAndBotTeamMembers2 = GameFlowData.Get().GetPlayerAndBotTeamMembers(team3);
						using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ActorData actorData = enumerator.Current;
								ActorBehavior actorBehavior = actorData.\u000E();
								num6 += actorBehavior.totalPlayerContribution;
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
						foreach (ActorData actorData2 in playerAndBotTeamMembers2)
						{
							ActorBehavior actorBehavior2 = actorData2.\u000E();
							num7 += actorBehavior2.totalPlayerContribution;
						}
						UIGameOverPanel.TeamELOs teamELOs;
						UIGameOverPanel.TeamELOs teamELOs2;
						UIGameOverPanel.TeamELOs teamELOs3;
						this.GenerateTeamELOValues(teamId, out teamELOs, out teamELOs2, out teamELOs3);
						this.m_debugStatusString.text = string.Format("<color=#cfcfcf>Time: <color=yellow>{0}</color>     Turn: <color=yellow>{1}</color>", arg, currentTurn);
						TextMeshProUGUI debugStatusString = this.m_debugStatusString;
						debugStatusString.text += string.Format("     Score: A: <color=#007fff>{0}</color> B: <color=#ff3f3f>{1}</color>", num4, num5);
						TextMeshProUGUI debugStatusString2 = this.m_debugStatusString;
						debugStatusString2.text += string.Format("     Contribution A: <color=#007fff>{0}</color> B: <color=#ff3f3f>{1}</color>", num6, num7);
						TextMeshProUGUI debugStatusString3 = this.m_debugStatusString;
						debugStatusString3.text += teamELOs.ToHTML("     Used");
						TextMeshProUGUI debugStatusString4 = this.m_debugStatusString;
						debugStatusString4.text += teamELOs2.ToHTML("     Acc");
						TextMeshProUGUI debugStatusString5 = this.m_debugStatusString;
						debugStatusString5.text += teamELOs3.ToHTML("     Char");
					}
				}
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
		}
	}

	private unsafe void GenerateTeamELOValues(Team ourTeam, out UIGameOverPanel.TeamELOs matchmaking, out UIGameOverPanel.TeamELOs account, out UIGameOverPanel.TeamELOs character)
	{
		matchmaking = new UIGameOverPanel.TeamELOs(ourTeam);
		account = new UIGameOverPanel.TeamELOs(ourTeam);
		character = new UIGameOverPanel.TeamELOs(ourTeam);
		Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
		if (forbiddenDevKnowledge.IsNullOrEmpty<KeyValuePair<int, ForbiddenDevKnowledge>>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.GenerateTeamELOValues(Team, UIGameOverPanel.TeamELOs*, UIGameOverPanel.TeamELOs*, UIGameOverPanel.TeamELOs*)).MethodHandle;
			}
			return;
		}
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		using (List<LobbyPlayerInfo>.Enumerator enumerator = teamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
				ForbiddenDevKnowledge forbiddenDevKnowledge2;
				if (forbiddenDevKnowledge.TryGetValue(lobbyPlayerInfo.PlayerId, out forbiddenDevKnowledge2))
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
					matchmaking.AddPlayer(lobbyPlayerInfo.TeamId, forbiddenDevKnowledge2.UsedMatchmakingElo);
					account.AddPlayer(lobbyPlayerInfo.TeamId, forbiddenDevKnowledge2.AccMatchmakingElo);
					character.AddPlayer(lobbyPlayerInfo.TeamId, forbiddenDevKnowledge2.CharMatchmakingElo);
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

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_container, visible, null);
	}

	public void SetToggleStatsVisible(bool visible, bool playSound = true)
	{
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		if (visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.SetToggleStatsVisible(bool, bool)).MethodHandle;
			}
			this.SetStatePage(this.m_currentPage);
		}
		if (playSound)
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
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		}
		for (int i = 0; i < this.m_friendlyTeam.Length; i++)
		{
			this.m_friendlyTeam[i].m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
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
		for (int j = 0; j < this.m_enemyTeam.Length; j++)
		{
			this.m_enemyTeam[j].m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
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
	}

	public void ToggleStatsWindow()
	{
		this.SetToggleStatsVisible(!this.m_container.gameObject.activeSelf, true);
	}

	private void Update()
	{
		if (HUD_UI.Get() != null)
		{
			if (this.m_panelOuterContainer.parent != this.m_ingamePosition)
			{
				this.m_panelOuterContainer.SetParent(this.m_ingamePosition);
				this.m_panelOuterContainer.localScale = Vector3.one;
				this.m_panelOuterContainer.anchoredPosition = Vector3.zero;
				this.m_panelOuterContainer.localEulerAngles = Vector3.zero;
				this.m_panelOuterContainer.sizeDelta = Vector2.zero;
			}
		}
		else if (this.m_panelOuterContainer.parent != this.m_frontendPosition)
		{
			this.m_panelOuterContainer.SetParent(this.m_frontendPosition);
			this.m_panelOuterContainer.localScale = Vector3.one;
			this.m_panelOuterContainer.anchoredPosition = Vector3.zero;
			this.m_panelOuterContainer.localEulerAngles = Vector3.zero;
			this.m_panelOuterContainer.sizeDelta = Vector2.zero;
		}
		UIManager.SetGameObjectActive(this.m_frontendHeader, this.m_container.gameObject.activeSelf && HUD_UI.Get() == null, null);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameTeardown)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameStatsWindow.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			this.SetToggleStatsVisible(false, true);
		}
	}

	public enum StatsPage
	{
		Numbers,
		Mods
	}
}
