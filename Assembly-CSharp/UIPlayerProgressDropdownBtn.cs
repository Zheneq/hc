using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressDropdownBtn : MonoBehaviour
{
	public _SelectableBtn m_button;

	public TextMeshProUGUI[] m_texts;

	public Image m_freelancerImage;

	public Image m_allFreelancerImage;

	private int m_typeSpecificData;

	private void OnDisable()
	{
		this.m_button.spriteController.ResetMouseState();
	}

	public int GetOptionData()
	{
		return this.m_typeSpecificData;
	}

	public void SetOptionData(int typeSpecificData)
	{
		this.m_typeSpecificData = typeSpecificData;
	}

	public void AttachToDropdown(UIPlayerProgressDropdownList dropdown)
	{
		this.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			dropdown.OnSelect(this.m_typeSpecificData);
		};
	}

	public void Setup(string text, CharacterType charType = CharacterType.None)
	{
		for (int i = 0; i < this.m_texts.Length; i++)
		{
			this.m_texts[i].text = text;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressDropdownBtn.Setup(string, CharacterType)).MethodHandle;
		}
		if (this.m_allFreelancerImage != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_allFreelancerImage, charType == CharacterType.None, null);
		}
		if (this.m_freelancerImage != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_freelancerImage, charType != CharacterType.None, null);
			if (charType != CharacterType.None)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
				this.m_freelancerImage.sprite = Resources.Load<Sprite>(characterResourceLink.m_characterSelectIconResourceString);
			}
		}
	}

	public void Setup(string text, CharacterRole charRole)
	{
		for (int i = 0; i < this.m_texts.Length; i++)
		{
			this.m_texts[i].text = text;
		}
		if (this.m_allFreelancerImage != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressDropdownBtn.Setup(string, CharacterRole)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_allFreelancerImage, false, null);
		}
		if (this.m_freelancerImage != null)
		{
			UIManager.SetGameObjectActive(this.m_freelancerImage, charRole != CharacterRole.None, null);
			this.m_freelancerImage.sprite = CharacterResourceLink.GetCharacterRoleSprite(charRole);
		}
	}

	public void CheckDisplayState(UIPlayerProgressDropdownBtn.ShouldShow shouldShowFunction)
	{
		UIManager.SetGameObjectActive(base.gameObject, shouldShowFunction(this.m_typeSpecificData), null);
	}

	public void SetSelectedIfEqual(int selectedValue)
	{
		this.m_button.SetSelected(this.m_typeSpecificData == selectedValue, false, string.Empty, string.Empty);
	}

	public bool IsOption(int typeSpecificValue)
	{
		return this.m_typeSpecificData == typeSpecificValue;
	}

	public delegate bool ShouldShow(int typeSpecificData);
}
