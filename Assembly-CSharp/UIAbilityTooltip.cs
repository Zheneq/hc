using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityTooltip : UITooltipBase
{
	[Serializable]
	public class PhaseUIElementInfo
	{
		public string m_phaseName;

		public Sprite m_icon;

		public Image m_tickFill;
	}

	[Header("-- For Ability Tooltip --")]
	public TextMeshProUGUI m_tooltipTitle;

	public TextMeshProUGUI m_tooltipDescription;

	[Space(10f)]
	public GameObject m_freeActionsLabelObj;

	public TextMeshProUGUI m_cooldownText;

	[Header("-- For Mod Tooltip --")]
	public RectTransform m_modTooltipParentTransform;

	public TextMeshProUGUI m_modTitle;

	public TextMeshProUGUI m_modTooltip;

	public RectTransform m_modCostContainer;

	public Image[] m_modCostNotches;

	[Header("-- For Status Tooltip --")]
	public RectTransform m_statusEffectTransform;

	public AbilityStatusEffectEntry m_statusEffectPrefab;

	public VerticalLayoutGroup m_layoutGroup;

	[Header("-- For Phases (assuming arrays have 3 elements, idx 0 for Prep, Idx 1 for Dash, Idx 2 for Combat --")]
	public TextMeshProUGUI m_phaseName;

	public Image m_phaseIcon;

	public Image m_phaseGradient;

	public Color[] m_phaseGradientColors;

	public PhaseUIElementInfo[] m_perPhaseUIElements;

	[Header("-- For Video --")]
	public bool disableVideo;

	public RectTransform m_movieContainer;

	public PlayRawImageMovieTexture m_movieTexturePlayer;

	[Header("-- For Resizing Tooltip --")]
	public float m_minAbilityTooltipDescHeight = 100f;

	public float m_minModTooltipDescHeight = 50f;

	public float m_abilityTooltipTextHeightPadding = 120f;

	public float m_modTooltipTextHeightPadding = 50f;

	private const string c_modTitleFormatString = "{0} <#8E8E8E>";

	private List<AbilityStatusEffectEntry> m_statusEffectDisplayList = new List<AbilityStatusEffectEntry>();

	private void Awake()
	{
		if (!(m_layoutGroup != null))
		{
			return;
		}
		while (true)
		{
			AbilityStatusEffectEntry[] componentsInChildren = m_layoutGroup.GetComponentsInChildren<AbilityStatusEffectEntry>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				m_statusEffectDisplayList.Add(componentsInChildren[i]);
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void Setup(Ability ability)
	{
		Setup(ability, null, null);
	}

	public void Setup(Ability ability, AbilityMod mod)
	{
		Setup(ability, mod, null);
	}

	public void Setup(Ability ability, AbilityMod mod, string movieAssetName)
	{
		m_tooltipTitle.text = ability.GetNameString();
		m_tooltipDescription.text = ability.GetToolTipString();
		if (!ability.m_flavorText.IsNullOrEmpty())
		{
			TextMeshProUGUI tooltipDescription = m_tooltipDescription;
			string text = tooltipDescription.text;
			tooltipDescription.text = new StringBuilder().AppendLine(text).AppendLine().Append("<i>").Append(ability.m_flavorText).Append("</i>").ToString();
		}
		if (m_freeActionsLabelObj != null)
		{
			UIManager.SetGameObjectActive(m_freeActionsLabelObj, ability.IsFreeAction());
		}
		if (m_cooldownText != null)
		{
			int num = ability.GetCooldownForUIDisplay();
			if (ability.GetModdedMaxStocks() > 0)
			{
				if (ability.GetModdedStockRefreshDuration() >= 0)
				{
					num = ability.GetModdedStockRefreshDuration();
				}
			}
			if (num > 0)
			{
				m_cooldownText.text = string.Format(StringUtil.TR("CooldownDuration", "Global"), num);
			}
			else
			{
				m_cooldownText.text = StringUtil.TR("NoCooldown", "Global");
			}
		}
		SetupModTooltip(mod, ability);
		SetupStatusTooltip(mod, ability);
		int phaseIndex = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(ability.RunPriority));
		SetupPhaseIndicators(phaseIndex, ability.GetPhaseString());
		SetupMoviePanel(movieAssetName);
	}

	private void SetupStatusTooltip(AbilityMod mod, Ability ability)
	{
		if (!(m_statusEffectTransform != null))
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
				List<StatusType> statusTypesForTooltip = ability.GetStatusTypesForTooltip();
				List<StatusType> list;
				if (mod != null)
				{
					list = mod.GetStatusTypesForTooltip();
				}
				else
				{
					list = new List<StatusType>();
				}
				int i = 0;
				for (int j = 0; j < statusTypesForTooltip.Count; j++)
				{
					if (i >= m_statusEffectDisplayList.Count)
					{
						AbilityStatusEffectEntry abilityStatusEffectEntry = UnityEngine.Object.Instantiate(m_statusEffectPrefab);
						abilityStatusEffectEntry.transform.SetParent(m_layoutGroup.transform);
						abilityStatusEffectEntry.transform.localPosition = Vector3.zero;
						abilityStatusEffectEntry.transform.localEulerAngles = Vector3.zero;
						abilityStatusEffectEntry.transform.localScale = Vector3.one;
						m_statusEffectDisplayList.Add(abilityStatusEffectEntry);
					}
					m_statusEffectDisplayList[i].Setup(statusTypesForTooltip[j]);
					i++;
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (!statusTypesForTooltip.Contains(list[k]))
					{
						if (i >= m_statusEffectDisplayList.Count)
						{
							AbilityStatusEffectEntry abilityStatusEffectEntry2 = UnityEngine.Object.Instantiate(m_statusEffectPrefab);
							abilityStatusEffectEntry2.transform.SetParent(m_layoutGroup.transform);
							abilityStatusEffectEntry2.transform.localPosition = Vector3.zero;
							abilityStatusEffectEntry2.transform.localEulerAngles = Vector3.zero;
							abilityStatusEffectEntry2.transform.localScale = Vector3.one;
							m_statusEffectDisplayList.Add(abilityStatusEffectEntry2);
						}
						m_statusEffectDisplayList[i].Setup(list[k]);
						i++;
					}
				}
				UIManager.SetGameObjectActive(m_statusEffectTransform, i != 0);
				for (; i < m_statusEffectDisplayList.Count; i++)
				{
					UIManager.SetGameObjectActive(m_statusEffectDisplayList[i], false);
				}
				return;
			}
		}
	}

	private void SetupModTooltip(AbilityMod mod, Ability ability)
	{
		if (!(m_modTooltipParentTransform != null))
		{
			return;
		}
		while (true)
		{
			if (mod != null)
			{
				m_modTitle.text = new StringBuilder().Append(mod.GetName()).Append(" <#8E8E8E>").ToString();
				if (m_modCostContainer != null)
				{
					for (int i = 0; i < m_modCostNotches.Length; i++)
					{
						UIManager.SetGameObjectActive(m_modCostNotches[i], i < mod.m_equipCost);
					}
				}
				m_modTooltip.text = mod.GetFullTooltip(ability);
				if (!m_modTooltipParentTransform.gameObject.activeSelf)
				{
					while (true)
					{
						UIManager.SetGameObjectActive(m_modTooltipParentTransform, true);
						return;
					}
				}
				return;
			}
			UIManager.SetGameObjectActive(m_modTooltipParentTransform, false);
			return;
		}
	}

	private void SetupPhaseIndicators(int phaseIndex, string phaseName)
	{
		if (phaseIndex < m_perPhaseUIElements.Length)
		{
			SetupPhaseUIElements(m_perPhaseUIElements[phaseIndex], phaseName);
		}
		else
		{
			SetupPhaseUIElements(null, null);
		}
		if (!(m_phaseGradient != null))
		{
			return;
		}
		while (true)
		{
			if (phaseIndex < m_phaseGradientColors.Length)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_phaseGradient.color = m_phaseGradientColors[phaseIndex];
						return;
					}
				}
			}
			m_phaseGradient.color = Color.clear;
			return;
		}
	}

	private void SetupPhaseUIElements(PhaseUIElementInfo uiElementInfo, string phaseName)
	{
		if (m_phaseName != null)
		{
			TextMeshProUGUI phaseName2 = m_phaseName;
			string text;
			if (phaseName != null)
			{
				text = phaseName;
			}
			else
			{
				text = string.Empty;
			}
			phaseName2.text = text;
		}
		if (m_phaseIcon != null)
		{
			Image phaseIcon = m_phaseIcon;
			object sprite;
			if (uiElementInfo != null)
			{
				sprite = uiElementInfo.m_icon;
			}
			else
			{
				sprite = null;
			}
			phaseIcon.sprite = (Sprite)sprite;
			if (!m_phaseIcon.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(m_phaseIcon, true);
			}
		}
		for (int i = 0; i < m_perPhaseUIElements.Length; i++)
		{
			if (m_perPhaseUIElements[i].m_tickFill != null)
			{
				UIManager.SetGameObjectActive(m_perPhaseUIElements[i].m_tickFill, m_perPhaseUIElements[i] == uiElementInfo);
			}
		}
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

	private void SetupMoviePanel(string movieAssetName)
	{
		if (!(m_movieTexturePlayer != null))
		{
			return;
		}
		while (true)
		{
			if (!movieAssetName.IsNullOrEmpty())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_movieTexturePlayer.Play(movieAssetName, true, true);
						UIManager.SetGameObjectActive(m_movieContainer, true);
						return;
					}
				}
			}
			UIManager.SetGameObjectActive(m_movieContainer, false);
			return;
		}
	}
}
