using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDisplay : MonoBehaviour
{
	public Animator m_animationController;
	public Image m_background;
	public Image m_centerPiece;
	public Image m_tutorialBar;
	public TextMeshProUGUI m_tutorialText;
	public GameObject m_tutorialCameraControlsPanel;
	public GameObject m_tutorialCombatPhasePanel;
	public GameObject m_tutorialDashPhasePanel;
	public GameObject m_tutorialPrepPhasePanel;
	public UIPlayerStatus[] m_teamPlayerIcons;
	public UIPlayerStatus[] m_enemyPlayerIcons;

	private bool m_PanelVisibility;
	private List<string> m_animsToPlayQueue;
	private string showDisplayAnimName = "TopDisplayPanelShow";

	private void Awake()
	{
		UIManager.SetGameObjectActive(m_tutorialBar, false);
		UIManager.SetGameObjectActive(m_tutorialText, false);
		UIManager.SetGameObjectActive(m_tutorialCameraControlsPanel, false);
		UIManager.SetGameObjectActive(m_tutorialCombatPhasePanel, false);
		UIManager.SetGameObjectActive(m_tutorialDashPhasePanel, false);
		UIManager.SetGameObjectActive(m_tutorialPrepPhasePanel, false);
	}

	private void Start()
	{
		m_animsToPlayQueue = new List<string>();
	}

	private void Update()
	{
		if (!IsAnimationPlaying() && m_animsToPlayQueue.Count > 0)
		{
			SetDisplaysVisible(true);
			m_animsToPlayQueue.RemoveAt(0);
		}
		ProcessTeamsForSpectator();
	}

	public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
	{
		foreach (var icon in m_teamPlayerIcons)
		{
			if (icon.ActorDataRef == theActor)
			{
				icon.UpdateCatalysts(cardAbilities);
				break;
			}
		}

		foreach (var icon in m_enemyPlayerIcons)
		{
			if (icon.ActorDataRef == theActor)
			{
				icon.UpdateCatalysts(cardAbilities);
				return;
			}
		}
	}

	private bool IsAnimationPlaying()
	{
		if (m_animationController.GetCurrentAnimatorClipInfo(0) != null
		    && m_animationController.GetCurrentAnimatorClipInfo(0).Length > 0)
		{
			return m_animationController.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EmptyAnimation";
		}
		return false;
	}

	private void SetDisplaysVisible(bool visible)
	{
		int num = m_teamPlayerIcons[0].IsActiveDisplay()
			? GameFlowData.Get().GetPlayerAndBotTeamMembers(m_teamPlayerIcons[0].GetTeam()).Count
			: m_teamPlayerIcons.Length;
		for (int i = 0; i < m_teamPlayerIcons.Length; i++)
		{
			if (i < num && m_teamPlayerIcons[i].IsActiveDisplay())
			{
				bool doActive =
					(SinglePlayerManager.Get() == null || !SinglePlayerManager.Get().GetTeamPlayerIconForceOff(i)) 
					&& visible;
				UIManager.SetGameObjectActive(m_teamPlayerIcons[i], doActive);
			}
			else
			{
				UIManager.SetGameObjectActive(m_teamPlayerIcons[i], false);
			}
		}
		
		num = m_enemyPlayerIcons[0].IsActiveDisplay()
			? GameFlowData.Get().GetPlayerAndBotTeamMembers(m_enemyPlayerIcons[0].GetTeam()).Count
			: m_enemyPlayerIcons.Length;
		for (int j = 0; j < m_enemyPlayerIcons.Length; j++)
		{
			if (j < num && m_enemyPlayerIcons[j].IsActiveDisplay())
			{
				bool doActive =
					(SinglePlayerManager.Get() == null || !SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(j))
					&& visible;
				UIManager.SetGameObjectActive(m_enemyPlayerIcons[j], doActive);
			}
			else
			{
				UIManager.SetGameObjectActive(m_enemyPlayerIcons[j], false);
			}
		}
	}

	public void DisplayPanelShowAnimDone()
	{
	}

	public void DisplayPanelHideAnimDone()
	{
		SetDisplaysVisible(m_PanelVisibility);
	}

	public void NotifyDecisionTimerShow()
	{
		if (!IsAnimationPlaying())
		{
			m_PanelVisibility = true;
			SetDisplaysVisible(true);
		}
		else if (m_animsToPlayQueue != null && !m_animsToPlayQueue.Contains(showDisplayAnimName))
		{
		}
	}

	public void NotifyLockedIn(bool isLocked)
	{
		foreach (UIPlayerStatus icon in m_teamPlayerIcons)
		{
			icon.NotifyLockedIn(isLocked);
		}

		foreach (UIPlayerStatus icon in m_enemyPlayerIcons)
		{
			icon.NotifyLockedIn(isLocked);
		}
	}

	private void ProcessTeamsForSpectator()
	{
		bool isClientSpectator = ClientGameManager.Get() != null
		         && ClientGameManager.Get().PlayerInfo != null
		         && ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator;
		bool isSpectator = GameManager.Get() != null
		         && GameManager.Get().PlayerInfo != null
		         && GameManager.Get().PlayerInfo.TeamId == Team.Spectator;
		
		if (GameFlowData.Get() == null
		    || GameFlowData.Get().LocalPlayerData == null
		    || (!isClientSpectator && !isSpectator))
		{
			return;
		}
		
		Team team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		if (team == Team.Invalid)
		{
			team = Team.TeamA;
		}

		List<ActorData> teamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team);
		Team otherTeam = team == Team.TeamA ? Team.TeamB : Team.TeamA;
		List<ActorData> otherTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(otherTeam);
		
		int i = 0;
		foreach (ActorData player in teamMembers)
		{
			if (i >= m_teamPlayerIcons.Length)
			{
				break;
			}

			if (GameplayUtils.IsPlayerControlled(player))
			{
				m_teamPlayerIcons[i].Setup(player);
				bool doActive = SinglePlayerManager.Get() == null 
				                || !SinglePlayerManager.Get().GetTeamPlayerIconForceOff(i);
				UIManager.SetGameObjectActive(m_teamPlayerIcons[i], doActive);
				i++;
			}
		}

		i = 0;
		foreach (ActorData player in otherTeamMembers)
		{
			if (i >= m_enemyPlayerIcons.Length)
			{
				return;
			}

			if (GameplayUtils.IsPlayerControlled(player))
			{
				m_enemyPlayerIcons[i].Setup(player);
				bool doActive = SinglePlayerManager.Get() == null
				                || !SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(i);
				UIManager.SetGameObjectActive(m_enemyPlayerIcons[i], doActive);
				i++;
			}
		}
	}

	public void ProcessTeams()
	{
		if (GameFlowData.Get() == null || GameManager.Get().GameConfig.GameType == GameType.Tutorial)
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			int i = 0;
			foreach (ActorData ally in GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetTeam()))
			{
				if (i >= m_teamPlayerIcons.Length)
				{
					break;
				}

				if (GameplayUtils.IsPlayerControlled(ally))
				{
					m_teamPlayerIcons[i].Setup(ally);
					i++;
				}
			}
			i = 0;
			foreach (ActorData enemy in GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetEnemyTeam()))
			{
				if (i >= m_enemyPlayerIcons.Length)
				{
					break;
				}
				if (GameplayUtils.IsPlayerControlled(enemy))
				{
					m_enemyPlayerIcons[i].Setup(enemy);
					i++;
				}
			}
		}
		else
		{
			ProcessTeamsForSpectator();
		}
		SetDisplaysVisible(true);
	}
}
