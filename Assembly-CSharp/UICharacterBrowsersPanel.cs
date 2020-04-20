using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterBrowsersPanel : MonoBehaviour
{
	public _ButtonSwapSprite m_closeBtn;

	public _SelectableBtn m_generalButton;

	public UICharacterGeneralPanel m_generalBrowserPanel;

	public _SelectableBtn m_skinButton;

	public UISkinBrowserPanel m_skinBrowserPanel;

	public _SelectableBtn m_modsButton;

	public UICharacterAbilitiesPanel m_modsBrowserPanel;

	public _SelectableBtn m_tauntButton;

	public UICharacterTauntsPanel m_tauntBrowserPanel;

	public Image m_modelHitbox;

	private List<_SelectableBtn> m_buttons = new List<_SelectableBtn>();

	private List<MonoBehaviour> m_browserPanels = new List<MonoBehaviour>();

	private CharacterType m_characterType;

	public void Setup(CharacterType characterType, MonoBehaviour defaultPanel)
	{
		this.m_characterType = characterType;
		this.m_closeBtn.m_soundToPlay = FrontEndButtonSounds.Close;
		this.SetupPanel(this.m_generalBrowserPanel, this.m_generalButton, characterType);
		this.SetupPanel(this.m_skinBrowserPanel, this.m_skinButton, characterType);
		this.SetupPanel(this.m_modsBrowserPanel, this.m_modsButton, characterType);
		this.SetupPanel(this.m_tauntBrowserPanel, this.m_tauntButton, characterType);
		this.ShowPanel(defaultPanel, characterType);
		UIEventTriggerUtils.AddListener(this.m_modelHitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.HighlightCharacter));
		UIEventTriggerUtils.AddListener(this.m_modelHitbox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.UnhighlightCharacter));
	}

	private void SetupPanel(MonoBehaviour panel, _SelectableBtn button, CharacterType characterType)
	{
		if (!(panel == null))
		{
			if (!(button == null))
			{
				this.m_buttons.Add(button);
				this.m_browserPanels.Add(panel);
				button.spriteController.callback = delegate(BaseEventData data)
				{
					this.ShowPanel(panel, characterType);
				};
				return;
			}
		}
	}

	private void ShowPanel(MonoBehaviour panel, CharacterType characterType)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		for (int i = 0; i < this.m_browserPanels.Count; i++)
		{
			if (this.m_browserPanels[i] != panel)
			{
				UIManager.SetGameObjectActive(this.m_browserPanels[i], false, null);
				this.m_buttons[i].SetSelected(false, false, string.Empty, string.Empty);
			}
			else
			{
				this.m_buttons[i].SetSelected(true, false, string.Empty, string.Empty);
			}
		}
		if (panel == this.m_skinBrowserPanel)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
			(panel as UISkinBrowserPanel).Setup(characterType, playerCharacterData.CharacterComponent.LastSkin);
		}
		else if (panel == this.m_generalBrowserPanel)
		{
			(panel as UICharacterGeneralPanel).Setup(characterType);
		}
		else if (panel == this.m_modsBrowserPanel)
		{
			(panel as UICharacterAbilitiesPanel).Setup(characterType);
		}
		else if (panel == this.m_tauntBrowserPanel)
		{
			(panel as UICharacterTauntsPanel).Setup(characterType);
		}
		UIManager.SetGameObjectActive(panel, true, null);
	}

	public CharacterType GetCharacterType()
	{
		return this.m_characterType;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible, null);
		UICharacterStoreAndProgressWorldObjects uicharacterStoreAndProgressWorldObjects = UICharacterStoreAndProgressWorldObjects.Get();
		if (uicharacterStoreAndProgressWorldObjects != null)
		{
			if (visible)
			{
				CharacterVisualInfo lastSkin = ClientGameManager.Get().GetPlayerCharacterData(this.m_characterType).CharacterComponent.LastSkin;
				uicharacterStoreAndProgressWorldObjects.LoadCharacterIntoSlot(this.m_characterType, 0, string.Empty, lastSkin, false);
				uicharacterStoreAndProgressWorldObjects.SetVisible(true);
			}
			else
			{
				uicharacterStoreAndProgressWorldObjects.SetVisible(false);
			}
		}
	}

	public void HighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(true);
		}
	}

	public void UnhighlightCharacter(BaseEventData data)
	{
		UIActorModelData componentInChildren = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0].GetComponentInChildren<UIActorModelData>();
		if (componentInChildren != null)
		{
			componentInChildren.SetMouseIsOver(false);
		}
	}
}
