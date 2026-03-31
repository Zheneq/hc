using System.Collections.Generic;
#if EVOS
using System.Linq;
using Evos.ActorStatus;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterProfile : MonoBehaviour
{
    public GameObject m_visualObject;
    public Image m_aliveProfileImage;
    public Image m_deadProfileImage;

    public TextMeshProUGUI m_healthText;
    public TextMeshProUGUI m_pendingHealthText;
    public TextMeshProUGUI m_energyText;
    public TextMeshProUGUI m_shieldText;
    public Image m_tutorialEnergyGlow;
    public GameObject m_tutorialEnergyArrows;

    public Image[] m_ggButtonLevelImages;
    public _ButtonSwapSprite m_useGGPackBtn;
    public TextMeshProUGUI m_GGPackCount;
    public Animator m_ggPackBtnAnimator;
    public Image m_ggPackCooldown;

    [Range(0f, 1f)]
    public float m_energyPercent;
    public ImageFilledSloped m_energyImage;
    [Range(0f, 1f)]
    public float m_healthPercent;
    public ImageFilledSloped m_healthImage;
    [Range(0f, 1f)]
    public float m_shieldPercent;
    public ImageFilledSloped m_shieldBarImage;
    [Range(0f, 1f)]
    public float m_pendingHPPercent;
    public ImageFilledSloped m_pendingHPImage;

    public RectTransform m_tauntTransform;
    public Button m_tauntButton;
    public UITauntSelection m_tauntSelectionPanel;

    public LayoutGroup m_buffGrid;
    public LayoutGroup m_debuffGrid;
    public UIBuffIndicator m_buffIndicatorPrefab;

    private float m_lastEnergyPercent = -1f;
    private float m_lastShieldPercent = -1f;
    private float m_lastHealthPercent = -1f;
    private float m_lastPendingHealthPercent = -1f;
    private float m_ggPackTimeLastUsed = -1f;
    private bool m_selectionMenuOpen;
    private bool m_tauntIsEnabled;
    private List<StatusType> previousStatuses;
#if EVOS
    private List<EvosActorStatusType> previousEvosStatuses;
#endif
    private bool m_waitingForGGPackUseResponse;
    private bool m_hasGgPacks;

    private const int GgPacksPerGame = 3;
    private const string ClipGgPackIdle = "HUDggPackIDLE";
    private const string ClipGgPackPress = "HUDggPackPRESS";
    private const string ClipGgPackHover = "HUDggPackHOVER";

    private void Awake()
    {
        for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
        {
            UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == 0);
        }

        UIManager.SetGameObjectActive(m_tutorialEnergyGlow, false);
        UIManager.SetGameObjectActive(m_tutorialEnergyArrows, false);
        previousStatuses = new List<StatusType>();
        if (m_useGGPackBtn != null)
        {
            m_useGGPackBtn.callback = UseGGPackBtnClicked;
            m_useGGPackBtn.GetComponent<UITooltipHoverObject>().Setup(
                TooltipType.Titled,
                tooltip =>
                {
                    if (m_hasGgPacks)
                    {
                        return false;
                    }

                    (tooltip as UITitledTooltip).Setup(
                        StringUtil.TR("OutOfGGBoosts", "Global"),
                        StringUtil.TR("YouAreOutOfGGBoosts", "Global"),
                        string.Empty);
                    return true;
                });
            m_useGGPackBtn.pointerEnterCallback = GGPackMouseOver;
            m_useGGPackBtn.pointerExitCallback = GGPackMouseExit;
        }

        ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
        HandleBankBalanceChange(null);
    }

    private void OnDestroy()
    {
        if (ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
        }
    }

    public void HandleBankBalanceChange(CurrencyData currencyData)
    {
        m_GGPackCount.text = $"x{ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack)}";
    }

    public void Setup()
    {
        GameType gameType = GameManager.Get().GameConfig.GameType;
        int numSelfGgPacksUsed = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
        bool canUseGgPack = GameManager.IsGameTypeValidForGGPack(gameType)
                            && numSelfGgPacksUsed < GgPacksPerGame
                            && !ReplayPlayManager.Get().IsPlayback();
        m_hasGgPacks = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack) > 0;
        m_useGGPackBtn.SetClickable(m_hasGgPacks);
        m_useGGPackBtn.SetForceExitCallback(!m_hasGgPacks);
        m_useGGPackBtn.SetForceHovercallback(!m_hasGgPacks);
        UIManager.SetGameObjectActive(m_ggPackBtnAnimator, canUseGgPack);
        m_waitingForGGPackUseResponse = false;
        if (canUseGgPack)
        {
            m_ggPackBtnAnimator.Play(ClipGgPackIdle);
        }

        for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
        {
            UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == numSelfGgPacksUsed);
        }
    }

    public void NotifyReceivedGGPackResponse()
    {
        m_waitingForGGPackUseResponse = false;
        Setup();
    }

    public void GGPackAnimDone()
    {
        if (!m_waitingForGGPackUseResponse)
        {
            Setup();
        }
    }

    public void GGPackMouseOver(BaseEventData data)
    {
        if (!m_hasGgPacks || !m_ggPackBtnAnimator.isActiveAndEnabled)
        {
            return;
        }

        AnimatorClipInfo[] currentAnimatorClipInfo = m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
        if (currentAnimatorClipInfo != null
            && currentAnimatorClipInfo.Length > 0
            && currentAnimatorClipInfo[0].clip.name != ClipGgPackPress)
        {
            m_ggPackBtnAnimator.Play(ClipGgPackHover);
        }
    }

    public void GGPackMouseExit(BaseEventData data)
    {
        if (!m_hasGgPacks || !m_ggPackBtnAnimator.isActiveAndEnabled)
        {
            return;
        }

        AnimatorClipInfo[] currentAnimatorClipInfo = m_ggPackBtnAnimator.GetCurrentAnimatorClipInfo(0);
        if (currentAnimatorClipInfo != null
            && currentAnimatorClipInfo.Length > 0
            && currentAnimatorClipInfo[0].clip.name != ClipGgPackPress)
        {
            m_ggPackBtnAnimator.Play(ClipGgPackIdle);
        }
    }

    public void UseGGPackBtnClicked(BaseEventData data)
    {
        if (Time.unscaledTime <= m_ggPackTimeLastUsed + GameBalanceVars.Get().GGPackInGameCooldownTimer)
        {
            return;
        }

        m_ggPackTimeLastUsed = Time.unscaledTime;
        m_useGGPackBtn.SetClickable(false);
        m_ggPackBtnAnimator.Play(ClipGgPackPress);
        int numSelfGgPacksUsed = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumSelfGGPacksUsed();
        switch (numSelfGgPacksUsed)
        {
            case 0:
                UIFrontEnd.PlaySound(FrontEndButtonSounds.GGButtonInGameUsed);
                break;
            case 1:
                AudioManager.PostEvent("ui/endgame/ggboost_button_silver");
                break;
            case 2:
                AudioManager.PostEvent("ui/endgame/ggboost_button_gold");
                break;
        }

        if (m_hasGgPacks)
        {
            ClientGameManager.Get().RequestToUseGGPack();
            m_waitingForGGPackUseResponse = true;
        }
    }

    private void Start()
    {
        if (m_tauntButton != null)
        {
            UIEventTriggerUtils.AddListener(
                m_tauntButton.gameObject,
                EventTriggerType.PointerClick,
                OnTauntClick);
        }

        if (m_tauntSelectionPanel.m_closeSelectionButton != null)
        {
            UIEventTriggerUtils.AddListener(
                m_tauntSelectionPanel.m_closeSelectionButton.gameObject,
                EventTriggerType.PointerClick,
                OnCloseSelectionClick);
        }

        m_tauntIsEnabled = true;
        ShowTaunt(false);
    }

    private void UpdatePendingHealthBar()
    {
        if (m_lastPendingHealthPercent == m_pendingHPPercent)
        {
            return;
        }

        if (m_healthPercent > 0f)
        {
            m_pendingHPImage.fillAmount = m_pendingHPPercent;
            UIManager.SetGameObjectActive(m_pendingHPImage, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_pendingHPImage, false);
        }

        m_lastPendingHealthPercent = m_pendingHPPercent;
    }

    private void UpdateHealthBar()
    {
        if (m_lastHealthPercent == m_healthPercent)
        {
            return;
        }

        if (m_healthPercent > 0f)
        {
            m_healthImage.fillAmount = m_healthPercent;
            UIManager.SetGameObjectActive(m_healthImage, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_healthImage, false);
        }

        m_lastHealthPercent = m_healthPercent;
    }

    private void UpdateShieldBar()
    {
        if (m_lastShieldPercent == m_shieldPercent)
        {
            return;
        }

        if (m_shieldPercent > 0f)
        {
            m_shieldBarImage.fillAmount = m_shieldPercent;
            UIManager.SetGameObjectActive(m_shieldBarImage, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_shieldBarImage, false);
        }

        m_lastShieldPercent = m_shieldPercent;
    }

    private void UpdateEnergyBar()
    {
        if (m_lastEnergyPercent == m_energyPercent)
        {
            return;
        }

        if (m_energyPercent > 0f)
        {
            m_energyImage.fillAmount = m_energyPercent;
            UIManager.SetGameObjectActive(m_energyImage, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_energyImage, false);
        }

        m_lastEnergyPercent = m_energyPercent;
    }

    private bool CanTaunt(ActorData actor)
    {
        if (!GameManager.Get().GameplayOverrides.AreTauntsEnabled())
        {
            return false;
        }

        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (activeOwnedActorData == null)
        {
            return false;
        }

        TurnStateEnum currentState = activeOwnedActorData.GetActorTurnSM().CurrentState;
        if (currentState != TurnStateEnum.DECIDING
            && currentState != TurnStateEnum.CONFIRMED
            && currentState != TurnStateEnum.VALIDATING_MOVE_REQUEST
            && currentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
        {
            return false;
        }

        ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
        AbilityData abilityData = activeOwnedActorData.GetAbilityData();

        List<AbilityData.ActionType> autoQueuedRequestActionTypes =
            activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
        foreach (AbilityData.ActionType autoQueuedRequestActionType in autoQueuedRequestActionTypes)
        {
            if (CanTauntForAction(actor, abilityData, component, autoQueuedRequestActionType))
            {
                return true;
            }
        }

        List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo =
            activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
        foreach (ActorTurnSM.ActionRequestForUndo requestForUndo in requestStackForUndo)
        {
            if (CanTauntForAction(actor, abilityData, component, requestForUndo.m_action))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CanTauntForAction(
        ActorData actor,
        AbilityData abilityData,
        ActorCinematicRequests cinematicRequests,
        AbilityData.ActionType actionType)
    {
        Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
        CharacterResourceLink characterResourceLink =
            GameFlowData.Get().activeOwnedActorData.GetCharacterResourceLink();
        PersistedCharacterData playerCharacterData =
            ClientGameManager.Get().GetPlayerCharacterData(actor.m_characterType);

        if (abilityOfActionType == null
            || playerCharacterData == null
            || cinematicRequests.IsAbilityCinematicRequested(actionType))
        {
            return false;
        }

        TauntCameraSet tauntCamSetData = abilityData.GetComponent<ActorData>().m_tauntCamSetData;
        if (tauntCamSetData == null)
        {
            return false;
        }

        for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
        {
            CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
            if (cameraShotSequence != null
                && abilityOfActionType.CanTriggerAnimAtIndexForTaunt(cameraShotSequence.m_animIndex)
                && cinematicRequests.NumRequestsLeft(cameraShotSequence.m_uniqueTauntID) > 0
                && AbilityData.CanTauntForActionTypeForPlayer(
                    playerCharacterData,
                    characterResourceLink,
                    actionType,
                    true,
                    cameraShotSequence.m_uniqueTauntID))
            {
                return true;
            }
        }

        return false;
    }

    private void ShowTauntSelection(bool visible)
    {
        if (!m_tauntIsEnabled)
        {
            return;
        }

        m_selectionMenuOpen = visible;
        UIManager.SetGameObjectActive(m_tauntSelectionPanel, m_selectionMenuOpen);
        int numTauntsAvailable = m_tauntSelectionPanel.SetupTauntList();
        UIManager.SetGameObjectActive(m_tauntTransform, !m_selectionMenuOpen);
        if (visible && numTauntsAvailable == 1)
        {
            m_tauntSelectionPanel.m_tauntButtons[0].SelectedTaunt(null);
        }
    }

    public void OnTauntClick(BaseEventData data)
    {
        ShowTauntSelection(true);
        UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntClick);
    }

    public void OnCloseSelectionClick(BaseEventData data)
    {
        ShowTauntSelection(false);
        UIFrontEnd.PlaySound(FrontEndButtonSounds.InGameTauntSelect);
    }

    public void UpdateStatusDisplay(bool forceUpdate)
    {
        if (m_buffGrid == null || m_debuffGrid == null)
        {
            return;
        }

        ActorStatus actorStatus = GameFlowData.Get().activeOwnedActorData.GetActorStatus();
        List<StatusType> activeStatusTypes = new List<StatusType>();
        bool needsUpdate = false;
        if (actorStatus != null)
        {
            for (int i = 0; i < (int)StatusType.NUM; i++)
            {
                StatusType statusType = (StatusType)i;
                if (!actorStatus.HasStatus(statusType, false))
                {
                    continue;
                }

                HUD_UIResources.StatusTypeIcon iconForStatusType =
                    HUD_UIResources.GetIconForStatusType(statusType);
                if (!iconForStatusType.displayIcon)
                {
                    continue;
                }

                activeStatusTypes.Add(statusType);
                if (previousStatuses != null && !previousStatuses.Contains(statusType))
                {
                    needsUpdate = true;
                }
            }
        }

        if (previousStatuses != null && previousStatuses.Count != activeStatusTypes.Count)
        {
            needsUpdate = true;
        }
        
#if EVOS
        EvosActorStatusManager evosActorStatusManager = EvosActorStatusManager.Get();
        List<EvosActorStatusType> activeEvosStatusTypes = new List<EvosActorStatusType>();
        if (!needsUpdate && evosActorStatusManager != null)
        {
            ActorData actor = GameFlowData.Get().activeOwnedActorData;
            for (int i = 0; i < (int)EvosActorStatusType.NUM; i++)
            {
                EvosActorStatusType statusType = (EvosActorStatusType)i;
                int statusCount = evosActorStatusManager.GetStatusCount(actor, statusType);
                if (statusCount == 0)
                {
                    continue;
                }

                HUD_UIResources.StatusTypeIcon iconForStatusType =
                    EvosActorStatusRepo.GetIconForStatusType(statusType);
                if (!iconForStatusType.displayIcon)
                {
                    continue;
                }

                for (int j = 0; j < statusCount; j++)
                {
                    activeEvosStatusTypes.Add(statusType);
                }
                if (previousEvosStatuses != null && previousEvosStatuses.Count(type => statusType.Equals(type)) != statusCount)
                {
                    needsUpdate = true;
                }
            }
        }

        if (previousEvosStatuses != null && previousEvosStatuses.Count != activeEvosStatusTypes.Count)
        {
            needsUpdate = true;
        }
#endif

        if (!needsUpdate && !forceUpdate)
        {
            return;
        }

        UIBuffIndicator[] buffIndicators = m_buffGrid.GetComponentsInChildren<UIBuffIndicator>(false);
        UIBuffIndicator[] debuffIndicators = m_debuffGrid.GetComponentsInChildren<UIBuffIndicator>(false);

        List<UIBuffIndicator> allIndicators = new List<UIBuffIndicator>(buffIndicators);
        foreach (UIBuffIndicator debuffIndicator in debuffIndicators)
        {
            allIndicators.Add(debuffIndicator);
        }

        while (allIndicators.Count > activeStatusTypes.Count
#if EVOS
               + activeEvosStatusTypes.Count
#endif
               )
        {
            UIManager.SetGameObjectActive(allIndicators[0], false);
            Destroy(allIndicators[0].gameObject);
            allIndicators.RemoveAt(0);
        }

        while (allIndicators.Count < activeStatusTypes.Count
#if EVOS
               + activeEvosStatusTypes.Count
#endif
               )
        {
            UIBuffIndicator item = Instantiate(m_buffIndicatorPrefab);
            allIndicators.Add(item);
        }

        for (int i = 0; i < activeStatusTypes.Count; i++)
        {
            UIBuffIndicator uIBuffIndicator = allIndicators[i];
            HUD_UIResources.StatusTypeIcon icon = HUD_UIResources.GetIconForStatusType(activeStatusTypes[i]);
            uIBuffIndicator.transform.SetParent(icon.isDebuff ? m_debuffGrid.transform : m_buffGrid.transform);
            uIBuffIndicator.transform.localScale = Vector3.one;
            uIBuffIndicator.transform.localPosition = Vector3.zero;
            uIBuffIndicator.transform.localEulerAngles = Vector3.zero;
            allIndicators[i].Setup(activeStatusTypes[i], actorStatus.GetDurationOfStatus(activeStatusTypes[i]));
        }
#if EVOS
        for (int i = 0; i < activeEvosStatusTypes.Count; i++)
        {
            UIBuffIndicator uIBuffIndicator = allIndicators[i + activeStatusTypes.Count];
            HUD_UIResources.StatusTypeIcon icon = EvosActorStatusRepo.GetIconForStatusType(activeEvosStatusTypes[i]);
            uIBuffIndicator.transform.SetParent(icon.isDebuff ? m_debuffGrid.transform : m_buffGrid.transform);
            uIBuffIndicator.transform.localScale = Vector3.one;
            uIBuffIndicator.transform.localPosition = Vector3.zero;
            uIBuffIndicator.transform.localEulerAngles = Vector3.zero;
            uIBuffIndicator.Setup(activeEvosStatusTypes[i]);
        }
#endif

        m_debuffGrid.enabled = false;
        m_debuffGrid.enabled = true;
        m_buffGrid.enabled = false;
        m_buffGrid.enabled = true;
        previousStatuses = activeStatusTypes;
#if EVOS
        previousEvosStatuses = activeEvosStatusTypes;
#endif
    }

    private void ShowTaunt(bool visible)
    {
        if (m_tauntIsEnabled == visible)
        {
            return;
        }

        m_tauntIsEnabled = visible;
        if (!visible)
        {
            m_selectionMenuOpen = false;
            UIManager.SetGameObjectActive(m_tauntSelectionPanel, m_selectionMenuOpen);
        }

        UIManager.SetGameObjectActive(m_tauntTransform, visible);
    }

    private void Update()
    {
        if (GameFlowData.Get() != null)
        {
            ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
            if (activeOwnedActorData != null)
            {
                bool showAliveProfileImage = false;
                bool showDeadProfileImage = false;
                if (m_visualObject != null && !m_visualObject.activeSelf)
                {
                    UIManager.SetGameObjectActive(m_visualObject, true);
                }

                m_aliveProfileImage.sprite = activeOwnedActorData.GetAliveHUDIcon();
                m_deadProfileImage.sprite = activeOwnedActorData.GetDeadHUDIcon();
                if (activeOwnedActorData.IsDead())
                {
                    showDeadProfileImage = true;
                }
                else
                {
                    showAliveProfileImage = true;
                }

                int hitPointsAfterResolution = activeOwnedActorData.GetHitPointsToDisplay();
                int maxHitPoints = activeOwnedActorData.GetMaxHitPoints();
                int energyToDisplay = activeOwnedActorData.GetTechPointsToDisplay();
                int actualMaxTechPoints = activeOwnedActorData.GetMaxTechPoints();
                int shieldPoints = activeOwnedActorData.GetShieldPoints();
                int hoTTotalToDisplay = activeOwnedActorData.GetHoTTotalToDisplay();

                m_pendingHealthText.text = hoTTotalToDisplay > 0 ? "+" + hoTTotalToDisplay : string.Empty;
                m_shieldText.text = shieldPoints > 0 ? "+" + shieldPoints : string.Empty;
                m_healthText.text = hitPointsAfterResolution.ToString();
                m_energyText.text = energyToDisplay.ToString();
                m_healthPercent = (float)hitPointsAfterResolution / (maxHitPoints + shieldPoints);
                m_shieldPercent = (float)(hitPointsAfterResolution + shieldPoints) / (maxHitPoints + shieldPoints);
                m_pendingHPPercent = (float)(hitPointsAfterResolution + shieldPoints + hoTTotalToDisplay)
                                     / (maxHitPoints + shieldPoints);
                m_energyPercent = (float)energyToDisplay / actualMaxTechPoints;

                ShowTaunt(CanTaunt(activeOwnedActorData));
                UIManager.SetGameObjectActive(m_aliveProfileImage, showAliveProfileImage);
                UIManager.SetGameObjectActive(m_deadProfileImage, showDeadProfileImage);
            }
            else if (m_visualObject != null && m_visualObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_visualObject, false);
            }
        }

        UpdateEnergyBar();
        UpdateHealthBar();
        UpdateShieldBar();
        UpdatePendingHealthBar();

        float ggPackReadyPercent = 1f;
        if (m_ggPackTimeLastUsed > 0f)
        {
            float timeSinceGgPackLastUsed = Time.unscaledTime - m_ggPackTimeLastUsed;
            ggPackReadyPercent =
                Mathf.Clamp01(timeSinceGgPackLastUsed / GameBalanceVars.Get().GGPackInGameCooldownTimer);
        }

        m_ggPackCooldown.fillAmount = 1f - ggPackReadyPercent;
    }

    private void OnEnable()
    {
        m_lastEnergyPercent = 1f;
        Update();
    }
}