using System;
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

	public UITextConsole m_textConsole
	{
		get
		{
			return UIChatBox.GetChatBox(UIManager.ClientState.InGame);
		}
	}

	internal static HUD_UI Get()
	{
		return HUD_UI.s_instance;
	}

	private void OnDestroy()
	{
		if (HighlightUtils.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.OnDestroy()).MethodHandle;
			}
			if (HighlightUtils.Get().SprintMouseOverCursor != null)
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
				UnityEngine.Object.Destroy(HighlightUtils.Get().SprintMouseOverCursor);
			}
		}
		HUD_UI.s_instance = null;
	}

	public Canvas m_mainCanvas
	{
		get
		{
			if (this.theCanvas == null)
			{
				this.theCanvas = UIManager.Get().GetBatchCanvas(this, CanvasBatchType.Static);
			}
			return this.theCanvas;
		}
	}

	public Camera m_hudCam
	{
		get
		{
			if (this.HUDCam == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.get_m_hudCam()).MethodHandle;
				}
				this.HUDCam = UIManager.Get().GetCamera(CameraLayerName.MainScreenLayer);
			}
			return this.HUDCam;
		}
	}

	public override void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		HUD_UI.s_instance = this;
		UIManager.SetGameObjectActive(this.m_mainScreenPanel, false, null);
		UIManager.SetGameObjectActive(this.m_mainScreenPanel.m_nameplatePanel, false, null);
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.HUD;
	}

	private void Start()
	{
		if (this.m_tutorialFullscreenPanel != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.Start()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_tutorialFullscreenPanel, true, null);
		}
		UIManager.SetGameObjectActive(this.m_mainScreenPanel, true, null);
		this.SetTauntBannerVisibility(false);
	}

	public void SetMainElementsVisible(bool visible, bool hideChat = false)
	{
		if (!visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.SetMainElementsVisible(bool, bool)).MethodHandle;
			}
			for (int i = 0; i < this.m_mainHUDElementContainers.Length; i++)
			{
				this.m_mainHUDElementContainers[i].alpha = 0f;
				this.m_mainHUDElementContainers[i].blocksRaycasts = false;
				this.m_mainHUDElementContainers[i].interactable = false;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
			if (UISystemEscapeMenu.Get() != null)
			{
				UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 0f;
			}
			UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 0f;
			if (hideChat)
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
				CanvasGroup component = this.m_textConsole.GetComponent<CanvasGroup>();
				component.alpha = 0f;
				component.blocksRaycasts = false;
				component.interactable = false;
			}
		}
		else
		{
			for (int j = 0; j < this.m_mainHUDElementContainers.Length; j++)
			{
				this.m_mainHUDElementContainers[j].alpha = 1f;
				this.m_mainHUDElementContainers[j].blocksRaycasts = true;
				this.m_mainHUDElementContainers[j].interactable = true;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UISystemMenuPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
			if (UISystemEscapeMenu.Get() != null)
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
				UISystemEscapeMenu.Get().GetComponent<CanvasGroup>().alpha = 1f;
			}
			UICharacterMovementPanel.Get().GetComponent<CanvasGroup>().alpha = 1f;
			if (hideChat)
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
				CanvasGroup component2 = this.m_textConsole.GetComponent<CanvasGroup>();
				component2.alpha = 1f;
				component2.blocksRaycasts = true;
				component2.interactable = true;
			}
		}
		UIManager.SetGameObjectActive(UIChatBox.Get().m_overconsPanel, visible, null);
		UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
		this.m_mainHudElementsVisible = visible;
	}

	public bool MainHUDElementsVisible()
	{
		return this.m_mainHudElementsVisible;
	}

	public void SetHUDVisibility(bool visible, bool nameplateVisible)
	{
		UIScreenManager.Get().SetHUDHide(visible, nameplateVisible, false, false);
	}

	public void SetupTauntBanner(ActorData actorData)
	{
		PlayerData playerData = actorData.PlayerData;
		if (playerData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.SetupTauntBanner(ActorData)).MethodHandle;
			}
			return;
		}
		if (!(this.m_tauntPlayerBanner == null))
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
			if (!(this.m_tauntPlayerBanner.m_playerName == null))
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
				if (!(this.m_tauntPlayerBanner.m_playerLevel == null))
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
					if (!(this.m_tauntPlayerBanner.m_playerLevel.gameObject == null) && !(this.m_tauntPlayerBanner.m_playerTitle == null))
					{
						if (!(this.m_tauntPlayerBanner.m_bannerRibbon == null))
						{
							if (!(GameFlow.Get() == null))
							{
								if (GameFlow.Get().playerDetails != null)
								{
									if (!(GameManager.Get() == null))
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
										if (GameManager.Get().TeamInfo != null)
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
											if (GameManager.Get().PlayerInfo != null)
											{
												if (!(GameWideData.Get() == null))
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
													if (GameWideData.Get().m_gameBalanceVars != null)
													{
														this.m_tauntPlayerBanner.m_playerName.text = actorData.\u000E();
														PlayerDetails playerDetails = GameFlow.Get().playerDetails[playerData.GetPlayer()];
														if (playerDetails == null)
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
															return;
														}
														List<LobbyPlayerInfo> list = new List<LobbyPlayerInfo>();
														list.AddRange(GameManager.Get().TeamInfo.TeamAPlayerInfo);
														list.AddRange(GameManager.Get().TeamInfo.TeamBPlayerInfo);
														for (int i = 0; i < list.Count; i++)
														{
															if (playerDetails.m_lobbyPlayerInfoId == list[i].PlayerId)
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
																UIManager.SetGameObjectActive(this.m_tauntPlayerBanner.m_playerLevel, false, null);
																string path = "Banners/Background/02_blue";
																GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(list[i].BannerID);
																if (banner != null)
																{
																	path = banner.m_resourceString;
																}
																this.m_tauntPlayerBanner.m_bannerBG.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
																string path2 = "Banners/Background/02_blue";
																GameBalanceVars.PlayerBanner banner2 = GameWideData.Get().m_gameBalanceVars.GetBanner(list[i].EmblemID);
																if (banner2 != null)
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
																	path2 = banner2.m_resourceString;
																}
																this.m_tauntPlayerBanner.m_bannerFG.sprite = (Sprite)Resources.Load(path2, typeof(Sprite));
																this.m_tauntPlayerBanner.m_playerTitle.text = GameWideData.Get().m_gameBalanceVars.GetTitle(list[i].TitleID, string.Empty, list[i].TitleLevel);
																GameBalanceVars.PlayerRibbon ribbon = GameWideData.Get().m_gameBalanceVars.GetRibbon(list[i].RibbonID);
																if (ribbon != null)
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
																	if (!ribbon.m_resourceString.IsNullOrEmpty())
																	{
																		this.m_tauntPlayerBanner.m_bannerRibbon.sprite = Resources.Load<Sprite>(ribbon.m_resourceString);
																		UIManager.SetGameObjectActive(this.m_tauntPlayerBanner.m_bannerRibbon, true, null);
																		goto IL_3B2;
																	}
																}
																UIManager.SetGameObjectActive(this.m_tauntPlayerBanner.m_bannerRibbon, false, null);
																IL_3B2:
																if (GameManager.Get().PlayerInfo.PlayerId == list[i].PlayerId)
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
																	this.m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_selfIndicatorBar;
																	this.m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_selfIndicatorBarGlow;
																}
																else if (GameManager.Get().PlayerInfo.TeamId == list[i].TeamId)
																{
																	this.m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_allyIndicatorBar;
																	this.m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_allyIndicatorBarGlow;
																}
																else
																{
																	this.m_tauntPlayerBanner.m_teamIndicator.color = HUD_UIResources.Get().m_enemyIndicatorBar;
																	this.m_tauntPlayerBanner.m_teamIndicatorGlow.color = HUD_UIResources.Get().m_enemyIndicatorBarGlow;
																}
																return;
															}
														}
														for (;;)
														{
															switch (3)
															{
															case 0:
																continue;
															}
															return;
														}
													}
												}
												return;
											}
											for (;;)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
										}
									}
									return;
								}
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							return;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
	}

	public void SetTauntBannerVisibility(bool isVisible)
	{
		Component tauntPlayerBanner = this.m_tauntPlayerBanner;
		bool doActive;
		if (isVisible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HUD_UI.SetTauntBannerVisibility(bool)).MethodHandle;
			}
			doActive = !UIScreenManager.Get().GetHideHUDCompletely();
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(tauntPlayerBanner, doActive, null);
	}

	public Canvas GetTopLevelCanvas()
	{
		return this.m_mainCanvas;
	}

	public void GameTeardown()
	{
		this.m_mainScreenPanel.m_sideNotificationsPanel.RemoveHandleMessage();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
