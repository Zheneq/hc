using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class UIEventTriggerUtils
{
	public static void AddListener(GameObject go, EventTriggerType triggerType, UIEventTriggerUtils.EventDelegate eventDelegate)
	{
		EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = go.AddComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = triggerType;
		entry.callback = new EventTrigger.TriggerEvent();
		UnityAction<BaseEventData> call = new UnityAction<BaseEventData>(eventDelegate.Invoke);
		entry.callback.AddListener(call);
		eventTrigger.triggers.Add(entry);
	}

	internal static bool HasTriggerOfType(GameObject go, EventTriggerType triggerType)
	{
		bool result = false;
		EventTrigger component = go.GetComponent<EventTrigger>();
		if (component != null)
		{
			for (int i = 0; i < component.triggers.Count; i++)
			{
				EventTrigger.Entry entry = component.triggers[i];
				if (entry.eventID == triggerType)
				{
					return true;
				}
			}
		}
		return result;
	}

	public delegate void EventDelegate(BaseEventData baseEvent);
}
