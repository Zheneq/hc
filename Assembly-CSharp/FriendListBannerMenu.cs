using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListBannerMenu : UITooltipBase
{
    public enum FriendMenuButtonAction
    {
        SendMessage,
        InviteToParty,
        ViewProfile,
        InviteToGroupChat,
        BlockPlayer,
        ReportPlayer,
        RemoveFriend,
        InviteToGame,
        ObserveGame,
        AddNote
    }

    [Serializable]
    public struct FriendListTooltipBannerButton
    {
        public Image m_icon;
        public TextMeshProUGUI m_label;
        public Button m_button;
    }

    public TextMeshProUGUI m_playerName;
    public Color m_unhighlightedMenuItemColor;
    public FriendListTooltipBannerButton[] m_menuButtons;
    public FriendListMenuGroupChat m_groupSubMenu;

    private FriendInfo m_friendInfo;

    public void Start()
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            FriendMenuButtonAction action = (FriendMenuButtonAction)i;
            if (IsValidButtonAction(action, true))
            {
                UIEventTriggerUtils.AddListener(
                    m_menuButtons[i].m_button.gameObject,
                    EventTriggerType.PointerEnter,
                    OnGroupChatMouseOver);
                UIEventTriggerUtils.AddListener(
                    m_menuButtons[i].m_button.gameObject,
                    EventTriggerType.PointerExit,
                    OnGroupChatMouseExit);
                UIEventTriggerUtils.AddListener(
                    m_menuButtons[i].m_button.gameObject,
                    EventTriggerType.PointerClick,
                    OnGroupChatMouseClicked);
            }
        }
    }

    public bool IsValidButtonAction(FriendMenuButtonAction action, bool IsForSetup = false)
    {
        switch (action)
        {
            case FriendMenuButtonAction.InviteToGame:
            {
                if (GameManager.Get() != null
                    && GameManager.Get().GameInfo != null
                    && GameManager.Get().GameInfo.GameConfig != null)
                {
                    return GameManager.Get().GameInfo.GameConfig.GameType == GameType.Custom
                           && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
                }

                return IsForSetup;
            }
            case FriendMenuButtonAction.InviteToParty:
                return GameManager.Get() == null
                       || GameManager.Get().GameInfo == null
                       || GameManager.Get().GameInfo.GameConfig == null
                       || GameManager.Get().GameInfo.GameConfig.GameType != GameType.Custom
                       || GameManager.Get().GameInfo.GameStatus == GameStatus.Stopped;
            case FriendMenuButtonAction.ObserveGame:
                return GameManager.Get() == null
                       || GameManager.Get().GameplayOverrides == null
                       || m_friendInfo.IsJoinable(GameManager.Get().GameplayOverrides);
            case FriendMenuButtonAction.SendMessage:
            case FriendMenuButtonAction.BlockPlayer:
            case FriendMenuButtonAction.RemoveFriend:
            case FriendMenuButtonAction.ReportPlayer:
            case FriendMenuButtonAction.AddNote:
                return true;
            default:
                return false;
        }
    }

    private void OpenAddNoteBox()
    {
        UIDialogPopupManager.OpenSingleLineInputDialog(
            StringUtil.TR("FriendNote", "Global"),
            string.Format(StringUtil.TR("AddANoteFor", "Global"), m_friendInfo.FriendHandle),
            StringUtil.TR("Ok", "Global"),
            StringUtil.TR("Cancel", "Global"),
            box =>
            {
                string text = (box as UISingleInputLineInputDialogBox).m_descriptionBoxInputField.text;
                SlashCommands.Get().RunSlashCommand(
                    "/friend",
                    $"{StringUtil.TR("NoteFriend", "SlashCommand")} {m_friendInfo.FriendHandle} {text}");
            });
    }

    public void OnGroupChatMouseClicked(BaseEventData data)
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            FriendMenuButtonAction action = (FriendMenuButtonAction)i;
            if (!IsValidButtonAction(action)
                || (data as PointerEventData).pointerCurrentRaycast.gameObject != m_menuButtons[i].m_button.gameObject)
            {
                continue;
            }

            switch (action)
            {
                case FriendMenuButtonAction.SendMessage:
                    FriendListPanel.Get().RequestToSendMessage(m_friendInfo);
                    break;
                case FriendMenuButtonAction.InviteToParty:
                    FriendListPanel.Get().RequestToInviteToParty(m_friendInfo);
                    break;
                case FriendMenuButtonAction.ViewProfile:
                    FriendListPanel.Get().RequestToViewProfile(m_friendInfo);
                    break;
                case FriendMenuButtonAction.BlockPlayer:
                    FriendListPanel.Get().RequestToBlockPlayer(m_friendInfo);
                    break;
                case FriendMenuButtonAction.ReportPlayer:
                    UILandingPageFullScreenMenus.Get().SetReportContainerVisible(
                        true,
                        m_friendInfo.FriendHandle,
                        m_friendInfo.FriendAccountId);
                    break;
                case FriendMenuButtonAction.RemoveFriend:
                    FriendListPanel.Get().RequestToRemoveFriend(m_friendInfo);
                    break;
                case FriendMenuButtonAction.InviteToGame:
                    FriendListPanel.Get().RequestToInviteToGame(m_friendInfo);
                    break;
                case FriendMenuButtonAction.ObserveGame:
                    FriendListPanel.Get().RequestToObserveGame(m_friendInfo);
                    break;
                case FriendMenuButtonAction.AddNote:
                    OpenAddNoteBox();
                    break;
            }

            break;
        }

        SetVisible(false);
    }

    private void OnDisable()
    {
    }

    public void OnGroupChatMouseOver(BaseEventData data)
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            FriendMenuButtonAction action = (FriendMenuButtonAction)i;
            if (IsValidButtonAction(action))
            {
                if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
                {
                    m_menuButtons[i].m_icon.color = Color.white;
                    m_menuButtons[i].m_label.color = Color.white;
                }
                else
                {
                    m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
                    m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
                }
            }
            else
            {
                m_menuButtons[i].m_icon.color = Color.gray;
                m_menuButtons[i].m_label.color = Color.gray;
            }
        }

        if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[(int)FriendMenuButtonAction.InviteToGroupChat].m_button.gameObject)
        {
            UIManager.SetGameObjectActive(m_groupSubMenu, true);
            m_groupSubMenu.Setup();
        }
        else
        {
            UIManager.SetGameObjectActive(m_groupSubMenu, false);
        }
    }

    public void OnGroupChatMouseExit(BaseEventData data)
    {
    }

    public void Setup(FriendInfo friendInfo)
    {
        m_friendInfo = friendInfo;
        m_playerName.text = friendInfo.FriendHandle;
        UIManager.SetGameObjectActive(m_groupSubMenu, false);
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            FriendMenuButtonAction action = (FriendMenuButtonAction)i;
            if (IsValidButtonAction(action))
            {
                m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
                m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
            }
            else
            {
                m_menuButtons[i].m_icon.color = Color.gray;
                m_menuButtons[i].m_label.color = Color.gray;
            }
        }
    }
}