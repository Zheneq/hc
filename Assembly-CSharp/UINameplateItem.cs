using System;
using System.Collections.Generic;
#if EVOS
using Evos.ActorStatus;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINameplateItem : MonoBehaviour, IGameEventListener
{
    [Serializable]
    public struct TeamBars
    {
        public Image m_teamMarker;
        [Range(0f, 1f)]
        public float m_currentHPPercent;
        public Image m_currentHPBar;
        [Range(0f, 1f)]
        public float m_damageEasePercent;
        public Image m_damageEasedBar;
    }

    [Serializable]
    public struct AbilityModifier
    {
        public Animator m_targetingTextAnimationController;
        public TextMeshProUGUI m_abilityModifierText;
    }

    [Serializable]
    public struct NameplateCombatText
    {
        public Animator m_CombatTextController;
        public TextMeshProUGUI m_combatText;
        public Image m_combatTextCover;
        public Image m_iconImage;
        public Color m_colorToChangeCombatText;
        public RectTransform m_TextScalar;
    }

    public enum BarColor
    {
        None,
        Self,
        Team,
        Enemy
    }

    public enum AbilityModifierType
    {
        Damage,
        Healing,
        Absorb,
        Energy,
        MAX
    }

    public struct StatusDisplayInfo
    {
        public StatusType statusType;
#if EVOS
        public EvosActorStatusType evosStatusType;
#endif
        public UINameplateStatus statusObject;
    }

    public struct StaticStatusDisplayInfo
    {
        public StatusType statusType;
#if EVOS
        public EvosActorStatusType evosStatusType;
#endif
        public UIBuffIndicator statusObject;
        public bool m_removedBuff;
    }

    public NameplateCombatText[] nameplateCombatText;
    public bool m_showCombatTextIcons = true;
    [Space(10f)]
    public UIBuffIndicator m_buffIndicatorPrefab;
    public GridLayoutGroup m_buffIndicatorGrid;
    public RectTransform m_statusContainer;
    public UINameplateStatus m_statusPrefab;
    [Space(10f)]
    public GridLayoutGroup m_targetingAbilityIconsGrid;
    public GameObject m_targetingAbilityIndicatorPrefab;
    public UITargetingAbilityCatalystPipContainer m_catalystIndicatorPrefab;
    [Space(10f)]
    public GameObject m_overconParent;
    public GameObject m_overconPrefab;
    [Space(10f)]
    public Button m_HPMouseOverHitBox;
    public CanvasGroup m_mouseOverHitBoxCanvasGroup;
    [Space(10f)]
    public Animator m_redBriefcaseContainer;
    public Animator m_blueBriefcaseContainer;
    public ImageFilledSloped m_redFillMarker;
    public ImageFilledSloped m_blueFillMarker;
    [Space(10f)]
    public RectTransform m_parentTransform;
    public Image m_coverSymbol;
    public Image m_zeroHealthIcon;
    public Image m_targetGlow;
    [Range(0f, 1f)]
    public float m_shieldPercent;
    public Image m_shieldBar;

    private EasedFloat m_ShieldEased;
    private EasedFloat m_HPDamageEased;

    [Range(0f, 1f)]
    public float m_hpGainedPercent;
    public Image m_hpGainBar;
    private EasedFloat m_HPGainedEased;
    public TeamBars m_selfBars;
    private EasedFloat m_HPEased;
    public TeamBars m_teamBars;
    public TeamBars m_enemyBars;

    [Range(0f, 1f)]
    public float m_tpGainedPercent;
    public Image m_tpGainBar;
    private EasedFloat m_TPEasedGained;
    [Range(0f, 1f)]
    public float m_tpGainedEasePercent;
    public Image m_tpGainEaseBar;
    private EasedFloat m_TPEased;
    public BarColor barsToShow;
    public TextMeshProUGUI m_textNameLabel;
    public TextMeshProUGUI m_healthLabel;
    public RectTransform m_maxEnergyContainer;
    public Animator m_maxEnergyAnimator;
    public AbilityModifier[] abilityModifiers;
    public AbilityModifier allyAbilityModifier;
    public Sprite[] m_abilityModifierSprites = new Sprite[4];
    public Image m_healthTickPrefab;
    public RectTransform m_tickContainer;
    [Header("-- For when an ability is happening --")]
    public float m_alphaWhenOthersHighlighted = 0.5f;
    public float m_scaleWhenOthersHighlighted = 0.9f;
    public float m_scaleWhenHighlighted = 1.2f;
    public TextMeshProUGUI m_debugText;
    public Color m_DamageColor = Color.red;
    public Color m_HealingColor = Color.green;
    public Color m_AbsorbColor = Color.magenta;
    public Color m_EnergyColor = Color.yellow;
    public Color m_FullEnergyColor = default(Color);
    public Color m_NonFullEnergyColor = new Color(1f, 1f, 0.42745f);
    public TMP_FontAsset m_buffFont;
    public TMP_FontAsset m_debuffFont;
    public CanvasGroup m_canvasGroup;
    public CanvasGroup m_combatTextCanvasGroup;
    public CanvasGroup m_abilityPreviewCanvasGroup;
    public float m_distanceFromCamera;

    private List<StatusDisplayInfo> m_statusEffectsAnimating;
    private List<StaticStatusDisplayInfo> m_statusEffects;
    private List<UITargetingAbilityIndicator> m_targetingAbilityIndicators;
    private UITargetingAbilityCatalystPipContainer m_catalsystPips;

    public const int c_maxNumTargetingAbilityIndicators = 6;

    private ActorData m_actorData;
    private int m_currentStatusStackCount;
    private int m_previousHP;
    private int m_previousHPMax;
    private int m_previousShieldValue;
    private int m_previousHPShieldAndHot;
    private int m_previousTP;
    private int m_previousTPMax;
    private int m_previousResolvedHP;
    private int m_maxHPWithShield;
    private bool m_visible;
    private bool m_lostHealth;
    private int m_sortOrder;

    private const float BAR_BACKGROUND_ANIM_SECONDS = 2.5f;
    private const float SHIELDBAR_BACKGROUND_ANIM_SECONDS = 1f;

    private Canvas myCanvas;
    private RectTransform CanvasRect;
    private UINameplateOvercon m_overcon;
    private int m_numOverconUsesThisTurn;
    private int m_lastTurnOverconUsed;
    private int m_numOverconUsesThisMatch;
    private int[] m_numOverconUsesById;
    private BarColor lastBarColor;
    private float m_alphaToUse;
    private List<Image> m_ticks;
    private bool m_mouseIsOverHP;
    private bool m_textVisible = true;
    private bool m_hpGainedIsFading = true;

    private const float kHpGainBarBlinkRate = 0.05f;

    private bool m_isMaxEnergy;
    private UINameplateTickShaderHelper[] m_tickShaderHelpers;
    private int m_lastUsedCombatTextIndex = -1;
    private float m_fadeoutStartTime;
    private bool m_isHoldingFlag;

    private Dictionary<AbilityTooltipSymbol, int> m_tempTargetingNumSymbolToValueMap =
        new Dictionary<AbilityTooltipSymbol, int>();

    private float m_setToDimTime = -1f;

    private void Awake()
    {
        m_sortOrder = -1;
        m_ShieldEased = new EasedFloat(0f);
        m_HPDamageEased = new EasedFloat(0f);
        m_HPGainedEased = new EasedFloat(0f);
        m_HPEased = new EasedFloat(0f);
        m_TPEasedGained = new EasedFloat(0f);
        m_TPEased = new EasedFloat(0f);
        m_statusEffectsAnimating = new List<StatusDisplayInfo>();
        m_statusEffects = new List<StaticStatusDisplayInfo>();
        m_targetingAbilityIndicators = new List<UITargetingAbilityIndicator>();
        m_visible = true;
        m_currentStatusStackCount = 0;
        UIEventTriggerUtils.AddListener(m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerEnter, MouseOverHP);
        UIEventTriggerUtils.AddListener(m_HPMouseOverHitBox.gameObject, EventTriggerType.PointerExit, MouseExitHP);
        UIManager.SetGameObjectActive(m_maxEnergyContainer, false);
        m_tickShaderHelpers = GetComponentsInChildren<UINameplateTickShaderHelper>(true);
        Mask componentInChildren = GetComponentInChildren<Mask>();
        if (componentInChildren != null)
        {
            UIManager.SetGameObjectActive(componentInChildren, false);
        }
    }

    public void SetSortOrder(int order)
    {
        m_sortOrder = order;
    }

    public int GetSortOrder()
    {
        return m_sortOrder;
    }

    public void MouseOverHP(BaseEventData data)
    {
        m_mouseIsOverHP = true;
    }

    public void MouseExitHP(BaseEventData data)
    {
        m_mouseIsOverHP = false;
    }

    public void SetTextVisible(bool visible)
    {
        m_textVisible = visible;
        UIManager.SetGameObjectActive(m_textNameLabel, visible);
        UIManager.SetGameObjectActive(m_healthLabel, visible);
    }

    private void Start()
    {
        UIManager.SetGameObjectActive(m_coverSymbol, false);
        for (int i = 0; i < abilityModifiers.Length; i++)
        {
            UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
        }

        UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
        SetDebugText(string.Empty);
        for (int i = 0; i < nameplateCombatText.Length; i++)
        {
            nameplateCombatText[i].m_combatTextCover.color = new Color(1f, 1f, 1f, 0f);
        }

        GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
        GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
        GameEventManager.Get().AddListener(this, GameEventManager.EventType.ClientResolutionStarted);
        GameEventManager.Get().AddListener(this, GameEventManager.EventType.NormalMovementStart);
        int num = 0;
        foreach (UIOverconData.NameToOverconEntry nameToOverconEntry in UIOverconData.Get().m_nameToOverconEntry)
        {
            if (num < nameToOverconEntry.m_overconId)
            {
                num = nameToOverconEntry.m_overconId;
            }
        }

        m_numOverconUsesById = new int[num + 1];
    }

    private void OnDestroy()
    {
        if (GameEventManager.Get() != null)
        {
            GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilityHighlightStart);
            GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
            GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ClientResolutionStarted);
            GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.NormalMovementStart);
        }
    }

    private void SetMaxHPWithShield(int newVal)
    {
        bool isChanged = m_maxHPWithShield != newVal;
        m_maxHPWithShield = newVal;
        if (isChanged)
        {
            float tickDivisions = m_maxHPWithShield / HUD_UIResources.Get().m_AmtOfHealthPerTick;
            foreach (UINameplateTickShaderHelper tickShaderHelper in m_tickShaderHelpers)
            {
                tickShaderHelper.SetTickDivisions(tickDivisions);
            }
        }
    }

    private void SetTeamBars(TeamBars bars, bool visible)
    {
        UIManager.SetGameObjectActive(bars.m_teamMarker, visible);
        UIManager.SetGameObjectActive(bars.m_currentHPBar, visible);
        UIManager.SetGameObjectActive(bars.m_damageEasedBar, visible);
        NotifyFlagStatusChange(m_isHoldingFlag);
    }

    public static BarColor GetRelationshipWithPlayer(ActorData theActor)
    {
        if (theActor == null)
        {
            return BarColor.None;
        }

        if (GameFlowData.Get().LocalPlayerData == null)
        {
            return BarColor.None;
        }

        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (activeOwnedActorData != null)
        {
            if (activeOwnedActorData == theActor)
            {
                return BarColor.Self;
            }

            if (activeOwnedActorData.GetTeam() == theActor.GetTeam())
            {
                return BarColor.Team;
            }

            return BarColor.Enemy;
        }

        if (theActor.GetTeam() == Team.TeamA)
        {
            return BarColor.Team;
        }

        return BarColor.Enemy;
    }

    public void HandleAnimCallback(Animator animationController, UINameplateAnimCallback.AnimationCallback type)
    {
        if (animationController == null)
        {
            return;
        }

        if (type == UINameplateAnimCallback.AnimationCallback.COMBAT_TEXT_COLOR)
        {
            for (int i = 0; i < nameplateCombatText.Length; i++)
            {
                if (animationController == nameplateCombatText[i].m_CombatTextController)
                {
                    SetCombatTextColor(i);
                }
            }
        }
    }

    public void SetCombatTextColor(int index)
    {
        if (-1 < index && index < nameplateCombatText.Length)
        {
            nameplateCombatText[index].m_combatText.color = nameplateCombatText[index].m_colorToChangeCombatText;
        }
    }

    public void StartTargetingNumberFadeout()
    {
        m_fadeoutStartTime = Time.time;
        for (int i = 0; i < abilityModifiers.Length; i++)
        {
            Color color = abilityModifiers[i].m_abilityModifierText.color;
            color.a = 1f;
            abilityModifiers[i].m_abilityModifierText.color = color;
            if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
            {
                abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
                abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
            }
        }

        Color color2 = allyAbilityModifier.m_abilityModifierText.color;
        color2.a = 1f;
        allyAbilityModifier.m_abilityModifierText.color = color2;
        if (allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
        {
            allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
            allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
        }
    }

    public void ShowTargetingNumberForConfirmedTargeting()
    {
        m_fadeoutStartTime = Time.time;
        for (int i = 0; i < abilityModifiers.Length; i++)
        {
            Color color = abilityModifiers[i].m_abilityModifierText.color;
            color.a = 1f;
            abilityModifiers[i].m_abilityModifierText.color = color;
            if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeSelf)
            {
                abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", true);
            }
        }

        Color color2 = allyAbilityModifier.m_abilityModifierText.color;
        color2.a = 1f;
        allyAbilityModifier.m_abilityModifierText.color = color2;
        if (allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeSelf)
        {
            allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
        }
    }

    public void PlayCombatText(ActorData actorData, string text, CombatTextCategory category, BuffIconToDisplay icon)
    {
        bool isCover = false;
        Color c = Color.black;
        BarColor relationshipWithPlayer = GetRelationshipWithPlayer(actorData);
        bool isHit = false;
        bool isMediumHit = false;
        int num = 0;
        const string combatTextHit = "CombatTextHit";
        const string combatTextHitMedium = "CombatTextHitMedium";
        bool isFound = false;
        for (int i = 0; i < nameplateCombatText.Length; i++)
        {
            AnimatorClipInfo[] currentAnimatorClipInfo =
                nameplateCombatText[i].m_CombatTextController.GetCurrentAnimatorClipInfo(0);
            if (currentAnimatorClipInfo.Length > 0
                && currentAnimatorClipInfo[0].clip.name != combatTextHit
                && currentAnimatorClipInfo[0].clip.name != combatTextHitMedium)
            {
                num = i;
                isFound = true;
                break;
            }
        }

        if (isFound)
        {
            m_lastUsedCombatTextIndex = num;
        }
        else
        {
            num = m_lastUsedCombatTextIndex = (m_lastUsedCombatTextIndex + 1) % nameplateCombatText.Length;
        }

        float number = 0f;
        switch (category)
        {
            case CombatTextCategory.Damage:
            {
                string[] array = text.Split('|');
                nameplateCombatText[num].m_combatText.text = array[0];
                number = int.Parse(array[0]);
                if (array[1] == "C")
                {
                    isCover = true;
                }

                if (relationshipWithPlayer != BarColor.Self && relationshipWithPlayer != BarColor.Team)
                {
                    if (relationshipWithPlayer == BarColor.Enemy)
                    {
                        nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 71f / 85f, 71f / 85f);
                        c = new Color(194f / 255f, 0f, 0f);
                        isMediumHit = false;
                    }
                }
                else
                {
                    nameplateCombatText[num].m_colorToChangeCombatText = new Color(1f, 0.011764f, 0.011764f);
                    c = new Color(91f / 255f, 0f, 0f);
                    isMediumHit = true;
                }

                isHit = true;
                break;
            }
            case CombatTextCategory.Healing:
                if (relationshipWithPlayer != BarColor.Self && relationshipWithPlayer != BarColor.Team)
                {
                    if (relationshipWithPlayer == BarColor.Enemy)
                    {
                        nameplateCombatText[num].m_colorToChangeCombatText = new Color(
                            2f / 51f,
                            127f / 255f,
                            16f / 255f);
                        c = new Color(0f, 9f / 85f, 0.003921569f);
                        isMediumHit = true;
                    }

                }
                else
                {
                    nameplateCombatText[num].m_colorToChangeCombatText = new Color(0.7843137f, 1f, 203f / 255f);
                    c = new Color(0f, 39f / 85f, 0.0117647061f);
                    isMediumHit = false;
                }
                nameplateCombatText[num].m_combatText.text = text;
                number = int.Parse(text);
                isHit = true;
                break;
            case CombatTextCategory.Absorb:
                nameplateCombatText[num].m_combatText.text = text;
                c = Color.blue;
                isHit = true;
                break;
            case CombatTextCategory.TP_Damage:
                nameplateCombatText[num].m_combatText.text = text;
                c = Color.magenta;
                isHit = true;
                break;
            case CombatTextCategory.TP_Recovery:
            {
                nameplateCombatText[num].m_combatText.text = text;
                c = Color.yellow * 0.7f;
                isHit = true;
                break;
            }
        }

        Image combatTextCover = nameplateCombatText[num].m_combatTextCover;

        UIManager.SetGameObjectActive(combatTextCover, isCover && m_showCombatTextIcons);
        if (nameplateCombatText[num].m_iconImage != null && HUD_UIResources.Get() != null)
        {
            Sprite combatTextIconSprite = HUD_UIResources.Get().GetCombatTextIconSprite(icon);
            if (combatTextIconSprite != null && m_showCombatTextIcons)
            {
                nameplateCombatText[num].m_iconImage.sprite = combatTextIconSprite;
                UIManager.SetGameObjectActive(nameplateCombatText[num].m_iconImage, true);
            }
            else
            {
                nameplateCombatText[num].m_iconImage.sprite = null;
                UIManager.SetGameObjectActive(nameplateCombatText[num].m_iconImage, false);
            }
        }

        bool moveToTop = false;
        if (isHit)
        {
            if (nameplateCombatText[num].m_CombatTextController != null)
            {
                nameplateCombatText[num].m_CombatTextController.Play(
                    isMediumHit ? combatTextHitMedium : combatTextHit,
                    0,
                    0f);

                nameplateCombatText[num].m_combatText.color = Color.white;
                nameplateCombatText[num].m_combatText.outlineColor = c;
                moveToTop = true;
            }
        }

        if (moveToTop)
        {
            MoveToTopOfNameplates();
        }

        float scaledCombatTextSize = HUD_UIResources.GetScaledCombatTextSize(number);
        nameplateCombatText[num].m_TextScalar.localScale = new Vector3(
            scaledCombatTextSize,
            scaledCombatTextSize,
            scaledCombatTextSize);
        
        if (GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
        {
            HighlightNameplateForAbility();
            m_setToDimTime = Time.time + 1f;
        }
    }

    private void HideCombatTexts()
    {
        for (int i = 0; i < nameplateCombatText.Length; i++)
        {
            nameplateCombatText[i].m_combatText.text = string.Empty;
            if (nameplateCombatText[i].m_iconImage != null)
            {
                UIManager.SetGameObjectActive(nameplateCombatText[i].m_iconImage, false);
            }

            if (nameplateCombatText[i].m_combatTextCover != null)
            {
                UIManager.SetGameObjectActive(nameplateCombatText[i].m_combatTextCover, false);
            }
        }
    }

    /*
     * Status icon finished fading out.
     */
    public void StatusFadeOutDone(StatusType newType
#if EVOS
        , EvosActorStatusType newEvosStatusType
#endif
        )
    {
#if EVOS
        Log.Info($"UINameplateItem.StatusFadeOutDone: {m_actorData} {newType} {newEvosStatusType}");
        
        HUD_UIResources.StatusTypeIcon iconForStatusType = newType == StatusType.INVALID
            ? EvosActorStatusRepo.GetIconForStatusType(newEvosStatusType)
            : HUD_UIResources.GetIconForStatusType(newType);
#else
                HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
#endif
        if (!iconForStatusType.displayIcon)
        {
            return;
        }

        UINameplateStatus uINameplateStatus = Instantiate(m_statusPrefab);
        uINameplateStatus.transform.SetParent(m_statusContainer.transform);
        uINameplateStatus.transform.localScale = Vector3.one;
        uINameplateStatus.transform.localEulerAngles = Vector3.zero;
        (uINameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2(
            m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt,
            m_currentStatusStackCount * (uINameplateStatus.transform as RectTransform).rect.height);
        Vector3 localPosition = uINameplateStatus.transform.localPosition;
        localPosition.z = 0f;
        uINameplateStatus.transform.localPosition = localPosition;
        uINameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
        uINameplateStatus.m_StatusText.text = "-" + iconForStatusType.popupText;
        float nameplateStatusFadeColorMultiplier = HUD_UIResources.Get().m_nameplateStatusFadeColorMultiplier;
        uINameplateStatus.m_StatusText.color *= nameplateStatusFadeColorMultiplier;
        Color color = uINameplateStatus.m_StatusText.color;
        color.a = 1f;
        uINameplateStatus.m_StatusText.color = color;
        uINameplateStatus.m_StatusIcon.color *= nameplateStatusFadeColorMultiplier;
        color = uINameplateStatus.m_StatusIcon.color;
        color.a = 1f;
        uINameplateStatus.m_StatusIcon.color = color;
        uINameplateStatus.m_StatusText.font = iconForStatusType.isDebuff ? m_debuffFont : m_buffFont;

        uINameplateStatus.DisplayAsLostStatus(this);
        StatusDisplayInfo item;
        item.statusObject = uINameplateStatus;
        item.statusType = newType;
#if EVOS
        item.evosStatusType = newEvosStatusType;
#endif
        m_statusEffectsAnimating.Add(item);
        m_currentStatusStackCount++;
    }

    public void UpdateBriefcaseThreshold(float percent)
    {
        m_blueFillMarker.fillAmount = percent;
        m_redFillMarker.fillAmount = percent;
    }

    public void NotifyFlagStatusChange(bool holdingFlag)
    {
        if (m_isHoldingFlag == holdingFlag)
        {
            return;
        }

        m_isHoldingFlag = holdingFlag;
        if (m_isHoldingFlag)
        {
            UIManager.SetGameObjectActive(m_redBriefcaseContainer, barsToShow == BarColor.Enemy);
            UIManager.SetGameObjectActive(
                m_blueBriefcaseContainer,
                barsToShow == BarColor.Self || barsToShow == BarColor.Team);
        }
        else
        {
            if (m_redBriefcaseContainer.gameObject.activeInHierarchy)
            {
                m_redBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
            }

            if (m_blueBriefcaseContainer.gameObject.activeInHierarchy)
            {
                m_blueBriefcaseContainer.Play("BriefcaseUIDefaultOUT");
            }
        }
    }

    public void AddStatus(StatusType newType)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(newType);
        if (!iconForStatusType.displayIcon)
        {
            return;
        }
  
#if EVOS      
        Log.Info($"UINameplateItem.AddStatus: {m_actorData} {newType}"); // TODO debug
#endif

        UINameplateStatus uINameplateStatus = Instantiate(m_statusPrefab);
        uINameplateStatus.transform.SetParent(m_statusContainer.transform);
        uINameplateStatus.transform.localScale = Vector3.one;
        uINameplateStatus.transform.localEulerAngles = Vector3.zero;
        (uINameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2(
            m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt,
            m_currentStatusStackCount * (uINameplateStatus.transform as RectTransform).rect.height);
        Vector3 localPosition = uINameplateStatus.transform.localPosition;
        localPosition.z = 0f;
        uINameplateStatus.transform.localPosition = localPosition;
        uINameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
        uINameplateStatus.m_StatusText.text = iconForStatusType.popupText;
        uINameplateStatus.m_StatusText.font = iconForStatusType.isDebuff ? m_debuffFont : m_buffFont;

        if (iconForStatusType.isDebuff)
        {
            uINameplateStatus.DisplayAsNegativeStatus(this);
        }
        else
        {
            uINameplateStatus.DisplayAsPositiveStatus(this);
        }

        StatusDisplayInfo item;
        item.statusObject = uINameplateStatus;
        item.statusType = newType;
#if EVOS
        item.evosStatusType = EvosActorStatusType.NONE;
#endif
        m_statusEffectsAnimating.Add(item);
        m_currentStatusStackCount++;
    }

    public void RemoveStatus(StatusType newType)
    {
#if EVOS      
        Log.Info($"UINameplateItem.RemoveStatus: {m_actorData} {newType}"); // TODO debug
#endif
        
        for (int i = 0; i < m_statusEffects.Count; i++)
        {
            StaticStatusDisplayInfo value = m_statusEffects[i];
            if (value.statusType == newType)
            {
                value.m_removedBuff = true;
                m_statusEffects[i] = value;
            }
        }
    }
    
#if EVOS
    public void AddStatus(EvosActorStatusType newType)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        HUD_UIResources.StatusTypeIcon iconForStatusType = EvosActorStatusRepo.GetIconForStatusType(newType);
        if (!iconForStatusType.displayIcon)
        {
            return;
        }
        
        Log.Info($"UINameplateItem.AddStatus: {m_actorData} {newType}");

        UINameplateStatus uINameplateStatus = Instantiate(m_statusPrefab);
        uINameplateStatus.transform.SetParent(m_statusContainer.transform);
        uINameplateStatus.transform.localScale = Vector3.one;
        uINameplateStatus.transform.localEulerAngles = Vector3.zero;
        (uINameplateStatus.transform.transform as RectTransform).anchoredPosition = new Vector2(
            m_currentStatusStackCount * HUD_UIResources.Get().m_nameplateStatusHorizontalShiftAmt,
            m_currentStatusStackCount * (uINameplateStatus.transform as RectTransform).rect.height);
        Vector3 localPosition = uINameplateStatus.transform.localPosition;
        localPosition.z = 0f;
        uINameplateStatus.transform.localPosition = localPosition;
        uINameplateStatus.m_StatusIcon.sprite = iconForStatusType.icon;
        uINameplateStatus.m_StatusText.text = iconForStatusType.popupText;
        uINameplateStatus.m_StatusText.font = iconForStatusType.isDebuff ? m_debuffFont : m_buffFont;

        if (iconForStatusType.isDebuff)
        {
            uINameplateStatus.DisplayAsNegativeStatus(this);
        }
        else
        {
            uINameplateStatus.DisplayAsPositiveStatus(this);
        }

        StatusDisplayInfo item;
        item.statusObject = uINameplateStatus;
        item.statusType = StatusType.INVALID;
        item.evosStatusType = newType;
        m_statusEffectsAnimating.Add(item);
        m_currentStatusStackCount++;
    }

    /**
     * Mark status as pending removal, triggers logic in <see cref="LateUpdate" />
     */
    public bool RemoveStatus(EvosActorStatusType newType)
    {
        Log.Info($"UINameplateItem.RemoveStatus: {m_actorData} {newType}");
        
        for (int i = 0; i < m_statusEffects.Count; i++)
        {
            StaticStatusDisplayInfo value = m_statusEffects[i];
            if (value.evosStatusType == newType && !value.m_removedBuff)
            {
                value.m_removedBuff = true;
                m_statusEffects[i] = value;
                return true;
            }
        }

        return false;
    }
#endif

    public void UpdateStatusDuration(StatusType status, int newDuration)
    {
        foreach (StaticStatusDisplayInfo staticStatusDisplayInfo in m_statusEffects)
        {
            if (staticStatusDisplayInfo.statusType == status)
            {
                staticStatusDisplayInfo.statusObject.Setup(status, newDuration);
                return;
            }
        }
    }

    public void SetDebugText(string text)
    {
        m_debugText.text = text;
    }

    public void UpdateTargetingAbilityIndicator(Ability ability, AbilityData.ActionType action, int index)
    {
        if (index < c_maxNumTargetingAbilityIndicators)
        {
            while (m_targetingAbilityIndicators.Count <= index)
            {
                GameObject indicatorObject = Instantiate(m_targetingAbilityIndicatorPrefab);
                UITargetingAbilityIndicator component = indicatorObject.GetComponent<UITargetingAbilityIndicator>();
                component.transform.SetParent(m_targetingAbilityIconsGrid.transform);
                component.transform.localScale = Vector3.one;
                component.transform.localPosition = Vector3.zero;
                component.transform.localEulerAngles = Vector3.zero;
                m_targetingAbilityIndicators.Add(component);
            }

            m_targetingAbilityIndicators[index].Setup(m_actorData, ability, action);
            if (!m_targetingAbilityIndicators[index].gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_targetingAbilityIndicators[index], true);
            }
        }

        if (m_catalsystPips == null)
        {
            m_catalsystPips = Instantiate(m_catalystIndicatorPrefab);
            m_catalsystPips.transform.SetParent(m_targetingAbilityIconsGrid.transform);
            m_catalsystPips.transform.localScale = Vector3.one;
            m_catalsystPips.transform.localPosition = Vector3.zero;
            m_catalsystPips.transform.localEulerAngles = Vector3.zero;
        }

        UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
        m_actorData.GetAbilityData().UpdateCatalystDisplay();
    }

    public void TurnOffTargetingAbilityIndicator(int fromIndex)
    {
        for (int i = fromIndex; i < m_targetingAbilityIndicators.Count; i++)
        {
            if (m_targetingAbilityIndicators[i].gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
            }
        }

        if (fromIndex == 0)
        {
            SetCatalystsVisible(false);
        }
    }

    public void SpawnOvercon(UIOverconData.NameToOverconEntry entry, bool skipValidation)
    {
        if (m_overconParent == null || m_overconPrefab == null)
        {
            return;
        }

        GameWideData gameWideData = GameWideData.Get();
        if (m_overcon != null)
        {
            Destroy(m_overcon.gameObject);
            m_overcon = null;
        }

        if (GameFlowData.Get().CurrentTurn > m_lastTurnOverconUsed)
        {
            m_numOverconUsesThisTurn = 0;
            m_lastTurnOverconUsed = GameFlowData.Get().CurrentTurn;
        }

        bool isLimited = true;
        string term = null;
        int num = 0;
        if (skipValidation)
        {
            isLimited = false;
        }
        else if (m_numOverconUsesThisTurn >= gameWideData.NumOverconsPerTurn)
        {
            if (m_actorData.isLocalPlayer)
            {
                term = "OverconLimitPerTurn";
                num = gameWideData.NumOverconsPerTurn;
            }
        }
        else if (m_numOverconUsesThisMatch >= gameWideData.NumOverconsPerMatch)
        {
            if (m_actorData.isLocalPlayer)
            {
                term = "OverconLimitPerMatch";
                num = gameWideData.NumOverconsPerMatch;
            }
        }
        else if (m_numOverconUsesById[entry.m_overconId] >= entry.m_maxUsesPerMatch)
        {
            if (m_actorData.isLocalPlayer)
            {
                term = "SpecificOverconLimitPerMatch";
                num = entry.m_maxUsesPerMatch;
            }
        }
        else
        {
            isLimited = false;
        }

        if (isLimited)
        {
            if (m_actorData.isLocalPlayer)
            {
                TextConsole.Message message = default(TextConsole.Message);
                message.MessageType = ConsoleMessageType.SystemMessage;
                message.Text = string.Format(StringUtil.TR(term, "HUDScene"), num);
                TextConsole.AllowedEmojis allowedEmojis;
                allowedEmojis.emojis = new List<int>();
                HUD_UI.Get().m_textConsole.HandleMessage(message, allowedEmojis);
            }

            return;
        }

        GameObject overconObject = Instantiate(m_overconPrefab);
        m_overcon = overconObject.GetComponent<UINameplateOvercon>();
        if (m_overcon != null)
        {
            if (!skipValidation)
            {
                m_numOverconUsesThisMatch++;
                m_numOverconUsesThisTurn++;
                m_numOverconUsesById[entry.m_overconId]++;
            }

            m_overcon.Initialize(m_actorData, entry);
            m_overcon.transform.SetParent(m_overconParent.transform);
            m_overcon.transform.localPosition = Vector3.zero;
            m_overcon.transform.localScale = Vector3.one;
            if (string.IsNullOrEmpty(entry.m_audioEvent))
            {
                UIFrontEnd.PlaySound(FrontEndButtonSounds.OverconUsed);
            }
            else
            {
                AudioManager.PostEvent(entry.m_audioEvent);
            }
        }
    }

    public void UpdateSelfNameplate(Ability abilityTargeting, bool inCover, int currentTargeterIndex, bool inConfirm)
    {
        UIManager.SetGameObjectActive(m_coverSymbol, false);
        int baseGainOnCast = 0;
        int baseGainOnDamageOncePerCast = 0;
        int baseGainOnHitOncePerCast = 0;
        int baseGainOnDamagePerTarget = 0;
        int baseGainOnHitPerTarget = 0;
        int baseGainOnHitPerAllyTarget = 0;
        int baseGainOnHitPerEnemyTarget = 0;
        HashSet<TechPointInteractionType> hashSet = new HashSet<TechPointInteractionType>();
        TechPointInteraction[] baseTechPointInteractions = abilityTargeting.GetBaseTechPointInteractions();
        for (int i = 0; i < baseTechPointInteractions.Length; i++)
        {
            int gainValue = baseTechPointInteractions[i].m_amount;
            TechPointInteractionType type = baseTechPointInteractions[i].m_type;
            if (hashSet.Contains(type))
            {
                continue;
            }

            hashSet.Add(type);
            if (abilityTargeting.CurrentAbilityMod != null)
            {
                gainValue = abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(type, gainValue);
            }

            switch (type)
            {
                case TechPointInteractionType.RewardOnCast:
                    baseGainOnCast += gainValue;
                    break;
                case TechPointInteractionType.RewardOnDamage_OncePerCast:
                    baseGainOnDamageOncePerCast += gainValue;
                    break;
                case TechPointInteractionType.RewardOnHit_OncePerCast:
                    baseGainOnHitOncePerCast += gainValue;
                    break;
                case TechPointInteractionType.RewardOnDamage_PerTarget:
                    baseGainOnDamagePerTarget += gainValue;
                    break;
                case TechPointInteractionType.RewardOnHit_PerTarget:
                    baseGainOnHitPerTarget += gainValue;
                    break;
                case TechPointInteractionType.RewardOnHit_PerAllyTarget:
                    baseGainOnHitPerAllyTarget += gainValue;
                    break;
                case TechPointInteractionType.RewardOnHit_PerEnemyTarget:
                    baseGainOnHitPerEnemyTarget += gainValue;
                    break;
            }
        }

        if (abilityTargeting.CurrentAbilityMod != null)
        {
            foreach (TechPointInteractionMod mod in abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods)
            {
                if (hashSet.Contains(mod.interactionType))
                {
                    continue;
                }

                hashSet.Add(mod.interactionType);
                int moddedGain = 0;
                TechPointInteractionType interactionType = mod.interactionType;
                hashSet.Add(interactionType);
                if (abilityTargeting.CurrentAbilityMod != null)
                {
                    moddedGain = Mathf.Max(
                        0,
                        abilityTargeting.CurrentAbilityMod.GetModdedTechPointForInteraction(interactionType, 0));
                }

                switch (interactionType)
                {
                    case TechPointInteractionType.RewardOnCast:
                        baseGainOnCast += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnDamage_OncePerCast:
                        baseGainOnDamageOncePerCast += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnHit_OncePerCast:
                        baseGainOnHitOncePerCast += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnDamage_PerTarget:
                        baseGainOnDamagePerTarget += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnHit_PerTarget:
                        baseGainOnHitPerTarget += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnHit_PerAllyTarget:
                        baseGainOnHitPerAllyTarget += moddedGain;
                        break;
                    case TechPointInteractionType.RewardOnHit_PerEnemyTarget:
                        baseGainOnHitPerEnemyTarget += moddedGain;
                        break;
                }
            }
        }

        int primaryGain = AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnCast);
        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        int targetCounts = abilityTargeting.GetTargetCounts(
            activeOwnedActorData,
            currentTargeterIndex,
            out int numAlliesExcludingSelf,
            out int numEnemies,
            out _);
        if (baseGainOnDamageOncePerCast > 0 && numEnemies > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnDamageOncePerCast);
        }

        if (baseGainOnDamagePerTarget > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnDamagePerTarget) * numEnemies;
        }

        if (baseGainOnHitOncePerCast > 0 && targetCounts > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnHitOncePerCast);
        }

        if (baseGainOnHitPerTarget > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnHitPerTarget) * targetCounts;
        }

        if (baseGainOnHitPerAllyTarget > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnHitPerAllyTarget) * numAlliesExcludingSelf;
        }

        if (baseGainOnHitPerEnemyTarget > 0)
        {
            primaryGain += AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, baseGainOnHitPerEnemyTarget) * numEnemies;
        }

        int additionalGain = abilityTargeting.GetAdditionalTechPointGainForNameplateItem(m_actorData, currentTargeterIndex);
        if (abilityTargeting.StatusAdjustAdditionalTechPointForTargeting())
        {
            additionalGain = AbilityUtils.CalculateTechPointsForTargeter(m_actorData, abilityTargeting, additionalGain);
        }

        bool fadeOut = inConfirm
                       && Time.time - m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay;
        bool skipModifierIndex = false;
        if (baseTechPointInteractions.Length <= 0
            && (abilityTargeting.CurrentAbilityMod == null
                || abilityTargeting.CurrentAbilityMod.m_techPointInteractionMods.Length <= 0)
            && additionalGain <= 0)
        {
            UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, false);
        }
        else if (inConfirm && primaryGain + additionalGain == 0)
        {
            UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, false);
        }
        else if (m_textVisible)
        {
            UIManager.SetGameObjectActive(abilityModifiers[0].m_abilityModifierText, true);
            abilityModifiers[0].m_targetingTextAnimationController.SetBool("IsOn", true);
            skipModifierIndex = true;
            abilityModifiers[0].m_abilityModifierText.text = (primaryGain + additionalGain).ToString();
            Color targetingNumberColor = GetTargetingNumberColor(
                m_EnergyColor,
                abilityModifiers[0].m_abilityModifierText.color.a,
                fadeOut);
            abilityModifiers[0].m_abilityModifierText.color = targetingNumberColor;
            SetGlowTargeting(true, BarColor.Self);
        }

        SetAbilityModifiersForTargetActor(
            skipModifierIndex ? 1 : 0,
            abilityTargeting,
            currentTargeterIndex,
            false,
            inConfirm,
            out _);
    }

    public void UpdateNameplateTargeted(
        ActorData targetingActor,
        Ability abilityTargeting,
        bool inCover,
        int currentTargeterIndex,
        bool inConfirm)
    {
        int targetedActorValue = 0;
        bool flag = false;
        foreach (ActorData actorData in GameFlowData.Get().GetActors())
        {
            if (actorData == null
                || actorData == targetingActor
                || actorData.GetTeam() != targetingActor.GetTeam())
            {
                continue;
            }
            
            ActorTargeting actorTargeting = actorData.GetActorTargeting();
            if (actorTargeting == null)
            {
                continue;
            }
            
            if (targetingActor.GetTeam() != m_actorData.GetTeam())
            {
                actorTargeting.IsTargetingActor(
                    m_actorData,
                    AbilityTooltipSymbol.Damage,
                    ref targetedActorValue);
            }
            else
            {
                actorTargeting.IsTargetingActor(
                    m_actorData,
                    AbilityTooltipSymbol.Healing,
                    ref targetedActorValue);
                flag = actorTargeting.IsTargetingActor(
                    m_actorData,
                    AbilityTooltipSymbol.Absorb,
                    ref targetedActorValue);
            }
        }

        if (targetedActorValue > 0 && HighlightUtils.Get().m_enableAccumulatedAllyNumbers)
        {
            if (m_textVisible)
            {
                UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, true);
                allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
                allyAbilityModifier.m_abilityModifierText.text = targetedActorValue + " +";
                bool fadeOut = inConfirm && Time.time - m_fadeoutStartTime
                    > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay;
                Color color = targetingActor.GetTeam() == m_actorData.GetTeam()
                    ? flag ? m_AbsorbColor : m_HealingColor
                    : m_DamageColor;
                color = GetTargetingNumberColor(color, allyAbilityModifier.m_abilityModifierText.color.a, fadeOut);
                allyAbilityModifier.m_abilityModifierText.color = color;
            }
        }
        else
        {
            UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
        }

        int numModifiers = SetAbilityModifiersForTargetActor(
            0,
            abilityTargeting,
            currentTargeterIndex,
            inCover,
            inConfirm,
            out bool hasDamage);
        for (int i = numModifiers; i < abilityModifiers.Length; i++)
        {
            UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
        }

        UIManager.SetGameObjectActive(m_coverSymbol, inCover && hasDamage);
        SetGlowTargeting(true, GetRelationshipWithPlayer(m_actorData));
    }

    public void Update()
    {
        if (GameFlowData.Get() == null)
        {
            return;
        }

        if (m_actorData == null)
        {
            Destroy(gameObject);
        }

        if (m_visible)
        {
            if (GameFlowData.Get().activeOwnedActorData != null
                && GameFlowData.Get().activeOwnedActorData.GetAbilityData() != null)
            {
                bool isInteractable = GameFlowData.Get().activeOwnedActorData.GetAbilityData().GetSelectedAbility() == null;
                SetStatusObjectInteractable(isInteractable);
            }
        }
        else
        {
            SetStatusObjectInteractable(false);
        }
    }

    private void SetStatusObjectInteractable(bool interactable)
    {
        for (int i = 0; i < m_statusEffects.Count; i++)
        {
            SetInteractable(m_statusEffects[i].statusObject.m_cGroup, interactable);
        }
    }

    private static void SetInteractable(CanvasGroup canvasGroup, bool interactable)
    {
        if (canvasGroup.blocksRaycasts != interactable)
        {
            canvasGroup.blocksRaycasts = interactable;
        }

        if (canvasGroup.interactable != interactable)
        {
            canvasGroup.interactable = interactable;
        }
    }

    public void SetCatalystsVisible(bool visible)
    {
        if (m_catalsystPips != null)
        {
            UIManager.SetGameObjectActive(m_catalsystPips, visible);
        }
    }

    public void UpdateCatalysts(List<Ability> cardAbilities)
    {
        if (m_catalsystPips == null)
        {
            return;
        }

        bool isActivePrep = false;
        bool isActiveDash = false;
        bool isActiveBlast = false;
        for (int i = 0; i < cardAbilities.Count; i++)
        {
            Ability ability = cardAbilities[i];
            if (ability != null)
            {
                AbilityRunPhase abilityRunPhase = Card.AbilityPriorityToRunPhase(ability.GetRunPriority());
                switch (abilityRunPhase)
                {
                    case AbilityRunPhase.Prep:
                        isActivePrep = true;
                        break;
                    case AbilityRunPhase.Dash:
                        isActiveDash = true;
                        break;
                    case AbilityRunPhase.Combat:
                        isActiveBlast = true;
                        break;
                }
            }
        }

        UIManager.SetGameObjectActive(m_catalsystPips.m_PrepPhaseOn, isActivePrep);
        UIManager.SetGameObjectActive(m_catalsystPips.m_DashPhaseOn, isActiveDash);
        UIManager.SetGameObjectActive(m_catalsystPips.m_BlastPhaseOn, isActiveBlast);
        UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
    }

    public void UpdateNameplateUntargeted(bool doInstantHide = false)
    {
        UIManager.SetGameObjectActive(m_coverSymbol, false);
        for (int i = 0; i < abilityModifiers.Length; i++)
        {
            if (doInstantHide)
            {
                if (abilityModifiers[i].m_abilityModifierText.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
                }
            }
            else if (abilityModifiers[i].m_targetingTextAnimationController.gameObject.activeInHierarchy)
            {
                abilityModifiers[i].m_targetingTextAnimationController.SetTrigger("DoOff");
                abilityModifiers[i].m_targetingTextAnimationController.SetBool("IsOn", false);
            }
        }

        if (doInstantHide)
        {
            if (allyAbilityModifier.m_abilityModifierText.gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(allyAbilityModifier.m_abilityModifierText, false);
            }
        }
        else if (allyAbilityModifier.m_targetingTextAnimationController.gameObject.activeInHierarchy)
        {
            allyAbilityModifier.m_targetingTextAnimationController.SetTrigger("DoOff");
            allyAbilityModifier.m_targetingTextAnimationController.SetBool("IsOn", false);
        }

        SetGlowTargeting(false, BarColor.None);
    }

    private void SetGlowTargeting(bool setGlow, BarColor teamColor)
    {
        UIManager.SetGameObjectActive(m_targetGlow, false);
    }

    private int SetAbilityModifiersForTargetActor(
        int startModifierIndex,
        Ability abilityTargeting,
        int currentTargeterIndex,
        bool inCover,
        bool inConfirm,
        out bool hasDamage)
    {
        hasDamage = false;
        ActorData actorData = GameFlowData.Get() != null ? GameFlowData.Get().activeOwnedActorData : null;
        bool fadeOut = inConfirm
                       && Time.time - m_fadeoutStartTime > HUD_UIResources.Get().m_confirmedTargetingFadeoutStartDelay;
        bool isDamageProcessed = false;
        bool isHealingProcessed = false;
        bool isEnergyProcessed = false;
        bool isAbsorbProcessed = false;
        m_tempTargetingNumSymbolToValueMap.Clear();
        Dictionary<AbilityTooltipSymbol, int> tempTargetingNumSymbolToValueMap = m_tempTargetingNumSymbolToValueMap;
        ActorTargeting.GetNameplateNumbersForTargeter(
            actorData,
            m_actorData,
            abilityTargeting,
            currentTargeterIndex,
            tempTargetingNumSymbolToValueMap);
        int numModifiers = startModifierIndex;
        foreach (KeyValuePair<AbilityTooltipSymbol, int> keyValuePair in tempTargetingNumSymbolToValueMap)
        {
            if (numModifiers < abilityModifiers.Length && m_textVisible)
            {
                AbilityTooltipSymbol key = keyValuePair.Key;
                int value = keyValuePair.Value;
                AbilityModifier abilityModifier = abilityModifiers[numModifiers];
                switch (key)
                {
                    case AbilityTooltipSymbol.Damage:
                        if (value > 0 && !isDamageProcessed)
                        {
                            UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
                            abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
                            string text = value.ToString();
                            string accessoryTargeterNumberString =
                                abilityTargeting.GetAccessoryTargeterNumberString(m_actorData, key, value);
                            if (!string.IsNullOrEmpty(accessoryTargeterNumberString))
                            {
                                text += accessoryTargeterNumberString;
                            }

                            abilityModifier.m_abilityModifierText.text = text;
                            abilityModifier.m_abilityModifierText.color = GetTargetingNumberColor(
                                m_DamageColor,
                                abilityModifier.m_abilityModifierText.color.a,
                                fadeOut);
                            isDamageProcessed = true;
                            hasDamage = true;
                            numModifiers++;
                        }

                        break;
                    case AbilityTooltipSymbol.Healing:
                        if (value > 0 && !isHealingProcessed)
                        {
                            UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
                            abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
                            abilityModifier.m_abilityModifierText.text = value.ToString();
                            abilityModifier.m_abilityModifierText.color = GetTargetingNumberColor(
                                m_HealingColor,
                                abilityModifier.m_abilityModifierText.color.a,
                                fadeOut);
                            isHealingProcessed = true;
                            numModifiers++;
                        }

                        break;
                    case AbilityTooltipSymbol.Absorb:
                        if (!isAbsorbProcessed)
                        {
                            UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
                            abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
                            abilityModifier.m_abilityModifierText.text = value.ToString();
                            abilityModifier.m_abilityModifierText.color = GetTargetingNumberColor(
                                m_AbsorbColor,
                                abilityModifier.m_abilityModifierText.color.a,
                                fadeOut);
                            isAbsorbProcessed = true;
                            numModifiers++;
                        }

                        break;
                    case AbilityTooltipSymbol.Energy:
                        if (actorData != m_actorData && !isEnergyProcessed)
                        {
                            UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, true);
                            abilityModifier.m_targetingTextAnimationController.SetBool("IsOn", true);
                            abilityModifier.m_abilityModifierText.text = value.ToString();
                            abilityModifier.m_abilityModifierText.color = GetTargetingNumberColor(
                                m_EnergyColor,
                                abilityModifier.m_abilityModifierText.color.a,
                                fadeOut);
                            isEnergyProcessed = true;
                            numModifiers++;
                        }

                        break;
                    default:
                        UIManager.SetGameObjectActive(abilityModifier.m_abilityModifierText, false);
                        break;
                }
            }
        }

        for (int i = numModifiers; i < abilityModifiers.Length; i++)
        {
            UIManager.SetGameObjectActive(abilityModifiers[i].m_abilityModifierText, false);
        }

        return numModifiers;
    }

    private Color GetTargetingNumberColor(Color fullColor, float alphaNow, bool fadeOut)
    {
        if (fadeOut)
        {
            Color result = fullColor;
            result.a = Mathf.Max(
                0f,
                alphaNow - Time.deltaTime * HUD_UIResources.Get().m_confirmedTargetingFadeoutSpeed);
            return result;
        }

        return fullColor;
    }

    public void SetBarColors(BarColor newBarcolor)
    {
        if (barsToShow == newBarcolor)
        {
            return;
        }

        barsToShow = newBarcolor;
        lastBarColor = newBarcolor;
        switch (barsToShow)
        {
            case BarColor.Self:
                SetTeamBars(m_selfBars, true);
                SetTeamBars(m_teamBars, false);
                SetTeamBars(m_enemyBars, false);
                break;
            case BarColor.Team:
                SetTeamBars(m_selfBars, false);
                SetTeamBars(m_teamBars, true);
                SetTeamBars(m_enemyBars, false);
                break;
            case BarColor.Enemy:
                SetTeamBars(m_selfBars, false);
                SetTeamBars(m_teamBars, false);
                SetTeamBars(m_enemyBars, true);
                break;
            default:
                SetTeamBars(m_selfBars, false);
                SetTeamBars(m_teamBars, false);
                SetTeamBars(m_enemyBars, false);
                break;
        }
    }

    private bool IsInFrontOfCamera()
    {
        bool result = false;
        if (m_actorData != null)
        {
            Vector3 vector = m_actorData.GetOverheadPosition(30f);
            result = Camera.main.WorldToViewportPoint(vector).z >= 0f;
        }

        return result;
    }

    private bool IsVisibleInDecisionPhase()
    {
        bool result = false;
        if (m_actorData != null)
        {
            result = m_actorData.IsNameplateVisible() && IsInFrontOfCamera();
        }

        return result;
    }

    public void ForceFinishStatusAnims()
    {
        UINameplateStatus[] componentsInChildren = m_statusContainer.GetComponentsInChildren<UINameplateStatus>();
        foreach (UINameplateStatus nameplateStatus in componentsInChildren)
        {
            nameplateStatus.AnimDone();
        }
    }

    private void SetVisible(bool visible)
    {
        if (!m_visible && visible)
        {
            ForceFinishStatusAnims();
        }

        if (m_visible && !visible)
        {
            HideCombatTexts();
        }

        m_visible = visible;
        if (visible)
        {
            SetAlpha(m_alphaToUse > 0f ? m_alphaToUse : 1f);
        }
        else
        {
            SetAlpha(0f);
        }

        if (!m_visible)
        {
            m_mouseIsOverHP = false;
        }

        foreach (UITargetingAbilityIndicator uitargetingAbilityIndicator in m_targetingAbilityIndicators)
        {
            uitargetingAbilityIndicator.SetCanvasGroupVisibility(m_visible);
        }
    }

    private void SetAlpha(float alpha)
    {
        m_alphaToUse = alpha;
        if (m_visible)
        {
            m_canvasGroup.alpha = m_alphaToUse;
            m_abilityPreviewCanvasGroup.alpha = m_alphaToUse;
        }
        else
        {
            m_canvasGroup.alpha = 0f;
            m_abilityPreviewCanvasGroup.alpha = 0f;
        }

        SetInteractable(m_abilityPreviewCanvasGroup, m_abilityPreviewCanvasGroup.alpha > 0f);
    }

    /**
     * Nameplate popup for gaining/losing status finished playing.
     */
    public void NotifyStatusAnimationDone(UINameplateStatus nameplateStatus, bool gainedStatus)
    {
        for (int i = 0; i < m_statusEffectsAnimating.Count; i++)
        {
            StatusDisplayInfo animatingEffect = m_statusEffectsAnimating[i];
            if (animatingEffect.statusObject != nameplateStatus)
            {
                continue;
            }

            if (gainedStatus)
            {
                UIBuffIndicator uIBuffIndicator = Instantiate(m_buffIndicatorPrefab);
                uIBuffIndicator.transform.SetParent(m_buffIndicatorGrid.transform);
                uIBuffIndicator.transform.localScale = Vector3.one;
                uIBuffIndicator.transform.localPosition = Vector3.zero;
                uIBuffIndicator.transform.localEulerAngles = Vector3.zero;
#if EVOS
                if (animatingEffect.statusType == StatusType.INVALID)
                {
                    uIBuffIndicator.Setup(animatingEffect.evosStatusType);
                }
                else
                {
                    uIBuffIndicator.Setup(
                        animatingEffect.statusType,
                        m_actorData.GetActorStatus().GetDurationOfStatus(animatingEffect.statusType));
                }
#else
                uIBuffIndicator.Setup(
                    animatingEffect.statusType,
                    m_actorData.GetActorStatus().GetDurationOfStatus(animatingEffect.statusType));
#endif
                CanvasGroup component = uIBuffIndicator.gameObject.GetComponent<CanvasGroup>();
                component.alpha = 0f;
                StaticStatusDisplayInfo newEffect;
                newEffect.m_removedBuff = false;
                newEffect.statusObject = uIBuffIndicator;
                newEffect.statusType = animatingEffect.statusType;
#if EVOS
                newEffect.evosStatusType = animatingEffect.evosStatusType;
#endif
#if EVOS
                HUD_UIResources.StatusTypeIcon iconForStatusType = animatingEffect.statusType == StatusType.INVALID
                    ? EvosActorStatusRepo.GetIconForStatusType(animatingEffect.evosStatusType)
                    : HUD_UIResources.GetIconForStatusType(animatingEffect.statusType);
#else
                HUD_UIResources.StatusTypeIcon
                    iconForStatusType = HUD_UIResources.GetIconForStatusType(animatingEffect.statusType);
#endif
                bool isFound = false;
#if EVOS
                if (newEffect.statusType != StatusType.INVALID) // we want duplicates
                {
#endif
                    foreach (StaticStatusDisplayInfo statusEffect in m_statusEffects)
                    {
                        if (!statusEffect.m_removedBuff && statusEffect.statusType == newEffect.statusType)
                        {
                            isFound = true;
                            break;
                        }
                    }
#if EVOS
                }
#endif

                if (!isFound)
                {
                    if (!iconForStatusType.isDebuff)
                    {
                        int index = 0;
                        for (int j = 0; j < m_statusEffects.Count; j++)
                        {
                            // insert after last buff
#if EVOS
                            HUD_UIResources.StatusTypeIcon icon = m_statusEffects[j].statusType == StatusType.INVALID
                                ? EvosActorStatusRepo.GetIconForStatusType(m_statusEffects[j].evosStatusType)
                                : HUD_UIResources.GetIconForStatusType(m_statusEffects[j].statusType);
#else
                            HUD_UIResources.StatusTypeIcon icon =
                                HUD_UIResources.GetIconForStatusType(m_statusEffects[j].statusType);
#endif
                            if (icon.isDebuff)
                            {
                                index = j;
                                break;
                            }
                        }

                        m_statusEffects.Insert(index, newEffect);
                        foreach (StaticStatusDisplayInfo info in m_statusEffects)
                        {
                            info.statusObject.gameObject.transform.SetAsLastSibling();
                        }
                    }
                    else
                    {
                        // insert in the end
                        m_statusEffects.Add(newEffect);
                    }
                }
            }

            m_statusEffectsAnimating.Remove(animatingEffect);
            Destroy(nameplateStatus.gameObject);
            i--;
            
#if EVOS
            if (animatingEffect.statusType == StatusType.INVALID)
            {
                 if (!gainedStatus
                     && (EvosActorStatusManager.Get() == null
                        || EvosActorStatusManager.Get().IsPendingRemoval(m_actorData, animatingEffect.evosStatusType)))
                 {
                     Log.Info($"UINameplateItem.NotifyStatusAnimationDone: Status {animatingEffect.statusType}/{animatingEffect.evosStatusType} fadeout done (removing)"); // TODO debug
                     // we started this chain by calling RemoveStatus, no need to start it again
                     // RemoveStatus(animatingEffect.evosStatusType);
                     // EvosActorStatusManager.Get().PendingRemovalProcessed(m_actorData, animatingEffect.evosStatusType);
                 }
            }
            else
#endif    
            if (!m_actorData.GetActorStatus().HasStatus(animatingEffect.statusType))
            {
                RemoveStatus(animatingEffect.statusType);
            }
        }
    }

    private void SetBarPercentVisual(Image image, float percent)
    {
        m_selfBars.m_currentHPPercent = m_HPEased;
        if (image.fillAmount != percent)
        {
            image.fillAmount = percent;
        }
    }

    private void LateUpdate()
    {
        if (Camera.main == null)
        {
            return;
        }

        if (m_setToDimTime > 0f && Time.time >= m_setToDimTime)
        {
            if (GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
            {
                DimNameplateForAbility();
            }
            else
            {
                m_setToDimTime = -1f;
            }
        }

        bool flag = IsVisibleInDecisionPhase();
        if (m_visible != flag)
        {
            SetVisible(flag);
        }

        if (GameFlowData.Get() != null && GameFlowData.Get().LocalPlayerData != null && m_actorData != null)
        {
            ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
            if (activeOwnedActorData != null)
            {
                if (activeOwnedActorData == m_actorData)
                {
                    SetBarColors(BarColor.Self);
                }
                else if (activeOwnedActorData.GetTeam() == m_actorData.GetTeam())
                {
                    SetBarColors(BarColor.Team);
                }
                else if (activeOwnedActorData.GetTeam() == m_actorData.GetTeam().OtherTeam())
                {
                    SetBarColors(BarColor.Enemy);
                }
                else
                {
                    SetBarColors(BarColor.None);
                }
            }
            else if (m_actorData.GetTeam() == Team.TeamA)
            {
                SetBarColors(BarColor.Team);
            }
            else
            {
                SetBarColors(BarColor.Enemy);
            }
        }

        if (lastBarColor != barsToShow)
        {
            SetBarColors(barsToShow);
        }

        if (m_actorData != null)
        {
            if (myCanvas == null)
            {
                myCanvas = HUD_UI.Get().GetTopLevelCanvas();
            }

            if (myCanvas != null && CanvasRect == null)
            {
                CanvasRect = myCanvas.transform as RectTransform;
            }

            Vector3 vector;
            if (m_actorData.IsNameplateVisible())
            {
                vector = m_actorData.GetOverheadPosition(-2f);
            }
            else if (m_actorData.IsDead())
            {
                vector = m_actorData.LastDeathPosition;
            }
            else if (m_actorData.IsActorVisibleToClient())
            {
                vector = m_actorData.GetOverheadPosition(-2f);
            }
            else
            {
                vector = m_actorData.GetClientLastKnownPosVec();
            }

            Vector2 vector2 = Camera.main.WorldToViewportPoint(vector);
            Vector2 anchoredPosition = new Vector2(
                vector2.x * CanvasRect.sizeDelta.x,
                vector2.y * CanvasRect.sizeDelta.y);
            (gameObject.transform as RectTransform).anchoredPosition = anchoredPosition;
            Vector3 position = Camera.main.transform.position;
            m_distanceFromCamera = (vector - position).sqrMagnitude;
            OnHitUpdate();
            int hitPointsToDisplay = m_actorData.GetHitPointsToDisplay();
            int hoTTotalToDisplay = m_actorData.GetHoTTotalToDisplay();
            if (hitPointsToDisplay == 0)
            {
                if (!m_zeroHealthIcon.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(m_zeroHealthIcon, true);
                }

                if (m_healthLabel.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(m_healthLabel, false);
                }
            }
            else
            {
                if (m_zeroHealthIcon.gameObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(m_zeroHealthIcon, false);
                }

                if (m_healthLabel.gameObject.activeSelf != m_textVisible)
                {
                    UIManager.SetGameObjectActive(m_healthLabel, m_textVisible);
                }
            }

            if (!ReferenceEquals(m_textNameLabel.text, m_actorData.GetDisplayName()))
            {
                m_textNameLabel.text = m_actorData.GetDisplayName();
            }

            int num = m_actorData.GetShieldPoints();
            if (num > 0)
            {
                m_mouseOverHitBoxCanvasGroup.ignoreParentGroups =
                    m_actorData.GetAbilityData().GetSelectedAbility() == null;
                if (m_mouseIsOverHP)
                {
                    if (hoTTotalToDisplay > 0)
                    {
                        m_healthLabel.text =
                            $"{hitPointsToDisplay.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{num.ToString()}</color> + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor)}>{hoTTotalToDisplay.ToString()}</color>";
                    }
                    else
                    {
                        m_healthLabel.text =
                            $"{hitPointsToDisplay.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{num.ToString()}</color>";
                    }
                }
                else
                {
                    m_healthLabel.text =
                        $"<color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextShieldColor)}>{(hitPointsToDisplay + num).ToString()}</color>";
                }
            }
            else if (hoTTotalToDisplay > 0)
            {
                m_mouseOverHitBoxCanvasGroup.ignoreParentGroups =
                    m_actorData.GetAbilityData().GetSelectedAbility() == null;
                if (m_mouseIsOverHP)
                {
                    m_healthLabel.text =
                        $"{hitPointsToDisplay.ToString()} + <color={HUD_UIResources.ColorToHex(HUD_UIResources.Get().m_nameplateHealthTextHotColor)}>{hoTTotalToDisplay.ToString()}</color>";
                }
                else
                {
                    m_healthLabel.color = Color.white;
                    string text = hitPointsToDisplay.ToString();
                    if (!m_healthLabel.text.Equals(text))
                    {
                        m_healthLabel.text = text;
                    }
                }
            }
            else
            {
                m_mouseOverHitBoxCanvasGroup.ignoreParentGroups = false;
                m_healthLabel.color = Color.white;
                string healthLabelText = hitPointsToDisplay.ToString();
                if (!m_healthLabel.text.Equals(healthLabelText))
                {
                    m_healthLabel.text = healthLabelText;
                }
            }

            int energyToDisplay = m_actorData.GetTechPointsToDisplay();
            int actualMaxTechPoints = m_actorData.GetMaxTechPoints();
            if (energyToDisplay != m_previousTP || actualMaxTechPoints != m_previousTPMax)
            {
                float endValue = 0f;
                if (actualMaxTechPoints != 0)
                {
                    endValue = energyToDisplay / (float)actualMaxTechPoints;
                }

                if (energyToDisplay > m_previousTP)
                {
                    m_TPEased.EaseTo(endValue, 0f);
                    m_TPEasedGained.EaseTo(endValue, 2.5f);
                }
                else
                {
                    m_TPEasedGained.EaseTo(endValue, 0f);
                    m_TPEased.EaseTo(endValue, 2.5f);
                }

                List<Ability> abilitiesAsList = m_actorData.GetAbilityData().GetAbilitiesAsList();
                bool isUltimateAvailable = false;
                if (abilitiesAsList.Count > 4 && abilitiesAsList[4] != null)
                {
                    isUltimateAvailable = energyToDisplay >= abilitiesAsList[4].GetModdedCost()
                            && !m_actorData.IsDead() 
                            && m_actorData.GetAbilityData().GetCooldownRemaining(AbilityData.ActionType.ABILITY_4) <= 0;
                }

                bool useFullEnergyColor = energyToDisplay == actualMaxTechPoints || isUltimateAvailable;
                m_tpGainBar.color = useFullEnergyColor ? m_FullEnergyColor : m_NonFullEnergyColor;
                if (useFullEnergyColor != m_isMaxEnergy)
                {
                    m_maxEnergyAnimator.GetComponent<_DisableGameObjectOnAnimationDoneEvent>().enabled = !useFullEnergyColor;
                    if (useFullEnergyColor)
                    {
                        UIManager.SetGameObjectActive(m_maxEnergyContainer, true);
                        if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == m_actorData)
                        {
                            UIFrontEnd.PlaySound(FrontEndButtonSounds.MaxEnergyReached);
                        }
                    }

                    m_maxEnergyAnimator.Play("NameplateMaxEnergyDefault" + (useFullEnergyColor ? "IN" : "OUT"));
                    m_isMaxEnergy = useFullEnergyColor;
                }
            }

            if (m_TPEased != m_tpGainedEasePercent)
            {
                m_tpGainedEasePercent = m_TPEased;
                SetBarPercentVisual(m_tpGainEaseBar, ModifyTPBarPercentForArt(m_tpGainedEasePercent));
            }

            if (m_TPEasedGained != m_tpGainedPercent)
            {
                m_tpGainedPercent = m_TPEasedGained;
                SetBarPercentVisual(m_tpGainBar, ModifyTPBarPercentForArt(m_tpGainedPercent));
            }

            if (m_shieldBar != null)
            {
                if (m_ShieldEased.EndValue() <= m_HPEased.EndValue())
                {
                    SetBarPercentVisual(m_shieldBar, 0f);
                }
                else
                {
                    SetBarPercentVisual(m_shieldBar, ModifyHPBarPercentForArt(m_shieldPercent));
                    m_shieldPercent = m_ShieldEased;
                }
            }

            if (m_hpGainBar != null)
            {
                SetBarPercentVisual(m_hpGainBar, ModifyHPBarPercentForArt(m_hpGainedPercent));
                m_hpGainedPercent = m_HPGainedEased;
                if (m_mouseIsOverHP)
                {
                    Color color2 = m_hpGainBar.color;
                    if (m_hpGainedIsFading)
                    {
                        color2.a -= 0.05f;
                        if (color2.a <= 0f)
                        {
                            color2.a = 0f;
                            m_hpGainedIsFading = !m_hpGainedIsFading;
                        }
                    }
                    else
                    {
                        color2.a += 0.05f;
                        if (color2.a >= 1f)
                        {
                            color2.a = 1f;
                            m_hpGainedIsFading = !m_hpGainedIsFading;
                        }
                    }

                    m_hpGainBar.color = color2;
                }
                else
                {
                    m_hpGainBar.color = Color.white;
                }
            }

            BarColor barColor = barsToShow;
            switch (barColor)
            {
                case BarColor.Self:
                {
                    if (m_selfBars.m_currentHPBar != null)
                    {
                        SetBarPercentVisual(m_selfBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
                        m_selfBars.m_currentHPPercent = m_HPEased;
                    }

                    if (m_selfBars.m_damageEasedBar != null)
                    {
                        SetBarPercentVisual(m_selfBars.m_damageEasedBar, ModifyHPBarPercentForArt(m_HPDamageEased));
                        m_selfBars.m_damageEasePercent = m_HPDamageEased;
                    }

                    break;
                }
                case BarColor.Team:
                {
                    if (m_teamBars.m_currentHPBar != null)
                    {
                        SetBarPercentVisual(m_teamBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
                        m_teamBars.m_currentHPPercent = m_HPEased;
                    }

                    if (m_teamBars.m_damageEasedBar != null)
                    {
                        SetBarPercentVisual(m_teamBars.m_damageEasedBar, ModifyHPBarPercentForArt(m_HPDamageEased));
                        m_teamBars.m_damageEasePercent = m_HPDamageEased;
                    }

                    break;
                }
                case BarColor.Enemy:
                {
                    if (m_enemyBars.m_currentHPBar != null)
                    {
                        SetBarPercentVisual(m_enemyBars.m_currentHPBar, ModifyHPBarPercentForArt(m_HPEased));
                        m_enemyBars.m_currentHPPercent = m_HPEased;
                    }

                    if (m_enemyBars.m_damageEasedBar != null)
                    {
                        SetBarPercentVisual(
                            m_enemyBars.m_damageEasedBar,
                            ModifyHPBarPercentForArt(m_HPDamageEased));
                        m_enemyBars.m_damageEasePercent = m_HPDamageEased;
                    }

                    break;
                }
            }

            if (m_statusEffectsAnimating.Count == 0)
            {
                m_currentStatusStackCount = 0;
            }

            for (int i = 0; i < m_statusEffects.Count; i++)
            {
                CanvasGroup component = m_statusEffects[i].statusObject.gameObject.GetComponent<CanvasGroup>();
                if (component == null)
                {
                    continue;
                }

                if (m_visible)
                {
                    if (!m_statusEffects[i].m_removedBuff)
                    {
                        component.alpha += Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
                    }
                    else
                    {
                        component.alpha -= Time.deltaTime * HUD_UIResources.Get().m_nameplateStaticStatusFadeSpeed;
                    }

                    if (component.alpha <= 0f)
                    {
#if EVOS
                        Log.Info($"UINameplateItem.LateUpdate: Status {m_statusEffects[i].statusType}/{m_statusEffects[i].evosStatusType} fadeout done (visible)"); // TODO debug
#endif
                        StatusFadeOutDone(m_statusEffects[i].statusType
#if EVOS
                            , m_statusEffects[i].evosStatusType
#endif
                        );
                        Destroy(m_statusEffects[i].statusObject.gameObject);
                        m_statusEffects.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    component.alpha = 0f;
                    if (m_statusEffects[i].m_removedBuff)
                    {
#if EVOS
                        Log.Info($"UINameplateItem.LateUpdate: Status {m_statusEffects[i].statusType}/{m_statusEffects[i].evosStatusType} fadeout done (invisible)"); // TODO debug
#endif
                        StatusFadeOutDone(m_statusEffects[i].statusType
#if EVOS
                            , m_statusEffects[i].evosStatusType
#endif
                        );
                        Destroy(m_statusEffects[i].statusObject.gameObject);
                        m_statusEffects.RemoveAt(i);
                        i--;
                    }
#if EVOS
                    // seems legal in fog of war
                    // else if (m_statusEffects[i].evosStatusType != EvosActorStatusType.NONE)
                    // {
                    //     Log.Error($"UINameplateItem.LateUpdate: Status {m_statusEffects[i].statusType}/{m_statusEffects[i].evosStatusType} fadeout done but it is not removed");
                    // }
#endif
                }
            }
        }
    }

    private float ModifyHPBarPercentForArt(float actualPct)
    {
        return actualPct;
    }

    private float ModifyTPBarPercentForArt(float actualPct)
    {
        return actualPct;
    }

    private void SnapBarValues()
    {
        m_previousHP = m_actorData.GetHitPointsToDisplay();
        m_previousHPMax = m_actorData.GetMaxHitPoints();
        m_previousShieldValue = m_actorData.GetShieldPoints();
        m_previousHPShieldAndHot = m_previousHP + m_previousShieldValue + m_actorData.GetHoTTotalToDisplay();
        SetMaxHPWithShield(m_previousHPMax + m_previousShieldValue);
        int num = m_previousHPMax + m_previousShieldValue;
        float duration = 0f;
        float endValue = num != 0 ? m_previousHP / (float)num : 0f;
        m_HPDamageEased.EaseTo(endValue, duration);
        m_HPGainedEased.EaseTo(m_previousHPShieldAndHot / (float)num, duration);
        m_HPEased.EaseTo(endValue, duration);
        if (m_previousShieldValue > 0)
        {
            m_ShieldEased.EaseTo((m_previousHP + (float)m_previousShieldValue) / num, duration);
        }
        else
        {
            m_ShieldEased.EaseTo(0f, duration);
        }
    }

    public void OnHitUpdate()
    {
        if (m_actorData == null)
        {
            return;
        }

        int absorb = m_actorData.GetShieldPoints();
        int hitPointsToDisplay = m_actorData.GetHitPointsToDisplay();
        int hitPoints = m_actorData.HitPoints;
        int hoTTotalToDisplay = m_actorData.GetHoTTotalToDisplay();
        int totalPoints = hitPointsToDisplay + absorb + hoTTotalToDisplay;
        float num3 = hitPointsToDisplay / (float)m_maxHPWithShield;
        float num4 = m_HPEased.EndValue();
        float endValue = (hitPointsToDisplay + (float)absorb) / m_maxHPWithShield;
        float endValue2 = totalPoints / (float)m_maxHPWithShield;
        if (hitPointsToDisplay < m_previousHP)
        {
            m_lostHealth = true;
            m_ShieldEased.EaseTo(endValue, 0f);
            float num5 = Mathf.Abs(num3 - m_HPEased.EndValue());
            m_HPEased.EaseTo(m_HPEased.EndValue() - num5, 0f);
            num5 = Mathf.Abs(num3 - m_HPDamageEased.EndValue());
            m_HPDamageEased.EaseTo(m_HPDamageEased.EndValue() - num5, 2.5f);
            m_previousHP = hitPointsToDisplay;
        }
        else if (m_previousHP < hitPointsToDisplay)
        {
            m_lostHealth = false;
            float num6 = Mathf.Abs(num3 - num4);
            if (m_previousShieldValue != 0)
            {
                m_ShieldEased.EaseTo(endValue, 2.5f);
            }

            m_HPEased.EaseTo(m_HPEased.EndValue() + num6, 2.5f);
            m_HPDamageEased.EaseTo(m_HPDamageEased.EndValue() + num6, 2.5f);
            m_previousHP = hitPointsToDisplay;
        }

        if (m_previousShieldValue < absorb)
        {
            SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + absorb);
            float endValue3 = (hitPointsToDisplay + (float)absorb) / m_maxHPWithShield;
            float endValue4 = hitPointsToDisplay / (float)m_maxHPWithShield;
            m_ShieldEased.EaseTo(endValue3, 0f);
            m_HPEased.EaseTo(endValue4, 0f);
            m_HPDamageEased.EaseTo(endValue3, 1f);
            m_previousShieldValue = absorb;
        }
        else if (absorb < m_previousShieldValue && !m_lostHealth)
        {
            SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + absorb);
            float endValue5 = hitPointsToDisplay / (float)m_maxHPWithShield;
            float endValue6 = (hitPointsToDisplay + (float)absorb) / m_maxHPWithShield;
            m_ShieldEased.EaseTo(endValue6, 0f);
            m_HPEased.EaseTo(endValue5, 0f);
            m_HPDamageEased.EaseTo(endValue6, 0f);
            m_previousShieldValue = absorb;
        }

        if (totalPoints != m_previousHPShieldAndHot)
        {
            m_HPGainedEased.EaseTo(endValue2, 0f);
            m_previousHPShieldAndHot = totalPoints;
        }

        if (m_previousResolvedHP != hitPoints)
        {
            m_previousResolvedHP = hitPoints;
            OnResolvedHitPoints();
        }
    }

    public void MoveToTopOfNameplates()
    {
        UIUtils.SetAsLastSiblingIfNeeded(transform);
    }

    public void OnResolvedHitPoints()
    {
        if (m_actorData == null)
        {
            return;
        }

        m_lostHealth = false;
        int num = m_actorData.GetShieldPoints();
        int hitPointsAfterResolution = m_actorData.GetHitPointsToDisplay();
        SetMaxHPWithShield(m_actorData.GetMaxHitPoints() + num);
        float endValue = hitPointsAfterResolution / (float)m_maxHPWithShield;
        float num2 = (hitPointsAfterResolution + (float)num) / m_maxHPWithShield;
        float num3 = (float)m_actorData.GetHoTTotalToDisplay() / m_maxHPWithShield;
        m_ShieldEased.EaseTo(num2, 2.5f);
        m_HPEased.EaseTo(endValue, 2.5f);
        m_HPGainedEased.EaseTo(num2 + num3, 2.5f);
        m_HPDamageEased.EaseTo(num2, 2.5f);
    }

    public void Setup(ActorData actorData)
    {
        m_actorData = actorData;
        m_actorData.m_onResolvedHitPoints = (ActorData.ActorDataDelegate)Delegate.Combine(
            m_actorData.m_onResolvedHitPoints,
            new ActorData.ActorDataDelegate(OnResolvedHitPoints));
        m_textNameLabel.text = m_actorData.GetDisplayName();
        SnapBarValues();
    }

    public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
    {
        if (m_actorData == null)
        {
            return;
        }

        switch (eventType)
        {
            case GameEventManager.EventType.TheatricsAbilityHighlightStart:
            {
                GameEventManager.TheatricsAbilityHighlightStartArgs theatricsAbilityHighlightStartArgs =
                    (GameEventManager.TheatricsAbilityHighlightStartArgs)args;
                if (!theatricsAbilityHighlightStartArgs.m_casters.Contains(m_actorData)
                    && !theatricsAbilityHighlightStartArgs.m_targets.Contains(m_actorData))
                {
                    DimNameplateForAbility();
                }
                else
                {
                    HighlightNameplateForAbility();
                }

                break;
            }
            case GameEventManager.EventType.TheatricsAbilitiesEnd:
                return;
            case GameEventManager.EventType.TurnTick:
                m_setToDimTime = -1f;
                SetAlpha(1f);
                transform.localScale = Vector3.one;
                break;
            case GameEventManager.EventType.ClientResolutionStarted:
                DimNameplateForAbility();
                return;
            case GameEventManager.EventType.NormalMovementStart:
            {
                GameEventManager.NormalMovementStartAgs normalMovementStartAgs =
                    (GameEventManager.NormalMovementStartAgs)args;
                if (normalMovementStartAgs.m_actorsBeingHitMidMovement.Contains(m_actorData))
                {
                    HighlightNameplateForAbility();
                }
                else
                {
                    DimNameplateForAbility();
                }

                return;
            }
        }
    }

    private void HighlightNameplateForAbility()
    {
        SetAlpha(1f);
        transform.localScale = m_scaleWhenHighlighted * Vector3.one;
        m_setToDimTime = -1f;
    }

    private void DimNameplateForAbility()
    {
        SetAlpha(m_alphaWhenOthersHighlighted);
        transform.localScale = m_scaleWhenOthersHighlighted * Vector3.one;
        m_setToDimTime = -1f;
    }
}