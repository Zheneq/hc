using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModePlayerDraftEntry : UIRankedModeDraftCharacterEntry
{
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
		if (this.m_requestSwap != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.Start()).MethodHandle;
			}
			this.m_requestSwap.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RequestSwapClicked);
			this.m_requestSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
		}
		if (this.m_acceptSwap != null)
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
			this.m_acceptSwap.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AcceptSwapClicked);
			this.m_acceptSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
		}
		if (this.m_rejectSwap != null)
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
			this.m_rejectSwap.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RejectSwapClicked);
			this.m_rejectSwap.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSwapClick;
		}
	}

	public void RequestSwapClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendRankedTradeRequest_AcceptOrOffer(base.GetSelectedCharacter());
	}

	public void AcceptSwapClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendRankedTradeRequest_AcceptOrOffer(base.GetSelectedCharacter());
	}

	public void RejectSwapClicked(BaseEventData data)
	{
		ClientGameManager.Get().SendRankedTradeRequest_Reject(base.GetSelectedCharacter());
	}

	public void Dismantle()
	{
		this.PlayerID = -1;
		this.AccountID = -1L;
	}

	public void Setup(LobbyPlayerInfo info, bool isEnemy = false)
	{
		this.AccountID = info.AccountId;
		this.PlayerID = info.PlayerId;
		if (isEnemy)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.Setup(LobbyPlayerInfo, bool)).MethodHandle;
			}
			this.m_playerName.text = string.Empty;
			this.m_playerTitle.text = string.Empty;
			this.m_playerLevel.text = string.Empty;
			this.SetBanner(null, GameBalanceVars.PlayerBanner.BannerType.Background);
			this.SetBanner(null, GameBalanceVars.PlayerBanner.BannerType.Foreground);
			this.SetRibbon(null);
		}
		else
		{
			this.m_playerName.text = info.GetHandle();
			this.m_playerTitle.text = GameBalanceVars.Get().GetTitle(info.TitleID, string.Empty, info.TitleLevel);
			this.m_playerLevel.text = string.Empty;
			GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(info.BannerID);
			GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(info.EmblemID);
			this.SetBanner(banner, GameBalanceVars.PlayerBanner.BannerType.Background);
			this.SetBanner(banner2, GameBalanceVars.PlayerBanner.BannerType.Foreground);
			GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(info.RibbonID);
			this.SetRibbon(ribbon);
		}
		if (this.m_requestSwap != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_requestSwap, true, null);
		}
		if (this.m_acceptSwap != null)
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
			UIManager.SetGameObjectActive(this.m_acceptSwap, true, null);
		}
		if (this.m_rejectSwap != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_rejectSwap, true, null);
		}
		this.CanBeTraded = true;
	}

	public void SetTradePhase(bool tradePhaseActive)
	{
		if (this.m_TradeButtonContainer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.SetTradePhase(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_TradeButtonContainer, tradePhaseActive, null);
		}
	}

	public void SetCharacterLocked(bool locked)
	{
		if (this.m_lockedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_lockedContainer, locked, null);
		}
	}

	public void SetTradeStatus(UIRankedModePlayerDraftEntry.TradeStatus status, bool isSelf, bool selfLockedIn)
	{
		if (this.m_tradeSent != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.SetTradeStatus(UIRankedModePlayerDraftEntry.TradeStatus, bool, bool)).MethodHandle;
			}
			Component tradeSent = this.m_tradeSent;
			bool doActive;
			if (status == UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!isSelf)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					doActive = !selfLockedIn;
					goto IL_49;
				}
			}
			doActive = false;
			IL_49:
			UIManager.SetGameObjectActive(tradeSent, doActive, null);
		}
		if (this.m_tradeReceived != null)
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
			Component tradeReceived = this.m_tradeReceived;
			bool doActive2;
			if (status == UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived)
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
				if (!isSelf)
				{
					doActive2 = !selfLockedIn;
					goto IL_87;
				}
			}
			doActive2 = false;
			IL_87:
			UIManager.SetGameObjectActive(tradeReceived, doActive2, null);
		}
		if (this.m_noTrade != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Component noTrade = this.m_noTrade;
			bool doActive3;
			if (status == UIRankedModePlayerDraftEntry.TradeStatus.NoTrade)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!isSelf)
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
					doActive3 = !selfLockedIn;
					goto IL_CC;
				}
			}
			doActive3 = false;
			IL_CC:
			UIManager.SetGameObjectActive(noTrade, doActive3, null);
		}
		if (this.m_lockedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_lockedContainer, status == UIRankedModePlayerDraftEntry.TradeStatus.StopTrading, null);
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.SetBanner(GameBalanceVars.PlayerBanner, GameBalanceVars.PlayerBanner.BannerType)).MethodHandle;
				}
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", banner.m_resourceString), new object[0]);
			}
		}
		else
		{
			sprite = (Sprite)Resources.Load("Banners/Background/rankedRedDefault", typeof(Sprite));
			if (sprite == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(Log.Category.UI, string.Format("Could not load banner resource from [{0}] as sprite.", "Banners/Background/rankedRedDefault"), new object[0]);
			}
		}
		if (sprite != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (bannerType == GameBalanceVars.PlayerBanner.BannerType.Background)
			{
				this.m_bannerImage.sprite = sprite;
			}
			else
			{
				this.m_profileImage.sprite = sprite;
			}
		}
	}

	private void SetRibbon(GameBalanceVars.PlayerRibbon ribbon)
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModePlayerDraftEntry.SetRibbon(GameBalanceVars.PlayerRibbon)).MethodHandle;
			}
			Sprite sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			if (sprite == null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(Log.Category.UI, string.Format("Could not load ribbon resource from [{0}] as sprite.", ribbon.m_resourceString), new object[0]);
			}
			this.m_ribbonImage.sprite = sprite;
			UIManager.SetGameObjectActive(this.m_ribbonImage, sprite != null, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
	}

	public enum TradeStatus
	{
		NoTrade,
		TradeRequestSent,
		TradeRequestReceived,
		StopTrading
	}
}
