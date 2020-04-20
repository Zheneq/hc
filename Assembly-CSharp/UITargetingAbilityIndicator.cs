using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITargetingAbilityIndicator : MonoBehaviour
{
	public Button m_mouseHitBox;

	public Image m_abilityIconImage;

	public TextMeshProUGUI m_cooldownLabel;

	public GameObject m_cooldownState;

	public GameObject m_defaultState;

	public GameObject m_hoverState;

	public GameObject m_queuedState;

	public GameObject m_targetingState;

	public Image m_phaseColorImage;

	public Image m_targetingPhaseColorImage;

	public Color m_prepPhaseColor = Color.green;

	public Color m_evasionPhaseColor = Color.yellow;

	public Color m_combatPhaseColor = Color.red;

	private Ability m_ability;

	private CanvasGroup m_canvasGroup;

	private int m_cachedCooldown;

	private int m_cachedStocksRemaining;

	private int m_cachedStockRefreshCountdown;

	private bool m_cachedAbilityUseAllowed;

	private bool m_cachedIsQueued;

	private bool m_cachedIsTargeting;

	private bool m_cachedEnergyOk;

	private void Awake()
	{
		this.m_canvasGroup = base.GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		if (this.m_mouseHitBox != null)
		{
			UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.MouseEnter));
			UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.MouseExit));
			this.m_mouseHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowTooltip), null);
		}
	}

	public void SetCanvasGroupVisibility(bool visible)
	{
		if (this.m_canvasGroup != null)
		{
			if (visible)
			{
				this.m_canvasGroup.alpha = 1f;
				this.m_canvasGroup.blocksRaycasts = true;
				this.m_canvasGroup.interactable = true;
			}
			else
			{
				this.m_canvasGroup.alpha = 0f;
				this.m_canvasGroup.blocksRaycasts = false;
				this.m_canvasGroup.interactable = false;
			}
		}
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		if (this.m_ability == null)
		{
			return false;
		}
		UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
		uiabilityTooltip.Setup(this.m_ability, this.m_ability.CurrentAbilityMod);
		return true;
	}

	public void MouseEnter(BaseEventData data)
	{
		if (this.m_hoverState != null)
		{
			UIManager.SetGameObjectActive(this.m_hoverState, true, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	public void MouseExit(BaseEventData data)
	{
		if (this.m_hoverState != null)
		{
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	public void OnDisable()
	{
		if (this.m_hoverState != null)
		{
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	public void Setup(ActorData actor, Ability ability, AbilityData.ActionType action)
	{
		if (actor != null)
		{
			if (actor.GetAbilityData() != null)
			{
				if (ability != null)
				{
					AbilityData abilityData = actor.GetAbilityData();
					bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.GetTeam());
					bool flag2;
					if (ClientGameManager.Get().PlayerInfo != null)
					{
						if (ClientGameManager.Get().PlayerInfo.IsSpectator)
						{
							flag2 = ClientGameManager.Get().SpectatorHideAbilityTargeter;
							goto IL_B1;
						}
					}
					flag2 = false;
					IL_B1:
					bool flag3 = flag2;
					bool flag4;
					if (!flag3)
					{
						flag4 = abilityData.HasQueuedAction(action);
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					bool flag6;
					if (!flag3)
					{
						flag6 = (abilityData.GetSelectedActionTypeForTargeting() == action);
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					bool flag8 = abilityData.IsAbilityAllowedByUnlockTurns(action);
					int cooldownRemaining = abilityData.GetCooldownRemaining(action);
					int stocksRemaining = abilityData.GetStocksRemaining(action);
					int stockRefreshCountdown = abilityData.GetStockRefreshCountdown(action);
					bool flag9 = ability.GetModdedCost() <= actor.GetEnergyToDisplay();
					if (this.m_ability == ability && this.m_cachedIsQueued == flag5)
					{
						if (this.m_cachedIsTargeting == flag7 && this.m_cachedAbilityUseAllowed == flag8)
						{
							if (this.m_cachedCooldown == cooldownRemaining)
							{
								if (this.m_cachedStocksRemaining == stocksRemaining)
								{
									if (this.m_cachedStockRefreshCountdown == stockRefreshCountdown)
									{
										if (this.m_cachedEnergyOk == flag9)
										{
											return;
										}
									}
								}
							}
						}
					}
					this.m_ability = ability;
					this.m_cachedIsQueued = flag5;
					this.m_cachedIsTargeting = flag7;
					this.m_cachedAbilityUseAllowed = flag8;
					this.m_cachedCooldown = cooldownRemaining;
					this.m_cachedStocksRemaining = stocksRemaining;
					this.m_cachedStockRefreshCountdown = stockRefreshCountdown;
					this.m_cachedEnergyOk = flag9;
					if (this.m_queuedState != null)
					{
						UIManager.SetGameObjectActive(this.m_queuedState, false, null);
					}
					this.ShowDisabledIndicator(false);
					if (this.m_targetingState != null)
					{
						UIManager.SetGameObjectActive(this.m_targetingState, false, null);
					}
					if (this.m_defaultState != null)
					{
						UIManager.SetGameObjectActive(this.m_defaultState, false, null);
					}
					this.m_abilityIconImage.sprite = ability.sprite;
					this.m_cooldownLabel.text = string.Empty;
					if (flag)
					{
						if (flag5)
						{
							if (this.m_queuedState != null)
							{
								UIManager.SetGameObjectActive(this.m_queuedState, true, null);
							}
							if (this.m_phaseColorImage != null)
							{
								this.SetPhaseImageColor(this.m_phaseColorImage, this.m_ability.GetRunPriority());
							}
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
							return;
						}
					}
					if (flag)
					{
						if (flag7)
						{
							if (this.m_targetingState != null)
							{
								UIManager.SetGameObjectActive(this.m_targetingState, true, null);
							}
							if (this.m_targetingPhaseColorImage != null)
							{
								this.SetPhaseImageColor(this.m_targetingPhaseColorImage, this.m_ability.GetRunPriority());
							}
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyTargetingAbilityIconColor;
							return;
						}
					}
					bool flag10 = this.m_ability.GetModdedMaxStocks() > 0;
					if (cooldownRemaining > 0)
					{
						if (!flag10)
						{
							this.ShowDisabledIndicator(true);
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
							this.m_cooldownLabel.text = cooldownRemaining.ToString();
							return;
						}
					}
					if (!flag8)
					{
						this.ShowDisabledIndicator(true);
						this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
						this.m_cooldownLabel.text = "1";
					}
					else
					{
						if (flag10)
						{
							if (stocksRemaining == 0)
							{
								this.ShowDisabledIndicator(true);
								this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
								this.m_cooldownLabel.text = stockRefreshCountdown.ToString();
								return;
							}
						}
						if (!flag9)
						{
							this.ShowDisabledIndicator(true);
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
							this.m_cooldownLabel.text = string.Empty;
						}
						else
						{
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
							if (this.m_defaultState != null)
							{
								UIManager.SetGameObjectActive(this.m_defaultState, true, null);
							}
						}
					}
				}
			}
		}
	}

	private void ShowDisabledIndicator(bool show)
	{
		if (this.m_cooldownState != null)
		{
			UIManager.SetGameObjectActive(this.m_cooldownState, show, null);
			CanvasGroup component = this.m_cooldownState.GetComponent<CanvasGroup>();
			float num;
			if (show)
			{
				num = (float)1;
			}
			else
			{
				num = (float)0;
			}
			component.alpha = num;
		}
	}

	private void SetPhaseImageColor(Image phaseImage, AbilityPriority abilityRunPhase)
	{
		if (abilityRunPhase <= AbilityPriority.Prep_Offense)
		{
			phaseImage.color = this.m_prepPhaseColor;
		}
		else if (abilityRunPhase <= AbilityPriority.Evasion)
		{
			phaseImage.color = this.m_evasionPhaseColor;
		}
		else
		{
			phaseImage.color = this.m_combatPhaseColor;
		}
	}
}
