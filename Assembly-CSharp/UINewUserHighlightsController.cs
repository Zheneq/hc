using LobbyGameClientMessages;
using System;
using System.Text;
using UnityEngine;

public class UINewUserHighlightsController : UIScene
{
	public enum DisplayState
	{
		None,
		PlayButton,
		GameModeButton,
		ReadyButton,
		FluxEarned,
		LootMatrixNavButton,
		MainLootMatrixOpenButton,
		SpecialLootMatrixOpenButton,
		GGEarned,
		DailyContracts,
		DailyContractsPowerUp,
		DailyContractsFinish,
		FreelancerToken1,
		FreelancerToken2,
		ChapterInfoButton,
		SeasonNavButton,
		SeasonsChapters,
		SeasonsChapterReward,
		SeasonsMissions,
		SeasonsStory
	}

	[Serializable]
	public class Display
	{
		public DisplayState m_state;

		public Animator m_animator;

		public string m_animPrefix;

		public float m_maxTime;

		public DisplayState m_nextAutomaticState;

		public string m_audioEvent;
	}

	public bool m_debugMode;

	public bool m_enableNewOnboarding;

	public Display[] m_displays;

	[Header("Display Specific Objects")]
	public RectTransform m_dailyLeftPowerup;

	public RectTransform m_dailyCenterPowerup;

	public RectTransform m_dailyRightPowerup;

	private bool m_isOutAnimating;

	private DisplayState m_displayState;

	private DisplayState m_desiredDisplayState;

	private float m_timeInState;

	private bool m_previousClickState;

	private static UINewUserHighlightsController s_instance;

	public static UINewUserHighlightsController Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.NewUserHighlights;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
	}

	private void Update()
	{
		UpdateDisplay();
		if (Input.GetMouseButtonDown(0))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (!m_previousClickState && m_displayState != 0)
					{
						Display display = GetDisplay(m_displayState);
						if (display != null)
						{
							if (display.m_maxTime > 0f)
							{
								HideDisplay();
								SetDesiredDisplay(display.m_nextAutomaticState);
							}
						}
					}
					m_previousClickState = true;
					return;
				}
			}
		}
		m_previousClickState = false;
	}

	public void SetDesiredDisplay(DisplayState desiredDisplayState)
	{
		if (!m_enableNewOnboarding)
		{
			if (desiredDisplayState != DisplayState.MainLootMatrixOpenButton)
			{
				if (desiredDisplayState != DisplayState.GGEarned && desiredDisplayState != DisplayState.SpecialLootMatrixOpenButton)
				{
					if (desiredDisplayState != DisplayState.FluxEarned && desiredDisplayState != DisplayState.LootMatrixNavButton)
					{
						if (desiredDisplayState != DisplayState.DailyContracts)
						{
							if (desiredDisplayState != DisplayState.DailyContractsPowerUp)
							{
								if (desiredDisplayState != DisplayState.FreelancerToken1)
								{
									if (desiredDisplayState != DisplayState.FreelancerToken2)
									{
										if (desiredDisplayState != DisplayState.ChapterInfoButton)
										{
											if (desiredDisplayState != DisplayState.SeasonsChapters)
											{
												if (desiredDisplayState != DisplayState.SeasonsChapterReward)
												{
													if (desiredDisplayState != DisplayState.SeasonNavButton)
													{
														if (desiredDisplayState != DisplayState.SeasonsMissions)
														{
															if (desiredDisplayState != DisplayState.SeasonsStory)
															{
																goto IL_012a;
															}
														}
													}
												}
											}
										}
										UINewUserFlowManager.MarkSeasonsNew(false);
										ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType._001D);
										return;
									}
								}
							}
						}
					}
				}
			}
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.NumLootMatrixesOpened, 1, null);
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFluxHighlight, 1, null);
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedGGHighlight, 1, null);
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.NumDailiesChosen, 1, null);
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFreelancerTokenHighlight, 1, null);
			return;
		}
		goto IL_012a;
		IL_012a:
		if (desiredDisplayState == m_desiredDisplayState)
		{
			return;
		}
		while (true)
		{
			m_desiredDisplayState = desiredDisplayState;
			UpdateDisplay();
			return;
		}
	}

	public void HideDisplay()
	{
		m_desiredDisplayState = DisplayState.None;
		SetDisplay(DisplayState.None);
	}

	public DisplayState GetDisplayState()
	{
		return m_displayState;
	}

	private void UpdateDisplay()
	{
		m_timeInState += Time.deltaTime;
		if (m_displayState == m_desiredDisplayState)
		{
			Display display = GetDisplay(m_displayState);
			if (display == null)
			{
				return;
			}
			if (!(display.m_maxTime > 0f))
			{
				return;
			}
			if (!(m_timeInState > display.m_maxTime))
			{
				return;
			}
			m_desiredDisplayState = display.m_nextAutomaticState;
		}
		Display display2 = GetDisplay(m_displayState);
		if (display2 == null)
		{
			SetDisplay(m_desiredDisplayState);
			return;
		}
		if (m_isOutAnimating)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (!IsAnimationDone(display2, true))
					{
						if (display2.m_animator.gameObject.activeSelf)
						{
							return;
						}
					}
					SetDisplay(m_desiredDisplayState);
					return;
				}
			}
		}
		PlayAnim(display2, true);
		m_isOutAnimating = true;
	}

	private void SetDisplay(DisplayState newDisplayState)
	{
		if (m_displayState != 0)
		{
			Display display = GetDisplay(m_displayState);
			if (!display.m_audioEvent.IsNullOrEmpty() && AnnouncerSounds.GetAnnouncerSounds() != null)
			{
				AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
				AudioManager.PostEvent(display.m_audioEvent, AudioManager.EventAction.StopSound);
			}
		}
		Display display2 = GetDisplay(newDisplayState);
		m_isOutAnimating = false;
		SetHighlightActive(newDisplayState);
		if (display2 != null)
		{
			PlayAnim(display2, false);
			if (!display2.m_audioEvent.IsNullOrEmpty())
			{
				if (AnnouncerSounds.GetAnnouncerSounds() != null)
				{
					AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
					AudioManager.PostEvent(display2.m_audioEvent);
				}
			}
		}
		if (newDisplayState != DisplayState.PlayButton)
		{
			if (newDisplayState != DisplayState.DailyContractsFinish)
			{
				goto IL_0111;
			}
		}
		QuestListPanel.Get().SetVisible(true);
		goto IL_0111;
		IL_0111:
		DisplayState displayState = m_displayState;
		m_displayState = newDisplayState;
		m_timeInState = 0f;
		m_previousClickState = true;
		if (displayState == DisplayState.DailyContractsPowerUp)
		{
			if (newDisplayState != DisplayState.DailyContractsPowerUp)
			{
				QuestOfferPanel.Get().SetVisible(false);
				goto IL_017f;
			}
		}
		if (displayState == DisplayState.SeasonsStory && newDisplayState != DisplayState.SeasonsStory)
		{
			ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType._001D);
		}
		goto IL_017f;
		IL_017f:
		if (m_displayState == DisplayState.FluxEarned)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFluxHighlight, 1, null);
					return;
				}
			}
		}
		if (m_displayState == DisplayState.GGEarned)
		{
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedGGHighlight, 1, null);
		}
		else
		{
			if (m_displayState != DisplayState.FreelancerToken2)
			{
				return;
			}
			while (true)
			{
				ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFreelancerTokenHighlight, 1, null);
				return;
			}
		}
	}

	private Display GetDisplay(DisplayState state)
	{
		for (int i = 0; i < m_displays.Length; i++)
		{
			if (m_displays[i].m_state != state)
			{
				continue;
			}
			while (true)
			{
				return m_displays[i];
			}
		}
		while (true)
		{
			return null;
		}
	}

	private void SetHighlightActive(DisplayState displayState)
	{
		for (int i = 0; i < m_displays.Length; i++)
		{
			UIManager.SetGameObjectActive(m_displays[i].m_animator, m_displays[i].m_state == displayState);
		}
	}

	private void PlayAnim(Display display, bool isOut)
	{
		display.m_animator.Play(GetAnimName(display, isOut));
	}

	private bool IsAnimationDone(Display display, bool isOut)
	{
		if (!display.m_animator.isInitialized)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = display.m_animator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo.Length == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (display.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		string animName = GetAnimName(display, isOut);
		if (clip.name != animName)
		{
			return false;
		}
		return true;
	}

	private string GetAnimName(Display display, bool isOut)
	{
		string animPrefix = display.m_animPrefix;
		object str;
		if (isOut)
		{
			str = "OUT";
		}
		else
		{
			str = "IN";
		}
		return new StringBuilder().Append(animPrefix).Append((string)str).ToString();
	}

	public void SetDailyMissionSelected(int selectedIndex)
	{
		UIManager.SetGameObjectActive(m_dailyLeftPowerup, selectedIndex != 0);
		UIManager.SetGameObjectActive(m_dailyCenterPowerup, selectedIndex != 1);
		UIManager.SetGameObjectActive(m_dailyRightPowerup, selectedIndex != 2);
	}
}
