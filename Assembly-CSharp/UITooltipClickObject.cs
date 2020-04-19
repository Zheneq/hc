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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITooltipClickObject.OnMouseClick(BaseEventData)).MethodHandle;
			}
			return;
		}
		PointerEventData pointerEventData = data as PointerEventData;
		bool flag = false;
		if (this.m_leftClickEnabled && pointerEventData.button == PointerEventData.InputButton.Left)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = true;
		}
		else if (this.m_rightClickEnabled && pointerEventData.button == PointerEventData.InputButton.Right)
		{
			flag = true;
		}
		else if (this.m_middleClickEnabled)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (pointerEventData.button == PointerEventData.InputButton.Middle)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = true;
			}
		}
		if (!flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return;
		}
		UITooltipManager.Get().ShowMenu(this);
	}
}
