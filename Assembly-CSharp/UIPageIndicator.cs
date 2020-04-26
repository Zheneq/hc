using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPageIndicator : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	private Action<UIPageIndicator> m_clickCallback;

	private void Start()
	{
		m_hitbox.callback = PageIndicatorClicked;
	}

	public void PageIndicatorClicked(BaseEventData data)
	{
		if (m_clickCallback == null)
		{
			return;
		}
		while (true)
		{
			m_clickCallback(this);
			return;
		}
	}

	public void SetClickCallback(Action<UIPageIndicator> callback)
	{
		m_clickCallback = callback;
	}

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
	}
}
