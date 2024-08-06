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
		m_InviteButton.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, OpenInviteTooltip);
		m_InviteButton.SetSelected(false, true, string.Empty, string.Empty);
		_MouseEventPasser mouseEventPasser = m_AcceptButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser.AddNewHandler(FriendListPanel.Get().m_scrollView);
		_MouseEventPasser mouseEventPasser2 = m_DeclineButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser2.AddNewHandler(FriendListPanel.Get().m_scrollView);
		_MouseEventPasser mouseEventPasser3 = m_InviteButton.spriteController.gameObject.AddComponent<_MouseEventPasser>();
		mouseEventPasser3.AddNewHandler(FriendListPanel.Get().m_scrollView);
		UIManager.SetGameObjectActive(m_selectedImage, false);
	}

	private bool OpenInviteTooltip(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("InviteToGroup", "FriendList"), string.Format(StringUtil.TR("InviteFriendName", "FriendList"), m_friendInfo.FriendHandle), string.Empty);
		return true;
	}

	public void CancelRequest(BaseEventData data)
	{
		if (m_subSection == FriendListPanel.FriendSubsection.FriendRequests)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					FriendListPanel.Get().RequestToCancelRequest(m_friendInfo);
					return;
				}
			}
		}
		if (m_subSection != FriendListPanel.FriendSubsection.Blocked)
		{
			if (m_subSection != FriendListPanel.FriendSubsection.InvitationsSent)
			{
				return;
			}
		}
		FriendListPanel.Get().RequestToRemoveFriend(m_friendInfo);
	}

	public void AcceptRequest(BaseEventData data)
	{
		if (m_subSection != 0)
		{
			return;
		}
		while (true)
		{
			FriendListPanel.Get().RequestToAcceptRequest(m_friendInfo);
			return;
		}
	}

	public void SendInvite(BaseEventData data)
	{
		if (m_subSection == FriendListPanel.FriendSubsection.Offline || m_subSection == FriendListPanel.FriendSubsection.Online)
		{
			FriendListPanel.Get().RequestToInviteToParty(m_friendInfo);
		}
	}

	private void SetupButtons(FriendListPanel.FriendSubsection subSection)
	{
		m_subSection = subSection;
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		if (subSection == FriendListPanel.FriendSubsection.Blocked)
		{
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.FriendRequests)
		{
			doActive = true;
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.InvitationsSent)
		{
			doActive2 = true;
		}
		else if (subSection == FriendListPanel.FriendSubsection.Offline)
		{
		}
		else if (subSection == FriendListPanel.FriendSubsection.Online)
		{
			doActive3 = true;
		}
		UIManager.SetGameObjectActive(m_AcceptButton, doActive);
		UIManager.SetGameObjectActive(m_DeclineButton, doActive2);
		UIManager.SetGameObjectActive(m_InviteButton, doActive3);
	}

	public void Setup(FriendInfo friendInfo, FriendListPanel.FriendSubsection subSection)
	{
		m_friendInfo = friendInfo;
		SetupButtons(subSection);
		string text = friendInfo.FriendHandle;
		if (!friendInfo.FriendNote.IsNullOrEmpty())
		{
			text = $"{friendInfo.FriendHandle}({friendInfo.FriendNote})";
		}
		m_playerName.text = text;
		if (m_hitbox.gameObject.GetComponent<_MouseEventPasser>() == null)
		{
			_MouseEventPasser mouseEventPasser = m_hitbox.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(FriendListPanel.Get().m_scrollView);
		}
		if (friendInfo.FriendStatus == FriendStatus.Friend)
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
					m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[1];
				}
				else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Busy.ToString())
				{
					m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[2];
				}
				else if (friendInfo.StatusString == FriendListHeader.PlayerOnlineStatus.Online.ToString())
				{
					m_playerStatusImage.color = FriendListPanel.Get().m_panelHeader.m_friendListColors[0];
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
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestSent)
		{
			m_playerStatusLabel.text = StringUtil.TR("AwaitingReply", "FriendList");
		}
		else if (friendInfo.FriendStatus == FriendStatus.RequestReceived)
		{
			m_playerStatusLabel.text = StringUtil.TR("AwaitingApproval", "FriendList");
		}
		else if (friendInfo.FriendStatus == FriendStatus.Blocked)
		{
			m_playerStatusLabel.text = StringUtil.TR("Blocked", "FriendList");
		}
		else
		{
			m_playerStatusLabel.text = string.Empty;
		}
		UpdateVisualInfo(friendInfo.TitleID, friendInfo.TitleLevel, friendInfo.BannerID, friendInfo.EmblemID, friendInfo.RibbonID, friendInfo.FriendNote);
	}

	public void UpdateVisualInfo(int titleId, int titleLevel, int bannerId, int emblemId, int ribbonId, string friendNote)
	{
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerId);
		Sprite sprite = null;
		if (banner != null)
		{
			sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
		}
		m_bannerBG.sprite = sprite;
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(emblemId);
		sprite = null;
		if (banner2 != null)
		{
			sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
		}
		m_bannerFG.sprite = sprite;
		m_friendInfo.TitleID = titleId;
		m_friendInfo.TitleLevel = titleLevel;
	}

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
		if (FriendListPanel.Get().IsVisible())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
#if !VANILLA && !SERVER
                            // Custom titles
                            string title = GameWideData.Get().m_gameBalanceVars.GetTitle(m_friendInfo.TitleID, m_friendInfo.FriendHandle, string.Empty, m_friendInfo.TitleLevel);
#else
					string title = GameWideData.Get().m_gameBalanceVars.GetTitle(m_friendInfo.TitleID, string.Empty, m_friendInfo.TitleLevel);
#endif
					string str = m_friendInfo.FriendHandle;
					if (!title.IsNullOrEmpty())
					{
						str = string.Format(StringUtil.TR("BannerDescription", "FriendList"), m_playerName.text, title);
					}
					str = str + Environment.NewLine + m_friendInfo.FriendNote;
					uITitledTooltip.Setup(m_playerStatusLabel.text, str, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
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
