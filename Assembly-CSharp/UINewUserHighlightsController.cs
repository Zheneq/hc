using System;
using LobbyGameClientMessages;
using UnityEngine;

public class UINewUserHighlightsController : UIScene
{
	public bool m_debugMode;

	public bool m_enableNewOnboarding;

	public UINewUserHighlightsController.Display[] m_displays;

	[Header("Display Specific Objects")]
	public RectTransform m_dailyLeftPowerup;

	public RectTransform m_dailyCenterPowerup;

	public RectTransform m_dailyRightPowerup;

	private bool m_isOutAnimating;

	private UINewUserHighlightsController.DisplayState m_displayState;

	private UINewUserHighlightsController.DisplayState m_desiredDisplayState;

	private float m_timeInState;

	private bool m_previousClickState;

	private static UINewUserHighlightsController s_instance;

	public static UINewUserHighlightsController Get()
	{
		return UINewUserHighlightsController.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.NewUserHighlights;
	}

	public override void Awake()
	{
		UINewUserHighlightsController.s_instance = this;
		base.Awake();
	}

	private void Update()
	{
		this.UpdateDisplay();
		if (Input.GetMouseButtonDown(0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.Update()).MethodHandle;
			}
			if (!this.m_previousClickState && this.m_displayState != UINewUserHighlightsController.DisplayState.None)
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
				UINewUserHighlightsController.Display display = this.GetDisplay(this.m_displayState);
				if (display != null)
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
					if (display.m_maxTime > 0f)
					{
						this.HideDisplay();
						this.SetDesiredDisplay(display.m_nextAutomaticState);
					}
				}
			}
			this.m_previousClickState = true;
		}
		else
		{
			this.m_previousClickState = false;
		}
	}

	public void SetDesiredDisplay(UINewUserHighlightsController.DisplayState desiredDisplayState)
	{
		if (!this.m_enableNewOnboarding)
		{
			if (desiredDisplayState != UINewUserHighlightsController.DisplayState.MainLootMatrixOpenButton)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.SetDesiredDisplay(UINewUserHighlightsController.DisplayState)).MethodHandle;
				}
				if (desiredDisplayState != UINewUserHighlightsController.DisplayState.GGEarned && desiredDisplayState != UINewUserHighlightsController.DisplayState.SpecialLootMatrixOpenButton)
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
					if (desiredDisplayState != UINewUserHighlightsController.DisplayState.FluxEarned && desiredDisplayState != UINewUserHighlightsController.DisplayState.LootMatrixNavButton)
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
						if (desiredDisplayState != UINewUserHighlightsController.DisplayState.DailyContracts)
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
							if (desiredDisplayState != UINewUserHighlightsController.DisplayState.DailyContractsPowerUp)
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
								if (desiredDisplayState != UINewUserHighlightsController.DisplayState.FreelancerToken1)
								{
									if (desiredDisplayState != UINewUserHighlightsController.DisplayState.FreelancerToken2)
									{
										if (desiredDisplayState != UINewUserHighlightsController.DisplayState.ChapterInfoButton)
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
											if (desiredDisplayState != UINewUserHighlightsController.DisplayState.SeasonsChapters)
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
												if (desiredDisplayState != UINewUserHighlightsController.DisplayState.SeasonsChapterReward)
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
													if (desiredDisplayState != UINewUserHighlightsController.DisplayState.SeasonNavButton)
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
														if (desiredDisplayState != UINewUserHighlightsController.DisplayState.SeasonsMissions)
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
															if (desiredDisplayState != UINewUserHighlightsController.DisplayState.SeasonsStory)
															{
																goto IL_12A;
															}
														}
													}
												}
											}
										}
										UINewUserFlowManager.MarkSeasonsNew(false);
										ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType.\u001D);
										return;
									}
									for (;;)
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
		IL_12A:
		if (desiredDisplayState != this.m_desiredDisplayState)
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
			this.m_desiredDisplayState = desiredDisplayState;
			this.UpdateDisplay();
		}
	}

	public void HideDisplay()
	{
		this.m_desiredDisplayState = UINewUserHighlightsController.DisplayState.None;
		this.SetDisplay(UINewUserHighlightsController.DisplayState.None);
	}

	public UINewUserHighlightsController.DisplayState GetDisplayState()
	{
		return this.m_displayState;
	}

	private void UpdateDisplay()
	{
		this.m_timeInState += Time.deltaTime;
		if (this.m_displayState == this.m_desiredDisplayState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.UpdateDisplay()).MethodHandle;
			}
			UINewUserHighlightsController.Display display = this.GetDisplay(this.m_displayState);
			if (display != null)
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
				if (display.m_maxTime > 0f)
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
					if (this.m_timeInState > display.m_maxTime)
					{
						this.m_desiredDisplayState = display.m_nextAutomaticState;
						goto IL_83;
					}
				}
			}
			return;
		}
		IL_83:
		UINewUserHighlightsController.Display display2 = this.GetDisplay(this.m_displayState);
		if (display2 == null)
		{
			this.SetDisplay(this.m_desiredDisplayState);
		}
		else if (this.m_isOutAnimating)
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
			if (!this.IsAnimationDone(display2, true))
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
				if (display2.m_animator.gameObject.activeSelf)
				{
					goto IL_F5;
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
			}
			this.SetDisplay(this.m_desiredDisplayState);
			IL_F5:;
		}
		else
		{
			this.PlayAnim(display2, true);
			this.m_isOutAnimating = true;
		}
	}

	private void SetDisplay(UINewUserHighlightsController.DisplayState newDisplayState)
	{
		if (this.m_displayState != UINewUserHighlightsController.DisplayState.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.SetDisplay(UINewUserHighlightsController.DisplayState)).MethodHandle;
			}
			UINewUserHighlightsController.Display display = this.GetDisplay(this.m_displayState);
			if (!display.m_audioEvent.IsNullOrEmpty() && AnnouncerSounds.GetAnnouncerSounds() != null)
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
				AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
				AudioManager.PostEvent(display.m_audioEvent, AudioManager.EventAction.StopSound, null, null);
			}
		}
		UINewUserHighlightsController.Display display2 = this.GetDisplay(newDisplayState);
		this.m_isOutAnimating = false;
		this.SetHighlightActive(newDisplayState);
		if (display2 != null)
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
			this.PlayAnim(display2, false);
			if (!display2.m_audioEvent.IsNullOrEmpty())
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
				if (AnnouncerSounds.GetAnnouncerSounds() != null)
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
					AnnouncerSounds.GetAnnouncerSounds().InstantiateOnboardingVOPrefabIfNeeded();
					AudioManager.PostEvent(display2.m_audioEvent, null);
				}
			}
		}
		if (newDisplayState != UINewUserHighlightsController.DisplayState.PlayButton)
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
			if (newDisplayState != UINewUserHighlightsController.DisplayState.DailyContractsFinish)
			{
				goto IL_111;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		QuestListPanel.Get().SetVisible(true, false, false);
		IL_111:
		UINewUserHighlightsController.DisplayState displayState = this.m_displayState;
		this.m_displayState = newDisplayState;
		this.m_timeInState = 0f;
		this.m_previousClickState = true;
		if (displayState == UINewUserHighlightsController.DisplayState.DailyContractsPowerUp)
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
			if (newDisplayState != UINewUserHighlightsController.DisplayState.DailyContractsPowerUp)
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
				QuestOfferPanel.Get().SetVisible(false);
				goto IL_17F;
			}
		}
		if (displayState == UINewUserHighlightsController.DisplayState.SeasonsStory && newDisplayState != UINewUserHighlightsController.DisplayState.SeasonsStory)
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
			ClientGameManager.Get().SendSeasonStatusConfirm(SeasonStatusConfirmed.DialogType.\u001D);
		}
		IL_17F:
		if (this.m_displayState == UINewUserHighlightsController.DisplayState.FluxEarned)
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
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFluxHighlight, 1, null);
		}
		else if (this.m_displayState == UINewUserHighlightsController.DisplayState.GGEarned)
		{
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedGGHighlight, 1, null);
		}
		else if (this.m_displayState == UINewUserHighlightsController.DisplayState.FreelancerToken2)
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
			ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasViewedFreelancerTokenHighlight, 1, null);
		}
	}

	private UINewUserHighlightsController.Display GetDisplay(UINewUserHighlightsController.DisplayState state)
	{
		for (int i = 0; i < this.m_displays.Length; i++)
		{
			if (this.m_displays[i].m_state == state)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.GetDisplay(UINewUserHighlightsController.DisplayState)).MethodHandle;
				}
				return this.m_displays[i];
			}
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
		return null;
	}

	private void SetHighlightActive(UINewUserHighlightsController.DisplayState displayState)
	{
		for (int i = 0; i < this.m_displays.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_displays[i].m_animator, this.m_displays[i].m_state == displayState, null);
		}
	}

	private void PlayAnim(UINewUserHighlightsController.Display display, bool isOut)
	{
		display.m_animator.Play(this.GetAnimName(display, isOut));
	}

	private bool IsAnimationDone(UINewUserHighlightsController.Display display, bool isOut)
	{
		if (!display.m_animator.isInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.IsAnimationDone(UINewUserHighlightsController.Display, bool)).MethodHandle;
			}
			return false;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = display.m_animator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo.Length == 0)
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
			return false;
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
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
			return false;
		}
		if (display.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
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
			return false;
		}
		string animName = this.GetAnimName(display, isOut);
		return !(clip.name != animName);
	}

	private string GetAnimName(UINewUserHighlightsController.Display display, bool isOut)
	{
		string animPrefix = display.m_animPrefix;
		string str;
		if (isOut)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINewUserHighlightsController.GetAnimName(UINewUserHighlightsController.Display, bool)).MethodHandle;
			}
			str = "OUT";
		}
		else
		{
			str = "IN";
		}
		return animPrefix + str;
	}

	public void SetDailyMissionSelected(int selectedIndex)
	{
		UIManager.SetGameObjectActive(this.m_dailyLeftPowerup, selectedIndex != 0, null);
		UIManager.SetGameObjectActive(this.m_dailyCenterPowerup, selectedIndex != 1, null);
		UIManager.SetGameObjectActive(this.m_dailyRightPowerup, selectedIndex != 2, null);
	}

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
		public UINewUserHighlightsController.DisplayState m_state;

		public Animator m_animator;

		public string m_animPrefix;

		public float m_maxTime;

		public UINewUserHighlightsController.DisplayState m_nextAutomaticState;

		public string m_audioEvent;
	}
}
