using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartyPanelCharacterSelect : MonoBehaviour
{
	public Image m_selectedBG;

	public Image m_charImage;

	public Image m_hover;

	public Image m_pressed;

	public Image m_disabled;

	public Image m_border;

	public Button m_buttonHitBox;

	public Image m_freeRotation;

	public Image m_TealBorder;

	public Image m_unavailable;

	public CharacterType m_characterType;

	private bool m_isDisabled;

	private bool m_pointerDown;

	private bool m_pointerEntered;

	private CharacterResourceLink m_characterResourceLink;

	private void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnButtonClicked));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnButtonEnter));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnButtonExit));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.OnButtonUp));
		UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnButtonDown));
		UIManager.SetGameObjectActive(this.m_selectedBG, false, null);
		UIManager.SetGameObjectActive(this.m_border, false, null);
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return this.m_characterResourceLink;
	}

	public bool CanBePurchased()
	{
		return this.m_isDisabled;
	}

	public void Setup(bool isAvailable)
	{
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		CharacterResourceLink characterResourceLink = null;
		if (this.m_characterType != CharacterType.None)
		{
			characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_characterType);
		}
		if (characterResourceLink != null)
		{
			this.m_characterResourceLink = characterResourceLink;
			UIManager.SetGameObjectActive(this.m_charImage, true, null);
			if (isAvailable)
			{
				UIManager.SetGameObjectActive(this.m_freeRotation, true, null);
				UIManager.SetGameObjectActive(this.m_TealBorder, true, null);
				UIManager.SetGameObjectActive(this.m_disabled, false, null);
				this.m_charImage.sprite = characterResourceLink.GetCharacterSelectIcon();
				this.m_isDisabled = false;
				UIManager.SetGameObjectActive(this.m_unavailable, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_freeRotation, false, null);
				UIManager.SetGameObjectActive(this.m_TealBorder, false, null);
				UIManager.SetGameObjectActive(this.m_disabled, true, null);
				UIManager.SetGameObjectActive(this.m_unavailable, true, null);
				this.m_isDisabled = true;
				this.m_charImage.sprite = characterResourceLink.GetCharacterSelectIconBW();
			}
		}
		else
		{
			this.m_characterResourceLink = null;
			UIManager.SetGameObjectActive(this.m_disabled, false, null);
			UIManager.SetGameObjectActive(this.m_freeRotation, false, null);
			UIManager.SetGameObjectActive(this.m_TealBorder, false, null);
			UIManager.SetGameObjectActive(this.m_charImage, false, null);
		}
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (!this.m_buttonHitBox.interactable)
		{
			return;
		}
		UICharacterScreen.Get().m_partyListPanel.SelectedBotCharacter(this.m_characterResourceLink);
		this.m_pointerDown = false;
		this.m_pointerEntered = false;
		this.m_hover.gameObject.SetActive(false);
		this.m_pressed.gameObject.SetActive(false);
	}

	public void SetSelected(bool isSelected)
	{
		UIManager.SetGameObjectActive(this.m_selectedBG, isSelected, null);
		UIManager.SetGameObjectActive(this.m_border, isSelected, null);
	}

	private void OnButtonEnter(BaseEventData data)
	{
		this.m_pointerEntered = true;
	}

	private void OnButtonExit(BaseEventData data)
	{
		this.m_pointerDown = false;
		this.m_pointerEntered = false;
	}

	private void OnButtonUp(BaseEventData data)
	{
		this.m_pointerDown = false;
	}

	private void OnButtonDown(BaseEventData data)
	{
		this.m_pointerDown = true;
	}

	private void Update()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
		if (this.m_pointerDown)
		{
			UIManager.SetGameObjectActive(this.m_hover, false, null);
			UIManager.SetGameObjectActive(this.m_pressed, true, null);
		}
		else if (this.m_pointerEntered)
		{
			UIManager.SetGameObjectActive(this.m_hover, true, null);
			UIManager.SetGameObjectActive(this.m_pressed, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_hover, false, null);
			UIManager.SetGameObjectActive(this.m_pressed, false, null);
		}
	}
}
