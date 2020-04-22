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
		MonoBehaviour[] components = GetComponents<MonoBehaviour>();
		MonoBehaviour[] array = components;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (!(monoBehaviour is IUIDataEntry))
			{
				continue;
			}
			if (m_overrideDataEntryInterface == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_overrideDataEntryInterface = (monoBehaviour as IUIDataEntry);
			}
			else
			{
				Log.Warning("Scroll List Item Entry has multiple IUIDataEntry interfaces attached");
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public IUIDataEntry GetDataEntryInterface()
	{
		return m_overrideDataEntryInterface;
	}

	public void SetVisible(bool visible)
	{
		m_isVisible = visible;
		if (!m_isVisible)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(base.gameObject, m_isVisible);
			return;
		}
	}

	private void Update()
	{
		UIManager.SetGameObjectActive(base.gameObject, m_isVisible);
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
		if (m_overrideDataEntryInterface != null)
		{
			return m_overrideDataEntryInterface.GetHeight();
		}
		Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
		return sizeDelta.y;
	}
}
