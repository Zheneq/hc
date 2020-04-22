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
		m_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		if (!(m_mouseHitBox != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, MouseEnter);
			UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerExit, MouseExit);
			m_mouseHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, ShowTooltip);
			return;
		}
	}

	public void SetCanvasGroupVisibility(bool visible)
	{
		if (!(m_canvasGroup != null))
		{
			return;
		}
		while (true)
		{
			if (visible)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_canvasGroup.alpha = 1f;
						m_canvasGroup.blocksRaycasts = true;
						m_canvasGroup.interactable = true;
						return;
					}
				}
			}
			m_canvasGroup.alpha = 0f;
			m_canvasGroup.blocksRaycasts = false;
			m_canvasGroup.interactable = false;
			return;
		}
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		if (m_ability == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
		uIAbilityTooltip.Setup(m_ability, m_ability.CurrentAbilityMod);
		return true;
	}

	public void MouseEnter(BaseEventData data)
	{
		if (!(m_hoverState != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_hoverState, true);
			m_hoverState.GetComponent<CanvasGroup>().alpha = 1f;
			return;
		}
	}

	public void MouseExit(BaseEventData data)
	{
		if (!(m_hoverState != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_hoverState, false);
			m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
			return;
		}
	}

	public void OnDisable()
	{
		if (!(m_hoverState != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_hoverState, false);
			m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
			return;
		}
	}

	public void Setup(ActorData actor, Ability ability, AbilityData.ActionType action)
	{
		if (!(actor != null))
		{
			return;
		}
		while (true)
		{
			if (!(actor.GetAbilityData() != null))
			{
				return;
			}
			while (true)
			{
				if (!(ability != null))
				{
					return;
				}
				while (true)
				{
					AbilityData abilityData = actor.GetAbilityData();
					bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.GetTeam());
					int num;
					if (ClientGameManager.Get().PlayerInfo != null)
					{
						if (ClientGameManager.Get().PlayerInfo.IsSpectator)
						{
							num = (ClientGameManager.Get().SpectatorHideAbilityTargeter ? 1 : 0);
							goto IL_00b1;
						}
					}
					num = 0;
					goto IL_00b1;
					IL_00b1:
					bool flag2 = (byte)num != 0;
					int num2;
					if (!flag2)
					{
						num2 = (abilityData.HasQueuedAction(action) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					bool flag3 = (byte)num2 != 0;
					int num3;
					if (!flag2)
					{
						num3 = ((abilityData.GetSelectedActionTypeForTargeting() == action) ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					bool flag4 = (byte)num3 != 0;
					bool flag5 = abilityData.IsAbilityAllowedByUnlockTurns(action);
					int cooldownRemaining = abilityData.GetCooldownRemaining(action);
					int stocksRemaining = abilityData.GetStocksRemaining(action);
					int stockRefreshCountdown = abilityData.GetStockRefreshCountdown(action);
					bool flag6 = ability.GetModdedCost() <= actor.GetEnergyToDisplay();
					if (m_ability == ability && m_cachedIsQueued == flag3)
					{
						if (m_cachedIsTargeting == flag4 && m_cachedAbilityUseAllowed == flag5)
						{
							if (m_cachedCooldown == cooldownRemaining)
							{
								if (m_cachedStocksRemaining == stocksRemaining)
								{
									if (m_cachedStockRefreshCountdown == stockRefreshCountdown)
									{
										if (m_cachedEnergyOk == flag6)
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
									}
								}
							}
						}
					}
					m_ability = ability;
					m_cachedIsQueued = flag3;
					m_cachedIsTargeting = flag4;
					m_cachedAbilityUseAllowed = flag5;
					m_cachedCooldown = cooldownRemaining;
					m_cachedStocksRemaining = stocksRemaining;
					m_cachedStockRefreshCountdown = stockRefreshCountdown;
					m_cachedEnergyOk = flag6;
					if (m_queuedState != null)
					{
						UIManager.SetGameObjectActive(m_queuedState, false);
					}
					ShowDisabledIndicator(false);
					if (m_targetingState != null)
					{
						UIManager.SetGameObjectActive(m_targetingState, false);
					}
					if (m_defaultState != null)
					{
						UIManager.SetGameObjectActive(m_defaultState, false);
					}
					m_abilityIconImage.sprite = ability.sprite;
					m_cooldownLabel.text = string.Empty;
					if (flag)
					{
						if (flag3)
						{
							if (m_queuedState != null)
							{
								UIManager.SetGameObjectActive(m_queuedState, true);
							}
							if (m_phaseColorImage != null)
							{
								SetPhaseImageColor(m_phaseColorImage, m_ability.GetRunPriority());
							}
							m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
							return;
						}
					}
					if (flag)
					{
						if (flag4)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									if (m_targetingState != null)
									{
										UIManager.SetGameObjectActive(m_targetingState, true);
									}
									if (m_targetingPhaseColorImage != null)
									{
										SetPhaseImageColor(m_targetingPhaseColorImage, m_ability.GetRunPriority());
									}
									m_abilityIconImage.color = HighlightUtils.Get().m_allyTargetingAbilityIconColor;
									return;
								}
							}
						}
					}
					bool flag7 = m_ability.GetModdedMaxStocks() > 0;
					if (cooldownRemaining > 0)
					{
						if (!flag7)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									ShowDisabledIndicator(true);
									m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
									m_cooldownLabel.text = cooldownRemaining.ToString();
									return;
								}
							}
						}
					}
					if (!flag5)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								ShowDisabledIndicator(true);
								m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
								m_cooldownLabel.text = "1";
								return;
							}
						}
					}
					if (flag7)
					{
						if (stocksRemaining == 0)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									ShowDisabledIndicator(true);
									m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
									m_cooldownLabel.text = stockRefreshCountdown.ToString();
									return;
								}
							}
						}
					}
					if (!flag6)
					{
						ShowDisabledIndicator(true);
						m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
						m_cooldownLabel.text = string.Empty;
						return;
					}
					m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
					if (m_defaultState != null)
					{
						UIManager.SetGameObjectActive(m_defaultState, true);
					}
					return;
				}
			}
		}
	}

	private void ShowDisabledIndicator(bool show)
	{
		if (!(m_cooldownState != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_cooldownState, show);
			CanvasGroup component = m_cooldownState.GetComponent<CanvasGroup>();
			int num;
			if (show)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			component.alpha = num;
			return;
		}
	}

	private void SetPhaseImageColor(Image phaseImage, AbilityPriority abilityRunPhase)
	{
		if (abilityRunPhase <= AbilityPriority.Prep_Offense)
		{
			phaseImage.color = m_prepPhaseColor;
			return;
		}
		if (abilityRunPhase <= AbilityPriority.Evasion)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					phaseImage.color = m_evasionPhaseColor;
					return;
				}
			}
		}
		phaseImage.color = m_combatPhaseColor;
	}
}
