using LobbyGameClientMessages;
using System.Collections.Generic;
using System.Text;
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
			TextMeshProUGUI[] componentsInChildren = UIEntry.GetComponentsInChildren<TextMeshProUGUI>(true);
			string text = string.Empty;
			if (m_subSection == FriendSubsection.Blocked)
			{
				text = StringUtil.TR("BlockedHeading", "NewFrontEndScene");
			}
			else if (m_subSection == FriendSubsection.FriendRequests)
			{
				text = StringUtil.TR("FriendRequestHeading", "NewFrontEndScene");
			}
			else if (m_subSection == FriendSubsection.InvitationsSent)
			{
				text = StringUtil.TR("InvitationsSentHeading", "NewFrontEndScene");
			}
			else if (m_subSection == FriendSubsection.Offline)
			{
				text = StringUtil.TR("OfflineHeading", "NewFrontEndScene");
			}
			else if (m_subSection == FriendSubsection.Online)
			{
				text = StringUtil.TR("OnlineHeading", "NewFrontEndScene");
			}
			text = ((!m_isExpanded) ? text.Replace("-", "+") : text.Replace("+", "-"));
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].text = text;
			}
			while (true)
			{
				_SelectableBtn component = UIEntry.GetComponent<_SelectableBtn>();
				if (component != null)
				{
					while (true)
					{
						component.spriteController.callback = OnTitleClicked;
						return;
					}
				}
				return;
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
			FriendListBannerEntry component = UIEntry.GetComponent<FriendListBannerEntry>();
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				SocialComponent.FriendData orCreateFriendInfo = ClientGameManager.Get().GetPlayerAccountData().SocialComponent.GetOrCreateFriendInfo(m_friendInfo.FriendAccountId);
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
			component.Setup(m_friendInfo, m_subSection);
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

	private bool[] SubsectionExpanded = new bool[5];

	private List<long> friendsLoggedOff = new List<long>();

	private Mask m_scrollViewMask;

	private bool initialized;

	private bool m_isVisible;

	private List<string> m_loggedInFriends;

	private static FriendListPanel s_instance;

	public RectTransform m_bannerMenuContainer
	{
		get
		{
			RectTransform rectTransform = null;
			return UIManager.Get().GetDefaultCanvas(SceneType.FrontEndNavPanel).gameObject.transform as RectTransform;
		}
	}

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
		_MouseEventPasser mouseEventPasser = m_scrollView.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(m_scrollView);
		ClientGameManager.Get().OnFriendStatusNotification += HandleFriendStatusNotification;
		FriendStatusNotification friendStatusNotification = new FriendStatusNotification();
		friendStatusNotification.FriendList = ClientGameManager.Get().FriendList;
		HandleFriendStatusNotification(friendStatusNotification);
		UpdateFriendListSize();
		_ButtonSwapSprite spriteController = m_recruitButton.spriteController;
		
		spriteController.callback = delegate
			{
				UIFrontEnd.Get().m_frontEndNavPanel.ToggleReferAFriend();
			};
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		FriendListBannerEntry[] componentsInChildren = m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			long friendAccountId = componentsInChildren[i].m_friendInfo.FriendAccountId;
			SocialComponent.FriendData value;
			if (accountData.SocialComponent.FriendInfo.TryGetValue(friendAccountId, out value))
			{
				componentsInChildren[i].UpdateVisualInfo(value.LastSeenTitleID, value.LastSeenTitleLevel, value.LastSeenBackbroundID, value.LastSeenForegroundID, value.LastSeenRibbonID, value.LastSeenNote);
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
		List<IDataEntry> list = new List<IDataEntry>();
		if (friendRequestedFriends.Count > 0)
		{
			bool flag = SubsectionExpanded[0];
			list.Add(new FriendInfoSubsectionTitleData(FriendSubsection.FriendRequests, flag));
			if (flag)
			{
				list.AddRange(friendRequestedFriends);
			}
		}
		if (onlineFriends.Count > 0)
		{
			bool flag2 = SubsectionExpanded[1];
			list.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Online, flag2));
			if (flag2)
			{
				list.AddRange(onlineFriends);
			}
		}
		if (offlineFriends.Count > 0)
		{
			bool flag3 = SubsectionExpanded[2];
			list.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Offline, flag3));
			if (flag3)
			{
				list.AddRange(offlineFriends);
			}
		}
		if (invitationsSentFriends.Count > 0)
		{
			bool flag4 = SubsectionExpanded[3];
			list.Add(new FriendInfoSubsectionTitleData(FriendSubsection.InvitationsSent, flag4));
			if (flag4)
			{
				list.AddRange(invitationsSentFriends);
			}
		}
		if (blockedFriends.Count > 0)
		{
			bool flag5 = SubsectionExpanded[4];
			list.Add(new FriendInfoSubsectionTitleData(FriendSubsection.Blocked, flag5));
			if (flag5)
			{
				list.AddRange(blockedFriends);
			}
		}
		m_friendScrollList.Setup(list);
		m_friendScrollList.ScrollValueChanged(m_scrollView.verticalScrollbar.value);
		int num = onlineFriends.Count + offlineFriends.Count + friendRequestedFriends.Count + invitationsSentFriends.Count + blockedFriends.Count;
		m_scrollView.scrollSensitivity = 100f;
		if (num == 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_hasFriendsInListContainer, false);
					UIManager.SetGameObjectActive(m_hasEmptyFriendListContainer, true);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_hasFriendsInListContainer, true);
		UIManager.SetGameObjectActive(m_hasEmptyFriendListContainer, false);
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
		FriendListBannerEntry[] componentsInChildren = m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].m_friendInfo.FriendAccountId != friendInfo.FriendAccountId)
			{
				continue;
			}
			while (true)
			{
				string text = friendInfo.FriendHandle;
				if (!friendInfo.FriendNote.IsNullOrEmpty())
				{
					text = new StringBuilder().Append(friendInfo.FriendHandle).Append("(").Append(friendInfo.FriendNote).Append(")").ToString();
				}
				componentsInChildren[i].m_playerName.text = text;
				return;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void AddFriend(FriendInfo friendInfo)
	{
		DisableScrollViewMask();
		if (friendInfo.FriendStatus == FriendStatus.Friend)
		{
			if (friendInfo.IsOnline)
			{
				onlineFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Online));
				if (!friendsLoggedOff.Contains(friendInfo.FriendAccountId))
				{
					bool flag = m_loggedInFriends.Exists((string x) => x == friendInfo.FriendHandle);
					if (friendInfo.IsOnline)
					{
						if (AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get())
						{
							if (!flag)
							{
								TextConsole.Get().Write(new TextConsole.Message
								{
									Text = string.Format(StringUtil.TR("FriendLoggedInClickToInvite", "FriendList"), friendInfo.FriendHandle, friendInfo.FriendHandle),
									MessageType = ConsoleMessageType.SystemMessage
								});
							}
						}
					}
					if (!flag)
					{
						m_loggedInFriends.Add(friendInfo.FriendHandle);
					}
				}
			}
			else
			{
				offlineFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Offline));
			}
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestReceived)
		{
			friendRequestedFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.FriendRequests));
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestSent)
		{
			invitationsSentFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.InvitationsSent));
		}
		else if (friendInfo.FriendStatus == FriendStatus.Blocked)
		{
			blockedFriends.Add(FriendInfoToBannerDataEntry(friendInfo, FriendSubsection.Blocked));
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
		while (true)
		{
			for (int j = 0; j < offlineFriends.Count; j++)
			{
				FriendInfo friendInfo2 = (offlineFriends[j] as FriendInfoData).m_friendInfo;
				if (friendInfo2 == null)
				{
					continue;
				}
				if (friendAccountId == friendInfo2.FriendAccountId)
				{
					offlineFriends.RemoveAt(j);
					j--;
				}
			}
			while (true)
			{
				for (int k = 0; k < friendRequestedFriends.Count; k++)
				{
					FriendInfo friendInfo3 = (friendRequestedFriends[k] as FriendInfoData).m_friendInfo;
					if (friendInfo3 != null)
					{
						if (friendAccountId == friendInfo3.FriendAccountId)
						{
							friendRequestedFriends.RemoveAt(k);
							k--;
						}
					}
				}
				for (int l = 0; l < invitationsSentFriends.Count; l++)
				{
					FriendInfo friendInfo4 = (invitationsSentFriends[l] as FriendInfoData).m_friendInfo;
					if (friendInfo4 != null && friendAccountId == friendInfo4.FriendAccountId)
					{
						invitationsSentFriends.RemoveAt(l);
						l--;
					}
				}
				while (true)
				{
					for (int m = 0; m < blockedFriends.Count; m++)
					{
						FriendInfo friendInfo5 = (blockedFriends[m] as FriendInfoData).m_friendInfo;
						if (friendInfo5 == null)
						{
							continue;
						}
						if (friendAccountId == friendInfo5.FriendAccountId)
						{
							blockedFriends.RemoveAt(m);
							m--;
						}
					}
					if (num > 0)
					{
						while (true)
						{
							friendsLoggedOff.Add(friendAccountId);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void RequestToAddFriend(string friendHandle)
	{
		ClientGameManager.Get().UpdateFriend(friendHandle, 0L, FriendOperation.Add, string.Empty, HandleFriendUpdateResponse);
	}

	public void RequestToBlockPlayer(FriendInfo friendInfo)
	{
		string title = StringUtil.TR("BlockPlayer", "FriendList");
		string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), friendInfo.FriendHandle);
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
		{
			ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Block, string.Empty, HandleFriendUpdateResponse);
		});
	}

	public void RequestToRemoveFriend(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Remove, string.Empty, HandleFriendUpdateResponse);
	}

	public void RequestToAcceptRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Accept, string.Empty, HandleFriendUpdateResponse);
	}

	public void RequestToRejectRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Reject, string.Empty, HandleFriendUpdateResponse);
	}

	public void RequestToCancelRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Reject, string.Empty, HandleFriendUpdateResponse);
	}

	public void RequestToSendMessage(FriendInfo friendInfo)
	{
		UIFrontEnd.Get().m_frontEndChatConsole.SelectInput(new StringBuilder().Append("/whisper ").Append(friendInfo.FriendHandle).Append(" ").ToString());
		SetVisible(false);
	}

	public void RequestToViewProfile(FriendInfo friendInfo)
	{
		Debug.Log(new StringBuilder().Append("Request To View Profile: ").Append(friendInfo.FriendHandle).ToString());
	}

	public void RequestToInviteToGame(FriendInfo friendInfo)
	{
		SlashCommands.Get().RunSlashCommand("/invitetogame", friendInfo.FriendHandle);
	}

	public void RequestToInviteToParty(FriendInfo friendInfo)
	{
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Custom)
				{
					TextConsole.AllowedEmojis allowedEmojis = default(TextConsole.AllowedEmojis);
					if (GameManager.Get().GameStatus != GameStatus.Stopped)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								SlashCommands.Get().RunSlashCommand("/invitetogame", friendInfo.FriendHandle);
								TextConsole.Message message = default(TextConsole.Message);
								message.MessageType = ConsoleMessageType.SystemMessage;
								message.Text = string.Format(StringUtil.TR("CustomGameInviteSent", "FriendList"), friendInfo.FriendHandle);
								allowedEmojis.emojis = new List<int>();
								UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(message, allowedEmojis);
								return;
							}
							}
						}
					}
				}
			}
		}
		SlashCommands.Get().RunSlashCommand("/invite", friendInfo.FriendHandle);
		TextConsole.Message message2 = default(TextConsole.Message);
		message2.MessageType = ConsoleMessageType.SystemMessage;
		message2.Text = string.Format(StringUtil.TR("GroupInviteSent", "FriendList"), friendInfo.FriendHandle);
		TextConsole.AllowedEmojis allowedEmojis2 = default(TextConsole.AllowedEmojis);
		allowedEmojis2.emojis = new List<int>();
		UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(message2, allowedEmojis2);
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
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	public void SetVisible(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (m_isVisible == visible)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (replayAnim)
					{
						DoDisplay(visible, replayAnim, ignoreSound);
					}
					return;
				}
			}
		}
		m_isVisible = visible;
		if (visible)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
			m_scrollView.verticalScrollbar.value = 1f;
			(m_scrollView.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
		}
		DoDisplay(visible, replayAnim, ignoreSound);
	}

	private void DoDisplay(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (m_friendListAnimator != null)
		{
			if (m_friendListAnimator.gameObject.activeInHierarchy)
			{
				if (visible)
				{
					if (!ignoreSound)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
					}
					UIAnimationEventManager.Get().PlayAnimation(m_friendListAnimator, "FriendPanelDefaultIN", null, string.Empty);
				}
				else
				{
					if (!ignoreSound)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
					}
					UIAnimationEventManager.Get().PlayAnimation(m_friendListAnimator, "FriendPanelDefaultOUT", null, string.Empty);
				}
				goto IL_00db;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, visible);
		goto IL_00db;
		IL_00db:
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn.SetSelected(visible, false, string.Empty, string.Empty);
			return;
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
				if (friend.Value.FriendStatus == FriendStatus.Blocked)
				{
					blockedFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Blocked));
				}
				else if (friend.Value.FriendStatus == FriendStatus.RequestReceived)
				{
					friendRequestedFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.FriendRequests));
				}
				else if (friend.Value.FriendStatus == FriendStatus.RequestSent)
				{
					invitationsSentFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.InvitationsSent));
				}
				else if (friend.Value.FriendStatus == FriendStatus.Friend)
				{
					if (friend.Value.IsOnline)
					{
						onlineFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Online));
					}
					else
					{
						offlineFriends.Add(FriendInfoToBannerDataEntry(friend.Value, FriendSubsection.Offline));
					}
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
		while (true)
		{
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
				while (true)
				{
					UIDialogPopupManager.OpenOneButtonDialog(string.Empty, text, StringUtil.TR("Ok", "Global"));
					return;
				}
			}
			return;
		}
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		while (true)
		{
			if (UIDialogPopupManager.Get().IsDialogBoxOpen())
			{
				return;
			}
			bool flag = true;
			int num;
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				num = ((EventSystem.current.currentSelectedGameObject.GetComponentInParent<FriendListBannerMenu>() != null) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag2 = (byte)num != 0;
			if (EventSystem.current != null)
			{
				if (EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
					{
						if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
						{
							FriendListPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<FriendListPanel>();
							bool flag3 = false;
							if (componentInParent == null)
							{
								_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
								if (UIFrontEnd.Get() != null)
								{
									while (true)
									{
										if (componentInParent2 != null)
										{
											_SelectableBtn friendMenuToggleBtn = UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn;
											if (componentInParent2 == friendMenuToggleBtn)
											{
												flag3 = true;
												break;
											}
											componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
											continue;
										}
										break;
									}
								}
							}
							if (!(componentInParent != null) && !flag3)
							{
								if (!flag2)
								{
									goto IL_01de;
								}
							}
							flag = false;
						}
					}
				}
			}
			goto IL_01de;
			IL_01de:
			if (flag && m_isVisible)
			{
				while (true)
				{
					UIFrontEnd.Get().TogglePlayerFriendListVisibility();
					return;
				}
			}
			return;
		}
	}
}
