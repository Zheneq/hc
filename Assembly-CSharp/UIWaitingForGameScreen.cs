using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWaitingForGameScreen : MonoBehaviour
{
	public TextMeshProUGUI m_queueMatchTypeLabel;

	public TextMeshProUGUI m_statusLabel;

	public _ButtonSwapSprite m_cancelButton;

	public GameObject m_WaitingInQueueWorldParent;

	public Animator m_circleWidgetAnimController;

	public RectTransform[] m_containers;

	private void Start()
	{
		UIEventTriggerUtils.AddListener(m_cancelButton.gameObject, EventTriggerType.PointerClick, CancelClicked);
	}

	public void SetVisible(bool visible)
	{
		for (int i = 0; i < m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_containers[i], visible);
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
			UIManager.SetGameObjectActive(m_circleWidgetAnimController, visible);
			if (visible)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_circleWidgetAnimController.Play("searchingForMatchesAnimationLoop");
						return;
					}
				}
			}
			m_circleWidgetAnimController.Play("Idle");
			return;
		}
	}

	public void CancelClicked(BaseEventData data)
	{
		AppState_WaitingForGame.Get().HandleCancelClicked();
	}

	public void Setup(GameType gameType)
	{
		string text = string.Empty;
		switch (gameType)
		{
		case GameType.Coop:
			text = StringUtil.TR("JOINCOOPERATIVEMATCH", "Global");
			break;
		case GameType.PvP:
			text = StringUtil.TR("JOINVERSUSMATCH", "Global");
			break;
		case GameType.Custom:
			text = StringUtil.TR("JOINCUSTOMMATCH", "Global");
			break;
		}
		m_queueMatchTypeLabel.text = text;
	}

	public void UpdateQueueStatus()
	{
		if ((bool)m_statusLabel)
		{
			m_statusLabel.text = ClientGameManager.Get().GenerateQueueLabel();
		}
	}

	public void InitQueueStatus()
	{
		UpdateQueueStatus();
	}

	public void UpdateGameStatus(LobbyGameInfo gameInfo)
	{
		if (!m_statusLabel)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (gameInfo.GameStatus != GameStatus.Assembling)
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
				if (gameInfo.GameStatus != GameStatus.FreelancerSelecting)
				{
					if (gameInfo.GameStatus == GameStatus.LoadoutSelecting)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								m_statusLabel.text = StringUtil.TR("SelectingLoadouts", "Global");
								return;
							}
						}
					}
					if (gameInfo.GameStatus == GameStatus.Launching)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								m_statusLabel.text = StringUtil.TR("GameServerStarting", "Global");
								return;
							}
						}
					}
					if (gameInfo.GameStatus == GameStatus.Launched)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							m_statusLabel.text = StringUtil.TR("GameServerReady", "Global");
							return;
						}
					}
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (gameInfo.ActivePlayers >= gameInfo.GameConfig.TotalHumanPlayers)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_statusLabel.text = StringUtil.TR("CreatingGame", "Global");
						return;
					}
				}
			}
			m_statusLabel.text = string.Format(StringUtil.TR("PlayersReadyCount", "Global"), gameInfo.ActivePlayers, gameInfo.GameConfig.TotalHumanPlayers);
			return;
		}
	}
}
