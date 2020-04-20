using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseButton : MonoBehaviour
{
	public Image[] m_phaseIndicators;

	public Image m_backgroundSprite;

	public Image m_freeActionSprite;

	public Image m_theSprite;

	public Text m_hotKeyLabel;

	public Text m_statusLabel;

	public Image m_outline;

	public Image m_glow;

	public Image m_queued;

	public Image m_disabled;

	public Button theButton;

	protected AbilityData.AbilityEntry m_abilityEntry;

	protected AbilityData.ActionType m_actionType;

	protected AbilityData m_abilityData;

	protected ActorTurnSM m_turnSM;

	public static Color s_bgUnavailableColor = new Color(0.274509817f, 0.274509817f, 0.274509817f);

	public static Color s_bgPrepColor = new Color(0.533333361f, 0.003921569f, 0.9411765f);

	public static Color s_bgEvasionColor = new Color(0.5254902f, 0.8392157f, 0.003921569f);

	public static Color s_bgCombatColor = new Color(0.980392158f, 0.6666667f, 0f);

	public static Color s_fgUnavailableColor = new Color(0.384313732f, 0.384313732f, 0.384313732f);

	public static Color s_queuedGlowColor = new Color(1f, 0.4f, 0f);

	public static Color s_ultReadyGlowColor = new Color(0f, 1f, 1f);

	public static Color s_invalidButton_cooldown_hotkeyTextColor = new Color(0.1f, 0.1f, 0.1f);

	public static Color s_invalidButton_state_hotkeyTextColor = new Color(0.2f, 0.2f, 0.2f);

	public static Color s_invalidButton_techPts_hotkeyTextColor = new Color(0.1f, 0.1f, 0.8f);

	public static Color s_freeActionTextColor = new Color(0f, 1f, 0f);

	public AbilityData.AbilityEntry GetEntry()
	{
		return this.m_abilityEntry;
	}

	public static Color ColorForUIPhase(UIQueueListPanel.UIPhase phase)
	{
		switch (phase)
		{
		case UIQueueListPanel.UIPhase.Prep:
			return UIBaseButton.s_bgPrepColor;
		case UIQueueListPanel.UIPhase.Evasion:
			return UIBaseButton.s_bgEvasionColor;
		case UIQueueListPanel.UIPhase.Combat:
			return UIBaseButton.s_bgCombatColor;
		default:
			return Color.black;
		}
	}

	public static int PhaseIndexForUIPhase(UIQueueListPanel.UIPhase phase)
	{
		switch (phase)
		{
		case UIQueueListPanel.UIPhase.Prep:
			return 0;
		case UIQueueListPanel.UIPhase.Evasion:
			return 1;
		case UIQueueListPanel.UIPhase.Combat:
			return 2;
		default:
			return -1;
		}
	}

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		if (this.m_abilityEntry != null)
		{
			if (!(this.m_abilityEntry.ability == null))
			{
				UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
				uiabilityTooltip.Setup(this.m_abilityEntry.ability);
				return true;
			}
		}
		return false;
	}

	public virtual void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData)
	{
		this.m_abilityEntry = abilityEntry;
		this.m_actionType = actionType;
		this.m_abilityData = abilityData;
		this.m_turnSM = this.m_abilityData.GetComponent<ActorTurnSM>();
		if (this.m_abilityEntry != null)
		{
			if (this.m_abilityEntry.ability != null)
			{
				UIManager.SetGameObjectActive(this.m_theSprite, true, null);
				this.m_theSprite.sprite = this.m_abilityEntry.ability.sprite;
				this.m_hotKeyLabel.text = this.m_abilityEntry.hotkey;
				return;
			}
		}
		if (this.m_abilityEntry != null)
		{
			this.m_statusLabel.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_theSprite, false, null);
			this.theButton.enabled = false;
			this.m_hotKeyLabel.text = this.m_abilityEntry.hotkey;
			this.m_hotKeyLabel.color = UIBaseButton.s_fgUnavailableColor;
		}
		else
		{
			this.m_statusLabel.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_theSprite, false, null);
			this.theButton.enabled = false;
			this.m_hotKeyLabel.color = UIBaseButton.s_fgUnavailableColor;
		}
	}

	public virtual void Start()
	{
		UITooltipHoverObject component;
		if (this.theButton != null)
		{
			component = this.theButton.GetComponent<UITooltipHoverObject>();
		}
		else
		{
			component = base.GetComponent<UITooltipHoverObject>();
		}
		if (component)
		{
			component.Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowTooltip), null);
		}
	}

	public virtual void Update()
	{
	}
}
