using System;
using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacterSelectCharacterSettingsPanel : MonoBehaviour
{
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

	protected UICharacterSelectCharacterSettingsPanel.TabPanel m_currentTab;

	protected CharacterType m_selectedCharType;

	protected virtual void Awake()
	{
		if (this.m_generalTabBtn != null)
		{
			this.m_generalTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabBtnClicked);
		}
		this.m_skinsTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabBtnClicked);
		this.m_abilitiesTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabBtnClicked);
		this.m_catalystTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabBtnClicked);
		this.m_tauntsTabBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TabBtnClicked);
		this.m_catalystTabBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
		{
			if (GameManager.Get().GameplayOverrides.EnableCards)
			{
				_ButtonSwapSprite spriteController = this.m_catalystTabBtn.spriteController;
				bool clickable;
				if (!this.m_selectedCharType.IsWillFill())
				{
					clickable = (this.m_selectedCharType != CharacterType.None);
				}
				else
				{
					clickable = false;
				}
				spriteController.SetClickable(clickable);
				this.m_catalystTabBtn.spriteController.SetForceHovercallback(false);
				this.m_catalystTabBtn.spriteController.SetForceExitCallback(false);
			}
			else
			{
				this.m_catalystTabBtn.spriteController.SetClickable(false);
				this.m_catalystTabBtn.spriteController.SetForceHovercallback(true);
				this.m_catalystTabBtn.spriteController.SetForceExitCallback(true);
			}
			if (!GameManager.Get().GameplayOverrides.EnableCards)
			{
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(StringUtil.TR("Disabled", "Global"), StringUtil.TR("CatalystsAreDisabled", "Global"), string.Empty);
				return true;
			}
			return false;
		}, null);
		this.m_closeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseClicked);
		if (this.m_buffInfoBtn != null)
		{
			UIManager.SetGameObjectActive(this.m_buffInfoBtn, true, null);
			this.m_buffInfoBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.BuffInfoMouseEnter);
			this.m_buffInfoBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.BuffInfoMouseExit);
		}
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	private void Start()
	{
		this.DoVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
	}

	public void BuffInfoMouseEnter(BaseEventData data)
	{
		if (this.m_tooltipContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_tooltipContainer, true, null);
		}
	}

	public void BuffInfoMouseExit(BaseEventData data)
	{
		if (this.m_tooltipContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_tooltipContainer, false, null);
		}
	}

	public void CloseClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectClose);
		this.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
	}

	public UICharacterSelectCharacterSettingsPanel.TabPanel GetTabPanel()
	{
		return this.m_currentTab;
	}

	protected virtual FrontEndButtonSounds GetTabClickSound()
	{
		return FrontEndButtonSounds.PlayCategorySelect;
	}

	public void TabBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(this.GetTabClickSound());
		if (this.m_generalTabBtn != null)
		{
			if (data.selectedObject == this.m_generalTabBtn.spriteController.gameObject)
			{
				this.OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.General, false);
				return;
			}
		}
		if (data.selectedObject == this.m_skinsTabBtn.spriteController.gameObject)
		{
			this.OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Skins, false);
		}
		else if (data.selectedObject == this.m_abilitiesTabBtn.spriteController.gameObject)
		{
			this.OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities, false);
		}
		else if (data.selectedObject == this.m_catalystTabBtn.spriteController.gameObject)
		{
			if (GameManager.Get().GameplayOverrides.EnableCards)
			{
				this.OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts, false);
			}
		}
		else if (data.selectedObject == this.m_tauntsTabBtn.spriteController.gameObject)
		{
			this.OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts, false);
		}
	}

	public static UICharacterSelectCharacterSettingsPanel Get()
	{
		return UICharacterSelectScreenController.Get().m_charSettingsPanel;
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		this.m_abilitiesSubPanel.NotifyLoadoutUpdate(response);
	}

	public void Refresh(CharacterResourceLink selectedCharacter, bool loadedForSelf = false, bool switchedChars = false)
	{
		if (selectedCharacter == null)
		{
			Log.Error("Called to refresh settings panel with null character", new object[0]);
			return;
		}
		LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
		if (selectedCharacter.m_characterType == groupInfo.ChararacterInfo.CharacterType)
		{
			if (!switchedChars)
			{
				if (GameManager.Get().PlayerInfo != null)
				{
					this.m_spellsSubPanel.Setup(selectedCharacter.m_characterType, GameManager.Get().PlayerInfo.CharacterInfo.CharacterCards, false, true);
				}
				else
				{
					this.m_spellsSubPanel.Setup(selectedCharacter.m_characterType, groupInfo.ChararacterInfo.CharacterCards, false, true);
				}
				this.m_abilitiesSubPanel.Setup(selectedCharacter, true);
				if (GameManager.Get().PlayerInfo != null)
				{
					this.m_skinsSubPanel.Setup(selectedCharacter, GameManager.Get().PlayerInfo.CharacterInfo.CharacterSkin, true);
				}
				else
				{
					this.m_skinsSubPanel.Setup(selectedCharacter, groupInfo.ChararacterInfo.CharacterSkin, true);
				}
				this.m_tauntsSubPanel.Setup(selectedCharacter, true);
				goto IL_29A;
			}
		}
		CharacterCardInfo cards;
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
					CharacterCardInfo characterCardInfo;
					if (playerCharacterData != null)
					{
						if (playerCharacterData.CharacterComponent != null)
						{
							characterCardInfo = playerCharacterData.CharacterComponent.LastCards;
							goto IL_102;
						}
					}
					characterCardInfo = default(CharacterCardInfo);
					IL_102:
					cards = characterCardInfo;
					goto IL_10D;
				}
			}
			cards = default(CharacterCardInfo);
		}
		IL_10D:
		this.m_spellsSubPanel.Setup(selectedCharacter.m_characterType, cards, loadedForSelf, false);
		this.m_abilitiesSubPanel.Setup(selectedCharacter, false);
		if (this.m_skinsSubPanel.GetDisplayedCharacterType() != selectedCharacter.m_characterType)
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
			this.m_skinsSubPanel.Setup(selectedCharacter, visualInfo, false);
		}
		this.m_tauntsSubPanel.Setup(selectedCharacter, false);
		IL_29A:
		if (this.m_generalSubPanel != null)
		{
			this.m_generalSubPanel.Setup(selectedCharacter);
		}
		if (selectedCharacter.m_characterType.IsWillFill())
		{
			if (this.m_generalTabBtn != null)
			{
				this.m_generalTabBtn.SetDisabled(true);
			}
			this.m_skinsTabBtn.SetDisabled(false);
			this.m_abilitiesTabBtn.SetDisabled(true);
			this.m_catalystTabBtn.SetDisabled(true);
			this.m_tauntsTabBtn.SetDisabled(true);
		}
		else
		{
			if (this.m_generalTabBtn != null)
			{
				this.m_generalTabBtn.SetDisabled(false);
			}
			this.m_skinsTabBtn.SetDisabled(false);
			this.m_abilitiesTabBtn.SetDisabled(false);
			this.m_catalystTabBtn.SetDisabled(false);
			this.m_tauntsTabBtn.SetDisabled(false);
		}
		this.m_selectedCharType = selectedCharacter.m_characterType;
	}

	public void OpenTab(UICharacterSelectCharacterSettingsPanel.TabPanel panel, bool instantCloseOthers = false)
	{
		bool flag = panel == UICharacterSelectCharacterSettingsPanel.TabPanel.General;
		bool flag2 = panel == UICharacterSelectCharacterSettingsPanel.TabPanel.Skins;
		bool flag3 = panel == UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities;
		bool flag4 = panel == UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts;
		bool flag5 = panel == UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts;
		this.m_currentTab = panel;
		if (this.m_generalTabBtn != null)
		{
			this.m_generalTabBtn.SetSelected(flag, false, string.Empty, string.Empty);
		}
		this.m_skinsTabBtn.SetSelected(flag2, false, string.Empty, string.Empty);
		this.m_abilitiesTabBtn.SetSelected(flag3, false, string.Empty, string.Empty);
		this.m_catalystTabBtn.SetSelected(flag4, false, string.Empty, string.Empty);
		this.m_tauntsTabBtn.SetSelected(flag5, false, string.Empty, string.Empty);
		if (this.m_generalSubPanel != null)
		{
			this.m_generalSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag;
			this.m_generalSubPanel.GetComponent<CanvasGroup>().interactable = flag;
		}
		this.m_skinsSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag2;
		this.m_skinsSubPanel.GetComponent<CanvasGroup>().interactable = flag2;
		this.m_abilitiesSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag3;
		this.m_abilitiesSubPanel.GetComponent<CanvasGroup>().interactable = flag3;
		this.m_spellsSubPanel.GetComponent<CanvasGroup>().blocksRaycasts = flag4;
		this.m_spellsSubPanel.GetComponent<CanvasGroup>().interactable = flag4;
		this.m_tauntsSubPanel.GetComponent<CanvasGroup>().interactable = flag5;
		this.m_tauntsSubPanel.GetComponent<CanvasGroup>().interactable = flag5;
		if (this.m_GeneralAnimator != null)
		{
			if (flag)
			{
				UIManager.SetGameObjectActive(this.m_GeneralAnimator, true, null);
			}
			else if (instantCloseOthers)
			{
				UIManager.SetGameObjectActive(this.m_GeneralAnimator, false, null);
			}
			else
			{
				this.m_GeneralAnimator.Play("GeneralContentOUT");
			}
		}
		if (flag2)
		{
			UIManager.SetGameObjectActive(this.m_SkinsAnimator, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_skinsSubPanel.m_purchasePanel, false, null);
			if (instantCloseOthers)
			{
				UIManager.SetGameObjectActive(this.m_SkinsAnimator, false, null);
			}
			else
			{
				this.m_SkinsAnimator.Play("SkinsContentOUT");
			}
		}
		if (flag3)
		{
			UIManager.SetGameObjectActive(this.m_AbilitiesAnimator, true, null);
		}
		else if (instantCloseOthers)
		{
			UIManager.SetGameObjectActive(this.m_AbilitiesAnimator, false, null);
		}
		else
		{
			this.m_AbilitiesAnimator.Play("AbilitiesContentOUT");
		}
		if (flag4)
		{
			UIManager.SetGameObjectActive(this.m_CatalystAnimator, true, null);
		}
		else if (instantCloseOthers)
		{
			UIManager.SetGameObjectActive(this.m_CatalystAnimator, false, null);
		}
		else
		{
			this.m_CatalystAnimator.Play("CatalystContentOUT");
		}
		if (flag5)
		{
			UIManager.SetGameObjectActive(this.m_TauntAnimator, true, null);
		}
		else if (instantCloseOthers)
		{
			UIManager.SetGameObjectActive(this.m_TauntAnimator, false, null);
		}
		else
		{
			this.m_TauntAnimator.Play("AbilitiesContentOUT");
		}
	}

	protected virtual void DoVisible(bool visible, UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None)
	{
		this.m_isVisible = visible;
		if (visible)
		{
			UICharacterSelectScreenController.Get().SetCharacterSelectVisible(false);
			UICharacterSelectScreenController.Get().PlayMainLobbyControllerAnim("LobbyPanelToggleOnIN", 1);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SideButtonsClickable = new bool?(false)
			});
			this.OpenTab(tab, true);
		}
		else
		{
			UICharacterSelectScreenController.Get().PlayMainLobbyControllerAnim("LobbyPanelToggleOffIN", 1);
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				SideButtonsClickable = new bool?(true)
			});
			UIManager.SetGameObjectActive(this.m_skinsSubPanel, false, null);
		}
		if (this.m_tooltipContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_tooltipContainer, false, null);
		}
		this.m_CanvasGroup.interactable = visible;
		this.m_CanvasGroup.blocksRaycasts = visible;
	}

	public void SetVisible(bool visible, UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None)
	{
		if (this.m_isVisible != visible)
		{
			this.DoVisible(visible, tab);
		}
		else if (visible && tab != this.m_currentTab)
		{
			this.OpenTab(tab, false);
		}
	}

	public enum TabPanel
	{
		None,
		Skins,
		Abilities,
		Catalysts,
		Taunts,
		General
	}
}
