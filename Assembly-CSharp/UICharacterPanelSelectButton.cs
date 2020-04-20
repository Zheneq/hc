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
			if (GameManager.Get() != null && GameManager.Get().IsCharacterVisible(this.m_characterType))
			{
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_characterType);
			}
		}
		if (characterResourceLink != null)
		{
			if (!this.m_isTooltipInitialized)
			{
				UITooltipHoverObject component = this.m_button.spriteController.GetComponent<UITooltipHoverObject>();
				if (characterResourceLink.m_characterType.IsWillFill())
				{
					component.Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
					{
						if (this.m_button.spriteController.IsClickable())
						{
							if (!(this.m_characterResourceLink == null))
							{
								UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
								uititledTooltip.Setup(this.m_characterResourceLink.GetDisplayName(), this.m_characterResourceLink.GetCharSelectTooltipDescription(), string.Empty);
								return true;
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
							if (!(this.m_characterResourceLink == null))
							{
								UICharacterTooltip uicharacterTooltip = tooltip as UICharacterTooltip;
								uicharacterTooltip.Setup(this.m_characterResourceLink, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
								return true;
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
				int level = playerCharacterData.ExperienceComponent.Level;
				string text = level.ToString();
				for (int i = 0; i < this.m_levelTextLabels.Length; i++)
				{
					this.m_levelTextLabels[i].text = text;
				}
				if (level < GameBalanceVars.Get().MaxCharacterLevel)
				{
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
				if (characterConfig.GameTypesProhibitedFrom != null)
				{
					if (UICharacterScreen.GetCurrentSpecificState() != null)
					{
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
			if (this.m_isMaster)
			{
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
			if (playerCharacterData.ExperienceComponent.Level <= 1)
			{
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
			if (GameManager.Get().PlayerInfo != null)
			{
				flag = (GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready);
				goto IL_75;
			}
		}
		flag = false;
		IL_75:
		bool flag2 = flag;
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
		{
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
		}
	}

	public void SetSelected(bool isSelected)
	{
		this.m_button.SetSelected(isSelected, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_button.m_selectedContainer, isSelected, null);
		this.m_button.spriteController.ResetMouseState();
	}
}
