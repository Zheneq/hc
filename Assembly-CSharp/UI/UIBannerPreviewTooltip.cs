using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBannerPreviewTooltip : UITooltipBase
{
	public TextMeshProUGUI m_previewTypeLabel;

	public Image m_bannerImage;

	public Image m_emblemImage;

	public Image m_ribbonImage;

	public TextMeshProUGUI m_titleText;

	public void Setup(string previewTitle, GameBalanceVars.PlayerBanner banner, GameBalanceVars.PlayerBanner emblem, GameBalanceVars.PlayerRibbon ribbon, string titleString)
	{
		m_previewTypeLabel.text = previewTitle;
		string path = "Banners/Background/02_blue";
		string path2 = "Banners/Emblems/Chest01";
		if (banner != null)
		{
			path = banner.m_resourceString;
		}
		if (emblem != null)
		{
			path2 = emblem.m_resourceString;
		}
		if (titleString.IsNullOrEmpty())
		{
			titleString = string.Empty;
		}
		m_bannerImage.sprite = Resources.Load<Sprite>(path);
		m_emblemImage.sprite = Resources.Load<Sprite>(path2);
		m_titleText.text = titleString;
		UIManager.SetGameObjectActive(m_ribbonImage, ribbon != null);
		if (ribbon == null)
		{
			return;
		}
		while (true)
		{
			m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			return;
		}
	}
}
