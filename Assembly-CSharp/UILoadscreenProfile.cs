using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadscreenProfile : MonoBehaviour
{
	public Image m_profileImage;

	public TextMeshProUGUI m_profileName;

	public TextMeshProUGUI m_playerTitle;

	public Image m_slider;

	public Image m_bannerImage;

	public Image m_emblemImage;

	public Image m_ribbonImage;

	public Image m_partyImage;

	public Image m_botImage;

	public RectTransform m_characterSilverFrame;

	public RectTransform m_characterGoldFrame;

	public RectTransform m_characterPurpleFrame;

	public RectTransform m_characterRedFrame;

	public RectTransform m_characterDiamondFrame;

	public RectTransform m_characterRainbowFrame;

	public int m_playerId;

	public RectTransform m_botBannerContainer;

	public RectTransform[] m_ggButtonLevelImages;

	public Animator m_animator;

	private bool m_isBot;

	private CharacterType charType;

	private LobbyPlayerInfo playerInfoRef;

	private bool m_isRed;

	private int m_lastGGPackDisplay = -1;

	public int CurrentGGPackLevel
	{
		get
		{
			return this.m_lastGGPackDisplay;
		}
	}

	public CharacterType GetCharType()
	{
		return this.charType;
	}

	public LobbyPlayerInfo GetPlayerInfo()
	{
		return this.playerInfoRef;
	}

	public void SetGGButtonLevel(int numGGpacks)
	{
		if (!this.m_isBot)
		{
			if (!this.playerInfoRef.IsRemoteControlled)
			{
				if (this.m_ggButtonLevelImages != null)
				{
					for (int i = 0; i < this.m_ggButtonLevelImages.Length; i++)
					{
						UIManager.SetGameObjectActive(this.m_ggButtonLevelImages[i], i == numGGpacks - 1, null);
					}
				}
				if (this.m_animator != null)
				{
					string arg = string.Empty;
					if (this.m_isRed)
					{
						arg = "Red";
					}
					else
					{
						arg = "Blue";
					}
					if (this.m_lastGGPackDisplay != numGGpacks)
					{
						string animToPlay = string.Empty;
						if (numGGpacks == 0)
						{
							animToPlay = string.Format("GGBoost{0}TeamItemEmptyIDLE", arg);
						}
						else if (numGGpacks == 1)
						{
							animToPlay = string.Format("GGBoost{0}TeamItemBlueIN", arg);
						}
						else if (numGGpacks == 2)
						{
							animToPlay = string.Format("GGBoost{0}TeamItemSilverIN", arg);
						}
						else if (numGGpacks == 3)
						{
							animToPlay = string.Format("GGBoost{0}TeamItemGoldIN", arg);
						}
						UIAnimationEventManager.Get().PlayAnimation(this.m_animator, animToPlay, null, string.Empty, 0, 0f, true, false, null, null);
						this.m_lastGGPackDisplay = numGGpacks;
					}
				}
				return;
			}
		}
	}

	public void Setup(CharacterResourceLink charLink, LobbyPlayerInfo playerInfo, bool isRed, bool IgnoreReplaceWithBots = false)
	{
		this.charType = charLink.m_characterType;
		this.playerInfoRef = playerInfo;
		this.m_isRed = isRed;
		UIManager.SetGameObjectActive(this, true, null);
		this.m_profileImage.sprite = charLink.GetCharacterSelectIcon();
		this.m_profileName.text = playerInfo.GetHandle();
		this.m_playerTitle.text = GameBalanceVars.Get().GetTitle(playerInfo.TitleID, string.Empty, playerInfo.TitleLevel);
		this.m_playerId = playerInfo.PlayerId;
		bool isBot;
		if (playerInfo.IsNPCBot)
		{
			if (!playerInfo.BotsMasqueradeAsHumans)
			{
				isBot = (!IgnoreReplaceWithBots && !playerInfo.ReplacedWithBots);
				goto IL_BD;
			}
		}
		isBot = false;
		IL_BD:
		this.m_isBot = isBot;
		bool doActive = true;
		int characterLevel = playerInfo.CharacterInfo.CharacterLevel;
		if (this.m_botImage != null && !this.m_isRed)
		{
			UIManager.SetGameObjectActive(this.m_botImage, this.m_isBot, null);
			doActive = !this.m_isBot;
		}
		if (!this.m_isBot)
		{
			if (!playerInfo.IsRemoteControlled)
			{
				this.m_slider.fillAmount = 0f;
				goto IL_140;
			}
		}
		this.m_slider.fillAmount = 1f;
		IL_140:
		int bannerID = -1;
		int bannerID2 = -1;
		int ribbonID = -1;
		if (playerInfo.IsRemoteControlled)
		{
			IEnumerator<LobbyPlayerInfo> enumerator = GameManager.Get().TeamInfo.TeamInfo(playerInfo.TeamId).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					if (lobbyPlayerInfo.PlayerId == playerInfo.ControllingPlayerId)
					{
						bannerID = lobbyPlayerInfo.BannerID;
						bannerID2 = lobbyPlayerInfo.EmblemID;
						ribbonID = lobbyPlayerInfo.RibbonID;
						for (int i = 0; i < lobbyPlayerInfo.RemoteCharacterInfos.Count; i++)
						{
							if (lobbyPlayerInfo.RemoteCharacterInfos[i] != null)
							{
								if (lobbyPlayerInfo.RemoteCharacterInfos[i].CharacterType == playerInfo.CharacterType)
								{
									characterLevel = lobbyPlayerInfo.RemoteCharacterInfos[i].CharacterLevel;
									break;
								}
							}
						}
						goto IL_261;
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			IL_261:;
		}
		else
		{
			bannerID = playerInfo.BannerID;
			bannerID2 = playerInfo.EmblemID;
			ribbonID = playerInfo.RibbonID;
		}
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerID);
		string path;
		if (banner != null)
		{
			path = banner.m_resourceString;
		}
		else
		{
			path = "Banners/Background/02_blue";
		}
		this.m_bannerImage.sprite = Resources.Load<Sprite>(path);
		GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerID2);
		string path2;
		if (banner2 != null)
		{
			path2 = banner2.m_resourceString;
		}
		else
		{
			path2 = "Banners/Emblems/Chest01";
		}
		this.m_emblemImage.sprite = Resources.Load<Sprite>(path2);
		UIManager.SetGameObjectActive(this.m_emblemImage, doActive, null);
		GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(ribbonID);
		if (ribbon != null)
		{
			this.m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			UIManager.SetGameObjectActive(this.m_ribbonImage, this.m_ribbonImage.sprite != null, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_ribbonImage, false, null);
		}
		if (this.m_partyImage != null)
		{
			UIManager.SetGameObjectActive(this.m_partyImage, false, null);
		}
		if (this.m_characterSilverFrame != null)
		{
			Component characterSilverFrame = this.m_characterSilverFrame;
			bool doActive2;
			if (characterLevel >= GameBalanceVars.Get().CharacterSilverLevel)
			{
				doActive2 = (characterLevel < GameBalanceVars.Get().CharacterMasteryLevel);
			}
			else
			{
				doActive2 = false;
			}
			UIManager.SetGameObjectActive(characterSilverFrame, doActive2, null);
		}
		if (this.m_characterGoldFrame != null)
		{
			Component characterGoldFrame = this.m_characterGoldFrame;
			bool doActive3;
			if (characterLevel >= GameBalanceVars.Get().CharacterMasteryLevel)
			{
				doActive3 = (characterLevel < GameBalanceVars.Get().CharacterPurpleLevel);
			}
			else
			{
				doActive3 = false;
			}
			UIManager.SetGameObjectActive(characterGoldFrame, doActive3, null);
		}
		if (this.m_characterPurpleFrame != null)
		{
			Component characterPurpleFrame = this.m_characterPurpleFrame;
			bool doActive4;
			if (characterLevel >= GameBalanceVars.Get().CharacterPurpleLevel)
			{
				doActive4 = (characterLevel < GameBalanceVars.Get().CharacterRedLevel);
			}
			else
			{
				doActive4 = false;
			}
			UIManager.SetGameObjectActive(characterPurpleFrame, doActive4, null);
		}
		if (this.m_characterRedFrame != null)
		{
			UIManager.SetGameObjectActive(this.m_characterRedFrame, characterLevel >= GameBalanceVars.Get().CharacterRedLevel && characterLevel < GameBalanceVars.Get().CharacterDiamondLevel, null);
		}
		if (this.m_characterDiamondFrame != null)
		{
			UIManager.SetGameObjectActive(this.m_characterDiamondFrame, characterLevel >= GameBalanceVars.Get().CharacterDiamondLevel && characterLevel < GameBalanceVars.Get().CharacterRainbowLevel, null);
		}
		if (this.m_characterRainbowFrame != null)
		{
			UIManager.SetGameObjectActive(this.m_characterRainbowFrame, characterLevel >= GameBalanceVars.Get().CharacterRainbowLevel, null);
		}
	}
}
