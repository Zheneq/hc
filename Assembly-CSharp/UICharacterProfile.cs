using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterProfile : MonoBehaviour
{
	public GameObject m_visualObject;

	public Image m_aliveProfileImage;

	public Image m_deadProfileImage;

	public TextMeshProUGUI m_healthText;

	public TextMeshProUGUI m_pendingHealthText;

	public TextMeshProUGUI m_energyText;

	public TextMeshProUGUI m_shieldText;

	public Image m_tutorialEnergyGlow;

	public GameObject m_tutorialEnergyArrows;

	public Image[] m_ggButtonLevelImages;

	public _ButtonSwapSprite m_useGGPackBtn;

	public TextMeshProUGUI m_GGPackCount;

	public Animator m_ggPackBtnAnimator;

	public Image m_ggPackCooldown;

	[Range(0f, 1f)]
	public float m_energyPercent;

	public ImageFilledSloped m_energyImage;

	[Range(0f, 1f)]
	public float m_healthPercent;

	public ImageFilledSloped m_healthImage;

	[Range(0f, 1f)]
	public float m_shieldPercent;

	public ImageFilledSloped m_shieldBarImage;

	[Range(0f, 1f)]
	public float m_pendingHPPercent;

	public ImageFilledSloped m_pendingHPImage;

	public RectTransform m_tauntTransform;

	public Button m_tauntButton;

	public UITauntSelection m_tauntSelectionPanel;

	public LayoutGroup m_buffGrid;

	public LayoutGroup m_debuffGrid;

	public UIBuffIndicator m_buffIndicatorPrefab;

	private float m_lastEnergyPercent = -1f;

	private float m_lastShieldPercent = -1f;

	private float m_lastHealthPercent = -1f;

	private float m_lastPendingHealthPercent = -1f;

	private float m_ggPackTimeLastUsed = -1f;

	private bool m_selectionMenuOpen;

	private bool m_tauntIsEnabled;

	private List<StatusType> previousStatuses;

	private bool m_waitingForGGPackUseResponse;

	private bool m_hasGgPacks;

	private void Awake()
	{
		for (int i = 0; i < this.m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_ggButtonLevelImages[i], i == 0, null);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.Awake()).MethodHandle;
		}
		UIManager.SetGameObjectActive(this.m_tutorialEnergyGlow, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialEnergyArrows, false, null);
		this.previousStatuses = new List<StatusType>();
		if (this.m_useGGPackBtn != null)
		{
			this.m_useGGPackBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.UseGGPackBtnClicked);
			this.m_useGGPackBtn.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
			{
				if (this.m_hasGgPacks)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(UICharacterProfile.<Awake>m__0(UITooltipBase)).MethodHandle;
					}
					return false;
				}
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(StringUtil.TR("OutOfGGBoosts", "Global"), StringUtil.TR("YouAreOutOfGGBoosts", "Global"), string.Empty);
				return true;
			}, null);
			this.m_useGGPackBtn.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.GGPackMouseOver);
			this.m_useGGPackBtn.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.GGPackMouseExit);
		}
		ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
		this.HandleBankBalanceChange(null);
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
		}
	}

	public void HandleBankBalanceChange(CurrencyData currencyData)
	{
		this.m_GGPackCount.text = string.Format("x{0}", ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack));
	}

	public void Setup()
	{
		GameType gameType = GameManager.Get().GameConfig.GameType;
		int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
		bool flag;
		if (GameManager.IsGameTypeValidForGGPack(gameType) && num < 3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.Setup()).MethodHandle;
			}
			flag = !ReplayPlayManager.Get().IsPlayback();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		this.m_hasGgPacks = (ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack) > 0);
		this.m_useGGPackBtn.SetClickable(this.m_hasGgPacks);
		this.m_useGGPackBtn.SetForceExitCallback(!this.m_hasGgPacks);
		this.m_useGGPackBtn.SetForceHovercallback(!this.m_hasGgPacks);
		UIManager.SetGameObjectActive(this.m_ggPackBtnAnimator, flag2, null);
		this.m_waitingForGGPackUseResponse = false;
		if (flag2)
		{
			this.m_ggPackBtnAnimator.Play("HUDggPackIDLE");
		}
		for (int i = 0; i < this.m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_ggButtonLevelImages[i], i == num, null);
		}
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

	public void NotifyReceivedGGPackResponse()
	{
		this.m_waitingForGGPackUseResponse = false;
		this.Setup();
	}

	public void GGPackAnimDone()
	{
		if (!this.m_waitingForGGPackUseResponse)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.GGPackAnimDone()).MethodHandle;
			}
			this.Setup();
		}
	}

	public void GGPackMouseOver(BaseEventData data)
	{
		if (this.m_hasGgPacks)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.GGPackMouseOver(BaseEventData)).MethodHandle;
			}
			if (this.m_ggPackBtnAnimator.isActiveAndEnabled)
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
				AnimatorClipInfo[] currentAnimatorClipInfo = this.m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo != null && currentAnimatorClipInfo.Length > 0)
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
					if (currentAnimatorClipInfo[0].clip.name != "HUDggPackPRESS")
					{
						this.m_ggPackBtnAnimator.Play("HUDggPackHOVER");
					}
				}
			}
		}
	}

	public void GGPackMouseExit(BaseEventData data)
	{
		if (this.m_hasGgPacks)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.GGPackMouseExit(BaseEventData)).MethodHandle;
			}
			if (this.m_ggPackBtnAnimator.isActiveAndEnabled)
			{
				AnimatorClipInfo[] currentAnimatorClipInfo = this.m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo != null)
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
					if (currentAnimatorClipInfo.Length > 0)
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
						if (currentAnimatorClipInfo[0].clip.name != "HUDggPackPRESS")
						{
							this.m_ggPackBtnAnimator.Play("HUDggPackIDLE");
						}
					}
				}
			}
		}
	}

	public void UseGGPackBtnClicked(BaseEventData data)
	{
		if (Time.unscaledTime > this.m_ggPackTimeLastUsed + GameBalanceVars.Get().GGPackInGameCooldownTimer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UseGGPackBtnClicked(BaseEventData)).MethodHandle;
			}
			this.m_ggPackTimeLastUsed = Time.unscaledTime;
			this.m_useGGPackBtn.SetClickable(false);
			this.m_ggPackBtnAnimator.Play("HUDggPackPRESS");
			int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
			if (num == 0)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.GGButtonInGameUsed);
			}
			else if (num == 1)
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
				AudioManager.PostEvent("ui/endgame/ggboost_button_silver", null);
			}
			else if (num == 2)
			{
				AudioManager.PostEvent("ui/endgame/ggboost_button_gold", null);
			}
			if (this.m_hasGgPacks)
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
				ClientGameManager.Get().RequestToUseGGPack();
				this.m_waitingForGGPackUseResponse = true;
			}
		}
	}

	private void Start()
	{
		if (this.m_tauntButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_tauntButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTauntClick));
		}
		if (this.m_tauntSelectionPanel.m_closeSelectionButton != null)
		{
			UIEventTriggerUtils.AddListener(this.m_tauntSelectionPanel.m_closeSelectionButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnCloseSelectionClick));
		}
		this.m_tauntIsEnabled = true;
		this.ShowTaunt(false);
	}

	private void UpdatePendingHealthBar()
	{
		if (this.m_lastPendingHealthPercent == this.m_pendingHPPercent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UpdatePendingHealthBar()).MethodHandle;
			}
			return;
		}
		if (this.m_healthPercent > 0f)
		{
			this.m_pendingHPImage.fillAmount = this.m_pendingHPPercent;
			UIManager.SetGameObjectActive(this.m_pendingHPImage, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_pendingHPImage, false, null);
		}
		this.m_lastPendingHealthPercent = this.m_pendingHPPercent;
	}

	private void UpdateHealthBar()
	{
		if (this.m_lastHealthPercent == this.m_healthPercent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UpdateHealthBar()).MethodHandle;
			}
			return;
		}
		if (this.m_healthPercent > 0f)
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
			this.m_healthImage.fillAmount = this.m_healthPercent;
			UIManager.SetGameObjectActive(this.m_healthImage, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_healthImage, false, null);
		}
		this.m_lastHealthPercent = this.m_healthPercent;
	}

	private void UpdateShieldBar()
	{
		if (this.m_lastShieldPercent == this.m_shieldPercent)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UpdateShieldBar()).MethodHandle;
			}
			return;
		}
		if (this.m_shieldPercent > 0f)
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
			this.m_shieldBarImage.fillAmount = this.m_shieldPercent;
			UIManager.SetGameObjectActive(this.m_shieldBarImage, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_shieldBarImage, false, null);
		}
		this.m_lastShieldPercent = this.m_shieldPercent;
	}

	private void UpdateEnergyBar()
	{
		if (this.m_lastEnergyPercent == this.m_energyPercent)
		{
			return;
		}
		if (this.m_energyPercent > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UpdateEnergyBar()).MethodHandle;
			}
			this.m_energyImage.fillAmount = this.m_energyPercent;
			UIManager.SetGameObjectActive(this.m_energyImage, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_energyImage, false, null);
		}
		this.m_lastEnergyPercent = this.m_energyPercent;
	}

	private bool CanTaunt(ActorData actor)
	{
		bool flag = false;
		if (GameManager.Get().GameplayOverrides.AreTauntsEnabled())
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.CanTaunt(ActorData)).MethodHandle;
				}
				TurnStateEnum currentState = activeOwnedActorData.\u000E().CurrentState;
				if (currentState != TurnStateEnum.DECIDING && currentState != TurnStateEnum.CONFIRMED)
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
					if (currentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
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
						if (currentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
						{
							return flag;
						}
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
				}
				ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
				AbilityData abilityData = activeOwnedActorData.\u000E();
				List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.\u000E().GetAutoQueuedRequestActionTypes();
				int num = 0;
				while (!flag)
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
					if (num >= autoQueuedRequestActionTypes.Count)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							goto IL_EC;
						}
					}
					else
					{
						flag = UICharacterProfile.CanTauntForAction(actor, abilityData, component, autoQueuedRequestActionTypes[num]);
						num++;
					}
				}
				IL_EC:
				if (!flag)
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
					List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.\u000E().GetRequestStackForUndo();
					int num2 = 0;
					while (!flag)
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
						if (num2 >= requestStackForUndo.Count)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								return flag;
							}
						}
						else
						{
							flag = UICharacterProfile.CanTauntForAction(actor, abilityData, component, requestStackForUndo[num2].m_action);
							num2++;
						}
					}
				}
			}
		}
		return flag;
	}

	public static bool CanTauntForAction(ActorData actor, AbilityData abilityData, ActorCinematicRequests cinematicRequests, AbilityData.ActionType actionType)
	{
		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
		CharacterResourceLink character = GameFlowData.Get().activeOwnedActorData.\u000E();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(actor.m_characterType);
		if (abilityOfActionType != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.CanTauntForAction(ActorData, AbilityData, ActorCinematicRequests, AbilityData.ActionType)).MethodHandle;
			}
			if (playerCharacterData != null)
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
				if (!cinematicRequests.IsAbilityCinematicRequested(actionType))
				{
					TauntCameraSet tauntCamSetData = abilityData.GetComponent<ActorData>().m_tauntCamSetData;
					if (tauntCamSetData != null)
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
						for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
						{
							CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
							if (cameraShotSequence != null)
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
								if (abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence.m_animIndex) && cinematicRequests.NumRequestsLeft(cameraShotSequence.m_uniqueTauntID) > 0)
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
									if (AbilityData.CanTauntForActionTypeForPlayer(playerCharacterData, character, actionType, true, cameraShotSequence.m_uniqueTauntID))
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	private void ShowTauntSelection(bool visible)
	{
		if (!this.m_tauntIsEnabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.ShowTauntSelection(bool)).MethodHandle;
			}
			return;
		}
		this.m_selectionMenuOpen = visible;
		UIManager.SetGameObjectActive(this.m_tauntSelectionPanel, this.m_selectionMenuOpen, null);
		int num = this.m_tauntSelectionPanel.SetupTauntList();
		UIManager.SetGameObjectActive(this.m_tauntTransform, !this.m_selectionMenuOpen, null);
		if (visible && num == 1)
		{
			this.m_tauntSelectionPanel.m_tauntButtons[0].SelectedTaunt(null);
		}
	}

	public void OnTauntClick(BaseEventData data)
	{
		this.ShowTauntSelection(true);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntClick);
	}

	public void OnCloseSelectionClick(BaseEventData data)
	{
		this.ShowTauntSelection(false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntSelect);
	}

	public void UpdateStatusDisplay(bool forceUpdate)
	{
		if (!(this.m_buffGrid == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.UpdateStatusDisplay(bool)).MethodHandle;
			}
			if (!(this.m_debuffGrid == null))
			{
				ActorStatus actorStatus = GameFlowData.Get().activeOwnedActorData.\u000E();
				List<StatusType> list = new List<StatusType>();
				bool flag = false;
				if (actorStatus != null)
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
					for (int i = 0; i < 0x3A; i++)
					{
						StatusType statusType = (StatusType)i;
						if (actorStatus.HasStatus(statusType, false))
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
							if (HUD_UIResources.GetIconForStatusType(statusType).displayIcon)
							{
								list.Add(statusType);
								if (this.previousStatuses != null)
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
									if (!this.previousStatuses.Contains(statusType))
									{
										flag = true;
									}
								}
							}
						}
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (this.previousStatuses != null)
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
					if (this.previousStatuses.Count != list.Count)
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
						flag = true;
					}
				}
				if (!flag)
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
					if (!forceUpdate)
					{
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
				UIBuffIndicator[] componentsInChildren = this.m_buffGrid.GetComponentsInChildren<UIBuffIndicator>(false);
				UIBuffIndicator[] componentsInChildren2 = this.m_debuffGrid.GetComponentsInChildren<UIBuffIndicator>(false);
				List<UIBuffIndicator> list2 = new List<UIBuffIndicator>(componentsInChildren);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					list2.Add(componentsInChildren2[j]);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				while (list2.Count > list.Count)
				{
					UIManager.SetGameObjectActive(list2[0], false, null);
					UnityEngine.Object.Destroy(list2[0].gameObject);
					list2.RemoveAt(0);
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
				while (list2.Count < list.Count)
				{
					UIBuffIndicator item = UnityEngine.Object.Instantiate<UIBuffIndicator>(this.m_buffIndicatorPrefab);
					list2.Add(item);
				}
				for (int k = 0; k < list.Count; k++)
				{
					UIBuffIndicator uibuffIndicator = list2[k];
					if (HUD_UIResources.GetIconForStatusType(list[k]).isDebuff)
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
						uibuffIndicator.transform.SetParent(this.m_debuffGrid.transform);
					}
					else
					{
						uibuffIndicator.transform.SetParent(this.m_buffGrid.transform);
					}
					uibuffIndicator.transform.localScale = Vector3.one;
					uibuffIndicator.transform.localPosition = Vector3.zero;
					uibuffIndicator.transform.localEulerAngles = Vector3.zero;
					list2[k].Setup(list[k], actorStatus.GetDurationOfStatus(list[k]));
				}
				this.m_debuffGrid.enabled = false;
				this.m_debuffGrid.enabled = true;
				this.m_buffGrid.enabled = false;
				this.m_buffGrid.enabled = true;
				this.previousStatuses = list;
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

	private void ShowTaunt(bool visible)
	{
		if (this.m_tauntIsEnabled == visible)
		{
			return;
		}
		this.m_tauntIsEnabled = visible;
		if (!visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.ShowTaunt(bool)).MethodHandle;
			}
			this.m_selectionMenuOpen = false;
			UIManager.SetGameObjectActive(this.m_tauntSelectionPanel, this.m_selectionMenuOpen, null);
		}
		UIManager.SetGameObjectActive(this.m_tauntTransform, visible, null);
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterProfile.Update()).MethodHandle;
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				bool doActive = false;
				bool doActive2 = false;
				if (this.m_visualObject != null)
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
					if (!this.m_visualObject.activeSelf)
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
						UIManager.SetGameObjectActive(this.m_visualObject, true, null);
					}
				}
				this.m_aliveProfileImage.sprite = activeOwnedActorData.\u000E();
				this.m_deadProfileImage.sprite = activeOwnedActorData.\u0012();
				if (activeOwnedActorData.\u000E())
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
					doActive2 = true;
				}
				else
				{
					doActive = true;
				}
				int num = activeOwnedActorData.\u0009();
				int num2 = activeOwnedActorData.\u0012();
				int num3 = activeOwnedActorData.\u0019();
				int num4 = activeOwnedActorData.\u0016();
				int num5 = activeOwnedActorData.\u0004();
				int num6 = activeOwnedActorData.\u0011();
				if (num6 > 0)
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
					this.m_pendingHealthText.text = "+" + num6.ToString();
				}
				else
				{
					this.m_pendingHealthText.text = string.Empty;
				}
				if (num5 > 0)
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
					this.m_shieldText.text = "+" + num5.ToString();
				}
				else
				{
					this.m_shieldText.text = string.Empty;
				}
				this.m_healthText.text = num.ToString();
				this.m_energyText.text = num3.ToString();
				this.m_healthPercent = (float)num / (float)(num2 + num5);
				this.m_shieldPercent = (float)(num + num5) / (float)(num2 + num5);
				this.m_pendingHPPercent = (float)(num + num5 + num6) / (float)(num2 + num5);
				this.m_energyPercent = (float)num3 / (float)num4;
				if (this.CanTaunt(activeOwnedActorData))
				{
					this.ShowTaunt(true);
				}
				else
				{
					this.ShowTaunt(false);
				}
				UIManager.SetGameObjectActive(this.m_aliveProfileImage, doActive, null);
				UIManager.SetGameObjectActive(this.m_deadProfileImage, doActive2, null);
			}
			else if (this.m_visualObject != null && this.m_visualObject.activeSelf)
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
				UIManager.SetGameObjectActive(this.m_visualObject, false, null);
			}
		}
		this.UpdateEnergyBar();
		this.UpdateHealthBar();
		this.UpdateShieldBar();
		this.UpdatePendingHealthBar();
		float num7 = 1f;
		if (this.m_ggPackTimeLastUsed > 0f)
		{
			float num8 = Time.unscaledTime - this.m_ggPackTimeLastUsed;
			num7 = Mathf.Clamp01(num8 / GameBalanceVars.Get().GGPackInGameCooldownTimer);
		}
		this.m_ggPackCooldown.fillAmount = 1f - num7;
	}

	private void OnEnable()
	{
		this.m_lastEnergyPercent = 1f;
		this.Update();
	}
}
