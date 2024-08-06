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

	public bool DoneAnimating
	{
		get;
		private set;
	}

	public void AnimationDone()
	{
		DoneAnimating = true;
	}

	public void Setup(UISideNotifications.UIGGPackNotificationInfo info)
	{
		m_UserNameLabel.text = info.GGPackUserName;
#if !VANILLA && !SERVER
        // Custom titles
        m_TitleLabel.text = GameBalanceVars.Get().GetTitle(info.GGPackUserTitle, info.GGPackUserName, string.Empty, info.GGPackUserTitleLevel);
#else
		m_TitleLabel.text = GameBalanceVars.Get().GetTitle(info.GGPackUserTitle, string.Empty, info.GGPackUserTitleLevel);
#endif
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(info.GGPackUserBannerForeground);
		GameBalanceVars.PlayerBanner banner2 = GameBalanceVars.Get().GetBanner(info.GGPackUserBannerBackground);
		GameBalanceVars.PlayerRibbon ribbon = GameBalanceVars.Get().GetRibbon(info.GGPackUserRibbon);
		for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
		{
			UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == info.NumGGPacksUsed - 1);
		}
		if (banner != null)
		{
			Sprite sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
			m_foregroundImage.sprite = sprite;
			UIManager.SetGameObjectActive(m_foregroundImage, sprite != null);
		}
		if (banner2 != null)
		{
			Sprite sprite2 = (Sprite)Resources.Load(banner2.m_resourceString, typeof(Sprite));
			m_backgroundImage.sprite = sprite2;
			UIManager.SetGameObjectActive(m_backgroundImage, sprite2 != null);
		}
		if (ribbon != null)
		{
			Sprite sprite3 = (Sprite)Resources.Load(ribbon.m_resourceString, typeof(Sprite));
			m_ribbonImage.sprite = sprite3;
			UIManager.SetGameObjectActive(m_ribbonImage, sprite3 != null);
		}
		else
		{
			UIManager.SetGameObjectActive(m_ribbonImage, false);
		}
	}
}
