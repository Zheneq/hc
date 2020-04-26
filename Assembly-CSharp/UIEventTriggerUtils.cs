using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class UIEventTriggerUtils
{
	public delegate void EventDelegate(BaseEventData baseEvent);

	public static void AddListener(GameObject go, EventTriggerType triggerType, EventDelegate eventDelegate)
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
		UnityAction<BaseEventData> call = eventDelegate.Invoke;
		entry.callback.AddListener(call);
		eventTrigger.triggers.Add(entry);
	}

	internal static bool HasTriggerOfType(GameObject go, EventTriggerType triggerType)
	{
		bool result = false;
		EventTrigger component = go.GetComponent<EventTrigger>();
		if (component != null)
		{
			int num = 0;
			while (true)
			{
				if (num < component.triggers.Count)
				{
					EventTrigger.Entry entry = component.triggers[num];
					if (entry.eventID == triggerType)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
	}
}
