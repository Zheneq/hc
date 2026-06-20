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
        Refresh(characterResourceLink);
    }

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