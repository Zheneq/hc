using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipHoverObject : UITooltipObject
{
	public Vector2[] m_anchorPoints;

	private void Awake()
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnMouseHover));
	}

	private void OnMouseHover(BaseEventData data)
	{
		if (base.IsSetup())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITooltipHoverObject.OnMouseHover(BaseEventData)).MethodHandle;
			}
			UITooltipManager.Get().ShowDisplayTooltip(this);
		}
	}
}
