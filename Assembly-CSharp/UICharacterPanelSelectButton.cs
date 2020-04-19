using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterPanelSelectButton : MonoBehaviour
{
	public Image m_charImageAvailable;

	public Image m_charImageLocked;

	public Image m_charImageDisabled;

	public RectTransform m_LockedContainer;

	public RectTransform m_AvailableContainer;

	public RectTransform m_DisabledContainer;

	public RectTransform m_freeRotation;

	public RectTransform m_UnavailableExpBarContainer;

	public RectTransform m_NormalExpBarContainer;

	public RectTransform m_MasterExpBarContainer;

	public ImageFilledSloped m_unavailableBar;

	public ImageFilledSloped m_normalBar;

	public ImageFilledSloped m_masterBar;

	public TextMeshProUGUI[] m_levelTextLabels;

	public _SelectableBtn m_button;

	public CharacterType m_characterType;

	protected bool m_isDisabled;

	private bool m_isMaster;

	private bool m_isTooltipInitialized;

	protected CharacterResourceLink m_characterResourceLink;

	private void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_button.spriteController.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnButtonClicked));
	}

	public void UpdateFreeRotationIcon()
	{
		if (this.m_characterResourceLink != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterPanelSelectButton.UpdateFreeRotationIcon()).MethodHandle;
			}
			bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(this.m_characterResourceLink.m_characterType, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
			UIManager.SetGameObjectActive(this.m_freeRotation, doActive, null);
		}
	}

	protected virtual FrontEndButtonSounds SoundToPlayOnClick()
	{
		return FrontEndButtonSounds.CharacterSelectModAdd;
	}

	public void SetClickable(bool clickable)
	{
		this.m_button.spriteController.SetClickable(clickable);
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return this.m_characterResourceLink;
	}

	public bool IsDisabled()
	{
		return this.m_isDisabled;
	}

	public virtual void Setup(bool isAvailable, bool selected = false)
	{
		bool flag;
		if (ClientGameManager.Get().GroupInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterPanelSelectButton.Setup(bool, bool)).MethodHandle;
			}
			flag = (ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		this.SetSelected(selected);
		CharacterResourceLink characterResourceLink = null;
		if (this.m_characterType != CharacterType.None)
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
			if (GameManager.Get() != null && GameManager.Get().IsCharacterVisible(this.m_characterType))
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
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_characterType);
			}
		}
		if (characterResourceLink != null)
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
			if (!this.m_isTooltipInitialized)
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
				UITooltipHoverObject component = this.m_button.spriteController.GetComponent<UITooltipHoverObject>();
				if (characterResourceLink.m_characterType.IsWillFill())
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
					component.Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
					{
						if (this.m_button.spriteController.IsClickable())
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UICharacterPanelSelectButton.<Setup>m__0(UITooltipBase)).MethodHandle;
							}
							if (!(this.m_characterResourceLink == null))
							{
								UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
								uititledTooltip.Setup(this.m_characterResourceLink.GetDisplayName(), this.m_characterResourceLink.GetCharSelectTooltipDescription(), string.Empty);
								return true;
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
						}
						return false;
					}, null);
				}
				else
				{
					component.Setup(TooltipType.Character, delegate(UITooltipBase tooltip)
					{
						if (this.m_button.spriteController.IsClickable())
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UICharacterPanelSelectButton.<Setup>m__1(UITooltipBase)).MethodHandle;
							}
							if (!(this.m_characterResourceLink == null))
							{
								UICharacterTooltip uicharacterTooltip = tooltip as UICharacterTooltip;
								uicharacterTooltip.Setup(this.m_characterResourceLink, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
								return true;
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
						return false;
					}, null);
				}
				this.m_isTooltipInitialized = true;
			}
			this.m_characterResourceLink = characterResourceLink;
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterResourceLink.m_characterType);
			bool enabled;
			if (!isAvailable)
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
				enabled = flag2;
			}
			else
			{
				enabled = true;
			}
			this.SetEnabled(enabled, playerCharacterData);
			bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(this.m_characterResourceLink.m_characterType, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
			UIManager.SetGameObjectActive(this.m_freeRotation, doActive, null);
			if (playerCharacterData != null)
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
				int level = playerCharacterData.ExperienceComponent.Level;
				string text = level.ToString();
				for (int i = 0; i < this.m_levelTextLabels.Length; i++)
				{
					this.m_levelTextLabels[i].text = text;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (level < GameBalanceVars.Get().MaxCharacterLevel)
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
					float fillAmount = (float)playerCharacterData.ExperienceComponent.XPProgressThroughLevel / (float)GameBalanceVars.Get().CharacterExperienceToLevel(level);
					this.m_unavailableBar.fillAmount = fillAmount;
					this.m_normalBar.fillAmount = fillAmount;
					this.m_masterBar.fillAmount = fillAmount;
				}
				else
				{
					this.m_isMaster = true;
					this.m_unavailableBar.fillAmount = 1f;
					this.m_normalBar.fillAmount = 1f;
					this.m_masterBar.fillAmount = 1f;
				}
				Component masterExpBarContainer = this.m_MasterExpBarContainer;
				bool doActive2;
				if (!this.m_isDisabled)
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
					doActive2 = this.m_isMaster;
				}
				else
				{
					doActive2 = false;
				}
				UIManager.SetGameObjectActive(masterExpBarContainer, doActive2, null);
			}
		}
		else
		{
			this.m_characterResourceLink = null;
			UIManager.SetGameObjectActive(this.m_freeRotation, false, null);
			UIManager.SetGameObjectActive(this.m_LockedContainer, false, null);
			UIManager.SetGameObjectActive(this.m_DisabledContainer, false, null);
			UIManager.SetGameObjectActive(this.m_AvailableContainer, false, null);
			UIManager.SetGameObjectActive(this.m_NormalExpBarContainer, false, null);
			UIManager.SetGameObjectActive(this.m_UnavailableExpBarContainer, false, null);
			UIManager.SetGameObjectActive(this.m_MasterExpBarContainer, false, null);
			this.SetEnabled(false, null);
		}
	}

	public virtual void SetEnabled(bool enabled, PersistedCharacterData playerCharacterData)
	{
		if (this.m_characterResourceLink != null)
		{
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			this.m_charImageAvailable.sprite = this.m_characterResourceLink.GetCharacterSelectIcon();
			this.m_charImageLocked.sprite = this.m_characterResourceLink.GetCharacterSelectIconBW();
			this.m_charImageDisabled.sprite = this.m_characterResourceLink.GetCharacterSelectIconBW();
			bool flag = false;
			CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(this.m_characterResourceLink.m_characterType);
			if (characterConfig != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterPanelSelectButton.SetEnabled(bool, PersistedCharacterData)).MethodHandle;
				}
				if (characterConfig.GameTypesProhibitedFrom != null)
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
					if (UICharacterScreen.GetCurrentSpecificState() != null)
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
						flag = characterConfig.GameTypesProhibitedFrom.Contains(UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
					}
				}
			}
			bool flag2 = !GameManager.Get().IsValidForHumanPreGameSelection(this.m_characterResourceLink.m_characterType);
			Component disabledContainer = this.m_DisabledContainer;
			bool doActive;
			if (!enabled)
			{
				if (!flag)
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
					doActive = flag2;
				}
				else
				{
					doActive = true;
				}
			}
			else
			{
				doActive = false;
			}
			UIManager.SetGameObjectActive(disabledContainer, doActive, null);
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
		this.m_isDisabled = !enabled;
		UIManager.SetGameObjectActive(this.m_LockedContainer, !enabled, null);
		Component availableContainer = this.m_AvailableContainer;
		bool doActive2;
		if (enabled)
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
			doActive2 = (this.m_characterResourceLink != null);
		}
		else
		{
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(availableContainer, doActive2, null);
		Component masterExpBarContainer = this.m_MasterExpBarContainer;
		bool doActive3;
		if (!this.m_isDisabled)
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
			if (this.m_isMaster)
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
				doActive3 = (this.m_characterResourceLink != null);
				goto IL_1B6;
			}
		}
		doActive3 = false;
		IL_1B6:
		UIManager.SetGameObjectActive(masterExpBarContainer, doActive3, null);
		bool flag3;
		if (playerCharacterData != null)
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
			if (playerCharacterData.ExperienceComponent.Level <= 1)
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
				flag3 = (playerCharacterData.ExperienceComponent.XPProgressThroughLevel > 0);
			}
			else
			{
				flag3 = true;
			}
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		Component normalExpBarContainer = this.m_NormalExpBarContainer;
		bool doActive4;
		if (!this.m_isDisabled)
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
			doActive4 = flag4;
		}
		else
		{
			doActive4 = false;
		}
		UIManager.SetGameObjectActive(normalExpBarContainer, doActive4, null);
		Component unavailableExpBarContainer = this.m_UnavailableExpBarContainer;
		bool doActive5;
		if (this.m_isDisabled)
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
			doActive5 = flag4;
		}
		else
		{
			doActive5 = false;
		}
		UIManager.SetGameObjectActive(unavailableExpBarContainer, doActive5, null);
	}

	protected virtual void OnButtonClicked(BaseEventData data)
	{
		if (!this.m_button.spriteController.IsClickable())
		{
			return;
		}
		UIFrontEnd.PlaySound(this.SoundToPlayOnClick());
		bool flag;
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterPanelSelectButton.OnButtonClicked(BaseEventData)).MethodHandle;
			}
			if (GameManager.Get().PlayerInfo != null)
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
				flag = (GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready);
				goto IL_75;
			}
		}
		flag = false;
		IL_75:
		bool flag2 = flag;
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
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
			flag2 = AppState_GroupCharacterSelect.Get().IsReady();
		}
		if (this.m_button.spriteController.IsClickable())
		{
			if (!flag2)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientRequestToServerSelectCharacter = new CharacterType?(this.m_characterResourceLink.m_characterType)
				});
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void SetSelected(bool isSelected)
	{
		this.m_button.SetSelected(isSelected, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_button.m_selectedContainer, isSelected, null);
		this.m_button.spriteController.ResetMouseState();
	}
}
