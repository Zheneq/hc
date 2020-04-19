using System;
using UnityEngine.EventSystems;

public class StandaloneInputModuleWithEventDataAccess : StandaloneInputModule
{
	private bool m_focusStatus = true;

	public override void Process()
	{
		if (this.m_focusStatus)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StandaloneInputModuleWithEventDataAccess.Process()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(StandaloneInputModuleWithEventDataAccess.OnApplicationFocus(bool)).MethodHandle;
			}
			PointerEventData lastPointerEventData = base.GetLastPointerEventData(-1);
			if (lastPointerEventData != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (lastPointerEventData.pointerEnter != null)
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
					EventTrigger component = lastPointerEventData.pointerEnter.GetComponent<EventTrigger>();
					if (component != null)
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
						for (int i = 0; i < component.triggers.Count; i++)
						{
							EventTrigger.Entry entry = component.triggers[i];
							if (entry.eventID == EventTriggerType.PointerExit)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								lastPointerEventData.pointerPress = lastPointerEventData.pointerEnter;
								entry.callback.Invoke(lastPointerEventData);
								return;
							}
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
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
