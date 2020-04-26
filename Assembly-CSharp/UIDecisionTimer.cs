using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDecisionTimer : MonoBehaviour
{
	private const int m_numTicks = 30;

	private const float m_tickSize = 0.03333333f;

	private const float m_lowTimeThreshold = 0.16667f;

	public Color m_enoughTimeColor;

	public Color m_lowTimeColor;

	public UIAbilityUsedTracker m_abilityUsedTracker;

	public RectTransform m_timerContainer;

	public RectTransform m_sliderContainer;

	public RectTransform m_timeBankTextcontainer;

	public RectTransform m_timeBankNotchesContainer;

	public Animator[] m_timeBankNotches;

	public TextMeshProUGUI m_secondsLabel;

	public TextMeshProUGUI m_millisecondsLabel;

	public TextMeshProUGUI m_timeBankSecondsLabel;

	public TextMeshProUGUI m_timeBankMillisecondsLabel;

	public TextMeshProUGUI m_savedSecondsLabel;

	public TextMeshProUGUI m_savedMillisecondsLabel;

	public CanvasGroup m_savedTimeAlpha;

	public CanvasGroup m_cancelledLabelAlpha;

	public Sprite m_defaultTimeBar;

	public Sprite m_lowTimeBar;

	public Animator m_animationController;

	public RectTransform m_soloOutOfTimerIndicator;

	public RectTransform m_soloInfiniteTimerIndicator;

	public _ButtonSwapSprite m_closeButton;

	[Range(0f, 1f)]
	public float m_currentTimeVal;

	public ImageFilledSloped m_currentTimeImage;

	private float m_lastCurrentTimeVal;

	[Range(0f, 1f)]
	public float m_savedTimeVal;

	public ImageFilledSloped m_savedTimeImage;

	private float m_lastSavedTimeVal;

	public ImageFilledSloped m_timeBankImage;

	[Range(0f, 1f)]
	public float m_currentVal;

	public float m_timeToFadeGainedTime = 1f;

	public float m_pauseTimeToStartFade = 1f;

	private bool m_glowOn;

	private bool m_isPlayingAnim;

	private bool m_initializedGameFlowData;

	private int m_numStartingTimeBankConsumables = 2;

	private static int GetNumStartingTimeBankConsumables()
	{
		int result = 0;
		if (GameWideData.Get() != null)
		{
			result = GameWideData.Get().m_tbConsumables;
		}
		if (GameFlowData.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (!GameManager.Get().GameConfig.SubTypes.IsNullOrEmpty())
				{
					if (GameManager.Get().GameConfig.InstanceSubType != null)
					{
						if (GameManager.Get().GameConfig.InstanceSubType.GameOverrides != null)
						{
							int? initialTimeBankConsumables = GameManager.Get().GameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
							if (initialTimeBankConsumables.HasValue)
							{
								int? initialTimeBankConsumables2 = GameManager.Get().GameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
								result = initialTimeBankConsumables2.Value;
							}
						}
					}
					else
					{
						Log.Info("No Instance Subtypes for getting timebank consumables");
					}
				}
			}
		}
		return result;
	}

	private void Start()
	{
		m_savedTimeAlpha.alpha = 0f;
		m_cancelledLabelAlpha.alpha = 0f;
		m_numStartingTimeBankConsumables = GetNumStartingTimeBankConsumables();
		for (int i = 0; i < m_timeBankNotches.Length; i++)
		{
			if (i < m_numStartingTimeBankConsumables)
			{
				UIManager.SetGameObjectActive(m_timeBankNotches[i], true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_timeBankNotches[i], false);
			}
		}
		m_closeButton.callback = CloseSoloInfiniteTimerIndicator;
		UIManager.SetGameObjectActive(m_soloInfiniteTimerIndicator, false);
		m_initializedGameFlowData = false;
	}

	private void CloseSoloInfiniteTimerIndicator(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_soloInfiniteTimerIndicator, false);
		GameFlowData.Get().SetPausedForDialog(false);
	}

	public void SetUseTimeBankBars(bool useTimeBank)
	{
		UIManager.SetGameObjectActive(m_secondsLabel, !useTimeBank);
		UIManager.SetGameObjectActive(m_millisecondsLabel, !useTimeBank);
		UIManager.SetGameObjectActive(m_currentTimeImage, !useTimeBank);
		UIManager.SetGameObjectActive(m_timeBankSecondsLabel, useTimeBank);
		UIManager.SetGameObjectActive(m_timeBankMillisecondsLabel, useTimeBank);
		UIManager.SetGameObjectActive(m_timeBankImage, useTimeBank);
	}

	public void SetGameObjectActive(bool active)
	{
		UIManager.SetGameObjectActive(base.gameObject, active);
	}

	public void SetGlowingTimer(bool value)
	{
		if (m_glowOn == value)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_glowOn = value;
		if (value)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					m_isPlayingAnim = true;
					float savedTimeToDisplay = GetSavedTimeToDisplay();
					if (savedTimeToDisplay > 0f)
					{
						UpdateTimeLabels(m_savedSecondsLabel, m_savedMillisecondsLabel, savedTimeToDisplay);
						string stateName = "LockedSavedTimeIndicator";
						m_animationController.Play(stateName, -1, 0f);
					}
					if (!base.gameObject.activeSelf)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								FinishedSavedTimeAnimation();
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		m_isPlayingAnim = true;
		string text = "UnlockedSavedTimeIndicator";
		AnimatorClipInfo[] currentAnimatorClipInfo = m_animationController.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo != null)
		{
			if (currentAnimatorClipInfo.Length > 0)
			{
				if (m_animationController.GetCurrentAnimatorClipInfo(0)[0].clip.name == text)
				{
					m_animationController.Play(text, -1, 0f);
				}
				else
				{
					m_animationController.Play(text);
				}
			}
		}
		if (base.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			FinishedUnlockTimeAnimation();
			return;
		}
	}

	public bool IsPlayingAnimation()
	{
		return m_isPlayingAnim;
	}

	public void FinishedSavedTimeAnimation()
	{
	}

	public void FinishedUnlockTimeAnimation()
	{
	}

	public void UpdateColor()
	{
		if (!(m_currentVal <= 0.16667f))
		{
			return;
		}
		while (true)
		{
			return;
		}
	}

	private float GetSavedTimeToDisplay()
	{
		return 0f;
	}

	public void UpdateSavedTimerPosition(float pct)
	{
		m_savedTimeVal = pct;
	}

	public void UpdateSliderAmount(float pct)
	{
		m_currentTimeVal = pct;
	}

	public void NotifyPhase(bool inDecision)
	{
		UIManager.SetGameObjectActive(m_timerContainer, inDecision);
		UIManager.SetGameObjectActive(m_sliderContainer, inDecision);
		UIManager.SetGameObjectActive(m_timeBankTextcontainer, inDecision);
	}

	public void UpdateTimeGainedAmount(float startTimePct, float currentTimePct, bool doNotUpdateAlpha)
	{
	}

	public void UpdateTimeLabels(TextMeshProUGUI secondsLabel, TextMeshProUGUI millisecondsLabel, float timeToDisplay)
	{
		timeToDisplay = Mathf.Max(timeToDisplay, 0f);
		int num = Mathf.FloorToInt(timeToDisplay);
		if (num > 9)
		{
			secondsLabel.text = num.ToString();
		}
		else
		{
			secondsLabel.text = "0" + num;
		}
		int num2 = Mathf.FloorToInt((timeToDisplay - (float)num) * 100f);
		if (num2 > 9)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					millisecondsLabel.text = ":" + num2;
					return;
				}
			}
		}
		millisecondsLabel.text = ":0" + num2;
	}

	private void UpdateCurrentTimeBar()
	{
		if (m_currentTimeVal == m_lastCurrentTimeVal)
		{
			return;
		}
		if (m_currentTimeVal > 0f)
		{
			float currentTimeVal = m_currentTimeVal;
			m_currentTimeImage.fillAmount = currentTimeVal;
			UIManager.SetGameObjectActive(m_currentTimeImage, true);
			if (GameFlowData.Get() != null)
			{
				if (m_currentTimeVal * GameFlowData.Get().m_maxTurnTime < 5f)
				{
					m_currentTimeImage.sprite = m_lowTimeBar;
				}
				else
				{
					m_currentTimeImage.sprite = m_defaultTimeBar;
				}
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_currentTimeImage, false);
		}
		m_lastCurrentTimeVal = m_currentTimeVal;
	}

	private void UpdateSavedTimeBar()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
			{
				if (GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState != TurnStateEnum.CONFIRMED)
				{
					m_savedTimeVal = 0f;
					UIManager.SetGameObjectActive(m_savedTimeImage, false);
				}
			}
		}
		if (m_savedTimeVal == m_lastSavedTimeVal)
		{
			return;
		}
		if (m_savedTimeVal > 0f)
		{
			UIManager.SetGameObjectActive(m_savedTimeImage, true);
			m_savedTimeImage.fillAmount = m_savedTimeVal;
		}
		else
		{
			UIManager.SetGameObjectActive(m_savedTimeImage, false);
		}
		m_lastSavedTimeVal = m_savedTimeVal;
	}

	private void UpdateConsumables(int numConsumablesLeft, bool beingUsed)
	{
		for (int i = 0; i < m_timeBankNotches.Length; i++)
		{
			if (!m_timeBankNotches[i].isInitialized)
			{
				continue;
			}
			if (i == m_numStartingTimeBankConsumables - numConsumablesLeft)
			{
				if (beingUsed)
				{
					m_timeBankNotches[i].Play("TimeBankItemDefaultUSED");
					continue;
				}
			}
			if (i < m_numStartingTimeBankConsumables - numConsumablesLeft)
			{
				m_timeBankNotches[i].Play("TimeBankItemDefaultOUT");
				continue;
			}
			RectTransform[] componentsInChildren = m_timeBankNotches[i].GetComponentsInChildren<RectTransform>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j].gameObject != m_timeBankNotches[i].gameObject)
				{
					UIManager.SetGameObjectActive(componentsInChildren[j], true);
				}
			}
			m_timeBankNotches[i].Play("TimeBankItemDefaultIN");
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

	private void UpdateTimeBankBar()
	{
		bool useTimeBankBars = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			TimeBank component = GameFlowData.Get().activeOwnedActorData.GetComponent<TimeBank>();
			if (component != null)
			{
				if (component.TimeToDisplay() <= 0f)
				{
					if (GameFlowData.Get().WillEnterTimebankMode())
					{
						useTimeBankBars = true;
						float num = component.TimeToDisplay() + GameWideData.Get().m_tbConsumableDuration;
						float fillAmount = num / GameWideData.Get().m_tbConsumableDuration;
						if (component.TimeToDisplay() < 0f)
						{
							UpdateTimeLabels(m_timeBankSecondsLabel, m_timeBankMillisecondsLabel, num);
						}
						m_timeBankImage.fillAmount = fillAmount;
					}
				}
				UpdateConsumables(component.GetConsumablesRemaining(), component.GetConsumableUsed());
			}
		}
		SetUseTimeBankBars(useTimeBankBars);
	}

	private void UpdateCheckTimeMessageIndicators()
	{
		if (!m_initializedGameFlowData)
		{
			if (GameFlowData.Get() != null)
			{
				m_initializedGameFlowData = true;
				if (GameFlowData.Get().PreventAutoLockInOnTimeout())
				{
					UIManager.SetGameObjectActive(m_soloInfiniteTimerIndicator, true);
				}
			}
		}
		bool doActive = true;
		if (m_currentTimeVal > 0f)
		{
			doActive = false;
		}
		else if (GameFlowData.Get() != null && GameFlowData.Get().PreventAutoLockInOnTimeout())
		{
			if (GameFlowData.Get().IsInDecisionState())
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().activeOwnedActorData != null)
					{
						TimeBank component = GameFlowData.Get().activeOwnedActorData.GetComponent<TimeBank>();
						if (component != null)
						{
							if (component.TimeToDisplay() <= 0f)
							{
								if (GameFlowData.Get().WillEnterTimebankMode())
								{
									float num = component.TimeToDisplay() + GameWideData.Get().m_tbConsumableDuration;
									if (num > 0f)
									{
										doActive = false;
									}
								}
							}
						}
						else
						{
							doActive = false;
						}
						goto IL_017f;
					}
				}
				doActive = false;
			}
			else
			{
				doActive = false;
			}
		}
		else
		{
			doActive = false;
		}
		goto IL_017f;
		IL_017f:
		if (m_soloInfiniteTimerIndicator.gameObject.activeSelf)
		{
			doActive = false;
			if (GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().IsInResolveState())
				{
					UIManager.SetGameObjectActive(m_soloInfiniteTimerIndicator, false);
				}
			}
		}
		UIManager.SetGameObjectActive(m_soloOutOfTimerIndicator, doActive);
	}

	private void Update()
	{
		UpdateCurrentTimeBar();
		UpdateSavedTimeBar();
		UpdateTimeBankBar();
		UpdateCheckTimeMessageIndicators();
	}

	private void OnEnable()
	{
		Update();
	}
}
