using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerPanelGroupMenu : UITooltipBase
{
    public enum GroupMenuButtonAction
    {
        SendMessage,
        ViewProfile,
        AddToFriends,
        PromoteToLeader,
        KickFromParty,
        ReportPlayer,
        LeaveParty
    }

    [Serializable]
    public struct GroupMenuButton
    {
        public GameObject m_container;
        public Image m_icon;
        public TextMeshProUGUI m_label;
        public Button m_button;
    }

    public TextMeshProUGUI m_playerName;
    public Color m_unhighlightedMenuItemColor;
    public GroupMenuButton[] m_menuButtons;

    private string m_memberHandle;
    private long m_memberAccountId = -1L;
    private bool m_botMasqueradingAsHuman;

    private void Awake()
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            UIEventTriggerUtils.AddListener(
                m_menuButtons[i].m_button.gameObject,
                EventTriggerType.PointerEnter,
                OnGroupChatMouseOver);
            UIEventTriggerUtils.AddListener(
                m_menuButtons[i].m_button.gameObject,
                EventTriggerType.PointerClick,
                OnGroupChatMouseClicked);
        }
    }

    public void OnGroupChatMouseClicked(BaseEventData data)
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            if (!IsValidButtonAction((GroupMenuButtonAction)i))
            {
                continue;
            }

            if (!((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject))
            {
                continue;
            }

            switch (i)
            {
                case (int)GroupMenuButtonAction.SendMessage:
                    UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + m_memberHandle + " ");
                    break;
                case (int)GroupMenuButtonAction.AddToFriends:
                    if (m_botMasqueradingAsHuman)
                    {
                        TextConsole.Get().Write(
                            new TextConsole.Message
                            {
                                Text = StringUtil.TR("AddFriendRequest", "SlashCommand"),
                                MessageType = ConsoleMessageType.SystemMessage
                            });
                    }
                    else
                    {
                        FriendListPanel.Get().RequestToAddFriend(m_memberHandle);
                    }

                    break;
                case (int)GroupMenuButtonAction.PromoteToLeader:
                    TextConsole.Get().OnInputSubmitted("/promote " + m_memberHandle);
                    break;
                case (int)GroupMenuButtonAction.KickFromParty:
                    TextConsole.Get().OnInputSubmitted("/kick " + m_memberHandle);
                    break;
                case (int)GroupMenuButtonAction.ReportPlayer:
                    UILandingPageFullScreenMenus.Get().SetReportContainerVisible(
                        true,
                        m_memberHandle,
                        m_memberAccountId,
                        m_botMasqueradingAsHuman);
                    break;
                case (int)GroupMenuButtonAction.LeaveParty:
                    TextConsole.Get().OnInputSubmitted("/leave");
                    break;
            }

            SetVisible(false);
            return;
        }
    }

    public void OnGroupChatMouseOver(BaseEventData data)
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GroupMenuButtonAction action = (GroupMenuButtonAction)i;
            UIManager.SetGameObjectActive(m_menuButtons[i].m_container, IsButtonActionVisible(action));
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
                m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
                m_menuButtons[i].m_label.color = Color.gray * 0.75f;
            }
        }
    }

    public void Setup(UpdateGroupMemberData memberInfo)
    {
        if (memberInfo != null)
        {
            m_memberHandle = memberInfo.MemberDisplayName;
            m_memberAccountId = memberInfo.AccountID;
        }
        else
        {
            m_memberHandle = string.Empty;
            m_memberAccountId = -1L;
            m_botMasqueradingAsHuman = false;
        }

        SetupCommon();
    }

    public void Setup(LobbyPlayerInfo memberInfo)
    {
        if (memberInfo != null)
        {
            m_memberHandle = memberInfo.GetHandle();
            m_memberAccountId = memberInfo.AccountId;
            m_botMasqueradingAsHuman = memberInfo.BotsMasqueradeAsHumans;
        }
        else
        {
            m_memberHandle = string.Empty;
            m_memberAccountId = -1L;
            m_botMasqueradingAsHuman = false;
        }

        SetupCommon();
    }

    public void SetupCommon()
    {
        if (!m_memberHandle.IsNullOrEmpty())
        {
            m_playerName.text = m_memberHandle;
        }

        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GroupMenuButtonAction action = (GroupMenuButtonAction)i;
            UIManager.SetGameObjectActive(m_menuButtons[i].m_container, IsButtonActionVisible(action));
            if (IsValidButtonAction(action))
            {
                m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
                m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
            }
            else
            {
                m_menuButtons[i].m_icon.color = Color.gray * 0.75f;
                m_menuButtons[i].m_label.color = Color.gray * 0.75f;
            }
        }
    }

    public bool IsValidButtonAction(GroupMenuButtonAction action)
    {
        if (m_memberAccountId < 0)
        {
            return false;
        }

        switch (action)
        {
            case GroupMenuButtonAction.AddToFriends:
                return true;
            case GroupMenuButtonAction.KickFromParty:
                return ClientGameManager.Get().GroupInfo.IsLeader;
            case GroupMenuButtonAction.LeaveParty:
                return ClientGameManager.Get().GroupInfo.InAGroup;
            case GroupMenuButtonAction.PromoteToLeader:
                return ClientGameManager.Get().GroupInfo.IsLeader;
            case GroupMenuButtonAction.SendMessage:
                return true;
            case GroupMenuButtonAction.ReportPlayer:
                return true;
            default:
                return false;
        }
    }

    public bool IsButtonActionVisible(GroupMenuButtonAction action)
    {
        return m_memberAccountId < 0
               || m_memberAccountId != ClientGameManager.Get().GetPlayerAccountData().AccountId
               || (action != GroupMenuButtonAction.SendMessage
                   && action != GroupMenuButtonAction.AddToFriends
                   && action != GroupMenuButtonAction.PromoteToLeader
                   && action != GroupMenuButtonAction.KickFromParty
                   && action != GroupMenuButtonAction.ReportPlayer);
    }
}