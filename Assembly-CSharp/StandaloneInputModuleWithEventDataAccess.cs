﻿using System;
using UnityEngine.EventSystems;

public class StandaloneInputModuleWithEventDataAccess : StandaloneInputModule
{
	private bool m_focusStatus = true;

	public override void Process()
	{
		if (this.m_focusStatus)
		{
			base.Process();
		}
	}

	public PointerEventData GetLastPointerEventDataPublic(int id)
	{
		if (this.m_focusStatus)
		{
			return base.GetLastPointerEventData(id);
		}
		PointerEventData lastPointerEventData = base.GetLastPointerEventData(id);
		lastPointerEventData.pointerEnter = null;
		return lastPointerEventData;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		this.m_focusStatus = focusStatus;
		if (!focusStatus)
		{
			PointerEventData lastPointerEventData = base.GetLastPointerEventData(-1);
			if (lastPointerEventData != null)
			{
				if (lastPointerEventData.pointerEnter != null)
				{
					EventTrigger component = lastPointerEventData.pointerEnter.GetComponent<EventTrigger>();
					if (component != null)
					{
						for (int i = 0; i < component.triggers.Count; i++)
						{
							EventTrigger.Entry entry = component.triggers[i];
							if (entry.eventID == EventTriggerType.PointerExit)
							{
								lastPointerEventData.pointerPress = lastPointerEventData.pointerEnter;
								entry.callback.Invoke(lastPointerEventData);
								return;
							}
						}
					}
				}
			}
		}
	}

	public bool HasFocus()
	{
		return this.m_focusStatus;
	}
}
