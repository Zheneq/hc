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
		UIManager.SetGameObjectActive(m_lockImage, false);
		if (m_theButton != null)
		{
			UIEventTriggerUtils.AddListener(m_theButton.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
			UIEventTriggerUtils.AddListener(m_theButton.gameObject, EventTriggerType.PointerExit, OnPointerExit);
			UIEventTriggerUtils.AddListener(m_theButton.gameObject, EventTriggerType.PointerDown, OnPointerDown);
			UIEventTriggerUtils.AddListener(m_theButton.gameObject, EventTriggerType.PointerUp, OnPointerUp);
		}
		RectTransform rectTransform = GetComponent<Transform>() as RectTransform;
		if (rectTransform != null)
		{
			m_originalAnchoredPosition = rectTransform.anchoredPosition;
		}
		m_theButtonImage = m_theButton.GetComponent<Image>();
	}

	private void Update()
	{
		UIManager.SetGameObjectActive(m_lockInPressed, m_mouseIsDown);
		UIManager.SetGameObjectActive(m_cancelPressed, m_mouseIsDown);
	}

	private void OnEnable()
	{
		Update();
	}

	private void OnPointerDown(BaseEventData data)
	{
		m_mouseIsDown = true;
		UpdateMouseDownLocation();
	}

	private void OnPointerUp(BaseEventData data)
	{
		m_mouseIsDown = false;
		UpdateMouseDownLocation();
	}

	private void OnPointerEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_lockInHover, true);
		UIManager.SetGameObjectActive(m_cancelHover, true);
		m_lockInText.color = m_lockinHoverColor;
		m_cancelText.color = m_cancelHoverColor;
	}

	private void OnPointerExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_lockInHover, false);
		UIManager.SetGameObjectActive(m_cancelHover, false);
		m_lockInText.color = Color.white;
		m_cancelText.color = Color.white;
		m_mouseIsDown = false;
		UpdateMouseDownLocation();
	}

	private void UpdateMouseDownLocation()
	{
		RectTransform rectTransform = GetComponent<Transform>() as RectTransform;
		if (!(rectTransform != null))
		{
			return;
		}
		while (true)
		{
			if (m_mouseIsDown)
			{
				rectTransform.anchoredPosition = m_originalAnchoredPosition + new Vector2(0f, 0f - m_lockInContainer.rect.height * 0.05f);
			}
			else
			{
				rectTransform.anchoredPosition = m_originalAnchoredPosition;
			}
			return;
		}
	}

	public void CancelClicked()
	{
	}

	public void LockedInClicked()
	{
		OnPointerExit(null);
	}

	public void Unclicked()
	{
	}

	public void SetClickCallback(UIEventTriggerUtils.EventDelegate callback)
	{
		if (!(m_theButton != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(m_theButton.gameObject, EventTriggerType.PointerClick, callback);
			return;
		}
	}

	public void CancelButtonAnimationDone()
	{
		UIManager.SetGameObjectActive(m_lockImage, false);
	}

	public void SetDecisionContainerVisible(bool visible, bool isDead)
	{
		int num;
		if (isDead)
		{
			if (!(SpawnPointManager.Get() == null))
			{
				num = ((!SpawnPointManager.Get().m_playersSelectRespawn) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		RectTransform decisionObjectContainer = m_decisionObjectContainer;
		int doActive;
		if (visible)
		{
			doActive = ((!flag) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(decisionObjectContainer, (byte)doActive != 0);
		Button theButton = m_theButton;
		int doActive2;
		if (visible)
		{
			doActive2 = ((!flag) ? 1 : 0);
		}
		else
		{
			doActive2 = 0;
		}
		UIManager.SetGameObjectActive(theButton, (byte)doActive2 != 0);
		RectTransform playerDeadContainer = m_playerDeadContainer;
		int doActive3;
		if (visible)
		{
			doActive3 = (flag ? 1 : 0);
		}
		else
		{
			doActive3 = 0;
		}
		UIManager.SetGameObjectActive(playerDeadContainer, (byte)doActive3 != 0);
		if (SinglePlayerManager.Get() != null)
		{
			if (SinglePlayerManager.Get().GetLockinPhaseDisplayForceOff())
			{
				UIManager.SetGameObjectActive(m_phaseMarkerContainer, false);
				goto IL_00f4;
			}
		}
		UIManager.SetGameObjectActive(m_phaseMarkerContainer, !visible);
		goto IL_00f4;
		IL_00f4:
		if (SinglePlayerManager.Get() != null)
		{
			if (SinglePlayerManager.Get().GetLockinPhaseTextForceOff())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_phaseLabelContainer, false);
						return;
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_phaseLabelContainer, !visible);
	}

	public void UpdatePhase()
	{
		int value = 3;
		string text = StringUtil.TR("MOVE", "Global");
		Color color = m_movementPhaseColor;
		UIQueueListPanel.UIPhase uIPhaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(ServerClientUtils.GetCurrentAbilityPhase());
		if (ServerClientUtils.GetCurrentActionPhase() == ActionBufferPhase.Abilities)
		{
			value = (int)uIPhaseFromAbilityPriority;
			UIManager.SetGameObjectActive(m_phaseIndicatorsContainer, true);
			UIManager.SetGameObjectActive(m_phaseIcon, true);
			UIManager.SetGameObjectActive(m_movementIcon, false);
			if (uIPhaseFromAbilityPriority != 0)
			{
				if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Evasion)
				{
					if (uIPhaseFromAbilityPriority != UIQueueListPanel.UIPhase.Combat)
					{
					}
					else
					{
						text = StringUtil.TR("BLAST", "Global");
						color = m_combatPhaseColor;
					}
				}
				else
				{
					text = StringUtil.TR("DASH", "Global");
					color = m_evasionPhaseColor;
				}
			}
			else
			{
				text = StringUtil.TR("PREP", "Global");
				color = m_prepPhaseColor;
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_phaseIndicatorsContainer, false);
			UIManager.SetGameObjectActive(m_phaseIcon, false);
			UIManager.SetGameObjectActive(m_movementIcon, true);
		}
		if (m_phaseText.text != text)
		{
			m_phaseTextController.Play("PhaseTextIdle");
			m_phaseTextController.Play("PhaseTextChange");
			if (text != StringUtil.TR("MOVE", "Global"))
			{
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(uIPhaseFromAbilityPriority - 1);
			}
			else
			{
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties();
			}
			m_phaseText.text = text;
			m_phaseColor.color = color;
		}
		value = Mathf.Clamp(value, 0, 3);
		for (int i = 0; i < m_phaseIndicators.Length; i++)
		{
			UIManager.SetGameObjectActive(m_phaseIndicators[i], i == value);
		}
		while (true)
		{
			m_phaseIcon.sprite = m_phaseIcons[value];
			return;
		}
	}

	public void EnableLockIn(bool active, bool enabled)
	{
		m_theButtonImage.enabled = enabled;
		if (m_lockInShowing == active)
		{
			return;
		}
		Unclicked();
		if (!m_lockInShowing)
		{
			UIManager.SetGameObjectActive(m_lockInContainer, true);
			UIManager.SetGameObjectActive(m_cancelContainer, false);
			UIManager.SetGameObjectActive(m_lockImage, false);
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyLockedIn(false);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyLockedIn(false);
		}
		else
		{
			UIManager.SetGameObjectActive(m_lockInContainer, false);
			UIManager.SetGameObjectActive(m_cancelContainer, true);
			m_animationController.Play("CancelButtonActive");
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyLockedIn(true);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyLockedIn(true);
		}
		m_lockInShowing = active;
	}

	public bool IsShowingLockIn()
	{
		return m_lockInShowing;
	}
}
