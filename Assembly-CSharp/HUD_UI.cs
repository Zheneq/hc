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
		if (HighlightUtils.Get() != null)
		{
			if (HighlightUtils.Get().SprintMouseOverCursor != null)
			{
				Object.Destroy(HighlightUtils.Get().SprintMouseOverCursor);
			}
		}
		s_instance = null;
	}

	public override void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
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
			for (int i = 0; i < m_mainHUDElementContainers.Length; i++)
			{
				m_mainHUDElementContainers[i].alpha = 0f;
				m_mainHUDElementContainers[i].blocksRaycasts = false;
				m_mainHUDElementContainers[i].interactable = false;
			}
			UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
			if (UISystemEscapeMenu.Get() != null)
			{
				UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 0f;
			}
			UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
			if (hideChat)
			{
				CanvasGroup component = m_textConsole.GetComponent<CanvasGroup>();
				component.alpha = 0f;
				component.blocksRaycasts = false;
				component.interactable = false;
			}
		}
		else
		{
			for (int j = 0; j < m_mainHUDElementContainers.Length; j++)
			{
				m_mainHUDElementContainers[j].alpha = 1f;
				m_mainHUDElementContainers[j].blocksRaycasts = true;
				m_mainHUDElementContainers[j].interactable = true;
			}
			UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
			if (UISystemEscapeMenu.Get() != null)
			{
				UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 1f;
			}
			UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
			if (hideChat)
			{
				CanvasGroup component2 = m_textConsole.GetComponent<CanvasGroup>();
				component2.alpha = 1f;
				component2.blocksRaycasts = true;
				component2.interactable = true;
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
		if (playerData == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_tauntPlayerBanner == null)
		{
			return;
		}
		while (true)
		{
			if (m_tauntPlayerBanner.m_playerName == null)
			{
				return;
			}
			while (true)
			{
				if (m_tauntPlayerBanner.m_playerLevel == null)
				{
					return;
				}
				while (true)
				{
					if (m_tauntPlayerBanner.m_playerLevel.gameObject == null || m_tauntPlayerBanner.m_playerTitle == null)
					{
						return;
					}
					if (m_tauntPlayerBanner.m_bannerRibbon == null)
					{
						while (true)
						{
							switch (5)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					if (GameFlow.Get() == null)
					{
						return;
					}
					if (GameFlow.Get().playerDetails == null)
					{
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					if (GameManager.Get() == null)
					{
						return;
					}
					while (true)
					{
						if (GameManager.Get().TeamInfo == null)
						{
							return;
						}
						while (true)
						{
							if (GameManager.Get().PlayerInfo == null)
							{
								while (true)
								{
									switch (5)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
							if (GameWideData.Get() == null)
							{
								return;
							}
							while (true)
							{
								if (GameWideData.Get().m_gameBalanceVars == null)
								{
									return;
								}
								m_tauntPlayerBanner.m_playerName.text = actorData.GetDisplayName();
								PlayerDetails playerDetails = GameFlow.Get().playerDetails[playerData.GetPlayer()];
								if (playerDetails == null)
								{
									while (true)
									{
										switch (6)
										{
										default:
											return;
										case 0:
											break;
										}
									}
								}
								List<LobbyPlayerInfo> list = new List<LobbyPlayerInfo>();
								list.AddRange(GameManager.Get().TeamInfo.TeamAPlayerInfo);
								list.AddRange(GameManager.Get().TeamInfo.TeamBPlayerInfo);
								for (int i = 0; i < list.Count; i++)
								{
									if (playerDetails.m_lobbyPlayerInfoId != list[i].PlayerId)
									{
										continue;
									}
									while (true)
									{
										UIManager.SetGameObjectActive(m_tauntPlayerBanner.m_playerLevel, false);
										string path = "Banners/Background/02_blue";
										GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(list[i].BannerID);
										if (banner != null)
										{
											path = banner.m_resourceString;
										}
										m_tauntPlayerBanner.m_bannerBG.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
										string path2 = "Banners/Background/02_blue";
										GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(list[i].EmblemID);
										if (banner2 != null)
										{
											path2 = banner2.m_resourceString;
										}
										m_tauntPlayerBanner.m_bannerFG.sprite = (Sprite)Resources.Load(path2, typeof(Sprite));
#if !VANILLA && !SERVER
                                        // Custom titles
                                        m_tauntPlayerBanner.m_playerTitle.text = GameWideData.Get().m_gameBalanceVars.GetTitle(list[i].TitleID, actorData.GetDisplayName(), string.Empty, list[i].TitleLevel);
#else
										m_tauntPlayerBanner.m_playerTitle.text = GameWideData.Get().m_gameBalanceVars.GetTitle(list[i].TitleID, string.Empty, list[i].TitleLevel);
#endif
										GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(list[i].RibbonID);
										if (ribbon != null)
										{
											if (!ribbon.m_resourceString.IsNullOrEmpty())
											{
												m_tauntPlayerBanner.m_bannerRibbon.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
												UIManager.SetGameObjectActive(m_tauntPlayerBanner.m_bannerRibbon, true);
												goto IL_03b2;
											}
										}
										UIManager.SetGameObjectActive(m_tauntPlayerBanner.m_bannerRibbon, false);
										goto IL_03b2;
										IL_03b2:
										if (GameManager.Get().PlayerInfo.PlayerId == list[i].PlayerId)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_selfIndicatorBar;
													m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_selfIndicatorBarGlow;
													return;
												}
											}
										}
										if (GameManager.Get().PlayerInfo.TeamId == list[i].TeamId)
										{
											m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_allyIndicatorBar;
											m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_allyIndicatorBarGlow;
										}
										else
										{
											m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_enemyIndicatorBar;
											m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_enemyIndicatorBarGlow;
										}
										return;
									}
								}
								while (true)
								{
									switch (3)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void SetTauntBannerVisibility(bool isVisible)
	{
		UITauntPlayerBanner tauntPlayerBanner = m_tauntPlayerBanner;
		int doActive;
		if (isVisible)
		{
			doActive = ((!UIScreenManager.Get().GetHideHUDCompletely()) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(tauntPlayerBanner, (byte)doActive != 0);
	}

	public Canvas GetTopLevelCanvas()
	{
		return m_mainCanvas;
	}

	public void GameTeardown()
	{
		m_mainScreenPanel.m_sideNotificationsPanel.RemoveHandleMessage();
		Object.Destroy(base.gameObject);
	}
}
