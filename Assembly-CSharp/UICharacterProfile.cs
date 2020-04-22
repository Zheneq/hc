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
		for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == 0);
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
			UIManager.SetGameObjectActive(m_tutorialEnergyGlow, false);
			UIManager.SetGameObjectActive(m_tutorialEnergyArrows, false);
			previousStatuses = new List<StatusType>();
			if (m_useGGPackBtn != null)
			{
				m_useGGPackBtn.callback = UseGGPackBtnClicked;
				m_useGGPackBtn.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
				{
					if (m_hasGgPacks)
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
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
					uITitledTooltip.Setup(StringUtil.TR("OutOfGGBoosts", "Global"), StringUtil.TR("YouAreOutOfGGBoosts", "Global"), string.Empty);
					return true;
				});
				m_useGGPackBtn.pointerEnterCallback = GGPackMouseOver;
				m_useGGPackBtn.pointerExitCallback = GGPackMouseExit;
			}
			ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
			HandleBankBalanceChange(null);
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
			return;
		}
	}

	public void HandleBankBalanceChange(CurrencyData currencyData)
	{
		m_GGPackCount.text = $"x{ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack)}";
	}

	public void Setup()
	{
		GameType gameType = GameManager.Get().GameConfig.GameType;
		int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
		int num2;
		if (GameManager.IsGameTypeValidForGGPack(gameType) && num < 3)
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
			num2 = ((!ReplayPlayManager.Get().IsPlayback()) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag = (byte)num2 != 0;
		m_hasGgPacks = (ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack) > 0);
		m_useGGPackBtn.SetClickable(m_hasGgPacks);
		m_useGGPackBtn.SetForceExitCallback(!m_hasGgPacks);
		m_useGGPackBtn.SetForceHovercallback(!m_hasGgPacks);
		UIManager.SetGameObjectActive(m_ggPackBtnAnimator, flag);
		m_waitingForGGPackUseResponse = false;
		if (flag)
		{
			m_ggPackBtnAnimator.Play("HUDggPackIDLE");
		}
		for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == num);
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void NotifyReceivedGGPackResponse()
	{
		m_waitingForGGPackUseResponse = false;
		Setup();
	}

	public void GGPackAnimDone()
	{
		if (m_waitingForGGPackUseResponse)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Setup();
			return;
		}
	}

	public void GGPackMouseOver(BaseEventData data)
	{
		if (!m_hasGgPacks)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_ggPackBtnAnimator.isActiveAndEnabled)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				AnimatorClipInfo[] currentAnimatorClipInfo = m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
				if (currentAnimatorClipInfo == null || currentAnimatorClipInfo.Length <= 0)
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
					if (currentAnimatorClipInfo[0].clip.name != "HUDggPackPRESS")
					{
						m_ggPackBtnAnimator.Play("HUDggPackHOVER");
					}
					return;
				}
			}
		}
	}

	public void GGPackMouseExit(BaseEventData data)
	{
		if (!m_hasGgPacks)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_ggPackBtnAnimator.isActiveAndEnabled)
			{
				return;
			}
			AnimatorClipInfo[] currentAnimatorClipInfo = m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo == null)
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
				if (currentAnimatorClipInfo.Length <= 0)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (currentAnimatorClipInfo[0].clip.name != "HUDggPackPRESS")
					{
						m_ggPackBtnAnimator.Play("HUDggPackIDLE");
					}
					return;
				}
			}
		}
	}

	public void UseGGPackBtnClicked(BaseEventData data)
	{
		if (!(Time.unscaledTime > m_ggPackTimeLastUsed + GameBalanceVars.Get().GGPackInGameCooldownTimer))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_ggPackTimeLastUsed = Time.unscaledTime;
			m_useGGPackBtn.SetClickable(false);
			m_ggPackBtnAnimator.Play("HUDggPackPRESS");
			int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
			if (num == 0)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.GGButtonInGameUsed);
			}
			else if (num == 1)
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
				AudioManager.PostEvent("ui/endgame/ggboost_button_silver");
			}
			else if (num == 2)
			{
				AudioManager.PostEvent("ui/endgame/ggboost_button_gold");
			}
			if (m_hasGgPacks)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					ClientGameManager.Get().RequestToUseGGPack();
					m_waitingForGGPackUseResponse = true;
					return;
				}
			}
			return;
		}
	}

	private void Start()
	{
		if (m_tauntButton != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIEventTriggerUtils.AddListener(m_tauntButton.gameObject, EventTriggerType.PointerClick, OnTauntClick);
		}
		if (m_tauntSelectionPanel.m_closeSelectionButton != null)
		{
			UIEventTriggerUtils.AddListener(m_tauntSelectionPanel.m_closeSelectionButton.gameObject, EventTriggerType.PointerClick, OnCloseSelectionClick);
		}
		m_tauntIsEnabled = true;
		ShowTaunt(false);
	}

	private void UpdatePendingHealthBar()
	{
		if (m_lastPendingHealthPercent == m_pendingHPPercent)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_healthPercent > 0f)
		{
			m_pendingHPImage.fillAmount = m_pendingHPPercent;
			UIManager.SetGameObjectActive(m_pendingHPImage, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_pendingHPImage, false);
		}
		m_lastPendingHealthPercent = m_pendingHPPercent;
	}

	private void UpdateHealthBar()
	{
		if (m_lastHealthPercent == m_healthPercent)
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
					return;
				}
			}
		}
		if (m_healthPercent > 0f)
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
			m_healthImage.fillAmount = m_healthPercent;
			UIManager.SetGameObjectActive(m_healthImage, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_healthImage, false);
		}
		m_lastHealthPercent = m_healthPercent;
	}

	private void UpdateShieldBar()
	{
		if (m_lastShieldPercent == m_shieldPercent)
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
					return;
				}
			}
		}
		if (m_shieldPercent > 0f)
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
			m_shieldBarImage.fillAmount = m_shieldPercent;
			UIManager.SetGameObjectActive(m_shieldBarImage, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_shieldBarImage, false);
		}
		m_lastShieldPercent = m_shieldPercent;
	}

	private void UpdateEnergyBar()
	{
		if (m_lastEnergyPercent == m_energyPercent)
		{
			return;
		}
		if (m_energyPercent > 0f)
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
			m_energyImage.fillAmount = m_energyPercent;
			UIManager.SetGameObjectActive(m_energyImage, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_energyImage, false);
		}
		m_lastEnergyPercent = m_energyPercent;
	}

	private bool CanTaunt(ActorData actor)
	{
		bool flag = false;
		if (GameManager.Get().GameplayOverrides.AreTauntsEnabled())
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				TurnStateEnum currentState = activeOwnedActorData.GetActorTurnSM().CurrentState;
				if (currentState != 0 && currentState != TurnStateEnum.CONFIRMED)
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
					if (currentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
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
						if (currentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
						{
							goto IL_0151;
						}
						while (true)
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
				AbilityData abilityData = activeOwnedActorData.GetAbilityData();
				List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
				int num = 0;
				while (!flag)
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
					if (num < autoQueuedRequestActionTypes.Count)
					{
						flag = CanTauntForAction(actor, abilityData, component, autoQueuedRequestActionTypes[num]);
						num++;
						continue;
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
					break;
				}
				if (!flag)
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
					List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
					int num2 = 0;
					while (!flag)
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
						if (num2 < requestStackForUndo.Count)
						{
							flag = CanTauntForAction(actor, abilityData, component, requestStackForUndo[num2].m_action);
							num2++;
							continue;
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
						break;
					}
				}
			}
		}
		goto IL_0151;
		IL_0151:
		return flag;
	}

	public static bool CanTauntForAction(ActorData actor, AbilityData abilityData, ActorCinematicRequests cinematicRequests, AbilityData.ActionType actionType)
	{
		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
		CharacterResourceLink characterResourceLink = GameFlowData.Get().activeOwnedActorData.GetCharacterResourceLink();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(actor.m_characterType);
		if (abilityOfActionType != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (playerCharacterData != null)
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
				if (!cinematicRequests.IsAbilityCinematicRequested(actionType))
				{
					TauntCameraSet tauntCamSetData = abilityData.GetComponent<ActorData>().m_tauntCamSetData;
					if (tauntCamSetData != null)
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
						for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
						{
							CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
							if (!(cameraShotSequence != null))
							{
								continue;
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
							if (abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence.m_animIndex) && cinematicRequests.NumRequestsLeft(cameraShotSequence.m_uniqueTauntID) > 0)
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
								if (AbilityData.CanTauntForActionTypeForPlayer(playerCharacterData, characterResourceLink, actionType, true, cameraShotSequence.m_uniqueTauntID))
								{
									return true;
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
		if (!m_tauntIsEnabled)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_selectionMenuOpen = visible;
		UIManager.SetGameObjectActive(m_tauntSelectionPanel, m_selectionMenuOpen);
		int num = m_tauntSelectionPanel.SetupTauntList();
		UIManager.SetGameObjectActive(m_tauntTransform, !m_selectionMenuOpen);
		if (visible && num == 1)
		{
			m_tauntSelectionPanel.m_tauntButtons[0].SelectedTaunt(null);
		}
	}

	public void OnTauntClick(BaseEventData data)
	{
		ShowTauntSelection(true);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntClick);
	}

	public void OnCloseSelectionClick(BaseEventData data)
	{
		ShowTauntSelection(false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntSelect);
	}

	public void UpdateStatusDisplay(bool forceUpdate)
	{
		if (m_buffGrid == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_debuffGrid == null)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			ActorStatus actorStatus = GameFlowData.Get().activeOwnedActorData.GetActorStatus();
			List<StatusType> list = new List<StatusType>();
			bool flag = false;
			if (actorStatus != null)
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
				for (int i = 0; i < 58; i++)
				{
					StatusType statusType = (StatusType)i;
					if (!actorStatus.HasStatus(statusType, false))
					{
						continue;
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
					HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(statusType);
					if (!iconForStatusType.displayIcon)
					{
						continue;
					}
					list.Add(statusType);
					if (previousStatuses != null)
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
						if (!previousStatuses.Contains(statusType))
						{
							flag = true;
						}
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (previousStatuses != null)
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
				if (previousStatuses.Count != list.Count)
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
					flag = true;
				}
			}
			if (!flag)
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
				if (!forceUpdate)
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
			}
			UIBuffIndicator[] componentsInChildren = m_buffGrid.GetComponentsInChildren<UIBuffIndicator>(false);
			UIBuffIndicator[] componentsInChildren2 = m_debuffGrid.GetComponentsInChildren<UIBuffIndicator>(false);
			List<UIBuffIndicator> list2 = new List<UIBuffIndicator>(componentsInChildren);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				list2.Add(componentsInChildren2[j]);
			}
			while (true)
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
				UIManager.SetGameObjectActive(list2[0], false);
				Object.Destroy(list2[0].gameObject);
				list2.RemoveAt(0);
			}
			while (true)
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
				UIBuffIndicator item = Object.Instantiate(m_buffIndicatorPrefab);
				list2.Add(item);
			}
			for (int k = 0; k < list.Count; k++)
			{
				UIBuffIndicator uIBuffIndicator = list2[k];
				HUD_UIResources.StatusTypeIcon iconForStatusType2 = HUD_UIResources.GetIconForStatusType(list[k]);
				if (iconForStatusType2.isDebuff)
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
					uIBuffIndicator.transform.SetParent(m_debuffGrid.transform);
				}
				else
				{
					uIBuffIndicator.transform.SetParent(m_buffGrid.transform);
				}
				uIBuffIndicator.transform.localScale = Vector3.one;
				uIBuffIndicator.transform.localPosition = Vector3.zero;
				uIBuffIndicator.transform.localEulerAngles = Vector3.zero;
				list2[k].Setup(list[k], actorStatus.GetDurationOfStatus(list[k]));
			}
			m_debuffGrid.enabled = false;
			m_debuffGrid.enabled = true;
			m_buffGrid.enabled = false;
			m_buffGrid.enabled = true;
			previousStatuses = list;
			return;
		}
	}

	private void ShowTaunt(bool visible)
	{
		if (m_tauntIsEnabled == visible)
		{
			return;
		}
		m_tauntIsEnabled = visible;
		if (!visible)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_selectionMenuOpen = false;
			UIManager.SetGameObjectActive(m_tauntSelectionPanel, m_selectionMenuOpen);
		}
		UIManager.SetGameObjectActive(m_tauntTransform, visible);
	}

	private void Update()
	{
		if (GameFlowData.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				bool doActive = false;
				bool doActive2 = false;
				if (m_visualObject != null)
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
					if (!m_visualObject.activeSelf)
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
						UIManager.SetGameObjectActive(m_visualObject, true);
					}
				}
				m_aliveProfileImage.sprite = activeOwnedActorData.GetAliveHUDIcon();
				m_deadProfileImage.sprite = activeOwnedActorData.GetDeadHUDIcon();
				if (activeOwnedActorData.IsDead())
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
					doActive2 = true;
				}
				else
				{
					doActive = true;
				}
				int hitPointsAfterResolution = activeOwnedActorData.GetHitPointsAfterResolution();
				int maxHitPoints = activeOwnedActorData.GetMaxHitPoints();
				int energyToDisplay = activeOwnedActorData.GetEnergyToDisplay();
				int actualMaxTechPoints = activeOwnedActorData.GetActualMaxTechPoints();
				int num = activeOwnedActorData._0004();
				int clientUnappliedHoTTotal_ToDisplay_zq = activeOwnedActorData.GetClientUnappliedHoTTotal_ToDisplay_zq();
				if (clientUnappliedHoTTotal_ToDisplay_zq > 0)
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
					m_pendingHealthText.text = "+" + clientUnappliedHoTTotal_ToDisplay_zq;
				}
				else
				{
					m_pendingHealthText.text = string.Empty;
				}
				if (num > 0)
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
					m_shieldText.text = "+" + num;
				}
				else
				{
					m_shieldText.text = string.Empty;
				}
				m_healthText.text = hitPointsAfterResolution.ToString();
				m_energyText.text = energyToDisplay.ToString();
				m_healthPercent = (float)hitPointsAfterResolution / (float)(maxHitPoints + num);
				m_shieldPercent = (float)(hitPointsAfterResolution + num) / (float)(maxHitPoints + num);
				m_pendingHPPercent = (float)(hitPointsAfterResolution + num + clientUnappliedHoTTotal_ToDisplay_zq) / (float)(maxHitPoints + num);
				m_energyPercent = (float)energyToDisplay / (float)actualMaxTechPoints;
				if (CanTaunt(activeOwnedActorData))
				{
					ShowTaunt(true);
				}
				else
				{
					ShowTaunt(false);
				}
				UIManager.SetGameObjectActive(m_aliveProfileImage, doActive);
				UIManager.SetGameObjectActive(m_deadProfileImage, doActive2);
			}
			else if (m_visualObject != null && m_visualObject.activeSelf)
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
				UIManager.SetGameObjectActive(m_visualObject, false);
			}
		}
		UpdateEnergyBar();
		UpdateHealthBar();
		UpdateShieldBar();
		UpdatePendingHealthBar();
		float num2 = 1f;
		if (m_ggPackTimeLastUsed > 0f)
		{
			float num3 = Time.unscaledTime - m_ggPackTimeLastUsed;
			num2 = Mathf.Clamp01(num3 / GameBalanceVars.Get().GGPackInGameCooldownTimer);
		}
		m_ggPackCooldown.fillAmount = 1f - num2;
	}

	private void OnEnable()
	{
		m_lastEnergyPercent = 1f;
		Update();
	}
}
