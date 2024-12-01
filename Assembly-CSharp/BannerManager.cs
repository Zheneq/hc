using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if EVOS
// Custom Banners
public class BannerManager
{
    private BannerFetcher bannerFetcher;
    private Dictionary<string, string> cachedBannerByHandle;
    private static BannerManager m_instance;
    
    public static BannerManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new BannerManager();
        }
        return m_instance;
    }
    
    private BannerManager()
    {
    }

    public void Init()
    {
        bannerFetcher = new BannerFetcher();
        bannerFetcher.BannersFetched += OnBannersFetched;
        RefreshBanners();
    }

    // Banners are cached request from api anyway so does not matter to me how fast this is updated. But 5 min should be enough
    public void RefreshBanners()
    {
        CoroutineRunner.Instance.RunCoroutine(bannerFetcher.FetchBannersFromApi());
    }

    private void OnBannersFetched(Dictionary<string, string> bannersByHandle)
    {
        if (bannersByHandle != null)
        {
            cachedBannerByHandle = bannersByHandle;
        }
    }

    public void UpdateBanner(
        int bannerId,
        int emblemId,
        string handle,
        Image bannerBG,
        Image bannerFG,
        bool showEmblem = true)
    {
        UpdateBanner(
            bannerId,
            emblemId,
            handle,
            mainBanner =>
            {
                if (bannerBG == null || mainBanner == null)
                {
                    return;
                }
                bannerBG.sprite = mainBanner;
            },
            secondaryBanner =>
            {
                if (bannerFG == null)
                {
                    return;
                }
                if (secondaryBanner != null)
                {
                    bannerFG.sprite = secondaryBanner;
                    bannerFG.gameObject.SetActive(showEmblem);
                }
                else
                {
                    bannerFG.gameObject.SetActive(false);
                }
            });
    }

    private void UpdateBanner(
        int bannerId,
        int emblemId,
        string handle,
        Action<Sprite> onBannerUpdated,
        Action<Sprite> onEmblemUpdated)
    {
        GameWideData gameData = GameWideData.Get();
        if (gameData == null || gameData.m_gameBalanceVars == null)
        {
            Debug.LogError("GameWideData or GameBalanceVars is null, just why?");
            onBannerUpdated?.Invoke((Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite)));
            onEmblemUpdated?.Invoke((Sprite)Resources.Load(UIPlayerBanner.standardEmblemResourceString, typeof(Sprite)));
            return;
        }

        GameBalanceVars.PlayerBanner banner = gameData.m_gameBalanceVars.GetBanner(bannerId);
        GameBalanceVars.PlayerBanner emblemBanner = gameData.m_gameBalanceVars.GetBanner(emblemId);

        // Return original banner if Custombanners is disabled
        if (HydrogenConfig.Get().DisableCustomBanners) 
        {
            onBannerUpdated?.Invoke(banner != null
                ? (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite))
                : (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite)));

            onEmblemUpdated?.Invoke(emblemBanner != null && emblemId != 0
                ? (Sprite)Resources.Load(emblemBanner.m_resourceString, typeof(Sprite))
                : (Sprite)Resources.Load(UIPlayerBanner.standardEmblemResourceString, typeof(Sprite)));
            return;
        }

        string handleKey = null;

        // Determine if handle is for a bot (no #) or player (contains #)
        if (!string.IsNullOrEmpty(handle))
        {
            if (handle.Contains("#"))
            {
                // Handle for players
                handleKey = handle.Split('#')[0].Trim();
            }
            else
            {
                // Handle for bots
                handleKey = handle.Trim();
            }
        }

        // Check if the handle (bot or player) exists in cachedBannerByHandle
        if (handleKey != null
            && cachedBannerByHandle != null
            && cachedBannerByHandle.TryGetValue(handleKey, out string url))
        {
            // If cached URL exists, load image from URL
            if (CoroutineRunner.Instance != null && bannerFetcher != null)
            {
                CoroutineRunner.Instance.RunCoroutine(
                    bannerFetcher.LoadImageFromURL(
                        url,
                        fetchedBannerSprite =>
                        {
                            if (fetchedBannerSprite != null)
                            {
                                onBannerUpdated?.Invoke(fetchedBannerSprite);
                                onEmblemUpdated?.Invoke(null);
                            }
                            else
                            {
                                onBannerUpdated?.Invoke(banner != null
                                    ? (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite))
                                    : (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite)));
                                onEmblemUpdated?.Invoke(emblemBanner != null
                                    ? (Sprite)Resources.Load(emblemBanner.m_resourceString, typeof(Sprite))
                                    : (Sprite)Resources.Load(UIPlayerBanner.standardEmblemResourceString, typeof(Sprite)));
                            }
                        }));
                return;
            }
            else
            {
                Debug.LogError("CoroutineRunner.Instance or bannerFetcher is null :(.");
            }
        }
        onBannerUpdated?.Invoke(banner != null
            ? (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite))
            : (Sprite)Resources.Load(UIPlayerBanner.standardResourceString, typeof(Sprite)));

        onEmblemUpdated?.Invoke(emblemBanner != null && emblemId != 0
            ? (Sprite)Resources.Load(emblemBanner.m_resourceString, typeof(Sprite))
            : (Sprite)Resources.Load(UIPlayerBanner.standardEmblemResourceString, typeof(Sprite)));
    }
}
#endif