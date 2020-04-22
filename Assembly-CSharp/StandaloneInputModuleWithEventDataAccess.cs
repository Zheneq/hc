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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PointerEventData lastPointerEventData = GetLastPointerEventData(-1);
			if (lastPointerEventData == null)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (!(lastPointerEventData.pointerEnter != null))
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					EventTrigger component = lastPointerEventData.pointerEnter.GetComponent<EventTrigger>();
					if (!(component != null))
					{
						return;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						for (int i = 0; i < component.triggers.Count; i++)
						{
							EventTrigger.Entry entry = component.triggers[i];
							if (entry.eventID != EventTriggerType.PointerExit)
							{
								continue;
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
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
