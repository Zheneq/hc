using TMPro;
using UnityEngine;

public class UIDropdownItem : MonoBehaviour
{
	public TextMeshProUGUI[] m_textLabels;

	public _ButtonSwapSprite m_hitbox;

	public _DropdownMenuList m_parent;

	[HideInInspector]
	public int Value;

	public void SetText(string newText)
	{
		TextMeshProUGUI[] textLabels = m_textLabels;
		foreach (TextMeshProUGUI textMeshProUGUI in textLabels)
		{
			textMeshProUGUI.text = newText;
		}
		while (true)
		{
			return;
		}
	}
}
