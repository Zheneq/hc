using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPassiveButton : MonoBehaviour
{
	public Button theButton;

	private void Start()
	{
		if (this.theButton != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPassiveButton.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
		}
	}

	public void OnPointerEnter(BaseEventData data)
	{
		this.OnButtonHover(true);
	}

	public void OnPointerExit(BaseEventData data)
	{
		this.OnButtonHover(false);
	}

	private void OnButtonHover(bool hover)
	{
	}
}
