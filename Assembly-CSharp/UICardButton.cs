using System;
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
		if (this.m_hitbox != null)
		{
			this.m_tooltipHoverObject = this.m_hitbox.GetComponent<UITooltipHoverObject>();
			this.m_tooltipHoverObject.Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowTooltip), null);
		}
	}

	private void PlayAnimation(string animName)
	{
		this.m_cardButtonAnimator.Play(animName);
	}

	private void PlayAnimation(string animName, int layer)
	{
		this.m_cardButtonAnimator.Play(animName, layer);
	}

	public void Start()
	{
		this.m_cardIndex = -1;
		if (this.m_status != null)
		{
			UIManager.SetGameObjectActive(this.m_status, false, null);
		}
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnCardButtonClick));
		if (this.m_hitbox != null)
		{
			UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnCardButtonDown));
			UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.OnCardButtonUp));
			UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
			UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
		}
	}

	public void OnPointerExit(BaseEventData data)
	{
		this.m_mouseOver = false;
		this.m_mouseDown = false;
	}

	public void OnPointerEnter(BaseEventData data)
	{
		this.m_mouseOver = true;
	}

	public bool ShowTooltip(UITooltipBase tooltip)
	{
		if (this.m_abilityEntry.ability == null)
		{
			return false;
		}
		UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
		uiabilityTooltip.Setup(this.m_abilityEntry.ability);
		return true;
	}

	public void OnCardButtonDown(BaseEventData data)
	{
		this.m_mouseDown = true;
	}

	public void OnCardButtonUp(BaseEventData data)
	{
		this.m_mouseDown = false;
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData, int cardIndex)
	{
		this.m_abilityEntry = abilityEntry;
		this.m_actionType = actionType;
		this.m_abilityData = abilityData;
		if (this.m_tooltipHoverObject != null)
		{
			this.m_tooltipHoverObject.Refresh();
		}
		ActorTurnSM turnSM;
		if (this.m_abilityData != null)
		{
			turnSM = this.m_abilityData.GetComponent<ActorTurnSM>();
		}
		else
		{
			turnSM = null;
		}
		this.m_turnSM = turnSM;
		if (abilityEntry != null)
		{
			if (!(abilityEntry.ability == null))
			{
				if (this.m_abilityEntry != null)
				{
					if (this.m_abilityEntry.ability != null)
					{
						UIManager.SetGameObjectActive(this.m_abilityIcon, true, null);
						this.m_abilityIcon.sprite = this.m_abilityEntry.ability.sprite;
						this.m_hotkeyLabel.text = this.m_abilityEntry.hotkey;
						goto IL_212;
					}
				}
				if (this.m_abilityEntry != null)
				{
					UIManager.SetGameObjectActive(this.m_status, false, null);
					UIManager.SetGameObjectActive(this.m_abilityIcon, false, null);
					this.m_hitbox.enabled = false;
					this.m_hotkeyLabel.text = this.m_abilityEntry.hotkey;
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_status, false, null);
					UIManager.SetGameObjectActive(this.m_abilityIcon, false, null);
					this.m_hitbox.enabled = false;
				}
				IL_212:
				int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.m_abilityEntry.ability.RunPriority));
				for (int i = 0; i < this.m_phaseMarkers.Length; i++)
				{
					Component component = this.m_phaseMarkers[i];
					bool doActive;
					if (i == num)
					{
						doActive = (num != -1);
					}
					else
					{
						doActive = false;
					}
					UIManager.SetGameObjectActive(component, doActive, null);
				}
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_abilityIcon, false, null);
		UIManager.SetGameObjectActive(this.m_freeactionIcon, false, null);
		UIManager.SetGameObjectActive(this.m_discardButton, false, null);
		UIManager.SetGameObjectActive(this.m_selected, false, null);
		UIManager.SetGameObjectActive(this.m_selectedGlow, false, null);
		UIManager.SetGameObjectActive(this.m_pressedIdle, false, null);
		UIManager.SetGameObjectActive(this.m_pressedIdle2, false, null);
		UIManager.SetGameObjectActive(this.m_pressed, false, null);
		UIManager.SetGameObjectActive(this.m_disabled, true, null);
		this.m_cardIndex = cardIndex;
		for (int j = 0; j < this.m_phaseMarkers.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_phaseMarkers[j], false, null);
		}
	}

	public KeyPreference GetKeyPreference()
	{
		if (this.m_abilityEntry != null)
		{
			return this.m_abilityEntry.keyPreference;
		}
		return KeyPreference.NullPreference;
	}

	public void Update()
	{
		if (this.m_abilityEntry != null)
		{
			if (this.m_abilityEntry.ability != null)
			{
				if (this.m_turnSM != null)
				{
					ActorTurnSM turnSM = this.m_turnSM;
					bool flag = this.m_abilityData.HasQueuedAction(this.m_actionType);
					bool flag2 = this.m_abilityEntry.ability == this.m_abilityData.GetSelectedAbility();
					bool flag3;
					if (this.m_abilityEntry.ability.IsSimpleAction())
					{
						flag3 = turnSM.CanQueueSimpleAction();
					}
					else
					{
						flag3 = turnSM.CanSelectAbility();
					}
					bool flag4 = GameFlowData.Get().gameState == GameState.BothTeams_Decision && this.m_abilityData.ValidateActionIsRequestable(this.m_actionType);
					bool flag5;
					if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve && turnSM.CurrentState != TurnStateEnum.CONFIRMED)
					{
						if (turnSM.CurrentState != TurnStateEnum.WAITING)
						{
							if (turnSM.CurrentState == TurnStateEnum.RESOLVING)
							{
								flag5 = (GameFlowData.Get().gameState == GameState.BothTeams_Decision);
							}
							else
							{
								flag5 = false;
							}
							goto IL_154;
						}
					}
					flag5 = true;
					IL_154:
					bool flag6 = flag5;
					if (flag3)
					{
						bool flag7;
						if (!flag6)
						{
							if (!flag4)
							{
								flag7 = !flag;
							}
							else
							{
								flag7 = false;
							}
						}
						else
						{
							flag7 = true;
						}
						flag6 = flag7;
					}
					flag6 = (!flag && (flag6 || !flag4));
					UIManager.SetGameObjectActive(this.m_freeactionIcon, this.m_abilityEntry.ability.IsFreeAction(), null);
					if (!this.m_mouseOver)
					{
						this.m_mouseDown = false;
					}
					bool flag8;
					if (!flag2)
					{
						flag8 = this.m_mouseDown;
					}
					else
					{
						flag8 = true;
					}
					bool flag9 = flag8;
					bool flag10 = true;
					if (this.m_mouseOver)
					{
						if (!flag9)
						{
							if (!flag)
							{
								if (!flag6)
								{
									if (!this.m_hover.gameObject.activeSelf)
									{
										this.PlayAnimation("AbilityButtonHover");
									}
									flag10 = false;
									UIManager.SetGameObjectActive(this.m_hover, true, null);
									goto IL_26E;
								}
							}
						}
					}
					UIManager.SetGameObjectActive(this.m_hover, false, null);
					IL_26E:
					if (flag9)
					{
						if (!flag)
						{
							if (!flag6)
							{
								if (!this.m_pressed.gameObject.activeSelf)
								{
									this.PlayAnimation("AbilityButtonSelected");
									if (this.m_mouseOver)
									{
										this.PlayAnimation("AbilityButtonSelectedNonLoop", 1);
									}
								}
								flag10 = false;
								UIManager.SetGameObjectActive(this.m_pressedIdle, true, null);
								UIManager.SetGameObjectActive(this.m_pressedIdle2, true, null);
								UIManager.SetGameObjectActive(this.m_pressed, true, null);
								goto IL_33C;
							}
						}
					}
					UIManager.SetGameObjectActive(this.m_pressedIdle, false, null);
					UIManager.SetGameObjectActive(this.m_pressedIdle2, false, null);
					UIManager.SetGameObjectActive(this.m_pressed, false, null);
					IL_33C:
					if (flag)
					{
						if (!flag6)
						{
							if (!this.m_selected.gameObject.activeSelf)
							{
								this.PlayAnimation("AbilityButtonBarQueued");
							}
							flag10 = false;
							UIManager.SetGameObjectActive(this.m_selected, true, null);
							UIManager.SetGameObjectActive(this.m_selectedGlow, true, null);
							goto IL_3B1;
						}
					}
					UIManager.SetGameObjectActive(this.m_selected, false, null);
					UIManager.SetGameObjectActive(this.m_selectedGlow, false, null);
					IL_3B1:
					if (flag10)
					{
					}
					string text = string.Empty;
					if (flag6)
					{
						this.m_abilityIcon.color = HUD_UIResources.Get().m_disabledAbilityButtonIconColor;
						UIManager.SetGameObjectActive(this.m_disabled, true, null);
						UIManager.SetGameObjectActive(this.m_default, false, null);
						if (this.m_abilityData != null)
						{
							if (!this.m_abilityData.IsAbilityAllowedByUnlockTurns(this.m_actionType))
							{
								text = this.m_abilityData.GetTurnsTillUnlock(this.m_actionType).ToString();
								UIManager.SetGameObjectActive(this.m_status, true, null);
							}
						}
					}
					else
					{
						this.m_abilityIcon.color = Color.white;
						UIManager.SetGameObjectActive(this.m_disabled, false, null);
						UIManager.SetGameObjectActive(this.m_default, flag10, null);
					}
					if (this.m_status.text != text)
					{
						this.m_status.text = text;
					}
					HighlightUtils.Get().SetRangeIndicatorMouseOverFlag((int)this.m_actionType, this.m_mouseOver);
					return;
				}
			}
		}
		if (this.m_status.text != string.Empty)
		{
			this.m_status.text = string.Empty;
		}
	}

	public void OnDiscardButtonClick(BaseEventData data)
	{
	}

	public void OnCardButtonClick(BaseEventData data)
	{
		if (!(this.m_abilityData == null))
		{
			if (!(this.m_abilityData.GetAbilityOfActionType(this.m_actionType) == null))
			{
				this.m_abilityData.AbilityButtonPressed(this.m_actionType, this.m_abilityEntry.ability);
				return;
			}
		}
	}

	public int GetCardIndex()
	{
		return this.m_cardIndex;
	}
}
