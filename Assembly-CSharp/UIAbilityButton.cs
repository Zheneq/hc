using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAbilityButton : MonoBehaviour
{
	public Image[] m_phaseIndicators;

	public RectTransform m_ultimateBorder;

	public RectTransform m_persistentAuraBorder;

	public Mask m_ToggleAbility;

	public Animator m_AbilityButtonAnimator;

	public Image m_borderGlow;

	public GameObject m_tutorialTip;

	public RectTransform m_chargeCounterContainer;

	public UIAbilityChargeCounter[] chargeCounters;

	public Image m_background;

	public Image m_default;

	public Image m_selected;

	public Image m_pressedIdle;

	public Image m_pressedIdle2;

	public Image m_hover;

	public Image m_pressed;

	public Image m_selectedGlow;

	public Image m_disabled;

	public Image m_charIcon;

	public Image m_freeactionIcon;

	public TextMeshProUGUI m_hotkeyLabel;

	public TextMeshProUGUI m_cooldownLabel;

	public Button m_hitbox;

	private bool m_mouseDown;

	private bool m_mouseOver;

	private bool m_mouseOverForRangePreview;

	private AbilityData.AbilityEntry m_abilityEntry;

	private AbilityData.ActionType m_actionType;

	private AbilityData m_abilityData;

	private ActorData m_actorData;

	private ActorTurnSM m_turnSM;

	public void Awake()
	{
		UIManager.SetGameObjectActive(m_borderGlow, false);
		UIManager.SetGameObjectActive(m_ToggleAbility, false);
		UIManager.SetGameObjectActive(m_tutorialTip, false);
		m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, ShowTooltip);
	}

	public void Start()
	{
		if (m_hitbox != null)
		{
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerClick, OnAbilityButtonClick);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerDown, OnAbilityButtonDown);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerUp, OnAbilityButtonUp);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerExit, OnPointerExit);
			m_hitbox.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.4f;
		}
		UIManager.SetGameObjectActive(m_background, true);
		if (m_ultimateBorder != null)
		{
			UIManager.SetGameObjectActive(m_ultimateBorder, false);
		}
		if (!(m_persistentAuraBorder != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_persistentAuraBorder, false);
			return;
		}
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData)
	{
		bool doActive = false;
		m_abilityEntry = abilityEntry;
		m_actionType = actionType;
		m_abilityData = abilityData;
		object actorData;
		if (m_abilityData != null)
		{
			actorData = m_abilityData.GetComponent<ActorData>();
		}
		else
		{
			actorData = null;
		}
		m_actorData = (ActorData)actorData;
		m_turnSM = m_abilityData.GetComponent<ActorTurnSM>();
		if (m_abilityEntry != null)
		{
			if (m_abilityEntry.ability != null)
			{
				UIManager.SetGameObjectActive(m_charIcon, true);
				m_charIcon.sprite = m_abilityEntry.ability.sprite;
				m_hotkeyLabel.text = m_abilityEntry.hotkey;
				doActive = m_abilityEntry.ability.IsFreeAction();
				int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilityEntry.ability.RunPriority));
				for (int i = 0; i < m_phaseIndicators.Length; i++)
				{
					Image component = m_phaseIndicators[i];
					int doActive2;
					if (i == num)
					{
						doActive2 = ((num != -1) ? 1 : 0);
					}
					else
					{
						doActive2 = 0;
					}
					UIManager.SetGameObjectActive(component, (byte)doActive2 != 0);
				}
				goto IL_01b5;
			}
		}
		if (m_abilityEntry != null)
		{
			m_cooldownLabel.text = string.Empty;
			UIManager.SetGameObjectActive(m_charIcon, false);
			m_hitbox.enabled = false;
			m_hotkeyLabel.text = m_abilityEntry.hotkey;
		}
		else
		{
			m_cooldownLabel.text = string.Empty;
			UIManager.SetGameObjectActive(m_charIcon, false);
			m_hitbox.enabled = false;
		}
		goto IL_01b5;
		IL_01b5:
		UIManager.SetGameObjectActive(m_freeactionIcon, doActive);
		m_hotkeyLabel.color = Color.white;
	}

	public void RefreshHotkey()
	{
		if (m_abilityEntry != null)
		{
			m_abilityEntry.InitHotkey();
			m_hotkeyLabel.text = m_abilityEntry.hotkey;
		}
	}

	public KeyPreference GetKeyPreference()
	{
		if (m_abilityEntry != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_abilityEntry.keyPreference;
				}
			}
		}
		return KeyPreference.NullPreference;
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
		if (m_abilityEntry != null)
		{
			if (m_abilityEntry.ability != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						uIAbilityTooltip.Setup(m_abilityEntry.ability, m_abilityEntry.ability.CurrentAbilityMod);
						return true;
					}
				}
			}
		}
		return false;
	}

	public void UnqueueAnimationDone()
	{
		UIManager.SetGameObjectActive(m_selected, false);
		UIManager.SetGameObjectActive(m_selectedGlow, false);
	}

	public void HoverOutAnimationDone()
	{
	}

	public void HoverInAnimationDone()
	{
	}

	public void OutlineFadeOutAnimationDone()
	{
	}

	public void OutlineFadeInAnimationDone()
	{
	}

	private void PlayAnimation(string animName)
	{
		m_AbilityButtonAnimator.Play(animName);
	}

	private void PlayAnimation(string animName, int layer)
	{
		m_AbilityButtonAnimator.Play(animName, layer);
	}

	public void OnPointerExit(BaseEventData data)
	{
		m_mouseOver = false;
		m_mouseOverForRangePreview = false;
		m_mouseDown = false;
	}

	public void OnPointerEnter(BaseEventData data)
	{
		if (!m_disabled.gameObject.activeSelf)
		{
			m_mouseOver = true;
		}
		m_mouseOverForRangePreview = true;
	}

	public void Update()
	{
		if (m_abilityEntry != null && m_abilityEntry.ability != null)
		{
			while (true)
			{
				ActorTurnSM turnSM;
				ActorData actorData;
				bool flag;
				bool flag2;
				bool flag3;
				int num2;
				bool doActive;
				bool doActive2;
				int num3;
				switch (1)
				{
				case 0:
					break;
				default:
					{
						turnSM = m_turnSM;
						actorData = m_actorData;
						flag = m_abilityData.HasQueuedAction(m_actionType);
						int num;
						if (m_abilityEntry.ability.GetModdedCost() > actorData.TechPoints)
						{
							num = (flag ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						flag2 = ((byte)num != 0);
						flag3 = (m_abilityEntry.ability == m_abilityData.GetSelectedAbility());
						if (m_abilityEntry.ability.GetModdedCost() > 0)
						{
							if (m_abilityEntry.ability.GetModdedCost() <= actorData.TechPoints)
							{
								num2 = ((m_abilityData.GetCooldownRemaining(m_actionType) <= 0) ? 1 : 0);
								goto IL_0103;
							}
						}
						num2 = 0;
						goto IL_0103;
					}
					IL_0103:
					doActive = ((byte)num2 != 0);
					doActive2 = m_abilityEntry.ability.ShouldShowPersistentAuraUI();
					if (m_ultimateBorder != null)
					{
						UIManager.SetGameObjectActive(m_ultimateBorder, doActive);
					}
					if (m_persistentAuraBorder != null)
					{
						UIManager.SetGameObjectActive(m_persistentAuraBorder, doActive2);
					}
					num3 = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilityEntry.ability.RunPriority));
					for (int i = 0; i < m_phaseIndicators.Length; i++)
					{
						UIManager.SetGameObjectActive(m_phaseIndicators[i], i == num3 && num3 != -1);
					}
					while (true)
					{
						string text;
						bool flag4;
						bool flag5;
						bool flag6;
						bool flag7;
						bool flag8;
						int num4;
						bool flag9;
						int num5;
						bool flag10;
						bool flag11;
						switch (2)
						{
						case 0:
							break;
						default:
							{
								text = string.Empty;
								if (!m_mouseOver)
								{
									m_mouseDown = false;
								}
								flag4 = (flag3 || m_mouseDown);
								flag5 = true;
								if (m_mouseOver)
								{
									if (!flag4)
									{
										if (!flag)
										{
											if (!m_hover.gameObject.activeSelf)
											{
												PlayAnimation("AbilityButtonHover");
											}
											flag5 = false;
											UIManager.SetGameObjectActive(m_hover, true);
											goto IL_0267;
										}
									}
								}
								UIManager.SetGameObjectActive(m_hover, false);
								goto IL_0267;
							}
							IL_08b8:
							if (m_cooldownLabel.text != text)
							{
								m_cooldownLabel.text = text;
							}
							flag6 = m_abilityEntry.ability.IsFreeAction();
							if (m_freeactionIcon != null)
							{
								if (m_freeactionIcon.gameObject.activeSelf != flag6)
								{
									UIManager.SetGameObjectActive(m_freeactionIcon, flag6);
								}
							}
							HighlightUtils.Get().SetRangeIndicatorMouseOverFlag((int)m_actionType, m_mouseOverForRangePreview);
							return;
							IL_0267:
							if (flag4 && !flag)
							{
								if (!m_pressed.gameObject.activeSelf)
								{
									PlayAnimation("AbilityButtonSelected");
									if (m_mouseOver)
									{
										PlayAnimation("AbilityButtonSelectedNonLoop", 1);
									}
								}
								flag5 = false;
								UIManager.SetGameObjectActive(m_pressedIdle, true);
								UIManager.SetGameObjectActive(m_pressedIdle2, true);
								UIManager.SetGameObjectActive(m_pressed, true);
							}
							else
							{
								UIManager.SetGameObjectActive(m_pressedIdle, false);
								UIManager.SetGameObjectActive(m_pressedIdle2, false);
								UIManager.SetGameObjectActive(m_pressed, false);
							}
							flag7 = false;
							if (flag)
							{
								if (!m_selected.gameObject.activeSelf)
								{
									PlayAnimation("AbilityButtonBarQueued");
								}
								flag5 = false;
								UIManager.SetGameObjectActive(m_selected, true);
								UIManager.SetGameObjectActive(m_selectedGlow, true);
							}
							else
							{
								PlayAnimation("AbilityButtonBarQueuedExit");
							}
							if (flag5)
							{
								if (!flag7)
								{
									if (!m_AbilityButtonAnimator.GetCurrentAnimatorStateInfo(0).IsName("AbilityButtonBarQueuedExit"))
									{
									}
								}
							}
							if (m_abilityEntry.ability.IsSimpleAction())
							{
								flag8 = turnSM.CanQueueSimpleAction();
							}
							else
							{
								flag8 = turnSM.CanSelectAbility();
							}
							if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
							{
								num4 = (m_abilityData.ValidateActionIsRequestable(m_actionType) ? 1 : 0);
							}
							else
							{
								num4 = 0;
							}
							flag9 = ((byte)num4 != 0);
							if (turnSM.CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST)
							{
								if (!flag9)
								{
									num5 = 1;
									goto IL_048e;
								}
							}
							if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
							{
								if (turnSM.CurrentState != TurnStateEnum.CONFIRMED)
								{
									if (turnSM.CurrentState != TurnStateEnum.WAITING)
									{
										num5 = ((turnSM.CurrentState == TurnStateEnum.RESOLVING && GameFlowData.Get().gameState == GameState.BothTeams_Decision) ? 1 : 0);
										goto IL_048e;
									}
								}
							}
							num5 = 1;
							goto IL_048e;
							IL_048e:
							flag10 = ((byte)num5 != 0);
							if (flag8)
							{
								int num6;
								if (!flag10)
								{
									if (!flag9)
									{
										num6 = ((!flag) ? 1 : 0);
									}
									else
									{
										num6 = 0;
									}
								}
								else
								{
									num6 = 1;
								}
								flag10 = ((byte)num6 != 0);
							}
							if (flag)
							{
								flag10 = false;
							}
							else
							{
								flag10 = (flag10 || !flag9);
							}
							if (flag10)
							{
								m_charIcon.color = HUD_UIResources.Get().m_disabledAbilityButtonIconColor;
								m_hotkeyLabel.color = HUD_UIResources.Get().m_disabledAbilityButtonIconColor;
								UIManager.SetGameObjectActive(m_disabled, true);
								UIManager.SetGameObjectActive(m_default, false);
							}
							else
							{
								if (m_abilityEntry.ability.UseCustomAbilityIconColor())
								{
									m_charIcon.color = m_abilityEntry.ability.GetCustomAbilityIconColor(actorData);
								}
								else
								{
									m_charIcon.color = Color.white;
								}
								m_hotkeyLabel.color = Color.white;
								UIManager.SetGameObjectActive(m_disabled, false);
								UIManager.SetGameObjectActive(m_default, flag5);
							}
							flag11 = true;
							if (!actorData.IsDead())
							{
								flag11 = m_abilityEntry.ability.CustomCanCastValidation(actorData);
							}
							if (m_abilityEntry.ability.GetModdedMaxStocks() <= 0)
							{
								UIManager.SetGameObjectActive(m_chargeCounterContainer, false);
								if (m_abilityEntry.GetCooldownRemaining() == 0 && flag2)
								{
									if (flag11)
									{
										goto IL_08b8;
									}
								}
								if (m_abilityEntry.GetCooldownRemaining() > 0)
								{
									if (actorData.TechPoints + actorData.ReservedTechPoints < actorData.GetActualMaxTechPoints() || !AbilityUtils.AbilityHasTag(m_abilityEntry.ability, AbilityTags.IgnoreCooldownIfFullEnergy))
									{
										text = m_abilityEntry.GetCooldownRemaining().ToString();
									}
								}
								else if (m_abilityEntry.GetCooldownRemaining() == -1)
								{
									text = "~";
								}
								else if (!flag11)
								{
									text = "X";
								}
							}
							else
							{
								UIManager.SetGameObjectActive(m_chargeCounterContainer, true);
								int moddedMaxStocks = m_abilityEntry.ability.GetModdedMaxStocks();
								for (int j = 0; j < chargeCounters.Length; j++)
								{
									if (j < moddedMaxStocks)
									{
										UIManager.SetGameObjectActive(chargeCounters[j], true);
									}
									else
									{
										UIManager.SetGameObjectActive(chargeCounters[j], false);
									}
								}
								AbilityData.ActionType actionTypeOfAbility = m_abilityData.GetActionTypeOfAbility(m_abilityEntry.ability);
								if (actionTypeOfAbility != AbilityData.ActionType.INVALID_ACTION)
								{
									int consumedStocksCount = m_abilityData.GetConsumedStocksCount(actionTypeOfAbility);
									int stockRefreshCountdown = m_abilityData.GetStockRefreshCountdown(actionTypeOfAbility);
									int moddedStockRefreshDuration = m_abilityEntry.ability.GetModdedStockRefreshDuration();
									int num7 = moddedMaxStocks - consumedStocksCount;
									int k;
									for (k = 0; k < num7; k++)
									{
										chargeCounters[k].SetTick(3);
									}
									if (k < chargeCounters.Length)
									{
										UIAbilityChargeCounter obj = chargeCounters[k];
										int tick;
										if (moddedStockRefreshDuration >= 0)
										{
											tick = 3 - stockRefreshCountdown;
										}
										else
										{
											tick = 0;
										}
										obj.SetTick(tick);
										k++;
									}
									for (; k < chargeCounters.Length; k++)
									{
										chargeCounters[k].SetTick(0);
									}
									if (!m_abilityData.IsAbilityAllowedByUnlockTurns(actionTypeOfAbility))
									{
										text = m_abilityData.GetTurnsTillUnlock(actionTypeOfAbility).ToString();
									}
									else if (stockRefreshCountdown > 0)
									{
										if (num7 != 0)
										{
											if (!m_abilityEntry.ability.RefillAllStockOnRefresh())
											{
												goto IL_08b8;
											}
										}
										text = stockRefreshCountdown.ToString();
									}
								}
							}
							goto IL_08b8;
						}
					}
				}
			}
		}
		if (!m_ultimateBorder)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_ultimateBorder, false);
			return;
		}
	}

	private void OnEnable()
	{
		Update();
	}

	public void OnAbilityButtonDown(BaseEventData data)
	{
		if (m_disabled.gameObject.activeSelf)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_mouseDown = true;
	}

	public void OnAbilityButtonUp(BaseEventData data)
	{
		m_mouseDown = false;
	}

	public void OnAbilityButtonClick(BaseEventData data)
	{
		if (m_abilityData == null)
		{
			return;
		}
		while (true)
		{
			if (m_turnSM == null)
			{
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
			m_abilityData.AbilityButtonPressed(m_actionType, m_abilityEntry.ability);
			return;
		}
	}
}
