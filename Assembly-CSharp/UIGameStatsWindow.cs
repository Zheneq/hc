using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameStatsWindow : UIScene, IGameEventListener
{
	public enum StatsPage
	{
		Numbers,
		Mods
	}

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

	private StatsPage m_currentPage;

	private float m_fullPanelHeight;

	private static UIGameStatsWindow s_instance;

	public static UIGameStatsWindow Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameStats;
	}

	public override void Awake()
	{
		s_instance = this;
		m_StatsBtn.spriteController.callback = OnStatsBtnClicked;
		m_ModsBtn.spriteController.callback = OnModsBtnClicked;
		m_closeBtn.spriteController.callback = OnCloseBtnClicked;
		m_closeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.Close;
		SetToggleStatsVisible(false, false);
		Vector2 sizeDelta = m_resizePanel.sizeDelta;
		m_fullPanelHeight = sizeDelta.y;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameTeardown);
		base.Awake();
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameTeardown);
	}

	public void Refresh()
	{
		SetStatePage(m_currentPage);
	}

	public void SetStatePage(StatsPage page)
	{
		if (page == StatsPage.Mods)
		{
			m_StatsBtn.SetSelected(false, false, string.Empty, string.Empty);
			m_ModsBtn.SetSelected(true, false, string.Empty, string.Empty);
			UIManager.SetGameObjectActive(m_statsLabelHeaders, false);
			UIManager.SetGameObjectActive(m_modLabelHeaders, true);
		}
		else if (page == StatsPage.Numbers)
		{
			m_StatsBtn.SetSelected(true, false, string.Empty, string.Empty);
			m_ModsBtn.SetSelected(false, false, string.Empty, string.Empty);
			UIManager.SetGameObjectActive(m_statsLabelHeaders, true);
			UIManager.SetGameObjectActive(m_modLabelHeaders, false);
		}
		m_currentPage = page;
		UIGameOverPlayerEntry[] friendlyTeam = m_friendlyTeam;
		foreach (UIGameOverPlayerEntry uIGameOverPlayerEntry in friendlyTeam)
		{
			uIGameOverPlayerEntry.SetStatPage(m_currentPage);
		}
		while (true)
		{
			UIGameOverPlayerEntry[] enemyTeam = m_enemyTeam;
			foreach (UIGameOverPlayerEntry uIGameOverPlayerEntry2 in enemyTeam)
			{
				uIGameOverPlayerEntry2.SetStatPage(m_currentPage);
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void OnStatsBtnClicked(BaseEventData data)
	{
		SetStatePage(StatsPage.Numbers);
	}

	public void OnModsBtnClicked(BaseEventData data)
	{
		SetStatePage(StatsPage.Mods);
	}

	public void OnCloseBtnClicked(BaseEventData data)
	{
		ToggleStatsWindow();
	}

	public void OnStatToggleClicked(BaseEventData data)
	{
		if (m_currentPage == StatsPage.Mods)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					SetStatePage(StatsPage.Numbers);
					return;
				}
			}
		}
		if (m_currentPage != 0)
		{
			return;
		}
		while (true)
		{
			SetStatePage(StatsPage.Mods);
			return;
		}
	}

	public void SetupTeamMemberList(PersistedCharacterMatchData matchData)
	{
		if (matchData == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		MatchResultsStats matchResults = matchData.MatchDetailsComponent.MatchResults;
		SetupTeamMemberList(matchResults);
		UIManager.SetGameObjectActive(m_frontendHeaderVictory, matchData.MatchComponent.Result == PlayerGameResult.Win);
		UIManager.SetGameObjectActive(m_frontendHeaderDefeat, matchData.MatchComponent.Result == PlayerGameResult.Lose);
		m_frontendHeaderStage.text = GameWideData.Get().GetMapDisplayName(matchData.MatchComponent.MapName);
		if (matchResults.GameTime == 0f)
		{
			m_frontendHeaderTurnTime.text = string.Empty;
		}
		else
		{
			m_frontendHeaderTurnTime.text = string.Format(StringUtil.TR("StatsTurnTime", "Frontend"), matchData.MatchComponent.NumOfTurns, StringUtil.FormatTime((int)matchResults.GameTime));
		}
	}

	public void SetupTeamMemberList(MatchResultsStats stats)
	{
		if (stats == null || stats.FriendlyStatlines == null)
		{
			return;
		}
		while (true)
		{
			if (stats.EnemyStatlines == null)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			for (int i = 0; i < Math.Min(stats.FriendlyStatlines.Length, m_friendlyTeam.Length); i++)
			{
				UIManager.SetGameObjectActive(m_friendlyTeam[i], true);
				m_friendlyTeam[i].Setup(stats.FriendlyStatlines[i]);
			}
			for (int j = Math.Min(stats.FriendlyStatlines.Length, m_friendlyTeam.Length); j < m_friendlyTeam.Length; j++)
			{
				UIManager.SetGameObjectActive(m_friendlyTeam[j], false);
			}
			while (true)
			{
				for (int k = 0; k < Math.Min(stats.EnemyStatlines.Length, m_enemyTeam.Length); k++)
				{
					UIManager.SetGameObjectActive(m_enemyTeam[k], true);
					m_enemyTeam[k].Setup(stats.EnemyStatlines[k]);
				}
				for (int l = Math.Min(stats.EnemyStatlines.Length, m_enemyTeam.Length); l < m_enemyTeam.Length; l++)
				{
					UIManager.SetGameObjectActive(m_enemyTeam[l], false);
				}
				Vector2 sizeDelta = m_resizePanel.sizeDelta;
				sizeDelta.y = m_fullPanelHeight - 50f * (float)(5 - stats.EnemyStatlines.Length);
				m_resizePanel.sizeDelta = sizeDelta;
				if (stats.RedScore == 0)
				{
					if (stats.BlueScore == 0)
					{
						m_frontendHeaderRedTeamScore.text = string.Empty;
						m_frontendHeaderBlueTeamScore.text = string.Empty;
						goto IL_021c;
					}
				}
				m_frontendHeaderRedTeamScore.text = string.Format(StringUtil.TR("StatsRedTeamScore", "Frontend"), stats.RedScore.ToString());
				m_frontendHeaderBlueTeamScore.text = string.Format(StringUtil.TR("StatsBlueTeamScore", "Frontend"), stats.BlueScore.ToString());
				goto IL_021c;
				IL_021c:
				if (m_frontendHeaderObjective != null)
				{
					if (stats.VictoryCondition.IsNullOrEmpty())
					{
						m_frontendHeaderObjective.text = string.Empty;
					}
					else
					{
						m_frontendHeaderObjective.text = string.Format(StringUtil.TR(stats.VictoryCondition), stats.VictoryConditionTurns);
					}
				}
				UIManager.SetGameObjectActive(m_frontendHeaderVictory, false);
				UIManager.SetGameObjectActive(m_frontendHeaderDefeat, false);
				m_frontendHeaderStage.text = string.Empty;
				m_frontendHeaderTurnTime.text = string.Empty;
				bool secretButtonClicked = Options_UI.Get().m_secretButtonClicked;
				if ((bool)m_debugStatus)
				{
					UIManager.SetGameObjectActive(m_debugStatus, secretButtonClicked);
				}
				if ((bool)m_debugLabels)
				{
					UIManager.SetGameObjectActive(m_debugLabels, secretButtonClicked);
				}
				if (!secretButtonClicked)
				{
					return;
				}
				while (true)
				{
					if (!m_debugStatusString)
					{
						return;
					}
					while (true)
					{
						Team teamId = GameManager.Get().PlayerInfo.TeamId;
						int num;
						if (teamId == Team.TeamB)
						{
							num = 1;
						}
						else
						{
							num = 0;
						}
						Team team = (Team)num;
						Team team2 = team.OtherTeam();
						string arg = string.Empty;
						float num2 = -1f;
						if (ObjectivePoints.Get() != null)
						{
							num2 = ObjectivePoints.Get().GetTotalMinutesOnMatchEnd() * 60f;
						}
						if (num2 > 0f)
						{
							int num3 = (int)(num2 / 60f);
							int num4 = (int)num2 % 60;
							if (num4 < 10)
							{
								arg = $"{num3}:0{num4}";
							}
							else
							{
								arg = $"{num3}:{num4}";
							}
						}
						else if (UITimerPanel.Get() != null)
						{
							arg = UITimerPanel.Get().m_timeLabel.text;
						}
						int currentTurn = GameFlowData.Get().CurrentTurn;
						int num5 = 0;
						int num6 = 0;
						if (ObjectivePoints.Get() != null)
						{
							num5 = ObjectivePoints.Get().GetPointsForTeam(team);
							num6 = ObjectivePoints.Get().GetPointsForTeam(team2);
						}
						int num7 = 0;
						int num8 = 0;
						List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team);
						List<ActorData> playerAndBotTeamMembers2 = GameFlowData.Get().GetPlayerAndBotTeamMembers(team2);
						using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ActorData current = enumerator.Current;
								ActorBehavior actorBehavior = current.GetActorBehavior();
								num7 += actorBehavior.totalPlayerContribution;
							}
						}
						foreach (ActorData item in playerAndBotTeamMembers2)
						{
							ActorBehavior actorBehavior2 = item.GetActorBehavior();
							num8 += actorBehavior2.totalPlayerContribution;
						}
						GenerateTeamELOValues(teamId, out UIGameOverPanel.TeamELOs matchmaking, out UIGameOverPanel.TeamELOs account, out UIGameOverPanel.TeamELOs character);
						m_debugStatusString.text = $"<color=#cfcfcf>Time: <color=yellow>{arg}</color>     Turn: <color=yellow>{currentTurn}</color>";
						m_debugStatusString.text += $"     Score: A: <color=#007fff>{num5}</color> B: <color=#ff3f3f>{num6}</color>";
						m_debugStatusString.text += $"     Contribution A: <color=#007fff>{num7}</color> B: <color=#ff3f3f>{num8}</color>";
						m_debugStatusString.text += matchmaking.ToHTML("     Used");
						m_debugStatusString.text += account.ToHTML("     Acc");
						m_debugStatusString.text += character.ToHTML("     Char");
						return;
					}
				}
			}
		}
	}

	private void GenerateTeamELOValues(Team ourTeam, out UIGameOverPanel.TeamELOs matchmaking, out UIGameOverPanel.TeamELOs account, out UIGameOverPanel.TeamELOs character)
	{
		matchmaking = new UIGameOverPanel.TeamELOs(ourTeam);
		account = new UIGameOverPanel.TeamELOs(ourTeam);
		character = new UIGameOverPanel.TeamELOs(ourTeam);
		Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
		if (forbiddenDevKnowledge.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		using (List<LobbyPlayerInfo>.Enumerator enumerator = teamInfo.TeamPlayerInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LobbyPlayerInfo current = enumerator.Current;
				if (forbiddenDevKnowledge.TryGetValue(current.PlayerId, out ForbiddenDevKnowledge value))
				{
					matchmaking.AddPlayer(current.TeamId, value.UsedMatchmakingElo);
					account.AddPlayer(current.TeamId, value.AccMatchmakingElo);
					character.AddPlayer(current.TeamId, value.CharMatchmakingElo);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_container, visible);
	}

	public void SetToggleStatsVisible(bool visible, bool playSound = true)
	{
		UIManager.SetGameObjectActive(m_container, visible);
		if (visible)
		{
			SetStatePage(m_currentPage);
		}
		if (playSound)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		}
		for (int i = 0; i < m_friendlyTeam.Length; i++)
		{
			m_friendlyTeam[i].m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
		}
		while (true)
		{
			for (int j = 0; j < m_enemyTeam.Length; j++)
			{
				m_enemyTeam[j].m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void ToggleStatsWindow()
	{
		SetToggleStatsVisible(!m_container.gameObject.activeSelf);
	}

	private void Update()
	{
		if (HUD_UI.Get() != null)
		{
			if (m_panelOuterContainer.parent != m_ingamePosition)
			{
				m_panelOuterContainer.SetParent(m_ingamePosition);
				m_panelOuterContainer.localScale = Vector3.one;
				m_panelOuterContainer.anchoredPosition = Vector3.zero;
				m_panelOuterContainer.localEulerAngles = Vector3.zero;
				m_panelOuterContainer.sizeDelta = Vector2.zero;
			}
		}
		else if (m_panelOuterContainer.parent != m_frontendPosition)
		{
			m_panelOuterContainer.SetParent(m_frontendPosition);
			m_panelOuterContainer.localScale = Vector3.one;
			m_panelOuterContainer.anchoredPosition = Vector3.zero;
			m_panelOuterContainer.localEulerAngles = Vector3.zero;
			m_panelOuterContainer.sizeDelta = Vector2.zero;
		}
		UIManager.SetGameObjectActive(m_frontendHeader, m_container.gameObject.activeSelf && HUD_UI.Get() == null);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameTeardown)
		{
			return;
		}
		while (true)
		{
			SetToggleStatsVisible(false);
			return;
		}
	}
}
