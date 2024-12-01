using System.Collections.Generic;
using UnityEngine;

public class HUD_UI : UIScene
{
    public UIMainScreenPanel m_mainScreenPanel;
    public UIDebugDisplayPanel m_debugDisplayPanel;
    public GameObject m_tutorialFullscreenPanel;
    public CanvasGroup[] m_mainHUDElementContainers;
    public UITauntPlayerBanner m_tauntPlayerBanner;

    private bool m_mainHudElementsVisible;

    private static HUD_UI s_instance;
    private Canvas theCanvas;
    private Camera HUDCam;

    public UITextConsole m_textConsole => UIChatBox.GetChatBox(UIManager.ClientState.InGame);

    public Canvas m_mainCanvas
    {
        get
        {
            if (theCanvas == null)
            {
                theCanvas = UIManager.Get().GetBatchCanvas(this, CanvasBatchType.Static);
            }

            return theCanvas;
        }
    }

    public Camera m_hudCam
    {
        get
        {
            if (HUDCam == null)
            {
                HUDCam = UIManager.Get().GetCamera(CameraLayerName.MainScreenLayer);
            }

            return HUDCam;
        }
    }

    internal static HUD_UI Get()
    {
        return s_instance;
    }

    private void OnDestroy()
    {
        if (HighlightUtils.Get() != null && HighlightUtils.Get().SprintMouseOverCursor != null)
        {
            Destroy(HighlightUtils.Get().SprintMouseOverCursor);
        }

        s_instance = null;
    }

    public override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        s_instance = this;
        UIManager.SetGameObjectActive(m_mainScreenPanel, false);
        UIManager.SetGameObjectActive(m_mainScreenPanel.m_nameplatePanel, false);
        base.Awake();
    }

    public override SceneType GetSceneType()
    {
        return SceneType.HUD;
    }

    private void Start()
    {
        if (m_tutorialFullscreenPanel != null)
        {
            UIManager.SetGameObjectActive(m_tutorialFullscreenPanel, true);
        }

        UIManager.SetGameObjectActive(m_mainScreenPanel, true);
        SetTauntBannerVisibility(false);
    }

    public void SetMainElementsVisible(bool visible, bool hideChat = false)
    {
        if (!visible)
        {
            foreach (CanvasGroup cGroup in m_mainHUDElementContainers)
            {
                cGroup.alpha = 0f;
                cGroup.blocksRaycasts = false;
                cGroup.interactable = false;
            }

            UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
            if (UISystemEscapeMenu.Get() != null)
            {
                UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 0f;
            }

            UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
            if (hideChat)
            {
                CanvasGroup cGroup = m_textConsole.GetComponent<CanvasGroup>();
                cGroup.alpha = 0f;
                cGroup.blocksRaycasts = false;
                cGroup.interactable = false;
            }
        }
        else
        {
            foreach (CanvasGroup cGroup in m_mainHUDElementContainers)
            {
                cGroup.alpha = 1f;
                cGroup.blocksRaycasts = true;
                cGroup.interactable = true;
            }

            UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
            if (UISystemEscapeMenu.Get() != null)
            {
                UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 1f;
            }

            UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
            if (hideChat)
            {
                CanvasGroup cGroup = m_textConsole.GetComponent<CanvasGroup>();
                cGroup.alpha = 1f;
                cGroup.blocksRaycasts = true;
                cGroup.interactable = true;
            }
        }

        UIManager.SetGameObjectActive(UIChatBox.Get().m_overconsPanel, visible);
        UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
        m_mainHudElementsVisible = visible;
    }

    public bool MainHUDElementsVisible()
    {
        return m_mainHudElementsVisible;
    }

    public void SetHUDVisibility(bool visible, bool nameplateVisible)
    {
        UIScreenManager.Get().SetHUDHide(visible, nameplateVisible);
    }

    public void SetupTauntBanner(ActorData actorData)
    {
        PlayerData playerData = actorData.PlayerData;
        if (playerData == null
            || m_tauntPlayerBanner == null
            || m_tauntPlayerBanner.m_playerName == null
            || m_tauntPlayerBanner.m_playerLevel == null
            || m_tauntPlayerBanner.m_playerLevel.gameObject == null
            || m_tauntPlayerBanner.m_playerTitle == null
            || m_tauntPlayerBanner.m_bannerRibbon == null
            || GameFlow.Get() == null
            || GameFlow.Get().playerDetails == null
            || GameManager.Get() == null
            || GameManager.Get().TeamInfo == null
            || GameManager.Get().PlayerInfo == null
            || GameWideData.Get() == null
            || GameWideData.Get().m_gameBalanceVars == null)
        {
            return;
        }

        m_tauntPlayerBanner.m_playerName.text = actorData.GetDisplayName();
        PlayerDetails playerDetails =
            GameFlow.Get().playerDetails[playerData.GetPlayer()];
        if (playerDetails == null)
        {
            return;
        }

        List<LobbyPlayerInfo> playerInfos = new List<LobbyPlayerInfo>();
        playerInfos.AddRange(GameManager.Get().TeamInfo.TeamAPlayerInfo);
        playerInfos.AddRange(GameManager.Get().TeamInfo.TeamBPlayerInfo);
        foreach (LobbyPlayerInfo playerInfo in playerInfos)
        {
            if (playerDetails.m_lobbyPlayerInfoId != playerInfo.PlayerId)
            {
                continue;
            }

            UIManager.SetGameObjectActive(
                m_tauntPlayerBanner.m_playerLevel,
                false);
#if EVOS
            BannerManager.GetInstance()?.UpdateBanner(
                playerInfo.BannerID,
                playerInfo.EmblemID,
                actorData.GetDisplayName(),
                m_tauntPlayerBanner.m_bannerBG,
                m_tauntPlayerBanner.m_bannerFG);
#else
            GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(playerInfo.BannerID);
            string bannerPath = banner != null
                ? banner.m_resourceString
                : UIPlayerBanner.standardResourceString;
            m_tauntPlayerBanner.m_bannerBG.sprite = (Sprite)Resources.Load(bannerPath, typeof(Sprite));
            GameBalanceVars.PlayerBanner emblem = GameWideData.Get().m_gameBalanceVars.GetBanner(playerInfo.EmblemID);
            string emblemPath = emblem != null
                ? emblem.m_resourceString
                : UIPlayerBanner.standardResourceString;
            m_tauntPlayerBanner.m_bannerFG.sprite = (Sprite)Resources.Load(emblemPath, typeof(Sprite));
#endif
            m_tauntPlayerBanner.m_playerTitle.text = GameWideData.Get()
                .m_gameBalanceVars
                .GetTitle(
                    playerInfo.TitleID,
#if EVOS
					actorData.GetDisplayName(), // Custom titles
#endif
                    string.Empty,
                    playerInfo.TitleLevel);

            GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(playerInfo.RibbonID);
            if (ribbon != null && !ribbon.m_resourceString.IsNullOrEmpty())
            {
                m_tauntPlayerBanner.m_bannerRibbon.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
                UIManager.SetGameObjectActive(m_tauntPlayerBanner.m_bannerRibbon, true);
            }
            else
            {
                UIManager.SetGameObjectActive(m_tauntPlayerBanner.m_bannerRibbon, false);
            }

            if (GameManager.Get().PlayerInfo.PlayerId == playerInfo.PlayerId)
            {
                m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_selfIndicatorBar;
                m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_selfIndicatorBarGlow;
            }
            else if (GameManager.Get().PlayerInfo.TeamId == playerInfo.TeamId)
            {
                m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_allyIndicatorBar;
                m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_allyIndicatorBarGlow;
            }
            else
            {
                m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_enemyIndicatorBar;
                m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_enemyIndicatorBarGlow;
            }

            break;
        }
    }

    public void SetTauntBannerVisibility(bool isVisible)
    {
        UIManager.SetGameObjectActive(
            m_tauntPlayerBanner,
            isVisible && !UIScreenManager.Get().GetHideHUDCompletely());
    }

    public Canvas GetTopLevelCanvas()
    {
        return m_mainCanvas;
    }

    public void GameTeardown()
    {
        m_mainScreenPanel.m_sideNotificationsPanel.RemoveHandleMessage();
        Destroy(gameObject);
    }
}
