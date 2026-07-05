using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectPartyList : MonoBehaviour
{
    public UICharacterSelectPlayerPortrait[] m_allyPortraits;
    public UICharacterSelectPlayerPortrait[] m_enemyPortraits;
    public RectTransform m_characterSelect;
    public UIStarsPanel m_starsPanel;
    public TextMeshProUGUI m_botSkillLabel;
    public LayoutGroup m_botCharacterGridContainer;
    public UIPartyPanelCharacterSelect m_botCharacterPrefab;
    public RectTransform[] m_containers;

    private LobbyPlayerInfo m_playerInfo;
    private LobbyPlayerInfo m_selectedBotInfo;
    private List<UIPartyPanelCharacterSelect> m_CharacterButtons = new List<UIPartyPanelCharacterSelect>();
    private Animator m_animator;

    private bool m_visible;
    private bool m_isDuplicateCharsAllowed;
    private int m_selectedPortraitIndex;
    private int m_numAllyPortraits;
    private int m_numAccountUpdatesToSkipForSwap;
    private bool m_isSwappingMainCharacter;

    private void Start()
    {
        UIManager.SetGameObjectActive(m_characterSelect, false);
        bool enableHiddenCharacters =
            GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
        CharacterType[] characterTypes = (CharacterType[])Enum.GetValues(typeof(CharacterType));
        List<CharacterType> validCharacterTypes = new List<CharacterType>();
        foreach (CharacterType characterType in characterTypes)
        {
            try
            {
                CharacterResourceLink characterResourceLink =
                    GameWideData.Get().GetCharacterResourceLink(characterType);
                if (characterType.IsValidForHumanGameplay()
                    && characterResourceLink.m_allowForPlayers
                    && (enableHiddenCharacters || !characterResourceLink.m_isHidden))
                {
                    CharacterConfig characterConfig = GameManager.Get().GameplayOverrides
                        .GetCharacterConfig(characterResourceLink.m_characterType);
                    if (characterConfig.AllowForPlayers
                        && (enableHiddenCharacters || !characterConfig.IsHidden))
                    {
                        validCharacterTypes.Add(characterType);
                    }
                }
            }
            catch
            {
            }
        }

        validCharacterTypes.Sort((CharacterType first, CharacterType second) =>
            first.GetDisplayName().CompareTo(second.GetDisplayName()));
        foreach (CharacterType characterType in validCharacterTypes)
        {
            UIPartyPanelCharacterSelect uIPartyPanelCharacterSelect = Instantiate(m_botCharacterPrefab);
            UIManager.ReparentTransform(
                uIPartyPanelCharacterSelect.gameObject.transform,
                m_botCharacterGridContainer.gameObject.transform,
                new Vector3(0.55f, 0.55f, 0.55f));
            uIPartyPanelCharacterSelect.m_characterType = characterType;
            m_CharacterButtons.Add(uIPartyPanelCharacterSelect);
        }

        ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
        m_visible = false;
    }

    private void OnDestroy()
    {
        if (ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
        }
    }

    private bool IsOutOfGame()
    {
        return GameManager.Get().GameStatus == GameStatus.Stopped;
    }

    public bool NotifySwapMainCharacter()
    {
        if (m_isSwappingMainCharacter)
        {
            return false;
        }

        m_numAccountUpdatesToSkipForSwap++;
        m_isSwappingMainCharacter = true;
        return true;
    }

    private void OnAccountDataUpdated(PersistedAccountData data)
    {
        if (!IsOutOfGame())
        {
            return;
        }

        if (m_isSwappingMainCharacter)
        {
            if (m_numAccountUpdatesToSkipForSwap > 0)
            {
                m_numAccountUpdatesToSkipForSwap--;
                return;
            }

            UICharacterSelectCharacterSettingsPanel.Get().SetVisible(
                true,
                UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
            m_isSwappingMainCharacter = false;
        }

        SetupForOutOfGame(m_numAllyPortraits, m_isDuplicateCharsAllowed);
    }

    public void SelectedBotCharacter(CharacterResourceLink theChar)
    {
        if (IsOutOfGame() && CanCharacterBeSelectedOutOfGame(theChar.m_characterType))
        {
            ClientGameManager.Get().UpdateRemoteCharacter(theChar.m_characterType, m_selectedPortraitIndex - 1);
            UnselectPortraits();
            return;
        }

        if (m_selectedBotInfo == null || !CanCharacterBeSelectedByBot(theChar.m_characterType))
        {
            return;
        }

        ClientGameManager.Get().UpdateSelectedCharacter(theChar.m_characterType, m_selectedBotInfo.PlayerId);
        UnselectPortraits();
    }

    public void SelectBotDifficulty(BotDifficulty difficulty)
    {
        if (m_selectedBotInfo == null)
        {
            return;
        }

        ClientGameManager.Get().UpdateBotDifficulty(difficulty, difficulty, m_selectedBotInfo.PlayerId);
    }

    private bool CanCharacterBeSelectedByBot(CharacterType type)
    {
        GameManager gameManager = GameManager.Get();
        bool result = gameManager.IsCharacterAllowedForBots(type) || m_selectedBotInfo.IsRemoteControlled;
        if (!GameManager.Get().GameConfig.HasGameOption(GameOptionFlag.AllowDuplicateCharacters))
        {
            foreach (LobbyPlayerInfo current in gameManager.TeamInfo.TeamPlayerInfo)
            {
                if (current.CharacterType == type && current.TeamId == m_selectedBotInfo.TeamId)
                {
                    return false;
                }
            }
        }

        return result;
    }

    private bool CanCharacterBeSelectedOutOfGame(CharacterType charType)
    {
        bool result = ClientGameManager.Get().IsCharacterAvailable(
            charType,
            ClientGameManager.Get().GroupInfo.SelectedQueueType);
        if (result && !m_isDuplicateCharsAllowed)
        {
            foreach (UICharacterSelectPlayerPortrait uICharacterSelectPlayerPortrait in m_allyPortraits)
            {
                if (uICharacterSelectPlayerPortrait.gameObject.activeSelf
                    && uICharacterSelectPlayerPortrait.CharType == charType)
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }

    private void ShowBotCharacterSelect(bool reveal)
    {
        bool show = reveal && m_selectedBotInfo != null;
        UIManager.SetGameObjectActive(m_characterSelect, show);
        if (!show)
        {
            return;
        }

        foreach (UIPartyPanelCharacterSelect characterButton in m_CharacterButtons)
        {
            if (GameManager.Get().IsCharacterVisible(characterButton.m_characterType))
            {
                UIManager.SetGameObjectActive(characterButton, true);
                characterButton.Setup(CanCharacterBeSelectedByBot(characterButton.m_characterType));
            }
            else
            {
                UIManager.SetGameObjectActive(characterButton, false);
            }
        }

        m_starsPanel.SetCurrentValue((int)(m_selectedBotInfo.Difficulty + 1));
        UIManager.SetGameObjectActive(m_starsPanel, true);
        UIManager.SetGameObjectActive(m_botSkillLabel, true);
    }

    public void UnselectPortraits()
    {
        foreach (UICharacterSelectPlayerPortrait portrait in m_enemyPortraits)
        {
            portrait.SetArrowsSelected(false);
        }

        foreach (UICharacterSelectPlayerPortrait portrait in m_allyPortraits)
        {
            portrait.SetArrowsSelected(false);
        }

        ShowBotCharacterSelect(false);
    }

    public void NotifyPlayerPortraitClicked(LobbyPlayerInfo playerInfo)
    {
        m_selectedBotInfo = null;
        if (playerInfo == null || playerInfo.PlayerId != m_playerInfo.PlayerId)
        {
            UnselectPortraits();
        }
    }

    public void NotifyBotPortraitClicked(UICharacterSelectPlayerPortrait ui_element, LobbyPlayerInfo botInfo)
    {
        bool reveal = false;
        if (m_playerInfo != null && m_playerInfo.IsGameOwner)
        {
            m_selectedBotInfo = botInfo;
            foreach (UICharacterSelectPlayerPortrait portrait in m_enemyPortraits)
            {
                if (portrait.SetArrowsSelected(ui_element == portrait))
                {
                    reveal = true;
                }
            }

            foreach (UICharacterSelectPlayerPortrait portrait in m_allyPortraits)
            {
                if (portrait.SetArrowsSelected(ui_element == portrait))
                {
                    reveal = true;
                }
            }
        }

        ShowBotCharacterSelect(reveal);
    }

    public void NotifyOutOfGamePortraitClicked(UICharacterSelectPlayerPortrait ui_element, CharacterType charType)
    {
        bool isPortraitSelected = false;
        foreach (UICharacterSelectPlayerPortrait portrait in m_enemyPortraits)
        {
            portrait.SetArrowsSelected(false);
        }

        for (int j = 0; j < m_allyPortraits.Length; j++)
        {
            if (m_allyPortraits[j].SetArrowsSelected(ui_element == m_allyPortraits[j]))
            {
                isPortraitSelected = true;
                m_selectedPortraitIndex = j;
            }
        }

        UIManager.SetGameObjectActive(m_characterSelect, isPortraitSelected);
        if (!isPortraitSelected)
        {
            return;
        }

        foreach (UIPartyPanelCharacterSelect characterButton in m_CharacterButtons)
        {
            if (GameManager.Get().IsCharacterVisible(characterButton.m_characterType))
            {
                UIManager.SetGameObjectActive(characterButton, true);
                characterButton.Setup(CanCharacterBeSelectedOutOfGame(characterButton.m_characterType));
            }
            else
            {
                UIManager.SetGameObjectActive(characterButton, false);
            }
        }

        UIManager.SetGameObjectActive(m_starsPanel, false);
        UIManager.SetGameObjectActive(m_botSkillLabel, false);
    }

    public void SetActive(bool active)
    {
        foreach (RectTransform container in m_containers)
        {
            if (container != null)
            {
                UIManager.SetGameObjectActive(container, active);
            }
        }
    }

    public void SetVisible(bool visible, bool force = false)
    {
        if (visible == m_visible && !force)
        {
            return;
        }

        m_visible = visible;
        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }

        if (m_visible)
        {
            UIAnimationEventManager.Get().PlayAnimation(m_animator, "Showing", null, string.Empty);
        }
        else
        {
            UIAnimationEventManager.Get().PlayAnimation(m_animator, "Hiding", null, string.Empty);
            UnselectPortraits();
        }
    }

    public void UpdateCharacterList(LobbyPlayerInfo playerInfo, LobbyTeamInfo teamInfo, LobbyGameInfo gameInfo)
    {
        int allyIndex = 0;
        int enemyIndex = 0;
        int allySlotCount = 0;
        int enemySlotCount = 0;
        m_playerInfo = playerInfo;

        if (playerInfo == null
            || teamInfo == null
            || gameInfo == null
            || teamInfo.TeamPlayerInfo == null)
        {
            return;
        }

        Team allyTeam = Team.TeamA;
        Team enemyTeam = Team.TeamB;
        switch (playerInfo.TeamId)
        {
            case Team.TeamA:
                allyTeam = Team.TeamA;
                enemyTeam = Team.TeamB;
                allySlotCount = gameInfo.GameConfig.TeamAPlayers;
                enemySlotCount = gameInfo.GameConfig.TeamBPlayers;
                break;
            case Team.TeamB:
                allyTeam = Team.TeamB;
                enemyTeam = Team.TeamA;
                allySlotCount = gameInfo.GameConfig.TeamBPlayers;
                enemySlotCount = gameInfo.GameConfig.TeamAPlayers;
                break;
        }

        foreach (LobbyPlayerInfo current in teamInfo.TeamPlayerInfo)
        {
            if (current.PlayerId != m_playerInfo.PlayerId)
            {
                continue;
            }

            if (current.TeamId == allyTeam)
            {
                m_allyPortraits[allyIndex].SetEnabled(true);
                m_allyPortraits[allyIndex].Setup(current);
                allyIndex++;
            }
            else if (current.TeamId == enemyTeam)
            {
                m_enemyPortraits[enemyIndex].SetEnabled(true);
                m_enemyPortraits[enemyIndex].Setup(current);
                enemyIndex++;
            }
        }

        foreach (LobbyPlayerInfo item in teamInfo.TeamPlayerInfo)
        {
            if (item.PlayerId == m_playerInfo.PlayerId)
            {
                continue;
            }

            if (item.TeamId == allyTeam)
            {
                m_allyPortraits[allyIndex].SetEnabled(true);
                m_allyPortraits[allyIndex].Setup(item);
                allyIndex++;
            }
            else if (item.TeamId == enemyTeam)
            {
                m_enemyPortraits[enemyIndex].SetEnabled(true);
                m_enemyPortraits[enemyIndex].Setup(item);
                enemyIndex++;
            }
        }

        for (; enemyIndex < m_enemyPortraits.Length; enemyIndex++)
        {
            if (enemyIndex < enemySlotCount)
            {
                m_enemyPortraits[enemyIndex].SetEnabled(true);
                m_enemyPortraits[enemyIndex].Setup(null);
            }
            else
            {
                m_enemyPortraits[enemyIndex].SetEnabled(false);
            }
        }

        for (; allyIndex < m_allyPortraits.Length; allyIndex++)
        {
            if (allyIndex < allySlotCount)
            {
                m_allyPortraits[allyIndex].SetEnabled(true);
                m_allyPortraits[allyIndex].Setup(null);
            }
            else
            {
                m_allyPortraits[allyIndex].SetEnabled(false);
            }
        }
    }

    public void SetupForOutOfGame(int numAllyPortraits, bool isDuplicateCharsAllowed)
    {
        m_isDuplicateCharsAllowed = isDuplicateCharsAllowed;
        m_numAllyPortraits = numAllyPortraits;
        foreach (UICharacterSelectPlayerPortrait portrait in m_enemyPortraits)
        {
            portrait.SetEnabled(false);
        }

        AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
        m_allyPortraits[0].Setup(accountComponent.LastCharacter, true);
        m_allyPortraits[0].SetEnabled(true);
        for (int i = 1; i < m_allyPortraits.Length; i++)
        {
            CharacterType charType = CharacterType.None;
            if (i - 1 < accountComponent.LastRemoteCharacters.Count)
            {
                charType = accountComponent.LastRemoteCharacters[i - 1];
            }

            m_allyPortraits[i].Setup(charType, false);
            m_allyPortraits[i].SetEnabled(i < numAllyPortraits);
        }

        if (!isDuplicateCharsAllowed)
        {
            List<CharacterType> replacementChars = new List<CharacterType>();
            List<int> replacementSlots = new List<int>();
            List<CharacterType> assignedChars = new List<CharacterType> { accountComponent.LastCharacter };
            for (int i = 0; i < accountComponent.LastRemoteCharacters.Count; i++)
            {
                CharacterType charType = accountComponent.LastRemoteCharacters[i];
                if (assignedChars.Contains(charType))
                {
                    foreach (CharacterType characterType in accountComponent.FreeRotationCharacters)
                    {
                        charType = characterType;
                        if (assignedChars.Contains(charType))
                        {
                            continue;
                        }

                        if (!accountComponent.LastRemoteCharacters.Contains(charType))
                        {
                            break;
                        }
                    }

                    replacementChars.Add(charType);
                    replacementSlots.Add(i);
                }

                assignedChars.Add(charType);
            }

            if (replacementChars.Count > 0)
            {
                ClientGameManager.Get().UpdateRemoteCharacter(replacementChars.ToArray(), replacementSlots.ToArray());
            }
        }

        UnselectPortraits();
    }
}