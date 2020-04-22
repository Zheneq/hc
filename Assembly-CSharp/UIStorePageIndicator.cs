using TMPro;
using UnityEngine;

public class UIStorePageIndicator : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	public void SetSelected(bool selected)
	{
		m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}

	public void SetPageNumber(int pageNumber)
	{
		TextMeshProUGUI[] componentsInChildren = GetComponentsInChildren<TextMeshProUGUI>();
		string text = pageNumber.ToString();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetText(text);
		}
		while (true)
		{
			return;
		}
	}
}
