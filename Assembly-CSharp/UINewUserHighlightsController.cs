using LobbyGameClientMessages;
using System;
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!m_previousClickState && m_displayState != 0)
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
						Display display = GetDisplay(m_displayState);
						if (display != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (desiredDisplayState != DisplayState.GGEarned && desiredDisplayState != DisplayState.SpecialLootMatrixOpenButton)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (desiredDisplayState != DisplayState.FluxEarned && desiredDisplayState != DisplayState.LootMatrixNavButton)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (desiredDisplayState != DisplayState.DailyContracts)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (desiredDisplayState != DisplayState.DailyContractsPowerUp)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (desiredDisplayState != DisplayState.FreelancerToken1)
								{
									if (desiredDisplayState != DisplayState.FreelancerToken2)
									{
										if (desiredDisplayState != DisplayState.ChapterInfoButton)
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												break;
											}
											if (desiredDisplayState != DisplayState.SeasonsChapters)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														continue;
													}
													break;
												}
												if (desiredDisplayState != DisplayState.SeasonsChapterReward)
												{
													while (true)
													{
														switch (1)
														{
														case 0:
															continue;
														}
														break;
													}
													if (desiredDisplayState != DisplayState.SeasonNavButton)
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
														if (desiredDisplayState != DisplayState.SeasonsMissions)
														{
															while (true)
															{
																switch (2)
																{
																case 0:
																	continue;
																}
																break;
															}
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
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
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
			switch (1)
			{
			case 0:
				continue;
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Display display = GetDisplay(m_displayState);
			if (display == null)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(display.m_maxTime > 0f))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (display2.m_animator.gameObject.activeSelf)
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
							break;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Display display = GetDisplay(m_displayState);
			if (!display.m_audioEvent.IsNullOrEmpty() && AnnouncerSounds.GetAnnouncerSounds() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
				AudioManager.PostEvent(display.m_audioEvent, AudioManager.EventAction.StopSound);
			}
		}
		Display display2 = GetDisplay(newDisplayState);
		m_isOutAnimating = false;
		SetHighlightActive(newDisplayState);
		if (display2 != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			PlayAnim(display2, false);
			if (!display2.m_audioEvent.IsNullOrEmpty())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (AnnouncerSounds.GetAnnouncerSounds() != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
					AudioManager.PostEvent(display2.m_audioEvent);
				}
			}
		}
		if (newDisplayState != DisplayState.PlayButton)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (newDisplayState != DisplayState.DailyContractsFinish)
			{
				goto IL_0111;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (newDisplayState != DisplayState.DailyContractsPowerUp)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				QuestOfferPanel.Get().SetVisible(false);
				goto IL_017f;
			}
		}
		if (displayState == DisplayState.SeasonsStory && newDisplayState != DisplayState.SeasonsStory)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
				switch (6)
				{
				case 0:
					continue;
				}
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
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return m_displays[i];
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			str = "OUT";
		}
		else
		{
			str = "IN";
		}
		return animPrefix + (string)str;
	}

	public void SetDailyMissionSelected(int selectedIndex)
	{
		UIManager.SetGameObjectActive(m_dailyLeftPowerup, selectedIndex != 0);
		UIManager.SetGameObjectActive(m_dailyCenterPowerup, selectedIndex != 1);
		UIManager.SetGameObjectActive(m_dailyRightPowerup, selectedIndex != 2);
	}
}
