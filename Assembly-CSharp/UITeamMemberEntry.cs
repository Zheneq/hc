using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITeamMemberEntry : MonoBehaviour
{
    private enum SubButtonIndex
    {
        None = -1,
        AddBot,
        BotSettings,
        KickPlayer,
        SwapTeam,
        RemoveChar,
        SwapSpectator
    }

    public GridLayoutGroup m_subActionBtnContainer;
    public Image m_hoverHighlight;
    public TextMeshProUGUI m_PlayerText;
    public Image[] m_subButtons;
    public LobbyPlayerInfo m_playerInfo;

    private Team m_teamId;

    private void Start()
    {
        for (int i = 0; i < m_subButtons.Length; i++)
        {
            m_subButtons[i].color = Color.gray;
            UIEventTriggerUtils.AddListener(m_subButtons[i].gameObject, EventTriggerType.PointerEnter, MouseSubBtnEnter);
            UIEventTriggerUtils.AddListener(m_subButtons[i].gameObject, EventTriggerType.PointerExit, MouseSubBtnExit);
            int index = i;
            m_subButtons[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => SetupTooltip(tooltip, (SubButtonIndex)index));
        }

        UIEventTriggerUtils.AddListener(m_subButtons[(int)SubButtonIndex.AddBot].gameObject, EventTriggerType.PointerClick, DoAddBot);
        m_subButtons[(int)SubButtonIndex.BotSettings].GetComponent<UITooltipClickObject>().Setup(TooltipType.BotSettingsMenu, SetupBotSettingsMenu);
        UIEventTriggerUtils.AddListener(m_subButtons[(int)SubButtonIndex.KickPlayer].gameObject, EventTriggerType.PointerClick, DoKickPlayer);
        UIEventTriggerUtils.AddListener(m_subButtons[(int)SubButtonIndex.SwapTeam].gameObject, EventTriggerType.PointerClick, DoSwapTeam);
        UIEventTriggerUtils.AddListener(m_subButtons[(int)SubButtonIndex.RemoveChar].gameObject, EventTriggerType.PointerClick, DoRemoveChar);
        UIEventTriggerUtils.AddListener(m_subButtons[(int)SubButtonIndex.SwapSpectator].gameObject, EventTriggerType.PointerClick, DoSwapSpectator);
    }

    private bool SetupTooltip(UITooltipBase tooltip, SubButtonIndex type)
    {
        string tooltipTitle = string.Empty;
        string tooltipText = string.Empty;
        switch (type)
        {
            case SubButtonIndex.AddBot:
                tooltipTitle = StringUtil.TR("AddBot", "Global");
                tooltipText = StringUtil.TR("AddBotToTeam", "Global");
                break;
            case SubButtonIndex.BotSettings:
                tooltipTitle = StringUtil.TR("ChangeOwner", "Global");
                tooltipText = StringUtil.TR("AssignOwnerOfBot", "Global");
                break;
            case SubButtonIndex.KickPlayer:
                tooltipTitle = StringUtil.TR("KickPlayer", "Global");
                tooltipText = StringUtil.TR("KickPlayerFromGame", "Global");
                break;
            case SubButtonIndex.RemoveChar:
                tooltipTitle = StringUtil.TR("RemoveBot", "Global");
                tooltipText = StringUtil.TR("RemoveBotFromGame", "Global");
                break;
            case SubButtonIndex.SwapTeam:
                tooltipTitle = StringUtil.TR("SwapTeam", "Global");
                tooltipText = StringUtil.TR("SwapPlayerToOtherTeam", "Global");
                break;
            case SubButtonIndex.SwapSpectator:
                tooltipTitle = StringUtil.TR("SwapSpectator", "Global");
                tooltipText = StringUtil.TR("SwapPlayerToFramSpectator", "Global");
                break;
            default:
                return false;
        }

        (tooltip as UITitledTooltip).Setup(tooltipTitle, tooltipText, string.Empty);
        return true;
    }

    public void MouseSubBtnEnter(BaseEventData data)
    {
        GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
        foreach (var subButton in m_subButtons)
        {
            if (subButton.gameObject == gameObject)
            {
                subButton.color = Color.white;
                return;
            }
        }
    }

    public void ClearSelection(GameObject selectedObject)
    {
        foreach (Image subButton in m_subButtons)
        {
            if (subButton.gameObject != selectedObject)
            {
                subButton.color = Color.gray;
            }
        }
    }

    public void MouseSubBtnExit(BaseEventData data)
    {
        GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
        ClearSelection(gameObject);
    }

    public void DoAddBot(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
        UIGameSettingsPanel.Get().AddBot(this);
    }

    private bool SetupBotSettingsMenu(UITooltipBase tooltip)
    {
        (tooltip as UIGameSettingBotMenu).Setup(this, m_subButtons[(int)SubButtonIndex.BotSettings]);
        return true;
    }

    public void DoKickPlayer(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
        if (!m_playerInfo.IsGameOwner)
        {
            UIGameSettingsPanel.Get().KickPlayer(this);
        }
    }

    public void DoSwapTeam(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
        UIGameSettingsPanel.Get().SwapTeam(this);
    }

    public void DoRemoveChar(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
        if (m_playerInfo.IsNPCBot)
        {
            UIGameSettingsPanel.Get().RemoveBot(this);
        }
        else
        {
            UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, null);
        }
    }

    public void DoSwapSpectator(BaseEventData data)
    {
        UIFrontEnd.PlaySound(FrontEndButtonSounds.TeamMemberCancel);
        UIGameSettingsPanel.Get().SwapSpectator(this);
    }

    public void SetTeamPlayerInfo(LobbyPlayerInfo playerInfo)
    {
        m_playerInfo = playerInfo;
        UIManager.SetGameObjectActive(m_hoverHighlight, true);
        UIManager.SetGameObjectActive(m_subActionBtnContainer, true);
        if (m_playerInfo == null)
        {
            m_PlayerText.text = "-";
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.AddBot], m_teamId != Team.Spectator);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.BotSettings], false);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.KickPlayer], false);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.RemoveChar], false);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapTeam], false);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapSpectator], false);
        }
        else
        {
            m_PlayerText.text = m_playerInfo.GetHandle();
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.AddBot], false);
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapTeam], true);
            if (!m_playerInfo.IsNPCBot && !m_playerInfo.IsRemoteControlled)
            {
                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.BotSettings], false);
                if (m_playerInfo.IsGameOwner)
                {
                    UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.KickPlayer], false);
                    UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapSpectator], true);
                }
                else
                {
                    UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.KickPlayer], true);
                    UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapSpectator], true);
                }

                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.RemoveChar], false);
            }
            else
            {
                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.BotSettings], true);
                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.KickPlayer], false);
                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.RemoveChar], true);
                UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapSpectator], false);
            }
        }

        foreach (Image subButton in m_subButtons)
        {
            subButton.color = Color.gray;
        }

        if (!GameManager.Get().GameplayOverrides.AllowSpectators)
        {
            if (m_teamId == Team.Spectator)
            {
                m_PlayerText.text = StringUtil.TR("COMINGSOON", "Global");
            }
            UIManager.SetGameObjectActive(m_subButtons[(int)SubButtonIndex.SwapSpectator], false);
        }
    }

    public void SetControllingPlayerInfo(LobbyPlayerInfo controllingPlayerInfo)
    {
        UIGameSettingsPanel.Get().SetControllingPlayerInfo(this, controllingPlayerInfo);
    }

    public void SetEmptyPlayerInfo()
    {
        m_playerInfo = null;
        UIManager.SetGameObjectActive(m_hoverHighlight, false);
        UIManager.SetGameObjectActive(m_subActionBtnContainer, false);
    }

    public LobbyPlayerInfo GetPlayerInfo()
    {
        return m_playerInfo;
    }

    public Team GetTeamId()
    {
        return m_teamId;
    }

    public void SetTeamId(Team teamId)
    {
        m_teamId = teamId;
    }
}