using System;
using System.Collections.Generic;
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
        if (m_layoutGroup != null)
        {
            AbilityStatusEffectEntry[] componentsInChildren =
                m_layoutGroup.GetComponentsInChildren<AbilityStatusEffectEntry>();
            foreach (AbilityStatusEffectEntry entry in componentsInChildren)
            {
                m_statusEffectDisplayList.Add(entry);
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
            m_tooltipDescription.text +=
                Environment.NewLine + Environment.NewLine + "<i>" + ability.m_flavorText + "</i>";
        }

        if (m_freeActionsLabelObj != null)
        {
            UIManager.SetGameObjectActive(m_freeActionsLabelObj, ability.IsFreeAction());
        }

        if (m_cooldownText != null)
        {
            int cooldown = ability.GetCooldownForUIDisplay();
            if (ability.GetModdedMaxStocks() > 0 && ability.GetModdedStockRefreshDuration() >= 0)
            {
                cooldown = ability.GetModdedStockRefreshDuration();
            }

            m_cooldownText.text = cooldown > 0
                ? string.Format(StringUtil.TR("CooldownDuration", "Global"), cooldown)
                : StringUtil.TR("NoCooldown", "Global");
        }

        SetupModTooltip(mod, ability);
        SetupStatusTooltip(mod, ability);

        int phaseIndex =
            UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(ability.RunPriority));
        SetupPhaseIndicators(phaseIndex, ability.GetPhaseString());

        SetupMoviePanel(movieAssetName);
    }

    private void SetupStatusTooltip(AbilityMod mod, Ability ability)
    {
        if (m_statusEffectTransform == null || ability == null)
        {
            return;
        }

        List<StatusType> statusTypesForTooltip = ability.GetStatusTypesForTooltip();
        List<StatusType> modStatusTypes = mod != null
            ? mod.GetStatusTypesForTooltip()
            : new List<StatusType>();

        int i = 0;
        foreach (StatusType status in statusTypesForTooltip)
        {
            if (i >= m_statusEffectDisplayList.Count)
            {
                AbilityStatusEffectEntry entry = Instantiate(m_statusEffectPrefab);
                entry.transform.SetParent(m_layoutGroup.transform);
                entry.transform.localPosition = Vector3.zero;
                entry.transform.localEulerAngles = Vector3.zero;
                entry.transform.localScale = Vector3.one;
                m_statusEffectDisplayList.Add(entry);
            }

            m_statusEffectDisplayList[i].Setup(status);
            i++;
        }

        foreach (StatusType statusType in modStatusTypes)
        {
            if (statusTypesForTooltip.Contains(statusType))
            {
                continue;
            }

            if (i >= m_statusEffectDisplayList.Count)
            {
                AbilityStatusEffectEntry entry = Instantiate(m_statusEffectPrefab);
                entry.transform.SetParent(m_layoutGroup.transform);
                entry.transform.localPosition = Vector3.zero;
                entry.transform.localEulerAngles = Vector3.zero;
                entry.transform.localScale = Vector3.one;
                m_statusEffectDisplayList.Add(entry);
            }

            m_statusEffectDisplayList[i].Setup(statusType);
            i++;
        }

        UIManager.SetGameObjectActive(m_statusEffectTransform, i != 0);

        for (; i < m_statusEffectDisplayList.Count; i++)
        {
            UIManager.SetGameObjectActive(m_statusEffectDisplayList[i], false);
        }
    }

    private void SetupModTooltip(AbilityMod mod, Ability ability)
    {
        if (m_modTooltipParentTransform == null)
        {
            return;
        }

        if (mod != null)
        {
            m_modTitle.text = $"{mod.GetName()} <#8E8E8E>";

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
                UIManager.SetGameObjectActive(m_modTooltipParentTransform, true);
            }
        }
        else
        {
            UIManager.SetGameObjectActive(m_modTooltipParentTransform, false);
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

        if (m_phaseGradient != null)
        {
            m_phaseGradient.color = phaseIndex < m_phaseGradientColors.Length
                ? m_phaseGradientColors[phaseIndex]
                : Color.clear;
        }
    }

    private void SetupPhaseUIElements(PhaseUIElementInfo uiElementInfo, string phaseName)
    {
        if (m_phaseName != null)
        {
            m_phaseName.text = phaseName ?? string.Empty;
        }

        if (m_phaseIcon != null)
        {
            m_phaseIcon.sprite = uiElementInfo?.m_icon;
            UIManager.SetGameObjectActive(m_phaseIcon, m_phaseIcon.sprite != null);
        }

        foreach (PhaseUIElementInfo element in m_perPhaseUIElements)
        {
            if (element.m_tickFill != null)
            {
                UIManager.SetGameObjectActive(element.m_tickFill, element == uiElementInfo);
            }
        }
    }

    private void SetupMoviePanel(string movieAssetName)
    {
        if (m_movieTexturePlayer == null)
        {
            return;
        }

        if (!movieAssetName.IsNullOrEmpty())
        {
            m_movieTexturePlayer.Play(movieAssetName, true, true);
            UIManager.SetGameObjectActive(m_movieContainer, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_movieContainer, false);
        }
    }
}