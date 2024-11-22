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
        DoneAnimating = true;
    }

    public void Setup(UISideNotifications.UIGGPackNotificationInfo info)
    {
        m_UserNameLabel.text = info.GGPackUserName;
        m_TitleLabel.text = GameBalanceVars.Get().GetTitle(
            info.GGPackUserTitle,
#if EVOS
			info.GGPackUserName, // Custom titles
#endif
            string.Empty,
            info.GGPackUserTitleLevel);
        GameBalanceVars.PlayerBanner bannerForeground =
            GameBalanceVars.Get().GetBanner(info.GGPackUserBannerForeground);
        GameBalanceVars.PlayerBanner bannerBackground =
            GameBalanceVars.Get().GetBanner(info.GGPackUserBannerBackground);
        GameBalanceVars.PlayerRibbon ribbon =
            GameBalanceVars.Get().GetRibbon(info.GGPackUserRibbon);
        for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
        {
            UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == info.NumGGPacksUsed - 1);
        }

        if (bannerForeground != null)
        {
            Sprite sprite = (Sprite)Resources.Load(bannerForeground.m_resourceString, typeof(Sprite));
            m_foregroundImage.sprite = sprite;
            UIManager.SetGameObjectActive(m_foregroundImage, sprite != null);
        }

        if (bannerBackground != null)
        {
            Sprite sprite = (Sprite)Resources.Load(bannerBackground.m_resourceString, typeof(Sprite));
            m_backgroundImage.sprite = sprite;
            UIManager.SetGameObjectActive(m_backgroundImage, sprite != null);
        }

        if (ribbon != null)
        {
            Sprite sprite = (Sprite)Resources.Load(ribbon.m_resourceString, typeof(Sprite));
            m_ribbonImage.sprite = sprite;
            UIManager.SetGameObjectActive(m_ribbonImage, sprite != null);
        }
        else
        {
            UIManager.SetGameObjectActive(m_ribbonImage, false);
        }
    }
}