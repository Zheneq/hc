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
		if (s_instance != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return s_instance;
				}
			}
		}
		return null;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.TutorialInterstitial;
	}

	public override void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_welcomeContainer, false);
		UIManager.SetGameObjectActive(m_choiceContainer, false);
		UIManager.SetGameObjectActive(m_tutorialLocationContainer, false);
		UIEventTriggerUtils.AddListener(m_welcomeHitbox.gameObject, EventTriggerType.PointerClick, WelcomeClicked);
		m_startTutorialHitbox.callback = TutorialClicked;
		m_getStartedHitbox.callback = GetStartedClicked;
		m_closeHitbox.callback = CloseClicked;
		m_startTutorialHitbox.m_ignoreDialogboxes = true;
		m_getStartedHitbox.m_ignoreDialogboxes = true;
		m_closeHitbox.m_ignoreDialogboxes = true;
		base.Awake();
	}

	public void StartInterstitial()
	{
		m_welcomeDisplayTime = Time.realtimeSinceStartup;
		UIManager.SetGameObjectActive(m_welcomeContainer, true);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialStart");
		ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.Coop;
	}

	public void WelcomeClicked(BaseEventData data)
	{
		if (!(Time.realtimeSinceStartup > m_welcomeDisplayTime + 2f))
		{
			return;
		}
		while (true)
		{
			m_welcomeContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
			UIManager.SetGameObjectActive(m_choiceContainer, true);
			ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialWelcomeClick");
			return;
		}
	}

	public void TutorialClicked(BaseEventData data)
	{
		UINewUserFlowManager.MarkShowPlayHighlight(true);
		AppState_GameTypeSelect.Get().Enter(GameType.Tutorial, "Prologue1");
		m_choiceContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
		UIFrontendLoadingScreen.Get().SetVisible(true);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialStartTutorialClick");
	}

	public void GetStartedClicked(BaseEventData data)
	{
		m_choiceContainer.GetComponent<Animator>().Play("TutorialFadeDefaultOUT");
		UIManager.SetGameObjectActive(m_tutorialLocationContainer, true);
		ClientGameManager.Get().MarkTutorialSkipped(TutorialVersion.CargoShip_Tutorial1);
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialSkipTutorialClick");
	}

	public void CloseClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_tutorialLocationContainer, false);
		UINewUserFlowManager.MarkShowPlayHighlight(true);
		UINewUserFlowManager.HighlightQueued();
		UINewUserFlowManager.OnNavBarDisplayed();
		ClientGameManager.Get().SendUIActionNotification("TutorialInterstitialEnd");
	}
}
