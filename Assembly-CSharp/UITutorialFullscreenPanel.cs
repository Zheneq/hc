using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITutorialFullscreenPanel : MonoBehaviour
{
	public GameObject m_AnimatorObject;

	public GameObject m_background;

	public GameObject m_combatPhasePanel;

	public GameObject m_dashPhasePanel;

	public GameObject m_prepPhasePanel;

	public GameObject m_statusEffectPanel;

	public GameObject m_teammateTargetingPanel;

	public GameObject m_energyAndUltimatesPanel;

	public GameObject m_continuePanel;

	public GameObject m_fadeOutPanel;

	public Button m_continueButton;

	private Animator m_FadeoutAnimator;

	private static UITutorialFullscreenPanel s_instance;

	public static UITutorialFullscreenPanel Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_background, false);
		UIManager.SetGameObjectActive(m_combatPhasePanel, false);
		UIManager.SetGameObjectActive(m_dashPhasePanel, false);
		UIManager.SetGameObjectActive(m_prepPhasePanel, false);
		UIManager.SetGameObjectActive(m_statusEffectPanel, false);
		UIManager.SetGameObjectActive(m_teammateTargetingPanel, false);
		UIManager.SetGameObjectActive(m_energyAndUltimatesPanel, false);
		UIManager.SetGameObjectActive(m_continuePanel, false);
		m_FadeoutAnimator = m_fadeOutPanel.GetComponent<Animator>();
	}

	private void Start()
	{
		if (m_continueButton != null)
		{
			UIEventTriggerUtils.AddListener(m_continueButton.gameObject, EventTriggerType.PointerClick, OnContinueButton);
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Update()
	{
		if (!m_background.activeSelf)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = m_AnimatorObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		if (!currentAnimatorStateInfo.IsName("TutorialCombatPhasePanelDONE") && !currentAnimatorStateInfo.IsName("TutorialPrepPhasePanelDONE"))
		{
			if (!currentAnimatorStateInfo.IsName("TutorialDashPhasePanelDONE"))
			{
				if (!currentAnimatorStateInfo.IsName("TutorialStatusEffectPanelDONE") && !currentAnimatorStateInfo.IsName("TutorialTeammateTargetingPanelDONE"))
				{
					if (!currentAnimatorStateInfo.IsName("TutorialEnergyAndUltimatesPanelDONE"))
					{
						return;
					}
				}
			}
		}
		ClearAllPanels();
	}

	private void SetupMoviePlayer(PlayRawImageMovieTexture moviePlayer, string movieName)
	{
		if (moviePlayer == null)
		{
			return;
		}
		while (true)
		{
			if (movieName.IsNullOrEmpty())
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < HUD_UIResources.Get().m_prologueVideoList.Length; i++)
			{
				if (HUD_UIResources.Get().m_prologueVideoList[i].Name == movieName)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						Log.Error("Unable to find prologue video");
						return;
					}
				}
			}
			if (HUD_UIResources.Get().m_prologueVideoList.Length <= num)
			{
				return;
			}
			while (true)
			{
				HUD_UIResources.TutorialVideoInfo videoInfo = HUD_UIResources.Get().m_prologueVideoList[num];
				string localizedVideoPath = HUD_UIResources.Get().GetLocalizedVideoPath(videoInfo, LocalizationManager.CurrentLanguageCode);
				if (localizedVideoPath.IsNullOrEmpty())
				{
					localizedVideoPath = HUD_UIResources.Get().GetLocalizedVideoPath(videoInfo, "en");
				}
				if (!localizedVideoPath.IsNullOrEmpty())
				{
					while (true)
					{
						moviePlayer.Play(localizedVideoPath, true, true);
						return;
					}
				}
				return;
			}
		}
	}

	public void ShowCombatPhasePanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_combatPhasePanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_AnimatorObject.GetComponent<Animator>().Play("TutorialCombatPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = m_combatPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		SetupMoviePlayer(componentInChildren, "Combat");
	}

	public void ShowDashPhasePanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_dashPhasePanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_AnimatorObject.GetComponent<Animator>().Play("TutorialDashPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = m_dashPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		SetupMoviePlayer(componentInChildren, "Dash");
	}

	public void ShowPrepPhasePanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_prepPhasePanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_AnimatorObject.GetComponent<Animator>().Play("TutorialPrepPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = m_prepPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		SetupMoviePlayer(componentInChildren, "Prep");
	}

	public void ShowStatusEffectPanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_statusEffectPanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_background.GetComponent<Animator>().Play("TutorialStatusEffectPanelIN", 0);
	}

	public void ShowTeammateTargetingPanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_teammateTargetingPanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_AnimatorObject.GetComponent<Animator>().Play("TutorialTeammateTargetingPanelIN", 0);
	}

	public void ShowEnergyAndUltimatesPanel()
	{
		UIManager.SetGameObjectActive(m_background, true);
		UIManager.SetGameObjectActive(m_energyAndUltimatesPanel, true);
		UIManager.SetGameObjectActive(m_continuePanel, true);
		m_AnimatorObject.GetComponent<Animator>().Play("TutorialEnergyAndUltimatesPanelIN", 0);
	}

	public void FadeOut()
	{
		UIAnimationEventManager.Get().PlayAnimation(m_FadeoutAnimator, "SlowFadeDefaultIN", FadeOutAnimDone, "FadeDefaultIDLE");
	}

	public void FadeIn()
	{
		UIAnimationEventManager.Get().PlayAnimation(m_FadeoutAnimator, "SlowFadeDefaultOUT", FadeInAnimDone, string.Empty);
	}

	public void FadeInAnimDone()
	{
		UIManager.SetGameObjectActive(m_fadeOutPanel, false);
	}

	public void FadeOutAnimDone()
	{
	}

	public void OnContinueButton(BaseEventData data)
	{
		if (m_prepPhasePanel.activeSelf)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialPrepPhasePanelOUT", 0);
		}
		if (m_dashPhasePanel.activeSelf)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialDashPhasePanelOUT", 0);
		}
		if (m_combatPhasePanel.activeSelf)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialCombatPhasePanelOUT", 0);
		}
		if (m_statusEffectPanel.activeSelf)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialStatusEffectPanelOUT", 0);
		}
		if (m_teammateTargetingPanel.activeSelf)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialTeammateTargetingPanelOUT", 0);
		}
		if (!m_energyAndUltimatesPanel.activeSelf)
		{
			return;
		}
		while (true)
		{
			m_AnimatorObject.GetComponent<Animator>().Play("TutorialEnergyAndUltimatesPanelOUT", 0);
			return;
		}
	}

	public void ClearAllPanels()
	{
		UIManager.SetGameObjectActive(m_background, false);
		UIManager.SetGameObjectActive(m_combatPhasePanel, false);
		UIManager.SetGameObjectActive(m_dashPhasePanel, false);
		UIManager.SetGameObjectActive(m_prepPhasePanel, false);
		UIManager.SetGameObjectActive(m_continuePanel, false);
		UIManager.SetGameObjectActive(m_statusEffectPanel, false);
		UIManager.SetGameObjectActive(m_teammateTargetingPanel, false);
		UIManager.SetGameObjectActive(m_energyAndUltimatesPanel, false);
		UIManager.SetGameObjectActive(m_fadeOutPanel, false);
		Animator component = m_AnimatorObject.GetComponent<Animator>();
		if (!component.isActiveAndEnabled)
		{
			return;
		}
		while (true)
		{
			component.Play("WaitingToRun", 0);
			return;
		}
	}

	public bool IsAnyPanelVisible()
	{
		return m_background.activeSelf;
	}
}
