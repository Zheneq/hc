using System;
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
            SetBanner(
                ClientGameManager.Get().GetCurrentBackgroundBanner(),
                GameBalanceVars.PlayerBanner.BannerType.Background);
            SetBanner(
                ClientGameManager.Get().GetCurrentForegroundBanner(),
                GameBalanceVars.PlayerBanner.BannerType.Foreground);
            SetRibbon(ClientGameManager.Get().GetCurrentRibbon());
            if (m_playerName != null)
            {
                m_playerName.text = accountData.UserName;
            }

            if (m_playerTitle != null)
            {
#if EVOS
                m_playerTitle.text = GameBalanceVars.Get().GetTitle(accountData.AccountComponent.SelectedTitleID, accountData.Handle);
#else
                m_playerTitle.text = GameBalanceVars.Get().GetTitle(accountData.AccountComponent.SelectedTitleID);
#endif
            }

            if (m_playerLevel != null)
            {
                m_playerLevel.text = ClientGameManager.Get().GetDisplayedStatString(accountData);
            }
        };
        ClientGameManager.Get().OnAccountDataUpdated += m_onAccountDataUpdated;
        if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
        {
            m_onAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
        }

        SetBanner(
            ClientGameManager.Get().GetCurrentBackgroundBanner(),
            GameBalanceVars.PlayerBanner.BannerType.Background);
        SetBanner(
            ClientGameManager.Get().GetCurrentForegroundBanner(),
            GameBalanceVars.PlayerBanner.BannerType.Foreground);
        SetRibbon(ClientGameManager.Get().GetCurrentRibbon());
        m_onPlayerBannerChange = delegate(
            GameBalanceVars.PlayerBanner foregroundBanner,
            GameBalanceVars.PlayerBanner backgroundBanner)
        {
            SetBanner(backgroundBanner, GameBalanceVars.PlayerBanner.BannerType.Background);
            SetBanner(foregroundBanner, GameBalanceVars.PlayerBanner.BannerType.Foreground);
        };
        ClientGameManager.Get().OnPlayerBannerChange += m_onPlayerBannerChange;
        m_onPlayerRibbonChange = delegate { SetRibbon(ClientGameManager.Get().GetCurrentRibbon()); };
        ClientGameManager.Get().OnPlayerRibbonChange += m_onPlayerRibbonChange;
    }

    private void OnDestroy()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (clientGameManager == null)
        {
            return;
        }

        clientGameManager.OnAccountDataUpdated -= m_onAccountDataUpdated;
        clientGameManager.OnPlayerBannerChange -= m_onPlayerBannerChange;
        clientGameManager.OnPlayerRibbonChange -= m_onPlayerRibbonChange;
    }

    private void SetBanner(GameBalanceVars.PlayerBanner banner, GameBalanceVars.PlayerBanner.BannerType bannerType)
    {
        Sprite sprite;
        if (banner != null)
        {
            sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
            if (sprite == null)
            {
                Log.Warning(
                    Log.Category.UI,
                    $"Could not load banner resource from [{banner.m_resourceString}] as sprite.");
            }
        }
        else
        {
            sprite = (Sprite)Resources.Load(standardResourceString, typeof(Sprite));
            if (sprite == null)
            {
                Log.Warning(
                    Log.Category.UI,
                    $"Could not load banner resource from [{standardResourceString}] as sprite.");
            }
        }

        if (sprite != null)
        {
            if (bannerType == GameBalanceVars.PlayerBanner.BannerType.Background)
            {
                m_bannerImage.sprite = sprite;
            }
            else
            {
                m_profileImage.sprite = sprite;
            }
        }
    }

    private void SetRibbon(GameBalanceVars.PlayerRibbon ribbon)
    {
        if (m_ribbonImage == null)
        {
            return;
        }

        Sprite sprite = null;
        if (ribbon != null)
        {
            sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
            if (sprite == null)
            {
                Log.Warning(
                    Log.Category.UI,
                    $"Could not load ribbon resource from [{ribbon.m_resourceString}] as sprite.");
            }
        }

        UIManager.SetGameObjectActive(m_ribbonImage, sprite != null);
        m_ribbonImage.sprite = sprite;
    }
}