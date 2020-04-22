using UnityEngine.EventSystems;

public class StandaloneInputModuleWithEventDataAccess : StandaloneInputModule
{
	private bool m_focusStatus = true;

	public override void Process()
	{
		if (!m_focusStatus)
		{
			return;
		}
		while (true)
		{
			base.Process();
			return;
		}
	}

	public PointerEventData GetLastPointerEventDataPublic(int id)
	{
		if (m_focusStatus)
		{
			return GetLastPointerEventData(id);
		}
		PointerEventData lastPointerEventData = GetLastPointerEventData(id);
		lastPointerEventData.pointerEnter = null;
		return lastPointerEventData;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		m_focusStatus = focusStatus;
		if (focusStatus)
		{
			return;
		}
		while (true)
		{
			PointerEventData lastPointerEventData = GetLastPointerEventData(-1);
			if (lastPointerEventData == null)
			{
				return;
			}
			while (true)
			{
				if (!(lastPointerEventData.pointerEnter != null))
				{
					return;
				}
				while (true)
				{
					EventTrigger component = lastPointerEventData.pointerEnter.GetComponent<EventTrigger>();
					if (!(component != null))
					{
						return;
					}
					while (true)
					{
						for (int i = 0; i < component.triggers.Count; i++)
						{
							EventTrigger.Entry entry = component.triggers[i];
							if (entry.eventID != EventTriggerType.PointerExit)
							{
								continue;
							}
							while (true)
							{
								lastPointerEventData.pointerPress = lastPointerEventData.pointerEnter;
								entry.callback.Invoke(lastPointerEventData);
								return;
							}
						}
						while (true)
						{
							switch (1)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
			}
		}
	}

	public bool HasFocus()
	{
		return m_focusStatus;
	}
}
