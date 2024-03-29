using TMPro;
using UnityEngine;

public class UINotificationPanel : MonoBehaviour
{
	public enum GamePhaseDisplay
	{
		LockedIn,
		Decision,
		Resolving
	}

	public RectTransform m_lowTurnsLeftContainer;

	public TextMeshProUGUI m_TurnNumberLabel;

	public TextMeshProUGUI m_TurnTextLabel;

	public TextMeshProUGUI m_OvertimeLabel;

	public TextMeshProUGUI m_phaseModeLabel;

	public Animator m_animController;

	private GamePhaseDisplay m_phaseDisplayRef;

	private bool m_turnCountUpdated;

	private bool m_phaseDisplayRefUpdated;

	private void Start()
	{
		UIManager.SetGameObjectActive(this, false);
		m_phaseDisplayRefUpdated = false;
		m_turnCountUpdated = false;
	}

	public void NotifyTurnCountSet()
	{
		m_turnCountUpdated = true;
		if (!m_turnCountUpdated || !m_phaseDisplayRefUpdated)
		{
			return;
		}
		while (true)
		{
			PlayNotification();
			return;
		}
	}

	private void PlayNotification()
	{
		switch (m_phaseDisplayRef)
		{
		case GamePhaseDisplay.Decision:
			m_phaseModeLabel.text = StringUtil.TR("DECISIONPHASE", "HUDScene");
			break;
		case GamePhaseDisplay.Resolving:
			m_phaseModeLabel.text = StringUtil.TR("RESOLUTIONPHASE", "HUDScene");
			break;
		}
		UIManager.SetGameObjectActive(this, true);
		if (ObjectivePoints.Get() != null)
		{
			if (m_phaseDisplayRef == GamePhaseDisplay.Decision)
			{
				int currentTurn = GameFlowData.Get().CurrentTurn;
				int num = ObjectivePoints.Get().m_timeLimitTurns - currentTurn;
				if (!ObjectivePoints.Get().InSuddenDeath())
				{
					if (currentTurn < ObjectivePoints.Get().m_timeLimitTurns || ObjectivePoints.Get().m_timeLimitTurns <= 0)
					{
						if (0 < num && num < 6)
						{
							UIManager.SetGameObjectActive(m_TurnNumberLabel, true);
							UIManager.SetGameObjectActive(m_TurnTextLabel, true);
							m_TurnNumberLabel.text = num.ToString();
							if (num > 1)
							{
								m_TurnTextLabel.text = StringUtil.TR("TurnsRemaining", "Global");
							}
							else
							{
								m_TurnTextLabel.text = StringUtil.TR("TurnRemaining", "Global");
							}
							UIManager.SetGameObjectActive(m_OvertimeLabel, false);
							m_animController.Play("SlideInAndOutTurnsRemaining");
							UIManager.SetGameObjectActive(m_lowTurnsLeftContainer, true);
						}
						else
						{
							m_animController.Play("SlideInAndOut");
							UIManager.SetGameObjectActive(m_lowTurnsLeftContainer, false);
						}
						goto IL_0241;
					}
				}
				UIManager.SetGameObjectActive(m_TurnNumberLabel, false);
				UIManager.SetGameObjectActive(m_TurnTextLabel, false);
				UIManager.SetGameObjectActive(m_OvertimeLabel, true);
				m_animController.Play("SlideInAndOutTurnsRemaining", -1, 0f);
				UIManager.SetGameObjectActive(m_lowTurnsLeftContainer, true);
				goto IL_0241;
			}
		}
		m_animController.Play("SlideInAndOut");
		UIManager.SetGameObjectActive(m_lowTurnsLeftContainer, false);
		goto IL_0241;
		IL_0241:
		m_turnCountUpdated = false;
		m_phaseDisplayRefUpdated = false;
	}

	public void DisplayNotification(GamePhaseDisplay phase)
	{
		if (phase == GamePhaseDisplay.LockedIn)
		{
			return;
		}
		while (true)
		{
			if (SinglePlayerManager.Get() != null && SinglePlayerManager.Get().GetNotificationPanelForceOff())
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
			m_phaseDisplayRef = phase;
			m_phaseDisplayRefUpdated = true;
			if (phase != GamePhaseDisplay.Resolving)
			{
				if (!m_turnCountUpdated || !m_phaseDisplayRefUpdated)
				{
					return;
				}
			}
			PlayNotification();
			return;
		}
	}

	public void TweenFinished()
	{
		UIManager.SetGameObjectActive(this, false);
	}
}
