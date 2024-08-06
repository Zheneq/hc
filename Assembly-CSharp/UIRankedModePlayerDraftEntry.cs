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

	public long AccountID
	{
		get;
		private set;
	}

	public int PlayerID
	{
		get;
		private set;
	}

	public bool CanBeTraded
	{
		get;
		set;
	}

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
		if (!(m_rejectSwap != null))
		{
			return;
		}
		while (true)
		{
			m_rejectSwap.spriteController.callback = RejectSwapClicked;
			m_rejectSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
			return;
		}
	}

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
#if !VANILLA && !SERVER
            // Custom titles
            m_playerTitle.text = GameBalanceVars.Get().GetTitle(info.TitleID, info.GetHandle(), string.Empty, info.TitleLevel);
#else
			m_playerTitle.text = GameBalanceVars.Get().GetTitle(info.TitleID, string.Empty, info.TitleLevel);
#endif
			m_playerLevel.text = string.Empty;
			GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(info.BannerID);
			GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(info.EmblemID);
			SetBanner(banner, GameBalanceVars.PlayerBanner.BannerType.Background);
			SetBanner(banner2, GameBalanceVars.PlayerBanner.BannerType.Foreground);
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
		if (!(m_TradeButtonContainer != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_TradeButtonContainer, tradePhaseActive);
			return;
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
		RectTransform tradeSent;
		int doActive;
		if (m_tradeSent != null)
		{
			tradeSent = m_tradeSent;
			if (status == TradeStatus.TradeRequestSent)
			{
				if (!isSelf)
				{
					doActive = ((!selfLockedIn) ? 1 : 0);
					goto IL_0049;
				}
			}
			doActive = 0;
			goto IL_0049;
		}
		goto IL_004f;
		IL_008d:
		RectTransform noTrade;
		int doActive2;
		if (m_noTrade != null)
		{
			noTrade = m_noTrade;
			if (status == TradeStatus.NoTrade)
			{
				if (!isSelf)
				{
					doActive2 = ((!selfLockedIn) ? 1 : 0);
					goto IL_00cc;
				}
			}
			doActive2 = 0;
			goto IL_00cc;
		}
		goto IL_00d2;
		IL_00d2:
		if (m_lockedContainer != null)
		{
			UIManager.SetGameObjectActive(m_lockedContainer, status == TradeStatus.StopTrading);
		}
		return;
		IL_0049:
		UIManager.SetGameObjectActive(tradeSent, (byte)doActive != 0);
		goto IL_004f;
		IL_004f:
		RectTransform tradeReceived;
		int doActive3;
		if (m_tradeReceived != null)
		{
			tradeReceived = m_tradeReceived;
			if (status == TradeStatus.TradeRequestReceived)
			{
				if (!isSelf)
				{
					doActive3 = ((!selfLockedIn) ? 1 : 0);
					goto IL_0087;
				}
			}
			doActive3 = 0;
			goto IL_0087;
		}
		goto IL_008d;
		IL_00cc:
		UIManager.SetGameObjectActive(noTrade, (byte)doActive2 != 0);
		goto IL_00d2;
		IL_0087:
		UIManager.SetGameObjectActive(tradeReceived, (byte)doActive3 != 0);
		goto IL_008d;
	}

	private void SetBanner(GameBalanceVars.PlayerBanner banner, GameBalanceVars.PlayerBanner.BannerType bannerType)
	{
		Sprite sprite = null;
		if (banner != null)
		{
			sprite = (Sprite)Resources.Load(banner.m_resourceString, typeof(Sprite));
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, $"Could not load banner resource from [{banner.m_resourceString}] as sprite.");
			}
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/rankedRedDefault", typeof(Sprite));
			if (sprite == null)
			{
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", "Banners/Background/rankedRedDefault"));
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
				m_bannerImage.sprite = sprite;
			}
			else
			{
				m_profileImage.sprite = sprite;
			}
			return;
		}
	}

	private void SetRibbon(GameBalanceVars.PlayerRibbon ribbon)
	{
		Sprite sprite = null;
		if (ribbon != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
					if (sprite == null)
					{
						Log.Warning(Log.Category.UI, $"Could not load ribbon resource from [{ribbon.m_resourceString}] as sprite.");
					}
					m_ribbonImage.sprite = sprite;
					UIManager.SetGameObjectActive(m_ribbonImage, sprite != null);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_ribbonImage, false);
	}
}
