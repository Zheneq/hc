using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressDropdownBtn : MonoBehaviour
{
	public delegate bool ShouldShow(int typeSpecificData);

	public _SelectableBtn m_button;

	public TextMeshProUGUI[] m_texts;

	public Image m_freelancerImage;

	public Image m_allFreelancerImage;

	private int m_typeSpecificData;

	private void OnDisable()
	{
		m_button.spriteController.ResetMouseState();
	}

	public int GetOptionData()
	{
		return m_typeSpecificData;
	}

	public void SetOptionData(int typeSpecificData)
	{
		m_typeSpecificData = typeSpecificData;
	}

	public void AttachToDropdown(UIPlayerProgressDropdownList dropdown)
	{
		m_button.spriteController.callback = delegate
		{
			dropdown.OnSelect(m_typeSpecificData);
		};
	}

	public void Setup(string text, CharacterType charType = CharacterType.None)
	{
		for (int i = 0; i < m_texts.Length; i++)
		{
			m_texts[i].text = text;
		}
		while (true)
		{
			if (m_allFreelancerImage != null)
			{
				UIManager.SetGameObjectActive(m_allFreelancerImage, charType == CharacterType.None);
			}
			if (!(m_freelancerImage != null))
			{
				return;
			}
			while (true)
			{
				UIManager.SetGameObjectActive(m_freelancerImage, charType != CharacterType.None);
				if (charType != 0)
				{
					while (true)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
						m_freelancerImage.sprite = Resources.Load<Sprite>(characterResourceLink.m_characterSelectIconResourceString);
						return;
					}
				}
				return;
			}
		}
	}

	public void Setup(string text, CharacterRole charRole)
	{
		for (int i = 0; i < m_texts.Length; i++)
		{
			m_texts[i].text = text;
		}
		if (m_allFreelancerImage != null)
		{
			UIManager.SetGameObjectActive(m_allFreelancerImage, false);
		}
		if (m_freelancerImage != null)
		{
			UIManager.SetGameObjectActive(m_freelancerImage, charRole != CharacterRole.None);
			m_freelancerImage.sprite = CharacterResourceLink.GetCharacterRoleSprite(charRole);
		}
	}

	public void CheckDisplayState(ShouldShow shouldShowFunction)
	{
		UIManager.SetGameObjectActive(base.gameObject, shouldShowFunction(m_typeSpecificData));
	}

	public void SetSelectedIfEqual(int selectedValue)
	{
		m_button.SetSelected(m_typeSpecificData == selectedValue, false, string.Empty, string.Empty);
	}

	public bool IsOption(int typeSpecificValue)
	{
		return m_typeSpecificData == typeSpecificValue;
	}
}
