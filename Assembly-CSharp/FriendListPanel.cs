using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListPanel : MonoBehaviour
{
    public enum FriendSubsection
    {
        FriendRequests,
        Online,
        Offline,
        InvitationsSent,
        Blocked,
        LAST
    }

    public class FriendInfoSubsectionTitleData : IDataEntry
    {
        private FriendSubsection m_subSection;
        private bool m_isExpanded;

        public FriendInfoSubsectionTitleData(FriendSubsection subsection, bool isExpanded)
        {
            m_subSection = subsection;
            m_isExpanded = isExpanded;
        }

        public int GetPrefabIndexToDisplay()
        {
            return 0;
        }

        public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
        {
            string text = string.Empty;
            switch (m_subSection)
            {
                case FriendSubsection.Blocked:
                    text = StringUtil.TR("BlockedHeading", "NewFrontEndScene");
                    break;
                case FriendSubsection.FriendRequests:
                    text = StringUtil.TR("FriendRequestHeading", "NewFrontEndScene");
                    break;
                case FriendSubsection.InvitationsSent:
                    text = StringUtil.TR("InvitationsSentHeading", "NewFrontEndScene");
                    break;
                case FriendSubsection.Offline:
                    text = StringUtil.TR("OfflineHeading", "NewFrontEndScene");
                    break;
                case FriendSubsection.Online:
                    text = StringUtil.TR("OnlineHeading", "NewFrontEndScene");
                    break;
            }

            text = m_isExpanded ? text.Replace("+", "-") : text.Replace("-", "+");
            foreach (TextMeshProUGUI txt in UIEntry.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                txt.text = text;
            }

            _SelectableBtn component = UIEntry.GetComponent<_SelectableBtn>();
            if (component != null)
            {
                component.spriteController.callback = OnTitleClicked;
            }
        }

        public void OnTitleClicked(BaseEventData data)
        {
            UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuOpen);
            Get().ToggleSubSection(m_subSection);
            Get().UpdateFriendListSize();
        }
    }

    public class FriendInfoData : IDataEntry
    {
        public FriendInfo m_friendInfo;
        public FriendSubsection m_subSection;

        public FriendInfoData(FriendInfo info, FriendSubsection subSection)
        {
            m_friendInfo = info;
            m_subSection = subSection;
        }

        public int GetPrefabIndexToDisplay()
        {
            return 1;
        }

        public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
        {
            FriendListBannerEntry friendListEntry = UIEntry.GetComponent<FriendListBannerEntry>();
            if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
            {
                SocialComponent.FriendData orCreateFriendInfo = ClientGameManager.Get()
                    .GetPlayerAccountData()
                    .SocialComponent
                    .GetOrCreateFriendInfo(m_friendInfo.FriendAccountId);
                if (!m_friendInfo.IsOnline)
                {
                    m_friendInfo.BannerID = orCreateFriendInfo.LastSeenBackbroundID;
                    m_friendInfo.EmblemID = orCreateFriendInfo.LastSeenForegroundID;
                    m_friendInfo.TitleID = orCreateFriendInfo.LastSeenTitleID;
                    m_friendInfo.TitleLevel = orCreateFriendInfo.LastSeenTitleLevel;
                    m_friendInfo.RibbonID = orCreateFriendInfo.LastSeenRibbonID;
                    m_friendInfo.FriendNote = orCreateFriendInfo.LastSeenNote;
                }
            }

            friendListEntry.Setup(m_friendInfo, m_subSection);
        }
    }

    public TextMeshProUGUI m_playerName;
    public ScrollRect m_scrollView;
    public RectTransform m_friendListContainer;
    public float m_paddingBetweenSubsections;
    public RectTransform m_hasFriendsInListContainer;
    public RectTransform m_hasEmptyFriendListContainer;
    public FriendListHeader m_panelHeader;
    public FriendListFooter m_panelFooter;
    public _LargeScrollList m_friendScrollList;
    public Animator m_friendListAnimator;
    public TextMeshProUGUI m_errorText;
    public _SelectableBtn m_recruitButton;

    private List<IDataEntry> onlineFriends = new List<IDataEntry>();
    private List<IDataEntry> offlineFriends = new List<IDataEntry>();
    private List<IDataEntry> friendRequestedFriends = new List<IDataEntry>();
    private List<IDataEntry> invitationsSentFriends = new List<IDataEntry>();
    private List<IDataEntry> blockedFriends = new List<IDataEntry>();

    private bool[] SubsectionExpanded = new bool[(int)FriendSubsection.LAST];
    private List<long> friendsLoggedOff = new List<long>();
    private Mask m_scrollViewMask;
    private bool initialized;
    private bool m_isVisible;
    private List<string> m_loggedInFriends;
    private static FriendListPanel s_instance;

    public RectTransform m_bannerMenuContainer =>
        UIManager.Get().GetDefaultCanvas(SceneType.FrontEndNavPanel).gameObject.transform as RectTransform;

    public static FriendListPanel Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
    }

    private void OnDestroy()
    {
        if (this == s_instance)
        {
            s_instance = null;
        }

        if (ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
            RemoveHandleMessage();
        }
    }

    public void Init()
    {
        if (initialized)
        {
            return;
        }

        s_instance = this;
        initialized = true;
        friendRequestedFriends.Clear();
        onlineFriends.Clear();
        offlineFriends.Clear();
        invitationsSentFriends.Clear();
        blockedFriends.Clear();
        for (int i = 0; i < SubsectionExpanded.Length; i++)
        {
            SubsectionExpanded[i] = true;
        }

        m_scrollViewMask = m_scrollView.GetComponent<Mask>();
        m_loggedInFriends = new List<string>();
        SetVisible(false, true, true);
        m_scrollView.gameObject.AddComponent<_MouseEventPasser>().AddNewHandler(m_scrollView);
        ClientGameManager.Get().OnFriendStatusNotification += HandleFriendStatusNotification;
        HandleFriendStatusNotification(
            new FriendStatusNotification
            {
                FriendList = ClientGameManager.Get().FriendList
            });
        UpdateFriendListSize();
        _ButtonSwapSprite spriteController = m_recruitButton.spriteController;

        spriteController.callback = delegate { UIFrontEnd.Get().m_frontEndNavPanel.ToggleReferAFriend(); };
    }

    private void OnAccountDataUpdated(PersistedAccountData accountData)
    {
        foreach (FriendListBannerEntry entry in m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true))
        {
            long friendAccountId = entry.m_friendInfo.FriendAccountId;
            if (accountData.SocialComponent.FriendInfo.TryGetValue(
                    friendAccountId,
                    out SocialComponent.FriendData value))
            {
                entry.UpdateVisualInfo(
                    value.LastSeenTitleID,
                    value.LastSeenTitleLevel,
                    value.LastSeenBackbroundID,
                    value.LastSeenForegroundID,
                    value.LastSeenRibbonID,
                    value.LastSeenNote);
            }
        }
    }

    public void RemoveHandleMessage()
    {
        ClientGameManager.Get().OnFriendStatusNotification -= HandleFriendStatusNotification;
    }

    public int GetNumFriendRequests()
    {
        return friendRequestedFriends.Count;
    }

    public int GetNumOnlineFriends()
    {
        return onlineFriends.Count;
    }

    public int GetNumOfflineFriends()
    {
        return offlineFriends.Count;
    }

    public int GetNumInvitationsSent()
    {
        return invitationsSentFriends.Count;
    }

    public void ToggleSubSection(FriendSubsection type)
    {
        SubsectionExpanded[(int)type] = !SubsectionExpanded[(int)type];
        UpdateFriendListSize();
    }

    public static IDataEntry FriendInfoToBannerDataEntry(FriendInfo info, FriendSubsection subsection)
    {
        return new FriendInfoData(info, subsection);
    }

    public void UpdateFriendListSize()
    {
        List<IDataEntry> friendListEntries = new List<IDataEntry>();
        if (friendRequestedFriends.Count > 0)
        {
            bool isExpanded = SubsectionExpanded[(int)FriendSubsection.FriendRequests];
            friendListEntries.Add(new FriendInfoSubsectionTitleData(FriendSubsection.FriendRequests, isExpanded));
            if (isExpanded)
            {
                friendListEntries.AddRange(friendRequestedFriends);
            }
        }

        if (onlineFriends.Count > 0)
        {
            bool isExpanded = SubsectionExpanded[(int)FriendSubsection.Online];
            friendListEntries.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Online, isExpanded));
            if (isExpanded)
            {
                friendListEntries.AddRange(onlineFriends);
            }
        }

        if (offlineFriends.Count > 0)
        {
            bool isExpanded = SubsectionExpanded[(int)FriendSubsection.Offline];
            friendListEntries.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Offline, isExpanded));
            if (isExpanded)
            {
                friendListEntries.AddRange(offlineFriends);
            }
        }

        if (invitationsSentFriends.Count > 0)
        {
            bool isExpanded = SubsectionExpanded[(int)FriendSubsection.InvitationsSent];
            friendListEntries.Add(new FriendInfoSubsectionTitleData(FriendSubsection.InvitationsSent, isExpanded));
            if (isExpanded)
            {
                friendListEntries.AddRange(invitationsSentFriends);
            }
        }

        if (blockedFriends.Count > 0)
        {
            bool isExpanded = SubsectionExpanded[(int)FriendSubsection.Blocked];
            friendListEntries.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Blocked, isExpanded));
            if (isExpanded)
            {
                friendListEntries.AddRange(blockedFriends);
            }
        }

        m_friendScrollList.Setup(friendListEntries);
        m_friendScrollList.ScrollValueChanged(m_scrollView.verticalScrollbar.value);

        int num = onlineFriends.Count
                  + offlineFriends.Count
                  + friendRequestedFriends.Count
                  + invitationsSentFriends.Count
                  + blockedFriends.Count;
        m_scrollView.scrollSensitivity = 100f;

        if (num == 0)
        {
            UIManager.SetGameObjectActive(m_hasFriendsInListContainer, false);
            UIManager.SetGameObjectActive(m_hasEmptyFriendListContainer, true);
        }
        else
        {
            UIManager.SetGameObjectActive(m_hasFriendsInListContainer, true);
            UIManager.SetGameObjectActive(m_hasEmptyFriendListContainer, false);
        }
    }

    public void DisableScrollViewMask()
    {
        m_scrollViewMask.enabled = false;
    }

    public void EnableScrollViewMask()
    {
        m_scrollViewMask.enabled = true;
    }

    public void UpdateFriendBannerNote(FriendInfo friendInfo)
    {
        foreach (FriendListBannerEntry entry in m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true))
        {
            if (entry.m_friendInfo.FriendAccountId != friendInfo.FriendAccountId)
            {
                continue;
            }

            entry.m_playerName.text = friendInfo.FriendNote.IsNullOrEmpty()
                ? friendInfo.FriendHandle
                : $"{friendInfo.FriendHandle}({friendInfo.FriendNote})";
            return;
        }
    }

    public void AddFriend(FriendInfo friendInfo)
    {
        DisableScrollViewMask();
        switch (friendInfo.FriendStatus)
        {
            case FriendStatus.Friend when friendInfo.IsOnline:
            {
                onlineFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Online));
                if (!friendsLoggedOff.Contains(friendInfo.FriendAccountId))
                {
                    bool shownNotification = m_loggedInFriends.Exists(x => x == friendInfo.FriendHandle);
                    if (friendInfo.IsOnline
                        && AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get()
                        && !shownNotification)
                    {
                        TextConsole.Get().Write(
                            new TextConsole.Message
                            {
                                Text = string.Format(
                                    StringUtil.TR("FriendLoggedInClickToInvite", "FriendList"),
                                    friendInfo.FriendHandle,
                                    friendInfo.FriendHandle),
                                MessageType = ConsoleMessageType.SystemMessage
                            });
                    }

                    if (!shownNotification)
                    {
                        m_loggedInFriends.Add(friendInfo.FriendHandle);
                    }
                }

                break;
            }
            case FriendStatus.Friend:
                offlineFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Offline));
                break;
            case FriendStatus.RequestReceived:
                friendRequestedFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.FriendRequests));
                break;
            case FriendStatus.RequestSent:
                invitationsSentFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.InvitationsSent));
                break;
            case FriendStatus.Blocked:
                blockedFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Blocked));
                break;
        }

        EnableScrollViewMask();
    }

    public void RemoveFriend(long friendAccountId)
    {
        int num = 0;
        for (int i = 0; i < onlineFriends.Count; i++)
        {
            FriendInfo friendInfo = (onlineFriends[i] as FriendInfoData).m_friendInfo;
            if (friendInfo != null && friendAccountId == friendInfo.FriendAccountId)
            {
                num++;
                onlineFriends.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < offlineFriends.Count; i++)
        {
            FriendInfo friendInfo = (offlineFriends[i] as FriendInfoData).m_friendInfo;
            if (friendInfo != null && friendAccountId == friendInfo.FriendAccountId)
            {
                offlineFriends.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < friendRequestedFriends.Count; i++)
        {
            FriendInfo friendInfo = (friendRequestedFriends[i] as FriendInfoData).m_friendInfo;
            if (friendInfo != null && friendAccountId == friendInfo.FriendAccountId)
            {
                friendRequestedFriends.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < invitationsSentFriends.Count; i++)
        {
            FriendInfo friendInfo4 = (invitationsSentFriends[i] as FriendInfoData).m_friendInfo;
            if (friendInfo4 != null && friendAccountId == friendInfo4.FriendAccountId)
            {
                invitationsSentFriends.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < blockedFriends.Count; i++)
        {
            FriendInfo friendInfo = (blockedFriends[i] as FriendInfoData).m_friendInfo;
            if (friendInfo != null && friendAccountId == friendInfo.FriendAccountId)
            {
                blockedFriends.RemoveAt(i);
                i--;
            }
        }

        if (num > 0)
        {
            friendsLoggedOff.Add(friendAccountId);
        }
    }

    public void RequestToAddFriend(string friendHandle)
    {
        ClientGameManager.Get().UpdateFriend(
            friendHandle,
            0L,
            FriendOperation.Add,
            string.Empty,
            HandleFriendUpdateResponse);
    }

    public void RequestToBlockPlayer(FriendInfo friendInfo)
    {
        string title = StringUtil.TR("BlockPlayer", "FriendList");
        string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), friendInfo.FriendHandle);
        UIDialogPopupManager.OpenTwoButtonDialog(
            title,
            description,
            StringUtil.TR("Yes", "Global"),
            StringUtil.TR("No", "Global"),
            reference =>
            {
                ClientGameManager.Get().UpdateFriend(
                    null,
                    friendInfo.FriendAccountId,
                    FriendOperation.Block,
                    string.Empty,
                    HandleFriendUpdateResponse);
            });
    }

    public void RequestToRemoveFriend(FriendInfo friendInfo)
    {
        ClientGameManager.Get().UpdateFriend(
            null,
            friendInfo.FriendAccountId,
            FriendOperation.Remove,
            string.Empty,
            HandleFriendUpdateResponse);
    }

    public void RequestToAcceptRequest(FriendInfo friendInfo)
    {
        ClientGameManager.Get().UpdateFriend(
            null,
            friendInfo.FriendAccountId,
            FriendOperation.Accept,
            string.Empty,
            HandleFriendUpdateResponse);
    }

    public void RequestToRejectRequest(FriendInfo friendInfo)
    {
        ClientGameManager.Get().UpdateFriend(
            null,
            friendInfo.FriendAccountId,
            FriendOperation.Reject,
            string.Empty,
            HandleFriendUpdateResponse);
    }

    public void RequestToCancelRequest(FriendInfo friendInfo)
    {
        ClientGameManager.Get().UpdateFriend(
            null,
            friendInfo.FriendAccountId,
            FriendOperation.Reject,
            string.Empty,
            HandleFriendUpdateResponse);
    }

    public void RequestToSendMessage(FriendInfo friendInfo)
    {
        UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + friendInfo.FriendHandle + " ");
        SetVisible(false);
    }

    public void RequestToViewProfile(FriendInfo friendInfo)
    {
        Debug.Log("Request To View Profile: " + friendInfo.FriendHandle);
    }

    public void RequestToInviteToGame(FriendInfo friendInfo)
    {
        SlashCommands.Get().RunSlashCommand("/invitetogame", friendInfo.FriendHandle);
    }

    public void RequestToInviteToParty(FriendInfo friendInfo)
    {
        if (GameManager.Get() != null
            && GameManager.Get().GameConfig != null
            && GameManager.Get().GameConfig.GameType == GameType.Custom
            && GameManager.Get().GameStatus != GameStatus.Stopped)
        {
            SlashCommands.Get().RunSlashCommand("/invitetogame", friendInfo.FriendHandle);
            UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(
                new TextConsole.Message
                {
                    MessageType = ConsoleMessageType.SystemMessage,
                    Text = string.Format(StringUtil.TR("CustomGameInviteSent", "FriendList"), friendInfo.FriendHandle)
                },
                new TextConsole.AllowedEmojis
                {
                    emojis = new List<int>()
                });
        }
        else
        {
            SlashCommands.Get().RunSlashCommand("/invite", friendInfo.FriendHandle);
            UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(
                new TextConsole.Message
                {
                    MessageType = ConsoleMessageType.SystemMessage,
                    Text = string.Format(StringUtil.TR("GroupInviteSent", "FriendList"), friendInfo.FriendHandle)
                },
                new TextConsole.AllowedEmojis
                {
                    emojis = new List<int>()
                });
        }
    }

    public void RequestToObserveGame(FriendInfo friendInfo)
    {
        SlashCommands.Get().RunSlashCommand("/spectategame", friendInfo.FriendHandle);
    }

    public bool IsVisible()
    {
        return m_isVisible;
    }

    public void FriendPanelFadeOutDone()
    {
        UIManager.SetGameObjectActive(gameObject, false);
    }

    public void SetVisible(bool visible, bool replayAnim = false, bool ignoreSound = false)
    {
        if (m_isVisible == visible)
        {
            if (replayAnim)
            {
                DoDisplay(visible, replayAnim, ignoreSound);
            }

            return;
        }

        m_isVisible = visible;
        if (visible)
        {
            UIManager.SetGameObjectActive(gameObject, true);
            m_scrollView.verticalScrollbar.value = 1f;
            (m_scrollView.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
        }

        DoDisplay(visible, replayAnim, ignoreSound);
    }

    private void DoDisplay(bool visible, bool replayAnim = false, bool ignoreSound = false)
    {
        if (m_friendListAnimator != null && m_friendListAnimator.gameObject.activeInHierarchy)
        {
            if (visible)
            {
                if (!ignoreSound)
                {
                    UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
                }

                UIAnimationEventManager.Get().PlayAnimation(m_friendListAnimator, "FriendPanelDefaultIN", null);
            }
            else
            {
                if (!ignoreSound)
                {
                    UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
                }

                UIAnimationEventManager.Get().PlayAnimation(m_friendListAnimator, "FriendPanelDefaultOUT", null);
            }
        }
        else
        {
            UIManager.SetGameObjectActive(gameObject, visible);
        }

        if (UIFrontEnd.Get() != null)
        {
            UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn.SetSelected(visible);
        }
    }

    private void HandleFriendStatusNotification(FriendStatusNotification notification)
    {
        UIManager.SetGameObjectActive(m_errorText, notification.FriendList.IsError);
        if (!notification.FriendList.IsDelta)
        {
            friendRequestedFriends.Clear();
            onlineFriends.Clear();
            offlineFriends.Clear();
            invitationsSentFriends.Clear();
            blockedFriends.Clear();
            FriendList friendList = ClientGameManager.Get().FriendList;
            foreach (KeyValuePair<long, FriendInfo> friend in friendList.Friends)
            {
                switch (friend.Value.FriendStatus)
                {
                    case FriendStatus.Blocked:
                        blockedFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Blocked));
                        break;
                    case FriendStatus.RequestReceived:
                        friendRequestedFriends.Add(
                            FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.FriendRequests));
                        break;
                    case FriendStatus.RequestSent:
                        invitationsSentFriends.Add(
                            FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.InvitationsSent));
                        break;
                    case FriendStatus.Friend when friend.Value.IsOnline:
                        onlineFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Online));
                        break;
                    case FriendStatus.Friend:
                        offlineFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Offline));
                        break;
                }
            }
        }

        friendsLoggedOff.Clear();
        foreach (FriendInfo value in notification.FriendList.Friends.Values)
        {
            RemoveFriend(value.FriendAccountId);
            AddFriend(value);
        }

        UpdateFriendListSize();
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

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || UIDialogPopupManager.Get().IsDialogBoxOpen())
        {
            return;
        }

        bool callSetClose = true;
        bool isMenuClicked = EventSystem.current.currentSelectedGameObject != null
                             && EventSystem.current.currentSelectedGameObject
                                 .GetComponentInParent<FriendListBannerMenu>() != null;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
        {
            StandaloneInputModuleWithEventDataAccess component = EventSystem
                .current
                .gameObject
                .GetComponent<StandaloneInputModuleWithEventDataAccess>();
            if (component != null && component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
            {
                FriendListPanel componentInParent = component
                    .GetLastPointerEventDataPublic(-1)
                    .pointerEnter
                    .GetComponentInParent<FriendListPanel>();
                bool clickedBtn = false;
                if (componentInParent == null)
                {
                    _SelectableBtn btn = component
                        .GetLastPointerEventDataPublic(-1)
                        .pointerEnter
                        .GetComponentInParent<_SelectableBtn>();
                    if (UIFrontEnd.Get() != null)
                    {
                        while (btn != null)
                        {
                            _SelectableBtn friendMenuToggleBtn =
                                UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn;
                            if (btn == friendMenuToggleBtn)
                            {
                                clickedBtn = true;
                                break;
                            }

                            btn = btn
                                .transform
                                .parent
                                .GetComponentInParent<_SelectableBtn>();
                        }
                    }
                }

                if (componentInParent != null || clickedBtn || isMenuClicked)
                {
                    callSetClose = false;
                }
            }
        }

        if (callSetClose && m_isVisible)
        {
            UIFrontEnd.Get().TogglePlayerFriendListVisibility();
        }
    }
}