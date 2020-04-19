using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpectatorHUD : MonoBehaviour
{
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

	private Dictionary<_SelectableBtn, UISpectatorHUD.OptionButtonContext> m_buttonToContext = new Dictionary<_SelectableBtn, UISpectatorHUD.OptionButtonContext>();

	private void Start()
	{
		this.SetupOptions();
		this.SetTeamViewing(Team.Invalid);
		this.m_decisionMode = false;
		this.SetTimerMode(this.m_decisionMode);
		if (this.m_timer)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.Start()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_timer, false, null);
		}
		if (this.m_teamAButton)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			UIEventTriggerUtils.AddListener(this.m_teamAButton, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTeamClicked));
		}
		if (this.m_teamBButton)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UIEventTriggerUtils.AddListener(this.m_teamBButton, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTeamClicked));
		}
		if (this.m_teamAllButton)
		{
			UIEventTriggerUtils.AddListener(this.m_teamAllButton, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnTeamClicked));
		}
	}

	private void SetupOptions()
	{
		this.m_optionsToggleBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleOptionBtnClicked);
		UIManager.SetGameObjectActive(this.m_optionListContainer, false, null);
		UIManager.SetGameObjectActive(this.m_optionsVisibleContainer, false, null);
		UIManager.SetGameObjectActive(this.m_optionsNotVisibleContainer, true, null);
		UIManager.SetGameObjectActive(this.m_spectatorOptionsContainer, true, null);
		this.m_optionsList = new List<_SelectableBtn>(this.m_optionListContainer.GetComponentsInChildren<_SelectableBtn>(true));
		int num = 0;
		UISpectatorHUD.SpectatorToggleOption[] array = null;
		if (HUD_UIResources.Get() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.SetupOptions()).MethodHandle;
			}
			array = HUD_UIResources.Get().m_spectatorOptionsToShow;
			num = array.Length;
		}
		else
		{
			Log.Error("HUD_UIResources is null when trying to setup Spectator HUD Options", new object[0]);
		}
		for (int i = 0; i < num; i++)
		{
			if (i >= this.m_optionsList.Count)
			{
				_SelectableBtn selectableBtn = UnityEngine.Object.Instantiate<_SelectableBtn>(this.m_optionBtnPrefab);
				selectableBtn.transform.SetParent(this.m_optionListContainer.transform);
				selectableBtn.transform.localEulerAngles = Vector3.zero;
				selectableBtn.transform.localScale = Vector3.one;
				selectableBtn.transform.localPosition = Vector3.zero;
				this.m_optionsList.Add(selectableBtn);
			}
			this.SetupOptionBtn(this.m_optionsList[i], array[i]);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = num; j < this.m_optionsList.Count; j++)
		{
			UIManager.SetGameObjectActive(this.m_optionsList[j], false, null);
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void ToggleOptionBtnClicked(BaseEventData data)
	{
		bool flag = !this.m_optionListContainer.gameObject.activeSelf;
		UIManager.SetGameObjectActive(this.m_optionListContainer, flag, null);
		UIManager.SetGameObjectActive(this.m_optionsVisibleContainer, flag, null);
		UIManager.SetGameObjectActive(this.m_optionsNotVisibleContainer, !flag, null);
	}

	private void SetupOptionBtn(_SelectableBtn btn, UISpectatorHUD.SpectatorToggleOption option)
	{
		string text = StringUtil.GetSpectatorToggleOptionName(option);
		if (string.IsNullOrEmpty(text))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.SetupOptionBtn(_SelectableBtn, UISpectatorHUD.SpectatorToggleOption)).MethodHandle;
			}
			text = option.ToString();
		}
		TextMeshProUGUI[] componentsInChildren = btn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
		btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OptionClicked);
		if (!this.m_buttonToContext.ContainsKey(btn))
		{
			this.m_buttonToContext[btn] = new UISpectatorHUD.OptionButtonContext(option);
		}
		else
		{
			Log.Error("Spectator option, trying to setup button multiple times", new object[0]);
		}
	}

	public void OptionClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.OptionClicked(BaseEventData)).MethodHandle;
			}
			if (!(pointerEventData.pointerCurrentRaycast.gameObject == null))
			{
				_ButtonSwapSprite component = pointerEventData.pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
				for (int i = 0; i < this.m_optionsList.Count; i++)
				{
					_SelectableBtn selectableBtn = this.m_optionsList[i];
					if (component == selectableBtn.spriteController)
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
						if (this.m_buttonToContext.ContainsKey(selectableBtn))
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							UISpectatorHUD.OptionButtonContext optionButtonContext = this.m_buttonToContext[selectableBtn];
							this.SetToggleOption(optionButtonContext.m_toggleOption, !component.selectableButton.IsSelected());
						}
						component.selectableButton.ToggleSelected(false);
						return;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					return;
				}
			}
		}
	}

	private void SetToggleOption(UISpectatorHUD.SpectatorToggleOption option, bool desiredValue)
	{
		if (option == UISpectatorHUD.SpectatorToggleOption.HideAbilityTemplates)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.SetToggleOption(UISpectatorHUD.SpectatorToggleOption, bool)).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				ClientGameManager.Get().SpectatorHideAbilityTargeter = desiredValue;
			}
		}
		if (option == UISpectatorHUD.SpectatorToggleOption.HideMovementLines)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			LineData.SpectatorHideMovementLines = desiredValue;
		}
	}

	private void Update()
	{
		this.SetDecisionMode(GameFlowData.Get().gameState == GameState.BothTeams_Decision);
		if (this.m_decisionMode)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.Update()).MethodHandle;
			}
			string text = ((int)GameFlowData.Get().GetTimeRemainingInDecision()).ToString();
			if (this.m_timerText != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (text != this.m_timerText.text)
				{
					this.m_timerText.text = text;
					Animator component = this.m_timer.GetComponent<Animator>();
					if (component != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						component.Play("SpectatorTimerNumberChangeIN", 1, 0f);
					}
				}
			}
		}
	}

	public void SetDecisionMode(bool decisionMode)
	{
		if (decisionMode != this.m_decisionMode)
		{
			this.SetTimerMode(decisionMode);
			this.m_decisionMode = decisionMode;
		}
	}

	private void SetTimerMode(bool decisionMode)
	{
		if (this.m_timer != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.SetTimerMode(bool)).MethodHandle;
			}
			Animator component = this.m_timer.GetComponent<Animator>();
			if (component != null)
			{
				if (decisionMode)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					UIManager.SetGameObjectActive(this.m_timer, true, null);
					component.Play("SpectatorTimerDefaultIN", 0, 0f);
					component.Play("SpectatorTimerNumberChangeIN", 1, 0f);
				}
				else
				{
					component.Play("SpectatorTimerDefaultOUT", 0, 0f);
				}
			}
		}
	}

	public void SetTeamViewing(Team team)
	{
		if (this.m_teamAPanel)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.SetTeamViewing(Team)).MethodHandle;
			}
			if (this.m_teamBPanel)
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
				if (this.m_teamAllPanel)
				{
					if (team == Team.TeamA)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						UIManager.SetGameObjectActive(this.m_teamAPanel, true, null);
						UIManager.SetGameObjectActive(this.m_teamBPanel, false, null);
						UIManager.SetGameObjectActive(this.m_teamAllPanel, false, null);
					}
					else if (team == Team.TeamB)
					{
						UIManager.SetGameObjectActive(this.m_teamAPanel, false, null);
						UIManager.SetGameObjectActive(this.m_teamBPanel, true, null);
						UIManager.SetGameObjectActive(this.m_teamAllPanel, false, null);
					}
					else
					{
						UIManager.SetGameObjectActive(this.m_teamAPanel, false, null);
						UIManager.SetGameObjectActive(this.m_teamBPanel, false, null);
						UIManager.SetGameObjectActive(this.m_teamAllPanel, true, null);
					}
				}
			}
		}
	}

	private void OnTeamClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (GameFlowData.Get().LocalPlayerData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISpectatorHUD.OnTeamClicked(BaseEventData)).MethodHandle;
			}
			if (pointerEventData != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (pointerEventData.pointerPress == this.m_teamAButton)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.TeamA);
				}
				else if (pointerEventData.pointerPress == this.m_teamBButton)
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
					GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.TeamB);
				}
				else if (pointerEventData.pointerPress == this.m_teamAllButton)
				{
					GameFlowData.Get().LocalPlayerData.SetSpectatingTeam(Team.Invalid);
				}
			}
		}
	}

	public enum SpectatorToggleOption
	{
		HideAbilityTemplates,
		HideMovementLines
	}

	public class OptionButtonContext
	{
		public UISpectatorHUD.SpectatorToggleOption m_toggleOption;

		public OptionButtonContext(UISpectatorHUD.SpectatorToggleOption option)
		{
			this.m_toggleOption = option;
		}
	}
}
