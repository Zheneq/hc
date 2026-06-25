using LobbyGameClientMessages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverBannerMenu : UITooltipBase
{
    public enum GameOverButtonAction
    {
        SendMessage,
        InviteToParty,
        AddFriend,
        BlockPlayer,
        ReportPlayer
    }

    [Serializable]
    public struct GameOverTooltipBannerButton
    {
        public Image m_icon;
        public TextMeshProUGUI m_label;
        public Button m_button;
    }

    public TextMeshProUGUI m_playerName;
    public Color m_unhighlightedMenuItemColor;
    public RectTransform m_bannerMenuContainer;
    public GameOverTooltipBannerButton[] m_menuButtons;

    private string m_playerHandle;
    private long m_playerAccountID;
    private bool m_botMasqueradingAsHuman;

    public void Start()
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GameOverButtonAction action = (GameOverButtonAction)i;
            if (IsValidButtonAction(action, true))
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
    }

    public bool IsValidButtonAction(GameOverButtonAction action, bool IsForSetup = false)
    {
        if (m_botMasqueradingAsHuman)
        {
            return true;
        }

        if (!m_playerHandle.IsNullOrEmpty() && m_playerAccountID != 0)
        {
            if (action == GameOverButtonAction.AddFriend
                && ClientGameManager.Get().FriendList.Friends.ContainsKey(m_playerAccountID))
            {
                FriendInfo friendInfo = ClientGameManager.Get().FriendList.Friends[m_playerAccountID];
                if (friendInfo.FriendStatus == FriendStatus.Friend || friendInfo.FriendStatus == FriendStatus.Blocked)
                {
                    return false;
                }
            }
        }
        else
        {
            return true;
        }

        if (action == GameOverButtonAction.BlockPlayer)
        {
            if (ClientGameManager.Get().FriendList.Friends.ContainsKey(m_playerAccountID))
            {
                FriendInfo friendInfo2 = ClientGameManager.Get().FriendList.Friends[m_playerAccountID];
                if (friendInfo2.FriendStatus == FriendStatus.Blocked)
                {
                    return false;
                }
            }
        }

        return action != GameOverButtonAction.ReportPlayer
               || m_playerAccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId;
    }

    private void HandleFriendUpdateResponse(FriendUpdateResponse response)
    {
        if (response.Success)
        {
            return;
        }

        if (response.LocalizedFailure != null)
        {
            response.ErrorMessage = response.LocalizedFailure.ToString();
        }
        else if (response.ErrorMessage.IsNullOrEmpty())
        {
            response.ErrorMessage = StringUtil.TR("ServerError", "Global");
        }

        string text = null;
        switch (response.FriendOperation)
        {
            case FriendOperation.Add:
                text = string.Format(StringUtil.TR("FailedFriendAdd", "FriendList"), response.ErrorMessage);
                break;
            case FriendOperation.Accept:
                text = string.Format(StringUtil.TR("FailedFriendAccept", "FriendList"), response.ErrorMessage);
                break;
            case FriendOperation.Reject:
                text = string.Format(StringUtil.TR("FailedFriendReject", "FriendList"), response.ErrorMessage);
                break;
            case FriendOperation.Remove:
                text = string.Format(StringUtil.TR("FailedFriendRemove", "FriendList"), response.ErrorMessage);
                break;
            case FriendOperation.Block:
                text = string.Format(StringUtil.TR("FailedFriendBlock", "FriendList"), response.ErrorMessage);
                break;
        }

        if (!text.IsNullOrEmpty())
        {
            UIDialogPopupManager.OpenOneButtonDialog(string.Empty, text, StringUtil.TR("Ok", "Global"));
        }
    }

    public void OnGroupChatMouseClicked(BaseEventData data)
    {
        if ((m_playerHandle.IsNullOrEmpty() || m_playerAccountID == 0) && !m_botMasqueradingAsHuman)
        {
            return;
        }

        int actionId = 0;
        for (; actionId < m_menuButtons.Length; actionId++)
        {
            if (IsValidButtonAction((GameOverButtonAction)actionId)
                && (data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[actionId].m_button.gameObject)
            {
                break;
            }
        }

        if (actionId >= m_menuButtons.Length)
        {
            return;
        }

        string blockReportHandle;
        bool botMasqueradingAsHuman;
        switch ((GameOverButtonAction)actionId)
        {
            case GameOverButtonAction.SendMessage:
                if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd
                    && UIFrontEnd.Get() != null
                    && UIFrontEnd.Get().m_frontEndChatConsole != null)
                {
                    UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + m_playerHandle + " ");
                    break;
                }

                if (UIManager.Get().CurrentState == UIManager.ClientState.InGame
                    && HUD_UI.Get() != null
                    && HUD_UI.Get().m_textConsole != null)
                {
                    HUD_UI.Get().m_textConsole.SelectInput("/whisper " + m_playerHandle + " ");
                }

                break;
            case GameOverButtonAction.InviteToParty:
                if (m_botMasqueradingAsHuman)
                {
                    TextConsole.Get().Write(
                        new TextConsole.Message
                        {
                            Text = string.Format(StringUtil.TR("InviteSentTo", "SlashCommand"), m_playerHandle),
                            MessageType = ConsoleMessageType.SystemMessage
                        });
                }
                else
                {
                    SlashCommands.Get().RunSlashCommand("/invite", m_playerHandle);
                }

                break;
            case GameOverButtonAction.AddFriend:
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
                    ClientGameManager.Get().UpdateFriend(
                        null,
                        m_playerAccountID,
                        FriendOperation.Add,
                        string.Empty,
                        HandleFriendUpdateResponse);
                }

                break;
            case GameOverButtonAction.BlockPlayer:
                string title = StringUtil.TR("BlockPlayer", "FriendList");
                string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), m_playerHandle);
                blockReportHandle = m_playerHandle;
                botMasqueradingAsHuman = m_botMasqueradingAsHuman;
                UIDialogPopupManager.OpenTwoButtonDialog(
                    title,
                    description,
                    StringUtil.TR("Yes", "Global"),
                    StringUtil.TR("No", "Global"),
                    delegate
                    {
                        if (botMasqueradingAsHuman)
                        {
                            TextConsole.Get().Write(
                                new TextConsole.Message
                                {
                                    Text = string.Format(
                                        StringUtil.TR("SuccessfullyBlocked", "SlashCommand"), blockReportHandle),
                                    MessageType = ConsoleMessageType.SystemMessage
                                });
                        }
                        else
                        {
                            SlashCommands.Get().RunSlashCommand("/block", blockReportHandle);
                        }
                    });
                break;
            case GameOverButtonAction.ReportPlayer:
                UILandingPageFullScreenMenus.Get().SetReportContainerVisible(
                    true,
                    m_playerHandle,
                    m_playerAccountID,
                    m_botMasqueradingAsHuman);
                break;
        }

        SetVisible(false);
    }

    private void OnDisable()
    {
        m_playerHandle = string.Empty;
        m_playerAccountID = 0L;
        m_botMasqueradingAsHuman = false;
    }

    public void OnGroupChatMouseOver(BaseEventData data)
    {
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GameOverButtonAction action = (GameOverButtonAction)i;
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
    }

    public void Setup(string accountHandle, long accountID)
    {
        m_playerHandle = accountHandle;
        m_playerAccountID = accountID;
        m_botMasqueradingAsHuman = false;
        m_playerName.text = m_playerHandle;
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GameOverButtonAction action = (GameOverButtonAction)i;
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

    public void Setup(MatchResultsStatline statLine)
    {
        m_playerHandle = statLine.DisplayName;
        m_playerAccountID = statLine.AccountID;
        m_botMasqueradingAsHuman = statLine.IsBotMasqueradingAsHuman;
        m_playerName.text = m_playerHandle;
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GameOverButtonAction action = (GameOverButtonAction)i;
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

    public void Setup(ActorData actorData)
    {
        PlayerDetails playerDetails = GameFlow.Get().playerDetails[actorData.PlayerData.GetPlayer()];
        m_playerHandle = playerDetails.m_handle;
        m_playerAccountID = playerDetails.m_accountId;
        m_botMasqueradingAsHuman = playerDetails.m_botsMasqueradeAsHumans;
        m_playerName.text = playerDetails.m_handle;
        for (int i = 0; i < m_menuButtons.Length; i++)
        {
            GameOverButtonAction action = (GameOverButtonAction)i;
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