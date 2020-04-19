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
		this.m_tooltipClickObject.Setup(TooltipType.FriendBannerMenu, new TooltipPopulateCall(this.OpenMenu), null);
		this.m_tooltipHoverObject.Setup(TooltipType.Titled, new TooltipPopulateCall(this.OpenHoverTooltip), null);
		if (this.m_hitbox.m_hoverImage != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.Start()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_hitbox.m_hoverImage.gameObject, false, null);
		}
		this.m_DeclineButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelRequest);
		this.m_AcceptButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AcceptRequest);
		this.m_InviteButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SendInvite);
		this.m_InviteButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.OpenInviteTooltip), null);
		this.m_InviteButton.SetSelected(false, true, string.Empty, string.Empty);
		_MouseEventPasser mouseEventPasser = this.m_AcceptButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(FriendListPanel.Get().m_scrollView);
		_MouseEventPasser mouseEventPasser2 = this.m_DeclineButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser2.AddNewHandler(FriendListPanel.Get().m_scrollView);
		_MouseEventPasser mouseEventPasser3 = this.m_InviteButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser3.AddNewHandler(FriendListPanel.Get().m_scrollView);
		UIManager.SetGameObjectActive(this.m_selectedImage, false, null);
	}

	private bool OpenInviteTooltip(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("InviteToGroup", "FriendList"), string.Format(StringUtil.TR("InviteFriendName", "FriendList"), this.m_friendInfo.FriendHandle), string.Empty);
		return true;
	}

	public void CancelRequest(BaseEventData data)
	{
		if (this.m_subSection == FriendListPanel.FriendSubsection.FriendRequests)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.CancelRequest(BaseEventData)).MethodHandle;
			}
			FriendListPanel.Get().RequestToCancelRequest(this.m_friendInfo);
		}
		else
		{
			if (this.m_subSection != FriendListPanel.FriendSubsection.Blocked)
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
				if (this.m_subSection != FriendListPanel.FriendSubsection.InvitationsSent)
				{
					return;
				}
			}
			FriendListPanel.Get().RequestToRemoveFriend(this.m_friendInfo);
		}
	}

	public void AcceptRequest(BaseEventData data)
	{
		if (this.m_subSection == FriendListPanel.FriendSubsection.FriendRequests)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.AcceptRequest(BaseEventData)).MethodHandle;
			}
			FriendListPanel.Get().RequestToAcceptRequest(this.m_friendInfo);
		}
	}

	public void SendInvite(BaseEventData data)
	{
		if (this.m_subSection == FriendListPanel.FriendSubsection.Offline || this.m_subSection == FriendListPanel.FriendSubsection.Online)
		{
			FriendListPanel.Get().RequestToInviteToParty(this.m_friendInfo);
		}
	}

	private void SetupButtons(FriendListPanel.FriendSubsection subSection)
	{
		this.m_subSection = subSection;
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		if (subSection == FriendListPanel.FriendSubsection.Blocked)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.SetupButtons(FriendListPanel.FriendSubsection)).MethodHandle;
			}
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.FriendRequests)
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
			doActive = true;
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.InvitationsSent)
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
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.Offline)
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
		}
		else if (subSection == FriendListPanel.FriendSubsection.Online)
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
			doActive3 = true;
		}
		UIManager.SetGameObjectActive(this.m_AcceptButton, doActive, null);
		UIManager.SetGameObjectActive(this.m_DeclineButton, doActive2, null);
		UIManager.SetGameObjectActive(this.m_InviteButton, doActive3, null);
	}

	public void Setup(FriendInfo friendInfo, FriendListPanel.FriendSubsection subSection)
	{
		this.m_friendInfo = friendInfo;
		this.SetupButtons(subSection);
		string text = friendInfo.FriendHandle;
		if (!friendInfo.FriendNote.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.Setup(FriendInfo, FriendListPanel.FriendSubsection)).MethodHandle;
			}
			text = string.Format("{0}({1})", friendInfo.FriendHandle, friendInfo.FriendNote);
		}
		this.m_playerName.text = text;
		if (this.m_hitbox.gameObject.GetComponent<_MouseEventPasser>() == null)
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
			_MouseEventPasser mouseEventPasser = this.m_hitbox.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(FriendListPanel.Get().m_scrollView);
		}
		if (friendInfo.FriendStatus == FriendStatus.Friend)
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
			if (friendInfo.StatusString.IsNullOrEmpty())
			{
				if (friendInfo.IsOnline)
				{
					this.m_playerStatusLabel.text = StringUtil.TR("Online", "FriendList");
					this.m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[0];
				}
				else
				{
					this.m_playerStatusLabel.text = StringUtil.TR("Offline", "FriendList");
					this.m_playerStatusImage.color = Color.gray;
				}
			}
			else
			{
				if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Away.ToString())
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
					this.m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[1];
				}
				else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Busy.ToString())
				{
					this.m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[2];
				}
				else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Online.ToString())
				{
					this.m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[0];
				}
				this.m_playerStatusLabel.text = StringUtil.TR(friendInfo.StatusString, "FriendList");
			}
			if (friendInfo.IsOnline)
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
				this.SetTextOnlineColor();
			}
			else
			{
				this.SetTextOfflineColor();
			}
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestSent)
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
			this.m_playerStatusLabel.text = StringUtil.TR("AwaitingReply", "FriendList");
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestReceived)
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
			this.m_playerStatusLabel.text = StringUtil.TR("AwaitingApproval", "FriendList");
		}
		else if (friendInfo.FriendStatus == FriendStatus.Blocked)
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
			this.m_playerStatusLabel.text = StringUtil.TR("Blocked", "FriendList");
		}
		else
		{
			this.m_playerStatusLabel.text = string.Empty;
		}
		this.UpdateVisualInfo(friendInfo.TitleID, friendInfo.TitleLevel, friendInfo.BannerID, friendInfo.EmblemID, friendInfo.RibbonID, friendInfo.FriendNote);
	}

	public void UpdateVisualInfo(int titleId, int titleLevel, int bannerId, int emblemId, int ribbonId, string friendNote)
	{
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerId);
		Sprite sprite;
		if (banner != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.UpdateVisualInfo(int, int, int, int, int, string)).MethodHandle;
			}
			sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
		}
		this.m_bannerBG.sprite = sprite;
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(emblemId);
		if (banner2 != null)
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
			sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
		}
		this.m_bannerFG.sprite = sprite;
		this.m_friendInfo.TitleID = titleId;
		this.m_friendInfo.TitleLevel = titleLevel;
	}

	public void SetTextOnlineColor()
	{
		this.m_playerName.color = Color.white;
		this.m_playerStatusLabel.color = Color.white;
	}

	public void SetTextOfflineColor()
	{
		this.m_playerName.color = this.m_offlineTextColor;
		this.m_playerStatusLabel.color = this.m_offlineTextColor;
	}

	private bool OpenMenu(UITooltipBase tooltip)
	{
		FriendListBannerMenu friendListBannerMenu = tooltip as FriendListBannerMenu;
		if (this.m_friendInfo.FriendStatus != FriendStatus.Blocked)
		{
			friendListBannerMenu.Setup(this.m_friendInfo);
			return true;
		}
		return false;
	}

	private bool OpenHoverTooltip(UITooltipBase tooltip)
	{
		if (FriendListPanel.Get().IsVisible())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListBannerEntry.OpenHoverTooltip(UITooltipBase)).MethodHandle;
			}
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			string title = GameWideData.Get().m_gameBalanceVars.GetTitle(this.m_friendInfo.TitleID, string.Empty, this.m_friendInfo.TitleLevel);
			string text = this.m_friendInfo.FriendHandle;
			if (!title.IsNullOrEmpty())
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
				text = string.Format(StringUtil.TR("BannerDescription", "FriendList"), this.m_playerName.text, title);
			}
			text = text + Environment.NewLine + this.m_friendInfo.FriendNote;
			uititledTooltip.Setup(this.m_playerStatusLabel.text, text, string.Empty);
			return true;
		}
		return false;
	}

	public bool IsSelected()
	{
		return this.m_selected;
	}

	public void SetSelected(bool selected)
	{
		this.m_selected = selected;
		UIManager.SetGameObjectActive(this.m_selectedImage, selected, null);
	}
}
