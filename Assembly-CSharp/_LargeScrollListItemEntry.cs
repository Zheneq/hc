using System;
using UnityEngine;

public class _LargeScrollListItemEntry : MonoBehaviour
{
	private IUIDataEntry m_overrideDataEntryInterface;

	private bool m_isVisible;

	private void Awake()
	{
		RectTransform rectTransform = base.gameObject.transform as RectTransform;
		rectTransform.anchorMin = new Vector2(0.5f, 1f);
		rectTransform.anchorMax = new Vector2(0.5f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 1f);
		MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (monoBehaviour is IUIDataEntry)
			{
				if (this.m_overrideDataEntryInterface == null)
				{
					this.m_overrideDataEntryInterface = (monoBehaviour as IUIDataEntry);
				}
				else
				{
					Log.Warning("Scroll List Item Entry has multiple IUIDataEntry interfaces attached", new object[0]);
				}
			}
		}
	}

	public IUIDataEntry GetDataEntryInterface()
	{
		return this.m_overrideDataEntryInterface;
	}

	public void SetVisible(bool visible)
	{
		this.m_isVisible = visible;
		if (this.m_isVisible)
		{
			UIManager.SetGameObjectActive(base.gameObject, this.m_isVisible, null);
		}
	}

	private void Update()
	{
		UIManager.SetGameObjectActive(base.gameObject, this.m_isVisible, null);
	}

	public void SetParent(RectTransform rectTransform)
	{
		base.gameObject.transform.SetParent(rectTransform);
		base.gameObject.transform.localPosition = Vector3.zero;
		base.gameObject.transform.localScale = Vector3.one;
		base.gameObject.transform.localEulerAngles = Vector3.zero;
	}

	public void SetAnchoredPosition(Vector2 position)
	{
		(base.gameObject.transform as RectTransform).anchoredPosition = position;
	}

	public float GetHeight()
	{
		if (this.m_overrideDataEntryInterface != null)
		{
			return this.m_overrideDataEntryInterface.GetHeight();
		}
		return (base.gameObject.transform as RectTransform).sizeDelta.y;
	}
}
