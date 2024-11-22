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
        if (m_isBot || playerInfoRef.IsRemoteControlled)
        {
            return;
        }
        
        if (m_ggButtonLevelImages != null)
        {
            for (int i = 0; i < m_ggButtonLevelImages.Length; i++)
            {
                UIManager.SetGameObjectActive(m_ggButtonLevelImages[i], i == numGGpacks - 1);
            }
        }

        if (m_animator != null && m_lastGGPackDisplay != numGGpacks)
        {
            string animToPlay = string.Empty;
            string color = m_isRed ? "Red" : "Blue";
            switch (numGGpacks)
            {
                case 0:
                    animToPlay = $"GGBoost{color}TeamItemEmptyIDLE";
                    break;
                case 1:
                    animToPlay = $"GGBoost{color}TeamItemBlueIN";
                    break;
                case 2:
                    animToPlay = $"GGBoost{color}TeamItemSilverIN";
                    break;
                case 3:
                    animToPlay = $"GGBoost{color}TeamItemGoldIN";
                    break;
            }

            UIAnimationEventManager.Get().PlayAnimation(m_animator, animToPlay, null);
            m_lastGGPackDisplay = numGGpacks;
        }
    }

    public void Setup(
        CharacterResourceLink charLink,
        LobbyPlayerInfo playerInfo,
        bool isRed,
        bool IgnoreReplaceWithBots = false)
    {
        charType = charLink.m_characterType;
        playerInfoRef = playerInfo;
        m_isRed = isRed;
        UIManager.SetGameObjectActive(this, true);
        m_profileImage.sprite = charLink.GetCharacterSelectIcon();
        m_profileName.text = playerInfo.GetHandle();
#if EVOS
        // Custom titles
        m_playerTitle.text = GameBalanceVars.Get().GetTitle(playerInfo.TitleID, playerInfo.Handle, string.Empty, playerInfo.TitleLevel);
#else
        m_playerTitle.text = GameBalanceVars.Get().GetTitle(playerInfo.TitleID, string.Empty, playerInfo.TitleLevel);
#endif
        m_playerId = playerInfo.PlayerId;
        m_isBot = playerInfo.IsNPCBot
                  && !playerInfo.BotsMasqueradeAsHumans
                  && !IgnoreReplaceWithBots
                  && !playerInfo.ReplacedWithBots;
        bool isEmblemVisible = true;
        int characterLevel = playerInfo.CharacterInfo.CharacterLevel;
        if (m_botImage != null && !m_isRed)
        {
            UIManager.SetGameObjectActive(m_botImage, m_isBot);
            isEmblemVisible = !m_isBot;
        }

        m_slider.fillAmount = !m_isBot && !playerInfo.IsRemoteControlled ? 0f : 1f;
        int bannerID = -1;
        int emblemID = -1;
        int ribbonID = -1;
        if (playerInfo.IsRemoteControlled)
        {
            foreach (LobbyPlayerInfo info in GameManager.Get().TeamInfo.TeamInfo(playerInfo.TeamId))
            {
                if (info.PlayerId == playerInfo.ControllingPlayerId)
                {
                    bannerID = info.BannerID;
                    emblemID = info.EmblemID;
                    ribbonID = info.RibbonID;
                    foreach (LobbyCharacterInfo characterInfo in info.RemoteCharacterInfos)
                    {
                        if (characterInfo != null && characterInfo.CharacterType == playerInfo.CharacterType)
                        {
                            characterLevel = characterInfo.CharacterLevel;
                            break;
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            bannerID = playerInfo.BannerID;
            emblemID = playerInfo.EmblemID;
            ribbonID = playerInfo.RibbonID;
        }

        GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(bannerID);
        m_bannerImage.sprite = Resources.Load<Sprite>(
            banner != null
                ? banner.m_resourceString
                : UIPlayerBanner.standardResourceString);
        GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(emblemID);
        m_emblemImage.sprite = Resources.Load<Sprite>(
            emblem != null
                ? emblem.m_resourceString
                : UIPlayerBanner.standardEmblemResourceString);
        UIManager.SetGameObjectActive(m_emblemImage, isEmblemVisible);
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
            UIManager.SetGameObjectActive(
                m_characterSilverFrame,
                characterLevel >= GameBalanceVars.Get().CharacterSilverLevel
                && characterLevel < GameBalanceVars.Get().CharacterMasteryLevel);
        }

        if (m_characterGoldFrame != null)
        {
            UIManager.SetGameObjectActive(
                m_characterGoldFrame,
                characterLevel >= GameBalanceVars.Get().CharacterMasteryLevel
                && characterLevel < GameBalanceVars.Get().CharacterPurpleLevel);
        }

        if (m_characterPurpleFrame != null)
        {
            UIManager.SetGameObjectActive(
                m_characterPurpleFrame,
                characterLevel >= GameBalanceVars.Get().CharacterPurpleLevel
                && characterLevel < GameBalanceVars.Get().CharacterRedLevel);
        }

        if (m_characterRedFrame != null)
        {
            UIManager.SetGameObjectActive(
                m_characterRedFrame,
                characterLevel >= GameBalanceVars.Get().CharacterRedLevel
                && characterLevel < GameBalanceVars.Get().CharacterDiamondLevel);
        }

        if (m_characterDiamondFrame != null)
        {
            UIManager.SetGameObjectActive(
                m_characterDiamondFrame,
                characterLevel >= GameBalanceVars.Get().CharacterDiamondLevel
                && characterLevel < GameBalanceVars.Get().CharacterRainbowLevel);
        }

        if (m_characterRainbowFrame != null)
        {
            UIManager.SetGameObjectActive(
                m_characterRainbowFrame,
                characterLevel >= GameBalanceVars.Get().CharacterRainbowLevel);
        }
    }
}