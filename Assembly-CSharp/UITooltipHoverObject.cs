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
			UITooltipManager.Get().ShowDisplayTooltip(this);
		}
	}
}
