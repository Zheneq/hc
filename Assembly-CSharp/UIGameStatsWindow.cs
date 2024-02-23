using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
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
		switch (page)
		{
			case StatsPage.Mods:
				m_StatsBtn.SetSelected(false, false, string.Empty, string.Empty);
				m_ModsBtn.SetSelected(true, false, string.Empty, string.Empty);
				UIManager.SetGameObjectActive(m_statsLabelHeaders, false);
				UIManager.SetGameObjectActive(m_modLabelHeaders, true);
				break;
			case StatsPage.Numbers:
				m_StatsBtn.SetSelected(true, false, string.Empty, string.Empty);
				m_ModsBtn.SetSelected(false, false, string.Empty, string.Empty);
				UIManager.SetGameObjectActive(m_statsLabelHeaders, true);
				UIManager.SetGameObjectActive(m_modLabelHeaders, false);
				break;
		}
		m_currentPage = page;
		foreach (UIGameOverPlayerEntry uIGameOverPlayerEntry in m_friendlyTeam)
		{
			uIGameOverPlayerEntry.SetStatPage(m_currentPage);
		}
		foreach (UIGameOverPlayerEntry uIGameOverPlayerEntry2 in m_enemyTeam)
		{
			uIGameOverPlayerEntry2.SetStatPage(m_currentPage);
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
		switch (m_currentPage)
		{
			case StatsPage.Mods:
				SetStatePage(StatsPage.Numbers);
				break;
			case StatsPage.Numbers:
				SetStatePage(StatsPage.Mods);
				break;
		}
	}

	public void SetupTeamMemberList(PersistedCharacterMatchData matchData)
	{
		if (matchData == null)
		{
			return;
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
			m_frontendHeaderTurnTime.text = string.Format(
				StringUtil.TR("StatsTurnTime", "Frontend"),
				matchData.MatchComponent.NumOfTurns,
				StringUtil.FormatTime((int)matchResults.GameTime));
		}
	}

	public void SetupTeamMemberList(MatchResultsStats stats)
	{
		if (stats == null
		    || stats.FriendlyStatlines == null
		    || stats.EnemyStatlines == null)
		{
			return;
		}
		for (int i = 0; i < Math.Min(stats.FriendlyStatlines.Length, m_friendlyTeam.Length); i++)
		{
			UIManager.SetGameObjectActive(m_friendlyTeam[i], true);
			m_friendlyTeam[i].Setup(stats.FriendlyStatlines[i]);
		}

		for (int i = Math.Min(stats.FriendlyStatlines.Length, m_friendlyTeam.Length); i < m_friendlyTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(m_friendlyTeam[i], false);
		}

		for (int i = 0; i < Math.Min(stats.EnemyStatlines.Length, m_enemyTeam.Length); i++)
		{
			UIManager.SetGameObjectActive(m_enemyTeam[i], true);
			m_enemyTeam[i].Setup(stats.EnemyStatlines[i]);
		}

		for (int i = Math.Min(stats.EnemyStatlines.Length, m_enemyTeam.Length); i < m_enemyTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(m_enemyTeam[i], false);
		}

		Vector2 sizeDelta = m_resizePanel.sizeDelta;
		sizeDelta.y = m_fullPanelHeight - 50f * (5 - stats.EnemyStatlines.Length);
		m_resizePanel.sizeDelta = sizeDelta;
		if (stats.RedScore == 0 && stats.BlueScore == 0)
		{
			m_frontendHeaderRedTeamScore.text = string.Empty;
			m_frontendHeaderBlueTeamScore.text = string.Empty;
		}
		else
		{
			m_frontendHeaderRedTeamScore.text = string.Format(StringUtil.TR("StatsRedTeamScore", "Frontend"), stats.RedScore.ToString());
			m_frontendHeaderBlueTeamScore.text = string.Format(StringUtil.TR("StatsBlueTeamScore", "Frontend"), stats.BlueScore.ToString());
		}

		if (m_frontendHeaderObjective != null)
		{
			m_frontendHeaderObjective.text = !stats.VictoryCondition.IsNullOrEmpty()
				? string.Format(StringUtil.TR(stats.VictoryCondition), stats.VictoryConditionTurns)
				: string.Empty;
		}

		UIManager.SetGameObjectActive(m_frontendHeaderVictory, false);
		UIManager.SetGameObjectActive(m_frontendHeaderDefeat, false);
		m_frontendHeaderStage.text = string.Empty;
		m_frontendHeaderTurnTime.text = string.Empty;
		bool secretButtonClicked = Options_UI.Get().m_secretButtonClicked;
		if (m_debugStatus != null)
		{
			UIManager.SetGameObjectActive(m_debugStatus, secretButtonClicked);
		}

		if (m_debugLabels != null)
		{
			UIManager.SetGameObjectActive(m_debugLabels, secretButtonClicked);
		}

		if (!secretButtonClicked || !m_debugStatusString)
		{
			return;
		}
		
		Team teamId = GameManager.Get().PlayerInfo.TeamId;
		Team myTeam = teamId == Team.TeamB ? Team.TeamB : Team.TeamA;
		Team enemyTeam = myTeam.OtherTeam();
		float totalSeconds = -1f;
		if (ObjectivePoints.Get() != null)
		{
			totalSeconds = ObjectivePoints.Get().GetTotalMinutesOnMatchEnd() * 60f;
		}

		string timeStr = string.Empty;
		if (totalSeconds > 0f)
		{
			int minutes = (int)(totalSeconds / 60f);
			int seconds = (int)totalSeconds % 60;
			timeStr = seconds < 10 ? new StringBuilder().Append(minutes).Append(":0").Append(seconds).ToString() : new StringBuilder().Append(minutes).Append(":").Append(seconds).ToString();
		}
		else if (UITimerPanel.Get() != null)
		{
			timeStr = UITimerPanel.Get().m_timeLabel.text;
		}

		int currentTurn = GameFlowData.Get().CurrentTurn;
		int scoreA = 0;
		int scoreB = 0;
		if (ObjectivePoints.Get() != null)
		{
			scoreA = ObjectivePoints.Get().GetPointsForTeam(myTeam);
			scoreB = ObjectivePoints.Get().GetPointsForTeam(enemyTeam);
		}

		int contributionA = 0;
		int contributionB = 0;
		foreach (ActorData current in GameFlowData.Get().GetPlayerAndBotTeamMembers(myTeam))
		{
			contributionA += current.GetActorBehavior().totalPlayerContribution;
		}

		foreach (ActorData item in GameFlowData.Get().GetPlayerAndBotTeamMembers(enemyTeam))
		{
			contributionB += item.GetActorBehavior().totalPlayerContribution;
		}

		UIGameOverPanel.TeamELOs matchmaking;
		UIGameOverPanel.TeamELOs account;
		UIGameOverPanel.TeamELOs character;
		GenerateTeamELOValues(teamId, out matchmaking, out account, out character);
		m_debugStatusString.text =
			new StringBuilder().Append("<color=#cfcfcf>Time: <color=yellow>").Append(timeStr).Append("</color>     Turn: <color=yellow>").Append(currentTurn).Append("</color>").ToString();
		m_debugStatusString.text +=
			new StringBuilder().Append("     Score: A: <color=#007fff>").Append(scoreA).Append("</color> B: <color=#ff3f3f>").Append(scoreB).Append("</color>").ToString();
		m_debugStatusString.text +=
			new StringBuilder().Append("     Contribution A: <color=#007fff>").Append(contributionA).Append("</color> B: <color=#ff3f3f>").Append(contributionB).Append("</color>").ToString();
		m_debugStatusString.text += matchmaking.ToHTML("     Used");
		m_debugStatusString.text += account.ToHTML("     Acc");
		m_debugStatusString.text += character.ToHTML("     Char");
	}

	private void GenerateTeamELOValues(
		Team ourTeam,
		out UIGameOverPanel.TeamELOs matchmaking,
		out UIGameOverPanel.TeamELOs account,
		out UIGameOverPanel.TeamELOs character)
	{
		matchmaking = new UIGameOverPanel.TeamELOs(ourTeam);
		account = new UIGameOverPanel.TeamELOs(ourTeam);
		character = new UIGameOverPanel.TeamELOs(ourTeam);
		Dictionary<int, ForbiddenDevKnowledge> forbiddenDevKnowledge = GameManager.Get().ForbiddenDevKnowledge;
		if (forbiddenDevKnowledge.IsNullOrEmpty())
		{
			return;
		}

		foreach (LobbyPlayerInfo current in GameManager.Get().TeamInfo.TeamPlayerInfo)
		{
			ForbiddenDevKnowledge value;
			if (forbiddenDevKnowledge.TryGetValue(current.PlayerId, out value))
			{
				matchmaking.AddPlayer(current.TeamId, value.UsedMatchmakingElo);
				account.AddPlayer(current.TeamId, value.AccMatchmakingElo);
				character.AddPlayer(current.TeamId, value.CharMatchmakingElo);
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
		foreach (UIGameOverPlayerEntry entry in m_friendlyTeam)
		{
			entry.m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
		}
		foreach (UIGameOverPlayerEntry entry in m_enemyTeam)
		{
			entry.m_ContributionHitBoxTooltip.spriteController.ForceSetPointerEntered(false);
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
		if (eventType == GameEventManager.EventType.GameTeardown)
		{
			SetToggleStatsVisible(false);
		}
	}
}
