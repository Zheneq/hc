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
		if (!IsAnimationPlaying())
		{
			if (m_animsToPlayQueue.Count > 0)
			{
				SetDisplaysVisible(true);
				m_animsToPlayQueue.RemoveAt(0);
			}
		}
		ProcessTeamsForSpectator();
	}

	public void UpdateCatalysts(ActorData theActor, List<Ability> cardAbilities)
	{
		int num = 0;
		while (true)
		{
			if (num < m_teamPlayerIcons.Length)
			{
				if (m_teamPlayerIcons[num].ActorDataRef == theActor)
				{
					m_teamPlayerIcons[num].UpdateCatalysts(cardAbilities);
					break;
				}
				num++;
				continue;
			}
			break;
		}
		for (int i = 0; i < m_enemyPlayerIcons.Length; i++)
		{
			if (!(m_enemyPlayerIcons[i].ActorDataRef == theActor))
			{
				continue;
			}
			while (true)
			{
				m_enemyPlayerIcons[i].UpdateCatalysts(cardAbilities);
				return;
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

	private bool IsAnimationPlaying()
	{
		if (m_animationController.GetCurrentAnimatorClipInfo(0) != null)
		{
			if (m_animationController.GetCurrentAnimatorClipInfo(0).Length > 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m_animationController.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EmptyAnimation";
					}
				}
			}
		}
		return false;
	}

	private void SetDisplaysVisible(bool visible)
	{
		int num;
		if (m_teamPlayerIcons[0].IsActiveDisplay())
		{
			num = GameFlowData.Get().GetPlayerAndBotTeamMembers(m_teamPlayerIcons[0].GetTeam()).Count;
		}
		else
		{
			num = m_teamPlayerIcons.Length;
		}
		for (int i = 0; i < m_teamPlayerIcons.Length; i++)
		{
			if (i < num && m_teamPlayerIcons[i].IsActiveDisplay())
			{
				bool doActive = visible;
				if ((bool)SinglePlayerManager.Get())
				{
					if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(i))
					{
						doActive = false;
					}
				}
				UIManager.SetGameObjectActive(m_teamPlayerIcons[i], doActive);
			}
			else
			{
				UIManager.SetGameObjectActive(m_teamPlayerIcons[i], false);
			}
		}
		if (m_enemyPlayerIcons[0].IsActiveDisplay())
		{
			num = GameFlowData.Get().GetPlayerAndBotTeamMembers(m_enemyPlayerIcons[0].GetTeam()).Count;
		}
		else
		{
			num = m_enemyPlayerIcons.Length;
		}
		for (int j = 0; j < m_enemyPlayerIcons.Length; j++)
		{
			if (j < num)
			{
				if (m_enemyPlayerIcons[j].IsActiveDisplay())
				{
					bool doActive2 = visible;
					if ((bool)SinglePlayerManager.Get() && SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(j))
					{
						doActive2 = false;
					}
					UIManager.SetGameObjectActive(m_enemyPlayerIcons[j], doActive2);
					continue;
				}
			}
			UIManager.SetGameObjectActive(m_enemyPlayerIcons[j], false);
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void DisplayPanelShowAnimDone()
	{
	}

	public void DisplayPanelHideAnimDone()
	{
		if (m_PanelVisibility)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					SetDisplaysVisible(true);
					return;
				}
			}
		}
		SetDisplaysVisible(false);
	}

	public void NotifyDecisionTimerShow()
	{
		if (!IsAnimationPlaying())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_PanelVisibility = true;
					SetDisplaysVisible(true);
					return;
				}
			}
		}
		if (m_animsToPlayQueue == null)
		{
			return;
		}
		while (true)
		{
			if (!m_animsToPlayQueue.Contains(showDisplayAnimName))
			{
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
			return;
		}
	}

	public void NotifyLockedIn(bool isLocked)
	{
		for (int i = 0; i < m_teamPlayerIcons.Length; i++)
		{
			m_teamPlayerIcons[i].NotifyLockedIn(isLocked);
		}
		while (true)
		{
			for (int j = 0; j < m_enemyPlayerIcons.Length; j++)
			{
				m_enemyPlayerIcons[j].NotifyLockedIn(isLocked);
			}
			return;
		}
	}

	private void ProcessTeamsForSpectator()
	{
		int num;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerInfo != null && ClientGameManager.Get().PlayerInfo.TeamId == Team.Spectator)
			{
				num = 1;
				goto IL_008d;
			}
		}
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				num = ((GameManager.Get().PlayerInfo.TeamId == Team.Spectator) ? 1 : 0);
				goto IL_008d;
			}
		}
		num = 0;
		goto IL_008d;
		IL_008d:
		bool flag = (byte)num != 0;
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().LocalPlayerData == null)
			{
				return;
			}
			while (true)
			{
				if (!flag)
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
				Team team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				if (team != 0)
				{
					if (team != Team.Invalid)
					{
						goto IL_0104;
					}
				}
				team = Team.TeamA;
				goto IL_0104;
				IL_0104:
				List<ActorData> playerAndBotTeamMembers = GameFlowData.Get().GetPlayerAndBotTeamMembers(team);
				GameFlowData gameFlowData = GameFlowData.Get();
				int team2;
				if (team == Team.TeamA)
				{
					team2 = 1;
				}
				else
				{
					team2 = 0;
				}
				List<ActorData> playerAndBotTeamMembers2 = gameFlowData.GetPlayerAndBotTeamMembers((Team)team2);
				int num2 = 0;
				using (List<ActorData>.Enumerator enumerator = playerAndBotTeamMembers.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						ActorData current = enumerator.Current;
						if (num2 >= m_teamPlayerIcons.Length)
						{
							break;
						}
						if (!GameplayUtils.IsPlayerControlled(current))
						{
						}
						else
						{
							m_teamPlayerIcons[num2].Setup(current);
							bool doActive = true;
							if ((bool)SinglePlayerManager.Get())
							{
								if (SinglePlayerManager.Get().GetTeamPlayerIconForceOff(num2))
								{
									doActive = false;
								}
							}
							UIManager.SetGameObjectActive(m_teamPlayerIcons[num2], doActive);
							num2++;
						}
					}
				}
				num2 = 0;
				using (List<ActorData>.Enumerator enumerator2 = playerAndBotTeamMembers2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData current2 = enumerator2.Current;
						if (num2 >= m_enemyPlayerIcons.Length)
						{
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
						if (GameplayUtils.IsPlayerControlled(current2))
						{
							m_enemyPlayerIcons[num2].Setup(current2);
							bool doActive2 = true;
							if ((bool)SinglePlayerManager.Get())
							{
								if (SinglePlayerManager.Get().GetEnemyPlayerIconForceOff(num2))
								{
									doActive2 = false;
								}
							}
							UIManager.SetGameObjectActive(m_enemyPlayerIcons[num2], doActive2);
							num2++;
						}
					}
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
			int num = 0;
			foreach (ActorData ally in GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetTeam()))
			{
				if (num >= m_teamPlayerIcons.Length)
				{
					break;
				}

				if (GameplayUtils.IsPlayerControlled(ally))
				{
					m_teamPlayerIcons[num].Setup(ally);
					num++;
				}
			}
			num = 0;
			foreach (ActorData enemy in GameFlowData.Get().GetPlayerAndBotTeamMembers(activeOwnedActorData.GetEnemyTeam()))
			{
				if (num >= m_enemyPlayerIcons.Length)
				{
					break;
				}
				if (GameplayUtils.IsPlayerControlled(enemy))
				{
					m_enemyPlayerIcons[num].Setup(enemy);
					num++;
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
