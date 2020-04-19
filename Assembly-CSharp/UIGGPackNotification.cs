using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGGPackNotification : MonoBehaviour
{
	public Image m_backgroundImage;

	public Image m_foregroundImage;

	public Image m_ribbonImage;

	public TextMeshProUGUI m_UserNameLabel;

	public TextMeshProUGUI m_TitleLabel;

	public RectTransform[] m_ggButtonLevelImages;

	public bool DoneAnimating { get; private set; }

	public void AnimationDone()
	{
		this.DoneAnimating = true;
	}

	public void Setup(UISideNotifications.UIGGPackNotificationInfo info)
	{
		this.m_UserNameLabel.text = info.GGPackUserName;
		this.m_TitleLabel.text = GameBalanceVars.Get().GetTitle(info.GGPackUserTitle, string.Empty, info.GGPackUserTitleLevel);
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(info.GGPackUserBannerForeground);
		GameBalanceVars.PlayerBanner banner2 = GameBalanceVars.Get().GetBanner(info.GGPackUserBannerBackground);
		GameBalanceVars.PlayerRibbon ribbon = GameBalanceVars.Get().GetRibbon(info.GGPackUserRibbon);
		for (int i = 0; i < this.m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_ggButtonLevelImages[i], i == info.NumGGPacksUsed - 1, null);
		}
		if (banner != null)
		{
			Sprite sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
			this.m_foregroundImage.sprite = sprite;
			UIManager.SetGameObjectActive(this.m_foregroundImage, sprite != null, null);
		}
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGGPackNotification.Setup(UISideNotifications.UIGGPackNotificationInfo)).MethodHandle;
			}
			Sprite sprite2 = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
			this.m_backgroundImage.sprite = sprite2;
			UIManager.SetGameObjectActive(this.m_backgroundImage, sprite2 != null, null);
		}
		if (ribbon != null)
		{
			Sprite sprite3 = (Sprite)Resources.Load(ribbon.m_resourceString, typeof(Sprite));
			this.m_ribbonImage.sprite = sprite3;
			UIManager.SetGameObjectActive(this.m_ribbonImage, sprite3 != null, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
	}
}
