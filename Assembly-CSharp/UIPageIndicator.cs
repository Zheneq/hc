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
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageIndicatorClicked);
	}

	public void PageIndicatorClicked(BaseEventData data)
	{
		if (this.m_clickCallback != null)
		{
			this.m_clickCallback(this);
		}
	}

	public void SetClickCallback(Action<UIPageIndicator> callback)
	{
		this.m_clickCallback = callback;
	}

	public void SetSelected(bool selected)
	{
		this.m_hitbox.selectableButton.SetSelected(selected, false, string.Empty, string.Empty);
	}

	public void SetPageNumber(int pageNumber)
	{
		TextMeshProUGUI[] componentsInChildren = base.GetComponentsInChildren<TextMeshProUGUI>();
		string text = pageNumber.ToString();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetText(text);
		}
	}
}
