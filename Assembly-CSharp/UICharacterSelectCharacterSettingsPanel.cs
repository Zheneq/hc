using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacterSelectCharacterSettingsPanel : MonoBehaviour
{
	public enum TabPanel
	{
		None,
		Skins,
		Abilities,
		Catalysts,
		Taunts,
		General
	}

	public UICharacterGeneralPanel m_generalSubPanel;

	public UICharacterSelectSpellsPanel m_spellsSubPanel;

	public UICharacterAbilitiesPanel m_abilitiesSubPanel;

	public UISkinBrowserPanel m_skinsSubPanel;

	public UICharacterTauntsPanel m_tauntsSubPanel;

	public _SelectableBtn m_closeBtn;

	public _SelectableBtn m_generalTabBtn;

	public _SelectableBtn m_skinsTabBtn;

	public _SelectableBtn m_abilitiesTabBtn;

	public _SelectableBtn m_catalystTabBtn;

	public _SelectableBtn m_tauntsTabBtn;

	public Animator m_GeneralAnimator;

	public Animator m_SkinsAnimator;

	public Animator m_AbilitiesAnimator;

	public Animator m_CatalystAnimator;

	public Animator m_TauntAnimator;

	public CanvasGroup m_CanvasGroup;

	public _SelectableBtn m_buffInfoBtn;

	public StatusEffectTooltip m_tooltipContainer;

	protected bool m_isVisible;

	protected TabPanel m_currentTab;

	protected CharacterType m_selectedCharType;

	protected virtual void Awake()
	{
		if (m_generalTabBtn != null)
		{
			m_generalTabBtn.spriteController.callback = TabBtnClicked;
		}
		m_skinsTabBtn.spriteController.callback = TabBtnClicked;
		m_abilitiesTabBtn.spriteController.callback = TabBtnClicked;
		m_catalystTabBtn.spriteController.callback = TabBtnClicked;
		m_tauntsTabBtn.spriteController.callback = TabBtnClicked;
		m_catalystTabBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
		{
			if (GameManager.Get().GameplayOverrides.EnableCards)
			{
				_ButtonSwapSprite spriteController = m_catalystTabBtn.spriteController;
				int clickable;
				if (!m_selectedCharType.IsWillFill())
				{
					clickable = ((m_selectedCharType != CharacterType.None) ? 1 : 0);
				}
				else
				{
					clickable = 0;
				}
				spriteController.SetClickable((byte)clickable != 0);
				m_catalystTabBtn.spriteController.SetForceHovercallback(false);
				m_catalystTabBtn.spriteController.SetForceExitCallback(false);
			}
			else
			{
				m_catalystTabBtn.spriteController.SetClickable(false);
				m_catalystTabBtn.spriteController.SetForceHovercallback(true);
				m_catalystTabBtn.spriteController.SetForceExitCallback(true);
			}
			if (!GameManager.Get().GameplayOverrides.EnableCards)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						uITitledTooltip.Setup(StringUtil.TR("Disabled", "Global"), StringUtil.TR("CatalystsAreDisabled", "Global"), string.Empty);
						return true;
					}
					}
				}
			}
			return false;
		});
		m_closeBtn.spriteController.callback = CloseClicked;
		if (!(m_buffInfoBtn != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_buffInfoBtn, true);
			m_buffInfoBtn.spriteController.pointerEnterCallback = BuffInfoMouseEnter;
			m_buffInfoBtn.spriteController.pointerExitCallback = BuffInfoMouseExit;
			return;
		}
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	private void Start()
	{
		DoVisible(false);
	}

	public void BuffInfoMouseEnter(BaseEventData data)
	{
		if (m_tooltipContainer != null)
		{
			UIManager.SetGameObjectActive(m_tooltipContainer, true);
		}
	}

	public void BuffInfoMouseExit(BaseEventData data)
	{
		if (!(m_tooltipContainer != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_tooltipContainer, false);
			return;
		}
	}

	public void CloseClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectClose);
		SetVisible(false);
	}

	public TabPanel GetTabPanel()
	{
		return m_currentTab;
	}

	protected virtual FrontEndButtonSounds GetTabClickSound()
	{
		return FrontEndButtonSounds.PlayCategorySelect;
	}

	public void TabBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(GetTabClickSound());
		if (m_generalTabBtn != null)
		{
			if (data.selectedObject == m_generalTabBtn.spriteController.gameObject)
			{
				OpenTab(TabPanel.General);
				return;
			}
		}
		if (data.selectedObject == m_skinsTabBtn.spriteController.gameObject)
		{
			OpenTab(TabPanel.Skins);
			return;
		}
		if (data.selectedObject == m_abilitiesTabBtn.spriteController.gameObject)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					OpenTab(TabPanel.Abilities);
					return;
				}
			}
		}
		if (data.selectedObject == m_catalystTabBtn.spriteController.gameObject)
		{
			if (!GameManager.Get().GameplayOverrides.EnableCards)
			{
				return;
			}
			while (true)
			{
				OpenTab(TabPanel.Catalysts);
				return;
			}
		}
		if (!(data.selectedObject == m_tauntsTabBtn.spriteController.gameObject))
		{
			return;
		}
		while (true)
		{
			OpenTab(TabPanel.Taunts);
			return;
		}
	}

	public static UICharacterSelectCharacterSettingsPanel Get()
	{
		return UICharacterSelectScreenController.Get().m_charSettingsPanel;
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		m_abilitiesSubPanel.NotifyLoadoutUpdate(response);
	}

	public void Refresh(CharacterResourceLink selectedCharacter, bool loadedForSelf = false, bool switchedChars = false)
	{
		if (selectedCharacter == null)
		{
			while (true)
			{
				Log.Error("Called to refresh settings panel with null character");
				return;
			}
		}
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		if (selectedCharacter.m_characterType == groupInfo.ChararacterInfo.CharacterType)
		{
			if (!switchedChars)
			{
				if (GameManager.Get().PlayerInfo != null)
				{
					m_spellsSubPanel.Setup(selectedCharacter.m_characterType, GameManager.Get().PlayerInfo.CharacterInfo.CharacterCards, false, true);
				}
				else
				{
					m_spellsSubPanel.Setup(selectedCharacter.m_characterType, groupInfo.ChararacterInfo.CharacterCards, false, true);
				}
				m_abilitiesSubPanel.Setup(selectedCharacter, true);
				if (GameManager.Get().PlayerInfo != null)
				{
					m_skinsSubPanel.Setup(selectedCharacter, GameManager.Get().PlayerInfo.CharacterInfo.CharacterSkin, true);
				}
				else
				{
					m_skinsSubPanel.Setup(selectedCharacter, groupInfo.ChararacterInfo.CharacterSkin, true);
				}
				m_tauntsSubPanel.Setup(selectedCharacter, true);
				goto IL_029a;
			}
		}
		CharacterCardInfo cards;
		CharacterCardInfo obj;
		if (GameManager.Get().PlayerInfo != null)
		{
			cards = GameManager.Get().PlayerInfo.CharacterInfo.CharacterCards;
		}
		else
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(selectedCharacter.m_characterType);
					if (playerCharacterData != null)
					{
						if (playerCharacterData.CharacterComponent != null)
						{
							obj = playerCharacterData.CharacterComponent.LastCards;
							goto IL_0102;
						}
					}
					obj = default(CharacterCardInfo);
					goto IL_0102;
				}
			}
			cards = default(CharacterCardInfo);
		}
		goto IL_010d;
		IL_010d:
		m_spellsSubPanel.Setup(selectedCharacter.m_characterType, cards, loadedForSelf);
		m_abilitiesSubPanel.Setup(selectedCharacter);
		if (m_skinsSubPanel.GetDisplayedCharacterType() != selectedCharacter.m_characterType)
		{
			CharacterVisualInfo visualInfo;
			if (GameManager.Get().PlayerInfo != null)
			{
				visualInfo = GameManager.Get().PlayerInfo.CharacterInfo.CharacterSkin;
			}
			else if (groupInfo.InAGroup)
			{
				visualInfo = groupInfo.ChararacterInfo.CharacterSkin;
			}
			else
			{
				visualInfo = UICharacterScreen.GetCurrentSpecificState().CharacterVisualInfoToDisplay;
			}
			m_skinsSubPanel.Setup(selectedCharacter, visualInfo);
		}
		m_tauntsSubPanel.Setup(selectedCharacter);
		goto IL_029a;
		IL_029a:
		if (m_generalSubPanel != null)
		{
			m_generalSubPanel.Setup(selectedCharacter);
		}
		if (selectedCharacter.m_characterType.IsWillFill())
		{
			if (m_generalTabBtn != null)
			{
				m_generalTabBtn.SetDisabled(true);
			}
			m_skinsTabBtn.SetDisabled(false);
			m_abilitiesTabBtn.SetDisabled(true);
			m_catalystTabBtn.SetDisabled(true);
			m_tauntsTabBtn.SetDisabled(true);
		}
		else
		{
			if (m_generalTabBtn != null)
			{
				m_generalTabBtn.SetDisabled(false);
			}
			m_skinsTabBtn.SetDisabled(false);
			m_abilitiesTabBtn.SetDisabled(false);
			m_catalystTabBtn.SetDisabled(false);
			m_tauntsTabBtn.SetDisabled(false);
		}
		m_selectedCharType = selectedCharacter.m_characterType;
		return;
		IL_0102:
		cards = obj;
		goto IL_010d;
	}

	public void OpenTab(TabPanel panel, bool instantCloseOthers = false)
	{
		bool flag = panel == TabPanel.General;
		bool flag2 = panel == TabPanel.Skins;
		bool flag3 = panel == TabPanel.Abilities;
		bool flag4 = panel == TabPanel.Catalysts;
		bool flag5 = panel == TabPanel.Taunts;
		m_currentTab = panel;
		if (m_generalTabBtn != null)
		{
			m_generalTabBtn.SetSelected(flag, false, string.Empty, string.Empty);
		}
		m_skinsTabBtn.SetSelected(flag2, false, string.Empty, string.Empty);
		m_abilitiesTabBtn.SetSelected(flag3, false, string.Empty, string.Empty);
		m_catalystTabBtn.SetSelected(flag4, false, string.Empty, string.Empty);
		m_tauntsTabBtn.SetSelected(flag5, false, string.Empty, string.Empty);
		if (m_generalSubPanel != null)
		{
			m_generalSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag;
			m_generalSubPanel.GetComponent<CanvasGroup>().interactable = flag;
		}
		m_skinsSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag2;
		m_skinsSubPanel.GetComponent<CanvasGroup>().interactable = flag2;
		m_abilitiesSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag3;
		m_abilitiesSubPanel.GetComponent<CanvasGroup>().interactable = flag3;
		m_spellsSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag4;
		m_spellsSubPanel.GetComponent<CanvasGroup>().interactable = flag4;
		m_tauntsSubPanel.GetComponent<CanvasGroup>().interactable = flag5;
		m_tauntsSubPanel.GetComponent<CanvasGroup>().interactable = flag5;
		if (m_GeneralAnimator != null)
		{
			if (flag)
			{
				UIManager.SetGameObjectActive(m_GeneralAnimator, true);
			}
			else if (instantCloseOthers)
			{
				UIManager.SetGameObjectActive(m_GeneralAnimator, false);
			}
			else
			{
				m_GeneralAnimator.Play("GeneralContentOUT");
			}
		}
		if (flag2)
		{
			UIManager.SetGameObjectActive(m_SkinsAnimator, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_skinsSubPanel.m_purchasePanel, false);
			if (instantCloseOthers)
			{
				UIManager.SetGameObjectActive(m_SkinsAnimator, false);
			}
			else
			{
				m_SkinsAnimator.Play("SkinsContentOUT");
			}
		}
		if (flag3)
		{
			UIManager.SetGameObjectActive(m_AbilitiesAnimator, true);
		}
		else if (instantCloseOthers)
		{
			UIManager.SetGameObjectActive(m_AbilitiesAnimator, false);
		}
		else
		{
			m_AbilitiesAnimator.Play("AbilitiesContentOUT");
		}
		if (flag4)
		{
			UIManager.SetGameObjectActive(m_CatalystAnimator, true);
		}
		else if (instantCloseOthers)
		{
			UIManager.SetGameObjectActive(m_CatalystAnimator, false);
		}
		else
		{
			m_CatalystAnimator.Play("CatalystContentOUT");
		}
		if (flag5)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_TauntAnimator, true);
					return;
				}
			}
		}
		if (instantCloseOthers)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_TauntAnimator, false);
					return;
				}
			}
		}
		m_TauntAnimator.Play("AbilitiesContentOUT");
	}

	protected virtual void DoVisible(bool visible, TabPanel tab = TabPanel.None)
	{
		m_isVisible = visible;
		if (visible)
		{
			UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
			UICharacterSelectScreenController.Get().PlayMainLobbyControllerAnim("LobbyPanelToggleOnIN", 1);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SideButtonsClickable = false
			});
			OpenTab(tab, true);
		}
		else
		{
			UICharacterSelectScreenController.Get().PlayMainLobbyControllerAnim("LobbyPanelToggleOffIN", 1);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SideButtonsClickable = true
			});
			UIManager.SetGameObjectActive(m_skinsSubPanel, false);
		}
		if (m_tooltipContainer != null)
		{
			UIManager.SetGameObjectActive(m_tooltipContainer, false);
		}
		m_CanvasGroup.interactable = visible;
		m_CanvasGroup.blocksRaycasts = visible;
	}

	public void SetVisible(bool visible, TabPanel tab = TabPanel.None)
	{
		if (m_isVisible != visible)
		{
			DoVisible(visible, tab);
		}
		else
		{
			if (!visible || tab == m_currentTab)
			{
				return;
			}
			while (true)
			{
				OpenTab(tab);
				return;
			}
		}
	}
}
