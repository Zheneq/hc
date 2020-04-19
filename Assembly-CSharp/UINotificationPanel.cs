using System;
using TMPro;
using UnityEngine;

public class UINotificationPanel : MonoBehaviour
{
	public RectTransform m_lowTurnsLeftContainer;

	public TextMeshProUGUI m_TurnNumberLabel;

	public TextMeshProUGUI m_TurnTextLabel;

	public TextMeshProUGUI m_OvertimeLabel;

	public TextMeshProUGUI m_phaseModeLabel;

	public Animator m_animController;

	private UINotificationPanel.GamePhaseDisplay m_phaseDisplayRef;

	private bool m_turnCountUpdated;

	private bool m_phaseDisplayRefUpdated;

	private void Start()
	{
		UIManager.SetGameObjectActive(this, false, null);
		this.m_phaseDisplayRefUpdated = false;
		this.m_turnCountUpdated = false;
	}

	public void NotifyTurnCountSet()
	{
		this.m_turnCountUpdated = true;
		if (this.m_turnCountUpdated && this.m_phaseDisplayRefUpdated)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINotificationPanel.NotifyTurnCountSet()).MethodHandle;
			}
			this.PlayNotification();
		}
	}

	private void PlayNotification()
	{
		switch (this.m_phaseDisplayRef)
		{
		case UINotificationPanel.GamePhaseDisplay.Decision:
			this.m_phaseModeLabel.text = StringUtil.TR("DECISIONPHASE", "HUDScene");
			break;
		case UINotificationPanel.GamePhaseDisplay.Resolving:
			this.m_phaseModeLabel.text = StringUtil.TR("RESOLUTIONPHASE", "HUDScene");
			break;
		}
		UIManager.SetGameObjectActive(this, true, null);
		if (ObjectivePoints.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINotificationPanel.PlayNotification()).MethodHandle;
			}
			if (this.m_phaseDisplayRef == UINotificationPanel.GamePhaseDisplay.Decision)
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
				int currentTurn = GameFlowData.Get().CurrentTurn;
				int num = ObjectivePoints.Get().m_timeLimitTurns - currentTurn;
				if (!ObjectivePoints.Get().InSuddenDeath())
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
					if (currentTurn >= ObjectivePoints.Get().m_timeLimitTurns && ObjectivePoints.Get().m_timeLimitTurns > 0)
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
					else
					{
						if (0 < num && num < 6)
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
							UIManager.SetGameObjectActive(this.m_TurnNumberLabel, true, null);
							UIManager.SetGameObjectActive(this.m_TurnTextLabel, true, null);
							this.m_TurnNumberLabel.text = num.ToString();
							if (num > 1)
							{
								this.m_TurnTextLabel.text = StringUtil.TR("TurnsRemaining", "Global");
							}
							else
							{
								this.m_TurnTextLabel.text = StringUtil.TR("TurnRemaining", "Global");
							}
							UIManager.SetGameObjectActive(this.m_OvertimeLabel, false, null);
							this.m_animController.Play("SlideInAndOutTurnsRemaining");
							UIManager.SetGameObjectActive(this.m_lowTurnsLeftContainer, true, null);
							goto IL_222;
						}
						this.m_animController.Play("SlideInAndOut");
						UIManager.SetGameObjectActive(this.m_lowTurnsLeftContainer, false, null);
						goto IL_222;
					}
				}
				UIManager.SetGameObjectActive(this.m_TurnNumberLabel, false, null);
				UIManager.SetGameObjectActive(this.m_TurnTextLabel, false, null);
				UIManager.SetGameObjectActive(this.m_OvertimeLabel, true, null);
				this.m_animController.Play("SlideInAndOutTurnsRemaining", -1, 0f);
				UIManager.SetGameObjectActive(this.m_lowTurnsLeftContainer, true, null);
				IL_222:
				goto IL_241;
			}
		}
		this.m_animController.Play("SlideInAndOut");
		UIManager.SetGameObjectActive(this.m_lowTurnsLeftContainer, false, null);
		IL_241:
		this.m_turnCountUpdated = false;
		this.m_phaseDisplayRefUpdated = false;
	}

	public void DisplayNotification(UINotificationPanel.GamePhaseDisplay phase)
	{
		if (phase != UINotificationPanel.GamePhaseDisplay.LockedIn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINotificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay)).MethodHandle;
			}
			if (!(SinglePlayerManager.Get() != null) || !SinglePlayerManager.Get().GetNotificationPanelForceOff())
			{
				this.m_phaseDisplayRef = phase;
				this.m_phaseDisplayRefUpdated = true;
				if (phase != UINotificationPanel.GamePhaseDisplay.Resolving)
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
					if (!this.m_turnCountUpdated || !this.m_phaseDisplayRefUpdated)
					{
						return;
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
				this.PlayNotification();
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

	public void TweenFinished()
	{
		UIManager.SetGameObjectActive(this, false, null);
	}

	public enum GamePhaseDisplay
	{
		LockedIn,
		Decision,
		Resolving
	}
}
