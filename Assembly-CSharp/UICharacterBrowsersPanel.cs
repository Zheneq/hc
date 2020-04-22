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
		m_characterType = characterType;
		m_closeBtn.m_soundToPlay = FrontEndButtonSounds.Close;
		SetupPanel(m_generalBrowserPanel, m_generalButton, characterType);
		SetupPanel(m_skinBrowserPanel, m_skinButton, characterType);
		SetupPanel(m_modsBrowserPanel, m_modsButton, characterType);
		SetupPanel(m_tauntBrowserPanel, m_tauntButton, characterType);
		ShowPanel(defaultPanel, characterType);
		UIEventTriggerUtils.AddListener(m_modelHitbox.gameObject, EventTriggerType.PointerEnter, HighlightCharacter);
		UIEventTriggerUtils.AddListener(m_modelHitbox.gameObject, EventTriggerType.PointerExit, UnhighlightCharacter);
	}

	private void SetupPanel(MonoBehaviour panel, _SelectableBtn button, CharacterType characterType)
	{
		if (panel == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(button == null))
			{
				m_buttons.Add(button);
				m_browserPanels.Add(panel);
				button.spriteController.callback = delegate
				{
					ShowPanel(panel, characterType);
				};
			}
			return;
		}
	}

	private void ShowPanel(MonoBehaviour panel, CharacterType characterType)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		for (int i = 0; i < m_browserPanels.Count; i++)
		{
			if (m_browserPanels[i] != panel)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				UIManager.SetGameObjectActive(m_browserPanels[i], false);
				m_buttons[i].SetSelected(false, false, string.Empty, string.Empty);
			}
			else
			{
				m_buttons[i].SetSelected(true, false, string.Empty, string.Empty);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (panel == m_skinBrowserPanel)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
				(panel as UISkinBrowserPanel).Setup(characterType, playerCharacterData.CharacterComponent.LastSkin);
			}
			else if (panel == m_generalBrowserPanel)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				(panel as UICharacterGeneralPanel).Setup(characterType);
			}
			else if (panel == m_modsBrowserPanel)
			{
				(panel as UICharacterAbilitiesPanel).Setup(characterType);
			}
			else if (panel == m_tauntBrowserPanel)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				(panel as UICharacterTauntsPanel).Setup(characterType);
			}
			UIManager.SetGameObjectActive(panel, true);
			return;
		}
	}

	public CharacterType GetCharacterType()
	{
		return m_characterType;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible);
		UICharacterStoreAndProgressWorldObjects uICharacterStoreAndProgressWorldObjects = UICharacterStoreAndProgressWorldObjects.Get();
		if (!(uICharacterStoreAndProgressWorldObjects != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (visible)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						CharacterVisualInfo lastSkin = ClientGameManager.Get().GetPlayerCharacterData(m_characterType).CharacterComponent.LastSkin;
						uICharacterStoreAndProgressWorldObjects.LoadCharacterIntoSlot(m_characterType, 0, string.Empty, lastSkin, false);
						uICharacterStoreAndProgressWorldObjects.SetVisible(true);
						return;
					}
					}
				}
			}
			uICharacterStoreAndProgressWorldObjects.SetVisible(false);
			return;
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
