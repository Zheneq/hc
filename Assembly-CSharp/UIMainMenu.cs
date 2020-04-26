using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMainMenu : MonoBehaviour
{
	public Animator m_topRightMenuController;

	public _SelectableBtn m_optionsBtn;

	public _SelectableBtn m_creditsBtn;

	public _SelectableBtn m_tutorialBtn;

	public _SelectableBtn m_trailerBtn;

	public _SelectableBtn m_exitGameBtn;

	public _SelectableBtn m_patchNotesBtn;

	public _SelectableBtn m_feedbackBtn;

	public _SelectableBtn m_keyBindingBtn;

	private bool m_menuOpen;

	private CanvasGroup m_menuListCanvasGroup;

	private static UIMainMenu m_instance;

	public static UIMainMenu Get()
	{
		return m_instance;
	}

	private void Awake()
	{
		m_instance = this;
		SetMenuVisible(false, true);
		m_optionsBtn.spriteController.callback = OptionsClicked;
		m_keyBindingBtn.spriteController.callback = KeyBindingClicked;
		m_creditsBtn.spriteController.callback = CreditsClicked;
		m_tutorialBtn.spriteController.callback = TutorialClicked;
		m_exitGameBtn.spriteController.callback = ExitGameClicked;
		m_trailerBtn.spriteController.callback = TrailerClicked;
		m_patchNotesBtn.spriteController.callback = PatchNotesClicked;
		m_feedbackBtn.spriteController.callback = FeedbackClicked;
		m_optionsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_keyBindingBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_creditsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_tutorialBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_exitGameBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_patchNotesBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_feedbackBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_menuListCanvasGroup = m_topRightMenuController.GetComponent<CanvasGroup>();
	}

	public void SetMenuVisible(bool visible, bool ignoreSoundCall = false)
	{
		m_menuOpen = visible;
		if (m_menuOpen)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
			if (!ignoreSoundCall)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
			if (!ignoreSoundCall)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
			}
		}
		if (m_menuListCanvasGroup != null)
		{
			m_menuListCanvasGroup.interactable = visible;
			m_menuListCanvasGroup.blocksRaycasts = visible;
		}
		if (!(UIFrontEnd.Get() != null) || !(UIFrontEnd.Get().m_frontEndNavPanel != null))
		{
			return;
		}
		while (true)
		{
			UIFrontEnd.Get().m_frontEndNavPanel.m_menuBtn.SetSelected(visible, false, string.Empty, string.Empty);
			return;
		}
	}

	public void HowToVideoClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().DisplayVideo("Video/HowTo", "How To Play");
	}

	public void PatchNotesClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().DisplayPatchNotes();
	}

	public void FeedbackClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().ToggleFeedbackContainerVisible();
	}

	public void OptionsClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		Options_UI.Get().ToggleOptions();
	}

	public void KeyBindingClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		KeyBinding_UI.Get().ToggleKeybinds();
	}

	public void TrailerClicked(BaseEventData data)
	{
		SetMenuVisible(false, true);
		Queue<string> queue = new Queue<string>();
		queue.Enqueue("Video/AR_CG");
		AppState_FullScreenMovie.Get().Enter(queue, AppState_FullScreenMovie.AppStates.None);
	}

	public void CreditsClicked(BaseEventData data)
	{
		SetMenuVisible(false);
		UIFrontEnd.Get().OnCreditsClick(data);
	}

	public void TutorialClicked(BaseEventData data)
	{
		SetMenuVisible(false);
		if (ClientGameManager.Get().IsServerLocked)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ServerIsLocked", "Global"), StringUtil.TR("CannotStartTutorial", "Global"), StringUtil.TR("Ok", "Global"));
		}
		else
		{
			AppState_LandingPage.Get().OnTutorial1Clicked();
		}
	}

	public void ExitGameClicked(BaseEventData data)
	{
		SetMenuVisible(false);
		UIFrontEnd.Get().OnExitGameClick(data);
	}

	public bool IsOpen()
	{
		return m_menuOpen;
	}

	public void CheckCanDoTutorial()
	{
		if (!(ClientGameManager.Get() != null) || ClientGameManager.Get().GroupInfo == null || !(AppState_CharacterSelect.Get() != null) || !(AppState_GroupCharacterSelect.Get() != null))
		{
			return;
		}
		while (true)
		{
			bool flag = true;
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				flag = false;
			}
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
			{
				flag = false;
			}
			if (AppState_GroupCharacterSelect.Get().InQueue())
			{
				flag = false;
			}
			m_tutorialBtn.spriteController.SetClickable(flag);
			TextMeshProUGUI componentInChildren = m_tutorialBtn.GetComponentInChildren<TextMeshProUGUI>();
			if (!(componentInChildren != null))
			{
				return;
			}
			while (true)
			{
				if (flag)
				{
					componentInChildren.color = Color.white;
				}
				else
				{
					componentInChildren.color = Color.gray;
				}
				return;
			}
		}
	}

	private void Update()
	{
		CheckCanDoTutorial();
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		while (true)
		{
			bool flag = true;
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
			{
				StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
				if (component != null)
				{
					if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
					{
						UIMainMenu componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<UIMainMenu>();
						bool flag2 = false;
						if (componentInParent == null)
						{
							_SelectableBtn componentInParent2 = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<_SelectableBtn>();
							if (UIFrontEnd.Get() != null)
							{
								while (true)
								{
									if (componentInParent2 != null)
									{
										_SelectableBtn menuBtn = UIFrontEnd.Get().m_frontEndNavPanel.m_menuBtn;
										if (componentInParent2 == menuBtn)
										{
											flag2 = true;
											break;
										}
										componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
										continue;
									}
									break;
								}
							}
						}
						if (componentInParent != null || flag2)
						{
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				while (true)
				{
					SetMenuVisible(false);
					return;
				}
			}
			return;
		}
	}
}
