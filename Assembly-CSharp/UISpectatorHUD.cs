using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpectatorHUD : MonoBehaviour
{
	public enum SpectatorToggleOption
	{
		HideAbilityTemplates,
		HideMovementLines
	}

	public class OptionButtonContext
	{
		public SpectatorToggleOption m_toggleOption;

		public OptionButtonContext(SpectatorToggleOption option)
		{
			m_toggleOption = option;
		}
	}

	public GameObject m_teamAButton;

	public GameObject m_teamBButton;

	public GameObject m_teamAllButton;

	public GameObject m_teamAPanel;

	public GameObject m_teamBPanel;

	public GameObject m_teamAllPanel;

	public GameObject m_timer;

	public TextMeshProUGUI m_timerText;

	public RectTransform m_spectatorOptionsContainer;

	public _SelectableBtn m_optionsToggleBtn;

	public RectTransform m_optionsVisibleContainer;

	public RectTransform m_optionsNotVisibleContainer;

	public LayoutGroup m_optionListContainer;

	public _SelectableBtn m_optionBtnPrefab;

	private bool m_decisionMode;

	private List<_SelectableBtn> m_optionsList;

	private Dictionary<_SelectableBtn, OptionButtonContext> m_buttonToContext = new Dictionary<_SelectableBtn, OptionButtonContext>();

	private void Start()
	{
		SetupOptions();
		SetTeamViewing(Team.Invalid);
		m_decisionMode = false;
		SetTimerMode(m_decisionMode);
		if ((bool)m_timer)
		{
			UIManager.SetGameObjectActive(m_timer, false);
		}
		if ((bool)m_teamAButton)
		{
			UIEventTriggerUtils.AddListener(m_teamAButton, EventTriggerType.PointerClick, OnTeamClicked);
		}
		if ((bool)m_teamBButton)
		{
			UIEventTriggerUtils.AddListener(m_teamBButton, EventTriggerType.PointerClick, OnTeamClicked);
		}
		if ((bool)m_teamAllButton)
		{
			UIEventTriggerUtils.AddListener(m_teamAllButton, EventTriggerType.PointerClick, OnTeamClicked);
		}
	}

	private void SetupOptions()
	{
		m_optionsToggleBtn.spriteController.callback = ToggleOptionBtnClicked;
		UIManager.SetGameObjectActive(m_optionListContainer, false);
		UIManager.SetGameObjectActive(m_optionsVisibleContainer, false);
		UIManager.SetGameObjectActive(m_optionsNotVisibleContainer, true);
		UIManager.SetGameObjectActive(m_spectatorOptionsContainer, true);
		m_optionsList = new List<_SelectableBtn>(m_optionListContainer.GetComponentsInChildren<_SelectableBtn>(true));
		int num = 0;
		SpectatorToggleOption[] array = null;
		if (HUD_UIResources.Get() != null)
		{
			array = HUD_UIResources.Get().m_spectatorOptionsToShow;
			num = array.Length;
		}
		else
		{
			Log.Error("HUD_UIResources is null when trying to setup Spectator HUD Options");
		}
		for (int i = 0; i < num; i++)
		{
			if (i >= m_optionsList.Count)
			{
				_SelectableBtn selectableBtn = Object.Instantiate(m_optionBtnPrefab);
				selectableBtn.transform.SetParent(m_optionListContainer.transform);
				selectableBtn.transform.localEulerAngles = Vector3.zero;
				selectableBtn.transform.localScale = Vector3.one;
				selectableBtn.transform.localPosition = Vector3.zero;
				m_optionsList.Add(selectableBtn);
			}
			SetupOptionBtn(m_optionsList[i], array[i]);
		}
		while (true)
		{
			for (int j = num; j < m_optionsList.Count; j++)
			{
				UIManager.SetGameObjectActive(m_optionsList[j], false);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void ToggleOptionBtnClicked(BaseEventData data)
	{
		bool flag = !m_optionListContainer.gameObject.activeSelf;
		UIManager.SetGameObjectActive(m_optionListContainer, flag);
		UIManager.SetGameObjectActive(m_optionsVisibleContainer, flag);
		UIManager.SetGameObjectActive(m_optionsNotVisibleContainer, !flag);
	}

	private void SetupOptionBtn(_SelectableBtn btn, SpectatorToggleOption option)
	{
		string text = StringUtil.GetSpectatorToggleOptionName(option);
		if (string.IsNullOrEmpty(text))
		{
			text = option.ToString();
		}
		TextMeshProUGUI[] componentsInChildren = btn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
		btn.spriteController.callback = OptionClicked;
		if (!m_buttonToContext.ContainsKey(btn))
		{
			m_buttonToContext[btn] = new OptionButtonContext(option);
		}
		else
		{
			Log.Error("Spectator option, trying to setup button multiple times");
		}
	}

	public void OptionClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		while (true)
		{
			if (pointerEventData.pointerCurrentRaycast.gameObject == null)
			{
				return;
			}
			_ButtonSwapSprite component = pointerEventData.pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
			for (int i = 0; i < m_optionsList.Count; i++)
			{
				_SelectableBtn selectableBtn = m_optionsList[i];
				if (!(component == selectableBtn.spriteController))
				{
					continue;
				}
				while (true)
				{
					if (m_buttonToContext.ContainsKey(selectableBtn))
					{
						OptionButtonContext optionButtonContext = m_buttonToContext[selectableBtn];
						SetToggleOption(optionButtonContext.m_toggleOption, !component.selectableButton.IsSelected());
					}
					component.selectableButton.ToggleSelected();
					return;
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void SetToggleOption(SpectatorToggleOption option, bool desiredValue)
	{
		if (option == SpectatorToggleOption.HideAbilityTemplates)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().SpectatorHideAbilityTargeter = desiredValue;
			}
		}
		if (option != SpectatorToggleOption.HideMovementLines)
		{
			return;
		}
		while (true)
		{
			LineData.SpectatorHideMovementLines = desiredValue;
			return;
		}
	}

	private void Update()
	{
		SetDecisionMode(GameFlowData.Get().gameState == GameState.BothTeams_Decision);
		if (!m_decisionMode)
		{
			return;
		}
		while (true)
		{
			string text = ((int)GameFlowData.Get().GetTimeRemainingInDecision()).ToString();
			if (!(m_timerText != null))
			{
				return;
			}
			while (true)
			{
				if (!(text != m_timerText.text))
				{
					return;
				}
				m_timerText.text = text;
				Animator component = m_timer.GetComponent<Animator>();
				if (component != null)
				{
					while (true)
					{
						component.Play("SpectatorTimerNumberChangeIN", 1, 0f);
						return;
					}
				}
				return;
			}
		}
	}

	public void SetDecisionMode(bool decisionMode)
	{
		if (decisionMode != m_decisionMode)
		{
			SetTimerMode(decisionMode);
			m_decisionMode = decisionMode;
		}
	}

	private void SetTimerMode(bool decisionMode)
	{
		if (!(m_timer != null))
		{
			return;
		}
		while (true)
		{
			Animator component = m_timer.GetComponent<Animator>();
			if (!(component != null))
			{
				return;
			}
			if (decisionMode)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_timer, true);
						component.Play("SpectatorTimerDefaultIN", 0, 0f);
						component.Play("SpectatorTimerNumberChangeIN", 1, 0f);
						return;
					}
				}
			}
			component.Play("SpectatorTimerDefaultOUT", 0, 0f);
			return;
		}
	}

	public void SetTeamViewing(Team team)
	{
		if (!m_teamAPanel)
		{
			return;
		}
		while (true)
		{
			if (!m_teamBPanel)
			{
				return;
			}
			while (true)
			{
				if (!m_teamAllPanel)
				{
					return;
				}
				switch (team)
				{
				case Team.TeamA:
					while (true)
					{
						UIManager.SetGameObjectActive(m_teamAPanel, true);
						UIManager.SetGameObjectActive(m_teamBPanel, false);
						UIManager.SetGameObjectActive(m_teamAllPanel, false);
						return;
					}
				case Team.TeamB:
					UIManager.SetGameObjectActive(m_teamAPanel, false);
					UIManager.SetGameObjectActive(m_teamBPanel, true);
					UIManager.SetGameObjectActive(m_teamAllPanel, false);
					break;
				default:
					UIManager.SetGameObjectActive(m_teamAPanel, false);
					UIManager.SetGameObjectActive(m_teamBPanel, false);
					UIManager.SetGameObjectActive(m_teamAllPanel, true);
					break;
				}
				return;
			}
		}
	}

	private void OnTeamClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (!(GameFlowData.Get().LocalPlayerData != null))
		{
			return;
		}
		while (true)
		{
			if (pointerEventData == null)
			{
				return;
			}
			while (true)
			{
				if (pointerEventData.pointerPress == m_teamAButton)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.TeamA);
							return;
						}
					}
				}
				if (pointerEventData.pointerPress == m_teamBButton)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.TeamB);
							return;
						}
					}
				}
				if (pointerEventData.pointerPress == m_teamAllButton)
				{
					GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.Invalid);
				}
				return;
			}
		}
	}
}
