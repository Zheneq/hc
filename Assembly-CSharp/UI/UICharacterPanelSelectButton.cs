using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterPanelSelectButton : MonoBehaviour
{
    public Image m_charImageAvailable;
    public Image m_charImageLocked;
    public Image m_charImageDisabled;

    public RectTransform m_LockedContainer;
    public RectTransform m_AvailableContainer;
    public RectTransform m_DisabledContainer;
    public RectTransform m_freeRotation;
    public RectTransform m_UnavailableExpBarContainer;
    public RectTransform m_NormalExpBarContainer;
    public RectTransform m_MasterExpBarContainer;

    public ImageFilledSloped m_unavailableBar;
    public ImageFilledSloped m_normalBar;
    public ImageFilledSloped m_masterBar;

    public TextMeshProUGUI[] m_levelTextLabels;
    public _SelectableBtn m_button;
    public CharacterType m_characterType;

    protected bool m_isDisabled;
    private bool m_isMaster;
    private bool m_isTooltipInitialized;
    protected CharacterResourceLink m_characterResourceLink;

    private void Start()
    {
        UIEventTriggerUtils.AddListener(
            m_button.spriteController.gameObject,
            EventTriggerType.PointerClick,
            OnButtonClicked);
    }

    public void UpdateFreeRotationIcon()
    {
        if (m_characterResourceLink != null)
        {
            bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(
                m_characterResourceLink.m_characterType,
                UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
            UIManager.SetGameObjectActive(m_freeRotation, doActive);
        }
    }

    protected virtual FrontEndButtonSounds SoundToPlayOnClick()
    {
        return FrontEndButtonSounds.CharacterSelectModAdd;
    }

    public void SetClickable(bool clickable)
    {
        m_button.spriteController.SetClickable(clickable);
    }

    public CharacterResourceLink GetCharacterResourceLink()
    {
        return m_characterResourceLink;
    }

    public bool IsDisabled()
    {
        return m_isDisabled;
    }

    public virtual void Setup(bool isAvailable, bool selected = false)
    {
        bool flag = ClientGameManager.Get().GroupInfo != null
                    && ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice;
        UIManager.SetGameObjectActive(gameObject, true);
        SetSelected(selected);
        CharacterResourceLink characterResourceLink = null;
        if (m_characterType != CharacterType.None
            && GameManager.Get() != null
            && GameManager.Get().IsCharacterVisible(m_characterType))
        {
            characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_characterType);
        }

        if (characterResourceLink == null)
        {
            m_characterResourceLink = null;
            UIManager.SetGameObjectActive(m_freeRotation, false);
            UIManager.SetGameObjectActive(m_LockedContainer, false);
            UIManager.SetGameObjectActive(m_DisabledContainer, false);
            UIManager.SetGameObjectActive(m_AvailableContainer, false);
            UIManager.SetGameObjectActive(m_NormalExpBarContainer, false);
            UIManager.SetGameObjectActive(m_UnavailableExpBarContainer, false);
            UIManager.SetGameObjectActive(m_MasterExpBarContainer, false);
            SetEnabled(false, null);
            return;
        }

        if (!m_isTooltipInitialized)
        {
            UITooltipHoverObject tooltipHoverObject = m_button.spriteController.GetComponent<UITooltipHoverObject>();
#if EVOS
            //Fill in draft
            if (characterResourceLink.m_characterType == CharacterType.PendingWillFill)
            {
                SetupTootlipForRandomDraft(
                    tooltipHoverObject,
                    LocalizationPayload.Create("CharacterRole_Assassin", "Global").ToString(),
                    LocalizationPayload.Create("Random_Character_Firepower", "Global").ToString());
            }
            else if (characterResourceLink.m_characterType == CharacterType.TestFreelancer1)
            {
                SetupTootlipForRandomDraft(
                    tooltipHoverObject,
                    LocalizationPayload.Create("CharacterRole_Tank", "Global").ToString(),
                    LocalizationPayload.Create("Random_Character_Tank", "Global").ToString());
            }
            else if (characterResourceLink.m_characterType == CharacterType.TestFreelancer2)
            {
                SetupTootlipForRandomDraft(
                    tooltipHoverObject,
                    LocalizationPayload.Create("CharacterRole_Support", "Global").ToString(),
                    LocalizationPayload.Create("Random_Character_Support", "Global").ToString());
            }
#else
            if (characterResourceLink.m_characterType.IsWillFill())
            {
                tooltipHoverObject.Setup(
                    TooltipType.Titled,
                    delegate(UITooltipBase tooltip)
                    {
                        if (m_button.spriteController.IsClickable() && m_characterResourceLink != null)
                        {
                            (tooltip as UITitledTooltip).Setup(
                                m_characterResourceLink.GetDisplayName(),
                                m_characterResourceLink.GetCharSelectTooltipDescription(),
                                string.Empty);
                            return true;
                        }

                        return false;
                    });
            }
#endif
            else
            {
                tooltipHoverObject.Setup(
                    TooltipType.Character,
                    delegate(UITooltipBase tooltip)
                    {
                        if (m_button.spriteController.IsClickable() && !(m_characterResourceLink == null))
                        {
                            (tooltip as UICharacterTooltip).Setup(
                                m_characterResourceLink,
                                UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
                            return true;
                        }

                        return false;
                    });
            }

            m_isTooltipInitialized = true;
        }

        m_characterResourceLink = characterResourceLink;
        PersistedCharacterData playerCharacterData =
            ClientGameManager.Get().GetPlayerCharacterData(characterResourceLink.m_characterType);
        SetEnabled(isAvailable || flag, playerCharacterData);
        bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(
            m_characterResourceLink.m_characterType,
            UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
        UIManager.SetGameObjectActive(m_freeRotation, doActive);
        if (playerCharacterData != null)
        {
            int level = playerCharacterData.ExperienceComponent.Level;
            foreach (TextMeshProUGUI txt in m_levelTextLabels)
            {
                txt.text = level.ToString();
            }

            if (level < GameBalanceVars.Get().MaxCharacterLevel)
            {
                float fillAmount = playerCharacterData.ExperienceComponent.XPProgressThroughLevel
                                   / (float)GameBalanceVars.Get().CharacterExperienceToLevel(level);
                m_unavailableBar.fillAmount = fillAmount;
                m_normalBar.fillAmount = fillAmount;
                m_masterBar.fillAmount = fillAmount;
            }
            else
            {
                m_isMaster = true;
                m_unavailableBar.fillAmount = 1f;
                m_normalBar.fillAmount = 1f;
                m_masterBar.fillAmount = 1f;
            }

            RectTransform masterExpBarContainer = m_MasterExpBarContainer;
            UIManager.SetGameObjectActive(masterExpBarContainer, !m_isDisabled && m_isMaster);
        }
    }
    
#if EVOS
    private void SetupTootlipForRandomDraft(UITooltipHoverObject tooltipHoverObject, string title, string text)
    {
        tooltipHoverObject.Setup(
            TooltipType.Titled,
            delegate(UITooltipBase tooltip)
            {
                if (!m_button.spriteController.IsClickable() || m_characterResourceLink == null)
                {
                    return false;
                }

                (tooltip as UITitledTooltip).Setup(title, text, string.Empty);
                return true;
            });
    }
#endif

    public virtual void SetEnabled(bool enabled, PersistedCharacterData playerCharacterData)
    {
        if (m_characterResourceLink != null)
        {
            UIManager.SetGameObjectActive(gameObject, true);
            m_charImageAvailable.sprite = m_characterResourceLink.GetCharacterSelectIcon();
            m_charImageLocked.sprite = m_characterResourceLink.GetCharacterSelectIconBW();
            m_charImageDisabled.sprite = m_characterResourceLink.GetCharacterSelectIconBW();
            CharacterConfig characterConfig = GameManager.Get().GameplayOverrides
                .GetCharacterConfig(m_characterResourceLink.m_characterType);
            bool isProhibited = characterConfig != null
                                && characterConfig.GameTypesProhibitedFrom != null
                                && UICharacterScreen.GetCurrentSpecificState() != null
                                && characterConfig.GameTypesProhibitedFrom.Contains(
                                    UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
            bool isNotValid =
                !GameManager.Get().IsValidForHumanPreGameSelection(m_characterResourceLink.m_characterType);
            RectTransform disabledContainer = m_DisabledContainer;
            UIManager.SetGameObjectActive(disabledContainer, !enabled && (isProhibited || isNotValid));
        }
        else
        {
            UIManager.SetGameObjectActive(gameObject, false);
        }

        m_isDisabled = !enabled;
        UIManager.SetGameObjectActive(m_LockedContainer, !enabled);
        RectTransform availableContainer = m_AvailableContainer;
        UIManager.SetGameObjectActive(availableContainer, enabled && m_characterResourceLink != null);
        RectTransform masterExpBarContainer = m_MasterExpBarContainer;
        UIManager.SetGameObjectActive(
            masterExpBarContainer,
            !m_isDisabled && m_isMaster && m_characterResourceLink != null);
        bool hasProgress = playerCharacterData != null
                           && (playerCharacterData.ExperienceComponent.Level > 1
                               || playerCharacterData.ExperienceComponent.XPProgressThroughLevel > 0);
        RectTransform normalExpBarContainer = m_NormalExpBarContainer;
        UIManager.SetGameObjectActive(normalExpBarContainer, !m_isDisabled && hasProgress);
        RectTransform unavailableExpBarContainer = m_UnavailableExpBarContainer;
        UIManager.SetGameObjectActive(unavailableExpBarContainer, m_isDisabled && hasProgress);
    }

    protected virtual void OnButtonClicked(BaseEventData data)
    {
        if (!m_button.spriteController.IsClickable())
        {
            return;
        }

        UIFrontEnd.PlaySound(SoundToPlayOnClick());
        bool isPlayerReady = ClientGameManager.Get() != null
                             && GameManager.Get().PlayerInfo != null
                             && GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready;
        if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
        {
            isPlayerReady = AppState_GroupCharacterSelect.Get().IsReady();
        }

        if (m_button.spriteController.IsClickable() && !isPlayerReady)
        {
            UIManager.Get().HandleNewSceneStateParameter(
                new UICharacterScreen.CharacterSelectSceneStateParameters
                {
                    ClientRequestToServerSelectCharacter = m_characterResourceLink.m_characterType
                });
        }
    }

    public void SetSelected(bool isSelected)
    {
        m_button.SetSelected(isSelected, false, string.Empty, string.Empty);
        UIManager.SetGameObjectActive(m_button.m_selectedContainer, isSelected);
        m_button.spriteController.ResetMouseState();
    }
}