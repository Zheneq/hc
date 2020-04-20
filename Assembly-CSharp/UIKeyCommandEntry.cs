using System;
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
		return this.m_preference;
	}

	private void Awake()
	{
		this.m_primaryKeyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PrimaryButtonClicked);
		this.m_secondaryKeyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SecondaryButtonClicked);
		this.m_keyCommandEntry.raycastTarget = false;
	}

	public void Init(KeyPreference keyPreference)
	{
		this.m_preference = keyPreference;
	}

	public void SetLabels(string keyCommand, string primaryKey, string secondaryKey)
	{
		this.m_keyCommandEntry.text = keyCommand;
		for (int i = 0; i < this.m_primaryLabels.Length; i++)
		{
			this.m_primaryLabels[i].text = primaryKey;
		}
		for (int j = 0; j < this.m_secondaryLabels.Length; j++)
		{
			this.m_secondaryLabels[j].text = secondaryKey;
		}
	}

	public void PrimaryButtonClicked(BaseEventData data)
	{
		if (Input.GetMouseButtonUp(0))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
			KeyBinding_UI.Get().ToggleKeyBindButton(this.m_preference, true);
		}
		else if (Input.GetMouseButtonUp(1))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
			KeyBinding_UI.Get().ClearKeyBindButton(this.m_preference, true);
		}
	}

	public void SecondaryButtonClicked(BaseEventData data)
	{
		if (Input.GetMouseButtonUp(0))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsChoice);
			KeyBinding_UI.Get().ToggleKeyBindButton(this.m_preference, false);
		}
		else if (Input.GetMouseButtonUp(1))
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.OptionsCancel);
			KeyBinding_UI.Get().ClearKeyBindButton(this.m_preference, false);
		}
	}
}
