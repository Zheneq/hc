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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.MouseEnter));
			UIEventTriggerUtils.AddListener(this.m_mouseHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.MouseExit));
			this.m_mouseHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowTooltip), null);
		}
	}

	public void SetCanvasGroupVisibility(bool visible)
	{
		if (this.m_canvasGroup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.SetCanvasGroupVisibility(bool)).MethodHandle;
			}
			if (visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.ShowTooltip(UITooltipBase)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.MouseEnter(BaseEventData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_hoverState, true, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	public void MouseExit(BaseEventData data)
	{
		if (this.m_hoverState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.MouseExit(BaseEventData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	public void OnDisable()
	{
		if (this.m_hoverState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.OnDisable()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			this.m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
		}
	}

	public void Setup(ActorData actor, Ability ability, AbilityData.ActionType action)
	{
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.Setup(ActorData, Ability, AbilityData.ActionType)).MethodHandle;
			}
			if (actor.\u000E() != null)
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
				if (ability != null)
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
					AbilityData abilityData = actor.\u000E();
					bool flag = GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.\u000E());
					bool flag2;
					if (ClientGameManager.Get().PlayerInfo != null)
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
					bool flag9 = ability.GetModdedCost() <= actor.\u0019();
					if (this.m_ability == ability && this.m_cachedIsQueued == flag5)
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
						if (this.m_cachedIsTargeting == flag7 && this.m_cachedAbilityUseAllowed == flag8)
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
							if (this.m_cachedCooldown == cooldownRemaining)
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
								if (this.m_cachedStocksRemaining == stocksRemaining)
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
									if (this.m_cachedStockRefreshCountdown == stockRefreshCountdown)
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
										if (this.m_cachedEnergyOk == flag9)
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						UIManager.SetGameObjectActive(this.m_queuedState, false, null);
					}
					this.ShowDisabledIndicator(false);
					if (this.m_targetingState != null)
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
						UIManager.SetGameObjectActive(this.m_targetingState, false, null);
					}
					if (this.m_defaultState != null)
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
						UIManager.SetGameObjectActive(this.m_defaultState, false, null);
					}
					this.m_abilityIconImage.sprite = ability.sprite;
					this.m_cooldownLabel.text = string.Empty;
					if (flag)
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
						if (flag5)
						{
							if (this.m_queuedState != null)
							{
								UIManager.SetGameObjectActive(this.m_queuedState, true, null);
							}
							if (this.m_phaseColorImage != null)
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
								this.SetPhaseImageColor(this.m_phaseColorImage, this.m_ability.GetRunPriority());
							}
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
							return;
						}
					}
					if (flag)
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
						if (flag7)
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!flag10)
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
							this.ShowDisabledIndicator(true);
							this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
							this.m_cooldownLabel.text = cooldownRemaining.ToString();
							return;
						}
					}
					if (!flag8)
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
						this.ShowDisabledIndicator(true);
						this.m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
						this.m_cooldownLabel.text = "1";
					}
					else
					{
						if (flag10)
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
							if (stocksRemaining == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.ShowDisabledIndicator(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_cooldownState, show, null);
			CanvasGroup component = this.m_cooldownState.GetComponent<CanvasGroup>();
			float num;
			if (show)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITargetingAbilityIndicator.SetPhaseImageColor(Image, AbilityPriority)).MethodHandle;
			}
			phaseImage.color = this.m_evasionPhaseColor;
		}
		else
		{
			phaseImage.color = this.m_combatPhaseColor;
		}
	}
}
