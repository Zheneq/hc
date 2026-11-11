using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModePlayerDraftEntry : UIRankedModeDraftCharacterEntry
{
    public enum TradeStatus
    {
        NoTrade,
        TradeRequestSent,
        TradeRequestReceived,
        StopTrading
    }

    public _SelectableBtn m_btn;
    public Image m_bannerImage;
    public Image m_profileImage;
    public Image m_ribbonImage;
    public TextMeshProUGUI m_playerName;
    public TextMeshProUGUI m_playerTitle;
    public TextMeshProUGUI m_playerLevel;
    public _SelectableBtn m_requestSwap;
    public _SelectableBtn m_acceptSwap;
    public _SelectableBtn m_rejectSwap;
    public RectTransform m_TradeButtonContainer;
    public RectTransform m_tradeSent;
    public RectTransform m_tradeReceived;
    public RectTransform m_noTrade;
    public RectTransform m_lockedContainer;

    public long AccountID { get; private set; }
    public int PlayerID { get; private set; }
    public bool CanBeTraded { get; set; }

    private void Start()
    {
        if (m_requestSwap != null)
        {
            m_requestSwap.spriteController.callback = RequestSwapClicked;
            m_requestSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
        }

        if (m_acceptSwap != null)
        {
            m_acceptSwap.spriteController.callback = AcceptSwapClicked;
            m_acceptSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
        }

        if (m_rejectSwap != null)
        {
            m_rejectSwap.spriteController.callback = RejectSwapClicked;
            m_rejectSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
        }

#if EVOS
        // custom: EVOS multilancer draft - click your own slot during loadout to edit it.
        // m_btn gets no pointer events, so listen on the whole entry
        // (clicks on child elements that do not handle them are passed up here).
        UIEventTriggerUtils.AddListener(gameObject, EventTriggerType.PointerClick, SlotClicked);
#endif
    }

#if EVOS
    public void SlotClicked(BaseEventData data)
    {
        UIRankedModeDraftScreen.Get()?.ClickedDraftSlot(this);
    }

    private bool m_isSlotClickable;
    private Graphic[] m_slotHitAreas;
    private bool[] m_defaultRaycastTargets;

    // custom: the entry does not get pointer events in the prefab. While the slot can be clicked,
    // make the whole entry a hit area (a transparent image on it), and the portrait in case it does not cover it.
    public void SetSlotClickable(bool clickable)
    {
        if (m_isSlotClickable == clickable)
        {
            return;
        }

        m_isSlotClickable = clickable;
        if (m_slotHitAreas == null)
        {
            Graphic entryHitArea = GetComponent<Graphic>();
            if (entryHitArea == null)
            {
                entryHitArea = gameObject.AddComponent<Image>();
                entryHitArea.color = Color.clear;
                entryHitArea.raycastTarget = false;
            }

            m_slotHitAreas = new[] { entryHitArea, m_selectCharacterImage };
            m_defaultRaycastTargets = new bool[m_slotHitAreas.Length];
            for (int i = 0; i < m_slotHitAreas.Length; i++)
            {
                m_defaultRaycastTargets[i] = m_slotHitAreas[i] != null && m_slotHitAreas[i].raycastTarget;
            }
        }

        for (int i = 0; i < m_slotHitAreas.Length; i++)
        {
            if (m_slotHitAreas[i] != null)
            {
                m_slotHitAreas[i].raycastTarget = clickable || m_defaultRaycastTargets[i];
            }
        }
    }
#endif

    public void RequestSwapClicked(BaseEventData data)
    {
        ClientGameManager.Get().SendRankedTradeRequest_AcceptOrOffer(GetSelectedCharacter());
    }

    public void AcceptSwapClicked(BaseEventData data)
    {
        ClientGameManager.Get().SendRankedTradeRequest_AcceptOrOffer(GetSelectedCharacter());
    }

    public void RejectSwapClicked(BaseEventData data)
    {
        ClientGameManager.Get().SendRankedTradeRequest_Reject(GetSelectedCharacter());
    }

    public void Dismantle()
    {
        PlayerID = -1;
        AccountID = -1L;
#if EVOS
        SetSlotClickable(false);
#endif
    }

    public void Setup(LobbyPlayerInfo info, bool isEnemy = false)
    {
        AccountID = info.AccountId;
        PlayerID = info.PlayerId;
        if (isEnemy)
        {
            m_playerName.text = string.Empty;
            m_playerTitle.text = string.Empty;
            m_playerLevel.text = string.Empty;
            SetBanner(null, GameBalanceVars.PlayerBanner.BannerType.Background);
            SetBanner(null, GameBalanceVars.PlayerBanner.BannerType.Foreground);
            SetRibbon(null);
        }
        else
        {
            m_playerName.text = info.GetHandle();
#if EVOS
            // Custom titles
            m_playerTitle.text = GameBalanceVars.Get().GetTitle(info.TitleID, info.GetHandle(), string.Empty, info.TitleLevel);
#else
            m_playerTitle.text = GameBalanceVars.Get().GetTitle(info.TitleID, string.Empty, info.TitleLevel);
#endif
            m_playerLevel.text = string.Empty;
#if EVOS
            BannerManager.GetInstance()?.UpdateBanner(
                info.BannerID,
                info.EmblemID,
                info.GetHandle(),
                m_bannerImage,
                m_profileImage);
#else
            GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(info.BannerID);
            GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(info.EmblemID);
            SetBanner(banner, GameBalanceVars.PlayerBanner.BannerType.Background);
            SetBanner(emblem, GameBalanceVars.PlayerBanner.BannerType.Foreground);
#endif
            GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(info.RibbonID);
            SetRibbon(ribbon);
        }

        if (m_requestSwap != null)
        {
            UIManager.SetGameObjectActive(m_requestSwap, true);
        }

        if (m_acceptSwap != null)
        {
            UIManager.SetGameObjectActive(m_acceptSwap, true);
        }

        if (m_rejectSwap != null)
        {
            UIManager.SetGameObjectActive(m_rejectSwap, true);
        }

        CanBeTraded = true;
    }

    public void SetTradePhase(bool tradePhaseActive)
    {
        if (m_TradeButtonContainer != null)
        {
            UIManager.SetGameObjectActive(m_TradeButtonContainer, tradePhaseActive);
        }
    }

    public void SetCharacterLocked(bool locked)
    {
        if (m_lockedContainer != null)
        {
            UIManager.SetGameObjectActive(m_lockedContainer, locked);
        }
    }

    public void SetTradeStatus(TradeStatus status, bool isSelf, bool selfLockedIn)
    {
        if (m_tradeSent != null)
        {
            UIManager.SetGameObjectActive(
                m_tradeSent, 
                status == TradeStatus.TradeRequestSent && !isSelf && !selfLockedIn);
        }

        if (m_tradeReceived != null)
        {
            UIManager.SetGameObjectActive(
                m_tradeReceived, 
                status == TradeStatus.TradeRequestReceived && !isSelf && !selfLockedIn);
        }

        if (m_noTrade != null)
        {
            UIManager.SetGameObjectActive(
                m_noTrade, 
                status == TradeStatus.NoTrade && !isSelf && !selfLockedIn);
        }

        if (m_lockedContainer != null)
        {
            UIManager.SetGameObjectActive(
                m_lockedContainer,
                status == TradeStatus.StopTrading);
        }
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
            sprite = (Sprite)Resources.Load(UIPlayerBanner.rankedModeEnemyBannerResourceString, typeof(Sprite));
            if (sprite == null)
            {
                Log.Warning(
                    Log.Category.UI,
                    $"Could not load banner resource from [{UIPlayerBanner.rankedModeEnemyBannerResourceString}] as sprite.");
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
        if (ribbon != null)
        {
            Sprite sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
            if (sprite == null)
            {
                Log.Warning(
                    Log.Category.UI,
                    $"Could not load ribbon resource from [{ribbon.m_resourceString}] as sprite.");
            }

            m_ribbonImage.sprite = sprite;
            UIManager.SetGameObjectActive(m_ribbonImage, sprite != null);
        }
        else
        {
            UIManager.SetGameObjectActive(m_ribbonImage, false);
        }
    }
}