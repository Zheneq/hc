using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICardButton : MonoBehaviour
{
	public RectTransform m_cardImageBG;

	public Button m_discardButton;

	public Image[] m_phaseMarkers;

	public TextMeshProUGUI m_status;

	public Button m_hitbox;

	public GameObject m_tutorialTip;

	public Image m_background;

	public Image m_default;

	public Image m_selected;

	public Image m_pressedIdle;

	public Image m_pressedIdle2;

	public Image m_hover;

	public Image m_pressed;

	public Image m_selectedGlow;

	public Image m_disabled;

	public Image m_abilityIcon;

	public Image m_freeactionIcon;

	public TextMeshProUGUI m_hotkeyLabel;

	public Animator m_cardButtonAnimator;

	private int m_cardIndex;

	private bool m_mouseDown;

	private bool m_mouseOver;

	private AbilityData.AbilityEntry m_abilityEntry;

	private AbilityData.ActionType m_actionType;

	private AbilityData m_abilityData;

	private ActorTurnSM m_turnSM;

	private UITooltipHoverObject m_tooltipHoverObject;

	private void Awake()
	{
		if (!(m_hitbox != null))
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
			m_tooltipHoverObject = m_hitbox.GetComponent<UITooltipHoverObject>();
			m_tooltipHoverObject.Setup(TooltipType.Ability, ShowTooltip);
			return;
		}
	}

	private void PlayAnimation(string animName)
	{
		m_cardButtonAnimator.Play(animName);
	}

	private void PlayAnimation(string animName, int layer)
	{
		m_cardButtonAnimator.Play(animName, layer);
	}

	public void Start()
	{
		m_cardIndex = -1;
		if (m_status != null)
		{
			UIManager.SetGameObjectActive(m_status, false);
		}
		UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerClick, OnCardButtonClick);
		if (!(m_hitbox != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerDown, OnCardButtonDown);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerUp, OnCardButtonUp);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
			UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerExit, OnPointerExit);
			return;
		}
	}

	public void OnPointerExit(BaseEventData data)
	{
		m_mouseOver = false;
		m_mouseDown = false;
	}

	public void OnPointerEnter(BaseEventData data)
	{
		m_mouseOver = true;
	}

	public bool ShowTooltip(UITooltipBase tooltip)
	{
		if (m_abilityEntry.ability == null)
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
		UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
		uIAbilityTooltip.Setup(m_abilityEntry.ability);
		return true;
	}

	public void OnCardButtonDown(BaseEventData data)
	{
		m_mouseDown = true;
	}

	public void OnCardButtonUp(BaseEventData data)
	{
		m_mouseDown = false;
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData, int cardIndex)
	{
		m_abilityEntry = abilityEntry;
		m_actionType = actionType;
		m_abilityData = abilityData;
		if (m_tooltipHoverObject != null)
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
			m_tooltipHoverObject.Refresh();
		}
		object turnSM;
		if (m_abilityData != null)
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
			turnSM = m_abilityData.GetComponent<ActorTurnSM>();
		}
		else
		{
			turnSM = null;
		}
		m_turnSM = (ActorTurnSM)turnSM;
		if (abilityEntry != null)
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
			if (!(abilityEntry.ability == null))
			{
				if (m_abilityEntry != null)
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
					if (m_abilityEntry.ability != null)
					{
						UIManager.SetGameObjectActive(m_abilityIcon, true);
						m_abilityIcon.sprite = m_abilityEntry.ability.sprite;
						m_hotkeyLabel.text = m_abilityEntry.hotkey;
						goto IL_0212;
					}
				}
				if (m_abilityEntry != null)
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
					UIManager.SetGameObjectActive(m_status, false);
					UIManager.SetGameObjectActive(m_abilityIcon, false);
					m_hitbox.enabled = false;
					m_hotkeyLabel.text = m_abilityEntry.hotkey;
				}
				else
				{
					UIManager.SetGameObjectActive(m_status, false);
					UIManager.SetGameObjectActive(m_abilityIcon, false);
					m_hitbox.enabled = false;
				}
				goto IL_0212;
			}
		}
		UIManager.SetGameObjectActive(m_abilityIcon, false);
		UIManager.SetGameObjectActive(m_freeactionIcon, false);
		UIManager.SetGameObjectActive(m_discardButton, false);
		UIManager.SetGameObjectActive(m_selected, false);
		UIManager.SetGameObjectActive(m_selectedGlow, false);
		UIManager.SetGameObjectActive(m_pressedIdle, false);
		UIManager.SetGameObjectActive(m_pressedIdle2, false);
		UIManager.SetGameObjectActive(m_pressed, false);
		UIManager.SetGameObjectActive(m_disabled, true);
		m_cardIndex = cardIndex;
		for (int i = 0; i < m_phaseMarkers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_phaseMarkers[i], false);
		}
		return;
		IL_0212:
		int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilityEntry.ability.RunPriority));
		for (int j = 0; j < m_phaseMarkers.Length; j++)
		{
			Image component = m_phaseMarkers[j];
			int doActive;
			if (j == num)
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
				doActive = ((num != -1) ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(component, (byte)doActive != 0);
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_abilityEntry.keyPreference;
				}
			}
		}
		return KeyPreference.NullPreference;
	}

	public void Update()
	{
		if (m_abilityEntry != null)
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
			if (m_abilityEntry.ability != null)
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
				if (m_turnSM != null)
				{
					while (true)
					{
						bool flag;
						bool flag2;
						bool flag3;
						bool flag4;
						int num;
						bool flag5;
						int num3;
						bool flag6;
						bool flag7;
						string text;
						switch (5)
						{
						case 0:
							break;
						default:
							{
								ActorTurnSM turnSM = m_turnSM;
								flag = m_abilityData.HasQueuedAction(m_actionType);
								flag2 = (m_abilityEntry.ability == m_abilityData.GetSelectedAbility());
								if (m_abilityEntry.ability.IsSimpleAction())
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
									flag3 = turnSM.CanQueueSimpleAction();
								}
								else
								{
									flag3 = turnSM.CanSelectAbility();
								}
								flag4 = (GameFlowData.Get().gameState == GameState.BothTeams_Decision && m_abilityData.ValidateActionIsRequestable(m_actionType));
								if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve && turnSM.CurrentState != TurnStateEnum.CONFIRMED)
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
									if (turnSM.CurrentState != TurnStateEnum.WAITING)
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
										if (turnSM.CurrentState == TurnStateEnum.RESOLVING)
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
											num = ((GameFlowData.Get().gameState == GameState.BothTeams_Decision) ? 1 : 0);
										}
										else
										{
											num = 0;
										}
										goto IL_0154;
									}
								}
								num = 1;
								goto IL_0154;
							}
							IL_0154:
							flag5 = ((byte)num != 0);
							if (flag3)
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
								int num2;
								if (!flag5)
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
									if (!flag4)
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
										num2 = ((!flag) ? 1 : 0);
									}
									else
									{
										num2 = 0;
									}
								}
								else
								{
									num2 = 1;
								}
								flag5 = ((byte)num2 != 0);
							}
							flag5 = (!flag && (flag5 || !flag4));
							UIManager.SetGameObjectActive(m_freeactionIcon, m_abilityEntry.ability.IsFreeAction());
							if (!m_mouseOver)
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
								m_mouseDown = false;
							}
							if (!flag2)
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
								num3 = (m_mouseDown ? 1 : 0);
							}
							else
							{
								num3 = 1;
							}
							flag6 = ((byte)num3 != 0);
							flag7 = true;
							if (m_mouseOver)
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
								if (!flag6)
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
									if (!flag)
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
										if (!flag5)
										{
											if (!m_hover.gameObject.activeSelf)
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
												PlayAnimation("AbilityButtonHover");
											}
											flag7 = false;
											UIManager.SetGameObjectActive(m_hover, true);
											goto IL_026e;
										}
									}
								}
							}
							UIManager.SetGameObjectActive(m_hover, false);
							goto IL_026e;
							IL_03b1:
							if (flag7)
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
							}
							text = string.Empty;
							if (flag5)
							{
								m_abilityIcon.color = HUD_UIResources.Get().m_disabledAbilityButtonIconColor;
								UIManager.SetGameObjectActive(m_disabled, true);
								UIManager.SetGameObjectActive(m_default, false);
								if (m_abilityData != null)
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
									if (!m_abilityData.IsAbilityAllowedByUnlockTurns(m_actionType))
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
										text = m_abilityData.GetTurnsTillUnlock(m_actionType).ToString();
										UIManager.SetGameObjectActive(m_status, true);
									}
								}
							}
							else
							{
								m_abilityIcon.color = Color.white;
								UIManager.SetGameObjectActive(m_disabled, false);
								UIManager.SetGameObjectActive(m_default, flag7);
							}
							if (m_status.text != text)
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
								m_status.text = text;
							}
							HighlightUtils.Get().SetRangeIndicatorMouseOverFlag((int)m_actionType, m_mouseOver);
							return;
							IL_033c:
							if (flag)
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
								if (!flag5)
								{
									if (!m_selected.gameObject.activeSelf)
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
										PlayAnimation("AbilityButtonBarQueued");
									}
									flag7 = false;
									UIManager.SetGameObjectActive(m_selected, true);
									UIManager.SetGameObjectActive(m_selectedGlow, true);
									goto IL_03b1;
								}
							}
							UIManager.SetGameObjectActive(m_selected, false);
							UIManager.SetGameObjectActive(m_selectedGlow, false);
							goto IL_03b1;
							IL_026e:
							if (flag6)
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
									if (!flag5)
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
										if (!m_pressed.gameObject.activeSelf)
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
											PlayAnimation("AbilityButtonSelected");
											if (m_mouseOver)
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
												PlayAnimation("AbilityButtonSelectedNonLoop", 1);
											}
										}
										flag7 = false;
										UIManager.SetGameObjectActive(m_pressedIdle, true);
										UIManager.SetGameObjectActive(m_pressedIdle2, true);
										UIManager.SetGameObjectActive(m_pressed, true);
										goto IL_033c;
									}
								}
							}
							UIManager.SetGameObjectActive(m_pressedIdle, false);
							UIManager.SetGameObjectActive(m_pressedIdle2, false);
							UIManager.SetGameObjectActive(m_pressed, false);
							goto IL_033c;
						}
					}
				}
			}
		}
		if (m_status.text != string.Empty)
		{
			m_status.text = string.Empty;
		}
	}

	public void OnDiscardButtonClick(BaseEventData data)
	{
	}

	public void OnCardButtonClick(BaseEventData data)
	{
		if (m_abilityData == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_abilityData.GetAbilityOfActionType(m_actionType) == null)
			{
				while (true)
				{
					switch (1)
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

	public int GetCardIndex()
	{
		return m_cardIndex;
	}
}
