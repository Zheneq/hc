using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class _ScrollRect : ScrollRect
{
	public bool m_ignoreOnDrag;

	public override void OnDrag(PointerEventData eventData)
	{
		if (this.m_ignoreOnDrag)
		{
			return;
		}
		base.OnDrag(eventData);
	}
}
