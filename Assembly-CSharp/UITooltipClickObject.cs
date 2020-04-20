using System;
using UnityEngine.EventSystems;

public class UITooltipClickObject : UITooltipObject
{
	public bool m_leftClickEnabled;

	public bool m_rightClickEnabled;

	public bool m_middleClickEnabled;

	private void Awake()
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnMouseClick));
	}

	private void OnMouseClick(BaseEventData data)
	{
		if (!base.IsSetup())
		{
			return;
		}
		PointerEventData pointerEventData = data as PointerEventData;
		bool flag = false;
		if (this.m_leftClickEnabled && pointerEventData.button == PointerEventData.InputButton.Left)
		{
			flag = true;
		}
		else if (this.m_rightClickEnabled && pointerEventData.button == PointerEventData.InputButton.Right)
		{
			flag = true;
		}
		else if (this.m_middleClickEnabled)
		{
			if (pointerEventData.button == PointerEventData.InputButton.Middle)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		UITooltipManager.Get().ShowMenu(this);
	}
}
