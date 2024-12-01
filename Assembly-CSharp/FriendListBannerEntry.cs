using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListBannerEntry : MonoBehaviour
{
    public UITooltipClickObject m_tooltipClickObject;
    public UITooltipHoverObject m_tooltipHoverObject;
    public _ButtonSwapSprite m_hitbox;
    public Image m_bannerBG;
    public Image m_bannerFG;
    public TextMeshProUGUI m_playerName;
    public TextMeshProUGUI m_playerStatusLabel;
    public Image m_playerStatusImage;
    public Image m_hoverSelectedImage;
    public Image m_selectedImage;
    public _SelectableBtn m_AcceptButton;
    public _SelectableBtn m_DeclineButton;
    public _SelectableBtn m_InviteButton;
    public Color m_offlineTextColor;

    [HideInInspector]
    public FriendInfo m_friendInfo;

    private FriendListPanel.FriendSubsection m_subSection;
    private bool m_selected;

    public void Start()
    {
        m_tooltipClickObject.Setup(TooltipType.FriendBannerMenu, OpenMenu);
        m_tooltipHoverObject.Setup(TooltipType.Titled, OpenHoverTooltip);
        if (m_hitbox.m_hoverImage != null)
        {
            UIManager.SetGameObjectActive(m_hitbox.m_hoverImage.gameObject, false);
        }

        m_DeclineButton.spriteController.callback = CancelRequest;
        m_AcceptButton.spriteController.callback = AcceptRequest;
        m_InviteButton.spriteController.callback = SendInvite;
        m_InviteButton.spriteController.GetComponent<UITooltipHoverObject>()
            .Setup(TooltipType.Titled, OpenInviteTooltip);
        m_InviteButton.SetSelected(false, true);
        m_AcceptButton.spriteController.gameObject
            .AddComponent<_MouseEventPasser>()
            .AddNewHandler(FriendListPanel.Get().m_scrollView);
        m_DeclineButton.spriteController.gameObject
            .AddComponent<_MouseEventPasser>()
            .AddNewHandler(FriendListPanel.Get().m_scrollView);
        m_InviteButton.spriteController.gameObject
            .AddComponent<_MouseEventPasser>()
            .AddNewHandler(FriendListPanel.Get().m_scrollView);
        UIManager.SetGameObjectActive(m_selectedImage, false);
    }

    private bool OpenInviteTooltip(UITooltipBase tooltip)
    {
        (tooltip as UITitledTooltip).Setup(
            StringUtil.TR("InviteToGroup", "FriendList"),
            string.Format(StringUtil.TR("InviteFriendName", "FriendList"), m_friendInfo.FriendHandle));
        return true;
    }

    public void CancelRequest(BaseEventData data)
    {
        switch (m_subSection)
        {
            case FriendListPanel.FriendSubsection.FriendRequests:
                FriendListPanel.Get().RequestToCancelRequest(m_friendInfo);
                break;
            case FriendListPanel.FriendSubsection.Blocked:
            case FriendListPanel.FriendSubsection.InvitationsSent:
                FriendListPanel.Get().RequestToRemoveFriend(m_friendInfo);
                break;
        }
    }

    public void AcceptRequest(BaseEventData data)
    {
        if (m_subSection == FriendListPanel.FriendSubsection.FriendRequests)
        {
            FriendListPanel.Get().RequestToAcceptRequest(m_friendInfo);
        }
    }

    public void SendInvite(BaseEventData data)
    {
        if (m_subSection == FriendListPanel.FriendSubsection.Offline
            || m_subSection == FriendListPanel.FriendSubsection.Online)
        {
            FriendListPanel.Get().RequestToInviteToParty(m_friendInfo);
        }
    }

    private void SetupButtons(FriendListPanel.FriendSubsection subSection)
    {
        m_subSection = subSection;
        bool showAccept = false;
        bool showDecline = false;
        bool showInvite = false;
        switch (subSection)
        {
            case FriendListPanel.FriendSubsection.Blocked:
                showDecline = true;
                break;
            case FriendListPanel.FriendSubsection.FriendRequests:
                showAccept = true;
                showDecline = true;
                break;
            case FriendListPanel.FriendSubsection.InvitationsSent:
                showDecline = true;
                break;
            case FriendListPanel.FriendSubsection.Offline:
                break;
            case FriendListPanel.FriendSubsection.Online:
                showInvite = true;
                break;
        }

        UIManager.SetGameObjectActive(m_AcceptButton, showAccept);
        UIManager.SetGameObjectActive(m_DeclineButton, showDecline);
        UIManager.SetGameObjectActive(m_InviteButton, showInvite);
    }

    public void Setup(FriendInfo friendInfo, FriendListPanel.FriendSubsection subSection)
    {
        m_friendInfo = friendInfo;
        SetupButtons(subSection);
        m_playerName.text = friendInfo.FriendNote.IsNullOrEmpty()
            ? friendInfo.FriendHandle
            : $"{friendInfo.FriendHandle}({friendInfo.FriendNote})";
        if (m_hitbox.gameObject.GetComponent<_MouseEventPasser>() == null)
        {
            _MouseEventPasser mouseEventPasser = m_hitbox.gameObject.AddComponent<_MouseEventPasser>();
            mouseEventPasser.AddNewHandler(FriendListPanel.Get().m_scrollView);
        }

        switch (friendInfo.FriendStatus)
        {
            case FriendStatus.Friend:
            {
                if (friendInfo.StatusString.IsNullOrEmpty())
                {
                    if (friendInfo.IsOnline)
                    {
                        m_playerStatusLabel.text = StringUtil.TR("Online", "FriendList");
                        m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[0];
                    }
                    else
                    {
                        m_playerStatusLabel.text = StringUtil.TR("Offline", "FriendList");
                        m_playerStatusImage.color = Color.gray;
                    }
                }
                else
                {
                    if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Away.ToString())
                    {
                        m_playerStatusImage.color = FriendListPanel.Get()
                            .m_panelHeader
                            .m_friendListColors[(int)FriendListHeader.PlayerOnlineStatus.Away];
                    }
                    else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Busy.ToString())
                    {
                        m_playerStatusImage.color = FriendListPanel.Get()
                            .m_panelHeader
                            .m_friendListColors[(int)FriendListHeader.PlayerOnlineStatus.Busy];
                    }
                    else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Online.ToString())
                    {
                        m_playerStatusImage.color = FriendListPanel.Get()
                            .m_panelHeader
                            .m_friendListColors[(int)FriendListHeader.PlayerOnlineStatus.Online];
                    }

                    m_playerStatusLabel.text = StringUtil.TR(friendInfo.StatusString, "FriendList");
                }

                if (friendInfo.IsOnline)
                {
                    SetTextOnlineColor();
                }
                else
                {
                    SetTextOfflineColor();
                }

                break;
            }
            case FriendStatus.RequestSent:
                m_playerStatusLabel.text = StringUtil.TR("AwaitingReply", "FriendList");
                break;
            case FriendStatus.RequestReceived:
                m_playerStatusLabel.text = StringUtil.TR("AwaitingApproval", "FriendList");
                break;
            case FriendStatus.Blocked:
                m_playerStatusLabel.text = StringUtil.TR("Blocked", "FriendList");
                break;
            default:
                m_playerStatusLabel.text = string.Empty;
                break;
        }
        
#if EVOS
        UpdateVisualInfo(
            friendInfo.TitleID,
            friendInfo.TitleLevel,
            friendInfo.BannerID,
            friendInfo.EmblemID,
            friendInfo.RibbonID,
            friendInfo.FriendNote,
            friendInfo.FriendHandle);
#else
        UpdateVisualInfo(
            friendInfo.TitleID,
            friendInfo.TitleLevel,
            friendInfo.BannerID,
            friendInfo.EmblemID,
            friendInfo.RibbonID,
            friendInfo.FriendNote);
#endif
    }


#if EVOS
    public void UpdateVisualInfo(
        int titleId,
        int titleLevel,
        int bannerId,
        int emblemId,
        int ribbonId,
        string friendNote,
        string handle)
    {
        BannerManager.GetInstance()?.UpdateBanner(bannerId, emblemId, handle, m_bannerBG, m_bannerFG);

        // Update title and level information
        m_friendInfo.TitleID = titleId;
        m_friendInfo.TitleLevel = titleLevel;
    }
#else
    public void UpdateVisualInfo(
        int titleId,
        int titleLevel,
        int bannerId,
        int emblemId,
        int ribbonId,
        string friendNote)
    {
        GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerId);
        m_bannerBG.sprite = banner != null
            ? (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite))
            : (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite));
        GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(emblemId);
        m_bannerFG.sprite = emblem != null
            ? (Sprite)Resources.Load(emblem.m_resourceString, typeof(Sprite))
            : (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite));
        m_friendInfo.TitleID = titleId;
        m_friendInfo.TitleLevel = titleLevel;
    }
#endif

    public void SetTextOnlineColor()
    {
        m_playerName.color = Color.white;
        m_playerStatusLabel.color = Color.white;
    }

    public void SetTextOfflineColor()
    {
        m_playerName.color = m_offlineTextColor;
        m_playerStatusLabel.color = m_offlineTextColor;
    }

    private bool OpenMenu(UITooltipBase tooltip)
    {
        FriendListBannerMenu friendListBannerMenu = tooltip as FriendListBannerMenu;
        if (m_friendInfo.FriendStatus != FriendStatus.Blocked)
        {
            friendListBannerMenu.Setup(m_friendInfo);
            return true;
        }

        return false;
    }

    private bool OpenHoverTooltip(UITooltipBase tooltip)
    {
        if (!FriendListPanel.Get().IsVisible())
        {
            return false;
        }

        UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
        
        string title = GameWideData.Get().m_gameBalanceVars.GetTitle(
            m_friendInfo.TitleID,
#if EVOS
            m_friendInfo.FriendHandle, // Custom titles
#endif
            string.Empty,
            m_friendInfo.TitleLevel);

        string str = m_friendInfo.FriendHandle;
        if (!title.IsNullOrEmpty())
        {
            str = string.Format(StringUtil.TR("BannerDescription", "FriendList"), m_playerName.text, title);
        }

        str += Environment.NewLine + m_friendInfo.FriendNote;
        uITitledTooltip.Setup(m_playerStatusLabel.text, str);
        return true;
    }

    public bool IsSelected()
    {
        return m_selected;
    }

    public void SetSelected(bool selected)
    {
        m_selected = selected;
        UIManager.SetGameObjectActive(m_selectedImage, selected);
    }
}