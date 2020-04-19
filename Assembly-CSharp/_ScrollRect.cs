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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ScrollRect.OnDrag(PointerEventData)).MethodHandle;
			}
			return;
		}
		base.OnDrag(eventData);
	}
}
