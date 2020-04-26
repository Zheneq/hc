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
		UIEventTriggerUtils.AddListener(m_button.spriteController.gameObject, EventTriggerType.PointerClick, OnButtonClicked);
	}

	public void UpdateFreeRotationIcon()
	{
		if (!(m_characterResourceLink != null))
		{
			return;
		}
		while (true)
		{
			bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(m_characterResourceLink.m_characterType, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
			UIManager.SetGameObjectActive(m_freeRotation, doActive);
			return;
		}
	}

	protected virtual FrontEndButtonSounds SoundToPlayOnClick()
	{
		return FrontEndButtonSounds.CharacterSelectModAdd;
	}

	public void SetClickable(bool clickable)
	{
		m_button.spriteController.SetClickable(clickable);
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return m_characterResourceLink;
	}

	public bool IsDisabled()
	{
		return m_isDisabled;
	}

	public virtual void Setup(bool isAvailable, bool selected = false)
	{
		int num;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			num = ((ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Practice) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		UIManager.SetGameObjectActive(base.gameObject, true);
		SetSelected(selected);
		CharacterResourceLink characterResourceLink = null;
		if (m_characterType != 0)
		{
			if (GameManager.Get() != null && GameManager.Get().IsCharacterVisible(m_characterType))
			{
				characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_characterType);
			}
		}
		if (characterResourceLink != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (!m_isTooltipInitialized)
					{
						UITooltipHoverObject component = m_button.spriteController.GetComponent<UITooltipHoverObject>();
						if (characterResourceLink.m_characterType.IsWillFill())
						{
							component.Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
							{
								if (m_button.spriteController.IsClickable())
								{
									if (!(m_characterResourceLink == null))
									{
										UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
										uITitledTooltip.Setup(m_characterResourceLink.GetDisplayName(), m_characterResourceLink.GetCharSelectTooltipDescription(), string.Empty);
										return true;
									}
								}
								return false;
							});
						}
						else
						{
							component.Setup(TooltipType.Character, delegate(UITooltipBase tooltip)
							{
								if (m_button.spriteController.IsClickable())
								{
									if (!(m_characterResourceLink == null))
									{
										UICharacterTooltip uICharacterTooltip = tooltip as UICharacterTooltip;
										uICharacterTooltip.Setup(m_characterResourceLink, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
										return true;
									}
								}
								return false;
							});
						}
						m_isTooltipInitialized = true;
					}
					m_characterResourceLink = characterResourceLink;
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterResourceLink.m_characterType);
					int enabled;
					if (!isAvailable)
					{
						enabled = (flag ? 1 : 0);
					}
					else
					{
						enabled = 1;
					}
					SetEnabled((byte)enabled != 0, playerCharacterData);
					bool doActive = ClientGameManager.Get().IsCharacterInFreeRotation(m_characterResourceLink.m_characterType, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay);
					UIManager.SetGameObjectActive(m_freeRotation, doActive);
					if (playerCharacterData != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
							{
								int level = playerCharacterData.ExperienceComponent.Level;
								string text = level.ToString();
								for (int i = 0; i < m_levelTextLabels.Length; i++)
								{
									m_levelTextLabels[i].text = text;
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
									{
										if (level < GameBalanceVars.Get().MaxCharacterLevel)
										{
											float fillAmount = (float)playerCharacterData.ExperienceComponent.XPProgressThroughLevel / (float)GameBalanceVars.Get().CharacterExperienceToLevel(level);
											m_unavailableBar.fillAmount = fillAmount;
											m_normalBar.fillAmount = fillAmount;
											m_masterBar.fillAmount = fillAmount;
										}
										else
										{
											m_isMaster = true;
											m_unavailableBar.fillAmount = 1f;
											m_normalBar.fillAmount = 1f;
											m_masterBar.fillAmount = 1f;
										}
										RectTransform masterExpBarContainer = m_MasterExpBarContainer;
										int doActive2;
										if (!m_isDisabled)
										{
											doActive2 = (m_isMaster ? 1 : 0);
										}
										else
										{
											doActive2 = 0;
										}
										UIManager.SetGameObjectActive(masterExpBarContainer, (byte)doActive2 != 0);
										return;
									}
									}
								}
							}
							}
						}
					}
					return;
				}
				}
			}
		}
		m_characterResourceLink = null;
		UIManager.SetGameObjectActive(m_freeRotation, false);
		UIManager.SetGameObjectActive(m_LockedContainer, false);
		UIManager.SetGameObjectActive(m_DisabledContainer, false);
		UIManager.SetGameObjectActive(m_AvailableContainer, false);
		UIManager.SetGameObjectActive(m_NormalExpBarContainer, false);
		UIManager.SetGameObjectActive(m_UnavailableExpBarContainer, false);
		UIManager.SetGameObjectActive(m_MasterExpBarContainer, false);
		SetEnabled(false, null);
	}

	public virtual void SetEnabled(bool enabled, PersistedCharacterData playerCharacterData)
	{
		if (m_characterResourceLink != null)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
			m_charImageAvailable.sprite = m_characterResourceLink.GetCharacterSelectIcon();
			m_charImageLocked.sprite = m_characterResourceLink.GetCharacterSelectIconBW();
			m_charImageDisabled.sprite = m_characterResourceLink.GetCharacterSelectIconBW();
			bool flag = false;
			bool flag2 = false;
			CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(m_characterResourceLink.m_characterType);
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
			flag2 = !GameManager.Get().IsValidForHumanPreGameSelection(m_characterResourceLink.m_characterType);
			RectTransform disabledContainer = m_DisabledContainer;
			int doActive;
			if (!enabled)
			{
				if (!flag)
				{
					doActive = (flag2 ? 1 : 0);
				}
				else
				{
					doActive = 1;
				}
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(disabledContainer, (byte)doActive != 0);
		}
		else
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
		}
		m_isDisabled = !enabled;
		UIManager.SetGameObjectActive(m_LockedContainer, !enabled);
		RectTransform availableContainer = m_AvailableContainer;
		int doActive2;
		if (enabled)
		{
			doActive2 = ((m_characterResourceLink != null) ? 1 : 0);
		}
		else
		{
			doActive2 = 0;
		}
		UIManager.SetGameObjectActive(availableContainer, (byte)doActive2 != 0);
		RectTransform masterExpBarContainer = m_MasterExpBarContainer;
		int doActive3;
		if (!m_isDisabled)
		{
			if (m_isMaster)
			{
				doActive3 = ((m_characterResourceLink != null) ? 1 : 0);
				goto IL_01b6;
			}
		}
		doActive3 = 0;
		goto IL_01b6;
		IL_01b6:
		UIManager.SetGameObjectActive(masterExpBarContainer, (byte)doActive3 != 0);
		int num;
		if (playerCharacterData != null)
		{
			if (playerCharacterData.ExperienceComponent.Level <= 1)
			{
				num = ((playerCharacterData.ExperienceComponent.XPProgressThroughLevel > 0) ? 1 : 0);
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
		bool flag3 = (byte)num != 0;
		RectTransform normalExpBarContainer = m_NormalExpBarContainer;
		int doActive4;
		if (!m_isDisabled)
		{
			doActive4 = (flag3 ? 1 : 0);
		}
		else
		{
			doActive4 = 0;
		}
		UIManager.SetGameObjectActive(normalExpBarContainer, (byte)doActive4 != 0);
		RectTransform unavailableExpBarContainer = m_UnavailableExpBarContainer;
		int doActive5;
		if (m_isDisabled)
		{
			doActive5 = (flag3 ? 1 : 0);
		}
		else
		{
			doActive5 = 0;
		}
		UIManager.SetGameObjectActive(unavailableExpBarContainer, (byte)doActive5 != 0);
	}

	protected virtual void OnButtonClicked(BaseEventData data)
	{
		if (!m_button.spriteController.IsClickable())
		{
			return;
		}
		UIFrontEnd.PlaySound(SoundToPlayOnClick());
		int num;
		if (ClientGameManager.Get() != null)
		{
			if (GameManager.Get().PlayerInfo != null)
			{
				num = ((GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready) ? 1 : 0);
				goto IL_0075;
			}
		}
		num = 0;
		goto IL_0075;
		IL_0075:
		bool flag = (byte)num != 0;
		if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
		{
			flag = AppState_GroupCharacterSelect.Get().IsReady();
		}
		if (!m_button.spriteController.IsClickable())
		{
			return;
		}
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestToServerSelectCharacter = m_characterResourceLink.m_characterType
		});
	}

	public void SetSelected(bool isSelected)
	{
		m_button.SetSelected(isSelected, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_button.m_selectedContainer, isSelected);
		m_button.spriteController.ResetMouseState();
	}
}
