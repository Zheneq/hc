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

	public int CurrentGGPackLevel => m_lastGGPackDisplay;

	public CharacterType GetCharType()
	{
		return charType;
	}

	public LobbyPlayerInfo GetPlayerInfo()
	{
		return playerInfoRef;
	}

	public void SetGGButtonLevel(int numGGpacks)
	{
		if (m_isBot)
		{
			return;
		}
		if (playerInfoRef.IsRemoteControlled)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_ggButtonLevelImages != null)
		{
			for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
			{
				UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == numGGpacks - 1);
			}
		}
		if (!(m_animator != null))
		{
			return;
		}
		while (true)
		{
			string empty = string.Empty;
			empty = ((!m_isRed) ? "Blue" : "Red");
			if (m_lastGGPackDisplay == numGGpacks)
			{
				return;
			}
			while (true)
			{
				string animToPlay = string.Empty;
				if (numGGpacks == 0)
				{
					animToPlay = $"GGBoost{empty}TeamItemEmptyIDLE";
				}
				else if (numGGpacks == 1)
				{
					animToPlay = $"GGBoost{empty}TeamItemBlueIN";
				}
				else if (numGGpacks == 2)
				{
					animToPlay = $"GGBoost{empty}TeamItemSilverIN";
				}
				else if (numGGpacks == 3)
				{
					animToPlay = $"GGBoost{empty}TeamItemGoldIN";
				}
				UIAnimationEventManager.Get().PlayAnimation(m_animator, animToPlay, null, string.Empty);
				m_lastGGPackDisplay = numGGpacks;
				return;
			}
		}
	}

	public void Setup(CharacterResourceLink charLink, LobbyPlayerInfo playerInfo, bool isRed, bool IgnoreReplaceWithBots = false)
	{
		charType = charLink.m_characterType;
		playerInfoRef = playerInfo;
		m_isRed = isRed;
		UIManager.SetGameObjectActive(this, true);
		m_profileImage.sprite = charLink.GetCharacterSelectIcon();
		m_profileName.text = playerInfo.GetHandle();
#if !VANILLA && !SERVER
        // Custom titles
        m_playerTitle.text = GameBalanceVars.Get().GetTitle(playerInfo.TitleID, playerInfo.Handle, string.Empty, playerInfo.TitleLevel);
#else
		m_playerTitle.text = GameBalanceVars.Get().GetTitle(playerInfo.TitleID, string.Empty, playerInfo.TitleLevel);
#endif
		m_playerId = playerInfo.PlayerId;
		int isBot;
		if (playerInfo.IsNPCBot)
		{
			if (!playerInfo.BotsMasqueradeAsHumans)
			{
				isBot = ((!IgnoreReplaceWithBots && !playerInfo.ReplacedWithBots) ? 1 : 0);
				goto IL_00bd;
			}
		}
		isBot = 0;
		goto IL_00bd;
		IL_00bd:
		m_isBot = ((byte)isBot != 0);
		bool doActive = true;
		int characterLevel = playerInfo.CharacterInfo.CharacterLevel;
		if (m_botImage != null && !m_isRed)
		{
			UIManager.SetGameObjectActive(m_botImage, m_isBot);
			doActive = !m_isBot;
		}
		if (!m_isBot)
		{
			if (!playerInfo.IsRemoteControlled)
			{
				m_slider.fillAmount = 0f;
				goto IL_0140;
			}
		}
		m_slider.fillAmount = 1f;
		goto IL_0140;
		IL_0140:
		int bannerID = -1;
		int bannerID2 = -1;
		int ribbonID = -1;
		if (playerInfo.IsRemoteControlled)
		{
			IEnumerator<LobbyPlayerInfo> enumerator = GameManager.Get().TeamInfo.TeamInfo(playerInfo.TeamId).GetEnumerator();
			try
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					LobbyPlayerInfo current = enumerator.Current;
					if (current.PlayerId == playerInfo.ControllingPlayerId)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
							{
								bannerID = current.BannerID;
								bannerID2 = current.EmblemID;
								ribbonID = current.RibbonID;
								for (int i = 0; i < current.RemoteCharacterInfos.Count; i++)
								{
									if (current.RemoteCharacterInfos[i] != null)
									{
										if (current.RemoteCharacterInfos[i].CharacterType == playerInfo.CharacterType)
										{
											characterLevel = current.RemoteCharacterInfos[i].CharacterLevel;
											break;
										}
									}
								}
								goto end_IL_0180;
							}
							}
						}
					}
				}
				end_IL_0180:;
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_024b;
						}
					}
				}
				end_IL_024b:;
			}
		}
		else
		{
			bannerID = playerInfo.BannerID;
			bannerID2 = playerInfo.EmblemID;
			ribbonID = playerInfo.RibbonID;
		}
		GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerID);
		string path = (banner == null) ? "Banners/Background/02_blue" : banner.m_resourceString;
		m_bannerImage.sprite = Resources.Load<Sprite>(path);
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
		m_emblemImage.sprite = Resources.Load<Sprite>(path2);
		UIManager.SetGameObjectActive(m_emblemImage, doActive);
		GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(ribbonID);
		if (ribbon != null)
		{
			m_ribbonImage.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
			UIManager.SetGameObjectActive(m_ribbonImage, m_ribbonImage.sprite != null);
		}
		else
		{
			UIManager.SetGameObjectActive(m_ribbonImage, false);
		}
		if (m_partyImage != null)
		{
			UIManager.SetGameObjectActive(m_partyImage, false);
		}
		if (m_characterSilverFrame != null)
		{
			RectTransform characterSilverFrame = m_characterSilverFrame;
			int doActive2;
			if (characterLevel >= GameBalanceVars.Get().CharacterSilverLevel)
			{
				doActive2 = ((characterLevel < GameBalanceVars.Get().CharacterMasteryLevel) ? 1 : 0);
			}
			else
			{
				doActive2 = 0;
			}
			UIManager.SetGameObjectActive(characterSilverFrame, (byte)doActive2 != 0);
		}
		if (m_characterGoldFrame != null)
		{
			RectTransform characterGoldFrame = m_characterGoldFrame;
			int doActive3;
			if (characterLevel >= GameBalanceVars.Get().CharacterMasteryLevel)
			{
				doActive3 = ((characterLevel < GameBalanceVars.Get().CharacterPurpleLevel) ? 1 : 0);
			}
			else
			{
				doActive3 = 0;
			}
			UIManager.SetGameObjectActive(characterGoldFrame, (byte)doActive3 != 0);
		}
		if (m_characterPurpleFrame != null)
		{
			RectTransform characterPurpleFrame = m_characterPurpleFrame;
			int doActive4;
			if (characterLevel >= GameBalanceVars.Get().CharacterPurpleLevel)
			{
				doActive4 = ((characterLevel < GameBalanceVars.Get().CharacterRedLevel) ? 1 : 0);
			}
			else
			{
				doActive4 = 0;
			}
			UIManager.SetGameObjectActive(characterPurpleFrame, (byte)doActive4 != 0);
		}
		if (m_characterRedFrame != null)
		{
			UIManager.SetGameObjectActive(m_characterRedFrame, characterLevel >= GameBalanceVars.Get().CharacterRedLevel && characterLevel < GameBalanceVars.Get().CharacterDiamondLevel);
		}
		if (m_characterDiamondFrame != null)
		{
			UIManager.SetGameObjectActive(m_characterDiamondFrame, characterLevel >= GameBalanceVars.Get().CharacterDiamondLevel && characterLevel < GameBalanceVars.Get().CharacterRainbowLevel);
		}
		if (!(m_characterRainbowFrame != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_characterRainbowFrame, characterLevel >= GameBalanceVars.Get().CharacterRainbowLevel);
			return;
		}
	}
}
