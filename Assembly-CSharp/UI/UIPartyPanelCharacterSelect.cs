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
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerClick, OnButtonClicked);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerEnter, OnButtonEnter);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerExit, OnButtonExit);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerUp, OnButtonUp);
		UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerDown, OnButtonDown);
		UIManager.SetGameObjectActive(m_selectedBG, false);
		UIManager.SetGameObjectActive(m_border, false);
	}

	public CharacterResourceLink GetCharacterResourceLink()
	{
		return m_characterResourceLink;
	}

	public bool CanBePurchased()
	{
		return m_isDisabled;
	}

	public void Setup(bool isAvailable)
	{
		UIManager.SetGameObjectActive(base.gameObject, true);
		CharacterResourceLink characterResourceLink = null;
		if (m_characterType != 0)
		{
			characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_characterType);
		}
		if (characterResourceLink != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_characterResourceLink = characterResourceLink;
					UIManager.SetGameObjectActive(m_charImage, true);
					if (isAvailable)
					{
						UIManager.SetGameObjectActive(m_freeRotation, true);
						UIManager.SetGameObjectActive(m_TealBorder, true);
						UIManager.SetGameObjectActive(m_disabled, false);
						m_charImage.sprite = characterResourceLink.GetCharacterSelectIcon();
						m_isDisabled = false;
						UIManager.SetGameObjectActive(m_unavailable, false);
					}
					else
					{
						UIManager.SetGameObjectActive(m_freeRotation, false);
						UIManager.SetGameObjectActive(m_TealBorder, false);
						UIManager.SetGameObjectActive(m_disabled, true);
						UIManager.SetGameObjectActive(m_unavailable, true);
						m_isDisabled = true;
						m_charImage.sprite = characterResourceLink.GetCharacterSelectIconBW();
					}
					return;
				}
			}
		}
		m_characterResourceLink = null;
		UIManager.SetGameObjectActive(m_disabled, false);
		UIManager.SetGameObjectActive(m_freeRotation, false);
		UIManager.SetGameObjectActive(m_TealBorder, false);
		UIManager.SetGameObjectActive(m_charImage, false);
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (!m_buttonHitBox.interactable)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UICharacterScreen.Get().m_partyListPanel.SelectedBotCharacter(m_characterResourceLink);
		m_pointerDown = false;
		m_pointerEntered = false;
		m_hover.gameObject.SetActive(false);
		m_pressed.gameObject.SetActive(false);
	}

	public void SetSelected(bool isSelected)
	{
		UIManager.SetGameObjectActive(m_selectedBG, isSelected);
		UIManager.SetGameObjectActive(m_border, isSelected);
	}

	private void OnButtonEnter(BaseEventData data)
	{
		m_pointerEntered = true;
	}

	private void OnButtonExit(BaseEventData data)
	{
		m_pointerDown = false;
		m_pointerEntered = false;
	}

	private void OnButtonUp(BaseEventData data)
	{
		m_pointerDown = false;
	}

	private void OnButtonDown(BaseEventData data)
	{
		m_pointerDown = true;
	}

	private void Update()
	{
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = base.transform.localPosition;
		transform.localPosition = new Vector3(x, localPosition2.y, 0f);
		if (m_pointerDown)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_hover, false);
					UIManager.SetGameObjectActive(m_pressed, true);
					return;
				}
			}
		}
		if (m_pointerEntered)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_hover, true);
					UIManager.SetGameObjectActive(m_pressed, false);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_hover, false);
		UIManager.SetGameObjectActive(m_pressed, false);
	}
}
