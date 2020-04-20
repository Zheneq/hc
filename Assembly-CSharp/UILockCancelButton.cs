using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILockCancelButton : MonoBehaviour
{
	public RectTransform m_lockInContainer;

	public RectTransform m_cancelContainer;

	public RectTransform m_decisionObjectContainer;

	public RectTransform m_phaseMarkerContainer;

	public RectTransform m_playerDeadContainer;

	public RectTransform m_phaseIndicatorsContainer;

	public RectTransform m_phaseLabelContainer;

	public Button m_theButton;

	public Image m_lockImage;

	public Image m_lockInDefault;

	public Image m_lockInHover;

	public Image m_lockInPressed;

	public TextMeshProUGUI m_lockInText;

	public Image m_cancelDefault;

	public Image m_cancelHover;

	public Image m_cancelPressed;

	public TextMeshProUGUI m_cancelText;

	public Image m_phaseColor;

	public Color m_prepPhaseColor;

	public Color m_evasionPhaseColor;

	public Color m_combatPhaseColor;

	public Color m_movementPhaseColor;

	public Image[] m_phaseIndicators;

	public Image m_phaseIcon;

	public Image m_movementIcon;

	public Sprite[] m_phaseIcons;

	public TextMeshProUGUI m_phaseText;

	public Color m_lockinHoverColor;

	public Color m_cancelHoverColor;

	public Animator m_animationController;

	public Animator m_phaseTextController;

	private bool m_mouseIsDown;

	private Vector2 m_originalAnchoredPosition;

	private bool m_lockInShowing;

	private Image m_theButtonImage;

	private void Start()
	{
		UIManager.SetGameObjectActive(this.m_lockImage, false, null);
		if (this.m_theButton != null)
		{
			UIEventTriggerUtils.AddListener(this.m_theButton.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
			UIEventTriggerUtils.AddListener(this.m_theButton.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
			UIEventTriggerUtils.AddListener(this.m_theButton.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnPointerDown));
			UIEventTriggerUtils.AddListener(this.m_theButton.gameObject, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.OnPointerUp));
		}
		RectTransform rectTransform = base.GetComponent<Transform>() as RectTransform;
		if (rectTransform != null)
		{
			this.m_originalAnchoredPosition = rectTransform.anchoredPosition;
		}
		this.m_theButtonImage = this.m_theButton.GetComponent<Image>();
	}

	private void Update()
	{
		UIManager.SetGameObjectActive(this.m_lockInPressed, this.m_mouseIsDown, null);
		UIManager.SetGameObjectActive(this.m_cancelPressed, this.m_mouseIsDown, null);
	}

	private void OnEnable()
	{
		this.Update();
	}

	private void OnPointerDown(BaseEventData data)
	{
		this.m_mouseIsDown = true;
		this.UpdateMouseDownLocation();
	}

	private void OnPointerUp(BaseEventData data)
	{
		this.m_mouseIsDown = false;
		this.UpdateMouseDownLocation();
	}

	private void OnPointerEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_lockInHover, true, null);
		UIManager.SetGameObjectActive(this.m_cancelHover, true, null);
		this.m_lockInText.color = this.m_lockinHoverColor;
		this.m_cancelText.color = this.m_cancelHoverColor;
	}

	private void OnPointerExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_lockInHover, false, null);
		UIManager.SetGameObjectActive(this.m_cancelHover, false, null);
		this.m_lockInText.color = Color.white;
		this.m_cancelText.color = Color.white;
		this.m_mouseIsDown = false;
		this.UpdateMouseDownLocation();
	}

	private void UpdateMouseDownLocation()
	{
		RectTransform rectTransform = base.GetComponent<Transform>() as RectTransform;
		if (rectTransform != null)
		{
			if (this.m_mouseIsDown)
			{
				rectTransform.anchoredPosition = this.m_originalAnchoredPosition + new Vector2(0f, -(this.m_lockInContainer.rect.height * 0.05f));
			}
			else
			{
				rectTransform.anchoredPosition = this.m_originalAnchoredPosition;
			}
		}
	}

	public void CancelClicked()
	{
	}

	public void LockedInClicked()
	{
		this.OnPointerExit(null);
	}

	public void Unclicked()
	{
	}

	public void SetClickCallback(UIEventTriggerUtils.EventDelegate callback)
	{
		if (this.m_theButton != null)
		{
			UIEventTriggerUtils.AddListener(this.m_theButton.gameObject, EventTriggerType.PointerClick, callback);
		}
	}

	public void CancelButtonAnimationDone()
	{
		UIManager.SetGameObjectActive(this.m_lockImage, false, null);
	}

	public void SetDecisionContainerVisible(bool visible, bool isDead)
	{
		bool flag;
		if (isDead)
		{
			if (!(SpawnPointManager.Get() == null))
			{
				flag = !SpawnPointManager.Get().m_playersSelectRespawn;
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		Component decisionObjectContainer = this.m_decisionObjectContainer;
		bool doActive;
		if (visible)
		{
			doActive = !flag2;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(decisionObjectContainer, doActive, null);
		Component theButton = this.m_theButton;
		bool doActive2;
		if (visible)
		{
			doActive2 = !flag2;
		}
		else
		{
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(theButton, doActive2, null);
		Component playerDeadContainer = this.m_playerDeadContainer;
		bool doActive3;
		if (visible)
		{
			doActive3 = flag2;
		}
		else
		{
			doActive3 = false;
		}
		UIManager.SetGameObjectActive(playerDeadContainer, doActive3, null);
		if (SinglePlayerManager.Get() != null)
		{
			if (SinglePlayerManager.Get().GetLockinPhaseDisplayForceOff())
			{
				UIManager.SetGameObjectActive(this.m_phaseMarkerContainer, false, null);
				goto IL_F4;
			}
		}
		UIManager.SetGameObjectActive(this.m_phaseMarkerContainer, !visible, null);
		IL_F4:
		if (SinglePlayerManager.Get() != null)
		{
			if (SinglePlayerManager.Get().GetLockinPhaseTextForceOff())
			{
				UIManager.SetGameObjectActive(this.m_phaseLabelContainer, false, null);
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_phaseLabelContainer, !visible, null);
	}

	public void UpdatePhase()
	{
		int num = 3;
		string text = StringUtil.TR("MOVE", "Global");
		Color color = this.m_movementPhaseColor;
		UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
		if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
		{
			num = (int)uiphaseFromAbilityPriority;
			UIManager.SetGameObjectActive(this.m_phaseIndicatorsContainer, true, null);
			UIManager.SetGameObjectActive(this.m_phaseIcon, true, null);
			UIManager.SetGameObjectActive(this.m_movementIcon, false, null);
			if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Prep)
			{
				if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
				{
					if (uiphaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
					{
					}
					else
					{
						text = StringUtil.TR("BLAST", "Global");
						color = this.m_combatPhaseColor;
					}
				}
				else
				{
					text = StringUtil.TR("DASH", "Global");
					color = this.m_evasionPhaseColor;
				}
			}
			else
			{
				text = StringUtil.TR("PREP", "Global");
				color = this.m_prepPhaseColor;
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_phaseIndicatorsContainer, false, null);
			UIManager.SetGameObjectActive(this.m_phaseIcon, false, null);
			UIManager.SetGameObjectActive(this.m_movementIcon, true, null);
		}
		if (this.m_phaseText.text != text)
		{
			this.m_phaseTextController.Play("PhaseTextIdle");
			this.m_phaseTextController.Play("PhaseTextChange");
			if (text != StringUtil.TR("MOVE", "Global"))
			{
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(uiphaseFromAbilityPriority - 1);
			}
			else
			{
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(UIQueueListPanel.UIPhase.None);
			}
			this.m_phaseText.text = text;
			this.m_phaseColor.color = color;
		}
		num = Mathf.Clamp(num, 0, 3);
		for (int i = 0; i < this.m_phaseIndicators.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_phaseIndicators[i], i == num, null);
		}
		this.m_phaseIcon.sprite = this.m_phaseIcons[num];
	}

	public void EnableLockIn(bool active, bool enabled)
	{
		this.m_theButtonImage.enabled = enabled;
		if (this.m_lockInShowing == active)
		{
			return;
		}
		this.Unclicked();
		if (!this.m_lockInShowing)
		{
			UIManager.SetGameObjectActive(this.m_lockInContainer, true, null);
			UIManager.SetGameObjectActive(this.m_cancelContainer, false, null);
			UIManager.SetGameObjectActive(this.m_lockImage, false, null);
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyLockedIn(false);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyLockedIn(false);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_lockInContainer, false, null);
			UIManager.SetGameObjectActive(this.m_cancelContainer, true, null);
			this.m_animationController.Play("CancelButtonActive");
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyLockedIn(true);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyLockedIn(true);
		}
		this.m_lockInShowing = active;
	}

	public bool IsShowingLockIn()
	{
		return this.m_lockInShowing;
	}
}
