using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBanner : MonoBehaviour
{
	public Image m_bannerImage;

	public Image m_profileImage;

	public Image m_ribbonImage;

	public TextMeshProUGUI m_playerName;

	public TextMeshProUGUI m_playerTitle;

	public TextMeshProUGUI m_playerLevel;

	public const string standardResourceString = "Banners/Background/02_blue";

	public const string standardEmblemResourceString = "Banners/Emblems/Chest01";

	public const string rankedModeEnemyBannerResourceString = "Banners/Background/rankedRedDefault";

	private Action<PersistedAccountData> m_onAccountDataUpdated;

	private Action<GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner> m_onPlayerBannerChange;

	private Action<GameBalanceVars.PlayerRibbon> m_onPlayerRibbonChange;

	private void Start()
	{
		m_onAccountDataUpdated = delegate(PersistedAccountData accountData)
		{
			SetBanner(ClientGameManager.Get().GetCurrentBackgroundBanner(), GameBalanceVars.PlayerBanner.BannerType.Background);
			SetBanner(ClientGameManager.Get().GetCurrentForegroundBanner(), GameBalanceVars.PlayerBanner.BannerType.Foreground);
			SetRibbon(ClientGameManager.Get().GetCurrentRibbon());
			if (m_playerName != null)
			{
				m_playerName.text = accountData.UserName;
			}
			if (m_playerTitle != null)
			{
				m_playerTitle.text = GameBalanceVars.Get().GetTitle(accountData.AccountComponent.SelectedTitleID, string.Empty);
			}
			if (m_playerLevel != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_playerLevel.text = ClientGameManager.Get().GetDisplayedStatString(accountData);
						return;
					}
				}
			}
		};
		ClientGameManager.Get().OnAccountDataUpdated += m_onAccountDataUpdated;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			m_onAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		SetBanner(ClientGameManager.Get().GetCurrentBackgroundBanner(), GameBalanceVars.PlayerBanner.BannerType.Background);
		SetBanner(ClientGameManager.Get().GetCurrentForegroundBanner(), GameBalanceVars.PlayerBanner.BannerType.Foreground);
		SetRibbon(ClientGameManager.Get().GetCurrentRibbon());
		m_onPlayerBannerChange = delegate(GameBalanceVars.PlayerBanner foregroundBanner, GameBalanceVars.PlayerBanner backgroundBanner)
		{
			SetBanner(backgroundBanner, GameBalanceVars.PlayerBanner.BannerType.Background);
			SetBanner(foregroundBanner, GameBalanceVars.PlayerBanner.BannerType.Foreground);
		};
		ClientGameManager.Get().OnPlayerBannerChange += m_onPlayerBannerChange;
		m_onPlayerRibbonChange = delegate
		{
			SetRibbon(ClientGameManager.Get().GetCurrentRibbon());
		};
		ClientGameManager.Get().OnPlayerRibbonChange += m_onPlayerRibbonChange;
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		clientGameManager.OnAccountDataUpdated -= m_onAccountDataUpdated;
		clientGameManager.OnPlayerBannerChange -= m_onPlayerBannerChange;
		clientGameManager.OnPlayerRibbonChange -= m_onPlayerRibbonChange;
	}

	private void SetBanner(GameBalanceVars.PlayerBanner banner, GameBalanceVars.PlayerBanner.BannerType bannerType)
	{
		Sprite sprite = null;
		if (banner != null)
		{
			sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, new StringBuilder().Append("Could not load banner resource from [").Append(banner.m_resourceString).Append("] as sprite.").ToString());
			}
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/02_blue", typeof(Sprite));
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", "Banners/Background/02_blue"));
			}
		}
		if (!(sprite != null))
		{
			return;
		}
		while (true)
		{
			if (bannerType == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_bannerImage.sprite = sprite;
						return;
					}
				}
			}
			m_profileImage.sprite = sprite;
			return;
		}
	}

	private void SetRibbon(GameBalanceVars.PlayerRibbon ribbon)
	{
		if (m_ribbonImage == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Sprite sprite = null;
		if (ribbon != null)
		{
			sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, new StringBuilder().Append("Could not load ribbon resource from [").Append(ribbon.m_resourceString).Append("] as sprite.").ToString());
			}
		}
		UIManager.SetGameObjectActive(m_ribbonImage, sprite != null);
		m_ribbonImage.sprite = sprite;
	}
}
