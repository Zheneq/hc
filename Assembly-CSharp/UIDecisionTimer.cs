using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDecisionTimer : MonoBehaviour
{
	private const int m_numTicks = 0x1E;

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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.GetNumStartingTimeBankConsumables()).MethodHandle;
			}
			if (GameManager.Get().GameConfig != null)
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
				if (!GameManager.Get().GameConfig.SubTypes.IsNullOrEmpty<GameSubType>())
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
					if (GameManager.Get().GameConfig.InstanceSubType != null)
					{
						if (GameManager.Get().GameConfig.InstanceSubType.GameOverrides != null)
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
							int? initialTimeBankConsumables = GameManager.Get().GameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
							if (initialTimeBankConsumables != null)
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
								int? initialTimeBankConsumables2 = GameManager.Get().GameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
								result = initialTimeBankConsumables2.Value;
							}
						}
					}
					else
					{
						Log.Info("No Instance Subtypes for getting timebank consumables", new object[0]);
					}
				}
			}
		}
		return result;
	}

	private void Start()
	{
		this.m_savedTimeAlpha.alpha = 0f;
		this.m_cancelledLabelAlpha.alpha = 0f;
		this.m_numStartingTimeBankConsumables = UIDecisionTimer.GetNumStartingTimeBankConsumables();
		for (int i = 0; i < this.m_timeBankNotches.Length; i++)
		{
			if (i < this.m_numStartingTimeBankConsumables)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.Start()).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_timeBankNotches[i], true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_timeBankNotches[i], false, null);
			}
		}
		this.m_closeButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseSoloInfiniteTimerIndicator);
		UIManager.SetGameObjectActive(this.m_soloInfiniteTimerIndicator, false, null);
		this.m_initializedGameFlowData = false;
	}

	private void CloseSoloInfiniteTimerIndicator(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_soloInfiniteTimerIndicator, false, null);
		GameFlowData.Get().SetPausedForDialog(false);
	}

	public void SetUseTimeBankBars(bool useTimeBank)
	{
		UIManager.SetGameObjectActive(this.m_secondsLabel, !useTimeBank, null);
		UIManager.SetGameObjectActive(this.m_millisecondsLabel, !useTimeBank, null);
		UIManager.SetGameObjectActive(this.m_currentTimeImage, !useTimeBank, null);
		UIManager.SetGameObjectActive(this.m_timeBankSecondsLabel, useTimeBank, null);
		UIManager.SetGameObjectActive(this.m_timeBankMillisecondsLabel, useTimeBank, null);
		UIManager.SetGameObjectActive(this.m_timeBankImage, useTimeBank, null);
	}

	public void SetGameObjectActive(bool active)
	{
		UIManager.SetGameObjectActive(base.gameObject, active, null);
	}

	public void SetGlowingTimer(bool value)
	{
		if (this.m_glowOn == value)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.SetGlowingTimer(bool)).MethodHandle;
			}
			return;
		}
		this.m_glowOn = value;
		if (value)
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
			this.m_isPlayingAnim = true;
			float savedTimeToDisplay = this.GetSavedTimeToDisplay();
			if (savedTimeToDisplay > 0f)
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
				this.UpdateTimeLabels(this.m_savedSecondsLabel, this.m_savedMillisecondsLabel, savedTimeToDisplay);
				string stateName = "LockedSavedTimeIndicator";
				this.m_animationController.Play(stateName, -1, 0f);
			}
			if (!base.gameObject.activeSelf)
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
				this.FinishedSavedTimeAnimation();
			}
		}
		else
		{
			this.m_isPlayingAnim = true;
			string text = "UnlockedSavedTimeIndicator";
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_animationController.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo != null)
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
				if (currentAnimatorClipInfo.Length > 0)
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
					if (this.m_animationController.GetCurrentAnimatorClipInfo(0)[0].clip.name == text)
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
						this.m_animationController.Play(text, -1, 0f);
					}
					else
					{
						this.m_animationController.Play(text);
					}
				}
			}
			if (!base.gameObject.activeSelf)
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
				this.FinishedUnlockTimeAnimation();
			}
		}
	}

	public bool IsPlayingAnimation()
	{
		return this.m_isPlayingAnim;
	}

	public void FinishedSavedTimeAnimation()
	{
	}

	public void FinishedUnlockTimeAnimation()
	{
	}

	public void UpdateColor()
	{
		if (this.m_currentVal <= 0.16667f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateColor()).MethodHandle;
			}
		}
	}

	private float GetSavedTimeToDisplay()
	{
		return 0f;
	}

	public void UpdateSavedTimerPosition(float pct)
	{
		this.m_savedTimeVal = pct;
	}

	public void UpdateSliderAmount(float pct)
	{
		this.m_currentTimeVal = pct;
	}

	public void NotifyPhase(bool inDecision)
	{
		UIManager.SetGameObjectActive(this.m_timerContainer, inDecision, null);
		UIManager.SetGameObjectActive(this.m_sliderContainer, inDecision, null);
		UIManager.SetGameObjectActive(this.m_timeBankTextcontainer, inDecision, null);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateTimeLabels(TextMeshProUGUI, TextMeshProUGUI, float)).MethodHandle;
			}
			secondsLabel.text = num.ToString();
		}
		else
		{
			secondsLabel.text = "0" + num.ToString();
		}
		int num2 = Mathf.FloorToInt((timeToDisplay - (float)num) * 100f);
		if (num2 > 9)
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
			millisecondsLabel.text = ":" + num2;
		}
		else
		{
			millisecondsLabel.text = ":0" + num2;
		}
	}

	private void UpdateCurrentTimeBar()
	{
		if (this.m_currentTimeVal == this.m_lastCurrentTimeVal)
		{
			return;
		}
		if (this.m_currentTimeVal > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateCurrentTimeBar()).MethodHandle;
			}
			float currentTimeVal = this.m_currentTimeVal;
			this.m_currentTimeImage.fillAmount = currentTimeVal;
			UIManager.SetGameObjectActive(this.m_currentTimeImage, true, null);
			if (GameFlowData.Get() != null)
			{
				if (this.m_currentTimeVal * GameFlowData.Get().m_maxTurnTime < 5f)
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
					this.m_currentTimeImage.sprite = this.m_lowTimeBar;
				}
				else
				{
					this.m_currentTimeImage.sprite = this.m_defaultTimeBar;
				}
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_currentTimeImage, false, null);
		}
		this.m_lastCurrentTimeVal = this.m_currentTimeVal;
	}

	private void UpdateSavedTimeBar()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateSavedTimeBar()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameFlowData.Get().activeOwnedActorData.\u000E().CurrentState != TurnStateEnum.CONFIRMED)
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
					this.m_savedTimeVal = 0f;
					UIManager.SetGameObjectActive(this.m_savedTimeImage, false, null);
				}
			}
		}
		if (this.m_savedTimeVal == this.m_lastSavedTimeVal)
		{
			return;
		}
		if (this.m_savedTimeVal > 0f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_savedTimeImage, true, null);
			this.m_savedTimeImage.fillAmount = this.m_savedTimeVal;
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_savedTimeImage, false, null);
		}
		this.m_lastSavedTimeVal = this.m_savedTimeVal;
	}

	private void UpdateConsumables(int numConsumablesLeft, bool beingUsed)
	{
		for (int i = 0; i < this.m_timeBankNotches.Length; i++)
		{
			if (this.m_timeBankNotches[i].isInitialized)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateConsumables(int, bool)).MethodHandle;
				}
				if (i == this.m_numStartingTimeBankConsumables - numConsumablesLeft)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (beingUsed)
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
						this.m_timeBankNotches[i].Play("TimeBankItemDefaultUSED");
						goto IL_F7;
					}
				}
				if (i < this.m_numStartingTimeBankConsumables - numConsumablesLeft)
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
					this.m_timeBankNotches[i].Play("TimeBankItemDefaultOUT");
				}
				else
				{
					RectTransform[] componentsInChildren = this.m_timeBankNotches[i].GetComponentsInChildren<RectTransform>(true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						if (componentsInChildren[j].gameObject != this.m_timeBankNotches[i].gameObject)
						{
							UIManager.SetGameObjectActive(componentsInChildren[j], true, null);
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_timeBankNotches[i].Play("TimeBankItemDefaultIN");
				}
			}
			IL_F7:;
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

	private void UpdateTimeBankBar()
	{
		bool useTimeBankBars = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateTimeBankBar()).MethodHandle;
			}
			TimeBank component = GameFlowData.Get().activeOwnedActorData.GetComponent<TimeBank>();
			if (component != null)
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
				if (component.TimeToDisplay() <= 0f)
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
					if (GameFlowData.Get().WillEnterTimebankMode())
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
						useTimeBankBars = true;
						float num = component.TimeToDisplay() + GameWideData.Get().m_tbConsumableDuration;
						float fillAmount = num / GameWideData.Get().m_tbConsumableDuration;
						if (component.TimeToDisplay() < 0f)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.UpdateTimeLabels(this.m_timeBankSecondsLabel, this.m_timeBankMillisecondsLabel, num);
						}
						this.m_timeBankImage.fillAmount = fillAmount;
					}
				}
				this.UpdateConsumables(component.GetConsumablesRemaining(), component.GetConsumableUsed());
			}
		}
		this.SetUseTimeBankBars(useTimeBankBars);
	}

	private void UpdateCheckTimeMessageIndicators()
	{
		if (!this.m_initializedGameFlowData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDecisionTimer.UpdateCheckTimeMessageIndicators()).MethodHandle;
			}
			if (GameFlowData.Get() != null)
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
				this.m_initializedGameFlowData = true;
				if (GameFlowData.Get().PreventAutoLockInOnTimeout())
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
					UIManager.SetGameObjectActive(this.m_soloInfiniteTimerIndicator, true, null);
				}
			}
		}
		bool doActive = true;
		if (this.m_currentTimeVal > 0f)
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
			doActive = false;
		}
		else if (GameFlowData.Get() != null && GameFlowData.Get().PreventAutoLockInOnTimeout())
		{
			if (GameFlowData.Get().IsInDecisionState())
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
				if (GameFlowData.Get() != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameFlowData.Get().activeOwnedActorData != null)
					{
						TimeBank component = GameFlowData.Get().activeOwnedActorData.GetComponent<TimeBank>();
						if (component != null)
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
							if (component.TimeToDisplay() <= 0f)
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
								if (GameFlowData.Get().WillEnterTimebankMode())
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
						goto IL_177;
					}
				}
				doActive = false;
				IL_177:;
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
		if (this.m_soloInfiniteTimerIndicator.gameObject.activeSelf)
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
			doActive = false;
			if (GameFlowData.Get() != null)
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
				if (GameFlowData.Get().IsInResolveState())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_soloInfiniteTimerIndicator, false, null);
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_soloOutOfTimerIndicator, doActive, null);
	}

	private void Update()
	{
		this.UpdateCurrentTimeBar();
		this.UpdateSavedTimeBar();
		this.UpdateTimeBankBar();
		this.UpdateCheckTimeMessageIndicators();
	}

	private void OnEnable()
	{
		this.Update();
	}
}
