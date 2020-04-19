using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITutorialInterstitial : UIScene
{
	public GameObject m_welcomeContainer;

	public GameObject m_choiceContainer;

	public GameObject m_tutorialLocationContainer;

	public Image m_welcomeHitbox;

	public _ButtonSwapSprite m_startTutorialHitbox;

	public _ButtonSwapSprite m_getStartedHitbox;

	public _ButtonSwapSprite m_closeHitbox;

	private const float m_welcomeClickDelay = 2f;

	private static UITutorialInterstitial s_instance;

	private float m_welcomeDisplayTime;

	public static UITutorialInterstitial Get()
	{
		if (UITutorialInterstitial.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialInterstitial.Get()).MethodHandle;
			}
			return UITutorialInterstitial.s_instance;
		}
		return null;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.TutorialInterstitial;
	}

	public override void Awake()
	{
		UITutorialInterstitial.s_instance = this;
		UIManager.SetGameObjectActive(this.m_welcomeContainer, false, null);
		UIManager.SetGameObjectActive(this.m_choiceContainer, false, null);
		UIManager.SetGameObjectActive(this.m_tutorialLocationContainer, false, null);
		UIEventTriggerUtils.AddListener(this.m_welcomeHitbox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.WelcomeClicked));
		this.m_startTutorialHitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TutorialClicked);
		this.m_getStartedHitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GetStartedClicked);
		this.m_closeHitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseClicked);
		this.m_startTutorialHitbox.m_ignoreDialogboxes = true;
		this.m_getStartedHitbox.m_ignoreDialogboxes = true;
		this.m_closeHitbox.m_ignoreDialogboxes = true;
		base.Awake();
	}

	public void StartInterstitial()
	{
		this.m_welcomeDisplayTime = Time.realtimeSinceStartup;
		UIManager.SetGameObjectActive(this.m_welcomeContainer, true, null);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialStart");
		ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.Coop;
	}

	public void WelcomeClicked(BaseEventData data)
	{
		if (Time.realtimeSinceStartup > this.m_welcomeDisplayTime + 2f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITutorialInterstitial.WelcomeClicked(BaseEventData)).MethodHandle;
			}
			this.m_welcomeContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
			UIManager.SetGameObjectActive(this.m_choiceContainer, true, null);
			ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialWelcomeClick");
		}
	}

	public void TutorialClicked(BaseEventData data)
	{
		UINewUserFlowManager.MarkShowPlayHighlight(true);
		AppState_GameTypeSelect.Get().Enter(GameType.Tutorial, "Prologue1");
		this.m_choiceContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
		UIFrontendLoadingScreen.Get().SetVisible(true);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialStartTutorialClick");
	}

	public void GetStartedClicked(BaseEventData data)
	{
		this.m_choiceContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
		UIManager.SetGameObjectActive(this.m_tutorialLocationContainer, true, null);
		ClientGameManager.Get().MarkTutorialSkipped(TutorialVersion.CargoShip_Tutorial1, null);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialSkipTutorialClick");
	}

	public void CloseClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_tutorialLocationContainer, false, null);
		UINewUserFlowManager.MarkShowPlayHighlight(true);
		UINewUserFlowManager.HighlightQueued();
		UINewUserFlowManager.OnNavBarDisplayed();
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialEnd");
	}
}
