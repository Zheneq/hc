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
		return m_memberInfo;
	}

	private void Start()
	{
		if (m_hitbox != null)
		{
			m_groupMenuClickListener.Setup(TooltipType.PlayerGroupMenu, OpenGroupMenu);
			m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupTooltip);
		}
		if (m_memberInfo != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_memberInfo.IsLeader)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, true);
				m_PartyLeaderIconAnimator.Play("leaderIconIN");
			}
			else
			{
				UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
			}
			if (m_memberInfo.IsReady)
			{
				UIManager.SetGameObjectActive(m_ReadyIconAnimator, true);
				m_ReadyIconAnimator.Play("readyIconIN");
			}
			else
			{
				UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
			UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
		}
		if (m_invitationSentContainer != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_invitationSentContainer, false);
		}
		if (m_playerLevel != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			m_playerLevel.text = string.Empty;
		}
		if (m_ribbonImage != null)
		{
			UIManager.SetGameObjectActive(m_ribbonImage, false);
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (m_memberInfo != null && !UITooltipManager.Get().IsVisible(TooltipType.PlayerGroupMenu))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					string tooltipText = StringUtil.TR("UnknownCharacter", "Global");
					if (m_memberInfo.MemberDisplayCharacter != 0)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_memberInfo.MemberDisplayCharacter);
						if (characterResourceLink != null)
						{
							while (true)
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
					UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
					uITitledTooltip.Setup(m_memberInfo.MemberDisplayName, tooltipText, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	private bool OpenGroupMenu(UITooltipBase tooltip)
	{
		if (m_memberInfo == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					FriendListPanel.Get().SetVisible(true, true);
					return false;
				}
			}
		}
		UIPlayerPanelGroupMenu uIPlayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
		uIPlayerPanelGroupMenu.Setup(m_memberInfo);
		UITooltipManager.Get().HideDisplayTooltip();
		return true;
	}

	public void SetAsLeader(bool isLeader)
	{
		if (isLeader)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!m_PartyLeaderIconAnimator.gameObject.activeSelf)
					{
						UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, true);
						m_PartyLeaderIconAnimator.Play("leaderIconIN");
					}
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
	}

	public void SetIsInGame(bool isInGame)
	{
		UIManager.SetGameObjectActive(m_IsInGameAnimator, true);
		if (isInGame)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_IsInGameAnimator.Play("readyIconIN");
					return;
				}
			}
		}
		bool flag = false;
		AnimatorClipInfo[] currentAnimatorClipInfo = m_IsInGameAnimator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo != null)
		{
			while (true)
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
				while (true)
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
					while (true)
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
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_IsInGameAnimator.Play("readyIconOUT");
			return;
		}
	}

	public void UpdateReadyState(bool IsReady)
	{
		UIManager.SetGameObjectActive(m_ReadyIconAnimator, true);
		if (IsReady)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AnimatorClipInfo[] currentAnimatorClipInfo = m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
					if (currentAnimatorClipInfo != null && currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip.name != "readyIconIN")
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (currentAnimatorClipInfo[0].clip.name != "readyIconON")
								{
									m_ReadyIconAnimator.Play("readyIconIN");
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		bool flag = false;
		AnimatorClipInfo[] currentAnimatorClipInfo2 = m_ReadyIconAnimator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo2 != null)
		{
			while (true)
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
				while (true)
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
					while (true)
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
			m_ReadyIconAnimator.Play("readyIconOUT");
		}
	}

	public void Setup(UpdateGroupMemberData info)
	{
		if (info == null)
		{
			return;
		}
		Sprite sprite = null;
		if (m_bannerImage != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(info.BackgroundBannerID);
			if (banner == null)
			{
				while (true)
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
			m_bannerImage.sprite = sprite;
		}
		if (m_emblemImage != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(info.ForegroundBannerID);
			if (banner2 == null)
			{
				while (true)
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
			m_emblemImage.sprite = sprite;
		}
		if (m_ribbonImage != null)
		{
			while (true)
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
				UIManager.SetGameObjectActive(m_ribbonImage, false);
			}
			else
			{
				m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
				UIManager.SetGameObjectActive(m_ribbonImage, m_ribbonImage.sprite != null);
			}
		}
		if (m_memberInfo != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_memberInfo.AccountID == info.AccountID)
			{
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		m_isHidden = false;
		if (m_playerContainer != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_playerContainer, true);
		}
		UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, info.IsLeader);
		if (info.IsLeader)
		{
			m_PartyLeaderIconAnimator.Play("leaderIconIN");
		}
		UpdateReadyState(info.IsReady);
		m_memberInfo = info;
		if (m_animController != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_animController.Play("UIBannerInviteIN", 1, 0f);
		}
		if (!(m_groupMenuClickListener != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_groupMenuClickListener.Refresh();
			return;
		}
	}

	private void DoHidden()
	{
		m_isHidden = true;
		if (m_playerContainer != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_playerContainer, false);
		}
		if (m_ribbonImage != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_ribbonImage, false);
		}
		if (m_animController != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_animController.Play("UIBannerDisabledIN", 1, 0f);
		}
		UIManager.SetGameObjectActive(m_PartyLeaderIconAnimator, false);
		UIManager.SetGameObjectActive(m_ReadyIconAnimator, false);
		UIManager.SetGameObjectActive(m_IsInGameAnimator, false);
		m_memberInfo = null;
	}

	public void SetToHidden()
	{
		if (m_isHidden)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			DoHidden();
			return;
		}
	}
}
