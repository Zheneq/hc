using System;
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
		this.m_previewTypeLabel.text = previewTitle;
		string path = "Banners/Background/02_blue";
		string path2 = "Banners/Emblems/Chest01";
		if (banner != null)
		{
			path = banner.m_resourceString;
		}
		if (emblem != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIBannerPreviewTooltip.Setup(string, GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerRibbon, string)).MethodHandle;
			}
			path2 = emblem.m_resourceString;
		}
		if (titleString.IsNullOrEmpty())
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
			titleString = string.Empty;
		}
		this.m_bannerImage.sprite = Resources.Load<Sprite>(path);
		this.m_emblemImage.sprite = Resources.Load<Sprite>(path2);
		this.m_titleText.text = titleString;
		UIManager.SetGameObjectActive(this.m_ribbonImage, ribbon != null, null);
		if (ribbon != null)
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
			this.m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
		}
	}
}
