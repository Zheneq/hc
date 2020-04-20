using System;
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
		return UIMainMenu.m_instance;
	}

	private void Awake()
	{
		UIMainMenu.m_instance = this;
		this.SetMenuVisible(false, true);
		this.m_optionsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OptionsClicked);
		this.m_keyBindingBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.KeyBindingClicked);
		this.m_creditsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CreditsClicked);
		this.m_tutorialBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TutorialClicked);
		this.m_exitGameBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ExitGameClicked);
		this.m_trailerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TrailerClicked);
		this.m_patchNotesBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PatchNotesClicked);
		this.m_feedbackBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FeedbackClicked);
		this.m_optionsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_keyBindingBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_creditsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_tutorialBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_exitGameBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_patchNotesBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_feedbackBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_menuListCanvasGroup = this.m_topRightMenuController.GetComponent<CanvasGroup>();
	}

	public void SetMenuVisible(bool visible, bool ignoreSoundCall = false)
	{
		this.m_menuOpen = visible;
		if (this.m_menuOpen)
		{
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			if (!ignoreSoundCall)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuOpen);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
			if (!ignoreSoundCall)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MainMenuClose);
			}
		}
		if (this.m_menuListCanvasGroup != null)
		{
			this.m_menuListCanvasGroup.interactable = visible;
			this.m_menuListCanvasGroup.blocksRaycasts = visible;
		}
		if (UIFrontEnd.Get() != null && UIFrontEnd.Get().m_frontEndNavPanel != null)
		{
			UIFrontEnd.Get().m_frontEndNavPanel.m_menuBtn.SetSelected(visible, false, string.Empty, string.Empty);
		}
	}

	public void HowToVideoClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().DisplayVideo("Video/HowTo", "How To Play");
	}

	public void PatchNotesClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().DisplayPatchNotes();
	}

	public void FeedbackClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		UILandingPageFullScreenMenus.Get().ToggleFeedbackContainerVisible();
	}

	public void OptionsClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		Options_UI.Get().ToggleOptions();
	}

	public void KeyBindingClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		KeyBinding_UI.Get().ToggleKeybinds();
	}

	public void TrailerClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, true);
		Queue<string> queue = new Queue<string>();
		queue.Enqueue("Video/AR_CG");
		AppState_FullScreenMovie.Get().Enter(queue, AppState_FullScreenMovie.AppStates.None);
	}

	public void CreditsClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, false);
		UIFrontEnd.Get().OnCreditsClick(data);
	}

	public void TutorialClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, false);
		if (ClientGameManager.Get().IsServerLocked)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ServerIsLocked", "Global"), StringUtil.TR("CannotStartTutorial", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
		else
		{
			AppState_LandingPage.Get().OnTutorial1Clicked();
		}
	}

	public void ExitGameClicked(BaseEventData data)
	{
		this.SetMenuVisible(false, false);
		UIFrontEnd.Get().OnExitGameClick(data);
	}

	public bool IsOpen()
	{
		return this.m_menuOpen;
	}

	public void CheckCanDoTutorial()
	{
		if (ClientGameManager.Get() != null && ClientGameManager.Get().GroupInfo != null && AppState_CharacterSelect.Get() != null && AppState_GroupCharacterSelect.Get() != null)
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
			this.m_tutorialBtn.spriteController.SetClickable(flag);
			TextMeshProUGUI componentInChildren = this.m_tutorialBtn.GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren != null)
			{
				if (flag)
				{
					componentInChildren.color = Color.white;
				}
				else
				{
					componentInChildren.color = Color.gray;
				}
			}
		}
	}

	private void Update()
	{
		this.CheckCanDoTutorial();
		if (Input.GetMouseButtonDown(0))
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
								while (componentInParent2 != null)
								{
									_SelectableBtn menuBtn = UIFrontEnd.Get().m_frontEndNavPanel.m_menuBtn;
									if (componentInParent2 == menuBtn)
									{
										flag2 = true;
										goto IL_16A;
									}
									componentInParent2 = componentInParent2.transform.parent.GetComponentInParent<_SelectableBtn>();
								}
							}
						}
						IL_16A:
						if (componentInParent != null || flag2)
						{
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				this.SetMenuVisible(false, false);
			}
		}
	}
}
