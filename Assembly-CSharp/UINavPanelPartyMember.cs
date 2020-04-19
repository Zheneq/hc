using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINavPanelPartyMember : MonoBehaviour
{
	public UITooltipClickObject m_groupMenuClickListener;

	public _ButtonSwapSprite m_hitbox;

	public Animator m_animController;

	public Animator m_PartyLeaderIconAnimator;

	public Animator m_ReadyIconAnimator;

	public Animator m_IsInGameAnimator;

	public Image m_bannerImage;

	public Image m_emblemImage;

	public Image m_ribbonImage;

	public TextMeshProUGUI m_playerLevel;

	public RectTransform m_playerContainer;

	public RectTransform m_invitationSentContainer;

	private UpdateGroupMemberData m_memberInfo;

	private bool m_isHidden;

	public UpdateGroupMemberData GetMemberInfo()
	{
		return this.m_memberInfo;
	}

	private void Start()
	{
		if (this.m_hitbox != null)
		{
			this.m_groupMenuClickListener.Setup(TooltipType.PlayerGroupMenu, new TooltipPopulateCall(this.OpenGroupMenu), null);
			this.m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupTooltip), null);
		}
		if (this.m_memberInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.Start()).MethodHandle;
			}
			if (this.m_memberInfo.IsLeader)
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
				UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, true, null);
				this.m_PartyLeaderIconAnimator.Play("leaderIconIN");
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, false, null);
			}
			if (this.m_memberInfo.IsReady)
			{
				UIManager.SetGameObjectActive(this.m_ReadyIconAnimator, true, null);
				this.m_ReadyIconAnimator.Play("readyIconIN");
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_ReadyIconAnimator, false, null);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, false, null);
			UIManager.SetGameObjectActive(this.m_ReadyIconAnimator, false, null);
		}
		if (this.m_invitationSentContainer != null)
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
			UIManager.SetGameObjectActive(this.m_invitationSentContainer, false, null);
		}
		if (this.m_playerLevel != null)
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
			this.m_playerLevel.text = string.Empty;
		}
		if (this.m_ribbonImage != null)
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (this.m_memberInfo != null && !UITooltipManager.Get().IsVisible(TooltipType.PlayerGroupMenu))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.SetupTooltip(UITooltipBase)).MethodHandle;
			}
			string tooltipText = StringUtil.TR("UnknownCharacter", "Global");
			if (this.m_memberInfo.MemberDisplayCharacter != CharacterType.None)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_memberInfo.MemberDisplayCharacter);
				if (characterResourceLink != null)
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
					tooltipText = characterResourceLink.GetDisplayName();
				}
			}
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(this.m_memberInfo.MemberDisplayName, tooltipText, string.Empty);
			return true;
		}
		return false;
	}

	private bool OpenGroupMenu(UITooltipBase tooltip)
	{
		if (this.m_memberInfo == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.OpenGroupMenu(UITooltipBase)).MethodHandle;
			}
			FriendListPanel.Get().SetVisible(true, true, false);
			return false;
		}
		UIPlayerPanelGroupMenu uiplayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
		uiplayerPanelGroupMenu.Setup(this.m_memberInfo);
		UITooltipManager.Get().HideDisplayTooltip();
		return true;
	}

	public void SetAsLeader(bool isLeader)
	{
		if (isLeader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.SetAsLeader(bool)).MethodHandle;
			}
			if (!this.m_PartyLeaderIconAnimator.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, true, null);
				this.m_PartyLeaderIconAnimator.Play("leaderIconIN");
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, false, null);
		}
	}

	public void SetIsInGame(bool isInGame)
	{
		UIManager.SetGameObjectActive(this.m_IsInGameAnimator, true, null);
		if (isInGame)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.SetIsInGame(bool)).MethodHandle;
			}
			this.m_IsInGameAnimator.Play("readyIconIN");
		}
		else
		{
			bool flag = false;
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_IsInGameAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo != null)
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
				if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip.name != "readyIconOUT")
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
					if (currentAnimatorClipInfo[0].clip.name != "readyIconOFF")
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
						flag = true;
					}
				}
			}
			if (flag)
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
				this.m_IsInGameAnimator.Play("readyIconOUT");
			}
		}
	}

	public void UpdateReadyState(bool IsReady)
	{
		UIManager.SetGameObjectActive(this.m_ReadyIconAnimator, true, null);
		if (IsReady)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.UpdateReadyState(bool)).MethodHandle;
			}
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo != null && currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip.name != "readyIconIN")
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
				if (currentAnimatorClipInfo[0].clip.name != "readyIconON")
				{
					this.m_ReadyIconAnimator.Play("readyIconIN");
				}
			}
		}
		else
		{
			bool flag = false;
			AnimatorClipInfo[] currentAnimatorClipInfo2 = this.m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo2 != null)
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
				if (currentAnimatorClipInfo2.Length > 0 && currentAnimatorClipInfo2[0].clip.name != "readyIconOUT")
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
					if (currentAnimatorClipInfo2[0].clip.name != "readyIconOFF")
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
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.m_ReadyIconAnimator.Play("readyIconOUT");
			}
		}
	}

	public void Setup(UpdateGroupMemberData info)
	{
		if (info == null)
		{
			return;
		}
		if (this.m_bannerImage != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.Setup(UpdateGroupMemberData)).MethodHandle;
			}
			GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(info.BackgroundBannerID);
			Sprite sprite;
			if (banner == null)
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
				sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
			}
			else
			{
				sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
			}
			this.m_bannerImage.sprite = sprite;
		}
		if (this.m_emblemImage != null)
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
			GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(info.ForegroundBannerID);
			Sprite sprite;
			if (banner2 == null)
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
				sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
			}
			else
			{
				sprite = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
			}
			this.m_emblemImage.sprite = sprite;
		}
		if (this.m_ribbonImage != null)
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
			GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(info.RibbonID);
			if (ribbon == null)
			{
				UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
			}
			else
			{
				this.m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
				UIManager.SetGameObjectActive(this.m_ribbonImage, this.m_ribbonImage.sprite != null, null);
			}
		}
		if (this.m_memberInfo != null)
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
			if (this.m_memberInfo.AccountID == info.AccountID)
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
				return;
			}
		}
		this.m_isHidden = false;
		if (this.m_playerContainer != null)
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
			UIManager.SetGameObjectActive(this.m_playerContainer, true, null);
		}
		UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, info.IsLeader, null);
		if (info.IsLeader)
		{
			this.m_PartyLeaderIconAnimator.Play("leaderIconIN");
		}
		this.UpdateReadyState(info.IsReady);
		this.m_memberInfo = info;
		if (this.m_animController != null)
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
			this.m_animController.Play("UIBannerInviteIN", 1, 0f);
		}
		if (this.m_groupMenuClickListener != null)
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
			this.m_groupMenuClickListener.Refresh();
		}
	}

	private void DoHidden()
	{
		this.m_isHidden = true;
		if (this.m_playerContainer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.DoHidden()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_playerContainer, false, null);
		}
		if (this.m_ribbonImage != null)
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
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
		if (this.m_animController != null)
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
			this.m_animController.Play("UIBannerDisabledIN", 1, 0f);
		}
		UIManager.SetGameObjectActive(this.m_PartyLeaderIconAnimator, false, null);
		UIManager.SetGameObjectActive(this.m_ReadyIconAnimator, false, null);
		UIManager.SetGameObjectActive(this.m_IsInGameAnimator, false, null);
		this.m_memberInfo = null;
	}

	public void SetToHidden()
	{
		if (!this.m_isHidden)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINavPanelPartyMember.SetToHidden()).MethodHandle;
			}
			this.DoHidden();
		}
	}
}
