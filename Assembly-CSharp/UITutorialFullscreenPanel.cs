using System;
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
		return UITutorialFullscreenPanel.s_instance;
	}

	private void Awake()
	{
		UITutorialFullscreenPanel.s_instance = this;
		UIManager.SetGameObjectActive(this.m_background, false, null);
		UIManager.SetGameObjectActive(this.m_combatPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_dashPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_prepPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_statusEffectPanel, false, null);
		UIManager.SetGameObjectActive(this.m_teammateTargetingPanel, false, null);
		UIManager.SetGameObjectActive(this.m_energyAndUltimatesPanel, false, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, false, null);
		this.m_FadeoutAnimator = this.m_fadeOutPanel.GetComponent<Animator>();
	}

	private void Start()
	{
		if (this.m_continueButton != null)
		{
			UIEventTriggerUtils.AddListener(this.m_continueButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnContinueButton));
		}
	}

	private void OnDestroy()
	{
		UITutorialFullscreenPanel.s_instance = null;
	}

	private void Update()
	{
		if (this.m_background.activeSelf)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_AnimatorObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
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
			this.ClearAllPanels();
		}
	}

	private void SetupMoviePlayer(PlayRawImageMovieTexture moviePlayer, string movieName)
	{
		if (!(moviePlayer == null))
		{
			if (!movieName.IsNullOrEmpty())
			{
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
					Log.Error("Unable to find prologue video", new object[0]);
					return;
				}
				if (HUD_UIResources.Get().m_prologueVideoList.Length > num)
				{
					HUD_UIResources.TutorialVideoInfo videoInfo = HUD_UIResources.Get().m_prologueVideoList[num];
					string localizedVideoPath = HUD_UIResources.Get().GetLocalizedVideoPath(videoInfo, LocalizationManager.CurrentLanguageCode);
					if (localizedVideoPath.IsNullOrEmpty())
					{
						localizedVideoPath = HUD_UIResources.Get().GetLocalizedVideoPath(videoInfo, "en");
					}
					if (!localizedVideoPath.IsNullOrEmpty())
					{
						moviePlayer.Play(localizedVideoPath, true, true, true);
					}
				}
				return;
			}
		}
	}

	public void ShowCombatPhasePanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_combatPhasePanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialCombatPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = this.m_combatPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		this.SetupMoviePlayer(componentInChildren, "Combat");
	}

	public void ShowDashPhasePanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_dashPhasePanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialDashPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = this.m_dashPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		this.SetupMoviePlayer(componentInChildren, "Dash");
	}

	public void ShowPrepPhasePanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_prepPhasePanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialPrepPhasePanelIN", 0);
		PlayRawImageMovieTexture componentInChildren = this.m_prepPhasePanel.GetComponentInChildren<PlayRawImageMovieTexture>(true);
		this.SetupMoviePlayer(componentInChildren, "Prep");
	}

	public void ShowStatusEffectPanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_statusEffectPanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_background.GetComponent<Animator>().Play("TutorialStatusEffectPanelIN", 0);
	}

	public void ShowTeammateTargetingPanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_teammateTargetingPanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialTeammateTargetingPanelIN", 0);
	}

	public void ShowEnergyAndUltimatesPanel()
	{
		UIManager.SetGameObjectActive(this.m_background, true, null);
		UIManager.SetGameObjectActive(this.m_energyAndUltimatesPanel, true, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, true, null);
		this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialEnergyAndUltimatesPanelIN", 0);
	}

	public void FadeOut()
	{
		UIAnimationEventManager.Get().PlayAnimation(this.m_FadeoutAnimator, "SlowFadeDefaultIN", new UIAnimationEventManager.AnimationDoneCallback(this.FadeOutAnimDone), "FadeDefaultIDLE", 0, 0f, true, false, null, null);
	}

	public void FadeIn()
	{
		UIAnimationEventManager.Get().PlayAnimation(this.m_FadeoutAnimator, "SlowFadeDefaultOUT", new UIAnimationEventManager.AnimationDoneCallback(this.FadeInAnimDone), string.Empty, 0, 0f, true, false, null, null);
	}

	public void FadeInAnimDone()
	{
		UIManager.SetGameObjectActive(this.m_fadeOutPanel, false, null);
	}

	public void FadeOutAnimDone()
	{
	}

	public void OnContinueButton(BaseEventData data)
	{
		if (this.m_prepPhasePanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialPrepPhasePanelOUT", 0);
		}
		if (this.m_dashPhasePanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialDashPhasePanelOUT", 0);
		}
		if (this.m_combatPhasePanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialCombatPhasePanelOUT", 0);
		}
		if (this.m_statusEffectPanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialStatusEffectPanelOUT", 0);
		}
		if (this.m_teammateTargetingPanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialTeammateTargetingPanelOUT", 0);
		}
		if (this.m_energyAndUltimatesPanel.activeSelf)
		{
			this.m_AnimatorObject.GetComponent<Animator>().Play("TutorialEnergyAndUltimatesPanelOUT", 0);
		}
	}

	public void ClearAllPanels()
	{
		UIManager.SetGameObjectActive(this.m_background, false, null);
		UIManager.SetGameObjectActive(this.m_combatPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_dashPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_prepPhasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_continuePanel, false, null);
		UIManager.SetGameObjectActive(this.m_statusEffectPanel, false, null);
		UIManager.SetGameObjectActive(this.m_teammateTargetingPanel, false, null);
		UIManager.SetGameObjectActive(this.m_energyAndUltimatesPanel, false, null);
		UIManager.SetGameObjectActive(this.m_fadeOutPanel, false, null);
		Animator component = this.m_AnimatorObject.GetComponent<Animator>();
		if (component.isActiveAndEnabled)
		{
			component.Play("WaitingToRun", 0);
		}
	}

	public bool IsAnyPanelVisible()
	{
		return this.m_background.activeSelf;
	}
}
