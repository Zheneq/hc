public class UIRankedCharacterSelectSettingsPanel : UICharacterSelectCharacterSettingsPanel
{
    protected override FrontEndButtonSounds GetTabClickSound()
    {
        return FrontEndButtonSounds.RankFreelancerSettingTab;
    }

    protected override void Awake()
    {
        if (m_abilitiesSubPanel.m_modloadouts != null)
        {
            UIManager.SetGameObjectActive(m_abilitiesSubPanel.m_modloadouts.transform.parent, true);
        }

        base.Awake();
    }

    public void Refresh()
    {
        if (!UIRankedModeDraftScreen.Get().IsVisible
            || UIRankedModeDraftScreen.Get().SelectedCharacter == CharacterType.None)
        {
            return;
        }

        CharacterResourceLink characterResourceLink = GameWideData.Get()
            .GetCharacterResourceLink(UIRankedModeDraftScreen.Get().SelectedCharacter);
#if EVOS
        // custom: multilancer draft. Refresh(CharacterResourceLink) sets up catalysts and skin from our primary
        // slot (GameManager.PlayerInfo). When editing another owned slot, use that slot's selections instead,
        // as catalyst edits are saved on top of what is displayed.
        LobbyPlayerInfo slotPlayerInfo = UIRankedModeDraftScreen.Get().GetLoadoutEditSlotPlayerInfo();
        if (slotPlayerInfo != null
            && slotPlayerInfo.CharacterInfo != null
            && slotPlayerInfo.CharacterType == characterResourceLink.m_characterType)
        {
            RefreshForSlot(characterResourceLink, slotPlayerInfo.CharacterInfo);
            return;
        }
#endif
        Refresh(characterResourceLink);
    }

#if EVOS
    // custom: Refresh(CharacterResourceLink) for an owned draft slot other than our primary one.
    // Cannot call base Refresh and override afterwards: spells panel setup may save default cards
    // for the displayed (wrong) selection and then ignore setups until the response arrives.
    private void RefreshForSlot(CharacterResourceLink characterResourceLink, LobbyCharacterInfo characterInfo)
    {
        bool sameCharacter = m_selectedCharType == characterResourceLink.m_characterType;
        m_spellsSubPanel.Setup(characterResourceLink.m_characterType, characterInfo.CharacterCards, false, sameCharacter);
        m_abilitiesSubPanel.Setup(characterResourceLink, sameCharacter);
        m_skinsSubPanel.Setup(characterResourceLink, characterInfo.CharacterSkin, sameCharacter);
        m_tauntsSubPanel.Setup(characterResourceLink, sameCharacter);
        if (m_generalSubPanel != null)
        {
            m_generalSubPanel.Setup(characterResourceLink);
        }

        m_selectedCharType = characterResourceLink.m_characterType;
    }
#endif

    public void UpdateSelectedCharType(CharacterType charType)
    {
        m_selectedCharType = charType;
    }

    public new static UIRankedCharacterSelectSettingsPanel Get()
    {
        return UIRankedModeDraftScreen.Get() != null
            ? UIRankedModeDraftScreen.Get().m_rankedModeCharacterSettings
            : null;
    }

    protected override void DoVisible(bool visible, TabPanel tab = TabPanel.None)
    {
        m_isVisible = visible;
        float alpha = 0f;
        if (visible)
        {
            OpenTab(tab, true);
            alpha = 1f;
        }

        UIManager.SetGameObjectActive(this, visible);
        m_CanvasGroup.alpha = alpha;
        m_CanvasGroup.interactable = visible;
        m_CanvasGroup.blocksRaycasts = visible;
    }
}