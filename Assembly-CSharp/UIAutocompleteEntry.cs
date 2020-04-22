using TMPro;
using UnityEngine;

public class UIAutocompleteEntry : MonoBehaviour
{
	public TextMeshProUGUI m_label;

	public _ButtonSwapSprite m_hitbox;

	private string m_textValue;

	public void Setup(string labelText, string textValue)
	{
		m_label.text = labelText;
		m_textValue = textValue;
	}

	public string GetTextValue()
	{
		return m_textValue;
	}
}
