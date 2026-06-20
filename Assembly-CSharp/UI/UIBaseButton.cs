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

	public static Color s_bgUnavailableColor = new Color(14f / 51f, 14f / 51f, 14f / 51f);

	public static Color s_bgPrepColor = new Color(8f / 15f, 0.003921569f, 0.9411765f);

	public static Color s_bgEvasionColor = new Color(134f / 255f, 214f / 255f, 0.003921569f);

	public static Color s_bgCombatColor = new Color(50f / 51f, 2f / 3f, 0f);

	public static Color s_fgUnavailableColor = new Color(98f / 255f, 98f / 255f, 98f / 255f);

	public static Color s_queuedGlowColor = new Color(1f, 0.4f, 0f);

	public static Color s_ultReadyGlowColor = new Color(0f, 1f, 1f);

	public static Color s_invalidButton_cooldown_hotkeyTextColor = new Color(0.1f, 0.1f, 0.1f);

	public static Color s_invalidButton_state_hotkeyTextColor = new Color(0.2f, 0.2f, 0.2f);

	public static Color s_invalidButton_techPts_hotkeyTextColor = new Color(0.1f, 0.1f, 0.8f);

	public static Color s_freeActionTextColor = new Color(0f, 1f, 0f);

	public AbilityData.AbilityEntry GetEntry()
	{
		return m_abilityEntry;
	}

	public static Color ColorForUIPhase(UIQueueListPanel.UIPhase phase)
	{
		switch (phase)
		{
		case UIQueueListPanel.UIPhase.Prep:
			return s_bgPrepColor;
		case UIQueueListPanel.UIPhase.Evasion:
			return s_bgEvasionColor;
		case UIQueueListPanel.UIPhase.Combat:
			return s_bgCombatColor;
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
		if (m_abilityEntry != null)
		{
			if (!(m_abilityEntry.ability == null))
			{
				UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
				uIAbilityTooltip.Setup(m_abilityEntry.ability);
				return true;
			}
		}
		return false;
	}

	public virtual void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData)
	{
		m_abilityEntry = abilityEntry;
		m_actionType = actionType;
		m_abilityData = abilityData;
		m_turnSM = m_abilityData.GetComponent<ActorTurnSM>();
		if (m_abilityEntry != null)
		{
			if (m_abilityEntry.ability != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_theSprite, true);
						m_theSprite.sprite = m_abilityEntry.ability.sprite;
						m_hotKeyLabel.text = m_abilityEntry.hotkey;
						return;
					}
				}
			}
		}
		if (m_abilityEntry != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_statusLabel.text = string.Empty;
					UIManager.SetGameObjectActive(m_theSprite, false);
					theButton.enabled = false;
					m_hotKeyLabel.text = m_abilityEntry.hotkey;
					m_hotKeyLabel.color = s_fgUnavailableColor;
					return;
				}
			}
		}
		m_statusLabel.text = string.Empty;
		UIManager.SetGameObjectActive(m_theSprite, false);
		theButton.enabled = false;
		m_hotKeyLabel.color = s_fgUnavailableColor;
	}

	public virtual void Start()
	{
		UITooltipHoverObject component;
		if (theButton != null)
		{
			component = theButton.GetComponent<UITooltipHoverObject>();
		}
		else
		{
			component = GetComponent<UITooltipHoverObject>();
		}
		if (!component)
		{
			return;
		}
		while (true)
		{
			component.Setup(TooltipType.Ability, ShowTooltip);
			return;
		}
	}

	public virtual void Update()
	{
	}
}
