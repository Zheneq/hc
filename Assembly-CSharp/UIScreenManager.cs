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
		EndNormalXPLoop();
		EndGGXPLoop();
	}

	public void PlayNormalXPLoop(bool endLoop = false)
	{
		if (normalXPLoopStartTime < 0f)
		{
			AudioManager.PostEvent("ui/endgame/points/counter_normal_loop");
			normalXPLoopStartTime = Time.time;
		}
		if (!endLoop)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			EndNormalXPLoop();
			return;
		}
	}

	public void EndNormalXPLoop()
	{
		normalXPLoopStartTime = -1f;
		if (UISounds.GetUISounds() != null)
		{
			UISounds.GetUISounds().Stop("ui/endgame/points/counter_normal_loop");
		}
	}

	public void PlayGGBoostXPLoop(bool endLoop = false)
	{
		if (ggXPLoopStartTime < 0f)
		{
			AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop");
			ggXPLoopStartTime = Time.time;
		}
		if (!endLoop)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			EndGGXPLoop();
			return;
		}
	}

	public void EndGGXPLoop()
	{
		ggXPLoopStartTime = -1f;
		if (UISounds.GetUISounds() != null)
		{
			UISounds.GetUISounds().Stop("ui/endgame/points/counter_ggboost_loop");
		}
	}

	private void Awake()
	{
		normalXPLoopStartTime = -1f;
		ggXPLoopStartTime = -1f;
		m_wasInGroup = false;
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void HandleGroupUpdateNotification()
	{
		bool flag = false;
		if (!m_wasInGroup && ClientGameManager.Get().GroupInfo.InAGroup)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (AppState_RankModeDraft.Get() != AppState.GetCurrent())
			{
				while (true)
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
		else if (m_wasInGroup)
		{
			while (true)
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
				while (true)
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
		m_wasInGroup = ClientGameManager.Get().GroupInfo.InAGroup;
		if (UIPlayCategoryMenu.Get() != null)
		{
			while (true)
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
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (UIFrontEnd.Get().m_playerPanel != null)
			{
				while (true)
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
				while (true)
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
				while (true)
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
			if (!(AppState.GetCurrent() != AppState_GroupCharacterSelect.Get()))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				int num;
				if (GameManager.Get() != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (int)GameManager.Get().GameStatus;
				}
				else
				{
					num = 0;
				}
				switch (num)
				{
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					return;
				}
				if (!flag)
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
					return;
				}
			}
		}
	}

	public bool GetHideHUDCompletely()
	{
		return m_HideHUDDebug;
	}

	public void SetHUDHide(bool visible, bool nameplateVisible, bool hideNameplateText = false, bool hideChat = false)
	{
		bool flag = !visible;
		if (m_HideHUD != flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_HideHUD = flag;
			if (!m_HideHUDDebug)
			{
				while (true)
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					HUD_UI.Get().SetMainElementsVisible(!m_HideHUD, hideChat);
				}
			}
		}
		Log.Info("HEALTHBARCHECK: HIDE " + m_HideHUDDebug);
		if (m_HideHUDDebug)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			Log.Info("HEALTHBARCHECK: pos " + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform).localPosition);
			Log.Info("HEALTHBARCHECK: size " + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform).sizeDelta);
			CanvasGroup component = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
			if (nameplateVisible)
			{
				while (true)
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
			return;
		}
	}

	public void SetHUDHideDebug(bool visible, bool nameplateVisible, bool hideNameplateText = false, bool hideChat = false)
	{
		bool flag = m_HideHUDDebug = !visible;
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().SetMainElementsVisible(!m_HideHUDDebug, hideChat);
		}
		if (HUD_UI.Get() != null)
		{
			CanvasGroup component = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
			if (nameplateVisible)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
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
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			UIFrontEnd uIFrontEnd = UIFrontEnd.Get();
			for (int i = 0; i < uIFrontEnd.m_frontendCanvasContainers.Length; i++)
			{
				SetCanvasGroupForVis(uIFrontEnd.m_frontendCanvasContainers[i], visible);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (UICharacterSelectScreenController.Get() != null)
				{
					while (true)
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
						UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().buttonContainer, visible);
					}
				}
				if (UICharacterSelectScreen.Get() != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().transform.parent, visible);
				}
				if (UICharacterScreen.Get() != null)
				{
					while (true)
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
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(UIChatBox.Get(), visible);
				}
				if (UICharacterSelectWorldObjects.Get() != null)
				{
					while (true)
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
						while (true)
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
								GameObject current = enumerator.Current;
								if (current != null)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									UIManager.SetGameObjectActive(current, visible);
								}
							}
							while (true)
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
						while (true)
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
						while (true)
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
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						UIManager.SetGameObjectActive(NavigationBar.Get(), visible);
						return;
					}
				}
				return;
			}
		}
	}

	private void SetCanvasGroupForVis(CanvasGroup canvasGroup, bool visible)
	{
		if (!(canvasGroup != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float alpha;
			if (visible)
			{
				while (true)
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
			return;
		}
	}

	public static UIScreenManager Get()
	{
		return s_instance;
	}

	public void ClearAllPanels()
	{
		if (UIGameOverScreen.Get() != null)
		{
			UIGameOverScreen.Get().SetVisible(false);
		}
		if (UIActorDebugPanel.Get() != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(UIActorDebugPanel.Get(), false);
			UIActorDebugPanel.Get().Reset();
		}
		if ((bool)UIDebugMenu.Get())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(UIDebugMenu.Get().m_container, false);
		}
		if (UILoadingScreenPanel.Get() != null)
		{
			UILoadingScreenPanel.Get().SetVisible(false);
		}
	}

	private void UpdateXPLoopTime()
	{
		if (normalXPLoopStartTime > 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Time.time - normalXPLoopStartTime >= 20f)
			{
				normalXPLoopStartTime = -1f;
				EndNormalXPLoop();
			}
		}
		if (ggXPLoopStartTime > 0f && Time.time - ggXPLoopStartTime >= 20f)
		{
			ggXPLoopStartTime = -1f;
			EndGGXPLoop();
		}
	}

	private void UpdateEscapeKeyHit()
	{
		if (!(UIFrontEnd.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(UIFrontEnd.Get().m_frontEndNavPanel != null))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (!UIFrontEnd.Get().m_frontEndNavPanel.gameObject.activeInHierarchy)
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (!Input.GetKeyDown(KeyCode.Escape))
					{
						return;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						if (!UIFrontEnd.Get().CanMenuEscape())
						{
							return;
						}
						if (Options_UI.Get().IsVisible())
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									Options_UI.Get().ToggleOptions();
									return;
								}
							}
						}
						if (KeyBinding_UI.Get().IsVisible())
						{
							if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									KeyBinding_UI.Get().ToggleKeybinds();
									return;
								}
							}
							return;
						}
						if (QuestListPanel.Get().IsVisible())
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									QuestListPanel.Get().SetVisible(false);
									return;
								}
							}
						}
						if (UILandingPageFullScreenMenus.Get().IsActive())
						{
							UILandingPageFullScreenMenus.Get().CloseMenu();
						}
						else if (AppState.GetCurrent() != AppState_FullScreenMovie.Get())
						{
							UIFrontEnd.Get().m_frontEndNavPanel.MenuBtnClicked(null);
						}
						return;
					}
				}
			}
		}
	}

	private void UpdateToggleHUDKey()
	{
		if (!(InputManager.Get() != null))
		{
			return;
		}
		if (GameFlowData.Get() != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameState gameState = GameFlowData.Get().gameState;
			if (gameState != GameState.BothTeams_Decision)
			{
				while (true)
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
					goto IL_015a;
				}
				while (true)
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
				while (true)
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
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag = !m_HideHUDDebug;
					SetHUDHideDebug(!flag, !flag, false, true);
					LineData.SetAllowMovementLinesVisibleForHud(!flag);
				}
			}
			if (HUD_UI.Get() != null)
			{
				while (true)
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag2 = !m_HideHUDDebug;
					SetHUDHideDebug(!flag2, true, flag2);
					GameFlowData.Get().activeOwnedActorData.GetActorController().SetMovementDistanceLinesVisible(!flag2);
				}
			}
		}
		else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUD))
		{
			bool flag3 = !m_HideHUDDebug;
			SetHUDHideDebug(!flag3, !flag3);
		}
		goto IL_015a;
		IL_015a:
		if (!InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.TakeScreenShot))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (!Directory.Exists("Screenshots"))
			{
				while (true)
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
				Match match = m_screenshotNameRegex.Match(files[i].Name);
				if (match != null)
				{
					while (true)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				int superSize = 1;
				if (!Input.GetKey(KeyCode.LeftControl))
				{
					while (true)
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
						goto IL_026f;
					}
					while (true)
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
				goto IL_026f;
				IL_026f:
				Application.CaptureScreenshot(string.Format(Application.dataPath + "/../ScreenShots/Screenshot{0}.png", FormatNumberForScreenshotIndex(num)), superSize);
				return;
			}
		}
	}

	private void UpdateCheckSetHandler()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_SetHandler)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					m_SetHandler = true;
					ClientGameManager.Get().OnGroupUpdateNotification += HandleGroupUpdateNotification;
					return;
				}
			}
			return;
		}
	}

	private void Update()
	{
		UpdateXPLoopTime();
		UpdateEscapeKeyHit();
		UpdateToggleHUDKey();
		UpdateCheckSetHandler();
	}

	public string FormatNumberForScreenshotIndex(int index)
	{
		string text = index.ToString();
		while (text.Length < 8)
		{
			text = "0" + text;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return text;
		}
	}

	public void TryLoadAndSetupInGameUI()
	{
		if (ClientGameManager.Get().InGameUIActivated)
		{
			Log.Info("OnHUD_UILoaded called, UI already activated.");
			return;
		}
		Log.Info("OnHUD_UILoaded called.");
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Log.Info("HEALTHBARCHECK: Entrance success");
			if (HUD_UI.Get().m_textConsole != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
			}
			if (HUD_UI.Get().m_mainScreenPanel != null)
			{
				HUD_UI.Get().m_mainScreenPanel.NotifyStartGame();
				HUD_UI.Get().m_mainScreenPanel.SetVisible(true);
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
			}
			ClientGameManager.Get().InGameUIActivated = true;
			return;
		}
	}
}
