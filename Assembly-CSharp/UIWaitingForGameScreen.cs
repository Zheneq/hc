using System;
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
		UIEventTriggerUtils.AddListener(this.m_cancelButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.CancelClicked));
	}

	public void SetVisible(bool visible)
	{
		for (int i = 0; i < this.m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_containers[i], visible, null);
		}
		UIManager.SetGameObjectActive(this.m_circleWidgetAnimController, visible, null);
		if (visible)
		{
			this.m_circleWidgetAnimController.Play("searchingForMatchesAnimationLoop");
		}
		else
		{
			this.m_circleWidgetAnimController.Play("Idle");
		}
	}

	public void CancelClicked(BaseEventData data)
	{
		AppState_WaitingForGame.Get().HandleCancelClicked();
	}

	public void Setup(GameType gameType)
	{
		string text = string.Empty;
		if (gameType != GameType.Coop)
		{
			if (gameType != GameType.PvP)
			{
				if (gameType == GameType.Custom)
				{
					text = StringUtil.TR("JOINCUSTOMMATCH", "Global");
				}
			}
			else
			{
				text = StringUtil.TR("JOINVERSUSMATCH", "Global");
			}
		}
		else
		{
			text = StringUtil.TR("JOINCOOPERATIVEMATCH", "Global");
		}
		this.m_queueMatchTypeLabel.text = text;
	}

	public void UpdateQueueStatus()
	{
		if (this.m_statusLabel)
		{
			this.m_statusLabel.text = ClientGameManager.Get().GenerateQueueLabel();
		}
	}

	public void InitQueueStatus()
	{
		this.UpdateQueueStatus();
	}

	public void UpdateGameStatus(LobbyGameInfo gameInfo)
	{
		if (this.m_statusLabel)
		{
			if (gameInfo.GameStatus != GameStatus.Assembling)
			{
				if (gameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
				}
				else
				{
					if (gameInfo.GameStatus == GameStatus.LoadoutSelecting)
					{
						this.m_statusLabel.text = StringUtil.TR("SelectingLoadouts", "Global");
						return;
					}
					if (gameInfo.GameStatus == GameStatus.Launching)
					{
						this.m_statusLabel.text = StringUtil.TR("GameServerStarting", "Global");
						return;
					}
					if (gameInfo.GameStatus == GameStatus.Launched)
					{
						this.m_statusLabel.text = StringUtil.TR("GameServerReady", "Global");
						return;
					}
					return;
				}
			}
			if (gameInfo.ActivePlayers >= gameInfo.GameConfig.TotalHumanPlayers)
			{
				this.m_statusLabel.text = StringUtil.TR("CreatingGame", "Global");
			}
			else
			{
				this.m_statusLabel.text = string.Format(StringUtil.TR("PlayersReadyCount", "Global"), gameInfo.ActivePlayers, gameInfo.GameConfig.TotalHumanPlayers);
			}
		}
	}
}
