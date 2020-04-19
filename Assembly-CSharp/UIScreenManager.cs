using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
	public float m_chatDisplayTime = 1f;

	public float m_chatRecentChatDisplayTime = 20f;

	private static UIScreenManager s_instance;

	private Regex m_screenshotNameRegex = new Regex("[S,s]creenshot\\d{8}.png");

	private bool m_HideHUD;

	private bool m_HideHUDDebug;

	private bool m_wasInGroup;

	private bool m_SetHandler;

	private float normalXPLoopStartTime;

	private float ggXPLoopStartTime;

	private const float timeOutXPLoopSound = 20f;

	public void EndAllLoopSounds()
	{
		this.EndNormalXPLoop();
		this.EndGGXPLoop();
	}

	public void PlayNormalXPLoop(bool endLoop = false)
	{
		if (this.normalXPLoopStartTime < 0f)
		{
			AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", null);
			this.normalXPLoopStartTime = Time.time;
		}
		if (endLoop)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.PlayNormalXPLoop(bool)).MethodHandle;
			}
			this.EndNormalXPLoop();
		}
	}

	public void EndNormalXPLoop()
	{
		this.normalXPLoopStartTime = -1f;
		if (UISounds.GetUISounds() != null)
		{
			UISounds.GetUISounds().Stop("ui/endgame/points/counter_normal_loop");
		}
	}

	public void PlayGGBoostXPLoop(bool endLoop = false)
	{
		if (this.ggXPLoopStartTime < 0f)
		{
			AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", null);
			this.ggXPLoopStartTime = Time.time;
		}
		if (endLoop)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.PlayGGBoostXPLoop(bool)).MethodHandle;
			}
			this.EndGGXPLoop();
		}
	}

	public void EndGGXPLoop()
	{
		this.ggXPLoopStartTime = -1f;
		if (UISounds.GetUISounds() != null)
		{
			UISounds.GetUISounds().Stop("ui/endgame/points/counter_ggboost_loop");
		}
	}

	private void Awake()
	{
		this.normalXPLoopStartTime = -1f;
		this.ggXPLoopStartTime = -1f;
		this.m_wasInGroup = false;
		UIScreenManager.s_instance = this;
	}

	private void OnDestroy()
	{
		UIScreenManager.s_instance = null;
	}

	public void HandleGroupUpdateNotification()
	{
		bool flag = false;
		if (!this.m_wasInGroup && ClientGameManager.Get().GroupInfo.InAGroup)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.HandleGroupUpdateNotification()).MethodHandle;
			}
			if (AppState_RankModeDraft.Get() != AppState.GetCurrent())
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
				flag = true;
			}
			AppState_GroupCharacterSelect.Get().NotifyJoinedNewGroup();
		}
		else if (this.m_wasInGroup)
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
			if (!ClientGameManager.Get().GroupInfo.InAGroup)
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
				AppState_GroupCharacterSelect.Get().NotifyDroppedGroup();
			}
		}
		this.m_wasInGroup = ClientGameManager.Get().GroupInfo.InAGroup;
		if (UIPlayCategoryMenu.Get() != null)
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
			UIPlayCategoryMenu.Get().UpdateGroupInfo();
		}
		if (UIFrontEnd.Get() != null)
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
			if (UIFrontEnd.Get().m_playerPanel != null)
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
				UIFrontEnd.Get().m_playerPanel.NotifyGroupUpdate(ClientGameManager.Get().GroupInfo.Members);
			}
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
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
				UIFrontEnd.Get().m_frontEndNavPanel.NotifyGroupUpdate();
			}
			if (UICharacterSelectScreenController.Get() != null)
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
				UICharacterSelectScreenController.Get().NotifyGroupUpdate();
				UICharacterSelectScreenController.Get().UpdateReadyButton();
			}
			if (AppState.GetCurrent() != AppState_GroupCharacterSelect.Get())
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
				GameStatus gameStatus;
				if (GameManager.Get() != null)
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
					gameStatus = GameManager.Get().GameStatus;
				}
				else
				{
					gameStatus = GameStatus.None;
				}
				switch (gameStatus)
				{
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 0xA:
				case 0xB:
					break;
				default:
					if (flag)
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
						UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
					}
					break;
				}
			}
		}
	}

	public bool GetHideHUDCompletely()
	{
		return this.m_HideHUDDebug;
	}

	public void SetHUDHide(bool visible, bool nameplateVisible, bool hideNameplateText = false, bool hideChat = false)
	{
		bool flag = !visible;
		if (this.m_HideHUD != flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.SetHUDHide(bool, bool, bool, bool)).MethodHandle;
			}
			this.m_HideHUD = flag;
			if (!this.m_HideHUDDebug)
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
				if (HUD_UI.Get() != null)
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
					HUD_UI.Get().SetMainElementsVisible(!this.m_HideHUD, hideChat);
				}
			}
		}
		Log.Info("HEALTHBARCHECK: HIDE " + this.m_HideHUDDebug, new object[0]);
		if (!this.m_HideHUDDebug)
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
			if (HUD_UI.Get() != null)
			{
				Log.Info("HEALTHBARCHECK: pos " + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform).localPosition, new object[0]);
				Log.Info("HEALTHBARCHECK: size " + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform).sizeDelta, new object[0]);
				CanvasGroup component = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
				if (nameplateVisible)
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
					component.alpha = 1f;
				}
				else
				{
					component.alpha = 0f;
				}
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetTextVisible(!hideNameplateText);
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCombatTextVisible(!hideNameplateText);
			}
		}
	}

	public void SetHUDHideDebug(bool visible, bool nameplateVisible, bool hideNameplateText = false, bool hideChat = false)
	{
		bool hideHUDDebug = !visible;
		this.m_HideHUDDebug = hideHUDDebug;
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().SetMainElementsVisible(!this.m_HideHUDDebug, hideChat);
		}
		if (HUD_UI.Get() != null)
		{
			CanvasGroup component = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
			if (nameplateVisible)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.SetHUDHideDebug(bool, bool, bool, bool)).MethodHandle;
				}
				component.alpha = 1f;
			}
			else
			{
				component.alpha = 0f;
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetTextVisible(!hideNameplateText);
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCombatTextVisible(!hideNameplateText);
		}
		if (UIFrontEnd.Get() != null)
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
			UIFrontEnd uifrontEnd = UIFrontEnd.Get();
			for (int i = 0; i < uifrontEnd.m_frontendCanvasContainers.Length; i++)
			{
				this.SetCanvasGroupForVis(uifrontEnd.m_frontendCanvasContainers[i], visible);
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (UICharacterSelectScreenController.Get() != null)
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
				if (UICharacterSelectScreenController.Get().buttonContainer != null)
				{
					UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().buttonContainer, visible, null);
				}
			}
			if (UICharacterSelectScreen.Get() != null)
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
				UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().transform.parent, visible, null);
			}
			if (UICharacterScreen.Get() != null)
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
				UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
			}
			if (UIChatBox.Get() != null)
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
				UIManager.SetGameObjectActive(UIChatBox.Get(), visible, null);
			}
			if (UICharacterSelectWorldObjects.Get() != null)
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
				if (UICharacterSelectWorldObjects.Get().m_objectsToHideForToggleUI != null)
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
					using (List<GameObject>.Enumerator enumerator = UICharacterSelectWorldObjects.Get().m_objectsToHideForToggleUI.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject gameObject = enumerator.Current;
							if (gameObject != null)
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
								UIManager.SetGameObjectActive(gameObject, visible, null);
							}
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				UICharacterSelectRing[] ringAnimations = UICharacterSelectWorldObjects.Get().m_ringAnimations;
				if (ringAnimations != null)
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
					for (int j = 0; j < ringAnimations.Length; j++)
					{
						if (ringAnimations[j] != null)
						{
							ringAnimations[j].PlayAnimation("ReadyOut");
						}
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
				}
			}
			if (NavigationBar.Get() != null)
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
				UIManager.SetGameObjectActive(NavigationBar.Get(), visible, null);
			}
		}
	}

	private void SetCanvasGroupForVis(CanvasGroup canvasGroup, bool visible)
	{
		if (canvasGroup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.SetCanvasGroupForVis(CanvasGroup, bool)).MethodHandle;
			}
			float alpha;
			if (visible)
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
				alpha = 1f;
			}
			else
			{
				alpha = 0f;
			}
			canvasGroup.alpha = alpha;
			canvasGroup.blocksRaycasts = visible;
			canvasGroup.interactable = visible;
		}
	}

	public static UIScreenManager Get()
	{
		return UIScreenManager.s_instance;
	}

	public void ClearAllPanels()
	{
		if (UIGameOverScreen.Get() != null)
		{
			UIGameOverScreen.Get().SetVisible(false);
		}
		if (UIActorDebugPanel.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.ClearAllPanels()).MethodHandle;
			}
			UIManager.SetGameObjectActive(UIActorDebugPanel.Get(), false, null);
			UIActorDebugPanel.Get().Reset();
		}
		if (UIDebugMenu.Get())
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
			UIManager.SetGameObjectActive(UIDebugMenu.Get().m_container, false, null);
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
	}

	private void UpdateXPLoopTime()
	{
		if (this.normalXPLoopStartTime > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.UpdateXPLoopTime()).MethodHandle;
			}
			if (Time.time - this.normalXPLoopStartTime >= 20f)
			{
				this.normalXPLoopStartTime = -1f;
				this.EndNormalXPLoop();
			}
		}
		if (this.ggXPLoopStartTime > 0f && Time.time - this.ggXPLoopStartTime >= 20f)
		{
			this.ggXPLoopStartTime = -1f;
			this.EndGGXPLoop();
		}
	}

	private void UpdateEscapeKeyHit()
	{
		if (UIFrontEnd.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.UpdateEscapeKeyHit()).MethodHandle;
			}
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
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
				if (UIFrontEnd.Get().m_frontEndNavPanel.gameObject.activeInHierarchy)
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
					if (Input.GetKeyDown(KeyCode.Escape))
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
						if (UIFrontEnd.Get().CanMenuEscape())
						{
							if (Options_UI.Get().IsVisible())
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
								Options_UI.Get().ToggleOptions();
							}
							else if (KeyBinding_UI.Get().IsVisible())
							{
								if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
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
									KeyBinding_UI.Get().ToggleKeybinds();
								}
							}
							else if (QuestListPanel.Get().IsVisible())
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
								QuestListPanel.Get().SetVisible(false, false, false);
							}
							else if (UILandingPageFullScreenMenus.Get().IsActive())
							{
								UILandingPageFullScreenMenus.Get().CloseMenu();
							}
							else if (AppState.GetCurrent() != AppState_FullScreenMovie.Get())
							{
								UIFrontEnd.Get().m_frontEndNavPanel.MenuBtnClicked(null);
							}
						}
					}
				}
			}
		}
	}

	private void UpdateToggleHUDKey()
	{
		if (InputManager.Get() != null)
		{
			if (GameFlowData.Get() != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.UpdateToggleHUDKey()).MethodHandle;
				}
				GameState gameState = GameFlowData.Get().gameState;
				if (gameState != GameState.BothTeams_Decision)
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
					if (gameState != GameState.BothTeams_Resolve)
					{
						goto IL_129;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (HUD_UI.Get() != null)
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
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUD))
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
						bool flag = !this.m_HideHUDDebug;
						this.SetHUDHideDebug(!flag, !flag, false, true);
						LineData.SetAllowMovementLinesVisibleForHud(!flag);
					}
				}
				if (HUD_UI.Get() != null)
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
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUDExceptNameplates))
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
						bool flag2 = !this.m_HideHUDDebug;
						this.SetHUDHideDebug(!flag2, true, flag2, false);
						GameFlowData.Get().activeOwnedActorData.\u000E().SetMovementDistanceLinesVisible(!flag2);
					}
				}
				IL_129:;
			}
			else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUD))
			{
				bool flag3 = !this.m_HideHUDDebug;
				this.SetHUDHideDebug(!flag3, !flag3, false, false);
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.TakeScreenShot))
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
				if (!Directory.Exists("Screenshots"))
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
					Directory.CreateDirectory("Screenshots");
				}
				DirectoryInfo directoryInfo = new DirectoryInfo("Screenshots");
				FileInfo[] files = directoryInfo.GetFiles();
				int num = 0;
				for (int i = 0; i < files.Length; i++)
				{
					Match match = this.m_screenshotNameRegex.Match(files[i].Name);
					if (match != null)
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
						Regex regex = new Regex("\\d{8}");
						Match match2 = regex.Match(match.Value);
						int num2 = int.Parse(match2.Value);
						if (num <= num2)
						{
							num = num2 + 1;
						}
					}
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
				int superSize = 1;
				if (!Input.GetKey(KeyCode.LeftControl))
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
					if (!Input.GetKey(KeyCode.RightControl))
					{
						goto IL_26F;
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
				superSize = 4;
				IL_26F:
				Application.CaptureScreenshot(string.Format(Application.dataPath + "/../ScreenShots/Screenshot{0}.png", this.FormatNumberForScreenshotIndex(num)), superSize);
			}
		}
	}

	private void UpdateCheckSetHandler()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.UpdateCheckSetHandler()).MethodHandle;
			}
			if (!this.m_SetHandler)
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
				this.m_SetHandler = true;
				ClientGameManager.Get().OnGroupUpdateNotification += this.HandleGroupUpdateNotification;
			}
		}
	}

	private void Update()
	{
		this.UpdateXPLoopTime();
		this.UpdateEscapeKeyHit();
		this.UpdateToggleHUDKey();
		this.UpdateCheckSetHandler();
	}

	public string FormatNumberForScreenshotIndex(int index)
	{
		string text = index.ToString();
		while (text.Length < 8)
		{
			text = "0" + text;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.FormatNumberForScreenshotIndex(int)).MethodHandle;
		}
		return text;
	}

	public void TryLoadAndSetupInGameUI()
	{
		if (ClientGameManager.Get().InGameUIActivated)
		{
			Log.Info("OnHUD_UILoaded called, UI already activated.", new object[0]);
			return;
		}
		Log.Info("OnHUD_UILoaded called.", new object[0]);
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIScreenManager.TryLoadAndSetupInGameUI()).MethodHandle;
			}
			Log.Info("HEALTHBARCHECK: Entrance success", new object[0]);
			if (HUD_UI.Get().m_textConsole != null)
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
				UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true, null);
			}
			if (HUD_UI.Get().m_mainScreenPanel != null)
			{
				HUD_UI.Get().m_mainScreenPanel.NotifyStartGame();
				HUD_UI.Get().m_mainScreenPanel.SetVisible(true);
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			}
			ClientGameManager.Get().InGameUIActivated = true;
		}
	}
}
