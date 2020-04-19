using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListPanel : MonoBehaviour
{
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
			return UIManager.Get().GetDefaultCanvas(SceneType.FrontEndNavPanel).gameObject.transform as RectTransform;
		}
	}

	public static FriendListPanel Get()
	{
		return FriendListPanel.s_instance;
	}

	private void Awake()
	{
		this.Init();
	}

	private void Start()
	{
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (this == FriendListPanel.s_instance)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.OnDestroy()).MethodHandle;
			}
			FriendListPanel.s_instance = null;
		}
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			this.RemoveHandleMessage();
		}
	}

	public void Init()
	{
		if (!this.initialized)
		{
			FriendListPanel.s_instance = this;
			this.initialized = true;
			this.friendRequestedFriends.Clear();
			this.onlineFriends.Clear();
			this.offlineFriends.Clear();
			this.invitationsSentFriends.Clear();
			this.blockedFriends.Clear();
			for (int i = 0; i < this.SubsectionExpanded.Length; i++)
			{
				this.SubsectionExpanded[i] = true;
			}
			this.m_scrollViewMask = this.m_scrollView.GetComponent<Mask>();
			this.m_loggedInFriends = new List<string>();
			this.SetVisible(false, true, true);
			_MouseEventPasser mouseEventPasser = this.m_scrollView.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(this.m_scrollView);
			ClientGameManager.Get().OnFriendStatusNotification += this.HandleFriendStatusNotification;
			this.HandleFriendStatusNotification(new FriendStatusNotification
			{
				FriendList = ClientGameManager.Get().FriendList
			});
			this.UpdateFriendListSize();
			_ButtonSwapSprite spriteController = this.m_recruitButton.spriteController;
			if (FriendListPanel.<>f__am$cache0 == null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.Init()).MethodHandle;
				}
				FriendListPanel.<>f__am$cache0 = delegate(BaseEventData data)
				{
					UIFrontEnd.Get().m_frontEndNavPanel.ToggleReferAFriend();
				};
			}
			spriteController.callback = FriendListPanel.<>f__am$cache0;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		FriendListBannerEntry[] componentsInChildren = this.m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			long friendAccountId = componentsInChildren[i].m_friendInfo.FriendAccountId;
			SocialComponent.FriendData friendData;
			if (accountData.SocialComponent.FriendInfo.TryGetValue(friendAccountId, out friendData))
			{
				componentsInChildren[i].UpdateVisualInfo(friendData.LastSeenTitleID, friendData.LastSeenTitleLevel, friendData.LastSeenBackbroundID, friendData.LastSeenForegroundID, friendData.LastSeenRibbonID, friendData.LastSeenNote);
			}
		}
	}

	public void RemoveHandleMessage()
	{
		ClientGameManager.Get().OnFriendStatusNotification -= this.HandleFriendStatusNotification;
	}

	public int GetNumFriendRequests()
	{
		return this.friendRequestedFriends.Count;
	}

	public int GetNumOnlineFriends()
	{
		return this.onlineFriends.Count;
	}

	public int GetNumOfflineFriends()
	{
		return this.offlineFriends.Count;
	}

	public int GetNumInvitationsSent()
	{
		return this.invitationsSentFriends.Count;
	}

	public void ToggleSubSection(FriendListPanel.FriendSubsection type)
	{
		this.SubsectionExpanded[(int)type] = !this.SubsectionExpanded[(int)type];
		this.UpdateFriendListSize();
	}

	public static IDataEntry FriendInfoToBannerDataEntry(FriendInfo info, FriendListPanel.FriendSubsection subsection)
	{
		return new FriendListPanel.FriendInfoData(info, subsection);
	}

	public void UpdateFriendListSize()
	{
		List<IDataEntry> list = new List<IDataEntry>();
		if (this.friendRequestedFriends.Count > 0)
		{
			bool flag = this.SubsectionExpanded[0];
			list.Add(new FriendListPanel.FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection.FriendRequests, flag));
			if (flag)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.UpdateFriendListSize()).MethodHandle;
				}
				list.AddRange(this.friendRequestedFriends);
			}
		}
		if (this.onlineFriends.Count > 0)
		{
			bool flag2 = this.SubsectionExpanded[1];
			list.Add(new FriendListPanel.FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection.Online, flag2));
			if (flag2)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				list.AddRange(this.onlineFriends);
			}
		}
		if (this.offlineFriends.Count > 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag3 = this.SubsectionExpanded[2];
			list.Add(new FriendListPanel.FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection.Offline, flag3));
			if (flag3)
			{
				list.AddRange(this.offlineFriends);
			}
		}
		if (this.invitationsSentFriends.Count > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag4 = this.SubsectionExpanded[3];
			list.Add(new FriendListPanel.FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection.InvitationsSent, flag4));
			if (flag4)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				list.AddRange(this.invitationsSentFriends);
			}
		}
		if (this.blockedFriends.Count > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag5 = this.SubsectionExpanded[4];
			list.Add(new FriendListPanel.FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection.Blocked, flag5));
			if (flag5)
			{
				list.AddRange(this.blockedFriends);
			}
		}
		this.m_friendScrollList.Setup(list, 0);
		this.m_friendScrollList.ScrollValueChanged(this.m_scrollView.verticalScrollbar.value);
		int num = this.onlineFriends.Count + this.offlineFriends.Count + this.friendRequestedFriends.Count + this.invitationsSentFriends.Count + this.blockedFriends.Count;
		this.m_scrollView.scrollSensitivity = 100f;
		if (num == 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_hasFriendsInListContainer, false, null);
			UIManager.SetGameObjectActive(this.m_hasEmptyFriendListContainer, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_hasFriendsInListContainer, true, null);
			UIManager.SetGameObjectActive(this.m_hasEmptyFriendListContainer, false, null);
		}
	}

	public void DisableScrollViewMask()
	{
		this.m_scrollViewMask.enabled = false;
	}

	public void EnableScrollViewMask()
	{
		this.m_scrollViewMask.enabled = true;
	}

	public void UpdateFriendBannerNote(FriendInfo friendInfo)
	{
		FriendListBannerEntry[] componentsInChildren = this.m_friendScrollList.GetComponentsInChildren<FriendListBannerEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].m_friendInfo.FriendAccountId == friendInfo.FriendAccountId)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.UpdateFriendBannerNote(FriendInfo)).MethodHandle;
				}
				string text = friendInfo.FriendHandle;
				if (!friendInfo.FriendNote.IsNullOrEmpty())
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					text = string.Format("{0}({1})", friendInfo.FriendHandle, friendInfo.FriendNote);
				}
				componentsInChildren[i].m_playerName.text = text;
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void AddFriend(FriendInfo friendInfo)
	{
		this.DisableScrollViewMask();
		if (friendInfo.FriendStatus == FriendStatus.Friend)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.AddFriend(FriendInfo)).MethodHandle;
			}
			if (friendInfo.IsOnline)
			{
				this.onlineFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(friendInfo, FriendListPanel.FriendSubsection.Online));
				if (!this.friendsLoggedOff.Contains(friendInfo.FriendAccountId))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag = this.m_loggedInFriends.Exists((string x) => x == friendInfo.FriendHandle);
					if (friendInfo.IsOnline)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get())
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!flag)
							{
								TextConsole.Get().Write(new TextConsole.Message
								{
									Text = string.Format(StringUtil.TR("FriendLoggedInClickToInvite", "FriendList"), friendInfo.FriendHandle, friendInfo.FriendHandle),
									MessageType = ConsoleMessageType.SystemMessage
								}, null);
							}
						}
					}
					if (!flag)
					{
						this.m_loggedInFriends.Add(friendInfo.FriendHandle);
					}
				}
			}
			else
			{
				this.offlineFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(friendInfo, FriendListPanel.FriendSubsection.Offline));
			}
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestReceived)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.friendRequestedFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(friendInfo, FriendListPanel.FriendSubsection.FriendRequests));
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestSent)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.invitationsSentFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(friendInfo, FriendListPanel.FriendSubsection.InvitationsSent));
		}
		else if (friendInfo.FriendStatus == FriendStatus.Blocked)
		{
			this.blockedFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(friendInfo, FriendListPanel.FriendSubsection.Blocked));
		}
		this.EnableScrollViewMask();
	}

	public void RemoveFriend(long friendAccountId)
	{
		int num = 0;
		for (int i = 0; i < this.onlineFriends.Count; i++)
		{
			FriendInfo friendInfo = (this.onlineFriends[i] as FriendListPanel.FriendInfoData).m_friendInfo;
			if (friendInfo != null && friendAccountId == friendInfo.FriendAccountId)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.RemoveFriend(long)).MethodHandle;
				}
				num++;
				this.onlineFriends.RemoveAt(i);
				i--;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = 0; j < this.offlineFriends.Count; j++)
		{
			FriendInfo friendInfo2 = (this.offlineFriends[j] as FriendListPanel.FriendInfoData).m_friendInfo;
			if (friendInfo2 != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (friendAccountId == friendInfo2.FriendAccountId)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.offlineFriends.RemoveAt(j);
					j--;
				}
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = 0; k < this.friendRequestedFriends.Count; k++)
		{
			FriendInfo friendInfo3 = (this.friendRequestedFriends[k] as FriendListPanel.FriendInfoData).m_friendInfo;
			if (friendInfo3 != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (friendAccountId == friendInfo3.FriendAccountId)
				{
					this.friendRequestedFriends.RemoveAt(k);
					k--;
				}
			}
		}
		for (int l = 0; l < this.invitationsSentFriends.Count; l++)
		{
			FriendInfo friendInfo4 = (this.invitationsSentFriends[l] as FriendListPanel.FriendInfoData).m_friendInfo;
			if (friendInfo4 != null && friendAccountId == friendInfo4.FriendAccountId)
			{
				this.invitationsSentFriends.RemoveAt(l);
				l--;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int m = 0; m < this.blockedFriends.Count; m++)
		{
			FriendInfo friendInfo5 = (this.blockedFriends[m] as FriendListPanel.FriendInfoData).m_friendInfo;
			if (friendInfo5 != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (friendAccountId == friendInfo5.FriendAccountId)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.blockedFriends.RemoveAt(m);
					m--;
				}
			}
		}
		if (num > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.friendsLoggedOff.Add(friendAccountId);
		}
	}

	public void RequestToAddFriend(string friendHandle)
	{
		ClientGameManager.Get().UpdateFriend(friendHandle, 0L, FriendOperation.Add, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
	}

	public void RequestToBlockPlayer(FriendInfo friendInfo)
	{
		string title = StringUtil.TR("BlockPlayer", "FriendList");
		string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), friendInfo.FriendHandle);
		UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
		{
			ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Block, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
		}, null, false, false);
	}

	public void RequestToRemoveFriend(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Remove, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
	}

	public void RequestToAcceptRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Accept, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
	}

	public void RequestToRejectRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Reject, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
	}

	public void RequestToCancelRequest(FriendInfo friendInfo)
	{
		ClientGameManager.Get().UpdateFriend(null, friendInfo.FriendAccountId, FriendOperation.Reject, string.Empty, new Action<FriendUpdateResponse>(this.HandleFriendUpdateResponse));
	}

	public void RequestToSendMessage(FriendInfo friendInfo)
	{
		UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + friendInfo.FriendHandle + " ");
		this.SetVisible(false, false, false);
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
		if (GameManager.Get() != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.RequestToInviteToParty(FriendInfo)).MethodHandle;
			}
			if (GameManager.Get().GameConfig != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get().GameConfig.GameType == GameType.Custom)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameManager.Get().GameStatus != GameStatus.Stopped)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						SlashCommands.Get().RunSlashCommand("/invitetogame", friendInfo.FriendHandle);
						TextConsole.Message message = default(TextConsole.Message);
						message.MessageType = ConsoleMessageType.SystemMessage;
						message.Text = string.Format(StringUtil.TR("CustomGameInviteSent", "FriendList"), friendInfo.FriendHandle);
						TextConsole.AllowedEmojis allowedEmojis;
						allowedEmojis.emojis = new List<int>();
						UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(message, allowedEmojis);
						return;
					}
				}
			}
		}
		SlashCommands.Get().RunSlashCommand("/invite", friendInfo.FriendHandle);
		TextConsole.Message message2 = default(TextConsole.Message);
		message2.MessageType = ConsoleMessageType.SystemMessage;
		message2.Text = string.Format(StringUtil.TR("GroupInviteSent", "FriendList"), friendInfo.FriendHandle);
		TextConsole.AllowedEmojis allowedEmojis2;
		allowedEmojis2.emojis = new List<int>();
		UIFrontEnd.Get().m_frontEndChatConsole.HandleMessage(message2, allowedEmojis2);
	}

	public void RequestToObserveGame(FriendInfo friendInfo)
	{
		SlashCommands.Get().RunSlashCommand("/spectategame", friendInfo.FriendHandle);
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public void FriendPanelFadeOutDone()
	{
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	public void SetVisible(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (this.m_isVisible == visible)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.SetVisible(bool, bool, bool)).MethodHandle;
			}
			if (replayAnim)
			{
				this.DoDisplay(visible, replayAnim, ignoreSound);
			}
			return;
		}
		this.m_isVisible = visible;
		if (visible)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			this.m_scrollView.verticalScrollbar.value = 1f;
			(this.m_scrollView.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
		}
		this.DoDisplay(visible, replayAnim, ignoreSound);
	}

	private void DoDisplay(bool visible, bool replayAnim = false, bool ignoreSound = false)
	{
		if (this.m_friendListAnimator != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.DoDisplay(bool, bool, bool)).MethodHandle;
			}
			if (this.m_friendListAnimator.gameObject.activeInHierarchy)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (visible)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!ignoreSound)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_friendListAnimator, "FriendPanelDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				}
				else
				{
					if (!ignoreSound)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_friendListAnimator, "FriendPanelDefaultOUT", null, string.Empty, 0, 0f, true, false, null, null);
				}
				goto IL_DB;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, visible, null);
		IL_DB:
		if (UIFrontEnd.Get() != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn.SetSelected(visible, false, string.Empty, string.Empty);
		}
	}

	private void HandleFriendStatusNotification(FriendStatusNotification notification)
	{
		UIManager.SetGameObjectActive(this.m_errorText, notification.FriendList.IsError, null);
		if (!notification.FriendList.IsDelta)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.HandleFriendStatusNotification(FriendStatusNotification)).MethodHandle;
			}
			this.friendRequestedFriends.Clear();
			this.onlineFriends.Clear();
			this.offlineFriends.Clear();
			this.invitationsSentFriends.Clear();
			this.blockedFriends.Clear();
			FriendList friendList = ClientGameManager.Get().FriendList;
			foreach (KeyValuePair<long, FriendInfo> keyValuePair in friendList.Friends)
			{
				if (keyValuePair.Value.FriendStatus == FriendStatus.Blocked)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.blockedFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(keyValuePair.Value, FriendListPanel.FriendSubsection.Blocked));
				}
				else if (keyValuePair.Value.FriendStatus == FriendStatus.RequestReceived)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.friendRequestedFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(keyValuePair.Value, FriendListPanel.FriendSubsection.FriendRequests));
				}
				else if (keyValuePair.Value.FriendStatus == FriendStatus.RequestSent)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.invitationsSentFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(keyValuePair.Value, FriendListPanel.FriendSubsection.InvitationsSent));
				}
				else if (keyValuePair.Value.FriendStatus == FriendStatus.Friend)
				{
					if (keyValuePair.Value.IsOnline)
					{
						this.onlineFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(keyValuePair.Value, FriendListPanel.FriendSubsection.Online));
					}
					else
					{
						this.offlineFriends.Add(FriendListPanel.FriendInfoToBannerDataEntry(keyValuePair.Value, FriendListPanel.FriendSubsection.Offline));
					}
				}
			}
		}
		this.friendsLoggedOff.Clear();
		foreach (FriendInfo friendInfo in notification.FriendList.Friends.Values)
		{
			this.RemoveFriend(friendInfo.FriendAccountId);
			this.AddFriend(friendInfo);
		}
		this.UpdateFriendListSize();
	}

	private void HandleFriendUpdateResponse(FriendUpdateResponse response)
	{
		if (!response.Success)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.HandleFriendUpdateResponse(FriendUpdateResponse)).MethodHandle;
			}
			if (response.LocalizedFailure != null)
			{
				response.ErrorMessage = response.LocalizedFailure.ToString();
			}
			else if (response.ErrorMessage.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				response.ErrorMessage = StringUtil.TR("ServerError", "Global");
			}
			string text = null;
			switch (response.FriendOperation)
			{
			case FriendOperation.Add:
				text = string.Format(StringUtil.TR("FailedFriendAdd", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Remove:
				text = string.Format(StringUtil.TR("FailedFriendRemove", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Accept:
				text = string.Format(StringUtil.TR("FailedFriendAccept", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Reject:
				text = string.Format(StringUtil.TR("FailedFriendReject", "FriendList"), response.ErrorMessage);
				break;
			case FriendOperation.Block:
				text = string.Format(StringUtil.TR("FailedFriendBlock", "FriendList"), response.ErrorMessage);
				break;
			}
			if (!text.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				UIDialogPopupManager.OpenOneButtonDialog(string.Empty, text, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.Update()).MethodHandle;
			}
			if (!UIDialogPopupManager.Get().IsDialogBoxOpen())
			{
				bool flag = true;
				bool flag2;
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = (EventSystem.current.currentSelectedGameObject.GetComponentInParent<FriendListBannerMenu>() != null);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (EventSystem.current != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (EventSystem.current.IsPointerOverGameObject(-1))
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
						if (component != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								FriendListPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<FriendListPanel>();
								bool flag4 = false;
								if (componentInParent == null)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
									if (UIFrontEnd.Get() != null)
									{
										for (;;)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										while (componentInParent2 != null)
										{
											_SelectableBtn friendMenuToggleBtn = UIFrontEnd.Get().m_playerPanel.m_friendMenuToggleBtn;
											if (componentInParent2 == friendMenuToggleBtn)
											{
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													break;
												}
												flag4 = true;
												goto IL_1B8;
											}
											componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
										}
										for (;;)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
									}
								}
								IL_1B8:
								if (!(componentInParent != null) && !flag4)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!flag3)
									{
										goto IL_1DE;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								flag = false;
							}
						}
					}
				}
				IL_1DE:
				if (flag && this.m_isVisible)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					UIFrontEnd.Get().TogglePlayerFriendListVisibility();
				}
			}
		}
	}

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
		private FriendListPanel.FriendSubsection m_subSection;

		private bool m_isExpanded;

		public FriendInfoSubsectionTitleData(FriendListPanel.FriendSubsection subsection, bool isExpanded)
		{
			this.m_subSection = subsection;
			this.m_isExpanded = isExpanded;
		}

		public int GetPrefabIndexToDisplay()
		{
			return 0;
		}

		public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
		{
			TextMeshProUGUI[] componentsInChildren = UIEntry.GetComponentsInChildren<TextMeshProUGUI>(true);
			string text = string.Empty;
			if (this.m_subSection == FriendListPanel.FriendSubsection.Blocked)
			{
				text = StringUtil.TR("BlockedHeading", "NewFrontEndScene");
			}
			else if (this.m_subSection == FriendListPanel.FriendSubsection.FriendRequests)
			{
				text = StringUtil.TR("FriendRequestHeading", "NewFrontEndScene");
			}
			else if (this.m_subSection == FriendListPanel.FriendSubsection.InvitationsSent)
			{
				text = StringUtil.TR("InvitationsSentHeading", "NewFrontEndScene");
			}
			else if (this.m_subSection == FriendListPanel.FriendSubsection.Offline)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.FriendInfoSubsectionTitleData.Setup(int, _LargeScrollListItemEntry)).MethodHandle;
				}
				text = StringUtil.TR("OfflineHeading", "NewFrontEndScene");
			}
			else if (this.m_subSection == FriendListPanel.FriendSubsection.Online)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				text = StringUtil.TR("OnlineHeading", "NewFrontEndScene");
			}
			if (this.m_isExpanded)
			{
				text = text.Replace("+", "-");
			}
			else
			{
				text = text.Replace("-", "+");
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].text = text;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			_SelectableBtn component = UIEntry.GetComponent<_SelectableBtn>();
			if (component != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				component.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnTitleClicked);
			}
		}

		public void OnTitleClicked(BaseEventData data)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuOpen);
			FriendListPanel.Get().ToggleSubSection(this.m_subSection);
			FriendListPanel.Get().UpdateFriendListSize();
		}
	}

	public class FriendInfoData : IDataEntry
	{
		public FriendInfo m_friendInfo;

		public FriendListPanel.FriendSubsection m_subSection;

		public FriendInfoData(FriendInfo info, FriendListPanel.FriendSubsection subSection)
		{
			this.m_friendInfo = info;
			this.m_subSection = subSection;
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListPanel.FriendInfoData.Setup(int, _LargeScrollListItemEntry)).MethodHandle;
				}
				SocialComponent.FriendData orCreateFriendInfo = ClientGameManager.Get().GetPlayerAccountData().SocialComponent.GetOrCreateFriendInfo(this.m_friendInfo.FriendAccountId);
				if (!this.m_friendInfo.IsOnline)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_friendInfo.BannerID = orCreateFriendInfo.LastSeenBackbroundID;
					this.m_friendInfo.EmblemID = orCreateFriendInfo.LastSeenForegroundID;
					this.m_friendInfo.TitleID = orCreateFriendInfo.LastSeenTitleID;
					this.m_friendInfo.TitleLevel = orCreateFriendInfo.LastSeenTitleLevel;
					this.m_friendInfo.RibbonID = orCreateFriendInfo.LastSeenRibbonID;
					this.m_friendInfo.FriendNote = orCreateFriendInfo.LastSeenNote;
				}
			}
			component.Setup(this.m_friendInfo, this.m_subSection);
		}
	}
}
