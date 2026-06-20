using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIKeyCommandEntry : MonoBehaviour
{
	public TextMeshProUGUI m_keyCommandEntry;

	public _SelectableBtn m_primaryKeyButton;

	public _SelectableBtn m_secondaryKeyButton;

	public TextMeshProUGUI[] m_primaryLabels;

	public TextMeshProUGUI[] m_secondaryLabels;

	private KeyPreference m_preference;

	public KeyPreference GetKeyPreference()
	{
		return m_preference;
	}

	private void Awake()
	{
		m_primaryKeyButton.spriteController.callback = PrimaryButtonClicked;
		m_secondaryKeyButton.spriteController.callback = SecondaryButtonClicked;
		m_keyCommandEntry.raycastTarget = false;
	}

	public void Init(KeyPreference keyPreference)
	{
		m_preference = keyPreference;
	}

	public void SetLabels(string keyCommand, string primaryKey, string secondaryKey)
	{
		m_keyCommandEntry.text = keyCommand;
		for (int i = 0; i < m_primaryLabels.Length; i++)
		{
			m_primaryLabels[i].text = primaryKey;
		}
		while (true)
		{
			for (int j = 0; j < m_secondaryLabels.Length; j++)
			{
				m_secondaryLabels[j].text = secondaryKey;
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void PrimaryButtonClicked(BaseEventData data)
	{
		if (Input.GetMouseButtonUp(0))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
					KeyBinding_UI.Get().ToggleKeyBindButton(m_preference, true);
					return;
				}
			}
		}
		if (Input.GetMouseButtonUp(1))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
			KeyBinding_UI.Get().ClearKeyBindButton(m_preference, true);
		}
	}

	public void SecondaryButtonClicked(BaseEventData data)
	{
		if (Input.GetMouseButtonUp(0))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
					KeyBinding_UI.Get().ToggleKeyBindButton(m_preference, false);
					return;
				}
			}
		}
		if (Input.GetMouseButtonUp(1))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
			KeyBinding_UI.Get().ClearKeyBindButton(m_preference, false);
		}
	}
}
