using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayCategoryMenu : MonoBehaviour
{
	public Animator m_playCategoryAnimator;

	public _SelectableBtn m_PracticeBtn;

	public _SelectableBtn m_SoloBtn;

	public _SelectableBtn m_CooperativeBtn;

	public _SelectableBtn m_VersusBtn;

	public _SelectableBtn m_RankedBtn;

	public _SelectableBtn m_CustomBtn;

	public RectTransform m_installDiscordContainer;

	public _SelectableBtn m_installDiscordBtn;

	public RectTransform m_installJoinContainer;

	public _ToggleSwap m_installJoinBtn;

	private List<_SelectableBtn> m_menuList;

	private bool m_visible;

	private bool m_autoJoinDiscord;

	private Dictionary<GameType, GameTypeAvailability> m_validGameTypes;

	private Scheduler m_taskScheduler;

	private Action m_checkDiscordStatusAction;

	private _SelectableBtn m_gamePadHoverBtn;

	public static UIPlayCategoryMenu Get()
	{
		if (UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				return UIFrontEnd.Get().m_frontEndNavPanel.m_playMenuCatgeory;
			}
		}
		return null;
	}

	private void Awake()
	{
		this.m_menuList = new List<_SelectableBtn>();
		this.m_PracticeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_SoloBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_CooperativeBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_VersusBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_RankedBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_CustomBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GameModeSelect;
		this.m_menuList.Add(this.m_PracticeBtn);
		this.m_menuList.Add(this.m_SoloBtn);
		this.m_menuList.Add(this.m_CooperativeBtn);
		this.m_menuList.Add(this.m_VersusBtn);
		this.m_menuList.Add(this.m_RankedBtn);
		this.m_menuList.Add(this.m_CustomBtn);
		this.m_CustomBtn.transform.SetAsLastSibling();
		this.m_installDiscordContainer.transform.SetAsLastSibling();
		this.m_installJoinContainer.transform.SetAsLastSibling();
		for (int i = 0; i < this.m_menuList.Count; i++)
		{
			_SelectableBtn btn = this.m_menuList[i];
			btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GameTypeClicked);
			btn.spriteController.SetForceHovercallback(true);
			btn.spriteController.SetForceExitCallback(true);
			btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.GameTypeTooltipSetup(tooltip, btn), null);
		}
		this.m_visible = true;
		this.SetVisible(false);
		this.m_installDiscordBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.InstallDiscordBtnClicked);
		if (Options_UI.Get() != null)
		{
			this.m_autoJoinDiscord = Options_UI.Get().GetEnableAutoJoinDiscord();
		}
		else
		{
			this.m_autoJoinDiscord = false;
		}
		this.m_installJoinBtn.SetOn(this.m_autoJoinDiscord, false);
		this.m_installJoinBtn.changedNotify = new _ToggleSwap.NotifyChanged(this.DiscordAutoJoinToggleClicked);
		this.m_taskScheduler = new Scheduler();
		this.m_checkDiscordStatusAction = delegate()
		{
			this.CheckDiscordStatus();
		};
	}

	private void Update()
	{
		bool flag = false;
		if (Options_UI.Get() != null)
		{
			flag = Options_UI.Get().GetEnableAutoJoinDiscord();
		}
		if (flag != this.m_autoJoinDiscord)
		{
			this.m_autoJoinDiscord = flag;
			this.m_installJoinBtn.SetOn(this.m_autoJoinDiscord, false);
		}
		if (GameManager.Get() != null)
		{
			if (!GameManager.Get().GameplayOverrides.DisableControlPadInput)
			{
				if (this.m_visible)
				{
					if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
					{
						this.m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
						if (this.m_gamePadHoverBtn == this.m_PracticeBtn)
						{
							this.m_gamePadHoverBtn = this.m_CustomBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_CooperativeBtn)
						{
							this.m_gamePadHoverBtn = this.m_PracticeBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_VersusBtn)
						{
							this.m_gamePadHoverBtn = this.m_CooperativeBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_RankedBtn)
						{
							this.m_gamePadHoverBtn = this.m_VersusBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_CustomBtn)
						{
							this.m_gamePadHoverBtn = this.m_RankedBtn;
						}
						this.m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
					}
					else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
					{
						this.m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
						if (this.m_gamePadHoverBtn == this.m_PracticeBtn)
						{
							this.m_gamePadHoverBtn = this.m_CooperativeBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_CooperativeBtn)
						{
							this.m_gamePadHoverBtn = this.m_VersusBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_VersusBtn)
						{
							this.m_gamePadHoverBtn = this.m_RankedBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_RankedBtn)
						{
							this.m_gamePadHoverBtn = this.m_CustomBtn;
						}
						else if (this.m_gamePadHoverBtn == this.m_CustomBtn)
						{
							this.m_gamePadHoverBtn = this.m_PracticeBtn;
						}
						this.m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
					}
					if (Input.GetButtonDown("GamepadButtonA"))
					{
						if (this.m_gamePadHoverBtn == this.m_CustomBtn)
						{
							this.CustomGameTypeClicked();
						}
						else
						{
							this.DoSelectGameType(this.m_gamePadHoverBtn.spriteController.gameObject);
						}
					}
				}
			}
		}
	}

	public void UITopCategoryDoneAnimOut()
	{
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	private void SetButtonClickable(_SelectableBtn btn, bool clickable)
	{
		btn.spriteController.SetClickable(clickable);
		UIManager.SetGameObjectActive(btn.spriteController.m_defaultImage, clickable, null);
		UIManager.SetGameObjectActive(btn.spriteController.m_hoverImage, clickable, null);
		UIManager.SetGameObjectActive(btn.spriteController.m_pressedImage, clickable, null);
	}

	public void UpdateGameTypeAvailability(Dictionary<GameType, GameTypeAvailability> validGameTypes)
	{
		this.m_validGameTypes = validGameTypes;
		using (Dictionary<GameType, GameTypeAvailability>.Enumerator enumerator = validGameTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<GameType, GameTypeAvailability> keyValuePair = enumerator.Current;
				bool clickable = false;
				ClientGameManager clientGameManager = ClientGameManager.Get();
				GameType key = keyValuePair.Key;
				GameTypeAvailability value = keyValuePair.Value;
				if (value.IsActive)
				{
					if (key == GameType.Ranked)
					{
						clickable = true;
						if (value.Requirements != null)
						{
							List<QueueRequirement> list = value.Requirements.ToList();
							for (int i = 0; i < list.Count; i++)
							{
								QueueRequirement queueRequirement = list[i];
								if (queueRequirement.Requirement == QueueRequirement.RequirementType.AccessLevel && queueRequirement.DoesApplicantPass(clientGameManager.QueueRequirementSystemInfo, clientGameManager.QueueRequirementApplicant, GameType.Ranked, null))
								{
									clickable = true;
									goto IL_F0;
								}
							}
						}
						IL_F0:;
					}
					else
					{
						DateTime? penaltyTimeout = value.PenaltyTimeout;
						bool flag = penaltyTimeout == null || DateTime.Now >= value.PenaltyTimeout.Value.ToLocalTime();
						if (flag)
						{
							if (clientGameManager.MeetsAllQueueRequirements(key))
							{
								clickable = true;
							}
						}
					}
				}
				if (key == GameType.Practice)
				{
					this.SetButtonClickable(this.m_PracticeBtn, clickable);
				}
				else if (key == GameType.Coop)
				{
					this.SetButtonClickable(this.m_CooperativeBtn, clickable);
				}
				else if (key == GameType.PvP)
				{
					this.SetButtonClickable(this.m_VersusBtn, clickable);
				}
				else if (key == GameType.Ranked)
				{
					this.SetButtonClickable(this.m_RankedBtn, clickable);
				}
			}
		}
	}

	public void SetMenuButtonsClickable(bool clickable)
	{
		if (ClientGameManager.Get().GroupInfo == null)
		{
			return;
		}
		bool flag;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.IsLeader)
			{
				flag = true;
				goto IL_6F;
			}
		}
		flag = !ClientGameManager.Get().GroupInfo.InAGroup;
		IL_6F:
		bool flag2 = flag;
		bool flag3 = clickable && UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn.IsSelected();
		if (flag3 && flag2)
		{
			this.UpdateGameTypeAvailability(ClientGameManager.Get().GameTypeAvailabilies);
		}
		else
		{
			for (int i = 0; i < this.m_menuList.Count; i++)
			{
				_SelectableBtn btn = this.m_menuList[i];
				bool clickable2;
				if (flag3)
				{
					clickable2 = flag2;
				}
				else
				{
					clickable2 = false;
				}
				this.SetButtonClickable(btn, clickable2);
			}
		}
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			this.SetButtonClickable(this.m_SoloBtn, false);
			this.SetButtonClickable(this.m_PracticeBtn, false);
			this.SetButtonClickable(this.m_CustomBtn, flag3);
		}
		else
		{
			this.SetButtonClickable(this.m_CustomBtn, flag3);
		}
		for (int j = 0; j < this.m_menuList.Count; j++)
		{
			if (!this.m_menuList[j].spriteController.IsClickable())
			{
				if (this.m_menuList[j].IsSelected())
				{
					int num = -1;
					for (int k = 0; k < this.m_menuList.Count; k++)
					{
						if (this.m_menuList[k].spriteController.IsClickable())
						{
							if (this.m_menuList[k] != this.m_CustomBtn)
							{
								num = k;
								break;
							}
						}
					}
					if (num != -1)
					{
						this.DoSelectGameType(this.m_menuList[num].spriteController.gameObject);
						return;
					}
				}
			}
		}
	}

	public void UpdateGroupInfo()
	{
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			this.SelectGroupGameType();
		}
	}

	private void SelectGroupGameType()
	{
		if (!(ClientGameManager.Get() == null))
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (this.m_menuList == null)
				{
				}
				else
				{
					UIPlayCategoryMenu.GameTypeButton gameTypeButton = this.GameTypeToGameTypeButton(ClientGameManager.Get().GroupInfo.SelectedQueueType);
					if (this.m_menuList[(int)gameTypeButton].IsSelected())
					{
						return;
					}
					GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
					if (UICharacterSelectScreenController.Get() != null)
					{
						int maxWillFillPerTeam = ClientGameManager.Get().GameTypeAvailabilies[selectedQueueType].MaxWillFillPerTeam;
						UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, maxWillFillPerTeam > 0, null);
						if (maxWillFillPerTeam == 0 && UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
						{
							UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
							{
								ClientRequestToServerSelectCharacter = new CharacterType?(CharacterType.Scoundrel)
							});
						}
					}
					UICharacterSelectScreen.Get().SelectedGameMode(ClientGameManager.Get().GroupInfo.SelectedQueueType);
					if (gameTypeButton == UIPlayCategoryMenu.GameTypeButton.Solo)
					{
						this.m_CooperativeBtn.SetSelected(true, false, string.Empty, string.Empty);
						this.m_gamePadHoverBtn = this.m_CooperativeBtn;
						UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
						{
							AllyBotTeammatesSelected = new bool?(true)
						});
					}
					else
					{
						for (int i = 0; i < this.m_menuList.Count; i++)
						{
							if (i == (int)gameTypeButton)
							{
								this.m_menuList[i].SetSelected(true, false, string.Empty, string.Empty);
								this.m_gamePadHoverBtn = this.m_menuList[i];
							}
							else
							{
								this.m_menuList[i].SetSelected(false, false, string.Empty, string.Empty);
							}
						}
					}
					UICharacterSelectScreenController.Get().SetupForRanked(ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked);
					return;
				}
			}
		}
	}

	public void DiscordAutoJoinToggleClicked(_ToggleSwap btn)
	{
		bool autoJoin = btn.IsChecked();
		ClientGameManager.Get().ConfigureDiscord(autoJoin);
	}

	public void InstallDiscordBtnClicked(BaseEventData data)
	{
		Application.OpenURL("https://discordapp.com/download");
		this.m_taskScheduler.AddTask(this.m_checkDiscordStatusAction, TimeSpan.FromSeconds(5.0), true);
	}

	private void CheckDiscordStatus()
	{
		if (DiscordClientInterface.IsEnabled)
		{
			if (!DiscordClientInterface.IsSdkEnabled)
			{
				if (!DiscordClientInterface.IsInstalled)
				{
					UIManager.SetGameObjectActive(this.m_installDiscordContainer, true, null);
					UIManager.SetGameObjectActive(this.m_installJoinContainer, false, null);
					this.m_taskScheduler.AddTask(this.m_checkDiscordStatusAction, TimeSpan.FromSeconds(5.0), true);
					goto IL_AA;
				}
			}
			UIManager.SetGameObjectActive(this.m_installDiscordContainer, false, null);
			UIManager.SetGameObjectActive(this.m_installJoinContainer, true, null);
			this.m_taskScheduler.RemoveTask(this.m_checkDiscordStatusAction);
			IL_AA:;
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_installDiscordContainer, false, null);
			UIManager.SetGameObjectActive(this.m_installJoinContainer, false, null);
		}
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	public void SetVisible(bool visible)
	{
		if (this.m_visible == visible)
		{
			return;
		}
		this.m_visible = visible;
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		if (visible)
		{
			this.m_playCategoryAnimator.Play("UITopCategoryIN", 0, 0f);
			this.SelectGroupGameType();
			this.CheckDiscordStatus();
		}
		else
		{
			this.m_playCategoryAnimator.Play("UITopCategoryOUT", 0, 0f);
		}
		for (int i = 0; i < this.m_menuList.Count; i++)
		{
			this.m_menuList[i].spriteController.SetClickable(visible);
		}
	}

	private UIPlayCategoryMenu.GameTypeButton GameTypeToGameTypeButton(GameType type)
	{
		UIPlayCategoryMenu.GameTypeButton result = UIPlayCategoryMenu.GameTypeButton.Versus;
		switch (type)
		{
		case GameType.Practice:
			result = UIPlayCategoryMenu.GameTypeButton.Practice;
			break;
		case GameType.Coop:
			result = UIPlayCategoryMenu.GameTypeButton.Cooperative;
			break;
		case GameType.PvP:
			result = UIPlayCategoryMenu.GameTypeButton.Versus;
			break;
		case GameType.Solo:
			result = UIPlayCategoryMenu.GameTypeButton.Solo;
			break;
		case GameType.Ranked:
			result = UIPlayCategoryMenu.GameTypeButton.Ranked;
			break;
		}
		return result;
	}

	private GameType GameTypeButtonToGameType(UIPlayCategoryMenu.GameTypeButton btn)
	{
		GameType result = GameType.None;
		switch (btn)
		{
		case UIPlayCategoryMenu.GameTypeButton.Practice:
			result = GameType.Practice;
			break;
		case UIPlayCategoryMenu.GameTypeButton.Solo:
			result = GameType.Solo;
			break;
		case UIPlayCategoryMenu.GameTypeButton.Cooperative:
			result = GameType.Coop;
			break;
		case UIPlayCategoryMenu.GameTypeButton.Versus:
			result = GameType.PvP;
			break;
		case UIPlayCategoryMenu.GameTypeButton.Ranked:
			result = GameType.Ranked;
			break;
		}
		return result;
	}

	private bool GameTypeTooltipSetup(UITooltipBase tooltip, _SelectableBtn btn)
	{
		if (!this.m_visible)
		{
			return false;
		}
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!btn.spriteController.IsClickable())
			{
				if (GameManager.Get().QueueInfo == null)
				{
					GameType gameType = GameType.None;
					if (btn == this.m_PracticeBtn)
					{
						gameType = GameType.Practice;
					}
					else if (btn == this.m_SoloBtn)
					{
						gameType = GameType.Solo;
					}
					else if (btn == this.m_CooperativeBtn)
					{
						gameType = GameType.Coop;
					}
					else if (btn == this.m_VersusBtn)
					{
						gameType = GameType.PvP;
					}
					else if (btn == this.m_RankedBtn)
					{
						gameType = GameType.Ranked;
					}
					else if (btn == this.m_CustomBtn)
					{
						gameType = GameType.Custom;
					}
					if (this.m_validGameTypes == null)
					{
						this.UpdateGameTypeAvailability(clientGameManager.GameTypeAvailabilies);
					}
					LocalizationPayload blockingQueueRestriction = clientGameManager.GetBlockingQueueRestriction(gameType);
					LobbyPlayerGroupInfo groupInfo = clientGameManager.GroupInfo;
					string tooltipText;
					if (blockingQueueRestriction != null)
					{
						tooltipText = blockingQueueRestriction.ToString();
					}
					else if (groupInfo != null && groupInfo.InAGroup && !groupInfo.IsLeader)
					{
						tooltipText = StringUtil.TR("OnlyLeaderCanChange", "Global");
					}
					else
					{
						GameTypeAvailability gameTypeAvailability;
						if (clientGameManager.GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
						{
							DateTime? penaltyTimeout = gameTypeAvailability.PenaltyTimeout;
							if (penaltyTimeout != null)
							{
								if (gameTypeAvailability.PenaltyTimeout != null)
								{
									tooltipText = LocalizationPayload.Create("QueueDodgePenaltyBlocksQueueEntry", "Matchmaking", new LocalizationArg[]
									{
										LocalizationArg_Handle.Create(clientGameManager.Handle)
									}).ToString();
									goto IL_253;
								}
							}
						}
						if (groupInfo != null && groupInfo.InAGroup && gameType == GameType.Custom)
						{
							tooltipText = StringUtil.TR("MustLeaveGroupBody", "Global");
						}
						else
						{
							tooltipText = StringUtil.TR("GameModeUnavailable", "Global");
						}
					}
					IL_253:
					UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
					uititledTooltip.Setup(StringUtil.TR("GameModeDisabled", "Global"), tooltipText, string.Empty);
					return true;
				}
			}
		}
		return false;
	}

	private void DoSelectGameType(GameObject btnHit)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		int num = -1;
		for (int i = 0; i < this.m_menuList.Count; i++)
		{
			if (this.m_menuList[i].spriteController.gameObject == btnHit)
			{
				num = i;
				this.m_menuList[i].SetSelected(true, false, string.Empty, string.Empty);
				this.m_gamePadHoverBtn = this.m_menuList[i];
			}
			else
			{
				this.m_menuList[i].SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		if (num != -1)
		{
			GameType gameType = this.GameTypeButtonToGameType((UIPlayCategoryMenu.GameTypeButton)num);
			AppState_GroupCharacterSelect.Get().SelectedGameMode(gameType);
			UICharacterSelectScreenController.Get().SetupForRanked(gameType == GameType.Ranked);
			if (btnHit == this.m_PracticeBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Practice);
			}
			else if (btnHit == this.m_CooperativeBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.CoOp);
			}
			else if (btnHit == this.m_VersusBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Pvp);
			}
			else if (btnHit == this.m_SoloBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Solo);
			}
			else if (btnHit == this.m_CustomBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Custom);
			}
			else if (btnHit == this.m_RankedBtn.spriteController.gameObject)
			{
				AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.Ranked);
			}
		}
	}

	public void GameTypeClicked(BaseEventData data)
	{
		if (UISeasonsPanel.Get().IsVisible())
		{
			return;
		}
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		if (gameObject == this.m_CustomBtn.spriteController.gameObject)
		{
			this.CustomGameTypeClicked();
			return;
		}
		this.DoSelectGameType(gameObject);
	}

	public void CustomGameTypeClicked()
	{
		ClientGameManager.Get().LeaveGame(true, GameResult.ClientLeft);
		Log.Info("Custom button Clicked, leaving queue", new object[0]);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		
		clientGameManager.LeaveQueue(delegate(LeaveMatchmakingQueueResponse r)
			{
				if (!r.Success)
				{
					Log.Warning("Failure to unqueue when entering custom: {0}", new object[]
					{
						r.ErrorMessage
					});
				}
			});
		AppState_GameTypeSelect.Get().OnCustomClicked();
		AppState_GroupCharacterSelect.Get().NotifyQueueDrop();
		this.SetVisible(false);
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UICharacterSelectScreen.Get().SelectedGameMode(GameType.Custom);
	}

	public GameType GetGameTypeForSelectedButton()
	{
		for (int i = 0; i < this.m_menuList.Count; i++)
		{
			if (this.m_menuList[i].IsSelected())
			{
				return this.GameTypeButtonToGameType((UIPlayCategoryMenu.GameTypeButton)i);
			}
		}
		return GameType.None;
	}

	public enum GameTypeButton
	{
		Practice,
		Solo,
		Cooperative,
		Versus,
		Ranked,
		Custom
	}
}
